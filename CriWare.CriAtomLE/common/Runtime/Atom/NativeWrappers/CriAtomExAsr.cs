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
#pragma warning disable 0465
	/// <summary>CriAtomExAsr API</summary>
	public static partial class CriAtomExAsr
	{
		/// <summary>レベル測定機能コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAsr.AttachBusAnalyzerByName"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExAsr.BusAnalyzerConfig"/> ）に、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.AttachBusAnalyzerByName"/>
		/// <seealso cref="CriAtomExAsr.BusAnalyzerConfig"/>
		public static unsafe void SetDefaultConfigForBusAnalyzer(out CriAtomExAsr.BusAnalyzerConfig pConfig)
		{
			fixed (CriAtomExAsr.BusAnalyzerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExAsr_SetDefaultConfigForBusAnalyzer_(pConfigPtr);
		}

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
		public static unsafe void SetBusFilterCallbackByName(ArgString busName, delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void> preFunc, delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void> postFunc, IntPtr obj)
		{
			NativeMethods.criAtomExAsr_SetBusFilterCallbackByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), (IntPtr)preFunc, (IntPtr)postFunc, obj);
		}

		/// <summary>波形フィルターコールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// バスに登録することができる PCM データを受け取るコールバック関数です。
		/// コールバック関数の登録には ::criAtomExAsr_SetBusFilterCallback 関数を使用します。
		/// コールバック関数を登録すると、サウンドレンダラが音声処理を行う度に、
		/// コールバック関数が実行されるようになります。
		/// フィルターコールバック関数には、 PCM データのフォーマットやチャンネル数、
		/// 参照可能なサンプル数、 PCM データを格納した領域のアドレスが返されます。
		/// コールバック内では PCM データの値を直接参照可能になるので、
		/// 再生中の音声の振幅をチェックするといった用途に利用可能です。
		/// また、コールバック関数内で PCM データを加工すると、再生音に反映されるため、
		/// PCM データに対してユーザ独自のエフェクトをかけることも可能です。
		/// （ただし、タイムストレッチ処理のようなデータ量が増減する加工を行うことはできません。）
		/// </para>
		/// <para>
		/// 備考:
		/// PCM データはチャンネル単位で分離されています。
		/// （インターリーブされていません。）
		/// サンプル数は32の倍数で、下限は32、上限は256となります。
		/// また、サンプル数はプラットフォームデバイスの出力の進捗に応じて変化します。
		/// 第 6 引数（ data 配列）には、各チャンネルの PCM データ配列の先頭アドレスが格納されています。
		/// （二次元配列の先頭アドレスではなく、チャンネルごとの PCM データ配列の先頭アドレスを格納した
		/// 一次元のポインタ配列です。）
		/// 格納されてくる PCM データはバスに設定されているエフェクトの処理後の音声です。
		/// プラットフォームによって、 PCM データのフォーマットは異なります。
		/// 実行環境のデータフォーマットについては、第 3 引数（ format ）で判別可能です。
		/// PCM データのフォーマットが 16 bit 整数型の場合、 format は <see cref="CriAtom.PcmFormat.Sint16"/> となり、
		/// PCM データのフォーマットが 32 bit 浮動小数点数型の場合、 format は <see cref="CriAtom.PcmFormat.Float32"/> となります。
		/// それぞれのケースで PCM データの値域は異なりますのでご注意ください。
		/// - <see cref="CriAtom.PcmFormat.Sint16"/> 時は -32768 ～ +32767
		/// - <see cref="CriAtom.PcmFormat.Float32"/> 時は -1.0f ～ +1.0f
		/// （多重音声のミキシングや前段のエフェクトによっては上記範囲を超えた値が出る可能性があります。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生する可能性があります。
		/// </para>
		/// </remarks>
		public unsafe class BusFilterCbFunc : NativeCallbackBase<BusFilterCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg : Interfaces.IPcmData
			{
				/// <summary>PCMの形式</summary>
				public CriAtom.PcmFormat format { get; }
				/// <summary>チャンネル数</summary>
				public Int32 numChannels { get; }
				/// <summary>サンプル数</summary>
				public Int32 numSamples { get; }
				/// <summary>PCMデータのチャンネル配列</summary>
				public NativeReference<IntPtr> data { get; }

				internal Arg(CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, NativeReference<IntPtr> data)
				{
					this.format = format;
					this.numChannels = numChannels;
					this.numSamples = numSamples;
					this.data = data;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, IntPtr* data) =>
				InvokeCallbackInternal(obj, new(format, numChannels, numSamples, data));
#if !NET5_0_OR_GREATER
			internal delegate void NativeDelegate(IntPtr obj, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, IntPtr* data);
			static NativeDelegate callbackDelegate = null;
#endif
			internal BusFilterCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>SoundxRプラグインインターフェースの登録</summary>
		/// <param name="soundxrInterface">SoundxRプラグインのインターフェース</param>
		/// <returns>false:登録に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// SoundxRプラグインのインターフェースを登録します。
		/// ツール上でバイノーラライザーに Sound xR を設定した ACF を登録する必要があります。
		/// 異なるバイノーラライザー設定の ACF を登録した場合は以下の挙動になります。
		/// - バイノーラライザー設定が存在しない古い ACF		 : バイノーラル結果が無音になる。
		/// - バイノーラライザー設定が CRI Binauralizer の ACF : バイノーラル結果が無音になる。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、ライブラリの初期化前に実行してください。
		/// </para>
		/// </remarks>
		public static bool RegisterSoundxRInterface(IntPtr soundxrInterface)
		{
			return NativeMethods.criAtomExAsr_RegisterSoundxRInterface(soundxrInterface);
		}

		/// <summary>ASRの初期化コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAsr.Initialize"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExAsr.Config"/> ）に、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.Initialize"/>
		/// <seealso cref="CriAtomExAsr.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomExAsr.Config pConfig)
		{
			fixed (CriAtomExAsr.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExAsr_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>ASR初期化用ワーク領域サイズの計算</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASR（Atom Sound Renderer）の初期化に必要なワーク領域のサイズを取得します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExAsr.Initialize"/> 関数でASRの初期化を行う場合、
		/// 本関数で計算したサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ASRの初期化に必要なワークメモリのサイズは、ASR初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomExAsr.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExAsr.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExAsr.Initialize"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExAsr.Config config)
		{
			fixed (CriAtomExAsr.Config* configPtr = &config)
				return NativeMethods.criAtomExAsr_CalculateWorkSize(configPtr);
		}

		/// <summary>ASR初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomExAsr.SetDefaultConfig"/> メソッドで
		/// 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomExAsr.Initialize"/> 関数
		/// に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExAsr.SetDefaultConfig"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.Initialize"/>
		/// <seealso cref="CriAtomExAsr.SetDefaultConfig"/>
		[System.Serializable]
		[System.Xml.Serialization.XmlType(Namespace = "CriAtomExAsr")]
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
			/// マスターバスの領域を1つ分含めるため、必ず1以上の値を設定して下さい。
			/// </para>
			/// </remarks>
			public Int32 numBuses;

			/// <summary>出力チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRの出力チャンネル数を指定します。
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
			/// 出力および処理過程のサンプリングレートを指定します。
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
			/// ASRの出力先サウンドレンダラの種別を指定します。
			/// sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、
			/// 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			/// </para>
			/// <para>
			/// 注意:
			/// <see cref="CriAtom.SoundRendererType.Asr"/>および<see cref="CriAtom.SoundRendererDefault"/>は指定しないでください。
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

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
			[System.Xml.Serialization.XmlIgnore]
			public IntPtr context;

			/// <summary>ASRラックの最大数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 作成可能なASRラックの最大個数です。
			/// </para>
			/// </remarks>
			public Int32 maxRacks;

			/// <summary>Ambisonicsのオーダータイプ（廃止）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// このメンバー変数は削除予定であり、使用しないでください。
			/// Ambisonicsを再生するためには <see cref="CriAtom.SoundRendererType.Ambisonics"/> を使用します。
			/// 詳しくはマニュアルを参照してください。
			/// </para>
			/// </remarks>
			public CriAtom.AmbisonicsOrderType ambisonicsOrderType;

		}
		/// <summary>ワーク領域サイズ計算用コンフィグ構造体の設定</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ワーク領域サイズの計算用に、ASR初期化用コンフィグ構造体
		/// （ <see cref="CriAtomExAsr.Config"/> 構造体）を仮登録します。
		/// エフェクトのアタッチに必要なワーク領域のサイズは、
		/// ASR初期化時（ <see cref="CriAtomExAsr.Initialize"/> 関数実行時）
		/// に設定する構造体のパラメーターによって変化します。
		/// そのため、通常はエフェクトのアタッチに必要なワーク領域サイズを計算する前に、
		/// ASRを初期化する必要があります。
		/// 本関数を使用してASR初期化用コンフィグ構造体を登録した場合、
		/// エフェクトのアタッチに必要なワーク領域のサイズを、
		/// 初期化処理なしに計算可能になります。
		/// （ <see cref="CriAtomEx.CalculateWorkSizeForDspBusSettingFromAcfData"/>
		/// 関数が実行可能となります。）
		/// </para>
		/// <para>
		/// 備考:
		/// 引数（ config ）に null を指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExAsr.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 現状、本関数で一旦コンフィグ構造体を設定すると、
		/// 設定前の状態（未初期化状態でのワーク領域サイズ計算をエラーとする動作）
		/// に戻すことができなくなります。
		/// （関数を再度実行してパラメーターを上書きすることは可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で登録した初期化用コンフィグ構造体は、
		/// ASR未初期化状態でのワーク領域サイズ計算にしか使用されません。
		/// ASR初期化後には本関数に設定したパラメーターではなく、
		/// 初期化時に指定されたパラメーターがワーク領域サイズの計算に使用されます。
		/// （本関数で登録する構造体のパラメーターと、
		/// ASRの初期化に使用する構造体のパラメーターが異なる場合、
		/// ワーク領域サイズが不足し、オブジェクトの作成に失敗する恐れがあります。）
		/// 本関数を実行した場合でも、 <see cref="CriAtomEx.CalculateWorkSizeForDspBusSetting"/>
		/// 関数は使用できません。
		/// DSPバス設定アタッチ用ワーク領域サイズの計算には、
		/// <see cref="CriAtomEx.CalculateWorkSizeForDspBusSettingFromAcfData"/>
		/// 関数を使用してください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeForDspBusSettingFromAcfData"/>
		public static unsafe void SetConfigForWorkSizeCalculation(in CriAtomExAsr.Config config)
		{
			fixed (CriAtomExAsr.Config* configPtr = &config)
				NativeMethods.criAtomExAsr_SetConfigForWorkSizeCalculation(configPtr);
		}

		/// <summary>ASRの初期化</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASR（Atom Sound Renderer）の初期化を行います。
		/// 本関数を実行することでASRが起動しASRラックが1個追加され、レンダリング結果の出力を開始します。
		/// </para>
		/// <para>
		/// 備考:
		/// ASRの初期化に必要なワークメモリのサイズは、ASR初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomExAsr.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExAsr.SetDefaultConfig"/> 適用時と同じパラメーター）で初期化処理を行います。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// 本関数を実行後、必ず対になる <see cref="CriAtomExAsr.Finalize"/> 関数を実行してください。
		/// また、 <see cref="CriAtomExAsr.Finalize"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExAsr.Finalize"/>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		public static unsafe void Initialize(in CriAtomExAsr.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExAsr.Config* configPtr = &config)
				NativeMethods.criAtomExAsr_Initialize(configPtr, work, workSize);
		}

		/// <summary>ASRの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASR（Atom Sound Renderer）の終了処理を行います。
		/// 本関数を実行することで、レンダリング結果の出力が停止されます。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// ASR初期化時に確保されたメモリ領域が解放されます。
		/// （ASR初期化時にワーク領域を渡した場合、本関数実行後であれば
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
		/// <seealso cref="CriAtomExAsr.Initialize"/>
		public static void Finalize()
		{
			NativeMethods.criAtomExAsr_Finalize();
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
		public static void SetBusVolumeByName(ArgString busName, Single volume)
		{
			NativeMethods.criAtomExAsr_SetBusVolumeByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), volume);
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
		public static unsafe void GetBusVolumeByName(ArgString busName, in Single volume)
		{
			fixed (Single* volumePtr = &volume)
				NativeMethods.criAtomExAsr_GetBusVolumeByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), volumePtr);
		}

		/// <summary>バスのパン情報の設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="panInfo">パン情報</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのパン情報を設定します。
		/// センドタイプがポストパンのセンド先に有効です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数はデフォルトの ASR ラックの DSP バス設定を参照します。
		/// 任意の ASR ラックの DSP バス設定を参照する場合、 <see cref="CriAtomExAsrRack.SetBusPanInfoByName"/> 関数を使用してください。
		/// パン情報のデフォルト値はCRI Atom Craftで設定した値です。
		/// </para>
		/// </remarks>
		public static unsafe void SetBusPanInfoByName(ArgString busName, in CriAtomExAsr.BusPanInfo panInfo)
		{
			fixed (CriAtomExAsr.BusPanInfo* panInfoPtr = &panInfo)
				NativeMethods.criAtomExAsr_SetBusPanInfoByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), panInfoPtr);
		}

		/// <summary>パン情報構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスにおけるパン情報をまとめた構造体です。
		/// バスのパン情報の設定や取得に使用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.SetBusPanInfoByName"/>
		/// <seealso cref="CriAtomExAsr.GetBusPanInfoByName"/>
		/// <seealso cref="CriAtomExAsrRack.SetBusPanInfoByName"/>
		/// <seealso cref="CriAtomExAsrRack.GetBusPanInfoByName"/>
		public unsafe partial struct BusPanInfo
		{
			/// <summary>音量</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パンにおける音量です。
			/// 0.0f ～ 1.0fの範囲で設定・取得されます。
			/// また、通常の音量と掛け合わされます。
			/// </para>
			/// </remarks>
			public Single volume;

			/// <summary>角度</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パンにおける角度です。
			/// 0.0f を正面とした -180.0f ～ 180.0fの範囲で設定・取得されます。
			/// </para>
			/// </remarks>
			public Single angle;

			/// <summary>インテリア距離</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パンにおけるインテリア距離です。
			/// 0.0f ～ 1.0fの範囲で設定・取得されます。
			/// </para>
			/// </remarks>
			public Single distance;

			/// <summary>マルチチャンネル音声の広がり</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// マルチチャンネル音源の角度に対する倍率です。
			/// 0.0f ～ 1.0fの範囲で設定・取得されます。
			/// </para>
			/// </remarks>
			public Single wideness;

			/// <summary>スプレッド</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パンにおけるスプレッドです。
			/// 0.0f ～ 1.0fの範囲で設定・取得されます。
			/// </para>
			/// </remarks>
			public Single spread;

		}
		/// <summary>バスのパン情報の取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="panInfo">パン情報</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのパン情報を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数はデフォルトの ASR ラックの DSP バス設定を参照します。
		/// 任意の ASR ラックの DSP バス設定を参照する場合、 <see cref="CriAtomExAsrRack.GetBusPanInfoByName"/> 関数を使用してください。
		/// </para>
		/// </remarks>
		public static unsafe void GetBusPanInfoByName(ArgString busName, out CriAtomExAsr.BusPanInfo panInfo)
		{
			fixed (CriAtomExAsr.BusPanInfo* panInfoPtr = &panInfo)
				NativeMethods.criAtomExAsr_GetBusPanInfoByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), panInfoPtr);
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
		public static unsafe void SetBusMatrixByName(ArgString busName, Int32 inputChannels, Int32 outputChannels, Single[] matrix)
		{
			fixed (Single* matrixPtr = matrix)
				NativeMethods.criAtomExAsr_SetBusMatrixByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), inputChannels, outputChannels, matrixPtr);
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
		public static void SetBusSendLevelByName(ArgString busName, ArgString sendtoBusName, Single level)
		{
			NativeMethods.criAtomExAsr_SetBusSendLevelByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), sendtoBusName.GetPointer(stackalloc byte[sendtoBusName.BufferSize]), level);
		}

		/// <summary>エフェクト動作時パラメーターの設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <param name="parameterIndex">エフェクト動作時パラメーターインデックス</param>
		/// <param name="parameterValue">エフェクト動作時パラメーター設定値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デフォルトのASRラックIDを使用してエフェクトの動作時パラメーターを設定します。
		/// 動作時パラメーターを設定する際は、本関数呼び出し前にあらかじめ
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数でバスが構築されている必要があります。
		/// どのバスにどのエフェクトが存在するかは、アタッチしたDSPバス設定に依存します。
		/// 指定したバスに指定したIDのエフェクトが存在しない場合、関数は失敗します。
		/// セットしたパラメーターはcriAtomExAsr_UpdateParameter関数を呼ぶまで実際にエフェクトに反映されません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomExAsr.UpdateEffectParameters"/>
		public static void SetEffectParameter(ArgString busName, ArgString effectName, UInt32 parameterIndex, Single parameterValue)
		{
			NativeMethods.criAtomExAsr_SetEffectParameter(busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]), parameterIndex, parameterValue);
		}

		/// <summary>エフェクト動作時パラメーターの反映</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デフォルトのASRラックIDを使用してエフェクトの動作時パラメーターを反映します。
		/// 動作時パラメーターを実際に反映するには、<see cref="CriAtomExAsr.SetEffectParameter"/> の他にも本関数を呼び出して下さい。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomExAsr.SetEffectParameter"/>
		public static void UpdateEffectParameters(ArgString busName, ArgString effectName)
		{
			NativeMethods.criAtomExAsr_UpdateEffectParameters(busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]));
		}

		/// <summary>エフェクト動作時パラメーターの取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="effectName">エフェクト名</param>
		/// <param name="parameterIndex">エフェクト動作時パラメーターインデックス</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デフォルトのASRラックIDを使用してエフェクトの動作時パラメーターを取得します。
		/// 動作時パラメーターを取得する際は、本関数呼び出し前にあらかじめ
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数でバスが構築されている必要があります。
		/// どのバスにどのエフェクトが存在するかは、アタッチしたDSPバス設定に依存します。指定したバスに指定した名前のエフェクトが存在しない場合、関数は失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		public static Single GetEffectParameter(ArgString busName, ArgString effectName, UInt32 parameterIndex)
		{
			return NativeMethods.criAtomExAsr_GetEffectParameter(busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]), parameterIndex);
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
		/// どのバスにどのエフェクトが存在するかは、アタッチしたDSPバス設定に依存します。指定したバスに指定したIDのエフェクトが存在しない場合、関数は失敗します。
		/// </para>
		/// <para>
		/// 注意:
		/// 音声再生中にバイパス設定を行うとノイズが発生することがあります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		public static void SetEffectBypass(ArgString busName, ArgString effectName, NativeBool bypass)
		{
			NativeMethods.criAtomExAsr_SetEffectBypass(busName.GetPointer(stackalloc byte[busName.BufferSize]), effectName.GetPointer(stackalloc byte[effectName.BufferSize]), bypass);
		}

		/// <summary>レベル測定機能の追加</summary>
		/// <param name="busName">バス名</param>
		/// <param name="config">レベル測定機能のコンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスにレベル測定機能を追加し、レベル測定処理を開始します。
		/// 本関数を実行後、 ::criAtomExAsr_GetBusAnalyzerInfo 関数を実行することで、
		/// RMSレベル（音圧）、ピークレベル（最大振幅）、ピークホールドレベルを
		/// 取得することが可能です。
		/// 複数バスのレベルを計測するには、バスごとに本関数を呼び出す必要があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は <see cref="CriAtomEx.AttachDspBusSetting"/> 関数と同一のリソースを操作します。
		/// そのため、現状は <see cref="CriAtomEx.AttachDspBusSetting"/> 関数を実行すると、
		/// ::criAtomExAsr_GetBusAnalyzerInfo 関数による情報取得ができなくなります。
		/// 本関数と <see cref="CriAtomEx.AttachDspBusSetting"/> 関数を併用する際には、
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数を実行する前に一旦
		/// ::criAtomExAsr_DetachBusAnalyzer 関数でレベル測定機能を無効化し、
		/// <see cref="CriAtomEx.AttachDspBusSetting"/> 関数実行後に再度本関数を実行してください。
		/// </para>
		/// </remarks>
		public static unsafe void AttachBusAnalyzerByName(ArgString busName, in CriAtomExAsr.BusAnalyzerConfig config)
		{
			fixed (CriAtomExAsr.BusAnalyzerConfig* configPtr = &config)
				NativeMethods.criAtomExAsr_AttachBusAnalyzerByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), configPtr);
		}

		/// <summary>レベル測定機能アタッチ用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomExAsr.SetDefaultConfigForBusAnalyzer"/> メソッドで
		/// 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomExAsr.AttachBusAnalyzerByName"/> 関数
		/// に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExAsr.SetDefaultConfigForBusAnalyzer"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.AttachBusAnalyzerByName"/>
		public unsafe partial struct BusAnalyzerConfig
		{
			/// <summary>測定間隔（ミリ秒単位）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定結果を更新する間隔です。
			/// </para>
			/// </remarks>
			public Int32 interval;

			/// <summary>ピークホールド時間（ミリ秒単位）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ピーク値がより大きい値で更新されたとき、下がらないようにホールドする時間です。
			/// </para>
			/// </remarks>
			public Int32 peakHoldTime;

		}
		/// <summary>レベル測定機能の削除</summary>
		/// <param name="busName">バス名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスからレベル測定機能を削除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.AttachBusAnalyzerByName"/>
		public static void DetachBusAnalyzerByName(ArgString busName)
		{
			NativeMethods.criAtomExAsr_DetachBusAnalyzerByName(busName.GetPointer(stackalloc byte[busName.BufferSize]));
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
		/// <seealso cref="CriAtomExAsr.AttachBusAnalyzerByName"/>
		public static unsafe void GetBusAnalyzerInfoByName(ArgString busName, out CriAtomExAsr.BusAnalyzerInfo info)
		{
			fixed (CriAtomExAsr.BusAnalyzerInfo* infoPtr = &info)
				NativeMethods.criAtomExAsr_GetBusAnalyzerInfoByName(busName.GetPointer(stackalloc byte[busName.BufferSize]), infoPtr);
		}

		/// <summary>レベル測定情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスのレベル測定情報を取得するための構造体です。
		/// ::criAtomExAsr_GetBusAnalyzerInfo 関数で利用します。
		/// </para>
		/// <para>
		/// 備考:
		/// 各レベル値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 以下のコードでデシベル表記に変換することができます。
		/// dB = 10.0f * log10f(level);
		/// </para>
		/// </remarks>
		public unsafe partial struct BusAnalyzerInfo
		{
			/// <summary>有効チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定結果が有効なチャンネル数です。
			/// </para>
			/// </remarks>
			public Int32 numChannels;

			/// <summary>RMSレベル</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定間隔間の音声振幅のRMS（二乗平均平方根）を計算した値です。
			/// 音圧レベルとして扱われます。
			/// </para>
			/// </remarks>
			public InlineArray16<Single> rmsLevels;

			/// <summary>ピークレベル</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定間隔間の音声振幅の最大値です。
			/// </para>
			/// </remarks>
			public InlineArray16<Single> peakLevels;

			/// <summary>ピークホールドレベル</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ホールドしているピークレベル値です。
			/// </para>
			/// </remarks>
			public InlineArray16<Single> peakHoldLevels;

		}
		/// <summary>最大バス数を取得</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 利用可能な最大バス数を取得します。
		/// デフォルト設定では <see cref="CriAtomExAsr.DefaultNumBuses"/> を返します。
		/// 最大バス数を変更するには、<see cref="CriAtomExAsr.Config"/>::num_buses を変更して
		/// ASRラックを作成してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.Config"/>
		/// <seealso cref="CriAtomExAsr.SetDefaultConfig"/>
		public static Int32 GetNumBuses()
		{
			return NativeMethods.criAtomExAsr_GetNumBuses();
		}

		/// <summary>ユーザ定義エフェクトインターフェースの登録</summary>
		/// <param name="afxInterface">ユーザ定義エフェクトのバージョン情報付きインターフェース</param>
		/// <returns>false:登録に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ユーザ定義エフェクトインターフェースをASRに登録します。
		/// ユーザ定義エフェクトインターフェースを登録したエフェクトはDSPバス設定をアタッチする際に使用できるようになります。
		/// 以下の条件に該当する場合は、ユーザ定義エフェクトインターフェースの登録に失敗し、エラーコールバックが返ります:
		/// - 同一のエフェクト名を持つユーザ定義エフェクトインターフェースが既に登録されている
		/// - Atomが使用しているユーザ定義エフェクトインターフェースと異なる
		/// - ユーザ定義エフェクトインターフェースの登録数上限（ <see cref="CriAtomExAsr.MaxNumUserEffectInterfaces"/> ）に達した
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はCRI ADX Audio Effect Plugin SDKで作成したユーザ定義エフェクトを登録する場合にのみ使用して下さい。
		/// ユーザ定義エフェクトインターフェースは、ユーザ定義エフェクトを含むDSPバス設定をアタッチする前に
		/// 本関数によって登録を行って下さい。
		/// ACFにデフォルトDSPバス設定が存在する場合、ACFの登録（ <see cref="CriAtomEx.RegisterAcfFile"/>, <see cref="CriAtomEx.RegisterAcfData"/> 関数）によってもDSPバス設定がアタッチされるため、
		/// ユーザ定義エフェクトがデフォルトDSPバス設定に含まれている場合はACFを登録する前にユーザ定義エフェクトインターフェースを登録して下さい。
		/// 一度登録を行ったインターフェースのポインタは、DSPバス設定をアタッチしている間参照され続けます。
		/// Atomライブラリ使用中にインターフェースの登録解除を行う場合は、 <see cref="CriAtomExAsr.UnregisterEffectInterface"/> を使用して下さい。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.UnregisterEffectInterface"/>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomEx.DetachDspBusSetting"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFile"/>
		/// <seealso cref="CriAtomEx.RegisterAcfData"/>
		public static bool RegisterEffectInterface(IntPtr afxInterface)
		{
			return NativeMethods.criAtomExAsr_RegisterEffectInterface(afxInterface);
		}

		/// <summary>ユーザ定義エフェクトインターフェースの登録解除</summary>
		/// <param name="afxInterface">ユーザ定義エフェクトのバージョン情報付きインターフェース</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エフェクトインターフェースの登録を解除します。
		/// 登録を解除したエフェクトはDSPバス設定をアタッチする際に使用できなくなります。
		/// 登録処理を行っていないエフェクトインターフェースの登録を解除することはできません（エラーコールバックが返ります）。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はCRI ADX Audio Effect Plugin SDKで作成したユーザ定義エフェクトを登録解除する場合にのみ使用して下さい。
		/// 登録を行ったユーザ定義エフェクトインターフェースはDSPバス設定がアタッチされている間参照され続けるため、
		/// 本関数は必ず <see cref="CriAtomEx.DetachDspBusSetting"/> の呼び出しの後に行って下さい。
		/// Atomライブラリの終了時（<see cref="CriAtomEx.Finalize"/> 関数の呼び出し時）には全てのユーザ定義エフェクトインターフェースの登録が解除されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.RegisterEffectInterface"/>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomEx.DetachDspBusSetting"/>
		public static void UnregisterEffectInterface(IntPtr afxInterface)
		{
			NativeMethods.criAtomExAsr_UnregisterEffectInterface(afxInterface);
		}

		/// <summary>IRリバーブエフェクトの負荷計測リセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASRエフェクトのIRリバーブの負荷計測をリセットします。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバスにIRリバーブエフェクトがセットされていなくても本関数を呼び出すことは可能ですが、何も処理されません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.GetIrReverbPerformanceInfo"/>
		public static void ResetIrReverbPerformanceInfo()
		{
			NativeMethods.criAtomExAsr_ResetIrReverbPerformanceInfo();
		}

		/// <summary>IRリバーブエフェクトの負荷計測</summary>
		/// <param name="info">IRリバーブの負荷計測情報構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス上のIRリバーブエフェクトの負荷をまとめて計測します。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバスにIRリバーブエフェクトがセットされていなくても本関数を呼び出すことは可能ですが、何も処理されません。
		/// </para>
		/// <para>
		/// 注意:
		/// プラットフォームによって計測される内容が異なる場合があります。
		/// 詳しくは各プラットフォームのCRI ADX マニュアルの IR リバーブを参照してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.ResetIrReverbPerformanceInfo"/>
		public static unsafe void GetIrReverbPerformanceInfo(out CriAtomExAsr.IrReverbPerformanceInfo info)
		{
			fixed (CriAtomExAsr.IrReverbPerformanceInfo* infoPtr = &info)
				NativeMethods.criAtomExAsr_GetIrReverbPerformanceInfo(infoPtr);
		}

		/// <summary>IRリバーブエフェクトの負荷計測構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// IRリバーブの負荷計測で得られる情報をまとめた構造体です。
		/// IRリバーブは一定サンプルを1ブロックとして非同期で処理し、ブロック単位で負荷を計測します。
		/// </para>
		/// <para>
		/// 注意:
		/// プラットフォームによって計測される内容が異なる場合があります。
		/// 詳しくは各プラットフォームのCRI ADX マニュアルの IR リバーブを参照してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.GetIrReverbPerformanceInfo"/>
		public unsafe partial struct IrReverbPerformanceInfo
		{
			/// <summary>ブロックサイズ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Craftで設定可能なIRリバーブの1ブロックのサンプル数です。
			/// </para>
			/// <para>
			/// 備考:
			/// 現在IRリバーブのブロックサイズは512,1024のみをサポートします。
			/// バスにIRリバーブエフェクトが存在しない場合、0を返します。
			/// </para>
			/// </remarks>
			public UInt32 blocksize;

			/// <summary>サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブ内部サンプリングレートです。
			/// </para>
			/// <para>
			/// 備考:
			/// 現在現在IRリバーブのサンプリングレートは48000Hzのみをサポートします。
			/// バスにIRリバーブエフェクトが存在しない場合、0を返します。
			/// </para>
			/// </remarks>
			public UInt32 samplingRate;

			/// <summary>処理回数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブの1ブロックの処理回数です。
			/// </para>
			/// </remarks>
			public UInt32 processCount;

			/// <summary>最新処理時間（マイクロ秒）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブの最新の1ブロックの処理にかかった時間です。
			/// </para>
			/// </remarks>
			public UInt32 lastProcessTime;

			/// <summary>最大処理時間（マイクロ秒）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブの計測リセットから取得時点までで最も長い1ブロックの処理時間です。
			/// </para>
			/// </remarks>
			public UInt32 maxProcessTime;

			/// <summary>平均処理時間（マイクロ秒）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブの計測リセットから取得時点までの1ブロックの平均処理時間です。
			/// </para>
			/// </remarks>
			public UInt32 averageProcessTime;

			/// <summary>最新処理インターバル（マイクロ秒）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブの最新の1ブロックの処理間隔です。
			/// </para>
			/// </remarks>
			public UInt32 lastProcessInterval;

			/// <summary>最大処理インターバル（マイクロ秒）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブの計測リセットから取得時点までで最も長い1ブロックの処理インターバルです。
			/// </para>
			/// </remarks>
			public UInt32 maxProcessInterval;

			/// <summary>平均処理インターバル（マイクロ秒）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// IRリバーブの計測リセットから取得時点までの1ブロックの平均処理インターバルです。
			/// </para>
			/// </remarks>
			public UInt32 averageProcessInterval;

		}
		/// <summary>PCMバッファーサイズの取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリ内で設定されているPCMデータの保存に使用するバッファーのサイズを取得します。
		/// （サイズはサンプル数単位で取得します。）
		/// </para>
		/// <para>
		/// 備考:
		/// ::criAtomExAsr_SetPcmBufferSize 関数にて設定を行っていない場合、 0 が返却されます。
		/// </para>
		/// </remarks>
		public static Int32 GetPcmBufferSize()
		{
			return NativeMethods.criAtomExAsr_GetPcmBufferSize();
		}

		/// <summary>バイノーラライザーの有効化</summary>
		/// <param name="enabled">有効フラグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バイノーラライザーの有効を指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// バイノーラライザーは sound_renderer_type が <see cref="CriAtom.SoundRendererType.Spatial"/> であるASRラックでのみ使用可能です。
		/// バイノーラライザーのスペシャライザーインタフェースが登録されている場合、登録したスペシャライザーを使用します。
		/// 登録されていない場合、Atom内蔵のスペシャライザーを使用します。
		/// </para>
		/// </remarks>
		public static void EnableBinauralizer(NativeBool enabled)
		{
			NativeMethods.criAtomExAsr_EnableBinauralizer(enabled);
		}

		/// <summary>バイノーラライザーの有効化状態の取得</summary>
		/// <returns>false:無効）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バイノーラライザーの有効化状態を取得します。
		/// </para>
		/// </remarks>
		public static bool IsEnabledBinauralizer()
		{
			return NativeMethods.criAtomExAsr_IsEnabledBinauralizer();
		}

		/// <summary>デフフォルトのバス数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デフォルトのバス数です。
		/// </para>
		/// </remarks>
		public const Int32 DefaultNumBuses = (8);
		/// <summary>デフォルトASRラックID</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 初期化時に自動的に作成されるASRラックIDです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		/// <seealso cref="CriAtomExAsrRack.Dispose"/>
		public const Int32 RackDefaultId = (0);
		/// <summary>不正なラックID</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAsrRack.CriAtomExAsrRack"/> 関数に失敗した際に返る値です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		/// <seealso cref="CriAtomExAsrRack.Dispose"/>
		public const Int32 RackIllegalId = (-1);
		/// <summary>ユーザ定義エフェクトインターフェースの最大登録数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録可能なユーザ定義エフェクトインターフェースの最大数です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.RegisterEffectInterface"/>
		/// <seealso cref="CriAtomExAsr.UnregisterEffectInterface"/>
		public const Int32 MaxNumUserEffectInterfaces = (256);

	}
}