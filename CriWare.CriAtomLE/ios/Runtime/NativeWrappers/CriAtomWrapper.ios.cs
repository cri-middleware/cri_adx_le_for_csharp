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
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_IOS && !UNITY_EDITOR) || ios)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetServerThreadPriority_IOS(Int32 prio);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_StartSound_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_StopSound_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_RecoverSound_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern NativeBool criAtom_IsInitializationSucceeded_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetupAudioSession_IOS(CriAtom.AudioSessionConfigIOS* config);
#else
			internal static void criAtom_SetServerThreadPriority_IOS(Int32 prio) { }
			internal static void criAtom_StartSound_IOS() { }
			internal static void criAtom_StopSound_IOS() { }
			internal static void criAtom_RecoverSound_IOS() { }
			internal static NativeBool criAtom_IsInitializationSucceeded_IOS() { return default(NativeBool); }
			internal static void criAtom_SetupAudioSession_IOS(CriAtom.AudioSessionConfigIOS* config) { }
#endif
		}
	}
	public partial class CriAtomEx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_IOS && !UNITY_EDITOR) || ios)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtomEx_CalculateWorkSize_IOS(CriAtomEx.ConfigIOS* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Initialize_IOS(CriAtomEx.ConfigIOS* config, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Finalize_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_SetServerThreadPriority_IOS(Int32 prio);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_StartSound_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_StopSound_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_RecoverSound_IOS();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern NativeBool criAtomEx_IsInitializationSucceeded_IOS();
#else
			internal static Int32 criAtomEx_CalculateWorkSize_IOS(CriAtomEx.ConfigIOS* config) { return default(Int32); }
			internal static void criAtomEx_Initialize_IOS(CriAtomEx.ConfigIOS* config, IntPtr work, Int32 workSize) { }
			internal static void criAtomEx_Finalize_IOS() { }
			internal static void criAtomEx_SetServerThreadPriority_IOS(Int32 prio) { }
			internal static void criAtomEx_StartSound_IOS() { }
			internal static void criAtomEx_StopSound_IOS() { }
			internal static void criAtomEx_RecoverSound_IOS() { }
			internal static NativeBool criAtomEx_IsInitializationSucceeded_IOS() { return default(NativeBool); }
#endif
		}
	}
	public partial class CriAtomPlayer
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_IOS && !UNITY_EDITOR) || ios)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtomPlayer_CalculateWorkSizeForMp3Player_IOS(CriAtom.Mp3PlayerConfigIOS* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern IntPtr criAtomPlayer_CreateMp3Player_IOS(CriAtom.Mp3PlayerConfigIOS* config, IntPtr work, Int32 workSize);
#else
			internal static Int32 criAtomPlayer_CalculateWorkSizeForMp3Player_IOS(CriAtom.Mp3PlayerConfigIOS* config) { return default(Int32); }
			internal static IntPtr criAtomPlayer_CreateMp3Player_IOS(CriAtom.Mp3PlayerConfigIOS* config, IntPtr work, Int32 workSize) { return default(IntPtr); }
#endif
		}
	}
}