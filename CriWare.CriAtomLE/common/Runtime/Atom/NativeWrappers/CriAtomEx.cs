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
using System.Xml.Serialization;

namespace CriWare
{
#pragma warning disable 0465
	/// <summary>CriAtomEx API</summary>
	public static partial class CriAtomEx
	{
		/// <summary>ライブラリ初期化用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">初期化用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomEx.Initialize"/>ForUserPcmOutput 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomEx.ConfigForUserPcmOutput"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigForUserPcmOutput"/>
		public static unsafe void SetDefaultConfigForUserPcmOutput(out CriAtomEx.ConfigForUserPcmOutput pConfig)
		{
			fixed (CriAtomEx.ConfigForUserPcmOutput* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx_SetDefaultConfigForUserPcmOutput_(pConfigPtr);
		}

		/// <summary>パフォーマンスモニター機能の追加</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス計測機能を追加し、パフォーマンス計測処理を開始します。
		/// 本関数を実行後、 <see cref="CriAtomEx.GetPerformanceInfo"/> 関数を実行することで、
		/// サーバー処理の負荷や、サーバー処理の実行間隔等、ライブラリのパフォーマンス情報を
		/// 取得することが可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.GetPerformanceInfo"/>
		/// <seealso cref="CriAtomEx.DetachPerformanceMonitor"/>
		public static void AttachPerformanceMonitor()
		{
			NativeMethods.criAtomEx_AttachPerformanceMonitor_();
		}

		/// <summary>パフォーマンスモニター機能の削除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス計測処理を終了し、パフォーマンス計測機能を削除します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		public static void DetachPerformanceMonitor()
		{
			NativeMethods.criAtomEx_DetachPerformanceMonitor_();
		}

		/// <summary>パフォーマンスモニターのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 現在までの計測結果を破棄します。
		/// パフォーマンスモニターは、 <see cref="CriAtomEx.AttachPerformanceMonitor"/> 関数実行直後
		/// からパフォーマンス情報の取得を開始し、計測結果を累積します。
		/// 以前の計測結果を以降の計測結果に含めたくない場合には、
		/// 本関数を実行し、累積された計測結果を一旦破棄する必要があります。
		/// </para>
		/// </remarks>
		public static void ResetPerformanceMonitor()
		{
			NativeMethods.criAtomEx_ResetPerformanceMonitor_();
		}

		/// <summary>パフォーマンス情報の取得</summary>
		/// <param name="pInfo">パフォーマンス情報</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス情報を取得します。
		/// 本関数は、 <see cref="CriAtomEx.AttachPerformanceMonitor"/> 関数実行後から
		/// <see cref="CriAtomEx.DetachPerformanceMonitor"/> 関数を実行するまでの間、利用可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachPerformanceMonitor"/>
		/// <seealso cref="CriAtomEx.DetachPerformanceMonitor"/>
		public static unsafe void GetPerformanceInfo(out CriAtom.PerformanceInfo pInfo)
		{
			fixed (CriAtom.PerformanceInfo* pInfoPtr = &pInfo)
				NativeMethods.criAtomEx_GetPerformanceInfo_(pInfoPtr);
		}

		/// <summary>チャンネルマッピングパターンの指定</summary>
		/// <param name="nch">マッピングパターンを変更するチャンネル数</param>
		/// <param name="type">マッピングパターン</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声データの各チャンネルと出力スピーカーの対応付けを変更します。
		/// 例えば、5ch音声データを再生した場合、デフォルト状態では各チャンネルが
		/// L, R, C, Ls, Rs の順で出力されます。
		/// これに対し、<see cref="CriAtomEx.SetChannelMapping"/>(5, 1); を実行した場合、
		/// 5ch音声データの各チャンネルが L, R, LFE, Ls, Rs の順で出力されるようになります。
		/// </para>
		/// <para>
		/// 備考:
		/// 現状、本関数は5ch音声データのマッピングパターン変更にしか対応していません。
		/// </para>
		/// </remarks>
		public static void SetChannelMapping(Int32 nch, UInt32 type)
		{
			NativeMethods.criAtomEx_SetChannelMapping_(nch, type);
		}

		/// <summary>ADXデータのビットレート計算</summary>
		/// <param name="numChannels">データのチャンネル数</param>
		/// <param name="samplingRate">データのサンプリングレート</param>
		/// <returns>ビットレート[bps]</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADXデータのビットレートを計算します。
		/// 計算に失敗すると本関数は-1を返します。
		/// 計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// </remarks>
		public static Int32 CalculateAdxBitrate(Int32 numChannels, Int32 samplingRate)
		{
			return NativeMethods.criAtomEx_CalculateAdxBitrate_(numChannels, samplingRate);
		}

		/// <summary>HCAデータのビットレート計算</summary>
		/// <param name="numChannels">データのチャンネル数</param>
		/// <param name="samplingRate">データのサンプリングレート</param>
		/// <param name="quality">データのエンコード品質</param>
		/// <returns>ビットレート[bps]</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCAデータのビットレートを計算します。
		/// 計算に失敗すると本関数は-1を返します。
		/// 計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// qualityにはCRI Atom CraftまたはCRI Atom Encoderで設定したエンコード品質を指定します。
		/// </para>
		/// </remarks>
		public static Int32 CalculateHcaBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality)
		{
			return NativeMethods.criAtomEx_CalculateHcaBitrate_(numChannels, samplingRate, quality);
		}

		/// <summary>HCA-MXデータのビットレート計算</summary>
		/// <param name="numChannels">データのチャンネル数</param>
		/// <param name="samplingRate">データのサンプリングレート</param>
		/// <param name="quality">データのエンコード品質</param>
		/// <returns>ビットレート[bps]</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXデータのビットレートを計算します。
		/// 計算に失敗すると本関数は-1を返します。
		/// 計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// qualityにはCRI Atom CraftまたはCRI Atom Encoderで設定したエンコード品質を指定します。
		/// </para>
		/// </remarks>
		public static Int32 CalculateHcaMxBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality)
		{
			return NativeMethods.criAtomEx_CalculateHcaMxBitrate_(numChannels, samplingRate, quality);
		}

		/// <summary>ストリーミング情報の取得</summary>
		/// <param name="streamingInfo">ストリーミング情報保存先のポインタ</param>
		/// <returns>値を取得できた</returns>
		/// <returns>値を取得できなかった</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリのストリーミング管理モジュールからストリーミング情報を取得します。
		/// 本関数は、呼び出された時点のストリーミング情報を streaming_info に保存します。
		/// </para>
		/// <para>
		/// 注意:
		/// Atomサーバー内の処理と一部排他制御しているため、
		/// 優先度逆転によりAtomサーバーを止めてしまわないように注意してください。
		/// 一部のプラットフォームでは、ストリーミング情報を取得できません。
		/// 本関数の戻り値を確認してください。
		/// エラーが原因でストリーミング情報を取得できなかった場合については、
		/// エラーコールバックが発生していないかを確認してください。
		/// </para>
		/// </remarks>
		public static unsafe bool GetStreamingInfo(out CriAtom.StreamingInfo streamingInfo)
		{
			fixed (CriAtom.StreamingInfo* streamingInfoPtr = &streamingInfo)
				return NativeMethods.criAtomEx_GetStreamingInfo_(streamingInfoPtr);
		}

		/// <summary>ファイルI/Oの空き時間を使ったストリーミング読み込みを行うかどうか</summary>
		/// <param name="flag">true=ファイルI/Oの空き時間を使って読み込む</param>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリのストリーミング管理モジュールに対して、
		/// ファイルI/Oの空き時間を使ってストリーミング読み込みを行うかどうかを設定します。
		/// trueを設定すると、CRI Atomライブラリのストリーミング管理モジュールは
		/// ファイルI/Oの空き時間を使って、空きバッファーに対してデータを余分に読み込みます。
		/// falseを設定すると、CRI Atomライブラリのストリーミング管理モジュールは
		/// ファイルI/Oの空き時間を使わなくなり、余分なストリーミング読み込みを行わなくなります。
		/// デフォルトではtrueを設定した状態です。
		/// </para>
		/// <para>
		/// 備考：
		/// ファイルI/Oの空き時間を使い、空きバッファーに対してデータを余分に読み込んでおくことで、
		/// シークの発生頻度を減らす事ができ、総合的なファイルI/Oの効率が向上します。
		/// 一方、通常ファイルのロード処理は、ストリーミングの読み込みよりも優先度が低いため、
		/// 空きバッファーが大きすぎると通常ファイルのロード処理を大幅に遅延させてしまいます。
		/// </para>
		/// <para>
		/// 注意:
		/// Atomサーバー内の処理と一部排他制御しているため、
		/// 優先度逆転によりAtomサーバーを止めてしまわないように注意してください。
		/// </para>
		/// </remarks>
		public static bool SetFreeTimeBufferingFlagForDefaultDevice(NativeBool flag)
		{
			return NativeMethods.criAtomEx_SetFreeTimeBufferingFlagForDefaultDevice_(flag);
		}

		/// <summary>球面座標構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 球面座標を扱うための構造体です。
		/// </para>
		/// </remarks>
		public unsafe partial struct SphericalCoordinates
		{
			/// <summary>方位角</summary>
			public Single azimuth;

			/// <summary>仰角</summary>
			public Single elevation;

			/// <summary>距離</summary>
			public Single distance;

		}
		/// <summary>距離減衰パラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音源の距離減衰に関するパラメーターを保持する構造体です。
		/// </para>
		/// </remarks>
		public unsafe partial struct _3dAttenuationParameter
		{
			public Single volume;

			public Single minAttenuationDistance;

			public Single maxAttenuationDistance;

		}
		/// <summary>ユーザアロケーターの登録</summary>
		/// <param name="pMallocFunc">メモリ確保関数</param>
		/// <param name="pFreeFunc">メモリ解放関数</param>
		/// <param name="pObj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atom ライブラリにメモリアロケーター（メモリの確保／解放関数）を登録します。
		/// 本メソッドでアロケーターを登録すると、Atomライブラリがワーク領域を必要とするタイミングで、
		/// ユーザが登録したメモリ確保／解放処理が呼び出されることになります。
		/// その結果、ワーク領域を必要とする関数（ <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数等）に対し、
		/// 個別にワーク領域をセットする処理を省略することが可能になります。
		/// （ワーク領域に null ポインタ、ワーク領域サイズに 0 バイトを指定した場合でも、
		/// アロケーターからの動的メモリ確保によりライブラリが問題なく動作するようになります。）
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ確保／解放関数のポインタに null を指定することで、
		/// アロケーターの登録を解除することも可能です。
		/// ただし、未解放のメモリ領域が残っている状態で登録を解除すると、
		/// エラーコールバックが返され、登録の解除に失敗します。
		/// （引き続き登録済みのアロケーターが呼び出されることになります。）
		/// 本メソッドは内部的に <see cref="CriAtom.SetUserAllocator"/> メソッドや
		/// <see cref="CriAtom.SetUserMallocFunction"/> 関数、 <see cref="CriAtom.SetUserFreeFunction"/>
		/// 関数を呼び出します。
		/// 本関数とこれらの API を併用しないようご注意ください。
		/// （本関数の呼び出しにより、上記 API にセットした内容が上書きされます。）
		/// また、登録されたメモリアロケーター関数はマルスレッドモード時に複数のスレッドからコール
		/// されることがあります。従って、メモリアロケート処理がスレッドセーフでない場合は独自に
		/// メモリアロケート処理を排他制御する必要があります。
		/// </para>
		/// </remarks>
		public static unsafe void SetUserAllocator(delegate* unmanaged[Cdecl]<IntPtr, UInt32, IntPtr> pMallocFunc, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> pFreeFunc, IntPtr pObj)
		{
			NativeMethods.criAtomEx_SetUserAllocator_((IntPtr)pMallocFunc, (IntPtr)pFreeFunc, pObj);
		}

		/// <summary>ライブラリ初期化用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">初期化用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomEx.Initialize"/> 関数に設定するコンフィグ構造体（ <see cref="CriAtomEx.Config"/> ）に、
		/// デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomEx.Config pConfig)
		{
			fixed (CriAtomEx.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>ライブラリ初期化用ワーク領域サイズの計算</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを使用するために必要な、ワーク領域のサイズを取得します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomEx.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.Config"/> 構造体のacf_infoメンバに値を設定している場合、本関数は失敗し-1を返します。
		/// 初期化処理内でACFデータの登録を行う場合は、本関数値を使用したメモリ確保ではなくADXシステムによる
		/// メモリアロケーターを使用したメモリ確保処理が必要になります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Config"/>
		/// <seealso cref="CriAtomEx.Initialize"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomEx.Config config)
		{
			fixed (CriAtomEx.Config* configPtr = &config)
				return NativeMethods.criAtomEx_CalculateWorkSize(configPtr);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <seealso cref="CriAtomEx.SetDefaultConfigForUserPcmOutput"/>
		public unsafe partial struct ConfigForUserPcmOutput
		{
			/// <summary>AtomEx初期化用コンフィグ構造体</summary>
			public CriAtomEx.Config atomEx;

			/// <summary>ASR初期化用コンフィグ</summary>
			public CriAtomExAsr.Config asr;

			/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
			public CriAtomExHcaMx.Config hcaMx;

		}
		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomEx.SetDefaultConfig"/> メソッドで構造体にデフォルト
		/// パラメーターをセットした後、 <see cref="CriAtomEx.Initialize"/> 関数に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomEx.SetDefaultConfig"/> メソッドを使用しない
		/// 場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Initialize"/>
		/// <seealso cref="CriAtomEx.SetDefaultConfig"/>
		[System.Serializable]
		[XmlType(Namespace = "CriAtomEx")]
		public unsafe partial struct Config
		{
			/// <summary>スレッドモデル</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリのスレッドモデルを指定します。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx.ThreadModel"/>
			public CriAtomEx.ThreadModel threadModel;

			/// <summary>サーバー処理の実行頻度</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// サーバー処理を実行する頻度を指定します。
			/// 通常、アプリケーションのフレームレートと同じ値を指定します。
			/// CRI Atomライブラリは、ファイル読み込みの管理や、音声データのデコード、音声の出力、
			/// ステータスの更新等、ライブラリ内部で行う処理のほとんどを1つの関数でまとめて
			/// 行います。
			/// CRIミドルウェアでは、こういったライブラリ内の処理を一括して行う関数のことを
			/// "サーバー処理"と呼んでいます。
			/// スレッドモデルが <see cref="CriAtomEx.ThreadModel.Multi"/> の場合、サーバー処理は
			/// CRI Atomライブラリが作成するスレッドで、定期的に実行されます。
			/// スレッドモデルが <see cref="CriAtomEx.ThreadModel.Single"/> や <see cref="CriAtomEx.ThreadModel.UserMulti"/>
			/// の場合、サーバー処理は <see cref="CriAtomEx.ExecuteMain"/> 関数内で実行されます。
			/// server_frequency には、サーバー処理を実行する頻度を指定します。
			/// スレッドモデルが <see cref="CriAtomEx.ThreadModel.Multi"/> の場合、CRI Atomライブラリは指定された
			/// 頻度でサーバー処理が実行されるよう、サーバー処理の呼び出し間隔を調節します。
			/// スレッドモデルが <see cref="CriAtomEx.ThreadModel.Single"/> や <see cref="CriAtomEx.ThreadModel.UserMulti"/>
			/// の場合、ユーザは <see cref="CriAtomEx.ExecuteMain"/> 関数を server_frequency で指定した頻度以上
			/// で実行する必要があります。
			/// アプリケーションのフレームレートの変動が大きく、サーバー処理を実行する頻度にバラツキ
			/// ができてしまう場合には、最悪のフレームレートを想定して server_frequency の値を指定
			/// するか、またはスレッドモデルに <see cref="CriAtomEx.ThreadModel.Multi"/> を指定してください。
			/// </para>
			/// <para>
			/// 備考:
			/// Atomライブラリのサーバー処理では、以下のような処理が行われます。
			/// - 発音リクエストの処理（ボイスの取得等）
			/// - パラメーターの更新（ボリュームやパン、ピッチ等の変更の適用）
			/// - 音声データのデコードと出力
			/// サーバー処理の実行頻度を多くすると、単位サーバー処理当たりの音声データデコード量が少なくなります。
			/// その結果、単位サーバー当たりの処理負荷は小さくなります（負荷が分散されます）が、
			/// サーバー処理の実行に伴うオーバーヘッドは大きくなります。
			/// （スレッドの起床回数やパラメーターの更新回数が多くなります。）
			/// サーバー処理の実行頻度を少なくすると、スレッドの起床や発音リクエストの処理、
			/// パラメーターの更新処理の回数が減り、アプリケーション全体の処理負荷は下がります。
			/// 反面、データをリロードする頻度が下がるため、単位サーバー処理当たりデコード量は増え、
			/// デコード結果を保持するためのバッファーサイズが余分に必要になります。
			/// また、発音リクエストを処理する頻度が下がるため、
			/// 発音リクエストから音声出力開始までにかかる時間は長くなります。
			/// </para>
			/// <para>
			/// 注意:
			/// スレッドモデルに <see cref="CriAtomEx.ThreadModel.Single"/> や <see cref="CriAtomEx.ThreadModel.UserMulti"/>
			/// を指定したにもかかわらず、 <see cref="CriAtomEx.ExecuteMain"/> 関数が server_frequency で
			/// 指定した値以下の頻度でしか実行されなかった場合、再生中の音が途切れる等の問題が
			/// 発生する可能性がありますので、ご注意ください。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx.ExecuteMain"/>
			public Single serverFrequency;

			/// <summary>パラメーター更新間隔</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// サーバー処理実行時にパラメーターの更新処理を行う間隔を指定します。
			/// parameter_update_interval の値を変更することで、
			/// サーバー処理の実行回数を変えることなくパラメーター更新頻度を下げることが可能です。
			/// 例えば、 parameter_update_interval を 2 に設定すると、
			/// サーバー処理 2 回に対し、 1 回だけパラメーターの変更が行われます。
			/// （パラメーターの更新頻度が 1/2 になります。）
			/// </para>
			/// <para>
			/// 備考:
			/// サーバー処理周波数（ server_frequency ）を下げると、
			/// サーバー処理の実行回数が減るため、アプリケーション全体の処理負荷は下がりますが、
			/// サーバー処理同士の間隔が開くため、バッファリングすべきデータの量が増加します。
			/// その結果、バッファリングのために必要なメモリのサイズは増加します。
			/// これに対し、サーバー処理周波数を変更せずにパラメーター更新間隔（ parameter_update_interval ）
			/// の値を上げた場合、メモリサイズを増加させずに負荷を下げることが可能となります。
			/// ただし、サーバー処理の駆動に伴う処理のオーバーヘッド（スレッドの起床負荷等）
			/// は削減されないため、サーバー処理の回数を減らす場合に比べ、負荷削減の効果は薄いです。
			/// </para>
			/// <para>
			/// 注意:
			/// parameter_update_interval の値を変更した場合、
			/// 発音リクエストの処理頻度も少なくなります。
			/// そのため、 parameter_update_interval の値を変更すると、
			/// 発音リクエストから音声出力開始までにかかる時間が長くなります。
			/// </para>
			/// </remarks>
			public Int32 parameterUpdateInterval;

			/// <summary>CRI Atom Library以外を使った音声出力を行うことを指定するフラグ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atom Libraryを使用せずに音声出力する際にtrueを指定します。
			/// <see cref="CriAtomEx.SetDefaultConfig"/> メソッドでは、CRI Atom Libraryを指定するため、falseが指定されます。
			/// CRI Atom Library以外の音声出力ライブラリを用いる場合は、本フラグにtrueを指定してから、<see cref="CriAtomEx.Initialize"/>を
			/// 実行するようにしてください。
			/// </para>
			/// <para>
			/// 注意:
			/// 本フラグを切り替える際には、Atomライブラリを<see cref="CriAtomEx.Finalize"/>で終了してから、フラグの内容を変更し、再度
			/// <see cref="CriAtomEx.Initialize"/>にて初期化処理を行うようにしてください。
			/// </para>
			/// </remarks>
			public NativeBool enableAtomSoundDisabledMode;

			/// <summary>最大バーチャルボイス数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーションで同時に発音制御を行うボイスの数です。
			/// Atomライブラリは、初期化時に max_virtual_voices で指定された数分だけ
			/// 発音管理に必要なリソースを確保します。
			/// </para>
			/// <para>
			/// 備考:
			/// max_virtual_voices で指定された数以上の音声を同時に発音することはできません。
			/// また、 max_virtual_voices 分の発音リクエストを行ったとしても、実際に
			/// 発音される音声の数は、必ずしも max_virtual_voices に一致するとは限りません。
			/// 実際に発音可能な音声の数は、ボイスプールで確保されたボイス数や、
			/// ターゲット機で利用可能なハードウェアボイスの数に依存します。
			/// バーチャルボイス数の目安は、「最大同時発音数＋1V当たりの発音リクエスト数」です。
			/// バーチャルボイス数が最大同時発音数より少ない場合や、
			/// 発音数とリクエスト数の合計が最大バーチャルボイスを超える場合、
			/// エラーコールバック関数に警告が返される可能性があります。
			/// <see cref="CriAtomEx.VoiceAllocationMethod.RetryVoiceAllocation"/> を指定して AtomEx プレーヤーを作成する場合、
			/// 上記よりもさらに多くのバーチャルボイスを必要とする可能性があります。
			/// </para>
			/// </remarks>
			public Int32 maxVirtualVoices;

			/// <summary>最大パラメーターブロック数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 音声再生時にパラメーター管理を行うための領域の数です。
			/// Atomライブラリは、初期化時に max_parameter_blocks で指定された数分だけ
			/// パラメーター管理に必要なリソースを確保します。
			/// </para>
			/// <para>
			/// 備考:
			/// 1つのキューを再生するのに必要なパラメーターブロック数は、
			/// 再生するキューの内容によって変化します。
			/// （操作するパラメーターの数に比例して必要なパラメーターブロック数は増加します。）
			/// パラメーターブロック数が不足した場合、再生するキューに対して
			/// 一部のパラメーターが設定されないことになります。
			/// （ボリュームやピッチ、フィルター等が意図した値にならない可能性があります。）
			/// アプリケーション実行中にパラメーターブロック数不足のエラーが発生した場合、
			/// max_parameter_blocks の値を増やしてください。
			/// </para>
			/// </remarks>
			public Int32 maxParameterBlocks;

			/// <summary>最大ボイスリミットグループ数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーションで作成するボイスリミットグループの数です。
			/// Atomライブラリは、初期化時に max_voice_limit_groups で指定された数分
			/// のボイスリミットグループを作成できるリソースを確保します。
			/// </para>
			/// <para>
			/// 注意:
			/// max_voice_limit_groups で指定された数以上のボイスリミットグループを
			/// 作成することはできません。
			/// オーサリングツール上で作成したボイスリミットグループの数が
			/// max_voice_limit_groups を超える場合、ACFファイルのロードに失敗します。
			/// </para>
			/// </remarks>
			public Int32 maxVoiceLimitGroups;

			/// <summary>最大カテゴリ数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーションで作成するカテゴリの数です。
			/// Atomライブラリは、初期化時に max_categories で指定された数分
			/// のカテゴリを作成できるリソースを確保します。
			/// </para>
			/// <para>
			/// 注意:
			/// max_categories で指定された数以上のカテゴリを作成することはできません。
			/// オーサリングツール上で作成したカテゴリの数が
			/// max_categories を超える場合、ACFファイルのロードに失敗します。
			/// </para>
			/// </remarks>
			public Int32 maxCategories;

			/// <summary>最大AISAC数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 一つのキューに紐づけることができるAISACの最大数です。
			/// Atomライブラリは、初期化時と AtomExPlayer 作成時に
			/// max_aisacs で指定された数分のAISACを参照できるリソースを確保します。
			/// max_aisacs に指定する値の上限は55です。
			/// キュー、トラック、カテゴリの AISAC が対象となります。
			/// ミキサー AISAC は本設定値の対象ではありません。
			/// </para>
			/// </remarks>
			public Byte maxAisacs;

			/// <summary>最大バスセンド数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 一つのボイスが同時にセンドすることができるバス数の最大数です。
			/// max_bus_sends に指定する値の上限は32です。
			/// </para>
			/// </remarks>
			public Byte maxBusSends;

			/// <summary>再生単位でのカテゴリ参照数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生単位で参照可能なカテゴリの数です。
			/// Atomライブラリは、初期化時に categories_per_playback で指定された数分
			/// のカテゴリを参照できるリソースを確保します。
			/// 指定可能な最大値は <see cref="CriAtomExCategory.MaxCategoriesPerPlayback"/> です。
			/// </para>
			/// <para>
			/// 注意:
			/// categories_per_playback で指定された数以上のカテゴリをキューやプレーヤーから参照することはできません。
			/// オーサリングツール上で作成したキューの参照カテゴリ数が
			/// categories_per_playback を超える場合、ACFファイルのロードに失敗します。
			/// </para>
			/// </remarks>
			public Int32 categoriesPerPlayback;

			/// <summary>最大再生シーケンス数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーションで同時に再生するシーケンスの数です。
			/// Atomライブラリは、初期化時に max_sequences で指定された数と max_virtual_voices で指定された数の総和分
			/// のシーケンスを再生できるリソースを確保します。
			/// </para>
			/// <para>
			/// 注意:
			/// Ver.2.00以降のライブラリでは全てのキューがシーケンスとして再生されるため、 max_sequences に加えて
			/// max_virtual_voices 数分のリソースが確保されます。
			/// max_sequences で指定された数以上のシーケンスを再生することはできません。
			/// エラーコールバックが発生した場合、この値を大きくしてください。
			/// </para>
			/// </remarks>
			public Int32 maxSequences;

			/// <summary>最大再生トラック数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーションで同時再生するシーケンス内のトラック総数です。
			/// Atomライブラリは、初期化時に max_tracks で指定された数と max_virtual_voices で指定された数の総和分
			/// のトラックを再生できるリソースを確保します。
			/// </para>
			/// <para>
			/// 注意:
			/// Ver.2.00以降のライブラリでは全てのキューがシーケンスとして再生されるため、 max_tracks に加えて
			/// max_virtual_voices 数分のリソースが確保されます。
			/// max_tracks で指定された数以上のトラックを再生することはできません。
			/// エラーコールバックが発生した場合、この値を大きくしてください。
			/// </para>
			/// </remarks>
			public UInt32 maxTracks;

			/// <summary>最大トラックアイテム数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーションで同時再生するシーケンス内のイベントの総数です。
			/// Atomライブラリは、初期化時に max_track_items で指定された数と max_virtual_voices で指定された数の総和分
			/// のトラックアイテムを作成できるリソースを確保します。
			/// </para>
			/// <para>
			/// 注意:
			/// Ver.2.00以降のライブラリでは全てのキューがシーケンスとして再生されるため、 max_track_items に加えて
			/// max_virtual_voices 数分のリソースが確保されます。
			/// max_track_items で指定された数以上のトラックアイテムを
			/// 作成することはできません。
			/// トラックアイテムは波形や、ループイベント等のシーケンストラック再生時に
			/// 管理が必要なイベントです。
			/// エラーコールバックが発生した場合、この値を大きくしてください。
			/// </para>
			/// </remarks>
			public UInt32 maxTrackItems;

			/// <summary>最大AISACオートモジュレーション数（使用停止）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Ver.2.00.00以降のライブラリでは使用停止となりました。
			/// ライブラリ内部での本メンバへの参照は行われません。
			/// </para>
			/// </remarks>
			public UInt32 maxAisacAutoModulations;

			/// <summary>ピッチ変更の上限値</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomライブラリ内で適用されるピッチ変更の上限値を設定します。
			/// max_pitchに設定された値以上のピッチ変更が、ライブラリ内でクリップされます。
			/// ピッチはセント単位で指定します。
			/// 1セントは1オクターブの1/1200です。半音は100セントです。
			/// 例えば、 max_pitch に 1200.0f を設定した場合、
			/// 1200セントを超えるピッチが設定されたキューを再生したとしても、
			/// ピッチが1200セントに抑えられて再生されます。
			/// </para>
			/// <para>
			/// 備考:
			/// キューに設定されたピッチに、AISACによるピッチ変更やドップラー効果が追加適用された場合、
			/// 予期せぬレベルまでピッチが上がる恐れがあります。
			/// （ピッチに比例して単位時間当たりのデコード量が増加するため、
			/// ピッチが高すぎる音を大量に鳴らした場合、処理負荷が急増する恐れがあります。）
			/// 本パラメーターであらかじめピッチ上限を設定しておくことで、
			/// 想定外の負荷変動を回避することが可能となります。
			/// 例えば、 max_pitch に 1200.0f を設定した場合、
			/// アプリケーション中でどのような操作を行ったとしてもピッチが1200セント
			/// （＝2倍速再生）までに抑えられるため、
			/// 単位時間あたりのデコード量は最大でも通常時の2倍までに制限されます。
			/// </para>
			/// <para>
			/// 注意:
			/// max_pitchには 0.0f 以上の値を設定する必要があります。
			/// （ 0.0f を指定した場合、ピッチの変更は一切行われなくなります。）
			/// </para>
			/// </remarks>
			public Single maxPitch;

			/// <summary>最大フェーダー数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomライブラリ内で使用するフェーダーの上限値を設定します。
			/// ここで設定し、初期化時に確保したフェーダーはTrackTransitionBySelectorデータ再生時にライブラリ内部で使用します。
			/// </para>
			/// </remarks>
			public UInt32 maxFaders;

			/// <summary>3Dポジション計算を行う際の座標系</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomライブラリが3Dポジション計算を行う際、どの座標系を使用するかを設定します。
			/// </para>
			/// </remarks>
			public CriAtomEx.CoordinateSystem coordinateSystem;

			/// <summary>パンタイプがオートの場合における、リスナーのオートマッチング機能の有効化</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パンタイプがオートの場合、リスナーのオートマッチング機能を有効化するかを設定します。
			/// </para>
			/// <para>
			/// 備考：
			/// パンタイプが3Dポジショニングの場合はリスナーのオートマッチング機能は常に有効です。
			/// </para>
			/// </remarks>
			public NativeBool enableAutoMatchingInPanTypeAuto;

			/// <summary>AtomExPlayerによるカテゴリの上書きの有効化</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomExPlayerに<see cref="CriAtomExPlayer.SetCategoryById"/>関数や<see cref="CriAtomExPlayer.SetCategoryByName"/>関数
			/// を用いてカテゴリをセットした場合にキューのカテゴリ設定を上書きする機能を有効化します。
			/// </para>
			/// <para>
			/// 備考：
			/// CRI Atom Ver.2.20.31未満のライブラリは、プレーヤーに対してカテゴリ設定を行うと、
			/// キューに設定されていたカテゴリが上書きにより無効になっていました。
			/// Ver.2.20.31未満の挙動に戻す必要がある場合には本フラグにfalseを設定してください。
			/// </para>
			/// </remarks>
			public NativeBool enableCategoryOverrideByExPlayer;

			/// <summary>シーケンス先読み割合の指定</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// シーケンサーが1サーバー処理で読み込む量の割合を指定します。
			/// 1.5fを指定すると次のサーバー処理が行われる予想時刻からさらに0.5サーバー分を先読みします。
			/// 最大で3.0f、最小で1.1fが指定可能です。範囲を超える場合はクリップされます。
			/// </para>
			/// <para>
			/// 備考:
			/// サーバー周期の揺れによって発生する発音タイミングのずれが発生しづらくなります。
			/// </para>
			/// </remarks>
			public Single sequencePrepareRatio;

			/// <summary>疑似乱数生成器インターフェース</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリで使用する疑似乱数生成器インターフェースを指定します。
			/// nullを指定した場合は、デフォルトの疑似乱数生成器を使用します。
			/// </para>
			/// </remarks>
			public NativeReference<CriAtomEx.RngInterface> rngIf;

			/// <summary>CRI File System の初期化パラメーターへのポインタ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI File Systemの初期化パラメーターへのポインタを指定します。
			/// nullを指定した場合、デフォルトパラメーターでCRI File Systemを初期化します。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx.Initialize"/>
			public NativeReference<CriFs.Config> fsConfig;

			/// <summary>ACF情報へのポインタ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 初期化時にACFの登録を行う際にACF情報へのポインタを指定します。
			/// nullを指定した場合、初期化時にACFの登録は行われません。
			/// 本メンバを設定して初期化処理内でACFの登録を行う場合、 <see cref="CriAtomEx.Config"/> 構造体の
			/// max_voice_limit_groups, max_categories, categories_per_playbackの各メンバ値は
			/// ACF設定値と比較して大きい方の値が初期化用設定値として使用されます。
			/// CriAtomEx初期化時にACFの登録を行った場合、環境によっては CriAtomExAsr, CriAtomExHcaMx
			/// 等のモジュール初期化にも一部ACF内の設定値が使用されます。
			/// ACF内の設定値を使用せずにこれらのモジュールを初期化したい場合は、本メンバを使用せずに
			/// ライブラリの初期化を行い、その後ACFの登録を行ってください。
			/// </para>
			/// <para>
			/// 注意:
			/// 本メンバを設定する場合、初期化処理内でのACFデータの登録とACFデータを元に初期化に
			/// 必要なワークを動的に確保するため、初期化関数呼び出し前にメモリアロケーター関数の登録と
			/// エラーコールバック関数の登録が必要になります。
			/// 本メンバを使用する場合、ワーク領域の確保は登録されたメモリアロケーター関数を使用して行います。
			/// 取得済みメモリ領域を使用しての初期化は行えません。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx.Initialize"/>
			/// <seealso cref="CriAtomEx.SetUserAllocator"/>
			public NativeReference<CriAtomExAcf.RegistrationInfo> acfInfo;

			/// <summary>プラットフォーム固有の初期化パラメーターへのポインタ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリを動作させるために必要な、
			/// プラットフォーム固有の初期化パラメーターへのポインタを指定します。
			/// nullを指定した場合、デフォルトパラメーターでプラットフォーム毎に必要な初期化を行います。
			/// パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。
			/// パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx.Initialize"/>
			[XmlIgnore]
			public IntPtr context;

			/// <summary>ライブラリバージョン番号</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atom ライブラリのバージョン番号です。
			/// <see cref="CriAtomEx.SetDefaultConfig"/> メソッドにより、 cri_atom.h ヘッダーに定義されているバージョン番号が設定されます。
			/// </para>
			/// <para>
			/// 注意:
			/// アプリケーションでは、この値を変更しないでください。
			/// </para>
			/// </remarks>
			public UInt32 version;

			/// <summary>モジュールバージョン番号</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atom Ex のバージョン番号です。
			/// <see cref="CriAtomEx.SetDefaultConfig"/> メソッドにより、本ヘッダーに定義されているバージョン番号が設定されます。
			/// </para>
			/// <para>
			/// 注意:
			/// アプリケーションでは、この値を変更しないでください。
			/// </para>
			/// </remarks>
			public UInt32 versionEx;

			/// <summary>ライブラリバージョン文字列</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atom ライブラリのバージョン文字列です。
			/// <see cref="CriAtomEx.SetDefaultConfig"/> メソッドにより、 cri_atom.h ヘッダーに定義されているバージョン文字列が設定されます。
			/// </para>
			/// <para>
			/// 注意:
			/// アプリケーションでは、この値を変更しないでください。
			/// </para>
			/// </remarks>
			public NativeString versionString;

			/// <summary>モジュールバージョン文字列</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atom Ex のバージョン文字列です。
			/// <see cref="CriAtomEx.SetDefaultConfig"/> メソッドにより、本ヘッダーに定義されているバージョン文字列が設定されます。
			/// </para>
			/// <para>
			/// 注意:
			/// アプリケーションでは、この値を変更しないでください。
			/// </para>
			/// </remarks>
			public NativeString versionExString;

		}
		/// <summary>ライブラリの初期化</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>初期化できたかどうか？（できた：true／できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		/// ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		/// （ライブラリの機能は、本関数を実行後、 <see cref="CriAtomEx.Finalize"/> 関数を実行するまでの間、
		/// 利用可能です。）
		/// ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// <see cref="CriAtomEx.Config"/> 構造体のacf_infoメンバを使用すると初期化処理内でACFデータの登録が行えます。
		/// 初期化処理内でのACFデータ登録を行う場合は、User Allocator方式でメモリ領域を確保する必要があります。
		/// User Allocator方式を用いる場合、ユーザはライブラリにメモリ確保関数を登録しておきます。
		/// workにnull、work_sizeに0を指定して本関数を呼び出すことで、
		/// ライブラリは登録済みのメモリ確保関数を使用して必要なメモリを自動的に確保します。
		/// ユーザがワーク領域を用意する必要はありません。
		/// 初期化時に確保されたメモリは、終了処理時（ <see cref="CriAtomEx.Finalize"/> 関数実行時）に解放されます。
		/// Fixed Memory方式を用いる場合、ワーク領域として別途確保済みのメモリ領域を本関数に
		/// 設定する必要があります。
		/// ワーク領域のサイズは <see cref="CriAtomEx.CalculateWorkSize"/> 関数で取得可能です。
		/// 初期化処理の前に <see cref="CriAtomEx.CalculateWorkSize"/> 関数で取得したサイズ分のメモリを予め
		/// 確保しておき、本関数に設定してください。
		/// 尚、Fixed Memory方式を用いた場合、ワーク領域はライブラリの終了処理（ <see cref="CriAtomEx.Finalize"/> 関数）
		/// を行うまでの間、ライブラリ内で利用され続けます。
		/// ライブラリの終了処理を行う前に、ワーク領域のメモリを解放しないでください。
		/// </para>
		/// <para>
		/// 例:
		/// 【User Allocator方式によるライブラリの初期化】
		/// User Allocator方式を用いる場合、ライブラリの初期化／終了の手順は以下の通りです。
		/// -# 初期化処理実行前に、 <see cref="CriAtomEx.SetUserAllocator"/> 関数を用いてメモリ確保／解放関数を登録する。
		/// -# 初期化用コンフィグ構造体にパラメーターをセットする。
		/// -# <see cref="CriAtomEx.Initialize"/> 関数で初期化処理を行う。
		/// （workにはnull、work_sizeには0を指定する。）
		/// -# アプリケーション終了時に <see cref="CriAtomEx.Finalize"/> 関数で終了処理を行う。
		/// </para>
		/// <para>
		/// 【Fixed Memory方式によるライブラリの初期化】
		/// Fixed Memory方式を用いる場合、ライブラリの初期化／終了の手順は以下の通りです。
		/// -# 初期化用コンフィグ構造体にパラメーターをセットする。
		/// -# ライブラリの初期化に必要なワーク領域のサイズを、 <see cref="CriAtomEx.CalculateWorkSize"/>
		/// 関数を使って計算する。
		/// -# ワーク領域サイズ分のメモリを確保する。
		/// -# <see cref="CriAtomEx.Initialize"/> 関数で初期化処理を行う。
		/// （workには確保したメモリのアドレスを、work_sizeにはワーク領域のサイズを指定する。）
		/// -# アプリケーション終了時に <see cref="CriAtomEx.Finalize"/> 関数で終了処理を行う。
		/// -# ワーク領域のメモリを解放する。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて
		/// 変化します。
		/// また、必要なワーク領域のサイズは、プラットフォームによっても異なります。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 現状、ストリーム再生を行うかどうかに関係なく、 Atom ライブラリは必ず
		/// CRI File System ライブラリの機能を使用します。
		/// そのため、CRI File Systemライブラリの初期化が行われていない場合、
		/// Atom ライブラリは初期化処理時に内部で CRI File System ライブラリの初期化を行います。
		/// Atom ライブラリが内部で CRI File System ライブラリを初期化する場合、
		/// CRI File System の初期化パラメーターとして、 <see cref="CriAtomEx.Config"/> 構造体の
		/// fs_config パラメーターを使用します。
		/// fs_config が null の場合、 Atom ライブラリはデフォルトパラメーター（
		/// ::criFs_SetDefaultConfig メソッドの設定値）で CRI File System ライブラリを初期化します。
		/// 尚、本関数を実行する時点で、既に CRI File System ライブラリが初期化済みである場合、
		/// 本関数内では CRI File System ライブラリの初期化は行われません。
		/// 本関数を実行後、必ず対になる <see cref="CriAtomEx.Finalize"/> 関数を実行してください。
		/// また、 <see cref="CriAtomEx.Finalize"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Config"/>
		/// <seealso cref="CriAtomEx.Finalize"/>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomEx.CalculateWorkSize"/>
		public static unsafe bool Initialize(in CriAtomEx.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx.Config* configPtr = &config)
				return NativeMethods.criAtomEx_Initialize(configPtr, work, workSize);
		}

		/// <summary>ライブラリの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Initialize"/>
		public static void Finalize()
		{
			NativeMethods.criAtomEx_Finalize();
		}

		/// <summary>スレッドモデル</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリがどのようなスレッドモデルで動作するかを表します。
		/// ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）に <see cref="CriAtomEx.Config"/>
		/// 構造体にて指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Initialize"/>
		/// <seealso cref="CriAtomEx.Config"/>
		public enum ThreadModel
		{
			/// <summary>マルチスレッド</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリは内部でスレッドを作成し、マルチスレッドにて動作します。
			/// スレッドは <see cref="CriAtomEx.Initialize"/> 関数呼び出し時に作成されます。
			/// ライブラリのサーバー処理は、作成されたスレッド上で定期的に実行されます。
			/// </para>
			/// </remarks>
			Multi = 0,
			/// <summary>マルチスレッド低遅延出力</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリは内部でスレッドを作成し、マルチスレッドにて動作します。
			/// スレッドは <see cref="CriAtomEx.Initialize"/> 関数呼び出し時に作成されます。
			/// ライブラリのサーバー処理は、作成されたスレッド上で定期的に実行されます。
			/// 並行して、サウンドシステムに連動したスレッド上でも音声のレンダリングが行われます。
			/// </para>
			/// <para>
			/// 備考:
			/// 本スレッドモデルは、一部のプラットフォームでしか使用できません。
			/// 本機能に未対応のプラットフォームでは、ライブラリ初期化時に
			/// <see cref="CriAtomEx.ThreadModel.MultiWithSonicsync"/>が指定された場合でも、
			/// <see cref="CriAtomEx.ThreadModel.Multi"/>指定時と同様の動作となります。
			/// </para>
			/// <para>
			/// 注意:
			/// ライブラリの処理は、複数のスレッドで分散して行われます。
			/// 本スレッドモデルを使用する場合、
			/// 処理負荷の計測には以下の2種類の関数を併用する必要があります。
			/// - <see cref="CriAtomEx.GetPerformanceInfo"/>
			/// - <see cref="CriAtomExAsrRack.GetPerformanceInfo"/>
			/// </para>
			/// </remarks>
			MultiWithSonicsync = 4,
			/// <summary>マルチスレッド（ユーザ駆動式）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリは内部でスレッドを作成し、マルチスレッドにて動作します。
			/// スレッドは <see cref="CriAtomEx.Initialize"/> 関数呼び出し時に作成されます。
			/// サーバー処理自体は作成されたスレッド上で実行されますが、
			/// <see cref="CriAtomEx.ThreadModel.Multi"/> とは異なり、自動的には実行されません。
			/// ユーザは <see cref="CriAtomEx.ExecuteMain"/> 関数で明示的にサーバー処理を駆動する必要があります。
			/// （  <see cref="CriAtomEx.ExecuteMain"/> 関数を実行すると、スレッドが起動し、サーバー処理が実行されます。）
			/// </para>
			/// </remarks>
			MultiUserDriven = 3,
			/// <summary>ユーザマルチスレッド</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリ内部ではスレッドを作成しませんが、ユーザが独自に作成したスレッド
			/// からサーバー処理関数を呼び出せるよう、内部の排他制御は行います。
			/// サーバー処理は <see cref="CriAtomEx.ExecuteMain"/> 関数内で同期実行されます。
			/// </para>
			/// </remarks>
			UserMulti = 1,
			/// <summary>シングルスレッド</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリ内部でスレッドを作成しません。また、内部の排他制御も行いません。
			/// サーバー処理は <see cref="CriAtomEx.ExecuteMain"/> 関数内で同期実行されます。
			/// </para>
			/// <para>
			/// 注意:
			/// このモデルを選択した場合、各APIとサーバー処理関数を同一スレッドから呼び出すようにしてください。
			/// </para>
			/// </remarks>
			Single = 2,
		}
		/// <summary>座標系</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリが3Dポジション計算を行う際、どの座標系を使用するかを表します。
		/// ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）に <see cref="CriAtomEx.Config"/>
		/// 構造体にて指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Initialize"/>
		/// <seealso cref="CriAtomEx.Config"/>
		public enum CoordinateSystem
		{
			/// <summary>左手座標系</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// xの正方向が右、yの正方向が上、zの正方向が奥となるような、左手デカルト座標系です。
			/// </para>
			/// </remarks>
			LeftHanded = 0,
			/// <summary>右手座標系</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// xの正方向が右、yの正方向が上、zの正方向が手前となるような、右手デカルト座標系です。
			/// </para>
			/// </remarks>
			RightHanded = 1,
		}
		/// <summary>疑似乱数生成器（Random Number Generator）インターフェース</summary>
		public unsafe partial struct RngInterface
		{
			/// <summary>ワーク領域サイズの計算</summary>
			/// <returns>ワーク領域サイズ</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 疑似乱数生成器を作成するために必要な、ワーク領域のサイズを取得します。
			/// </para>
			/// </remarks>
			public IntPtr calculateWorkSize;

			/// <summary>疑似乱数生成器の作成</summary>
			/// <returns>疑似乱数生成器オブジェクト</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 疑似乱数生成器を作成します。
			/// 疑似乱数生成器の作成に失敗した場合はnullを返します。
			/// </para>
			/// </remarks>
			public IntPtr create;

			/// <summary>疑似乱数生成器の破棄</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 疑似乱数生成器を破棄します。
			/// </para>
			/// </remarks>
			public IntPtr destroy;

			/// <summary>疑似乱数の生成</summary>
			/// <returns>疑似乱数</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 新しい疑似乱数を生成します。
			/// 生成された疑似乱数はmin以上max以下である必要があります。（min,maxは範囲に含む）
			/// </para>
			/// </remarks>
			public IntPtr generate;

			/// <summary>乱数種の設定</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 擬似乱数生成の元となる乱数種を設定します。
			/// </para>
			/// </remarks>
			public IntPtr setSeed;

		}
		/// <summary>ライブラリ初期化状態の取得</summary>
		/// <returns>初期化中かどうか</returns>
		/// <returns>未初期化状態</returns>
		/// <returns>初期化済み</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリが既に初期化されているかどうかをチェックします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Initialize"/>
		/// <seealso cref="CriAtomEx.Finalize"/>
		public static bool IsInitialized()
		{
			return NativeMethods.criAtomEx_IsInitialized();
		}

		/// <summary>サーバー処理の実行</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリの内部状態を更新します。
		/// アプリケーションは、この関数を定期的に実行する必要があります。
		/// サーバー処理を実行すべき回数は、ライブラリ初期化時のパラメーターに依存します。
		/// ライブラリ初期化時にスレッドモデルを <see cref="CriAtomEx.ThreadModel.Multi"/> に設定した場合、
		/// リアルタイム性の要求される処理は全てCRI Atomライブラリ内で定期的に自動実行されるため、
		/// 本関数の呼び出し頻度は少なくても問題は発生しません。
		/// （最低でも毎秒1回程度実行されていれば、音切れ等の問題が発生することはありません。）
		/// ライブラリ初期化時にスレッドモデルを <see cref="CriAtomEx.ThreadModel.Single"/> や
		/// <see cref="CriAtomEx.ThreadModel.UserMulti"/> に設定した場合、ファイルの読み込み管理や、
		/// データのデコード、音声の出力等、音声再生に必要な処理のほぼ全てが本関数内で実行されます。
		/// また、音声再生処理に同期して、CRI File Systemライブラリのファイルアクセスとデータ展開処理を実行します。
		/// そのため、ライブラリ初期化時に指定したサーバー処理の実行頻度（ <see cref="CriAtomEx.Config"/> 構造体の
		/// server_frequency ）を下回る頻度で本関数を実行した場合や、
		/// 大きいデータの読み込み、圧縮ファイルの読み込み等を行う場合、
		/// 音切れ等の問題が発生する可能性があるので注意してください。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリ初期化時にスレッドモデルを <see cref="CriAtomEx.ThreadModel.Multi"/> に設定した場合でも、
		/// 本関数を実行する必要があります。
		/// （スレッドモデルを <see cref="CriAtomEx.ThreadModel.Multi"/> の場合、ステータス更新等、ごく一部の
		/// 処理のみを行うため、本関数内で長時間処理がブロックされることはありません。）
		/// CRI File Systemライブラリのサーバー処理は、CRI Atomライブラリ内部で実行されます。
		/// そのため、本関数を実行している場合、アプリケーション側で別途CRI File Systemライブラリ
		/// のサーバー処理を呼び出す必要はありません。
		/// </para>
		/// </remarks>
		public static void ExecuteMain()
		{
			NativeMethods.criAtomEx_ExecuteMain();
		}

		/// <summary>ユーザーマルチスレッド用サーバー処理の実行</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリのみを更新します。
		/// スレッドモデルが<see cref="CriAtomEx.ThreadModel.UserMulti"/>の場合、
		/// アプリケーションは、この関数を定期的に実行する必要があります。
		/// ファイルの読み込み管理や、データのデコード、音声の出力等、
		/// 音声再生に必要な処理のほぼ全てが本関数内で実行されます。
		/// そのため、ライブラリ初期化時に指定したサーバー処理の実行頻度（ <see cref="CriAtomEx.Config"/> 構造体の
		/// server_frequency ）を下回る頻度で本関数を実行した場合、音切れ等の問題が発生する可能性
		/// があります。
		/// また、本関数は<see cref="CriAtomEx.ExecuteMain"/> 関数と異なり、CRI File Systemライブラリのサーバー処理を実行しません。
		/// アプリケーションが必要なサーバー処理を正しい順序で実行してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.ThreadModel.Single"/> に設定した場合、サーバー処理の排他制御が行われないので、
		/// 複数のスレッドから呼び出さないようにしてください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ExecuteMain"/>
		public static void ExecuteAudioProcess()
		{
			NativeMethods.criAtomEx_ExecuteAudioProcess();
		}

		/// <summary>サーバー処理の割り込みを防止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理の割り込み抑止します。
		/// 本関数実行後、<see cref="CriAtomEx.Unlock"/> 関数実行までの間、サーバー処理の動作が抑止されます。
		/// 複数のAPIを同一オーディオフレーム内で確実に実行したい場合には、本関数でサーバー処理の
		/// 割り込みを防止し、それらの関数を実行してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 割り込み防止区間で同時に複数のプレーヤーの再生をスタートした場合でも、
		/// 以下の要因により再生する波形がサンプル単位で同期しない場合があります。
		/// - ストリーム再生時のデータ供給に伴う発音遅延
		/// - 発音リソースの奪い取りに伴う発音遅延
		/// 再生同期を行いたい場合は、<see cref="CriAtomExPlayer.Prepare"/> 関数を使用して再生準備を行い、
		/// 準備完了後に割り込み防止区間で再生を開始してください。
		/// 本関数実行後、長時間<see cref="CriAtomEx.Unlock"/> 関数を呼ばない場合、音声再生が途切れる恐れがあります。
		/// サーバー処理の割り込みを防止する区間は、最小限に抑える必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Unlock"/>
		public static void Lock()
		{
			NativeMethods.criAtomEx_Lock();
		}

		/// <summary>サーバー処理の割り込み防止を解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomEx.Lock"/> 関数による、サーバー処理の割り込み防止を解除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Lock"/>
		public static void Unlock()
		{
			NativeMethods.criAtomEx_Unlock();
		}

		/// <summary>時刻の取得</summary>
		/// <returns>時刻（マイクロ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリ内のマスタタイマから時刻を取得します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ResetTimer"/>
		public static UInt64 GetTimeMicro()
		{
			return NativeMethods.criAtomEx_GetTimeMicro();
		}

		/// <summary>タイマのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリ内のマスタタイマの時刻をリセットします。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は <see cref="CriAtomEx.GetTimeMicro"/> 関数が返す値に対してのみ影響します。
		/// 本関数を実行しても、AtomExプレーヤーの再生時刻がクリアされることはありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ResetTimer"/>
		public static void ResetTimer()
		{
			NativeMethods.criAtomEx_ResetTimer();
		}

		/// <summary>タイマのポーズ</summary>
		/// <param name="sw">true=タイマ一時停止、false=タイマ再開</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリ内のマスタタイマを一時停止／再開します。
		/// マスタタイマを一時停止すると、シーケンス時刻が進行しなくなります。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は アプリケーションが休止したり一時停止するようなプラットフォームにおいて、
		/// 休止中や一時停止中でもタイマが進行してしまうプラットフォーム向けの機能です。
		/// アプリケーションが休止状態や一時停止状態に遷移する前に
		/// 本関数でマスタタイマを一時停止しておくことで、休止中のシーケンスの進行を止める事ができます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で一時停止する対象はあくまでAtomライブラリ内のマスタタイマです。
		/// 本関数では発音中のボイス等を一時停止できません。
		/// 本関数で設定したポーズフラグは、CRI Atomサーバー処理が実行されたタイミングで反映されます。
		/// 即座に同期をとる必要がある場合は、<see cref="CriAtomEx.ExecuteAudioProcess"/> 関数を呼び出す事で同期をとることができます。
		/// ただし、<see cref="CriAtomEx.ExecuteAudioProcess"/>を呼び出したスレッドでオーディオ処理が実行されるため、
		/// そのCPU負荷を許容できるかに注意してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ResetTimer"/>
		public static void PauseTimer(NativeBool sw)
		{
			NativeMethods.criAtomEx_PauseTimer(sw);
		}

		/// <summary>ワーク領域サイズ計算用コンフィグ構造体の設定</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ワーク領域サイズの計算用に、ライブラリ初期化用コンフィグ構造体
		/// （ <see cref="CriAtomEx.Config"/> 構造体）を仮登録します。
		/// ACFの登録やボイスプールの作成に必要なワーク領域のサイズは、
		/// ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に設定する構造体のパラメーターによって変化します。
		/// そのため、通常はACFの登録やボイスプールの作成に必要なワーク領域サイズを計算する前に、
		/// ライブラリを初期化する必要があります。
		/// 本関数を使用してライブラリ初期化用コンフィグ構造体を登録した場合、
		/// ACFの登録やボイスプールの作成に必要なワーク領域のサイズを、
		/// 初期化処理なしに計算可能になります。
		/// 本関数を実行することで、以下の処理が初期化処理なしに実行可能となります。
		/// - <see cref="CriAtomEx.CalculateWorkSizeForRegisterAcfData"/> 関数
		/// - ボイスプール作成用ワーク領域サイズの計算
		/// （ <see cref="CriAtomExVoicePool.CalculateWorkSizeForStandardVoicePool"/> 関数等）
		/// </para>
		/// <para>
		/// 備考:
		/// 引数（ config ）に null を指定した場合、デフォルト設定
		/// （ <see cref="CriAtomEx.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// 現状、本関数で一旦コンフィグ構造体を設定すると、
		/// 設定前の状態（未初期化状態でのワーク領域サイズ計算をエラーとする動作）
		/// に戻すことができなくなります。
		/// （関数を再度実行してパラメーターを上書きすることは可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で登録した初期化用コンフィグ構造体は、
		/// ライブラリ未初期化状態でのワーク領域サイズ計算にしか使用されません。
		/// ライブラリ初期化後には本関数に設定したパラメーターではなく、
		/// 初期化時に指定されたパラメーターがワーク領域サイズの計算に使用されます。
		/// （本関数で登録する構造体のパラメーターと、
		/// ライブラリの初期化に使用する構造体のパラメーターが異なる場合、
		/// ワーク領域サイズが不足し、オブジェクトの作成に失敗する恐れがあります。）
		/// <see cref="CriAtomEx.RegisterAcfFile"/> 関数や <see cref="CriAtomExAcb.LoadAcbFile"/> 関数等、
		/// ワーク領域計算時にファイルアクセスが必要になる API については、
		/// 本関数を実行した場合でもワーク領域サイズの計算が行えません。
		/// （ワーク領域サイズを計算するためにはライブラリを初期化する必要が
		/// あります。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeForRegisterAcfData"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForStandardVoicePool"/>
		public static unsafe void SetConfigForWorkSizeCalculation(in CriAtomEx.Config config)
		{
			fixed (CriAtomEx.Config* configPtr = &config)
				NativeMethods.criAtomEx_SetConfigForWorkSizeCalculation(configPtr);
		}

		/// <summary>オンメモリACFデータの登録に必要なワーク領域サイズの計算</summary>
		/// <param name="acfData">ACFデータアドレス</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomEx.RegisterAcfData"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomEx.RegisterAcfData"/> 関数でACF情報を登録する際には、
		/// 本関数が返すサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.RegisterAcfData"/>
		public static Int32 CalculateWorkSizeForRegisterAcfData(IntPtr acfData, Int32 acfDataSize)
		{
			return NativeMethods.criAtomEx_CalculateWorkSizeForRegisterAcfData(acfData, acfDataSize);
		}

		/// <summary>オンメモリACFデータの登録</summary>
		/// <param name="acfData">ACFデータアドレス</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>データの登録に成功したか</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ上に配置されたACFデータをライブラリに取り込みます。
		/// ACF情報の登録に必要なワーク領域のサイズは、
		/// <see cref="CriAtomEx.CalculateWorkSizeForRegisterAcfData"/> 関数で計算します。
		/// ACFファイルの登録に成功すると、本関数は戻り値として true を返します。
		/// データ不正などの理由等によりACFファイルの読み込みに失敗した場合、本関数は戻り値
		/// としてfalse を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数は、関数実行時に再生中の音声をすべて停止します。
		/// また、プレーヤーに設定した以下のACFに関連するパラメーターを全てリセットします。
		/// -# AISAC
		/// -# AISACコントロール値
		/// -# カテゴリ
		/// -# セレクターラベル
		/// -# バスセンド
		/// 本関数にセットしたデータ領域とワーク領域は、 <see cref="CriAtomEx.UnregisterAcf"/> 関数を実行するまでの間、
		/// アプリケーションで保持する必要があります。
		/// （ <see cref="CriAtomEx.UnregisterAcf"/> 関数実行前に、ワーク領域のメモリを解放しないでください。）
		/// また、データ領域の一部はワークとして使用されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.UnregisterAcf"/>
		public static bool RegisterAcfData(IntPtr acfData, Int32 acfDataSize, IntPtr work = default, Int32 workSize = default)
		{
			return NativeMethods.criAtomEx_RegisterAcfData(acfData, acfDataSize, work, workSize);
		}

		/// <summary>ACFファイルの登録に必要なワーク領域サイズの計算</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="path">ファイルパス</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomEx.RegisterAcfFile"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// 本関数は、 <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録の有無によって
		/// 計算に使用する要素が異なります。
		/// - アロケーター登録時：ACFファイルを一時的に読み込み、ACF内に記録されているカテゴリ数、
		/// 再生単位でのカテゴリ参照数、REACT数を使用したサイズ計算が行われます。
		/// - アロケーター未登録時：ライブラリ初期化時に指定した <see cref="CriAtomEx.Config"/> 構造体の
		/// max_categoriesメンバ、categories_per_playbackメンバを使用したサイズ計算が行われます。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.RegisterAcfFile"/>
		public static Int32 CalculateWorkSizeForRegisterAcfFile(CriFsBinder binder, ArgString path)
		{
			return NativeMethods.criAtomEx_CalculateWorkSizeForRegisterAcfFile(binder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]));
		}

		/// <summary>ACFファイルの登録に必要なワーク領域サイズの計算（CPKコンテンツID指定）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="id">CPKコンテンツID</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomEx.RegisterAcfFileById"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// ファイルパスの代わりにCPKコンテンツIDを指定する点を除けば、
		/// <see cref="CriAtomEx.CalculateWorkSizeForRegisterAcfFile"/> 関数と機能は同じです。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeForRegisterAcfFile"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFileById"/>
		public static Int32 CalculateWorkSizeForRegisterAcfFileById(CriFsBinder binder, UInt16 id)
		{
			return NativeMethods.criAtomEx_CalculateWorkSizeForRegisterAcfFileById(binder?.NativeHandle ?? default, id);
		}

		/// <summary>ACFファイルの登録</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="path">ファイルパス</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ファイル読み込み結果</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイルをロードし、ライブラリに取り込みます。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// ワーク領域を指定して本関数を使用する場合、 <see cref="CriAtomEx.CalculateWorkSizeForRegisterAcfFile"/> 関数
		/// を使用してワークサイズを計算してください。
		/// ACFファイルの登録に成功すると、本関数は戻り値として true を返します。
		/// リードエラー等によりACFファイルの読み込みに失敗した場合、本関数は戻り値として
		/// false を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数は、関数実行時に再生中の音声をすべて停止します。
		/// また、プレーヤーに設定した以下のACFに関連するパラメーターを全てリセットします。
		/// -# AISAC
		/// -# AISACコントロール値
		/// -# カテゴリ
		/// -# セレクターラベル
		/// -# バスセンド
		/// 本関数は、関数実行開始時に criFsLoader_Create 関数でローダーを確保し、
		/// 終了時に criFsLoader_Destroy 関数でローダーを破棄します。
		/// 本関数を実行する際には、空きローダーオブジェクトが１つ以上ある状態になるよう、
		/// ローダー数を調整してください。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomEx.CalculateWorkSizeForRegisterAcfFile"/> 関数によって計算したワークサイズ分の
		/// ワーク領域を指定した本関数の呼び出しでfalseが返された場合、ワーク領域不足が要因
		/// の可能性があります。
		/// ライブラリ初期化時に指定する <see cref="CriAtomEx.Config"/> 構造体の以下のメンバの設定値
		/// が適切であるか確認してください。
		/// - max_categories：ACF内のカテゴリ数、REACT数と同値以上
		/// - categories_per_playback：ACF内の再生単位でのカテゴリ参照数と同値以上
		/// 本関数にセットしたワーク領域は、 <see cref="CriAtomEx.UnregisterAcf"/> 関数を実行するまでの間、
		/// アプリケーションで保持する必要があります。
		/// （ <see cref="CriAtomEx.UnregisterAcf"/> 関数実行前に、ワーク領域のメモリを解放しないでください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.UnregisterAcf"/>
		public static bool RegisterAcfFile(CriFsBinder binder, ArgString path, IntPtr work = default, Int32 workSize = default)
		{
			return NativeMethods.criAtomEx_RegisterAcfFile(binder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]), work, workSize);
		}

		/// <summary>ACFファイルの登録（CPKコンテンツID指定）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="id">CPKコンテンツID</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ファイル読み込み結果</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイルをロードし、ライブラリに取り込みます。
		/// ファイルパスの代わりにCPKコンテンツIDを指定する点を除けば、
		/// <see cref="CriAtomEx.RegisterAcfFile"/> 関数と機能は同じです。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、関数実行時に再生中の音声をすべて停止します。
		/// また、プレーヤーに設定したパラメーターを全てリセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.RegisterAcfFile"/>
		public static bool RegisterAcfFileById(CriFsBinder binder, UInt16 id, IntPtr work = default, Int32 workSize = default)
		{
			return NativeMethods.criAtomEx_RegisterAcfFileById(binder?.NativeHandle ?? default, id, work, workSize);
		}

		/// <summary>ACFの登録解除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACF情報の登録を解除します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、関数実行時に再生中の音声をすべて停止します。
		/// また、プレーヤーに設定したパラメーターを全てリセットします。
		/// （ACFファイルが登録されてない際に、音声再生中に本関数を実行した場合は音声は停止されません）
		/// <see cref="CriAtomEx.RegisterAcfFile"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.RegisterAcfData"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFile"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFileById"/>
		public static void UnregisterAcf()
		{
			NativeMethods.criAtomEx_UnregisterAcf();
		}

		/// <summary>オンメモリACFのバージョン取得</summary>
		/// <param name="acfData">ACFデータアドレス</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <param name="flag">レジスト可能フラグ</param>
		/// <returns>ACFフォーマットバージョン</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ上に配置されたACFデータのフォーマットバージョンを取得します。
		/// また、flag引数にレジスト可能なバージョンかどうかをBool値で返します。
		/// </para>
		/// </remarks>
		public static unsafe UInt32 GetAcfVersion(IntPtr acfData, Int32 acfDataSize, out NativeBool flag)
		{
			fixed (NativeBool* flagPtr = &flag)
				return NativeMethods.criAtomEx_GetAcfVersion(acfData, acfDataSize, flagPtr);
		}

		/// <summary>ACFファイルのバージョン取得</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="path">ファイルパス</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <param name="flag">レジスト可能フラグ</param>
		/// <returns>ACFフォーマットバージョン</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイルをロードし、ACFデータのフォーマットバージョンを取得します。
		/// ACF情報の登録に必要なワーク領域のサイズは、
		/// <see cref="CriAtomEx.CalculateWorkSizeForRegisterAcfFile"/> 関数で計算します。
		/// ACFファイルフォーマットバージョンを元にflag引数にレジスト可能なバージョンかどうかをBool値で返します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、関数実行開始時に criFsLoader_Create 関数でローダーを確保し、
		/// 終了時に criFsLoader_Destroy 関数でローダーを破棄します。
		/// 本関数を実行する際には、空きローダーオブジェクトが１つ以上ある状態になるよう、
		/// ローダー数を調整してください。
		/// 本関数にセットしたワーク領域は、 アプリケーションで保持する必要はありません。
		/// （ロードしたデータは関数終了時に解放されます。）
		/// </para>
		/// </remarks>
		public static unsafe UInt32 GetAcfVersionFromFile(CriFsBinder binder, ArgString path, out NativeBool flag, IntPtr work = default, Int32 workSize = default)
		{
			fixed (NativeBool* flagPtr = &flag)
				return NativeMethods.criAtomEx_GetAcfVersionFromFile(binder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]), work, workSize, flagPtr);
		}

		/// <summary>ACFファイルのバージョン取得（CPKコンテンツID指定）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="id">CPKコンテンツID</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <param name="flag">レジスト可能フラグ</param>
		/// <returns>ACFフォーマットバージョン</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイルをロードし、ACFデータのフォーマットバージョンを取得します。
		/// ファイルパスの代わりにCPKコンテンツIDを指定する点を除けば、
		/// <see cref="CriAtomEx.GetAcfVersionFromFile"/> 関数と機能は同じです。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.GetAcfVersionFromFile"/>
		public static unsafe UInt32 GetAcfVersionFromFileById(CriFsBinder binder, UInt16 id, out NativeBool flag, IntPtr work = default, Int32 workSize = default)
		{
			fixed (NativeBool* flagPtr = &flag)
				return NativeMethods.criAtomEx_GetAcfVersionFromFileById(binder?.NativeHandle ?? default, id, work, workSize, flagPtr);
		}

		/// <summary>レジスト可能バージョン情報取得</summary>
		/// <param name="versionLow">レジスト可能下位バージョン</param>
		/// <param name="versionHigh">レジスト可能上位バージョン</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// レジスト可能なACFのバージョン情報を取得します。
		/// 上位バージョンはライブラリビルド時点での情報のため、この値より上位のACFでも
		/// レジスト可能な場合もあります。
		/// </para>
		/// </remarks>
		public static unsafe void GetSupportedAcfVersion(out UInt32 versionLow, out UInt32 versionHigh)
		{
			fixed (UInt32* versionLowPtr = &versionLow)
			fixed (UInt32* versionHighPtr = &versionHigh)
				NativeMethods.criAtomEx_GetSupportedAcfVersion(versionLowPtr, versionHighPtr);
		}

		/// <summary>オーディオヘッダーの解析</summary>
		/// <param name="buffer">オーディオデータを格納したバッファー</param>
		/// <param name="bufferSize">オーディオデータを格納したバッファーのサイズ</param>
		/// <param name="info">フォーマット情報</param>
		/// <returns>フォーマット情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリにロードされた音声データのフォーマットを解析します。
		/// 解析に成功すると、本関数は true を返し、音声データのフォーマット情報を
		/// 第3引数（ info ）に格納します。
		/// 解析に失敗した場合、本関数は false を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数の第1引数（ buffer ）には、オーディオデータのヘッダー領域
		/// （音声ファイルの先頭部分をロードしたもの）を格納しておく必要があります。
		/// 音声データの途中部分をセットした場合や、ヘッダー前に余計なデータが付加されている場合、
		/// ヘッダーの途中までしか格納されていない場合には、本関数はフォーマットの解析に失敗します。
		/// ADXデータやHCAデータについては、音声ファイルの先頭から2048バイト分の領域をセットすれば、
		/// フォーマットの解析に失敗することはありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 現状、本関数はADXデータとHCAデータの解析にしか対応していません。
		/// HCA-MXデータについては解析は可能ですが、ヘッダー情報からはHCAデータなのか
		/// HCA-MXデータなのかは区別できないため、フォーマット種別として
		/// <see cref="CriAtomEx.FormatHca"/> が返されます。
		/// </para>
		/// </remarks>
		public static unsafe bool AnalyzeAudioHeader(IntPtr buffer, Int32 bufferSize, out CriAtomEx.FormatInfo info)
		{
			fixed (CriAtomEx.FormatInfo* infoPtr = &info)
				return NativeMethods.criAtomEx_AnalyzeAudioHeader(buffer, bufferSize, infoPtr);
		}

		/// <summary>音声データフォーマット情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声データのフォーマット情報です。
		/// </para>
		/// <para>
		/// 備考:
		/// メモリ上に配置された音声データについては、 <see cref="CriAtomEx.AnalyzeAudioHeader"/>
		/// 関数を実行することで音声データのフォーマット情報を取得可能です。
		/// 再生中の音声データのフォーマットについては
		/// <see cref="CriAtomExPlayback.GetFormatInfo"/> 関数で取得可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AnalyzeAudioHeader"/>
		/// <seealso cref="CriAtomExPlayback.GetFormatInfo"/>
		public unsafe partial struct FormatInfo
		{
			/// <summary>フォーマット種別</summary>
			public UInt32 format;

			/// <summary>サンプリング周波数</summary>
			public Int32 samplingRate;

			/// <summary>総サンプル数</summary>
			public Int64 numSamples;

			/// <summary>ループ開始サンプル</summary>
			public Int64 loopOffset;

			/// <summary>ループ区間サンプル数</summary>
			public Int64 loopLength;

			/// <summary>チャンネル数</summary>
			public Int32 numChannels;

			/// <summary>予約領域</summary>
			public InlineArray1<UInt32> reserved;

		}
		/// <summary>乱数種の設定</summary>
		/// <param name="seed">乱数種</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリ全体で共有する疑似乱数生成器に乱数種を設定します。
		/// 乱数種を設定することにより、各種ランダム再生処理に再現性を持たせることができます。
		/// AtomExプレーヤーごとに再現性を持たせたい場合は、<see cref="CriAtomExPlayer.SetRandomSeed"/> 関数を使用してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetRandomSeed"/>
		public static void SetRandomSeed(UInt32 seed)
		{
			NativeMethods.criAtomEx_SetRandomSeed(seed);
		}

		/// <summary>ACBオブジェクトが即時解放可能かどうかのチェック</summary>
		/// <param name="buffer">バッファー</param>
		/// <param name="size">バッファーサイズ</param>
		/// <returns>再生中かどうか（true = 再生中のプレーヤーあり、false = 再生中のプレーヤーなし）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.SetData"/> 関数でセットしたバッファー領域が解放可能かどうかをチェックします。
		/// </para>
		/// <para>
		/// 備考:
		/// メモリ再生を行っているAtomExプレーヤーを全て停止させた場合でも、
		/// ライブラリ内には当該メモリ領域を参照しているボイスが存在する可能性があります。
		/// （ <see cref="CriAtomExPlayer.StopWithoutReleaseTime"/> 関数で停止処理を行った場合や、
		/// ボイスの奪い取りが発生した場合、AtomExプレーヤーからボイスは切り離されますが、
		/// ボイスが完全に停止するまでの間、データは参照される可能性があります。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、指定したデータ領域を参照しているAtomプレーヤーの存在を
		/// 検索する処理が動作します。
		/// そのため、本関数実行中に他スレッドでAtomプレーヤーの作成／破棄を行うと、
		/// アクセス違反やデッドロック等の重大な不具合を誘発する恐れがあります。
		/// 本関数実行時にAtomプレーヤーの作成／破棄を他スレッドで行う必要がある場合、
		/// Atomプレーヤーの作成／破棄を <see cref="CriAtomEx.Lock"/> 関数でロックしてから実行ください。
		/// <see cref="CriAtomExPlayer.SetData"/> 関数でセットしたバッファーを解放する際には、
		/// データをセットしたプレーヤーに対し停止処理を行った後、
		/// 本関数が false を返す状態になるまで待つ必要があります。
		/// 本関数が true を返すタイミングでバッファー領域を解放した場合、
		/// アクセス違反等の致命的な問題が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetData"/>
		public static bool IsDataPlaying(IntPtr buffer, Int32 size)
		{
			return NativeMethods.criAtomEx_IsDataPlaying(buffer, size);
		}

		/// <summary>DSPバス設定のアタッチ用ワークサイズの計算</summary>
		/// <param name="setting">DSPバス設定の名前</param>
		/// <returns>必要ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定からDSPバスを構築するのに必要なワーク領域サイズを計算します。
		/// 本関数を実行するには、あらかじめACF情報を登録しておく必要があります
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバス設定のアタッチに必要なワークメモリのサイズは、CRI Atom Craftで作成した
		/// DSPバス設定の内容によって変化します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomEx.RegisterAcfData"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFile"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFileById"/>
		public static Int32 CalculateWorkSizeForDspBusSetting(ArgString setting)
		{
			return NativeMethods.criAtomEx_CalculateWorkSizeForDspBusSetting(setting.GetPointer(stackalloc byte[setting.BufferSize]));
		}

		/// <summary>DSPバス設定のアタッチ用ワークサイズの計算</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfBufferSize">ACFデータサイズ</param>
		/// <param name="settingName">DSPバス設定の名前</param>
		/// <returns>必要ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定からDSPバスを構築するのに必要なワーク領域サイズを計算します。
		/// <see cref="CriAtomEx.CalculateWorkSizeForDspBusSetting"/> 関数と違い、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// （ただし、ACFデータを事前にメモリにロードし、
		/// <see cref="CriAtomExAsr.SetConfigForWorkSizeCalculation"/>
		/// 関数でASR初期化用コンフィグ構造体を仮登録しておく必要があります。）
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、
		/// エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバス設定のアタッチに必要なワークメモリのサイズは、CRI Atom Craftで作成した
		/// DSPバス設定の内容によって変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// ハードウェアDSPを使用するプラットフォーム（ASRを使用しないプラットフォーム）では、
		/// 本関数でワーク領域サイズを取得することができない可能性があります。
		/// （本関数実行時にエラーコールバックが発生したり、負値が返される可能性があります。）
		/// 本関数が動作しないプラットフォームについては、
		/// ライブラリの初期化後に <see cref="CriAtomEx.CalculateWorkSizeForDspBusSetting"/>
		/// 関数を使用して必要なワーク領域サイズを計算してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		/// <seealso cref="CriAtomEx.RegisterAcfData"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFile"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFileById"/>
		/// <seealso cref="CriAtomExAsr.SetConfigForWorkSizeCalculation"/>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeForDspBusSetting"/>
		public static Int32 CalculateWorkSizeForDspBusSettingFromAcfData(IntPtr acfData, Int32 acfBufferSize, ArgString settingName)
		{
			return NativeMethods.criAtomEx_CalculateWorkSizeForDspBusSettingFromAcfData(acfData, acfBufferSize, settingName.GetPointer(stackalloc byte[settingName.BufferSize]));
		}

		/// <summary>DSPバス設定のアタッチ</summary>
		/// <param name="setting">DSPバス設定の名前</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定からDSPバスを構築してサウンドレンダラにアタッチします。
		/// 本関数を実行するには、あらかじめACF情報を登録しておく必要があります。
		/// </para>
		/// <para>
		/// 備考:
		/// DSPバス設定のアタッチに必要なワークメモリのサイズは、
		/// CRI Atom Craftで作成したDSPバス設定の内容によって変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		/// 音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、
		/// 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.DetachDspBusSetting"/>
		/// <seealso cref="CriAtomEx.RegisterAcfData"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFile"/>
		/// <seealso cref="CriAtomEx.RegisterAcfFileById"/>
		public static void AttachDspBusSetting(ArgString setting, IntPtr work = default, Int32 workSize = default)
		{
			NativeMethods.criAtomEx_AttachDspBusSetting(setting.GetPointer(stackalloc byte[setting.BufferSize]), work, workSize);
		}

		/// <summary>DSPバス設定のデタッチ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定をデタッチします。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// DSPバス設定アタッチ時に確保されたメモリ領域が解放されます。
		/// （DSPバス設定アタッチ時にワーク領域を渡した場合、本関数実行後であれば
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
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		public static void DetachDspBusSetting()
		{
			NativeMethods.criAtomEx_DetachDspBusSetting();
		}

		/// <summary>DSPバススナップショットの適用</summary>
		/// <param name="snapshotName">スナップショット名</param>
		/// <param name="timeMs">時間（ミリ秒）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバススナップショットを適用します。
		/// 本関数を呼び出すと、スナップショットで設定したパラメーターに time_ms 掛けて変化します。
		/// 引数 snapshot_name に CRI_NULL を指定すると、元のDSPバス設定の状態（スナップショットが適用されていない状態）に戻ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AttachDspBusSetting"/>
		public static void ApplyDspBusSnapshot(ArgString snapshotName, Int32 timeMs)
		{
			NativeMethods.criAtomEx_ApplyDspBusSnapshot(snapshotName.GetPointer(stackalloc byte[snapshotName.BufferSize]), timeMs);
		}

		/// <summary>適用中のDSPバススナップショット名の取得</summary>
		/// <returns>
		/// CriChar8*	スナップショット名文字列へのポインタ。
		/// スナップショットが適用されていない場合や取得に失敗した場合はCRI_NULLが返ります。
		/// </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 適用中のDSPバススナップショット名を取得します。
		/// スナップショットが適用されていない場合はCRI_NULLが返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ApplyDspBusSnapshot"/>
		public static NativeString GetAppliedDspBusSnapshotName()
		{
			return NativeMethods.criAtomEx_GetAppliedDspBusSnapshotName();
		}

		/// <summary>キューリンクコールバック関数の登録</summary>
		/// <param name="func">キューリンクコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー再生時にキューリンクを処理した際に、キューリンク情報を受け取るコールバック関数を登録します。
		/// </para>
		/// <para>
		/// 注意:
		/// 登録されたコールバック関数は、ライブラリ内でキューリンクを処理したタイミングで実行されます。
		/// そのため、ライブラリ処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.CueLinkCbFunc"/>
		public static unsafe void SetCueLinkCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.CueLinkInfo*, Int32> func, IntPtr obj)
		{
			NativeMethods.criAtomEx_SetCueLinkCallback((IntPtr)func, obj);
		}
		static unsafe void SetCueLinkCallbackInternal(IntPtr func, IntPtr obj) => SetCueLinkCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.CueLinkInfo*, Int32>)func, obj);
		static CriAtomEx.CueLinkCbFunc _cueLinkCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetCueLinkCallback" />
		public static CriAtomEx.CueLinkCbFunc CueLinkCallback => _cueLinkCallback ?? (_cueLinkCallback = new CriAtomEx.CueLinkCbFunc(SetCueLinkCallbackInternal));

		/// <summary>キューリンクコールバック</summary>
		/// <returns>
		/// 
		/// AtomExライブラリのキューリンクコールバック関数型です。
		/// コールバック関数の登録には <see cref="CriAtomEx.SetCueLinkCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、ライブラリ内でキューリンクが処理されるタイミングで実行されます。
		/// そのため、ライブラリ処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </returns>
		/// <remarks>
		/// <para>説明:</para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetCueLinkCallback"/>
		public unsafe class CueLinkCbFunc : NativeCallbackBase<CueLinkCbFunc.Arg, Int32>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>キューリンク情報</summary>
				public NativeReference<CriAtomEx.CueLinkInfo> info { get; }

				internal Arg(NativeReference<CriAtomEx.CueLinkInfo> info)
				{
					this.info = info;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static Int32 CallbackFunc(IntPtr obj, CriAtomEx.CueLinkInfo* info) =>
				InvokeCallbackInternal(obj, new(info));
#if !NET5_0_OR_GREATER
			delegate Int32 NativeDelegate(IntPtr obj, CriAtomEx.CueLinkInfo* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal CueLinkCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomEx.CueLinkInfo*, Int32>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>キューリンクコールバック用Info構造体</summary>
		public unsafe partial struct CueLinkInfo
		{
			/// <summary>プレーヤーオブジェクト</summary>
			public IntPtr player;

			/// <summary>リンク元再生ID</summary>
			public UInt32 baseId;

			/// <summary>リンク元キュー</summary>
			public CriAtomEx.SourceInfo baseCue;

			/// <summary>リンク先再生ID</summary>
			public UInt32 targetId;

			/// <summary>リンク先キュー</summary>
			public CriAtomEx.SourceInfo targetCue;

			/// <summary>リンクタイプ</summary>
			public CriAtomEx.CueLinkType linkType;

		}
		/// <summary>再生元の情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生するまたは再生中の音声の、再生元（何を再生する／している）の情報です。
		/// <see cref="CriAtomExPlayback.GetSource"/> 関数で取得可能です。
		/// 取得した情報を元に、<see cref="CriAtomExAcb.GetCueInfoByIndex"/> 関数等を利用することで、
		/// より詳細な情報を取得することができます。
		/// </para>
		/// <para>
		/// 備考
		/// 再生元のタイプによって、取得できる情報が異なります。
		/// typeを参照し、共用体sourceの中のどの構造体としてアクセスするかを選択してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.GetSource"/>
		/// <seealso cref="CriAtomExAcb.GetCueInfoByIndex"/>
		public unsafe partial struct SourceInfo
		{
			/// <summary>再生元のタイプ</summary>
			public CriAtomEx.SourceType type;

			/// <summary>再生元情報共用体</summary>
			public CriAtomEx.SourceInfoInfoTag info;

		}
		/// <summary>再生元のタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する、またはAtomExプレーヤーで再生中の音声の、再生元のタイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SourceInfo"/>
		public enum SourceType
		{
			/// <summary>未設定</summary>
			None = 0,
			/// <summary>キューID</summary>
			CueId = 1,
			/// <summary>キュー名</summary>
			CueName = 2,
			/// <summary>キューインデックス</summary>
			CueIndex = 3,
			/// <summary>オンメモリデータ</summary>
			Data = 4,
			/// <summary>ファイル名</summary>
			File = 5,
			/// <summary>CPKコンテンツID</summary>
			ContentId = 6,
			/// <summary>音声データID</summary>
			WaveId = 7,
			/// <summary>振動ID</summary>
			VibrationId = 8,
			/// <summary>サウンドジェネレータID</summary>
			SoundGeneratorId = 9,
			/// <summary>RawPcmFloatID</summary>
			RawPcmFloatId = 10,
			/// <summary>入力ポート</summary>
			InputPort = 11,
		}
		/// <summary>再生元情報共用体</summary>
		[StructLayout(LayoutKind.Explicit)]
		public unsafe partial struct SourceInfoInfoTag
		{
			/// <summary>キューID情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoCueIdTag cueId;

			/// <summary>キュー名情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoCueNameTag cueName;

			/// <summary>キューインデックス情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoCueIndexTag cueIndex;

			/// <summary>オンメモリデータ情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoDataTag data;

			/// <summary>ファイル情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoFileTag file;

			/// <summary>CPKコンテンツID情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoContentIdTag contentId;

			/// <summary>波形データID情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoWaveIdTag waveId;

			/// <summary>振動ID情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoVibrationIdTag vibrationId;

			[FieldOffset(0)] public CriAtomEx.SourceInfoVibrationNameTag vibrationName;

			/// <summary>サウンドジェネレータID情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoSoundGeneratorParameterTag soundGeneratorParameter;

			/// <summary>RawPCM FloatID情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoRawPcmFloatParameterTag rawPcmFloatParameter;

			/// <summary>入力ポート情報</summary>
			[FieldOffset(0)] public CriAtomEx.SourceInfoInputPortTag inputPort;

		}
		/// <summary>キューID情報</summary>
		public unsafe partial struct SourceInfoCueIdTag
		{
			/// <summary>ACBオブジェクト</summary>
			public IntPtr acb;

			/// <summary>キューID</summary>
			public Int32 id;

		}
		/// <summary>キュー名情報</summary>
		public unsafe partial struct SourceInfoCueNameTag
		{
			/// <summary>ACBオブジェクト</summary>
			public IntPtr acb;

			/// <summary>キュー名</summary>
			public NativeString name;

		}
		/// <summary>キューインデックス情報</summary>
		public unsafe partial struct SourceInfoCueIndexTag
		{
			/// <summary>ACBオブジェクト</summary>
			public IntPtr acb;

			/// <summary>キューインデックス</summary>
			public Int32 index;

		}
		/// <summary>オンメモリデータ情報</summary>
		public unsafe partial struct SourceInfoDataTag
		{
			/// <summary>メモリアドレス</summary>
			public IntPtr buffer;

			/// <summary>サイズ</summary>
			public Int32 size;

		}
		/// <summary>ファイル情報</summary>
		public unsafe partial struct SourceInfoFileTag
		{
			/// <summary>バインダーオブジェクト</summary>
			public IntPtr binder;

			/// <summary>ファイルパス</summary>
			public NativeString path;

		}
		/// <summary>CPKコンテンツID情報</summary>
		public unsafe partial struct SourceInfoContentIdTag
		{
			/// <summary>バインダーオブジェクト</summary>
			public IntPtr binder;

			/// <summary>コンテンツID</summary>
			public Int32 id;

		}
		/// <summary>波形データID情報</summary>
		public unsafe partial struct SourceInfoWaveIdTag
		{
			/// <summary>AWBオブジェクト</summary>
			public IntPtr awb;

			/// <summary>波形データID</summary>
			public Int32 id;

		}
		/// <summary>振動ID情報</summary>
		public unsafe partial struct SourceInfoVibrationIdTag
		{
			/// <summary>振動データID</summary>
			public Int32 id;

		}

		public unsafe partial struct SourceInfoVibrationNameTag
		{
			/// <summary>振動データ名</summary>
			public NativeString name;

		}
		/// <summary>サウンドジェネレータID情報</summary>
		public unsafe partial struct SourceInfoSoundGeneratorParameterTag
		{
			/// <summary>周波数</summary>
			public Single frequency;

			/// <summary>波形</summary>
			public Int32 waveType;

		}
		/// <summary>RawPCM FloatID情報</summary>
		public unsafe partial struct SourceInfoRawPcmFloatParameterTag
		{
			/// <summary>データアドレス</summary>
			public NativeReference<Single> data;

			/// <summary>総サンプル数</summary>
			public UInt32 totalSamples;

		}
		/// <summary>入力ポート情報</summary>
		public unsafe partial struct SourceInfoInputPortTag
		{
			/// <summary>入力ポートタイプ</summary>
			public CriAtomExInputPort.Type type;

			/// <summary>入力ポートオブジェクト</summary>
			public IntPtr port;

		}
		/// <summary>キューリンクコールバックタイプ</summary>
		public enum CueLinkType
		{
			/// <summary>静的リンク</summary>
			Static = 0,
			/// <summary>動的リンク</summary>
			Dynamic = 1,
		}
		/// <summary>5.1chスピーカー角度の設定</summary>
		/// <param name="angleL">フロントレフトスピーカーの角度</param>
		/// <param name="angleR">フロントライトスピーカーの角度</param>
		/// <param name="angleSl">サラウンドレフトスピーカーの角度</param>
		/// <param name="angleSr">サラウンドライトスピーカーの角度</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パン3Dや3Dポジショニングの計算時に使用する、出力スピーカーの角度（配置）を設定します。
		/// 角度は、正面方向を0度として-180度から180度の間で設定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は5.1ch向けのパンスピーカータイプ (4CH または 5CH) にのみ影響します。
		/// 7.1ch向けのパンスピーカータイプ (6CH または 7CH) のスピーカー角度を変更する場合は、<see cref="CriAtomEx.SetSpeakerAngleArray"/> 関数を使用してください。
		/// 設定するスピーカー角度は、angle_sl &lt; angle_l &lt; angle_r &lt; angle_sr の順となるような配置にする必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetSpeakerAngleArray"/>
		public static void SetSpeakerAngles(Single angleL, Single angleR, Single angleSl, Single angleSr)
		{
			NativeMethods.criAtomEx_SetSpeakerAngles(angleL, angleR, angleSl, angleSr);
		}

		/// <summary>スピーカー角度の設定</summary>
		/// <param name="speakerSystem">出力スピーカーの並び順</param>
		/// <param name="angleArray">出力スピーカーの角度配列</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パン3Dや3Dポジショニングの計算時に使用する、出力スピーカーの角度（配置）を設定します。
		/// 角度は、正面方向を0度として-180度から180度の間で設定してください。
		/// 角度配列は、出力スピーカーの並び順のスピーカー数以上の要素数の配列を指定してください。
		/// 角度配列に null を指定すると、出力スピーカーの並び順に合わせて、デフォルトの角度を設定します。
		/// </para>
		/// <para>
		/// 注意:
		/// FRONT LEFTとFRONT RIGHTの位置を入れ替えるような設定をした場合、意図しない挙動になる可能性があります。
		/// </para>
		/// <para>
		/// 補足:
		/// LOW FREQUENCYの角度を変更しても、パン3Dや3Dポジショニングの計算結果は変化しません。
		/// 設定した角度は、各スピーカーシステムごとに独立して設定されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetSpeakerAngles"/>
		public static unsafe void SetSpeakerAngleArray(UInt32 speakerSystem, in Single angleArray)
		{
			fixed (Single* angleArrayPtr = &angleArray)
				NativeMethods.criAtomEx_SetSpeakerAngleArray(speakerSystem, angleArrayPtr);
		}

		/// <summary>バーチャルスピーカー角度の設定</summary>
		/// <param name="speakerSystem">バーチャルスピーカーの並び順</param>
		/// <param name="angleArray">バーチャルスピーカーの角度配列</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バーチャルスピーカーにおけるパン3Dや3Dポジショニングの計算時に使用する、
		/// 出力スピーカーの角度（配置）を設定します。
		/// 本関数の操作は <see cref="CriAtomEx.SetSpeakerAngleArray"/> 関数と同様なため、基本的な説明はそちらを参照して下さい。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数での設定は <see cref="CriAtomEx.ControlVirtualSpeakerSetting"/> 関数にてバーチャルスピーカー設定を有効にしない限り、
		/// 設定したバーチャルスピーカー角度はパン3Dや3Dポジショニングの計算に反映されません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetSpeakerAngleArray"/>
		/// <seealso cref="CriAtomEx.ControlVirtualSpeakerSetting"/>
		public static unsafe void SetVirtualSpeakerAngleArray(UInt32 speakerSystem, in Single angleArray)
		{
			fixed (Single* angleArrayPtr = &angleArray)
				NativeMethods.criAtomEx_SetVirtualSpeakerAngleArray(speakerSystem, angleArrayPtr);
		}

		/// <summary>バーチャルスピーカー設定のON/OFF</summary>
		/// <param name="sw">スイッチ（false = 無効、true = 有効）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パン3Dや3Dポジショニングの計算時にバーチャルスピーカーの設定を使用する機能のON/OFFを設定します。
		/// この設定を有効にすると、マルチチャンネルサウンドは <see cref="CriAtomEx.SetVirtualSpeakerAngleArray"/> 関数にて設定した
		/// バーチャルスピーカー角度からそれぞれ再生されます。
		/// </para>
		/// <para>
		/// 注意:
		/// デフォルトの状態は「無効」になっています。
		/// また、何かボイスを再生中に「有効」にした場合、パン3Dや3Dポジショニングの計算には即時反映されません。
		/// 次回ボイス再生時から反映されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetVirtualSpeakerAngleArray"/>
		public static void ControlVirtualSpeakerSetting(NativeBool sw)
		{
			NativeMethods.criAtomEx_ControlVirtualSpeakerSetting(sw);
		}

		/// <summary>ゲーム変数の総数の取得</summary>
		/// <returns>ゲーム変数の総数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイル内に登録されているゲーム変数の総数を取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ACFファイルを登録しておく必要があります。
		/// ACFファイルが登録されていない場合、-1が返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.GetGameVariableInfo"/>
		public static Int32 GetNumGameVariables()
		{
			return NativeMethods.criAtomEx_GetNumGameVariables();
		}

		/// <summary>ゲーム変数情報の取得（インデックス指定）</summary>
		/// <param name="index">ゲーム変数インデックス</param>
		/// <param name="info">ゲーム変数情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ゲーム変数インデックスからゲーム変数情報を取得します。
		/// 指定したインデックスのゲーム変数が存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.GameVariableInfo"/>
		public static unsafe bool GetGameVariableInfo(UInt16 index, out CriAtomEx.GameVariableInfo info)
		{
			fixed (CriAtomEx.GameVariableInfo* infoPtr = &info)
				return NativeMethods.criAtomEx_GetGameVariableInfo(index, infoPtr);
		}

		/// <summary>ゲーム変数情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ゲーム変数情報を取得するための構造体です。
		/// <see cref="CriAtomEx.GameVariableInfo"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.GetGameVariableInfo"/>
		public unsafe partial struct GameVariableInfo
		{
			/// <summary>ゲーム変数名</summary>
			public NativeString name;

			/// <summary>ゲーム変数ID</summary>
			public UInt32 id;

			/// <summary>ゲーム変数値</summary>
			public Single value;

		}
		/// <summary>ゲーム変数の取得</summary>
		/// <param name="id">ゲーム変数ID</param>
		/// <returns>ゲーム変数値</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイル内に登録されているゲーム変数値を取得します。
		/// 指定した id のゲーム変数が存在しない場合、-1.0f が返ります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ACFファイルを登録しておく必要があります。
		/// </para>
		/// </remarks>
		public static Single GetGameVariableById(UInt32 id)
		{
			return NativeMethods.criAtomEx_GetGameVariableById(id);
		}

		/// <summary>ゲーム変数の取得</summary>
		/// <param name="name">ゲーム変数名</param>
		/// <returns>ゲーム変数値</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイル内に登録されているゲーム変数を取得します。
		/// 指定した名前のゲーム変数が存在しない場合、-1.0f が返ります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ACFファイルを登録しておく必要があります。
		/// </para>
		/// </remarks>
		public static Single GetGameVariableByName(ArgString name)
		{
			return NativeMethods.criAtomEx_GetGameVariableByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ゲーム変数の設定</summary>
		/// <param name="id">ゲーム変数ID</param>
		/// <param name="value">ゲーム変数値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイル内に登録されているゲーム変数に値を設定します。
		/// 設定可能な範囲は0.0f～1.0fの間です。
		/// </para>
		/// <para>
		/// 備考:
		/// ゲーム変数の値は以下のサウンド制御において参照されます。
		/// - スイッチキューによる再生トラックの切り替え
		/// - AISAC によるパラメーターの制御
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ACFファイルを登録しておく必要があります。
		/// ゲーム変数の値に同じ値を設定した際は AISAC のパラメーター更新の処理は発生しません。
		/// </para>
		/// </remarks>
		public static void SetGameVariableById(UInt32 id, Single value)
		{
			NativeMethods.criAtomEx_SetGameVariableById(id, value);
		}

		/// <summary>ゲーム変数の設定</summary>
		/// <param name="name">ゲーム変数名</param>
		/// <param name="value">ゲーム変数値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイル内に登録されているゲーム変数に値を設定します。
		/// 設定可能な範囲は0.0f～1.0fの間です。
		/// </para>
		/// <para>
		/// 備考:
		/// ゲーム変数の値は以下のサウンド制御において参照されます。
		/// - スイッチキューによる再生トラックの切り替え
		/// - AISAC によるパラメーターの制御
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ACFファイルを登録しておく必要があります。
		/// ゲーム変数の値に同じ値を設定した際は AISAC のパラメーター更新の処理は発生しません。
		/// </para>
		/// </remarks>
		public static void SetGameVariableByName(ArgString name, Single value)
		{
			NativeMethods.criAtomEx_SetGameVariableByName(name.GetPointer(stackalloc byte[name.BufferSize]), value);
		}

		/// <summary>プレイバックキャンセルコールバック関数の登録</summary>
		/// <param name="func">プレイバックキャンセルコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー再生時に再生開始処理のキャンセルが発生した際に、プレイバックキャンセル情報を受け取るコールバック関数を登録します。
		/// </para>
		/// <para>
		/// 注意:
		/// 登録されたコールバック関数は、ライブラリ内で再生開始処理がキャンセルされるタイミングで実行されます。
		/// そのため、ライブラリ処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.CancelCbFunc"/>
		public static unsafe void SetPlaybackCancelCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.CancelInfo*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomEx_SetPlaybackCancelCallback((IntPtr)func, obj);
		}
		static unsafe void SetPlaybackCancelCallbackInternal(IntPtr func, IntPtr obj) => SetPlaybackCancelCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.CancelInfo*, void>)func, obj);
		static CriAtomExPlayback.CancelCbFunc _playbackCancelCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetPlaybackCancelCallback" />
		public static CriAtomExPlayback.CancelCbFunc PlaybackCancelCallback => _playbackCancelCallback ?? (_playbackCancelCallback = new CriAtomExPlayback.CancelCbFunc(SetPlaybackCancelCallbackInternal));

		/// <summary>ACF整合性チェック機能のON/OFF</summary>
		/// <param name="sw">スイッチ（false = チェック無効、true = チェック有効）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBロード時のACFとの整合性チェック機能のON/OFFを設定します。
		/// </para>
		/// <para>
		/// 注意:
		/// デフォルトの状態は「チェック有効」になっています。「チェック無効」に設定した場合に、
		/// 整合性がない組み合わせのデータを使用すると、本来目的とする効果が得られません。
		/// また、「チェック無効」にした場合でも、音声処理実行時にACBから参照しているACF項目が
		/// 見つからないときには別途エラーコールバックが発生します。
		/// </para>
		/// </remarks>
		public static void ControlAcfConsistencyCheck(NativeBool sw)
		{
			NativeMethods.criAtomEx_ControlAcfConsistencyCheck(sw);
		}

		/// <summary>ACF整合性チェックエラーレベルの設定</summary>
		/// <param name="level">エラーレベル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBロード時のACFとの整合性チェックで発生するエラーの通知レベルを設定します。
		/// デフォルト状態での通知レベルは <see cref="CriErr.Level.Warning"/> です。
		/// </para>
		/// </remarks>
		public static void SetAcfConsistencyCheckErrorLevel(CriErr.Level level)
		{
			NativeMethods.criAtomEx_SetAcfConsistencyCheckErrorLevel(level);
		}

		/// <summary>トラックトランジションバイセレクターコールバック関数の登録</summary>
		/// <param name="func">トラックトランジションバイセレクターコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トラックトランジションバイセレクタータイプキューの再生時にトランジション処理を行った際の情報を受け取るコールバック関数を登録します。
		/// </para>
		/// <para>
		/// 注意:
		/// 登録されたコールバック関数は、ライブラリ内でトランジション処理が開始されるタイミングで実行されます。
		/// そのため、ライブラリ処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.TrackTransitionBySelectorCbFunc"/>
		public static unsafe void SetTrackTransitionBySelectorCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.TrackTransitionBySelectorInfo*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomEx_SetTrackTransitionBySelectorCallback((IntPtr)func, obj);
		}
		static unsafe void SetTrackTransitionBySelectorCallbackInternal(IntPtr func, IntPtr obj) => SetTrackTransitionBySelectorCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.TrackTransitionBySelectorInfo*, void>)func, obj);
		static CriAtomEx.TrackTransitionBySelectorCbFunc _trackTransitionBySelectorCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetTrackTransitionBySelectorCallback" />
		public static CriAtomEx.TrackTransitionBySelectorCbFunc TrackTransitionBySelectorCallback => _trackTransitionBySelectorCallback ?? (_trackTransitionBySelectorCallback = new CriAtomEx.TrackTransitionBySelectorCbFunc(SetTrackTransitionBySelectorCallbackInternal));


		public unsafe class TrackTransitionBySelectorCbFunc : NativeCallbackBase<TrackTransitionBySelectorCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				public NativeReference<CriAtomEx.TrackTransitionBySelectorInfo> info { get; }

				internal Arg(NativeReference<CriAtomEx.TrackTransitionBySelectorInfo> info)
				{
					this.info = info;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtomEx.TrackTransitionBySelectorInfo* info) =>
				InvokeCallbackInternal(obj, new(info));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomEx.TrackTransitionBySelectorInfo* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal TrackTransitionBySelectorCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomEx.TrackTransitionBySelectorInfo*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>トラックトランジションバイセレクターコールバック用Info構造体</summary>
		public unsafe partial struct TrackTransitionBySelectorInfo
		{
			/// <summary>プレーヤーオブジェクト</summary>
			public IntPtr player;

			/// <summary>再生ID</summary>
			public UInt32 id;

			/// <summary>セレクター名</summary>
			public NativeString selector;

			/// <summary>ラベル名</summary>
			public NativeString label;

		}
		/// <summary>各種リソースの使用状況</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 各種リソースの使用状況を表わす構造体です。
		/// </para>
		/// </remarks>
		public unsafe partial struct ResourceUsage
		{
			/// <summary>対象リソースの現在の使用数</summary>
			public UInt32 useCount;

			/// <summary>対象リソースの制限数</summary>
			public UInt32 limit;

		}
		/// <summary>AISACコントロール情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AISACコントロール情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetAisacControlInfo"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetAisacControlInfo"/>
		public unsafe partial struct AisacControlInfo
		{
			/// <summary>AISACコントロール名</summary>
			public NativeString name;

			/// <summary>AISACコントロールID</summary>
			public UInt32 id;

		}
		/// <summary>Global Aisac情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Global Aisac情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetGlobalAisacInfo"/> 関数に引数として渡します。
		/// </para>
		/// <para>
		/// 注意:
		/// typeが<see cref="CriAtomExAcf.AisacType.AutoModulation"/> の場合、
		/// control_idは内部的に使用されるインデックス値となります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetGlobalAisacInfo"/>
		public unsafe partial struct GlobalAisacInfo
		{
			/// <summary>Global Aisac名</summary>
			public NativeString name;

			/// <summary>データインデックス</summary>
			public UInt16 index;

			/// <summary>グラフ数</summary>
			public UInt16 numGraphs;

			/// <summary>Aisacタイプ</summary>
			public CriAtomExAcf.AisacType type;

			/// <summary>ランダムレンジ</summary>
			public Single randomRange;

			/// <summary>Control Id</summary>
			public UInt16 controlId;

			/// <summary>未使用</summary>
			public UInt16 dummy;

		}
		/// <summary>Aisac Graph情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Global Aisac Graph情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetGlobalAisacGraphInfo"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetGlobalAisacGraphInfo"/>
		public unsafe partial struct AisacGraphInfo
		{
			/// <summary>Graphタイプ</summary>
			public CriAtomEx.AisacGraphType type;

		}
		/// <summary>Aisacグラフタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Aisacグラフのタイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.AisacGraphInfo"/>
		public enum AisacGraphType
		{
			/// <summary>未使用</summary>
			Non = 0,
			/// <summary>ボリューム</summary>
			Volume = 1,
			/// <summary>ピッチ</summary>
			Pitch = 2,
			/// <summary>バンドパスフィルターの高域カットオフ周波数</summary>
			BandpassHi = 3,
			/// <summary>バンドパスフィルターの低域カットオフ周波数</summary>
			BandpassLow = 4,
			/// <summary>バイクアッドフィルターの周波数</summary>
			BiquadFreq = 5,
			/// <summary>バイクアッドフィルターのQ値</summary>
			BiquadQ = 6,
			/// <summary>バスセンドレベル0</summary>
			Bus0Send = 7,
			/// <summary>バスセンドレベル1</summary>
			Bus1Send = 8,
			/// <summary>バスセンドレベル2</summary>
			Bus2Send = 9,
			/// <summary>バスセンドレベル3</summary>
			Bus3Send = 10,
			/// <summary>バスセンドレベル4</summary>
			Bus4Send = 11,
			/// <summary>バスセンドレベル5</summary>
			Bus5Send = 12,
			/// <summary>バスセンドレベル6</summary>
			Bus6Send = 13,
			/// <summary>バスセンドレベル7</summary>
			Bus7Send = 14,
			/// <summary>パンニング3D角度</summary>
			Pan3dAngle = 15,
			/// <summary>パンニング3Dボリューム</summary>
			Pan3dVolume = 16,
			/// <summary>パンニング3D距離</summary>
			Pan3dInteriorDistance = 17,
			/// <summary>ACB Ver.0.11.00以降では使用しない</summary>
			Pan3dCenter = 18,
			/// <summary>ACB Ver.0.11.00以降では使用しない</summary>
			Pan3dLfe = 19,
			/// <summary>AISACコントロールID 0</summary>
			Aisac0 = 20,
			/// <summary>AISACコントロールID 1</summary>
			Aisac1 = 21,
			/// <summary>AISACコントロールID 2</summary>
			Aisac2 = 22,
			/// <summary>AISACコントロールID 3</summary>
			Aisac3 = 23,
			/// <summary>AISACコントロールID 4</summary>
			Aisac4 = 24,
			/// <summary>AISACコントロールID 5</summary>
			Aisac5 = 25,
			/// <summary>AISACコントロールID 6</summary>
			Aisac6 = 26,
			/// <summary>AISACコントロールID 7</summary>
			Aisac7 = 27,
			/// <summary>AISACコントロールID 8</summary>
			Aisac8 = 28,
			/// <summary>AISACコントロールID 9</summary>
			Aisac9 = 29,
			/// <summary>AISACコントロールID 10</summary>
			Aisac10 = 30,
			/// <summary>AISACコントロールID 11</summary>
			Aisac11 = 31,
			/// <summary>AISACコントロールID 12</summary>
			Aisac12 = 32,
			/// <summary>AISACコントロールID 13</summary>
			Aisac13 = 33,
			/// <summary>AISACコントロールID 14</summary>
			Aisac14 = 34,
			/// <summary>AISACコントロールID 15</summary>
			Aisac15 = 35,
			/// <summary>ボイスプライオリティ</summary>
			Priority = 36,
			/// <summary>プリディレイ</summary>
			PreDelayTime = 37,
			/// <summary>バイクアッドフィルターのゲイン</summary>
			BiquadGain = 38,
			/// <summary>パンニング3D センターレベル</summary>
			Pan3dMixdownCenter = 39,
			/// <summary>パンニング3D LFEレベル</summary>
			Pan3dMixdownLfe = 40,
			/// <summary>エンベロープ アタック</summary>
			EgAttack = 41,
			/// <summary>エンベロープ リリース</summary>
			EgRelease = 42,
			/// <summary>シーケンス再生レシオ</summary>
			PlaybackRatio = 43,
			/// <summary>L chドライセンド</summary>
			DrySendL = 44,
			/// <summary>R chドライセンド</summary>
			DrySendR = 45,
			/// <summary>Center chドライセンド</summary>
			DrySendCenter = 46,
			/// <summary>LFE chドライセンド</summary>
			DrySendLfe = 47,
			/// <summary>Surround L chドライセンド</summary>
			DrySendSl = 48,
			/// <summary>Surround R chドライセンド</summary>
			DrySendSr = 49,
			/// <summary>Ex1 chドライセンド</summary>
			DrySendEx1 = 50,
			/// <summary>Ex2 chドライセンド</summary>
			DrySendEx2 = 51,
			/// <summary>マルチチャンネル音源の広がり</summary>
			Wideness = 52,
			/// <summary>スプレッド</summary>
			Spread = 53,
		}
		/// <summary>文字コード</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 文字コード（文字符号化方式）を表します。
		/// </para>
		/// </remarks>
		public enum CharacterEncoding
		{
			/// <summary>UTF-8</summary>
			Utf8 = 0,
			/// <summary>Shift_JIS</summary>
			Sjis = 1,
		}
		/// <summary>セレクター情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// セレクター情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetSelectorInfoByIndex"/> 関数または <see cref="CriAtomExAcf.GetSelectorInfoByName"/> 関数に
		/// 引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetSelectorInfoByIndex"/>
		/// <seealso cref="CriAtomExAcf.GetSelectorInfoByName"/>
		public unsafe partial struct SelectorInfo
		{
			/// <summary>セレクター名</summary>
			public NativeString name;

			/// <summary>データインデックス</summary>
			public UInt16 index;

			/// <summary>ラベル数</summary>
			public UInt16 numLabels;

			/// <summary>グローバル参照ラベルインデックス</summary>
			public UInt16 globalLabelIndex;

		}
		/// <summary>セレクターラベル情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// セレクターラベル情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetSelectorLabelInfo"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetSelectorLabelInfo"/>
		public unsafe partial struct SelectorLabelInfo
		{
			/// <summary>セレクター名</summary>
			public NativeString selectorName;

			/// <summary>セレクターラベル名</summary>
			public NativeString labelName;

		}
		/// <summary>Waveform information</summary>
		public unsafe partial struct WaveformInfo
		{
			/// <summary>波形データID</summary>
			public Int32 waveId;

			/// <summary>フォーマット種別</summary>
			public UInt32 format;

			/// <summary>サンプリング周波数</summary>
			public Int32 samplingRate;

			/// <summary>チャンネル数</summary>
			public Int32 numChannels;

			/// <summary>トータルサンプル数</summary>
			public Int64 numSamples;

			/// <summary>ストリーミングフラグ</summary>
			public NativeBool streamingFlag;

			/// <summary>予約領域</summary>
			public InlineArray1<UInt32> reserved;

		}
		/// <summary>キュー情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューの詳細情報です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetCueInfoByName"/>
		/// <seealso cref="CriAtomExAcb.GetCueInfoById"/>
		/// <seealso cref="CriAtomExAcb.GetCueInfoByIndex"/>
		public unsafe partial struct CueInfo
		{
			/// <summary>キューID</summary>
			public Int32 id;

			/// <summary>タイプ</summary>
			public CriAtomExAcb.CueType type;

			/// <summary>キュー名</summary>
			public NativeString name;

			/// <summary>ユーザーデータ</summary>
			public NativeString userData;

			/// <summary>長さ(msec)</summary>
			public Int64 length;

			/// <summary>カテゴリインデックス</summary>
			public InlineArray16<UInt16> categories;

			/// <summary>キューリミット</summary>
			public Int16 numLimits;

			/// <summary>ブロック数</summary>
			public UInt16 numBlocks;

			/// <summary>トラック数</summary>
			public UInt16 numTracks;

			/// <summary>関連する波形数</summary>
			public UInt16 numRelatedWaveforms;

			/// <summary>プライオリティ</summary>
			public Byte priority;

			/// <summary>ヘッダー公開フラグ</summary>
			public Byte headerVisibility;

			/// <summary>プレーヤーパラメーター無効化フラグ</summary>
			public Byte ignorePlayerParameter;

			/// <summary>再生確率</summary>
			public Byte probability;

			/// <summary>パンタイプ</summary>
			public CriAtomEx.PanType panType;

			/// <summary>3D情報</summary>
			public CriAtomEx.CuePos3dInfo pos3dInfo;

			/// <summary>ゲーム変数</summary>
			public CriAtomEx.GameVariableInfo gameVariableInfo;

			/// <summary>ボリューム</summary>
			public Single volume;

			/// <summary>無音時処理モード</summary>
			public CriAtomEx.SilentMode silentMode;

		}
		/// <summary>パンタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// どのようにして定位計算を行うかを指定するためのデータ型です。
		/// <see cref="CriAtomExPlayer.SetPanType"/> 関数で利用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPanType"/>
		public enum PanType
		{
			/// <summary>不明</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パンタイプが判明していない状態です。
			/// <see cref="CriAtomExAcb.GetCueInfoByName"/> 関数などにて、 ACB Ver.1.35.00 未満の
			/// ACB データ内のキューの情報（ <see cref="CriAtomEx.CueInfo"/> ）を取得した場合に得られます。
			/// </para>
			/// <para>
			/// 注意:
			/// <see cref="CriAtomExPlayer.SetPanType"/> 関数にて指定すると、エラーが発生します。
			/// </para>
			/// </remarks>
			Unknown = -1,
			/// <summary>パン3D</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// パン3Dで定位を計算します。
			/// </para>
			/// </remarks>
			Pan3d = 0,
			/// <summary>3Dポジショニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 3Dポジショニングで定位を計算します。
			/// </para>
			/// </remarks>
			_3dPos = 1,
			/// <summary>自動</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomExプレーヤーに3D音源／3Dリスナーが設定されている場合は3Dポジショニングで、
			/// 設定されていない場合はパン3Dで、それぞれ定位を計算します。
			/// </para>
			/// </remarks>
			Auto = 2,
		}
		/// <summary>キュー3D情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 波形情報は、キューの3D詳細情報です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.CueInfo"/>
		public unsafe partial struct CuePos3dInfo
		{
			/// <summary>コーン内部角度</summary>
			public Single coneInsideAngle;

			/// <summary>コーン外部角度</summary>
			public Single coneOutsideAngle;

			/// <summary>最小減衰距離</summary>
			public Single minDistance;

			/// <summary>最大減衰距離</summary>
			public Single maxDistance;

			/// <summary>Zero距離InteriorPan適用距離</summary>
			public Single sourceRadius;

			/// <summary>InteriorPan適用境界距離</summary>
			public Single interiorDistance;

			/// <summary>ドップラー係数</summary>
			public Single dopplerFactor;

			public CriAtomEx.CuePos3dInfoTagRandomPosition randomPosition;

			/// <summary>距離減衰AISACコントロール</summary>
			public UInt32 distanceAisacControl;

			/// <summary>リスナー基準方位角AISACコントロール</summary>
			public UInt32 listenerBaseAngleAisacControl;

			/// <summary>音源基準方位角AISACコントロール</summary>
			public UInt32 sourceBaseAngleAisacControl;

			/// <summary>リスナー基準仰俯角AISACコントロール</summary>
			public UInt32 listenerBaseElevationAisacControl;

			/// <summary>音源基準仰俯角AISACコントロール</summary>
			public UInt32 sourceBaseElevationAisacControl;

		}

		public unsafe partial struct CuePos3dInfoTagRandomPosition
		{
			/// <summary>元の3D音源に追従するかどうか</summary>
			public NativeBool followsOriginalSource;

			/// <summary>位置座標の算出方法</summary>
			public Int32 calculationType;

			/// <summary>位置座標の算出方法に関する各種パラメーター配列</summary>
			public InlineArray3<Single> calculationParameters;

		}
		/// <summary>無音時処理モード</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 無音時処理モードを指定するためのデータ型です。
		/// <see cref="CriAtomExPlayer.SetSilentMode"/> 関数で利用します。
		/// 無音となったかどうかは、以下のいずれかの値が0になったかどうかで判断します。
		/// - ボリューム
		/// - 3Dパンニングの演算結果によるボリューム
		/// - 3Dポジショニングの演算結果によるボリューム
		/// </para>
		/// <para>
		/// 注意:
		/// センドレベルや2Dパンの設定値では無音と判断されない点にご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetSilentMode"/>
		public enum SilentMode
		{
			/// <summary>何もしない</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 無音となっても特別な処理は行いません。（デフォルト値）
			/// </para>
			/// </remarks>
			Normal = 0,
			/// <summary>停止する</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 無音となった際は自動的に停止します。
			/// </para>
			/// </remarks>
			Stop = 1,
			/// <summary>バーチャル化する</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 無音となった際は自動的にバーチャル化します。
			/// </para>
			/// </remarks>
			Virtual = 2,
			/// <summary>再発音型でバーチャル化する</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 無音となった際は自動的に再発音型でバーチャル化します。
			/// </para>
			/// </remarks>
			VirtualRetrigger = 3,
		}
		/// <summary>標準ボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 標準ボイスプールの仕様を指定するための構造体です。
		/// <see cref="CriAtomExVoicePool.AllocateStandardVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForStandardVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateStandardVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForStandardVoicePool"/>
		public unsafe partial struct StandardVoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtom.StandardPlayerConfig playerConfig;

			/// <summary>ストリーム再生専用かどうか</summary>
			public NativeBool isStreamingOnly;

			/// <summary>最小チャンネル数</summary>
			public Int32 minChannels;

		}
		/// <summary>ADXボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADXボイスプールの仕様を指定するための構造体です。
		/// <see cref="CriAtomExVoicePool.AllocateAdxVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForAdxVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateAdxVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForAdxVoicePool"/>
		public unsafe partial struct AdxVoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtom.AdxPlayerConfig playerConfig;

			/// <summary>ストリーム再生専用かどうか</summary>
			public NativeBool isStreamingOnly;

			/// <summary>最小チャンネル数</summary>
			public Int32 minChannels;

		}
		/// <summary>HCAボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCAボイスプールの仕様を指定するための構造体です。
		/// <see cref="CriAtomExVoicePool.AllocateHcaVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForHcaVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateHcaVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForHcaVoicePool"/>
		public unsafe partial struct HcaVoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtom.HcaPlayerConfig playerConfig;

			/// <summary>ストリーム再生専用かどうか</summary>
			public NativeBool isStreamingOnly;

			/// <summary>最小チャンネル数</summary>
			public Int32 minChannels;

		}
		/// <summary>Waveボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Waveボイスプールの仕様を指定するための構造体です。
		/// <see cref="CriAtomExVoicePool.AllocateWaveVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForWaveVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateWaveVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForWaveVoicePool"/>
		public unsafe partial struct WaveVoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtom.WavePlayerConfig playerConfig;

			/// <summary>ストリーム再生専用かどうか</summary>
			public NativeBool isStreamingOnly;

			/// <summary>最小チャンネル数</summary>
			public Int32 minChannels;

		}
		/// <summary>AIFFボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AIFFボイスプールの仕様を指定するための構造体です。
		/// <see cref="CriAtomExVoicePool.AllocateAiffVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForAiffVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateAiffVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForAiffVoicePool"/>
		public unsafe partial struct AiffVoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtom.AiffPlayerConfig playerConfig;

			/// <summary>ストリーム再生専用かどうか</summary>
			public NativeBool isStreamingOnly;

			/// <summary>最小チャンネル数</summary>
			public Int32 minChannels;

		}
		/// <summary>RawPCMボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// RawPCMボイスプールの仕様を指定するための構造体です。
		/// <see cref="CriAtomExVoicePool.AllocateRawPcmVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForRawPcmVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateRawPcmVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForRawPcmVoicePool"/>
		public unsafe partial struct RawPcmVoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtom.RawPcmPlayerConfig playerConfig;

		}
		/// <summary>インストゥルメントボイスプール作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インストゥルメントボイスプールの仕様を指定するための構造体です。
		/// :: <see cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/> 関数に引数として渡します。
		/// プールできるボイス数の最大数は <see cref="CriAtomEx.MaxVoicesPerPool"/> で、
		/// 最小数は <see cref="CriAtomEx.MinVoicesPerPool"/> です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForInstrumentVoicePool"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AllocateInstrumentVoicePool"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForInstrumentVoicePool"/>
		public unsafe partial struct InstrumentVoicePoolConfig
		{
			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>プールするボイスの数</summary>
			public Int32 numVoices;

			/// <summary>ボイスの仕様</summary>
			public CriAtomInstrument.PlayerConfig playerConfig;

		}
		/// <summary>AISAC情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AISAC情報を取得するための構造体です。
		/// <see cref="CriAtomExPlayer.GetAttachedAisacInfo"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.GetAttachedAisacInfo"/>
		public unsafe partial struct AisacInfo
		{
			/// <summary>AISAC名</summary>
			public NativeString name;

			/// <summary>デフォルトコントロール値が設定されているか</summary>
			public NativeBool defaultControlFlag;

			/// <summary>デフォルトAISACコントロール値</summary>
			public Single defaultControlValue;

			/// <summary>Control Id</summary>
			public UInt32 controlId;

			/// <summary>Control Name</summary>
			public NativeString controlName;

		}
		/// <summary>REACT駆動パラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTの駆動パラメーター情報を設定取得するための構造体です。
		/// <see cref="CriAtomExCategory.SetReactParameter"/> , <see cref="CriAtomExCategory.GetReactParameter"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public unsafe partial struct ReactParameter
		{
			public CriAtomEx.ReactParameterTagParameter parameter;

			/// <summary>REACTタイプ</summary>
			public CriAtomEx.ReactType type;

			/// <summary>ポーズ中のキューは適用するか</summary>
			public NativeBool enablePausingCue;

		}

		[StructLayout(LayoutKind.Explicit)]
		public unsafe partial struct ReactParameterTagParameter
		{
			/// <summary>ダッカーパラメーター</summary>
			[FieldOffset(0)] public CriAtomEx.ReactDuckerParameter ducker;

			/// <summary>AISACモジュレーショントリガーパラメーター</summary>
			[FieldOffset(0)] public CriAtomEx.ReactAisacModulationParameter aisacModulation;

		}
		/// <summary>REACTによるダッカーパラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTによるダッカーの駆動パラメーター情報を設定取得するための構造体です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ReactParameter"/>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public unsafe partial struct ReactDuckerParameter
		{
			public CriAtomEx.ReactDuckerParameterTagTarget target;

			/// <summary>ダッカーの操作対象</summary>
			public CriAtomEx.ReactDuckerTargetType targetType;

			/// <summary>変化開始フェードパラメーター</summary>
			public CriAtomEx.ReactFadeParameter entry;

			/// <summary>変化終了フェードパラメーター</summary>
			public CriAtomEx.ReactFadeParameter exit;

			/// <summary>ホールドタイプ</summary>
			public CriAtomEx.ReactHoldType holdType;

			/// <summary>ホールド時間（ミリ秒）</summary>
			public UInt16 holdTimeMs;

		}

		[StructLayout(LayoutKind.Explicit)]
		public unsafe partial struct ReactDuckerParameterTagTarget
		{
			[FieldOffset(0)] public CriAtomEx.ReactDuckerParameterTagVolume volume;

			[FieldOffset(0)] public CriAtomEx.ReactDuckerParameterTagAisacControlValue aisacControlValue;

		}

		public unsafe partial struct ReactDuckerParameterTagVolume
		{
			/// <summary>減衰ボリュームレベル</summary>
			public Single level;

		}

		public unsafe partial struct ReactDuckerParameterTagAisacControlValue
		{
			/// <summary>AISACコントロールid</summary>
			public UInt32 controlId;

			/// <summary>AISACコントロール値</summary>
			public Single controlValue;

		}
		/// <summary>REACTによるダッキングのターゲット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTによるダッキング対象のタイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ReactParameter"/>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public enum ReactDuckerTargetType
		{
			/// <summary>ボリュームのダッカー</summary>
			Volume = 0,
			/// <summary>AISACコントロール値のダッカー</summary>
			AisacControlValue = 1,
		}
		/// <summary>REACTフェードパラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTのフェード駆動パラメーター情報を設定取得するための構造体です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ReactParameter"/>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public unsafe partial struct ReactFadeParameter
		{
			/// <summary>変化曲線タイプ</summary>
			public CriAtomEx.CurveType curveType;

			/// <summary>変化曲線の強さ（0.0f ～ 2.0f）</summary>
			public Single curveStrength;

			/// <summary>フェード時間（ミリ秒）</summary>
			public UInt16 fadeTimeMs;

		}
		/// <summary>曲線タイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 曲線のタイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ReactFadeParameter"/>
		public enum CurveType
		{
			/// <summary>直線</summary>
			Linear = 0,
			/// <summary>低速変化</summary>
			Square = 1,
			/// <summary>高速変化</summary>
			SquareReverse = 2,
			/// <summary>S字曲線</summary>
			S = 3,
			/// <summary>逆S字曲線</summary>
			FlatAtHalf = 4,
		}
		/// <summary>REACTホールドタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTホールド（減衰時間の維持）タイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ReactParameter"/>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public enum ReactHoldType
		{
			/// <summary>再生中にホールドを行う</summary>
			WhilePlaying = 0,
			/// <summary>固定時間でホールドを行う</summary>
			FixedTime = 1,
		}
		/// <summary>AISACモジュレーショントリガーパラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AISACモジュレーショントリガーの駆動パラメーター情報を設定取得するための構造体です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ReactParameter"/>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public unsafe partial struct ReactAisacModulationParameter
		{
			/// <summary>変化AISACモジュレーションキーが有効か否か</summary>
			public NativeBool enableDecrementAisacModulationKey;

			/// <summary>変化AISACモジュレーションキー</summary>
			public UInt32 decrementAisacModulationKey;

			/// <summary>戻りAISACモジュレーションキーが有効か否か</summary>
			public NativeBool enableIncrementAisacModulationKey;

			/// <summary>戻りAISACモジュレーションキー</summary>
			public UInt32 incrementAisacModulationKey;

		}
		/// <summary>REACTタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// REACTのタイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ReactParameter"/>
		/// <seealso cref="CriAtomExCategory.SetReactParameter"/>
		/// <seealso cref="CriAtomExCategory.GetReactParameter"/>
		public enum ReactType
		{
			/// <summary>ダッカー</summary>
			Ducker = 0,
			/// <summary>AISACモジュレーショントリガー</summary>
			AisacModulationTrigger = 1,
		}
		/// <summary>ボイス確保方式</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomEx プレーヤーがボイスを確保する際の動作仕様を指定するためのデータ型です。
		/// AtomEx プレーヤーを作成する際、 <see cref="CriAtomExPlayer.Config"/> 構造体のメンバに指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Config"/>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		public enum VoiceAllocationMethod
		{
			/// <summary>ボイスの確保は1回限り</summary>
			AllocateVoiceOnce = 0,
			/// <summary>ボイスを繰り返し確保する</summary>
			RetryVoiceAllocation = 1,
		}
		/// <summary>ポーズ解除対象</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ポーズを解除する対象を指定するためのデータ型です。
		/// <see cref="CriAtomExPlayer.Resume"/> 関数、および <see cref="CriAtomExPlayback.Resume"/>
		/// 関数の引数として使用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Resume"/>
		/// <seealso cref="CriAtomExPlayback.Resume"/>
		public enum ResumeMode
		{
			/// <summary>一時停止方法に関係なく再生を再開</summary>
			AllPlayback = 0,
			/// <summary>Pause 関数でポーズをかけた音声のみ再生を再開</summary>
			PausedPlayback = 1,
			/// <summary>Prepare 関数で再生準備を指示した音声の再生を開始</summary>
			PreparedPlayback = 2,
			ModeReserved = 3,
		}
		/// <summary>ボイス制御方式</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する音声の発音制御方法を指定するためのデータ型です。
		/// <see cref="CriAtomExPlayer.SetVoiceControlMethod"/> 関数で利用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetVoiceControlMethod"/>
		public enum VoiceControlMethod
		{
			/// <summary>後着優先</summary>
			PreferLast = 0,
			/// <summary>先着優先</summary>
			PreferFirst = 1,
			/// <summary>データ設定優先</summary>
			PreferData = 2,
		}
		/// <summary>パラメーターID</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラメーターを指定するためのIDです。
		/// <see cref="CriAtomExPlayer.GetParameterFloat32"/> 関数等で利用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.GetParameterFloat32"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterSint32"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterUint32"/>
		public enum ParameterId
		{
			/// <summary>ボリューム</summary>
			Volume = 0,
			/// <summary>ピッチ</summary>
			Pitch = 1,
			/// <summary>パンニング3D角度</summary>
			Pan3dAngle = 2,
			/// <summary>パンニング3D距離</summary>
			Pan3dDistance = 3,
			/// <summary>パンニング3Dボリューム</summary>
			Pan3dVolume = 4,
			/// <summary>パンタイプ</summary>
			PanType = 5,
			/// <summary>パンスピーカータイプ</summary>
			PanSpeakerType = 6,
			/// <summary>2Dパン（チャンネル0）</summary>
			PanCh0 = 7,
			/// <summary>2Dパン（チャンネル1）</summary>
			PanCh1 = 8,
			/// <summary>バスセンドレベル0</summary>
			BusSendLevel0 = 9,
			/// <summary>バスセンドレベル1</summary>
			BusSendLevel1 = 10,
			/// <summary>バスセンドレベル2</summary>
			BusSendLevel2 = 11,
			/// <summary>バスセンドレベル3</summary>
			BusSendLevel3 = 12,
			/// <summary>バスセンドレベル4</summary>
			BusSendLevel4 = 13,
			/// <summary>バスセンドレベル5</summary>
			BusSendLevel5 = 14,
			/// <summary>バスセンドレベル6</summary>
			BusSendLevel6 = 15,
			/// <summary>バスセンドレベル7</summary>
			BusSendLevel7 = 16,
			/// <summary>バンドパスフィルターの低域カットオフ周波数</summary>
			BandpassFilterCofLow = 17,
			/// <summary>バンドパスフィルターの高域カットオフ周波数</summary>
			BandpassFilterCofHigh = 18,
			/// <summary>バイクアッドフィルターのフィルタータイプ</summary>
			BiquadFilterType = 19,
			/// <summary>バイクアッドフィルターの周波数</summary>
			BiquadFilterFreq = 20,
			/// <summary>バイクアッドフィルターのQ値</summary>
			BiquadFilterQ = 21,
			/// <summary>バイクアッドフィルターのゲイン</summary>
			BiquadFilterGain = 22,
			/// <summary>エンベロープのアタックタイム</summary>
			EnvelopeAttackTime = 23,
			/// <summary>エンベロープのホールドタイム</summary>
			EnvelopeHoldTime = 24,
			/// <summary>エンベロープのディケイタイム</summary>
			EnvelopeDecayTime = 25,
			/// <summary>エンベロープのリリースタイム</summary>
			EnvelopeReleaseTime = 26,
			/// <summary>エンベロープのサスティンレベル</summary>
			EnvelopeSustainLevel = 27,
			/// <summary>再生開始位置</summary>
			StartTime = 28,
			/// <summary>ボイスプライオリティ</summary>
			Priority = 31,
			/// <summary>無音時処理モード</summary>
			SilentMode = 32,
			/// <summary>インサーションDSPのパラメーター0</summary>
			DspParameter0 = 33,
			/// <summary>インサーションDSPのパラメーター1</summary>
			DspParameter1 = 34,
			/// <summary>インサーションDSPのパラメーター2</summary>
			DspParameter2 = 35,
			/// <summary>インサーションDSPのパラメーター3</summary>
			DspParameter3 = 36,
			/// <summary>インサーションDSPのパラメーター4</summary>
			DspParameter4 = 37,
			/// <summary>インサーションDSPのパラメーター5</summary>
			DspParameter5 = 38,
			/// <summary>インサーションDSPのパラメーター6</summary>
			DspParameter6 = 39,
			/// <summary>インサーションDSPのパラメーター7</summary>
			DspParameter7 = 40,
			/// <summary>インサーションDSPのパラメーター8</summary>
			DspParameter8 = 41,
			/// <summary>インサーションDSPのパラメーター9</summary>
			DspParameter9 = 42,
			/// <summary>インサーションDSPのパラメーター10</summary>
			DspParameter10 = 43,
			/// <summary>インサーションDSPのバイパスフラグ</summary>
			DspBypassFlag = 44,
		}
		/// <summary>パンニング時の出力スピーカータイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 定位計算を行う際、出力としてどのスピーカーを使用するかを表します。
		/// <see cref="CriAtomExPlayer.ChangeDefaultPanSpeakerType"/> 関数、<see cref="CriAtomExPlayer.SetPanSpeakerType"/> 関数で利用します。
		/// </para>
		/// <para>
		/// 備考:
		/// ステレオスピーカーのプラットフォームでは、どれを選んだとしても最終的にはステレオにダウンミックスされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPanSpeakerType"/>
		public enum PanSpeakerType
		{
			/// <summary>4chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, Ls, Rsを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_4ch = 0,
			/// <summary>5chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, C, Ls, Rsを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_5ch = 1,
			/// <summary>6chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, Ls, Rs, Lsb, Rsbを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_6ch = 2,
			/// <summary>7chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, C, Ls, Rs, Lsb, Rsbを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_7ch = 3,
			/// <summary>5.0.2chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, C, Ls, Rs, Lts, Rtsを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_5_0_2ch = 4,
			/// <summary>7.0.4chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, C, Ls, Rs, Lsb, Rsb、Ltf、Rtf、Ltb、Rtbを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_7_0_4ch = 5,
			/// <summary>4.0.2chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, Ls, Rs, Lts, Rtsを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_4_0_2ch = 6,
			/// <summary>6.0.4chパンニング</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// L, R, Ls, Rs, Lsb, Rsb、Ltf、Rtf、Ltb、Rtbを使用してパンニングを行います。
			/// </para>
			/// </remarks>
			_6_0_4ch = 7,
			/// <summary>パンニング自動設定</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 出力先のチャンネル数に応じて最大のチャンネル構成をを使用してパンニングを行います。センタースピーカーは含まれません。デフォルトのパンスピーカータイプです。
			/// </para>
			/// </remarks>
			Auto = 10,
			/// <summary>パンニング自動設定（センタースピーカーあり）</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 出力先のチャンネル数に応じて最大のチャンネル構成をを使用してパンニングを行います。センタースピーカーが含まれます。
			/// </para>
			/// </remarks>
			AutoWithCenter = 11,
		}
		/// <summary>パンニング時の角度タイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// マルチチャンネル素材の定位計算を行う際、各入力チャンネルをどのような角度として扱うかを表します。
		/// <see cref="CriAtomExPlayer.SetPanAngleType"/> 関数で利用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPanAngleType"/>
		public enum PanAngleType
		{
			/// <summary>オフセット</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 設定されているパン3D角度を中心として、スピーカーの配置を元にしたオフセット値を各チャンネル毎に加えて、
			/// それぞれの入力チャンネルで個別にパンニング計算を行います。
			/// 例えばステレオ素材でパン3D角度を0度と設定した場合、Lチャンネルは-30度となりそのままLスピーカーから出力され、
			/// Rチャンネルは+30度となりそのままRスピーカーから出力されます。
			/// またパン3D角度を+30度と設定した場合、Lチャンネルは0度、Rチャンネルは60度の位置に定位しているものとして、
			/// パンニング計算が行われます。
			/// </para>
			/// </remarks>
			Offset = 0,
			/// <summary>固定</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 入力チャンネル数に応じて、各チャンネルが該当スピーカー位置に固定して存在しているものとして、
			/// 各スピーカー間のバランスを計算するような形でパンニング計算を行います。
			/// 例えばステレオ素材でパン3D角度を0度と設定した場合、LチャンネルはLスピーカーから約0.7倍で出力され、
			/// Rチャンネルはスピーカーから約0.7倍で出力されます。
			/// またパン3D角度を+30度と設定した場合、Lチャンネルはまったく出力されず、RチャンネルはRスピーカーからそのまま出力されます。
			/// </para>
			/// <para>
			/// 備考:
			/// この挙動はCRI Audioの頃のパン3Dと同等です。
			/// どのスピーカーにどのチャンネルを割り当てるかは、<see cref="CriAtomExPlayer.SetDrySendLevel"/> 関数で設定するドライセンドレベルでの扱いと同様です。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomExPlayer.SetDrySendLevel"/>
			Fix = 1,
			/// <summary>環境音ミックス</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 特殊なパン角度タイプです。使用しないでください。
			/// </para>
			/// </remarks>
			AmbienceMix = 4,
			/// <summary>環境音直線補間</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 特殊なパン角度タイプです。使用しないでください。
			/// </para>
			/// </remarks>
			AmbienceStraight = 5,
		}
		/// <summary>スピーカーID</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声を出力するスピーカーを指定するためのIDです。
		/// <see cref="CriAtomExPlayer.SetSendLevel"/> 関数で利用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetSendLevel"/>
		public enum SpeakerId
		{
			/// <summary>フロントレフトスピーカー</summary>
			FrontLeft = 0,
			/// <summary>フロントライトスピーカー</summary>
			FrontRight = 1,
			/// <summary>フロントセンタースピーカー</summary>
			FrontCenter = 2,
			/// <summary>LFE（≒サブウーハー）</summary>
			LowFrequency = 3,
			/// <summary>サラウンドレフトスピーカー</summary>
			SurroundLeft = 4,
			/// <summary>サラウンドライトスピーカー</summary>
			SurroundRight = 5,
			/// <summary>サラウンドバックレフトスピーカー</summary>
			SurroundBackLeft = 6,
			/// <summary>サラウンドバックライトスピーカー</summary>
			SurroundBackRight = 7,
		}
		/// <summary>バイクアッドフィルターのタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バイクアッドフィルターのタイプを指定するためのデータ型です。
		/// <see cref="CriAtomExPlayer.SetBiquadFilterParameters"/> 関数で利用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetBiquadFilterParameters"/>
		public enum BiquadFilterType
		{
			/// <summary>フィルター無効</summary>
			Off = 0,
			/// <summary>ローパスフィルター</summary>
			Lowpass = 1,
			/// <summary>ハイパスフィルター</summary>
			Highpass = 2,
			/// <summary>ノッチフィルター</summary>
			Notch = 3,
			/// <summary>ローシェルフフィルター</summary>
			Lowshelf = 4,
			/// <summary>ハイシェルフフィルター</summary>
			Highshelf = 5,
			/// <summary>ピーキングフフィルター</summary>
			Peaking = 6,
		}
		/// <summary>シーケンスコールバックイベント用Info構造体</summary>
		public unsafe partial struct SequenceEventInfo
		{
			/// <summary>イベント位置</summary>
			public UInt64 position;

			/// <summary>プレーヤーオブジェクト</summary>
			public IntPtr player;

			/// <summary>データ埋め込み文字列</summary>
			public NativeString @string;

			/// <summary>再生ID</summary>
			public UInt32 id;

			/// <summary>イベントタイプ</summary>
			public CriAtomEx.SequecneEventType type;

			/// <summary>データ埋め込み値</summary>
			public UInt32 value;

			/// <summary>予約領域</summary>
			public InlineArray1<UInt32> reserved;

		}
		/// <summary>シーケンスイベントコールバック</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExライブラリのシーケンスイベントコールバックタイプです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SequenceEventInfo"/>
		public enum SequecneEventType
		{
			/// <summary>シーケンスコールバック</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// シーケンスデータ内に埋め込まれたコールバックイベント情報。
			/// </para>
			/// </remarks>
			Callback = 0,
		}
		/// <summary>3次元ベクトル構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3次元ベクトルを扱うための構造体です。
		/// </para>
		/// </remarks>
		public unsafe partial struct Vector
		{
			/// <summary>X軸の要素</summary>
			public Single x;

			/// <summary>Y軸の要素</summary>
			public Single y;

			/// <summary>Z軸の要素</summary>
			public Single z;

		}
		/// <summary>パンタイプがパン3Dの時に、距離減衰AISACと角度AISACコントロール値を音源に反映するか設定</summary>
		/// <param name="flag">AISACコントロール値を適用するか？（true:する、false:しない）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 引数の flag に true を設定すると、パンタイプがパン3Dの音源を再生する際に、3Dソースと3Dリスナーオブジェクトが
		/// 設定されているときは常に距離減衰AISACと角度AISACの計算結果が音源に適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// Atomライブラリのデフォルトでは、パン3D音源に対して距離減衰AISACと角度AISACの計算結果は適用されません。
		/// 本関数はCRI Atomライブラリ Ver.2.17.19 以前の動作との互換の為に追加されました。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.IsEnableCalculationAisacControlFrom3dPosition"/>
		public static void EnableCalculationAisacControlFrom3dPosition(NativeBool flag)
		{
			NativeMethods.criAtomEx_EnableCalculationAisacControlFrom3dPosition(flag);
		}

		/// <summary>パンタイプがパン3Dの時に、距離減衰AISACと角度AISACコントロール値を音源に反映しているか取得</summary>
		/// <returns>計算結果を適用している</returns>
		/// <returns>計算結果を適用していない</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンタイプがパン3Dの音源を再生する際に、3Dソースと3Dリスナーオブジェクトが
		/// 設定されているときに距離減衰AISACと角度AISACの計算結果が音源に適用されているか否かを取得します。
		/// </para>
		/// <para>
		/// 注意:
		/// Atomライブラリのデフォルトでは、パン3D音源に対して距離減衰AISACと角度AISACの計算結果は適用されません。
		/// 本関数はCRI Atomライブラリ Ver.2.17.19 以前の動作との互換の為に追加されました。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.EnableCalculationAisacControlFrom3dPosition"/>
		public static bool IsEnableCalculationAisacControlFrom3dPosition()
		{
			return NativeMethods.criAtomEx_IsEnableCalculationAisacControlFrom3dPosition();
		}

		/// <summary>ピッチシフタDSPのアタッチ用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ピッチシフタDSPをボイスプールにアタッチするための構造体です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForDspPitchShifter"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspPitchShifter"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForDspPitchShifter"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForDspPitchShifter"/>
		public unsafe partial struct DspPitchShifterConfig
		{
			/// <summary>作成するDSPの数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アタッチ先のボイス数と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 numDsp;

			/// <summary>DSPの最大チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSPが処理可能な最大チャンネル数です。
			/// アタッチ先ボイスプールのプレーヤー設定の最大チャンネル数（max_channels）と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>DSPの最大サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSPが処理可能な最大サンプリングレートです。
			/// アタッチ先ボイスプールのプレーヤー設定の最大サンプリングレート（max_sampling_rate）と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>DSP固有設定の構造体</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSP固有のコンフィグ設定を行います。
			/// 詳しくは <see cref="CriAtomDsp.PitchShifterConfig"/> をご参照ください。
			/// </para>
			/// </remarks>
			public CriAtomDsp.PitchShifterConfig specific;

		}
		/// <summary>タイムストレッチDSPのアタッチ用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// タイムストレッチDSPをボイスプールにアタッチするための構造体です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExVoicePool.SetDefaultConfigForDspTimeStretch"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspTimeStretch"/>
		/// <seealso cref="CriAtomExVoicePool.CalculateWorkSizeForDspTimeStretch"/>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForDspTimeStretch"/>
		public unsafe partial struct DspTimeStretchConfig
		{
			/// <summary>作成するDSPの数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アタッチ先のボイス数と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 numDsp;

			/// <summary>DSPの最大チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSPが処理可能な最大チャンネル数です。
			/// アタッチ先ボイスプールのプレーヤー設定の最大チャンネル数（max_channels）と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>DSPの最大サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSPが処理可能な最大サンプリングレートです。
			/// アタッチ先ボイスプールのプレーヤー設定の最大サンプリングレート（max_sampling_rate）と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>DSP固有設定の構造体</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSP固有のコンフィグ設定を行います。
			/// 詳しくは <see cref="CriAtomDsp.TimeStretchConfig"/> をご参照ください。
			/// </para>
			/// </remarks>
			public CriAtomDsp.TimeStretchConfig specific;

		}
		/// <summary>AFX形式のインサーションDSPのアタッチ用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AFX形式のインサーションDSPをアタッチする際に指定するパラメーターです。
		/// <see cref="CriAtomExVoicePool.AttachDspAfx"/> 関数の引数として使います。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.AttachDspAfx"/>
		public unsafe partial struct DspAfxConfig
		{
			/// <summary>作成するDSPの数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アタッチ先のボイス数と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 numDsp;

			/// <summary>DSPの最大チャンネル数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSPが処理可能な最大チャンネル数です。
			/// アタッチ先ボイスプールのプレーヤー設定の最大チャンネル数（max_channels）と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>DSPの最大サンプリングレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// DSPが処理可能な最大サンプリングレートです。
			/// アタッチ先ボイスプールのプレーヤー設定の最大サンプリングレート（max_sampling_rate）と同じ値を指定する必要があります。
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>Afx形式DSP設定の構造体</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Afx形式のDSPのコンフィグ設定を行います。
			/// 詳しくは <see cref="CriAtomDsp.AfxConfig"/> をご参照ください。
			/// </para>
			/// </remarks>
			public CriAtomDsp.AfxConfig specific;

		}
		/// <summary>ボイスイベントコールバックの登録</summary>
		/// <param name="func">ボイスイベントコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスイベントコールバックを登録します。
		/// 本関数を使用してボイスイベントコールバックを登録することで、
		/// ボイスイベント（ボイスの取得／解放／奪い取り）発生時の詳細情報
		/// （再生／停止される音声データの詳細情報等）が取得可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomEx.VoiceEventCbFunc"/> の説明をご参照ください。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で登録したコールバックには、ボイス単位のリミット制御
		/// *（ボイスプライオリティに基づいた波形単位のプライオリティ制御）
		/// に関する情報のみが返されます。
		/// （カテゴリキュープライオリティによる制御に関する情報は、現状取得できません。）
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.VoiceEventCbFunc"/>
		public static unsafe void SetVoiceEventCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.VoiceEvent, CriAtomEx.VoiceInfoDetail*, CriAtomEx.VoiceInfoDetail*, CriAtomEx.VoiceInfoDetail*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomEx_SetVoiceEventCallback((IntPtr)func, obj);
		}
		static unsafe void SetVoiceEventCallbackInternal(IntPtr func, IntPtr obj) => SetVoiceEventCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.VoiceEvent, CriAtomEx.VoiceInfoDetail*, CriAtomEx.VoiceInfoDetail*, CriAtomEx.VoiceInfoDetail*, void>)func, obj);
		static CriAtomEx.VoiceEventCbFunc _voiceEventCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetVoiceEventCallback" />
		public static CriAtomEx.VoiceEventCbFunc VoiceEventCallback => _voiceEventCallback ?? (_voiceEventCallback = new CriAtomEx.VoiceEventCbFunc(SetVoiceEventCallbackInternal));

		/// <summary>ボイスイベントコールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスイベントの通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtomEx.SetVoiceEventCallback"/> 関数に本関数型のコールバック関数を登録することで、
		/// ボイスイベント発生時にコールバックを受け取ることが可能となります。
		/// コールバック関数の第3～5引数（request、removed、removed_in_group）に入る値は、
		/// ボイスイベントの種別（第2引数のvoice_eventの値）により以下のように変わります。
		/// (1) <see cref="CriAtomEx.VoiceEvent.Allocate"/>時
		/// 第3引数requestに、ボイスを取得した発音リクエストの情報が入ります。
		/// 第4引数、第5引数にはnullが入ります。
		/// (2) <see cref="CriAtomEx.VoiceEvent.AllocateAndRemove"/>時
		/// 第3引数requestに、ボイスを取得した発音リクエストの情報が入ります。
		/// 第4引数removedには、ボイスを奪い取られ、発音が停止した再生の情報が入ります。
		/// 第5引数にはnullが入ります。
		/// (3) <see cref="CriAtomEx.VoiceEvent.AllocateAndRemoveInGroup"/>時
		/// 第3引数requestに、ボイスを取得した発音リクエストの情報が入ります。
		/// 第4引数にはnullが入ります。
		/// 第5引数removed_in_groupには、ボイスを奪い取られ、発音が停止した再生の情報が入ります。
		/// (4) <see cref="CriAtomEx.VoiceEvent.AloocateAndRemoveTwo"/>時
		/// 第3引数requestに、ボイスを取得した発音リクエストの情報が入ります。
		/// 第4引数removedには、ボイスを奪い取られ、発音が停止した再生の情報が入ります。
		/// 第5引数removed_in_groupには、グループ内の発音数調整により、停止された再生の情報が入ります。
		/// (5) <see cref="CriAtomEx.VoiceEvent.Reject"/>時
		/// 第3引数requestに、ボイスの取得が棄却された発音リクエストの情報が入ります。
		/// 第4引数、第5引数にはnullが入ります。
		/// (6) <see cref="CriAtomEx.VoiceEvent.RejectByGroupLimit"/>時
		/// 第3引数requestに、ボイスの取得が棄却された発音リクエストの情報が入ります。
		/// 第4引数、第5引数にはnullが入ります。
		/// (7) <see cref="CriAtomEx.VoiceEvent.Remove"/>時
		/// 第4引数removedに、再生が終了または停止したボイスの情報が入ります。
		/// 第3引数、第5引数にはnullが入ります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバックでは、ボイス単位のリミット制御
		/// *（ボイスプライオリティに基づいた波形単位のプライオリティ制御）
		/// に関する情報のみが取得可能です。
		/// ボイス取得前にキューリミット制御で発音が棄却された場合、
		/// 本コールバックに<see cref="CriAtomEx.VoiceEvent.Reject"/>等の情報は返されません。
		/// （カテゴリキュープライオリティによる制御に関する情報は、現状取得できません。）
		/// ボイスイベントコールバック時点では、 request->atom_player
		/// には再生すべき音声データがまだセットされていません。
		/// そのため、再生する音声データの情報等については、 atom_player に問い合わせず、
		/// <see cref="CriAtomEx.VoiceInfoDetail"/> 構造体のメンバ値を使用してください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetVoiceEventCallback"/>
		/// <seealso cref="CriAtomEx.VoiceEvent"/>
		/// <seealso cref="CriAtomEx.VoiceInfoDetail"/>
		public unsafe class VoiceEventCbFunc : NativeCallbackBase<VoiceEventCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>発生イベント</summary>
				public CriAtomEx.VoiceEvent voiceEvent { get; }
				/// <summary>発音要求の詳細情報</summary>
				public NativeReference<CriAtomEx.VoiceInfoDetail> request { get; }
				/// <summary>停止ボイスの詳細情報</summary>
				public NativeReference<CriAtomEx.VoiceInfoDetail> removed { get; }
				/// <summary>グループ内停止ボイスの詳細情報</summary>
				public NativeReference<CriAtomEx.VoiceInfoDetail> removedInGroup { get; }

				internal Arg(CriAtomEx.VoiceEvent voiceEvent, NativeReference<CriAtomEx.VoiceInfoDetail> request, NativeReference<CriAtomEx.VoiceInfoDetail> removed, NativeReference<CriAtomEx.VoiceInfoDetail> removedInGroup)
				{
					this.voiceEvent = voiceEvent;
					this.request = request;
					this.removed = removed;
					this.removedInGroup = removedInGroup;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtomEx.VoiceEvent voiceEvent, CriAtomEx.VoiceInfoDetail* request, CriAtomEx.VoiceInfoDetail* removed, CriAtomEx.VoiceInfoDetail* removedInGroup) =>
				InvokeCallbackInternal(obj, new(voiceEvent, request, removed, removedInGroup));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomEx.VoiceEvent voiceEvent, CriAtomEx.VoiceInfoDetail* request, CriAtomEx.VoiceInfoDetail* removed, CriAtomEx.VoiceInfoDetail* removedInGroup);
			static NativeDelegate callbackDelegate = null;
#endif
			internal VoiceEventCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomEx.VoiceEvent, CriAtomEx.VoiceInfoDetail*, CriAtomEx.VoiceInfoDetail*, CriAtomEx.VoiceInfoDetail*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ボイスイベント</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスイベントの種別を示す値です。
		/// ボイスイベントコールバックに引数として渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.VoiceEventCbFunc"/>
		/// <seealso cref="CriAtomEx.SetVoiceEventCallback"/>
		public enum VoiceEvent
		{
			/// <summary>ボイスの新規確保</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ボイスプールから空きボイスが取得され、新規に発音が開始されたことを示す値です。
			/// </para>
			/// </remarks>
			Allocate = 0,
			/// <summary>ボイスの奪い取り</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生中のボイスが奪い取られたことを示す値です。
			/// 再生中のボイスが1つ停止され、そのボイスが別の音声の再生に再利用されました。
			/// 停止される波形データと新規に再生する波形データは、
			/// 異なるボイスリミットグループに所属しています。
			/// （どちらか一方、もしくは両方の波形データがボイスリミットグループに所属していない場合も、
			/// ボイスの奪い取り発生時に本イベントが発生します。）
			/// </para>
			/// </remarks>
			AllocateAndRemove = 1,
			/// <summary>グループ内でのボイスの奪い取り</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生中のボイスが奪い取られたことを示す値です。
			/// 再生中のボイスが1つ停止され、そのボイスが別の音声の再生に再利用されました。
			/// <see cref="CriAtomEx.VoiceEvent.AllocateAndRemove"/> と異なり、
			/// 停止される波形データと新規に再生する波形データとが、
			/// 同一のボイスリミットグループに所属する場合に本イベントが発生します。
			/// </para>
			/// </remarks>
			AllocateAndRemoveInGroup = 2,
			/// <summary>ボイスの奪い取りとボイス数の調整</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生中のボイスが奪い取られ、さらにボイス数の調整が行われたことを示す値です。
			/// 2つのボイスが停止され、そのうち1つのボイスが別の音声の再生に再利用されます。
			/// （停止されただけのボイスは、空きボイスとしてボイスプールに戻されます。）
			/// 音声データの再生要件を満たすボイスを奪い取った結果、グループ内のボイス数があふれ、
			/// グループ内でボイス数を調整した場合に本イベントが発生します。
			/// </para>
			/// <para>
			/// 備考:
			/// このケースは、ボイスリミットグループ上限数分のHCAデータを再生中に、
			/// 同一ボイスリミットグループに所属するADXデータを再生した場合等に発生します。
			/// ADXデータを再生するため、グループ外のADXボイスを停止した結果、
			/// HCAデータとADXデータの合計数がボイスリミットグループ上限を超えた場合、
			/// 低プライオリティのHCAデータがさらに1つ停止される形になります。
			/// （1つの発音リクエストに対し、2つの音声が停止する形になります。）
			/// </para>
			/// </remarks>
			AloocateAndRemoveTwo = 3,
			/// <summary>発音要求の棄却</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生しようとした波形データのプライオリティが、
			/// 全ボイス中で最も低かった場合（他のボイスを奪い取れなかった場合）に、
			/// 本イベントが発生します。
			/// </para>
			/// </remarks>
			Reject = 4,
			/// <summary>グループ内での発音要求の棄却</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生しようとした波形データのプライオリティが、
			/// 所属するグループ内で最も低かった場合（グループ内の他のボイスを奪い取れなかった場合）に、
			/// 本イベントが発生します。
			/// </para>
			/// </remarks>
			RejectByGroupLimit = 5,
			/// <summary>ボイスの停止</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生完了や再生停止要求により、ボイスが停止された場合に本イベントが発生します。
			/// 停止されたボイスは、空きボイスとしてボイスプールに戻されます。
			/// </para>
			/// </remarks>
			Remove = 6,
		}
		/// <summary>ボイスの詳細情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスイベント発生時のボイスの詳細情報を保持した構造体です。
		/// ボイスイベントコールバックに引数として渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.VoiceEventCbFunc"/>
		/// <seealso cref="CriAtomEx.SetVoiceEventCallback"/>
		public unsafe partial struct VoiceInfoDetail
		{
			/// <summary>再生ID</summary>
			public UInt32 playbackId;

			/// <summary>キュー情報</summary>
			public CriAtomEx.SourceInfo cueInfo;

			/// <summary>波形情報</summary>
			public CriAtomEx.SourceInfo waveInfo;

			/// <summary>グループ番号</summary>
			public Int32 groupNo;

			/// <summary>プライオリティ</summary>
			public Int32 priority;

			/// <summary>ボイス制御方法</summary>
			public CriAtomEx.VoiceControlMethod controlMethod;

			/// <summary>ボイス確保方法</summary>
			public CriAtomEx.VoiceAllocationMethod allocationMethod;

			/// <summary>ボイスプール識別子</summary>
			public UInt32 identifier;

			/// <summary>フォーマット種別</summary>
			public UInt32 format;

			/// <summary>サンプリング周波数</summary>
			public Int32 samplingRate;

			/// <summary>チャンネル数</summary>
			public Int32 numChannels;

			/// <summary>ストリーム再生かどうか</summary>
			public NativeBool streamingFlag;

			/// <summary>発音に使用するプレーヤー</summary>
			public IntPtr atomPlayer;

		}
		/// <summary>ボイス情報の列挙</summary>
		/// <param name="func">ボイス情報コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生中のボイスの情報を列挙します。
		/// 本関数を実行すると、第 1 引数（ func ）
		/// でセットされたコールバック関数が再生中のボイスの数分だけ呼び出されます。
		/// コールバック関数には、再生中のボイスに関する詳細情報が
		/// <see cref="CriAtomEx.VoiceInfoDetail"/> 構造体として渡されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomEx.VoiceInfoCbFunc"/> の説明をご参照ください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で登録したコールバックには、
		/// 発音可能なボイスリソースを持つボイスの情報だけが返されます。
		/// （バーチャル化されたボイスの情報は返されません。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.VoiceInfoCbFunc"/>
		public static unsafe void EnumerateVoiceInfos(delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.VoiceInfoDetail*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomEx_EnumerateVoiceInfos((IntPtr)func, obj);
		}

		/// <summary>ボイス情報コールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイス情報の通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtomEx.EnumerateVoiceInfos"/> 関数に本関数型のコールバック関数を登録することで、
		/// 再生中のボイスの情報をコールバックで受け取ることが可能となります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.EnumerateVoiceInfos"/>
		/// <seealso cref="CriAtomEx.VoiceInfoDetail"/>
		public unsafe class VoiceInfoCbFunc : NativeCallbackBase<VoiceInfoCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ボイスの詳細情報</summary>
				public NativeReference<CriAtomEx.VoiceInfoDetail> voiceInfo { get; }

				internal Arg(NativeReference<CriAtomEx.VoiceInfoDetail> voiceInfo)
				{
					this.voiceInfo = voiceInfo;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtomEx.VoiceInfoDetail* voiceInfo) =>
				InvokeCallbackInternal(obj, new(voiceInfo));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomEx.VoiceInfoDetail* voiceInfo);
			static NativeDelegate callbackDelegate = null;
#endif
			internal VoiceInfoCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomEx.VoiceInfoDetail*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ボイス停止を監視するコールバック関数の登録</summary>
		/// <param name="func">ボイス停止監視コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスの停止を監視するコールバック関数の登録をします。
		/// 本関数を実行すると、第 1 引数（ func ）
		/// でセットされたコールバック関数が監視している再生ID内で発音しているボイスが停止する際に呼び出されます。
		/// コールバック関数には、停止ボイスに関する情報が
		/// <see cref="CriAtomEx.MonitoringVoiceStopInfo"/> 構造体として渡されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomEx.MonitoringVoiceStopCbFunc"/> の説明をご参照ください。
		/// <see cref="CriAtomEx.SetMonitoringVoiceStopPlaybackId"/> 関数にて設定した監視再生IDにて再生中のキューが、
		/// 以下の構造・設定を持つ場合にコールバック関数にて通知される再生IDが監視再生IDとは異なることがあります。
		/// これは再生内部処理ににて別途再生IDが割り振られるためとなります。
		/// - キューリンクを使用している
		/// - シーケンスタイプがトラック遷移タイプのキュー
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.MonitoringVoiceStopCbFunc"/>
		/// <seealso cref="CriAtomEx.SetMonitoringVoiceStopPlaybackId"/>
		public static unsafe void SetMonitoringVoiceStopCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.MonitoringVoiceStopInfo*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomEx_SetMonitoringVoiceStopCallback((IntPtr)func, obj);
		}
		static unsafe void SetMonitoringVoiceStopCallbackInternal(IntPtr func, IntPtr obj) => SetMonitoringVoiceStopCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx.MonitoringVoiceStopInfo*, void>)func, obj);
		static CriAtomEx.MonitoringVoiceStopCbFunc _monitoringVoiceStopCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetMonitoringVoiceStopCallback" />
		public static CriAtomEx.MonitoringVoiceStopCbFunc MonitoringVoiceStopCallback => _monitoringVoiceStopCallback ?? (_monitoringVoiceStopCallback = new CriAtomEx.MonitoringVoiceStopCbFunc(SetMonitoringVoiceStopCallbackInternal));

		/// <summary>ボイス停止を監視するコールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 監視している再生ID内で発音しているボイスが停止した際に使用される、コールバック関数の型です。
		/// </para>
		/// <para>
		/// 注意:
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetMonitoringVoiceStopCallback"/>
		public unsafe class MonitoringVoiceStopCbFunc : NativeCallbackBase<MonitoringVoiceStopCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ボイス停止内容</summary>
				public NativeReference<CriAtomEx.MonitoringVoiceStopInfo> voiceStop { get; }

				internal Arg(NativeReference<CriAtomEx.MonitoringVoiceStopInfo> voiceStop)
				{
					this.voiceStop = voiceStop;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtomEx.MonitoringVoiceStopInfo* voiceStop) =>
				InvokeCallbackInternal(obj, new(voiceStop));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomEx.MonitoringVoiceStopInfo* voiceStop);
			static NativeDelegate callbackDelegate = null;
#endif
			internal MonitoringVoiceStopCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomEx.MonitoringVoiceStopInfo*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ボイス停止情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイス停止情報を取得するための構造体です。
		/// <see cref="CriAtomEx.MonitoringVoiceStopCbFunc"/> 関数型の引数として渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.MonitoringVoiceStopCbFunc"/>
		public unsafe partial struct MonitoringVoiceStopInfo
		{
			/// <summary>再生ID</summary>
			public UInt32 playbackId;

			/// <summary>停止理由</summary>
			public CriAtom.VoiceStopReason reason;

			/// <summary>停止AtomPlayer</summary>
			public IntPtr atomPlayer;

		}
		/// <summary>ボイス停止を監視する再生IDの登録</summary>
		/// <param name="playbackId">ボイス停止を監視する再生ID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスの停止を監視する再生IDを登録をします。
		/// 監視可能な再生IDは１つだけです。
		/// 既に再生IDが設定済みの状態で本関数を呼び出した場合は、監視再生ID情報が上書きされます。
		/// 監視を行うためには<see cref="CriAtomEx.SetMonitoringVoiceStopCallback"/> 関数で通知を行うためのコールバック関数を登録してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 監視再生IDにて再生中のキューのアクショントラックによる新規キューの再生が行われた場合、
		/// この新規キューからの発音ボイスについてはボイス停止の監視対象とはなりません。
		/// これはアクションによる再生開始が、呼び出し元キューとの依存関係を持たない状態で行われるためとなります。
		/// このため、アクションの呼び出し元キューと呼び出し先キューの両方より発音しているボイスの停止を同時に監視することは出来ません。
		/// アクションにて再生を開始したキュー再生IDは <see cref="CriAtomExPlayer.SetPlaybackEventCallback"/> 関数でコールバック関数を
		/// 登録して取得出来ます。適宜、取得・設定を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetMonitoringVoiceStopCallback"/>
		public static void SetMonitoringVoiceStopPlaybackId(CriAtomExPlayback playbackId)
		{
			NativeMethods.criAtomEx_SetMonitoringVoiceStopPlaybackId(playbackId.NativeHandle);
		}

		/// <summary>AISACコントロールIDの無効値</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AISACコントロールIDの無効値です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlById"/>
		/// <seealso cref="CriAtomExAcf.GetAisacControlIdByName"/>
		/// <seealso cref="CriAtomExAcf.GetAisacControlNameById"/>
		public const UInt32 InvalidAisacControlId = (0xffffffff);
		/// <summary>デフォルトボイス数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプール当たりのボイス数のデフォルト値です。
		/// ボイスプール作成時にデフォルト値設定を使用すると、
		/// <see cref="CriAtomEx.DefaultVoicesPerPool"/> 数分のボイスが確保されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExVoicePool.SetDefaultConfigForStandardVoicePool"/>
		public const Int32 DefaultVoicesPerPool = (8);
		/// <summary>最小ボイス数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプール当たりのボイス数の最小値です。
		/// </para>
		/// </remarks>
		public const Int32 MinVoicesPerPool = (1);
		/// <summary>最大ボイス数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスプール当たりのボイス数の最大値です。
		/// </para>
		/// </remarks>
		public const Int32 MaxVoicesPerPool = (32767);
		/// <summary>無効な再生ID</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で音声の再生を開始した際、
		/// ボイスリミットコントロール等によりボイスが確保できなかった場合に返される、
		/// 無効な再生IDです。
		/// </para>
		/// <para>
		/// 備考:
		/// 再生 ID を指定する API に対して本 ID をセットした場合でも、
		/// エラーコールバックは発生しません。
		/// （何もされずに関数からリターンします。）
		/// そのため、 <see cref="CriAtomExPlayer.Start"/> 関数の結果にかかわらず、
		/// 再生 ID を使用した処理を常時行っても、特に問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public const UInt32 InvalidPlaybackId = (0xFFFFFFFF);
		/// <summary>無効なブロックインデックス</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayback.GetCurrentBlockIndex"/> 関数で再生中の音声のカレントブロック
		/// インデックスを取得した際、再生中の音声がブロックシーケンスではない場合に
		/// 返される無効なインデックスです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.GetCurrentBlockIndex"/>
		public const UInt32 InvalidBlockIndex = (0xFFFFFFFF);
		/// <summary>フェードアウト処理の無効化指定値</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェーダーのフェードアウト処理を無効化するための値です。
		/// <see cref="CriAtomExPlayer.SetFadeOutTime"/> 関数の第2引数に本パラメーターをセットすることで、
		/// フェードアウト処理を無効化することが可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFadeOutTime"/>
		public const Int32 IgnoreFadeOut = (-1);
		/// <summary>不正なストリーミングキャッシュID値</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/> 関数に失敗した際に返る値です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExStreamingCache.CriAtomExStreamingCache"/>
		/// <seealso cref="CriAtomExStreamingCache.Destroy"/>
		public const Int32 StreamingCacheIllegalId = (CriAtom.StreamingCacheIllegalId);
		/// <summary>出力ポートの名前の長さの最大値</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExOutputPort.Config"/>::name に名前として指定できる文字列の最大長です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.Config"/>
		public const Int32 OutputPortMaxNameLength = (64);
		/// <summary>HCA</summary>
		public const Int32 FormatHca = (CriAtom.FormatHca);









	}
}