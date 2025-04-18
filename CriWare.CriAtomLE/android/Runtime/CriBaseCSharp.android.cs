using System.Runtime.InteropServices;
using System;

namespace CriWare{
	partial class CriBaseCSharp{
		/// <summary>JavaVMポインタの取得(Android)</summary>
		public static IntPtr GetJavaVM() => NativeMethods.criJniShared_GetJavaVM();
		internal class NativeMethods {
#if !CRI_ENABLE_HEADLESS_MODE && ((UNITY_ANDROID && !UNITY_EDITOR) || android)
			[DllImport("cri_jni_shared", CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern IntPtr criJniShared_GetJavaVM();
#else
			internal static IntPtr criJniShared_GetJavaVM() => IntPtr.Zero;
#endif
		}
	}
}