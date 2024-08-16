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
	/// <summary>Atomプレーヤーオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// CriAtomPlayerHn は、音声再生用に作られたプレーヤーを操作するためのオブジェクトです。
	/// ::criAtomPlayer_CreateAdxPlayer 関数等で音声再生用のプレーヤーを作成すると、
	/// 関数はプレーヤー操作用に、この"Atomプレーヤーオブジェクト"を返します。
	/// Atomプレーヤーとは、コーデックに依存しない再生制御のためのインターフェースを提供する、
	/// 抽象化されたプレーヤーオブジェクトです。
	/// Atomプレーヤーの作成方法は再生する音声コーデックにより異なりますが、
	/// 作成されたプレーヤーの制御については、Atomプレーヤー用のAPIが共通で利用可能です。
	/// データのセットや再生の開始、ステータスの取得等、プレーヤーに対して行う操作は、
	/// 全てAtomプレーヤーオブジェクトを介して実行されます。
	/// </para>
	/// </remarks>
	/// <seealso cref="criAtomPlayer_CreateAdxPlayer"/>
	public partial class CriAtomPlayer : IDisposable
	{
		/// <summary>MP3プレーヤ作成用ワーク領域サイズの計算</summary>
		/// <param name="config">MP3プレーヤ作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// MP3再生用プレーヤを作成するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤの作成に必要なワークメモリのサイズは、プレーヤ作成用コンフィグ
		/// 構造体（ <see cref="CriAtom.Mp3PlayerConfigIOS"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ ::criAtomPlayer_SetDefaultConfigForMp3Player_IOS 適用時と同じパラメータ）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.Mp3PlayerConfigIOS"/>
		/// <seealso cref="CriAtomPlayer.CreateMp3PlayerIOS"/>
		public static unsafe Int32 CalculateWorkSizeForMp3PlayerIOS(in CriAtom.Mp3PlayerConfigIOS config)
		{
			fixed (CriAtom.Mp3PlayerConfigIOS* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForMp3Player_IOS(configPtr);
		}

		/// <summary>MP3プレーヤの作成</summary>
		/// <param name="config">MP3プレーヤ作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>Atomプレーヤオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// MP3が再生可能なプレーヤを作成します。
		/// 本関数は完了復帰型の関数です。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// MP3プレーヤの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.Mp3PlayerConfigIOS"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForMp3PlayerIOS"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		public static unsafe CriAtomPlayer CreateMp3PlayerIOS(in CriAtom.Mp3PlayerConfigIOS config, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			fixed (CriAtom.Mp3PlayerConfigIOS* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateMp3Player_IOS(configPtr, work, workSize)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

	}
}