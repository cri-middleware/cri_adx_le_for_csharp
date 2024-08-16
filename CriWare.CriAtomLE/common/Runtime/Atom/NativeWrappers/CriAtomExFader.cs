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
	/// <summary>CriAtomExFader API</summary>
	public static partial class CriAtomExFader
	{
		/// <summary>フェーダーアタッチ用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.AttachFader"/> 関数の引数に指定する、フェーダーアタッチ用のコンフィグ構造体です。
		/// </para>
		/// <para>
		/// 注意:
		/// 現状指定可能なパラメーターはありませんが、将来パラメーターが追加される可能性があるため、
		/// 本構造体を使用する際には <see cref="CriAtomExFader.SetDefaultConfig"/> メソッドを使用し、
		/// 構造体の初期化を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExFader.SetDefaultConfig"/>
		/// <seealso cref="CriAtomExPlayer.CalculateWorkSizeForFader"/>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		public unsafe partial struct Config
		{
			/// <summary>予約値（0を指定してください）</summary>
			public Int32 reserved;

		}
		/// <summary>フェーダーアタッチ用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">フェーダーアタッチ用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.AttachFader"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExFader.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExFader.Config"/>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		public static unsafe void SetDefaultConfig(out CriAtomExFader.Config pConfig)
		{
			fixed (CriAtomExFader.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExFader_SetDefaultConfig_(pConfigPtr);
		}

	}
}