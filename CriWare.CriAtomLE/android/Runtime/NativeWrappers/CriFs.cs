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
		/// <summary>ファイルアクセススレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ファイルアクセススレッドのプライオリティを設定します。
		/// criFs_Initialize() の呼出し後に設定してください。
		/// プライオリティはナイス値で指定してください。範囲は-20(最高)から19(最低)です。
		/// アプリケーションのメインスレッドよりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は -7 です。
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
		public static CriErr.Error SetFileAccessThreadPriorityANDROID(Int32 prio)
		{
			return NativeMethods.criFs_SetFileAccessThreadPriority_ANDROID(prio);
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
		public static unsafe CriErr.Error GetFileAccessThreadPriorityANDROID(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetFileAccessThreadPriority_ANDROID(prioPtr);
		}

		/// <summary>メモリファイルシステムスレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリファイルシステムスレッドのプライオリティを設定します。
		/// プライオリティはナイス値で指定してください。範囲は-20(最高)から19(最低)です。
		/// アプリケーションのメインスレッドよりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は 7 です。
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
		public static CriErr.Error SetMemoryFileSystemThreadPriorityANDROID(Int32 prio)
		{
			return NativeMethods.criFs_SetMemoryFileSystemThreadPriority_ANDROID(prio);
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
		public static unsafe CriErr.Error GetMemoryFileSystemThreadPriorityANDROID(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetMemoryFileSystemThreadPriority_ANDROID(prioPtr);
		}

		/// <summary>データ展開スレッドのプライオリティ設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ展開スレッドのプライオリティを設定します。
		/// プライオリティはナイス値で指定してください。範囲は-20(最高)から19(最低)です。
		/// アプリケーションのメインスレッドよりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は 9 です。
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
		public static CriErr.Error SetDataDecompressionThreadPriorityANDROID(Int32 prio)
		{
			return NativeMethods.criFs_SetDataDecompressionThreadPriority_ANDROID(prio);
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
		public static unsafe CriErr.Error GetDataDecompressionThreadPriorityANDROID(out Int32 prio)
		{
			fixed (Int32* prioPtr = &prio)
				return NativeMethods.criFs_GetDataDecompressionThreadPriority_ANDROID(prioPtr);
		}

		/// <summary>JavaVMオブジェクトの登録</summary>
		/// <param name="vm">JavaVMオブジェクトへの参照</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// JavaVMオブジェクトへの参照をCriFileSystemライブラリに登録します。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// JavaVMオブジェクトへの参照を登録した場合、ライブラリ内部で作成されたスレッドはJavaVMにアタッチされます。
		/// </para>
		/// </remarks>
		public static void SetJavaVMANDROID(IntPtr vm)
		{
			NativeMethods.criFs_SetJavaVM_ANDROID(vm);
		}

		/// <summary>
		/// 本関数は旧仕様の関数で、互換性のため残してあります。
		/// <see cref="CriFs.EnableAssetsAccessANDROID"/>の方をお使い下さい。
		/// </summary>
		public static CriErr.Error SetContextANDROID(IntPtr jobj)
		{
			return NativeMethods.criFs_SetContext_ANDROID(jobj);
		}

		/// <summary>Androidプロジェクト内のassetsフォルダーに対するアクセスの有効化</summary>
		/// <param name="vm">JavaVMオブジェクトへの参照</param>
		/// <param name="jobj">android.content.Contextオブジェクト</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// assetフォルダーへのアクセスを有効化します。
		/// アクセスが終了した場合、 <see cref="CriFs.DisableAssetsAccessANDROID"/> 関数 を呼び出してください。
		/// </para>
		/// <para>
		/// 注意:
		/// :
		/// 本関数を呼び出す前に、CRI File Systemライブラリの初期化が完了済みである事を確認して下さい。
		/// </para>
		/// </remarks>
		public static CriErr.Error EnableAssetsAccessANDROID(IntPtr vm, IntPtr jobj)
		{
			return NativeMethods.criFs_EnableAssetsAccess_ANDROID(vm, jobj);
		}

		/// <summary>Androidプロジェクト内のassetsフォルダーに対するアクセスの無効化</summary>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// assetフォルダーへのアクセスを無効化します。
		/// </para>
		/// </remarks>
		public static CriErr.Error DisableAssetsAccessANDROID()
		{
			return NativeMethods.criFs_DisableAssetsAccess_ANDROID();
		}

	}
}