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
#pragma warning disable 0465
	/// <summary>CriAtom API</summary>
	public static partial class CriAtom
	{
		/// <summary>サーバスレッドプライオリティの設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRIサーバスレッドのプライオリティを設定します。
		/// 引数 prio は pthread のプライオリティ設定値として使用します。
		/// 指定できる値の範囲は通常 -16～99で、数字が大きい方が優先度が高くなります。
		/// アプリケーションのメインスレッド(0)よりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は10です。
		/// </para>
		/// <para>
		/// 注意:
		/// ::criAtom_Initialize_MACOSX 関数実行前に本関数を実行することはできません。
		/// サーバ処理スレッドは、CRI File Systemライブラリでも利用されています。
		/// すでにCRI File SystemライブラリのAPIでサーバ処理スレッドの設定を変更している場合
		/// 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// </remarks>
		public static void SetServerThreadPriorityMACOSX(Int32 prio)
		{
			NativeMethods.criAtom_SetServerThreadPriority_MACOSX(prio);
		}

	}
}