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
	/// <summary>ボイスプールオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// ボイスプールを制御するためのオブジェクトです。
	/// <see cref="CriAtomExVoicePool.AllocateStandardVoicePool"/> 関数等でボイスプールを作成した際、
	/// 関数の戻り値として返されます。
	/// ボイスプールオブジェクトは、ボイスプールの情報取得や、ボイスプールを解放する
	/// 際に使用します。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExVoicePool.AllocateStandardVoicePool"/>
	/// <seealso cref="CriAtomExVoicePool.Dispose"/>
	public partial class CriAtomExVoicePool : IDisposable
	{
		/// <summary>ADXボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">ADXボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateAdxVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomEx.AdxVoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AdxVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateAdxVoicePool"/>
		public static unsafe void SetDefaultConfigForAdxVoicePool(out CriAtomEx.AdxVoicePoolConfig pConfig)
		{
			fixed (CriAtomEx.AdxVoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForAdxVoicePool_(pConfigPtr);
		}

		/// <summary>AIFFボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">AIFFボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateAiffVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomEx.AiffVoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AiffVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateAiffVoicePool"/>
		public static unsafe void SetDefaultConfigForAiffVoicePool(out CriAtomEx.AiffVoicePoolConfig pConfig)
		{
			fixed (CriAtomEx.AiffVoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForAiffVoicePool_(pConfigPtr);
		}

		/// <summary>ピッチシフタDSPのアタッチ用コンフィグにデフォルト値をセット</summary>
		/// <param name="pConfig">ピッチシフタDSPのアタッチ用コンフィグへのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ピッチシフタDSPのアタッチ用コンフィグ（ <see cref="CriAtomEx.DspPitchShifterConfig"/> ）に、
		/// デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.DspPitchShifterConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AttachDspPitchShifter"/>
		public static unsafe void SetDefaultConfigForDspPitchShifter(out CriAtomEx.DspPitchShifterConfig pConfig)
		{
			fixed (CriAtomEx.DspPitchShifterConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForDspPitchShifter_(pConfigPtr);
		}

		/// <summary>タイムストレッチDSPのアタッチ用コンフィグにデフォルト値をセット</summary>
		/// <param name="pConfig">タイムストレッチDSPのアタッチ用コンフィグへのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// タイムストレッチDSPのアタッチ用コンフィグ（ <see cref="CriAtomEx.DspTimeStretchConfig"/> ）に、
		/// デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.DspTimeStretchConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AttachDspTimeStretch"/>
		public static unsafe void SetDefaultConfigForDspTimeStretch(out CriAtomEx.DspTimeStretchConfig pConfig)
		{
			fixed (CriAtomEx.DspTimeStretchConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForDspTimeStretch_(pConfigPtr);
		}

		/// <summary>HCA-MXボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">HCA-MXボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExHcaMx.VoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExHcaMx.VoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/>
		public static unsafe void SetDefaultConfigForHcaMxVoicePool(out CriAtomExHcaMx.VoicePoolConfig pConfig)
		{
			fixed (CriAtomExHcaMx.VoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForHcaMxVoicePool_(pConfigPtr);
		}

		/// <summary>HCAボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">HCAボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateHcaVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomEx.HcaVoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.HcaVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateHcaVoicePool"/>
		public static unsafe void SetDefaultConfigForHcaVoicePool(out CriAtomEx.HcaVoicePoolConfig pConfig)
		{
			fixed (CriAtomEx.HcaVoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForHcaVoicePool_(pConfigPtr);
		}

		/// <summary>インストゥルメントボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">RawPCMボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ ::  <see cref="CriAtomEx.InstrumentVoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// <para>
		/// 注意:
		/// デフォルト値のままではプールの作成に失敗します。
		/// ユーザが登録したインターフェースのインストゥルメント名を設定する必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.InstrumentVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/>
		public static unsafe void SetDefaultConfigForInstrumentVoicePool(out CriAtomEx.InstrumentVoicePoolConfig pConfig)
		{
			fixed (CriAtomEx.InstrumentVoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForInstrumentVoicePool_(pConfigPtr);
		}

		/// <summary>RawPCMボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">RawPCMボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateRawPcmVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomEx.RawPcmVoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.RawPcmVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateRawPcmVoicePool"/>
		public static unsafe void SetDefaultConfigForRawPcmVoicePool(out CriAtomEx.RawPcmVoicePoolConfig pConfig)
		{
			fixed (CriAtomEx.RawPcmVoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForRawPcmVoicePool_(pConfigPtr);
		}

		/// <summary>Waveボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">Waveボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateWaveVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomEx.WaveVoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.WaveVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateWaveVoicePool"/>
		public static unsafe void SetDefaultConfigForWaveVoicePool(out CriAtomEx.WaveVoicePoolConfig pConfig)
		{
			fixed (CriAtomEx.WaveVoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForWaveVoicePool_(pConfigPtr);
		}

		/// <summary>標準ボイスプール作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">標準ボイスプール作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExVoicePool.AllocateStandardVoicePool"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomEx.StandardVoicePoolConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.StandardVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateStandardVoicePool"/>
		public static unsafe void SetDefaultConfigForStandardVoicePool(out CriAtomEx.StandardVoicePoolConfig pConfig)
		{
			fixed (CriAtomEx.StandardVoicePoolConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomExVoicePool_SetDefaultConfigForStandardVoicePool_(pConfigPtr);
		}

		/// <summary>ボイスプールの列挙</summary>
		/// <param name="func">ボイスプールコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// アプリケーション中で作成したボイスプールを列挙します。
		/// 本関数を実行すると、第 1 引数（ func ）
		/// でセットされたコールバック関数がボイスプールの数分だけ呼び出されます。
		/// （ボイスプールオブジェクトが、引数としてコールバック関数に渡されます。）
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomExVoicePool.CbFunc"/> の説明をご参照ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.CbFunc"/>
		public static unsafe void EnumerateVoicePools(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExVoicePool_EnumerateVoicePools((IntPtr)func, obj);
		}

		/// <summary>ボイスプールコールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプールの列挙に使用する、コールバック関数の型です。
		/// <see cref="CriAtomExVoicePool.EnumerateVoicePools"/> 関数に本関数型のコールバック関数を登録することで、
		/// 作成済みボイスプールをコールバックで受け取ることが可能となります。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.EnumerateVoicePools"/>
		public unsafe class CbFunc : NativeCallbackBase<CbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ボイスプールオブジェクト</summary>
				public IntPtr pool { get; }

				internal Arg(IntPtr pool)
				{
					this.pool = pool;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr pool) =>
				InvokeCallbackInternal(obj, new(pool));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr pool);
			static NativeDelegate callbackDelegate = null;
#endif
			internal CbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>標準ボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">標準ボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 標準ボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateStandardVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateStandardVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプールの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.StandardVoicePoolConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForStandardVoicePool"/> メソッド使用時
		/// と同じパラメーター）でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateStandardVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForStandardVoicePool(in CriAtomEx.StandardVoicePoolConfig config)
		{
			fixed (CriAtomEx.StandardVoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForStandardVoicePool(configPtr);
		}

		/// <summary>標準ボイスプールの作成</summary>
		/// <param name="config">標準ボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// 標準ボイスプールを作成します。
		/// （標準ボイスは、ADXデータとHCAデータの両方の再生に対応したボイスです。）
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForStandardVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、ADXとHCAの再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでADXやHCAデータ（もしくはADXやHCAデータを含むキュー）の再生を行うと、
		/// AtomExプレーヤーは作成された標準ボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール作成時には、プール作成用コンフィグ構造体
		/// （ <see cref="CriAtomEx.StandardVoicePoolConfig"/> 構造体の num_voices ）
		/// で指定した数分のボイスが、ライブラリ内で作成されます。
		/// 作成するボイスの数が多いほど、同時に再生可能な音声の数は増えますが、
		/// 反面、使用するメモリは増加します。
		/// ボイスプール作成時には、ボイス数の他に、再生可能な音声のチャンネル数、
		/// サンプリング周波数、ストリーム再生の有無を指定します。
		/// ボイスプール作成時に指定する音声チャンネル数（ <see cref="CriAtomEx.StandardVoicePoolConfig"/>
		/// 構造体の player_config.max_channels ）は、ボイスプール内のボイスが再生できる
		/// 音声データのチャンネル数になります。
		/// チャンネル数を少なくすることで、ボイスプールの作成に必要なメモリサイズは
		/// 小さくなりますが、指定されたチャンネル数を越えるデータは再生できなくなります。
		/// 例えば、ボイスプールをモノラルで作成した場合、ステレオのデータは再生できません。
		/// （ステレオデータを再生する場合、AtomExプレーヤーは、ステレオが再生可能な
		/// ボイスプールからのみボイスを取得します。）
		/// ただし、ステレオのボイスプールを作成した場合、モノラルデータ再生時にステレオ
		/// ボイスプールのボイスが使用される可能性はあります。
		/// サンプリングレート（ <see cref="CriAtomEx.StandardVoicePoolConfig"/> 構造体の
		/// player_config.max_sampling_rate ）についても、値を下げることでもボイスプール
		/// に必要なメモリサイズは小さくすることが可能ですが、指定されたサンプリングレート
		/// を越えるデータは再生できなくなります。
		/// （指定されたサンプリングレート以下のデータのみが再生可能です。）
		/// ストリーミング再生の有無（<see cref="CriAtomEx.StandardVoicePoolConfig"/> 構造体の
		/// player_config.streaming_flag ）についても、オンメモリ再生のみのボイスプールは
		/// ストリーミング再生可能なボイスプールに比べ、サイズが小さくなります。
		/// 尚、AtomExプレーヤーがデータを再生した際に、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをボイスプール破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ストリーム再生用のボイスプールは、内部的にボイスの数分だけローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// ストリーム再生用のボイスプールを作成する場合、ボイス数分のローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ボイスプールの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.StandardVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForStandardVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateStandardVoicePool(in CriAtomEx.StandardVoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomEx.StandardVoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateStandardVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>ADXボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">ADXボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADXボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateAdxVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateAdxVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプールの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.AdxVoicePoolConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForAdxVoicePool"/> メソッド使用時と
		/// 同じパラメーター）でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateAdxVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForAdxVoicePool(in CriAtomEx.AdxVoicePoolConfig config)
		{
			fixed (CriAtomEx.AdxVoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForAdxVoicePool(configPtr);
		}

		/// <summary>ADXボイスプールの作成</summary>
		/// <param name="config">ADXボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// ADXボイスプールを作成します。
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForAdxVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、ADX再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでADXデータ（もしくはADXデータを含むキュー）の再生を行うと、
		/// AtomExプレーヤーは作成されたADXボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール作成時には、プール作成用コンフィグ構造体
		/// （ <see cref="CriAtomEx.AdxVoicePoolConfig"/> 構造体の num_voices ）
		/// で指定した数分のボイスが、ライブラリ内で作成されます。
		/// 作成するボイスの数が多いほど、同時に再生可能なADX音声の数は増えますが、
		/// 反面、使用するメモリは増加します。
		/// ボイスプール作成時には、ボイス数の他に、再生可能な音声のチャンネル数、
		/// サンプリング周波数、ストリーム再生の有無を指定します。
		/// ボイスプール作成時に指定する音声チャンネル数（ <see cref="CriAtomEx.AdxVoicePoolConfig"/>
		/// 構造体の player_config.max_channels ）は、ボイスプール内のボイスが再生できる
		/// 音声データのチャンネル数になります。
		/// チャンネル数を少なくすることで、ボイスプールの作成に必要なメモリサイズは
		/// 小さくなりますが、指定されたチャンネル数を越えるデータは再生できなくなります。
		/// 例えば、ボイスプールをモノラルで作成した場合、ステレオのデータは再生できません。
		/// （ステレオデータを再生する場合、AtomExプレーヤーは、ステレオが再生可能な
		/// ボイスプールからのみボイスを取得します。）
		/// ただし、ステレオのボイスプールを作成した場合、モノラルデータ再生時にステレオ
		/// ボイスプールのボイスが使用される可能性はあります。
		/// サンプリングレート（ <see cref="CriAtomEx.AdxVoicePoolConfig"/> 構造体の
		/// player_config.max_sampling_rate ）についても、値を下げることでもボイスプール
		/// に必要なメモリサイズは小さくすることが可能ですが、指定されたサンプリングレート
		/// を越えるデータは再生できなくなります。
		/// （指定されたサンプリングレート以下のデータのみが再生可能です。）
		/// ストリーミング再生の有無（<see cref="CriAtomEx.AdxVoicePoolConfig"/> 構造体の
		/// player_config.streaming_flag ）についても、オンメモリ再生のみのボイスプールは
		/// ストリーミング再生可能なボイスプールに比べ、サイズが小さくなります。
		/// 尚、AtomExプレーヤーがデータを再生した際に、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをボイスプール破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ストリーム再生用のボイスプールは、内部的にボイスの数分だけローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// ストリーム再生用のボイスプールを作成する場合、ボイス数分のローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ボイスプールの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AdxVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForAdxVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateAdxVoicePool(in CriAtomEx.AdxVoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomEx.AdxVoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateAdxVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>HCAボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">HCAボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCAボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateHcaVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateHcaVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプールの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.HcaVoicePoolConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForHcaVoicePool"/> メソッド使用時と
		/// 同じパラメーター）でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateHcaVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForHcaVoicePool(in CriAtomEx.HcaVoicePoolConfig config)
		{
			fixed (CriAtomEx.HcaVoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForHcaVoicePool(configPtr);
		}

		/// <summary>HCAボイスプールの作成</summary>
		/// <param name="config">HCAボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// HCAボイスプールを作成します。
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForHcaVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、HCA再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでHCAデータ（もしくはHCAデータを含むキュー）の再生を行うと、
		/// AtomExプレーヤーは作成されたHCAボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール作成時には、プール作成用コンフィグ構造体
		/// （ <see cref="CriAtomEx.HcaVoicePoolConfig"/> 構造体の num_voices ）
		/// で指定した数分のボイスが、ライブラリ内で作成されます。
		/// 作成するボイスの数が多いほど、同時に再生可能なHCA音声の数は増えますが、
		/// 反面、使用するメモリは増加します。
		/// ボイスプール作成時には、ボイス数の他に、再生可能な音声のチャンネル数、
		/// サンプリング周波数、ストリーム再生の有無を指定します。
		/// ボイスプール作成時に指定する音声チャンネル数（ <see cref="CriAtomEx.HcaVoicePoolConfig"/>
		/// 構造体の player_config.max_channels ）は、ボイスプール内のボイスが再生できる
		/// 音声データのチャンネル数になります。
		/// チャンネル数を少なくすることで、ボイスプールの作成に必要なメモリサイズは
		/// 小さくなりますが、指定されたチャンネル数を越えるHCAデータは再生できなくなります。
		/// 例えば、ボイスプールをモノラルで作成した場合、ステレオのHCAデータは再生できません。
		/// （ステレオHCAデータを再生する場合、AtomExプレーヤーは、ステレオHCAが再生可能な
		/// ボイスプールからのみボイスを取得します。）
		/// ただし、ステレオのボイスプールを作成した場合、モノラルデータ再生時にステレオ
		/// ボイスプールのボイスが使用される可能性はあります。
		/// サンプリングレート（ <see cref="CriAtomEx.HcaVoicePoolConfig"/> 構造体の
		/// player_config.max_sampling_rate ）についても、値を下げることでもボイスプール
		/// に必要なメモリサイズは小さくすることが可能ですが、指定されたサンプリングレート
		/// を越えるHCAデータは再生できなくなります。
		/// （指定されたサンプリングレート以下のHCAデータのみが再生可能です。）
		/// ストリーミング再生の有無（<see cref="CriAtomEx.HcaVoicePoolConfig"/> 構造体の
		/// player_config.streaming_flag ）についても、オンメモリ再生のみのボイスプールは
		/// ストリーミング再生可能なボイスプールに比べ、サイズが小さくなります。
		/// 尚、AtomExプレーヤーがデータを再生した際に、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをボイスプール破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ストリーム再生用のボイスプールは、内部的にボイスの数分だけローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// ストリーム再生用のボイスプールを作成する場合、ボイス数分のローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ボイスプールの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.HcaVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForHcaVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateHcaVoicePool(in CriAtomEx.HcaVoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomEx.HcaVoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateHcaVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>HCA-MXボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">HCA-MXボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプールの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomExHcaMx.VoicePoolConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForHcaMxVoicePool"/> メソッド使用時と
		/// 同じパラメーター）でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはHCA-MX初期化時（ <see cref="CriAtomExHcaMx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、HCA-MXを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForHcaMxVoicePool(in CriAtomExHcaMx.VoicePoolConfig config)
		{
			fixed (CriAtomExHcaMx.VoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForHcaMxVoicePool(configPtr);
		}

		/// <summary>HCA-MXボイスプールの作成</summary>
		/// <param name="config">HCA-MXボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// HCA-MXボイスプールを作成します。
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForHcaMxVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、HCA-MX再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでHCA-MXデータ（もしくはHCA-MXデータを含むキュー）の再生を行うと、
		/// AtomExプレーヤーは作成されたHCA-MXボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール作成時には、プール作成用コンフィグ構造体
		/// （ <see cref="CriAtomExHcaMx.VoicePoolConfig"/> 構造体の num_voices ）
		/// で指定した数分のボイスが、ライブラリ内で作成されます。
		/// 作成するボイスの数が多いほど、同時に再生可能なHCA-MX音声の数は増えますが、
		/// 反面、使用するメモリは増加します。
		/// ボイスプール作成時には、ボイス数の他に、再生可能な音声のチャンネル数、
		/// サンプリング周波数、ストリーム再生の有無を指定します。
		/// ボイスプール作成時に指定する音声チャンネル数（ <see cref="CriAtomExHcaMx.VoicePoolConfig"/>
		/// 構造体の player_config.max_channels ）は、ボイスプール内のボイスが再生できる
		/// 音声データのチャンネル数になります。
		/// チャンネル数を少なくすることで、ボイスプールの作成に必要なメモリサイズは
		/// 小さくなりますが、指定されたチャンネル数を越えるHCA-MXデータは再生できなくなります。
		/// 例えば、ボイスプールをモノラルで作成した場合、ステレオのHCA-MXデータは再生できません。
		/// （ステレオHCA-MXデータを再生する場合、AtomExプレーヤーは、ステレオHCA-MXが再生可能な
		/// ボイスプールからのみボイスを取得します。）
		/// ただし、ステレオのボイスプールを作成した場合、モノラルデータ再生時にステレオ
		/// ボイスプールのボイスが使用される可能性はあります。
		/// サンプリングレート（ <see cref="CriAtomExHcaMx.VoicePoolConfig"/> 構造体の
		/// player_config.max_sampling_rate ）についても、値を下げることでもボイスプール
		/// に必要なメモリサイズは小さくすることが可能ですが、指定されたサンプリングレート
		/// 以外のHCA-MXデータは再生できなくなります。
		/// （他のボイスプールと異なり、同一サンプリングレートのデータのみが再生可能です。）
		/// ストリーミング再生の有無（<see cref="CriAtomExHcaMx.VoicePoolConfig"/> 構造体の
		/// player_config.streaming_flag ）についても、オンメモリ再生のみのボイスプールは
		/// ストリーミング再生可能なボイスプールに比べ、サイズが小さくなります。
		/// 尚、AtomExプレーヤーがデータを再生した際に、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、必ずHCA-MXの初期化処理（ <see cref="CriAtomExHcaMx.Initialize"/> 関数）
		/// を実行しておく必要があります。
		/// また、 <see cref="CriAtomExHcaMx.Initialize"/> 関数実行時に指定した数以上のHCA-MXデータは再生できません。
		/// HCA-MXボイスプールを作成する際には、 <see cref="CriAtomExHcaMx.VoicePoolConfig"/> 構造体の num_voices
		/// の値が、HCA-MX初期化時に指定する <see cref="CriAtomExHcaMx.Config"/> 構造体の max_voices の数を超えないよう、
		/// ご注意ください。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをボイスプール破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ストリーム再生用のボイスプールは、内部的にボイスの数分だけローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// ストリーム再生用のボイスプールを作成する場合、ボイス数分のローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ボイスプールの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExHcaMx.VoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForHcaMxVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateHcaMxVoicePool(in CriAtomExHcaMx.VoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomExHcaMx.VoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateHcaMxVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>Waveボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">Waveボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Waveボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateWaveVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateWaveVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプールの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.WaveVoicePoolConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForWaveVoicePool"/> メソッド使用時と
		/// 同じパラメーター）でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateWaveVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForWaveVoicePool(in CriAtomEx.WaveVoicePoolConfig config)
		{
			fixed (CriAtomEx.WaveVoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForWaveVoicePool(configPtr);
		}

		/// <summary>Waveボイスプールの作成</summary>
		/// <param name="config">Waveボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// Waveボイスプールを作成します。
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForWaveVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、Wave再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでWaveデータ（もしくはWaveデータを含むキュー）の再生を行うと、
		/// AtomExプレーヤーは作成されたWaveボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール作成時には、プール作成用コンフィグ構造体
		/// （ <see cref="CriAtomEx.WaveVoicePoolConfig"/> 構造体の num_voices ）
		/// で指定した数分のボイスが、ライブラリ内で作成されます。
		/// 作成するボイスの数が多いほど、同時に再生可能なWave音声の数は増えますが、
		/// 反面、使用するメモリは増加します。
		/// ボイスプール作成時には、ボイス数の他に、再生可能な音声のチャンネル数、
		/// サンプリング周波数、ストリーム再生の有無を指定します。
		/// ボイスプール作成時に指定する音声チャンネル数（ <see cref="CriAtomEx.WaveVoicePoolConfig"/>
		/// 構造体の player_config.max_channels ）は、ボイスプール内のボイスが再生できる
		/// 音声データのチャンネル数になります。
		/// チャンネル数を少なくすることで、ボイスプールの作成に必要なメモリサイズは
		/// 小さくなりますが、指定されたチャンネル数を越えるWaveデータは再生できなくなります。
		/// 例えば、ボイスプールをモノラルで作成した場合、ステレオのWaveデータは再生できません。
		/// （ステレオWaveデータを再生する場合、AtomExプレーヤーは、ステレオWaveが再生可能な
		/// ボイスプールからのみボイスを取得します。）
		/// ただし、ステレオのボイスプールを作成した場合、モノラルデータ再生時にステレオ
		/// ボイスプールのボイスが使用される可能性はあります。
		/// サンプリングレート（ <see cref="CriAtomEx.WaveVoicePoolConfig"/> 構造体の
		/// player_config.max_sampling_rate ）についても、値を下げることでもボイスプール
		/// に必要なメモリサイズは小さくすることが可能ですが、指定されたサンプリングレート
		/// を越えるWaveデータは再生できなくなります。
		/// （指定されたサンプリングレート以下のWaveデータのみが再生可能です。）
		/// ストリーミング再生の有無（<see cref="CriAtomEx.WaveVoicePoolConfig"/> 構造体の
		/// player_config.streaming_flag ）についても、オンメモリ再生のみのボイスプールは
		/// ストリーミング再生可能なボイスプールに比べ、サイズが小さくなります。
		/// 尚、AtomExプレーヤーがデータを再生した際に、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをボイスプール破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ストリーム再生用のボイスプールは、内部的にボイスの数分だけローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// ストリーム再生用のボイスプールを作成する場合、ボイス数分のローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ボイスプールの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// 再生可能なフォーマットは、32bit以下の非圧縮PCMデータのみです。
		/// ループ再生を行う場合、ストリーム再生用の音声データについては、
		/// smplチャンクがdataチャンクよりも手前に配置されている必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.WaveVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForWaveVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateWaveVoicePool(in CriAtomEx.WaveVoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomEx.WaveVoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateWaveVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>AIFFボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">AIFFボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AIFFボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateAiffVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateAiffVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプールの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.AiffVoicePoolConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForAiffVoicePool"/> メソッド使用時と
		/// 同じパラメーター）でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateAiffVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForAiffVoicePool(in CriAtomEx.AiffVoicePoolConfig config)
		{
			fixed (CriAtomEx.AiffVoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForAiffVoicePool(configPtr);
		}

		/// <summary>AIFFボイスプールの作成</summary>
		/// <param name="config">AIFFボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// AIFFボイスプールを作成します。
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForAiffVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、AIFF再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでAIFFデータ（もしくはAIFFデータを含むキュー）の再生を行うと、
		/// AtomExプレーヤーは作成されたAIFFボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール作成時には、プール作成用コンフィグ構造体
		/// （ <see cref="CriAtomEx.AiffVoicePoolConfig"/> 構造体の num_voices ）
		/// で指定した数分のボイスが、ライブラリ内で作成されます。
		/// 作成するボイスの数が多いほど、同時に再生可能なAIFF音声の数は増えますが、
		/// 反面、使用するメモリは増加します。
		/// ボイスプール作成時には、ボイス数の他に、再生可能な音声のチャンネル数、
		/// サンプリング周波数、ストリーム再生の有無を指定します。
		/// ボイスプール作成時に指定する音声チャンネル数（ <see cref="CriAtomEx.AiffVoicePoolConfig"/>
		/// 構造体の player_config.max_channels ）は、ボイスプール内のボイスが再生できる
		/// 音声データのチャンネル数になります。
		/// チャンネル数を少なくすることで、ボイスプールの作成に必要なメモリサイズは
		/// 小さくなりますが、指定されたチャンネル数を越えるAIFFデータは再生できなくなります。
		/// 例えば、ボイスプールをモノラルで作成した場合、ステレオのAIFFデータは再生できません。
		/// （ステレオAIFFデータを再生する場合、AtomExプレーヤーは、ステレオAIFFが再生可能な
		/// ボイスプールからのみボイスを取得します。）
		/// ただし、ステレオのボイスプールを作成した場合、モノラルデータ再生時にステレオ
		/// ボイスプールのボイスが使用される可能性はあります。
		/// サンプリングレート（ <see cref="CriAtomEx.AiffVoicePoolConfig"/> 構造体の
		/// player_config.max_sampling_rate ）についても、値を下げることでもボイスプール
		/// に必要なメモリサイズは小さくすることが可能ですが、指定されたサンプリングレート
		/// を越えるAIFFデータは再生できなくなります。
		/// （指定されたサンプリングレート以下のAIFFデータのみが再生可能です。）
		/// ストリーミング再生の有無（<see cref="CriAtomEx.AiffVoicePoolConfig"/> 構造体の
		/// player_config.streaming_flag ）についても、オンメモリ再生のみのボイスプールは
		/// ストリーミング再生可能なボイスプールに比べ、サイズが小さくなります。
		/// 尚、AtomExプレーヤーがデータを再生した際に、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをボイスプール破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ストリーム再生用のボイスプールは、内部的にボイスの数分だけローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// ストリーム再生用のボイスプールを作成する場合、ボイス数分のローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ボイスプールの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// 再生可能なフォーマットは、32bit以下の非圧縮PCMデータのみです。
		/// ループ再生を行う場合、ストリーム再生用の音声データについては、
		/// INSTチャンクがSSNDチャンクよりも手前に配置されている必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AiffVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForAiffVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateAiffVoicePool(in CriAtomEx.AiffVoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomEx.AiffVoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateAiffVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>RawPCMボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">RawPCMボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// RawPCMボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateRawPcmVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateRawPcmVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプールの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.RawPcmVoicePoolConfig"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForRawPcmVoicePool"/> メソッド使用時と
		/// 同じパラメーター）でワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateRawPcmVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForRawPcmVoicePool(in CriAtomEx.RawPcmVoicePoolConfig config)
		{
			fixed (CriAtomEx.RawPcmVoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForRawPcmVoicePool(configPtr);
		}

		/// <summary>RawPCMボイスプールの作成</summary>
		/// <param name="config">RawPCMボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// RawPCMボイスプールを作成します。
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForRawPcmVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、RawPCM再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでRawPCMデータ（もしくはRawPCMデータを含むキュー）の再生を行うと、
		/// AtomExプレーヤーは作成されたRawPCMボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール作成時には、プール作成用コンフィグ構造体
		/// （ <see cref="CriAtomEx.RawPcmVoicePoolConfig"/> 構造体の num_voices ）
		/// で指定した数分のボイスが、ライブラリ内で作成されます。
		/// 作成するボイスの数が多いほど、同時に再生可能なRawPCM音声の数は増えますが、
		/// 反面、使用するメモリは増加します。
		/// ボイスプール作成時には、ボイス数の他に、再生可能な音声のチャンネル数、
		/// サンプリング周波数、ストリーム再生の有無を指定します。
		/// ボイスプール作成時に指定する音声チャンネル数（ <see cref="CriAtomEx.RawPcmVoicePoolConfig"/>
		/// 構造体の player_config.max_channels ）は、実際に供給するRawPCMのフォーマットの
		/// チャンネル数を指定します。
		/// サンプリングレート（ <see cref="CriAtomEx.RawPcmVoicePoolConfig"/> 構造体の
		/// player_config.max_sampling_rate ）についても、実際に供給するRawPCMの
		/// フォーマットのサンプリングレートを指定します。
		/// 尚、AtomExプレーヤーがデータを再生した際に、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをボイスプール破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// ストリーム再生用のボイスプールは、内部的にボイスの数分だけローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// ストリーム再生用のボイスプールを作成する場合、ボイス数分のローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ボイスプールの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.RawPcmVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForRawPcmVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateRawPcmVoicePool(in CriAtomEx.RawPcmVoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomEx.RawPcmVoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateRawPcmVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>インストゥルメントボイスプールの作成</summary>
		/// <param name="config">インストゥルメントボイスプール作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>インストゥルメントボイスプールオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明：
		/// インストゥルメントボイスプールを作成します。
		/// ボイスプールを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExVoicePool.CalculateWorkSizeForInstrumentVoicePool"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 本関数を実行することで、インストゥルメントトラックの再生が可能なボイスがプールされます。
		/// AtomExプレーヤーでインストゥルメントトラックの再生を行うと、
		/// AtomExプレーヤーは作成されたインストゥルメントボイスプールからボイスを取得し、再生を行います。
		/// ボイスプールの作成に成功すると、戻り値としてボイスプールオブジェクトが返されます。
		/// アプリケーション終了時には、作成したボイスプールを <see cref="CriAtomExVoicePool.Dispose"/>
		/// 関数で破棄する必要があります。
		/// ボイスプールの作成に失敗すると、本関数はnullを返します。
		/// ボイスプールの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.InstrumentVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForInstrumentVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.Dispose"/>
		public static unsafe CriAtomExVoicePool AllocateInstrumentVoicePool(in CriAtomEx.InstrumentVoicePoolConfig config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtomEx.InstrumentVoicePoolConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomExVoicePool_AllocateInstrumentVoicePool(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExVoicePool(handle);
		}

		/// <summary>ボイスプールの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 作成済みのボイスプールを破棄します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// ボイスプール作成時に確保されたメモリ領域が解放されます。
		/// （ボイスプール作成時にワーク領域を渡した場合、本関数実行後であれば
		/// ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 音声再生中にボイスプールを破棄した場合、本関数内で再生停止を待ってから
		/// リソースの解放が行われます。
		/// （ファイルから再生している場合は、さらに読み込み完了待ちが行われます。）
		/// そのため、本関数内で処理が長時間（数フレーム）ブロックされる可能性があります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateStandardVoicePool"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomExVoicePool_Free(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomExVoicePool() => Dispose();
#pragma warning restore 1591

		/// <summary>全てのボイスプールを破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 作成済みのボイスプールを全て破棄します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// ボイスプール作成時に確保されたメモリ領域が解放されます。
		/// （ボイスプール作成時にワーク領域を渡した場合、本関数実行後であれば
		/// ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 音声再生中にボイスプールを破棄した場合、本関数内で再生停止を待ってから
		/// リソースの解放が行われます。
		/// （ファイルから再生している場合は、さらに読み込み完了待ちが行われます。）
		/// そのため、本関数内で処理が長時間（数フレーム）ブロックされる可能性があります。
		/// ボイスプールの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateStandardVoicePool"/>
		public static void FreeAll()
		{
			NativeMethods.criAtomExVoicePool_FreeAll();
		}

		/// <summary>ボイスの使用状況の取得</summary>
		/// <param name="curNum">現在使用中のボイス数</param>
		/// <param name="limit">利用可能なボイスの最大数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプール内のボイスのうち、現在使用中のボイスの数、および利用可能な
		/// 最大ボイス数（＝プール作成時に指定した max_voices の数）を取得します。
		/// </para>
		/// </remarks>
		public unsafe void GetNumUsedVoices(out Int32 curNum, out Int32 limit)
		{
			fixed (Int32* curNumPtr = &curNum)
			fixed (Int32* limitPtr = &limit)
				NativeMethods.criAtomExVoicePool_GetNumUsedVoices(NativeHandle, curNumPtr, limitPtr);
		}

		/// <summary>プレーヤーオブジェクトの取得</summary>
		/// <param name="index">プレーヤーインデックス</param>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプール内で作成されたAtomプレーヤーオブジェクトを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は情報取得用途にのみ利用可能なデバッグ関数です。
		/// </para>
		/// </remarks>
		public CriAtomPlayer GetPlayerHandle(Int32 index)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExVoicePool_GetPlayerHandle(NativeHandle, index)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>インストゥルメントボイスプール作成用ワーク領域サイズの計算</summary>
		/// <param name="config">インストゥルメントボイスプール作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インストゥルメントボイスプールの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/> 関数でボイスプールを作成する際には、
		/// <see cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/>
		public static unsafe Int32 CalculateWorkSizeForInstrumentVoicePool(in CriAtomEx.InstrumentVoicePoolConfig config)
		{
			fixed (CriAtomEx.InstrumentVoicePoolConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForInstrumentVoicePool(configPtr);
		}

		/// <summary>DSPのデタッチ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプールに追加したDSPを取り外します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// <para>
		/// 注意:
		/// 備考:
		/// 現在、本関数を使用できないプラットフォームが存在します。
		/// </para>
		/// </remarks>
		public void DetachDsp()
		{
			NativeMethods.criAtomExVoicePool_DetachDsp(NativeHandle);
		}

		/// <summary>ピッチシフタDSPアタッチ用ワーク領域サイズの計算</summary>
		/// <param name="config">アタッチ用コンフィグ</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ピッチシフタDSPのアタッチに必要なワーク領域サイズを計算します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspPitchShifter"/>
		public static unsafe Int32 CalculateWorkSizeForDspPitchShifter(in CriAtomEx.DspPitchShifterConfig config)
		{
			fixed (CriAtomEx.DspPitchShifterConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForDspPitchShifter(configPtr);
		}

		/// <summary>ピッチシフタDSPのアタッチ</summary>
		/// <param name="config">アタッチ用コンフィグ</param>
		/// <param name="work">アタッチ用ワーク領域へのポインタ</param>
		/// <param name="workSize">アタッチ用ワーク領域のサイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプールにピッチシフタDSPを追加します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// <para>
		/// 注意:
		/// 備考:
		/// 現在、本関数を使用できないプラットフォームが存在します。
		/// </para>
		/// </remarks>
		public unsafe void AttachDspPitchShifter(in CriAtomEx.DspPitchShifterConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx.DspPitchShifterConfig* configPtr = &config)
				NativeMethods.criAtomExVoicePool_AttachDspPitchShifter(NativeHandle, configPtr, work, workSize);
		}

		/// <summary>タイムストレッチDSPアタッチ用ワーク領域サイズの計算</summary>
		/// <param name="config">アタッチ用コンフィグ</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// タイムストレッチDSPのアタッチに必要なワーク領域サイズを計算します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspTimeStretch"/>
		public static unsafe Int32 CalculateWorkSizeForDspTimeStretch(in CriAtomEx.DspTimeStretchConfig config)
		{
			fixed (CriAtomEx.DspTimeStretchConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForDspTimeStretch(configPtr);
		}

		/// <summary>タイムストレッチDSPのアタッチ</summary>
		/// <param name="config">アタッチ用コンフィグ</param>
		/// <param name="work">アタッチ用ワーク領域へのポインタ</param>
		/// <param name="workSize">アタッチ用ワーク領域のサイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプールにタイムストレッチDSPを追加します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// <para>
		/// 注意:
		/// 備考:
		/// 現在、本関数を使用できないプラットフォームが存在します。
		/// </para>
		/// </remarks>
		public unsafe void AttachDspTimeStretch(in CriAtomEx.DspTimeStretchConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx.DspTimeStretchConfig* configPtr = &config)
				NativeMethods.criAtomExVoicePool_AttachDspTimeStretch(NativeHandle, configPtr, work, workSize);
		}

		/// <summary>タイムストレッチDSPアタッチ用ワーク領域サイズの計算</summary>
		/// <param name="config">アタッチ用コンフィグ</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AFX形式のDSPのアタッチに必要なワーク領域サイズを計算します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspAfx"/>
		public static unsafe Int32 CalculateWorkSizeForDspAfx(in CriAtomEx.DspAfxConfig config)
		{
			fixed (CriAtomEx.DspAfxConfig* configPtr = &config)
				return NativeMethods.criAtomExVoicePool_CalculateWorkSizeForDspAfx(configPtr);
		}

		/// <summary>AFX形式のDSPのアタッチ</summary>
		/// <param name="config">アタッチ用コンフィグ</param>
		/// <param name="work">アタッチ用ワーク領域へのポインタ</param>
		/// <param name="workSize">アタッチ用ワーク領域のサイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプールにAFX形式のDSPを追加します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// <para>
		/// 注意:
		/// 備考:
		/// 現在、本関数を使用できないプラットフォームが存在します。
		/// </para>
		/// </remarks>
		public unsafe void AttachDspAfx(in CriAtomEx.DspAfxConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx.DspAfxConfig* configPtr = &config)
				NativeMethods.criAtomExVoicePool_AttachDspAfx(NativeHandle, configPtr, work, workSize);
		}


		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExVoicePool(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExVoicePool other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExVoicePool a, CriAtomExVoicePool b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExVoicePool a, CriAtomExVoicePool b) =>
			!(a == b);

	}
}