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
	/// <summary>3Dトランシーバーオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 3Dトランシーバーを扱うためのオブジェクトです。
	/// 3Dトランシーバー機能に使用します。
	/// 3Dトランシーバーのパラメーター、位置情報の設定等は、3Dトランシーバーオブジェクトを介して実行されます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomEx3dTransceiver.CriAtomEx3dTransceiver"/>
	public partial class CriAtomEx3dTransceiver : IDisposable
	{
		/// <summary>3Dトランシーバーオブジェクト作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">3Dトランシーバーオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーオブジェクト作成用コンフィグ構造体（ <see cref="CriAtomEx3dTransceiver.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomEx3dTransceiver.Config pConfig)
		{
			fixed (CriAtomEx3dTransceiver.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx3dTransceiver_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>3Dトランシーバーオブジェクトの作成に必要なワーク領域サイズの計算</summary>
		/// <param name="config">3Dトランシーバーオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <returns>3Dトランシーバーオブジェクト作成用ワークサイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーオブジェクトを作成するために必要な、ワーク領域のサイズを取得します。
		/// アロケーターを登録せずに3Dトランシーバーオブジェクトを作成する場合、
		/// あらかじめ本関数で計算したワーク領域サイズ分のメモリを
		/// ワーク領域として <see cref="CriAtomEx3dTransceiver.CriAtomEx3dTransceiver"/> 関数にセットする必要があります。
		/// 3Dトランシーバーオブジェクトの作成に必要なワークメモリのサイズは、3Dトランシーバーオブジェクト作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx3dTransceiver.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomEx3dTransceiver.SetDefaultConfig"/> 適用時と同じパラメーター）で
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
		/// <seealso cref="CriAtomEx3dTransceiver.CriAtomEx3dTransceiver"/>
		/// <seealso cref="CriAtomEx3dTransceiver.Config"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomEx3dTransceiver.Config config)
		{
			fixed (CriAtomEx3dTransceiver.Config* configPtr = &config)
				return NativeMethods.criAtomEx3dTransceiver_CalculateWorkSize(configPtr);
		}

		/// <summary>3Dトランシーバーオブジェクト作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーオブジェクトを作成する場合に使用する構造体です。
		/// 現状指定可能なパラメーターはありませんが、将来パラメーターが追加される可能性があるため、
		/// 本構造体を使用する際には <see cref="CriAtomEx3dTransceiver.SetDefaultConfig"/> メソッドを使用し、
		/// 構造体の初期化を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.SetDefaultConfig"/>
		/// <seealso cref="CriAtomEx3dTransceiver.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dTransceiver.CriAtomEx3dTransceiver"/>
		public unsafe partial struct Config
		{
			/// <summary>予約値（0を指定してください）</summary>
			public Int32 reserved;

		}
		/// <summary>3Dトランシーバーオブジェクトの作成</summary>
		/// <param name="config">3Dトランシーバーオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <param name="work">3Dトランシーバーオブジェクト作成用ワーク領域へのポインタ</param>
		/// <param name="workSize">3Dトランシーバーオブジェクト作成用ワークサイズ</param>
		/// <returns>3Dトランシーバーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーオブジェクト作成用コンフィグに基づいて、3Dトランシーバーオブジェクトを作成します。
		/// 作成に成功すると、3Dトランシーバーオブジェクトを返します。
		/// 3Dトランシーバーオブジェクトを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomEx3dTransceiver.CalculateWorkSize"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dTransceiver.Dispose"/>
		public unsafe CriAtomEx3dTransceiver(in CriAtomEx3dTransceiver.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx3dTransceiver.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomEx3dTransceiver_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomEx3dTransceiver(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomEx3dTransceiver.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomEx3dTransceiver_Create(configPtr, work, workSize);
		}

		/// <summary>3Dトランシーバーオブジェクトの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーオブジェクトを破棄します。
		/// 本関数を実行した時点で、3Dトランシーバーオブジェクト作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定した3Dトランシーバーオブジェクトも無効になります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.CriAtomEx3dTransceiver"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomEx3dTransceiver_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomEx3dTransceiver() => Dispose();
#pragma warning restore 1591

		/// <summary>3Dトランシーバー入力の位置の設定</summary>
		/// <param name="position">位置ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー入力の位置を設定します。
		/// 位置は、距離減衰、および定位計算に使用されます。
		/// 位置は、3次元ベクトルで指定します。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public unsafe void SetInputPosition(in CriAtomEx.Vector position)
		{
			fixed (CriAtomEx.Vector* positionPtr = &position)
				NativeMethods.criAtomEx3dTransceiver_SetInputPosition(NativeHandle, positionPtr);
		}

		/// <summary>3Dトランシーバー出力の位置の設定</summary>
		/// <param name="position">位置ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー出力の位置を設定します。
		/// 位置は、距離減衰、および定位計算に使用されます。
		/// 位置は、3次元ベクトルで指定します。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public unsafe void SetOutputPosition(in CriAtomEx.Vector position)
		{
			fixed (CriAtomEx.Vector* positionPtr = &position)
				NativeMethods.criAtomEx3dTransceiver_SetOutputPosition(NativeHandle, positionPtr);
		}

		/// <summary>3Dトランシーバーの更新</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーに設定されているパラメーターを使用して、3Dトランシーバーを更新します。
		/// 本関数では、3Dトランシーバーに設定されている全てのパラメーターを更新します。
		/// パラメーターをひとつ変更する度に本関数にて更新処理を行うよりも、
		/// 複数のパラメーターを変更してから更新処理を行った方が効率的です。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はAtomExプレーヤーのパラメーター更新（<see cref="CriAtomExPlayer.UpdateAll"/>, <see cref="CriAtomExPlayer.Update"/>）
		/// とは独立して動作します。3Dトランシーバーのパラメーターを変更した際は、本関数にて更新処理を行ってください。
		/// </para>
		/// </remarks>
		public void Update()
		{
			NativeMethods.criAtomEx3dTransceiver_Update(NativeHandle);
		}

		/// <summary>3Dトランシーバー入力の向きの設定</summary>
		/// <param name="front">前方ベクトル</param>
		/// <param name="top">上方ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーの向きを前方ベクトルと上方ベクトルで設定します。
		/// 向きは、3次元ベクトルで指定します。設定された向きベクトルは、ライブラリ内部で正規化して使用されます。
		/// デフォルト値は以下のとおりです。
		/// - 前方ベクトル：(0.0f, 0.0f, 1.0f)
		/// - 上方ベクトル：(0.0f, 1.0f, 0.0f)
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public unsafe void SetInputOrientation(in CriAtomEx.Vector front, in CriAtomEx.Vector top)
		{
			fixed (CriAtomEx.Vector* frontPtr = &front)
			fixed (CriAtomEx.Vector* topPtr = &top)
				NativeMethods.criAtomEx3dTransceiver_SetInputOrientation(NativeHandle, frontPtr, topPtr);
		}

		/// <summary>3Dトランシーバー出力の向きの設定</summary>
		/// <param name="front">前方ベクトル</param>
		/// <param name="top">上方ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー出力の向きを設定します。
		/// 本関数で設定した向きは、サウンドコーンの向きとして設定されます。
		/// サウンドコーンは、3Dトランシーバーから音が発生する方向を表し、音の指向性の表現に使用されます。
		/// サウンドコーンの向きは、3次元ベクトルで指定します。設定された向きベクトルは、ライブラリ内部で正規化して使用されます。
		/// デフォルト値は以下のとおりです。
		/// - 前方ベクトル：(0.0f, 0.0f, 1.0f)
		/// - 上方ベクトル：(0.0f, 1.0f, 0.0f)
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.SetOutputConeParameter"/>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public unsafe void SetOutputOrientation(in CriAtomEx.Vector front, in CriAtomEx.Vector top)
		{
			fixed (CriAtomEx.Vector* frontPtr = &front)
			fixed (CriAtomEx.Vector* topPtr = &top)
				NativeMethods.criAtomEx3dTransceiver_SetOutputOrientation(NativeHandle, frontPtr, topPtr);
		}

		/// <summary>3Dトランシーバー出力のサウンドコーンパラメーターの設定</summary>
		/// <param name="insideAngle">サウンドコーンのインサイドアングル</param>
		/// <param name="outsideAngle">サウンドコーンのアウトサイドアングル</param>
		/// <param name="outsideVolume">サウンドコーンのアウトサイドボリューム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー出力のサウンドコーンパラメーターを設定します。
		/// サウンドコーンは、3Dトランシーバーから音が発生する方向を表し、音の指向性の表現に使用されます。
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
		/// デフォルト値は、::criAtomEx3dTransceiver_ChangeDefaultConeParameter 関数にて変更可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetOutputConeParameter(Single insideAngle, Single outsideAngle, Single outsideVolume)
		{
			NativeMethods.criAtomEx3dTransceiver_SetOutputConeParameter(NativeHandle, insideAngle, outsideAngle, outsideVolume);
		}

		/// <summary>3Dトランシーバーの最小距離／最大距離の設定</summary>
		/// <param name="minAttenuationDistance">最小距離</param>
		/// <param name="maxAttenuationDistance">最大距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーの最小距離／最大距離を設定します。
		/// 最小距離は、これ以上音量が大きくならない距離を表します。最大距離は、最小音量になる距離を表します。
		/// ライブラリ初期化時のデフォルト値は以下のとおりです。
		/// - 最小距離：0.0f
		/// - 最大距離：0.0f
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetOutputMinMaxAttenuationDistance(Single minAttenuationDistance, Single maxAttenuationDistance)
		{
			NativeMethods.criAtomEx3dTransceiver_SetOutputMinMaxAttenuationDistance(NativeHandle, minAttenuationDistance, maxAttenuationDistance);
		}

		/// <summary>3Dトランシーバー出力のインテリアパンニング境界距離の設定</summary>
		/// <param name="transceiverRadius">3Dトランシーバーの半径</param>
		/// <param name="interiorDistance">インテリア距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー出力のインテリアパンニング境界距離の設定をします。
		/// 3Dトランシーバー出力の半径は、3Dトランシーバー出力を球としたときの半径です。
		/// インテリア距離は、インテリアパンニング適用される3Dトランシーバーの半径からの距離です。
		/// 3Dトランシーバーの半径内では、インテリアパンニング適用されますが、インテリア距離が0.0と扱われるため、
		/// 全てのスピーカーから同じ音量で音声が再生されます。
		/// インテリア距離内では、インテリアパンニング適用されます。
		/// インテリア距離外では、インテリアパンニング適用されず、3Dトランシーバー位置に最も近い1つ、
		/// または2つのスピーカーから音声が再生されます。
		/// ライブラリ初期化時のデフォルト値は以下のとおりです。
		/// - 3Dトランシーバーの半径：0.0f
		/// - インテリア距離：0.0f（3Dトランシーバーの最小距離に依存）
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetOutputInteriorPanField(Single transceiverRadius, Single interiorDistance)
		{
			NativeMethods.criAtomEx3dTransceiver_SetOutputInteriorPanField(NativeHandle, transceiverRadius, interiorDistance);
		}

		/// <summary>3Dトランシーバー入力のクロスフェード境界距離の設定</summary>
		/// <param name="directAudioRadius">直接音領域の半径</param>
		/// <param name="crossfadeDistance">クロスフェード距離</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー入力のクロスフェード境界距離の設定をします。
		/// 直接音領域の半径内では、3Dトランシーバーの出力からの音声は再生されず、3D音源からの音声のみが再生されます。
		/// クロスフェード距離は、3Dトランシーバー出力と3D音源からの音声のクロスフェードが適用される直接音領域からの距離です。
		/// 直接音領域では、クロスフェードの割合が音源からの音声=1、3Dトランシーバーからの音声=0になるので、
		/// 3Dトランシーバー出力からの音声は聞こえなくなり、音源からの音声のみが再生されます。
		/// クロスフェード距離内では、リスナーの位置に応じてクロスフェードが適用されます。
		/// クロスフェード距離外では、3D音源からの音声は聞こえず、3Dトランシーバー出力からの音声のみが聞こえるようになります。
		/// ライブラリ初期化時のデフォルト値は以下のとおりです。
		/// - 直接音領域の半径：0.0f
		/// - クロスフェード距離：0.0f
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetInputCrossFadeField(Single directAudioRadius, Single crossfadeDistance)
		{
			NativeMethods.criAtomEx3dTransceiver_SetInputCrossFadeField(NativeHandle, directAudioRadius, crossfadeDistance);
		}

		/// <summary>3Dトランシーバー出力のボリュームの設定</summary>
		/// <param name="volume">ボリューム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー出力のボリュームを設定します。
		/// 3Dトランシーバー出力のボリュームは、定位に関わる音量（L,R,SL,SR）にのみ影響し、LFEやセンターへの出力レベルには影響しません。
		/// ボリューム値には、0.0f～1.0fの範囲で実数値を指定します。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのボリュームで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// ライブラリ初期化時のデフォルト値は1.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetOutputVolume(Single volume)
		{
			NativeMethods.criAtomEx3dTransceiver_SetOutputVolume(NativeHandle, volume);
		}

		/// <summary>3DトランシーバーにAISACを取り付ける</summary>
		/// <param name="globalAisacName">取り付けるグローバルAISAC名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3DトランシーバーにAISACをアタッチ（取り付け）します。
		/// AISACをアタッチすることにより、キューやトラックにAISACを設定していなくても、AISACの効果を得ることができます。
		/// AISACのアタッチに失敗した場合、関数内でエラーコールバックが発生します。
		/// AISACのアタッチに失敗した理由については、エラーコールバックのメッセージを確認してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 全体設定（ACFファイル）に含まれるグローバルAISACのみ、アタッチ可能です。
		/// AISACの効果を得るには、キューやトラックに設定されているAISACと同様に、該当するAISACコントロール値を設定する必要があります。
		/// </para>
		/// <para>
		/// 注意:
		/// キューやトラックに「AISACコントロール値を変更するAISAC」が設定されていたとしても、
		/// その適用結果のAISACコントロール値は、3DトランシーバーにアタッチしたAISACには影響しません。
		/// 現在、「オートモジュレーション」や「ランダム」といったコントロールタイプのAISACのアタッチには対応しておりません。
		/// 現在、3DトランシーバーにアタッチできるAISACの最大数は、8個固定です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.DetachAisac"/>
		public void AttachAisac(ArgString globalAisacName)
		{
			NativeMethods.criAtomEx3dTransceiver_AttachAisac(NativeHandle, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>3DトランシーバーからAISACを取り外す</summary>
		/// <param name="globalAisacName">取り外すグローバルAISAC名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3DトランシーバーからAISACをデタッチ（取り外し）します。
		/// AISACのデタッチに失敗した場合、関数内でエラーコールバックが発生します。
		/// AISACのデタッチに失敗した理由については、エラーコールバックのメッセージを確認してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.AttachAisac"/>
		public void DetachAisac(ArgString globalAisacName)
		{
			NativeMethods.criAtomEx3dTransceiver_DetachAisac(NativeHandle, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>角度AISACコントロール値の最大変化量の設定</summary>
		/// <param name="maxDelta">角度AISACコントロール値の最大変化量</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 角度AISACによりAISACコントロール値が変更される際の、最大変化量を設定します。
		/// 最大変化量を低めに変更すると、3Dトランシーバーとリスナー間の相対角度が急激に変わった場合でも、
		/// 角度AISACによるAISACコントロール値の変化をスムーズにすることができます。
		/// 例えば、(0.5f / 30.0f)を設定すると、角度が0度→180度に変化した場合に、30フレームかけて変化するような変化量となります。
		/// デフォルト値は1.0f（制限なし）です。
		/// データ側では本パラメーターは設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// 本関数で設定している最大変化量は、定位角度を元に計算されている、角度AISACコントロール値の変化にのみ適用されます。
		/// 定位角度自体には影響はありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetMaxAngleAisacDelta(Single maxDelta)
		{
			NativeMethods.criAtomEx3dTransceiver_SetMaxAngleAisacDelta(NativeHandle, maxDelta);
		}

		/// <summary>距離AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">距離AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 最小距離、最大距離間の距離減衰に連動するAISACコントロールIDを指定します。
		/// 本関数でAISACコントロールIDを設定した場合、デフォルトの距離減衰は無効になります。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetDistanceAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dTransceiver_SetDistanceAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>リスナー基準方位角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">リスナー基準方位角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// リスナーから見た3Dトランシーバーの方位角に連動するAISACコントロールIDを指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetListenerBasedAzimuthAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dTransceiver_SetListenerBasedAzimuthAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>リスナー基準仰俯角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">リスナー基準仰俯角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// リスナーから見た3Dトランシーバーの仰俯角に連動するAISACコントロールIDを指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetListenerBasedElevationAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dTransceiver_SetListenerBasedElevationAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>3Dトランシーバー出力基準方位角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">3Dトランシーバー基準方位角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー出力の位置から見たリスナーの方位角に連動するAISACコントロールIDを指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetTransceiverOutputBasedAzimuthAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dTransceiver_SetTransceiverOutputBasedAzimuthAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>3Dトランシーバー出力基準仰俯角AISACコントロールIDの設定</summary>
		/// <param name="aisacControlId">3Dトランシーバー基準仰俯角AISACコントロールID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバー出力の位置から見たリスナーの仰俯角に連動するAISACコントロールIDを指定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void SetTransceiverOutputBasedElevationAngleAisacControlId(UInt32 aisacControlId)
		{
			NativeMethods.criAtomEx3dTransceiver_SetTransceiverOutputBasedElevationAngleAisacControlId(NativeHandle, aisacControlId);
		}

		/// <summary>3Dトランシーバーオブジェクトに対する3Dリージョンオブジェクトの設定</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dトランシーバーオブジェクトに対して3Dリージョンオブジェクトを設定します。
		/// *
		/// </para>
		/// <para>
		/// 注意:
		/// 同一のExPlayerに設定されている3D音源と3Dリスナーに設定されているリージョンが異なり、
		/// かつ3D音源と同じリージョンが設定されている3Dトランシーバーがない場合、音声はミュートされます。
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dTransceiver.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/>
		/// <seealso cref="CriAtomEx3dTransceiver.Update"/>
		public void Set3dRegionHn(CriAtomEx3dRegion ex3dRegion)
		{
			NativeMethods.criAtomEx3dTransceiver_Set3dRegionHn(NativeHandle, ex3dRegion?.NativeHandle ?? default);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomEx3dTransceiver(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomEx3dTransceiver other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomEx3dTransceiver a, CriAtomEx3dTransceiver b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomEx3dTransceiver a, CriAtomEx3dTransceiver b) =>
			!(a == b);

	}
}