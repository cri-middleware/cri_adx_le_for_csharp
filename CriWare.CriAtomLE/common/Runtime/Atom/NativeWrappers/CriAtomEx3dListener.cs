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
	/// <summary>3Dリスナーオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// 3Dリスナーを扱うためのオブジェクトです。
	/// 3Dポジショニング機能に使用します。
	/// 3Dリスナーのパラメーター、位置情報の設定等は、3Dリスナーオブジェクトを介して実行されます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomEx3dListener.CriAtomEx3dListener"/>
	public partial class CriAtomEx3dListener : IDisposable, Interfaces.IPositionable
	{
		/// <summary>3Dリスナーオブジェクト作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">3Dリスナーオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーオブジェクト作成用コンフィグ構造体（ <see cref="CriAtomEx3dListener.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtomEx3dListener.Config pConfig)
		{
			fixed (CriAtomEx3dListener.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomEx3dListener_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>3Dリスナーオブジェクトの作成に必要なワーク領域サイズの計算</summary>
		/// <param name="config">3Dリスナーオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <returns>3Dリスナーオブジェクト作成用ワークサイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーオブジェクトを作成するために必要な、ワーク領域のサイズを取得します。
		/// アロケーターを登録せずに3Dリスナーオブジェクトを作成する場合、
		/// あらかじめ本関数で計算したワーク領域サイズ分のメモリを
		/// ワーク領域として <see cref="CriAtomEx3dListener.CriAtomEx3dListener"/> 関数にセットする必要があります。
		/// 3Dリスナーオブジェクトの作成に必要なワークメモリのサイズは、3Dリスナーオブジェクト作成用コンフィグ
		/// 構造体（ <see cref="CriAtomEx3dListener.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomEx3dListener.SetDefaultConfig"/> 適用時と同じパラメーター）で
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
		/// <seealso cref="CriAtomEx3dListener.CriAtomEx3dListener"/>
		/// <seealso cref="CriAtomEx3dListener.Config"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomEx3dListener.Config config)
		{
			fixed (CriAtomEx3dListener.Config* configPtr = &config)
				return NativeMethods.criAtomEx3dListener_CalculateWorkSize(configPtr);
		}

		/// <summary>3Dリスナーオブジェクト作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーオブジェクトを作成する場合に使用する構造体です。
		/// 現状指定可能なパラメーターはありませんが、将来パラメーターが追加される可能性があるため、
		/// 本構造体を使用する際には <see cref="CriAtomEx3dListener.SetDefaultConfig"/> メソッドを使用し、
		/// 構造体の初期化を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.SetDefaultConfig"/>
		/// <seealso cref="CriAtomEx3dListener.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dListener.CriAtomEx3dListener"/>
		public unsafe partial struct Config
		{
			/// <exclude/>
			public Int32 reserved;

		}
		/// <summary>3Dリスナーオブジェクトの作成</summary>
		/// <param name="config">3Dリスナーオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <param name="work">3Dリスナーオブジェクト作成用ワーク領域へのポインタ</param>
		/// <param name="workSize">3Dリスナーオブジェクト作成用ワークサイズ</param>
		/// <returns>3Dリスナーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーオブジェクト作成用コンフィグに基づいて、3Dリスナーオブジェクトを作成します。
		/// 作成に成功すると、3Dリスナーオブジェクトを返します。
		/// 3Dリスナーオブジェクトを作成する際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomEx3dListener.CalculateWorkSize"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.CalculateWorkSize"/>
		/// <seealso cref="CriAtomEx3dListener.Dispose"/>
		public unsafe CriAtomEx3dListener(in CriAtomEx3dListener.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx3dListener.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomEx3dListener_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomEx3dListener(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomEx3dListener.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomEx3dListener_Create(configPtr, work, workSize);
		}

		/// <summary>3Dリスナーオブジェクトの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーオブジェクトを破棄します。
		/// 本関数を実行した時点で、3Dリスナーオブジェクト作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定した3Dリスナーオブジェクトも無効になります。
		/// 3DリスナーオブジェクトをセットしたAtomExプレーヤーで再生している音声がある場合、
		/// 本関数を実行する前に、それらの音声を停止するか、そのAtomExプレーヤーを破棄してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.CriAtomEx3dListener"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomEx3dListener_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomEx3dListener() => Dispose();
#pragma warning restore 1591

		/// <summary>3Dリスナーの位置の設定</summary>
		/// <param name="position">位置ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーの位置を設定します。
		/// 位置は、距離減衰、および定位計算に使用されます。
		/// 位置は、3次元ベクトルで指定します。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。
		/// データ側には位置は設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		public unsafe void SetPosition(in CriAtomEx.Vector position)
		{
			fixed (CriAtomEx.Vector* positionPtr = &position)
				NativeMethods.criAtomEx3dListener_SetPosition(NativeHandle, positionPtr);
		}

		/// <summary>3Dリスナーの速度の設定</summary>
		/// <param name="velocity">速度ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーの速度を設定します。
		/// 速度は、ドップラー効果の計算に使用されます。
		/// 速度は、3次元ベクトルで指定します。速度の単位は、1秒あたりの移動距離です。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。
		/// データ側には速度は設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		public unsafe void SetVelocity(in CriAtomEx.Vector velocity)
		{
			fixed (CriAtomEx.Vector* velocityPtr = &velocity)
				NativeMethods.criAtomEx3dListener_SetVelocity(NativeHandle, velocityPtr);
		}

		/// <summary>3Dリスナーの更新</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーに設定されているパラメーターを使用して、3Dリスナーを更新します。
		/// 本関数では、3Dリスナーに設定されている全てのパラメーターを更新します。
		/// パラメーターをひとつ変更する度に本関数にて更新処理を行うよりも、
		/// 複数のパラメーターを変更してから更新処理を行った方が効率的です。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はAtomExプレーヤーのパラメーター更新（<see cref="CriAtomExPlayer.UpdateAll"/>, <see cref="CriAtomExPlayer.Update"/>）
		/// とは独立して動作します。3Dリスナーのパラメーターを変更した際は、本関数にて更新処理を行ってください。
		/// </para>
		/// </remarks>
		public void Update()
		{
			NativeMethods.criAtomEx3dListener_Update(NativeHandle);
		}

		/// <summary>3D音源パラメーターの初期化</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーに設定されているパラメーターをクリアし、初期値に戻します。
		/// </para>
		/// <para>
		/// 注意:
		/// クリアしたパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		public void ResetParameters()
		{
			NativeMethods.criAtomEx3dListener_ResetParameters(NativeHandle);
		}

		/// <summary>3Dリスナーの位置の取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーの位置を取得します。
		/// 位置は、3次元ベクトルで取得します。
		/// </para>
		/// </remarks>
		public CriAtomEx.Vector GetPosition()
		{
			return NativeMethods.criAtomEx3dListener_GetPosition(NativeHandle);
		}

		/// <summary>3Dリスナーの向きの設定</summary>
		/// <param name="front">前方ベクトル</param>
		/// <param name="top">上方ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーの向きを前方ベクトルと上方ベクトルで設定します。
		/// 向きは、3次元ベクトルで指定します。設定された向きベクトルは、ライブラリ内部で正規化して使用されます。
		/// デフォルト値は以下のとおりです。
		/// - 前方ベクトル：(0.0f, 0.0f, 1.0f)
		/// - 上方ベクトル：(0.0f, 1.0f, 0.0f)
		/// データ側にはリスナーの向きは設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		public unsafe void SetOrientation(in CriAtomEx.Vector front, in CriAtomEx.Vector top)
		{
			fixed (CriAtomEx.Vector* frontPtr = &front)
			fixed (CriAtomEx.Vector* topPtr = &top)
				NativeMethods.criAtomEx3dListener_SetOrientation(NativeHandle, frontPtr, topPtr);
		}

		/// <summary>3Dリスナーのドップラー倍率の設定</summary>
		/// <param name="dopplerMultiplier">ドップラー倍率</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーのドップラー倍率を設定します。この倍率はドップラー効果の計算に使用されます。
		/// 例えば、doppler_multiplierに10.0fを指定すると、ドップラー効果が通常の10倍になります。
		/// doppler_multiplierには0.0f以上の値を指定します。
		/// デフォルト値は1.0fです。
		/// データ側にはリスナーのドップラー倍率は設定できないため、常に本関数での設定値が使用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		public void SetDopplerMultiplier(Single dopplerMultiplier)
		{
			NativeMethods.criAtomEx3dListener_SetDopplerMultiplier(NativeHandle, dopplerMultiplier);
		}

		/// <summary>3Dリスナーの注目点の設定</summary>
		/// <param name="focusPoint">注目点ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーの注目点を設定します。
		/// 注目点は、3Dポジショニングを行うにあたって、
		/// 注目点を設定すると、リスナーの位置と注目点の間が直線で結ばれ、その直線上でマイクを移動させることができるようになります。
		/// 例えば、リスナーはカメラと常に同期させておき、主要キャラクタの位置に注目点を設定することで、状況に応じて、客観的か主観的かを柔軟に表現／調整するような使い方ができます。
		/// なお、リスナーの位置と注目点の間で移動できるマイクは、現実世界のマイクと異なり、距離センサ（距離減衰計算用）と方向センサ（定位計算用）を分離しています。
		/// これらを独立して操作することで、例えば「主役キャラに注目するので、距離減衰はキャラ位置基準で行いたい」「定位は画面の見た目に合わせたいため、定位計算はカメラ位置基準で行いたい」という表現を行うことができます。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。距離センサや方向センサのフォーカスレベルを設定しない状況では、注目点を設定する必要はありません。その場合、従来どおり、全ての3Dポジショニング計算をリスナー位置基準で行います。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		/// <seealso cref="CriAtomEx3dListener.SetDistanceFocusLevel"/>
		/// <seealso cref="CriAtomEx3dListener.SetDirectionFocusLevel"/>
		public unsafe void SetFocusPoint(in CriAtomEx.Vector focusPoint)
		{
			fixed (CriAtomEx.Vector* focusPointPtr = &focusPoint)
				NativeMethods.criAtomEx3dListener_SetFocusPoint(NativeHandle, focusPointPtr);
		}

		/// <summary>距離センサのフォーカスレベルの設定</summary>
		/// <param name="distanceFocusLevel">距離センサのフォーカスレベル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 距離センサのフォーカスレベルを設定します。
		/// 距離センサは、3Dポジショニング計算のうち、距離減衰計算の基準となる位置を表します。定位を無視して距離減衰のかかり具合のみを感知するマイク、といった扱いです。
		/// フォーカスレベルは、注目点に対してどれだけセンサ（マイク）を近づけるかを表します。センサ（マイク）は、リスナー位置と注目点の間を結んだ直線上で動かすことができ、0.0fがリスナー位置、1.0fが注目点と同じ位置になります。
		/// 例えば、距離センサのフォーカスレベルを1.0f、方向センサのフォーカスレベルを0.0fとすることで、注目点を基準に距離減衰を適用し、リスナー位置を基準に定位を決定します。
		/// デフォルト値は0.0fです。距離センサや方向センサのフォーカスレベルを設定しない状況では、従来どおり、全ての3Dポジショニング計算をリスナー位置基準で行います。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		/// <seealso cref="CriAtomEx3dListener.SetFocusPoint"/>
		/// <seealso cref="CriAtomEx3dListener.SetDirectionFocusLevel"/>
		public void SetDistanceFocusLevel(Single distanceFocusLevel)
		{
			NativeMethods.criAtomEx3dListener_SetDistanceFocusLevel(NativeHandle, distanceFocusLevel);
		}

		/// <summary>方向センサのフォーカスレベルの設定</summary>
		/// <param name="directionFocusLevel">方向センサのフォーカスレベル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 方向センサのフォーカスレベルを設定します。
		/// 方向センサは、3Dポジショニング計算のうち、定位計算の基準となる位置を表します。距離減衰を無視して定位のみを感知するマイク、といった扱いです。
		/// 方向センサの向きについては、リスナーの向き（<see cref="CriAtomEx3dListener.SetOrientation"/> 関数で設定）をそのまま使用します。
		/// フォーカスレベルは、注目点に対してどれだけセンサ（マイク）を近づけるかを表します。センサ（マイク）は、リスナー位置と注目点の間を結んだ直線上で動かすことができ、0.0fがリスナー位置、1.0fが注目点と同じ位置になります。
		/// 例えば、距離センサのフォーカスレベルを1.0f、方向センサのフォーカスレベルを0.0fとすることで、注目点を基準に距離減衰を適用し、リスナー位置を基準に定位を決定します。
		/// デフォルト値は0.0fです。距離センサや方向センサのフォーカスレベルを設定しない状況では、従来どおり、全ての3Dポジショニング計算をリスナー位置基準で行います。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		/// <seealso cref="CriAtomEx3dListener.SetFocusPoint"/>
		/// <seealso cref="CriAtomEx3dListener.SetDistanceFocusLevel"/>
		public void SetDirectionFocusLevel(Single directionFocusLevel)
		{
			NativeMethods.criAtomEx3dListener_SetDirectionFocusLevel(NativeHandle, directionFocusLevel);
		}

		/// <summary>3Dリスナーの注目点の取得</summary>
		/// <param name="focusPoint">注目点ベクトル</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーの注目点を取得します。
		/// デフォルト値は(0.0f, 0.0f, 0.0f)です。3Dリスナーの注目点を設定していない場合、デフォルト値が返却されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.GetDistanceFocusLevel"/>
		/// <seealso cref="CriAtomEx3dListener.GetDirectionFocusLevel"/>
		public unsafe void GetFocusPoint(out CriAtomEx.Vector focusPoint)
		{
			fixed (CriAtomEx.Vector* focusPointPtr = &focusPoint)
				NativeMethods.criAtomEx3dListener_GetFocusPoint(NativeHandle, focusPointPtr);
		}

		/// <summary>距離センサのフォーカスレベルの取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 距離センサのフォーカスレベルを設定します。
		/// デフォルト値は0.0fです。距離センサのフォーカスレベルを設定していない場合、デフォルト値が返却されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.GetDistanceFocusLevel"/>
		/// <seealso cref="CriAtomEx3dListener.GetDirectionFocusLevel"/>
		public Single GetDistanceFocusLevel()
		{
			return NativeMethods.criAtomEx3dListener_GetDistanceFocusLevel(NativeHandle);
		}

		/// <summary>方向センサのフォーカスレベルの取得</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 方向センサのフォーカスレベルを取得します。
		/// デフォルト値は0.0fです。方向センサのフォーカスレベルを設定していない場合、デフォルト値が返却されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener.GetFocusPoint"/>
		/// <seealso cref="CriAtomEx3dListener.GetDistanceFocusLevel"/>
		public Single GetDirectionFocusLevel()
		{
			return NativeMethods.criAtomEx3dListener_GetDirectionFocusLevel(NativeHandle);
		}

		/// <summary>3Dリスナーオブジェクトに対する3Dリージョンオブジェクトの設定</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dリスナーオブジェクトに対して3Dリージョンオブジェクトを設定します。
		/// *
		/// </para>
		/// <para>
		/// 注意:
		/// 同一のExPlayerに設定されている3D音源と3Dリスナーに設定されているリージョンが異なり、
		/// かつ3D音源と同じリージョンが設定されている3Dトランシーバーがない場合、音声はミュートされます。
		/// 設定したパラメーターを実際に適用するには、<see cref="CriAtomEx3dListener.Update"/> 関数を呼び出す必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dRegion.CriAtomEx3dRegion"/>
		/// <seealso cref="CriAtomEx3dListener.Update"/>
		public void Set3dRegionHn(CriAtomEx3dRegion ex3dRegion)
		{
			NativeMethods.criAtomEx3dListener_Set3dRegionHn(NativeHandle, ex3dRegion?.NativeHandle ?? default);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomEx3dListener(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomEx3dListener other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomEx3dListener a, CriAtomEx3dListener b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomEx3dListener a, CriAtomEx3dListener b) =>
			!(a == b);

	}
}