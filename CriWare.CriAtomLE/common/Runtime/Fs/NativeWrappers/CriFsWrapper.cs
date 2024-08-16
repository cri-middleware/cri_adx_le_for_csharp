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
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criFs_SetUserMallocFunction(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criFs_SetUserFreeFunction(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern CriErr.Error criFs_SetSelectIoCallback(IntPtr func);
#else
			internal static void criFs_SetUserMallocFunction(IntPtr func, IntPtr obj){}
		internal static void criFs_SetUserFreeFunction(IntPtr func, IntPtr obj){}
		internal static CriErr.Error criFs_SetSelectIoCallback(IntPtr func){return default(CriErr.Error);}
#endif
		}
	}
	public partial class CriFsBinder
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criFsBinder_Destroy(IntPtr binder);
#else
			internal static void criFsBinder_Destroy(IntPtr binder){}
#endif
		}
	}
}