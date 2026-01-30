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
	public partial class CriAtom
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern Int32 criAtom_CalculateWorkSize_PC(CriAtom.ConfigPC* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtom_Initialize_PC(CriAtom.ConfigPC* config, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtom_Finalize_PC();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtom_SetThreadPriority_PC(Int32 prio);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern int criAtom_GetThreadPriority_PC();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtom_SetThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern IntPtr criAtom_GetThreadAffinityMask_PC();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern Int32 criAtom_CalculateStartParallelMixerWorkSize(UInt32 numSubMixers);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtom_StartParallelMixer(UInt32 numSubMixers, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtom_FinishParallelMixer();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern NativeBool criAtom_IsDuringParallelMixer();
#else
			internal static Int32 criAtom_CalculateWorkSize_PC(CriAtom.ConfigPC* config) { return default(Int32); }
			internal static void criAtom_Initialize_PC(CriAtom.ConfigPC* config, IntPtr work, Int32 workSize) { }
			internal static void criAtom_Finalize_PC() { }
			internal static void criAtom_SetThreadPriority_PC(Int32 prio) { }
			internal static int criAtom_GetThreadPriority_PC() { return default(int); }
			internal static void criAtom_SetThreadAffinityMask_PC(IntPtr mask) { }
			internal static IntPtr criAtom_GetThreadAffinityMask_PC() { return default(IntPtr); }
			internal static Int32 criAtom_CalculateStartParallelMixerWorkSize(UInt32 numSubMixers) { return default(Int32); }
			internal static void criAtom_StartParallelMixer(UInt32 numSubMixers, IntPtr work, Int32 workSize) { }
			internal static void criAtom_FinishParallelMixer() { }
			internal static NativeBool criAtom_IsDuringParallelMixer() { return default(NativeBool); }
#endif
		}
	}
	public partial class CriAtomEx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern Int32 criAtomEx_CalculateWorkSize_PC(CriAtomEx.ConfigPC* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtomEx_Initialize_PC(CriAtomEx.ConfigPC* config, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
		internal static extern void criAtomEx_Finalize_PC();
#else
			internal static Int32 criAtomEx_CalculateWorkSize_PC(CriAtomEx.ConfigPC* config) { return default(Int32); }
			internal static void criAtomEx_Initialize_PC(CriAtomEx.ConfigPC* config, IntPtr work, Int32 workSize) { }
			internal static void criAtomEx_Finalize_PC() { }
#endif
		}
	}
}