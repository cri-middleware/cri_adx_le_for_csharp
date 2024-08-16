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
	/// <summary>トゥイーンオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomExTween"/> は、トゥイーンを操作するためのオブジェクトです。
	/// トゥイーンとは、簡単な手順でパラメーターの時間変化を行うためのモジュールです。
	/// <see cref="CriAtomExTween.CriAtomExTween"/> 関数でトゥイーンを作成すると、
	/// 本関数はトゥイーン操作用に、この"トゥイーンオブジェクト"を返します。
	/// パラメーターの時間変化の開始等、トゥイーンに対して行う操作は、
	/// 全てトゥイーンオブジェクトを介して実行されます。
	/// また、AtomExプレーヤーにトゥイーンを関連づける際にも使用します。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExTween.CriAtomExTween"/>
	/// <seealso cref="CriAtomExPlayer.AttachTween"/>
	public partial class CriAtomExTween : IDisposable
	{
		/// <summary>トゥイーン作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExTween.CriAtomExTween"/> 関数に設定するコンフィグ構造体（ <see cref="CriAtomExTween.Config"/> ）に、
		/// デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExTween.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomExTween.Config pConfig)
		{
			fixed (CriAtomExTween.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExTween_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>トゥイーンの作成に必要なワーク領域サイズの計算</summary>
		public static unsafe Int32 CalculateWorkSize(ref CriAtomExTween.Config config)
		{
			fixed (CriAtomExTween.Config* configPtr = &config)
				return NativeMethods.criAtomExTween_CalculateWorkSize(configPtr);
		}

		/// <summary>トゥイーン作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥイーンを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomExTween.CriAtomExTween"/> 関数の引数に指定します。
		/// 本構造体を使用する際には <see cref="CriAtomExTween.SetDefaultConfig"/> メソッドを使用し、
		/// 構造体の初期化を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExTween.SetDefaultConfig"/>
		/// <seealso cref="CriAtomExTween.CalculateWorkSize"/>
		/// <seealso cref="CriAtomExTween.CriAtomExTween"/>
		public unsafe partial struct Config
		{
			/// <summary>ID指定共用体</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パラメータータイプに従い、パラメーターIDまたはAISACコントロールIDを指定します。
			/// </para>
			/// </remarks>
			public CriAtomExTween.ConfigParameterIdTag id;

			/// <summary>パラメータータイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パラメータータイプを指定します。
			/// </para>
			/// </remarks>
			public CriAtomExTween.ParameterType parameterType;

		}
		/// <summary>ID指定共用体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラメータータイプに従い、パラメーターIDまたはAISACコントロールIDを指定します。
		/// </para>
		/// </remarks>
		[StructLayout(LayoutKind.Explicit)]
		public unsafe partial struct ConfigParameterIdTag
		{
			/// <summary>パラメーターID</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パラメータータイプに<see cref="CriAtomExTween.ParameterType.Basic"/>を指定する場合、このメンバでパラメーターIDを指定します。
			/// </para>
			/// </remarks>
			[FieldOffset(0)] public CriAtomEx.ParameterId parameterId;

			/// <summary>AISACコントロールID</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パラメータータイプに<see cref="CriAtomExTween.ParameterType.Aisac"/>を指定する場合、このメンバでAISACコントロールIDを指定します。
			/// </para>
			/// </remarks>
			[FieldOffset(0)] public UInt32 aisacControlId;

		}
		/// <summary>Tweenのパラメータータイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Tweenで操作するパラメーターのタイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExTween.Config"/>
		public enum ParameterType
		{
			/// <summary>基本パラメーター</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ボリューム、ピッチ等、<see cref="CriAtomEx.ParameterId"/>で指定するパラメーターを操作する際に指定します。
			/// </para>
			/// </remarks>
			Basic = 0,
			/// <summary>AISACコントロール値</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AISACコントロール値を操作する際に指定します。
			/// </para>
			/// </remarks>
			Aisac = 1,
		}
		/// <summary>トゥイーンの作成</summary>
		/// <param name="config">トゥイーン作成用コンフィグ構造体へのポインタ</param>
		/// <param name="work">トゥイーン作成用ワーク領域へのポインタ</param>
		/// <param name="workSize">トゥイーン作成用ワークサイズ</param>
		/// <returns>トゥイーンオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥイーン作成用コンフィグに基づいて、トゥイーンを作成します。
		/// 作成に成功すると、トゥイーンオブジェクトを返します。
		/// トゥイーンを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExTween.CalculateWorkSize"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// 作成したトゥイーンは、<see cref="CriAtomExPlayer.AttachTween"/> 関数にてAtomExプレーヤーにアタッチすることで効果を発揮します。
		/// </para>
		/// <para>
		/// 備考:
		/// トゥイーンの保持するパラメーターの初期値は、コンフィグ構造体でパラメータータイプに<see cref="CriAtomExTween.ParameterType.Basic"/>を指定した場合は各パラメーターのデフォルト値、またはパラメータータイプに<see cref="CriAtomExTween.ParameterType.Aisac"/>を指定した場合は0.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// TweenオブジェクトをアタッチしたAtomExプレーヤーで再生している音声がある場合、
		/// 本関数を実行する前に、それらの音声を停止するか、そのAtomExプレーヤーを破棄してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExTween.CalculateWorkSize"/>
		/// <seealso cref="CriAtomExTween.Dispose"/>
		/// <seealso cref="CriAtomExPlayer.AttachTween"/>
		public unsafe CriAtomExTween(in CriAtomExTween.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExTween.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomExTween_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomExTween(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomExTween.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomExTween_Create(configPtr, work, workSize);
		}

		/// <summary>トゥイーンの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥイーンを破棄します。
		/// 本関数を実行した時点で、トゥイーン作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定したトゥイーンオブジェクトも無効になります。
		/// トゥイーンをアタッチしたAtomExプレーヤーで再生している音声がある場合、
		/// 本関数を実行する前に、それらの音声を停止するか、そのAtomExプレーヤーを破棄してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExTween.CriAtomExTween"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomExTween_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomExTween() => Dispose();
#pragma warning restore 1591

		/// <summary>現在値の取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥイーンが保持しているパラメーターの現在値を取得します。
		/// </para>
		/// </remarks>
		public Single GetValue()
		{
			return NativeMethods.criAtomExTween_GetValue(NativeHandle);
		}

		/// <summary>現在値から指定値に変化</summary>
		/// <param name="timeMs">変化に要する時間（ミリ秒単位）</param>
		/// <param name="value">変化後の最終値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// time_msで指定した時間をかけて、本関数呼び出し時にトゥイーンが保持している現在値から、valueで指定した値へと変化します。
		/// 変化カーブはリニア（線形）です。
		/// </para>
		/// </remarks>
		public void MoveTo(UInt16 timeMs, Single value)
		{
			NativeMethods.criAtomExTween_MoveTo(NativeHandle, timeMs, value);
		}

		/// <summary>指定値から現在値に変化</summary>
		/// <param name="timeMs">変化に要する時間（ミリ秒単位）</param>
		/// <param name="value">変化前の開始値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// time_msで指定した時間をかけて、valueで指定した値から、本関数呼び出し時にトゥイーンが保持している現在値へと変化します。
		/// 変化カーブはリニア（線形）です。
		/// </para>
		/// </remarks>
		public void MoveFrom(UInt16 timeMs, Single value)
		{
			NativeMethods.criAtomExTween_MoveFrom(NativeHandle, timeMs, value);
		}

		/// <summary>トゥイーンの停止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥイーンによるパラメーターの時間変化を停止します。
		/// </para>
		/// </remarks>
		public void Stop()
		{
			NativeMethods.criAtomExTween_Stop(NativeHandle);
		}

		/// <summary>トゥイーンのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥイーンの保持しているパラメーターの現在値をリセットし、初期値に戻します。
		/// トゥイーンの保持するパラメーターの初期値は、コンフィグ構造体でパラメータータイプに<see cref="CriAtomExTween.ParameterType.Basic"/>を指定した場合は各パラメーターのデフォルト値、またはパラメータータイプに<see cref="CriAtomExTween.ParameterType.Aisac"/>を指定した場合は0.0fです。
		/// </para>
		/// <para>
		/// 備考:
		/// トゥイーンによる時間変化が動作していた場合、動作を停止します。
		/// </para>
		/// </remarks>
		public void Reset()
		{
			NativeMethods.criAtomExTween_Reset(NativeHandle);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExTween(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExTween other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExTween a, CriAtomExTween b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExTween a, CriAtomExTween b) =>
			!(a == b);

	}
}