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
	/// <summary>CriAtomLatencyEstimator API</summary>
	public static partial class CriAtomLatencyEstimator
	{
		/// <summary>遅延推測器の初期化</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 遅延推測器を初期化します。
		/// 遅延推測器を使用する際、本関数の呼び出しは必須です。
		/// 本関数を呼び出すと、Atomライブラリ内部で遅延推測器が生成、起動します。
		/// 起動された遅延推測器のスレッドは、他スレッドの要求等を待たず遅延推測処理を開始します。
		/// 遅延推測を終了するには、<see cref="CriAtomLatencyEstimator.FinalizeANDROID"/> 関数を呼ぶ必要があります。
		/// また、本関数は完了復帰です。多重呼び出しを許容しますが、Atomライブラリ内では
		/// 本関数の呼び出し回数をカウントしています。
		/// 実際の初期化処理が実行されるのは、最初の呼び出しの時だけになります。
		/// なお、この呼び出し回数カウンタは、<see cref="CriAtomLatencyEstimator.FinalizeANDROID"/> 関数を呼び出す度に
		/// デクリメントされます。
		/// </para>
		/// <para>
		/// 注意:
		/// ::criAtom_Initialize_ANDROID 関数実行前に本関数を実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomLatencyEstimator.FinalizeANDROID"/>
		public static void InitializeANDROID()
		{
			NativeMethods.criAtomLatencyEstimator_Initialize_ANDROID();
		}

		/// <summary>遅延推測器の終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 遅延推測器を終了します。
		/// 遅延推測器を終了する際、本関数を呼び出してください。
		/// 起動中の遅延推測器スレッドは、本関数の呼び出しによって推測処理を終了します。
		/// 推測処理を終えたスレッドは、自動的に破棄されます。
		/// また、本関数は完了復帰です。多重呼び出しを許容します。
		/// 本関数を呼び出すと、<see cref="CriAtomLatencyEstimator.InitializeANDROID"/> 関数の呼び出しカウンタが
		/// デクリメントされます。本関数と<see cref="CriAtomLatencyEstimator.InitializeANDROID"/> 関数の呼び出し回数が
		/// 同数になるよう注意してください。
		/// </para>
		/// <para>
		/// 注意:
		/// ::criAtom_Initialize_ANDROID 関数実行前に本関数を実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomLatencyEstimator.InitializeANDROID"/>
		public static void FinalizeANDROID()
		{
			NativeMethods.criAtomLatencyEstimator_Finalize_ANDROID();
		}

		/// <summary>遅延推測器の情報取得</summary>
		/// <returns>遅延推測器の情報</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 遅延推測器の現在の情報を取得します。
		/// 取得できる情報は「遅延推測器の状態」「推測遅延時間(ミリ秒)」の２つです。
		/// <see cref="CriAtomLatencyEstimator.InitializeANDROID"/> 関数を呼び出した後、本関数を呼ぶことで
		/// 現在の遅延推測器の情報(状態、推測遅延時間)を取得することができます。
		/// 状態が <see cref="CriAtomLatencyEstimator.Status.Done"/> である時、推測遅延時間は0でない数値になります。
		/// それ以外の状態では、推測遅延時間は0を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomLatencyEstimator.InitializeANDROID"/> 関数実行前に本関数を実行しないでください。
		/// <see cref="CriAtomLatencyEstimator.FinalizeANDROID"/> 関数実行後に本関数を実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomLatencyEstimator.InitializeANDROID"/>
		/// <seealso cref="CriAtomLatencyEstimator.FinalizeANDROID"/>
		public static CriAtomLatencyEstimator.Info GetCurrentInfoANDROID()
		{
			return NativeMethods.criAtomLatencyEstimator_GetCurrentInfo_ANDROID();
		}

		/// <summary>遅延推測器 情報構造体</summary>
		public unsafe partial struct Info
		{
			/// <summary>遅延推測器 状態列挙型</summary>
			public CriAtomLatencyEstimator.Status status;

			public UInt32 latencyMsec;

		}
		/// <summary>遅延推測器 状態列挙型</summary>
		public enum Status
		{
			/// <summary>初期状態/停止状態	(実行中スレッドなし)</summary>
			Stop = 0,
			/// <summary>遅延時間を推測中		(実行中スレッドあり)</summary>
			Processing = 1,
			/// <summary>遅延時間の推測完了	(実行中スレッドなし)</summary>
			Done = 2,
			/// <summary>エラー				(実行中スレッドなし)</summary>
			Error = 3,
		}
		/// <summary>ライブラリ初期化状態の取得</summary>
		/// <returns>初期化済みかどうか</returns>
		/// <returns>未初期化状態</returns>
		/// <returns>初期化済み</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 遅延推測器が既に初期化されているかどうかをチェックします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomLatencyEstimator.InitializeANDROID"/>
		/// <seealso cref="CriAtomLatencyEstimator.FinalizeANDROID"/>
		public static bool IsInitializedANDROID()
		{
			return NativeMethods.criAtomLatencyEstimator_IsInitialized_ANDROID();
		}

	}
}