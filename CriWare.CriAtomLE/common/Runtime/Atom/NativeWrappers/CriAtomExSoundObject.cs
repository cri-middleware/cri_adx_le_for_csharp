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
	/// <summary>サウンドオブジェクトオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomExSoundObject"/> は、サウンドオブジェクトを操作するためのオブジェクトです。
	/// <see cref="CriAtomExSoundObject.CriAtomExSoundObject"/> 関数でサウンドオブジェクトを作成すると、
	/// 関数はサウンドオブジェクト操作用に、この"サウンドオブジェクトオブジェクト"を返します。
	/// サウンドオブジェクトに対して行う操作は、全てサウンドオブジェクトオブジェクトを介して実行されます。
	/// </para>
	/// <para>
	/// 備考:
	/// サウンドオブジェクトとは、複数の音が鳴る「物体」や「空間」、「状況」等を抽象 化した概念です。
	/// サウンドオブジェクトをアプリケーション内の「物体」や「空間」、「状況」等に関連付けることにより、
	/// より自然に音声のコントロールを行うことができます。
	/// 例えば、あるキャラクタが存在するとき、そのキャラクタ用のサウンドオブジェクトを作成することで、
	/// キャラクタ毎に発音数制限を行ったり、キャラクタ消滅とともにまとめて再生停止を行う、
	/// というようなことが簡単にできるようになります。
	/// サウンドオブジェクト自身は、発音のための機能を持ちません。
	/// 発音や個別のコントロールは、サウンドオブジェクトに関連付けられたAtomExプレーヤーで行います。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExSoundObject.CriAtomExSoundObject"/>
	public partial class CriAtomExSoundObject : IDisposable
	{
		/// <summary>サウンドオブジェクト作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">サウンドオブジェクト作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExSoundObject.CriAtomExSoundObject"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExSoundObject.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.Config"/>
		/// <seealso cref="CriAtomExSoundObject.CriAtomExSoundObject"/>
		public static unsafe void SetDefaultConfig(out CriAtomExSoundObject.Config pConfig)
		{
			fixed (CriAtomExSoundObject.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExSoundObject_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>サウンドオブジェクト用ワーク領域サイズの計算</summary>
		/// <param name="config">サウンドオブジェクト作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドオブジェクトを作成するために必要な、ワーク領域のサイズを取得します。
		/// アロケーターを登録せずにサウンドオブジェクトを作成する場合、
		/// あらかじめ本関数で計算したワーク領域サイズ分のメモリを
		/// ワーク領域として <see cref="CriAtomExSoundObject.CriAtomExSoundObject"/> 関数にセットする必要があります。
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomExSoundObject.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExSoundObject.SetDefaultConfig"/> 適用時と同じパラメーター）で
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
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.Config"/>
		/// <seealso cref="CriAtomExSoundObject.CriAtomExSoundObject"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExSoundObject.Config config)
		{
			fixed (CriAtomExSoundObject.Config* configPtr = &config)
				return NativeMethods.criAtomExSoundObject_CalculateWorkSize(configPtr);
		}

		/// <summary>サウンドオブジェクト作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドオブジェクトを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomExSoundObject.CriAtomExSoundObject"/> 関数の引数に指定します。
		/// 作成されるサウンドオブジェクトは、オブジェクト作成時に本構造体で指定された設定に応じて、
		/// 内部リソースを必要なだけ確保します。
		/// サウンドオブジェクトが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、各メンバの設定前に必ず
		/// <see cref="CriAtomExSoundObject.SetDefaultConfig"/> メソッドを使用してデフォルト値をセットしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.CriAtomExSoundObject"/>
		/// <seealso cref="CriAtomExSoundObject.SetDefaultConfig"/>
		public unsafe partial struct Config
		{
			/// <summary>ボイスリミットスコープの有効化</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ボイスリミットグループによる発音数制御を、このサウンドオブジェクトで独立して行うかどうかを指定します。
			/// trueを指定すると、このサウンドオブジェクトに関連付けられたExプレーヤーから再生した音声の発音数について、
			/// このサウンドオブジェクト内でのみカウントし、ボイスリミットグループによる発音数制御を行います。
			/// falseを指定した場合、サウンドオブジェクトではボイスリミットグループによる発音数制御は行わず、
			/// CRI Atomライブラリ全体での発音数制御に従います。
			/// </para>
			/// <para>
			/// 備考:
			/// デフォルト値はfalse（サウンドオブジェクトでボイスリミットを行わない）です。
			/// </para>
			/// </remarks>
			public NativeBool enableVoiceLimitScope;

			/// <summary>カテゴリキューリミットスコープの有効化</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// カテゴリによるキュー再生数制御を、このサウンドオブジェクトで独立して行うかどうかを指定します。
			/// trueを指定すると、このサウンドオブジェクトに関連付けられたExプレーヤーから再生したキューのカテゴリ再生数について、
			/// このサウンドオブジェクト内でのみカウントし、再生数制御を行います。
			/// falseを指定した場合、このサウンドオブジェクトではカテゴリによる再生数制御は行わず、
			/// CRI Atomライブラリ全体でのカテゴリによる再生数制御に従います。
			/// </para>
			/// <para>
			/// 備考:
			/// デフォルト値はfalse（サウンドオブジェクトでカテゴリキューリミットを行わない）です。
			/// </para>
			/// </remarks>
			public NativeBool enableCategoryCueLimitScope;

		}
		/// <summary>サウンドオブジェクトの作成</summary>
		/// <param name="config">サウンドオブジェクト作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>サウンドオブジェクトオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドオブジェクトを作成します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.Config"/>
		/// <seealso cref="CriAtomExSoundObject.CalculateWorkSize"/>
		/// <seealso cref="CriAtomExSoundObject"/>
		/// <seealso cref="CriAtomExSoundObject.Dispose"/>
		public unsafe CriAtomExSoundObject(in CriAtomExSoundObject.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExSoundObject.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomExSoundObject_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomExSoundObject(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomExSoundObject.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomExSoundObject_Create(configPtr, work, workSize);
		}

		/// <summary>サウンドオブジェクトの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドオブジェクトを破棄します。
		/// 本関数を実行した時点で、サウンドオブジェクト作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定したサウンドオブジェクトオブジェクトも無効になります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.CriAtomExSoundObject"/>
		/// <seealso cref="CriAtomExSoundObject"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomExSoundObject_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomExSoundObject() => Dispose();
#pragma warning restore 1591

		/// <summary>AtomExプレーヤーの追加</summary>
		/// <param name="player">AtomExプレーヤー</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドオブジェクトにAtomExプレーヤーを追加します。
		/// 追加したAtomExプレーヤーはサウンドオブジェクトと関連付けられ、
		/// サウンドオブジェクトによる以下の影響を受けるようになります。
		/// - 発音数制限やイベント機能が影響する範囲（スコープ）の限定
		/// - 再生コントロール（停止、ポーズ等）
		/// - パラメーターコントロール
		/// 追加したAtomExプレーヤーをサウンドオブジェクトから削除する場合は、 <see cref="CriAtomExSoundObject.DeletePlayer"/>
		/// 関数を呼び出してください。
		/// 対象のAtomExプレーヤーが既にサウンドオブジェクトに追加済みの場合は、何も起こりません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数の呼び出しは、追加しようとしているAtomExプレーヤーで音声を再生していない状態で行ってください。
		/// ステータスが <see cref="CriAtomExPlayer.Status.Stop"/> ではないAtomExプレーヤーが指定された場合、
		/// 追加時に <see cref="CriAtomExPlayer.StopWithoutReleaseTime"/> 関数にて再生停止が行われます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.DeletePlayer"/>
		/// <seealso cref="CriAtomExSoundObject.DeleteAllPlayers"/>
		public void AddPlayer(CriAtomExPlayer player)
		{
			NativeMethods.criAtomExSoundObject_AddPlayer(NativeHandle, player?.NativeHandle ?? default);
		}

		/// <summary>AtomExプレーヤーの削除</summary>
		/// <param name="player">AtomExプレーヤー</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドオブジェクトからAtomExプレーヤーを削除します。
		/// 削除したAtomExプレーヤーはサウンドオブジェクトとの関連付けが切られ、
		/// サウンドオブジェクトによる影響を受けなくなります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数の呼び出しは、削除しようとしているAtomExプレーヤーで音声を再生していない状態で行ってください。
		/// ステータスが <see cref="CriAtomExPlayer.Status.Stop"/> ではないAtomExプレーヤーが指定された場合、
		/// 削除時に <see cref="CriAtomExPlayer.StopWithoutReleaseTime"/> 関数にて再生停止が行われます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.AddPlayer"/>
		/// <seealso cref="CriAtomExSoundObject.DeleteAllPlayers"/>
		public void DeletePlayer(CriAtomExPlayer player)
		{
			NativeMethods.criAtomExSoundObject_DeletePlayer(NativeHandle, player?.NativeHandle ?? default);
		}

		/// <summary>全てのAtomExプレーヤーの削除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サウンドオブジェクトに関連付けられている全てのAtomExプレーヤーを削除します。
		/// 削除したAtomExプレーヤーはサウンドオブジェクトとの関連付けが切られ、
		/// サウンドオブジェクトによる影響を受けなくなります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数の呼び出しは、削除しようとしているAtomExプレーヤーで音声を再生していない状態で行ってください。
		/// ステータスが <see cref="CriAtomExPlayer.Status.Stop"/> ではないAtomExプレーヤーが含まれていた場合、
		/// 削除時に <see cref="CriAtomExPlayer.StopWithoutReleaseTime"/> 関数にて再生停止が行われます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject.AddPlayer"/>
		/// <seealso cref="CriAtomExSoundObject.DeletePlayer"/>
		public void DeleteAllPlayers()
		{
			NativeMethods.criAtomExSoundObject_DeleteAllPlayers(NativeHandle);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExSoundObject(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExSoundObject other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExSoundObject a, CriAtomExSoundObject b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExSoundObject a, CriAtomExSoundObject b) =>
			!(a == b);

	}
}