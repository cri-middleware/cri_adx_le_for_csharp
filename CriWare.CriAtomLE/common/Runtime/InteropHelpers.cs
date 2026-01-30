/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if NET5_0_OR_GREATER
using System.Text.Unicode;
#endif

namespace CriWare.InteropHelpers
{
	/// <summary>
	/// Blittable論理型定義
	/// </summary>
	/// <remarks>
	/// CRIWAREネイティブと同じbit幅を持つbool値です。
	/// <see cref="System.Boolean"/>に対して双方向のキャストが可能です。
	/// </remarks>
	[System.Serializable]
	public struct NativeBool : IXmlSerializable
	{
		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public Int32 value;

		public XmlSchema GetSchema() => null;
		public void ReadXml(XmlReader reader) =>
			value = reader.ReadInnerXml() == "false" ? 0 : 1;
		public void WriteXml(XmlWriter writer) =>
			writer.WriteString(value == 0 ? "false" : "true");

		/// <summary>
		/// <see cref="System.Boolean"/>へのキャスト
		/// </summary>
		public static implicit operator bool(NativeBool native) => native.value != 0;
		/// <summary>
		/// <see cref="System.Boolean"/>からのキャスト
		/// </summary>
		public static implicit operator NativeBool(bool val) => new NativeBool() { value = val ? 1 : 0 };
	}

	static class NativeStringCache
	{
		public static Dictionary<IntPtr, string> stringsCache = new Dictionary<nint, string>();
	}

	/// <summary>
	/// Blittable文字列型
	/// </summary>
	/// <remarks>
	/// ポインタによる文字列を表現する値です。
	/// <see cref="System.String"/>へのキャストが可能です。
	/// </remarks>
	public struct NativeString
	{

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
		public string ToStringCached()
		{
			if (!NativeStringCache.stringsCache.ContainsKey(pointer))
				NativeStringCache.stringsCache.Add(pointer, ToString());
			return NativeStringCache.stringsCache[pointer];
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			MemoryUtils.EnsureAccessAllowed(pointer);
			return Marshal.PtrToStringUTF8(pointer);
		}

		/// <summary>
		/// <see cref="System.String"/>へのキャスト
		/// </summary>
		public static implicit operator string(NativeString native) => native.ToString();

		/// <summary>
		/// 文字列ポインタの取得
		/// </summary>
		/// <returns>ネイティブ文字列へのポインタ</returns>
		public IntPtr GetUnsafeStringPointer() => pointer;

		/// <exclude />
		public static implicit operator NativeString(IntPtr ptr) => new NativeString() { pointer = ptr };
	}

	/// <summary>
	/// 文字列引数構造体
	/// </summary>
	/// <remarks>
	/// CRIWAREのAPIが要求する文字列引数に利用する型です。
	/// 各種文字列/バイト列表現からのキャストが可能です。
	/// <see cref="System.String"/>を渡した場合のみ、UTF8への変換が行われます。
	/// </remarks>
	public unsafe struct ArgString
	{
		byte* pointer;
		string utf16str;

		/// <summary>
		/// <see cref="System.String"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(string str)
		{
			return new ArgString() { utf16str = str };
		}
		/// <summary>
		/// <see cref="Span{T}"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(Span<byte> str)
		{
			fixed (byte* ptr = str)
				return new ArgString() { pointer = ptr };
		}
		/// <summary>
		/// <see cref="ReadOnlySpan{T}"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(ReadOnlySpan<byte> str)
		{
			fixed (byte* ptr = str)
				return new ArgString() { pointer = ptr };
		}
		/// <summary>
		/// <see cref="IntPtr"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(IntPtr ptr)
		{
			return new ArgString() { pointer = (byte*)ptr };
		}
		/// <summary>
		/// <see cref="NativeString"/>からのキャスト
		/// </summary>
		public static implicit operator ArgString(NativeString arg)
		{
			return new ArgString() { pointer = (byte*)arg.GetUnsafeStringPointer() };
		}

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public int BufferSize => utf16str?.Length * 3 ?? 0;
		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public nint GetPointer(Span<byte> buffer)
		{
			if (utf16str != null)
			{
#if NET5_0_OR_GREATER
				Utf8.FromUtf16(utf16str, buffer, out _, out _);
#else
				fixed (byte* ptr = buffer)
				fixed (char* strPtr = utf16str)
					System.Text.Encoding.UTF8.GetBytes(strPtr, utf16str.Length, ptr, utf16str.Length * 3);
#endif
				fixed (byte* ptr = buffer)
					pointer = ptr;
			}
			return (nint)pointer;
		}
	}

	/// <summary>
	/// ネイティブ参照
	/// </summary>
	/// <typeparam name="T">参照先の型</typeparam> 
	public unsafe struct NativeReference<T> where T : unmanaged
	{
#pragma warning disable 0649
		T* pointer;
#pragma warning restore 0649

		/// <summary>
		/// 参照先の値の取得
		/// </summary>
		/// <returns>参照先の値</returns>
		/// <exception cref="NullReferenceException"/>
		/// <exception cref="InvalidOperationException">参照先が現在のコンテキストで利用できない場合</exception>
		public T GetValue()
		{
			MemoryUtils.EnsureAccessAllowed((nint)pointer);
			return *pointer;
		}
		/// <summary>
		/// 参照の取得
		/// </summary>
		/// <returns>値への参照</returns>
		/// <exception cref="NullReferenceException"/>
		[Obsolete]
		public ref T GetRefValue()
		{
			if (pointer == null) throw new NullReferenceException();
			return ref *pointer;
		}
		/// <summary>
		/// 参照先の書き換え
		/// </summary>
		/// <param name="value">設定する値</param>
		/// <exception cref="NullReferenceException"></exception> 
		/// <exception cref="InvalidOperationException">参照先が現在のコンテキストで利用できない場合</exception>
		public void SetValue(T value)
		{
			MemoryUtils.EnsureAccessAllowed((nint)pointer);
			*pointer = value;
		}

		/// <summary>Spanへの変換</summary>
		/// <param name="length">Spanの長さ</param>
		/// <returns>参照先を表すSpan</returns>
		/// <exception cref="NullReferenceException"></exception> 
		public Span<T> AsSpan(int length)
		{
			if (pointer == null) throw new NullReferenceException();
			return new Span<T>(pointer, length);
		}

		/// <exclude/>
		public static implicit operator NativeReference<T>(T* pt) =>
			new NativeReference<T>() { pointer = pt };

		/// <exclude/>
		public static explicit operator T*(NativeReference<T> nr) =>
			nr.pointer;
	}

	static unsafe class MemoryUtils
	{
		internal static void EnsureAccessAllowed(nint memory)
		{
			if (memory == 0) throw new NullReferenceException();
			if (NativeAllocator.GetContainingMemory(memory) != 0) return;
			if (IsOnStack(memory)) return;
			if (IsInImage(memory)) return;
			throw new InvalidOperationException("Referenced object is not available in this context.");
		}

		internal static bool IsOnStack(nint memory)
		{
#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win
			nint low, high;
			GetCurrentThreadStackLimits(&low, &high);
			return low <= memory && memory < high;
			[DllImport("kernel32.dll")]
			static extern void GetCurrentThreadStackLimits(nint* lowLimit, nint* highLimit);
#elif (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX || osx
			var thread = pthread_self();
			var high = pthread_get_stackaddr_np(thread);
			return ((ulong)high - (ulong)pthread_get_stacksize_np(thread)) <= (ulong)memory && (ulong)memory < (ulong)high;
			[DllImport("libSystem.B.dylib")]
			static extern UIntPtr pthread_self();
			[DllImport("libSystem.B.dylib")]
			static extern IntPtr pthread_get_stackaddr_np(UIntPtr thread);
			[DllImport("libSystem.B.dylib")]
			static extern UIntPtr pthread_get_stacksize_np(UIntPtr thread);
#else
			return true;
#endif
		}

		internal static bool IsInImage(nint memory)
		{
#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win
			return cri_memory_utils_is_in_image(memory) != 0;
			[DllImport(CriBaseCSharp.LibraryName)]
			static extern int cri_memory_utils_is_in_image(IntPtr memory);
#elif (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX || osx
			var info = stackalloc nint[8];
			return dladdr(memory, info) != 0;
			[DllImport("libSystem.B.dylib")]
			static extern int dladdr(IntPtr addr, nint* info);
#else
			return true;
#endif
		}
	}

#pragma warning disable 0649
#pragma warning disable 0169
	// dotnet8のときはInlineArrayAttributeにまかせる対応も可能
	// パディングが怖いので16までは素直に並べる

	/// <summary>
	/// Blittable配列型
	/// </summary>
	/// <typeparam name="T">配列の要素の型</typeparam>
	/// <remarks>
	/// ネイティブ層と同じメモリレイアウトの配列型です。
	/// CRIWAREのAPIに受け渡す構造体のフィールドとして利用されます。
	/// </remarks>
	public struct InlineArray1<T> where T : unmanaged
	{
		T element0;
		/// <summary>
		/// <see cref="Span{T}"/>へのキャスト
		/// </summary>
		public static unsafe implicit operator Span<T>(in InlineArray1<T> array){
			fixed (T* ptr = &array.element0)
				return new Span<T>(ptr, 1);
		}
		/// <summary>
		/// 配列内の要素の取得
		/// </summary>
		/// <param name="index">要素インデックス</param>
		/// <returns>インデックスに対応する配列内の要素</returns> 
		public T this[int index] => ((Span<T>)this)[index];
	}

	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray2<T> where T : unmanaged
	{
		internal T element0, element1;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray2<T> array){
			fixed (T* ptr = &array.element0)
				return new Span<T>(ptr, 2);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray3<T> where T : unmanaged
	{
		T element0, element1, element2;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray3<T> array){
			fixed (T* ptr = &array.element0)
				return new Span<T>(ptr, 3);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray4<T> where T : unmanaged
	{
		internal T element0, element1, element2, element3;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray4<T> array) {
			fixed (T* ptr = &array.element0)
				return new Span<T>(ptr, 4);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray8<T> where T : unmanaged
	{
		T element0, element1, element2, element3;
		T element4, element5, element6, element7;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray8<T> array){
			fixed (T* ptr = &array.element0)
				return new Span<T>(ptr, 8);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray16<T> where T : unmanaged
	{
		internal T element0, element1, element2, element3;
		T element4, element5, element6, element7;
		T element8, element9, element10, element11;
		T element12, element13, element14, element15;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray16<T> array){
			fixed (T* ptr = &array.element0)
				return new Span<T>(ptr, 16);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray32<T> where T : unmanaged
	{
		// 16以上ならパディング単位に揃ってるはずなので入れ子で並べる
		internal InlineArray2<InlineArray16<T>> elements;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray32<T> array){
			fixed (T* ptr = &array.elements.element0.element0)
				return new Span<T>(ptr, 32);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray64<T> where T : unmanaged
	{
		// 16以上ならパディング単位に揃ってるはずなので入れ子で並べる
		InlineArray4<InlineArray16<T>> elements;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray64<T> array){
			fixed (T* ptr = &array.elements.element0.element0)
				return new Span<T>(ptr, 64);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
	/// <inheritdoc cref="InlineArray1{T}"/>
	public struct InlineArray1024<T> where T : unmanaged {
		// 16以上ならパディング単位に揃ってるはずなので入れ子で並べる
		InlineArray32<InlineArray32<T>> elements;
		/// <inheritdoc cref="InlineArray1{T}.op_Implicit(in InlineArray1{T})"/>
		public static unsafe implicit operator Span<T>(in InlineArray1024<T> array){
			fixed (T* ptr = &array.elements.elements.element0.element0.elements.element0.element0)
				return new Span<T>(ptr, 1024);
		}
		/// <inheritdoc cref="InlineArray1{T}.this[int]"/>
		public T this[int index] => ((Span<T>)this)[index];
	}
#pragma warning restore 0649
#pragma warning restore 0169
}