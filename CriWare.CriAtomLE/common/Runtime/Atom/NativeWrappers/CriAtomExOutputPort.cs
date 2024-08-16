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
	/// <summary>出力ポートオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomExOutputPort"/> は、音声の出力先を指定するためのオブジェクトです。
	/// 出力ポートオブジェクトは、以下の２つの関数で取得することができます。
	/// - <see cref="CriAtomExOutputPort.CriAtomExOutputPort"/> 関数で新しいオブジェクトを作成
	/// - <see cref="CriAtomExAcf.GetOutputPortHnByName"/> 関数でACF内の出力ポートオブジェクトを取得
	/// ACF内の出力ポートオブジェクトは、ACFファイルを登録したとき自動的に作成されます。
	/// プレーヤーに対して <see cref="CriAtomExPlayer.AddOutputPort"/> 関数または <see cref="CriAtomExPlayer.AddPreferredOutputPort"/> 関数にて
	/// 別の出力ポートオブジェクトが指定されていない場合、出力ポートが指定された音声の再生にはACF内の出力ポートが使用されます。
	/// </para>
	/// <para>
	/// 備考:
	/// 出力ポートは、音声の出力先を抽象化した概念です。
	/// 音声トラックに予め出力先の名前を指定すると、再生するとき自動的に同じ名前の出力ポートオブジェクトが割り当てられます。
	/// 出力ポートはASRラックと紐付けられており、出力ポートオブジェクトが割り当てられた音声は、そのASRラックを介して再生されます。
	/// そのため、ACF内の出力ポートオブジェクトを含め、全ての出力ポートオブジェクトは使用する前に必ず
	/// <see cref="CriAtomExOutputPort.SetAsrRackId"/> 関数で適切なASRラックと紐付ける必要があります。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
	/// <seealso cref="CriAtomExAcf.GetOutputPortHnByName"/>
	/// <seealso cref="CriAtomExOutputPort.SetAsrRackId"/>
	public partial class CriAtomExOutputPort : IDisposable
	{
		/// <summary>出力ポートオブジェクトの破棄可能の判定</summary>
		/// <returns>破棄可能か？（true = 可能, false = 不可能）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートオブジェクトの破棄が可能か判定します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExPlayer.AddOutputPort"/> 関数または <see cref="CriAtomExPlayer.AddPreferredOutputPort"/> 関数を使用して
		/// プレーヤーに追加中の出力ポートオブジェクトは破棄することができません。
		/// <see cref="CriAtomExPlayer.RemoveOutputPort"/> 関数または <see cref="CriAtomExPlayer.RemovePreferredOutputPort"/> 関数を使用して
		/// プレーヤーから取り外してから破棄してください。
		/// また、ACFファイルの情報から作成されたACF内の出力ポートオブジェクトは破棄することができません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.Dispose"/>
		public bool IsDestroyable()
		{
			return NativeMethods.criAtomExOutputPort_IsDestroyable(NativeHandle);
		}

		/// <summary>出力ポート作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="outputportName">出力ポート名</param>
		/// <param name="pConfig">出力ポート作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExOutputPort.CriAtomExOutputPort"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExOutputPort.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.Config"/>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		public static unsafe void SetDefaultConfig(out CriAtomExOutputPort.Config pConfig, ArgString outputportName)
		{
			fixed (CriAtomExOutputPort.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExOutputPort_SetDefaultConfig_(pConfigPtr, outputportName.GetPointer(stackalloc byte[outputportName.BufferSize]));
		}

		/// <summary>出力ポートオブジェクト作成用ワーク領域サイズの計算</summary>
		/// <param name="config">出力ポートオブジェクト作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートオブジェクトの作成に必要なワーク領域のサイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExOutputPort.CriAtomExOutputPort"/> 関数で出力ポートオブジェクトを作成する際には、
		/// <see cref="CriAtomExOutputPort.CriAtomExOutputPort"/> 関数に本関数が返すサイズ分のメモリをワーク
		/// 領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExOutputPort.Config"/>::name に指定する出力ポート名の長さは、
		/// <see cref="CriAtomEx.OutputPortMaxNameLength"/> 以下である必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExOutputPort.Config config)
		{
			fixed (CriAtomExOutputPort.Config* configPtr = &config)
				return NativeMethods.criAtomExOutputPort_CalculateWorkSize(configPtr);
		}

		/// <summary>出力ポート作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートを作成するための構造体です。
		/// <see cref="CriAtomExOutputPort.CriAtomExOutputPort"/> 関数の引数に指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		public unsafe partial struct Config
		{
			/// <summary>出力ポート名</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 出力ポートの名前を指定します。
			/// </para>
			/// <para>
			/// 備考:
			/// 文字列の長さは <see cref="CriAtomEx.OutputPortMaxNameLength"/> 以下である必要があります。
			/// 一度指定したポート名をあとから変更することはできません。
			/// </para>
			/// </remarks>
			public NativeString name;

			/// <summary>出力ポートタイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 出力ポートのタイプを指定します。
			/// </para>
			/// <para>
			/// 備考:
			/// 一度指定したポートタイプをあとから変更することはできません。
			/// </para>
			/// </remarks>
			public CriAtomExOutputPort.Type type;

			/// <summary>出力ポートのカテゴリの最大無視設定数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 出力ポートに指定できるカテゴリ無視設定が行える最大数。
			/// </para>
			/// <para>
			/// 備考:
			/// 一度指定した値をあとから変更することはできません。
			/// </para>
			/// </remarks>
			public UInt32 maxIgnoredCategories;

		}
		/// <summary>出力ポートタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートの種別を示す値です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.Config"/>
		public enum Type
		{
			/// <summary>サウンドタイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 通常の音声を再生する出力ポートタイプです。
			/// </para>
			/// </remarks>
			Audio = 0,
			/// <summary>振動タイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// オーディオベースの振動を再生する出力ポートタイプです。
			/// </para>
			/// </remarks>
			Vibration = 1,
			/// <summary>振動タイプ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 不正なタイプです。関数の返り値がエラーのとき使用されます。
			/// </para>
			/// </remarks>
			Invalid = -1,
		}
		/// <summary>出力ポートオブジェクトの作成</summary>
		/// <param name="config">ASR初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>出力ポートオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートオブジェクトの作成を行います。
		/// 出力ポートはASRラックと紐付けられ、出力ポートが指定されたボイスはその出力ポートに紐付けられた
		/// ASRラックにて再生されるようになります。
		/// </para>
		/// <para>
		/// 備考:
		/// ACFファイルに設定された出力ポートオブジェクトは <see cref="CriAtomEx.RegisterAcfFile"/> 関数などを用いて
		/// ACFファイルを登録したとき、ACF内に自動的に作成されるため、
		/// 本関数で新たに作成する必要はありません。
		/// ACF内の出力ポートオブジェクトは <see cref="CriAtomExAcf.GetOutputPortHnByName"/> 関数で取得できます。
		/// そのため、本関数はアプリケーション上で新たに出力ポートオブジェクトが必要になった場合に使用してください。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// 出力ポートオブジェクトの生成に成功した場合は、本関数は生成した出力ポートオブジェクトを返します。
		/// 生成に失敗した場合は null を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で作成された出力ポートオブジェクトには、<see cref="CriAtomExOutputPort.Config"/>::type で指定したタイプによって
		/// 以下のASRラックIDが初期値としてセットされています。
		/// - <see cref="CriAtomExOutputPort.Type.Audio"/> を指定：<see cref="CriAtomExAsr.RackDefaultId"/>
		/// - <see cref="CriAtomExOutputPort.Type.Vibration"/> を指定：<see cref="CriAtomExAsr.RackIllegalId"/>
		/// </para>
		/// <para>
		/// 出力ポートオブジェクトを使用する前に、必ず <see cref="CriAtomExOutputPort.SetAsrRackId"/>
		/// 関数で適切なASRラックを設定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.Dispose"/>
		/// <seealso cref="CriAtomExAcf.GetOutputPortHnByName"/>
		/// <seealso cref="CriAtomExPlayer.AddOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.AddPreferredOutputPort"/>
		public unsafe CriAtomExOutputPort(in CriAtomExOutputPort.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExOutputPort.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomExOutputPort_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomExOutputPort(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomExOutputPort.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomExOutputPort_Create(configPtr, work, workSize);
		}

		/// <summary>出力ポートオブジェクトの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートオブジェクトの破棄を行います。
		/// </para>
		/// <para>
		/// 備考:
		/// 但し、 <see cref="CriAtomExPlayer.AddOutputPort"/> 関数または <see cref="CriAtomExPlayer.AddPreferredOutputPort"/> 関数を使用して
		/// プレーヤーに追加中の出力ポートオブジェクトは破棄することができません。
		/// <see cref="CriAtomExPlayer.RemoveOutputPort"/> 関数または <see cref="CriAtomExPlayer.RemovePreferredOutputPort"/> 関数を使用して
		/// プレーヤーから取り外してから破棄してください。
		/// また、ACFファイルの情報から作成されたACF内の出力ポートオブジェクトは破棄することができません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomExOutputPort_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomExOutputPort() => Dispose();
#pragma warning restore 1591

		/// <summary>ASRラックIDの指定</summary>
		/// <param name="rackId">ASRラックID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートにASRラック指定します。
		/// 出力ポートが指定されたボイスは、その出力ポートに指定されているASRラックで再生されます。
		/// </para>
		/// <para>
		/// 備考:
		/// ACFファイル登録時に作成された出力ポートオブジェクトや <see cref="CriAtomExOutputPort.CriAtomExOutputPort"/> 関数で作成された
		/// 出力ポートオブジェクトには、必ず本関数で適切なASRラックを指定する必要があります。
		/// 出力ポートのタイプなどによって、指定できるASRラックに制限がある場合があります。
		/// 詳細に関しましてはマニュアルを参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で出力ポートのASRラックIDを変更しても、既に再生されている音声には影響しません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		/// <seealso cref="CriAtomExAcf.GetOutputPortHnByName"/>
		public void SetAsrRackId(CriAtomExAsrRack rackId)
		{
			NativeMethods.criAtomExOutputPort_SetAsrRackId(NativeHandle, rackId?.NativeHandle ?? default);
		}

		/// <summary>振動タイプの出力ポートのチャンネルレベルの設定</summary>
		/// <param name="channel">チャンネルインデックス（0 = L, 1 = R）</param>
		/// <param name="level">レベル(0 ~ 2.0)</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 振動タイプの出力ポートに対し、振動デバイスの各チャンネルへの出力レベルを設定します。
		/// </para>
		/// <para>
		/// 備考:
		/// 振動タイプの出力ポートは２チャンネルで動作しており、最終出力デバイスがモノラルの場合-3dBのダウンミックスが適用されます。
		/// この関数で設定した値は、音が再生中でも即時反映されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.SetMonauralMix"/>
		public void SetVibrationChannelLevel(Int32 channel, Single level)
		{
			NativeMethods.criAtomExOutputPort_SetVibrationChannelLevel(NativeHandle, channel, level);
		}

		/// <summary>振動タイプの出力ポートのモノラルミックス有無設定</summary>
		/// <param name="monauralMix">モノラルミックス有無（true = 有効, false = 無効）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 振動タイプの出力ポートは２チャンネルで動作するため、入力されるボイスがステレオ以上の音声データか、
		/// 3Dパンが設定されている場合、その結果が振動デバイスの左右に伝わります。
		/// モノラルミックスを有効にすると、振動デバイスへ出力する前に一度モノラルにダウンミックスを行うことでそれらの影響をなくすことができます。
		/// <see cref="CriAtomExOutputPort.SetVibrationChannelLevel"/> 関数を使用して、モノラルミックス後振動デバイスへ送られるレベルを設定することも可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// この関数で設定した値は、音が再生中でも即時反映されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.SetVibrationChannelLevel"/>
		public void SetMonauralMix(NativeBool monauralMix)
		{
			NativeMethods.criAtomExOutputPort_SetMonauralMix(NativeHandle, monauralMix);
		}

		/// <summary>出力ポートが指定カテゴリのパラメータを無視するかの設定</summary>
		/// <param name="categoryId">カテゴリID</param>
		/// <param name="ignoreParameters">無視設定パラメータ （true:無視する, false:無視しない）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートに対して指定したカテゴリIDのパラメータを無視するかを設定します。
		/// </para>
		/// <para>
		/// 備考:
		/// 出力ポートに対して指定したカテゴリIDのパラメータを無視させることでカテゴリの影響をなくすことができます。
		/// 同じカテゴリIDを指定して、カテゴリを無視する・無視しないを変更することも可能です。
		/// 設定できるカテゴリは最大4つまでです。4つ設定している状態でこの関数を実行すると警告が発生し、処理は行われません。
		/// </para>
		/// <para>
		/// 備考:
		/// この関数で設定は再生時に反映されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		/// <seealso cref="CriAtomExAcf.GetOutputPortHnByName"/>
		public void IgnoreCategoryParametersById(CriAtomExCategory categoryId, NativeBool ignoreParameters)
		{
			NativeMethods.criAtomExOutputPort_IgnoreCategoryParametersById(NativeHandle, categoryId.NativeHandle, ignoreParameters);
		}

		/// <summary>出力ポートに設定した指定カテゴリのパラメータを無視する設定をリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力ポートに設定した指定カテゴリのパラメータを無視する設定をリセットします。
		/// </para>
		/// <para>
		/// 備考:
		/// 出力ポートに対して指定したカテゴリIDのパラメータを無視する設定をすべてリセットします。
		/// 設定されているカテゴリの数もリセットされるため新たにカテゴリを指定してカテゴリのパラメータを無視する設定を行えます。
		/// </para>
		/// <para>
		/// 備考:
		/// この関数で設定は再生時に反映されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		/// <seealso cref="CriAtomExOutputPort.IgnoreCategoryParametersById"/>
		public void ResetIgnoreCategory()
		{
			NativeMethods.criAtomExOutputPort_ResetIgnoreCategory(NativeHandle);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExOutputPort(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExOutputPort other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExOutputPort a, CriAtomExOutputPort b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExOutputPort a, CriAtomExOutputPort b) =>
			!(a == b);

	}
}