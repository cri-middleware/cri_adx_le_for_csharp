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
	/// <summary>CriAtomAsr API</summary>
	public static partial class CriAtomAsr
	{
		/// <summary><see cref="CriAtomAsr.Config"/>へのデフォルトパラメーターをセット</summary>
		/// <param name="pConfig">初期化用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomAsr.Initialize"/> 関数に設定するコンフィグ構造体（ <see cref="CriAtomAsr.Config"/> ）に、
		/// デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAsr.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomAsr.Config pConfig)
		{
			fixed (CriAtomAsr.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomAsr_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>ASR初期化用ワーク領域サイズの計算</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASR（Atom Sound Renderer）の初期化に必要なワーク領域のサイズを取得します。
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomAsr.Initialize"/> 関数でASRの初期化を行う場合、
		/// 本関数で計算したサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ASRの初期化に必要なワークメモリのサイズは、ASR初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomAsr.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomAsr.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtomAsr.Initialize"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomAsr.Config config)
		{
			fixed (CriAtomAsr.Config* configPtr = &config)
				return NativeMethods.criAtomAsr_CalculateWorkSize(configPtr);
		}

		/// <summary>ASR初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomAsr.SetDefaultConfig"/> メソッドで
		/// 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomAsr.Initialize"/> 関数
		/// に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomAsr.SetDefaultConfig"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomAsr.Initialize"/>
		/// <seealso cref="CriAtomAsr.SetDefaultConfig"/>
		public unsafe partial struct Config
		{
			/// <summary>サーバー処理の実行頻度</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// サーバー処理を実行する頻度を指定します。
			/// </para>
			/// <para>
			/// 注意:
			/// Atomライブラリ初期化時に指定した値（ <see cref="CriAtom.Config"/> 構造体の
			/// server_frequency ）と、同じ値をセットする必要があります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.Config"/>
			public Single serverFrequency;

			/// <summary>バス数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRが作成するバスの数を指定します。
			/// バスはサウンドのミックスや、エフェクトの管理等を行います。
			/// </para>
			/// </remarks>
			public Int32 numBuses;

			/// <summary>出力チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRの出力チャンネル数を指定します。
			/// パン3Dもしくは3Dポジショニング機能を使用する場合は6ch以上を指定します。
			/// </para>
			/// </remarks>
			public Int32 outputChannels;

			/// <summary>ミキサーのスピーカーマッピング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRラックのスピーカーマッピングを指定します。
			/// </para>
			/// </remarks>
			public CriAtom.SpeakerMapping speakerMapping;

			/// <summary>出力サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 出力および処理過程のサンプリングレートを指定します。
			/// 通常、ターゲット機のサウンドデバイスのサンプリングレートを指定します。
			/// </para>
			/// <para>
			/// 備考:
			/// 低くすると処理負荷を下げることができますが音質が悪くなります。
			/// </para>
			/// </remarks>
			public Int32 outputSamplingRate;

			/// <summary>サウンドレンダラタイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ASRの出力先サウンドレンダラの種別を指定します。
			/// sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、
			/// 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			/// </para>
			/// <para>
			/// 注意:
			/// <see cref="CriAtom.SoundRendererType.Asr"/>および<see cref="CriAtom.SoundRendererDefault"/>は指定しないでください。
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。
			/// nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでASRラックを作成します。
			/// パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。
			/// パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。
			/// </para>
			/// </remarks>
			public IntPtr context;

			/// <summary>ASRラックの最大数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 作成可能なASRラックの最大個数です。
			/// </para>
			/// </remarks>
			public Int32 maxRacks;

			/// <summary>Ambisonicsのオーダータイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomライブラリがAmbisonicsの再生を行う際、どのオーダータイプを使用するか設定します。
			/// </para>
			/// <para>
			/// 備考:
			/// Ambisonicsの再生に非対応のプラットフォームでは、この値は無視されます。
			/// また、 <see cref="CriAtom.AmbisonicsOrderType.None"/> を指定した場合、Ambisonicsの再生を行いません。
			/// </para>
			/// </remarks>
			public CriAtom.AmbisonicsOrderType ambisonicsOrderType;

		}
		/// <summary>ASRの初期化</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASR（Atom Sound Renderer）の初期化を行います。
		/// 本関数を実行することでASRが起動され、レンダリング結果の出力を開始します。
		/// </para>
		/// <para>
		/// 備考:
		/// ASRの初期化に必要なワークメモリのサイズは、ASR初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomAsr.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomAsr.SetDefaultConfig"/> 適用時と同じパラメーター）で初期化処理を行います。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// 本関数を実行後、必ず対になる <see cref="CriAtomAsr.Finalize"/> 関数を実行してください。
		/// また、 <see cref="CriAtomAsr.Finalize"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtomAsr.Finalize"/>
		public static unsafe void Initialize(in CriAtomAsr.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomAsr.Config* configPtr = &config)
				NativeMethods.criAtomAsr_Initialize(configPtr, work, workSize);
		}

		/// <summary>ASRの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ASR（Atom Sound Renderer）の終了処理を行います。
		/// 本関数を実行することで、レンダリング結果の出力が停止されます。
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// ASR初期化時に確保されたメモリ領域が解放されます。
		/// （ASR初期化時にワーク領域を渡した場合、本関数実行後であれば
		/// ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtomAsr.Initialize"/>
		public static void Finalize()
		{
			NativeMethods.criAtomAsr_Finalize();
		}

	}
}