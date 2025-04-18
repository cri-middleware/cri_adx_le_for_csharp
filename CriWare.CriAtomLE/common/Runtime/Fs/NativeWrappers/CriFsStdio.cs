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

	public partial class CriFsStdio : IDisposable
	{
		/// <summary>ANSI C に準じたファイルオープン </summary>
		/// <param name="bndr">オープンしたいファイルがバインドされているCriFsBinderのハンドル </param>
		/// <param name="fname">オープンしたいファイルパス </param>
		/// <param name="mode">オープンモード ("r":読み込み専用モード,"w":書き込み専用モード) </param>
		/// <returns>
		/// <see cref="CriFsStdio"/> 成功した場合、有効なCriFsStdioオブジェクトを返します。
		///  失敗した場合はnullを返します。 
		/// </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// 指定されたファイルをオープンします。
		///  第一引数には、オープンしたいファイルがバインドされているバインダーを指定します。
		///  プラットフォーム標準のファイルパスからファイルをオープンしたい場合、第一引数にはnullを指定します。
		///  第二引数には、オープンしたいファイルパスを文字列で指定します。
		///  第三引数は、オープンのモードです。"r"を指定すると読み込み専用モード、
		///  "w"を指定すると書き込み専用モードでファイルをオープンします。
		///  書き込み専用モードは、ファイル書き込みをサポートしているプラットフォームでのみ正常に動作し、 未サポートのプラットフォームではエラーコールバックが発生し、オープンは失敗します。
		/// </para>
		/// <para>
		/// 備考：
		/// ファイルの書き込みは以下のルールで行われます。
		/// <list type="bullet">
		/// <item><description>指定したファイルが存在しない場合、新規にファイルを作成。</description></item>
		/// <item><description>指定したファイルが既に存在する場合、既存ファイルを編集します。
		///  （既存ファイルが削除されることはありません。） 
		///  既存ファイルを削除して新規にファイルの書き込みを行いたい場合には、 本関数を実行する前に ::criFsStdio_RemoveFile 関数でファイルの削除を行ってください。 </description></item>
		/// </list>
		/// </para>
		/// <nativeinfo declaration="CriFsStdioHn CRIAPI criFsStdio_OpenFile(CriFsBinderHn bndr, const char *fname, const char *mode)"/>
		/// </remarks>
		/// <seealso cref="CriFsStdio.Dispose"/>
		public static CriFsStdio OpenFile(CriFsBinder bndr, IntPtr fname, IntPtr mode)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criFsStdio_OpenFile(bndr?.NativeHandle ?? default, fname, mode)) == IntPtr.Zero) ? null : new CriFsStdio(handle);
		}

		/// <summary>ANSI C に準じたファイルクローズ </summary>
		/// <returns><see cref="CriErr.Error"/> エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// 指定したファイルをクローズします。
		///  第一引数には、クローズしたいファイルのCriFsStdioオブジェクトを指定します。
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFsStdio_CloseFile(CriFsStdioHn stdhn)"/>
		/// </remarks>
		/// <seealso cref="CriFsStdio.OpenFile"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criFsStdio_CloseFile(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriFsStdio() => Dispose();
#pragma warning restore 1591

		/// <summary>ANSI C に準じたAPIに基づくファイルサイズ取得 </summary>
		/// <returns>CriSint64 指定したオブジェクトが有効であれば、ファイルサイズを返します。 </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// 指定したファイルのサイズを取得します。
		///  第一引数には、サイズを取得したいファイルのCriFsStdioオブジェクトを指定します。
		/// </para>
		/// <nativeinfo declaration="CriSint64 CRIAPI criFsStdio_GetFileSize(CriFsStdioHn stdhn)"/>
		/// </remarks>
		public Int64 GetFileSize()
		{
			return NativeMethods.criFsStdio_GetFileSize(NativeHandle);
		}

		/// <summary>ANSI C に準じた ファイルリードオフセットのシーク </summary>
		/// <param name="offset">シークのオフセット（byte） </param>
		/// <param name="seekType">シーク開始位置の指定 </param>
		/// <returns>
		/// CriSint64 成功 0
		///  失敗 -1
		/// </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// 指定したファイルのリードオフセットをシークします。
		///  第一引数には、リードオフセットをシークしたいファイルの、CriFsStdioオブジェクトを指定します。
		///  第二引数には、シークのオフセットを指定します。単位はbyteです。
		/// </para>
		/// <para>
		/// 注意:
		/// ファイル先頭より手前にシークすることは出来ません。ファイルリードオフセットがファイル先頭より 手前になるようシークオフセットを指定した場合、シーク結果のファイルリードオフセットはファイル先頭になります。
		///  一方、ファイル終端を超えたシークは可能です。
		///  また、指定のCriFsStdioオブジェクトが中間バッファーを持つ場合、 本関数で中間バッファーの有効範囲外にシークすると、中間バッファーの内容が破棄されます。
		/// </para>
		/// <nativeinfo declaration="CriSint64 CRIAPI criFsStdio_SeekFile(CriFsStdioHn rdr, CriSint64 offset, CRIFSSTDIO_SEEK_TYPE seek_type)"/>
		/// </remarks>
		public Int64 SeekFile(Int64 offset, CriFsStdio.SEEKTYPE seekType)
		{
			return NativeMethods.criFsStdio_SeekFile(NativeHandle, offset, seekType);
		}

		/// <summary>ANSI C に準じたAPIに基づくファイルからのデータ読込 </summary>
		/// <param name="rsize">読込要求サイズ（byte） </param>
		/// <param name="buf">読込先バッファー </param>
		/// <param name="bsize">読込先バッファーのサイズ（byte） </param>
		/// <returns>
		/// CriSint64 読込成功 読み込めたサイズ（byte）
		///  読込失敗 -1 
		/// </returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// ファイルからデータを指定サイズ（byte）分読み込みます。
		///  第一引数には、データの読込元であるファイルのCriFsStdioオブジェクトを指定します。
		///  第二引数には、読み込むサイズを指定します。
		///  第三引数には、読み込んだデータの書き込み先バッファーを指定します。
		///  第四引数には、読み込んだデータの書き込み先バッファーサイズを指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 戻り値は、常に<b>読込要求サイズ以下になる</b>ことに注意してください。
		///  例えばファイル終端では、戻り値が読込要求サイズより小さくなることがありますが、 読込に失敗しているわけではありません。読込に失敗した場合、-1を返します。 
		/// </para>
		/// <nativeinfo declaration="CriSint64 CRIAPI criFsStdio_ReadFile(CriFsStdioHn stdhn, CriSint64 rsize, void *buf, CriSint64 bsize)"/>
		/// </remarks>
		public Int64 ReadFile(Int64 rsize, IntPtr buf, Int64 bsize)
		{
			return NativeMethods.criFsStdio_ReadFile(NativeHandle, rsize, buf, bsize);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriFsStdio(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriFsStdio other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriFsStdio a, CriFsStdio b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriFsStdio a, CriFsStdio b) =>
			!(a == b);

		/// <summary>ファイル上のシーク開始位置 </summary>
		public enum SEEKTYPE
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>ファイルの先頭 </para>
			/// </remarks>
			Set = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>現在の読込位置 </para>
			/// </remarks>
			Cur = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>ファイルの終端 </para>
			/// </remarks>
			End = 2,
			EnumBeSint32 = 2147483647,
		}
	}
}