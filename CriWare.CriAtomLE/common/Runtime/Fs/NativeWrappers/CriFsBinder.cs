/****************************************************************************
 *
 * Copyright (c) 2025 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading;
using CriWare.InteropHelpers;

namespace CriWare
{
	/// <summary>CriFsBinderハンドル </summary>
	/// <remarks>
	/// <para>
	/// 説明：
	/// バインダーとは、ファイルを効率良く扱うためのデータベースです。
	/// <list type="bullet">
	/// <item><description><see cref="CriFsBinder"/> (バインダーオブジェクト)とバインド
	///  バインダーを利用するには、バインダーオブジェクト( <see cref="CriFsBinder"/> )を作成し、 CPKファイル／ファイル／ディレクトリをバインダーに結びつけます。 このバインダーへの結び付けをバインドと呼びます。
	///  バインダーを作成すると、バインダーオブジェクト( <see cref="CriFsBinder"/> )が取得されます。
	/// </description></item>
	/// <item><description><see cref="CriFsBind"/> （バインドID）
	///  バインダーにバインドを行うと、バインドIDが作成されます。個々のバインドを識別するために使用します。
	/// </description></item>
	/// <item><description>ファイルのバインドとアンバインド
	///  バインダーには、CPKファイルやファイル、ディレクトリをどのような組み合わせででもバインドできます。
	///  バインドした項目のバインド状態を解除することをアンバインドと呼びます。
	/// </description></item>
	/// <item><description>利用できるバインド数
	///  作成できるバインダー数や同時にバインドできる最大数は、 CriFsConfig の num_binders (バインダー数)や max_binds (同時バインド可能な最大数)で指定します。
	/// </description></item>
	/// <item><description>CPKファイルのバインド
	///  CPKファイルに収納されている個々のファイル（コンテンツファイル）にアクセスするには、 CPKファイルをバインドする必要があります。
	///  CPKファイルのコンテンツファイルもバインドできます。元のCPKファイルをアンバインドした場合、 バインドされているコンテンツファイルもアンバインドされます（暗黙的アンバインド）。
	/// </description></item>
	/// <item><description>バインダーのプライオリティ
	///  バインダーは、目的のファイルがどのバインドIDにあるのかを検索します。
	///  このバインドIDの検索順は、基本的にはバインドされた順番になりますが、バインドIDのプライオリティを 操作することで、検索順を変更することができます。
	/// </description></item>
	/// <item><description>バインダーとCriFsのAPI
	///  CriFsLoader, CriFsGroupLoader, CriFsBinderには、バインダーを引数に持つAPIがあります。 その際には、 <see cref="CriFsBinder"/> と <see cref="CriFsBind"/> 、どちらを指定するのかに注意してください。 </description></item>
	/// </list>
	/// </para>
	/// </remarks>
	public partial class CriFsBinder : IDisposable
	{
		/// <summary>バインダーの生成 </summary>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// バインダーを生成し、バインダーオブジェクトを返します。
		/// </para>
		/// <para>
		/// 例：
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFsBinder_Create(CriFsBinderHn *bndrhn)"/>
		/// </remarks>

		public unsafe CriFsBinder()
		{
			IntPtr bndridPtr = default;
			var result = NativeMethods.criFsBinder_Create(&bndridPtr);
			NativeHandle = bndridPtr;
		}

		/// <summary>バインダーの破棄 </summary>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// バインダーを破棄します。
		/// </para>
		/// <para>
		/// 注意：
		/// 破棄するバインダーにバインドされているバインドIDも同時に破棄されます。
		///  本関数で破棄できるのは、<see cref="CriFsBinder.CriFsBinder"/> 関数により生成されたバインダーオブジェクトのみです。
		///  ::criFsBinder_GetHandle 関数により <see cref="CriFsBind"/> から取得されたバインダーオブジェクトは破棄できません。
		///  <see cref="CriFsBind"/> については ::criFsBinder_Unbind 関数をご使用ください。
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFsBinder_Destroy(CriFsBinderHn bndrhn)"/>
		/// </remarks>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criFsBinder_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriFsBinder() => Dispose();
#pragma warning restore 1591

		/// <summary>ファイルのバインド </summary>
		/// <param name="srcbndrhn">バインド対象のファイルを検索するためのバインダーハンドル </param>
		/// <param name="path">バインドするファイルのパス名 </param>
		/// <param name="bndrid">バインドID </param>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// ファイルをバインドし、バインドIDを返します。
		///  srcbndrhnのバインダーからpathで指定されたファイルを検索し、bndrhnにバインドします。 srcbndrhnがnullの場合、デフォルトデバイス上のファイルを検索します。
		///  ワーク領域(work)のサイズは、criFsBinder_GetWorkSizeForBindFileで取得できます。 ワーク領域は、バインドIDが破棄されるまで保持して下さい。
		///  メモリ確保／解放コールバック関数が登録されている場合、ワーク領域にnull(ワークサイズは０）を設定すると、 必要なワーク領域をメモリ確保／解放コールバック関数を使用して動的に確保します。
		///  バインドを開始できない場合、バインドIDは CRIFSBINDER_BID_NULL が返されます。 バインドIDに CRIFSBINDER_BID_NULL 以外が返された場合は内部リソースを確保していますので、 バインドの成功／失敗に関らず、不要になったバインドIDはアンバインドしてください。<br>
		///  バインドされたファイルはファイルオープン状態で保持します。 このため、内部的にCriFsLoaderを作成しています。<br>
		///  本関数は即時復帰関数です。本関数から復帰した直後は、ファイルのバインドはまだ完了しておらず、 バインドIDを利用したファイルへのアクセスは行えません。
		///  バインドIDのバインド状態が完了（ <see cref="CriFsBinder.Status.Complete"/> ）となった後に、 ファイルは利用可能となります。
		///  バインド状態は <see cref="CriFsBinder.GetStatus"/> 関数で取得します。
		/// </para>
		/// <para>
		/// 例：
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFsBinder_BindFile(CriFsBinderHn bndrhn, CriFsBinderHn srcbndrhn, const CriChar8 *path, void *work, CriSint32 worksize, CriFsBindId *bndrid)"/>
		/// </remarks>
		public unsafe CriErr.Error BindFile(CriFsBinder srcbndrhn, ArgString path, out CriFsBind bndrid)
		{
			fixed (CriFsBind* bndridPtr = &bndrid)
				return (CriErr.Error)NativeMethods.criFsBinder_BindFile(NativeHandle, srcbndrhn?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]), default, default, (UInt32*)bndridPtr);
		}

		/// <summary>ファイルセクションのバインド </summary>
		/// <param name="srcbndrhn">バインド対象のファイルを検索するためのバインダーハンドル </param>
		/// <param name="path">バインドするファイルのパス名 </param>
		/// <param name="offset">データの開始位置（バイト） </param>
		/// <param name="size">データサイズ（バイト） </param>
		/// <param name="sectionName">セクション名 </param>
		/// <param name="bndrid">バインドID </param>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// ファイルの一部分をバインドし、その箇所を仮想的なファイルとして扱えるよう設定します。
		///  srcbndrhnのバインダーからpathで指定されたファイルを検索してバインドします。 srcbndrhnがnullの場合、デフォルトデバイスを使用します。
		///  ワーク領域(work)のサイズは、criFsBinder_GetWorkSizeForBindFileSectionで取得できます。 ワーク領域は、バインドIDが破棄されるまで保持して下さい。
		///  メモリ確保／解放コールバック関数が登録されている場合、ワーク領域にnull(ワークサイズは０）を設定すると、 必要なワーク領域をメモリ確保／解放コールバック関数を使用して動的に確保します。
		///  バインドを開始できない場合、バインドIDは CRIFSBINDER_BID_NULL が返されます。 バインドIDに CRIFSBINDER_BID_NULL 以外が返された場合は内部リソースを確保していますので、 バインドの成功／失敗に関らず、不要になったバインドIDはアンバインドしてください。<br>
		///  バインドされたファイルはファイルオープン状態で保持します。 このため、内部的にCriFsLoaderを作成しています。<br>
		///  本関数は即時復帰関数です。本関数から復帰した直後は、ファイルのバインドはまだ完了しておらず、 バインドIDを利用したファイルへのアクセスは行えません。
		///  バインドIDのバインド状態が完了（ <see cref="CriFsBinder.Status.Complete"/> ）となった後に、 ファイルは利用可能となります。
		///  バインド状態は <see cref="CriFsBinder.GetStatus"/> 関数で取得します。
		/// </para>
		/// <para>
		/// 例：
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFsBinder_BindFileSection(CriFsBinderHn bndrhn, CriFsBinderHn srcbndrhn, const CriChar8 *path, CriUint64 offset, CriSint32 size, const CriChar8 *section_name, void *work, CriSint32 worksize, CriFsBindId *bndrid)"/>
		/// </remarks>
		/// <seealso cref="CriFsBinder.GetStatus"/>
		public unsafe CriErr.Error BindFileSection(CriFsBinder srcbndrhn, ArgString path, UInt64 offset, Int32 size, ArgString sectionName, out CriFsBind bndrid)
		{
			fixed (CriFsBind* bndridPtr = &bndrid)
				return (CriErr.Error)NativeMethods.criFsBinder_BindFileSection(NativeHandle, srcbndrhn?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]), offset, size, sectionName.GetPointer(stackalloc byte[sectionName.BufferSize]), default, default, (UInt32*)bndridPtr);
		}

		/// <summary>バインド状態の取得 </summary>
		/// <param name="bndrid">バインドID </param>
		/// <param name="status"><see cref="CriFsBinder.Status"/>バインダーステータス </param>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// 指定されたバインドIDのバインド状態を取得します。 
		///  バインド状態が <see cref="CriFsBinder.Status.Complete"/> になるまでは、 そのバインドIDによるファイルアクセスを行えません。
		/// </para>
		/// <para>
		/// 例：
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFsBinder_GetStatus(CriFsBindId bndrid, CriFsBinderStatus *status)"/>
		/// </remarks>
		public static unsafe CriErr.Error GetStatus(CriFsBind bndrid, out CriFsBinder.Status status)
		{
			fixed (CriFsBinder.Status* statusPtr = &status)
				return (CriErr.Error)NativeMethods.criFsBinder_GetStatus(bndrid.NativeHandle, statusPtr);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriFsBinder(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriFsBinder other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriFsBinder a, CriFsBinder b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriFsBinder a, CriFsBinder b) =>
			!(a == b);

		/// <summary>バインダーステータス </summary>
		/// <remarks>
		/// <para>
		/// 説明：
		/// <see cref="CriFsBinder.GetStatus"/> 関数で取得される、バインドIDの状態です。
		///  バインドが完了するまで、バインドした項目にアクセスすることはできません。
		///  バインド対象が存在しなかったり、バインドに必要なリソースが不足する場合は、 バインド失敗となります。
		///  バインド失敗時の詳しい情報はエラーコールバック関数で取得してください。 
		/// </para>
		/// </remarks>
		public enum Status
		{
			None = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>バインド処理中 </para>
			/// </remarks>
			Analyze = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>バインド完了 </para>
			/// </remarks>
			Complete = 2,
			/// <summary></summary>
			/// <remarks>
			/// <para>アンバインド処理中 </para>
			/// </remarks>
			Unbind = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>アンバインド完了 </para>
			/// </remarks>
			Removed = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>バインド無効 </para>
			/// </remarks>
			Invalid = 5,
			/// <summary></summary>
			/// <remarks>
			/// <para>バインド失敗 </para>
			/// </remarks>
			Error = 6,
			EnumBeSint32 = 2147483647,
		}
	}
}