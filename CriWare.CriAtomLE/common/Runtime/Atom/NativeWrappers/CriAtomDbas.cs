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
	/// <summary>Atom D-BAS ID</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomDbas"/> は、D-BAS管理用のIDです。
	/// <see cref="CriAtomDbas.CriAtomDbas"/> 関数でD-BASを作成すると取得できます。
	/// アプリケーションがこのD-BAS IDを利用するのは、D-BASの破棄時のみです。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomDbas.CriAtomDbas"/>
	/// <seealso cref="CriAtomDbas.Dispose"/>
	public partial class CriAtomDbas : IDisposable
	{
		/// <summary>ストリーム再生中のAtomプレーヤーオブジェクトを取得</summary>
		/// <param name="players">プレーヤーオブジェクト受け取り用配列</param>
		/// <param name="length">プレーヤーオブジェクト受け取り用配列要素数</param>
		/// <returns>プレーヤー数</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ストリーム再生中のAtomプレーヤーオブジェクトを取得します。
		/// プレーヤーオブジェクトの取得に成功すると、
		/// 第3引数（players配列）にプレーヤーオブジェクトのアドレスが保存され、
		/// プレーヤーオブジェクト数が戻り値として返されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第3引数（players配列）にnull、第4引数（length）に0を指定することで、
		/// ストリーム再生中のプレーヤーの数だけを戻り値として取得可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// プレーヤー数を取得してからオブジェクトを取得する場合、
		/// プレーヤー数取得とオブジェクト取得の間にサーバー処理が割り込まないよう、
		/// <see cref="CriAtom.Lock"/> 関数で排他制御を行う必要があります。
		/// （サーバー処理のタイミングで、プレーヤー数が変わる可能性があります。）
		/// 配列要素数がストリーム再生中のプレーヤー数に満たない場合、
		/// 本関数はエラー値（-1）を返します。
		/// </para>
		/// </remarks>
		public unsafe Int32 GetStreamingPlayerHandles(IntPtr[] players, Int32 length)
		{
			fixed (IntPtr* ptrs = players){
				var result = NativeMethods.criAtomDbas_GetStreamingPlayerHandles(NativeHandle, ptrs, Math.Min(length, players.Length));
				return result;
			}
		}

		/// <summary><see cref="CriAtomDbas.Config"/> へのデフォルトパラメーターのセット</summary>
		/// <param name="pConfig">D-BAS作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomDbas.CriAtomDbas"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomDbas.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDbas.Config"/>
		/// <seealso cref="CriAtomDbas.CriAtomDbas"/>
		/// <seealso cref="CriAtomDbas.CalculateWorkSize"/>
		public static unsafe void SetDefaultConfig(out CriAtomDbas.Config pConfig)
		{
			fixed (CriAtomDbas.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomDbas_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>D-BAS作成用ワークサイズの計算</summary>
		/// <param name="config">D-BAS作成用コンフィグ構造体へのポインタ</param>
		/// <returns>D-BAS作成用ワークサイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// D-BAS作成用パラメーターに基づいて、D-BASの作成に必要ワークサイズを計算します。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 設定によっては、D-BASが2GB以上のワークサイズを必要とする場合があり、
		/// その際はエラーになり、 -1 を返します。
		/// エラーが発生した場合は、max_streamsかmax_bpsの値を低く設定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDbas.CriAtomDbas"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomDbas.Config config)
		{
			fixed (CriAtomDbas.Config* configPtr = &config)
				return NativeMethods.criAtomDbas_CalculateWorkSize(configPtr);
		}

		/// <summary>D-BAS作成パラメーター構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomDbas.CriAtomDbas"/> 関数の引数に指定する、D-BASの作成パラメーター構造体です。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomDbas.SetDefaultConfig"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDbas.CriAtomDbas"/>
		/// <seealso cref="CriAtomDbas.CalculateWorkSize"/>
		/// <seealso cref="CriAtomDbas.SetDefaultConfig"/>
		public unsafe partial struct Config
		{
			/// <summary>D-BAS 識別子</summary>
			public UInt32 identifier;

			/// <summary>最大ストリーミング数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// D-BASで管理する瞬間最大ストリーミング本数を指定します。
			/// オーディオだけでなく、Sofdec2で再生するムービーデータのストリーミング本数も加味する必要があります。
			/// 例えば、シーンAではオーディオデータを２本、
			/// シーンBではオーディオデータを１本とムービーデータを２本、ストリーミング再生するとします。
			/// この場合、瞬間最大ストリーミング本数はシーンBの３本を設定してください。
			/// つまり、アプリケーション全体を通して、最悪状態のストリーミング本数を想定した値を設定してください。
			/// </para>
			/// </remarks>
			public Int32 maxStreams;

			/// <summary>最大ビットレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ストリーミング全体における瞬間最大ビットレートを指定します。
			/// この値には、アプリケーション全体を通して、
			/// ストリーミング再生するデータの消費ビットレートのピーク値を設定してください。
			/// オーディオだけでなく、Sofdec2で再生するムービーデータの消費ビットレートも加味する必要があります。
			/// 例えば、シーンAではオーディオデータを４本、シーンBではムービーデータを１本、ストリーミング再生するとします。
			/// この時、オーディオデータ４本分の消費ビットレートよりもムービーデータ１本の消費ビットレートが大きい場合、
			/// ムービーデータの消費ビットレートを設定してください。
			/// つまり、アプリケーション全体を通して、最悪状態の消費ビットレートを想定した値を設定してください。
			/// </para>
			/// </remarks>
			public Int32 maxBps;

			/// <summary>CRI Mana側で再生する最大ストリーミング数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Mana側で再生する瞬間最大ストリーミング本数を指定します。
			/// D-BASのメモリ使用量の計算では、max_streams からこの値を減じたストリーミング本数を、
			/// CRI Atomの最大ストリーミング本数として扱います。
			/// CRI Mana側でストリーミング再生を行わない場合は 0 に設定してください。
			/// </para>
			/// </remarks>
			public Int32 maxManaStreams;

			/// <summary>CRI Mana側で再生する最大ビットレート</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Mana側で再生する瞬間最大ビットレートを指定します。
			/// D-BASのメモリ使用量の計算では、max_bps からこの値を減じたビットレートを、
			/// CRI Atomの最大ビットレートとして扱います。
			/// CRI Mana側でストリーミング再生を行わない場合は 0 に設定してください。
			/// </para>
			/// </remarks>
			public Int32 maxManaBps;

			/// <summary>１ストリームに割り当てる最低保証バッファー数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// １ストリームに割り当てる最低保証バッファー数を指定します。単位は[個]です。
			/// ADX内部ではバッファーをブロック単位で管理しているため、
			/// ファイル終端やループ先頭等の半端なデータサイズに対しても、
			/// バッファーブロックを１つ分割り当てます。
			/// ワンショットのストリーミング再生では、
			/// ダブルバッファリングでデータを読むだけの単純な処理であっても、
			/// ストリームが途切れる事はありません。
			/// 一方、ループ付きデータの場合では、ループ終端のわずかなデータに１ブロック、
			/// ループ先頭のわずかなデータにも１ブロックを使ってしまうと、
			/// バッファリング済みのデータが極端に少ないにもかかわらず、
			/// 次のデータを読み込むバッファーが空かないためストリームが途切れてしまいます。
			/// 本パラメーターは、D-BASが確保するストリーミングバッファーサイズに影響を与えます。
			/// 音が途切れない事を十分に確認できていれば、
			/// 本パラメーターの下限値は <see cref="CriAtomDbas.MinimumNumSecurementBuffers"/> です。
			/// </para>
			/// </remarks>
			public Int32 numSecurementBuffers;

		}
		/// <summary>D-BASの作成</summary>
		/// <param name="config">D-BAS作成用コンフィグ構造体へのポインタ</param>
		/// <param name="work">D-BAS作成用ワーク領域へのポインタ</param>
		/// <param name="workSize">D-BAS作成用ワークサイズ</param>
		/// <returns>D-BAS管理用ID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// D-BAS作成用パラメーターに基づいて、D-BASを作成します。
		/// 作成に成功すると、D-BASをライブラリに登録し、有効な管理用IDを返します。
		/// D-BASの作成に失敗した場合、本関数は <see cref="CriAtomDbas.IllegalId"/> を返します。
		/// （エラーの原因はエラーコールバックに返されます。）
		/// 取得したIDは<see cref="CriAtomDbas.Dispose"/> 関数で使用します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDbas.CalculateWorkSize"/>
		/// <seealso cref="CriAtomDbas.Dispose"/>
		public unsafe CriAtomDbas(in CriAtomDbas.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomDbas.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomDbas_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomDbas(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomDbas.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomDbas_Create(configPtr, work, workSize);
		}

		/// <summary>D-BASの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomDbas.CriAtomDbas"/> 関数で取得した管理用IDを指定して、D-BASを破棄します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDbas.CriAtomDbas"/>
		public void Dispose()
		{
			if (!CriAtom.IsInitialized()) return;
			NativeMethods.criAtomDbas_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomDbas() => Dispose();
#pragma warning restore 1591

		/// <summary>Atom D-BAS ID</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomDbas.CriAtomDbas"/> 関数に失敗した際に返る値です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDbas.CriAtomDbas"/>
		/// <seealso cref="CriAtomDbas.Dispose"/>
		public const Int32 IllegalId = (-1);
		/// <summary>D-BAS作成時に指定可能なバッファー保証数の下限値</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// D-BAS作成時に指定可能なバッファー保証数の下限値です。
		/// 途切れずにストリーミング再生するためには、
		/// 最低でも2バッファー（ダブルバッファリング）必要なため、
		/// 2未満の値に設定する事はできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDbas.CriAtomDbas"/>
		/// <seealso cref="CriAtomDbas.CalculateWorkSize"/>
		public const Int32 MinimumNumSecurementBuffers = (2);
		/// <summary>ネイティブハンドル</summary>

		public Int32 NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomDbas(Int32 handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomDbas other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomDbas a, CriAtomDbas b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomDbas a, CriAtomDbas b) =>
			!(a == b);

	}
}