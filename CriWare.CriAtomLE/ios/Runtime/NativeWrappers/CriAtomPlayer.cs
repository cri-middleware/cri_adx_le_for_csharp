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

	public partial class CriAtomPlayer : IDisposable
	{
		/// <summary>MP3プレーヤ作成用ワーク領域サイズの計算 </summary>
		/// <param name="config">MP3プレーヤ作成用コンフィグ構造体 </param>
		/// <returns>CriSint32 ワーク領域サイズ </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// MP3再生用プレーヤを作成するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤの作成に必要なワークメモリのサイズは、プレーヤ作成用コンフィグ 構造体（ <see cref="CriAtom.Mp3PlayerConfigIOS"/> ）の内容によって変化します。
		///  引数にnullを指定した場合、デフォルト設定 （ criAtomPlayer_SetDefaultConfigForMp3Player_IOS 適用時と同じパラメータ）で ワーク領域サイズを計算します。 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 CRIAPI criAtomPlayer_CalculateWorkSizeForMp3Player_IOS(const CriAtomMp3PlayerConfig_IOS *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Mp3PlayerConfigIOS"/>
		/// <seealso cref="CriAtomPlayer.CreateMp3PlayerIOS"/>
		public static unsafe Int32 CalculateWorkSizeForMp3PlayerIOS(in CriAtom.Mp3PlayerConfigIOS config)
		{
			fixed (CriAtom.Mp3PlayerConfigIOS* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForMp3Player_IOS(configPtr);
		}

		/// <summary>MP3プレーヤの作成 </summary>
		/// <param name="config">MP3プレーヤ作成用コンフィグ構造体 </param>
		/// <returns><see cref="CriAtomPlayer"/> Atomプレーヤオブジェクト </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// MP3が再生可能なプレーヤを作成します。
		///  本関数は完了復帰型の関数です。
		///  ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		///  MP3プレーヤの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる タイミングで行うようお願いいたします。
		/// </para>
		/// <nativeinfo declaration="CriAtomPlayerHn CRIAPI criAtomPlayer_CreateMp3Player_IOS(const CriAtomMp3PlayerConfig_IOS *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Mp3PlayerConfigIOS"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForMp3PlayerIOS"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		public static unsafe CriAtomPlayer CreateMp3PlayerIOS(in CriAtom.Mp3PlayerConfigIOS config)
		{
			IntPtr handle;
			fixed (CriAtom.Mp3PlayerConfigIOS* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateMp3Player_IOS(configPtr, default, default)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

	}
}