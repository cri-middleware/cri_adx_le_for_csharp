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
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ 構造体（ <see cref="CriAtom.ConfigIOS"/> ）の内容によって変化します。
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.CalculateWorkSizeIOS"/> 関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 CRIAPI criAtom_CalculateWorkSize_IOS(const CriAtomConfig_IOS *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigIOS"/>
		/// <seealso cref="CriAtom.InitializeIOS"/>
		public static unsafe Int32 CalculateWorkSizeIOS(in CriAtom.ConfigIOS config)
		{
			fixed (CriAtom.ConfigIOS* configPtr = &config)
				return NativeMethods.criAtom_CalculateWorkSize_IOS(configPtr);
		}

		/// <summary>ライブラリの初期化 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		///  ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		///  （ライブラリの機能は、本関数を実行後、 <see cref="CriAtom.FinalizeIOS"/> 関数を実行するまでの間、 利用可能です。）
		///  ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて 変化します。
		///  ワーク領域サイズの計算には、 <see cref="CriAtom.CalculateWorkSizeIOS"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケータを登録済みの場合、 本関数にワーク領域を指定する必要はありません。
		///  （ work に null 、 work_size に 0 を指定することで、登録済みのアロケータ から必要なワーク領域サイズ分のメモリが動的に確保されます。） 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// <list type="bullet">
		/// <item><description><see cref="CriAtom.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomAsr.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Initialize"/> 本関数を実行する場合、上記関数を実行しないでください。
		///  本関数を実行後、必ず対になる <see cref="CriAtom.FinalizeIOS"/> 関数を実行してください。
		///  また、 <see cref="CriAtom.FinalizeIOS"/> 関数を実行するまでは、本関数を再度実行しないでください。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.InitializeIOS"/> 関数をご利用ください。 </description></item>
		/// </list>
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Initialize_IOS(const CriAtomConfig_IOS *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigIOS"/>
		/// <seealso cref="CriAtom.FinalizeIOS"/>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtom.CalculateWorkSizeIOS"/>
		public static unsafe void InitializeIOS(in CriAtom.ConfigIOS config)
		{
			fixed (CriAtom.ConfigIOS* configPtr = &config)
				NativeMethods.criAtom_Initialize_IOS(configPtr, default, default);
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
		/// <item><description><see cref="CriAtom.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomAsr.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Finalize"/> 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtom.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.FinalizeIOS"/> 関数をご利用ください。 </description></item>
		/// </list>
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Finalize_IOS(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeIOS"/>
		public static void FinalizeIOS()
		{
			NativeMethods.criAtom_Finalize_IOS();
		}

		/// <summary>サーバスレッドプライオリティの設定 </summary>
		/// <param name="prio">スレッドのプライオリティ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRIサーバスレッドのプライオリティを設定します。
		///  引数 prio は pthread のプライオリティ設定値として使用します。
		///  プライオリティ設定値はメインスレッドからの相対値になります。
		///  アプリケーションのメインスレッド(0)よりも高いプライオリティを指定してください。
		///  プライオリティのデフォルト値は16です。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtom.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		///  サーバ処理スレッドは、CRI File Systemライブラリでも利用されています。
		///  すでにCRI File SystemライブラリのAPIでサーバ処理スレッドの設定を変更している場合 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetServerThreadPriority_IOS(int prio)"/>
		/// </remarks>
		public static void SetServerThreadPriorityIOS(Int32 prio)
		{
			NativeMethods.criAtom_SetServerThreadPriority_IOS(prio);
		}

		/// <summary>サウンド処理の再開 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionのInterruption Callbak関数から呼び出すための関数です。
		///  サウンド処理を再開します。
		///  本関数を呼び出す前に、AudioSessionのパメラータ設定とアクティベイトを行ってください。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.StartSoundIOS"/> 関数をご利用ください。 
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtom.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_StartSound_IOS(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.StopSoundIOS"/>
		public static void StartSoundIOS()
		{
			NativeMethods.criAtom_StartSound_IOS();
		}

		/// <summary>サウンド処理の停止 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionのInterruption Callbak関数から呼び出すための関数です。
		///  サウンド処理を停止します。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.StopSoundIOS"/> 関数をご利用ください。 
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtom.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_StopSound_IOS(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.StartSoundIOS"/>
		public static void StopSoundIOS()
		{
			NativeMethods.criAtom_StopSound_IOS();
		}

		/// <summary>サウンドの復旧 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionAddPropertyListener Callback関数から呼び出すための関数です。
		///  ライブラリ内部のボイスを復旧します。
		///  iOSのデーモンであるmediaserverdが死亡した際には、ライブラリ内のボイスが無効なボイスになり、 再生成が必要になります。
		///  このように、ボイスの復旧が必要な際に呼び出してください。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.RecoverSoundIOS"/> 関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_RecoverSound_IOS(void)"/>
		/// </remarks>
		public static void RecoverSoundIOS()
		{
			NativeMethods.criAtom_RecoverSound_IOS();
		}

		/// <summary>サウンドの初期化に成功したか否か </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドライブラリの初期化に成功したか否かを返す関数です。
		///  iOSでは、アプリがバックグラウンドにある状態でサウンドライブラリの初期化を行った場合に 内部的にAudioSessionの初期化等に失敗している場合があります。
		///  本関数で初期化が失敗していることを確認した場合は、アプリがフォアグラウンドにある状態で 再度ライブラリの初期化を行うか、 <see cref="CriAtom.RecoverSoundIOS"/> を用いて サウンドの復旧を行う必要があります。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.IsInitializationSucceededIOS"/> 関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="CriBool CRIAPI criAtom_IsInitializationSucceeded_IOS(void)"/>
		/// </remarks>
		public static bool IsInitializationSucceededIOS()
		{
			return NativeMethods.criAtom_IsInitializationSucceeded_IOS();
		}

		/// <summary>AudioSessionの設定 </summary>
		/// <param name="config">コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンフィグに従ってAudioSessionの設定を行います。
		///  より詳細な設定を行いたい場合はこの関数を呼び出さず、AudioSessionの各種APIを用いて設定してください。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetupAudioSession_IOS(CriAtomAudioSessionConfig_IOS *config)"/>
		/// </remarks>
		public static unsafe void SetupAudioSessionIOS(in CriAtom.AudioSessionConfigIOS config)
		{
			fixed (CriAtom.AudioSessionConfigIOS* configPtr = &config)
				NativeMethods.criAtom_SetupAudioSession_IOS(configPtr);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 注意:
		/// 本構造体は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本構造体の代わりに <see cref="CriAtomEx.ConfigIOS"/> 構造体をご利用ください。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeIOS"/>
		[Serializable]
		public unsafe partial struct ConfigIOS
		{
			/// <summary>ライブラリ初期化用コンフィグ構造体 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリの動作仕様を指定するための構造体です。
			/// <see cref="CriAtom.Initialize"/> 関数の引数に指定します。
			///  CRI Atomライブラリは、初期化時に本構造体で指定された設定に応じて、内部リソースを 必要なだけ確保します。
			///  ライブラリが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて 変化します。 
			/// </para>
			/// <para>
			/// 備考:
			/// デフォルト設定を使用する場合、 <see cref="CriAtom.SetDefaultConfig"/> メソッドで構造体にデフォルト パラメーターをセットした後、 <see cref="CriAtom.Initialize"/> 関数に構造体を指定してください。
			/// </para>
			/// <para>
			/// 注意:
			/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtom.SetDefaultConfig"/> メソッドで必ず構造体を初期化してください。
			///  （構造体のメンバに不定値が入らないようご注意ください。） 
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.Initialize"/>
			/// <seealso cref="CriAtom.SetDefaultConfig"/>
			public CriAtom.Config atom;

			/// <summary>ASR初期化用コンフィグ構造体</summary>
			/// <remarks>
			/// <para>
			/// 備考:
			/// デフォルト設定を使用する場合、 <see cref="CriAtomAsr.SetDefaultConfig"/> メソッドで 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomAsr.Initialize"/> 関数 に構造体を指定してください。
			/// </para>
			/// <para>
			/// 注意:
			/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomAsr.SetDefaultConfig"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
			///  （構造体のメンバに不定値が入らないようご注意ください。） 
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomAsr.Initialize"/>
			/// <seealso cref="CriAtomAsr.SetDefaultConfig"/>
			public CriAtomAsr.Config asr;

			/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
			/// <remarks>
			/// <para>
			/// 備考:
			/// デフォルト設定を使用する場合、 <see cref="CriAtomHcaMx.SetDefaultConfig"/> メソッドで 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomHcaMx.Initialize"/> 関数 に構造体を指定してください。
			/// </para>
			/// <para>
			/// 注意:
			/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomHcaMx.SetDefaultConfig"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
			///  （構造体のメンバに不定値が入らないようご注意ください。） 
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomHcaMx.Initialize"/>
			/// <seealso cref="CriAtomHcaMx.SetDefaultConfig"/>
			public CriAtomHcaMx.Config hcaMx;

			public UInt32 bufferingTime;

			public Int32 outputSamplingRate;

		}
		/// <summary>AudioSession設定用コンフィグ構造体</summary>
		/// <seealso cref="CriAtom.SetupAudioSessionIOS"/>
		[Serializable]
		public unsafe partial struct AudioSessionConfigIOS
		{
			public NativeBool enableMicrophone;

			public NativeBool enableBackgroundAudio;

		}
		/// <summary></summary>
		/// <remarks>
		/// <para>MP3プレーヤ作成用コンフィグ構造体</para>
		/// </remarks>
		[Serializable]
		public unsafe partial struct Mp3PlayerConfigIOS
		{
			public Int32 maxChannels;

			public Int32 maxSamplingRate;

			public NativeBool streamingFlag;

			/// <summary>サウンドレンダラタイプ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤー、またはASRがが内部で作成するサウンドレンダラの種別を指定するためのデータ型です。
			///  AtomプレーヤーやASR作成時にコンフィグ構造体のパラメーターとして指定します。 
			/// </para>
			/// <para>
			/// 注意:
			/// <see cref="CriAtom.SoundRendererType.Any"/> は <see cref="CriAtomExPlayer.SetSoundRendererType"/> 関数に対してのみ指定可能です。
			///  ボイスプール作成時には使用できません。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.AdxPlayerConfig"/>
			/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
			/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
			public CriAtom.SoundRendererType soundRendererType;

		}
	}
}