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
	/// <summary>CriFsBinder ID. </summary>
	/// <remarks>
	/// <para>
	/// 説明：
	/// バインダーに対してバインドを行うと、 <see cref="CriFsBind"/> (バインドID)が作成されます。
	///  バインドIDは、個々のバインドを識別するためのもので、値は符号なし32ビット値の範囲を とります。
	///  この型の変数は、無効なバインドIDであることを意味する特別値 CRIFSBINDER_BID_NULL (ゼロ) をとる場合もあります。
	/// </para>
	/// </remarks>
	public readonly partial struct CriFsBind
	{
		/// <summary>ネイティブハンドル</summary>

		public UInt32 NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriFsBind(UInt32 handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriFsBind other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriFsBind a, CriFsBind b)
		{

			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriFsBind a, CriFsBind b) =>
			!(a == b);

	}
}