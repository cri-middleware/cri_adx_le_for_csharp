using System.Runtime.InteropServices;
using System;

namespace CriWare{
	partial class CriBaseCSharp{
		/// <summary>JavaVMポインタの取得(Android)</summary>
		public static IntPtr GetJavaVM() => NativeMethods.criThread_GetJavaVM();

#if CRI_BUILD_LE && android
		static CriBaseCSharp() => NativeMethods.criErr_GetErrorCount(CriErr.Level.Error);
#endif

		internal class NativeMethods {
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_ANDROID && !UNITY_EDITOR) || android)
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern IntPtr criThread_GetJavaVM();
#else
			internal static IntPtr criThread_GetJavaVM() => IntPtr.Zero;
#endif
#if CRI_BUILD_LE && android
			[DllImport("cri_ware_android_le", CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern UInt32 criErr_GetErrorCount(CriErr.Level level);
#endif
		}
	}
}