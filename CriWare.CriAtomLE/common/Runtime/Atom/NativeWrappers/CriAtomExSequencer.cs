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
	/// <summary>CriAtomExSequencer API</summary>
	public static partial class CriAtomExSequencer
	{
		/// <summary>シーケンスコールバック関数の登録</summary>
		/// <param name="func">シーケンスコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// シーケンスデータに埋め込まれたコールバック情報を受け取るコールバック関数を登録します。
		/// 登録されたコールバック関数は、サーバー関数内でコールバックイベントを処理したタイミングで実行されます。
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
		/// <seealso cref="CriAtomExSequencer.EventCbFunc"/>
		public static unsafe void SetEventCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.SequenceEventInfo*, Int32> func, IntPtr obj)
		{
			NativeMethods.criAtomExSequencer_SetEventCallback((IntPtr)func, obj);
		}
		static unsafe void SetEventCallbackInternal(IntPtr func, IntPtr obj) => SetEventCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.SequenceEventInfo*, Int32>)func, obj);
		static CriAtomExSequencer.EventCbFunc _eventCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetEventCallback" />
		public static CriAtomExSequencer.EventCbFunc EventCallback => _eventCallback ?? (_eventCallback = new CriAtomExSequencer.EventCbFunc(SetEventCallbackInternal));

		/// <summary>シーケンスコールバック</summary>
		/// <returns>
		/// 
		/// AtomExライブラリのシーケンスコールバック関数型です。
		/// コールバック関数の登録には <see cref="CriAtomExSequencer.SetEventCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、サーバー関数内でシーケンスが処理されるタイミングで実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </returns>
		/// <remarks>
		/// <para>説明:</para>
		/// </remarks>
		/// <seealso cref="CriAtomExSequencer.SetEventCallback"/>
		public unsafe class EventCbFunc : NativeCallbackBase<EventCbFunc.Arg, Int32>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>シーケンスイベント情報</summary>
				public NativeReference<CriAtomEx.SequenceEventInfo> info { get; }

				internal Arg(NativeReference<CriAtomEx.SequenceEventInfo> info)
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
			static Int32 CallbackFunc(IntPtr obj, CriAtomEx.SequenceEventInfo* info) =>
				InvokeCallbackInternal(obj, new(info));
#if !NET5_0_OR_GREATER
			delegate Int32 NativeDelegate(IntPtr obj, CriAtomEx.SequenceEventInfo* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal EventCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomEx.SequenceEventInfo*, Int32>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
	}
}