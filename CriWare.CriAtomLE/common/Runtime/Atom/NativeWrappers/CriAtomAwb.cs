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
	/// <summary>AWBオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 再生データが含まれているAWBファイルのTOC情報を示すオブジェクトです。
	/// <see cref="CriAtomAwb.LoadToc"/> 関数で取得します。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomAwb.LoadToc"/>
	public partial class CriAtomAwb : IDisposable
	{
		/// <summary>AWBファイルのTOC情報ロード用ワーク領域サイズの計算</summary>
		/// <param name="num">AWBファイルに含まれるコンテンツ数</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBファイルのTOC情報をロードするために十分なワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.LoadToc"/>
		/// <seealso cref="CriAtomAwb.LoadTocAsync"/>
		public static Int32 CalculateWorkSizeForLoadToc(Int32 num)
		{
			return NativeMethods.criAtomAwb_CalculateWorkSizeForLoadToc(num);
		}

		/// <summary>AWBファイルのTOC情報ロード（同期版）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="path">AWBファイル名</param>
		/// <param name="work">AWBファイルのTOC情報ロード用ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>AWBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声再生の音源として使用するAWBファイルのTOC情報をロードします。
		/// ロード完了まで本関数は復帰しませんので、シーンの切り替えや、
		/// 負荷変動が許容できるタイミングで実行してください。
		/// ロードに成功すると、戻り値に有効なAWBオブジェクトが返りますので、
		/// <see cref="CriAtomPlayer.SetWaveId"/> 関数に指定して使用してください。
		/// 使い終わったAWBオブジェクトは、<see cref="CriAtomAwb.Dispose"/> 関数で解放してください。
		/// TOC情報のロードに失敗した場合はnullが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第三引数にnull、第四引数に0を指定して実行すると、
		/// 必要なワーク領域を関数内部で動的に確保します。
		/// 動的に確保した領域は、<see cref="CriAtomAwb.Dispose"/> 関数で解放されます。
		/// </para>
		/// <para>
		/// 注意:
		/// AWBオブジェクトは内部的にバインダー（ <see cref="CriFsBinder"/> ）、およびローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// AWBファイルのTOC情報をロードする場合、AWBオブジェクト数分のバインダー、およびローダーが
		/// 確保できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetWaveId"/>
		/// <seealso cref="CriAtomAwb.Dispose"/>
		/// <seealso cref="CriAtomAwb.LoadTocById"/>
		public static CriAtomAwb LoadToc(CriFsBinder binder, ArgString path, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomAwb_LoadToc(binder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]), work, workSize)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>ID指定によるAWBファイルのTOC情報ロード（同期版）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="id">AWBファイルが格納されているCPKコンテンツID</param>
		/// <param name="work">AWBファイルのTOC情報ロード用ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>AWBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomAwb.LoadToc"/> 関数とほぼ同様の機能を持つ関数です。
		/// <see cref="CriAtomAwb.LoadToc"/> 関数と異なる点は、
		/// パス指定ではなくCPK内のコンテンツID指定でAWBファイルのTOC情報をロードする点です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetWaveId"/>
		/// <seealso cref="CriAtomAwb.Dispose"/>
		/// <seealso cref="CriAtomAwb.LoadToc"/>
		public static CriAtomAwb LoadTocById(CriFsBinder binder, UInt16 id, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomAwb_LoadTocById(binder?.NativeHandle ?? default, id, work, workSize)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>AWBファイルのTOC情報ロード（非同期版）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="path">AWBファイル名</param>
		/// <param name="work">AWBファイルのTOC情報ロード用ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>AWBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声再生の音源として使用するAWBファイルのTOC情報をロードします。
		/// 本関数はロード要求を発行する非同期関数なので、
		/// ロードの完了をアプリケーション側で待つ必要があります。
		/// <see cref="CriAtomAwb.GetStatus"/> 関数でAWBオブジェクトのステータスを定期的に取得し、
		/// ロードの完了を確認してください。
		/// ロードの完了を待っている間はAWBオブジェクトのステータス更新のために、
		/// 定期的に<see cref="CriAtom.ExecuteMain"/> 関数を実行する必要があります。
		/// ロード要求の発行に成功すると戻り値に有効なAWBオブジェクトが返ります。
		/// ロードが正しく完了した後は、<see cref="CriAtomPlayer.SetWaveId"/> 関数に指定して使用してください。
		/// 使い終わったAWBオブジェクトは、<see cref="CriAtomAwb.Dispose"/> 関数で解放してください。
		/// TOC情報のロード要求の発行に失敗した場合はnullが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第三引数にnull、第四引数に0を指定して実行すると、
		/// 必要なワーク領域を関数内部で動的に確保します。
		/// 動的に確保した領域は、<see cref="CriAtomAwb.Dispose"/> 関数で解放されます。
		/// 本関数で取得したAWBオブジェクトのステータスがエラー状態（<see cref="CriAtomAwb.Status.Error"/>）になった場合も、
		/// <see cref="CriAtomAwb.Dispose"/> 関数で解放してください。
		/// </para>
		/// <para>
		/// 注意:
		/// AWBオブジェクトは内部的にバインダー（ <see cref="CriFsBinder"/> ）、およびローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// AWBファイルのTOC情報をロードする場合、AWBオブジェクト数分のバインダー、およびローダーが
		/// 確保できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetWaveId"/>
		/// <seealso cref="CriAtomAwb.Dispose"/>
		/// <seealso cref="CriAtomAwb.GetStatus"/>
		/// <seealso cref="CriAtomAwb.LoadTocAsyncById"/>
		public static CriAtomAwb LoadTocAsync(CriFsBinder binder, ArgString path, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomAwb_LoadTocAsync(binder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]), work, workSize)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>ID指定によるAWBファイルのTOC情報ロード（非同期版）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="id">AWBファイルが格納されているCPKコンテンツID</param>
		/// <param name="work">AWBファイルのTOC情報ロード用ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>AWBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomAwb.LoadTocAsync"/> 関数とほぼ同様の機能を持つ関数です。
		/// <see cref="CriAtomAwb.LoadTocAsync"/> 関数と異なる点は、
		/// パス指定ではなくCPK内のコンテンツID指定でAWBファイルのTOC情報をロードする点です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetWaveId"/>
		/// <seealso cref="CriAtomAwb.Dispose"/>
		/// <seealso cref="CriAtomAwb.GetStatus"/>
		/// <seealso cref="CriAtomAwb.LoadTocAsync"/>
		public static CriAtomAwb LoadTocAsyncById(CriFsBinder binder, UInt16 id, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomAwb_LoadTocAsyncById(binder?.NativeHandle ?? default, id, work, workSize)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>オンメモリAWBオブジェクトの作成</summary>
		/// <param name="awbMem">AWBファイルを読み込んだメモリ領域へのポインタ</param>
		/// <param name="awbMemSize">AWBファイルサイズ</param>
		/// <param name="work">オンメモリAWB用ワーク領域へのポインタ</param>
		/// <param name="workSize">オンメモリAWB用ワーク領域サイズ</param>
		/// <returns>オンメモリAWBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ上に読み込まれたAWBファイルイメージから、オンメモリAWBオブジェクトを作成します。
		/// 同じオンメモリAWBファイルイメージから複数のオンメモリAWBオブジェクトを作成することができます。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// User Allocator方式を用いる場合、ユーザがワーク領域を用意する必要はありません。
		/// workにnull、work_sizeに0を指定するだけで、必要なメモリを登録済みのメモリ確保関数から確保します。
		/// オンメモリAWBオブジェクト作成時に動的に確保されたメモリは、
		/// オンメモリAWB破棄時（ <see cref="CriAtomAwb.Dispose"/> 関数実行時）に解放されます。
		/// Fixed Memor方式を用いる場合は、<see cref="CriAtomAwb.WorksizeForLoadfrommemory"/> 関数を使って
		/// 必要なワーク領域サイズを求めてください。
		/// ワーク領域とは異なり、awb_mem は必ずユーザの責任で管理する必要がある点には注意してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数が成功すると、 awb_memで渡されたメモリ領域をオンメモリAWBデータ用に書き換えます。
		/// そのため、複数スレッドから<see cref="CriAtomAwb.WorksizeForLoadfrommemory"/> 関数を呼び出している場合は、
		/// 排他処理によりスレッドの実行順が入れ替わる場合があります。
		/// なお、awb_mem で指すメモリ領域は <see cref="CriAtomAwb.Dispose"/> 関数実行後に手動で解放してください。
		/// AWBオブジェクトは内部的にバインダー（ <see cref="CriFsBinder"/> ）を確保します。
		/// AWBファイルのTOC情報をロードする場合、AWBオブジェクト数分のバインダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.WorksizeForLoadfrommemory"/>
		/// <seealso cref="CriAtomAwb.Dispose"/>
		public static CriAtomAwb LoadFromMemory(IntPtr awbMem, Int32 awbMemSize, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomAwb_LoadFromMemory(awbMem, awbMemSize, work, workSize)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>AWBオブジェクトの種別を示す値を取得</summary>
		/// <returns>AWBオブジェクトの種別を示す値</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトがTOC情報のみであるか、オンメモリAWBオブジェクトなのかを示す値を取得します。
		/// 本関数が失敗した場合は<see cref="CriAtomAwb.Type.Error"/>を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.GetWaveDataInfo"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		public CriAtomAwb.Type GetAwbType()
		{
			return NativeMethods.criAtomAwb_GetType(NativeHandle);
		}

		/// <summary>AWBの種別</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトがTOC情報のみのオブジェクトなのか、メモリ上にロードされたAWBオブジェクトなのかを示す値です。
		/// <see cref="CriAtomAwb.GetAwbType"/> 関数で取得します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.GetAwbType"/>
		public enum Type
		{
			/// <summary>TOC情報のみのAWBオブジェクト</summary>
			Toc = 0,
			/// <summary>オンメモリAWBオブジェクト</summary>
			Onmemory = 1,
			/// <summary>無効なAWBオブジェクト</summary>
			Error = 2,
		}
		/// <summary>AWBのTOC情報から波形データのファイル情報を取得</summary>
		/// <param name="id">波形データID</param>
		/// <param name="offset">波形データのオフセット（Byte）</param>
		/// <param name="size">波形データのサイズ（Byte）</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトからidで指定した波形データのファイルオフセットとサイズを取得します。
		/// 取得したオフセットとサイズは、AWBファイルから波形データを直接読み込む場合に使用します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、AWBオブジェクトの種別を<see cref="CriAtomAwb.GetAwbType"/> 関数で確認してください。
		/// AWBオブジェクトの種別が<see cref="CriAtomAwb.Type.Toc"/>と異なる場合、または不正なAWBオブジェクトだった場合、本関数は失敗し、エラーコールバックが発生します。
		/// 本関数が失敗した場合、出力値であるoffsetとsizeの値は不定です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.GetAwbType"/>
		/// <seealso cref="CriAtomAwb.GetWaveDataInfo"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomAwb.GetAwbType"/>
		public unsafe bool GetWaveFileInfo(Int32 id, out Int64 offset, out UInt32 size)
		{
			fixed (Int64* offsetPtr = &offset)
			fixed (UInt32* sizePtr = &size)
				return NativeMethods.criAtomAwb_GetWaveFileInfo(NativeHandle, id, offsetPtr, sizePtr);
		}

		/// <summary>オンメモリAWBから波形データの情報を取得</summary>
		/// <param name="id">波形データID</param>
		/// <param name="waveDataStart">波形データの先頭ポインタ（Byte）</param>
		/// <param name="size">波形データのサイズ（Byte）</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オンメモリAWBからidで指定した波形データの先頭ポインタとサイズを取得します。
		/// オンメモリAWBから波形データを再生する場合、通常は<see cref="CriAtomPlayer.SetWaveId"/> 関数で十分なので、そちらも参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、AWBオブジェクトの種別を<see cref="CriAtomAwb.GetAwbType"/> 関数で確認してください。
		/// AWBオブジェクトの種別が異なる場合、または不正なAWBオブジェクトだった場合、本関数は失敗し、エラーコールバックが発生します。
		/// 本関数が失敗した場合、出力値であるwave_data_startとsizeの値は不定です。
		/// *
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.GetAwbType"/>
		/// <seealso cref="CriAtomAwb.GetWaveFileInfo"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetWaveId"/>
		public unsafe void GetWaveDataInfo(Int32 id, out IntPtr waveDataStart, out UInt32 size)
		{
			fixed (IntPtr* waveDataStartPtr = &waveDataStart)
			fixed (UInt32* sizePtr = &size)
				NativeMethods.criAtomAwb_GetWaveDataInfo(NativeHandle, id, waveDataStartPtr, sizePtr);
		}

		/// <summary>AWBオブジェクトを介してAWBファイルに含まれるコンテンツ数を取得</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトを介してAWBファイルに含まれているコンテンツ数（波形データ数）を取得します。
		/// コンテンツファイル数の値の有効範囲は1～65535です。
		/// エラーが発生した場合は 0 を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.LoadToc"/>
		/// <seealso cref="CriAtomAwb.LoadFromMemory"/>
		public UInt16 GetNumContents()
		{
			return NativeMethods.criAtomAwb_GetNumContents(NativeHandle);
		}

		/// <summary>AWBオブジェクトの解放</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトを解放します。
		/// 使い終わったAWBオブジェクトは、本関数で解放してください。
		/// 解放したAWBオブジェクトは無効なオブジェクトになるので、使用しないでください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、指定したAWBデータを参照しているAtomプレーヤーが存在しないか、
		/// ライブラリ内で検索処理が行われます。
		/// そのため、本関数実行中に他スレッドでAtomプレーヤーの作成／破棄を行うと、
		/// アクセス違反やデッドロック等の重大な不具合を誘発する恐れがあります。
		/// 本関数実行時にAtomプレーヤーの作成／破棄を他スレッドで行う必要がある場合、
		/// 本関数を <see cref="CriAtom.Lock"/> 関数でロックしてから実行してください。
		/// AtomExプレーヤーを使用してAWBファイルを再生する場合、
		/// 再生中に本関数でAWBオブジェクトを破棄してはいけません。
		/// 必ずAtomExプレーヤーを停止させてから本関数を実行してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.LoadToc"/>
		/// <seealso cref="CriAtomAwb.LoadTocAsync"/>
		/// <seealso cref="CriAtomAwb.GetStatus"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomAwb_Release(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomAwb() => Dispose();
#pragma warning restore 1591

		/// <summary>AWBオブジェクトが即時解放可能かどうかのチェック</summary>
		/// <returns>AWBの状態（true = 即時解放可能、false = 再生中のプレーヤーあり）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトを即座に解放可能かどうかをチェックします。
		/// 本関数が false を返すタイミングで <see cref="CriAtomAwb.Dispose"/> 関数を実行すると、
		/// AWBオブジェクトを参照しているプレーヤーに対する停止処理が行われます。
		/// （ストリーム再生用のAWBオブジェクトの場合、ファイル読み込み完了を待つため、
		/// <see cref="CriAtomAwb.Dispose"/> 関数内で長時間処理がブロックされる可能性があります。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、指定したAWBデータを参照しているAtomプレーヤーが存在しないか、
		/// ライブラリ内で検索処理が行われます。
		/// そのため、本関数実行中に他スレッドでAtomプレーヤーの作成／破棄を行うと、
		/// アクセス違反やデッドロック等の重大な不具合を誘発する恐れがあります。
		/// 本関数実行時にAtomプレーヤーの作成／破棄を他スレッドで行う必要がある場合、
		/// 本関数を <see cref="CriAtom.Lock"/> 関数でロックしてから実行してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.Dispose"/>
		public bool IsReadyToRelease()
		{
			return NativeMethods.criAtomAwb_IsReadyToRelease(NativeHandle);
		}

		/// <summary>AWBオブジェクトのステータス取得</summary>
		/// <returns>AWBオブジェクトの状態を示す値</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトのステータスを取得します。
		/// 本関数で取得するAWBオブジェクトのステータスは、<see cref="CriAtom.ExecuteMain"/> 関数を
		/// 実行することで更新されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.LoadToc"/>
		/// <seealso cref="CriAtomAwb.LoadTocAsync"/>
		public CriAtomAwb.Status GetStatus()
		{
			return NativeMethods.criAtomAwb_GetStatus(NativeHandle);
		}

		/// <summary>AWBステータス</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBの準備状態を示す値です。
		/// <see cref="CriAtomAwb.GetStatus"/> 関数で取得します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.GetStatus"/>
		public enum Status
		{
			/// <summary>停止中</summary>
			Stop = 0,
			/// <summary>ロード中</summary>
			Loading = 1,
			/// <summary>ロード完了</summary>
			Complete = 2,
			/// <summary>ロード失敗</summary>
			Error = 3,
		}
		/// <summary>波形データIDの取得</summary>
		/// <param name="index">波形インデックス</param>
		/// <returns>(0以上)								正常に処理が完了</returns>
		/// <returns>(-1)		エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AWBオブジェクトからindexで指定した波形データIDを取得します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.LoadToc"/>
		/// <seealso cref="CriAtomAwb.LoadTocAsync"/>
		public Int32 GetIdByIndex(UInt16 index)
		{
			return NativeMethods.criAtomAwb_GetIdByIndex(NativeHandle, index);
		}

		/// <summary>オンメモリAWBオブジェクトの作成に必要なワーク領域サイズ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オンメモリAWBオブジェクトの作成に必要なワーク領域サイズです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAwb.LoadFromMemory"/>
		public const Int32 WorksizeForLoadfrommemory = (64);
		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomAwb(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomAwb other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomAwb a, CriAtomAwb b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomAwb a, CriAtomAwb b) =>
			!(a == b);

	}
}