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
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win)
		[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetServerThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetServerThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetFileAccessThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetFileAccessThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetMemoryFileSystemThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetMemoryFileSystemThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetDataDecompressionThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetDataDecompressionThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetInstallerThreadPriority_PC(Int32 prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetInstallerThreadPriority_PC(Int32* prio);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetServerThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetServerThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetFileAccessThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetFileAccessThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetMemoryFileSystemThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetMemoryFileSystemThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetDataDecompressionThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetDataDecompressionThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SetInstallerThreadAffinityMask_PC(IntPtr mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_GetInstallerThreadAffinityMask_PC(IntPtr* mask);
[DllImport(CriFsCSharp.libraryName, CallingConvention = CriFsCSharp.callingConversion)]
internal static extern CriErr.Error criFs_SwitchPathUnicodeToUtf8_PC(NativeBool sw);
#else
			internal static CriErr.Error criFs_SetServerThreadPriority_PC(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetServerThreadPriority_PC(Int32* prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetFileAccessThreadPriority_PC(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetFileAccessThreadPriority_PC(Int32* prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetMemoryFileSystemThreadPriority_PC(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetMemoryFileSystemThreadPriority_PC(Int32* prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetDataDecompressionThreadPriority_PC(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetDataDecompressionThreadPriority_PC(Int32* prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetInstallerThreadPriority_PC(Int32 prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetInstallerThreadPriority_PC(Int32* prio) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetServerThreadAffinityMask_PC(IntPtr mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetServerThreadAffinityMask_PC(IntPtr* mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetFileAccessThreadAffinityMask_PC(IntPtr mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetFileAccessThreadAffinityMask_PC(IntPtr* mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetMemoryFileSystemThreadAffinityMask_PC(IntPtr mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetMemoryFileSystemThreadAffinityMask_PC(IntPtr* mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetDataDecompressionThreadAffinityMask_PC(IntPtr mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetDataDecompressionThreadAffinityMask_PC(IntPtr* mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SetInstallerThreadAffinityMask_PC(IntPtr mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_GetInstallerThreadAffinityMask_PC(IntPtr* mask) { return default(CriErr.Error); }
			internal static CriErr.Error criFs_SwitchPathUnicodeToUtf8_PC(NativeBool sw) { return default(CriErr.Error); }
#endif
		}
	}
}