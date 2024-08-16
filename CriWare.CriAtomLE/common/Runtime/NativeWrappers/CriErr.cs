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
using System.ComponentModel;

namespace CriWare
{
#pragma warning disable 0465
	/// <summary>CriErr API</summary>
	public static partial class CriErr
	{
		/// <summary>エラーID文字列からエラーメッセージへ変換</summary>
		/// <param name="errid">エラーID文字列</param>
		/// <param name="p1">補足情報1</param>
		/// <param name="p2">補足情報2</param>
		/// <returns></returns>
		/// <remarks>
		/// <para header="説明">エラーID文字列から詳細なエラーメッセージへ変換します。</para>
		/// </remarks>
		public static NativeString ConvertIdToMessage(ArgString errid, UInt32 p1, UInt32 p2)
		{
			return NativeMethods.criErr_ConvertIdToMessage(errid.GetPointer(stackalloc byte[errid.BufferSize]), p1, p2);
		}

		/// <summary>エラーコールバック関数の登録</summary>
		/// <param name="cbf">エラーコールバック関数</param>
		/// <returns></returns>
		/// <remarks>
		/// <para header="説明">
		/// エラーコールバック関数を登録します。
		/// 登録された関数は、CRIミドルウエアライブラリ内でエラーが発生したときに呼び出されます。
		/// </para>
		/// <para header="注意">
		/// 同時に登録できるエラーコールバック関数は１つです。<br/>
		/// 登録後に再度本関数を呼び出した場合は現在の登録を上書きします。<br/>
		/// エラーコールバックの上書きが発生した場合、上書きした側、された側双方に警告が返されます。<br/>
		/// アプリケーションの正常な処理の流れとして、意図的にエラーコールバック関数の差し替えを行うケースがある場合には、<br/>
		/// 一旦「<see cref="CriErr.SetCallback"/>(default);」を実行してコールバックの登録を解除し、<br/>
		/// その後に改めてエラーコールバック関数の登録を行ってください。<br/>
		/// </para>
		/// </remarks>
		public static unsafe void SetCallback(delegate*unmanaged[Cdecl]<NativeString, uint, uint, IntPtr, void> cbf)
		{
			NativeMethods.criErr_SetCallback((IntPtr)cbf);
		}
		static CriErr.CbFunc _callback = null;

		/// <summary>
		/// コールバックオブジェクト
		/// </summary>
		public static CriErr.CbFunc Callback => _callback ?? (_callback = new CriErr.CbFunc(NativeMethods.criErr_SetCallback));

		/// <summary>エラーコールバックオブジェクト型</summary>
		public unsafe class CbFunc : NativeCallbackBase<(NativeString errid, UInt32 p1, UInt32 p2, IntPtr parray)>
		{
#if ENABLE_IL2CPP
			[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
			static void CallbackFunc(NativeString errid, UInt32 p1, UInt32 p2, IntPtr parray) =>
				InvokeCallbackInternal(instanceKey, (errid, p1, p2, parray));

			delegate void NativeDelegate(NativeString errid, UInt32 p1, UInt32 p2, IntPtr parray);

			static IntPtr instanceKey;
			static NativeDelegate callbackDelegate = null;
			internal CbFunc(Action<IntPtr> setFunction) :
				base((ptr, key) => {
					instanceKey = key;
					setFunction(ptr);
				}, Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc))
			{ }
		}
		/// <summary>エラー通知レベルの変更</summary>
		/// <param name="level">エラー通知レベル</param>
		/// <returns></returns>
		/// <remarks>
		/// <para header="説明">エラーコールバックに通知するエラーのレベルを変更します。</para>
		/// </remarks>
		public static void SetErrorNotificationLevel(CriErr.NotificationLevel level)
		{
			NativeMethods.criErr_SetErrorNotificationLevel(level);
		}

		/// <summary>エラー通知レベル</summary>
		public enum NotificationLevel
		{
			/// <summary>全てのエラーを通知</summary>
			All = 0,
			/// <summary>エラーのみ通知（警告は無視）</summary>
			Fatal = 1,
		}
		/// <summary>エラー発生回数の取得</summary>
		/// <param name="level">エラーレベル</param>
		/// <returns></returns>
		/// <remarks>
		/// <para header="説明">エラー発生回数を取得します。</para>
		/// </remarks>
		public static UInt32 GetErrorCount(CriErr.Level level)
		{
			return NativeMethods.criErr_GetErrorCount(level);
		}

		/// <summary>Error level</summary>
		public enum Level
		{
			/// <summary>
			/// エラー
			/// </summary>
			Error = 0,
			/// <summary>
			/// 警告
			/// </summary>
			Warning = 1,
		}
		/// <summary>エラー発生回数のリセット</summary>
		/// <param name="level">エラーレベル</param>
		/// <returns></returns>
		/// <remarks>
		/// <para header="説明">エラー発生回数のカウンタを0に戻します。</para>
		/// </remarks>
		public static void ResetErrorCount(CriErr.Level level)
		{
			NativeMethods.criErr_ResetErrorCount(level);
		}
	}
}