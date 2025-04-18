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
	/// <summary>CriFsLoaderハンドル </summary>
	public partial class CriFsLoader : IDisposable
	{
		/// <summary>CriFsLoaderの破棄 </summary>
		/// <remarks>
		/// <nativeinfo declaration="void criFsLoader_Destroy(CriFsLoaderHn binder)"/>
		/// </remarks>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criFsLoader_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriFsLoader() => Dispose();
#pragma warning restore 1591

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriFsLoader(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriFsLoader other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriFsLoader a, CriFsLoader b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriFsLoader a, CriFsLoader b) =>
			!(a == b);

	}
}