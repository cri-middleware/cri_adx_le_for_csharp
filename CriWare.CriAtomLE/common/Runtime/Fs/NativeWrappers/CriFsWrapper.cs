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
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criFs_SetUserMallocFunction(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criFs_SetUserFreeFunction(IntPtr func, IntPtr obj);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFs_GetNumUsedBinders(Int32* curNum, Int32* maxNum, Int32* limit);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFs_SetSelectIoCallback(IntPtr func);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFs_ControlFileIoMode(CriFs.FileIoMode ioMode);
#else
			internal static void criFs_SetUserMallocFunction(IntPtr func, IntPtr obj){}
		internal static void criFs_SetUserFreeFunction(IntPtr func, IntPtr obj){}
		internal static Int32 criFs_GetNumUsedBinders(Int32* curNum, Int32* maxNum, Int32* limit){return default(Int32);}
		internal static Int32 criFs_SetSelectIoCallback(IntPtr func){return default(Int32);}
		internal static Int32 criFs_ControlFileIoMode(CriFs.FileIoMode ioMode){return default(Int32);}
#endif
		}
	}
	public partial class CriFsBinder
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFsBinder_Create(IntPtr* bndrhn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFsBinder_Destroy(IntPtr bndrhn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFsBinder_BindFile(IntPtr bndrhn, IntPtr srcbndrhn, IntPtr path, IntPtr work, Int32 worksize, UInt32* bndrid);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFsBinder_BindFileSection(IntPtr bndrhn, IntPtr srcbndrhn, IntPtr path, UInt64 offset, Int32 size, IntPtr sectionName, IntPtr work, Int32 worksize, UInt32* bndrid);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFsBinder_GetStatus(UInt32 bndrid, CriFsBinder.Status* status);
#else
			internal static Int32 criFsBinder_Create(IntPtr* bndrhn){return default(Int32);}
		internal static Int32 criFsBinder_Destroy(IntPtr bndrhn){return default(Int32);}
		internal static Int32 criFsBinder_BindFile(IntPtr bndrhn, IntPtr srcbndrhn, IntPtr path, IntPtr work, Int32 worksize, UInt32* bndrid){return default(Int32);}
		internal static Int32 criFsBinder_BindFileSection(IntPtr bndrhn, IntPtr srcbndrhn, IntPtr path, UInt64 offset, Int32 size, IntPtr sectionName, IntPtr work, Int32 worksize, UInt32* bndrid){return default(Int32);}
		internal static Int32 criFsBinder_GetStatus(UInt32 bndrid, CriFsBinder.Status* status){return default(Int32);}
#endif
		}
	}
	public partial class CriFsStdio
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern IntPtr criFsStdio_OpenFile(IntPtr bndr, IntPtr fname, IntPtr mode);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int32 criFsStdio_CloseFile(IntPtr stdhn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criFsStdio_GetFileSize(IntPtr stdhn);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criFsStdio_SeekFile(IntPtr rdr, Int64 offset, CriFsStdio.SEEKTYPE seekType);
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern Int64 criFsStdio_ReadFile(IntPtr stdhn, Int64 rsize, IntPtr buf, Int64 bsize);
#else
			internal static IntPtr criFsStdio_OpenFile(IntPtr bndr, IntPtr fname, IntPtr mode){return default(IntPtr);}
		internal static Int32 criFsStdio_CloseFile(IntPtr stdhn){return default(Int32);}
		internal static Int64 criFsStdio_GetFileSize(IntPtr stdhn){return default(Int64);}
		internal static Int64 criFsStdio_SeekFile(IntPtr rdr, Int64 offset, CriFsStdio.SEEKTYPE seekType){return default(Int64);}
		internal static Int64 criFsStdio_ReadFile(IntPtr stdhn, Int64 rsize, IntPtr buf, Int64 bsize){return default(Int64);}
#endif
		}
	}
	public partial class CriFsLoader
	{
		unsafe partial class NativeMethods
		{
#if !CRI_ENABLE_HEADLESS_MODE
			[DllImport(CriAtomCSharp.libraryName, CallingConvention = CriAtomCSharp.callingConversion)]
			internal static extern void criFsLoader_Destroy(IntPtr binder);
#else
			internal static void criFsLoader_Destroy(IntPtr binder){}
#endif
		}
	}
}