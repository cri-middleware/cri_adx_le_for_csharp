/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Collections.Generic;

namespace CriWare.InteropHelpers
{
	/// <summary>コールバックオブジェクト基底クラス</summary>
	/// <remarks>
	/// CRIWAREが呼び出すコールバックを扱うオブジェクトの基底クラスです。
	/// 本クラスの<see cref="Event"/>をい対してイベントリスナーを追加できます。
	/// </remarks>
	public abstract class NativeCallbackBase<TArgs> : CriWare.Interfaces.ICallback<TArgs>
		where TArgs : unmanaged
	{
#pragma warning disable 1591
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		static protected Dictionary<nint, NativeCallbackBase<TArgs>> instances = new Dictionary<nint, NativeCallbackBase<TArgs>>();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		protected static void InvokeCallbackInternal(IntPtr obj, TArgs args) =>
			instances[obj].Event?.Invoke(args);
#pragma warning restore 1591
		/// <inheritdoc/>
		public event Action<TArgs> Event;

		Action<IntPtr, IntPtr> setFunction;
		/// <summary>
		/// ネイティブコールバックオブジェクトのコンストラクタ
		/// </summary>
		protected NativeCallbackBase(Action<IntPtr, IntPtr> setFunction, IntPtr nativeCallback)
		{
			this.setFunction = setFunction;
			nint key = Guid.NewGuid().GetHashCode();
			instances.Add(key, this);
			setFunction(nativeCallback, key);
		}

		/// <summary>
		/// ネイティブコールバックオブジェクトのデストラクタ
		/// </summary>
		~NativeCallbackBase() => setFunction(IntPtr.Zero, IntPtr.Zero);
	}

	/// <inheritdoc cref="NativeCallbackBase{TArgs}"/>
	public abstract class NativeCallbackBase<TArgs, TReturn> : CriWare.Interfaces.ICallback<TArgs, TReturn>
		where TArgs : unmanaged
		where TReturn : unmanaged
	{
#pragma warning disable 1591
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		static protected Dictionary<nint, NativeCallbackBase<TArgs, TReturn>> instances = new Dictionary<nint, NativeCallbackBase<TArgs, TReturn>>();
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		protected static TReturn InvokeCallbackInternal(IntPtr obj, TArgs args) =>
			instances[obj].Event?.Invoke(args) ?? default;
#pragma warning restore 1591
		/// <inheritdoc/>
		public event Func<TArgs, TReturn> Event;

		Action<IntPtr, IntPtr> setFunction;
		/// <inheritdoc cref="NativeCallbackBase{TArgs}.NativeCallbackBase(Action{IntPtr, IntPtr}, IntPtr)"/>
		protected NativeCallbackBase(Action<IntPtr, IntPtr> setFunction, IntPtr nativeCallback)
		{
			this.setFunction = setFunction;
			nint key = Guid.NewGuid().GetHashCode();
			instances.Add(key, this);
			setFunction(nativeCallback, key);
		}
		/// <summary>
		/// ネイティブコールバックオブジェクトのデストラクタ
		/// </summary>
		~NativeCallbackBase() => setFunction(IntPtr.Zero, IntPtr.Zero);
	}
}
