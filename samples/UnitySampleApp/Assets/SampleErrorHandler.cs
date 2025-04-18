using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using System;
using Unity.Burst;
using Unity.Collections;
using System.Runtime.InteropServices;

[BurstCompile]
class SampleErrorHandler : System.IDisposable
{
	static SampleErrorHandler _instance = null;

#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoadMethod]
#else
	[RuntimeInitializeOnLoadMethod]
#endif
	public unsafe static void RegisterErrorHandler()
	{
		if (Application.isBatchMode) return;
		_instance ??= new SampleErrorHandler();
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
	delegate void NativeDelegate(CriWare.InteropHelpers.NativeString errid, UInt32 p1, UInt32 p2, IntPtr parray);

	NativeDelegate _callback = null;
	unsafe SampleErrorHandler() =>
		CriErr.SetCallback((delegate*unmanaged[Cdecl]<CriWare.InteropHelpers.NativeString, uint, uint, IntPtr, void>)
#if ENABLE_IL2CPP
			Marshal.GetFunctionPointerForDelegate(_callback = LogMessage)
#else
			BurstCompiler.CompileFunctionPointer<NativeDelegate>(LogMessage).Value
#endif
		);
	public unsafe void Dispose() =>
		CriErr.SetCallback(null);
	~SampleErrorHandler() =>
		Dispose();

#if !ENABLE_IL2CPP
	[BurstCompile]
#endif
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
	static unsafe void LogMessage(CriWare.InteropHelpers.NativeString msg, uint p1, uint p2, IntPtr parray){
		var msgPtr = (byte*)CriErr.ConvertIdToMessage(msg, p1, p2).GetUnsafeStringPointer();
		var logMessage = new FixedString512Bytes();
		for(int i = 0;i < 512;i++){
			if(msgPtr[i] == 0) break;
			logMessage.Add(msgPtr[i]);
		}
		if(msgPtr[0] == 'W')
			Debug.LogWarning(logMessage);
		else
			Debug.LogError(logMessage);
	}
}
