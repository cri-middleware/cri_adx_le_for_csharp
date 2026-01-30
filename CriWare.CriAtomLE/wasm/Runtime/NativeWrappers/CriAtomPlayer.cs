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
	/// <summary>Atomプレーヤーハンドル </summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomPlayer"/> は、音声再生用に作られたプレーヤーを操作するためのオブジェクトです。
	/// <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数等で音声再生用のプレーヤーを作成すると、 関数はプレーヤー操作用に、この"Atomプレーヤーオブジェクト"を返します。 
	///  Atomプレーヤーとは、コーデックに依存しない再生制御のためのインターフェースを提供する、 抽象化されたプレーヤーオブジェクトです。
	///  Atomプレーヤーの作成方法は再生する音声コーデックにより異なりますが、 作成されたプレーヤーの制御については、Atomプレーヤー用のAPIが共通で利用可能です。 
	///  データのセットや再生の開始、ステータスの取得等、プレーヤーに対して行う操作は、 全てAtomプレーヤーオブジェクトを介して実行されます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
	public partial class CriAtomPlayer : IDisposable
	{
		/// <summary>WebAudioプレーヤ作成用ワーク領域サイズの計算 </summary>
		/// <param name="config">WebAudioプレーヤ作成用コンフィグ構造体 </param>
		/// <returns>CriSint32 ワーク領域サイズ </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WebAudio再生用プレーヤを作成するために必要な、ワーク領域のサイズを取得します。
		///  ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		///  ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤの作成に必要なワークメモリのサイズは、プレーヤ作成用コンフィグ 構造体（ <see cref="CriAtom.WebAudioPlayerConfig"/> ）の内容によって変化します。
		///  引数にnullを指定した場合、デフォルト設定 （ criAtomPlayer_SetDefaultConfigForWebAudioPlayer 適用時と同じパラメータ）で ワーク領域サイズを計算します。 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ criAtom_Initialize 関数実行時） に指定したパラメータによって変化します。
		///  そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomPlayer_CalculateWorkSizeForWebAudioPlayer(const CriAtomWebAudioPlayerConfig *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.WebAudioPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateWebAudioPlayer"/>
		public static unsafe Int32 CalculateWorkSizeForWebAudioPlayer(in CriAtom.WebAudioPlayerConfig config)
		{
			fixed (CriAtom.WebAudioPlayerConfig* configPtr = &config)
				return NativeMethods.criAtomPlayer_CalculateWorkSizeForWebAudioPlayer(configPtr);
		}

		/// <summary>WebAudioプレーヤの作成 </summary>
		/// <param name="config">WebAudioプレーヤ作成用コンフィグ構造体 </param>
		/// <returns><see cref="CriAtomPlayer"/> Atomプレーヤオブジェクト </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WebAudioが再生可能なプレーヤを作成します。
		///  本関数で作成されたAtomプレーヤには、WebAudioデータのデコード機能が付加されています。
		///  作成されたプレーヤで再生できる音声のフォーマットは、第一引数（config）に指定した パラメータによって決まります。
		///  例えば、configのmax_sampling_rateに24000を設定した場合、作成されたプレーヤでは 24kHzを超えるサンプリングレートの音声データは再生できなくなります。
		///  configにnullを指定した場合、デフォルト設定（ criAtomPlayer_SetDefaultConfigForWebAudioPlayer 適用時と同じパラメータ）でプレーヤを作成します。 
		///  プレーヤを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		///  各方式の詳細については、別途 <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数の説明をご参照ください。
		/// <see cref="CriAtomPlayer.CreateWebAudioPlayer"/> 関数を実行すると、Atomプレーヤが作成され、 プレーヤを制御するためのオブジェクト（ <see cref="CriAtomPlayer"/> ）が返されます。
		///  データやデコーダのセット、再生の開始、ステータスの取得等、Atomプレーヤに対して 行う操作は、全てオブジェクトに対して行います。
		///  作成されたAtomプレーヤオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// <list type="number">
		/// <item><description><see cref="CriAtomPlayer.SetData"/> 関数を使用して、Atomプレーヤに再生するデータをセットする。
		///  （ファイル再生時は、 <see cref="CriAtomPlayer.SetFile"/> 関数または <see cref="CriAtomPlayer.SetContentId"/> 関数を使用する。）
		/// </description></item>
		/// <item><description><see cref="CriAtomPlayer.Start"/> 関数で再生を開始する。
		/// </description></item>
		/// </list>
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		///  ストリーム再生用のAtomプレーヤは、内部的にローダ（ <see cref="CriFsLoader"/> ）を確保します。
		///  ストリーム再生用のAtomプレーヤを作成する場合、プレーヤオブジェクト数分のローダが確保 できる設定でAtomライブラリ（またはCRI File Systemライブラリ）を初期化する 必要があります。
		///  本関数は完了復帰型の関数です。
		///  WebAudioプレーヤの作成にかかる時間は、プラットフォームによって異なります。
		///  ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		///  WebAudioプレーヤの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる タイミングで行うようお願いいたします。
		/// </para>
		/// <nativeinfo declaration="CriAtomPlayerHn criAtomPlayer_CreateWebAudioPlayer(const CriAtomWebAudioPlayerConfig *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.WebAudioPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForWebAudioPlayer"/>
		/// <seealso cref="CriAtomPlayer"/>
		/// <seealso cref="CriAtomPlayer.Dispose"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetFile"/>
		/// <seealso cref="CriAtomPlayer.SetContentId"/>
		/// <seealso cref="CriAtomPlayer.Start"/>
		/// <seealso cref="CriAtomPlayer.CreateWebAudioPlayer"/>
		public static unsafe CriAtomPlayer CreateWebAudioPlayer(in CriAtom.WebAudioPlayerConfig config)
		{
			IntPtr handle;
			fixed (CriAtom.WebAudioPlayerConfig* configPtr = &config)
				return ((handle = NativeMethods.criAtomPlayer_CreateWebAudioPlayer(configPtr, default, default)) == IntPtr.Zero) ? default : new CriAtomPlayer(handle);
		}

	}
}