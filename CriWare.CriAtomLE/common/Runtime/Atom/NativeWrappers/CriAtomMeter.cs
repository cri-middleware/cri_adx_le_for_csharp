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
	/// <summary>CriAtomMeter API</summary>
	public static partial class CriAtomMeter
	{
		/// <summary>レベルメーター機能コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomMeter.AttachLevelMeter"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.LevelMeterConfig"/> ）に、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLevelMeter"/>
		/// <seealso cref="CriAtom.LevelMeterConfig"/>
		public static unsafe void SetDefaultConfigForLevelMeter(out CriAtom.LevelMeterConfig pConfig)
		{
			fixed (CriAtom.LevelMeterConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomMeter_SetDefaultConfigForLevelMeter_(pConfigPtr);
		}

		/// <summary>ラウドネスメーター機能コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomMeter.AttachLoudnessMeter"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.LoudnessMeterConfig"/> ）に、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLoudnessMeter"/>
		/// <seealso cref="CriAtom.LoudnessMeterConfig"/>
		public static unsafe void SetDefaultConfigForLoudnessMeter(out CriAtom.LoudnessMeterConfig pConfig)
		{
			fixed (CriAtom.LoudnessMeterConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomMeter_SetDefaultConfigForLoudnessMeter_(pConfigPtr);
		}

		/// <summary>トゥルーピークメーター機能コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomMeter.AttachTruePeakMeter"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtom.TruePeakMeterConfig"/> ）に、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachTruePeakMeter"/>
		/// <seealso cref="CriAtom.TruePeakMeterConfig"/>
		public static unsafe void SetDefaultConfigForTruePeakMeter(out CriAtom.TruePeakMeterConfig pConfig)
		{
			fixed (CriAtom.TruePeakMeterConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomMeter_SetDefaultConfigForTruePeakMeter_(pConfigPtr);
		}

		/// <summary>レベルメーター機能用のワークサイズの計算</summary>
		/// <param name="config">レベルメーター追加用のコンフィグ構造体</param>
		/// <returns>必要なワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// レベルメーター追加に必要なワーク領域サイズを計算します。
		/// config にnullを指定するとデフォルト設定で計算されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLevelMeter"/>
		public static unsafe Int32 CalculateWorkSizeForLevelMeter(in CriAtom.LevelMeterConfig config)
		{
			fixed (CriAtom.LevelMeterConfig* configPtr = &config)
				return NativeMethods.criAtomMeter_CalculateWorkSizeForLevelMeter(configPtr);
		}

		/// <summary>レベルメーター機能の追加</summary>
		/// <param name="config">レベルメーター追加用のコンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリにレベルメーター機能を追加します。
		/// config にnullを指定するとデフォルト設定でレベルメーターが追加されます。
		/// work にnull、work_size に0を指定すると、登録されたユーザアロケーターによって
		/// ワーク領域が確保されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.GetLevelInfo"/>
		public static unsafe void AttachLevelMeter(in CriAtom.LevelMeterConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.LevelMeterConfig* configPtr = &config)
				NativeMethods.criAtomMeter_AttachLevelMeter(configPtr, work, workSize);
		}

		/// <summary>レベルメーター機能の解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリのレベルメーター機能を解除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLevelMeter"/>
		public static void DetachLevelMeter()
		{
			NativeMethods.criAtomMeter_DetachLevelMeter();
		}

		/// <summary>レベル情報の取得</summary>
		/// <param name="info">レベル情報の構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// レベルメーターの結果を取得します。
		/// 指定するバスには <see cref="CriAtomMeter.AttachLevelMeter"/> 関数であらかじめ
		/// レベルメーター機能を追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLevelMeter"/>
		public static unsafe void GetLevelInfo(out CriAtom.LevelInfo info)
		{
			fixed (CriAtom.LevelInfo* infoPtr = &info)
				NativeMethods.criAtomMeter_GetLevelInfo(infoPtr);
		}

		/// <summary>ラウドネスメーター機能用のワークサイズの計算</summary>
		/// <param name="config">ラウドネスメーター追加用のコンフィグ構造体</param>
		/// <returns>必要なワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ITU-R BS.1770-3規格のラウドネスメーター追加に必要なワーク領域サイズを計算します。
		/// config にnullを指定するとデフォルト設定で計算されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLoudnessMeter"/>
		public static unsafe Int32 CalculateWorkSizeForLoudnessMeter(in CriAtom.LoudnessMeterConfig config)
		{
			fixed (CriAtom.LoudnessMeterConfig* configPtr = &config)
				return NativeMethods.criAtomMeter_CalculateWorkSizeForLoudnessMeter(configPtr);
		}

		/// <summary>ラウドネスメーター機能の追加</summary>
		/// <param name="config">ラウドネスメーター追加用のコンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリにITU-R BS.1770-3規格のラウドネスメーター機能を追加します。
		/// config にnullを指定するとデフォルト設定でラウドネスメーターが追加されます。
		/// work にnull、work_size に0を指定すると、登録されたユーザアロケーターによって
		/// ワーク領域が確保されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.GetLoudnessInfo"/>
		public static unsafe void AttachLoudnessMeter(in CriAtom.LoudnessMeterConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.LoudnessMeterConfig* configPtr = &config)
				NativeMethods.criAtomMeter_AttachLoudnessMeter(configPtr, work, workSize);
		}

		/// <summary>ラウドネスメーター機能の解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリのラウドネスメーター機能を解除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLoudnessMeter"/>
		public static void DetachLoudnessMeter()
		{
			NativeMethods.criAtomMeter_DetachLoudnessMeter();
		}

		/// <summary>ラウドネス情報の取得</summary>
		/// <param name="info">ラウドネス情報の構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ラウドネスメーターの測定結果を取得します。
		/// 本関数を呼び出す前にライブラリへラウドネスメーターを追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLoudnessMeter"/>
		public static unsafe void GetLoudnessInfo(out CriAtom.LoudnessInfo info)
		{
			fixed (CriAtom.LoudnessInfo* infoPtr = &info)
				NativeMethods.criAtomMeter_GetLoudnessInfo(infoPtr);
		}

		/// <summary>ラウドネスメーターのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ラウドネスメーターの蓄積データをリセットします。
		/// 本関数を呼び出す前にライブラリへラウドネスメーターを追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLoudnessMeter"/>
		public static void ResetLoudnessMeter()
		{
			NativeMethods.criAtomMeter_ResetLoudnessMeter();
		}

		/// <summary>トゥルーピークメーター機能用のワークサイズの計算</summary>
		/// <param name="config">トゥルーピークメーター追加用のコンフィグ構造体</param>
		/// <returns>必要なワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ITU-R BS.1770-3規格のトゥルーピークメーター追加に必要なワーク領域サイズを計算します。
		/// config にnullを指定するとデフォルト設定で計算されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachTruePeakMeter"/>
		public static unsafe Int32 CalculateWorkSizeForTruePeakMeter(in CriAtom.TruePeakMeterConfig config)
		{
			fixed (CriAtom.TruePeakMeterConfig* configPtr = &config)
				return NativeMethods.criAtomMeter_CalculateWorkSizeForTruePeakMeter(configPtr);
		}

		/// <summary>トゥルーピークメーター機能の追加</summary>
		/// <param name="config">トゥルーピークメーター追加用のコンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリにITU-R BS.1770-3規格のトゥルーピークメーター機能を追加します。
		/// config にnullを指定するとデフォルト設定でトゥルーピークメーターが追加されます。
		/// work にnull、work_size に0を指定すると、登録されたユーザアロケーターによって
		/// ワーク領域が確保されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.GetTruePeakInfo"/>
		public static unsafe void AttachTruePeakMeter(in CriAtom.TruePeakMeterConfig config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtom.TruePeakMeterConfig* configPtr = &config)
				NativeMethods.criAtomMeter_AttachTruePeakMeter(configPtr, work, workSize);
		}

		/// <summary>トゥルーピークメーター機能の解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリのトゥルーピークメーター機能を解除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLoudnessMeter"/>
		public static void DetachTruePeakMeter()
		{
			NativeMethods.criAtomMeter_DetachTruePeakMeter();
		}

		/// <summary>トゥルーピーク情報の取得</summary>
		/// <param name="info">トゥルーピーク情報の構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥルーピークメーターの測定結果を取得します。
		/// 本関数を呼び出す前にライブラリへトゥルーピークメーターを追加しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachTruePeakMeter"/>
		public static unsafe void GetTruePeakInfo(out CriAtom.TruePeakInfo info)
		{
			fixed (CriAtom.TruePeakInfo* infoPtr = &info)
				NativeMethods.criAtomMeter_GetTruePeakInfo(infoPtr);
		}

	}
}