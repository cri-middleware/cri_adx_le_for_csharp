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
	/// <summary>CriAtomExMonitor API</summary>
	public static partial class CriAtomExMonitor
	{
		/// <summary>Atomモニターライブラリ初期化コンフィグ構造体にデフォルト値をセット </summary>
		/// <param name="pConfig">コンフィグ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数に設定するコンフィグ構造体 （ <see cref="CriAtomExMonitor.Config"/> ）に、デフォルト値をセットします。
		/// </para>
		/// <nativeinfo declaration="void criAtomExMonitor_SetDefaultConfig_(CriAtomExMonitorConfig *p_config)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		/// <seealso cref="CriAtomExMonitor.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomExMonitor.Config pConfig)
		{
			fixed (CriAtomExMonitor.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExMonitor_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>モニター機能初期化用ワーク領域サイズの計算 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <returns>CriSint32 ワーク領域サイズ </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// モニター機能を使用するために必要な、ワーク領域のサイズを取得します。
		///  ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		///  ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// モニター機能が必要とするワーク領域のサイズは、モニター機能初期化用コンフィグ 構造体（ <see cref="CriAtomExMonitor.Config"/> ）の内容によって変化します。
		///  引数にnullを指定した場合、デフォルト設定 （ <see cref="CriAtomExMonitor.SetDefaultConfig"/> 適用時と同じパラメーター）で ワーク領域サイズを計算します。 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomExMonitor_CalculateWorkSize(const CriAtomExMonitorConfig *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.SetDefaultConfig"/>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		/// <seealso cref="CriAtomExMonitor.Config"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExMonitor.Config config)
		{
			fixed (CriAtomExMonitor.Config* configPtr = &config)
				return NativeMethods.criAtomExMonitor_CalculateWorkSize(configPtr);
		}

		/// <summary>Atomモニターライブラリ初期化用コンフィグ構造体 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// モニターライブラリの動作仕様を指定するための構造体です。
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数の引数に指定します。
		///  CRI AtomEx Monitorライブラリは、初期化時に本構造体で指定された設定に応じて、内部リソースを 必要なだけ確保します。
		///  ライブラリが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて 変化します。 
		/// </para>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomExMonitor.SetDefaultConfig"/> メソッドで構造体にデフォルト パラメーターをセットした後、 <see cref="CriAtomExMonitor.Initialize"/> 関数に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExMonitor.SetDefaultConfig"/> メソッドを使用しない 場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		/// <seealso cref="CriAtomExMonitor.SetDefaultConfig"/>
		[Serializable]
		public unsafe partial struct Config
		{
			/// <summary>インゲームプレビュー用管理リソースの最大値 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ここで指定する管理リソースはインゲームプレビュー時に ACB オブジェクト１つに対し１つ消費されます。
			///  同時に使用する ACB 数より大きな値を設定するようにしてください。
			/// </para>
			/// </remarks>
			public UInt32 maxPreivewObject;

			/// <summary>通信用バッファサイズ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// モニタライブラリとオーサリングツール間で行う通信に使用するバッファサイズを指定します。
			///  バッファは送信用、受信用の2つがあり、それぞれ設定値の半分の拡張領域がさらに付加されます。
			///  このため実際に必要なバッファサイズは設定値の3倍となります。
			/// </para>
			/// </remarks>
			public UInt32 communicationBufferSize;

			/// <summary>追加バッファ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// TCP/IP接続以外の接続を行う等、通信バッファを外部から指定する必要がある場合に使用します。
			///  特に指定がない場合は使用しません。
			/// </para>
			/// </remarks>
			public IntPtr additionalBuffer;

			/// <summary>追加バッファサイズ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 外部指定の通信バッファサイズです。
			/// </para>
			/// </remarks>
			public UInt32 additionalBufferSize;

			/// <summary>再生位置情報更新間隔 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// サーバ処理実行時に再生位置情報の送信処理を行う間隔を指定します。
			///  playback_position_update_interval の値を変更することで、 サーバ処理の実行回数を変えることなく再生位置情報の送信頻度を下げることが可能です。
			///  例えば、 playback_position_update_interval を 2 に設定すると、 サーバ処理 2 回に対し、 1 回だけ再生位置情報の送信が行われます。
			///  （再生位置情報の送信頻度が 1/2 になります。）
			/// </para>
			/// </remarks>
			public Int32 playbackPositionUpdateInterval;

		}
		/// <summary>モニター機能の初期化 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// モニター機能を初期化します。
		///  モニター機能を利用するには、必ずこの関数を実行する必要があります。
		///  （モニター機能は、本関数を実行後、 <see cref="CriAtomExMonitor.Finalize"/> 関数を実行するまでの間、 利用可能です。）
		///  本関数の呼び出しは、<see cref="CriAtomEx.Initialize"/> 関数実行後 <see cref="CriAtomEx.Finalize"/> 関数を実行するまでの間に 行うようにしてください。
		/// </para>
		/// <nativeinfo declaration="void criAtomExMonitor_Initialize(const CriAtomExMonitorConfig *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Finalize"/>
		/// <seealso cref="CriAtomExMonitor.Config"/>
		public static unsafe void Initialize(in CriAtomExMonitor.Config config)
		{
			fixed (CriAtomExMonitor.Config* configPtr = &config)
				NativeMethods.criAtomExMonitor_Initialize(configPtr, default, default);
		}

		/// <summary>モニター機能の終了 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// モニター機能を終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void criAtomExMonitor_Finalize(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		public static void Finalize()
		{
			NativeMethods.criAtomExMonitor_Finalize();
		}

		/// <summary>サーバーIPアドレス文字列の取得 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバーIPアドレス文字列を取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="const CriChar8* criAtomExMonitor_GetServerIpString(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		/// <seealso cref="CriAtomExMonitor.GetClientIpString"/>
		public static NativeString GetServerIpString()
		{
			return NativeMethods.criAtomExMonitor_GetServerIpString();
		}

		/// <summary>クライアントIPアドレス文字列の取得 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// クライアントIPアドレス文字列を取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="const CriChar8* criAtomExMonitor_GetClientIpString(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		/// <seealso cref="CriAtomExMonitor.GetServerIpString"/>
		public static NativeString GetClientIpString()
		{
			return NativeMethods.criAtomExMonitor_GetClientIpString();
		}

		/// <summary>ツール接続状態の取得 </summary>
		/// <returns>CriBool 接続状態（true:接続、false:未接続） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Craft 接続状態を取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExMonitor_IsConnected(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		public static bool IsConnected()
		{
			return NativeMethods.criAtomExMonitor_IsConnected();
		}

		/// <summary>プロファイラー接続状態の取得 </summary>
		/// <returns>CriBool 接続状態（true:接続、false:未接続） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プロファイラー接続状態を取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExMonitor_IsConnectedToProfiler(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		public static bool IsConnectedToProfiler()
		{
			return NativeMethods.criAtomExMonitor_IsConnectedToProfiler();
		}

		/// <summary>ログ取得コールバックの登録 </summary>
		/// <param name="cbf">コールバック関数 </param>
		/// <param name="obj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ログ取得用コールバック関数を登録します。
		///  関数を登録するとログ取得を開始し、nullを設定することでログ取得を停止します。
		///  取得するログのモード切替は <see cref="CriAtomExMonitor.SetLogMode"/> 関数で設定指定ください。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void criAtomExMonitor_SetLogCallback(CriAtomExMonitorLogCbFunc cbf, void *obj)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.Initialize"/>
		/// <seealso cref="CriAtomExMonitor.SetLogMode"/>
		public static unsafe void SetLogCallback(delegate* unmanaged[Cdecl]<IntPtr, NativeString, void> cbf, IntPtr obj)
		{
			NativeMethods.criAtomExMonitor_SetLogCallback((IntPtr)cbf, obj);
		}
		static unsafe void SetLogCallbackInternal(IntPtr func, IntPtr obj) => SetLogCallback((delegate* unmanaged[Cdecl]<IntPtr, NativeString, void>)func, obj);
		static CriAtomExMonitor.LogCbFunc _logCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetLogCallback" />
		public static CriAtomExMonitor.LogCbFunc LogCallback => _logCallback ?? (_logCallback = new CriAtomExMonitor.LogCbFunc(SetLogCallbackInternal));

		public unsafe class LogCbFunc : NativeCallbackBase<LogCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				public NativeString logString { get; }

				internal Arg(NativeString logString)
				{
					this.logString = logString;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CriAtomExMonitorLogCbFuncCallbackFunc(IntPtr obj, NativeString logString) =>
				InvokeCallbackInternal(obj, new(logString));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr obj, NativeString logString);
			static NativeDelegate callbackDelegate = null;
#endif
			internal LogCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, NativeString, void>)&CriAtomExMonitorLogCbFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomExMonitorLogCbFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ログモードの設定 </summary>
		/// <param name="mode">ログ出力モード </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ログ取得のモードを設定します。
		///  本関数で設定したモードにしたがって <see cref="CriAtomExMonitor.SetLogCallback"/> 関数で 登録したログ取得用コールバック関数が呼び出されます。
		/// </para>
		/// <nativeinfo declaration="void criAtomExMonitor_SetLogMode(CriUint32 mode)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.SetLogCallback"/>
		public static void SetLogMode(UInt32 mode)
		{
			NativeMethods.criAtomExMonitor_SetLogMode(mode);
		}

		/// <summary>ユーザログの出力 </summary>
		/// <param name="message">ユーザログメッセージ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ユーザログを出力します。
		///  本関数によって出力したログはログ取得コールバックやプロファイラで取得、確認が行えます。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExMonitor_OutputUserLog(const CriChar8 *message)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.SetLogCallback"/>
		public static void OutputUserLog(ArgString message)
		{
			NativeMethods.criAtomExMonitor_OutputUserLog(message.GetPointer(stackalloc byte[message.BufferSize]));
		}

		/// <summary>データ更新通知コールバック関数の登録 </summary>
		/// <param name="func">データ更新通知コールバック関数 </param>
		/// <param name="obj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インゲームプレビュー時にオーサリングツールからのデータ更新処理が発生した場合に呼び出すコールバック関数を登録します。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		///  コールバック関数はAtomMonitorライブラリ内のサーバ処理からも実行されます。
		///  そのため、サーバ処理への割り込みを考慮しないAPIを実行した場合、 エラーが発生したり、デッドロックが発生する可能性があります。
		///  コールバック関数内で長時間処理をブロックすると、音切れ等の問題 が発生しますので、ご注意ください。
		///  コールバック関数は1つしか登録できません。
		///  登録操作を複数回行った場合、既に登録済みのコールバック関数が、 後から登録したコールバック関数により上書きされてしまいます。
		///  funcにnullを指定するとことで登録済み関数の登録解除が行えます。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExMonitor.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void criAtomExMonitor_SetDataUpdateNotificationCallback(CriAtomExMonitorDataUpdateNotificationCbFunc func, void *obj)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.DataUpdateNotificationCbFunc"/>
		/// <seealso cref="CriAtomExMonitor.DataUpdateNotificationInfo"/>
		public static unsafe void SetDataUpdateNotificationCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomExMonitor.DataUpdateNotificationInfo*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExMonitor_SetDataUpdateNotificationCallback((IntPtr)func, obj);
		}
		static unsafe void SetDataUpdateNotificationCallbackInternal(IntPtr func, IntPtr obj) => SetDataUpdateNotificationCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomExMonitor.DataUpdateNotificationInfo*, void>)func, obj);
		static CriAtomExMonitor.DataUpdateNotificationCbFunc _dataUpdateNotificationCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetDataUpdateNotificationCallback" />
		public static CriAtomExMonitor.DataUpdateNotificationCbFunc DataUpdateNotificationCallback => _dataUpdateNotificationCallback ?? (_dataUpdateNotificationCallback = new CriAtomExMonitor.DataUpdateNotificationCbFunc(SetDataUpdateNotificationCallbackInternal));

		/// <summary>オーサリングツールによるデータ更新通知情報取得コールバック関数 </summary>
		/// <returns>なし </returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// インゲームプレビュー時にオーサリングツールからのデータ更新処理が発生した場合に呼び出すコールバック関数です。
		///  インゲームプレビュー時にアプリケーション側でデータ更新状態を取得したい場合に使用します。
		///  コールバック関数の登録には <see cref="CriAtomExMonitor.SetDataUpdateNotificationCallback"/> 関数を使用します。
		///  登録したコールバック関数は、インゲームプレビュー中にオーサリングツールからのデータ更新前後のタイミングで実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		///  本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、 ご注意ください。
		///  コールバック関数に引数として渡される<see cref="CriAtomExMonitor.DataUpdateNotificationInfo"/>構造体への参照はコールバック関数内だけで行ってください。
		///  コールバック関数外で参照する場合は、別領域に内容を保存してから行ってください。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.SetDataUpdateNotificationCallback"/>
		/// <seealso cref="CriAtomExMonitor.DataUpdateNotificationInfo"/>
		public unsafe class DataUpdateNotificationCbFunc : NativeCallbackBase<DataUpdateNotificationCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>データ更新通知情報取得 </summary>
				public NativeReference<CriAtomExMonitor.DataUpdateNotificationInfo> info { get; }

				internal Arg(NativeReference<CriAtomExMonitor.DataUpdateNotificationInfo> info)
				{
					this.info = info;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CriAtomExMonitorDataUpdateNotificationCbFuncCallbackFunc(IntPtr obj, CriAtomExMonitor.DataUpdateNotificationInfo* info) =>
				InvokeCallbackInternal(obj, new(info));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr obj, CriAtomExMonitor.DataUpdateNotificationInfo* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal DataUpdateNotificationCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomExMonitor.DataUpdateNotificationInfo*, void>)&CriAtomExMonitorDataUpdateNotificationCbFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomExMonitorDataUpdateNotificationCbFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>データ更新情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インゲームプレビュー時のAtomCraft（オーサリングツール）によるデータ更新情報です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.SetDataUpdateNotificationCallback"/>
		public unsafe partial struct DataUpdateNotificationInfo
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>更新ターゲット </para>
			/// </remarks>
			public CriAtomExMonitor.DataUpdateTarget target;

			/// <summary></summary>
			/// <remarks>
			/// <para>イベント </para>
			/// </remarks>
			public CriAtomExMonitor.DataUpdateEvent @event;

			/// <summary></summary>
			/// <remarks>
			/// <para>ACBハンドル </para>
			/// </remarks>
			public IntPtr acbHn;

			/// <summary></summary>
			/// <remarks>
			/// <para>名前 </para>
			/// </remarks>
			public NativeString name;

		}
		/// <summary>データ更新タイプ </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インゲームプレビュー時のAtomCraft（オーサリングツール）によるデータ更新のターゲットです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.DataUpdateNotificationInfo"/>
		public enum DataUpdateTarget
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>ACF更新 </para>
			/// </remarks>
			Acf = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>ACB更新 </para>
			/// </remarks>
			Acb = 1,
		}
		/// <summary>データ更新ステータス </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インゲームプレビュー時のAtomCraft（オーサリングツール）によるデータ更新の状態です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExMonitor.DataUpdateNotificationInfo"/>
		public enum DataUpdateEvent
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>開始イベント </para>
			/// </remarks>
			Begin = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>終了イベント </para>
			/// </remarks>
			End = 1,
		}
	}
}