/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using CriWare.InteropHelpers;

namespace CriWare
{
	/// <summary>カテゴリID</summary>
	/// <remarks>
	/// <para header="説明">
	/// カテゴリIDは、ユーザがオーサリングツール上でカテゴリに対して割り当てた一意のIDです。
	/// カテゴリIDをプログラム中で保持する際には、本変数型を用いて値を取り扱う必要があります。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExCategory.SetVolumeById"/>
	/// <seealso cref="CriAtomExCategory.MuteById"/>
	/// <seealso cref="CriAtomExCategory.SoloById"/>
	public partial class CriAtomExCategory
	{
		/// <summary>ネイティブハンドル</summary>

		public UInt32 NativeHandle { get; }

		/// <summary>既存ハンドルからのインスタンス生成</summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExCategory(UInt32 handle) =>
			NativeHandle = handle;
		/// <inheritdoc/>
		public override bool Equals(object obj) =>
			obj is CriAtomExCategory other && NativeHandle.Equals(other.NativeHandle);
		/// <inheritdoc/>
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <inheritdoc/>
		public static bool operator ==(CriAtomExCategory a, CriAtomExCategory b)
		{

			return a.Equals(b);
		}
		/// <inheritdoc/>
		public static bool operator !=(CriAtomExCategory a, CriAtomExCategory b) =>
			!(a == b);

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void SetVolumeById(Single volume)
		{
			NativeMethods.criAtomExCategory_SetVolumeById(NativeHandle, volume);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public Single GetVolumeById()
		{
			return NativeMethods.criAtomExCategory_GetVolumeById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public Single GetTotalVolumeById()
		{
			return NativeMethods.criAtomExCategory_GetTotalVolumeById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void MuteById(NativeBool mute)
		{
			NativeMethods.criAtomExCategory_MuteById(NativeHandle, mute);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public bool IsMutedById()
		{
			return NativeMethods.criAtomExCategory_IsMutedById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void SoloById(NativeBool solo, Single muteVolume)
		{
			NativeMethods.criAtomExCategory_SoloById(NativeHandle, solo, muteVolume);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public bool IsSoloedById()
		{
			return NativeMethods.criAtomExCategory_IsSoloedById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void PauseById(NativeBool sw)
		{
			NativeMethods.criAtomExCategory_PauseById(NativeHandle, sw);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public bool IsPausedById()
		{
			return NativeMethods.criAtomExCategory_IsPausedById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void SetFadeInTimeById(UInt16 ms)
		{
			NativeMethods.criAtomExCategory_SetFadeInTimeById(NativeHandle, ms);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void SetFadeOutTimeById(UInt16 ms)
		{
			NativeMethods.criAtomExCategory_SetFadeOutTimeById(NativeHandle, ms);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void SetAisacControlById(UInt32 controlId, Single controlValue)
		{
			NativeMethods.criAtomExCategory_SetAisacControlById(NativeHandle, controlId, controlValue);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public bool ResetAllAisacControlById()
		{
			return NativeMethods.criAtomExCategory_ResetAllAisacControlById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void AttachAisacById(ArgString globalAisacName)
		{
			NativeMethods.criAtomExCategory_AttachAisacById(NativeHandle, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void DetachAisacById(ArgString globalAisacName)
		{
			NativeMethods.criAtomExCategory_DetachAisacById(NativeHandle, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void DetachAisacAllById()
		{
			NativeMethods.criAtomExCategory_DetachAisacAllById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public Int32 GetNumAttachedAisacsById()
		{
			return NativeMethods.criAtomExCategory_GetNumAttachedAisacsById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public unsafe bool GetAttachedAisacInfoById(Int32 aisacAttachedIndex, out CriAtomEx.AisacInfo aisacInfo)
		{
			fixed (CriAtomEx.AisacInfo* aisacInfoPtr = &aisacInfo)
				return NativeMethods.criAtomExCategory_GetAttachedAisacInfoById(NativeHandle, aisacAttachedIndex, aisacInfoPtr);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public unsafe bool GetCurrentAisacControlValueById(UInt32 aisacControlId, out Single controlValue)
		{
			fixed (Single* controlValuePtr = &controlValue)
				return NativeMethods.criAtomExCategory_GetCurrentAisacControlValueById(NativeHandle, aisacControlId, controlValuePtr);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public Int32 GetNumCuePlayingCountById()
		{
			return NativeMethods.criAtomExCategory_GetNumCuePlayingCountById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void StopById()
		{
			NativeMethods.criAtomExCategory_StopById(NativeHandle);
		}

		/// <exclude/>
		[Obsolete("This API is Obsolete. Use static version of this Method.")]
		public void StopWithoutReleaseTimeById()
		{
			NativeMethods.criAtomExCategory_StopWithoutReleaseTimeById(NativeHandle);
		}
	}
}