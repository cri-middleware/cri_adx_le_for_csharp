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
	/// <summary>CriAtomEx API</summary>
	public static partial class CriAtomEx
	{
		/// <summary>サンプリングレートの取得 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 現在環境のサンプリングレートを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// WebAudioはブラウザーや環境によって出力サンプリングレートが異なります。
		///  そのためAtomを初期化する前にこの関数でサンプリングレート取得し、
		///  同じサンプリングレートでAtomを初期化してください。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomEx_GetWebAudioSamplingRate_WEBAUDIO(void)"/>
		/// </remarks>
		public static Int32 GetWebAudioSamplingRateWEBAUDIO()
		{
			return NativeMethods.criAtomEx_GetWebAudioSamplingRate_WEBAUDIO();
		}

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
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ 構造体（ <see cref="CriAtomEx.ConfigWEBAUDIO"/> ）の内容によって変化します。
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.ConfigWEBAUDIO"/> 構造体のacf_infoメンバに値を設定している場合、本関数は失敗し-1を返します。
		///  初期化処理内でACFデータの登録を行う場合は、本関数値を使用したメモリ確保ではなくADXシステムによる メモリアロケータを使用したメモリ確保処理が必要になります。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomEx_CalculateWorkSize_WEBAUDIO(const CriAtomExConfig_WEBAUDIO *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigWEBAUDIO"/>
		/// <seealso cref="CriAtomEx.InitializeWEBAUDIO"/>
		public static unsafe Int32 CalculateWorkSizeWEBAUDIO(in CriAtomEx.ConfigWEBAUDIO config)
		{
			fixed (CriAtomEx.ConfigWEBAUDIO* configPtr = &config)
				return NativeMethods.criAtomEx_CalculateWorkSize_WEBAUDIO(configPtr);
		}

		/// <summary>ライブラリの初期化 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		///  ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		///  （ライブラリの機能は、本関数を実行後、 <see cref="CriAtomEx.Finalize"/>_WebAudio 関数を実行するまでの間、 利用可能です。）
		///  ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて 変化します。
		///  ワーク領域サイズの計算には、 <see cref="CriAtomEx.CalculateWorkSize"/>_WebAudio 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケータを登録済みの場合、 本関数にワーク領域を指定する必要はありません。
		///  （ work に null 、 work_size に 0 を指定することで、登録済みのアロケータ から必要なワーク領域サイズ分のメモリが動的に確保されます。） 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// <list type="bullet">
		/// <item><description><see cref="CriAtomEx.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomExAsr.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomExHcaMx.Initialize"/></description></item>
		/// </list>
		/// </para>
		/// <see><see cref="CriAtomEx.FinalizeWEBAUDIO"/></see>
		/// <para>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		///  本関数を実行後、必ず対になる 
		///  関数を実行してください。
		///  また、 <see cref="CriAtomEx.Finalize"/>_WebAudio 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// <nativeinfo declaration="void criAtomEx_Initialize_WEBAUDIO(const CriAtomExConfig_WEBAUDIO *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigWEBAUDIO"/>
		/// <seealso cref="CriAtomEx.FinalizeWEBAUDIO"/>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeWEBAUDIO"/>
		public static unsafe void InitializeWEBAUDIO(in CriAtomEx.ConfigWEBAUDIO config)
		{
			fixed (CriAtomEx.ConfigWEBAUDIO* configPtr = &config)
				NativeMethods.criAtomEx_Initialize_WEBAUDIO(configPtr, default, default);
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
		/// <item><description><see cref="CriAtomEx.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomExAsr.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomExHcaMx.Finalize"/></description></item>
		/// </list>
		/// </para>
		/// <see><see cref="CriAtomEx.InitializeWEBAUDIO"/></see>
		/// <para>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		///  関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void criAtomEx_Finalize_WEBAUDIO(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtomEx.InitializeWEBAUDIO"/>
		public static void FinalizeWEBAUDIO()
		{
			NativeMethods.criAtomEx_Finalize_WEBAUDIO();
		}

		/// <summary>バッファリング時間の設定 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomが内部でバッファリングする時間を設定します。
		/// </para>
		/// <para>
		/// 注意:
		/// ブラウザーのAtomはシングルスレッドで動作するため、
		///  アプリケーションの処理によってAtomの定期処理が阻害されると
		///  音途切れが発生します。内部バッファリングを増やすことで
		///  音途切れの影響を緩和することができます。
		///  一方、バッファリング時間を増やすと発音遅延が増えるデメリットがあります。
		///  そのため、ファイル読み込みなど重い処理の前に本関数でバッファリング時間を増やし、
		///  処理後に0に戻すことを推奨します。 設定できる値は0（デフォルト）～200 msです。 
		/// </para>
		/// <nativeinfo declaration="void criAtomEx_SetBufferingTime_WEBAUDIO(CriSint32 buffering_time)"/>
		/// </remarks>
		public static void SetBufferingTimeWEBAUDIO(Int32 bufferingTime)
		{
			NativeMethods.criAtomEx_SetBufferingTime_WEBAUDIO(bufferingTime);
		}

		[Serializable]
		public unsafe partial struct ConfigWEBAUDIO
		{
			public CriAtomEx.Config atomEx;

			public CriAtomExAsr.Config asr;

			public NativeBool initializeHcaMx;

			public CriAtomExHcaMx.Config hcaMx;

			public CriAtomEx.MixerType_WEBAUDIO defaultMixerType;

		}
		public enum MixerType_WEBAUDIO
		{
			Asr = 0,
			Webaudio = 1,
		}
	}
}