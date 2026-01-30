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
	/// <summary>CriAtomExCategory API</summary>
	public partial class CriAtomExCategory
	{
		/// <summary>カテゴリ情報取得用構造体 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリ情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetCategoryInfo"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetCategoryInfo"/>
		public unsafe partial struct Info
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>グループ番号 </para>
			/// </remarks>
			public UInt32 groupNo;

			/// <summary></summary>
			/// <remarks>
			/// <para>カテゴリID </para>
			/// </remarks>
			public UInt32 id;

			/// <summary></summary>
			/// <remarks>
			/// <para>カテゴリ名 </para>
			/// </remarks>
			public NativeString name;

			/// <summary></summary>
			/// <remarks>
			/// <para>キューリミット数 </para>
			/// </remarks>
			public UInt32 numCueLimits;

			/// <summary></summary>
			/// <remarks>
			/// <para>ボリューム </para>
			/// </remarks>
			public Single volume;

		}
		/// <summary>ID指定によるカテゴリに対するボリューム設定 </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="volume">ボリューム値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリに対してボリュームを設定します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数による設定値はACFによって設定されているカテゴリボリュームを上書き変更します。
		///  本関数による設定値とACF設定値との乗算適用は行われないことに注意してください。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetVolumeById(CriAtomExCategoryId id, CriFloat32 volume)"/>
		/// </remarks>
		public static void SetVolumeById(UInt32 id, Single volume)
		{
			NativeMethods.criAtomExCategory_SetVolumeById(id, volume);
		}

		/// <summary>ID指定によるカテゴリボリューム取得 </summary>
		/// <param name="id">カテゴリID return CriFloat32 カテゴリボリューム </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリのボリュームを取得します。 
		/// </para>
		/// <nativeinfo declaration="CriFloat32 criAtomExCategory_GetVolumeById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static Single GetVolumeById(UInt32 id)
		{
			return NativeMethods.criAtomExCategory_GetVolumeById(id);
		}

		/// <summary>ID指定による最終カテゴリボリューム取得 </summary>
		/// <param name="id">カテゴリID return CriFloat32 カテゴリボリューム </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でREACT、AISACなどの影響を受けた最終的なカテゴリのボリューム値を取得します。 
		/// </para>
		/// <para>
		/// 備考:
		/// 最終的なカテゴリのボリューム値を取得するためにパラメーターの計算処理を行うので、 負荷が大きい関数です。 
		/// </para>
		/// <nativeinfo declaration="CriFloat32 criAtomExCategory_GetTotalVolumeById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static Single GetTotalVolumeById(UInt32 id)
		{
			return NativeMethods.criAtomExCategory_GetTotalVolumeById(id);
		}

		/// <summary>名前指定によるカテゴリに対するボリューム設定 </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="volume">ボリューム値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリに対してボリュームを設定します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数による設定値はACFによって設定されているカテゴリボリュームを上書き変更します。
		///  本関数による設定値とACF設定値との乗算適用は行われないことに注意してください。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetVolumeByName(const CriChar8 *name, CriFloat32 volume)"/>
		/// </remarks>
		public static void SetVolumeByName(ArgString name, Single volume)
		{
			NativeMethods.criAtomExCategory_SetVolumeByName(name.GetPointer(stackalloc byte[name.BufferSize]), volume);
		}

		/// <summary>名前指定によるカテゴリボリューム取得 </summary>
		/// <param name="name">カテゴリ名 return CriFloat32 カテゴリボリューム </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリのボリュームを取得します。 
		/// </para>
		/// <nativeinfo declaration="CriFloat32 criAtomExCategory_GetVolumeByName(const CriChar8 *name)"/>
		/// </remarks>
		public static Single GetVolumeByName(ArgString name)
		{
			return NativeMethods.criAtomExCategory_GetVolumeByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>名前指定による最終カテゴリボリューム取得 </summary>
		/// <param name="name">カテゴリ名 return CriFloat32 カテゴリボリューム </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でREACT、AISACなどの影響を受けた最終的なカテゴリのボリューム値を取得します。 
		/// </para>
		/// <para>
		/// 備考:
		/// 最終的なカテゴリのボリューム値を取得するためにパラメーターの計算処理を行うので、 負荷が大きい関数です。 
		/// </para>
		/// <nativeinfo declaration="CriFloat32 criAtomExCategory_GetTotalVolumeByName(const CriChar8 *name)"/>
		/// </remarks>
		public static Single GetTotalVolumeByName(ArgString name)
		{
			return NativeMethods.criAtomExCategory_GetTotalVolumeByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ID指定によるカテゴリミュート状態設定 </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="mute">ミュート状態（CRI_TRUE = ミュート、CRI_FALSE = ミュート解除） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリのミュート状態を設定します。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_MuteById(CriAtomExCategoryId id, CriBool mute)"/>
		/// </remarks>
		public static void MuteById(UInt32 id, NativeBool mute)
		{
			NativeMethods.criAtomExCategory_MuteById(id, mute);
		}

		/// <summary>ID指定によるカテゴリミュート状態取得 </summary>
		/// <param name="id">カテゴリID return CriBool ミュート状態（CRI_TRUE = ミュート中、CRI_FALSE = ミュートされていない） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリのミュート状態を取得します。 
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_IsMutedById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static bool IsMutedById(UInt32 id)
		{
			return NativeMethods.criAtomExCategory_IsMutedById(id);
		}

		/// <summary>名前指定によるカテゴリミュート状態設定 </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="mute">ミュート状態（CRI_TRUE = ミュート、CRI_FALSE = ミュート解除） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリのミュート状態を設定します。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_MuteByName(const CriChar8 *name, CriBool mute)"/>
		/// </remarks>
		public static void MuteByName(ArgString name, NativeBool mute)
		{
			NativeMethods.criAtomExCategory_MuteByName(name.GetPointer(stackalloc byte[name.BufferSize]), mute);
		}

		/// <summary>名前指定によるカテゴリミュート状態取得 </summary>
		/// <param name="name">カテゴリ名 return CriBool ミュート状態（CRI_TRUE = ミュート中、CRI_FALSE = ミュートされていない） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリのミュート状態を取得します。 
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_IsMutedByName(const CriChar8 *name)"/>
		/// </remarks>
		public static bool IsMutedByName(ArgString name)
		{
			return NativeMethods.criAtomExCategory_IsMutedByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ID指定によるカテゴリソロ状態設定 </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="solo">ソロ状態（CRI_TRUE = ソロ、CRI_FALSE = ソロ解除） </param>
		/// <param name="muteVolume">他のカテゴリに適用するミュートボリューム値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリのソロ状態を設定します。
		///  mute_volumeで指定したボリュームは同一カテゴリグループに所属する カテゴリに対して適用されます。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SoloById(CriAtomExCategoryId id, CriBool solo, CriFloat32 mute_volume)"/>
		/// </remarks>
		public static void SoloById(UInt32 id, NativeBool solo, Single muteVolume)
		{
			NativeMethods.criAtomExCategory_SoloById(id, solo, muteVolume);
		}

		/// <summary>ID指定によるカテゴリソロ状態取得 </summary>
		/// <param name="id">カテゴリID return CriBool ソロ状態（CRI_TRUE = ソロ中、CRI_FALSE = ソロではない） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリのソロ状態を取得します。 
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_IsSoloedById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static bool IsSoloedById(UInt32 id)
		{
			return NativeMethods.criAtomExCategory_IsSoloedById(id);
		}

		/// <summary>名前指定によるカテゴリソロ状態設定 </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="solo">ソロ状態（CRI_TRUE = ソロ、CRI_FALSE = ソロ解除） </param>
		/// <param name="muteVolume">他のカテゴリに適用するミュートボリューム値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリのソロ状態を設定します。
		///  mute_volumeで指定したボリュームは同一カテゴリグループに所属する カテゴリに対して適用されます。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SoloByName(const CriChar8 *name, CriBool solo, CriFloat32 mute_volume)"/>
		/// </remarks>
		public static void SoloByName(ArgString name, NativeBool solo, Single muteVolume)
		{
			NativeMethods.criAtomExCategory_SoloByName(name.GetPointer(stackalloc byte[name.BufferSize]), solo, muteVolume);
		}

		/// <summary>名前指定によるカテゴリソロ状態取得 </summary>
		/// <param name="name">カテゴリ名 return CriBool ソロ状態（CRI_TRUE = ソロ中、CRI_FALSE = ソロではない） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリのソロ状態を取得します。 
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_IsSoloedByName(const CriChar8 *name)"/>
		/// </remarks>
		public static bool IsSoloedByName(ArgString name)
		{
			return NativeMethods.criAtomExCategory_IsSoloedByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ID指定によるカテゴリのポーズ／ポーズ解除 </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="sw">スイッチ（CRI_FALSE = ポーズ解除、CRI_TRUE = ポーズ） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリのポーズ／ポーズ解除を行います。
		/// <see cref="CriAtomExCategory.SetFadeOutTimeById"/> 関数や<see cref="CriAtomExCategory.SetFadeOutTimeByName"/> 関数でフェードアウト時間が設定されている場合にポーズを行うと、設定された時間でフェードアウトした後に実際にポーズします。
		/// <see cref="CriAtomExCategory.SetFadeInTimeById"/> 関数や<see cref="CriAtomExCategory.SetFadeInTimeByName"/> 関数でフェードイン時間が設定されている場合にポーズ解除を行うと、ポーズ解除後、設定された時間でフェードインします。
		/// </para>
		/// <para>
		/// 備考:
		/// カテゴリのポーズは、AtomExプレーヤー／再生音のポーズ （<see cref="CriAtomExPlayer.Pause"/> 関数や<see cref="CriAtomExPlayback.Pause"/> 関数でのポーズ）とは独立して扱われ、 音声の最終的なポーズ状態は、それぞれのポーズ状態を考慮して決まります。
		///  すなわち、どちらかがポーズ状態ならポーズ、どちらもポーズ解除状態ならポーズ解除、となります。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_PauseById(CriAtomExCategoryId id, CriBool sw)"/>
		/// </remarks>
		public static void PauseById(UInt32 id, NativeBool sw)
		{
			NativeMethods.criAtomExCategory_PauseById(id, sw);
		}

		/// <summary>ID指定によるカテゴリのポーズ状態取得 </summary>
		/// <param name="id">カテゴリID return CriBool ポーズ状態 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリのポーズ状態を取得します。 
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_IsPausedById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static bool IsPausedById(UInt32 id)
		{
			return NativeMethods.criAtomExCategory_IsPausedById(id);
		}

		/// <summary>名前指定によるカテゴリのポーズ／ポーズ解除 </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="sw">スイッチ（CRI_FALSE = ポーズ解除、CRI_TRUE = ポーズ） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリのポーズ／ポーズ解除を行います。
		///  カテゴリを名前で指定する以外は、<see cref="CriAtomExCategory.PauseById"/> 関数と仕様は同じです。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_PauseByName(const CriChar8 *name, CriBool sw)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.PauseById"/>
		public static void PauseByName(ArgString name, NativeBool sw)
		{
			NativeMethods.criAtomExCategory_PauseByName(name.GetPointer(stackalloc byte[name.BufferSize]), sw);
		}

		/// <summary>名前指定によるカテゴリのポーズ状態取得 </summary>
		/// <param name="name">カテゴリ名 return CriBool ポーズ状態 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリのポーズ状態を取得します。 
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_IsPausedByName(const CriChar8 *name)"/>
		/// </remarks>
		public static bool IsPausedByName(ArgString name)
		{
			return NativeMethods.criAtomExCategory_IsPausedByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>フェードイン時間の設定（カテゴリID指定） </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="ms">フェードイン時間（ミリ秒単位） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにフェードイン時間を設定します。
		///  フェードイン時間はポーズ解除を行った際に利用されます。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetFadeInTimeById(CriAtomExCategoryId id, CriUint16 ms)"/>
		/// </remarks>
		public static void SetFadeInTimeById(UInt32 id, UInt16 ms)
		{
			NativeMethods.criAtomExCategory_SetFadeInTimeById(id, ms);
		}

		/// <summary>フェードイン時間の設定（カテゴリ名指定） </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="ms">フェードイン時間（ミリ秒単位） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにフェードイン時間を設定します。
		///  フェードイン時間はポーズ解除を行った際に利用されます。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetFadeInTimeByName(const CriChar8 *name, CriUint16 ms)"/>
		/// </remarks>
		public static void SetFadeInTimeByName(ArgString name, UInt16 ms)
		{
			NativeMethods.criAtomExCategory_SetFadeInTimeByName(name.GetPointer(stackalloc byte[name.BufferSize]), ms);
		}

		/// <summary>フェードアウト時間の設定（カテゴリID指定） </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="ms">フェードアウト時間（ミリ秒単位） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにフェードアウト時間を設定します。
		///  フェードアウト時間はポーズを行った際に利用されます。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetFadeOutTimeById(CriAtomExCategoryId id, CriUint16 ms)"/>
		/// </remarks>
		public static void SetFadeOutTimeById(UInt32 id, UInt16 ms)
		{
			NativeMethods.criAtomExCategory_SetFadeOutTimeById(id, ms);
		}

		/// <summary>フェードアウト時間の設定（カテゴリ名指定） </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="ms">フェードアウト時間（ミリ秒単位） </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにフェードアウト時間を設定します。
		///  フェードアウト時間はポーズを行った際に利用されます。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetFadeOutTimeByName(const CriChar8 *name, CriUint16 ms)"/>
		/// </remarks>
		public static void SetFadeOutTimeByName(ArgString name, UInt16 ms)
		{
			NativeMethods.criAtomExCategory_SetFadeOutTimeByName(name.GetPointer(stackalloc byte[name.BufferSize]), ms);
		}

		/// <summary>ID指定によるカテゴリに対するAISACコントロール値設定 </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="controlId">AISACコントロールID </param>
		/// <param name="controlValue">AISACコントロール値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ID指定でカテゴリに対してAISACコントロール値を設定します。
		/// </para>
		/// <para>
		/// 備考:
		/// カテゴリをIDで、AISACコントロールを名前で指定したい場合、<see cref="CriAtomExAcf.GetAisacControlIdByName"/> 関数にて変換を行ってください。 
		/// </para>
		/// <para>
		/// 注意:
		/// キューやトラックに設定されているAISACに関しては、プレーヤーでのAISACコントロール値設定よりも、<b>カテゴリのAISACコントロール値を優先して</b>参照します。
		///  カテゴリにアタッチしたAISACについては、常に<b>カテゴリに設定したAISACコントロール値のみ</b>、参照されます。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetAisacControlById(CriAtomExCategoryId id, CriAtomExAisacControlId control_id, CriFloat32 control_value)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.SetAisacControlByName"/>
		/// <seealso cref="CriAtomExCategory.AttachAisacById"/>
		/// <seealso cref="CriAtomExCategory.AttachAisacByName"/>
		public static void SetAisacControlById(UInt32 id, UInt32 controlId, Single controlValue)
		{
			NativeMethods.criAtomExCategory_SetAisacControlById(id, controlId, controlValue);
		}

		/// <summary>名前指定によるカテゴリに対するAISACコントロール値設定 </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="controlName">AISACコントロール名 </param>
		/// <param name="controlValue">AISACコントロール値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 名前指定でカテゴリに対してAISACコントロール値を設定します。
		///  カテゴリおよびAISACコントロールを名前で指定する以外は、<see cref="CriAtomExCategory.SetAisacControlById"/> 関数と仕様は同じです。
		/// </para>
		/// <para>
		/// 備考:
		/// カテゴリを名前、AISACコントロールをIDで指定したい場合、<see cref="CriAtomExAcf.GetAisacControlNameById"/> 関数にて変換を行ってください。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetAisacControlByName(const CriChar8 *name, const CriChar8 *control_name, CriFloat32 control_value)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.SetAisacControlById"/>
		/// <seealso cref="CriAtomExCategory.AttachAisacById"/>
		/// <seealso cref="CriAtomExCategory.AttachAisacByName"/>
		public static void SetAisacControlByName(ArgString name, ArgString controlName, Single controlValue)
		{
			NativeMethods.criAtomExCategory_SetAisacControlByName(name.GetPointer(stackalloc byte[name.BufferSize]), controlName.GetPointer(stackalloc byte[controlName.BufferSize]), controlValue);
		}

		/// <summary>ID指定でカテゴリにアタッチされている全てのAISACコントロール値をデフォルト値に設定する </summary>
		/// <param name="categoryId">カテゴリID </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされている全てのAISACコントロール値をデフォルト値に設定します。
		///  存在しないカテゴリを指定した場合、falseが返ります。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_ResetAllAisacControlById(CriAtomExCategoryId category_id)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.GetNumAttachedAisacsById"/>
		public static bool ResetAllAisacControlById(UInt32 categoryId)
		{
			return NativeMethods.criAtomExCategory_ResetAllAisacControlById(categoryId);
		}

		/// <summary>名前指定でカテゴリにアタッチされている全てのAISACコントロール値をデフォルト値に設定する </summary>
		/// <param name="categoryName">カテゴリ名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされている全てのAISACコントロール値をデフォルト値に設定します。
		///  存在しないカテゴリを指定した場合、falseが返ります。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_ResetAllAisacControlByName(const CriChar8 *category_name)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.GetNumAttachedAisacsById"/>
		public static bool ResetAllAisacControlByName(ArgString categoryName)
		{
			return NativeMethods.criAtomExCategory_ResetAllAisacControlByName(categoryName.GetPointer(stackalloc byte[categoryName.BufferSize]));
		}

		/// <summary>ID指定でカテゴリにAISACを取り付ける </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="globalAisacName">取り付けるグローバルAISAC名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにAISACをアタッチ（取り付け）します。 AISACをアタッチすることにより、キューやトラックにAISACを設定していなくても、AISACの効果を得ることができます。
		///  AISACのアタッチに失敗した場合、関数内でエラーコールバックが発生します。
		///  AISACのアタッチに失敗した理由については、エラーコールバックのメッセージを確認してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 全体設定（ACFファイル）に含まれるグローバルAISACのみ、アタッチ可能です。
		///  AISACの効果を得るには、キューやトラックに設定されているAISACと同様に、該当するAISACコントロール値を設定する必要があります。
		/// </para>
		/// <para>
		/// 注意:
		/// キューやトラックに「AISACコントロール値を変更するAISAC」が設定されていたとしても、 その適用結果のAISACコントロール値は、カテゴリにアタッチしたAISACには影響しません。
		///  カテゴリにアタッチしたAISACについては、常に<b>カテゴリに設定したAISACコントロール値のみ</b>、参照されます。
		///  現在、「オートモジュレーション」や「ランダム」といったコントロールタイプのAISACのアタッチには対応しておりません。
		///  現在、カテゴリにアタッチできるAISACの最大数は、8個固定です。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_AttachAisacById(CriAtomExCategoryId id, const CriChar8 *global_aisac_name)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.DetachAisacById"/>
		public static void AttachAisacById(UInt32 id, ArgString globalAisacName)
		{
			NativeMethods.criAtomExCategory_AttachAisacById(id, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>名前指定でカテゴリにAISACを取り付ける </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="globalAisacName">取り付けるグローバルAISAC名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにAISACをアタッチ（取り付け）します。 カテゴリを名前で指定する以外は、<see cref="CriAtomExCategory.AttachAisacById"/> 関数と仕様は同じです。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_AttachAisacByName(const CriChar8 *name, const CriChar8 *global_aisac_name)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.AttachAisacById"/>
		/// <seealso cref="CriAtomExCategory.DetachAisacByName"/>
		public static void AttachAisacByName(ArgString name, ArgString globalAisacName)
		{
			NativeMethods.criAtomExCategory_AttachAisacByName(name.GetPointer(stackalloc byte[name.BufferSize]), globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>ID指定でカテゴリからAISACを取り外す </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="globalAisacName">取り外すグローバルAISAC名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリからAISACをデタッチ（取り外し）します。
		///  AISACのデタッチに失敗した場合、関数内でエラーコールバックが発生します。
		///  AISACのデタッチに失敗した理由については、エラーコールバックのメッセージを確認してください。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_DetachAisacById(CriAtomExCategoryId id, const CriChar8 *global_aisac_name)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.AttachAisacById"/>
		public static void DetachAisacById(UInt32 id, ArgString globalAisacName)
		{
			NativeMethods.criAtomExCategory_DetachAisacById(id, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>名前指定でカテゴリからAISACを取り外す </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="globalAisacName">取り外すグローバルAISAC名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリからAISACをデタッチ（取り外し）します。
		///  カテゴリを名前で指定する以外は、<see cref="CriAtomExCategory.DetachAisacById"/> 関数と仕様は同じです。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_DetachAisacByName(const CriChar8 *name, const CriChar8 *global_aisac_name)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.DetachAisacById"/>
		/// <seealso cref="CriAtomExCategory.AttachAisacByName"/>
		public static void DetachAisacByName(ArgString name, ArgString globalAisacName)
		{
			NativeMethods.criAtomExCategory_DetachAisacByName(name.GetPointer(stackalloc byte[name.BufferSize]), globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>ID指定でカテゴリから全てのAISACを取り外す </summary>
		/// <param name="id">カテゴリID </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリから全てのAISACをデタッチ（取り外し）します。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_DetachAisacAllById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static void DetachAisacAllById(UInt32 id)
		{
			NativeMethods.criAtomExCategory_DetachAisacAllById(id);
		}

		/// <summary>名前指定でカテゴリから全てのAISACを取り外す </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリから全てのAISACをデタッチ（取り外し）します。 
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_DetachAisacAllByName(const CriChar8 *name)"/>
		/// </remarks>
		public static void DetachAisacAllByName(ArgString name)
		{
			NativeMethods.criAtomExCategory_DetachAisacAllByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ID指定でカテゴリにアタッチされているAISAC数を取得する </summary>
		/// <param name="id">カテゴリID </param>
		/// <returns>カテゴリにアタッチされているAISAC数 </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされているAISAC数を取得します。
		///  存在しないカテゴリを指定した場合、負値が返ります。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomExCategory_GetNumAttachedAisacsById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static Int32 GetNumAttachedAisacsById(UInt32 id)
		{
			return NativeMethods.criAtomExCategory_GetNumAttachedAisacsById(id);
		}

		/// <summary>名前指定でカテゴリにアタッチされているAISAC数を取得する </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <returns>カテゴリにアタッチされているAISAC数 </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされているAISAC数を取得します。 存在しないカテゴリを指定した場合、負値が返ります。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomExCategory_GetNumAttachedAisacsByName(const CriChar8 *name)"/>
		/// </remarks>
		public static Int32 GetNumAttachedAisacsByName(ArgString name)
		{
			return NativeMethods.criAtomExCategory_GetNumAttachedAisacsByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ID指定でカテゴリにアタッチされているAISACの情報を取得する </summary>
		/// <param name="id">カテゴリID </param>
		/// <param name="aisacAttachedIndex">アタッチされているAISACのインデックス </param>
		/// <param name="aisacInfo">AISAC情報 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされているAISACの情報を取得します。
		///  存在しないカテゴリを指定した場合や、無効なインデックスを指定した場合、falseが返ります。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_GetAttachedAisacInfoById(CriAtomExCategoryId id, CriSint32 aisac_attached_index, CriAtomExAisacInfo *aisac_info)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.GetNumAttachedAisacsById"/>
		public static unsafe bool GetAttachedAisacInfoById(UInt32 id, Int32 aisacAttachedIndex, out CriAtomEx.AisacInfo aisacInfo)
		{
			fixed (CriAtomEx.AisacInfo* aisacInfoPtr = &aisacInfo)
				return NativeMethods.criAtomExCategory_GetAttachedAisacInfoById(id, aisacAttachedIndex, aisacInfoPtr);
		}

		/// <summary>名前指定でカテゴリにアタッチされているAISACの情報を取得する </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <param name="aisacAttachedIndex">アタッチされているAISACのインデックス </param>
		/// <param name="aisacInfo">AISAC情報 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされているAISACの情報を取得します。
		///  存在しないカテゴリを指定した場合や、無効なインデックスを指定した場合、falseが返ります。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_GetAttachedAisacInfoByName(const CriChar8 *name, CriSint32 aisac_attached_index, CriAtomExAisacInfo *aisac_info)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.GetNumAttachedAisacsByName"/>
		public static unsafe bool GetAttachedAisacInfoByName(ArgString name, Int32 aisacAttachedIndex, out CriAtomEx.AisacInfo aisacInfo)
		{
			fixed (CriAtomEx.AisacInfo* aisacInfoPtr = &aisacInfo)
				return NativeMethods.criAtomExCategory_GetAttachedAisacInfoByName(name.GetPointer(stackalloc byte[name.BufferSize]), aisacAttachedIndex, aisacInfoPtr);
		}

		/// <summary>ID指定でカテゴリにアタッチされているAISACコントロールの現在値を取得する </summary>
		/// <param name="categoryId">カテゴリID </param>
		/// <param name="aisacControlId">AISACコントロールID </param>
		/// <param name="controlValue">AISACコントロールの現在値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされているAISACコントロールの現在値を取得します。
		///  存在しないカテゴリやAISACコントロールを指定した場合、falseが返ります。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_GetCurrentAisacControlValueById(CriAtomExCategoryId category_id, CriAtomExAisacControlId aisac_control_id, CriFloat32 *control_value)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.GetNumAttachedAisacsById"/>
		public static unsafe bool GetCurrentAisacControlValueById(UInt32 categoryId, UInt32 aisacControlId, out Single controlValue)
		{
			fixed (Single* controlValuePtr = &controlValue)
				return NativeMethods.criAtomExCategory_GetCurrentAisacControlValueById(categoryId, aisacControlId, controlValuePtr);
		}

		/// <summary>名前指定でカテゴリにアタッチされているAISACコントロールの現在値を取得する </summary>
		/// <param name="categoryName">カテゴリ名 </param>
		/// <param name="aisacControlName">AISACコントロール名 </param>
		/// <param name="controlValue">AISACコントロールの現在値 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリにアタッチされているAISACコントロールの現在値を取得します。
		///  存在しないカテゴリを指定した場合や、無効なインデックスを指定した場合、falseが返ります。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_GetCurrentAisacControlValueByName(const CriChar8 *category_name, const CriChar8 *aisac_control_name, CriFloat32 *control_value)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.GetNumAttachedAisacsById"/>
		public static unsafe bool GetCurrentAisacControlValueByName(ArgString categoryName, ArgString aisacControlName, out Single controlValue)
		{
			fixed (Single* controlValuePtr = &controlValue)
				return NativeMethods.criAtomExCategory_GetCurrentAisacControlValueByName(categoryName.GetPointer(stackalloc byte[categoryName.BufferSize]), aisacControlName.GetPointer(stackalloc byte[aisacControlName.BufferSize]), controlValuePtr);
		}

		/// <summary>REACT駆動パラメーターの設定 </summary>
		/// <param name="reactName">REACT名 </param>
		/// <param name="reactParameter">REACT駆動パラメーター構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTを駆動させるパラメーターを設定します。
		///  REACTが動作している間はパラメーターを設定することはできません（警告が発生します）。
		///  存在しないREACT名を指定した場合は、エラーコールバックが返ります。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_SetReactParameter(const CriChar8 *react_name, const CriAtomExReactParameter *react_parameter)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public static unsafe void SetReactParameter(ArgString reactName, in CriAtomEx.ReactParameter reactParameter)
		{
			fixed (CriAtomEx.ReactParameter* reactParameterPtr = &reactParameter)
				NativeMethods.criAtomExCategory_SetReactParameter(reactName.GetPointer(stackalloc byte[reactName.BufferSize]), reactParameterPtr);
		}

		/// <summary>REACT駆動パラメーターの取得 </summary>
		/// <param name="reactName">REACT名 </param>
		/// <param name="reactParameter">REACT駆動パラメーター構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTを駆動させるパラメーターの現在値を取得します。
		///  存在しないREACT名を指定した場合は、エラーコールバックが発生しfalseが返ります。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtomExCategory_GetReactParameter(const CriChar8 *react_name, CriAtomExReactParameter *react_parameter)"/>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		public static unsafe bool GetReactParameter(ArgString reactName, out CriAtomEx.ReactParameter reactParameter)
		{
			fixed (CriAtomEx.ReactParameter* reactParameterPtr = &reactParameter)
				return NativeMethods.criAtomExCategory_GetReactParameter(reactName.GetPointer(stackalloc byte[reactName.BufferSize]), reactParameterPtr);
		}

		/// <summary>REACT動作ステータスの取得 </summary>
		/// <param name="reactName">REACT名 </param>
		/// <returns><see cref="CriAtomEx.ReactStatus"/> REACT動作ステータス </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTの動作ステータスを取得します。
		///  存在しないREACT名を指定した場合は、エラーコールバックが発生し <see cref="CriAtomEx.ReactStatus.Error"/> が返ります。
		/// </para>
		/// <nativeinfo declaration="CriAtomExReactStatus criAtomExCategory_GetReactStatus(const CriChar8 *react_name)"/>
		/// </remarks>
		public static CriAtomEx.ReactStatus GetReactStatus(ArgString reactName)
		{
			return (CriAtomEx.ReactStatus)NativeMethods.criAtomExCategory_GetReactStatus(reactName.GetPointer(stackalloc byte[reactName.BufferSize]));
		}

		/// <summary>ID指定でカテゴリに所属する発音中のキュー数を取得する </summary>
		/// <param name="id">カテゴリID </param>
		/// <returns>カテゴリに所属する発音中のキュー数 </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリに所属する発音中のキュー数を取得します。
		///  存在しないカテゴリを指定した場合、負値が返ります。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomExCategory_GetNumCuePlayingCountById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static Int32 GetNumCuePlayingCountById(UInt32 id)
		{
			return NativeMethods.criAtomExCategory_GetNumCuePlayingCountById(id);
		}

		/// <summary>名前指定でカテゴリに所属する発音中のキュー数を取得する </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <returns>カテゴリに所属する発音中のキュー数 </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリに所属する発音中のキュー数を取得します。 存在しないカテゴリを指定した場合、負値が返ります。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomExCategory_GetNumCuePlayingCountByName(const CriChar8 *name)"/>
		/// </remarks>
		public static Int32 GetNumCuePlayingCountByName(ArgString name)
		{
			return NativeMethods.criAtomExCategory_GetNumCuePlayingCountByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ID指定でカテゴリに所属する発音中のキューを停止する </summary>
		/// <param name="id">カテゴリID </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したカテゴリに所属する発音中のキューを停止します。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_StopById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static void StopById(UInt32 id)
		{
			NativeMethods.criAtomExCategory_StopById(id);
		}

		/// <summary>名前指定でカテゴリに所属する発音中のキューを停止する </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したカテゴリに所属する発音中のキューを停止します。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_StopByName(const CriChar8 *name)"/>
		/// </remarks>
		public static void StopByName(ArgString name)
		{
			NativeMethods.criAtomExCategory_StopByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ID指定でカテゴリに所属する発音中のキューを即時停止する </summary>
		/// <param name="id">カテゴリID </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したカテゴリに所属する発音中のキューを即時停止します。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_StopWithoutReleaseTimeById(CriAtomExCategoryId id)"/>
		/// </remarks>
		public static void StopWithoutReleaseTimeById(UInt32 id)
		{
			NativeMethods.criAtomExCategory_StopWithoutReleaseTimeById(id);
		}

		/// <summary>名前指定でカテゴリに所属する発音中のキューを即時停止する </summary>
		/// <param name="name">カテゴリ名 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したカテゴリに所属する発音中のキューを即時停止します。
		/// </para>
		/// <nativeinfo declaration="void criAtomExCategory_StopWithoutReleaseTimeByName(const CriChar8 *name)"/>
		/// </remarks>
		public static void StopWithoutReleaseTimeByName(ArgString name)
		{
			NativeMethods.criAtomExCategory_StopWithoutReleaseTimeByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>最大再生毎カテゴリ参照数</summary>
		/// <seealso cref="CriAtomEx.CueInfo"/>
		public const Int32 MaxCategoriesPerPlayback = (16);

	}
}