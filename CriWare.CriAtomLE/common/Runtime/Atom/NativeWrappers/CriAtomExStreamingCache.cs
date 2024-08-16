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
	/// <see cref="CriAtomExStreamingCache"/> は、ストリーミングキャッシュ管理用IDです。
	/// <see cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/> 関数でストリーミングキャッシュを作成すると取得できます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/>
	public partial struct CriAtomExStreamingCache
	{
		/// <summary>指定したCue（ID指定）のストリーム用データがキャッシュ済みかを取得します</summary>
		/// <param name="acbHn">Cueを含んでいるACBオブジェクト</param>
		/// <param name="id">キャッシュ済み確認対象のCueID</param>
		/// <returns>= キャッシュ済み</returns>
		/// <returns>= 未キャッシュ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 対象とするストリーミングキャッシュにおいて、
		/// IDで指定したCueのストリーミング用データがキャッシュ済みかを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、Cueが複数のストリーム用データを持つ場合については正確な情報を返しません。
		/// Cueが複数のストリーム用データを持つ場合、
		/// Cue内で最初に見つかったストリーム用データがキャッシュされた段階で
		/// trueを返します。
		/// </para>
		/// </remarks>
		public bool IsCachedWaveformById(CriAtomExAcb acbHn, Int32 id)
		{
			return NativeMethods.criAtomExStreamingCache_IsCachedWaveformById(NativeHandle, acbHn?.NativeHandle ?? default, id);
		}

		/// <summary>指定したCue（名前指定）のストリーム用データがキャッシュ済みかを取得します</summary>
		/// <param name="acbHn">Cueを含んでいるACBオブジェクト</param>
		/// <param name="name">キャッシュ済み確認対象のCue名</param>
		/// <returns>= キャッシュ済み</returns>
		/// <returns>= 未キャッシュ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 対象とするストリーミングキャッシュにおいて、
		/// Cue名で指定したCueのストリーミング用データがキャッシュ済みかを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、Cueが複数のストリーム用データを持つ場合については正確な情報を返しません。
		/// Cueが複数のストリーム用データを持つ場合、
		/// Cue内で最初に見つかったストリーム用データがキャッシュされた段階で
		/// trueを返します。
		/// </para>
		/// </remarks>
		public bool IsCachedWaveformByName(CriAtomExAcb acbHn, ArgString name)
		{
			return NativeMethods.criAtomExStreamingCache_IsCachedWaveformByName(NativeHandle, acbHn?.NativeHandle ?? default, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>指定したCue（ID指定）のストリーム用データをキャッシュにロード</summary>
		/// <param name="acbHn">Cueを含んでいるACBオブジェクト</param>
		/// <param name="cueId">キャッシュ対象のCueID</param>
		/// <returns>成功／失敗</returns>
		/// <returns>= ロードの失敗</returns>
		/// <returns>= ロードの成功</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ストリーミングキャッシュに対し、IDで指定したCueのストリーミング用データをロードします。
		/// 本関数に成功すると::trueを返し、指定したCueがキャッシュ完了状態になります。
		/// 本関数に失敗すると、::falseを返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は完了復帰です。
		/// </para>
		/// <para>
		/// 注意:
		/// Cueが複数のストリーム用データを持つ場合、
		/// 本関数はCue内で最初に見つかったストリーム用データのみをロードします。
		/// </para>
		/// </remarks>
		public bool LoadWaveformById(CriAtomExAcb acbHn, Int32 cueId)
		{
			return NativeMethods.criAtomExStreamingCache_LoadWaveformById(NativeHandle, acbHn?.NativeHandle ?? default, cueId);
		}

		/// <summary>指定したCue（名前指定）のストリーム用データをキャッシュにロード</summary>
		/// <param name="acbHn">Cueを含んでいるACBオブジェクト</param>
		/// <param name="name">キャッシュ対象のCue名</param>
		/// <returns>成功／失敗</returns>
		/// <returns>= ロードの失敗</returns>
		/// <returns>= ロードの成功</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ストリーミングキャッシュに対し、IDで指定したCueのストリーミング用データをロードします。
		/// 本関数に成功すると::trueを返し、指定したCueがキャッシュ完了状態になります。
		/// 本関数に失敗すると、::falseを返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は完了復帰です。
		/// </para>
		/// <para>
		/// 注意:
		/// Cueが複数のストリーム用データを持つ場合、
		/// 本関数はCue内で最初に見つかったストリーム用データのみをロードします。
		/// </para>
		/// </remarks>
		public bool LoadWaveformByName(CriAtomExAcb acbHn, ArgString name)
		{
			return NativeMethods.criAtomExStreamingCache_LoadWaveformByName(NativeHandle, acbHn?.NativeHandle ?? default, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary><see cref="CriAtomExStreamingCache.Config"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">ストリーミングキャッシュ作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExStreamingCache.CalculateWorkSize"/> 関数、
		/// <see cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExStreamingCache.Config"/> ）に対し、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExStreamingCache.CalculateWorkSize"/>
		/// <seealso cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/>
		public static unsafe void SetDefaultConfig(out CriAtomExStreamingCache.Config pConfig)
		{
			fixed (CriAtomExStreamingCache.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExStreamingCache_SetDefaultConfig_(pConfigPtr);
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
		/// <summary>ストリーミングキャッシュ作成に必要なワークサイズの計算</summary>
		/// <param name="pConfig">ストリーミングキャッシュ作成用構造体</param>
		/// <returns>ストリーミングキャッシュ作成に必要なワークサイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ストリーミングキャッシュ作成に必要なワークサイズを計算します。
		/// configで与えられるパラメーターに依存し、必要なワークサイズは増加します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExStreamingCache.Config pConfig)
		{
			fixed (CriAtomExStreamingCache.Config* pConfigPtr = &pConfig)
				return NativeMethods.criAtomExStreamingCache_CalculateWorkSize_(pConfigPtr);
		}

		/// <summary>ストリーミングキャッシュの作成</summary>
		/// <param name="config">ストリーミングキャッシュ作成用構造体</param>
		/// <param name="work">ストリーミングキャッシュ作成用ワーク</param>
		/// <param name="workSize">
		/// ストリーミングキャッシュ作成用ワークサイズ
		/// return		<see cref="CriAtomExStreamingCache"/>	ストリーミングキャッシュID
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
		/// 本関数に失敗した場合、<see cref="CriAtomEx.StreamingCacheIllegalId"/>が返ります。
		/// </para>
		/// <para>
		/// 注意:
		/// ファイル全体をキャッシュする事が前提です。
		/// よって、キャッシュ用に割り当てられたメモリサイズがストリーミング再生対象とする
		/// どのファイルサイズよりも小さい場合、一切キャッシュされません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExStreamingCache.CalculateWorkSize"/>
		/// <seealso cref="CriAtomExStreamingCache.Destroy"/>
		public unsafe CriAtomExStreamingCache(in CriAtomExStreamingCache.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExStreamingCache.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomExStreamingCache_Create_(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomExStreamingCache(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomExStreamingCache.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomExStreamingCache_Create_(configPtr, work, workSize);
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
		/// <seealso cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/>
		public void Destroy()
		{
			NativeMethods.criAtomExStreamingCache_Destroy_(NativeHandle);
		}

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
		/// <seealso cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/>
		public void Clear()
		{
			NativeMethods.criAtomExStreamingCache_Clear_(NativeHandle);
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
		/// <seealso cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/>
		public bool IsCachedWaveId(CriAtomAwb awb, Int32 id)
		{
			return NativeMethods.criAtomExStreamingCache_IsCachedWaveId_(NativeHandle, awb?.NativeHandle ?? default, id);
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
			return NativeMethods.criAtomExStreamingCache_IsCachedFile_(NativeHandle, srcBinder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]));
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExStreamingCache(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExStreamingCache other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExStreamingCache a, CriAtomExStreamingCache b)
		{

			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExStreamingCache a, CriAtomExStreamingCache b) =>
			!(a == b);

	}
}