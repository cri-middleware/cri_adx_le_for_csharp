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
	public partial class CriFs
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_ANDROID && !UNITY_EDITOR) || android)
		[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_SetFileAccessThreadPriority_ANDROID(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_GetFileAccessThreadPriority_ANDROID(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_SetMemoryFileSystemThreadPriority_ANDROID(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_GetMemoryFileSystemThreadPriority_ANDROID(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_SetDataDecompressionThreadPriority_ANDROID(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_GetDataDecompressionThreadPriority_ANDROID(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern void criFs_SetJavaVM_ANDROID(IntPtr vm);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_EnableAssetsAccess_ANDROID(IntPtr vm, IntPtr jobj);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_EnableAssetsAccessForPrefix_ANDROID(IntPtr vm, IntPtr jobj, IntPtr prefix);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
		internal static extern Int32 criFs_DisableAssetsAccess_ANDROID();
#else
			internal static Int32 criFs_SetFileAccessThreadPriority_ANDROID(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetFileAccessThreadPriority_ANDROID(Int32* prio) { return default(Int32); }
			internal static Int32 criFs_SetMemoryFileSystemThreadPriority_ANDROID(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetMemoryFileSystemThreadPriority_ANDROID(Int32* prio) { return default(Int32); }
			internal static Int32 criFs_SetDataDecompressionThreadPriority_ANDROID(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetDataDecompressionThreadPriority_ANDROID(Int32* prio) { return default(Int32); }
			internal static void criFs_SetJavaVM_ANDROID(IntPtr vm) { }
			internal static Int32 criFs_EnableAssetsAccess_ANDROID(IntPtr vm, IntPtr jobj) { return default(Int32); }
			internal static Int32 criFs_EnableAssetsAccessForPrefix_ANDROID(IntPtr vm, IntPtr jobj, IntPtr prefix) { return default(Int32); }
			internal static Int32 criFs_DisableAssetsAccess_ANDROID() { return default(Int32); }
#endif
		}
	}
}