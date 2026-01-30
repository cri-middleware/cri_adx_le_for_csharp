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
	/// <summary>CriAtom API</summary>
	public static partial class CriAtom
	{
		/// <summary></summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Vibrationが再生可能なプレーヤを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomPlayer.CreateWebAudioPlayer"/> 関数の引数に指定します。
		///  作成されるプレーヤは、オブジェクト作成時に本構造体で指定された設定に応じて、 内部リソースを必要なだけ確保します。
		///  プレーヤが必要とするワーク領域のサイズは、本構造体で指定されたパラメータに応じて変化します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 criAtomPlayer_SetDefaultConfigForWebAudioPlayer メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// <para>WebAudioプレーヤ作成用コンフィグ構造体 </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateWebAudioPlayer"/>
		[Serializable]
		public unsafe partial struct WebAudioPlayerConfig
		{
			public Int32 maxChannels;

			public NativeBool streamingFlag;

		}
	}
}