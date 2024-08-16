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
	public partial class CriAtom
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX || osx)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetServerThreadPriority_MACOSX(Int32 prio);
#else
			internal static void criAtom_SetServerThreadPriority_MACOSX(Int32 prio) { }
#endif
		}
	}
	public partial class CriAtomEx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX || osx)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtomEx_CalculateWorkSize_MACOSX(CriAtomEx.ConfigMACOSX* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Initialize_MACOSX(CriAtomEx.ConfigMACOSX* config, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Finalize_MACOSX();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_SetServerThreadPriority_MACOSX(Int32 prio);
#else
			internal static Int32 criAtomEx_CalculateWorkSize_MACOSX(CriAtomEx.ConfigMACOSX* config) { return default(Int32); }
			internal static void criAtomEx_Initialize_MACOSX(CriAtomEx.ConfigMACOSX* config, IntPtr work, Int32 workSize) { }
			internal static void criAtomEx_Finalize_MACOSX() { }
			internal static void criAtomEx_SetServerThreadPriority_MACOSX(Int32 prio) { }
#endif
		}
	}
}