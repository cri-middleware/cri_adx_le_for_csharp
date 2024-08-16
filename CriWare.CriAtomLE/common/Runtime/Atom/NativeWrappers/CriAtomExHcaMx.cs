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
	/// <summary>CriAtomExHcaMx API</summary>
	public static partial class CriAtomExHcaMx
	{
		/// <summary>HCA-MX初期化コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExHcaMx.Initialize"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExHcaMx.Config"/> ）に、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExHcaMx.Initialize"/>
		/// <seealso cref="CriAtomExHcaMx.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomExHcaMx.Config pConfig)
		{
			fixed (CriAtomExHcaMx.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExHcaMx_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomExHcaMx.SetDefaultConfig"/> メソッドで
		/// 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomExHcaMx.Initialize"/> 関数
		/// に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExHcaMx.SetDefaultConfig"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExHcaMx.Initialize"/>
		/// <seealso cref="CriAtomExHcaMx.SetDefaultConfig"/>
		[System.Serializable]
		[System.Xml.Serialization.XmlType(Namespace = "CriAtomExHcaMx")]
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
			/// Atomライブラリ初期化時に指定した値（ <see cref="CriAtomEx.Config"/> 構造体の
			/// server_frequency ）と、同じ値をセットする必要があります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx.Config"/>
			public Single serverFrequency;

			/// <summary>ミキサ数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// HCA-MXデコード結果を送信するミキサの数を指定します。
			/// ミキサを複数作成することで、
			/// ミキサごとに異なるDSPバスのDSP FXを適用することが可能になります。
			/// </para>
			/// <para>
			/// 注意:
			/// HCA-MXのデコード処理、および定常状態の処理負荷は、
			/// ミキサの数に比例して重くなります。
			/// 本パラメーターを0に設定した場合でも、ミキサは 1 つだけ作成されます。
			/// （旧バージョンとの互換性維持のため。）
			/// HCA-MXを使用しない場合には、本パラメーターと max_voices の両方を 0
			/// に設定してください。
			/// </para>
			/// </remarks>
			public Int32 numMixers;

			/// <summary>ミキサに登録可能な最大ボイス数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ミキサごとに登録可能なHCA-MXボイスの数を指定します。
			/// HCA-MXボイスプールを作成する際には、ボイスの総数が
			/// num_mixers × max_voices を超えないようご注意ください。
			/// </para>
			/// </remarks>
			public Int32 maxVoices;

			/// <summary>入力データの最大チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーション中で再生するHCA-MXデータの最大チャンネル数を指定します。
			/// 再生するデータがモノラルの場合は1を、ステレオの場合は2を指定してください。
			/// </para>
			/// <para>
			/// 備考:
			/// HCA-MX初期化時に max_input_channels に指定された数以下の音声データが
			/// 再生可能になります。
			/// 例えば、 max_input_channels に6を指定した場合、5.1ch音声だけでなく、
			/// モノラル音声やステレオ音声も再生可能になります。
			/// 100個のデータのうち、99個がモノラル、1個がステレオの場合でも、
			/// max_input_channels には2を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 maxInputChannels;

			/// <summary>最大サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// HCA-MXの出力に指定できる最大サンプリングレートです。
			/// ミキサの最終出力でピッチを変更する場合に設定します。
			/// ミキサの最終出力でピッチを変更しない場合は、output_sampling_rateと同じ値を設定してください。
			/// </para>
			/// <para>
			/// 備考:
			/// 例えばHCA-MX再生時に<see cref="CriAtomExHcaMx.SetFrequencyRatio"/> 関数に 2.0f を指定してピッチを上げる場合は、
			/// output_sampling_rate * 2 を指定してHCA-MXを初期化してください。
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>出力チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// HCA-MXデータの出力チャンネル数を指定します。
			/// 通常、ターゲット機に接続されたスピーカーの数（出力デバイスの
			/// 最大チャンネル数）を指定します。
			/// </para>
			/// <para>
			/// 備考:
			/// モノラル音声のみを再生し、パンをコントロールしない場合には、
			/// output_channels を1にすることで、処理負荷を下げることが可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// output_channels の数を max_input_channels 以下の値に設定することは
			/// できません。
			/// </para>
			/// </remarks>
			public Int32 outputChannels;

			/// <summary>出力サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生するHCA-MXデータのサンプリングレートを指定します。
			/// HCA-MXデータを作成する際には、必ず全ての音声データを同一のサンプリング
			/// レートで作成し、その値を output_sampling_rate に指定してください。
			/// </para>
			/// <para>
			/// 備考:
			/// HCA-MXは、音単位のサンプリングレート変更を行えません。
			/// </para>
			/// </remarks>
			public Int32 outputSamplingRate;

			/// <summary>サウンドレンダラタイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// HCA-MXの出力先サウンドレンダラの種別を指定します。
			/// sound_renderer_type に <see cref="CriAtom.SoundRendererDefault"/> を指定した場合、
			/// 音声データはデフォルト設定のサウンドレンダラに転送されます。
			/// sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、
			/// 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			/// sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合、
			/// 音声データはASR（Atom Sound Renderer）に転送されます。
			/// （ASRの出力先は、ASR初期化時に別途指定。）
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

		}
		/// <summary>HCA-MX初期化用ワーク領域サイズの計算</summary>
		/// <param name="config">HCA-MX初期化用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXの初期化に必要なワーク領域のサイズを取得します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExHcaMx.Initialize"/> 関数でHCA-MXの初期化を行う場合、
		/// 本関数で計算したサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// HCA-MXの初期化に必要なワークメモリのサイズは、HCA-MX初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomExHcaMx.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExHcaMx.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExHcaMx.Initialize"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExHcaMx.Config config)
		{
			fixed (CriAtomExHcaMx.Config* configPtr = &config)
				return NativeMethods.criAtomExHcaMx_CalculateWorkSize(configPtr);
		}

		/// <summary>ワーク領域サイズ計算用コンフィグ構造体の設定</summary>
		/// <param name="config">HCA-MX初期化用コンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ワーク領域サイズの計算用に、HCA-MX初期化用コンフィグ構造体
		/// （ <see cref="CriAtomExHcaMx.Config"/> 構造体）を仮登録します。
		/// HCA-MXボイスプールの作成に必要なワーク領域のサイズは、
		/// HCA-MX初期化時（ <see cref="CriAtomExHcaMx.Initialize"/> 関数実行時）
		/// に設定する構造体のパラメーターによって変化します。
		/// そのため、通常はボイスプールの作成に必要なワーク領域サイズを計算する前に、
		/// HCA-MXを初期化する必要があります。
		/// 本関数を使用してHCA-MX初期化用コンフィグ構造体を登録した場合、
		/// <see cref="CriAtomExVoicePool.CalculateWorkSizeForHcaMxVoicePool"/>
		/// 関数が初期化処理なしに使用可能となります。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数（ config ）に null を指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExVoicePool.SetDefaultConfigForHcaMxVoicePool"/>
		/// 適用時と同じパラメーター）でワーク領域サイズを計算します。
		/// 現状、本関数で一旦コンフィグ構造体を設定すると、
		/// 設定前の状態（未初期化状態でのワーク領域サイズ計算をエラーとする動作）
		/// に戻すことができなくなります。
		/// （関数を再度実行してパラメーターを上書きすることは可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で登録した初期化用コンフィグ構造体は、
		/// HCA-MX未初期化状態でのワーク領域サイズ計算にしか使用されません。
		/// HCA-MX初期化後には本関数に設定したパラメーターではなく、
		/// 初期化時に指定されたパラメーターがワーク領域サイズの計算に使用されます。
		/// （本関数で登録する構造体のパラメーターと、
		/// HCA-MX初期化時に使用する構造体のパラメーターが異なる場合、
		/// ワーク領域サイズが不足し、オブジェクトの作成に失敗する恐れがあります。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForHcaMxVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForHcaMxVoicePool"/>
		public static unsafe void SetConfigForWorkSizeCalculation(in CriAtomExHcaMx.Config config)
		{
			fixed (CriAtomExHcaMx.Config* configPtr = &config)
				NativeMethods.criAtomExHcaMx_SetConfigForWorkSizeCalculation(configPtr);
		}

		/// <summary>HCA-MXの初期化</summary>
		/// <param name="config">HCA-MX初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXの初期化を行います。
		/// 本関数を実行することで、HCA-MXデータの出力機能が起動されます。
		/// </para>
		/// <para>
		/// 備考:
		/// HCA-MXの初期化に必要なワークメモリのサイズは、HCA-MX初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomExHcaMx.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExHcaMx.SetDefaultConfig"/> 適用時と同じパラメーター）で初期化処理を行います。
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
		/// 本関数を実行後、必ず対になる <see cref="CriAtomExHcaMx.Finalize"/> 関数を実行してください。
		/// また、 <see cref="CriAtomExHcaMx.Finalize"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExHcaMx.CalculateWorkSize"/>
		public static unsafe void Initialize(in CriAtomExHcaMx.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExHcaMx.Config* configPtr = &config)
				NativeMethods.criAtomExHcaMx_Initialize(configPtr, work, workSize);
		}

		/// <summary>HCA-MXの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXの終了処理を行います。
		/// 本関数を実行することで、HCA-MXデータの出力が停止されます。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// HCA-MX初期化時に確保されたメモリ領域が解放されます。
		/// （HCA-MX初期化時にワーク領域を渡した場合、本関数実行後であれば
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
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomExHcaMx.Initialize"/>
		public static void Finalize()
		{
			NativeMethods.criAtomExHcaMx_Finalize();
		}

		/// <summary>ミキサのバスセンドレベル設定</summary>
		/// <param name="mixerId">ミキサID</param>
		/// <param name="busName">バス名</param>
		/// <param name="level">センドレベル値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ミキサのバスセンドレベルを設定します。
		/// デフォルト状態では、HCA-MXのデコード結果はミキサに格納された後、
		/// バス0へ1.0fのレベルで送信されます。
		/// 本関数を使用することで、デコード結果を他のバスへもセンドすることが可能になります。
		/// （ミキサごとに異なるバスエフェクトを適用可能になります。）
		/// </para>
		/// </remarks>
		public static void SetBusSendLevelByName(Int32 mixerId, ArgString busName, Single level)
		{
			NativeMethods.criAtomExHcaMx_SetBusSendLevelByName(mixerId, busName.GetPointer(stackalloc byte[busName.BufferSize]), level);
		}

		/// <summary>ミキサの出力周波数調整比の設定</summary>
		/// <param name="mixerId">ミキサID</param>
		/// <param name="ratio">センドレベル値（0.25f～4.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ミキサの出力周波数調整比を設定します。
		/// 周波数調整比は、音声データの周波数と再生周波数の比率で、再生速度の倍率と等価です。
		/// 周波数比が1.0fを超える場合、音声データは原音より高速に再生され、
		/// 1.0f未満の場合は、音声データは原音より低速で再生されます。
		/// 本関数を使用することで、対象のミキサを使用するプレーヤーで再生される全てのHCA-MXボイス
		/// （HCA-MX用にエンコードされた音声データの再生）について、再生速度が変更されます
		/// （HCA-MXボイスを再生する場合、個々のプレーヤーでの再生速度の設定は無視されます）。
		/// </para>
		/// </remarks>
		public static void SetFrequencyRatio(Int32 mixerId, Single ratio)
		{
			NativeMethods.criAtomExHcaMx_SetFrequencyRatio(mixerId, ratio);
		}

		/// <summary>ASRラックIDの指定</summary>
		/// <param name="mixerId">ミキサID</param>
		/// <param name="rackId">ASRラックID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ミキサの出力先ASRラックIDを指定します。
		/// 本関数を使用することで、対象のミキサを使用するプレーヤーで再生される全てのHCA-MXボイス
		/// （HCA-MX用にエンコードされた音声データの再生）について、出力先ASRラックIDが変更されます
		/// （HCA-MXボイスを再生する場合、個々のプレーヤーでのASRラックIDの設定は無視されます）。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は ミキサのサウンドレンダラタイプにASRを使用する場合にのみ効果があります。
		/// （他のサウンドレンダラタイプの場合、本関数の設定値は無視されます。）
		/// </para>
		/// </remarks>
		public static void SetAsrRackId(Int32 mixerId, Int32 rackId)
		{
			NativeMethods.criAtomExHcaMx_SetAsrRackId(mixerId, rackId);
		}

		/// <summary>HCA-MXボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXボイスプールの仕様を指定するための構造体です。
		/// <see cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForHcaMxVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForHcaMxVoicePool"/>
		public unsafe partial struct VoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtomHcaMx.PlayerConfig playerConfig;

			/// <summary>ストリーム再生専用かどうか</summary>
			public NativeBool isStreamingOnly;

			/// <summary>最小チャンネル数</summary>
			public Int32 minChannels;

		}
	}
}