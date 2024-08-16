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
	/// <summary>CriAtomHcaMx API</summary>
	public static partial class CriAtomHcaMx
	{
		/// <summary>HCA-MX初期化コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">コンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomHcaMx.Initialize"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomHcaMx.Config"/> ）に、デフォルト値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomHcaMx.Initialize"/>
		/// <seealso cref="CriAtomHcaMx.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomHcaMx.Config pConfig)
		{
			fixed (CriAtomHcaMx.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomHcaMx_SetDefaultConfig_(pConfigPtr);
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
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomHcaMx.Initialize"/> 関数でHCA-MXの初期化を行う場合、
		/// 本関数で計算したサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// HCA-MXの初期化に必要なワークメモリのサイズは、HCA-MX初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomHcaMx.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomHcaMx.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtomHcaMx.Initialize"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomHcaMx.Config config)
		{
			fixed (CriAtomHcaMx.Config* configPtr = &config)
				return NativeMethods.criAtomHcaMx_CalculateWorkSize(configPtr);
		}

		/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomHcaMx.SetDefaultConfig"/> メソッドで
		/// 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomHcaMx.Initialize"/> 関数
		/// に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomHcaMx.SetDefaultConfig"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomHcaMx.Initialize"/>
		/// <seealso cref="CriAtomHcaMx.SetDefaultConfig"/>
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

			/// <summary>ミキサ数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// HCA-MXデコード結果を送信するミキサの数を指定します。
			/// ミキサを複数作成することで、
			/// ミキサごとに異なるバスエフェクトを適用することが可能になります。
			/// </para>
			/// <para>
			/// 注意:
			/// HCA-MXのデコード処理、および定常状態の処理負荷は、
			/// ミキサの数に比例して重くなります。
			/// </para>
			/// </remarks>
			public Int32 numMixers;

			/// <summary>ミキサに登録可能な最大プレーヤー数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ミキサごとに登録可能なHCA-MXプレーヤーの数を指定します。
			/// </para>
			/// </remarks>
			public Int32 maxPlayers;

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
			/// </para>
			/// <para>
			/// 備考:
			/// 例えば<see cref="CriAtomHcaMx.SetFrequencyRatio"/> 関数に 2.0f を指定する場合は
			/// output_sampling_rate * 2 を指定してください。
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
			/// HCA-MXは、音単位のサンプリングレート変更を行えません。
			/// HCA-MXデータを作成する際には、必ず全ての音声データを同一のサンプリング
			/// レートで作成し、その値を output_sampling_rate に指定してください。
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
		/// <summary>ワーク領域サイズ計算用コンフィグ構造体の設定</summary>
		/// <param name="config">HCA-MX初期化用コンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ワーク領域サイズの計算用に、HCA-MX初期化用コンフィグ構造体
		/// （ <see cref="CriAtomHcaMx.Config"/> 構造体）を仮登録します。
		/// HCA-MXプレーヤーの作成に必要なワーク領域のサイズは、
		/// HCA-MX初期化時（ <see cref="CriAtomHcaMx.Initialize"/> 関数実行時）
		/// に設定する構造体のパラメーターによって変化します。
		/// そのため、通常はプレーヤーの作成に必要なワーク領域サイズを計算する前に、
		/// HCA-MXを初期化する必要があります。
		/// 本関数を使用してHCA-MX初期化用コンフィグ構造体を登録した場合、
		/// <see cref="CriAtomPlayer.CalculateWorkSizeForHcaMxPlayer"/>
		/// 関数が初期化処理なしに使用可能となります。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数（ config ）に null を指定した場合、デフォルト設定
		/// （ <see cref="CriAtomPlayer.SetDefaultConfigForHcaMxPlayer"/>
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
		/// <seealso cref="CriAtomPlayer.CalculateWorkSizeForHcaMxPlayer"/>
		/// <seealso cref="CriAtomPlayer.SetDefaultConfigForHcaMxPlayer"/>
		public static unsafe void SetConfigForWorkSizeCalculation(in CriAtomHcaMx.Config config)
		{
			fixed (CriAtomHcaMx.Config* configPtr = &config)
				NativeMethods.criAtomHcaMx_SetConfigForWorkSizeCalculation(configPtr);
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
		/// 構造体（ <see cref="CriAtomHcaMx.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomHcaMx.SetDefaultConfig"/> 適用時と同じパラメーター）で初期化処理を行います。
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
		/// 本関数を実行後、必ず対になる <see cref="CriAtomHcaMx.Finalize"/> 関数を実行してください。
		/// また、 <see cref="CriAtomHcaMx.Finalize"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtomHcaMx.CalculateWorkSize"/>
		public static unsafe void Initialize(in CriAtomHcaMx.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomHcaMx.Config* configPtr = &config)
				NativeMethods.criAtomHcaMx_Initialize(configPtr, work, workSize);
		}

		/// <summary>HCA-MXの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXの終了処理を行います。
		/// 本関数を実行することで、HCA-MXデータの出力が停止されます。
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
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
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtomHcaMx.Initialize"/>
		public static void Finalize()
		{
			NativeMethods.criAtomHcaMx_Finalize();
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
			NativeMethods.criAtomHcaMx_SetBusSendLevelByName(mixerId, busName.GetPointer(stackalloc byte[busName.BufferSize]), level);
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
		/// 本関数を使用することで、対象のミキサを使用する全てのプレーヤーの再生速度を
		/// 変更することができます（個々のプレーヤーの再生速度は変更できません）。
		/// </para>
		/// </remarks>
		public static void SetFrequencyRatio(Int32 mixerId, Single ratio)
		{
			NativeMethods.criAtomHcaMx_SetFrequencyRatio(mixerId, ratio);
		}

		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXが再生可能なプレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomPlayer.CreateHcaMxPlayer"/> 関数の引数に指定します。
		/// 作成されるプレーヤーは、オブジェクト作成時に本構造体で指定された設定に応じて、
		/// 内部リソースを必要なだけ確保します。
		/// プレーヤーが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomPlayer.SetDefaultConfigForHcaMxPlayer"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateHcaMxPlayer"/>
		/// <seealso cref="CriAtomPlayer.SetDefaultConfigForHcaMxPlayer"/>
		public unsafe partial struct PlayerConfig
		{
			/// <summary>最大出力チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のチャンネル数を指定します。
			/// <see cref="CriAtomPlayer.CreateHcaMxPlayer"/> 関数で作成されたAtomプレーヤーは、max_channelsで指定した
			/// チャンネル数"以下の"音声データを再生可能です。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>最大サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のサンプリングレートを指定します。
			/// <see cref="CriAtomPlayer.CreateHcaMxPlayer"/> 関数で作成されたAtomプレーヤーは、max_sampling_rateで指定した
			/// サンプリングレートと一致する音声データのみを再生可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// 最大サンプリングレートを下げることで、Atomプレーヤー作成時に必要となるワークメモリ
			/// のサイズを抑えることが可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大サンプリングレートに一致しないデータは、再生することはできません。
			/// 例えば、最大サンプリングレートを48000に設定した場合、作成されたAtomプレーヤーで
			/// 48000Hz以外の音声を再生することはできません。
			/// （レート変換されて出力されることはありません。）
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>ストリーミング再生を行うかどうか</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーでストリーミング再生（ファイルからの再生）を行うかどうかを指定します。
			/// streaming_flagにfalseを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ
			/// 再生（ <see cref="CriAtomPlayer.SetData"/> 関数で指定したメモリアドレスの再生）のみをサポート
			/// します。（ファイルからの再生はできません。）
			/// streaming_flagにtrueを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ
			/// 再生に加え、ファイルからの再生（ <see cref="CriAtomPlayer.SetFile"/> 関数や
			/// <see cref="CriAtomPlayer.SetContentId"/> 関数で指定されたファイルの再生）をサポートします。
			/// </para>
			/// <para>
			/// 補足:
			/// streaming_flagをtrueにした場合、Atomプレーヤー作成時にファイル読み込み用のリソース
			/// が確保されます。
			/// そのため、streaming_flagをfalseの場合に比べ、Atomプレーヤーの作成に必要なメモリの
			/// サイズが大きくなります。
			/// </para>
			/// </remarks>
			public NativeBool streamingFlag;

		}
	}
}