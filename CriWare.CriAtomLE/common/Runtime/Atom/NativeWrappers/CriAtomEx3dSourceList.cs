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
	/// <summary>3D音源オブジェクトリスト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 3D音源オブジェクトを管理するリストオブジェクトです。
	/// 3Dポジショニング機能におけるマルチポジショニング再生に使用します。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomEx3dSourceList.CriAtomEx3dSourceList"/>
	public partial class CriAtomEx3dSourceList : IDisposable
	{
		/// <summary>3D音源オブジェクトリスト作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">3D音源オブジェクトリスト作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリスト作成用コンフィグ構造体（ <see cref="CriAtomEx3dSourceList.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomEx3dSourceList.Config pConfig)
		{
			fixed (CriAtomEx3dSourceList.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx3dSourceList_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>3D音源オブジェクトリストの作成に必要なワーク領域サイズの計算</summary>
		/// <param name="config">3D音源オブジェクトリスト作成用コンフィグ構造体へのポインタ</param>
		/// <returns>3D音源オブジェクトリスト作成用ワークサイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリストを作成するために必要なワーク領域のサイズを取得します。
		/// アロケーターを登録せずに3D音源オブジェクトリストを作成する場合、
		/// あらかじめ本関数で計算したワーク領域サイズ分のメモリを
		/// ワーク領域として <see cref="CriAtomEx3dSourceList.CriAtomEx3dSourceList"/> 関数にセットする必要があります。
		/// 3D音源オブジェクトリストの作成に必要なワークメモリのサイズは、3D音源オブジェクトリスト作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx3dSourceList.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomEx3dSourceList.SetDefaultConfig"/> 適用時と同じパラメーター）で
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
		/// <seealso cref="CriAtomEx3dSourceList.CriAtomEx3dSourceList"/>
		/// <seealso cref="CriAtomEx3dSourceList.Config"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomEx3dSourceList.Config config)
		{
			fixed (CriAtomEx3dSourceList.Config* configPtr = &config)
				return NativeMethods.criAtomEx3dSourceList_CalculateWorkSize(configPtr);
		}

		/// <summary>3D音源オブジェクトリスト作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリストを作成する場合に使用する構造体です。
		/// 現状指定可能なパラメーターはありませんが、将来パラメーターが追加される可能性があるため、
		/// 本構造体を使用する際には <see cref="CriAtomEx3dSourceList.SetDefaultConfig"/> メソッドを使用し、
		/// 構造体の初期化を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList.SetDefaultConfig"/>
		/// <seealso cref="CriAtomEx3dSourceList.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dSourceList.CriAtomEx3dSourceList"/>
		public unsafe partial struct Config
		{
			/// <exclude/>
			public Int32 reserved;

		}
		/// <summary>3D音源オブジェクトリストの作成</summary>
		/// <param name="config">3D音源オブジェクトリスト作成用コンフィグ構造体へのポインタ</param>
		/// <param name="work">3D音源オブジェクトリスト作成用ワーク領域へのポインタ</param>
		/// <param name="workSize">3D音源オブジェクトリスト作成用ワークサイズ</param>
		/// <returns>3D音源オブジェクトリスト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリスト作成用コンフィグに基づいて、3D音源オブジェクトリストを作成します。
		/// 作成に成功すると、3D音源オブジェクトリストを返します。
		/// 3D音源オブジェクトリストを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomEx3dSourceList.CalculateWorkSize"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dSourceList.Dispose"/>
		public unsafe CriAtomEx3dSourceList(in CriAtomEx3dSourceList.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx3dSourceList.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomEx3dSourceList_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomEx3dSourceList(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomEx3dSourceList.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomEx3dSourceList_Create(configPtr, work, workSize);
		}

		/// <summary>3D音源オブジェクトリストの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリストを破棄します。
		/// 本関数を実行した時点で、3D音源オブジェクトリスト作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定した3D音源オブジェクトリストも無効になります。
		/// 3D音源オブジェクトリストをセットしたAtomExプレーヤーで再生している音声がある場合、
		/// 本関数を実行する前に、それらの音声を停止するか、そのAtomExプレーヤーを破棄してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 3D音源オブジェクトリストに3D音源オブジェクトが追加されている状態で本関数を実行した場合、
		/// 追加されていた3D音源オブジェクトは自動的に3D音源オブジェクトリストから削除されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList.CriAtomEx3dSourceList"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomEx3dSourceList_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomEx3dSourceList() => Dispose();
#pragma warning restore 1591

		/// <summary>3D音源オブジェクトリストへの3D音源オブジェクトの追加</summary>
		/// <param name="ex3dSource">3D音源オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリストに3D音源オブジェクトを追加します。
		/// 追加したAtomExプレーヤーは3D音源オブジェクトリストと関連付けられ、
		/// マルチポジショニング再生が可能となります。
		/// 追加した3D音源オブジェクトを3D音源オブジェクトリストから削除する場合は、 <see cref="CriAtomEx3dSourceList.Remove"/> 関数または
		/// <see cref="CriAtomEx3dSourceList.RemoveAll"/> 関数を呼び出してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 以下の条件に当てはまる3D音源オブジェクトは3D音源オブジェクトリストに追加することはできません。
		/// - 既にAtomExプレーヤーに設定されている
		/// - 既に他の3D音源オブジェクトリストに追加されている
		/// 本関数は再生中のAtomExプレーヤーに取り付けられている3D音源オブジェクトリストに対しても使用可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList.Remove"/>
		/// <seealso cref="CriAtomEx3dSourceList.RemoveAll"/>
		public void Add(CriAtomEx3dSource ex3dSource)
		{
			NativeMethods.criAtomEx3dSourceList_Add(NativeHandle, ex3dSource?.NativeHandle ?? default);
		}

		/// <summary>3D音源オブジェクトリストから3D音源オブジェクトの削除</summary>
		/// <param name="ex3dSource">3D音源オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリストから、指定した3D音源オブジェクトを削除します。
		/// 3D音源オブジェクトリストから全ての3D音源オブジェクトを削除したい場合は、 <see cref="CriAtomEx3dSourceList.RemoveAll"/> 関数を
		/// 呼び出してください。
		/// 3D音源オブジェクトリストに3D音源オブジェクトを追加したい場合は、 <see cref="CriAtomEx3dSourceList.Add"/> 関数を呼び出してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は再生中のAtomExプレーヤーに取り付けられている3D音源オブジェクトリストに対しても使用可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList.RemoveAll"/>
		public void Remove(CriAtomEx3dSource ex3dSource)
		{
			NativeMethods.criAtomEx3dSourceList_Remove(NativeHandle, ex3dSource?.NativeHandle ?? default);
		}

		/// <summary>3D音源オブジェクトリストから3D音源オブジェクトの全削除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトリストから追加されている全ての3D音源オブジェクトを削除します。
		/// 3D音源オブジェクトリストから特定の3D音源オブジェクトを削除したい場合は、 <see cref="CriAtomEx3dSourceList.Remove"/> 関数を
		/// 呼び出してください。
		/// 3D音源オブジェクトリストに3D音源オブジェクトを追加したい場合は、 <see cref="CriAtomEx3dSourceList.Add"/> 関数を呼び出してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は再生中のAtomExプレーヤーに取り付けられている3D音源オブジェクトリストに対しても使用可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList.Remove"/>
		public void RemoveAll()
		{
			NativeMethods.criAtomEx3dSourceList_RemoveAll(NativeHandle);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomEx3dSourceList(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomEx3dSourceList other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomEx3dSourceList a, CriAtomEx3dSourceList b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomEx3dSourceList a, CriAtomEx3dSourceList b) =>
			!(a == b);

	}
}