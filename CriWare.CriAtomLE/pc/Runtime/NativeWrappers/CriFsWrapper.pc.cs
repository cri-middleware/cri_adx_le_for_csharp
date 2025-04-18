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
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win)
		[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetServerThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetServerThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetFileAccessThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetFileAccessThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetMemoryFileSystemThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetMemoryFileSystemThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetDataDecompressionThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetDataDecompressionThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetInstallerThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetInstallerThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetServerThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetServerThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetFileAccessThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetFileAccessThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetMemoryFileSystemThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetMemoryFileSystemThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetDataDecompressionThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetDataDecompressionThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SetInstallerThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_GetInstallerThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern Int32 criFs_SwitchPathUnicodeToUtf8_PC(NativeBool sw);
#else
			internal static Int32 criFs_SetServerThreadPriority_PC(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetServerThreadPriority_PC(Int32* prio) { return default(Int32); }
			internal static Int32 criFs_SetFileAccessThreadPriority_PC(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetFileAccessThreadPriority_PC(Int32* prio) { return default(Int32); }
			internal static Int32 criFs_SetMemoryFileSystemThreadPriority_PC(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetMemoryFileSystemThreadPriority_PC(Int32* prio) { return default(Int32); }
			internal static Int32 criFs_SetDataDecompressionThreadPriority_PC(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetDataDecompressionThreadPriority_PC(Int32* prio) { return default(Int32); }
			internal static Int32 criFs_SetInstallerThreadPriority_PC(Int32 prio) { return default(Int32); }
			internal static Int32 criFs_GetInstallerThreadPriority_PC(Int32* prio) { return default(Int32); }
			internal static Int32 criFs_SetServerThreadAffinityMask_PC(IntPtr mask) { return default(Int32); }
			internal static Int32 criFs_GetServerThreadAffinityMask_PC(IntPtr* mask) { return default(Int32); }
			internal static Int32 criFs_SetFileAccessThreadAffinityMask_PC(IntPtr mask) { return default(Int32); }
			internal static Int32 criFs_GetFileAccessThreadAffinityMask_PC(IntPtr* mask) { return default(Int32); }
			internal static Int32 criFs_SetMemoryFileSystemThreadAffinityMask_PC(IntPtr mask) { return default(Int32); }
			internal static Int32 criFs_GetMemoryFileSystemThreadAffinityMask_PC(IntPtr* mask) { return default(Int32); }
			internal static Int32 criFs_SetDataDecompressionThreadAffinityMask_PC(IntPtr mask) { return default(Int32); }
			internal static Int32 criFs_GetDataDecompressionThreadAffinityMask_PC(IntPtr* mask) { return default(Int32); }
			internal static Int32 criFs_SetInstallerThreadAffinityMask_PC(IntPtr mask) { return default(Int32); }
			internal static Int32 criFs_GetInstallerThreadAffinityMask_PC(IntPtr* mask) { return default(Int32); }
			internal static Int32 criFs_SwitchPathUnicodeToUtf8_PC(NativeBool sw) { return default(Int32); }
#endif
		}
	}
}