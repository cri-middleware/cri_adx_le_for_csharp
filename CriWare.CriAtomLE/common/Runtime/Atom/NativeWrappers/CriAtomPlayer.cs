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
	/// <summary>Atomプレーヤーオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomPlayer"/> は、音声再生用に作られたプレーヤーを操作するためのオブジェクトです。
	/// <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数等で音声再生用のプレーヤーを作成すると、
	/// 関数はプレーヤー操作用に、この"Atomプレーヤーオブジェクト"を返します。
	/// Atomプレーヤーとは、コーデックに依存しない再生制御のためのインターフェースを提供する、
	/// 抽象化されたプレーヤーオブジェクトです。
	/// Atomプレーヤーの作成方法は再生する音声コーデックにより異なりますが、
	/// 作成されたプレーヤーの制御については、Atomプレーヤー用のAPIが共通で利用可能です。
	/// データのセットや再生の開始、ステータスの取得等、プレーヤーに対して行う操作は、
	/// 全てAtomプレーヤーオブジェクトを介して実行されます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
	public partial class CriAtomPlayer : IDisposable
	{
		/// <summary><see cref="CriAtomInstrument.PlayerConfig"/>へデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">インストゥルメントプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateRawPcmPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.RawPcmPlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/>
		public static unsafe void SetDefaultConfigForInstrumentPlayer(out CriAtomInstrument.PlayerConfig pConfig)
		{
			fixed (CriAtomInstrument.PlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForInstrumentPlayer_(pConfigPtr);
		}

		/// <summary><see cref="CriAtomPlayer.ConfigASR"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">プレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーを作成する際に、動作仕様を指定するためのコンフィグ構造体
		/// （ <see cref="CriAtomPlayer.ConfigASR"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.ConfigASR"/>
		public static unsafe void SetDefaultConfigASR(in CriAtomPlayer.ConfigASR pConfig)
		{
			fixed (CriAtomPlayer.ConfigASR* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfig_ASR_(pConfigPtr);
		}

		/// <summary>波形フィルターコールバック関数の登録</summary>
		/// <param name="func">波形フィルターコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デコード結果の PCM データを受け取るコールバック関数を登録します。
		/// 登録されたコールバック関数は、 Atom プレーヤーが音声データをデコードしたタイミングで呼び出されます。
		/// </para>
		/// <para>
		/// 注意:
		/// ステータス変更コールバック関数内で長時間処理をブロックすると、音切れ等の問題
		/// が発生しますので、ご注意ください。
		/// HCA-MXコーデックやプラットフォーム固有の音声圧縮コーデックを使用している場合、
		/// フィルターコールバックは利用できません。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.FilterCbFunc"/>
		public unsafe void SetFilterCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomPlayer_SetFilterCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetFilterCallbackInternal(IntPtr func, IntPtr obj) => SetFilterCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)func, obj);
		CriAtomPlayer.FilterCbFunc _filterCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetFilterCallback" />
		public CriAtomPlayer.FilterCbFunc FilterCallback => _filterCallback ?? (_filterCallback = new CriAtomPlayer.FilterCbFunc(SetFilterCallbackInternal));

		/// <summary>波形フィルターコールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// デコード結果の PCM データを受け取るコールバック関数です。
		/// コールバック関数の登録には <see cref="CriAtomPlayer.SetFilterCallback"/> 関数を使用します。
		/// コールバック関数を登録すると、 Atom プレーヤーが音声データをデコードする度に、
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
		/// 第 6 引数（ data 配列）には、各チャンネルの PCM データ配列の先頭アドレスが格納されています。
		/// （二次元配列の先頭アドレスではなく、チャンネルごとの PCM データ配列の先頭アドレスを格納した
		/// 一次元のポインタ配列です。）
		/// プラットフォームによって、 PCM データのフォーマットは異なります。
		/// 実行環境のデータフォーマットについては、第 3 引数（ format ）で判別可能です。
		/// PCM データのフォーマットが 16 bit 整数型の場合、 format は <see cref="CriAtom.PcmFormat.Sint16"/> となり、
		/// PCM データのフォーマットが 32 bit 浮動小数点数型の場合、 format は <see cref="CriAtom.PcmFormat.Float32"/> となります。
		/// それぞれのケースで PCM データの値域は異なりますのでご注意ください。
		/// - <see cref="CriAtom.PcmFormat.Sint16"/> 時は -32768 ～ +32767
		/// - <see cref="CriAtom.PcmFormat.Float32"/> 時は -1.0f ～ +1.0f
		/// *
		/// （デコード時点ではクリッピングが行われていないため、 <see cref="CriAtom.PcmFormat.Float32"/>
		/// 時は上記範囲をわずかに超えた値が出る可能性があります。）
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetFilterCallback"/>
		public unsafe class FilterCbFunc : NativeCallbackBase<FilterCbFunc.Arg>
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
			delegate void NativeDelegate(IntPtr obj, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, IntPtr* data);
			static NativeDelegate callbackDelegate = null;
#endif
			internal FilterCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary><see cref="CriAtom.AdxPlayerConfig"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">ADXプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.AdxPlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		public static unsafe void SetDefaultConfigForAdxPlayer(out CriAtom.AdxPlayerConfig pConfig)
		{
			fixed (CriAtom.AdxPlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForAdxPlayer_(pConfigPtr);
		}

		/// <summary><see cref="CriAtom.AiffPlayerConfig"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">AIFFプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateAiffPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.AiffPlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AiffPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAiffPlayer"/>
		public static unsafe void SetDefaultConfigForAiffPlayer(out CriAtom.AiffPlayerConfig pConfig)
		{
			fixed (CriAtom.AiffPlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForAiffPlayer_(pConfigPtr);
		}

		/// <summary><see cref="CriAtomHcaMx.PlayerConfig"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">HCA-MXプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateHcaMxPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomHcaMx.PlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomHcaMx.PlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateHcaMxPlayer"/>
		public static unsafe void SetDefaultConfigForHcaMxPlayer(out CriAtomHcaMx.PlayerConfig pConfig)
		{
			fixed (CriAtomHcaMx.PlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForHcaMxPlayer_(pConfigPtr);
		}

		/// <summary><see cref="CriAtom.HcaPlayerConfig"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">HCAプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateHcaPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.HcaPlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.HcaPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateHcaPlayer"/>
		public static unsafe void SetDefaultConfigForHcaPlayer(out CriAtom.HcaPlayerConfig pConfig)
		{
			fixed (CriAtom.HcaPlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForHcaPlayer_(pConfigPtr);
		}

		/// <summary><see cref="CriAtom.RawPcmPlayerConfig"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">RawPCMプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateRawPcmPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.RawPcmPlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.RawPcmPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateRawPcmPlayer"/>
		public static unsafe void SetDefaultConfigForRawPcmPlayer(out CriAtom.RawPcmPlayerConfig pConfig)
		{
			fixed (CriAtom.RawPcmPlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForRawPcmPlayer_(pConfigPtr);
		}

		/// <summary><see cref="CriAtom.StandardPlayerConfig"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">標準プレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.StandardPlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.StandardPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateStandardPlayer"/>
		public static unsafe void SetDefaultConfigForStandardPlayer(out CriAtom.StandardPlayerConfig pConfig)
		{
			fixed (CriAtom.StandardPlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForStandardPlayer_(pConfigPtr);
		}

		/// <summary><see cref="CriAtom.WavePlayerConfig"/>へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">WAVEプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.CreateWavePlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.WavePlayerConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.WavePlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateWavePlayer"/>
		public static unsafe void SetDefaultConfigForWavePlayer(out CriAtom.WavePlayerConfig pConfig)
		{
			fixed (CriAtom.WavePlayerConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomPlayer_SetDefaultConfigForWavePlayer_(pConfigPtr);
		}

		/// <summary>標準プレーヤー作成用ワーク領域サイズの計算</summary>
		/// <param name="config">標準プレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 標準プレーヤー（ADXとHCAを再生可能なプレーヤー）を作成するために必要な、
		/// ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtom.StandardPlayerConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForStandardPlayer"/> 適用時と同じパラメーター）
		/// でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ ::criAtom_Initialize 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.StandardPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateStandardPlayer"/>
		public static unsafe Int32 CalculateWorkSizeForStandardPlayer(in CriAtom.StandardPlayerConfig config)
		{
			fixed (CriAtom.StandardPlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForStandardPlayer(configPtr);
		}

		/// <summary>標準プレーヤーの作成</summary>
		/// <param name="config">標準プレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADXとHCAが再生可能なプレーヤーを作成します。
		/// 本関数で作成されたAtomプレーヤーには、ADXデータとHCAデータのデコード機能が付加されています。
		/// 作成されたプレーヤーで再生できる音声のフォーマットは、第一引数（config）に指定した
		/// パラメーターによって決まります。
		/// 例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤーでは
		/// 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		/// configにnullを指定した場合、デフォルト設定（ <see cref="CriAtomPlayer.SetDefaultConfigForStandardPlayer"/>
		/// 適用時と同じパラメーター）でプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// User Allocator方式を用いる場合、ユーザがワーク領域を用意する必要はありません。
		/// workにnull、work_sizeに0を指定するだけで、必要なメモリを登録済みのメモリ確保関数から確保します。
		/// 標準プレーヤー作成時に確保されたメモリは、標準プレーヤー破棄時（ <see cref="CriAtomPlayer.Dispose"/>
		/// 関数実行時）に解放されます。
		/// Fixed Memory方式を用いる場合、ワーク領域として別途確保済みのメモリ領域を本関数に
		/// 設定する必要があります。
		/// ワーク領域のサイズは <see cref="CriAtomPlayer.CalculateWorkSizeForStandardPlayer"/> 関数で取得可能です。
		/// 標準プレーヤー作成前に <see cref="CriAtomPlayer.CalculateWorkSizeForStandardPlayer"/> 関数で取得した
		/// サイズ分のメモリを予め確保しておき、本関数に設定してください。
		/// 尚、Fixed Memory方式を用いた場合、ワーク領域は標準プレーヤーの破棄
		/// （ <see cref="CriAtomPlayer.Dispose"/> 関数）を行うまでの間、ライブラリ内で利用され続けます。
		/// 標準プレーヤーの破棄を行う前に、ワーク領域のメモリを解放しないでください。
		/// </para>
		/// <para>
		/// 例:
		/// 【User Allocator方式による標準プレーヤーの作成】
		/// User Allocator方式を用いる場合、標準プレーヤーの作成／破棄の手順は以下のようになります。
		/// -# 標準プレーヤー作成前に、 <see cref="CriAtom.SetUserMallocFunction"/> 関数と
		/// <see cref="CriAtom.SetUserFreeFunction"/> 関数を用いてメモリ確保／解放関数を登録する。
		/// -# 標準プレーヤー作成用コンフィグ構造体にパラメーターをセットする。
		/// -# <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数で標準プレーヤーを作成する。
		/// （workにはnull、work_sizeには0を指定する。）
		/// -# オブジェクトが不要になったら <see cref="CriAtomPlayer.Dispose"/> 関数で標準プレーヤーを破棄する。
		/// *
		/// </para>
		/// <para>
		/// ※ライブラリ初期化時にメモリ確保／解放関数を登録済みの場合、標準プレーヤー作成時
		/// に再度関数を登録する必要はありません。
		/// 【Fixed Memory方式による標準プレーヤーの作成】
		/// Fixed Memory方式を用いる場合、標準プレーヤーの作成／破棄の手順は以下のようになります。
		/// -# 標準プレーヤー作成用コンフィグ構造体にパラメーターをセットする。
		/// -# 標準プレーヤーの作成に必要なワーク領域のサイズを、
		/// <see cref="CriAtomPlayer.CalculateWorkSizeForStandardPlayer"/> 関数を使って計算する。
		/// -# ワーク領域サイズ分のメモリを確保する。
		/// -# <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数で標準プレーヤーを作成する。
		/// （workには確保したメモリのアドレスを、work_sizeにはワーク領域のサイズを指定する。）
		/// -# オブジェクトが不要になったら <see cref="CriAtomPlayer.Dispose"/> 関数で標準プレーヤーを破棄する。
		/// -# ワーク領域のメモリを解放する。
		/// *
		/// </para>
		/// <para>
		/// <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数を実行すると、Atomプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		/// データのセット、再生の開始、ステータスの取得等、Atomプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// 作成されたAtomプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// ストリーム再生用のAtomプレーヤーは、内部的にローダー（ CriFsLoaderHn ）を確保します。
		/// ストリーム再生用のAtomプレーヤーを作成する場合、プレーヤーオブジェクト数分のローダーが確保
		/// できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// 本関数は完了復帰型の関数です。
		/// 標準プレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// 標準プレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.StandardPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForStandardPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateStandardPlayer"/>
		public static unsafe CriAtomPlayer CreateStandardPlayer(in CriAtom.StandardPlayerConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtom.StandardPlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateStandardPlayer(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>Atomプレーヤーの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーを破棄します。
		/// 本関数を実行した時点で、Atomプレーヤー作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定したAtomプレーヤーオブジェクトも無効になります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 音声再生中のAtomプレーヤーを破棄しようとした場合、本関数内で再生停止を
		/// 待ってからリソースの解放が行われます。
		/// （ファイルから再生している場合は、さらに読み込み完了待ちが行われます。）
		/// そのため、本関数内で処理が長時間（数フレーム）ブロックされる可能性があります。
		/// Atomプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomPlayer_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomPlayer() => Dispose();
#pragma warning restore 1591

		/// <summary>ADXプレーヤー作成用ワーク領域サイズの計算</summary>
		/// <param name="config">ADXプレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADX再生用プレーヤーを作成するために必要な、ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtom.AdxPlayerConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForAdxPlayer"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ ::criAtom_Initialize 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		public static unsafe Int32 CalculateWorkSizeForAdxPlayer(in CriAtom.AdxPlayerConfig config)
		{
			fixed (CriAtom.AdxPlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForAdxPlayer(configPtr);
		}

		/// <summary>ADXプレーヤーの作成</summary>
		/// <param name="config">ADXプレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADXが再生可能なプレーヤーを作成します。
		/// 本関数で作成されたAtomプレーヤーには、ADXデータのデコード機能が付加されています。
		/// 作成されたプレーヤーで再生できる音声のフォーマットは、第一引数（config）に指定した
		/// パラメーターによって決まります。
		/// 例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤーでは
		/// 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		/// configにnullを指定した場合、デフォルト設定（ <see cref="CriAtomPlayer.SetDefaultConfigForAdxPlayer"/>
		/// 適用時と同じパラメーター）でプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// User Allocator方式を用いる場合、ユーザがワーク領域を用意する必要はありません。
		/// workにnull、work_sizeに0を指定するだけで、必要なメモリを登録済みのメモリ確保関数から確保します。
		/// ADXプレーヤー作成時に確保されたメモリは、ADXプレーヤー破棄時（ <see cref="CriAtomPlayer.Dispose"/>
		/// 関数実行時）に解放されます。
		/// Fixed Memory方式を用いる場合、ワーク領域として別途確保済みのメモリ領域を本関数に
		/// 設定する必要があります。
		/// ワーク領域のサイズは <see cref="CriAtomPlayer.CalculateWorkSizeForAdxPlayer"/> 関数で取得可能です。
		/// ADXプレーヤー作成前に <see cref="CriAtomPlayer.CalculateWorkSizeForAdxPlayer"/> 関数で取得した
		/// サイズ分のメモリを予め確保しておき、本関数に設定してください。
		/// 尚、Fixed Memory方式を用いた場合、ワーク領域はADXプレーヤーの破棄
		/// （ <see cref="CriAtomPlayer.Dispose"/> 関数）を行うまでの間、ライブラリ内で利用され続けます。
		/// ADXプレーヤーの破棄を行う前に、ワーク領域のメモリを解放しないでください。
		/// </para>
		/// <para>
		/// 例:
		/// 【User Allocator方式によるADXプレーヤーの作成】
		/// User Allocator方式を用いる場合、ADXプレーヤーの作成／破棄の手順は以下のようになります。
		/// -# ADXプレーヤー作成前に、 <see cref="CriAtom.SetUserMallocFunction"/> 関数と
		/// <see cref="CriAtom.SetUserFreeFunction"/> 関数を用いてメモリ確保／解放関数を登録する。
		/// -# ADXプレーヤー作成用コンフィグ構造体にパラメーターをセットする。
		/// -# <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数でADXプレーヤーを作成する。
		/// （workにはnull、work_sizeには0を指定する。）
		/// -# オブジェクトが不要になったら <see cref="CriAtomPlayer.Dispose"/> 関数でADXプレーヤーを破棄する。
		/// *
		/// </para>
		/// <para>
		/// ※ライブラリ初期化時にメモリ確保／解放関数を登録済みの場合、ADXプレーヤー作成時
		/// に再度関数を登録する必要はありません。
		/// 【Fixed Memory方式によるADXプレーヤーの作成】
		/// Fixed Memory方式を用いる場合、ADXプレーヤーの作成／破棄の手順は以下のようになります。
		/// -# ADXプレーヤー作成用コンフィグ構造体にパラメーターをセットする。
		/// -# ADXプレーヤーの作成に必要なワーク領域のサイズを、
		/// <see cref="CriAtomPlayer.CalculateWorkSizeForAdxPlayer"/> 関数を使って計算する。
		/// -# ワーク領域サイズ分のメモリを確保する。
		/// -# <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数でADXプレーヤーを作成する。
		/// （workには確保したメモリのアドレスを、work_sizeにはワーク領域のサイズを指定する。）
		/// -# オブジェクトが不要になったら <see cref="CriAtomPlayer.Dispose"/> 関数でADXプレーヤーを破棄する。
		/// -# ワーク領域のメモリを解放する。
		/// *
		/// </para>
		/// <para>
		/// <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数を実行すると、Atomプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		/// データやデコーダーのセット、再生の開始、ステータスの取得等、Atomプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// 作成されたAtomプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// ストリーム再生用のAtomプレーヤーは、内部的にローダー（ CriFsLoaderHn ）を確保します。
		/// ストリーム再生用のAtomプレーヤーを作成する場合、プレーヤーオブジェクト数分のローダーが確保
		/// できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ADXプレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ADXプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForAdxPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		public static unsafe CriAtomPlayer CreateAdxPlayer(in CriAtom.AdxPlayerConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtom.AdxPlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateAdxPlayer(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>HCAプレーヤー作成用ワーク領域サイズの計算</summary>
		/// <param name="config">HCAプレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA再生用プレーヤーを作成するために必要な、ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtom.HcaPlayerConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForHcaPlayer"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ ::criAtom_Initialize 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.HcaPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateHcaPlayer"/>
		public static unsafe Int32 CalculateWorkSizeForHcaPlayer(in CriAtom.HcaPlayerConfig config)
		{
			fixed (CriAtom.HcaPlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForHcaPlayer(configPtr);
		}

		/// <summary>HCAプレーヤーの作成</summary>
		/// <param name="config">HCAプレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCAが再生可能なプレーヤーを作成します。
		/// 本関数で作成されたAtomプレーヤーには、HCAデータのデコード機能が付加されています。
		/// 作成されたプレーヤーで再生できる音声のフォーマットは、第一引数（config）に指定した
		/// パラメーターによって決まります。
		/// 例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤーでは
		/// 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		/// configにnullを指定した場合、デフォルト設定（ <see cref="CriAtomPlayer.SetDefaultConfigForHcaPlayer"/>
		/// 適用時と同じパラメーター）でプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// 各方式の詳細については、別途 <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数の説明をご参照ください。
		/// <see cref="CriAtomPlayer.CreateHcaPlayer"/> 関数を実行すると、Atomプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		/// データやデコーダーのセット、再生の開始、ステータスの取得等、Atomプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// 作成されたAtomプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// ストリーム再生用のAtomプレーヤーは、内部的にローダー（ CriFsLoaderHn ）を確保します。
		/// ストリーム再生用のAtomプレーヤーを作成する場合、プレーヤーオブジェクト数分のローダーが確保
		/// できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// 本関数は完了復帰型の関数です。
		/// HCAプレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// HCAプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.HcaPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForHcaPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateHcaPlayer"/>
		public static unsafe CriAtomPlayer CreateHcaPlayer(in CriAtom.HcaPlayerConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtom.HcaPlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateHcaPlayer(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>HCA-MXプレーヤー作成用ワーク領域サイズの計算</summary>
		/// <param name="config">HCA-MXプレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MX再生用プレーヤーを作成するために必要な、ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomHcaMx.PlayerConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForHcaMxPlayer"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはHCA-MX初期化時（ <see cref="CriAtomHcaMx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、HCA-MXを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomHcaMx.PlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateHcaMxPlayer"/>
		public static unsafe Int32 CalculateWorkSizeForHcaMxPlayer(in CriAtomHcaMx.PlayerConfig config)
		{
			fixed (CriAtomHcaMx.PlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForHcaMxPlayer(configPtr);
		}

		/// <summary>HCA-MXプレーヤーの作成</summary>
		/// <param name="config">HCA-MXプレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXが再生可能なプレーヤーを作成します。
		/// 本関数で作成されたAtomプレーヤーには、HCA-MXデータのデコード機能が付加されています。
		/// 作成されたプレーヤーで再生できる音声のフォーマットは、第一引数（config）に指定した
		/// パラメーターによって決まります。
		/// 例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤーでは
		/// 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		/// configにnullを指定した場合、デフォルト設定（ <see cref="CriAtomPlayer.SetDefaultConfigForHcaMxPlayer"/>
		/// 適用時と同じパラメーター）でプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// 各方式の詳細については、別途 <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数の説明をご参照ください。
		/// <see cref="CriAtomPlayer.CreateHcaMxPlayer"/> 関数を実行すると、Atomプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		/// データやデコーダーのセット、再生の開始、ステータスの取得等、Atomプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// 作成されたAtomプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ストリーム再生用のAtomプレーヤーは、内部的にローダー（ CriFsLoaderHn ）を確保します。
		/// ストリーム再生用のAtomプレーヤーを作成する場合、プレーヤーオブジェクト数分のローダーが確保
		/// できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// 本関数を実行する前に、HCA-MXを初期化しておく必要があります。
		/// 本関数は完了復帰型の関数です。
		/// HCA-MXプレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// HCA-MXプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomHcaMx.PlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForHcaMxPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateHcaMxPlayer"/>
		public static unsafe CriAtomPlayer CreateHcaMxPlayer(in CriAtomHcaMx.PlayerConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomHcaMx.PlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateHcaMxPlayer(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>WAVEプレーヤー作成用ワーク領域サイズの計算</summary>
		/// <param name="config">WAVEプレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WAVE再生用プレーヤーを作成するために必要な、ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtom.WavePlayerConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForWavePlayer"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ ::criAtom_Initialize 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.WavePlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateWavePlayer"/>
		public static unsafe Int32 CalculateWorkSizeForWavePlayer(in CriAtom.WavePlayerConfig config)
		{
			fixed (CriAtom.WavePlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForWavePlayer(configPtr);
		}

		/// <summary>WAVEプレーヤーの作成</summary>
		/// <param name="config">WAVEプレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WAVEが再生可能なプレーヤーを作成します。
		/// 本関数で作成されたAtomプレーヤーには、WAVEデータのデコード機能が付加されています。
		/// 作成されたプレーヤーで再生できる音声のフォーマットは、第一引数（config）に指定した
		/// パラメーターによって決まります。
		/// 例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤーでは
		/// 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		/// configにnullを指定した場合、デフォルト設定（ <see cref="CriAtomPlayer.SetDefaultConfigForWavePlayer"/>
		/// 適用時と同じパラメーター）でプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// 各方式の詳細については、別途 <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数の説明をご参照ください。
		/// <see cref="CriAtomPlayer.CreateWavePlayer"/> 関数を実行すると、Atomプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		/// データやデコーダーのセット、再生の開始、ステータスの取得等、Atomプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// 作成されたAtomプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// ストリーム再生用のAtomプレーヤーは、内部的にローダー（ CriFsLoaderHn ）を確保します。
		/// ストリーム再生用のAtomプレーヤーを作成する場合、プレーヤーオブジェクト数分のローダーが確保
		/// できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// 本関数は完了復帰型の関数です。
		/// WAVEプレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// WAVEプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// 現状、Waveファイルのチャンク解析は厳密には行っていません。
		/// チャンクの並び順がFORMチャンク、COMMチャンク、SSNDチャンクではない場合や、
		/// その他のチャンクを含むWaveファイルは、解析に失敗する可能性があります。
		/// また、現時点で対応しているフォーマットは、モノラルまたはステレオの
		/// 16bit 非圧縮データのみです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.WavePlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForWavePlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateWavePlayer"/>
		public static unsafe CriAtomPlayer CreateWavePlayer(in CriAtom.WavePlayerConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtom.WavePlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateWavePlayer(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>AIFFプレーヤー作成用ワーク領域サイズの計算</summary>
		/// <param name="config">AIFFプレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AIFF再生用プレーヤーを作成するために必要な、ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtom.AiffPlayerConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForAiffPlayer"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ ::criAtom_Initialize 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AiffPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAiffPlayer"/>
		public static unsafe Int32 CalculateWorkSizeForAiffPlayer(in CriAtom.AiffPlayerConfig config)
		{
			fixed (CriAtom.AiffPlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForAiffPlayer(configPtr);
		}

		/// <summary>AIFFプレーヤーの作成</summary>
		/// <param name="config">AIFFプレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AIFFが再生可能なプレーヤーを作成します。
		/// 本関数で作成されたAtomプレーヤーには、AIFFデータのデコード機能が付加されています。
		/// 作成されたプレーヤーで再生できる音声のフォーマットは、第一引数（config）に指定した
		/// パラメーターによって決まります。
		/// 例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤーでは
		/// 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		/// configにnullを指定した場合、デフォルト設定（ <see cref="CriAtomPlayer.SetDefaultConfigForAiffPlayer"/>
		/// 適用時と同じパラメーター）でプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// 各方式の詳細については、別途 <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数の説明をご参照ください。
		/// <see cref="CriAtomPlayer.CreateAiffPlayer"/> 関数を実行すると、Atomプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		/// データやデコーダーのセット、再生の開始、ステータスの取得等、Atomプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// 作成されたAtomプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// ストリーム再生用のAtomプレーヤーは、内部的にローダー（ CriFsLoaderHn ）を確保します。
		/// ストリーム再生用のAtomプレーヤーを作成する場合、プレーヤーオブジェクト数分のローダーが確保
		/// できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// 本関数は完了復帰型の関数です。
		/// AIFFプレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// AIFFプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// 現状、AIFFファイルのチャンク解析は厳密には行っていません。
		/// チャンクの並び順がFORMチャンク、COMMチャンク、SSNDチャンクではない場合や、
		/// その他のチャンクを含むAIFFファイルは、解析に失敗する可能性があります。
		/// また、現時点で対応しているフォーマットは、モノラルまたはステレオの
		/// 16bit 非圧縮データのみです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AiffPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForAiffPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateAiffPlayer"/>
		public static unsafe CriAtomPlayer CreateAiffPlayer(in CriAtom.AiffPlayerConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtom.AiffPlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateAiffPlayer(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>RawPCMプレーヤー作成用ワーク領域サイズの計算</summary>
		/// <param name="config">RawPCMプレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// RawPCM再生用プレーヤーを作成するために必要な、ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtom.RawPcmPlayerConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForRawPcmPlayer"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ ::criAtom_Initialize 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.RawPcmPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateRawPcmPlayer"/>
		public static unsafe Int32 CalculateWorkSizeForRawPcmPlayer(in CriAtom.RawPcmPlayerConfig config)
		{
			fixed (CriAtom.RawPcmPlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForRawPcmPlayer(configPtr);
		}

		/// <summary>RawPCMプレーヤーの作成</summary>
		/// <param name="config">RawPCMプレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// RawPCMが再生可能なプレーヤーを作成します。
		/// 本関数で作成されたAtomプレーヤーには、RawPCMデータのデコード機能が付加されています。
		/// 作成されたプレーヤーで再生できる音声のフォーマットは、第一引数（config）に指定した
		/// パラメーターによって決まります。
		/// 例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤーでは
		/// 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		/// configにnullを指定した場合、デフォルト設定（ <see cref="CriAtomPlayer.SetDefaultConfigForRawPcmPlayer"/>
		/// 適用時と同じパラメーター）でプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// 各方式の詳細については、別途 <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数の説明をご参照ください。
		/// <see cref="CriAtomPlayer.CreateRawPcmPlayer"/> 関数を実行すると、Atomプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		/// データやデコーダーのセット、再生の開始、ステータスの取得等、Atomプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// 作成されたAtomプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// ストリーム再生用のAtomプレーヤーは、内部的にローダー（ CriFsLoaderHn ）を確保します。
		/// ストリーム再生用のAtomプレーヤーを作成する場合、プレーヤーオブジェクト数分のローダーが確保
		/// できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する
		/// 必要があります。
		/// 本関数は完了復帰型の関数です。
		/// RawPCMプレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// RawPCMプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.RawPcmPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForRawPcmPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateRawPcmPlayer"/>
		public static unsafe CriAtomPlayer CreateRawPcmPlayer(in CriAtom.RawPcmPlayerConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtom.RawPcmPlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateRawPcmPlayer(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>音声データのセット（オンメモリデータの指定）</summary>
		/// <param name="buffer">バッファーアドレス</param>
		/// <param name="bufferSize">バッファーサイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ上に配置された音声データを、Atomプレーヤーに関連付けます。
		/// 本関数でメモリアドレスとサイズを指定後、 <see cref="CriAtomPlayer.Start"/> 関数で再生を
		/// 開始すると、指定されたデータが再生されます。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたデータの情報は、他のデータがセットされるまでAtomプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考:
		/// データ要求コールバック関数（ <see cref="CriAtomPlayer.DataRequestCbFunc"/> ）内で本関数を実行すると、
		/// 前回セットした音声の終端に連結して次のデータが再生されます。
		/// </para>
		/// <para>
		/// データ要求コールバック関数内で <see cref="CriAtomPlayer.SetFile"/> 関数を実行することで、
		/// オンメモリデータとファイルを連結して再生することも可能です。
		/// （ただし、先に再生するオンメモリデータが短すぎる場合、次に再生するファイルの
		/// 読み込みが間に合わず、音声が途切れる可能性があります。）
		/// </para>
		/// <para>
		/// 注意:
		/// プレーヤーが記憶するのはバッファーのアドレスとサイズのみです。
		/// （バッファー内のデータがコピーされるわけではありません。）
		/// そのため、指定したデータの再生が終了するまでの間、
		/// アプリケーション側でバッファーを保持し続ける必要があります。
		/// 本関数は停止中のプレーヤーに対してのみ実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.Start"/>
		public void SetData(IntPtr buffer, Int32 bufferSize)
		{
			NativeMethods.criAtomPlayer_SetData(NativeHandle, buffer, bufferSize);
		}

		/// <summary>再生の開始</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声データの再生処理を開始します。
		/// 本関数を実行する前に、事前に <see cref="CriAtomPlayer.SetData"/> 関数等を使用し、再生する
		/// 音声データをAtomプレーヤーにセットしておく必要があります。
		/// </para>
		/// <para>
		/// 本関数実行後、再生の進み具合（発音が開始されたか、再生が完了したか等）がどうなって
		/// いるかは、ステータスを取得することで確認が可能です。
		/// ステータスの取得には、 <see cref="CriAtomPlayer.GetStatus"/> 関数を使用します。
		/// <see cref="CriAtomPlayer.GetStatus"/> 関数は以下の5通りのステータスを返します。
		/// -# <see cref="CriAtomPlayer.Status.Stop"/>
		/// -# <see cref="CriAtomPlayer.Status.Prep"/>
		/// -# <see cref="CriAtomPlayer.Status.Playing"/>
		/// -# <see cref="CriAtomPlayer.Status.Playend"/>
		/// -# <see cref="CriAtomPlayer.Status.Error"/>
		/// *
		/// Atomプレーヤーを作成した時点では、Atomプレーヤーのステータスは停止状態
		/// （ <see cref="CriAtomPlayer.Status.Stop"/> ）です。
		/// 再生する音声データをセット後、本関数を実行することで、Atomプレーヤーのステータスが
		/// 準備状態（ <see cref="CriAtomPlayer.Status.Prep"/> ）に変更されます。
		/// （<see cref="CriAtomPlayer.Status.Prep"/> は、データ供給やデコードの開始を待っている状態です。）
		/// 再生の開始に充分なデータが供給された時点で、Atomプレーヤーはステータスを
		/// 再生状態（ <see cref="CriAtomPlayer.Status.Playing"/> ）に変更し、音声の出力を開始します。
		/// セットされたデータを全て再生し終えると、Atomプレーヤーはステータスを再生終了状態
		/// （ <see cref="CriAtomPlayer.Status.Playend"/> ）に変更します。
		/// 尚、再生中にエラーが発生した場合には、Atomプレーヤーはステータスをエラー状態
		/// （ <see cref="CriAtomPlayer.Status.Error"/> ）に変更します。
		/// Atomプレーヤーのステータスをチェックし、ステータスに応じて処理を切り替えることで、
		/// 音声の再生状態に連動したプログラムを作成することが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 再生開始後、実際に音声が出力されるまでには、タイムラグがあります。
		/// オンメモリのデータ再生時（ <see cref="CriAtomPlayer.SetData"/> 関数でデータをセットした場合）は、
		/// 本関数実行後、最初にサーバー処理が実行されたタイミングでステータスが
		/// <see cref="CriAtomPlayer.Status.Playing"/> に遷移します。
		/// しかし、ストリーミング再生時は、再生を維持するために必要なデータが充分バッファリング
		/// されるまでの間、<see cref="CriAtomPlayer.Status.Prep"/> を維持し続けます。
		/// （必要充分なデータが供給された時点で、 <see cref="CriAtomPlayer.Status.Playing"/> に遷移します。）
		/// 尚、ステータスが <see cref="CriAtomPlayer.Status.Playing"/> に遷移するタイミングは、
		/// あくまで"サウンドライブラリに対して再生指示を発行する"タイミングになります。
		/// そのため、実際にスピーカーから音が出るタイミングは、各プラットフォームのサウンド
		/// ライブラリの処理時間に依存します。
		/// ストリーミング再生時に発音が開始されるタイミングは、同時にストリーミング再生を行う
		/// 音声の数や、デバイスの読み込み速度によって変化します。
		/// ストリーミング再生時に意図としたタイミングで発音を開始させたい場合には、
		/// <see cref="CriAtomPlayer.Pause"/> 関数で事前にポーズをかけておき、Atomプレーヤーのステータスが
		/// <see cref="CriAtomPlayer.Status.Playing"/> に遷移した時点で、ポーズを解除してください。
		/// （ポーズをかけた状態でステータスが <see cref="CriAtomPlayer.Status.Playing"/> に遷移した場合、
		/// ポーズ解除後最初のサーバー処理が実行されたタイミングで、発音が開始されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 既に再生を開始したAtomプレーヤーに対して本関数を実行することはできません。
		/// （ADXライブラリとは異なり、再生中のAtomプレーヤーに対して再度再生の開始を指示した場合、
		/// エラーになります。）
		/// Atomプレーヤーに対して再生を指示する場合には、必ず事前にステータスをチェックし、
		/// ステータスが準備中（ <see cref="CriAtomPlayer.Status.Prep"/> ）や再生中（ <see cref="CriAtomPlayer.Status.Playing"/> ）
		/// になっていないことをご確認ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.GetStatus"/>
		/// <seealso cref="CriAtomPlayer.Pause"/>
		/// <seealso cref="CriAtom.ExecuteMain"/>
		public void Start()
		{
			NativeMethods.criAtomPlayer_Start(NativeHandle);
		}

		/// <summary>データ要求コールバック関数の登録</summary>
		/// <param name="func">データ要求コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ要求コールバック関数の登録を行います。
		/// データ要求コールバックは、複数の音声データをシームレスに連結して再生する際に
		/// 使用します。
		/// 登録したコールバック関数は、Atomプレーヤーが連結再生用のデータを要求するタイミングで
		/// 実行されます。
		/// （前回のデータを読み込み終えて、次に再生すべきデータを要求するタイミングで
		/// コールバック関数が実行されます。）
		/// 登録したコールバック関数内で <see cref="CriAtomPlayer.SetData"/> 関数等を用いてAtomプレーヤーに
		/// データをセットすると、セットされたデータは現在再生中のデータに続いてシームレスに
		/// 連結されて再生されます。
		/// また、コールバック関数内で <see cref="CriAtomPlayer.SetPreviousDataAgain"/> 関数を実行することで、
		/// 同一データを繰り返し再生し続けることも可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 登録したコールバック関数内でデータを指定しなかった場合、現在のデータを再生し
		/// 終えた時点で、Atomプレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playend"/> に遷移します。
		/// タイミング等の問題により、データを指定することができないが、ステータスを
		/// <see cref="CriAtomPlayer.Status.Playend"/> に遷移させたくない場合には、コールバック関数内で
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行してください。
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行することで、約1V後に再度データ要求
		/// コールバック関数が呼び出されます。（コールバック処理をリトライ可能。）
		/// ただし、 <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行した場合、再生が途切れる
		/// （連結箇所に一定時間無音が入る）可能性があります。
		/// </para>
		/// <para>
		/// 注意:
		/// データ要求コールバック関数内で長時間処理をブロックすると、音切れ等の問題が
		/// 発生しますので、ご注意ください。
		/// シームレス連結再生をサポートしないコーデックを使用している場合、
		/// データ要求コールバック関数内で次のデータをセットしても、
		/// データは続けて再生されません。
		/// （HCA-MXやプラットフォーム固有の音声圧縮コーデックを使用している場合、
		/// シームレス連結再生はできません。）
		/// シームレス連結再生に使用する波形データのフォーマットは、
		/// 全て同じにする必要があります。
		/// 具体的には、以下のパラメーターが同じである必要があります。
		/// - コーデック
		/// - チャンネル数
		/// - サンプリングレート
		/// *
		/// パラメーターが異なる波形を連結しようとした場合、
		/// 意図しない速度で音声データが再生されたり、
		/// エラーコールバックが発生する等の問題が発生します。
		/// コールバック関数内でループ付きの波形データをセットした場合でも、
		/// ループ再生は行われません。
		/// （ループポイントが無視され、再生が終了します。）
		/// コールバック関数内でAtomプレーヤーを破棄しないでください。
		/// コールバックを抜けた後も、しばらくの間はサーバー処理内で当該オブジェクトのリソース
		/// が参照されるため、アクセス違反等の重大な問題が発生する可能性があります。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.DataRequestCbFunc"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetPreviousDataAgain"/>
		/// <seealso cref="CriAtomPlayer.DeferCallback"/>
		public unsafe void SetDataRequestCallback(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtomPlayer_SetDataRequestCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetDataRequestCallbackInternal(IntPtr func, IntPtr obj) => SetDataRequestCallback((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)func, obj);
		CriAtomPlayer.DataRequestCbFunc _dataRequestCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetDataRequestCallback" />
		public CriAtomPlayer.DataRequestCbFunc DataRequestCallback => _dataRequestCallback ?? (_dataRequestCallback = new CriAtomPlayer.DataRequestCbFunc(SetDataRequestCallbackInternal));

		/// <summary>音声データのセット（ファイルの指定）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="path">ファイルパス</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声ファイルをAtomプレーヤーに関連付けます。
		/// 本関数でファイルを指定後、 <see cref="CriAtomPlayer.Start"/> 関数で再生を開始すると、
		/// 指定されたファイルがストリーミング再生されます。
		/// 尚、本関数を実行した時点では、ファイルの読み込みは開始されません。
		/// ファイルの読み込みが開始されるのは、 <see cref="CriAtomPlayer.Start"/> 関数実行後です。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたファイルの情報は、他のデータがセットされるまでAtomプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考:
		/// データ要求コールバック関数（ <see cref="CriAtomPlayer.DataRequestCbFunc"/> ）内で本関数を実行すると、
		/// 前回セットした音声の終端に連結して次のデータが再生されます。
		/// </para>
		/// <para>
		/// 尚、第二引数（binder）にバインダーを指定することで、
		/// CPKファイル内のコンテンツを再生することも可能です。
		/// </para>
		/// <para>
		/// データ要求コールバック関数内で <see cref="CriAtomPlayer.SetData"/> 関数を実行することで、
		/// ファイルとオンメモリデータを連結して再生することも可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// ファイルからの再生を行う場合には、ストリーミング再生に対応した
		/// Atomプレーヤーを使用する必要があります。
		/// （ <see cref="CriAtom.AdxPlayerConfig"/> のstreaming_flagにtrueを設定して
		/// Atomプレーヤーを作成する必要があります。）
		/// 本関数は停止中のプレーヤーに対してのみ実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		public void SetFile(CriFsBinder binder, ArgString path)
		{
			NativeMethods.criAtomPlayer_SetFile(NativeHandle, binder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]));
		}

		/// <summary>音声データのセット（CPKコンテンツIDの指定）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="id">コンテンツID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンテンツをAtomプレーヤーに関連付けます。
		/// CRI File Systemライブラリを使用してCPKファイル内のコンテンツファイルを
		/// ID指定で再生するために使用します。
		/// 本関数にバインダーとコンテンツIDを指定後、 <see cref="CriAtomPlayer.Start"/> 関数で再生を
		/// 開始すると、指定されたコンテンツファイルがストリーミング再生されます。
		/// 尚、本関数を実行した時点では、ファイルの読み込みは開始されません。
		/// ファイルの読み込みが開始されるのは、 <see cref="CriAtomPlayer.Start"/> 関数実行後です。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたファイルの情報は、他のデータがセットされるまでAtomプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考:
		/// データ要求コールバック関数（ <see cref="CriAtomPlayer.DataRequestCbFunc"/> ）内で本関数を実行すると、
		/// 前回セットした音声の終端に連結して次のデータが再生されます。
		/// </para>
		/// <para>
		/// データ要求コールバック関数内で <see cref="CriAtomPlayer.SetData"/> 関数を実行することで、
		/// ファイルとオンメモリデータを連結して再生することも可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// ファイルからの再生を行う場合には、ストリーミング再生に対応した
		/// Atomプレーヤーを使用する必要があります。
		/// （ <see cref="CriAtom.AdxPlayerConfig"/> のstreaming_flagにtrueを設定して
		/// Atomプレーヤーを作成する必要があります。）
		/// 本関数は停止中のプレーヤーに対してのみ実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		public void SetContentId(CriFsBinder binder, Int32 id)
		{
			NativeMethods.criAtomPlayer_SetContentId(NativeHandle, binder?.NativeHandle ?? default, id);
		}

		/// <summary>音声データのセット（音声データIDの指定）</summary>
		/// <param name="awb">AWBオブジェクト</param>
		/// <param name="id">波形データID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生する波形データをAtomプレーヤーに関連付けます。
		/// 本関数にAWBオブジェクトと波形データIDを指定後、 <see cref="CriAtomPlayer.Start"/> 関数で再生を
		/// 開始すると、指定した波形データがストリーミング再生されます。
		/// 尚、本関数を実行した時点では、ファイルの読み込みは開始されません。
		/// ファイルの読み込みが開始されるのは、 <see cref="CriAtomPlayer.Start"/> 関数実行後です。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたファイルの情報は、他のデータがセットされるまでAtomプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考:
		/// データ要求コールバック関数（ <see cref="CriAtomPlayer.DataRequestCbFunc"/> ）内で本関数を実行すると、
		/// 前回セットした音声の終端に連結して次のデータが再生されます。
		/// </para>
		/// <para>
		/// データ要求コールバック関数内で <see cref="CriAtomPlayer.SetData"/> 関数を実行することで、
		/// ファイルとオンメモリデータを連結して再生することも可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// ファイルからの再生を行う場合には、ストリーミング再生に対応した
		/// Atomプレーヤーを使用する必要があります。
		/// （ <see cref="CriAtom.AdxPlayerConfig"/> のstreaming_flagにtrueを設定して
		/// Atomプレーヤーを作成する必要があります。）
		/// 本関数は停止中のプレーヤーに対してのみ実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		public void SetWaveId(CriAtomAwb awb, Int32 id)
		{
			NativeMethods.criAtomPlayer_SetWaveId(NativeHandle, awb?.NativeHandle ?? default, id);
		}

		/// <summary>同一音声データの再セット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 前回再生したデータを、再度再生するようAtomプレーヤーに指示します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はデータ要求コールバック関数内でのみ使用します。
		/// （データ要求コールバック関数外でも実行可能ですが、その場合何の効果もありません。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetDataRequestCallback"/>
		public void SetPreviousDataAgain()
		{
			NativeMethods.criAtomPlayer_SetPreviousDataAgain(NativeHandle);
		}

		/// <summary>コールバック関数の再実行要求</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ要求コールバック関数の処理を先延ばしします。
		/// データ要求コールバック関数が実行された時点で、次に再生する音声データが
		/// まだ決まっていない場合、本関数を実行することで、コールバック関数をリトライ
		/// することが可能になります。
		/// （数ミリ秒後に再度データ要求コールバック関数が呼ばれます。）
		/// </para>
		/// <para>
		/// 備考:
		/// データ要求コールバック関数内で何もしなかった場合、Atomプレーヤーのステータスは
		/// <see cref="CriAtomPlayer.Status.Playend"/> に遷移します。
		/// しかし、データ要求コールバック関数内で本関数を実行した場合、Atomプレーヤーの
		/// ステータスは <see cref="CriAtomPlayer.Status.Playing"/> を維持し続けます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行した場合、Atomプレーヤーのステータスは <see cref="CriAtomPlayer.Status.Playing"/>
		/// を維持し続けますが、音声出力は途切れる可能性があります。
		/// （データの読み込みが間に合わない場合、前回再生した音声と、次にセットする音声
		/// との間に、無音が入る可能性があります。）
		/// 本関数はデータ要求コールバック関数内でのみ使用可能です。
		/// （データ要求コールバック関数外で実行した場合、エラーが発生します。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetDataRequestCallback"/>
		public void DeferCallback()
		{
			NativeMethods.criAtomPlayer_DeferCallback(NativeHandle);
		}

		/// <summary>ステータスの取得</summary>
		/// <returns>ステータス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーのステータスを取得します。
		/// ステータスはAtomプレーヤーの再生状態を示す値で、以下の5通りの値が存在します。
		/// -# <see cref="CriAtomPlayer.Status.Stop"/>
		/// -# <see cref="CriAtomPlayer.Status.Prep"/>
		/// -# <see cref="CriAtomPlayer.Status.Playing"/>
		/// -# <see cref="CriAtomPlayer.Status.Playend"/>
		/// -# <see cref="CriAtomPlayer.Status.Error"/>
		/// *
		/// Atomプレーヤーを作成した時点では、Atomプレーヤーのステータスは停止状態
		/// （ <see cref="CriAtomPlayer.Status.Stop"/> ）です。
		/// 再生する音声データをセット後、<see cref="CriAtomPlayer.Start"/> 関数を実行することで、
		/// Atomプレーヤーのステータスが準備状態（ <see cref="CriAtomPlayer.Status.Prep"/> ）に変更されます。
		/// （ <see cref="CriAtomPlayer.Status.Prep"/> は、データ供給やデコードの開始を待っている状態です。）
		/// 再生の開始に充分なデータが供給された時点で、Atomプレーヤーはステータスを
		/// 再生状態（ <see cref="CriAtomPlayer.Status.Playing"/> ）に変更し、音声の出力を開始します。
		/// セットされたデータを全て再生し終えると、Atomプレーヤーはステータスを再生終了状態
		/// （ <see cref="CriAtomPlayer.Status.Playend"/> ）に変更します。
		/// 尚、再生中にエラーが発生した場合には、Atomプレーヤーはステータスをエラー状態
		/// （ <see cref="CriAtomPlayer.Status.Error"/> ）に変更します。
		/// Atomプレーヤーのステータスをチェックし、ステータスに応じて処理を切り替えることで、
		/// 音声の再生状態に連動したプログラムを作成することが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// デバイスからのデータ読み込みに失敗した場合や、データエラーが発生した場合
		/// （不正なデータを読み込んだ場合）、Atomプレーヤーのステータスはエラー状態になります。
		/// データ読み込みエラー発生時にアプリケーションでエラーメッセージ等を表示する場合には、
		/// ステータスが <see cref="CriAtomPlayer.Status.Error"/> になっていないかどうかをチェックし、
		/// 適宜エラーメッセージの表示を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.Start"/>
		public CriAtomPlayer.Status GetStatus()
		{
			return NativeMethods.criAtomPlayer_GetStatus(NativeHandle);
		}

		/// <summary>再生のポーズ／ポーズ解除</summary>
		/// <param name="flag">動作フラグ（true = ポーズ、false = ポーズ解除）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生をポーズ（一時停止）したり、ポーズを解除します。
		/// ポーズするか、ポーズを解除するかは、引数のflagで指定します。
		/// flagにtrueを指定した場合、再生はポーズされます。
		/// flagにfalseを指定した場合、ポーズが解除されます。
		/// Atomプレーヤーがポーズされているかどうかは <see cref="CriAtomPlayer.IsPaused"/> 関数を使用する
		/// ことで確認が可能です。
		/// 本関数は主に以下の2通りの用途に利用します。
		/// - 音声出力の一時停止／一時停止解除。
		/// - ストリーミング再生の頭出し。
		/// *
		/// 【音声出力の一時停止／一時停止解除について】
		/// 再生中のAtomプレーヤーに対してポーズを行うと、その時点で音声の出力を中断します。
		/// ポーズされたAtomプレーヤーに対してポーズ解除を行うと、ポーズ時に中断された箇所から
		/// 再生が再開されます。
		/// 【ストリーミング再生の頭出しについて】
		/// ポーズ処理は再生開始前のAtomプレーヤーに対しても有効です。
		/// 再生開始前のAtomプレーヤーに対してポーズをかけた場合、ポーズされたAtomプレーヤーに
		/// <see cref="CriAtomPlayer.Start"/> 関数で再生指示しても、音声の出力は行われません。
		/// しかし、再生準備は行われるため、データが充分に供給されていれば、ステータスは
		/// <see cref="CriAtomPlayer.Status.Playing"/> まで遷移します。
		/// ステータスが <see cref="CriAtomPlayer.Status.Playing"/> の状態でポーズされているAtomプレーヤー
		/// については、ポーズ解除を行ったタイミングで発音を開始させることが可能です。
		/// そのため、以下の処理手順を踏むことで、ストリーミング再生の発音タイミングを
		/// 他のアクションに同期させることが可能です。
		/// -# <see cref="CriAtomPlayer.Pause"/> 関数でAtomプレーヤーをポーズさせる。
		/// -# <see cref="CriAtomPlayer.Start"/> 関数でAtomプレーヤーに再生開始を指示する。
		/// -# Atomプレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playing"/> になるのを待つ。
		/// -# 発音を開始したいタイミングで <see cref="CriAtomPlayer.Pause"/> 関数を実行し、ポーズを解除する。
		/// *
		/// </para>
		/// <para>
		/// 備考:
		/// 厳密には、 <see cref="CriAtomPlayer.Pause"/> 関数実行後、最初にサーバー処理が動作した時点で
		/// ポーズ処理が行われます。
		/// そのため、サーバー処理が実行される前にポーズ⇒ポーズ解除の操作を行うと、
		/// 音声が止まることなく再生が進む形になります。
		/// 尚、ポーズされたAtomプレーヤーに対して再度ポーズを行ったり、ポーズされていない
		/// Atomプレーヤーに対してポーズ解除を行っても、エラーは発生しません。
		/// （何も処理されずに関数を抜けます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.IsPaused"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		public void Pause(NativeBool flag)
		{
			NativeMethods.criAtomPlayer_Pause(NativeHandle, flag);
		}

		/// <summary>再生の停止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生の停止要求を発行します。
		/// 音声再生中のAtomプレーヤーに対して本関数を実行すると、Atomプレーヤーは再生を停止
		/// （ファイルの読み込みや、発音を止める）し、ステータスを停止状態
		/// （ <see cref="CriAtomPlayer.Status.Stop"/> ）に遷移します。
		/// 既に停止しているAtomプレーヤー（ステータスが <see cref="CriAtomPlayer.Status.Playend"/> や
		/// <see cref="CriAtomPlayer.Status.Error"/> のAtomプレーヤー） に対して本関数を実行すると、
		/// Atomプレーヤーのステータスを <see cref="CriAtomPlayer.Status.Stop"/> に変更します。
		/// </para>
		/// <para>
		/// 注意:
		/// 音声再生中のAtomプレーヤーに対して本関数を実行した場合、ステータスが即座に
		/// <see cref="CriAtomPlayer.Status.Stop"/> になるとは限りません。
		/// （停止状態になるまでに、時間がかかる場合があります。）
		/// そのため、本関数で再生を停止後、続けて別の音声データを再生する場合には、
		/// 必ずステータスが <see cref="CriAtomPlayer.Status.Stop"/> に遷移したことを確認してから
		/// 次のデータをセット（または再生の開始）を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.GetStatus"/>
		public void Stop()
		{
			NativeMethods.criAtomPlayer_Stop(NativeHandle);
		}

		/// <summary>ポーズされているかどうかのチェック</summary>
		/// <returns>ポーズ状態（true = ポーズされている、false = ポーズされていない）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーがポーズされているかどうかをチェックします。
		/// ポーズされているかどうかは、戻り値で判定します。
		/// 戻り値がtrueだった場合、Atomプレーヤーはポーズされています。
		/// 戻り値がfalseだった場合、Atomプレーヤーはポーズされていません。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は <see cref="CriAtomPlayer.Pause"/> 関数で指定された動作フラグをそのまま返します。
		/// （ <see cref="CriAtomPlayer.Pause"/> 関数の第2引数にセットした値がflagとして返されます。）
		/// そのため、本関数の結果と実際に音声出力が停止しているかどうかは、必ずしも一致するとは
		/// 限りません。
		/// （ <see cref="CriAtomPlayer.Pause"/> 関数の実行タイミングと、実際に音声出力が停止するタイミングに
		/// タイムラグが存在するため。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.Pause"/>
		public bool IsPaused()
		{
			return NativeMethods.criAtomPlayer_IsPaused(NativeHandle);
		}

		/// <summary>再生ステータス</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーの再生状態を示す値です。
		/// <see cref="CriAtomPlayer.GetStatus"/> 関数で取得可能です。
		/// 再生状態は、通常以下の順序で遷移します。
		/// -# <see cref="CriAtomPlayer.Status.Stop"/>
		/// -# <see cref="CriAtomPlayer.Status.Prep"/>
		/// -# <see cref="CriAtomPlayer.Status.Playing"/>
		/// -# <see cref="CriAtomPlayer.Status.Playend"/>
		/// *
		/// Atomプレーヤー作成直後の状態は、停止状態（ <see cref="CriAtomPlayer.Status.Stop"/> ）です。
		/// <see cref="CriAtomPlayer.SetData"/> 関数等でデータをセットし、 <see cref="CriAtomPlayer.Start"/> 関数を
		/// 実行すると、再生準備状態（ <see cref="CriAtomPlayer.Status.Prep"/> ）に遷移し、再生準備を始めます。
		/// データが充分供給され、再生準備が整うと、ステータスは再生中（ <see cref="CriAtomPlayer.Status.Playing"/> ）
		/// に変わり、音声の出力が開始されます。
		/// セットされたデータを全て再生し終えた時点で、ステータスは再生完了
		/// （ <see cref="CriAtomPlayer.Status.Playend"/> ）に変わります。
		/// </para>
		/// <para>
		/// 備考
		/// 再生中に <see cref="CriAtomPlayer.Stop"/> 関数を実行した場合、上記の流れに関係なく、
		/// 最終的にステータスは <see cref="CriAtomPlayer.Status.Stop"/> に戻ります。
		/// （ <see cref="CriAtomPlayer.Stop"/> 関数の呼び出しタイミングによっては、 <see cref="CriAtomPlayer.Status.Stop"/>
		/// に遷移するまでに時間がかかる場合があります。）
		/// また、再生中に不正なデータを読み込んだ場合や、ファイルアクセスに失敗した場合も、
		/// 上記の流れに関係なく、ステータスは <see cref="CriAtomPlayer.Status.Error"/> に遷移します。
		/// </para>
		/// <para>
		/// 注意:
		/// ステータスが <see cref="CriAtomPlayer.Status.Prep"/> や <see cref="CriAtomPlayer.Status.Playing"/> のタイミングでは、
		/// データのセット（ <see cref="CriAtomPlayer.SetData"/> 関数）や、再生の開始（ <see cref="CriAtomPlayer.Start"/> 関数）
		/// は行えません。
		/// 現在再生中のAtomプレーヤーを停止して別のデータを再生したい場合は、一旦 <see cref="CriAtomPlayer.Stop"/>
		/// 関数で再生を停止させ、ステータスが <see cref="CriAtomPlayer.Status.Stop"/> に遷移してから次のデータを
		/// セット／再生する必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.GetStatus"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.Stop"/>
		public enum Status
		{
			/// <summary>停止中</summary>
			Stop = 0,
			/// <summary>再生準備中</summary>
			Prep = 1,
			/// <summary>再生中</summary>
			Playing = 2,
			/// <summary>再生完了</summary>
			Playend = 3,
			/// <summary>エラーが発生</summary>
			Error = 4,
		}
		/// <summary>チャンネル数の取得</summary>
		/// <returns>チャンネル数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーで再生中の音声について、チャンネル数を取得します。
		/// チャンネル数が取得できなかった場合、本関数は -1 を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生時刻は、プレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playing"/>
		/// になるまで取得できません。
		/// （ <see cref="CriAtomPlayer.Status.Prep"/> 時に本関数を実行した場合、エラー値が返されます。）
		/// </para>
		/// </remarks>
		public Int32 GetNumChannels()
		{
			return NativeMethods.criAtomPlayer_GetNumChannels(NativeHandle);
		}

		/// <summary>再生済みサンプル数の取得</summary>
		/// <param name="numPlayed">再生済みサンプル数（サンプル数単位）</param>
		/// <param name="samplingRate">サンプリングレート（Hz単位）</param>
		/// <returns>サンプル数が取得できたかどうか（ true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーで再生中の音声について、再生済みのサンプル数、
		/// およびサンプリングレートを取得します。
		/// 再生時刻はサンプル数単位、サンプリングレートはHz単位です。
		/// サンプル数が正しく取得できた場合、戻り値は true になります。
		/// 再生済みサンプル数が取得できなかった場合、戻り値は false になります。
		/// （同時に、 sampling_rate は -1 になります。）
		/// </para>
		/// <para>
		/// 備考
		/// 取得する必要のない引数については、 null を指定可能です。
		/// 例えば、サンプリングレートのみを取得したい場合、第2引数（ num_played ）
		/// には null を指定可能です。
		/// 本関数が返す再生済みサンプル数は、出力済み音声データの累積値です。
		/// そのため、ループ再生時や、シームレス連結再生時を行った場合でも、
		/// 再生位置に応じてサンプル数が巻き戻ることはありません。
		/// また、 <see cref="CriAtomPlayer.Pause"/> 関数でポーズをかけた場合、
		/// 再生済みサンプル数のカウントアップも停止します。
		/// （ポーズを解除すればカウントアップが再開されます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 再生済みサンプル数は、プレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playing"/>
		/// になるまで取得できません。
		/// （ <see cref="CriAtomPlayer.Status.Prep"/> 時に本関数を実行した場合、エラー値が返されます。）
		/// 再生サンプル数の精度は、プラットフォームのサウンドライブラリに依存します。
		/// </para>
		/// </remarks>
		public unsafe bool GetNumPlayedSamples(out Int64 numPlayed, out Int32 samplingRate)
		{
			fixed (Int64* numPlayedPtr = &numPlayed)
			fixed (Int32* samplingRatePtr = &samplingRate)
				return NativeMethods.criAtomPlayer_GetNumPlayedSamples(NativeHandle, numPlayedPtr, samplingRatePtr);
		}

		/// <summary>サウンドバッファへの書き込みサンプル数の取得</summary>
		/// <param name="numRendered">書き込み済みサンプル数（サンプル数単位）</param>
		/// <param name="samplingRate">サンプリングレート（Hz単位）</param>
		/// <returns>サンプル数が取得できたかどうか（ true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤで再生中の音声について、サウンドバッファへの書き込み済みのサンプル数、
		/// およびサンプリングレートを取得します。
		/// <see cref="CriAtomPlayer.GetNumPlayedSamples"/> 関数と異なり、
		/// サウンドバッファに書き込まれた未出力の音声データのサンプル数を含む値を返します。
		/// サンプル数が正しく取得できた場合、戻り値は true になります。
		/// 書き込み済みサンプル数が取得できなかった場合、戻り値は false になります。
		/// （同時に、 sampling_rate は -1 になります。）
		/// </para>
		/// <para>
		/// 備考
		/// 取得する必要のない引数については、 null を指定可能です。
		/// 例えば、サンプリングレートのみを取得したい場合、第2引数（ num_rendered ）
		/// には null を指定可能です。
		/// 本関数が返すサンプル数は、書き込み済みサンプル数の累積値です。
		/// そのため、ループ再生やシームレス連結再生を行った場合でも、
		/// 再生位置に応じてサンプル数が巻き戻ることはありません。
		/// また、 <see cref="CriAtomPlayer.Pause"/> 関数でポーズをかけた場合、
		/// 書き込み済みサンプル数のカウントアップも停止します。
		/// （ポーズを解除すればカウントアップが再開されます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 書き込み済みサンプル数は、プレーヤのステータスが <see cref="CriAtomPlayer.Status.Playing"/>
		/// になるまで取得できません。
		/// （ <see cref="CriAtomPlayer.Status.Prep"/> 時に本関数を実行した場合、エラー値が返されます。）
		/// 書き込み済みサンプル数の精度は、プラットフォームのサウンドライブラリに依存します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.GetNumPlayedSamples"/>
		public unsafe bool GetNumRenderedSamples(out Int64 numRendered, out Int32 samplingRate)
		{
			fixed (Int64* numRenderedPtr = &numRendered)
			fixed (Int32* samplingRatePtr = &samplingRate)
				return NativeMethods.criAtomPlayer_GetNumRenderedSamples(NativeHandle, numRenderedPtr, samplingRatePtr);
		}

		/// <summary>デコードデータサイズの取得</summary>
		/// <returns>デコードデータ量（単位はバイト）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤー内でデコードした音声データのバイト数を返します。
		/// </para>
		/// <para>
		/// 備考
		/// 本関数が返すデコード量は、再生開始時点からの累積値です。
		/// そのため、ループ再生時や、シームレス連結再生時を行った場合でも、
		/// 再生位置に応じてデコード量が巻き戻ることはありません。
		/// また、 <see cref="CriAtomPlayer.Pause"/> 関数でポーズをかけた場合、
		/// デコード量のカウントアップも停止します。
		/// （ポーズを解除すればカウントアップが再開されます。）
		/// </para>
		/// <para>
		/// 注意:
		/// HCA-MXを使用する場合や、圧縮された音声データを直接ハードウェアに送信するプラットフォーム
		/// （デコード処理がプラットフォームSDKに隠蔽されているコーデック）
		/// については、本関数でデコード量を取得できません。
		/// </para>
		/// </remarks>
		public Int64 GetDecodedDataSize()
		{
			return NativeMethods.criAtomPlayer_GetDecodedDataSize(NativeHandle);
		}

		/// <summary>デコードサンプル数の取得</summary>
		/// <returns>デコードサンプル数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤー内でデコードした音声データのバイト数を返します。
		/// </para>
		/// <para>
		/// 備考
		/// 本関数が返すデコードサンプル数は、再生開始時点からの累積値です。
		/// そのため、ループ再生時や、シームレス連結再生時を行った場合でも、
		/// 再生位置に応じてデコードサンプル数が巻き戻ることはありません。
		/// また、 <see cref="CriAtomPlayer.Pause"/> 関数でポーズをかけた場合、
		/// デコードサンプル数のカウントアップも停止します。
		/// （ポーズを解除すればカウントアップが再開されます。）
		/// </para>
		/// <para>
		/// 注意:
		/// HCA-MXを使用する場合や、圧縮された音声データを直接ハードウェアに送信するプラットフォーム
		/// （デコード処理がプラットフォームSDKに隠蔽されているコーデック）
		/// については、本関数でデコードサンプル数を取得できません。
		/// </para>
		/// </remarks>
		public Int64 GetNumDecodedSamples()
		{
			return NativeMethods.criAtomPlayer_GetNumDecodedSamples(NativeHandle);
		}

		/// <summary>再生時刻の取得</summary>
		/// <returns>再生時刻（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーで再生中の音声について、現在の再生時刻を取得します。
		/// 再生時刻の単位はミリ秒単位です。
		/// 引数に誤りがある場合（ player が null の場合）、本関数は -1 を返します。
		/// 停止中や再生準備中等、再生時刻が取得できないタイミングで本関数を実行した場合、
		/// 本関数は 0 を返します。
		/// </para>
		/// <para>
		/// 備考
		/// 再生時刻は再生済みサンプル数を元に計算されています。
		/// そのため、 <see cref="CriAtomPlayer.SetFrequencyRatio"/> 関数を用いてピッチを上げた場合、
		/// 再生時刻は実時間よりも早く進みます。
		/// （ピッチを下げた場合、再生時刻は実時刻よりも遅く進みます。）
		/// 本関数が返す再生時刻は、出力済み音声データの累積値です。
		/// そのため、ループ再生時や、シームレス連結再生時を行った場合でも、
		/// 再生位置に応じて時刻が巻き戻ることはありません。
		/// また、 <see cref="CriAtomPlayer.Pause"/> 関数でポーズをかけた場合、
		/// 再生時刻のカウントアップも停止します。
		/// （ポーズを解除すればカウントアップが再開されます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 再生時刻は、プレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playing"/>
		/// になるまで取得できません。
		/// （ <see cref="CriAtomPlayer.Status.Prep"/> 時に本関数を実行した場合、 0 が返されます。）
		/// 再生時刻の精度は、プラットフォームのサウンドライブラリに依存します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.GetStatus"/>
		/// <seealso cref="CriAtomPlayer.SetFrequencyRatio"/>
		public Int64 GetTime()
		{
			return NativeMethods.criAtomPlayer_GetTime(NativeHandle);
		}

		/// <summary>再生音声のフォーマット情報の取得</summary>
		/// <param name="info">フォーマット情報</param>
		/// <returns>情報が取得できたかどうか（ true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomPlayer.Start"/> 関数で再生された音声のフォーマット情報を取得します。
		/// フォーマット情報が取得できた場合、本関数は true を返します。
		/// フォーマット情報が取得できなかった場合、本関数は false を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみフォーマット情報を取得可能です。
		/// 再生開始前や再生準備中に本関数を実行すると、フォーマット情報の取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.GetStatus"/>
		public unsafe bool GetFormatInfo(out CriAtom.FormatInfo info)
		{
			fixed (CriAtom.FormatInfo* infoPtr = &info)
				return NativeMethods.criAtomPlayer_GetFormatInfo(NativeHandle, infoPtr);
		}

		/// <summary>入力バッファー内データ残量の取得</summary>
		/// <returns>入力バッファー内のデータ残量（Byte単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーの入力バッファー内のデータ残量を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は情報取得用途にのみに利用可能なデバッグ関数です。
		/// 音途切れの不具合が発生した際、本関数を使用して再生中のプレーヤーの
		/// 入力バッファーにデータが残っているかどうかをチェック可能です。
		/// プレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playing"/> にもかかわらず、
		/// データ残量が長時間 0 の場合、何らかの異常によりデータの供給が
		/// ブロックされている可能性があります。
		/// </para>
		/// </remarks>
		public Int32 GetInputBufferRemainSize()
		{
			return NativeMethods.criAtomPlayer_GetInputBufferRemainSize(NativeHandle);
		}

		/// <summary>出力バッファー内データ残量の取得</summary>
		/// <returns>出力バッファー内のデータ残量（サンプル数単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーの出力バッファー内のデータ残量を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は情報取得用途にのみに利用可能なデバッグ関数です。
		/// 音途切れの不具合が発生した際、本関数を使用して再生中のプレーヤーの
		/// 出力バッファーにデータが残っているかどうかをチェック可能です。
		/// プレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playing"/> にもかかわらず、
		/// データ残量が長時間 0 の場合、何らかの異常によりデコード処理が
		/// 行われていない可能性があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声データをAtomライブラリ内でデコードするケースについてのみ有効です。
		/// 圧縮された音声データを直接ハードウェアに送信するプラットフォーム
		/// （デコード処理がプラットフォームSDKに隠蔽されているコーデック）
		/// については、本関数でデータ残量を取得できません。
		/// </para>
		/// </remarks>
		public Int32 GetOutputBufferRemainSamples()
		{
			return NativeMethods.criAtomPlayer_GetOutputBufferRemainSamples(NativeHandle);
		}

		/// <summary>再生開始位置の指定</summary>
		/// <param name="startTimeMs">再生開始位置（ミリ秒指定）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤーで再生する音声について、再生を開始する位置を指定します。
		/// 音声データを途中から再生したい場合、再生開始前に本関数で再生開始位置を
		/// 指定する必要があります。
		/// 再生開始位置の指定はミリ秒単位で行います。
		/// 例えば、 start_time_ms に 10000 をセットして本関数を実行すると、
		/// 次に再生する音声データは 10 秒目の位置から再生されます。
		/// </para>
		/// <para>
		/// 備考
		/// 本関数で再生位置を指定した場合でも、指定した時刻ぴったりの位置から再生が
		/// 開始されるとは限りません。
		/// （使用する音声コーデックによっては、指定時刻の少し手前から再生が開始されます。）
		/// 音声データ途中からの再生は、音声データ先頭からの再生に比べ、発音開始の
		/// タイミングが遅くなります。
		/// これは、一旦音声データのヘッダーを解析後、指定位置にジャンプしてからデータを読み
		/// 直して再生を開始するためです。
		/// </para>
		/// <para>
		/// 注意:
		/// start_time_ms には64bit値をセット可能ですが、現状、32bit以上の再生時刻を
		/// 指定することはできません。
		/// 音声再生中に本関数を実行しても、再生中の音声の再生位置は変更されません。
		/// 本関数で設定した値は、 <see cref="CriAtomPlayer.Start"/> 関数で音声の再生を開始する
		/// タイミングでのみ参照されます。
		/// 機種固有の音声フォーマットについても、再生開始位置を指定できない場合があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.Start"/>
		public void SetStartTime(Int64 startTimeMs)
		{
			NativeMethods.criAtomPlayer_SetStartTime(NativeHandle, startTimeMs);
		}

		/// <summary>ボリュームの指定</summary>
		/// <param name="vol">ボリューム値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力音声のボリュームを指定します。
		/// 本関数を使用することで、Atomプレーヤーで再生する音声のボリュームを自由に
		/// 変更可能です。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのボリュームで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// \備考:
		/// ボリューム値には0.0f以上の値が設定可能です。
		/// （Atomライブラリ Ver.1.21.07より、
		/// ボリューム値に1.0fを超える値を指定できるようになりました。）
		/// 1.0fを超える値をセットした場合、<b>プラットフォームによっては</b>、
		/// 波形データを元素材よりも大きな音量で再生可能です。
		/// ボリューム値に負値を指定した場合、値は0.0fにクリップされます。
		/// （波形データの位相が反転されることはありません。）
		/// 本関数の設定値と、以下の関数のボリューム設定値は独立して制御されます。
		/// - <see cref="CriAtomPlayer.SetChannelVolume"/>
		/// - <see cref="CriAtomPlayer.SetSendLevel"/>
		/// *
		/// 例えば、本関数に0.5fを、 <see cref="CriAtomPlayer.SetChannelVolume"/>
		/// 関数にも0.5fを設定した場合、
		/// 出力音声のボリュームは原音を0.25f倍したボリュームで出力されます。
		/// （0.5f×0.5f＝0.25fの演算が行われます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 1.0fを超えるボリュームを指定する場合、以下の点に注意する必要があります。
		/// - プラットフォームごとに挙動が異なる可能性がある。
		/// - 音割れが発生する可能性がある。
		/// *
		/// 本関数に1.0fを超えるボリューム値を設定した場合でも、
		/// 音声が元の波形データよりも大きな音量で再生されるかどうかは、
		/// プラットフォームや音声圧縮コーデックの種別によって異なります。
		/// そのため、マルチプラットフォームタイトルでボリュームを調整する場合には、
		/// 1.0fを超えるボリューム値を使用しないことをおすすめします。
		/// （1.0fを超えるボリューム値を指定した場合、同じ波形データを再生した場合でも、
		/// 機種ごとに異なる音量で出力される可能性があります。）
		/// また、音量を上げることが可能な機種であっても、
		/// ハードウェアで出力可能な音量には上限があるため、
		/// 音割れによるノイズが発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.GetVolume"/>
		public void SetVolume(Single vol)
		{
			NativeMethods.criAtomPlayer_SetVolume(NativeHandle, vol);
		}

		/// <summary>ボリュームの指定</summary>
		/// <returns>ボリューム値</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力音声のボリュームを取得します。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetVolume"/>
		public Single GetVolume()
		{
			return NativeMethods.criAtomPlayer_GetVolume(NativeHandle);
		}

		/// <summary>チャンネル単位のボリューム指定</summary>
		/// <param name="ch">チャンネル番号</param>
		/// <param name="vol">ボリューム値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力音声のボリュームをチャンネル単位で指定します。
		/// 本関数を使用することで、Atomプレーヤーで再生する音声のボリュームを、
		/// チャンネル単位で自由に変更可能です。
		/// 第2引数のチャンネル番号は"音声データのチャンネル番号"を指定します。
		/// （出力スピーカーのIDではありません。）
		/// 例えば、モノラル音声の0番のボリュームを変更した場合、
		/// スピーカーから出力される音声のボリューム全てが変更されます。
		/// （ <see cref="CriAtomPlayer.SetVolume"/> 関数を実行するのと同じ動作をします。）
		/// これに対し、ステレオ音声の0番のボリュームを変更すると、デフォルト設定
		/// ではレフトスピーカーから出力される音声のボリュームのみが変更されます。
		/// （ <see cref="CriAtomPlayer.SetSendLevel"/> 関数を併用している場合は、
		/// 必ずしもレフトスピーカーから出力される音量のボリュームが変更される
		/// とは限りません。）
		/// ボリューム値には、0.0f～1.0fの範囲で実数値を指定します。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのボリュームで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// \備考:
		/// ボリューム値に1.0fを超える値を指定した場合、値は1.0fにクリップされます。
		/// （原音より大きな音量で音声が再生されることはありません。）
		/// 同様に、ボリューム値に0.0f未満の値を指定した場合も、値は0.0fにクリップされます。
		/// （位相が反転されることはありません。）
		/// 本関数の設定値と、以下の関数のボリューム設定値は独立して制御されます。
		/// - <see cref="CriAtomPlayer.SetVolume"/>
		/// - <see cref="CriAtomPlayer.SetSendLevel"/>
		/// 例えば、本関数に0.5fを、 <see cref="CriAtomPlayer.SetVolume"/> 関数にも0.5fを設定した場合、
		/// 出力音声のボリュームは原音を0.25f倍したボリュームで出力されます。
		/// （0.5f×0.5f＝0.25fの演算が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetVolume"/>
		/// <seealso cref="CriAtomPlayer.SetSendLevel"/>
		/// <seealso cref="CriAtomPlayer.SetPanAdx1Compatible"/>
		public void SetChannelVolume(Int32 ch, Single vol)
		{
			NativeMethods.criAtomPlayer_SetChannelVolume(NativeHandle, ch, vol);
		}

		/// <summary>センドレベルの設定</summary>
		/// <param name="ch">チャンネル番号</param>
		/// <param name="spk">スピーカーID</param>
		/// <param name="level">ボリューム値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// センドレベルを指定します。
		/// センドレベルは、音声データの各チャンネルの音声を、どのスピーカーから
		/// どの程度の音量で出力するかを指定するための仕組みです。
		/// 第2引数のチャンネル番号は"音声データのチャンネル番号"を指定します。
		/// 第3引数のスピーカーIDには、指定したチャンネル番号のデータをどのスピーカーから
		/// 出力するかを指定し、第4引数の送信時のボリュームを指定します。
		/// ボリューム値は、0.0f～1.0fの範囲で実数値を指定します。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのボリュームで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// </para>
		/// <para>
		/// チャンネル単位のボリューム指定（ <see cref="CriAtomPlayer.SetChannelVolume"/> 関数）
		/// と異なり、本関数では1つのチャンネルのデータを複数のスピーカーから異なる
		/// ボリュームで出力することが可能です。
		/// </para>
		/// <para>
		/// 尚、セットされたセンドレベルの値は <see cref="CriAtomPlayer.ResetSendLevel"/> 関数で
		/// リセットすることが可能です。
		/// \備考:
		/// センドレベルの設定には「自動設定」「手動設定」の2通りが存在します。
		/// Atomプレーヤーを作成した直後や、 <see cref="CriAtomPlayer.ResetSendLevel"/> 関数で
		/// センドレベルをクリアした場合、センドレベルの設定は「自動設定」となります。
		/// 本関数を実行した場合、センドレベルの設定は「手動設定」となります。
		/// 「自動設定」の場合、Atomプレーヤーは以下のように音声をルーティングします。
		/// 【モノラル音声を再生する場合】
		/// チャンネル0の音声を左右のスピーカーから約0.7f（-3dB）のボリュームで出力します。
		/// 【ステレオ音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、
		/// チャンネル1の音声をライトスピーカーから出力します。
		/// 【4ch音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、チャンネル1の音声をライトスピーカーから、
		/// チャンネル2の音声をサラウンドレフトスピーカーから、
		/// チャンネル3の音声をサラウンドライトスピーカーからでそれぞれ出力します。
		/// 【5ch音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、チャンネル1の音声をライトスピーカーから、
		/// チャンネル2の音声をセンタースピーカーから、
		/// チャンネル3の音声をサラウンドレフトスピーカーから、
		/// チャンネル4の音声をサラウンドライトスピーカーからそれぞれ出力します。
		/// （ 5ch音声を再生する場合、 ::criAtom_SetChannelMapping
		/// 関数で別の並び順に変更することも可能です。）
		/// 【5.1ch音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、チャンネル1の音声をライトスピーカーから、
		/// チャンネル2の音声をセンタースピーカーから、チャンネル3の音声をLFEから、
		/// チャンネル4の音声をサラウンドレフトスピーカーから、
		/// チャンネル5の音声をサラウンドライトスピーカーからそれぞれ出力します。
		/// （ 6ch音声を再生する場合、 ::criAtom_SetChannelMapping
		/// 関数で別の並び順に変更することも可能です。）
		/// 【7.1ch音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、チャンネル1の音声をライトスピーカーから、
		/// チャンネル2の音声をセンタースピーカーから、チャンネル3の音声をLFEから、
		/// チャンネル4の音声をサラウンドレフトスピーカーから、
		/// チャンネル5の音声をサラウンドライトスピーカーから、
		/// チャンネル6の音声をサラウンドバックレフトスピーカーから、
		/// チャンネル7の音声をサラウンドバックライトスピーカーからそれぞれ出力します。
		/// これに対し、本関数を用いて「手動設定」を行った場合、音声データのチャンネル数に
		/// 関係なく、指定されたルーティングで音声が出力されます。
		/// （センドレベルを設定していないチャンネルの音声は出力されません。）
		/// センドレベルの設定をクリアし、ルーティングを「自動設定」の状態に戻したい場合は、
		/// <see cref="CriAtomPlayer.ResetSendLevel"/> 関数を実行してください。
		/// ボリューム値に1.0fを超える値を指定した場合、値は1.0fにクリップされます。
		/// （原音より大きな音量で音声が再生されることはありません。）
		/// 同様に、ボリューム値に0.0f未満の値を指定した場合も、値は0.0fにクリップされます。
		/// （位相が反転されることはありません。）
		/// 本関数の設定値と、以下の関数のボリューム設定値は独立して制御されます。
		/// - <see cref="CriAtomPlayer.SetVolume"/>
		/// - <see cref="CriAtomPlayer.SetChannelVolume"/>
		/// 例えば、本関数に0.5fを、 <see cref="CriAtomPlayer.SetVolume"/> 関数にも0.5fを設定した場合、
		/// 出力音声のボリュームは原音を0.25f倍したボリュームで出力されます。
		/// （0.5f×0.5f＝0.25fの演算が行われます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 再生する音声データがマルチチャンネルのデータであっても、センドレベルが一部の
		/// チャンネルのみにしか設定されていない場合、センドレベルの設定されていない
		/// チャンネルの音声は出力されません。
		/// 本関数と <see cref="CriAtomPlayer.SetPanAdx1Compatible"/> 関数を併用しないでください。
		/// <see cref="CriAtomPlayer.SetPanAdx1Compatible"/> 関数が、内部的に本関数を呼び出すため、
		/// 両者を併用した場合、後から実行した関数により設定が上書きされる可能性があります。
		/// 音源の定位をコントロールする際には、本関数かまたは <see cref="CriAtomPlayer.SetPanAdx1Compatible"/> 関数
		/// のいずれか一方のみをご利用ください。
		/// （3Dパンを利用する場合は本関数を、2Dパンのみを行う場合は <see cref="CriAtomPlayer.SetPanAdx1Compatible"/>
		/// 関数をご利用ください。）
		/// 本関数は一部の機種でのみ利用が可能です。
		/// （プラットフォームのサウンドライブラリの仕様によっては実装が困難な場合が
		/// あり、その場合は利用できません。）
		/// 本関数が利用可能かどうかは、別途マニュアルの機種依存情報のページをご参照ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetVolume"/>
		/// <seealso cref="CriAtomPlayer.SetChannelVolume"/>
		/// <seealso cref="CriAtomPlayer.SetPanAdx1Compatible"/>
		/// <seealso cref="CriAtomPlayer.ResetSendLevel"/>
		public void SetSendLevel(Int32 ch, CriAtom.SpeakerId spk, Single level)
		{
			NativeMethods.criAtomPlayer_SetSendLevel(NativeHandle, ch, spk, level);
		}

		/// <summary>センドレベルのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// センドレベルの設定をリセットします。
		/// 本関数を実行することで、過去にセットされたセンドレベル設定が全てクリアされます。
		/// \備考:
		/// センドレベルの設定には「自動設定」「手動設定」の2通りが存在します。
		/// Atomプレーヤーを作成した直後や、本関数でセンドレベルをリセットした場合、
		/// センドレベルの設定は「自動設定」となります。
		/// （自動設定時のルーティングについては、 <see cref="CriAtomPlayer.SetSendLevel"/>
		/// 関数の説明を参照してください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数と <see cref="CriAtomPlayer.ResetPan"/> 関数を併用しないでください。
		/// <see cref="CriAtomPlayer.ResetPan"/> 関数が、内部的に本関数を呼び出すため、
		/// 両者を併用した場合、後から実行した関数により設定が上書きされる可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetSendLevel"/>
		/// <seealso cref="CriAtomPlayer.ResetPan"/>
		public void ResetSendLevel()
		{
			NativeMethods.criAtomPlayer_ResetSendLevel(NativeHandle);
		}

		/// <summary>パンの設定</summary>
		/// <param name="ch">チャンネル番号</param>
		/// <param name="pan">パン設定値（-1.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パン（音源の定位位置）を指定します。
		/// 本関数を実行することで、モノラル音声やステレオ音声に対し、音源の定位位置を自由
		/// にコントロールすることが可能です。
		/// 第2引数のチャンネル番号は"音声データのチャンネル番号"を指定します。
		/// 第3引数のパン設定値には、指定したチャンネル番号のデータの定位をどの位置にする
		/// かを指定します。
		/// パン設定値は、-1.0f～1.0fの範囲で実数値を指定します。
		/// 音源は、負の値を指定すると中央より左側（値が小さいほど左寄り）、0.0fを指定すると
		/// 中央、正の値を指定すると中央より右側（値が大きいほど右寄り）に定位します。
		/// （キリのいい値では、-1.0fが左端、0.0fが中央、1.0fが右端になります。）
		/// -1.0fと1.0fの間では、音源の位置はリニアに変化します。
		/// つまり、パン設定値を一定量ずつ変化させながら-1.0～1.0まで変更した場合、
		/// 音源は左端から右端へ一定速度で移動することになります。
		/// </para>
		/// <para>
		/// 備考:
		/// パンの設定には「自動設定」「手動設定」の2通りが存在します。
		/// Atomプレーヤーを作成した直後や、 <see cref="CriAtomPlayer.ResetPan"/> 関数で
		/// パンをクリアした場合、パンの設定は「自動設定」となります。
		/// 本関数を実行した場合、パンの設定は「手動設定」となります。
		/// 「自動設定」の場合、Atomプレーヤーは以下のように音声をルーティングします。
		/// 【モノラル音声を再生する場合】
		/// チャンネル0の音声を左右のスピーカーから約0.7f（-3dB）のボリュームで出力します。
		/// 【ステレオ音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、
		/// チャンネル1の音声をライトスピーカーから出力します。
		/// これに対し、本関数を用いて「手動設定」を行った場合、音声データのチャンネル数に
		/// 関係なく、指定されたルーティングで音声が出力されます。
		/// パンの設定をクリアし、ルーティングを「自動設定」の状態に戻したい場合は、
		/// <see cref="CriAtomPlayer.ResetPan"/> 関数を実行してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数でパンをコントロール可能なのは、モノラル音声とステレオ音声のみです。
		/// 3ch以上の音声に対してパンをコントロールしたい場合には、 <see cref="CriAtomPlayer.SetSendLevel"/>
		/// 関数を使用する必要があります。
		/// 再生する音声データがステレオの場合、チャンネル0番とチャンネル1番のそれぞれの
		/// について、独立してパンをコントロールすることが可能です。
		/// ただし、設定されたパンがモノラル音声向けなのか、ステレオ音声向けなのかは区別
		/// されないため、ステレオ設定用にパン設定を行ったAtomプレーヤーでモノラル音声を再生
		/// した場合、意図しない位置に音源が定位する可能性があります。
		/// 再生する音声データがステレオにもかかわらず、どちらか一方のチャンネルに対して
		/// しかパンが設定されていない場合、パンを設定していないチャンネルの音声の定位位置
		/// は 0.0f （中央からの出力）になります。
		/// ステレオ音声のパンをコントロールする際には、必ず両方のチャンネルについてパンの
		/// 設定を行ってください。
		/// 本関数と <see cref="CriAtomPlayer.SetSendLevel"/> 関数を併用しないでください。
		/// 本関数が内部的に <see cref="CriAtomPlayer.SetSendLevel"/> 関数を呼び出すため、
		/// 両者を併用した場合、後から実行した関数により設定が上書きされる可能性があります。
		/// 音源の定位をコントロールする際には、本関数かまたは <see cref="CriAtomPlayer.SetSendLevel"/> 関数
		/// のいずれか一方のみをご利用ください。
		/// （3Dパンを利用する場合は <see cref="CriAtomPlayer.SetSendLevel"/> 関数を、2Dパンのみを行う場合は
		/// 本関数をご利用ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetVolume"/>
		/// <seealso cref="CriAtomPlayer.SetChannelVolume"/>
		/// <seealso cref="CriAtomPlayer.SetPanAdx1Compatible"/>
		/// <seealso cref="CriAtomPlayer.ResetPan"/>
		public void SetPanAdx1Compatible(Int32 ch, Single pan)
		{
			NativeMethods.criAtomPlayer_SetPanAdx1Compatible(NativeHandle, ch, pan);
		}

		/// <summary>パンのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンの設定をリセットします。
		/// 本関数を実行することで、過去にセットされたパン設定が全てクリアされます。
		/// \備考:
		/// パンの設定には「自動設定」「手動設定」の2通りが存在します。
		/// Atomプレーヤーを作成した直後や、本関数でパンをリセットした場合、
		/// パンの設定は「自動設定」となります。
		/// （自動設定時のルーティングについては、 <see cref="CriAtomPlayer.SetPanAdx1Compatible"/>
		/// 関数の説明を参照してください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数と <see cref="CriAtomPlayer.ResetSendLevel"/> 関数を併用しないでください。
		/// 本関数が内部的に <see cref="CriAtomPlayer.ResetSendLevel"/> 関数を呼び出すため、
		/// 両者を併用した場合、後から実行した関数により設定が上書きされる可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetPanAdx1Compatible"/>
		/// <seealso cref="CriAtomPlayer.ResetSendLevel"/>
		public void ResetPan()
		{
			NativeMethods.criAtomPlayer_ResetPan(NativeHandle);
		}

		/// <summary>周波数調整比の設定</summary>
		/// <param name="ratio">周波数調整比</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声の周波数調整比を設定します。
		/// 周波数調整比は、音声データの周波数と再生周波数の比率で、再生速度の倍率と等価です。
		/// 周波数比が1.0fを超える場合、音声データは原音より高速に再生され、
		/// 1.0f未満の場合は、音声データは原音より低速で再生されます。
		/// 周波数比は、音声のピッチにも影響します。
		/// 例えば、周波数比を1.0fで再生した場合、音声データは原音通りのピッチで再生されますが、
		/// 周波数比を2.0fに変更した場合、ピッチは1オクターブ上がます。
		/// （再生速度が2倍になるため。）
		/// </para>
		/// <para>
		/// 注意:
		/// 周波数比に1.0fを超える値を設定した場合、再生する音声のデータが通常より
		/// 速く消費されるため、音声データの供給や、データのデコードが間に合わなくなる
		/// 可能性があります。
		/// （音切れ等の問題が発生する可能性があります。）
		/// 周波数比に1.0fを超える値を設定する場合には、Atomプレーヤー作成時に指定する
		/// 最大サンプリングレートの値を、周波数比を考慮した値に設定してください。
		/// （Atomプレーヤー作成時に指定する <see cref="CriAtom.AdxPlayerConfig"/> 構造体
		/// の max_sampling_rate の値に、「原音のサンプリングレート×周波数比」で
		/// 計算される値を指定する必要があります。）
		/// </para>
		/// <para>
		/// 本関数は一部の機種でのみ利用が可能です。
		/// （プラットフォームのサウンドライブラリの仕様によっては実装が困難な場合が
		/// あり、その場合は利用できません。）
		/// 本関数が利用可能かどうかは、別途マニュアルの機種依存情報のページをご参照ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		/// <seealso cref="CriAtomPlayer.SetMaxFrequencyRatio"/>
		public void SetFrequencyRatio(Single ratio)
		{
			NativeMethods.criAtomPlayer_SetFrequencyRatio(NativeHandle, ratio);
		}

		/// <summary>最大周波数調整比の設定</summary>
		/// <param name="ratio">最大周波数調整比</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声の最大周波数調整比を設定します。
		/// 本関数で最大周波数調整比を指定することで、指定範囲内でのピッチ変更が即座に反映されるようになります。
		/// </para>
		/// <para>
		/// 備考:
		/// Atom Ver.2.10.00以前のライブラリでは、ピッチを上げた際に音が途切れる
		/// （再生速度が速くなった結果、音声データの供給が足りなくなる）ケースがありました。
		/// この対策として、Atom Ver.2.10.00ではピッチを上げても音が途切れないよう、
		/// 音声を充分にバッファリングしてからピッチを上げるよう動作を変更しています。
		/// 修正により、ピッチ操作によって音が途切れることはなくなりましたが、
		/// ピッチを上げる際にバッファリングを待つ時間分だけピッチ変更が遅れる形になるため、
		/// 音の変化が以前のバージョンと比べて緩慢になる可能性があります。
		/// （短時間にピッチを上げ下げするケースにおいて、音の鳴り方が変わる可能性があります。）
		/// 本関数で最大周波数調整比をあらかじめ設定した場合、
		/// 指定された速度を想定して常にバッファリングが行われるようになるため、
		/// （指定された範囲内の周波数においては）バッファリングなしにピッチ変更が即座に行われます。
		/// 短時間にピッチを上げ下げするケースについては、
		/// 予想される最大周波数調整比をあらかじめ本関数で設定してから再生を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetFrequencyRatio"/>
		public void SetMaxFrequencyRatio(Single ratio)
		{
			NativeMethods.criAtomPlayer_SetMaxFrequencyRatio(NativeHandle, ratio);
		}

		/// <summary>ループ回数の制限</summary>
		/// <param name="count">ループ制限回数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 波形データのループ再生回数を制限します。
		/// 例えば、countに1を指定した場合、ループ波形データは1回のみループして再生を終了します。
		/// （ループエンドポイントに到達後、1回だけループスタート位置に戻ります。）
		/// </para>
		/// <para>
		/// 備考:
		/// デフォルト状態では、ループポイント付きの音声データは無限にループ再生されます。
		/// ループ回数を一旦制限した後、ループ回数を再度無限回に戻したい場合には、
		/// count に <see cref="CriAtomPlayer.NoLoopLimitation"/> を指定してください。
		/// count に <see cref="CriAtomPlayer.IgnoreLoop"/> を指定することで、
		/// ループポイント付きの音声データをループさせずに再生することも可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// ループ制限回数の指定は、音声再生開始前に行う必要があります。
		/// 再生中に本関数を実行しても、ループ回数は変更されません。
		/// 再生中の任意のタイミングでループ再生を停止したい場合、
		/// ループ再生ではなく、シームレス連結再生で制御を行ってください。
		/// 本関数で指定したループ制限回数は、
		/// あらかじめループポイントが設定された波形データを再生する場合にのみ適用されます。
		/// 波形データ自体にループポイントが設定されていない場合、
		/// 本関数を実行しても何の効果もありません。
		/// 本関数を使用してループ回数を指定した場合でも、
		/// ループ終了時にループエンドポイント以降の波形データが再生されることはありません。
		/// （指定回数分ループした後、ループエンドポイントで再生が停止します。）
		/// 例外的に、以下の条件を満たす場合に限り、（ループはされませんが）
		/// ワンショットでループポイント以降のデータを含めて再生することが可能です。
		/// - criatomencd.exe で -nodelterm を指定してデータをエンコードする。
		/// - 本関数に <see cref="CriAtomPlayer.IgnoreLoop"/> を指定してから再生を行う。
		/// *
		/// 本関数でループ回数を制限できるのは、ADXコーデックとHCAコーデックのみです。
		/// プラットフォーム依存の音声コーデックに対して本関数を実行しないでください。
		/// （再生が終了しない、ノイズが発生する等の問題が発生します。）
		/// </para>
		/// </remarks>
		public void LimitLoopCount(Int32 count)
		{
			NativeMethods.criAtomPlayer_LimitLoopCount(NativeHandle, count);
		}

		/// <summary>HCA-MXデコード先ミキサIDの指定</summary>
		/// <param name="mixerId">ミキサID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXのデコード先ミキサIDを指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は <see cref="CriAtomPlayer.CreateHcaMxPlayer"/>
		/// 関数で作成されたプレーヤーに対してのみ効果があります。
		/// （他の関数で作成されたプレーヤーに対しては、何の効果もありません。）
		/// 本関数は停止中のプレーヤーに対してのみ実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateHcaMxPlayer"/>
		public void SetHcaMxMixerId(Int32 mixerId)
		{
			NativeMethods.criAtomPlayer_SetHcaMxMixerId(NativeHandle, mixerId);
		}

		/// <summary>ASRラックIDの指定</summary>
		/// <param name="rackId">ラックID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスの出力先のラックIDを指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は停止中のプレーヤーに対してのみ実行可能です。
		/// </para>
		/// </remarks>
		public void SetAsrRackId(Int32 rackId)
		{
			NativeMethods.criAtomPlayer_SetAsrRackId(NativeHandle, rackId);
		}

		/// <summary>RawPCMフォーマットの指定</summary>
		/// <param name="pcmFormat">RawPCMのデータフォーマット</param>
		/// <param name="numChannels">チャンネル数</param>
		/// <param name="samplingRate">サンプリングレート</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// RawPCMのデータフォーマット情報を指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は <see cref="CriAtomPlayer.CreateRawPcmPlayer"/>
		/// 関数で作成されたプレーヤーに対してのみ効果があります。
		/// （他の関数で作成されたプレーヤーに対しては、何の効果もありません。）
		/// 本関数は停止中のプレーヤーに対してのみ実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateRawPcmPlayer"/>
		public void SetRawPcmFormat(CriAtom.PcmFormat pcmFormat, Int32 numChannels, Int32 samplingRate)
		{
			NativeMethods.criAtomPlayer_SetRawPcmFormat(NativeHandle, pcmFormat, numChannels, samplingRate);
		}

		/// <summary>データ要求コールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// 次に再生するデータを指定するためのコールバック関数です。
		/// 複数の音声データをシームレスに連結して再生する際に使用します。
		/// コールバック関数の登録には <see cref="CriAtomPlayer.SetDataRequestCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、Atomプレーヤーが連結再生用のデータを要求するタイミングで
		/// 実行されます。
		/// （前回のデータを読み込み終えて、次に再生すべきデータを要求するタイミングで
		/// コールバック関数が実行されます。）
		/// 本関数内で <see cref="CriAtomPlayer.SetData"/> 関数等を用いてAtomプレーヤーにデータをセットすると、
		/// セットされたデータは現在再生中のデータに続いてシームレスに連結されて再生されます。
		/// また、本関数内で <see cref="CriAtomPlayer.SetPreviousDataAgain"/> 関数を実行することで、
		/// 同一データを繰り返し再生し続けることも可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数内でデータを指定しなかった場合、現在のデータを再生し終えた時点で、
		/// Atomプレーヤーのステータスが <see cref="CriAtomPlayer.Status.Playend"/> に遷移します。
		/// タイミング等の問題により、データを指定することができないが、ステータスを
		/// <see cref="CriAtomPlayer.Status.Playend"/> に遷移させたくない場合には、コールバック関数内で
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行してください。
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行することで、約1V後に再度データ要求
		/// コールバック関数が呼び出されます。（コールバック処理をリトライ可能。）
		/// ただし、 <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行した場合、再生が途切れる
		/// （連結箇所に一定時間無音が入る）可能性があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetDataRequestCallback"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetPreviousDataAgain"/>
		/// <seealso cref="CriAtomPlayer.DeferCallback"/>
		public unsafe class DataRequestCbFunc : NativeCallbackBase<DataRequestCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>Atomプレーヤーオブジェクト</summary>
				public IntPtr player { get; }

				internal Arg(IntPtr player)
				{
					this.player = player;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr player) =>
				InvokeCallbackInternal(obj, new(player));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr player);
			static NativeDelegate callbackDelegate = null;
#endif
			internal DataRequestCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ステータス変更コールバック関数の登録</summary>
		/// <param name="func">ステータス変更コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ステータス変更コールバック関数を登録します。
		/// 登録したコールバック関数は、Atomプレーヤーのステータスが更新されるタイミングで
		/// 実行されます。
		/// 変更されたステータスについては、コールバック関数の引数として渡されるAtomプレーヤー
		/// オブジェクトに対し、 <see cref="CriAtomPlayer.GetStatus"/> 関数を実行することで取得可能です。
		/// ステータス変更コールバックを利用することで、Atomプレーヤーのステータス変更に
		/// 合わせて特定の処理を行うことが可能になります。
		/// </para>
		/// <para>
		/// 備考:
		/// 厳密には、ステータス遷移～コールバック関数実行までの間に他の処理が割り込みで動作する
		/// 余地があるため、ステータス遷移とコールバック関数実行のタイミングがズレる可能性があります。
		/// </para>
		/// <para>
		/// 注意:
		/// ステータス変更コールバック関数内で長時間処理をブロックすると、音切れ等の問題
		/// が発生しますので、ご注意ください。
		/// ステータス変更コールバック関数を抜けるまでは、Atomプレーヤーのステータスが
		/// 変更されることはありません。
		/// そのため、ステータス変更コールバック関数内でAtomプレーヤーのステータス遷移を
		/// 待つ処理を行うと、デッドロックが発生し、処理が先に進まなくなります。
		/// コールバック関数内でAtomプレーヤーを破棄しないでください。
		/// コールバックを抜けた後も、しばらくの間はサーバー処理内で当該オブジェクトのリソース
		/// が参照されるため、アクセス違反等の重大な問題が発生する可能性があります。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.GetStatus"/>
		public unsafe void SetStatusChangeCallback(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtomPlayer_SetStatusChangeCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetStatusChangeCallbackInternal(IntPtr func, IntPtr obj) => SetStatusChangeCallback((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)func, obj);
		CriAtomPlayer.StatusChangeCbFunc _statusChangeCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetStatusChangeCallback" />
		public CriAtomPlayer.StatusChangeCbFunc StatusChangeCallback => _statusChangeCallback ?? (_statusChangeCallback = new CriAtomPlayer.StatusChangeCbFunc(SetStatusChangeCallbackInternal));

		/// <summary>ステータス変更コールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// Atomプレーヤーのステータスが変更されるタイミングで実行されるコールバック関数です。
		/// コールバック関数の登録には <see cref="CriAtomPlayer.SetStatusChangeCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、Atomプレーヤーのステータスが更新されるタイミングで
		/// 実行されます。
		/// 変更されたステータスについては、引数で渡されるAtomプレーヤーオブジェクト（player）に対し、
		/// <see cref="CriAtomPlayer.GetStatus"/> 関数を実行することで取得可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 厳密には、ステータス遷移～コールバック関数実行までの間に他の処理が割り込みで動作する
		/// 余地があるため、ステータス遷移とコールバック関数実行のタイミングがズレる可能性があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetStatusChangeCallback"/>
		/// <seealso cref="CriAtomPlayer.GetStatus"/>
		public unsafe class StatusChangeCbFunc : NativeCallbackBase<StatusChangeCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>Atomプレーヤーオブジェクト</summary>
				public IntPtr player { get; }

				internal Arg(IntPtr player)
				{
					this.player = player;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr player) =>
				InvokeCallbackInternal(obj, new(player));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr player);
			static NativeDelegate callbackDelegate = null;
#endif
			internal StatusChangeCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>パラメーター変更コールバック関数の登録</summary>
		/// <param name="func">パラメーター変更コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラメーター変更コールバック関数を登録します。
		/// 登録したコールバック関数は、Atomプレーヤーのパラメーターが更新されるタイミングで実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// パラメーター変更コールバック関数内で長時間処理をブロックすると、音切れ等の問題
		/// が発生しますので、ご注意ください。
		/// コールバック関数内でAtomプレーヤーを破棄しないでください。
		/// コールバックを抜けた後も、しばらくの間はサーバー処理内で当該オブジェクトのリソース
		/// が参照されるため、アクセス違反等の重大な問題が発生する可能性があります。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		public unsafe void SetParameterChangeCallback(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, CriAtom.ParameterId, Single, void> func, IntPtr obj)
		{
			NativeMethods.criAtomPlayer_SetParameterChangeCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetParameterChangeCallbackInternal(IntPtr func, IntPtr obj) => SetParameterChangeCallback((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, CriAtom.ParameterId, Single, void>)func, obj);
		CriAtomPlayer.ParameterChangeCbFunc _parameterChangeCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetParameterChangeCallback" />
		public CriAtomPlayer.ParameterChangeCbFunc ParameterChangeCallback => _parameterChangeCallback ?? (_parameterChangeCallback = new CriAtomPlayer.ParameterChangeCbFunc(SetParameterChangeCallbackInternal));

		/// <summary>パラメーター変更コールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// Atomプレーヤーのパラメーターが変更されるタイミングで実行されるコールバック関数です。
		/// コールバック関数の登録には <see cref="CriAtomPlayer.SetParameterChangeCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、Atomプレーヤーのパラメーターが更新されるタイミングで実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetParameterChangeCallback"/>
		public unsafe class ParameterChangeCbFunc : NativeCallbackBase<ParameterChangeCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>Atomプレーヤーオブジェクト</summary>
				public IntPtr player { get; }
				/// <summary>パラメーターID</summary>
				public CriAtom.ParameterId id { get; }
				/// <summary>パラメーター値</summary>
				public Single value { get; }

				internal Arg(IntPtr player, CriAtom.ParameterId id, Single value)
				{
					this.player = player;
					this.id = id;
					this.value = value;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr player, CriAtom.ParameterId id, Single value) =>
				InvokeCallbackInternal(obj, new(player, id, value));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr player, CriAtom.ParameterId id, Single value);
			static NativeDelegate callbackDelegate = null;
#endif
			internal ParameterChangeCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, CriAtom.ParameterId, Single, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ロード要求コールバック関数の登録</summary>
		/// <param name="func">ロード要求コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ロード要求コールバック関数の登録を行います。
		/// ロード要求コールバックは、Atomプレーヤーのファイルロード状態を監視する際に使用します。
		/// （デバッグ目的の関数なので、通常本関数を使用する必要はありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// ロード要求コールバック関数内で長時間処理をブロックすると、音切れ等の問題が
		/// 発生しますので、ご注意ください。
		/// コールバック関数内でAtomプレーヤーを破棄しないでください。
		/// コールバックを抜けた後も、しばらくの間はサーバー処理内で当該オブジェクトのリソース
		/// が参照されるため、アクセス違反等の重大な問題が発生する可能性があります。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.LoadRequestCbFunc"/>
		public unsafe void SetLoadRequestCallback(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, NativeString, Int64, Int64, void> func, IntPtr obj)
		{
			NativeMethods.criAtomPlayer_SetLoadRequestCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetLoadRequestCallbackInternal(IntPtr func, IntPtr obj) => SetLoadRequestCallback((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, NativeString, Int64, Int64, void>)func, obj);
		CriAtomPlayer.LoadRequestCbFunc _loadRequestCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetLoadRequestCallback" />
		public CriAtomPlayer.LoadRequestCbFunc LoadRequestCallback => _loadRequestCallback ?? (_loadRequestCallback = new CriAtomPlayer.LoadRequestCbFunc(SetLoadRequestCallbackInternal));

		/// <summary>ロード要求コールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// Atomプレーヤーのファイルロード状態を監視するための、デバッグ用のコールバック関数です。
		/// コールバック関数の登録には <see cref="CriAtomPlayer.SetLoadRequestCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、Atomプレーヤーが音声データのロード要求を発行するタイミングで実行されます。
		/// </para>
		/// <para>備考:</para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetLoadRequestCallback"/>
		public unsafe class LoadRequestCbFunc : NativeCallbackBase<LoadRequestCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ファイルロード先バインダー</summary>
				public IntPtr binder { get; }
				/// <summary>ファイルパス</summary>
				public NativeString path { get; }
				/// <summary>ロード開始位置</summary>
				public Int64 offset { get; }
				/// <summary>ロード要求サイズ</summary>
				public Int64 length { get; }

				internal Arg(IntPtr binder, NativeString path, Int64 offset, Int64 length)
				{
					this.binder = binder;
					this.path = path;
					this.offset = offset;
					this.length = length;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr binder, NativeString path, Int64 offset, Int64 length) =>
				InvokeCallbackInternal(obj, new(binder, path, offset, length));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr binder, NativeString path, Int64 offset, Int64 length);
			static NativeDelegate callbackDelegate = null;
#endif
			internal LoadRequestCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, NativeString, Int64, Int64, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>プレーヤー作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// 各コーデック毎の CriAtomXXXPlayerConfig 内の context に指定します。
		/// ※ XXX はコーデック名
		/// </para>
		/// <para>
		/// 注意:
		/// 本構造体は <see cref="CriAtom.SoundRendererType.Asr"/> を使用した場合のみ参照されます。
		/// 将来的にメンバが増える可能性があるため、
		/// <see cref="CriAtomPlayer.SetDefaultConfigASR"/> メソッドで必ず構造体を初期化してください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		public unsafe partial struct ConfigASR
		{
			/// <summary>センド可能バス数</summary>
			public Int32 maxRoutes;

		}
		/// <summary>ループ回数制限なし</summary>
		public const Int32 NoLoopLimitation = (-1);
		/// <summary>ループ情報を無視</summary>
		public const Int32 IgnoreLoop = (-2);
		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomPlayer(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomPlayer other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomPlayer a, CriAtomPlayer b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomPlayer a, CriAtomPlayer b) =>
			!(a == b);

	}
}