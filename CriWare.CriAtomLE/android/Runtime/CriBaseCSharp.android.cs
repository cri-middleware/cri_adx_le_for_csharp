using System.Runtime.InteropServices;
using System;

namespace CriWare{
	partial class CriBaseCSharp{
		/// <summary>JavaVMポインタの取得(Android)</summary>
		public static IntPtr GetJavaVM() => NativeMethods.criThread_GetJavaVM();
		internal class NativeMethods {
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_ANDROID && !UNITY_EDITOR) || android)
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern IntPtr criThread_GetJavaVM();
#else
			internal static IntPtr criThread_GetJavaVM() => IntPtr.Zero;
#endif
		}
	}
}