/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Collections.Generic;

namespace CriWare.Interfaces
{
	/// <summary>
	/// Update可能なオブジェクトのインターフェイス
	/// </summary>
	/// <remarks>
	/// CRIWAREが提供するオブジェクトの一部は毎フレーム明示的にアップデートを呼び出す必要があります。
	/// 毎フレームのUpdate呼び出しが必要なクラスインスタンスは本インターフェイスを介して制御できます。
	/// </remarks>
	public interface IUpdatable
	{
		/// <summary>
		/// フレーム毎の更新処理
		/// </summary>
		public void Update();
	}

	/// <summary>
	/// コールバックインターフェイス
	/// </summary>
	/// <typeparam name="TArgs">コールバック引数型</typeparam>
	/// <remarks>
	/// CRIWAREが呼び出すコールバックイベントを取り扱うためのインターフェイスです。
	/// 本インターフェイスに対する拡張メソッドを用意することで、各イベントに対して追加のハンドリング処理などを実装できます。
	/// 特に、ゲームエンジンやUIフレームワークを利用する場合はスレッド移譲などのハンドリングを行ってください。
	/// </remarks>
	public interface ICallback<TArgs>
	{
		/// <summary>
		/// コールバックイベント
		/// </summary>
		public event Action<TArgs> Event;
	}

	/// <inheritdoc cref="ICallback{TArgs}" />
	/// <typeparam name="TArgs">コールバック引数型</typeparam>
	/// <typeparam name="TReturn">コールバックが返却する型</typeparam>
	public interface ICallback<TArgs, TReturn>
	{
		/// <inheritdoc cref="ICallback{TArgs}.Event"/>
		public event Func<TArgs, TReturn> Event;
	}

	/// <exclude/>
	public interface ILibrary
	{
		/// <exclude/>
		Type[] DependentLibraries { get; }
		/// <exclude/>
		void InitializeLibrary();
		/// <exclude/>
		void FinalizeLibrary();
		/// <exclude/>
		bool IsInitialized { get; }
	}
}
