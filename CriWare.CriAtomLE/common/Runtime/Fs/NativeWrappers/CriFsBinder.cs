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
	/// <summary>CriFsBinderオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明：
	/// バインダーとは、ファイルを効率良く扱うためのデータベースです。
	/// - <see cref="CriFsBinder"/> (バインダーオブジェクト)とバインド
	/// バインダーを利用するには、バインダーオブジェクト( <see cref="CriFsBinder"/> )を作成し、
	/// CPKファイル／ファイル／ディレクトリをバインダーに結びつけます。
	/// このバインダーへの結び付けをバインドと呼びます。
	/// バインダーを作成すると、バインダーオブジェクト( <see cref="CriFsBinder"/> )が取得されます。
	/// - CriFsBindId （バインドID）
	/// バインダーにバインドを行うと、バインドIDが作成されます。個々のバインドを識別するために使用します。
	/// - ファイルのバインドとアンバインド
	/// バインダーには、CPKファイルやファイル、ディレクトリをどのような組み合わせででもバインドできます。
	/// バインドした項目のバインド状態を解除することをアンバインドと呼びます。
	/// - 利用できるバインド数
	/// 作成できるバインダー数や同時にバインドできる最大数は、 CriFsConfig の
	/// num_binders (バインダー数)や max_binds (同時バインド可能な最大数)で指定します。
	/// - CPKファイルのバインド
	/// CPKファイルに収納されている個々のファイル（コンテンツファイル）にアクセスするには、
	/// CPKファイルをバインドする必要があります。
	/// CPKファイルのコンテンツファイルもバインドできます。元のCPKファイルをアンバインドした場合、
	/// バインドされているコンテンツファイルもアンバインドされます（暗黙的アンバインド）。
	/// - バインダーのプライオリティ
	/// バインダーは、目的のファイルがどのバインドIDにあるのかを検索します。
	/// このバインドIDの検索順は、基本的にはバインドされた順番になりますが、バインドIDのプライオリティを
	/// 操作することで、検索順を変更することができます。
	/// - バインダーとCriFsのAPI
	/// CriFsLoader, CriFsGroupLoader, CriFsBinderには、バインダーを引数に持つAPIがあります。
	/// その際には、 <see cref="CriFsBinder"/> と CriFsBindId 、どちらを指定するのかに注意してください。
	/// </para>
	/// </remarks>
	public partial class CriFsBinder : IDisposable
	{
		/// <summary>CriFsBinderの破棄</summary>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criFsBinder_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriFsBinder() => Dispose();
#pragma warning restore 1591

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriFsBinder(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriFsBinder other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriFsBinder a, CriFsBinder b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriFsBinder a, CriFsBinder b) =>
			!(a == b);

	}
}