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
	public partial class CriAtomEx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_ANDROID && !UNITY_EDITOR) || android)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtomEx_CalculateWorkSize_ANDROID(CriAtomEx.ConfigANDROID* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Initialize_ANDROID(CriAtomEx.ConfigANDROID* config, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Finalize_ANDROID();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_StartSound_ANDROID();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_StopSound_ANDROID();
#else
			internal static Int32 criAtomEx_CalculateWorkSize_ANDROID(CriAtomEx.ConfigANDROID* config) { return default(Int32); }
			internal static void criAtomEx_Initialize_ANDROID(CriAtomEx.ConfigANDROID* config, IntPtr work, Int32 workSize) { }
			internal static void criAtomEx_Finalize_ANDROID() { }
			internal static void criAtomEx_StartSound_ANDROID() { }
			internal static void criAtomEx_StopSound_ANDROID() { }
#endif
		}
	}
	public partial class CriAtomLatencyEstimator
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_ANDROID && !UNITY_EDITOR) || android)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomLatencyEstimator_Initialize_ANDROID();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomLatencyEstimator_Finalize_ANDROID();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern CriAtomLatencyEstimator.Info criAtomLatencyEstimator_GetCurrentInfo_ANDROID();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern NativeBool criAtomLatencyEstimator_IsInitialized_ANDROID();
#else
			internal static void criAtomLatencyEstimator_Initialize_ANDROID() { }
			internal static void criAtomLatencyEstimator_Finalize_ANDROID() { }
			internal static CriAtomLatencyEstimator.Info criAtomLatencyEstimator_GetCurrentInfo_ANDROID() { return default(CriAtomLatencyEstimator.Info); }
			internal static NativeBool criAtomLatencyEstimator_IsInitialized_ANDROID() { return default(NativeBool); }
#endif
		}
	}
}