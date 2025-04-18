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
	public partial class CriAtomEx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_WEBGL && !UNITY_EDITOR) || browser)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtomEx_CalculateWorkSize_WEBAUDIO(CriAtomEx.ConfigWEBAUDIO* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Initialize_WEBAUDIO(CriAtomEx.ConfigWEBAUDIO* config, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Finalize_WEBAUDIO();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_SetBufferingTime_WEBAUDIO(Int32 bufferingTime);
#else
			internal static Int32 criAtomEx_CalculateWorkSize_WEBAUDIO(CriAtomEx.ConfigWEBAUDIO* config) { return default(Int32); }
			internal static void criAtomEx_Initialize_WEBAUDIO(CriAtomEx.ConfigWEBAUDIO* config, IntPtr work, Int32 workSize) { }
			internal static void criAtomEx_Finalize_WEBAUDIO() { }
			internal static void criAtomEx_SetBufferingTime_WEBAUDIO(Int32 bufferingTime) { }
#endif
		}
	}
}