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
	/// <summary>CriAtom API</summary>
	public static partial class CriAtom
	{
		/// <summary>ライブラリ初期化用ワーク領域サイズの計算</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを使用するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ
		/// 構造体（ <see cref="CriAtom.ConfigWASAPI"/> ）の内容によって変化します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.CalculateWorkSizeWASAPI"/> 関数をご利用ください。
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_CalculateWorkSize_WASAPI(const CriAtomConfig_WASAPI *)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigWASAPI"/>
		/// <seealso cref="CriAtom.InitializeWASAPI"/>
		public static unsafe Int32 CalculateWorkSizeWASAPI(in CriAtom.ConfigWASAPI config)
		{
			fixed (CriAtom.ConfigWASAPI* configPtr = &config)
				return NativeMethods.criAtom_CalculateWorkSize_WASAPI(configPtr);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 注意:
		/// 本構造体は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本構造体の代わりに
		/// <see cref="CriAtomEx.ConfigWASAPI"/> 構造体をご利用ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeWASAPI"/>
		[Serializable]
		public unsafe partial struct ConfigWASAPI
		{
			/// <summary>Atom初期化用コンフィグ構造体</summary>
			public CriAtom.Config atom;

			/// <summary>ASR初期化用コンフィグ</summary>
			public CriAtomAsr.Config asr;

			/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
			public CriAtomHcaMx.Config hcaMx;

		}
		/// <summary>ライブラリの初期化</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		/// ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		/// （ライブラリの機能は、本関数を実行後、 <see cref="CriAtom.FinalizeWASAPI"/> 関数を実行するまでの間、
		/// 利用可能です。）
		/// ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて
		/// 変化します。
		/// ワーク領域サイズの計算には、 <see cref="CriAtom.CalculateWorkSizeWASAPI"/>
		/// 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// - <see cref="CriAtom.Initialize"/>
		/// - <see cref="CriAtomAsr.Initialize"/>
		/// - <see cref="CriAtomHcaMx.Initialize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// 本関数を実行後、必ず対になる <see cref="CriAtom.FinalizeWASAPI"/> 関数を実行してください。
		/// また、 <see cref="CriAtom.FinalizeWASAPI"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.InitializeWASAPI"/> 関数をご利用ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_Initialize_WASAPI(const CriAtomConfig_WASAPI *, void *, CriSint32)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigWASAPI"/>
		/// <seealso cref="CriAtom.FinalizeWASAPI"/>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtom.CalculateWorkSizeWASAPI"/>
		public static unsafe void InitializeWASAPI(in CriAtom.ConfigWASAPI config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.ConfigWASAPI* configPtr = &config)
				NativeMethods.criAtom_Initialize_WASAPI(configPtr, work, workSize);
		}

		/// <summary>ライブラリの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// - <see cref="CriAtom.Finalize"/>
		/// - <see cref="CriAtomAsr.Finalize"/>
		/// - <see cref="CriAtomHcaMx.Finalize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtom.InitializeWASAPI"/> 関数実行前に本関数を実行することはできません。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.FinalizeWASAPI"/> 関数をご利用ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_Finalize_WASAPI()"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeWASAPI"/>
		public static void FinalizeWASAPI()
		{
			NativeMethods.criAtom_Finalize_WASAPI();
		}

		/// <summary>ミキサフォーマットの取得</summary>
		/// <param name="format">ミキサのフォーマット</param>
		/// <returns>ミキサのフォーマットが取得できたかどうか（ true = 成功、false = 失敗）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 共有モード時に使用されるミキサのフォーマットを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は IAudioClient::GetMixFormat 関数のラッパーです。
		/// 本関数を実行すると、関数内で AudioClient を作成し、GetMixFormat 関数を実行します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリ初期化前にのみ使用可能です。
		/// 共有モードと排他モードとでは、使用できるフォーマットが異なります。
		/// 本関数で取得する WAVEFORMATEXTENSIBLE 構造体は IEEE float 形式のPCMデータフォーマットを返しますが、
		/// このフォーマットは排他モードではほとんどの場合使用できません。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_GetAudioClientMixFormat_WASAPI(WAVEFORMATEXTENSIBLE *)"/>
		/// </remarks>
		public static bool GetAudioClientMixFormatWASAPI(IntPtr format)
		{
			return NativeMethods.criAtom_GetAudioClientMixFormat_WASAPI(format);
		}

		/// <summary>指定したフォーマットが利用可能かどうかチェック</summary>
		/// <param name="format">使用するフォーマット</param>
		/// <returns>指定されたフォーマットが利用可能かどうか（ true = 利用可能、false = 利用不可能）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 引数で指定したフォーマットが、排他モードで利用可能かどうかをチェックします。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は IAudioClient::IsFormatSupported 関数のラッパーです。
		/// 本関数を実行すると、関数内で AudioClient を作成し、IsFormatSupported 関数を実行します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリ初期化前にのみ使用可能です。
		/// 一部のデバイス／パラメーターについて、本関数が成功するにもかかわらず、
		/// WASAPI の初期化に失敗するケースが確認されています。
		/// 本関数が true を返したにもかかわらず、ライブラリの初期化に失敗する場合には、
		/// 指定するフォーマットを変更するか、または共有モードをご使用ください。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_GetAudioClientIsFormatSupported_WASAPI(const WAVEFORMATEX *)"/>
		/// </remarks>
		public static bool GetAudioClientIsFormatSupportedWASAPI(IntPtr format)
		{
			return NativeMethods.criAtom_GetAudioClientIsFormatSupported_WASAPI(format);
		}

		/// <summary>共有方式の指定</summary>
		/// <param name="mode">使用するモード</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WASAPIを共有モードで使用するか、排他モードで使用するかを指定します。
		/// 本関数を実行しない場合や、 AUDCLNT_SHAREMODE_SHARED を指定して実行した場合、
		/// Atomライブラリは WASAPI を共有モードで初期化します。
		/// AUDCLNT_SHAREMODE_EXCLUSIVE を指定して本関数を実行した場合、
		/// Atomライブラリは WASAPI を排他モードで初期化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリの初期化よりも先に実行しておく必要があります。
		/// 排他モードを使用する場合、本関数でのモード指定に加え、
		/// <see cref="CriAtom.SetAudioClientFormatWASAPI"/> 関数によるフォーマットの指定が必要です。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetAudioClientShareMode_WASAPI(AUDCLNT_SHAREMODE)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioClientFormatWASAPI"/>
		public static void SetAudioClientShareModeWASAPI(Int32 mode)
		{
			NativeMethods.criAtom_SetAudioClientShareMode_WASAPI(mode);
		}

		/// <summary>共有方式の取得</summary>
		/// <returns>共有方式</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 現在指定されている共有方式を取得します。
		/// </para>
		/// <nativeinfo declaration="AUDCLNT_SHAREMODE criAtom_GetAudioClientShareMode_WASAPI()"/>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioClientShareModeWASAPI"/>
		public static Int32 GetAudioClientShareModeWASAPI()
		{
			return NativeMethods.criAtom_GetAudioClientShareMode_WASAPI();
		}

		/// <summary>出力フォーマットの指定</summary>
		/// <param name="format">使用するフォーマット</param>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// 排他モードで使用するフォーマットを指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数に指定したフォーマットが、 IAudioClient::Initialize 関数に渡されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリの初期化よりも先に実行しておく必要があります。
		/// 排他モードを使用する場合、本関数でのモード指定に加え、
		/// <see cref="CriAtom.SetAudioClientShareModeWASAPI"/> 関数によるモード指定が必要です。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetAudioClientFormat_WASAPI(const WAVEFORMATEX *)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioClientShareModeWASAPI"/>
		public static void SetAudioClientFormatWASAPI(IntPtr format)
		{
			NativeMethods.criAtom_SetAudioClientFormat_WASAPI(format);
		}

		/// <summary>バッファリング時間の指定</summary>
		/// <param name="refTime">バッファリング時間</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WASAPI 初期化時に指定するバッファリング時間を指定します。
		/// Atomライブラリは、本関数で指定された時間分のデータを保持可能なサイズのサウンドバッファーを確保します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数に指定した値が IAudioClient::Initialize 関数に渡されます。
		/// ref_timeに0を指定した場合や、本関数を使用しない場合、
		/// Atomライブラリは初期化時に指定されるサーバー処理周波数の値から、
		/// 適切なバッファリング時間を計算します。
		/// PC環境ではハードウェア性能にばらつきがあるため、
		/// ワースト性能のハードウェアに合わせてデフォルトのバッファリング量が多めに設定されています。
		/// （デフォルト状態では4V分のバッファーを持っています。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリの初期化よりも先に実行しておく必要があります。
		/// バッファリング時間を小さくしすぎると、音途切れ等の問題が発生します。
		/// PC環境ではハードウェアに依存して必要なバッファリング量が異なるため、
		/// 本関数を使用した場合、テスト環境でうまく動作していても、
		/// ユーザの環境によっては音途切れが発生する可能性があります。
		/// そのため、バッファリング量を変更する場合には、
		/// ユーザが設定値を変更できる仕組み（オプション画面等）を提供することもご検討ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetAudioClientBufferDuration_WASAPI(REFERENCE_TIME)"/>
		/// </remarks>
		public static void SetAudioClientBufferDurationWASAPI(Int64 refTime)
		{
			NativeMethods.criAtom_SetAudioClientBufferDuration_WASAPI(refTime);
		}

		/// <summary>AudioClientの取得</summary>
		/// <returns>AudioClient</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリ内で作成されたAudioClientを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// サウンドデバイスが搭載されていないPCで本関数を実行した場合、
		/// 本関数はnullを返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化する必要があります。
		/// </para>
		/// <nativeinfo declaration="int * criAtom_GetAudioClient_WASAPI()"/>
		/// </remarks>
		public static IntPtr GetAudioClientWASAPI()
		{
			return NativeMethods.criAtom_GetAudioClient_WASAPI();
		}

		/// <summary>デバイスが無効化されたかどうかのチェック</summary>
		/// <returns>デバイスが無効化されたかどうか（true = 無効化された、false = 正常に動作中）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドデバイスが無効化されたかどうかを返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数がtrueを返すのは、アプリケーション実行中にサウンドデバイスを無効化した場合のみです。
		/// 元々サウンドデバイスが搭載されていないPCで本関数を実行した場合、本関数はfalseを返します。
		/// （サウンドデバイスの有無は別途 <see cref="CriAtom.GetAudioClientWASAPI"/> 関数でチェックする必要があります。）
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_IsDeviceInvalidated_WASAPI()"/>
		/// </remarks>
		/// <seealso cref="CriAtom.GetAudioClientWASAPI"/>
		public static bool IsDeviceInvalidatedWASAPI()
		{
			return NativeMethods.criAtom_IsDeviceInvalidated_WASAPI();
		}

		/// <summary>サウンドデバイスの指定</summary>
		/// <param name="type">サウンドレンダラタイプ</param>
		/// <param name="deviceId">デバイスID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドレンダラタイプとサウンドデバイスの紐づけを行います。
		/// 本関数でサウンドレンダラに対してデバイスIDを設定すると、
		/// 当該サウンドレンダラを指定して出力した音声は、
		/// 全て指定したIDに合致するサウンドデバイスから出力されます。
		/// type には、以下の値が指定可能です。
		/// - <see cref="CriAtom.SoundRendererType.Hw1"/>（<see cref="CriAtom.SoundRendererType.Native"/>と同じ値）
		/// - <see cref="CriAtom.SoundRendererType.Hw2"/>
		/// - <see cref="CriAtom.SoundRendererType.Hw3"/>
		/// - <see cref="CriAtom.SoundRendererType.Hw4"/>
		/// 第2引数（device_id）にnullまたは長さ0の文字列を指定した場合、
		/// 当該サウンドレンダラとデバイスIDの紐づけが解除されます。
		/// （既定のデバイスから音声を出力するよう、動作が変更されます。）
		/// </para>
		/// <para>
		/// 備考:
		/// サウンドデバイスのIDは IMMDevice::GetId で取得する必要があります。
		/// 指定されたIDに一致するサウンドデバイスが見つからない場合、
		/// 当該デバイスを指定して再生された音声は、既定のデバイスから出力されます。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetDeviceId_WASAPI(CriAtomSoundRendererType, LPCWSTR)"/>
		/// </remarks>
		public static void SetDeviceIdWASAPI(CriAtom.SoundRendererType type, IntPtr deviceId)
		{
			NativeMethods.criAtom_SetDeviceId_WASAPI(type, deviceId);
		}

		/// <summary>オーディオエンドポイントの列挙</summary>
		/// <param name="callback">オーディオエンドポイントコールバック関数</param>
		/// <param name="object">ユーザ指定オブジェクト</param>
		/// <returns>列挙されたACBオブジェクトの数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オーディオエンドポイントを列挙します。
		/// 本関数を実行すると、第 1 引数（ callback ）
		/// でセットされたコールバック関数がオーディオエンドポイント数分だけ呼び出されます。
		/// コールバック関数には、IMMDeviceインスタンスが引数として渡されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ object ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtom.AudioEndpointCbFuncWASAPI"/> の説明をご参照ください。
		/// 戻り値は列挙されたオーディオエンドポイントの数（登録したコールバック関数が呼び出された回数）です。
		/// オーディオエンドポイントが存在しない場合、本関数は 0 を返します。
		/// エラーが発生した際には -1 を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// IMMDeviceインスタンスをコールバック関数内で破棄してはいけません。
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_EnumAudioEndpoints_WASAPI(CriAtomAudioEndpointCbFunc_WASAPI, void *)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.AudioEndpointCbFuncWASAPI"/>
		public static unsafe Int32 EnumAudioEndpointsWASAPI(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> callback, IntPtr @object)
		{
			return NativeMethods.criAtom_EnumAudioEndpoints_WASAPI((IntPtr)callback, @object);
		}

		/// <summary>オーディオエンドポイント列挙コールバック</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オーディオエンドポイントの通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtom.EnumAudioEndpointsWASAPI"/> 関数に本関数型のコールバック関数を登録することで、
		/// IMMDeviceインスタンスをコールバック経由で受け取ることが可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// IMMDeviceインスタンスをコールバック関数内で破棄してはいけません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.EnumAudioEndpointsWASAPI"/>
		public unsafe class AudioEndpointCbFuncWASAPI : NativeCallbackBase<AudioEndpointCbFuncWASAPI.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>IMMDeviceインスタンス</summary>
				public IntPtr device { get; }

				internal Arg(IntPtr device)
				{
					this.device = device;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CriAtomAudioEndpointCbFunc_WASAPICallbackFunc(IntPtr @object, IntPtr device) =>
				InvokeCallbackInternal(@object, new(device));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr @object, IntPtr device);
			static NativeDelegate callbackDelegate = null;
#endif
			internal AudioEndpointCbFuncWASAPI(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CriAtomAudioEndpointCbFunc_WASAPICallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomAudioEndpointCbFunc_WASAPICallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>デバイス更新通知の登録</summary>
		/// <param name="callback">デバイス更新通知コールバック関数</param>
		/// <param name="object">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デバイスの更新通知を受け取るためのコールバックを設定します。
		/// 本関数を実行すると、デバイスが更新された際、第 1 引数（ callback ）
		/// でセットされたコールバック関数が呼び出されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ object ）にセットした値は、コールバック関数の引数として渡されます。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetDeviceUpdateCallback_WASAPI(CriAtomDeviceUpdateCbFunc_WASAPI, void *)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.AudioEndpointCbFuncWASAPI"/>
		public static unsafe void SetDeviceUpdateCallbackWASAPI(delegate* unmanaged[Cdecl]<IntPtr, void> callback, IntPtr @object)
		{
			NativeMethods.criAtom_SetDeviceUpdateCallback_WASAPI((IntPtr)callback, @object);
		}

		/// <summary>デバイス更新通知コールバック</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デバイスの更新通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtom.SetDeviceUpdateCallbackWASAPI"/> 関数に本関数型のコールバック関数を登録することで、
		/// デバイスが更新された際にコールバック経由で通知を受け取ることが可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetDeviceUpdateCallbackWASAPI"/>
		public unsafe class DeviceUpdateCbFuncWASAPI : NativeCallbackBase<DeviceUpdateCbFuncWASAPI.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{

			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CriAtomDeviceUpdateCbFunc_WASAPICallbackFunc(IntPtr @object) =>
				InvokeCallbackInternal(@object, new());
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr @object);
			static NativeDelegate callbackDelegate = null;
#endif
			internal DeviceUpdateCbFuncWASAPI(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, void>)&CriAtomDeviceUpdateCbFunc_WASAPICallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomDeviceUpdateCbFunc_WASAPICallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>スペーシャルオーディオ機能の有効化</summary>
		/// <param name="type">サウンドレンダラタイプ</param>
		/// <param name="sw">機能を有効にするかどうか（true = 有効化、false = 無効化）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペーシャルオーディオ機能（Microsoft Spatial Sound）を有効にします。
		/// 引数の type には、スペーシャルオーディオ機能を有効（又は無効）にするサウンドレンダラを指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// 現行のライブラリでは、スペーシャルオーディオ機能はデフォルトで有効です。
		/// そのため、スペーシャルオーディオ機能を無効にしたい場合を除き、本関数を明示的に呼び出す必要はありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリ初期化前に使用する必要があります。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetSpatialAudioEnabled_WASAPI(CriAtomSoundRendererType, CriBool)"/>
		/// </remarks>
		public static void SetSpatialAudioEnabledWASAPI(CriAtom.SoundRendererType type, NativeBool sw)
		{
			NativeMethods.criAtom_SetSpatialAudioEnabled_WASAPI(type, sw);
		}

		/// <summary>スペーシャルオーディオ機能が有効かどうかのチェック</summary>
		/// <param name="type">サウンドレンダラタイプ</param>
		/// <returns>機能が有効かどうか（true = 有効、false = 無効）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペーシャルオーディオ機能が有効になっているかどうかをチェックします。
		/// 引数の type には、スペーシャルオーディオ機能が有効化どうかをチェックしたいサウンドレンダラを指定します。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_IsSpatialAudioEnabled_WASAPI(CriAtomSoundRendererType)"/>
		/// </remarks>
		public static bool IsSpatialAudioEnabledWASAPI(CriAtom.SoundRendererType type)
		{
			return NativeMethods.criAtom_IsSpatialAudioEnabled_WASAPI(type);
		}

		/// <summary>ライブラリ初期化用ワーク領域サイズの計算</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを使用するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ
		/// 構造体（ <see cref="CriAtom.ConfigPC"/> ）の内容によって変化します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.CalculateWorkSizePC"/> 関数をご利用ください。
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_CalculateWorkSize_PC(const CriAtomConfig_PC *)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigPC"/>
		/// <seealso cref="CriAtom.InitializePC"/>
		public static unsafe Int32 CalculateWorkSizePC(in CriAtom.ConfigPC config)
		{
			fixed (CriAtom.ConfigPC* configPtr = &config)
				return NativeMethods.criAtom_CalculateWorkSize_PC(configPtr);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 注意:
		/// 本構造体は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本構造体の代わりに
		/// <see cref="CriAtomEx.ConfigPC"/> 構造体をご利用ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		[Serializable]
		public unsafe partial struct ConfigPC
		{
			/// <summary>Atom初期化用コンフィグ構造体</summary>
			public CriAtom.Config atom;

			/// <summary>ASR初期化用コンフィグ</summary>
			public CriAtomAsr.Config asr;

			/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
			public CriAtomHcaMx.Config hcaMx;

		}
		/// <summary>ライブラリの初期化</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		/// ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		/// （ライブラリの機能は、本関数を実行後、 <see cref="CriAtom.FinalizePC"/> 関数を実行するまでの間、
		/// 利用可能です。）
		/// ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて
		/// 変化します。
		/// ワーク領域サイズの計算には、 <see cref="CriAtom.CalculateWorkSizePC"/>
		/// 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// - <see cref="CriAtom.Initialize"/>
		/// - <see cref="CriAtomAsr.Initialize"/>
		/// - <see cref="CriAtomHcaMx.Initialize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// 本関数を実行後、必ず対になる <see cref="CriAtom.FinalizePC"/> 関数を実行してください。
		/// また、 <see cref="CriAtom.FinalizePC"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.InitializePC"/> 関数をご利用ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_Initialize_PC(const CriAtomConfig_PC *, void *, CriSint32)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigPC"/>
		/// <seealso cref="CriAtom.FinalizePC"/>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtom.CalculateWorkSizePC"/>
		public static unsafe void InitializePC(in CriAtom.ConfigPC config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.ConfigPC* configPtr = &config)
				NativeMethods.criAtom_Initialize_PC(configPtr, work, workSize);
		}

		/// <summary>ライブラリの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// - <see cref="CriAtom.Finalize"/>
		/// - <see cref="CriAtomAsr.Finalize"/>
		/// - <see cref="CriAtomHcaMx.Finalize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtom.InitializePC"/> 関数実行前に本関数を実行することはできません。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.FinalizePC"/> 関数をご利用ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_Finalize_PC()"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		public static void FinalizePC()
		{
			NativeMethods.criAtom_Finalize_PC();
		}

		/// <summary>サーバー処理スレッドのプライオリティ変更</summary>
		/// <param name="prio">スレッドプライオリティ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのプライオリティを変更します。
		/// デフォルト状態（本関数を実行しない場合）では、サーバー処理スレッドのプライオリティは
		/// THREAD_PRIORITY_HIGHEST に設定されます。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// サーバー処理スレッドは、CRI File Systemライブラリでも利用されています。
		/// すでにCRI File SystemライブラリのAPIでサーバー処理スレッドの設定を変更している場合
		/// 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetThreadPriority_PC(CriSint32)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.GetThreadPriorityPC"/>
		public static void SetThreadPriorityPC(Int32 prio)
		{
			NativeMethods.criAtom_SetThreadPriority_PC(prio);
		}

		/// <summary>サーバー処理スレッドのプライオリティ取得</summary>
		/// <returns>スレッドプライオリティ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのプライオリティを取得します。
		/// 取得に成功すると、本関数はサーバー処理を行うスレッドのプライオリティを返します。
		/// 取得に失敗した場合、本関数は THREAD_PRIORITY_ERROR_RETURN を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラー値を返します。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラー値を返します。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_GetThreadPriority_PC()"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.SetThreadPriorityPC"/>
		public static Int32 GetThreadPriorityPC()
		{
			return NativeMethods.criAtom_GetThreadPriority_PC();
		}

		/// <summary>サーバー処理スレッドのアフィニティマスク変更</summary>
		/// <param name="mask">スレッドアフィニティマスク</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのアフィニティマスクを変更します。
		/// デフォルト状態（本関数を実行しない場合）では、サーバー処理が動作するプロセッサは
		/// 一切制限されません。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// サーバー処理スレッドは、CRI File Systemライブラリでも利用されています。
		/// すでにCRI File SystemライブラリのAPIでサーバー処理スレッドの設定を変更している場合
		/// 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetThreadAffinityMask_PC(DWORD_PTR)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.GetThreadAffinityMaskPC"/>
		public static void SetThreadAffinityMaskPC(IntPtr mask)
		{
			NativeMethods.criAtom_SetThreadAffinityMask_PC(mask);
		}

		/// <summary>サーバー処理スレッドのアフィニティマスクの取得</summary>
		/// <returns>スレッドアフィニティマスク</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのアフィニティマスクを取得します。
		/// 取得に成功すると、本関数はサーバー処理を行うスレッドのアフィニティマスクを返します。
		/// 取得に失敗した場合、本関数は 0 を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラー値を返します。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラー値を返します。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// <nativeinfo declaration="DWORD_PTR criAtom_GetThreadAffinityMask_PC()"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.SetThreadAffinityMaskPC"/>
		public static IntPtr GetThreadAffinityMaskPC()
		{
			return NativeMethods.criAtom_GetThreadAffinityMask_PC();
		}

	}
}