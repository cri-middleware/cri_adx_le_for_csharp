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
	/// <summary>入力ポートオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 入力ポートを操作するためのオブジェクトです。
	/// ::criAtomExInputPort_Create 関数で作成します。
	/// </para>
	/// </remarks>
	public partial struct CriAtomExInputPort
	{
		/// <summary>入力ポート種別</summary>
		public enum Type
		{
			None = 0,
			/// <summary>マイク入力</summary>
			Mic = 1,
			/// <summary>AUX入力</summary>
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