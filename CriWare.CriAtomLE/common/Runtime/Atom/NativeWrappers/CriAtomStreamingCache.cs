/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
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
	/// <summary>ストリーミングキャッシュID</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomStreamingCache"/> は、ストリーミングキャッシュ管理用IDです。
	/// <see cref="CriAtomStreamingCache.CriAtomStreamingCache"/> 関数でストリーミングキャッシュを作成すると取得できます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
	public partial class CriAtomStreamingCache : IDisposable
	{
		/// <summary><see cref="CriAtomStreamingCache.Config"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">ストリーミングキャッシュ作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomStreamingCache.CalculateWorkSize"/> 関数、
		/// <see cref="CriAtomStreamingCache.CriAtomStreamingCache"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomStreamingCache.Config"/> ）に対し、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CalculateWorkSize"/>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		public static unsafe void SetDefaultConfig(out CriAtomStreamingCache.Config pConfig)
		{
			fixed (CriAtomStreamingCache.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomStreamingCache_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>ストリーミングキャッシュ作成に必要なワークサイズの計算</summary>
		/// <param name="config">
		/// ストリーミングキャッシュ作成用構造体
		/// return		CriSint32	ストリーミングキャッシュ作成に必要なワークサイズ
		/// </param>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ストリーミングキャッシュ作成に必要なワークサイズを計算します。
		/// configで与えられるパラメーターに依存し、必要なワークサイズは増加します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomStreamingCache.Config config)
		{
			fixed (CriAtomStreamingCache.Config* configPtr = &config)
				return NativeMethods.criAtomStreamingCache_CalculateWorkSize(configPtr);
		}

		/// <summary>ストリーミングキャッシュ作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーにストリーミングキャッシュを作成する際に、
		/// キャッシュ可能なファイルサイズ等を登録するための構造体です。
		/// <see cref="CriAtomStreamingCache.CalculateWorkSize"/> 関数、
		/// <see cref="CriAtomStreamingCache.CriAtomStreamingCache"/> 関数の引数に指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CalculateWorkSize"/>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		public unsafe partial struct Config
		{
			/// <summary>キャッシュするファイルの最大パス長</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ストリーミングキャッシュでキャッシュするファイルの最大パス長を指定します。
			/// </para>
			/// </remarks>
			public Int32 maxPath;

			/// <summary>キャッシュ可能なファイルの最大数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ストリーミングキャッシュでキャッシュするファイル数を指定します。
			/// </para>
			/// </remarks>
			public Int32 maxFiles;

			/// <summary>キャッシュ可能なファイルサイズ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ストリーミングキャッシュでキャッシュ可能なファイルサイズを指定します。
			/// このパラメーターで指定した以上のファイルをキャッシュすることはできません。
			/// また、ストリーミングキャッシュの必要ワークサイズは、
			/// このパラメーターで指定したサイズ以上のサイズを要求されます。
			/// </para>
			/// </remarks>
			public Int32 cacheSize;

		}
		/// <summary>ストリーミングキャッシュの作成</summary>
		/// <param name="config">ストリーミングキャッシュ作成用構造体</param>
		/// <param name="work">ストリーミングキャッシュ作成用ワーク</param>
		/// <param name="workSize">
		/// ストリーミングキャッシュ作成用ワークサイズ
		/// return		<see cref="CriAtomStreamingCache"/>	ストリーミングキャッシュID
		/// </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ストリーミングキャッシュを作成します。
		/// Atomプレーヤーにストリーミングキャッシュを設定することで、
		/// ストリーミング再生を行いつつメモリ上にファイル全体を保持（キャッシュ）します。
		/// 同じファイルの2回目以降の再生では、キャッシュを使ったメモリ再生に自動的に切り替わります。
		/// また、再生データがループデータを持っていた場合、
		/// ループ以降の再生は自動的にメモリ再生で行われるようになります。
		/// 本機能はAtomプレーヤーがストリーミング再生を行う場合のみ機能します。
		/// 本関数に失敗した場合、<see cref="CriAtom.StreamingCacheIllegalId"/>が返ります。
		/// </para>
		/// <para>
		/// 注意:
		/// ファイル全体をキャッシュする事が前提です。よって、キャッシュ用に割り当てられた
		/// メモリサイズがストリーミング再生対象とするどのファイルサイズよりも小さい場合、
		/// 一切キャッシュされません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CalculateWorkSize"/>
		/// <seealso cref="CriAtomStreamingCache.Dispose"/>
		public unsafe CriAtomStreamingCache(in CriAtomStreamingCache.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomStreamingCache.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomStreamingCache_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomStreamingCache(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomStreamingCache.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomStreamingCache_Create(configPtr, work, workSize);
		}

		/// <summary>ストリーミングキャッシュの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したストリーミングキャッシュを破棄します。
		/// </para>
		/// <para>
		/// 注意:
		/// 指定したストリーミングキャッシュを利用しているプレーヤーが存在しない状態で、
		/// 本関数を実行してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomStreamingCache_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomStreamingCache() => Dispose();
#pragma warning restore 1591

		/// <summary>ストリーミングキャッシュのキャッシュ内容をクリア</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したストリーミングキャッシュの内容をクリアします。
		/// キャッシュは古い順にクリアされます。
		/// 指定したストリーミングキャッシュを使用中のプレーヤーが存在する場合、
		/// キャッシュのクリアは途中で中断されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 一番古いキャッシュを使用中のプレーヤーが存在する場合、本関数を実行しても
		/// キャッシュは一切クリアされません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		public void Clear()
		{
			NativeMethods.criAtomStreamingCache_Clear(NativeHandle);
		}

		/// <summary>WaveID指定でキャッシュ済み検索</summary>
		/// <param name="awb">AWBオブジェクト</param>
		/// <param name="id">WaveID</param>
		/// <returns>キャッシュ済みであればtrue、それ以外はfalse</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したストリーミングキャッシュ中に、指定の音声データがキャッシュされているかを検索します。
		/// 指定の音声データがキャッシュされている状態であればtrueを、
		/// キャッシュされていない状態であればfalseを返します。
		/// </para>
		/// <para>
		/// 注意:
		/// AWBオブジェクトがメモリ再生用の場合、本関数は音声データの有無にかかわらずtrueを返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		public bool IsCachedWaveId(CriAtomAwb awb, Int32 id)
		{
			return NativeMethods.criAtomStreamingCache_IsCachedWaveId(NativeHandle, awb?.NativeHandle ?? default, id);
		}

		/// <summary>パス指定でキャッシュ済み検索</summary>
		/// <param name="srcBinder">音声データファイル読み込み元のバインダーオブジェクト</param>
		/// <param name="path">音声データファイルのパス</param>
		/// <returns>キャッシュ済みであればtrue、それ以外はfalse</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したストリーミングキャッシュ中に、指定の音声データがキャッシュされているかを検索します。
		/// 指定の音声データがキャッシュされている状態であればtrueを、
		/// キャッシュされていない状態であればfalseを返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		public bool IsCachedFile(CriFsBinder srcBinder, ArgString path)
		{
			return NativeMethods.criAtomStreamingCache_IsCachedFile(NativeHandle, srcBinder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]));
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomStreamingCache(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomStreamingCache other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomStreamingCache a, CriAtomStreamingCache b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomStreamingCache a, CriAtomStreamingCache b) =>
			!(a == b);

	}
}