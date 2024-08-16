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
	/// <summary>ASRラックID</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// ASRラック管理用のID型です。
	/// <see cref="CriAtomExAsrRack.CriAtomExAsrRack"/> 関数でASRラックを作成すると取得できます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
	/// <seealso cref="CriAtomExAsrRack.AttachDspBusSetting"/>
	public partial class CriAtomExAsrRack : IDisposable
	{
		/// <summary>波形フィルターコールバック関数の登録</summary>
		/// <param name="busName">バス名</param>
		/// <param name="preFunc">エフェクト処理前のフィルターコールバック関数</param>
		/// <param name="postFunc">エフェクト処理後のフィルターコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスに流れている PCM データを受け取るコールバック関数を登録します。
		/// 登録されたコールバック関数は、サウンドレンダラが音声処理を行ったタイミングで呼び出されます。
		/// エフェクト処理前とエフェクト処理後の2種類の使用しないほうはnull指定が可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 波形フィルターコールバック関数内で長時間処理をブロックすると、音切れ等の問題
		/// が発生しますので、ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.FilterCbFunc"/>
		public unsafe void SetBusFilterCallbackByName(ArgString busName, delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void> preFunc, delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void> postFunc, IntPtr obj)
		{
			NativeMethods.criAtomExAsrRack_SetBusFilterCallbackByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), (IntPtr)preFunc, (IntPtr)postFunc, obj);
		}

		/// <summary>チャンネルベース 再生用 ASR ラックIDを取得</summary>
		/// <returns>ASRラックID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// チャンネルベース 再生のみを行うASRラックのIDを取得します。
		/// 作成されていない場合、<see cref="CriAtomExAsr.RackIllegalId"/>が返されます。
		/// </para>
		/// </remarks>
		public static Int32 GetChannelBasedAudioRackId()
		{
			return NativeMethods.criAtomExAsrRack_GetChannelBasedAudioRackId();
		}

		/// <summary>ObjectBasedAudio 再生用ASRラックIDを取得</summary>
		/// <returns>ASRラックID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ObjectBasedAudio 再生のみを行うASRラックのIDを取得します。
		/// 作成されていない場合、<see cref="CriAtomExAsr.RackIllegalId"/>が返されます。
		/// </para>
		/// </remarks>
		public static Int32 GetObjectBasedAudioRackId()
		{
			return NativeMethods.criAtomExAsrRack_GetObjectBasedAudioRackId();
		}

		/// <summary><see cref="CriAtomAsr.Config"/>へのデフォルトパラメーターをセット</summary>
		/// <param name="pConfig">初期化用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomAsr.Initialize"/> 関数に設定するコンフィグ構造体（ <see cref="CriAtomAsr.Config"/> ）に、
		/// デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAsr.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomExAsrRack.Config pConfig)
		{
			fixed (CriAtomExAsrRack.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExAsrRack_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>ASRラック作成用ワーク領域サイズの計算</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASRラックの作成に必要なワーク領域のサイズを取得します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExAsrRack.CriAtomExAsrRack"/> 関数でASRの初期化を行う場合、
		/// 本関数で計算したサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ASRラックの初期化に必要なワークメモリのサイズは、ASRラック初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomExAsrRack.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExAsr.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExAsrRack.Config config)
		{
			fixed (CriAtomExAsrRack.Config* configPtr = &config)
				return NativeMethods.criAtomExAsrRack_CalculateWorkSize(configPtr);
		}

		/// <summary>ASRラック作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomExAsrRack.SetDefaultConfig"/> メソッドで
		/// 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomExAsrRack.CriAtomExAsrRack"/> 関数
		/// に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExAsrRack.SetDefaultConfig"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		/// <seealso cref="CriAtomExAsrRack.SetDefaultConfig"/>
		public unsafe partial struct Config
		{
			/// <summary>サーバー処理の実行頻度</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// サーバー処理を実行する頻度を指定します。
			/// </para>
			/// <para>
			/// 注意:
			/// Atomライブラリ初期化時に指定した値（ <see cref="CriAtomEx.Config"/> 構造体の
			/// server_frequency ）と、同じ値をセットする必要があります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.Config"/>
			public Single serverFrequency;

			/// <summary>バス数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRが作成するバスの数を指定します。
			/// バスはサウンドのミックスや、エフェクトの管理等を行います。
			/// </para>
			/// </remarks>
			public Int32 numBuses;

			/// <summary>出力チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRラックの出力チャンネル数を指定します。
			/// パン3Dもしくは3Dポジショニング機能を使用する場合は6ch以上を指定します。
			/// </para>
			/// </remarks>
			public Int32 outputChannels;

			/// <summary>ミキサーのスピーカーマッピング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRラックのスピーカーマッピングを指定します。
			/// </para>
			/// </remarks>
			public CriAtom.SpeakerMapping speakerMapping;

			/// <summary>出力サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRラックの出力および処理過程のサンプリングレートを指定します。
			/// 通常、ターゲット機のサウンドデバイスのサンプリングレートを指定します。
			/// </para>
			/// <para>
			/// 備考:
			/// 低くすると処理負荷を下げることができますが音質が落ちます。
			/// </para>
			/// </remarks>
			public Int32 outputSamplingRate;

			/// <summary>サウンドレンダラタイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRラックの出力先サウンドレンダラの種別を指定します。
			/// sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、
			/// 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>出力先ASRラックID</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRラックの出力先ASRラックIDを指定します。
			/// sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合のみ有効です。
			/// </para>
			/// </remarks>
			public Int32 outputRackId;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。
			/// nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでASRラックを作成します。
			/// パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。
			/// パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。
			/// </para>
			/// </remarks>
			public IntPtr context;

		}
		/// <summary>DSPバス設定のアタッチ用ワークサイズの計算</summary>
		/// <param name="config">ASRラック作成用コンフィグ構造体</param>
		/// <param name="setting">DSPバス設定の名前</param>
		/// <returns>必要ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定からバスを構築するのに必要なワーク領域サイズを計算します。
		/// 本関数を実行するには、あらかじめ::criAtomEx_RegisterAcfConfig 関数でACF情報を
		/// 登録しておく必要があります
		/// configには <see cref="CriAtomExAsrRack.CriAtomExAsrRack"/> 関数に指定するものと同じ構造体を指定してください。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバス設定のアタッチに必要なワークメモリのサイズは、CRI Atom Craftで作成した
		/// DSPバス設定の内容によって変化します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachDspBusSetting"/>
		public static unsafe Int32 CalculateWorkSizeForDspBusSettingFromConfig(in CriAtomExAsrRack.Config config, ArgString setting)
		{
			fixed (CriAtomExAsrRack.Config* configPtr = &config)
				return NativeMethods.criAtomExAsrRack_CalculateWorkSizeForDspBusSettingFromConfig(configPtr, setting.GetPointer(stackalloc byte[setting.BufferSize]));
		}

		/// <summary>DSPバス設定のアタッチ用ワークサイズの計算</summary>
		/// <param name="setting">DSPバス設定の名前</param>
		/// <returns>必要ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定からバスを構築するのに必要なワーク領域サイズを計算します。
		/// 本関数を実行するには、あらかじめ::criAtomEx_RegisterAcfConfig 関数でACF情報を
		/// 登録しておく必要があります
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバス設定のアタッチに必要なワークメモリのサイズは、CRI Atom Craftで作成した
		/// DSPバス設定の内容によって変化します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachDspBusSetting"/>
		public Int32 CalculateWorkSizeForDspBusSetting(ArgString setting)
		{
			return NativeMethods.criAtomExAsrRack_CalculateWorkSizeForDspBusSetting(NativeHandle, setting.GetPointer(stackalloc byte[setting.BufferSize]));
		}

		/// <summary>DSPバス設定のアタッチ用ワークサイズの計算</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <param name="rackConfig">ASRラック作成用コンフィグ構造体</param>
		/// <param name="setting">DSPバス設定の名前</param>
		/// <returns>必要ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定からバスを構築するのに必要なワーク領域サイズを計算します。
		/// <see cref="CriAtomExAsrRack.CalculateWorkSizeForDspBusSettingFromConfig"/> 関数と違い、
		/// メモリ上にロード済みのACFデータを使用してワークメモリサイズの計算が可能です。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバス設定のアタッチに必要なワークメモリのサイズは、CRI Atom Craftで作成した
		/// DSPバス設定の内容によって変化します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachDspBusSetting"/>
		public static unsafe Int32 CalculateWorkSizeForDspBusSettingFromAcfDataAndConfig(IntPtr acfData, Int32 acfDataSize, in CriAtomExAsrRack.Config rackConfig, ArgString setting)
		{
			fixed (CriAtomExAsrRack.Config* rackConfigPtr = &rackConfig)
				return NativeMethods.criAtomExAsrRack_CalculateWorkSizeForDspBusSettingFromAcfDataAndConfig(acfData, acfDataSize, rackConfigPtr, setting.GetPointer(stackalloc byte[setting.BufferSize]));
		}

		/// <summary>ASRラックの作成</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ASRラックID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASRラックの作成を行います。
		/// ASRラックとはバスの集合体のことで、DSPバス設定をアタッチすることができます。
		/// 本関数を実行することでASRにASRラックが追加され、レンダリング結果の出力を開始します。
		/// この関数で追加したASRラックは出力先を選択することができ、プラットフォームネイティブの
		/// サウンドレンダラか、ASRを選択することで他のASRラックに出力することも可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAsr.Initialize"/> 関数を実行すると、デフォルトのASRラックが追加されます。
		/// criAtomExAsr_*** 関数はデフォルトのASRラックを操作するAPIになります。
		/// ASRの初期化に必要なワークメモリのサイズは、ASR初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomExAsr.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExAsr.SetDefaultConfig"/> 適用時と同じパラメーター）で初期化処理を行います。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ASRラックの生成に成功した場合は、本関数は生成したASRラックIDを返します。
		/// 生成に失敗した場合は -1 を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExAsrRack.Dispose"/>
		/// <seealso cref="CriAtomExAsrRack.AttachDspBusSetting"/>
		public unsafe CriAtomExAsrRack(in CriAtomExAsrRack.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExAsrRack.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomExAsrRack_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomExAsrRack(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomExAsrRack.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomExAsrRack_Create(configPtr, work, workSize);
		}
		/// <summary>ASRラックの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASRラックの破棄を行います。
		/// 本関数を実行することで、レンダリング結果の出力が停止されます。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// ASRラック作成時に確保されたメモリ領域が解放されます。
		/// （ASRラック作成時にワーク領域を渡した場合、本関数実行後であれば
		/// ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		public void Dispose()
		{

			NativeMethods.criAtomExAsrRack_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomExAsrRack() => Dispose();
#pragma warning restore 1591

		/// <summary>ASRラックの総レンダリング量の取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASRラックのレンダリング済みサンプル数とサンプリングレートを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数のレンダリング済みサンプル数の増加パターンは実行中のプラットフォームや出力デバイスによって変化する可能性があります。
		/// </para>
		/// </remarks>
		public unsafe void GetNumRenderedSamples(ref Int64 numSamples, ref Int32 samplingRate)
		{
			fixed (Int64* numSamplesPtr = &numSamples)
			fixed (Int32* samplingRatePtr = &samplingRate)
				NativeMethods.criAtomExAsrRack_GetNumRenderedSamples(NativeHandle, numSamplesPtr, samplingRatePtr);
		}

		/// <summary>パフォーマンスモニターのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 現在までの計測結果を破棄します。
		/// パフォーマンスモニターは、
		/// ASRラック作成直後からパフォーマンス情報の取得を開始し、計測結果を累積します。
		/// 以前の計測結果を以降の計測結果に含めたくない場合には、
		/// 本関数を実行し、累積された計測結果を一旦破棄する必要があります。
		/// </para>
		/// </remarks>
		public void ResetPerformanceMonitor()
		{
			NativeMethods.criAtomExAsrRack_ResetPerformanceMonitor(NativeHandle);
		}

		/// <summary>パフォーマンス情報の取得</summary>
		/// <param name="info">パフォーマンス情報</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス情報を取得します。
		/// 本関数は、指定されたASRラックのレンダリング負荷のみを計測します。
		/// </para>
		/// <para>
		/// 備考:
		/// スレッドモデルに<see cref="CriAtomEx.ThreadModel.MultiWithSonicsync"/>を指定しない場合、
		/// 本関数を使用する必要はありません。
		/// （<see cref="CriAtomEx.ThreadModel.MultiWithSonicsync"/>以外のスレッドモデルを使用している場合、
		/// 本関数の処理負荷は、 ::CriAtomExPerformanceInfo に包含されています。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.PerformanceInfo"/>
		public unsafe void GetPerformanceInfo(out CriAtomExAsrRack.PerformanceInfo info)
		{
			fixed (CriAtomExAsrRack.PerformanceInfo* infoPtr = &info)
				NativeMethods.criAtomExAsrRack_GetPerformanceInfo(NativeHandle, infoPtr);
		}

		/// <summary>パフォーマンス情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス情報を取得するための構造体です。
		/// <see cref="CriAtomExAsrRack.GetPerformanceInfo"/> 関数で利用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.GetPerformanceInfo"/>
		public unsafe partial struct PerformanceInfo
		{
			/// <summary>信号生成処理回数</summary>
			public UInt32 processCount;

			/// <summary>処理時間の最終計測値（マイクロ秒単位）</summary>
			public UInt32 lastProcessTime;

			/// <summary>処理時間の最大値（マイクロ秒単位）</summary>
			public UInt32 maxProcessTime;

			/// <summary>処理時間の平均値（マイクロ秒単位）</summary>
			public UInt32 averageProcessTime;

			/// <summary>処理間隔の最終計測値（マイクロ秒単位）</summary>
			public UInt32 lastProcessInterval;

			/// <summary>処理間隔の最大値（マイクロ秒単位）</summary>
			public UInt32 maxProcessInterval;

			/// <summary>処理間隔の平均値（マイクロ秒単位）</summary>
			public UInt32 averageProcessInterval;

			/// <summary>単位処理で生成されたサンプル数の最終計測値</summary>
			public UInt32 lastProcessSamples;

			/// <summary>単位処理で生成されたサンプル数の最大値</summary>
			public UInt32 maxProcessSamples;

			/// <summary>単位処理で生成されたサンプル数の平均値</summary>
			public UInt32 averageProcessSamples;

		}
		/// <summary>DSPバス設定のアタッチ</summary>
		/// <param name="setting">DSPバス設定の名前</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定からバスを構築してASRラックにアタッチします。
		/// 本関数を実行するには、あらかじめ::criAtomEx_RegisterAcfConfig 関数でACF情報を
		/// 登録しておく必要があります
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバス設定のアタッチに必要なワークメモリのサイズは、
		/// CRI Atom Craftで作成したDSPバス設定の内容によって変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.DetachDspBusSetting"/>
		public void AttachDspBusSetting(ArgString setting, IntPtr work = default, Int32 workSize = default)
		{
			NativeMethods.criAtomExAsrRack_AttachDspBusSetting(NativeHandle, setting.GetPointer(stackalloc byte[setting.BufferSize]), work, workSize);
		}

		/// <summary>DSPバス設定のデタッチ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定をASRラックからデタッチします。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// DSPバス設定アタッチ時に確保されたメモリ領域が解放されます。
		/// （DSPバス設定アタッチ時にワーク領域を渡した場合、本関数実行後であれば
		/// ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachDspBusSetting"/>
		public void DetachDspBusSetting()
		{
			NativeMethods.criAtomExAsrRack_DetachDspBusSetting(NativeHandle);
		}

		/// <summary>DSPバススナップショットの適用</summary>
		/// <param name="snapshotName">スナップショット名</param>
		/// <param name="timeMs">時間（ミリ秒）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバススナップショットを適用します。
		/// 本関数を呼び出すと、スナップショットで設定したパラメーターに time_ms 掛けて変化します。
		/// 引数 snapshot_name に CRI_NULL を指定すると、元のDSPバス設定の状態（スナップショットが適用されていない状態）に戻ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachDspBusSetting"/>
		public void ApplyDspBusSnapshot(ArgString snapshotName, Int32 timeMs)
		{
			NativeMethods.criAtomExAsrRack_ApplyDspBusSnapshot(NativeHandle, snapshotName.GetPointer(stackalloc byte[snapshotName.BufferSize]), timeMs);
		}

		/// <summary>適用中のDSPバススナップショット名の取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASRラックIDを指定して適用中のDSPバススナップショット名を取得します。
		/// スナップショットが適用されていない場合はCRI_NULLが返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.ApplyDspBusSnapshot"/>
		public NativeString GetAppliedDspBusSnapshotName()
		{
			return NativeMethods.criAtomExAsrRack_GetAppliedDspBusSnapshotName(NativeHandle);
		}

		/// <summary>バスのボリュームの設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="volume">ボリューム値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのボリュームを設定します。
		/// センドタイプがポストボリューム、ポストパンのセンド先に有効です。
		/// ボリューム値には、0.0f～1.0fの範囲で実数値を指定します。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのボリュームで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// ボリュームのデフォルト値はCRI Atom Craftで設定した値です。
		/// </para>
		/// </remarks>
		public void SetBusVolumeByName(ArgString busName, Single volume)
		{
			NativeMethods.criAtomExAsrRack_SetBusVolumeByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), volume);
		}

		/// <summary>バスのボリュームの取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="volume">ボリューム値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのボリュームを設定します。
		/// センドタイプがポストボリューム、ポストパンのセンド先に有効です。
		/// ボリューム値は実数値で得られます。
		/// ボリュームのデフォルト値はCRI Atom Craftで設定した値です。
		/// </para>
		/// </remarks>
		public unsafe void GetBusVolumeByName(ArgString busName, in Single volume)
		{
			fixed (Single* volumePtr = &volume)
				NativeMethods.criAtomExAsrRack_GetBusVolumeByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), volumePtr);
		}

		/// <summary>バスのパン情報の設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="panInfo">パン情報</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのパン情報を設定します。
		/// センドタイプがポストパンのセンド先に有効です。
		/// パン情報のデフォルト値は CRI Atom Craft で設定した値です。
		/// </para>
		/// </remarks>
		public unsafe void SetBusPanInfoByName(ArgString busName, in CriAtomExAsr.BusPanInfo panInfo)
		{
			fixed (CriAtomExAsr.BusPanInfo* panInfoPtr = &panInfo)
				NativeMethods.criAtomExAsrRack_SetBusPanInfoByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), panInfoPtr);
		}

		/// <summary>バスのパン情報の取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="panInfo">パン情報</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのパン情報を取得します。
		/// </para>
		/// </remarks>
		public unsafe void GetBusPanInfoByName(ArgString busName, out CriAtomExAsr.BusPanInfo panInfo)
		{
			fixed (CriAtomExAsr.BusPanInfo* panInfoPtr = &panInfo)
				NativeMethods.criAtomExAsrRack_GetBusPanInfoByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), panInfoPtr);
		}

		/// <summary>バスのレベル行列の設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="inputChannels">入力チャンネル数</param>
		/// <param name="outputChannels">出力チャンネル数</param>
		/// <param name="matrix">レベル行列を1次元に表したレベル値の配列</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのレベル行列を設定します。
		/// センドタイプがポストパンのセンド先に有効です。
		/// レベルマトリックスは、音声データの各チャンネルの音声を、どのスピーカーから
		/// どの程度の音量で出力するかを指定するための仕組みです。
		/// matrixは[input_channels * output_channels]の配列です。
		/// 入力チャンネルch_inから出力チャンネルch_outにセンドされるレベルは
		/// matrix[ch_in * output_channels + ch_out]にセットします。
		/// レベル行列のデフォルト値は単位行列です。
		/// レベル値には、0.0f～1.0fの範囲で実数値を指定します。
		/// レベル値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのレベルで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// </para>
		/// </remarks>
		public unsafe void SetBusMatrixByName(ArgString busName, Int32 inputChannels, Int32 outputChannels, Single[] matrix)
		{
			fixed (Single* matrixPtr = matrix)
				NativeMethods.criAtomExAsrRack_SetBusMatrixByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), inputChannels, outputChannels, matrixPtr);
		}

		/// <summary>バスのセンドレベルの設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="sendtoBusName">センド先のバス名</param>
		/// <param name="level">レベル値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// センド先バスに音声データを送る際のレベルを設定します。
		/// レベル値には、0.0f～1.0fの範囲で実数値を指定します。
		/// レベル値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのレベルで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// レベルのデフォルト値はCRI Atom Craftで設定した値です。
		/// </para>
		/// </remarks>
		public void SetBusSendLevelByName(ArgString busName, ArgString sendtoBusName, Single level)
		{
			NativeMethods.criAtomExAsrRack_SetBusSendLevelByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), sendtoBusName.GetPointer(stackalloc byte[sendtoBusName.BufferSize]), level);
		}

		/// <summary>エフェクト動作時パラメーターの設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <param name="parameterIndex">エフェクト動作時パラメーターインデックス</param>
		/// <param name="parameterValue">エフェクトパラメーター設定値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エフェクトの動作時パラメーターを設定します。
		/// エフェクトパラメーターを設定する際は、本関数呼び出し前にあらかじめ
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数でバスが構築されている必要があります。
		/// どのバスにどのエフェクトが存在するかは、アタッチしたDSPバス設定に依存します。指定したバスに指定した名前のエフェクトが存在しない場合、関数は失敗します。
		/// また、
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomExAsrRack.UpdateEffectParameters"/>
		public void SetEffectParameter(ArgString busName, ArgString effectName, UInt32 parameterIndex, Single parameterValue)
		{
			NativeMethods.criAtomExAsrRack_SetEffectParameter(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]), parameterIndex, parameterValue);
		}

		/// <summary>エフェクトの動作時パラメーターの反映</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エフェクトの動作時パラメーターを反映します。
		/// 動作時パラメーターを実際に反映するには、<see cref="CriAtomExAsrRack.SetEffectParameter"/> の他にも本関数を呼び出して下さい。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomExAsrRack.SetEffectParameter"/>
		public void UpdateEffectParameters(ArgString busName, ArgString effectName)
		{
			NativeMethods.criAtomExAsrRack_UpdateEffectParameters(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]));
		}

		/// <summary>エフェクトの動作時パラメーターの取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <param name="parameterIndex">エフェクトの動作時パラメーターインデックス</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エフェクトの動作時パラメーターを取得します。
		/// 動作時パラメーターを取得する際は、本関数呼び出し前にあらかじめ
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数でバスが構築されている必要があります。
		/// どのバスにどのエフェクトが存在するかは、アタッチしたDSPバス設定に依存します。指定したバスに指定した名前のエフェクトが存在しない場合、関数は失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		public Single GetEffectParameter(ArgString busName, ArgString effectName, UInt32 parameterIndex)
		{
			return NativeMethods.criAtomExAsrRack_GetEffectParameter(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]), parameterIndex);
		}

		/// <summary>エフェクトのバイパス設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <param name="bypass">バイパス設定（true:バイパスを行う, false:バイパスを行わない）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エフェクトのバイパス設定を行います。
		/// バイパス設定されたエフェクトは音声処理の際、スルーされるようになります。
		/// エフェクトのバイパス設定をする際は、本関数呼び出し前にあらかじめ
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数でバスが構築されている必要があります。
		/// どのバスにどのエフェクトが存在するかは、アタッチしたDSPバス設定に依存します。指定したバスに指定した名前のエフェクトが存在しない場合、関数は失敗します。
		/// </para>
		/// <para>
		/// 注意:
		/// 音声再生中にバイパス設定を行うとノイズが発生することがあります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		public void SetEffectBypass(ArgString busName, ArgString effectName, NativeBool bypass)
		{
			NativeMethods.criAtomExAsrRack_SetEffectBypass(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]), bypass);
		}

		/// <summary>エフェクトのバイパス設定の取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <returns>バイパス設定されているか？（true:バイパスを行う, false:バイパスを行わない）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エフェクトのバイパス設定を取得します。
		/// バイパス設定されたエフェクトは音声処理の際、スルーされるようになります。
		/// エフェクトのバイパス設定をする際は、本関数呼び出し前にあらかじめ
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数でバスが構築されている必要があります。
		/// どのバスにどのエフェクトが存在するかは、アタッチしたDSPバス設定に依存します。指定したバスに指定した名前のエフェクトが存在しない場合、関数はfalseを返却します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.SetEffectBypass"/>
		public bool GetEffectBypass(ArgString busName, ArgString effectName)
		{
			return NativeMethods.criAtomExAsrRack_GetEffectBypass(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]));
		}

		/// <summary>レベル測定機能の追加</summary>
		/// <param name="busName">バス名</param>
		/// <param name="config">レベル測定機能のコンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスにレベル測定機能を追加し、レベル測定処理を開始します。
		/// 本関数を実行後、 ::criAtomExAsrRack_GetBusAnalyzerInfo 関数を実行することで、
		/// RMSレベル（音圧）、ピークレベル（最大振幅）、ピークホールドレベルを
		/// 取得することが可能です。
		/// 複数バスのレベルを計測するには、バスごとに本関数を呼び出す必要があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は <see cref="CriAtomExAsrRack.AttachDspBusSetting"/> 関数と同一のリソースを操作します。
		/// そのため、現状は <see cref="CriAtomExAsrRack.AttachDspBusSetting"/> 関数を実行すると、
		/// ::criAtomExAsrRack_GetBusAnalyzerInfo 関数による情報取得ができなくなります。
		/// 本関数と <see cref="CriAtomExAsrRack.AttachDspBusSetting"/> 関数を併用する際には、
		/// <see cref="CriAtomExAsrRack.AttachDspBusSetting"/> 関数を実行する前に一旦
		/// ::criAtomExAsrRack_DetachBusAnalyzer 関数でレベル測定機能を無効化し、
		/// <see cref="CriAtomExAsrRack.AttachDspBusSetting"/> 関数実行後に再度本関数を実行してください。
		/// </para>
		/// </remarks>
		public unsafe void AttachBusAnalyzerByName(ArgString busName, in CriAtomExAsr.BusAnalyzerConfig config)
		{
			fixed (CriAtomExAsr.BusAnalyzerConfig* configPtr = &config)
				NativeMethods.criAtomExAsrRack_AttachBusAnalyzerByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), configPtr);
		}

		/// <summary>レベル測定機能の削除</summary>
		/// <param name="busName">バス名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスからレベル測定機能を削除します。
		/// </para>
		/// </remarks>
		public void DetachBusAnalyzerByName(ArgString busName)
		{
			NativeMethods.criAtomExAsrRack_DetachBusAnalyzerByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]));
		}

		/// <summary>レベル測定結果の取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="info">レベル測定結果の構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスからレベル測定機能の結果を取得します。
		/// </para>
		/// </remarks>
		public unsafe void GetBusAnalyzerInfoByName(ArgString busName, out CriAtomExAsr.BusAnalyzerInfo info)
		{
			fixed (CriAtomExAsr.BusAnalyzerInfo* infoPtr = &info)
				NativeMethods.criAtomExAsrRack_GetBusAnalyzerInfoByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), infoPtr);
		}

		/// <summary>代替ASRラックIDの設定</summary>
		/// <param name="altRackId">代替ASRラックID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したIDのASRラックが存在しない場合に、代わりになるASRラックのIDを設定します。
		/// （ rack_id のASRラックが存在しない場合に、その音声を alt_rack_id のASRラック経由で出力します。）
		/// デフォルト設定は <see cref="CriAtomExAsr.RackDefaultId"/>
		/// （指定したIDのASRラックがなければデフォルトASRから出力する）です。
		/// </para>
		/// <para>
		/// 備考:
		/// 存在しないASRラックへの出力をエラーとして扱いたい場合、
		/// alt_rack_id に rack_id と同じ値を設定してください。
		/// </para>
		/// </remarks>
		public void SetAlternateRackId(CriAtomExAsrRack altRackId)
		{
			NativeMethods.criAtomExAsrRack_SetAlternateRackId(NativeHandle, altRackId?.NativeHandle ?? default);
		}

		/// <summary>最大バス数を取得</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したIDのASRラックで利用可能な最大バス数を取得します。
		/// デフォルト設定では <see cref="CriAtomExAsr.DefaultNumBuses"/> を返します。
		/// 最大バス数を変更するには、<see cref="CriAtomExAsrRack.Config"/>::num_buses を変更して
		/// ASRラックを作成してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.Config"/>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		/// <seealso cref="CriAtomExAsrRack.SetDefaultConfig"/>
		public Int32 GetNumBuses()
		{
			return NativeMethods.criAtomExAsrRack_GetNumBuses(NativeHandle);
		}

		/// <summary>指定したバスの振幅解析器の解析結果取得</summary>
		/// <param name="busNo">バス番号</param>
		/// <param name="rms">振幅結果出力バッファー</param>
		/// <param name="numChannels">振幅結果出力バッファーのチャンネル数</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 振幅解析器の現在の解析結果（RMS値）を取得します。
		/// 指定したバスに振幅解析器がない場合や、指定したチャンネル数がASRバスよりも多い場合、取得に失敗します。
		/// </para>
		/// </remarks>
		public unsafe bool GetAmplitudeAnalyzerRms(Int32 busNo, out Single rms, UInt32 numChannels)
		{
			fixed (Single* rmsPtr = &rms)
				return NativeMethods.criAtomExAsrRack_GetAmplitudeAnalyzerRms(NativeHandle, busNo, rmsPtr, numChannels);
		}

		/// <summary>指定したバスの振幅解析器の解析結果取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="rms">振幅結果出力バッファー</param>
		/// <param name="numChannels">振幅結果出力バッファーのチャンネル数</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 振幅解析器の現在の解析結果（RMS値）を取得します。
		/// 指定したバスに振幅解析器がない場合や、指定したチャンネル数がASRバスよりも多い場合、取得に失敗します。
		/// </para>
		/// </remarks>
		public unsafe bool GetAmplitudeAnalyzerRmsByName(ArgString busName, out Single rms, UInt32 numChannels)
		{
			fixed (Single* rmsPtr = &rms)
				return NativeMethods.criAtomExAsrRack_GetAmplitudeAnalyzerRmsByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), rmsPtr, numChannels);
		}

		/// <summary>指定したバスのコンプレッサーの振幅乗算値取得</summary>
		/// <param name="busNo">バス番号</param>
		/// <param name="gain">振幅乗算値出力バッファー</param>
		/// <param name="numChannels">振幅乗算値出力バッファーのチャンネル数</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンプレッサーが入力波形に乗算する値を取得します。
		/// 指定したバスにコンプレッサーがない場合や、指定したチャンネル数がASRバスよりも多い場合、取得に失敗します。
		/// </para>
		/// </remarks>
		public unsafe bool GetCompressorGain(Int32 busNo, out Single gain, UInt32 numChannels)
		{
			fixed (Single* gainPtr = &gain)
				return NativeMethods.criAtomExAsrRack_GetCompressorGain(NativeHandle, busNo, gainPtr, numChannels);
		}

		/// <summary>指定したバスのコンプレッサーの振幅乗算値取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="gain">振幅乗算値出力バッファー</param>
		/// <param name="numChannels">振幅乗算値出力バッファーのチャンネル数</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンプレッサーが入力波形に乗算する値を取得します。
		/// 指定したバスにコンプレッサーがない場合や、指定したチャンネル数がASRバスよりも多い場合、取得に失敗します。
		/// </para>
		/// </remarks>
		public unsafe bool GetCompressorGainByName(ArgString busName, out Single gain, UInt32 numChannels)
		{
			fixed (Single* gainPtr = &gain)
				return NativeMethods.criAtomExAsrRack_GetCompressorGainByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), gainPtr, numChannels);
		}

		/// <summary>指定したバスのコンプレッサーの振幅値取得</summary>
		/// <param name="busNo">バス番号</param>
		/// <param name="rms">振幅乗算値出力バッファー</param>
		/// <param name="numChannels">振幅乗算値出力バッファーのチャンネル数</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンプレッサーに適用されている振幅値を取得します。
		/// 指定したバスにコンプレッサーがない場合や、指定したチャンネル数がASRバスよりも多い場合、取得に失敗します。
		/// </para>
		/// </remarks>
		public unsafe bool GetCompressorRms(Int32 busNo, out Single rms, UInt32 numChannels)
		{
			fixed (Single* rmsPtr = &rms)
				return NativeMethods.criAtomExAsrRack_GetCompressorRms(NativeHandle, busNo, rmsPtr, numChannels);
		}

		/// <summary>指定したバスのコンプレッサーの振幅値取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="rms">振幅値出力バッファー</param>
		/// <param name="numChannels">振幅値出力バッファーのチャンネル数</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンプレッサーに適用されている振幅値を取得します。
		/// 指定したバスにコンプレッサーがない場合や、指定したチャンネル数がASRバスよりも多い場合、取得に失敗します。
		/// </para>
		/// </remarks>
		public unsafe bool GetCompressorRmsByName(ArgString busName, out Single rms, UInt32 numChannels)
		{
			fixed (Single* rmsPtr = &rms)
				return NativeMethods.criAtomExAsrRack_GetCompressorRmsByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), rmsPtr, numChannels);
		}

		/// <summary>指定したASRラックのAISACコントロールに値を適用（コントロールID指定）</summary>
		/// <param name="controlId">AISACコントロールID</param>
		/// <param name="controlValue">AISACコントロール値</param>
		/// <returns>適用に成功したか？（true:適用に成功した, false:適用に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したASRラックにアタッチされているDSPバス設定のAISACコントロールに値をセットします。
		/// 失敗した場合、エラーコールバックが返されます。
		/// </para>
		/// </remarks>
		public bool SetAisacControlById(UInt32 controlId, Single controlValue)
		{
			return NativeMethods.criAtomExAsrRack_SetAisacControlById(NativeHandle, controlId, controlValue);
		}

		/// <summary>指定したASRラックのAISACコントロールに値を適用（コントロール名指定）</summary>
		/// <param name="controlName">AISACコントロール名</param>
		/// <param name="controlValue">AISACコントロール値</param>
		/// <returns>適用に成功したか？（true:適用に成功した, false:適用に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したASRラックにアタッチされているDSPバス設定のAISACコントロールに値をセットします。
		/// 失敗した場合、エラーコールバックが返されます。
		/// </para>
		/// </remarks>
		public bool SetAisacControlByName(ArgString controlName, Single controlValue)
		{
			return NativeMethods.criAtomExAsrRack_SetAisacControlByName(NativeHandle, controlName.GetPointer(stackalloc byte[controlName.BufferSize]), controlValue);
		}

		/// <summary>指定したASRラックのAISACコントロールに値を取得（コントロールID指定）</summary>
		/// <param name="controlId">AISACコントロールID</param>
		/// <param name="controlValue">AISACコントロール値</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したASRラックにアタッチされているDSPバス設定のAISACコントロールに値を取得します。
		/// 失敗した場合、エラーコールバックが返されます。
		/// </para>
		/// </remarks>
		public unsafe bool GetAisacControlById(UInt32 controlId, out Single controlValue)
		{
			fixed (Single* controlValuePtr = &controlValue)
				return NativeMethods.criAtomExAsrRack_GetAisacControlById(NativeHandle, controlId, controlValuePtr);
		}

		/// <summary>指定したASRラックのAISACコントロールに値を取得（コントロール名指定）</summary>
		/// <param name="controlName">AISACコントロール名</param>
		/// <param name="controlValue">AISACコントロール値</param>
		/// <returns>取得に成功したか？（true:取得に成功した, false:取得に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したASRラックにアタッチされているDSPバス設定のAISACコントロールに値を取得します。
		/// 失敗した場合、エラーコールバックが返されます。
		/// </para>
		/// </remarks>
		public unsafe bool GetAisacControlByName(ArgString controlName, out Single controlValue)
		{
			fixed (Single* controlValuePtr = &controlValue)
				return NativeMethods.criAtomExAsrRack_GetAisacControlByName(NativeHandle, controlName.GetPointer(stackalloc byte[controlName.BufferSize]), controlValuePtr);
		}

		/// <summary>指定したASRラックの出力デバイスタイプを取得</summary>
		/// <returns>出力デバイスのタイプ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したASRラックが出力しているデバイスのタイプを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// デバイスタイプが取得できないプラットフォームでは常に <see cref="CriAtom.DeviceType.Unknown"/> が返されます。
		/// また、プラットフォームによっては取得に時間がかかる場合があるため、
		/// <see cref="CriAtom.SetDeviceUpdateCallback"/> で登録したコールバック関数の中で使用することが推奨されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetDeviceUpdateCallback"/>
		public CriAtom.DeviceType GetDeviceType()
		{
			return NativeMethods.criAtomExAsrRack_GetDeviceType(NativeHandle);
		}

		/// <summary>Ambisonics再生用ASRラックIDを取得</summary>
		/// <returns>ASRラックID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Ambisonics再生のみを行うASRラックのIDを取得します。
		/// 作成されていない場合、<see cref="CriAtomExAsr.RackIllegalId"/>が返されます。
		/// </para>
		/// </remarks>
		public static Int32 GetAmbisonicRackId()
		{
			return NativeMethods.criAtomExAsrRack_GetAmbisonicRackId();
		}

		/// <summary>ASRラック指定レベルメーター機能用のワークサイズの計算</summary>
		/// <param name="config">レベルメーター追加用のコンフィグ構造体</param>
		/// <returns>必要なワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックへのレベルメーター追加に必要なワーク領域サイズを計算します。
		/// config にnullを指定するとデフォルト設定で計算されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachLevelMeter"/>
		public unsafe Int32 CalculateWorkSizeForLevelMeter(in CriAtom.LevelMeterConfig config)
		{
			fixed (CriAtom.LevelMeterConfig* configPtr = &config)
				return NativeMethods.criAtomExAsrRack_CalculateWorkSizeForLevelMeter(NativeHandle, configPtr);
		}

		/// <summary>ASRラック指定レベルメーター機能の追加</summary>
		/// <param name="config">レベルメーター追加用のコンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックへレベルメーター機能を追加します。
		/// config にnullを指定するとデフォルト設定でレベルメーターが追加されます。
		/// work にnull、work_size に0を指定すると、登録されたユーザアロケーターによって
		/// ワーク領域が確保されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.GetLevelInfo"/>
		public unsafe void AttachLevelMeter(in CriAtom.LevelMeterConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.LevelMeterConfig* configPtr = &config)
				NativeMethods.criAtomExAsrRack_AttachLevelMeter(NativeHandle, configPtr, work, workSize);
		}

		/// <summary>ASRラック指定レベルメーター機能の解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックに追加されたレベルメーター機能を解除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachLevelMeter"/>
		public void DetachLevelMeter()
		{
			NativeMethods.criAtomExAsrRack_DetachLevelMeter(NativeHandle);
		}

		/// <summary>ASRラック指定レベル情報の取得</summary>
		/// <param name="info">レベル情報の構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックのレベルメーターの結果を取得します。
		/// 指定するラックには <see cref="CriAtomExAsrRack.AttachLevelMeter"/> 関数であらかじめ
		/// レベルメーター機能を追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachLevelMeter"/>
		public unsafe void GetLevelInfo(out CriAtom.LevelInfo info)
		{
			fixed (CriAtom.LevelInfo* infoPtr = &info)
				NativeMethods.criAtomExAsrRack_GetLevelInfo(NativeHandle, infoPtr);
		}

		/// <summary>ASRラック指定ラウドネスメーター機能用のワークサイズの計算</summary>
		/// <param name="config">ラウドネスメーター追加用のコンフィグ構造体</param>
		/// <returns>必要なワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックへのITU-R BS.1770-3規格のラウドネスメーター追加に必要なワーク領域サイズを計算します。
		/// config にnullを指定するとデフォルト設定で計算されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachLoudnessMeter"/>
		public unsafe Int32 CalculateWorkSizeForLoudnessMeter(in CriAtom.LoudnessMeterConfig config)
		{
			fixed (CriAtom.LoudnessMeterConfig* configPtr = &config)
				return NativeMethods.criAtomExAsrRack_CalculateWorkSizeForLoudnessMeter(NativeHandle, configPtr);
		}

		/// <summary>ASRラック指定ラウドネスメーター機能の追加</summary>
		/// <param name="config">ラウドネスメーター追加用のコンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックへITU-R BS.1770-3規格のラウドネスメーター機能を追加します。
		/// config にnullを指定するとデフォルト設定でラウドネスメーターが追加されます。
		/// work にnull、work_size に0を指定すると、登録されたユーザアロケーターによって
		/// ワーク領域が確保されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.GetLoudnessInfo"/>
		public unsafe void AttachLoudnessMeter(in CriAtom.LoudnessMeterConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.LoudnessMeterConfig* configPtr = &config)
				NativeMethods.criAtomExAsrRack_AttachLoudnessMeter(NativeHandle, configPtr, work, workSize);
		}

		/// <summary>ASRラック指定ラウドネスメーター機能の解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックのラウドネスメーター機能を解除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.DetachLoudnessMeter"/>
		public void DetachLoudnessMeter()
		{
			NativeMethods.criAtomExAsrRack_DetachLoudnessMeter(NativeHandle);
		}

		/// <summary>ASRラック指定ラウドネスメーター情報の取得</summary>
		/// <param name="info">ラウドネス情報の構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックのラウドネスメーターの結果を取得します。
		/// 指定するラックには <see cref="CriAtomExAsrRack.AttachLoudnessMeter"/> 関数であらかじめ
		/// ラウドネスメーター機能を追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachLoudnessMeter"/>
		public unsafe void GetLoudnessInfo(out CriAtom.LoudnessInfo info)
		{
			fixed (CriAtom.LoudnessInfo* infoPtr = &info)
				NativeMethods.criAtomExAsrRack_GetLoudnessInfo(NativeHandle, infoPtr);
		}

		/// <summary>ASRラック指定ラウドネスメーターのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックのラウドネスメーターの蓄積データをリセットします。
		/// 本関数を呼び出す前にライブラリへラウドネスメーターを追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachLoudnessMeter"/>
		public void ResetLoudnessMeter()
		{
			NativeMethods.criAtomExAsrRack_ResetLoudnessMeter(NativeHandle);
		}

		/// <summary>ASRラック指定トゥルーピークメーター機能用のワークサイズの計算</summary>
		/// <param name="config">トゥルーピークメーター追加用のコンフィグ構造体</param>
		/// <returns>必要なワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックへのITU-R BS.1770-3規格のトゥルーピークメーター追加に必要なワーク領域サイズを計算します。
		/// config にnullを指定するとデフォルト設定で計算されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachTruePeakMeter"/>
		public unsafe Int32 CalculateWorkSizeForTruePeakMeter(in CriAtom.TruePeakMeterConfig config)
		{
			fixed (CriAtom.TruePeakMeterConfig* configPtr = &config)
				return NativeMethods.criAtomExAsrRack_CalculateWorkSizeForTruePeakMeter(NativeHandle, configPtr);
		}

		/// <summary>ASRラック指定トゥルーピークメーター機能の追加</summary>
		/// <param name="config">トゥルーピークメーター追加用のコンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックへITU-R BS.1770-3規格のトゥルーピークメーター機能を追加します。
		/// config にnullを指定するとデフォルト設定でトゥルーピークメーターが追加されます。
		/// work にnull、work_size に0を指定すると、登録されたユーザアロケーターによって
		/// ワーク領域が確保されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.GetTruePeakInfo"/>
		public unsafe void AttachTruePeakMeter(in CriAtom.TruePeakMeterConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.TruePeakMeterConfig* configPtr = &config)
				NativeMethods.criAtomExAsrRack_AttachTruePeakMeter(NativeHandle, configPtr, work, workSize);
		}

		/// <summary>ASRラック指定トゥルーピークメーター機能の解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックに追加したトゥルーピークメーター機能を解除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachTruePeakMeter"/>
		public void DetachTruePeakMeter()
		{
			NativeMethods.criAtomExAsrRack_DetachTruePeakMeter(NativeHandle);
		}

		/// <summary>ASRラック指定トゥルーピーク情報の取得</summary>
		/// <param name="info">トゥルーピーク情報の構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// rack_idで指定したASRラックのトゥルーピークメーターの測定結果を取得します。
		/// 本関数を呼び出す前にライブラリへトゥルーピークメーターを追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.AttachTruePeakMeter"/>
		public unsafe void GetTruePeakInfo(out CriAtom.TruePeakInfo info)
		{
			fixed (CriAtom.TruePeakInfo* infoPtr = &info)
				NativeMethods.criAtomExAsrRack_GetTruePeakInfo(NativeHandle, infoPtr);
		}

		/// <summary>ネイティブハンドル</summary>

		public Int32 NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExAsrRack(Int32 handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExAsrRack other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExAsrRack a, CriAtomExAsrRack b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExAsrRack a, CriAtomExAsrRack b) =>
			!(a == b);

	}
}