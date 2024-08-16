/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#if NET5_0_OR_GREATER
using System.Text.Unicode;
#endif

namespace CriWare.InteropHelpers {
	/// <summary>
	/// Blittable論理型定義
	/// </summary>
	/// <remarks>
	/// CRIWAREネイティブと同じbit幅を持つbool値です。
	/// <see cref="System.Boolean"/>に対して双方向のキャストが可能です。
	/// </remarks>
	[System.Serializable]
	public struct NativeBool {
		/// <summary>
		/// Int32表現での値
		/// </summary>
		/// <remarks>
		/// CRIWAREが取り扱う真偽値はメモリ上ではInt32として表現されます。
		/// 通常、この値には0または1が格納されています。
		/// </remarks>
		public Int32 value;

		/// <summary>
		/// <see cref="System.Boolean"/>へのキャスト
		/// </summary>
		public static implicit operator bool(NativeBool native) => native.value != 0;
		/// <summary>
		/// <see cref="System.Boolean"/>からのキャスト
		/// </summary>
		public static implicit operator NativeBool(bool val) => new NativeBool(){value = val?1:0};
	}

	/// <summary>
	/// Blittable文字列型
	/// </summary>
	/// <remarks>
	/// ポインタによる文字列を表現する値です。
	/// <see cref="System.String"/>へのキャストが可能です。
	/// </remarks>
	public struct NativeString {
		static Dictionary<IntPtr, string> stringsCache = new Dictionary<nint, string>();

#pragma warning disable 0649
		IntPtr pointer;
#pragma warning restore 0649

		/// <summary>
		/// マネージド文字列への変換(キャッシュあり)
		/// </summary>
		/// <returns>変換後の文字列</returns>
		/// <remarks>
		/// <see cref="ToString"/>による変換では、呼び出しのたびに<see cref="string"/>を新たにヒープ上に確保します。
		/// 本APIでは指し示すネイティブ文字列ポインタが同一の場合はキャッシュされた文字列を返すため、ヒープへの文字列確保は初回のみとなります。
		/// ただし、ネイティブ文字列領域が動的に書き換わっている場合は現在の状態を取得できないため、<see cref="ToString"/>による変換をおすすめします。
		/// </remarks>
		public string ToStringCached(){
			if (!stringsCache.ContainsKey(pointer))
				stringsCache.Add(pointer, ToString());
			return stringsCache[pointer];
		}

		/// <inheritdoc/>
		public override string ToString() => Marshal.PtrToStringUTF8(pointer);

		/// <summary>
		/// <see cref="System.String"/>へのキャスト
		/// </summary>
		public static implicit operator string(NativeString native) => native.ToString();

		/// <summary>
		/// 文字列ポインタの取得
		/// </summary>
		/// <returns>ネイティブ文字列へのポインタ</returns>
		public IntPtr GetUnsafeStringPointer() => pointer;
	}

	/// <summary>
	/// 文字列引数構造体
	/// </summary>
	/// <remarks>
	/// CRIWAREのAPIが要求する文字列引数に利用する型です。
	/// 各種文字列/バイト列表現からのキャストが可能です。
	/// <see cref="System.String"/>を渡した場合のみ、UTF8への変換が行われます。
	/// </remarks>
	public unsafe struct ArgString {
		byte* pointer;
		string utf16str;

		/// <summary>
		/// <see cref="System.String"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(string str){
			return new ArgString(){utf16str = str};
		}
		/// <summary>
		/// <see cref="Span{T}"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(Span<byte> str){
			fixed(byte* ptr = str)
				return new ArgString(){pointer = ptr};
		}
		/// <summary>
		/// <see cref="ReadOnlySpan{T}"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(ReadOnlySpan<byte> str){
			fixed(byte* ptr = str)
				return new ArgString(){pointer = ptr};
		}
		/// <summary>
		/// <see cref="IntPtr"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(IntPtr ptr){
			return new ArgString(){pointer = (byte*)ptr};
		}
		/// <summary>
		/// <see cref="NativeString"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(NativeString arg){
			return new ArgString(){pointer = (byte*)arg.GetUnsafeStringPointer()};
		}
		
		/// <summary>
		/// 文字コード変換時に必要になるバッファサイズ
		/// </summary>
		public int BufferSize => utf16str?.Length * 3 ?? 0;
		/// <summary>
		/// 文字列ポインタの取得
		/// </summary>
		/// <param name="buffer">変換時に利用するバッファ領域</param>
		/// <returns>文字列ポインタ</returns>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public nint GetPointer(Span<byte> buffer){
			if(utf16str != null){
#if NET5_0_OR_GREATER
				Utf8.FromUtf16(utf16str, buffer, out _, out _);
#else
				fixed (byte* ptr = buffer)
				fixed (char* strPtr = utf16str)
					System.Text.Encoding.UTF8.GetBytes(strPtr, utf16str.Length, ptr, utf16str.Length * 3);
#endif
				fixed(byte* ptr = buffer)
					pointer = ptr;
			}
			return (nint)pointer;
		}
	}

	/// <summary>
	/// ネイティブ参照
	/// </summary>
	/// <typeparam name="T">参照先の型</typeparam> 
	public unsafe struct NativeReference<T> where T : unmanaged {
#pragma warning disable 0649
		T* pointer;
#pragma warning restore 0649

		/// <summary>
		/// 参照先の値の取得
		/// </summary>
		/// <returns>参照先の値</returns>
		/// <exception cref="NullReferenceException"/>
		public T GetValue(){
			if(pointer == null) throw new NullReferenceException();
			return *pointer;
		}
		/// <summary>
		/// 参照の取得
		/// </summary>
		/// <returns>値への参照</returns>
		/// <exception cref="NullReferenceException"/>
		public ref T GetRefValue(){
			if(pointer == null) throw new NullReferenceException();
			return ref *pointer;
		}
		/// <summary>
		/// 参照先の書き換え
		/// </summary>
		/// <param name="value">設定する値</param>
		/// <exception cref="NullReferenceException"></exception> 
		public void SetValue(T value){
			if(pointer == null) throw new NullReferenceException();
			*pointer = value;
		}

		/// <exclude/>
		public static implicit operator NativeReference<T>(T* pt) =>
			new NativeReference<T>(){pointer = pt};
		
		/// <exclude/>
		public static explicit operator T*(NativeReference<T> nr) =>
			nr.pointer;
	}

#pragma warning disable 0649
#pragma warning disable 0169
	// dotnet8のときはInlineArrayAttributeにまかせる対応入れてもいいかも
	// パディングが怖いので16までは素直に並べる

	/// <summary>
	/// Blittable配列型
	/// </summary>
	/// <typeparam name="T">配列の要素の型</typeparam>
	/// <remarks>
	/// ネイティブ層と同じメモリレイアウトの配列型です。
	/// CRIWAREのAPIに受け渡す構造体のフィールドとして利用されます。
	/// </remarks>
	public struct InlineArray1<T> where T : unmanaged {
		T element0;
		/// <summary>
		/// <see cref="Span{T}"/>へのキャスト
		/// </summary>
		public static implicit operator Span<T>(InlineArray1<T> array)
			=> MemoryMarshal.CreateSpan(ref array.element0, 1);
		/// <summary>
		/// 配列内の要素の取得
		/// </summary>
		/// <param name="index">要素インデックス</param>
		/// <returns>インデックスに対応する配列内の要素</returns> 
		public T this[int index] => ((Span<T>)this)[index];
	}

	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray2<T> where T : unmanaged {
		T element0, element1;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(InlineArray1{T})"/>
		public static implicit operator Span<T>(InlineArray2<T> array)
			=> MemoryMarshal.CreateSpan(ref array.element0, 2);
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray3<T> where T : unmanaged {
		T element0, element1, element2;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(InlineArray1{T})"/>
		public static implicit operator Span<T>(InlineArray3<T> array)
			=> MemoryMarshal.CreateSpan(ref array.element0, 3);
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray4<T> where T : unmanaged {
		internal T element0, element1, element2, element3;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(InlineArray1{T})"/>
		public static implicit operator Span<T>(InlineArray4<T> array)
			=> MemoryMarshal.CreateSpan(ref array.element0, 4);
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray8<T> where T : unmanaged {
		T element0, element1, element2, element3;
		T element4, element5, element6, element7;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(InlineArray1{T})"/>
		public static implicit operator Span<T>(InlineArray8<T> array)
			=> MemoryMarshal.CreateSpan(ref array.element0, 8);
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray16<T> where T : unmanaged {
		internal T element0, element1, element2, element3;
		T element4, element5, element6, element7;
		T element8, element9, element10, element11;
		T element12, element13, element14, element15;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(InlineArray1{T})"/>
		public static implicit operator Span<T>(InlineArray16<T> array)
			=> MemoryMarshal.CreateSpan(ref array.element0, 16);
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray64<T> where T : unmanaged {
		// 16以上ならパディング単位に揃ってるはずなので入れ子で並べる
		InlineArray4<InlineArray16<T>> elements;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(InlineArray1{T})"/>
		public static implicit operator Span<T>(InlineArray64<T> array)
			=> MemoryMarshal.CreateSpan(ref array.elements.element0.element0, 64);
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
#pragma warning restore 0649
#pragma warning restore 0169
}