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
	/// <summary>入力ポートハンドル </summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 入力ポートを操作するためのオブジェクトです。
	/// </para>
	/// </remarks>
	public readonly partial struct CriAtomExInputPort
	{
		/// <summary>入力ポート種別 </summary>
		public enum Type
		{
			None = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>マイク入力 </para>
			/// </remarks>
			Mic = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>AUX入力 </para>
			/// </remarks>
			Aux = 2,
		}
		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExInputPort(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExInputPort other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExInputPort a, CriAtomExInputPort b)
		{

			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExInputPort a, CriAtomExInputPort b) =>
			!(a == b);

	}
}