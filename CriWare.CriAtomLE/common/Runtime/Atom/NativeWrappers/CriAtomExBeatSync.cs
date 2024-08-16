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
	/// <summary>CriAtomExBeatSync API</summary>
	public static partial class CriAtomExBeatSync
	{
		/// <summary>ビート同期情報構造体</summary>
		public unsafe partial struct Info
		{
			/// <summary>プレーヤーオブジェクト</summary>
			public IntPtr player;

			/// <summary>再生ID</summary>
			public UInt32 playbackId;

			/// <summary>小節数</summary>
			public UInt32 barCount;

			/// <summary>拍数</summary>
			public UInt32 beatCount;

			/// <summary>拍の進捗(0.0f～1.0f)</summary>
			public Single beatProgress;

			/// <summary>テンポ(拍/分)</summary>
			public Single bpm;

			/// <summary>同期オフセット(ms)</summary>
			public Int32 offset;

			/// <summary>拍子数</summary>
			public UInt32 numBeats;

			/// <summary>ビート同期ラベル</summary>
			public NativeString label;

		}
		/// <summary>ビート同期位置検出コールバック関数の登録</summary>
		/// <param name="func">ビート同期位置検出コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ビート同期位置検出情報を受け取るコールバック関数を登録します。
		/// 登録されたコールバック関数は、サーバー関数内でビート同期位置検出を処理されるタイミングで実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExBeatSync.CbFunc"/>
		public static unsafe void SetCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomExBeatSync.Info*, Int32> func, IntPtr obj)
		{
			NativeMethods.criAtomExBeatSync_SetCallback((IntPtr)func, obj);
		}
		static unsafe void SetCallbackInternal(IntPtr func, IntPtr obj) => SetCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomExBeatSync.Info*, Int32>)func, obj);
		static CriAtomExBeatSync.CbFunc _callback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetCallback" />
		public static CriAtomExBeatSync.CbFunc Callback => _callback ?? (_callback = new CriAtomExBeatSync.CbFunc(SetCallbackInternal));

		/// <summary>ビート同期位置検出コールバック</summary>
		/// <returns>
		/// 
		/// AtomExライブラリのビート同期位置検出コールバック関数型です。
		/// コールバック関数の登録には <see cref="CriAtomExBeatSync.SetCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、サーバー関数内でビート同期位置検出が処理されるタイミングで実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </returns>
		/// <remarks>
		/// <para>説明:</para>
		/// </remarks>
		/// <seealso cref="CriAtomExBeatSync.SetCallback"/>
		public unsafe class CbFunc : NativeCallbackBase<CbFunc.Arg, Int32>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ビート同期情報</summary>
				public NativeReference<CriAtomExBeatSync.Info> info { get; }

				internal Arg(NativeReference<CriAtomExBeatSync.Info> info)
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
			static Int32 CallbackFunc(IntPtr obj, CriAtomExBeatSync.Info* info) =>
				InvokeCallbackInternal(obj, new(info));
#if !NET5_0_OR_GREATER
			delegate Int32 NativeDelegate(IntPtr obj, CriAtomExBeatSync.Info* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal CbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomExBeatSync.Info*, Int32>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
	}
}