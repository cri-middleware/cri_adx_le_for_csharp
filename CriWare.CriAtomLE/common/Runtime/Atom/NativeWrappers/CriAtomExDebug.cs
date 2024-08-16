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
	/// <summary>CriAtomExDebug API</summary>
	public static partial class CriAtomExDebug
	{
		/// <summary>CriAtomEx 内部の各種リソースの状況の取得</summary>
		/// <param name="resourcesInfo">CriAtomEx 内部の各種リソースの状況</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CriAtomEx 内部の各種リソースの状況取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// 開発支援デバッグ機能です。アプリケーション開発時にのみ使用してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExDebug.ResourcesInfo"/>
		public static unsafe void GetResourcesInfo(out CriAtomExDebug.ResourcesInfo resourcesInfo)
		{
			fixed (CriAtomExDebug.ResourcesInfo* resourcesInfoPtr = &resourcesInfo)
				NativeMethods.criAtomExDebug_GetResourcesInfo(resourcesInfoPtr);
		}

		/// <summary>CriAtomEx 内部の各種リソースの状況</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CriAtomEx 内部の各種リソースの状況を表す構造体です。
		/// <see cref="CriAtomExDebug.GetResourcesInfo"/> を使用して取得してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 開発支援デバッグ機能です。アプリケーション開発時にのみ使用してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExDebug.GetResourcesInfo"/>
		public unsafe partial struct ResourcesInfo
		{
			/// <summary>バーチャルボイスの使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_virtual_voices の数）</summary>
			public CriAtomEx.ResourceUsage virtualVoiceUsage;

			/// <summary>シーケンスの使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_sequences の数）</summary>
			public CriAtomEx.ResourceUsage sequenceUsage;

			/// <summary>シーケンストラックの使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_tracks の数）</summary>
			public CriAtomEx.ResourceUsage sequenceTrackUsage;

			/// <summary>シーケンストラックアイテムの使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_track_items の数）</summary>
			public CriAtomEx.ResourceUsage sequenceTrackItemUsage;

			/// <summary>パラメーターブロックの使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_parameter_blocks の数）</summary>
			public CriAtomEx.ResourceUsage parameterBlock;

			/// <summary>ビート同期情報の使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_virtual_voices の数）</summary>
			public CriAtomEx.ResourceUsage beatSyncInfo;

			/// <summary>ビート同期遷移設定の使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_virtual_voices の数）</summary>
			public CriAtomEx.ResourceUsage beatSyncTransitionSetting;

			/// <summary>ビート同期ジョブの使用状況（limit はライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/>::max_virtual_voices * 2 の数）</summary>
			public CriAtomEx.ResourceUsage beatSyncJob;

		}
	}
}