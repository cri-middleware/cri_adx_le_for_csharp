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
#pragma warning disable 0465
	/// <summary>CriAtom API</summary>
	public static partial class CriAtom
	{
		/// <summary>ライブラリ初期化用ワーク領域サイズの計算 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <returns>CriSint32 ワーク領域サイズ </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを使用するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ 構造体（ <see cref="CriAtom.ConfigWASAPI"/> ）の内容によって変化します。
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.CalculateWorkSizeWASAPI"/> 関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_CalculateWorkSize_WASAPI(const CriAtomConfig_WASAPI *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigWASAPI"/>
		/// <seealso cref="CriAtom.InitializeWASAPI"/>
		public static unsafe Int32 CalculateWorkSizeWASAPI(in CriAtom.ConfigWASAPI config)
		{
			fixed (CriAtom.ConfigWASAPI* configPtr = &config)
				return NativeMethods.criAtom_CalculateWorkSize_WASAPI(configPtr);
		}

		/// <summary>ライブラリの初期化 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		///  ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		///  （ライブラリの機能は、本関数を実行後、 <see cref="CriAtom.FinalizeWASAPI"/> 関数を実行するまでの間、 利用可能です。）
		///  ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて 変化します。
		///  ワーク領域サイズの計算には、 <see cref="CriAtom.CalculateWorkSizeWASAPI"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、 本関数にワーク領域を指定する必要はありません。
		///  （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター から必要なワーク領域サイズ分のメモリが動的に確保されます。） 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// <list type="bullet">
		/// <item><description>criAtom_Initialize</description></item>
		/// <item><description><see cref="CriAtomAsr.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Initialize"/> 本関数を実行する場合、上記関数を実行しないでください。
		///  本関数を実行後、必ず対になる <see cref="CriAtom.FinalizeWASAPI"/> 関数を実行してください。
		///  また、 <see cref="CriAtom.FinalizeWASAPI"/> 関数を実行するまでは、本関数を再度実行しないでください。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.InitializeWASAPI"/> 関数をご利用ください。 </description></item>
		/// </list>
		/// </para>
		/// <nativeinfo declaration="void criAtom_Initialize_WASAPI(const CriAtomConfig_WASAPI *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigWASAPI"/>
		/// <seealso cref="CriAtom.FinalizeWASAPI"/>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtom.CalculateWorkSizeWASAPI"/>
		public static unsafe void InitializeWASAPI(in CriAtom.ConfigWASAPI config)
		{
			fixed (CriAtom.ConfigWASAPI* configPtr = &config)
				NativeMethods.criAtom_Initialize_WASAPI(configPtr, default, default);
		}

		/// <summary>ライブラリの終了 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// <list type="bullet">
		/// <item><description>criAtom_Finalize</description></item>
		/// <item><description><see cref="CriAtomAsr.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Finalize"/> 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtom.InitializeWASAPI"/> 関数実行前に本関数を実行することはできません。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.FinalizeWASAPI"/> 関数をご利用ください。 </description></item>
		/// </list>
		/// </para>
		/// <nativeinfo declaration="void criAtom_Finalize_WASAPI(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeWASAPI"/>
		public static void FinalizeWASAPI()
		{
			NativeMethods.criAtom_Finalize_WASAPI();
		}

		/// <summary>デフォルトデバイス種別の指定 </summary>
		/// <param name="role">デフォルトデバイスとして使用するデバイスの種別 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デフォルトデバイスの種別を指定します。 roleにeConsoleを指定した場合、Atomライブラリは既定のデバイスを使用して音声を出力します。
		///  roleにeCommunicationsを指定した場合、Atomライブラリは既定の通信デバイスを使用して音声を出力します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリ初期化前に使用する必要があります。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetDefaultDeviceRole_WASAPI(ERole role)"/>
		/// </remarks>
		public static void SetDefaultDeviceRoleWASAPI(Int32 role)
		{
			NativeMethods.criAtom_SetDefaultDeviceRole_WASAPI(role);
		}

		/// <summary>ミキサフォーマットの取得 </summary>
		/// <param name="format">ミキサのフォーマット </param>
		/// <returns>CriBool ミキサのフォーマットが取得できたかどうか（ true = 成功、false = 失敗） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 共有モード時に使用されるミキサのフォーマットを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は IAudioClient::GetMixFormat 関数のラッパーです。
		///  本関数を実行すると、関数内で AudioClient を作成し、GetMixFormat 関数を実行します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリ初期化前にのみ使用可能です。
		///  共有モードと排他モードとでは、使用できるフォーマットが異なります。
		///  本関数で取得する WAVEFORMATEXTENSIBLE 構造体は IEEE float 形式のPCMデータフォーマットを返しますが、 このフォーマットは排他モードではほとんどの場合使用できません。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_GetAudioClientMixFormat_WASAPI(WAVEFORMATEXTENSIBLE *format)"/>
		/// </remarks>
		public static bool GetAudioClientMixFormatWASAPI(IntPtr format)
		{
			return NativeMethods.criAtom_GetAudioClientMixFormat_WASAPI(format);
		}

		/// <summary>指定したフォーマットが利用可能かどうかチェック </summary>
		/// <param name="format">使用するフォーマット </param>
		/// <returns>CriBool 指定されたフォーマットが利用可能かどうか（ true = 利用可能、false = 利用不可能） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 引数で指定したフォーマットが、排他モードで利用可能かどうかをチェックします。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は IAudioClient::IsFormatSupported 関数のラッパーです。
		///  本関数を実行すると、関数内で AudioClient を作成し、IsFormatSupported 関数を実行します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリ初期化前にのみ使用可能です。
		///  一部のデバイス／パラメーターについて、本関数が成功するにもかかわらず、 WASAPI の初期化に失敗するケースが確認されています。
		///  本関数が true を返したにもかかわらず、ライブラリの初期化に失敗する場合には、 指定するフォーマットを変更するか、または共有モードをご使用ください。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_GetAudioClientIsFormatSupported_WASAPI(const WAVEFORMATEX *format)"/>
		/// </remarks>
		public static bool GetAudioClientIsFormatSupportedWASAPI(IntPtr format)
		{
			return NativeMethods.criAtom_GetAudioClientIsFormatSupported_WASAPI(format);
		}

		/// <summary>共有方式の指定 </summary>
		/// <param name="mode">使用するモード </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WASAPIを共有モードで使用するか、排他モードで使用するかを指定します。
		///  本関数を実行しない場合や、 AUDCLNT_SHAREMODE_SHARED を指定して実行した場合、 Atomライブラリは WASAPI を共有モードで初期化します。
		///  AUDCLNT_SHAREMODE_EXCLUSIVE を指定して本関数を実行した場合、 Atomライブラリは WASAPI を排他モードで初期化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリの初期化よりも先に実行しておく必要があります。
		///  排他モードを使用する場合、本関数でのモード指定に加え、 <see cref="CriAtom.SetAudioClientFormatWASAPI"/> 関数によるフォーマットの指定が必要です。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetAudioClientShareMode_WASAPI(AUDCLNT_SHAREMODE mode)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioClientFormatWASAPI"/>
		public static void SetAudioClientShareModeWASAPI(Int32 mode)
		{
			NativeMethods.criAtom_SetAudioClientShareMode_WASAPI(mode);
		}

		/// <summary>共有方式の取得 </summary>
		/// <returns>AUDCLNT_SHAREMODE 共有方式 </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 現在指定されている共有方式を取得します。 
		/// </para>
		/// <nativeinfo declaration="AUDCLNT_SHAREMODE criAtom_GetAudioClientShareMode_WASAPI(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioClientShareModeWASAPI"/>
		public static Int32 GetAudioClientShareModeWASAPI()
		{
			return NativeMethods.criAtom_GetAudioClientShareMode_WASAPI();
		}

		/// <summary>出力フォーマットの指定 </summary>
		/// <param name="format">使用するフォーマット </param>
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
		///  排他モードを使用する場合、本関数でのモード指定に加え、 <see cref="CriAtom.SetAudioClientShareModeWASAPI"/> 関数によるモード指定が必要です。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetAudioClientFormat_WASAPI(const WAVEFORMATEX *format)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioClientShareModeWASAPI"/>
		public static void SetAudioClientFormatWASAPI(IntPtr format)
		{
			NativeMethods.criAtom_SetAudioClientFormat_WASAPI(format);
		}

		/// <summary>バッファリング時間の指定 </summary>
		/// <param name="refTime">バッファリング時間 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WASAPI 初期化時に指定するバッファリング時間を指定します。
		///  Atomライブラリは、本関数で指定された時間分のデータを保持可能なサイズのサウンドバッファーを確保します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数に指定した値が IAudioClient::Initialize 関数に渡されます。
		///  ref_timeに0を指定した場合や、本関数を使用しない場合、 Atomライブラリは初期化時に指定されるサーバー処理周波数の値から、 適切なバッファリング時間を計算します。
		///  PC環境ではハードウェア性能にばらつきがあるため、 ワースト性能のハードウェアに合わせてデフォルトのバッファリング量が多めに設定されています。
		///  （デフォルト状態では4V分のバッファーを持っています。） 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリの初期化よりも先に実行しておく必要があります。
		///  バッファリング時間を小さくしすぎると、音途切れ等の問題が発生します。
		///  PC環境ではハードウェアに依存して必要なバッファリング量が異なるため、 本関数を使用した場合、テスト環境でうまく動作していても、 ユーザの環境によっては音途切れが発生する可能性があります。
		///  そのため、バッファリング量を変更する場合には、 ユーザが設定値を変更できる仕組み（オプション画面等）を提供することもご検討ください。 
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetAudioClientBufferDuration_WASAPI(REFERENCE_TIME ref_time)"/>
		/// </remarks>
		public static void SetAudioClientBufferDurationWASAPI(Int64 refTime)
		{
			NativeMethods.criAtom_SetAudioClientBufferDuration_WASAPI(refTime);
		}

		/// <summary>AudioClientの取得 </summary>
		/// <returns>IAudioClient AudioClient </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリ内で作成されたAudioClientを取得します。 
		/// </para>
		/// <para>
		/// 備考:
		/// サウンドデバイスが搭載されていないPCで本関数を実行した場合、 本関数はnullを返します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化する必要があります。 
		/// </para>
		/// <nativeinfo declaration="IAudioClient* criAtom_GetAudioClient_WASAPI(void)"/>
		/// </remarks>
		public static unsafe IntPtr GetAudioClientWASAPI()
		{
			return NativeMethods.criAtom_GetAudioClient_WASAPI();
		}

		/// <summary>デバイスが無効化されたかどうかのチェック </summary>
		/// <returns>CriBool デバイスが無効化されたかどうか（true = 無効化された、false = 正常に動作中） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドデバイスが無効化されたかどうかを返します。 
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数がtrueを返すのは、アプリケーション実行中にサウンドデバイスを無効化した場合のみです。
		///  元々サウンドデバイスが搭載されていないPCで本関数を実行した場合、本関数はfalseを返します。
		///  （サウンドデバイスの有無は別途 <see cref="CriAtom.GetAudioClientWASAPI"/> 関数でチェックする必要があります。） 
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_IsDeviceInvalidated_WASAPI(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.GetAudioClientWASAPI"/>
		public static bool IsDeviceInvalidatedWASAPI()
		{
			return NativeMethods.criAtom_IsDeviceInvalidated_WASAPI();
		}

		/// <summary>サウンドデバイスの指定 </summary>
		/// <param name="type">サウンドレンダラタイプ </param>
		/// <param name="deviceId">デバイスID </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドレンダラタイプとサウンドデバイスの紐づけを行います。
		///  本関数でサウンドレンダラに対してデバイスIDを設定すると、 当該サウンドレンダラを指定して出力した音声は、 全て指定したIDに合致するサウンドデバイスから出力されます。 
		///  type には、以下の値が指定可能です。
		/// <list type="bullet">
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw1"/>（<see cref="CriAtom.SoundRendererType.Native"/>と同じ値）</description></item>
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw2"/></description></item>
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw3"/></description></item>
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw4"/> 第2引数（device_id）にnullまたは長さ0の文字列を指定した場合、 当該サウンドレンダラとデバイスIDの紐づけが解除されます。
		///  （既定のデバイスから音声を出力するよう、動作が変更されます。） </description></item>
		/// </list>
		/// </para>
		/// <para>
		/// 備考:
		/// サウンドデバイスのIDは IMMDevice::GetId で取得する必要があります。
		///  指定されたIDに一致するサウンドデバイスが見つからない場合、 当該デバイスを指定して再生された音声は、既定のデバイスから出力されます。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetDeviceId_WASAPI(CriAtomSoundRendererType type, LPCWSTR device_id)"/>
		/// </remarks>
		public static void SetDeviceIdWASAPI(CriAtom.SoundRendererType type, IntPtr deviceId)
		{
			NativeMethods.criAtom_SetDeviceId_WASAPI(type, deviceId);
		}

		/// <summary>サウンドデバイスのID取得 </summary>
		/// <param name="type">サウンドレンダラタイプ </param>
		/// <param name="deviceId">デバイスID格納領域 </param>
		/// <param name="count">デバイスID格納領域のサイズ（文字数） </param>
		/// <param name="isDefaultDevice">デフォルトデバイスかどうか </param>
		/// <returns>CriBool デバイスIDが取得できたかどうか </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドレンダラタイプに紐づけられたサウンドデバイスのIDを取得します。
		///  type には、以下の値が指定可能です。
		/// <list type="bullet">
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw1"/>（<see cref="CriAtom.SoundRendererType.Native"/>と同じ値）</description></item>
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw2"/></description></item>
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw3"/></description></item>
		/// <item><description><see cref="CriAtom.SoundRendererType.Hw4"/> 
		///  第2引数（device_id）にはデバイスID文字列を受け取るためのメモリ領域を指定します。 この領域は本関数を呼び出すアプリケーション側で確保する必要があります。
		///  第3引数（count）には、第2引数で指定したメモリ領域に格納可能な最大文字数を指定します。 
		///  指定したサウンドレンダラに対応するデバイスがデフォルトデバイスの場合、 第4引数（is_default_device）には true がセットされます。
		///  指定したサウンドレンダラに対応するデバイスが <see cref="CriAtom.SetDeviceIdWASAPI"/> 関数で 指定されたデバイスの場合、第4引数（is_default_device）には false がセットされます。
		///  サウンドデバイスのIDが取得できた場合、本関数は true を返します。
		///  本関数が false を返した場合、以下のいずれかに該当しています。
		/// </description></item>
		/// </list>
		/// </para>
		/// <list>
		/// <list type="bullet">
		/// <item><description>サウンドデバイスが存在しない</description></item>
		/// <item><description>デバイスID文字列を格納するための領域が不足している</description></item>
		/// <item><description>プラットフォームSDKがエラーを返した</description></item>
		/// <item><description>不正な引数が指定された サウンドデバイスが存在しない場合以外は合わせてエラーコールバックが通知されます。 第3引数で指定するサイズはバイト数ではなく文字数です。
		///  デバイスID文字列は通常ワイド文字列（wchar_t型）であるため、 第2引数で指定するメモリ領域は、文字数に sizeof(wchar_t) を掛けたサイズ分必要です。 
		///  デフォルトデバイスが使用されている場合（第4引数で取得した値が true の場合）に、 本関数で取得したデバイスIDを <see cref="CriAtom.SetDeviceIdWASAPI"/> 関数に指定する （デフォルトデバイスを取得したデバイスIDで上書きする）と、 デフォルトデバイス指定を追従しない形に挙動が変更されてしまいます。
		///  具体的には、デフォルトデバイス使用時はユーザーのWindows上での指定に応じて サウンドデバイスが自動的に切り替わるのに対し、 デバイスID上書き後はユーザーがWindows上で既定のデバイスを変更したとしても、 デバイスが自動的には変更されない挙動となります。 本関数を使用する際には、ユーザーによる既定のデバイスの変更操作が無効化されないよう、 デバイスIDだけでなくデフォルトデバイスかどうかも合わせて確認してください。 <see cref="CriAtom.SetDeviceIdWASAPI"/></description></item>
		/// </list>
		/// </list>
		/// <nativeinfo declaration="CriBool criAtom_GetDeviceId_WASAPI(CriAtomSoundRendererType type, LPWSTR device_id, CriSint32 count, CriBool *is_default_device)"/>
		/// </remarks>
		public static unsafe bool GetDeviceIdWASAPI(CriAtom.SoundRendererType type, IntPtr deviceId, Int32 count, out NativeBool isDefaultDevice)
		{
			fixed (NativeBool* isDefaultDevicePtr = &isDefaultDevice)
				return NativeMethods.criAtom_GetDeviceId_WASAPI(type, deviceId, count, isDefaultDevicePtr);
		}

		/// <summary>オーディオエンドポイントの列挙 </summary>
		/// <param name="callback">オーディオエンドポイントコールバック関数 </param>
		/// <param name="object">ユーザ指定オブジェクト </param>
		/// <returns>CriSint32 列挙されたACBオブジェクトの数 </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オーディオエンドポイントを列挙します。
		///  本関数を実行すると、第 1 引数（ callback ） でセットされたコールバック関数がオーディオエンドポイント数分だけ呼び出されます。
		///  コールバック関数には、IMMDeviceインスタンスが引数として渡されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ object ）にセットした値は、コールバック関数の引数として渡されます。
		///  コールバック関数のその他の引数については、 別途 <see cref="CriAtom.AudioEndpointCbFuncWASAPI"/> の説明をご参照ください。
		///  戻り値は列挙されたオーディオエンドポイントの数（登録したコールバック関数が呼び出された回数）です。
		///  オーディオエンドポイントが存在しない場合、本関数は 0 を返します。
		///  エラーが発生した際には -1 を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// IMMDeviceインスタンスをコールバック関数内で破棄してはいけません。
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_EnumAudioEndpoints_WASAPI(CriAtomAudioEndpointCbFunc_WASAPI callback, void *object)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.AudioEndpointCbFuncWASAPI"/>
		public static unsafe Int32 EnumAudioEndpointsWASAPI(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> callback, IntPtr @object)
		{
			return NativeMethods.criAtom_EnumAudioEndpoints_WASAPI((IntPtr)callback, @object);
		}

		/// <summary>デバイス更新通知の登録 </summary>
		/// <param name="callback">デバイス更新通知コールバック関数 </param>
		/// <param name="object">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デバイスの更新通知を受け取るためのコールバックを設定します。
		///  本関数を実行すると、デバイスが更新された際、第 1 引数（ callback ） でセットされたコールバック関数が呼び出されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ object ）にセットした値は、コールバック関数の引数として渡されます。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetDeviceUpdateCallback_WASAPI(CriAtomDeviceUpdateCbFunc_WASAPI callback, void *object)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.AudioEndpointCbFuncWASAPI"/>
		public static unsafe void SetDeviceUpdateCallbackWASAPI(delegate* unmanaged[Cdecl]<IntPtr, void> callback, IntPtr @object)
		{
			NativeMethods.criAtom_SetDeviceUpdateCallback_WASAPI((IntPtr)callback, @object);
		}

		/// <summary>スペーシャルオーディオ機能の有効化 </summary>
		/// <param name="type">サウンドレンダラタイプ </param>
		/// <param name="sw">機能を有効にするかどうか（CRI_TRUE = 有効化、CRI_FALSE = 無効化） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペーシャルオーディオ機能（Microsoft Spatial Sound）を有効にします。
		///  引数の type には、スペーシャルオーディオ機能を有効（又は無効）にするサウンドレンダラを指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// 現行のライブラリでは、スペーシャルオーディオ機能はデフォルトで有効です。
		///  そのため、スペーシャルオーディオ機能を無効にしたい場合を除き、本関数を明示的に呼び出す必要はありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はライブラリ初期化前に使用する必要があります。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetSpatialAudioEnabled_WASAPI(CriAtomSoundRendererType type, CriBool sw)"/>
		/// </remarks>
		public static void SetSpatialAudioEnabledWASAPI(CriAtom.SoundRendererType type, NativeBool sw)
		{
			NativeMethods.criAtom_SetSpatialAudioEnabled_WASAPI(type, sw);
		}

		/// <summary>スペーシャルオーディオ機能が有効かどうかのチェック </summary>
		/// <param name="type">サウンドレンダラタイプ </param>
		/// <returns>CriBool 機能が有効かどうか（true = 有効、false = 無効） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペーシャルオーディオ機能が有効になっているかどうかをチェックします。
		///  引数の type には、スペーシャルオーディオ機能が有効化どうかをチェックしたいサウンドレンダラを指定します。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_IsSpatialAudioEnabled_WASAPI(CriAtomSoundRendererType type)"/>
		/// </remarks>
		public static bool IsSpatialAudioEnabledWASAPI(CriAtom.SoundRendererType type)
		{
			return NativeMethods.criAtom_IsSpatialAudioEnabled_WASAPI(type);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 注意:
		/// 本構造体は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本構造体の代わりに <see cref="CriAtomEx.ConfigWASAPI"/> 構造体をご利用ください。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeWASAPI"/>
		[Serializable]
		public unsafe partial struct ConfigWASAPI
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>Atom初期化用コンフィグ構造体 </para>
			/// </remarks>
			public CriAtom.Config atom;

			/// <summary></summary>
			/// <remarks>
			/// <para>ASR初期化用コンフィグ </para>
			/// </remarks>
			public CriAtomAsr.Config asr;

			/// <summary></summary>
			/// <remarks>
			/// <para>HCA-MX初期化用コンフィグ構造体 </para>
			/// </remarks>
			public CriAtomHcaMx.Config hcaMx;

		}
		/// <summary>オーディオエンドポイント列挙コールバック </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オーディオエンドポイントの通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtom.EnumAudioEndpointsWASAPI"/> 関数に本関数型のコールバック関数を登録することで、 IMMDeviceインスタンスをコールバック経由で受け取ることが可能です。
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
				/// <summary>IMMDeviceインスタンス </summary>
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
		/// <summary>デバイス更新通知コールバック </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デバイスの更新通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtom.SetDeviceUpdateCallbackWASAPI"/> 関数に本関数型のコールバック関数を登録することで、 デバイスが更新された際にコールバック経由で通知を受け取ることが可能です。
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
	}
}