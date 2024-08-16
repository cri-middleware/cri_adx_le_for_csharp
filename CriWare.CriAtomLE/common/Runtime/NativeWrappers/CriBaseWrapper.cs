/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using CriWare.InteropHelpers;

namespace CriWare
{
	public partial class CriErr
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern NativeString criErr_ConvertIdToMsg(IntPtr errid);
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern NativeString criErr_ConvertIdToMessage(IntPtr errid, UInt32 p1, UInt32 p2);
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern void criErr_SetCallback(IntPtr cbf);
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern void criErr_SetErrorNotificationLevel(CriErr.NotificationLevel level);
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern UInt32 criErr_GetErrorCount(CriErr.Level level);
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern void criErr_ResetErrorCount(CriErr.Level level);
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern void criErr_Notify(CriErr.Level level, IntPtr errid);
#else
			internal static NativeString criErr_ConvertIdToMsg(IntPtr errid){return default(NativeString);}
		internal static NativeString criErr_ConvertIdToMessage(IntPtr errid, UInt32 p1, UInt32 p2){return default(NativeString);}
		internal static void criErr_SetCallback(IntPtr cbf){}
		internal static void criErr_SetErrorNotificationLevel(CriErr.NotificationLevel level){}
		internal static UInt32 criErr_GetErrorCount(CriErr.Level level){return default(UInt32);}
		internal static void criErr_ResetErrorCount(CriErr.Level level){}
		internal static void criErr_Notify(CriErr.Level level, IntPtr errid){}
#endif
		}
	}
}