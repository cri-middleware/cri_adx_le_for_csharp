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
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern NativeBool criAtom_GetAudioClientMixFormat_WASAPI(IntPtr format);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern NativeBool criAtom_GetAudioClientIsFormatSupported_WASAPI(IntPtr format);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetAudioClientShareMode_WASAPI(Int32 mode);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtom_GetAudioClientShareMode_WASAPI();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetAudioClientFormat_WASAPI(IntPtr format);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetAudioClientBufferDuration_WASAPI(Int64 refTime);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern IntPtr criAtom_GetAudioClient_WASAPI();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern NativeBool criAtom_IsDeviceInvalidated_WASAPI();
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetDeviceId_WASAPI(CriAtom.SoundRendererType type, Int16* deviceId);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtom_EnumAudioEndpoints_WASAPI(IntPtr callback, IntPtr @object);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetDeviceUpdateCallback_WASAPI(IntPtr callback, IntPtr @object);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtom_SetSpatialAudioEnabled_WASAPI(CriAtom.SoundRendererType type, NativeBool sw);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern NativeBool criAtom_IsSpatialAudioEnabled_WASAPI(CriAtom.SoundRendererType type);
#else
			internal static NativeBool criAtom_GetAudioClientMixFormat_WASAPI(IntPtr format) { return default(NativeBool); }
			internal static NativeBool criAtom_GetAudioClientIsFormatSupported_WASAPI(IntPtr format) { return default(NativeBool); }
			internal static void criAtom_SetAudioClientShareMode_WASAPI(Int32 mode) { }
			internal static Int32 criAtom_GetAudioClientShareMode_WASAPI() { return default(Int32); }
			internal static void criAtom_SetAudioClientFormat_WASAPI(IntPtr format) { }
			internal static void criAtom_SetAudioClientBufferDuration_WASAPI(Int64 refTime) { }
			internal static IntPtr criAtom_GetAudioClient_WASAPI() { return default(IntPtr); }
			internal static NativeBool criAtom_IsDeviceInvalidated_WASAPI() { return default(NativeBool); }
			internal static void criAtom_SetDeviceId_WASAPI(CriAtom.SoundRendererType type, Int16* deviceId) { }
			internal static Int32 criAtom_EnumAudioEndpoints_WASAPI(IntPtr callback, IntPtr @object) { return default(Int32); }
			internal static void criAtom_SetDeviceUpdateCallback_WASAPI(IntPtr callback, IntPtr @object) { }
			internal static void criAtom_SetSpatialAudioEnabled_WASAPI(CriAtom.SoundRendererType type, NativeBool sw) { }
			internal static NativeBool criAtom_IsSpatialAudioEnabled_WASAPI(CriAtom.SoundRendererType type) { return default(NativeBool); }
#endif
		}
	}
	public partial class CriAtomEx
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win)
		[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern Int32 criAtomEx_CalculateWorkSize_WASAPI(CriAtomEx.ConfigWASAPI* config);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Initialize_WASAPI(CriAtomEx.ConfigWASAPI* config, IntPtr work, Int32 workSize);
[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
internal static extern void criAtomEx_Finalize_WASAPI();
#else
			internal static Int32 criAtomEx_CalculateWorkSize_WASAPI(CriAtomEx.ConfigWASAPI* config) { return default(Int32); }
			internal static void criAtomEx_Initialize_WASAPI(CriAtomEx.ConfigWASAPI* config, IntPtr work, Int32 workSize) { }
			internal static void criAtomEx_Finalize_WASAPI() { }
#endif
		}
	}
}