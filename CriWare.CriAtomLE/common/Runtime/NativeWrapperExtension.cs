/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using CriWare.InteropHelpers;
namespace CriWare{
	public partial class CriAtomEx {
		/// <inheritdoc cref="CriAtomEx.RegisterAcfData(nint, int, nint, int)"/>
		public static unsafe void RegisterAcfData(ReadOnlySpan<byte> data){
			fixed(byte* ptr = data)
				RegisterAcfData((IntPtr)ptr, data.Length);
		}
	}

	public partial class CriAtomExAcb {
		/// <inheritdoc cref="CriAtomExAcb.LoadAcbData(nint, int, CriFsBinder, ArgString, nint, int)"/>
		public static unsafe CriAtomExAcb LoadAcbData(ReadOnlySpan<byte> data, CriFsBinder awbBinder, ArgString awbPath){
			fixed(byte* ptr = data)
				return LoadAcbData((IntPtr)ptr, data.Length, awbBinder, awbPath);
		}
	}

	public partial class CriAtomExAsr {

		unsafe class BusFilterPreCbFunc : NativeCallbackBase<BusFilterCbFunc.Arg>
		{
#if ENABLE_IL2CPP
			[AOT.MonoPInvokeCallback(typeof(BusFilterCbFunc.NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, IntPtr* data) =>
				InvokeCallbackInternal(obj, new(format, numChannels, numSamples, data));
#if !NET5_0_OR_GREATER
			static BusFilterCbFunc.NativeDelegate callbackDelegate = null;
#endif
			internal static IntPtr CallbackPointer => 
#if !NET5_0_OR_GREATER
				Marshal.GetFunctionPointerForDelegate(callbackDelegate = CallbackFunc);
#else
				(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)&CallbackFunc;
#endif

			internal BusFilterPreCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction, CallbackPointer){ }
		}

		unsafe class BusFilterPostCbFunc : NativeCallbackBase<BusFilterCbFunc.Arg>
		{
#if ENABLE_IL2CPP
			[AOT.MonoPInvokeCallback(typeof(BusFilterCbFunc.NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
			[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, IntPtr* data) =>
				InvokeCallbackInternal(obj, new(format, numChannels, numSamples, data));
#if !NET5_0_OR_GREATER
			static BusFilterCbFunc.NativeDelegate callbackDelegate = null;
#endif
			internal static IntPtr CallbackPointer => 
#if !NET5_0_OR_GREATER
				Marshal.GetFunctionPointerForDelegate(callbackDelegate = CallbackFunc);
#else
				(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)&CallbackFunc;
#endif
			internal BusFilterPostCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction, CallbackPointer){ }
		}

		/// <summary>
		/// バスフィルターコールバックオブジェクト
		/// </summary>
		public class BusFilterCbSet {
			Action<ArgString, IntPtr, IntPtr, IntPtr> setFunc;
			ArgString busName;
			BusFilterPreCbFunc _pre;
			/// <summary>エフェクト処理前コールバック</summary>
			public unsafe NativeCallbackBase<BusFilterCbFunc.Arg> Pre => 
				_pre ?? (_pre = new BusFilterPreCbFunc((_, obj) => setFunc(busName, BusFilterPreCbFunc.CallbackPointer, BusFilterPostCbFunc.CallbackPointer, obj)));
			BusFilterPostCbFunc _post;
			/// <summary>エフェクト処理後コールバック</summary>
			public unsafe NativeCallbackBase<BusFilterCbFunc.Arg> Post =>
				_post ?? (_post = new BusFilterPostCbFunc((_, obj) => setFunc(busName, BusFilterPreCbFunc.CallbackPointer, BusFilterPostCbFunc.CallbackPointer, obj)));

			internal BusFilterCbSet(Action<ArgString, IntPtr, IntPtr, IntPtr> setFunc, ArgString busName){
				this.setFunc = setFunc;
				this.busName = busName;
			}
		}

		/// <summary>バスフィルターコールバックオブジェクトリスト</summary>
		public class BusFilterCbList {
			Dictionary<string, BusFilterCbSet> instances = new Dictionary<string, BusFilterCbSet>();

			/// <summary>
			/// バス名に対応するコールバックオブジェクトの取得
			/// </summary>
			/// <param name="busName">バス名</param>
			/// <returns>バスフィルターコールバックオブジェクト</returns>
			public BusFilterCbSet this[string busName]{get{
				if(!instances.ContainsKey(busName))
					instances.Add(busName, new BusFilterCbSet(setFunc, busName));
				return instances[busName];
			}}

			Action<ArgString, IntPtr, IntPtr, IntPtr> setFunc;
			internal BusFilterCbList(Action<ArgString, IntPtr, IntPtr, IntPtr> setFunc){
				this.setFunc = setFunc;
			}
		}

		static BusFilterCbList _busFilterCbList;

		/// <summary>バスフィルターコールバック登録用のオブジェクト</summary>
		/// <remarks>
		/// バスフィルターコールバックにはバス毎に異なるコールバック関数を指定できます。
		/// また、エフェクト処理の前後それぞれに異なる関数を指定可能です。
		/// 本プロパティで取得できるオブジェクトは、各コールバックオブジェクトを取得可能なリストとして振る舞います。
		/// 例として、バス名"MasterOut"のバスのエフェクト処理後のコールバックを登録する場合、次のようになります。
		/// <code>
		/// CriWare.CriAtomExAsr.BusFilterCallbackByName["MasterOut"].Post.Event += CallbackMethod;
		///	</code>
		/// </remarks>
		public static BusFilterCbList BusFilterCallbackByName => _busFilterCbList ?? (_busFilterCbList = new BusFilterCbList(SetBusFilterCallbackByNameInternal));

		static unsafe void SetBusFilterCallbackByNameInternal(ArgString busName, IntPtr preFunc, IntPtr postFunc, IntPtr obj)
		{
			if (!CriAtomEx.IsInitialized()) return;
			SetBusFilterCallbackByName(busName, (delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)preFunc, (delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)postFunc, obj);
		}
	}

	public partial class CriAtomExAsrRack {
		CriAtomExAsr.BusFilterCbList _busFilterCbList;
		/// <inheritdoc cref="CriAtomExAsr.BusFilterCallbackByName"/>
		public CriAtomExAsr.BusFilterCbList BusFilterCallbackByName => _busFilterCbList ?? (_busFilterCbList = new CriAtomExAsr.BusFilterCbList(this.SetBusFilterCallbackByNameInternal));

		unsafe void SetBusFilterCallbackByNameInternal(ArgString busName, IntPtr preFunc, IntPtr postFunc, IntPtr obj)
		{
			if (!CriAtomEx.IsInitialized()) return;
			SetBusFilterCallbackByName(busName, (delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)preFunc, (delegate* unmanaged[Cdecl]<IntPtr, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)postFunc, obj);
		}
	}
}

