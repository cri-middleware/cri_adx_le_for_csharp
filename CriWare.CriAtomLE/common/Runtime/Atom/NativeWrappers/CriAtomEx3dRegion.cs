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
	/// <summary>3Dリージョンオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 3Dリージョンを扱うためのオブジェクトです。
	/// 3Dトランシーバー機能に使用します。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/>
	public partial class CriAtomEx3dRegion : IDisposable
	{
		/// <summary>3Dリージョンオブジェクト作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">3Dリージョンオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリージョンオブジェクト作成用コンフィグ構造体（ <see cref="CriAtomEx3dRegion.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomEx3dRegion.Config pConfig)
		{
			fixed (CriAtomEx3dRegion.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx3dRegion_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>3Dリージョンオブジェクトの作成に必要なワーク領域サイズの計算</summary>
		/// <param name="config">3Dリージョンオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <returns>3Dリージョンオブジェクト作成用ワークサイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリージョンオブジェクトを作成するために必要な、ワーク領域のサイズを取得します。
		/// アロケーターを登録せずに3Dリージョンオブジェクトを作成する場合、
		/// あらかじめ本関数で計算したワーク領域サイズ分のメモリを
		/// ワーク領域として <see cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/> 関数にセットする必要があります。
		/// 3Dリージョンオブジェクトの作成に必要なワークメモリのサイズは、3Dリージョンオブジェクト作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx3dRegion.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomEx3dRegion.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// ワーク領域サイズ計算時に失敗した場合、戻り値は -1 になります。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックの
		/// メッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/>
		/// <seealso cref="CriAtomEx3dRegion.Config"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomEx3dRegion.Config config)
		{
			fixed (CriAtomEx3dRegion.Config* configPtr = &config)
				return NativeMethods.criAtomEx3dRegion_CalculateWorkSize(configPtr);
		}

		/// <summary>3Dリージョンオブジェクト作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリージョンオブジェクトを作成する場合に使用する構造体です。
		/// 現状指定可能なパラメーターはありませんが、将来パラメーターが追加される可能性があるため、
		/// 本構造体を使用する際には <see cref="CriAtomEx3dRegion.SetDefaultConfig"/> メソッドを使用し、
		/// 構造体の初期化を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.SetDefaultConfig"/>
		/// <seealso cref="CriAtomEx3dRegion.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/>
		public unsafe partial struct Config
		{
			/// <summary>予約値（0を指定してください）</summary>
			public Int32 reserved;

		}
		/// <summary>3Dリージョンオブジェクトの作成</summary>
		/// <param name="config">3Dリージョンオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <param name="work">3Dリージョンオブジェクト作成用ワーク領域へのポインタ</param>
		/// <param name="workSize">3Dリージョンオブジェクト作成用ワークサイズ</param>
		/// <returns>3Dリージョンオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリージョンオブジェクト作成用コンフィグに基づいて、3Dリージョンオブジェクトを作成します。
		/// 作成に成功すると、3Dリージョンオブジェクトを返します。
		/// 3Dリージョンオブジェクトを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomEx3dRegion.CalculateWorkSize"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dRegion.Dispose"/>
		public unsafe CriAtomEx3dRegion(in CriAtomEx3dRegion.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx3dRegion.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomEx3dRegion_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomEx3dRegion(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomEx3dRegion.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomEx3dRegion_Create(configPtr, work, workSize);
		}

		/// <summary>3Dリージョンオブジェクトの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリージョンオブジェクトを破棄します。
		/// 本関数を実行した時点で、3Dリージョンオブジェクト作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定した3Dリージョンオブジェクトも無効になります。
		/// 3Dリージョンオブジェクトをセットした3D音源、3Dリスナー、3Dトランシーバーがある場合、
		/// 本関数を実行する前に、当該3Dリージョンを設定している全てのオブジェクトを破棄するか、
		/// 設定を外すようにしてください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomEx3dRegion_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomEx3dRegion() => Dispose();
#pragma warning restore 1591

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomEx3dRegion(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomEx3dRegion other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomEx3dRegion a, CriAtomEx3dRegion b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomEx3dRegion a, CriAtomEx3dRegion b) =>
			!(a == b);

	}
}