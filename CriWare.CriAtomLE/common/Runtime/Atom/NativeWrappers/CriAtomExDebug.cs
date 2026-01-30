/****************************************************************************
 *
 * Copyright (c) 2025 CRI Middleware Co., Ltd.
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
		/// <summary>CriAtomEx 内部の各種リソースの状況の取得 </summary>
		/// <param name="resourcesInfo">CriAtomEx 内部の各種リソースの状況 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CriAtomEx 内部の各種リソースの状況取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// 開発支援デバッグ機能です。アプリケーション開発時にのみ使用してください。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExDebug_GetResourcesInfo(CriAtomExDebugResourcesInfo *resources_info)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExDebug.ResourcesInfo"/>
		public static unsafe void GetResourcesInfo(out CriAtomExDebug.ResourcesInfo resourcesInfo)
		{
			fixed (CriAtomExDebug.ResourcesInfo* resourcesInfoPtr = &resourcesInfo)
				NativeMethods.criAtomExDebug_GetResourcesInfo(resourcesInfoPtr);
		}

		/// <summary>CriAtomEx 内部の各種リソースの状況 </summary>
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
			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_virtual_voices</see>
			/// <para>
			/// バーチャルボイスの使用状況（limit はライブラリ初期化時に指定した 
			///  の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage virtualVoiceUsage;

			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_sequences</see>
			/// <para>
			/// シーケンスの使用状況（limit はライブラリ初期化時に指定した 
			///  の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage sequenceUsage;

			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_tracks</see>
			/// <para>
			/// シーケンストラックの使用状況（limit はライブラリ初期化時に指定した 
			///  の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage sequenceTrackUsage;

			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_track_items</see>
			/// <para>
			/// シーケンストラックアイテムの使用状況（limit はライブラリ初期化時に指定した 
			///  の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage sequenceTrackItemUsage;

			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_parameter_blocks</see>
			/// <para>
			/// パラメーターブロックの使用状況（limit はライブラリ初期化時に指定した 
			///  の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage parameterBlock;

			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_virtual_voices</see>
			/// <para>
			/// ビート同期情報の使用状況（limit はライブラリ初期化時に指定した 
			///  の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage beatSyncInfo;

			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_virtual_voices</see>
			/// <para>
			/// ビート同期遷移設定の使用状況（limit はライブラリ初期化時に指定した 
			///  の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage beatSyncTransitionSetting;

			/// <summary></summary>
			/// <remarks>
			/// <see><see cref="CriAtomEx.Config"/>::max_virtual_voices</see>
			/// <para>
			/// ビート同期ジョブの使用状況（limit はライブラリ初期化時に指定した 
			///  * 2 の数） 
			/// </para>
			/// </remarks>
			public CriAtomEx.ResourceUsage beatSyncJob;

		}
	}
}