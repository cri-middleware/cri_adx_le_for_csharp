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
	/// <summary>CriAtomEx API</summary>
	public static partial class CriAtomEx
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
		/// 構造体（ <see cref="CriAtomEx.ConfigIOS"/> ）の内容によって変化します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.ConfigIOS"/> 構造体のacf_infoメンバに値を設定している場合、本関数は失敗し-1を返します。
		/// 初期化処理内でACFデータの登録を行う場合は、本関数値を使用したメモリ確保ではなくADXシステムによる
		/// メモリアロケータを使用したメモリ確保処理が必要になります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigIOS"/>
		/// <seealso cref="CriAtomEx.InitializeIOS"/>
		public static unsafe Int32 CalculateWorkSizeIOS(in CriAtomEx.ConfigIOS config)
		{
			fixed (CriAtomEx.ConfigIOS* configPtr = &config)
				return NativeMethods.criAtomEx_CalculateWorkSize_IOS(configPtr);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <seealso cref="CriAtomEx.InitializeIOS"/>
		public unsafe partial struct ConfigIOS
		{
			/// <summary>AtomEx初期化用コンフィグ構造体</summary>
			public CriAtomEx.Config atomEx;

			/// <summary>ASR初期化用コンフィグ</summary>
			public CriAtomExAsr.Config asr;

			/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
			public CriAtomExHcaMx.Config hcaMx;

			/// <summary>出力バッファリング時間(単位:msec)</summary>
			public UInt32 bufferingTime;

			/// <summary>出力サンプリング周波数</summary>
			public Int32 outputSamplingRate;

			/// <summary>OSからの通知対応処理を行う</summary>
			public NativeBool useHandlingOsNotifications;

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
		/// （ライブラリの機能は、本関数を実行後、 <see cref="CriAtomEx.FinalizeIOS"/> 関数を実行するまでの間、
		/// 利用可能です。）
		/// ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて
		/// 変化します。
		/// ワーク領域サイズの計算には、 <see cref="CriAtomEx.CalculateWorkSizeIOS"/>
		/// 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケータを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケータ
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// - <see cref="CriAtomEx.Initialize"/>
		/// - <see cref="CriAtomExAsr.Initialize"/>
		/// - <see cref="CriAtomExHcaMx.Initialize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// 本関数を実行後、必ず対になる <see cref="CriAtomEx.FinalizeIOS"/> 関数を実行してください。
		/// また、 <see cref="CriAtomEx.FinalizeIOS"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigIOS"/>
		/// <seealso cref="CriAtomEx.FinalizeIOS"/>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeIOS"/>
		public static unsafe void InitializeIOS(in CriAtomEx.ConfigIOS config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx.ConfigIOS* configPtr = &config)
				NativeMethods.criAtomEx_Initialize_IOS(configPtr, work, workSize);
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
		/// - <see cref="CriAtomEx.Finalize"/>
		/// - <see cref="CriAtomExAsr.Finalize"/>
		/// - <see cref="CriAtomExHcaMx.Finalize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtomEx.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.InitializeIOS"/>
		public static void FinalizeIOS()
		{
			NativeMethods.criAtomEx_Finalize_IOS();
		}

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
		/// <see cref="CriAtomEx.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		/// サーバ処理スレッドは、CRI File Systemライブラリでも利用されています。
		/// すでにCRI File SystemライブラリのAPIでサーバ処理スレッドの設定を変更している場合
		/// 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// </remarks>
		public static void SetServerThreadPriorityIOS(Int32 prio)
		{
			NativeMethods.criAtomEx_SetServerThreadPriority_IOS(prio);
		}

		/// <summary>サウンド処理の再開</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionのInterruption Callbak関数から呼び出すための関数です。
		/// サウンド処理を再開します。
		/// 本関数を呼び出す前に、AudioSessionのパメラータ設定とアクティベイトを行ってください。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.StopSoundIOS"/>
		public static void StartSoundIOS()
		{
			NativeMethods.criAtomEx_StartSound_IOS();
		}

		/// <summary>サウンド処理の停止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AudioSessionのInterruption Callbak関数から呼び出すための関数です。
		/// サウンド処理を停止します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.InitializeIOS"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.StartSoundIOS"/>
		public static void StopSoundIOS()
		{
			NativeMethods.criAtomEx_StopSound_IOS();
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
		/// </para>
		/// </remarks>
		public static void RecoverSoundIOS()
		{
			NativeMethods.criAtomEx_RecoverSound_IOS();
		}

		/// <summary>サウンドの初期化に成功したか否か</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドライブラリの初期化に成功したか否かを返す関数です。
		/// iOSでは、アプリがバックグラウンドにある状態でサウンドライブラリの初期化を行った場合に
		/// 内部的にAudioSessionの初期化等に失敗している場合があります。
		/// 本関数で初期化が失敗していることを確認した場合は、アプリがフォアグラウンドにある状態で
		/// 再度ライブラリの初期化を行うか、 <see cref="CriAtomEx.RecoverSoundIOS"/> を用いて
		/// サウンドの復旧を行う必要があります。
		/// </para>
		/// </remarks>
		public static bool IsInitializationSucceededIOS()
		{
			return NativeMethods.criAtomEx_IsInitializationSucceeded_IOS();
		}

	}
}