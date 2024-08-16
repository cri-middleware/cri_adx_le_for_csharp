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
	/// <summary>CriFs API</summary>
	public static partial class CriFs
	{
		/// <summary>サーバー処理スレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのプライオリティを設定します。
		/// 引数 prio には Win32 API のスレッド優先レベル（ SetThreadPriority 関数の引数）を指定します。
		/// アプリケーションのメインスレッドよりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は THREAD_PRIORITY_HIGHEST です。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// サーバー処理スレッドは、CRI Atomライブラリで使用しているものと同じです（共用しています）。
		/// すでにCRI AtomライブラリのAPIでサーバー処理スレッドの設定を変更している場合、
		/// 本関数を呼び出すと設定を上書きしてしまうのでご注意ください。
		/// </para>
		/// </remarks>
		public static CriErr.Error SetServerThreadPriorityPC(Int32 prio)
		{
			return NativeMethods.criFs_SetServerThreadPriority_PC(prio);
		}

		/// <summary>サーバー処理スレッドのプライオリティ取得</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのプライオリティを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetServerThreadPriorityPC(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetServerThreadPriority_PC(prioPtr);
		}

		/// <summary>ファイルアクセススレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ファイルアクセススレッドのプライオリティを設定します。
		/// 引数 prio には Win32 API のスレッド優先レベル（ SetThreadPriority 関数の引数）を指定します。
		/// アプリケーションのメインスレッドよりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は THREAD_PRIORITY_HIGHEST です。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetFileAccessThreadPriorityPC(Int32 prio)
		{
			return NativeMethods.criFs_SetFileAccessThreadPriority_PC(prio);
		}

		/// <summary>ファイルアクセススレッドのプライオリティ取得</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ファイルアクセススレッドのプライオリティを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetFileAccessThreadPriorityPC(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetFileAccessThreadPriority_PC(prioPtr);
		}

		/// <summary>メモリファイルシステムスレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリファイルシステムスレッドのプライオリティを設定します。
		/// 引数 prio には Win32 API のスレッド優先レベル（ SetThreadPriority 関数の引数）を指定します。
		/// アプリケーションのメインスレッドよりも低いプライオリティを指定してください。
		/// プライオリティのデフォルト値は THREAD_PRIORITY_LOWEST です。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetMemoryFileSystemThreadPriorityPC(Int32 prio)
		{
			return NativeMethods.criFs_SetMemoryFileSystemThreadPriority_PC(prio);
		}

		/// <summary>メモリファイルシステムスレッドのプライオリティ取得</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリファイルシステムスレッドのプライオリティを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetMemoryFileSystemThreadPriorityPC(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetMemoryFileSystemThreadPriority_PC(prioPtr);
		}

		/// <summary>データ展開スレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ展開スレッドのプライオリティを設定します。
		/// 引数 prio には Win32 API のスレッド優先レベル（ SetThreadPriority 関数の引数）を指定します。
		/// アプリケーションのメインスレッドよりも低いプライオリティを指定してください。
		/// プライオリティのデフォルト値は THREAD_PRIORITY_LOWEST です。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetDataDecompressionThreadPriorityPC(Int32 prio)
		{
			return NativeMethods.criFs_SetDataDecompressionThreadPriority_PC(prio);
		}

		/// <summary>データ展開スレッドのプライオリティ取得</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ展開スレッドのプライオリティを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetDataDecompressionThreadPriorityPC(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetDataDecompressionThreadPriority_PC(prioPtr);
		}

		/// <summary>インストーラースレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インストーラースレッドのプライオリティを設定します。
		/// 引数 prio には Win32 API のスレッド優先レベル（ SetThreadPriority 関数の引数）を指定します。
		/// アプリケーションのメインスレッドよりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は THREAD_PRIORITY_ABOVE_NORMAL です。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetInstallerThreadPriorityPC(Int32 prio)
		{
			return NativeMethods.criFs_SetInstallerThreadPriority_PC(prio);
		}

		/// <summary>インストーラースレッドのプライオリティ取得</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インストーラースレッドのプライオリティを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetInstallerThreadPriorityPC(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetInstallerThreadPriority_PC(prioPtr);
		}

		/// <summary>サーバー処理スレッドのアフィニティマスク設定</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのアフィニティマスクを設定します。
		/// 引数 mask には Win32 API のスレッドアフィニティマスク（ SetThreadAffinityMask 関数の引数）を指定します。
		/// デフォルト状態ではアフィニティマスクは設定されていません。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// サーバー処理スレッドは、CRI Atomライブラリで使用しているものと同じです（共用しています）。
		/// すでにCRI AtomライブラリのAPIでサーバー処理スレッドの設定を変更している場合、
		/// 本関数を呼び出すと設定を上書きしてしまうのでご注意ください。
		/// </para>
		/// </remarks>
		public static CriErr.Error SetServerThreadAffinityMaskPC(IntPtr mask)
		{
			return NativeMethods.criFs_SetServerThreadAffinityMask_PC(mask);
		}

		/// <summary>サーバー処理スレッドのアフィニティマスク取得</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのアフィニティマスクを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetServerThreadAffinityMaskPC(out IntPtr mask)
		{
			fixed (IntPtr* maskPtr = &mask)
				return NativeMethods.criFs_GetServerThreadAffinityMask_PC(maskPtr);
		}

		/// <summary>ファイルアクセススレッドのアフィニティマスク設定</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ファイルアクセススレッドのアフィニティマスクを設定します。
		/// 引数 mask には Win32 API のスレッドアフィニティマスク（ SetThreadAffinityMask 関数の引数）を指定します。
		/// デフォルト状態ではアフィニティマスクは設定されていません。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetFileAccessThreadAffinityMaskPC(IntPtr mask)
		{
			return NativeMethods.criFs_SetFileAccessThreadAffinityMask_PC(mask);
		}

		/// <summary>ファイルアクセススレッドのアフィニティマスク取得</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ファイルアクセススレッドのアフィニティマスクを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetFileAccessThreadAffinityMaskPC(out IntPtr mask)
		{
			fixed (IntPtr* maskPtr = &mask)
				return NativeMethods.criFs_GetFileAccessThreadAffinityMask_PC(maskPtr);
		}

		/// <summary>メモリファイルシステムスレッドのアフィニティマスク設定</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリファイルシステムスレッドのアフィニティマスクを設定します。
		/// 引数 mask には Win32 API のスレッドアフィニティマスク（ SetThreadAffinityMask 関数の引数）を指定します。
		/// デフォルト状態ではアフィニティマスクは設定されていません。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetMemoryFileSystemThreadAffinityMaskPC(IntPtr mask)
		{
			return NativeMethods.criFs_SetMemoryFileSystemThreadAffinityMask_PC(mask);
		}

		/// <summary>メモリファイルシステムスレッドのアフィニティマスク取得</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリファイルシステムスレッドのアフィニティマスクを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetMemoryFileSystemThreadAffinityMaskPC(out IntPtr mask)
		{
			fixed (IntPtr* maskPtr = &mask)
				return NativeMethods.criFs_GetMemoryFileSystemThreadAffinityMask_PC(maskPtr);
		}

		/// <summary>データ展開スレッドのアフィニティマスク設定</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ展開スレッドのアフィニティマスクを設定します。
		/// 引数 mask には Win32 API のスレッドアフィニティマスク（ SetThreadAffinityMask 関数の引数）を指定します。
		/// デフォルト状態ではアフィニティマスクは設定されていません。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetDataDecompressionThreadAffinityMaskPC(IntPtr mask)
		{
			return NativeMethods.criFs_SetDataDecompressionThreadAffinityMask_PC(mask);
		}

		/// <summary>データ展開スレッドのアフィニティマスク取得</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ展開スレッドのアフィニティマスクを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetDataDecompressionThreadAffinityMaskPC(out IntPtr mask)
		{
			fixed (IntPtr* maskPtr = &mask)
				return NativeMethods.criFs_GetDataDecompressionThreadAffinityMask_PC(maskPtr);
		}

		/// <summary>インストーラースレッドのアフィニティマスク設定</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インストーラースレッドのアフィニティマスクを設定します。
		/// 引数 mask には Win32 API のスレッドアフィニティマスク（ SetThreadAffinityMask 関数の引数）を指定します。
		/// デフォルト状態ではアフィニティマスクは設定されていません。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		/// （エラーコールバックが発生します。）
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行しても、効果はありません。
		/// （エラーコールバックが発生します。）
		/// </para>
		/// </remarks>
		public static CriErr.Error SetInstallerThreadAffinityMaskPC(IntPtr mask)
		{
			return NativeMethods.criFs_SetInstallerThreadAffinityMask_PC(mask);
		}

		/// <summary>インストーラースレッドのアフィニティマスク取得</summary>
		/// <param name="mask">アフィニティマスク</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インストーラースレッドのアフィニティマスクを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル
		/// （ <see cref="CriFs.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		/// 他のスレッドモデルを選択した場合、本関数はエラーを返します。
		/// 本関数は初期化後～終了処理前の間に実行する必要があります。
		/// 初期化前や終了処理後に本関数を実行した場合、本関数はエラーを返します。
		/// </para>
		/// </remarks>
		public static unsafe CriErr.Error GetInstallerThreadAffinityMaskPC(out IntPtr mask)
		{
			fixed (IntPtr* maskPtr = &mask)
				return NativeMethods.criFs_GetInstallerThreadAffinityMask_PC(maskPtr);
		}

		/// <summary>パスの文字エンコーディングタイプを Unicode から UTF-8 に切り替え</summary>
		/// <param name="sw">文字列エンコーディングタイプを UTF-8 にするかどうか</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// アプリケーションからUTF-8のファイルパスを指定したい場合、本関数をご利用ください。
		/// デフォルトでは、ファイルパス文字列をマルチバイト文字セットとして取り扱います。(CreateFileAを使用)
		/// そのため、実際にシステムが解釈する文字エンコーディングはシステムロケールに依存します。
		/// 本関数を使用すると、ファイルパスをUnicodeとして取り扱うように変更することができます。(CreateFileWを使用)
		/// 引数に true を指定して実行すると、ファイルパスはUTF-8としてみなし、ライブラリ内部でUTF-16に変換してファイルオープンを行います。
		/// </para>
		/// </remarks>
		public static CriErr.Error SwitchPathUnicodeToUtf8PC(NativeBool sw)
		{
			return NativeMethods.criFs_SwitchPathUnicodeToUtf8_PC(sw);
		}

	}
}