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
	/// <summary>CriAtomDsp API</summary>
	public static partial class CriAtomDsp
	{
		/// <summary>セント値からDSPパラメーターへの変換</summary>
		/// <param name="cent">セント値</param>
		/// <returns>DSPパラメーター値</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// -1200～1200の範囲を0.0f～1.0fの範囲に正規化します。
		/// </para>
		/// </remarks>
		public static Single ConvertParameterFromCent(Single cent)
		{
			return NativeMethods.criAtomDsp_ConvertParameterFromCent(cent);
		}

		/// <summary>ピッチシフタアタッチ用パラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ピッチシフタDSPをアタッチする際に指定するパラメーターです。
		/// <see cref="CriAtomExVoicePool.AttachDspPitchShifter"/> 関数に
		/// <see cref="CriAtomEx.DspPitchShifterConfig"/> 構造体のメンバとして指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspPitchShifter"/>
		public unsafe partial struct PitchShifterConfig
		{
			/// <summary>ピッチシフトモード</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ピッチシフトの処理方法（アルゴリズム）を指定します。
			/// 音声によって設定を変更すると音質が向上することがあります。
			/// 下記は指定可能な値と対応するモード名です。
			/// 0: Music
			/// 1: Vocal
			/// 2: SoundEffect
			/// 3: Speech
			/// </para>
			/// </remarks>
			public Int32 mode;

			/// <summary>ウインドウサイズ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ピッチシフトの処理単位です。
			/// 音声によって設定を変更すると音質が向上することがあります。
			/// 128,256,512,1024,2048のいずれかが設定可能です。
			/// </para>
			/// </remarks>
			public Int32 windowSize;

			/// <summary>オーバーラップ回数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ピッチシフトの結果のオーバーラップ回数です。
			/// 多く設定するほど品質が向上しますが、処理負荷とのトレードオフです。
			/// 1,2,4,8のいずれかが設定可能です。
			/// </para>
			/// </remarks>
			public Int32 overlapTimes;

		}
		/// <summary>タイムストレッチ用パラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// タイムストレッチDSPをアタッチする際に指定するパラメーターです。
		/// <see cref="CriAtomExVoicePool.AttachDspTimeStretch"/> 関数に
		/// <see cref="CriAtomEx.DspTimeStretchConfig"/> 構造体のメンバとして指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspTimeStretch"/>
		public unsafe partial struct TimeStretchConfig
		{
			/// <exclude/>
			public Int32 reserved;

		}
		/// <summary>インサーションDSPのAFX用パラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AFX形式のインサーションDSPをアタッチする際に指定するパラメーターです。
		/// <see cref="CriAtomExVoicePool.AttachDspAfx"/> 関数に
		/// <see cref="CriAtomEx.DspAfxConfig"/> 構造体のメンバとして指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspAfx"/>
		public unsafe partial struct AfxConfig
		{
			/// <summary>コンフィグパラメータ数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// コンフィグパラメータ数を指定します。
			/// </para>
			/// </remarks>
			public UInt32 numConfigParameters;

			/// <summary>コンフィグパラメータ配列</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// エフェクトインスタンス初期化時に使用するパラメータの配列を設定します。
			/// 本パラメータは、インスタンス生成時に必要となる追加のパラメータ情報であり、動作時パラメータとは異なります。
			/// 例えば、ディレイエフェクトのコンフィグパラメータである最大遅延時間は、
			/// インスタンス生成時に用意する遅延バッファサイズを確定させる為に用意されており、動作時は使用しません。
			/// </para>
			/// </remarks>
			public NativeReference<Single> configParameters;

			/// <summary>動作時パラメータ数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 動作時パラメータ数を指定します。
			/// </para>
			/// </remarks>
			public UInt32 numParameters;

			/// <summary>デフォルトパラメータ配列</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// エフェクトの動作時パラメータのデフォルト値配列を設定します。
			/// ボイスプールにエフェクトをアタッチした時や、エフェクトのパラメーターを初期化した場合、ここで指定したパラメータになります。
			/// </para>
			/// </remarks>
			public NativeReference<Single> defaultParameters;

			/// <summary>Afx形式エフェクトのインタフェース</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アタッチするAfx形式のエフェクトインタフェースを設定します。
			/// </para>
			/// </remarks>
			public IntPtr afxIf;

		}
	}
}