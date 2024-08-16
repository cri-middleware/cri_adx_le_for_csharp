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
	/// <summary>再生ID</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomExPlayer.Start"/> 関数実行時に返されるIDです。
	/// プレーヤー単位ではなく、 <see cref="CriAtomExPlayer.Start"/> 関数で再生した個々の音声に対して
	/// パラメーター変更や状態取得を行いたい場合、本IDを使用して制御を行う必要があります。
	/// 無効な再生IDは<see cref="CriAtomEx.InvalidPlaybackId"/>です。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExPlayer.Start"/>
	/// <seealso cref="CriAtomExPlayback.GetStatus"/>
	/// <seealso cref="CriAtomEx.InvalidPlaybackId"/>
	public partial struct CriAtomExPlayback
	{
		/// <summary>ネイティブハンドル</summary>

		public UInt32 NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExPlayback(UInt32 handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExPlayback other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExPlayback a, CriAtomExPlayback b)
		{

			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExPlayback a, CriAtomExPlayback b) =>
			!(a == b);

		/// <summary>プレイバックキャンセルコールバック</summary>
		/// <returns>
		/// 
		/// AtomExライブラリのプレイバックキャンセルコールバック関数型です。
		/// コールバック関数の登録には <see cref="CriAtomEx.SetPlaybackCancelCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、ライブラリ内で再生開始処理がキャンセルされるタイミングで実行されます。
		/// そのため、ライブラリ処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </returns>
		/// <remarks>
		/// <para>説明:</para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetPlaybackCancelCallback"/>
		public unsafe class CancelCbFunc : NativeCallbackBase<CancelCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>プレイバックキャンセル情報</summary>
				public NativeReference<CriAtomExPlayback.CancelInfo> info { get; }

				internal Arg(NativeReference<CriAtomExPlayback.CancelInfo> info)
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
			static void CallbackFunc(IntPtr obj, CriAtomExPlayback.CancelInfo* info) =>
				InvokeCallbackInternal(obj, new(info));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomExPlayback.CancelInfo* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal CancelCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.CancelInfo*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>プレイバックキャンセルコールバック用Info構造体</summary>
		public unsafe partial struct CancelInfo
		{
			/// <summary>キャンセルタイプ</summary>
			public CriAtomExPlayback.CancelType type;

			/// <summary>プレーヤーオブジェクト</summary>
			public IntPtr player;

			/// <summary>再生ID</summary>
			public UInt32 id;

		}
		/// <summary>プレイバックキャンセルタイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレイバックキャンセルの種別を示す値です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.CancelInfo"/>
		/// <seealso cref="CriAtomEx.SetPlaybackCancelCallback"/>
		public enum CancelType
		{
			/// <summary>キューリミット</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// キューリミットによる発音キャンセル。
			/// </para>
			/// </remarks>
			CueLimit = 0,
			/// <summary>カテゴリキューリミット</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// カテゴリキューリミットによる発音キャンセル。
			/// </para>
			/// </remarks>
			CategoryCueLimit = 1,
			/// <summary>プロバビリティ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 確率による発音キャンセル。
			/// </para>
			/// </remarks>
			Probability = 2,
			/// <summary>キューリミット</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// キューリミットによる発音停止。
			/// </para>
			/// </remarks>
			StopByCueLimit = 3,
			/// <summary>スイッチ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// スイッチによる発音キャンセル。
			/// </para>
			/// </remarks>
			Switch = 4,
			/// <summary>トラック不明</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生トラック不明による発音キャンセル。
			/// </para>
			/// </remarks>
			NoTrackToPlay = 5,
		}
		/// <summary>プレイバックコールバック関数型</summary>
		/// <returns>列挙を続けるかどうか（true：継続、false：中止）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレイバックの列挙に使用する、コールバック関数の型です。
		/// <see cref="CriAtomExPlayer.EnumeratePlaybacks"/> 関数に本関数型のコールバック関数を登録することで、
		/// プレーヤーで再生中のプレイバックIDをコールバックで受け取ることが可能となります。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.EnumeratePlaybacks"/>
		public unsafe class CbFunc : NativeCallbackBase<CbFunc.Arg, NativeBool>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>プレイバックID</summary>
				public UInt32 playbackId { get; }

				internal Arg(UInt32 playbackId)
				{
					this.playbackId = playbackId;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static NativeBool CallbackFunc(IntPtr obj, UInt32 playbackId) =>
				InvokeCallbackInternal(obj, new(playbackId));
#if !NET5_0_OR_GREATER
			delegate NativeBool NativeDelegate(IntPtr obj, UInt32 playbackId);
			static NativeDelegate callbackDelegate = null;
#endif
			internal CbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, UInt32, NativeBool>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>再生トラック情報用Info構造体</summary>
		public unsafe partial struct TrackInfo
		{
			/// <summary>再生ID</summary>
			public UInt32 id;

			/// <summary>親シーケンスタイプ</summary>
			public CriAtomExAcb.CueType sequenceType;

			/// <summary>プレーヤーオブジェクト</summary>
			public IntPtr player;

			/// <summary>トラック番号</summary>
			public UInt16 trackNo;

			/// <summary>予約領域</summary>
			public InlineArray1<UInt16> reserved;

		}
		/// <summary>再生イベントコールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生イベントの通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtomExPlayer.SetPlaybackEventCallback"/> 関数に本関数型のコールバック関数を登録することで、
		/// 再生イベント発生時にコールバックを受け取ることが可能となります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPlaybackEventCallback"/>
		/// <seealso cref="CriAtomExPlayback.Event"/>
		/// <seealso cref="CriAtomExPlayback.InfoDetail"/>
		public unsafe class EventCbFunc : NativeCallbackBase<EventCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>発生したイベント</summary>
				public CriAtomExPlayback.Event playbackEvent { get; }
				/// <summary>詳細情報</summary>
				public NativeReference<CriAtomExPlayback.InfoDetail> info { get; }

				internal Arg(CriAtomExPlayback.Event playbackEvent, NativeReference<CriAtomExPlayback.InfoDetail> info)
				{
					this.playbackEvent = playbackEvent;
					this.info = info;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, CriAtomExPlayback.Event playbackEvent, CriAtomExPlayback.InfoDetail* info) =>
				InvokeCallbackInternal(obj, new(playbackEvent, info));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomExPlayback.Event playbackEvent, CriAtomExPlayback.InfoDetail* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal EventCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.Event, CriAtomExPlayback.InfoDetail*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>再生イベント</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生イベントの種別を示す値です。
		/// 再生イベントコールバックに引数として渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.EventCbFunc"/>
		/// <seealso cref="CriAtomExPlayer.SetPlaybackEventCallback"/>
		public enum Event
		{
			/// <summary>新規再生リソースの確保</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// キューの再生に必要なリソースが確保されたことを示す値です。
			/// リソース確保時点ではボイスの割り当ては行われておらず、
			/// 発音がされていません（バーチャル化した状態で作成されます）。
			/// </para>
			/// </remarks>
			Allocate = 0,
			/// <summary>ボイスの割り当て</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// バーチャル状態の再生リソースに対してボイスが割り当てられたことを示す値です。
			/// ボイスが割り当てられたことで、キューの発音が開始されます。
			/// </para>
			/// <para>
			/// 備考:
			/// キューに複数の波形データが含まれる場合、いずれか1つの波形データが再生された時点で本イベントが発生します。
			/// （キュー再生に関連するボイスの数が0から1に変わる瞬間に本イベントが発生します。）
			/// 既にボイスが割り当てられた状態で、さらに追加のボイスが割り当てられるタイミングでは本イベントは発生ません。
			/// </para>
			/// </remarks>
			FromVirtualToNormal = 1,
			/// <summary>バーチャル化</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// キューの再生がバーチャル化されたことを示す値です。
			/// 以下のいずれかの要因により、発音中のキューからボイスが切り離された場合に発生します。
			/// - キューに含まれる波形データを終端まで再生したため、ボイスが不要になった
			/// - <see cref="CriAtomExPlayer.Stop"/> 関数等の呼び出しにより、再生中の波形データが停止された
			/// - プライオリティ制御により、再生中の波形データが停止され、ボイスが奪い取られた
			/// </para>
			/// <para>
			/// 備考:
			/// 本イベントは、キューに含まれる"波形データ"が再生されなくなった状態を示します。
			/// 本イベント発生時点では、キューの再生は終了していません。
			/// （キューの再生が終了した際には、別途 <see cref="CriAtomExPlayback.Event.Remove"/> イベントが発生します。）
			/// キューに複数の波形データが含まれる場合、全ての波形データが再生されなくなった時点で本イベントが発生します。
			/// （キュー再生に関連するボイスの数が1から0に変わる瞬間に本イベントが発生します。）
			/// 複数のボイスが割り当てられた状態でそのうちの1つが停止された場合には、本イベントは発生ません。
			/// </para>
			/// </remarks>
			FromNormalToVirtual = 2,
			/// <summary>再生リソースの解放</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 再生リソースが解放されたことを示す値です。
			/// キューの再生が完了した際や、再生停止要求によりキューが停止された場合に本イベントが発生します。
			/// </para>
			/// <para>
			/// 備考:
			/// キューに含まれる波形データが再生されている場合、
			/// 本イベント発生前に、必ず <see cref="CriAtomExPlayback.Event.FromNormalToVirtual"/> イベントが発生します。
			/// </para>
			/// </remarks>
			Remove = 3,
		}
		/// <summary>再生情報詳細</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生イベント発生時に、当該再生に関する詳細情報を通知するための構造体です。
		/// 再生イベントコールバックに引数として渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.EventCbFunc"/>
		/// <seealso cref="CriAtomExPlayer.SetPlaybackEventCallback"/>
		public unsafe partial struct InfoDetail
		{
			/// <summary>再生中のプレーヤー</summary>
			public IntPtr player;

			/// <summary>再生ID</summary>
			public UInt32 id;

		}
		/// <summary>再生音の停止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生音単位で停止処理を行います。
		/// 本関数を使用することで、プレーヤーによって再生された音声を、プレーヤー単位ではなく、
		/// 個別に停止させることが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// AtomEx プレーヤーによって再生された全ての音声を停止したい場合、
		/// 本関数ではなく <see cref="CriAtomExPlayer.Stop"/> 関数をご利用ください。
		/// （ <see cref="CriAtomExPlayer.Stop"/> 関数は、そのプレーヤーで再生中の全ての音声を停止します。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で再生音の停止を行うと、再生中の音声のステータスは
		/// <see cref="CriAtomExPlayback.Status.Removed"/> に遷移します。
		/// 停止時にボイスリソースも破棄されるため、一旦 <see cref="CriAtomExPlayback.Status.Removed"/>
		/// 状態に遷移した再生 ID からは、以降情報を取得できなくなります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Stop"/>
		/// <seealso cref="CriAtomExPlayback.GetStatus"/>
		public void Stop()
		{
			NativeMethods.criAtomExPlayback_Stop(NativeHandle);
		}

		/// <summary>再生音の停止（リリースタイム無視）</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生音単位で停止処理を行います。
		/// この際、再生中の音声にエンベロープのリリースタイムが設定されていたとしても、それを無視して停止します。
		/// 本関数を使用することで、プレーヤーによって再生された音声を、プレーヤー単位ではなく、
		/// 個別に停止させることが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーによって再生された全ての音声を停止したい場合、
		/// 本関数ではなく <see cref="CriAtomExPlayer.StopWithoutReleaseTime"/> 関数をご利用ください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で再生音の停止を行うと、再生中の音声のステータスは
		/// <see cref="CriAtomExPlayback.Status.Removed"/> に遷移します。
		/// 停止時にボイスリソースも破棄されるため、一旦 <see cref="CriAtomExPlayback.Status.Removed"/>
		/// 状態に遷移した再生 ID からは、以降情報を取得できなくなります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.StopWithoutReleaseTime"/>
		public void StopWithoutReleaseTime()
		{
			NativeMethods.criAtomExPlayback_StopWithoutReleaseTime(NativeHandle);
		}

		/// <summary>再生音のポーズ／ポーズ解除</summary>
		/// <param name="sw">スイッチ（ false = ポーズ解除、 true = ポーズ ）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生音単位でポーズ／ポーズ解除を行います。
		/// sw に true を指定して本関数を実行すると、指定したIDの音声がポーズ
		/// （一時停止）されます。
		/// sw に false を指定して本関数を実行すると、指定したIDの音声のポーズが
		/// 解除され、一時停止していた音声の再生が再開されます。
		/// 本関数を使用することで、プレーヤーによって再生された音声を、プレーヤー単位ではなく、
		/// 個別にポーズ／ポーズ解除させることが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーによって再生された全ての音声をポーズ／ポーズ解除したい場合、
		/// 本関数ではなく <see cref="CriAtomExPlayer.Pause"/> 関数をご利用ください。
		/// フェーダーをアタッチしたプレーヤーで再生した音声に対して本関数で個別にポーズ／ポーズ解除の操作を行った場合、
		/// クロスフェード処理はフェードイン側の音声のポーズ状態に同期して行われます。
		/// 例えば、クロスフェード中に<see cref="CriAtomExPlayer.Pause"/> 関数で両方の音声をポーズした場合、
		/// フェードイン側の音声のポーズを解除すればクロスフェード処理が再開されますが、
		/// フェードアウト側の音声のポーズを解除してもクロスフェード処理は再開されません。
		/// </para>
		/// <para>
		/// 注意:
		/// 第2引数（sw）に false を指定してポーズ解除の操作を行った場合、
		/// 本関数でポーズをかけた音声だけでなく、<see cref="CriAtomExPlayer.Prepare"/>
		/// 関数で再生準備中の音声についても再生が開始されてしまいます。
		/// （旧バージョンとの互換性維持のための仕様です。）
		/// 本関数でポーズをかけた音声についてのみポーズを解除したい場合、
		/// 本関数を使用せず、 <see cref="CriAtomExPlayback.Resume"/>(id, <see cref="CriAtomEx.ResumeMode.PausedPlayback"/>);
		/// を実行してポーズ解除を行ってください。
		/// フェーダーをアタッチしたプレーヤーに対し再生ID指定でポーズの解除を行うと、
		/// フェードインする音声の発音リソースが確保できない場合や、
		/// フェードインする音声の発音リソースが奪い取られた場合にポーズ解除が行えず、
		/// フェードアウト側の音声がいつまで経ってもフェードアウトしない状態となります。
		/// フェーダーをアタッチしたプレーヤーで再生した音声に対しては、本関数ではなく、
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズの解除を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.IsPaused"/>
		/// <seealso cref="CriAtomExPlayer.Pause"/>
		/// <seealso cref="CriAtomExPlayback.Resume"/>
		public void Pause(NativeBool sw)
		{
			NativeMethods.criAtomExPlayback_Pause(NativeHandle, sw);
		}

		/// <summary>再生音の機能別のポーズ解除</summary>
		/// <param name="mode">ポーズ解除対象</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生音単位で一時停止状態の解除を行います。
		/// <see cref="CriAtomExPlayback.Pause"/> 関数と異なり、 <see cref="CriAtomExPlayer.Prepare"/>
		/// 関数で再生開始待ちの音声と、 <see cref="CriAtomExPlayer.Pause"/> 関数（または
		/// <see cref="CriAtomExPlayback.Pause"/> 関数でポーズをかけた音声とを、
		/// 個別に再開させることが可能です。
		/// 第2引数（mode）に <see cref="CriAtomEx.ResumeMode.PausedPlayback"/> を指定して本関数を実行すると、
		/// ユーザが <see cref="CriAtomExPlayer.Pause"/> 関数（または <see cref="CriAtomExPlayback.Pause"/>
		/// 関数）で一時停止状態になった音声の再生が再開されます。
		/// 第2引数（mode）に <see cref="CriAtomEx.ResumeMode.PreparedPlayback"/> を指定して本関数を実行すると、
		/// ユーザが <see cref="CriAtomExPlayer.Prepare"/> 関数で再生準備を指示した音声の再生が開始されます。
		/// <see cref="CriAtomExPlayback.Pause"/> 関数でポーズ状態のプレーヤーに対して
		/// <see cref="CriAtomExPlayer.Prepare"/> 関数で再生準備を行った場合、
		/// その音声は <see cref="CriAtomEx.ResumeMode.PausedPlayback"/>
		/// 指定のポーズ解除処理と、 <see cref="CriAtomEx.ResumeMode.PreparedPlayback"/>
		/// 指定のポーズ解除処理の両方が行われるまで、再生が開始されません。
		/// </para>
		/// <para>
		/// 備考:
		/// フェーダーをアタッチしたプレーヤーで再生した音声に対して本関数で個別にポーズ解除の操作を行った場合、
		/// クロスフェード処理はフェードイン側の音声のポーズ状態に同期して行われます。
		/// 例えば、クロスフェード中に<see cref="CriAtomExPlayer.Pause"/> 関数で両方の音声をポーズした場合、
		/// フェードイン側の音声のポーズを解除すればクロスフェード処理が再開されますが、
		/// フェードアウト側の音声のポーズを解除してもクロスフェード処理は再開されません。
		/// </para>
		/// <para>
		/// 注意:
		/// フェーダーをアタッチしたプレーヤーに対し再生ID指定でポーズの解除を行うと、
		/// フェードインする音声の発音リソースが確保できない場合や、
		/// フェードインする音声の発音リソースが奪い取られた場合にポーズ解除が行えず、
		/// フェードアウト側の音声がいつまで経ってもフェードアウトしない状態となります。
		/// フェーダーをアタッチしたプレーヤーで再生した音声に対しては、本関数ではなく、
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズの解除を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.IsPaused"/>
		/// <seealso cref="CriAtomExPlayer.Resume"/>
		/// <seealso cref="CriAtomExPlayer.Pause"/>
		public void Resume(CriAtomEx.ResumeMode mode)
		{
			NativeMethods.criAtomExPlayback_Resume(NativeHandle, mode);
		}

		/// <summary>再生音のポーズ状態の取得</summary>
		/// <returns>ポーズ中かどうか（false = ポーズされていない、true = ポーズ中）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生中の音声がポーズ中かどうかを返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.Pause"/>
		public bool IsPaused()
		{
			return NativeMethods.criAtomExPlayback_IsPaused(NativeHandle);
		}

		/// <summary>再生ステータスの取得</summary>
		/// <returns>再生ステータス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声のステータスを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExPlayer.GetStatus"/> 関数がAtomExプレーヤーのステータスを返すのに対し、
		/// 本関数は再生済みの個々の音声のステータスを取得します。
		/// 再生中の音声のボイスリソースは、以下の場合に削除されます。
		/// - 再生が完了した場合。
		/// - <see cref="CriAtomExPlayback.Stop"/> 関数で再生中の音声を停止した場合。
		/// - 高プライオリティの発音リクエストにより再生中のボイスが奪い取られた場合。
		/// - 再生中にエラーが発生した場合。
		/// そのため、 <see cref="CriAtomExPlayback.Stop"/> 関数を使用して明示的に再生を停止したか、
		/// その他の要因によって再生が停止されたかの違いに関係なく、
		/// 再生音のステータスはいずれの場合も <see cref="CriAtomExPlayback.Status.Removed"/>
		/// に遷移します。
		/// （エラーの発生を検知する必要がある場合には、本関数ではなく、<see cref="CriAtomExPlayer.GetStatus"/>
		/// 関数で AtomEx プレーヤーのステータスをチェックする必要があります。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		/// <seealso cref="CriAtomExPlayback.Stop"/>
		public CriAtomExPlayback.Status GetStatus()
		{
			return NativeMethods.criAtomExPlayback_GetStatus(NativeHandle);
		}

		/// <summary>再生ステータス</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生済みの音声のステータスです。
		/// <see cref="CriAtomExPlayback.GetStatus"/> 関数で取得可能です。
		/// 再生状態は、通常以下の順序で遷移します。
		/// -# <see cref="CriAtomExPlayback.Status.Prep"/>
		/// -# <see cref="CriAtomExPlayback.Status.Playing"/>
		/// -# <see cref="CriAtomExPlayback.Status.Removed"/>
		/// </para>
		/// <para>
		/// 備考
		/// <see cref="CriAtomExPlayback.Status"/>はAtomExプレーヤーのステータスではなく、
		/// プレーヤーで再生を行った（ <see cref="CriAtomExPlayer.Start"/> 関数を実行した）
		/// 音声のステータスです。
		/// 再生中の音声リソースは、発音が停止された時点で破棄されます。
		/// そのため、以下のケースで再生音のステータスが
		/// <see cref="CriAtomExPlayback.Status.Removed"/> に遷移します。
		/// - 再生が完了した場合。
		/// - <see cref="CriAtomExPlayback.Stop"/> 関数で再生中の音声を停止した場合。
		/// - 高プライオリティの発音リクエストにより再生中のボイスが奪い取られた場合。
		/// - 再生中にエラーが発生した場合。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayback.GetStatus"/>
		/// <seealso cref="CriAtomExPlayback.Stop"/>
		public enum Status
		{
			/// <summary>再生準備中</summary>
			Prep = 1,
			/// <summary>再生中</summary>
			Playing = 2,
			/// <summary>削除された</summary>
			Removed = 3,
		}
		/// <summary>再生音声のフォーマット情報の取得</summary>
		/// <param name="info">フォーマット情報</param>
		/// <returns>情報が取得できたかどうか（ true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声のフォーマット情報を取得します。
		/// フォーマット情報が取得できた場合、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 複数の音声データを含むキューを再生した場合、最初に見つかった音声
		/// データの情報が返されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみフォーマット情報を取得可能です。
		/// 再生準備中や再生終了後、発音数制御によりボイスが消去された場合には、
		/// フォーマット情報の取得に失敗します。
		/// ボイスの再生状態は <see cref="CriAtomExPlayback.GetStatus"/> 関数で取得することはできないためご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		public unsafe bool GetFormatInfo(out CriAtomEx.FormatInfo info)
		{
			fixed (CriAtomEx.FormatInfo* infoPtr = &info)
				return NativeMethods.criAtomExPlayback_GetFormatInfo(NativeHandle, infoPtr);
		}

		/// <summary>再生音声の再生元情報の取得</summary>
		/// <param name="source">再生元情報</param>
		/// <returns>= 情報が取得できた</returns>
		/// <returns>= 情報が取得できなかった</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声の再生元情報を取得します。
		/// 取得した情報を元に、<see cref="CriAtomExAcb.GetCueInfoByIndex"/> 関数等を利用することで、
		/// より詳細な情報を取得することができます。
		/// 再生元情報が取得できた場合、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// </para>
		/// <para>
		/// 備考
		/// 再生元のタイプによって、取得できる情報が異なります。
		/// typeを参照し、共用体sourceの中のどの構造体としてアクセスするかを選択してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみ再生元情報を取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// 再生元情報の取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		public unsafe bool GetSource(out CriAtomEx.SourceInfo source)
		{
			fixed (CriAtomEx.SourceInfo* sourcePtr = &source)
				return NativeMethods.criAtomExPlayback_GetSource(NativeHandle, sourcePtr);
		}

		/// <summary>Atomプレーヤーの取得</summary>
		/// <returns>Atomプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生IDに紐づけられたボイス（＝Atomプレーヤーオブジェクト）を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 複数の波形データを含むキューを再生している場合、
		/// 本関数は最初に見つかったボイスに対応するAtomプレーヤーオブジェクトを返します。
		/// 波形データが再生されていない場合、本関数はnullを返します。
		/// </para>
		/// </remarks>
		public CriAtomPlayer GetAtomPlayer()
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExPlayback_GetAtomPlayer(NativeHandle)) == IntPtr.Zero) ? null : new CriAtomPlayer(handle);
		}

		/// <summary>再生時刻の取得</summary>
		/// <returns>再生時刻（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声の再生時刻を取得します。
		/// 再生時刻が取得できた場合、本関数は 0 以上の値を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は負値を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数が返す再生時刻は「再生開始後からの経過時間」です。
		/// ループ再生時や、シームレス連結再生時を行った場合でも、
		/// 再生位置に応じて時刻が巻き戻ることはありません。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズをかけた場合、
		/// 再生時刻のカウントアップも停止します。
		/// （ポーズを解除すれば再度カウントアップが再開されます。）
		/// 本関数で取得可能な時刻の精度は、サーバー処理の周波数に依存します。
		/// （時刻の更新はサーバー処理単位で行われます。）
		/// より精度の高い時刻を取得する必要がある場合には、本関数の代わりに
		/// <see cref="CriAtomExPlayback.GetNumPlayedSamples"/> 関数を使用し、
		/// 再生済みサンプル数を取得してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 戻り値の型はCriSint64ですが、現状、32bit以上の精度はありません。
		/// 再生時刻を元に制御を行う場合、約24日で再生時刻が異常になる点に注意が必要です。
		/// （ 2147483647 ミリ秒を超えた時点で、再生時刻がオーバーフローし、負値になります。）
		/// 本関数は、音声再生中のみ時刻を取得可能です。
		/// （ <see cref="CriAtomExPlayer.GetTime"/> 関数と異なり、本関数は再生中の音声ごとに時刻を
		/// 取得可能ですが、再生終了時刻を取ることができません。）
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// 再生時刻の取得に失敗します。
		/// （負値が返ります。）
		/// ドライブでリードリトライ処理等が発生し、一時的に音声データの供給が途切れた場合でも、
		/// 再生時刻のカウントアップが途切れることはありません。
		/// （データ供給停止により再生が停止した場合でも、時刻は進み続けます。）
		/// そのため、本関数で取得した時刻を元に映像との同期を行った場合、
		/// リードリトライ発生毎に同期が大きくズレる可能性があります。
		/// 波形データと映像の同期を厳密に取る必要がある場合は、本関数の代わりに
		/// <see cref="CriAtomExPlayback.GetNumPlayedSamples"/> 関数を使用し、
		/// 再生済みサンプル数との同期を取ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetTime"/>
		/// <seealso cref="CriAtomExPlayback.GetNumPlayedSamples"/>
		public Int64 GetTime()
		{
			return NativeMethods.criAtomExPlayback_GetTime(NativeHandle);
		}

		/// <summary>再生時刻の取得（再生音声に同期した補正込み）</summary>
		/// <returns>再生時刻（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声の再生時刻を取得します。
		/// 再生時刻が取得できた場合、本関数は 0 以上の値を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は負値を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExPlayback.GetTime"/> 関数が返す「再生開始後からの経過時間」とは
		/// 異なり、本関数からは再生中の音声に同期した再生時刻を取得することが
		/// 可能です。
		/// デバイスのリードリトライ処理等により音声データの供給が途切れて
		/// 再生が停止した場合、またはシステムの割り込みにより音声出力が妨げられた
		/// 場合には、再生時刻のカウントアップが一時的に停止します。
		/// 再生された音声に厳密に同期した処理を行いたい場合は、本関数で
		/// 取得した再生時刻を用いてください。
		/// ただし、ループ再生時や、シームレス連結再生時に行った場合でも、
		/// 再生位置に応じて時刻が巻き戻ることはありません。
		/// また、波形の詰まっていないシーケンスキューや
		/// 再生波形が切り替わるブロックシーケンスキューに対しては、正常に再生時刻を
		/// 取得することができません。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズをかけた場合、
		/// 再生時刻のカウントアップも停止します。
		/// （ポーズを解除すれば再度カウントアップが再開されます。）
		/// 本関数による再生時刻の取得を行う場合は、対応するAtomExプレーヤー作成時に、
		/// <see cref="CriAtomExPlayer.Config"/> 構造体の enable_audio_synced_timer を true に
		/// 設定してください。
		/// デフォルトでは無効になっています。
		/// 戻り値の型は long ですが、現状、32bit以上の精度はありません。
		/// 再生時刻を元に制御を行う場合、約24日で再生時刻が異常になる点に注意が必要です。
		/// （ 2147483647 ミリ秒を超えた時点で、再生時刻がオーバーフローし、負値になります。）
		/// 本関数は、音声再生中のみ時刻を取得可能です。
		/// （ <see cref="CriAtomExPlayer.GetTime"/> 関数と異なり、本関数は再生中の音声ごとに時刻を
		/// 取得可能ですが、再生終了時刻を取ることができません。）
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// 再生時刻の取得に失敗します。
		/// （負値が返ります。）
		/// 本関数は内部で時刻計算を行っており、プラットフォームによっては処理負荷が
		/// 問題になる可能性があります。また、アプリケーションの同じフレーム内であっても、
		/// 呼び出し毎に更新された時刻を返します。
		/// アプリケーションによる再生時刻の利用方法にもよりますが、基本的に本関数を用いた
		/// 時刻取得は1フレームにつき一度のみ行うようにしてください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayback.GetTime"/>
		public Int64 GetTimeSyncedWithAudio()
		{
			return NativeMethods.criAtomExPlayback_GetTimeSyncedWithAudio(NativeHandle);
		}

		/// <summary>再生時刻の取得（再生音声に同期した補正込み）</summary>
		/// <returns>再生時刻（マイクロ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声の再生時刻を取得します。
		/// 再生時刻が取得できた場合、本関数は 0 以上の値を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は負値を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExPlayback.GetTime"/> 関数が返す「再生開始後からの経過時間」とは
		/// 異なり、本関数からは再生中の音声に同期した再生時刻を取得することが
		/// 可能です。
		/// デバイスのリードリトライ処理等により音声データの供給が途切れて
		/// 再生が停止した場合、またはシステムの割り込みにより音声出力が妨げられた
		/// 場合には、再生時刻のカウントアップが一時的に停止します。
		/// 再生された音声に厳密に同期した処理を行いたい場合は、本関数で
		/// 取得した再生時刻を用いてください。
		/// ただし、ループ再生時や、シームレス連結再生時に行った場合でも、
		/// 再生位置に応じて時刻が巻き戻ることはありません。
		/// また、波形の詰まっていないシーケンスキューや
		/// 再生波形が切り替わるブロックシーケンスキューに対しては、正常に再生時刻を
		/// 取得することができません。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズをかけた場合、
		/// 再生時刻のカウントアップも停止します。
		/// （ポーズを解除すれば再度カウントアップが再開されます。）
		/// 本関数による再生時刻の取得を行う場合は、対応するAtomExプレーヤー作成時に、
		/// <see cref="CriAtomExPlayer.Config"/> 構造体の enable_audio_synced_timer を true に
		/// 設定してください。
		/// デフォルトでは無効になっています。
		/// 本関数は、音声再生中のみ時刻を取得可能です。
		/// （ <see cref="CriAtomExPlayer.GetTime"/> 関数と異なり、本関数は再生中の音声ごとに時刻を
		/// 取得可能ですが、再生終了時刻を取ることができません。）
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// 再生時刻の取得に失敗します。
		/// （負値が返ります。）
		/// 本関数は内部で時刻計算を行っており、プラットフォームによっては処理負荷が
		/// 問題になる可能性があります。また、アプリケーションの同じフレーム内であっても、
		/// 呼び出し毎に更新された時刻を返します。
		/// アプリケーションによる再生時刻の利用方法にもよりますが、基本的に本関数を用いた
		/// 時刻取得は1フレームにつき一度のみ行うようにしてください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayback.GetTime"/>
		public Int64 GetTimeSyncedWithAudioMicro()
		{
			return NativeMethods.criAtomExPlayback_GetTimeSyncedWithAudioMicro(NativeHandle);
		}

		/// <summary>シーケンス再生位置の取得</summary>
		/// <returns>シーケンス再生位置（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声のシーケンス再生位置を取得します。
		/// 再生位置が取得できた場合、本関数は 0 以上の値を返します。
		/// 指定したシーケンスが既に消去されている場合等には、本関数は負値を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数が返す再生時刻は「シーケンスデータ上の再生位置」です。
		/// シーケンスループや、ブロック遷移を行った場合は、巻き戻った値が返ります。
		/// キュー指定以外での再生ではシーケンサーが動作しません。キュー再生以外の再生に対して
		/// 本関数は負値を返します。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズをかけた場合、
		/// 再生位置の更新も停止します。
		/// （ポーズを解除すれば再度更新が再開されます。）
		/// 本関数で取得可能な時刻の精度は、サーバー処理の周波数に依存します。
		/// （時刻の更新はサーバー処理単位で行われます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 戻り値の型はCriSint64ですが、現状、32bit以上の精度はありません。
		/// 再生位置を元に制御を行う場合、シーケンスループ等の設定がないデータでは約24日で再生位置が異常になる点に注意が必要です。
		/// （ 2147483647 ミリ秒を超えた時点で、再生位置がオーバーフローし、負値になります。）
		/// 本関数は、音声再生中のみ位置を取得可能です。
		/// 再生終了後や、発音数制御によりシーケンスが消去された場合には、
		/// 再生位置の取得に失敗します。
		/// （負値が返ります。）
		/// </para>
		/// </remarks>
		public Int64 GetSequencePosition()
		{
			return NativeMethods.criAtomExPlayback_GetSequencePosition(NativeHandle);
		}

		/// <summary>再生サンプル数の取得</summary>
		/// <param name="numSamples">再生済みサンプル数</param>
		/// <param name="samplingRate">サンプリングレート</param>
		/// <returns>サンプル数が取得できたかどうか（ true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声の再生サンプル数、
		/// およびサンプリングレートを返します。
		/// 再生サンプル数が取得できた場合、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// （エラー発生時は num_samples や sampling_rate の値も負値になります。）
		/// </para>
		/// <para>
		/// 備考:
		/// 再生済みサンプル数の値の精度は、プラットフォーム SDK
		/// のサウンドライブラリに依存します。
		/// （プラットフォームによって、再生済みサンプル数の正確さは異なります。）
		/// 複数の音声データを含むキューを再生した場合、最初に見つかった音声
		/// データの情報が返されます。
		/// </para>
		/// <para>
		/// 注意:
		/// ドライブでリードリトライ処理等が発生し、音声データの供給が途切れた場合、
		/// 再生サンプル数のカウントアップが停止します。
		/// （データ供給が再開されれば、カウントアップが再開されます。）
		/// 本関数は、音声再生中のみ再生サンプル数を取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// 再生サンプル数の取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public unsafe bool GetNumPlayedSamples(out Int64 numSamples, out Int32 samplingRate)
		{
			fixed (Int64* numSamplesPtr = &numSamples)
			fixed (Int32* samplingRatePtr = &samplingRate)
				return NativeMethods.criAtomExPlayback_GetNumPlayedSamples(NativeHandle, numSamplesPtr, samplingRatePtr);
		}

		/// <summary>サウンドバッファーへの書き込みサンプル数の取得</summary>
		/// <param name="numSamples">書き込み済みサンプル数</param>
		/// <param name="samplingRate">サンプリングレート</param>
		/// <returns>サンプル数が取得できたかどうか（ true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声について、
		/// サウンドバッファーへの書き込み済みサンプル数、およびサンプリングレートを返します。
		/// 本関数は <see cref="CriAtomExPlayback.GetNumPlayedSamples"/> 関数と異なり、
		/// サウンドバッファーに書き込まれた未出力の音声データのサンプル数を含む値を返します。
		/// 書き込み済みサンプル数が取得できた場合、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// （エラー発生時は num_samples や sampling_rate の値も負値になります。）
		/// </para>
		/// <para>
		/// 備考:
		/// 書き込み済みサンプル数の値の精度は、プラットフォーム SDK
		/// のサウンドライブラリに依存します。
		/// （プラットフォームによって、書き込み済みサンプル数の正確さは異なります。）
		/// 複数の音声データを含むキューを再生した場合、最初に見つかった音声
		/// データの情報が返されます。
		/// </para>
		/// <para>
		/// 注意:
		/// ドライブでリードリトライ処理等が発生し、音声データの供給が途切れた場合、
		/// 書き込み済みサンプル数のカウントアップが停止します。
		/// （データ供給が再開されれば、カウントアップが再開されます。）
		/// 本関数は、音声再生中のみ書き込み済みサンプル数を取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// 書き込み済みサンプル数の取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.GetNumPlayedSamples"/>
		public unsafe bool GetNumRenderedSamples(out Int64 numSamples, out Int32 samplingRate)
		{
			fixed (Int64* numSamplesPtr = &numSamples)
			fixed (Int32* samplingRatePtr = &samplingRate)
				return NativeMethods.criAtomExPlayback_GetNumRenderedSamples(NativeHandle, numSamplesPtr, samplingRatePtr);
		}

		/// <summary>パラメータの取得（浮動小数点数）</summary>
		/// <param name="parameterId">パラメーターID</param>
		/// <param name="valueFloat32">パラメーター設定値</param>
		/// <returns>true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声に設定されている各種パラメーターの値を取得します。
		/// 値は浮動小数点数で取得されます。
		/// パラメーターが取得できた場合、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみパラメーターを取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// パラメーターの取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ParameterId"/>
		/// <seealso cref="CriAtomExPlayback.GetParameterUint32"/>
		/// <seealso cref="CriAtomExPlayback.GetParameterSint32"/>
		public unsafe bool GetParameterFloat32(CriAtomEx.ParameterId parameterId, out Single valueFloat32)
		{
			fixed (Single* valueFloat32Ptr = &valueFloat32)
				return NativeMethods.criAtomExPlayback_GetParameterFloat32(NativeHandle, parameterId, valueFloat32Ptr);
		}

		/// <summary>パラメーターの取得（符号なし整数）</summary>
		/// <param name="parameterId">パラメーターID</param>
		/// <param name="valueUint32">パラメーター設定値</param>
		/// <returns>true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声に設定されている各種パラメーターの値を取得します。
		/// 値は符号なし整数で取得されます。
		/// パラメーターが取得できた場合、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみパラメーターを取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// パラメーターの取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ParameterId"/>
		/// <seealso cref="CriAtomExPlayback.GetParameterFloat32"/>
		/// <seealso cref="CriAtomExPlayback.GetParameterSint32"/>
		public unsafe bool GetParameterUint32(CriAtomEx.ParameterId parameterId, out UInt32 valueUint32)
		{
			fixed (UInt32* valueUint32Ptr = &valueUint32)
				return NativeMethods.criAtomExPlayback_GetParameterUint32(NativeHandle, parameterId, valueUint32Ptr);
		}

		/// <summary>パラメーターの取得（符号付き整数）</summary>
		/// <param name="parameterId">パラメーターID</param>
		/// <param name="valueSint32">パラメーター設定値</param>
		/// <returns>true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声に設定されている各種パラメーターの値を取得します。
		/// 値は符号付き整数で取得されます。
		/// パラメーターが取得できた場合、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみパラメーターを取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// パラメーターの取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ParameterId"/>
		/// <seealso cref="CriAtomExPlayback.GetParameterFloat32"/>
		/// <seealso cref="CriAtomExPlayback.GetParameterUint32"/>
		public unsafe bool GetParameterSint32(CriAtomEx.ParameterId parameterId, out Int32 valueSint32)
		{
			fixed (Int32* valueSint32Ptr = &valueSint32)
				return NativeMethods.criAtomExPlayback_GetParameterSint32(NativeHandle, parameterId, valueSint32Ptr);
		}

		/// <summary>AISACコントロール値の取得（コントロールID指定）</summary>
		/// <param name="controlId">コントロールID</param>
		/// <param name="controlValue">コントロール値（0.0f～1.0f）、未設定時は-1.0f</param>
		/// <returns>true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声に設定されているAISACコントロール値を、コントロールID指定で取得します。
		/// AISACコントロール値が取得できた場合（未設定時も「-1.0fが取得できた」と扱われます）、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみAISACコントロール値を取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// AISACコントロール値の取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlById"/>
		/// <seealso cref="CriAtomExPlayback.GetAisacControlByName"/>
		public unsafe bool GetAisacControlById(UInt32 controlId, out Single controlValue)
		{
			fixed (Single* controlValuePtr = &controlValue)
				return NativeMethods.criAtomExPlayback_GetAisacControlById(NativeHandle, controlId, controlValuePtr);
		}

		/// <summary>AISACコントロール値の取得（コントロール名指定）</summary>
		/// <param name="controlName">コントロール名</param>
		/// <param name="controlValue">コントロール値（0.0f～1.0f）、未設定時は-1.0f</param>
		/// <returns>true = 取得できた、 false = 取得できなかった）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生された音声に設定されているAISACコントロール値を、コントロール名指定で取得します。
		/// AISACコントロール値が取得できた場合（未設定時も「-1.0fが取得できた」と扱われます）、本関数は true を返します。
		/// 指定したボイスが既に消去されている場合等には、本関数は false を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、音声再生中のみAISACコントロール値を取得可能です。
		/// 再生終了後や、発音数制御によりボイスが消去された場合には、
		/// AISACコントロール値の取得に失敗します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlById"/>
		/// <seealso cref="CriAtomExPlayback.GetAisacControlByName"/>
		public unsafe bool GetAisacControlByName(ArgString controlName, out Single controlValue)
		{
			fixed (Single* controlValuePtr = &controlValue)
				return NativeMethods.criAtomExPlayback_GetAisacControlByName(NativeHandle, controlName.GetPointer(stackalloc byte[controlName.BufferSize]), controlValuePtr);
		}

		/// <summary>再生音のブロック遷移</summary>
		/// <param name="index">ブロックインデックス</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生音単位でブロック遷移を行います。
		/// 本関数を実行すると、指定したIDの音声がブロックシーケンスの場合はデータの
		/// 設定に従った任意の遷移タイミングで指定ブロックに遷移します。
		/// </para>
		/// <para>
		/// 備考:
		/// 再生開始ブロックの指定は <see cref="CriAtomExPlayer.SetFirstBlockIndex"/> 関数を使用して行い、
		/// 再生中のブロックインデックス取得は <see cref="CriAtomExPlayback.GetCurrentBlockIndex"/> 関数を使用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFirstBlockIndex"/>
		/// <seealso cref="CriAtomExPlayback.GetCurrentBlockIndex"/>
		public void SetNextBlockIndex(Int32 index)
		{
			NativeMethods.criAtomExPlayback_SetNextBlockIndex(NativeHandle, index);
		}

		/// <summary>再生音のカレントブロックインデックスの取得</summary>
		/// <returns>カレントブロックインデックス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生されたブロックシーケンスの
		/// カレントブロックインデックスを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 再生IDにより再生しているデータがブロックシーケンスではない場合は、
		/// <see cref="CriAtomEx.InvalidBlockIndex"/> が返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.SetFirstBlockIndex"/>
		/// <seealso cref="CriAtomExPlayback.SetNextBlockIndex"/>
		public Int32 GetCurrentBlockIndex()
		{
			return NativeMethods.criAtomExPlayback_GetCurrentBlockIndex(NativeHandle);
		}

		/// <summary>再生トラック情報の取得</summary>
		/// <param name="info">再生トラック情報</param>
		/// <returns>成功／失敗</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生中のキューのトラック情報を取得します。
		/// 取得できるトラック情報はキュー直下の情報だけです。サブシーケンスやキューリンクの情報は取得できません。
		/// </para>
		/// <para>
		/// 備考:
		/// 以下に該当するデータを再生中の場合、トラック情報の取得に失敗します。
		/// - キュー以外のデータを再生している。（トラック情報が存在しないため）
		/// - 再生中のキューがポリフォニックタイプ、またはセレクター参照のスイッチタイプである。（トラック情報が複数存在する可能性があるため）
		/// - 再生中のキューがトラック遷移タイプである。（遷移により再生トラックが変わるため）
		/// </para>
		/// </remarks>
		public unsafe bool GetPlaybackTrackInfo(out CriAtomExPlayback.TrackInfo info)
		{
			fixed (CriAtomExPlayback.TrackInfo* infoPtr = &info)
				return NativeMethods.criAtomExPlayback_GetPlaybackTrackInfo(NativeHandle, infoPtr);
		}

		/// <summary>ビート同期情報の取得</summary>
		/// <param name="info">ビート同期情報</param>
		/// <returns>成功／失敗</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生中のキューのビート同期情報を取得します。
		/// 現在のBPM、小節のカウント、拍のカウント、拍の進捗割合(0.0～1.0)を取得することができます。
		/// キューにはビート同期情報が設定されている必要があります。
		/// キューリンクやスタートアクションで再生しているキューの情報は取得できません。
		/// </para>
		/// <para>
		/// 備考:
		/// 以下に該当するデータを再生中の場合、ビート同期情報の取得に失敗します。
		/// - キュー以外のデータを再生している。（ビート同期情報が存在しないため）
		/// - ビート同期情報が設定されていないキューを再生している。
		/// - ビート同期情報が設定されているキューを"間接的"に再生している。（キューリンクやスタートアクションで再生している）
		/// </para>
		/// </remarks>
		public unsafe bool GetBeatSyncInfo(out CriAtomExBeatSync.Info info)
		{
			fixed (CriAtomExBeatSync.Info* infoPtr = &info)
				return NativeMethods.criAtomExPlayback_GetBeatSyncInfo(NativeHandle, infoPtr);
		}

		/// <summary>ビート同期オフセットの設定</summary>
		/// <param name="timeMs">オフセット時間</param>
		/// <returns>成功／失敗</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生中のキューのビート同期オフセットを設定します。
		/// キューにはビート同期情報が設定されている必要があります。
		/// キューリンクやスタートアクションで再生しているキューへの設定はできません。
		/// </para>
		/// <para>
		/// 備考:
		/// 以下に該当するデータを再生中の場合、ビート同期オフセットの設定に失敗します。
		/// - キュー以外のデータを再生している。（ビート同期情報が存在しないため）
		/// - ビート同期情報が設定されていないキューを再生している。
		/// - ビート同期情報が設定されているキューを"間接的"に再生している。（キューリンクやスタートアクションで再生している）
		/// </para>
		/// </remarks>
		public bool SetBeatSyncOffset(Int16 timeMs)
		{
			return NativeMethods.criAtomExPlayback_SetBeatSyncOffset(NativeHandle, timeMs);
		}

	}
}