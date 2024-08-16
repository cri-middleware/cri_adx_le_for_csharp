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
		/// <summary>サーバスレッドプライオリティの設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRIサーバスレッドのプライオリティを設定します。
		/// 引数 prio は pthread のプライオリティ設定値として使用します。
		/// プライオリティ設定値はメインスレッドからの相対値になります。
		/// アプリケーションのメインスレッド(0)よりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は16です。
		/// </para>
		/// <para>
		/// 注意:
		/// ::criAtom_Initialize_IOS 関数実行前に本関数を実行することはできません。
		/// サーバ処理スレッドは、CRI File Systemライブラリでも利用されています。
		/// すでにCRI File SystemライブラリのAPIでサーバ処理スレッドの設定を変更している場合
		/// 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// </remarks>
		public static void SetServerThreadPriorityIOS(Int32 prio)
		{
			NativeMethods.criAtom_SetServerThreadPriority_IOS(prio);
		}

		/// <summary>サウンド処理の再開</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionのInterruption Callbak関数から呼び出すための関数です。
		/// サウンド処理を再開します。
		/// 本関数を呼び出す前に、AudioSessionのパメラータ設定とアクティベイトを行ってください。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.StartSoundIOS"/> 関数をご利用ください。
		/// </para>
		/// <para>
		/// 注意:
		/// ::criAtom_Initialize_IOS 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.StopSoundIOS"/>
		public static void StartSoundIOS()
		{
			NativeMethods.criAtom_StartSound_IOS();
		}

		/// <summary>サウンド処理の停止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionのInterruption Callbak関数から呼び出すための関数です。
		/// サウンド処理を停止します。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.StopSoundIOS"/> 関数をご利用ください。
		/// </para>
		/// <para>
		/// 注意:
		/// ::criAtom_Initialize_IOS 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.StartSoundIOS"/>
		public static void StopSoundIOS()
		{
			NativeMethods.criAtom_StopSound_IOS();
		}

		/// <summary>サウンドの復旧</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionAddPropertyListener Callback関数から呼び出すための関数です。
		/// ライブラリ内部のボイスを復旧します。
		/// iOSのデーモンであるmediaserverdが死亡した際には、ライブラリ内のボイスが無効なボイスになり、
		/// 再生成が必要になります。
		/// このように、ボイスの復旧が必要な際に呼び出してください。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.RecoverSoundIOS"/> 関数をご利用ください。
		/// </para>
		/// </remarks>
		public static void RecoverSoundIOS()
		{
			NativeMethods.criAtom_RecoverSound_IOS();
		}

		/// <summary>サウンドの初期化に成功したか否か</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドライブラリの初期化に成功したか否かを返す関数です。
		/// iOSでは、アプリがバックグラウンドにある状態でサウンドライブラリの初期化を行った場合に
		/// 内部的にAudioSessionの初期化等に失敗している場合があります。
		/// 本関数で初期化が失敗していることを確認した場合は、アプリがフォアグラウンドにある状態で
		/// 再度ライブラリの初期化を行うか、 <see cref="CriAtom.RecoverSoundIOS"/> を用いて
		/// サウンドの復旧を行う必要があります。
		/// 本関数は下位レイヤ向けのAPIです。
		/// AtomExレイヤの機能を利用する際には、本関数の代わりに
		/// <see cref="CriAtomEx.IsInitializationSucceededIOS"/> 関数をご利用ください。
		/// </para>
		/// </remarks>
		public static bool IsInitializationSucceededIOS()
		{
			return NativeMethods.criAtom_IsInitializationSucceeded_IOS();
		}

		/// <summary>AudioSessionの設定</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンフィグに従ってAudioSessionの設定を行います。
		/// より詳細な設定を行いたい場合はこの関数を呼び出さず、AudioSessionの各種APIを用いて設定してください。
		/// </para>
		/// </remarks>
		public static unsafe void SetupAudioSessionIOS(ref CriAtom.AudioSessionConfigIOS config)
		{
			fixed (CriAtom.AudioSessionConfigIOS* configPtr = &config)
				NativeMethods.criAtom_SetupAudioSession_IOS(configPtr);
		}

		/// <summary>AudioSession設定用コンフィグ構造体</summary>
		/// <seealso cref="CriAtom.SetupAudioSessionIOS"/>
		public unsafe partial struct AudioSessionConfigIOS
		{
			/// <summary>マイクデバイスを使用するか</summary>
			public NativeBool enableMicrophone;

			/// <summary>バックグラウンド再生を許可するか</summary>
			public NativeBool enableBackgroundAudio;

		}
		/// <summary>MP3プレーヤ作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// MP3が再生可能なプレーヤを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomPlayer.CreateMp3PlayerIOS"/> 関数の引数に指定します。
		/// 作成されるプレーヤは、オブジェクト作成時に本構造体で指定された設定に応じて、
		/// 内部リソースを必要なだけ確保します。
		/// プレーヤが必要とするワーク領域のサイズは、本構造体で指定されたパラメータに応じて変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、
		/// ::criAtomPlayer_SetDefaultConfigForMp3Player_IOS メソッドで必ず構造体を初期化してください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateMp3PlayerIOS"/>
		public unsafe partial struct Mp3PlayerConfigIOS
		{
			public Int32 maxChannels;

			public Int32 maxSamplingRate;

			public NativeBool streamingFlag;

			/// <summary>サウンドレンダラタイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤー、またはASRがが内部で作成するサウンドレンダラの種別を指定するためのデータ型です。
			/// AtomプレーヤーやASR作成時にコンフィグ構造体のパラメーターとして指定します。
			/// </para>
			/// <para>
			/// 注意:
			/// <see cref="CriAtom.SoundRendererType.Any"/> は <see cref="CriAtomExPlayer.SetSoundRendererType"/> 関数に対してのみ指定可能です。
			/// ボイスプール作成時には使用できません。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.AdxPlayerConfig"/>
			/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
			/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
			public CriAtom.SoundRendererType soundRendererType;

		}
	}
}