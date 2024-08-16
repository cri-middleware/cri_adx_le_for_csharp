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
	public partial class CriFs
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_ANDROID && !UNITY_EDITOR) || android)
		[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetFileAccessThreadPriority_ANDROID(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetFileAccessThreadPriority_ANDROID(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetMemoryFileSystemThreadPriority_ANDROID(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetMemoryFileSystemThreadPriority_ANDROID(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetDataDecompressionThreadPriority_ANDROID(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetDataDecompressionThreadPriority_ANDROID(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern void criFs_SetJavaVM_ANDROID(IntPtr vm);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetContext_ANDROID(IntPtr jobj);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_EnableAssetsAccess_ANDROID(IntPtr vm, IntPtr jobj);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_DisableAssetsAccess_ANDROID();
#else
			internal static CriErr.Error criFs_SetFileAccessThreadPriority_ANDROID(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetFileAccessThreadPriority_ANDROID(Int32* prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetMemoryFileSystemThreadPriority_ANDROID(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetMemoryFileSystemThreadPriority_ANDROID(Int32* prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetDataDecompressionThreadPriority_ANDROID(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetDataDecompressionThreadPriority_ANDROID(Int32* prio) { return default(CriErr.Error); }
			internal static void criFs_SetJavaVM_ANDROID(IntPtr vm) { }
			internal static CriErr.Error criFs_SetContext_ANDROID(IntPtr jobj) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_EnableAssetsAccess_ANDROID(IntPtr vm, IntPtr jobj) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_DisableAssetsAccess_ANDROID() { return default(CriErr.Error); }
#endif
		}
	}
}