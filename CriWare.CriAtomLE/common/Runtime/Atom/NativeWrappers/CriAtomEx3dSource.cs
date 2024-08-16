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
	/// <summary>3D音源オブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 3D音源を扱うためのオブジェクトです。
	/// 3Dポジショニング機能に使用します。
	/// 3D音源のパラメーター、位置情報の設定等は、3D音源オブジェクトを介して実行されます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomEx3dSource.CriAtomEx3dSource"/>
	public partial class CriAtomEx3dSource : IDisposable, Interfaces.IPositionable
	{
		/// <summary>3D音源の位置のランダム化に関するコンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">3D音源の位置のランダム化に関するコンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置のランダム化に関するコンフィグ構造体（ <see cref="CriAtomEx3dSource.RandomPositionConfig"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionConfig"/>
		public static unsafe void SetDefaultConfigForRandomPosition(out CriAtomEx3dSource.RandomPositionConfig pConfig)
		{
			fixed (CriAtomEx3dSource.RandomPositionConfig* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx3dSource_SetDefaultConfigForRandomPosition_(pConfigPtr);
		}

		/// <summary>3D音源の最小距離／最大距離の設定</summary>
		/// <param name="minAttenuationDistance">最小距離</param>
		/// <param name="maxAttenuationDistance">最大距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の最小距離／最大距離を設定します。
		/// 最小距離は、これ以上音量が大きくならない距離を表します。最大距離は、最小音量になる距離を表します。
		/// ライブラリ初期化時のデフォルト値は以下のとおりです。
		/// - 最小距離：0.0f
		/// - 最大距離：0.0f
		/// デフォルト値は、<see cref="CriAtomEx3dSource.ChangeDefaultMinMaxAttenuationDistance"/> 関数にて変更可能です。
		/// データ側に当該パラメーターが設定されている場合に本関数を呼び出すと、データ側の値を上書き（無視）して適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		/// <seealso cref="CriAtomEx3dSource.ChangeDefaultMinMaxDistance"/>
		public void SetMinMaxDistance(Single minAttenuationDistance, Single maxAttenuationDistance)
		{
			NativeMethods.criAtomEx3dSource_SetMinMaxDistance_(NativeHandle, minAttenuationDistance, maxAttenuationDistance);
		}

		/// <summary>3D音源の最小距離／最大距離のデフォルト値変更</summary>
		/// <param name="minAttenuationDistance">最小距離</param>
		/// <param name="maxAttenuationDistance">最大距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の最小距離／最大距離のデフォルト値を変更します。
		/// 本関数によってデフォルト値を変更すると、以降に作成する3D音源オブジェクト（ <see cref="CriAtomEx3dSource"/> ）の
		/// 最小距離／最大距離の初期値が本関数で設定した値となります。
		/// </para>
		/// <para>
		/// 注意:
		/// 当該パラメーターに関して「ツール側で値が変更されていない（デフォルト状態）」データの場合、暗黙的にデフォルト値が適用されます。
		/// そのため、本関数でデフォルト値を変更すると、ツールでの編集時に意図していたパラメーターと異なってしまう可能性があります。
		/// 但し、以下に該当するデータは本関数の影響を受けません。
		/// - ツールのプロパティにて、最小距離／最大距離の初期値設定を0.0以外に設定している
		/// - インゲームプレビュー用にビルドしている
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetMinMaxDistance"/>
		public static void ChangeDefaultMinMaxDistance(Single minAttenuationDistance, Single maxAttenuationDistance)
		{
			NativeMethods.criAtomEx3dSource_ChangeDefaultMinMaxDistance_(minAttenuationDistance, maxAttenuationDistance);
		}

		/// <summary>3D音源オブジェクト作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">3D音源オブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクト作成用コンフィグ構造体（ <see cref="CriAtomEx3dSource.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomEx3dSource.Config pConfig)
		{
			fixed (CriAtomEx3dSource.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx3dSource_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>3D音源オブジェクトの作成に必要なワーク領域サイズの計算</summary>
		/// <param name="config">3D音源オブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <returns>3D音源オブジェクト作成用ワークサイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトを作成するために必要な、ワーク領域のサイズを取得します。
		/// アロケーターを登録せずに3D音源オブジェクトを作成する場合、
		/// あらかじめ本関数で計算したワーク領域サイズ分のメモリを
		/// ワーク領域として <see cref="CriAtomEx3dSource.CriAtomEx3dSource"/> 関数にセットする必要があります。
		/// 3D音源オブジェクトの作成に必要なワークメモリのサイズは、3D音源オブジェクト作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx3dSource.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomEx3dSource.SetDefaultConfig"/> 適用時と同じパラメーター）で
		/// ワーク領域サイズを計算します。
		/// ワーク領域サイズ計算時に失敗した場合、戻り値は -1 になります。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックの
		/// メッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.CriAtomEx3dSource"/>
		/// <seealso cref="CriAtomEx3dSource.Config"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomEx3dSource.Config config)
		{
			fixed (CriAtomEx3dSource.Config* configPtr = &config)
				return NativeMethods.criAtomEx3dSource_CalculateWorkSize(configPtr);
		}

		/// <summary>3D音源オブジェクト作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトを作成する場合に使用する構造体です。
		/// 将来パラメーターが追加される可能性があるため、
		/// 本構造体を使用する際には <see cref="CriAtomEx3dSource.SetDefaultConfig"/> メソッドを使用し、
		/// 構造体の初期化を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetDefaultConfig"/>
		/// <seealso cref="CriAtomEx3dSource.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dSource.CriAtomEx3dSource"/>
		public unsafe partial struct Config
		{
			/// <summary>距離によるボイスプライオリティ減衰を有効にする</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 距離によるプライオリティ減衰を有効にするかどうかを設定します。
			/// 本パラメーターをtrueに設定して3D音源オブジェクトを作成すると、その3D音源オブジェクトで発音される
			/// 3D音のボイスプライオリティは、リスナーとの距離によって減衰を受けるようになります。
			/// ボイスプライオリティの減衰値は、そのボイスに設定されている最小距離で0、最大距離で-255です。
			/// </para>
			/// <para>
			/// 備考:
			/// 距離によるボイスプライオリティ減衰は、他のボイスプライオリティ設定と加算されて適用されます。
			/// すなわち、最終的なボイスプライオリティは、以下のそれぞれを加算した値になります。
			/// - データに設定されている値
			/// - <see cref="CriAtomExPlayer.SetVoicePriority"/> 関数による設定値
			/// - 距離によるボイスプライオリティ減衰値
			/// 本パラメーターのデフォルト値はfalse（距離によるボイスプライオリティ無効）です。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomExPlayer.SetVoicePriority"/>
			public NativeBool enableVoicePriorityDecay;

			/// <summary>3D音源における位置のランダム化に関する座標リストの要素数の最大値</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 3D音源における位置のランダム化に関する座標リストの要素数の最大値を
			/// 設定します。
			/// 本設定値に従って、内部的に位置座標リストの領域を確保します。
			/// </para>
			/// <para>
			/// 備考:
			/// 3D音源における位置のランダム化を使用しない場合や
			/// <see cref="CriAtomEx3dSource.RandomPositionConfig"/> 構造体の変数 calculation_type に
			/// 対して、 <see cref="CriAtomEx3dSource.RandomPositionCalculationType.List"/> を指定
			/// しない場合、本設定値は 0 を指定してください。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx3dSource.RandomPositionConfig"/>
			public UInt32 randomPositionListMaxLength;

		}
		/// <summary>3D音源オブジェクトの作成</summary>
		/// <param name="config">3D音源オブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <param name="work">3D音源オブジェクト作成用ワーク領域へのポインタ</param>
		/// <param name="workSize">3D音源オブジェクト作成用ワークサイズ</param>
		/// <returns>3D音源オブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクト作成用コンフィグに基づいて、3D音源オブジェクトを作成します。
		/// 作成に成功すると、3D音源オブジェクトを返します。
		/// 3D音源オブジェクトを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomEx3dSource.CalculateWorkSize"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dSource.Dispose"/>
		public unsafe CriAtomEx3dSource(in CriAtomEx3dSource.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx3dSource.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomEx3dSource_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomEx3dSource(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomEx3dSource.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomEx3dSource_Create(configPtr, work, workSize);
		}

		/// <summary>3D音源オブジェクトの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトを破棄します。
		/// 本関数を実行した時点で、3D音源オブジェクト作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定した3D音源オブジェクトも無効になります。
		/// 3D音源オブジェクトをセットしたAtomExプレーヤーで再生している音声がある場合、
		/// 本関数を実行する前に、それらの音声を停止するか、そのAtomExプレーヤーを破棄してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.CriAtomEx3dSource"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomEx3dSource_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomEx3dSource() => Dispose();
#pragma warning restore 1591

		/// <summary>3D音源の位置の設定</summary>
		/// <param name="position">位置ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置を設定します。
		/// 位置は、距離減衰、および定位計算に使用されます。
		/// 位置は、3次元ベクトルで指定します。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。
		/// データ側には位置は設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public unsafe void SetPosition(in CriAtomEx.Vector position)
		{
			fixed (CriAtomEx.Vector* positionPtr = &position)
				NativeMethods.criAtomEx3dSource_SetPosition(NativeHandle, positionPtr);
		}

		/// <summary>3D音源の速度の設定</summary>
		/// <param name="velocity">速度ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の速度を設定します。
		/// 速度は、ドップラー効果の計算に使用されます。
		/// 速度は、3次元ベクトルで指定します。速度の単位は、1秒あたりの移動距離です。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。
		/// データ側には速度は設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public unsafe void SetVelocity(in CriAtomEx.Vector velocity)
		{
			fixed (CriAtomEx.Vector* velocityPtr = &velocity)
				NativeMethods.criAtomEx3dSource_SetVelocity(NativeHandle, velocityPtr);
		}

		/// <summary>3D音源の更新</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源に設定されているパラメーターを使用して、3D音源を更新します。
		/// 本関数では、3D音源に設定されている全てのパラメーターを更新します。
		/// パラメーターをひとつ変更する度に本関数にて更新処理を行うよりも、
		/// 複数のパラメーターを変更してから更新処理を行った方が効率的です。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はAtomExプレーヤーのパラメーター更新（<see cref="CriAtomExPlayer.UpdateAll"/>, <see cref="CriAtomExPlayer.Update"/>）
		/// とは独立して動作します。3D音源のパラメーターを変更した際は、本関数にて更新処理を行ってください。
		/// </para>
		/// </remarks>
		public void Update()
		{
			NativeMethods.criAtomEx3dSource_Update(NativeHandle);
		}

		/// <summary>3D音源パラメーターの初期化</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源に設定されているパラメーターをクリアし、初期値に戻します。
		/// </para>
		/// <para>
		/// 注意:
		/// クリアしたパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void ResetParameters()
		{
			NativeMethods.criAtomEx3dSource_ResetParameters(NativeHandle);
		}

		/// <summary>3D音源の位置の取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置を取得します。
		/// 位置は、3次元ベクトルで取得します。
		/// </para>
		/// </remarks>
		public CriAtomEx.Vector GetPosition()
		{
			return NativeMethods.criAtomEx3dSource_GetPosition(NativeHandle);
		}

		/// <summary>3D音源の向きの設定</summary>
		/// <param name="front">前方ベクトル</param>
		/// <param name="top">上方ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の向きを設定します。
		/// 本関数で設定した向きは、サウンドコーンの向きとして設定されます。
		/// サウンドコーンは、音源から音が発生する方向を表し、音の指向性の表現に使用されます。
		/// サウンドコーンの向きは、3次元ベクトルで指定します。設定された向きベクトルは、ライブラリ内部で正規化して使用されます。
		/// データ側にはサウンドコーンの向きは設定できないため、常に本関数での設定値が使用されます。
		/// デフォルト値は以下のとおりです。
		/// - 前方ベクトル：(0.0f, 0.0f, 1.0f)
		/// - 上方ベクトル：(0.0f, 1.0f, 0.0f)
		/// </para>
		/// <para>
		/// 備考:
		/// サウンドコーンの向きを設定した場合、上方ベクトルは無視され、前方ベクトルのみが使用されます。
		/// また、Ambisonics再生を使用している場合、本関数で指定した向きおよびリスナーの向きに従ってAmbisonicsが回転します。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// また、Ambiosnicsに対してサウンドコーンを適用することは出来ません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetConeParameter"/>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public unsafe void SetOrientation(in CriAtomEx.Vector front, in CriAtomEx.Vector top)
		{
			fixed (CriAtomEx.Vector* frontPtr = &front)
			fixed (CriAtomEx.Vector* topPtr = &top)
				NativeMethods.criAtomEx3dSource_SetOrientation(NativeHandle, frontPtr, topPtr);
		}

		/// <summary>3D音源のサウンドコーンパラメーターの設定</summary>
		/// <param name="insideAngle">サウンドコーンのインサイドアングル</param>
		/// <param name="outsideAngle">サウンドコーンのアウトサイドアングル</param>
		/// <param name="outsideVolume">サウンドコーンのアウトサイドボリューム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のサウンドコーンパラメーターを設定します。
		/// サウンドコーンは、音源から音が発生する方向を表し、音の指向性の表現に使用されます。
		/// サウンドコーンは、内側コーン、外側コーンで構成されます。インサイドアングルは内側コーンの角度、
		/// アウトサイドアングルは外側コーンの角度、アウトサイドボリュームは外側コーンの角度以上の方向での音量をそれぞれ表します。
		/// 内側コーンの角度より小さい角度の方向では、コーンによる減衰を受けません。
		/// 内側コーンと外側コーンの間の方向では、徐々にアウトサイドボリュームまで減衰します。
		/// インサイドアングルおよびアウトサイドアングルは、0.0f～360.0fを度で指定します。
		/// アウトサイドボリュームは、0.0f～1.0fを振幅に対する倍率で指定します（単位はデシベルではありません）。
		/// ライブラリ初期化時のデフォルト値は以下のとおりであり、コーンによる減衰は行われません。
		/// - インサイドアングル：360.0f
		/// - アウトサイドアングル：360.0f
		/// - アウトサイドボリューム：0.0f
		/// デフォルト値は、<see cref="CriAtomEx3dSource.ChangeDefaultConeParameter"/> 関数にて変更可能です。
		/// データ側に当該パラメーターが設定されている場合に本関数を呼び出すと、以下のように適用されます。
		/// - インサイドアングル：加算
		/// - アウトサイドアングル：加算
		/// - アウトサイドボリューム：乗算
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		/// <seealso cref="CriAtomEx3dSource.ChangeDefaultConeParameter"/>
		public void SetConeParameter(Single insideAngle, Single outsideAngle, Single outsideVolume)
		{
			NativeMethods.criAtomEx3dSource_SetConeParameter(NativeHandle, insideAngle, outsideAngle, outsideVolume);
		}

		/// <summary>3D音源のサウンドコーンパラメーターのデフォルト値変更</summary>
		/// <param name="insideAngle">サウンドコーンのインサイドアングル</param>
		/// <param name="outsideAngle">サウンドコーンのアウトサイドアングル</param>
		/// <param name="outsideVolume">サウンドコーンのアウトサイドボリューム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のサウンドコーンパラメーターのデフォルト値を変更します。
		/// 本関数によってデフォルト値を変更すると、以降に作成する3D音源オブジェクト（ <see cref="CriAtomEx3dSource"/> ）の
		/// サウンドコーンパラメーターの初期値が本関数で設定した値となります。
		/// ライブラリ初期化時のデフォルト値については、 <see cref="CriAtomEx3dSource.SetConeParameter"/> 関数を参照して下さい。
		/// </para>
		/// <para>
		/// 注意:
		/// 当該パラメーターに関して「ツール側で値が変更されていない（デフォルト状態）」データの場合、暗黙的にデフォルト値が適用されます。
		/// そのため、本関数でデフォルト値を変更すると、ツールでの編集時に意図していたパラメーターと異なってしまう可能性があります。
		/// 但し、インゲームプレビュー用にビルドされたデータは本関数の影響を受けません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetConeParameter"/>
		public static void ChangeDefaultConeParameter(Single insideAngle, Single outsideAngle, Single outsideVolume)
		{
			NativeMethods.criAtomEx3dSource_ChangeDefaultConeParameter(insideAngle, outsideAngle, outsideVolume);
		}

		/// <summary>3D音源の最小距離／最大距離の設定</summary>
		/// <param name="minAttenuationDistance">最小距離</param>
		/// <param name="maxAttenuationDistance">最大距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の最小距離／最大距離を設定します。
		/// 最小距離は、これ以上音量が大きくならない距離を表します。最大距離は、最小音量になる距離を表します。
		/// ライブラリ初期化時のデフォルト値は以下のとおりです。
		/// - 最小距離：0.0f
		/// - 最大距離：0.0f
		/// デフォルト値は、<see cref="CriAtomEx3dSource.ChangeDefaultMinMaxAttenuationDistance"/> 関数にて変更可能です。
		/// データ側に当該パラメーターが設定されている場合に本関数を呼び出すと、データ側の値を上書き（無視）して適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		/// <seealso cref="CriAtomEx3dSource.ChangeDefaultMinMaxAttenuationDistance"/>
		public void SetMinMaxAttenuationDistance(Single minAttenuationDistance, Single maxAttenuationDistance)
		{
			NativeMethods.criAtomEx3dSource_SetMinMaxAttenuationDistance(NativeHandle, minAttenuationDistance, maxAttenuationDistance);
		}

		/// <summary>3D音源の最小距離／最大距離のデフォルト値変更</summary>
		/// <param name="minAttenuationDistance">最小距離</param>
		/// <param name="maxAttenuationDistance">最大距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の最小距離／最大距離のデフォルト値を変更します。
		/// 本関数によってデフォルト値を変更すると、以降に作成する3D音源オブジェクト（ <see cref="CriAtomEx3dSource"/> ）の
		/// 最小距離／最大距離の初期値が本関数で設定した値となります。
		/// ライブラリ初期化時のデフォルト値については、 <see cref="CriAtomEx3dSource.SetMinMaxAttenuationDistance"/> 関数を参照して下さい。
		/// </para>
		/// <para>
		/// 注意:
		/// 当該パラメーターに関して「ツール側で値が変更されていない（デフォルト状態）」データの場合、暗黙的にデフォルト値が適用されます。
		/// そのため、本関数でデフォルト値を変更すると、ツールでの編集時に意図していたパラメーターと異なってしまう可能性があります。
		/// 但し、以下に該当するデータは本関数の影響を受けません。
		/// - ツールのプロパティにて、最小距離／最大距離の初期値設定を0.0以外に設定している
		/// - インゲームプレビュー用にビルドしている
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetMinMaxAttenuationDistance"/>
		public static void ChangeDefaultMinMaxAttenuationDistance(Single minAttenuationDistance, Single maxAttenuationDistance)
		{
			NativeMethods.criAtomEx3dSource_ChangeDefaultMinMaxAttenuationDistance(minAttenuationDistance, maxAttenuationDistance);
		}

		/// <summary>3D音源のインテリアパンニング境界距離の設定</summary>
		/// <param name="sourceRadius">3D音源の半径</param>
		/// <param name="interiorDistance">インテリア距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のインテリアパンニング境界距離の設定をします。
		/// 3D音源の半径は、3D音源を球としたときの半径です。
		/// インテリア距離は、インテリアパンニング適用される3D音源の半径からの距離です。
		/// 3D音源の半径内では、インテリアパンニング適用されますが、インテリア距離が0.0と扱われるため、
		/// 全てのスピーカーから同じ音量で音声が再生されます。
		/// インテリア距離内では、インテリアパンニング適用されます。
		/// インテリア距離外では、インテリアパンニング適用されず、音源位置に最も近い1つ、
		/// または2つのスピーカーから音声が再生されます。
		/// ライブラリ初期化時のデフォルト値は以下のとおりです。
		/// - 3D音源の半径：0.0f
		/// - インテリア距離：0.0f（3D音源の最小距離に依存）
		/// デフォルト値は、 <see cref="CriAtomEx3dSource.ChangeDefaultInteriorPanField"/> 関数にて変更可能です。
		/// また、現在ツールにて当該パラメーターを設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		/// <seealso cref="CriAtomEx3dSource.ChangeDefaultInteriorPanField"/>
		public void SetInteriorPanField(Single sourceRadius, Single interiorDistance)
		{
			NativeMethods.criAtomEx3dSource_SetInteriorPanField(NativeHandle, sourceRadius, interiorDistance);
		}

		/// <summary>3D音源のインテリアパンニング境界距離のデフォルト値変更</summary>
		/// <param name="sourceRadius">3D音源の半径</param>
		/// <param name="interiorDistance">インテリア距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のインテリアパンニング境界距離のデフォルト値を変更します。
		/// 本関数によってデフォルト値を変更すると、以降に作成する3D音源オブジェクト（ <see cref="CriAtomEx3dSource"/> ）の
		/// インテリアパンニング境界距離の初期値が本関数で設定した値となります。
		/// ライブラリ初期化時のデフォルト値については、 <see cref="CriAtomEx3dSource.SetInteriorPanField"/> 関数を参照して下さい。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetInteriorPanField"/>
		public static void ChangeDefaultInteriorPanField(Single sourceRadius, Single interiorDistance)
		{
			NativeMethods.criAtomEx3dSource_ChangeDefaultInteriorPanField(sourceRadius, interiorDistance);
		}

		/// <summary>3D音源のドップラー係数の設定</summary>
		/// <param name="dopplerFactor">ドップラー係数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のドップラー係数を設定します。
		/// ドップラー係数は、音速を340m/sとして計算されたドップラー効果に対して、誇張表現するための倍率を指定します。
		/// 例えば、2.0fを指定すると、音速を340m/sとして計算したピッチを2倍して適用します。
		/// 0.0fを指定すると、ドップラー効果は無効になります。
		/// ライブラリ初期化時のデフォルト値は0.0fです。
		/// デフォルト値は、<see cref="CriAtomEx3dSource.ChangeDefaultDopplerFactor"/> 関数にて変更可能です。
		/// データ側に当該パラメーターが設定されている場合に本関数を呼び出すと、データ側の値を上書き（無視）して適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		/// <seealso cref="CriAtomEx3dSource.ChangeDefaultDopplerFactor"/>
		public void SetDopplerFactor(Single dopplerFactor)
		{
			NativeMethods.criAtomEx3dSource_SetDopplerFactor(NativeHandle, dopplerFactor);
		}

		/// <summary>3D音源のドップラー係数のデフォルト値変更</summary>
		/// <param name="dopplerFactor">ドップラー係数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のドップラー係数のデフォルト値を変更します。
		/// 本関数によってデフォルト値を変更すると、以降に作成する3D音源オブジェクト（ <see cref="CriAtomEx3dSource"/> ）の
		/// ドップラー係数の初期値が本関数で設定した値となります。
		/// ライブラリ初期化時のデフォルト値については、 <see cref="CriAtomEx3dSource.SetDopplerFactor"/> 関数を参照して下さい。
		/// </para>
		/// <para>
		/// 注意:
		/// 当該パラメーターに関して「ツール側で値が変更されていない（デフォルト状態）」データの場合、暗黙的にデフォルト値が適用されます。
		/// そのため、本関数でデフォルト値を変更すると、ツールでの編集時に意図していたパラメーターと異なってしまう可能性があります。
		/// 但し、インゲームプレビュー用にビルドされたデータは本関数の影響を受けません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetDopplerFactor"/>
		public static void ChangeDefaultDopplerFactor(Single dopplerFactor)
		{
			NativeMethods.criAtomEx3dSource_ChangeDefaultDopplerFactor(dopplerFactor);
		}

		/// <summary>3D音源のボリュームの設定</summary>
		/// <param name="volume">ボリューム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のボリュームを設定します。
		/// 3D音源のボリュームは、定位に関わる音量（L,R,SL,SR）にのみ影響し、LFEやセンターへの出力レベルには影響しません。
		/// ボリューム値には、0.0f～1.0fの範囲で実数値を指定します。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのボリュームで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// ライブラリ初期化時のデフォルト値は1.0fです。
		/// デフォルト値は、<see cref="CriAtomEx3dSource.ChangeDefaultVolume"/> 関数にて変更可能です。
		/// データ側に当該パラメーターが設定されている場合に本関数を呼び出すと、データ側の値と乗算して適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		/// <seealso cref="CriAtomEx3dSource.ChangeDefaultVolume"/>
		public void SetVolume(Single volume)
		{
			NativeMethods.criAtomEx3dSource_SetVolume(NativeHandle, volume);
		}

		/// <summary>3D音源のボリュームのデフォルト値変更</summary>
		/// <param name="volume">ボリューム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のボリュームのデフォルト値を変更します。
		/// 本関数によってデフォルト値を変更すると、以降に作成する3D音源オブジェクト（ <see cref="CriAtomEx3dSource"/> ）の
		/// ボリュームの初期値が本関数で設定した値となります。
		/// ライブラリ初期化時のデフォルト値については、 <see cref="CriAtomEx3dSource.SetVolume"/> 関数を参照して下さい。
		/// </para>
		/// <para>
		/// 注意:
		/// 当該パラメーターに関して「ツール側で値が変更されていない（デフォルト状態）」データの場合、暗黙的にデフォルト値が適用されます。
		/// そのため、本関数でデフォルト値を変更すると、ツールでの編集時に意図していたパラメーターと異なってしまう可能性があります。
		/// 但し、インゲームプレビュー用にビルドされたデータは本関数の影響を受けません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetVolume"/>
		public static void ChangeDefaultVolume(Single volume)
		{
			NativeMethods.criAtomEx3dSource_ChangeDefaultVolume(volume);
		}

		/// <summary>角度AISACコントロール値の最大変化量の設定</summary>
		/// <param name="maxDelta">角度AISACコントロール値の最大変化量</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 角度AISACによりAISACコントロール値が変更される際の、最大変化量を設定します。
		/// 最大変化量を低めに変更すると、音源とリスナー間の相対角度が急激に変わった場合でも、
		/// 角度AISACによるAISACコントロール値の変化をスムーズにすることができます。
		/// 例えば、(0.5f / 30.0f)を設定すると、角度が0度→180度に変化した場合に、30フレームかけて変化するような変化量となります。
		/// デフォルト値は1.0f（制限なし）です。
		/// データ側では本パラメーターは設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数で設定している最大変化量は、定位角度を元に計算されている、角度AISACコントロール値の変化にのみ適用されます。
		/// 定位角度自体には影響はありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void SetMaxAngleAisacDelta(Single maxDelta)
		{
			NativeMethods.criAtomEx3dSource_SetMaxAngleAisacDelta(NativeHandle, maxDelta);
		}

		/// <summary>距離AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">距離AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 最小距離、最大距離間の距離減衰に連動するAISACコントロールIDを指定します。
		/// 本関数でAISACコントロールIDを設定した場合、デフォルトの距離減衰は無効になります。
		/// データ側に設定されている距離AISACコントロールIDは、本関数によって上書き適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数で指定した AISAC コントロール ID に対するコントロール値は、
		/// 以下関数のいずれかを使用してプレーヤーに設定したコントロール値より、優先して適用されます。
		/// - <see cref="CriAtomExPlayer.SetAisacControlById"/> 関数
		/// - <see cref="CriAtomExPlayer.SetAisacControlByName"/> 関数
		/// 再生中の 3D 音源に対して AISAC コントロール ID の変更を行った場合、
		/// 変更前のコントロール ID に対する最終コントロール値が適用され続けます。
		/// このため、変更前／変更後の２つのパラメーターが適用され、意図した出力が得られません。
		/// 指定する3D音源に紐づく音声が再生されていない状態で本関数を実行することを推奨します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void SetDistanceAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dSource_SetDistanceAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>リスナー基準方位角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">リスナー基準方位角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// リスナーから見た音源の方位角に連動するAISACコントロールIDを指定します。
		/// データ側に設定されているリスナー基準方位角AISACコントロールIDは、本関数によって上書き適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数で指定した AISAC コントロール ID に対するコントロール値は、
		/// 以下関数のいずれかを使用してプレーヤーに設定したコントロール値より、優先して適用されます。
		/// - <see cref="CriAtomExPlayer.SetAisacControlById"/> 関数
		/// - <see cref="CriAtomExPlayer.SetAisacControlByName"/> 関数
		/// 再生中の 3D 音源に対して AISAC コントロール ID の変更を行った場合、
		/// 変更前のコントロール ID に対する最終コントロール値が適用され続けます。
		/// このため、変更前／変更後の２つのパラメーターが適用され、意図した出力が得られません。
		/// 指定する3D音源に紐づく音声が再生されていない状態で本関数を実行することを推奨します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void SetListenerBasedAzimuthAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dSource_SetListenerBasedAzimuthAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>リスナー基準仰俯角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">リスナー基準仰俯角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// リスナーから見た音源の仰俯角に連動するAISACコントロールIDを指定します。
		/// データ側に設定されているリスナー基準仰俯角AISACコントロールIDは、本関数によって上書き適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数で指定した AISAC コントロール ID に対するコントロール値は、
		/// 以下関数のいずれかを使用してプレーヤーに設定したコントロール値より、優先して適用されます。
		/// - <see cref="CriAtomExPlayer.SetAisacControlById"/> 関数
		/// - <see cref="CriAtomExPlayer.SetAisacControlByName"/> 関数
		/// 再生中の 3D 音源に対して AISAC コントロール ID の変更を行った場合、
		/// 変更前のコントロール ID に対する最終コントロール値が適用され続けます。
		/// このため、変更前／変更後の２つのパラメーターが適用され、意図した出力が得られません。
		/// 指定する3D音源に紐づく音声が再生されていない状態で本関数を実行することを推奨します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void SetListenerBasedElevationAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dSource_SetListenerBasedElevationAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>音源基準方位角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">音源基準方位角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音源から見たリスナーの方位角に連動するAISACコントロールIDを指定します。
		/// データ側に設定されている音源基準方位角AISACコントロールIDは、本関数によって上書き適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数で指定した AISAC コントロール ID に対するコントロール値は、
		/// 以下関数のいずれかを使用してプレーヤーに設定したコントロール値より、優先して適用されます。
		/// - <see cref="CriAtomExPlayer.SetAisacControlById"/> 関数
		/// - <see cref="CriAtomExPlayer.SetAisacControlByName"/> 関数
		/// 再生中の 3D 音源に対して AISAC コントロール ID の変更を行った場合、
		/// 変更前のコントロール ID に対する最終コントロール値が適用され続けます。
		/// このため、変更前／変更後の２つのパラメーターが適用され、意図した出力が得られません。
		/// 指定する3D音源に紐づく音声が再生されていない状態で本関数を実行することを推奨します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void SetSourceBasedAzimuthAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dSource_SetSourceBasedAzimuthAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>音源基準仰俯角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">音源基準仰俯角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音源から見たリスナーの仰俯角に連動するAISACコントロールIDを指定します。
		/// データ側に設定されている音源基準仰俯角AISACコントロールIDは、本関数によって上書き適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数で指定した AISAC コントロール ID に対するコントロール値は、
		/// 以下関数のいずれかを使用してプレーヤーに設定したコントロール値より、優先して適用されます。
		/// - <see cref="CriAtomExPlayer.SetAisacControlById"/> 関数
		/// - <see cref="CriAtomExPlayer.SetAisacControlByName"/> 関数
		/// 再生中の 3D 音源に対して AISAC コントロール ID の変更を行った場合、
		/// 変更前のコントロール ID に対する最終コントロール値が適用され続けます。
		/// このため、変更前／変更後の２つのパラメーターが適用され、意図した出力が得られません。
		/// 指定する3D音源に紐づく音声が再生されていない状態で本関数を実行することを推奨します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void SetSourceBasedElevationAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dSource_SetSourceBasedElevationAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>3D音源オブジェクトに対する3Dリージョンオブジェクトの設定</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトに対して3Dリージョンオブジェクトを設定します。
		/// *
		/// </para>
		/// <para>
		/// 注意:
		/// 同一のExPlayerに設定されている3D音源と3Dリスナーに設定されているリージョンが異なり、
		/// かつ3D音源と同じリージョンが設定されている3Dトランシーバーがない場合、音声はミュートされます。
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/>
		/// <seealso cref="CriAtomEx3dSource.Update"/>
		public void Set3dRegionHn(CriAtomEx3dRegion ex3dRegion)
		{
			NativeMethods.criAtomEx3dSource_Set3dRegionHn(NativeHandle, ex3dRegion?.NativeHandle ?? default);
		}

		/// <summary>3D音源に対する位置のランダム化の設定</summary>
		/// <param name="config">3D音源の位置のランダム化に関するコンフィグ構造体のポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源オブジェクトに対して位置のランダム化の設定をします。
		/// 本関数を実行すると、再生時に音声の位置が元の位置情報および設定値に従って
		/// ランダムに変化します。
		/// </para>
		/// <para>
		/// 備考:
		/// 第2引数 config に対して null を指定すると、設定を解除することが可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数は再生中の音声に対してパラメーターは適用されません。
		/// 次回再生の音声から適用されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionConfig"/>
		public unsafe void SetRandomPositionConfig(in CriAtomEx3dSource.RandomPositionConfig config)
		{
			fixed (CriAtomEx3dSource.RandomPositionConfig* configPtr = &config)
				NativeMethods.criAtomEx3dSource_SetRandomPositionConfig(NativeHandle, configPtr);
		}

		/// <summary>3D音源の位置のランダム化に関するコンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置のランダム化に関する設定をまとめた構造体です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomEx3dSource.SetDefaultConfigForRandomPosition"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetRandomPositionConfig"/>
		/// <seealso cref="CriAtomEx3dSource.SetDefaultConfigForRandomPosition"/>
		public unsafe partial struct RandomPositionConfig
		{
			/// <summary>元の3D音源に追従するかどうか</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ランダムに決定された3D音源が元の3D音源に追従して動くかどうかを設定します。
			/// trueの場合は3D音源の位置や向きに追従します。
			/// falseの場合は3D音源に追従せず、再生開始時の位置に留まります。
			/// </para>
			/// </remarks>
			public NativeBool followsOriginalSource;

			/// <summary>座標の算出方法</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ランダムな座標を決定する際の算出方法を設定します。
			/// 詳細は <see cref="CriAtomEx3dSource.RandomPositionCalculationType"/> を参照してください。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx3dSource.RandomPositionCalculationType"/>
			public CriAtomEx3dSource.RandomPositionCalculationType calculationType;

			/// <summary>座標の算出方法に関する各種パラメーター配列</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 各座標の算出方法にて使用するパラメーター配列です。
			/// calculation_type に設定した座標の算出方法に対するパラメーター配列に
			/// 対する各要素の設定は以下の通りです。
			/// - <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Rectangle"/>
			/// - 0: 左右幅(x軸), 1: 前後幅(z軸), 2: 0.0f
			/// - <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Cuboid"/>
			/// - 0: 左右幅(x軸), 1: 前後幅(z軸), 2: 上下幅(y軸)
			/// - <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Circle"/>
			/// - 0: 半径, 1: 0.0f, 2: 0.0f
			/// - <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Cylinder"/>
			/// - 0: 半径, 1: 上下幅(y軸), 2: 0.0f
			/// - <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Sphere"/>
			/// - 0: 半径, 1: 0.0f, 2: 0.0f
			/// .
			/// なお、各図形は元の3D音源を中心とします。
			/// </para>
			/// <para>
			/// 備考:
			/// 以下の座標の算出方法では、本パラメーターは無視されます。
			/// - <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Callback"/>
			/// - <see cref="CriAtomEx3dSource.RandomPositionCalculationType.List"/>
			/// .
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx3dSource.RandomPositionCalculationType"/>
			public InlineArray3<Single> calculationParameters;

		}
		/// <summary>3D音源の位置のランダム化における位置座標の算出方法</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置のランダム化における位置座標の算出方法の定義です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionConfig"/>
		public enum RandomPositionCalculationType
		{
			/// <summary>設定無し</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 座標算出処理を行いません。
			/// </para>
			/// <para>
			/// 注意:
			/// 本定義は情報取得用にのみ用いられます。
			/// そのため、本設定値を <see cref="CriAtomEx3dSource.RandomPositionConfig"/> 構造体に
			/// 指定して <see cref="CriAtomEx3dSource.SetRandomPositionConfig"/> 関数を実行した場合、
			/// エラーが発生します。
			/// </para>
			/// </remarks>
			None = -1,
			/// <summary>矩形</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// xz 平面上の矩形内で座標を算出します。
			/// </para>
			/// </remarks>
			Rectangle = 0,
			/// <summary>直方体</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// xyz 空間上の直方体内で座標を算出します。
			/// </para>
			/// </remarks>
			Cuboid = 1,
			/// <summary>円</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// xz 平面上の円内で座標を算出します。
			/// </para>
			/// </remarks>
			Circle = 2,
			/// <summary>円柱</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// xyz 空間上の円柱内で座標を算出します。
			/// </para>
			/// </remarks>
			Cylinder = 3,
			/// <summary>球</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// xyz 空間上の球内で座標を算出します。
			/// </para>
			/// </remarks>
			Sphere = 4,
			/// <summary>コールバック</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ユーザー定義のコールバック関数内にて座標を決定します。
			/// </para>
			/// <para>
			/// 備考:
			/// 別途座標算出を行うコールバック関数の登録が必要となります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx3dSource.RandomPositionCalculationCbFunc"/>
			/// <seealso cref="CriAtomEx3dSource.SetRandomPositionCalculationCallback"/>
			Callback = 5,
			/// <summary>座標リスト</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 事前に設定した座標リストを元に座標を決定します。
			/// </para>
			/// <para>
			/// 備考:
			/// 別途座標リストの設定が必要となります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomEx3dSource.SetRandomPositionList"/>
			List = 6,
		}
		/// <summary>3D音源の位置のランダム化における位置座標算出コールバック関数の登録</summary>
		/// <param name="func">3D音源のランダム化における位置座標算出コールバック関数</param>
		/// <param name="obj">ユーザー指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源に対して、3D音源の位置のランダム化における位置座標算出コールバック関数を登録します。
		/// 登録したコールバック関数は、3D音源の位置のランダム化が有効な場合、
		/// 座標算出を行うタイミングで実行されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 3D音源の位置のランダム化に関するコンフィグ構造体である
		/// <see cref="CriAtomEx3dSource.RandomPositionConfig"/> の変数 calculation_type に対して
		/// <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Callback"/>
		/// を設定している場合のみ、座標算出時に指定したコールバック関数を実行します。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック登録の適用には、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// コールバック関数は各3D音源のオブジェクトにつき、1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionCalculationCbFunc"/>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionConfig"/>
		public unsafe void SetRandomPositionCalculationCallback(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, CriAtomEx.Vector*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomEx3dSource_SetRandomPositionCalculationCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetRandomPositionCalculationCallbackInternal(IntPtr func, IntPtr obj) => SetRandomPositionCalculationCallback((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, CriAtomEx.Vector*, void>)func, obj);
		CriAtomEx3dSource.RandomPositionCalculationCbFunc _randomPositionCalculationCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetRandomPositionCalculationCallback" />
		public CriAtomEx3dSource.RandomPositionCalculationCbFunc RandomPositionCalculationCallback => _randomPositionCalculationCallback ?? (_randomPositionCalculationCallback = new CriAtomEx3dSource.RandomPositionCalculationCbFunc(SetRandomPositionCalculationCallbackInternal));

		/// <summary>3D音源の位置のランダム化における位置座標算出コールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源のランダム化における位置座標算出コールバック関数の型です。
		/// <see cref="CriAtomEx3dSource.SetRandomPositionCalculationCallback"/> 関数を実行することで、
		/// コールバック関数の登録が可能です。
		/// 本コールバック関数は、3D音源の位置のランダム化における位置座標を算出する際に
		/// 呼び出されます。
		/// アプリケーション側で位置座標を決定したい場合にご利用ください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// 本コールバック関数は <see cref="CriAtomEx3dSource.RandomPositionConfig"/> 構造体に
		/// <see cref="CriAtomEx3dSource.RandomPositionCalculationType.Callback"/> を指定したときのみ呼び出されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionConfig"/>
		/// <seealso cref="CriAtomEx3dSource.SetRandomPositionCalculationCallback"/>
		public unsafe class RandomPositionCalculationCbFunc : NativeCallbackBase<RandomPositionCalculationCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>元の3D音源のオブジェクト</summary>
				public IntPtr ex3dSource { get; }
				/// <summary>処理結果の位置</summary>
				public NativeReference<CriAtomEx.Vector> resultPos { get; }

				internal Arg(IntPtr ex3dSource, NativeReference<CriAtomEx.Vector> resultPos)
				{
					this.ex3dSource = ex3dSource;
					this.resultPos = resultPos;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr ex3dSource, CriAtomEx.Vector* resultPos) =>
				InvokeCallbackInternal(obj, new(ex3dSource, resultPos));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr ex3dSource, CriAtomEx.Vector* resultPos);
			static NativeDelegate callbackDelegate = null;
#endif
			internal RandomPositionCalculationCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, CriAtomEx.Vector*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>3D音源の位置のランダム化における位置座標リストの設定</summary>
		/// <param name="positionList">位置座標リスト</param>
		/// <param name="length">リストの要素数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 位置座標リストの配列を指定します。
		/// 3D音源の位置のランダム化が有効の場合、指定した配列の座標からランダムに座標が決定されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 位置座標の算出方法として <see cref="CriAtomEx3dSource.RandomPositionCalculationType.List"/>
		/// を設定している場合のみ、本パラメーターを参照します。
		/// その他の算出方法を設定している場合、本パラメーターは無視されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// 本関数に設定した領域は、 <see cref="CriAtomEx3dSource.SetRandomPositionList"/> 関数
		/// の実行完了後は、内部からは参照されません。
		/// 代わりに内部で確保した位置座標リストに保存します。
		/// そのため、 <see cref="CriAtomEx3dSource.Config"/>::random_position_list_max_length を
		/// 超える値を設定するとエラーが発生します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionConfig"/>
		/// <seealso cref="CriAtomEx3dSource.Config"/>
		public unsafe void SetRandomPositionList(in CriAtomEx.Vector positionList, UInt32 length)
		{
			fixed (CriAtomEx.Vector* positionListPtr = &positionList)
				NativeMethods.criAtomEx3dSource_SetRandomPositionList(NativeHandle, positionListPtr, length);
		}

		/// <summary>3D音源の位置のランダム化における位置座標結果コールバック関数の登録</summary>
		/// <param name="func">3D音源のランダム化における位置座標結果コールバック関数</param>
		/// <param name="obj">ユーザー指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置のランダム化における位置座標結果コールバック関数を登録します。
		/// 登録したコールバック関数は、3D音源の位置のランダム化が有効な場合、
		/// 座標算出が行われた後のタイミングで実行されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 追従設定が有効の場合、元のソースの移動に応じて本コールバック関数が実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック登録の適用には、<see cref="CriAtomEx3dSource.Update"/> 関数を呼び出す必要があります。
		/// サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionResultCbFunc"/>
		public unsafe void SetRandomPositionResultCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx3dSource.RandomPositionResultInfoDetail*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomEx3dSource_SetRandomPositionResultCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetRandomPositionResultCallbackInternal(IntPtr func, IntPtr obj) => SetRandomPositionResultCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomEx3dSource.RandomPositionResultInfoDetail*, void>)func, obj);
		CriAtomEx3dSource.RandomPositionResultCbFunc _randomPositionResultCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetRandomPositionResultCallback" />
		public CriAtomEx3dSource.RandomPositionResultCbFunc RandomPositionResultCallback => _randomPositionResultCallback ?? (_randomPositionResultCallback = new CriAtomEx3dSource.RandomPositionResultCbFunc(SetRandomPositionResultCallbackInternal));

		/// <summary>3D音源の位置のランダム化における位置座標結果コールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置のランダム化における位置座標結果コールバック関数の型です。
		/// <see cref="CriAtomEx3dSource.SetRandomPositionResultCallback"/> 関数を実行することで、
		/// コールバック関数の登録が可能です。
		/// 本コールバック関数は、3D音源の位置のランダム化が有効な場合、位置座標の算出が行われた後に
		/// 呼び出されます。
		/// アプリケーション側でランダム化された位置座標情報を取得したい場合にご利用ください。
		/// </para>
		/// <para>
		/// 備考:
		/// 元の3D音源に追従する設定が有効の場合、元の3D音源の位置の変更に応じて本コールバック関数が実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.SetRandomPositionResultCallback"/>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionResultInfoDetail"/>
		public unsafe class RandomPositionResultCbFunc : NativeCallbackBase<RandomPositionResultCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>位置座標結果の詳細</summary>
				public NativeReference<CriAtomEx3dSource.RandomPositionResultInfoDetail> resultInfo { get; }

				internal Arg(NativeReference<CriAtomEx3dSource.RandomPositionResultInfoDetail> resultInfo)
				{
					this.resultInfo = resultInfo;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtomEx3dSource.RandomPositionResultInfoDetail* resultInfo) =>
				InvokeCallbackInternal(obj, new(resultInfo));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomEx3dSource.RandomPositionResultInfoDetail* resultInfo);
			static NativeDelegate callbackDelegate = null;
#endif
			internal RandomPositionResultCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomEx3dSource.RandomPositionResultInfoDetail*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>3D音源の位置のランダム化における位置座標結果の詳細</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3D音源の位置のランダム化における位置座標の結果出力時に、当該再生に関する詳細情報を通知するための構造体です。
		/// 3D音源の位置のランダム化における位置座標結果コールバック関数に引数として渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.RandomPositionResultCbFunc"/>
		public unsafe partial struct RandomPositionResultInfoDetail
		{
			/// <summary>元の3D音源のオブジェクト</summary>
			public IntPtr ex3dSource;

			/// <summary>最終的な位置座標</summary>
			public CriAtomEx.Vector resultPos;

			/// <summary>元の3D音源の位置に対するオフセット座標</summary>
			public CriAtomEx.Vector offsetPos;

		}
		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomEx3dSource(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomEx3dSource other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomEx3dSource a, CriAtomEx3dSource b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomEx3dSource a, CriAtomEx3dSource b) =>
			!(a == b);

	}
}