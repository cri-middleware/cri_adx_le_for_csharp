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
	/// <summary>プレーヤーオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// <see cref="CriAtomExPlayer"/> は、音声再生用に作られたプレーヤーを操作するためのオブジェクトです。
	/// <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数で音声再生用のプレーヤーを作成すると、
	/// 関数はプレーヤー操作用に、この"AtomExプレーヤーオブジェクト"を返します。
	/// データのセットや再生の開始、ステータスの取得等、プレーヤーに対して行う操作は、
	/// 全てAtomExプレーヤーオブジェクトを介して実行されます。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
	public partial class CriAtomExPlayer : IDisposable
	{
		/// <summary>デフォルトのパンニング処理を上書き</summary>
		/// <param name="func">パンニング処理関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング処理をユーザー独自の処理に置き換えます。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数でパンニング処理関数を登録すると、Atomライブラリのパンニング処理が無効化され、
		/// パンニングの際にユーザーが指定したコールバック関数を呼び出すよう動作が変更されます。
		/// コールバック内でセンドレベルマトリクスを操作することにより、
		/// ユーザー独自のパンニングアルゴリズムを使用することが可能となります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.PanCbFunc"/>
		public static unsafe void OverrideDefaultPanMethod(delegate* unmanaged[Cdecl]<IntPtr, Int32, CriAtom.ChannelConfig, Int32, CriAtom.SpeakerMapping, CriAtomEx.SphericalCoordinates*, CriAtomEx._3dAttenuationParameter*, Single**, NativeBool> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_OverrideDefaultPanMethod((IntPtr)func, obj);
		}

		/// <summary>パンニングコールバック関数型</summary>
		/// <returns>独自のパンニング処理を行ったかどうか</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング処理用のコールバック関数の型です。
		/// <see cref="CriAtomExPlayer.OverrideDefaultPanMethod"/> 関数を実行することで、
		/// コールバック関数の登録が可能です。
		/// 本コールバック関数は、パンニング処理の際に呼び出されます。
		/// アプリケーション側で独自のパンニング処理を行いたい場合にご利用ください。
		/// </para>
		/// <para>
		/// 補足
		/// コールバック関数呼び出し時点では、センドレベルマトリクスはゼロクリアされています。
		/// そのため、コールバック関数内でセンドレベルマトリクスをゼロクリアする必要はありません。
		/// アプリケーション側でセンドレベルマトリクスを操作した場合には、
		/// 関数の戻り値で true を返す必要があります。
		/// false を返した場合、コールバック内で指定したセンドレベルは無視され、
		/// Atomライブラリのデフォルトのパンニング処理が適用されます。
		/// （特定のチャンネル数の音声のみに独自のパンニング処理を行いたい等、
		/// デフォルトのパンニング処理と独自のパンニング処理を切り替える必要がある場合には、
		/// 戻り値で制御してください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.OverrideDefaultPanMethod"/>
		public unsafe class PanCbFunc : NativeCallbackBase<PanCbFunc.Arg, NativeBool>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>入力音声のチャンネル数</summary>
				public Int32 inputChannels { get; }
				/// <summary>入力音声のチャンネル構成</summary>
				public CriAtom.ChannelConfig channelConfig { get; }
				/// <summary>出力先のチャンネル数</summary>
				public Int32 outputChannels { get; }
				/// <summary>出力先のスピーカーマッピング</summary>
				public CriAtom.SpeakerMapping speakerMapping { get; }
				/// <summary>音源の位置情報</summary>
				public NativeReference<CriAtomEx.SphericalCoordinates> location { get; }
				/// <summary>距離減衰パラメーター</summary>
				public NativeReference<CriAtomEx._3dAttenuationParameter> parameter { get; }
				/// <summary>センドレベルマトリクス</summary>
				public NativeReference<NativeReference<Single>> matrix { get; }

				internal Arg(Int32 inputChannels, CriAtom.ChannelConfig channelConfig, Int32 outputChannels, CriAtom.SpeakerMapping speakerMapping, NativeReference<CriAtomEx.SphericalCoordinates> location, NativeReference<CriAtomEx._3dAttenuationParameter> parameter, NativeReference<NativeReference<Single>> matrix)
				{
					this.inputChannels = inputChannels;
					this.channelConfig = channelConfig;
					this.outputChannels = outputChannels;
					this.speakerMapping = speakerMapping;
					this.location = location;
					this.parameter = parameter;
					this.matrix = matrix;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static NativeBool CallbackFunc(IntPtr @object, Int32 inputChannels, CriAtom.ChannelConfig channelConfig, Int32 outputChannels, CriAtom.SpeakerMapping speakerMapping, CriAtomEx.SphericalCoordinates* location, CriAtomEx._3dAttenuationParameter* parameter, Single** matrix) =>
				InvokeCallbackInternal(@object, new(inputChannels, channelConfig, outputChannels, speakerMapping, location, parameter, (NativeReference<Single>*)matrix));
#if !NET5_0_OR_GREATER
			delegate NativeBool NativeDelegate(IntPtr obj, Int32 inputChannels, CriAtom.ChannelConfig channelConfig, Int32 outputChannels, CriAtom.SpeakerMapping speakerMapping, CriAtomEx.SphericalCoordinates* location, CriAtomEx._3dAttenuationParameter* parameter, Single** matrix);
			static NativeDelegate callbackDelegate = null;
#endif
			internal PanCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, Int32, CriAtom.ChannelConfig, Int32, CriAtom.SpeakerMapping, CriAtomEx.SphericalCoordinates*, CriAtomEx._3dAttenuationParameter*, Single**, NativeBool>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>波形フィルターコールバック関数の登録</summary>
		/// <param name="func">波形フィルターコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デコード結果の PCM データを受け取るコールバック関数を登録します。
		/// 登録されたコールバック関数は、ボイスが音声データをデコードしたタイミングで呼び出されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 複数の音声データを含むキューを再生した場合、
		/// 最初に見つかった波形データについてのみコールバックが実行されます。
		/// （複数の波形データを含むキューについては、
		/// 2つ目以降の波形データの情報を取ることができません。）
		/// コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// 波形フィルターコールバック関数内で長時間処理をブロックすると、音切れ等の問題
		/// が発生しますので、ご注意ください。
		/// HCA-MXコーデックやプラットフォーム固有の音声圧縮コーデックを使用している場合、
		/// フィルターコールバックは利用できません。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.FilterCbFunc"/>
		public unsafe void SetFilterCallback(delegate* unmanaged[Cdecl]<IntPtr, UInt32, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_SetFilterCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetFilterCallbackInternal(IntPtr func, IntPtr obj) => SetFilterCallback((delegate* unmanaged[Cdecl]<IntPtr, UInt32, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)func, obj);
		CriAtomExPlayer.FilterCbFunc _filterCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetFilterCallback" />
		public CriAtomExPlayer.FilterCbFunc FilterCallback => _filterCallback ?? (_filterCallback = new CriAtomExPlayer.FilterCbFunc(SetFilterCallbackInternal));

		/// <summary>波形フィルターコールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// デコード結果の PCM データを受け取るコールバック関数です。
		/// コールバック関数の登録には <see cref="CriAtomExPlayer.SetFilterCallback"/> 関数を使用します。
		/// コールバック関数を登録すると、ボイスが音声データをデコードする度に、
		/// コールバック関数が実行されるようになります。
		/// フィルターコールバック関数には、 PCM データのフォーマットやチャンネル数、
		/// 参照可能なサンプル数、 PCM データを格納した領域のアドレスが返されます。
		/// コールバック内では PCM データの値を直接参照可能になるので、
		/// 再生中の音声の振幅をチェックするといった用途に利用可能です。
		/// また、コールバック関数内で PCM データを加工すると、再生音に反映されるため、
		/// PCM データに対してユーザ独自のエフェクトをかけることも可能です。
		/// （ただし、タイムストレッチ処理のようなデータ量が増減する加工を行うことはできません。）
		/// </para>
		/// <para>
		/// 備考:
		/// PCM データはチャンネル単位で分離されています。
		/// （インターリーブされていません。）
		/// 第 6 引数（ data 配列）には、各チャンネルの PCM データ配列の先頭アドレスが格納されています。
		/// （二次元配列の先頭アドレスではなく、チャンネルごとの PCM データ配列の先頭アドレスを格納した
		/// 一次元のポインタ配列です。）
		/// プラットフォームによって、 PCM データのフォーマットは異なります。
		/// 実行環境のデータフォーマットについては、第 3 引数（ format ）で判別可能です。
		/// PCM データのフォーマットが 16 bit 整数型の場合、 format は <see cref="CriAtom.PcmFormat.Sint16"/> となり、
		/// PCM データのフォーマットが 32 bit 浮動小数点数型の場合、 format は <see cref="CriAtom.PcmFormat.Float32"/> となります。
		/// それぞれのケースで PCM データの値域は異なりますのでご注意ください。
		/// - <see cref="CriAtom.PcmFormat.Sint16"/> 時は -32768 ～ +32767
		/// - <see cref="CriAtom.PcmFormat.Float32"/> 時は -1.0f ～ +1.0f
		/// （デコード時点ではクリッピングが行われていないため、 <see cref="CriAtom.PcmFormat.Float32"/>
		/// 時は上記範囲をわずかに超えた値が出る可能性があります。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFilterCallback"/>
		public unsafe class FilterCbFunc : NativeCallbackBase<FilterCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg : Interfaces.IPcmData
			{
				/// <summary>再生ID</summary>
				public UInt32 id { get; }
				/// <summary>PCMの形式</summary>
				public CriAtom.PcmFormat format { get; }
				/// <summary>チャンネル数</summary>
				public Int32 numChannels { get; }
				/// <summary>サンプル数</summary>
				public Int32 numSamples { get; }
				/// <summary>PCMデータのチャンネル配列</summary>
				public NativeReference<IntPtr> data { get; }

				internal Arg(UInt32 id, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, NativeReference<IntPtr> data)
				{
					this.id = id;
					this.format = format;
					this.numChannels = numChannels;
					this.numSamples = numSamples;
					this.data = data;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, UInt32 id, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, IntPtr* data) =>
				InvokeCallbackInternal(obj, new(id, format, numChannels, numSamples, data));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, UInt32 id, CriAtom.PcmFormat format, Int32 numChannels, Int32 numSamples, IntPtr* data);
			static NativeDelegate callbackDelegate = null;
#endif
			internal FilterCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, UInt32, CriAtom.PcmFormat, Int32, Int32, IntPtr*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>マルチチャンネル音声の広がり設定</summary>
		/// <param name="wideness">マルチチャンネル音声の広がり</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// マルチチャンネル音声の広がりを指定します。
		/// マルチチャンネル（ステレオ、5.1ch等）の音声素材をパンニングするときに、各入力チャンネル間の角度をどれだけ広げるかを指定します。
		/// 引数 wideness の値域は 0.0 ～ 1.0 です。デフォルト値は 1.0 です。
		/// 例えばステレオ音声を広がり0.5で再生すると、正面から見て左チャンネルは-15度(-30度*0.5)、右チャンネルは15度(30度*0.5)に定位するようパンニング計算されます。
		/// モノラル音声には影響しません。
		/// </para>
		/// <para>
		/// 備考：
		/// 本関数でパンニング時の角度タイプを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定された広がりでパンニング計算されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声の広がりを更新することができます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetWideness(Single wideness)
		{
			NativeMethods.criAtomExPlayer_SetWideness(NativeHandle, wideness);
		}

		/// <summary>サウンドオブジェクトの取得</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// このAtomExプレーヤーに関連付けられているサウンドオブジェクトを取得します。
		/// どのサウンドオブジェクトにも関連付けられていない場合はnullを返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExSoundObject"/>
		/// <seealso cref="CriAtomExSoundObject.AddPlayer"/>
		public CriAtomExSoundObject GetSoundObject()
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExPlayer_GetSoundObject(NativeHandle)) == IntPtr.Zero) ? null : new CriAtomExSoundObject(handle);
		}

		/// <summary>プレーヤー作成用コンフィグ構造体にデフォルト値をセット</summary>
		/// <param name="pConfig">AtomExプレーヤー作成用コンフィグ構造体へのポインタ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数に設定するコンフィグ構造体
		/// （ <see cref="CriAtomExPlayer.Config"/> ）に、デフォルトの値をセットします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Config"/>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		public static unsafe void SetDefaultConfig(out CriAtomExPlayer.Config pConfig)
		{
			fixed (CriAtomExPlayer.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtomExPlayer_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>出力ポートオブジェクトの追加</summary>
		/// <param name="outputPort">出力ポートオブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに出力ポートを追加します。
		/// <see cref="CriAtomExPlayer.MaxOutputPorts"/> に定義された数分の出力ポートを指定することが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数または <see cref="CriAtomExPlayer.ClearOutputPorts"/>
		/// 関数にてクリアされます。
		/// また、<see cref="CriAtomExPlayer.RemoveOutputPort"/> 関数で特定のオブジェクトのみを取り外すことも可能です。
		/// キュー再生時に本関数を呼び出すと、以下の設定は全て<b>無視</b>され、キューは全て追加した出力ポートを通して再生されます。
		/// - データ側に設定されているパラメーターパレットのASRラックID設定
		/// - <see cref="CriAtomExPlayer.SetAsrRackId"/> 関数及び <see cref="CriAtomExPlayer.SetAsrRackIdArray"/> 関数で指定したASRラックID
		/// - データ側に設定されているトラックの出力ポート名
		/// 出力ポートは再生開始前に設定する必要があります。
		/// 既に再生が開始された音声に対し、後から出力ポートを変更することはできません。
		/// 複数の出力ポートを指定したプレーヤーを再生した場合、ボイスはその指定された出力ポートの数だけ使用されます。
		/// そのため、事前に指定する出力ポート数分のボイスを確保しておく必要があります。
		/// <see cref="CriAtomExPlayer.SetData"/> 関数等を使用したキュー再生以外の再生時では、本関数にて指定した複数の出力ポートの内、
		/// 1つ目に設定した出力ポートのみが適用されます。
		/// HCA-MX用にエンコードされた音声データには、本関数の設定が適用されません。
		/// HCA-MX用にエンコードされた音声データについて出力先設定する場合、
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は ボイスのサウンドレンダラタイプにASRを使用する場合にのみ効果があります。
		/// （他のボイスを使用する場合、本関数の設定値は無視されます。）
		/// <see cref="CriAtomExHcaMx.SetAsrRackId"/> 関数を使用して、HCA-MXミキサ自体の出力先ASRラックIDを設定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.RemoveOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.ClearOutputPorts"/>
		public void AddOutputPort(CriAtomExOutputPort outputPort)
		{
			NativeMethods.criAtomExPlayer_AddOutputPort(NativeHandle, outputPort?.NativeHandle ?? default);
		}

		/// <summary>出力ポートオブジェクトの取り外し</summary>
		/// <param name="outputPort">出力ポートオブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに追加した出力ポートを取り外します。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーに <see cref="CriAtomExPlayer.AddOutputPort"/> 関数で追加した特定の出力ポートオブジェクトを取り外します。
		/// プレーヤーに設定されている出力ポートを全てを取り外すには <see cref="CriAtomExPlayer.ClearOutputPorts"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 既に再生が開始された音声に対し、後から出力ポートを変更することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.AddOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.ClearOutputPorts"/>
		public void RemoveOutputPort(CriAtomExOutputPort outputPort)
		{
			NativeMethods.criAtomExPlayer_RemoveOutputPort(NativeHandle, outputPort?.NativeHandle ?? default);
		}

		/// <summary>出力ポートオブジェクトのクリア</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに追加した出力ポートを全てクリアします。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーに <see cref="CriAtomExPlayer.AddOutputPort"/> 関数で追加した特定の出力ポートオブジェクトを全てクリアします。
		/// 特定のオブジェクトを取り外すためには <see cref="CriAtomExPlayer.RemoveOutputPort"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 既に再生が開始された音声に対し、後から出力ポートを変更することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.CriAtomExOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.AddOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.RemoveOutputPort"/>
		public void ClearOutputPorts()
		{
			NativeMethods.criAtomExPlayer_ClearOutputPorts(NativeHandle);
		}

		/// <summary>優先出力ポートオブジェクトの追加</summary>
		/// <param name="outputPort">出力ポートオブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーにACF内の出力ポートより優先的に参照される出力ポートを追加します。
		/// <see cref="CriAtomExPlayer.MaxOutputPorts"/> に定義された数分の出力ポートを指定することが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数または
		/// <see cref="CriAtomExPlayer.ClearPreferredOutputPorts"/> にてクリアされます。
		/// また、<see cref="CriAtomExPlayer.RemovePreferredOutputPort"/> 関数や
		/// <see cref="CriAtomExPlayer.RemovePreferredOutputPortByName"/> 関数にて特定のオブジェクトのみを取り外すことも可能です。
		/// 出力ポート名が設定されているトラックを持つキューを再生したとき、通常はACF登録時に自動生成された
		/// 出力ポートから出力されます。
		/// 本関数でプレーヤーに対して優先出力ポートを追加したとき、
		/// 前述したキューを再生した際には追加された同名の優先出力ポートから出力されるようになります。
		/// </para>
		/// <para>
		/// 注意:
		/// データ側に出力ポート名が設定されていないトラックの再生には影響しません。
		/// 出力ポートは再生開始前に設定する必要があります。
		/// 既に再生が開始された音声に対し、後から出力ポートを変更することはできません。
		/// 一つのプレーヤーに対し、同じ名前の優先出力ポートを登録することはできません。
		/// <see cref="CriAtomExPlayer.SetData"/> 関数等を使用したキュー再生以外の再生時では、本関数にて指定した複数の出力ポートの内、
		/// 1つ目に設定した出力ポートのみが適用されます。
		/// <see cref="CriAtomExPlayer.SetAsrRackId"/> 関数実行後に本関数を実行すると、 <see cref="CriAtomExPlayer.SetAsrRackId"/> 関数にて
		/// 設定したASRラックID設定は上書きされます。
		/// HCA-MX用にエンコードされた音声データには、本関数の設定が適用されません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.RemovePreferredOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.RemovePreferredOutputPortByName"/>
		/// <seealso cref="CriAtomExPlayer.ClearPreferredOutputPorts"/>
		public void AddPreferredOutputPort(CriAtomExOutputPort outputPort)
		{
			NativeMethods.criAtomExPlayer_AddPreferredOutputPort(NativeHandle, outputPort?.NativeHandle ?? default);
		}

		/// <summary>優先出力ポートオブジェクトの取り外し</summary>
		/// <param name="outputPort">出力ポートオブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに追加した優先出力ポートを取り外します。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーに <see cref="CriAtomExPlayer.AddPreferredOutputPort"/> 関数で追加した特定の優先出力ポートオブジェクトを取り外します。
		/// プレーヤーに設定されている出力ポートを全て取り外すには <see cref="CriAtomExPlayer.ClearOutputPorts"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 優先出力ポートを取り外しても、既に再生が開始された音声には影響しません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AddOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.RemovePreferredOutputPortByName"/>
		/// <seealso cref="CriAtomExPlayer.ClearPreferredOutputPorts"/>
		public void RemovePreferredOutputPort(CriAtomExOutputPort outputPort)
		{
			NativeMethods.criAtomExPlayer_RemovePreferredOutputPort(NativeHandle, outputPort?.NativeHandle ?? default);
		}

		/// <summary>優先出力ポートオブジェクトの取り外し（名前指定）</summary>
		/// <param name="name">出力ポート名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに追加した優先出力ポートを名前を指定して取り外します。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーに <see cref="CriAtomExPlayer.AddPreferredOutputPort"/> 関数で追加した特定の優先出力ポートオブジェクトを取り外します。
		/// プレーヤーに設定されている出力ポートを全てクリアするためには <see cref="CriAtomExPlayer.ClearOutputPorts"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 優先出力ポートを取り外しても、既に再生が開始された音声には影響しません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AddOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.RemovePreferredOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.ClearPreferredOutputPorts"/>
		public void RemovePreferredOutputPortByName(ArgString name)
		{
			NativeMethods.criAtomExPlayer_RemovePreferredOutputPortByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>優先出力ポートオブジェクトのクリア</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに追加した優先出力ポートを全てクリアします。
		/// </para>
		/// <para>
		/// 備考:
		/// プレーヤーに <see cref="CriAtomExPlayer.AddPreferredOutputPort"/> 関数で追加した優先出力ポートオブジェクトを全てクリアします。
		/// 特定の優先出力ポートを取り外すためには、<see cref="CriAtomExPlayer.RemovePreferredOutputPort"/> 関数または
		/// <see cref="CriAtomExPlayer.RemovePreferredOutputPortByName"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 優先出力ポートを取り外しても、既に再生が開始された音声には影響しません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AddOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.RemovePreferredOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.RemovePreferredOutputPortByName"/>
		public void ClearPreferredOutputPorts()
		{
			NativeMethods.criAtomExPlayer_ClearPreferredOutputPorts(NativeHandle);
		}

		/// <summary>AtomExPlayer用ワーク領域サイズの計算</summary>
		/// <param name="config">プレーヤー作成用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーを作成するために必要な、ワーク領域のサイズを取得します。
		/// アロケーターを登録せずにAtomExプレーヤーを作成する場合、
		/// あらかじめ本関数で計算したワーク領域サイズ分のメモリを
		/// ワーク領域として <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数にセットする必要があります。
		/// プレーヤーの作成に必要なワークメモリのサイズは、プレーヤー作成用コンフィグ
		/// 構造体（ <see cref="CriAtomExPlayer.Config"/> ）の内容によって変化します。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExPlayer.SetDefaultConfig"/> 適用時と同じパラメーター）で
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
		/// <seealso cref="CriAtomExPlayer.Config"/>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomExPlayer.Config config)
		{
			fixed (CriAtomExPlayer.Config* configPtr = &config)
				return NativeMethods.criAtomExPlayer_CalculateWorkSize(configPtr);
		}

		/// <summary>プレーヤー作成用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数の引数に指定します。
		/// 作成されるプレーヤーは、オブジェクト作成時に本構造体で指定された設定に応じて、
		/// 内部リソースを必要なだけ確保します。
		/// プレーヤーが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomExPlayer.SetDefaultConfig"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer.SetDefaultConfig"/>
		public unsafe partial struct Config
		{
			/// <summary>ボイス確保方式</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomExプレーヤーがボイスを確保する際の方式を指定します。
			/// voice_allocation_method に <see cref="CriAtomEx.VoiceAllocationMethod.AllocateVoiceOnce"/> を指定した場合、
			/// AtomExプレーヤーはボイスの確保を発音開始のタイミングでのみ行います。
			/// 再生開始時点でボイスを確保できなかった場合や、
			/// 発音数制御により再生中にボイスが奪い取られた場合、
			/// 発音に関連するリソースが解放されるため、その波形データはその時点で停止します。
			/// （再生が始まらなかった波形データや、再生が途中で停止された波形データが、
			/// 追加の再生リクエストなしに再生されることはありません。）
			/// これに対し、 voice_allocation_method に <see cref="CriAtomEx.VoiceAllocationMethod.RetryVoiceAllocation"/>
			/// を指定した場合、AtomExプレーヤーはボイスの確保を必要な限り何度も繰り返します。
			/// ボイスが確保できない場合やボイスを奪い取られた場合でも、
			/// 発音を管理するリソース（バーチャルボイス）は解放しないので、
			/// 再度ボイスに空きができた時点で、発音処理が再開されます。
			/// </para>
			/// <para>
			/// 備考:
			/// <see cref="CriAtomEx.VoiceAllocationMethod.RetryVoiceAllocation"/> を指定した場合、発音中のボイスの処理に加え、
			/// 発音を行っていないバーチャルボイスについても定期的にボイスの再取得処理等が行われるため、
			/// <see cref="CriAtomEx.VoiceAllocationMethod.AllocateVoiceOnce"/> を指定した場合に比べ、
			/// 処理負荷が高くなる可能性があります。
			/// <see cref="CriAtomEx.VoiceAllocationMethod.RetryVoiceAllocation"/> 指定時、
			/// ボイスの再確保に成功すると、波形データは<b>再生時刻を考慮した位置から</b>シーク再生されます。
			/// </para>
			/// <para>
			/// 注意:
			/// <see cref="CriAtomEx.VoiceAllocationMethod.RetryVoiceAllocation"/> を指定して AtomEx プレーヤーを作成した場合でも、
			/// Atom ライブラリ初期化時に指定する max_virtual_voices
			/// の数を超える再生要求があった場合、発音は再開されなくなります。
			/// （エラーコールバック関数に警告が返され、バーチャルボイスも削除されます。）
			/// <see cref="CriAtomEx.VoiceAllocationMethod.RetryVoiceAllocation"/> を指定する際には、
			/// 初期化時に必要充分なバーチャルボイスを確保してください。
			/// （ max_virtual_voices に大きめの値を指定してください。）
			/// <see cref="CriAtomEx.VoiceAllocationMethod.RetryVoiceAllocation"/> を指定した場合、
			/// 再生されなかった波形データやボイスが奪い取られた波形データが、
			/// いつどこから再生再開されるか、厳密に制御することはできません。
			/// （実行タイミングにより毎回異なった結果になる可能性があります。）
			/// </para>
			/// </remarks>
			public CriAtomEx.VoiceAllocationMethod voiceAllocationMethod;

			/// <summary>最大パス文字列数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomExプレーヤーが保持するパス文字列の数です。
			/// </para>
			/// <para>
			/// 備考:
			/// <see cref="CriAtomExPlayer.SetFile"/> 関数を実行すると、
			/// 指定したパス文字列がAtomExプレーヤー内に保持されます。
			/// AtomExプレーヤーはデフォルト状態ではパス文字列を1つしか保持しません。
			/// （メモリサイズ削減のため。）
			/// プレーヤー作成時に指定する max_path_strings の数を増やせば、
			/// AtomExプレーヤーは指定された数分のパス文字列を保存するようになります。
			/// max_path_strings に2以上の値を指定することで、
			/// 1つのプレーヤーで複数のファイルを同時にパス指定で再生することが可能となります。
			/// ただし、 max_path_strings の値に応じて必要なワーク領域のサイズは増加します。
			/// （max_path_strings×max_pathバイトのメモリが必要となります。）
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomExPlayer.SetDefaultConfig"/>
			/// <seealso cref="CriAtomExPlayer.SetFile"/>
			public Int32 maxPathStrings;

			/// <summary>最大パス長</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomExプレーヤーに指定可能なファイルパスの最大長です。
			/// ファイル名を指定して音声の再生を行う場合、使用するパスの最大長を max_path
			/// として指定する必要があります。
			/// </para>
			/// <para>
			/// 備考:
			/// 本パラメーターは、パッキングされていない音声ファイルを、
			/// ファイル名を指定して再生する際にのみセットする必要があります。
			/// ファイル名指定の再生を行わず、キューIDや波形データIDを指定して再生を行う場合、
			/// max_path を 0 に設定することが可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// <see cref="CriAtomExPlayer.SetDefaultConfig"/> メソッドで <see cref="CriAtomExPlayer.Config"/> 構造体に
			/// デフォルト値を設定した場合、 max_path には 0 がセットされます。
			/// ファイル名を指定して再生を行う場合、 <see cref="CriAtomExPlayer.SetDefaultConfig"/>
			/// メソッドを使用しないか、または <see cref="CriAtomExPlayer.SetDefaultConfig"/> メソッド実行後
			/// に再度パスの最大長をセットする必要があります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomExPlayer.SetDefaultConfig"/>
			/// <seealso cref="CriAtomExPlayer.SetFile"/>
			public Int32 maxPath;

			/// <summary>最大AISAC数</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 一つのキューに紐づけることができるAISACの最大数です。
			/// Atomライブラリは、初期化時と AtomExPlayer 作成時に
			/// max_aisacs で指定された数分のAISACを参照できるリソースを確保します。
			/// max_aisacs に指定する値の上限は55です。
			/// </para>
			/// </remarks>
			public Byte maxAisacs;

			/// <summary>時刻更新の有無</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomExプレーヤーが時刻更新処理を行うかどうかを指定します。
			/// </para>
			/// <para>
			/// 備考:
			/// updates_time に false を指定した場合、
			/// 作成されたAtomExプレーヤーは再生時刻の更新を行いません。
			/// その結果、 <see cref="CriAtomExPlayer.GetTime"/> 関数による再生時刻の取得は行えなくなりますが、
			/// 音声再生時の処理負荷をわずかに下げることが可能となります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomExPlayer.GetTime"/>
			public NativeBool updatesTime;

			/// <summary>音声同期時刻更新の有無</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// AtomExプレーヤーが再生音声に同期した時刻更新処理を行うかどうかを指定します。
			/// </para>
			/// <para>
			/// 備考:
			/// enable_audio_synced_timer に true を指定した場合、
			/// 作成されたAtomExプレーヤーを用いて再生された音声に対して、
			/// 再生済みサンプル数に同期するように補正した再生時刻の更新を行うようになります。
			/// 補正された再生時刻は <see cref="CriAtomExPlayback.GetTimeSyncedWithAudio"/> 関数によって取得できます。
			/// 音声再生時の処理負荷が上がるため、音声に同期した正確な再生時刻を取得したいプレーヤーの
			/// 作成時のみ true を指定するようにしてください。
			/// </para>
			/// <para>
			/// 注意:
			/// 本フラグを有効にした場合、AtomExプレーヤーに対するピッチ指定が行えなくなります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomExPlayback.GetTimeSyncedWithAudio"/>
			public NativeBool enableAudioSyncedTimer;

		}
		/// <summary>AtomExPlayerの作成</summary>
		/// <param name="config">AtomExプレーヤー作成用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>AtomExプレーヤーオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーを作成します。
		/// プレーヤーを作成する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// User Allocator方式を用いる場合、ユーザがワーク領域を用意する必要はありません。
		/// workにnull、work_sizeに0を指定するだけで、必要なメモリを登録済みのメモリ確保関数から確保します。
		/// AtomExプレーヤー作成時に確保されたメモリは、AtomExプレーヤー破棄時（ <see cref="CriAtomExPlayer.Dispose"/>
		/// 関数実行時）に解放されます。
		/// Fixed Memory方式を用いる場合、ワーク領域として別途確保済みのメモリ領域を本関数に
		/// 設定する必要があります。
		/// ワーク領域のサイズは <see cref="CriAtomExPlayer.CalculateWorkSize"/> 関数で取得可能です。
		/// AtomExプレーヤー作成前に <see cref="CriAtomExPlayer.CalculateWorkSize"/> 関数で取得した
		/// サイズ分のメモリを予め確保しておき、本関数に設定してください。
		/// 尚、Fixed Memory方式を用いた場合、ワーク領域はAtomExプレーヤーの破棄
		/// （ <see cref="CriAtomExPlayer.Dispose"/> 関数）を行うまでの間、ライブラリ内で利用され続けます。
		/// AtomExプレーヤーの破棄を行う前に、ワーク領域のメモリを解放しないでください。
		/// </para>
		/// <para>
		/// 例:
		/// 【User Allocator方式によるAtomExプレーヤーの作成】
		/// User Allocator方式を用いる場合、AtomExプレーヤーの作成／破棄の手順は以下のようになります。
		/// -# AtomExプレーヤー作成前に、 <see cref="CriAtomEx.SetUserAllocator"/> 関数を用いてメモリ確保／解放関数を登録する。
		/// -# AtomExプレーヤー作成用コンフィグ構造体にパラメーターをセットする。
		/// -# <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数でAtomExプレーヤーを作成する。
		/// （workにはnull、work_sizeには0を指定する。）
		/// -# オブジェクトが不要になったら <see cref="CriAtomExPlayer.Dispose"/> 関数でAtomExプレーヤーを破棄する。
		/// </para>
		/// <para>
		/// ※ライブラリ初期化時にメモリ確保／解放関数を登録済みの場合、AtomExプレーヤー作成時
		/// に再度関数を登録する必要はありません。
		/// 【Fixed Memory方式によるAtomExプレーヤーの作成】
		/// Fixed Memory方式を用いる場合、AtomExプレーヤーの作成／破棄の手順は以下のようになります。
		/// -# AtomExプレーヤー作成用コンフィグ構造体にパラメーターをセットする。
		/// -# AtomExプレーヤーの作成に必要なワーク領域のサイズを、
		/// <see cref="CriAtomExPlayer.CalculateWorkSize"/> 関数を使って計算する。
		/// -# ワーク領域サイズ分のメモリを確保する。
		/// -# <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数でAtomExプレーヤーを作成する。
		/// （workには確保したメモリのアドレスを、work_sizeにはワーク領域のサイズを指定する。）
		/// -# オブジェクトが不要になったら <see cref="CriAtomExPlayer.Dispose"/> 関数でAtomExプレーヤーを破棄する。
		/// -# ワーク領域のメモリを解放する。
		/// </para>
		/// <para>
		/// <see cref="CriAtomExPlayer.CriAtomExPlayer"/> 関数を実行すると、AtomExプレーヤーが作成され、
		/// プレーヤーを制御するためのオブジェクト（ <see cref="CriAtomExPlayer"/> ）が返されます。
		/// データのセット、再生の開始、ステータスの取得等、AtomExプレーヤーに対して
		/// 行う操作は、全てオブジェクトに対して行います。
		/// プレーヤーの作成に失敗した場合、戻り値として null が返されます。
		/// プレーヤーの作成に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// 作成されたAtomExプレーヤーオブジェクトを使用して音声データを再生する手順は以下のとおりです。
		/// -# <see cref="CriAtomExPlayer.SetData"/> 関数を使用して、AtomExプレーヤーに再生するデータをセットする。
		/// （ファイル再生時は、 <see cref="CriAtomExPlayer.SetFile"/> 関数または <see cref="CriAtomExPlayer.SetContentId"/>
		/// 関数を使用する。）
		/// -# <see cref="CriAtomExPlayer.Start"/> 関数で再生を開始する。
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
		/// 本関数は完了復帰型の関数です。
		/// AtomExプレーヤーの作成にかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// AtomExプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Config"/>
		/// <seealso cref="CriAtomExPlayer.CalculateWorkSize"/>
		/// <seealso cref="CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer.Dispose"/>
		/// <seealso cref="CriAtomExPlayer.SetData"/>
		/// <seealso cref="CriAtomExPlayer.SetFile"/>
		/// <seealso cref="CriAtomExPlayer.SetContentId"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public unsafe CriAtomExPlayer(in CriAtomExPlayer.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExPlayer.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomExPlayer_Create(configPtr, work, workSize);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomExPlayer(IntPtr work = default, Int32 workSize = default)
		{
			CriAtomExPlayer.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomExPlayer_Create(configPtr, work, workSize);
		}

		/// <summary>AtomExプレーヤーの破棄</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーを破棄します。
		/// 本関数を実行した時点で、AtomExプレーヤー作成時に確保されたリソースが全て解放されます。
		/// また、引数に指定したAtomExプレーヤーオブジェクトも無効になります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		/// 音声再生中のAtomExプレーヤーを破棄しようとした場合、本関数内で再生停止を
		/// 待ってからリソースの解放が行われます。
		/// （ファイルから再生している場合は、さらに読み込み完了待ちが行われます。）
		/// そのため、本関数内で処理が長時間（数フレーム）ブロックされる可能性があります。
		/// AtomExプレーヤーの作成／破棄は、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomExPlayer_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomExPlayer() => Dispose();
#pragma warning restore 1591

		/// <summary>音声データのセット（キューID指定）</summary>
		/// <param name="acbHn">ACBオブジェクト</param>
		/// <param name="id">キューID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを、AtomExプレーヤーに関連付けます。
		/// 本関数でキューIDを指定後、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を
		/// 開始すると、指定されたキューが再生されます。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたデータの情報は、他のデータがセットされるまでAtomExプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考:
		/// 第2引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータのキューがプレーヤーにセットされます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// <see cref="CriAtomExPlayer.SetCueId"/> 関数でキューをセットした場合、以下の関数で設定された
		/// パラメーターは無視されます。
		/// - <see cref="CriAtomExPlayer.SetFormat"/>
		/// - <see cref="CriAtomExPlayer.SetNumChannels"/>
		/// - <see cref="CriAtomExPlayer.SetSamplingRate"/>
		/// （音声フォーマットやチャンネル数、サンプリングレート等の情報は、
		/// ACB ファイルの情報を元に自動的にセットされます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public void SetCueId(CriAtomExAcb acbHn, Int32 id)
		{
			NativeMethods.criAtomExPlayer_SetCueId(NativeHandle, acbHn?.NativeHandle ?? default, id);
		}

		/// <summary>再生の開始</summary>
		/// <returns>再生ID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声データの再生処理を開始します。
		/// 本関数を実行する前に、事前に <see cref="CriAtomExPlayer.SetData"/> 関数等を使用し、再生する
		/// 音声データをAtomExプレーヤーにセットしておく必要があります。
		/// </para>
		/// <para>
		/// 本関数実行後、再生の進み具合（発音が開始されたか、再生が完了したか等）がどうなって
		/// いるかは、ステータスを取得することで確認が可能です。
		/// ステータスの取得には、 <see cref="CriAtomExPlayer.GetStatus"/> 関数を使用します。
		/// <see cref="CriAtomExPlayer.GetStatus"/> 関数は以下の5通りのステータスを返します。
		/// -# <see cref="CriAtomExPlayer.Status.Stop"/>
		/// -# <see cref="CriAtomExPlayer.Status.Prep"/>
		/// -# <see cref="CriAtomExPlayer.Status.Playing"/>
		/// -# <see cref="CriAtomExPlayer.Status.Playend"/>
		/// -# <see cref="CriAtomExPlayer.Status.Error"/>
		/// AtomExプレーヤーを作成した時点では、AtomExプレーヤーのステータスは停止状態
		/// （ <see cref="CriAtomExPlayer.Status.Stop"/> ）です。
		/// 再生する音声データをセット後、本関数を実行することで、AtomExプレーヤーのステータスが
		/// 準備状態（ <see cref="CriAtomExPlayer.Status.Prep"/> ）に変更されます。
		/// （<see cref="CriAtomExPlayer.Status.Prep"/> は、データ供給やデコードの開始を待っている状態です。）
		/// 再生の開始に充分なデータが供給された時点で、AtomExプレーヤーはステータスを
		/// 再生状態（ <see cref="CriAtomExPlayer.Status.Playing"/> ）に変更し、音声の出力を開始します。
		/// セットされたデータを全て再生し終えると、AtomExプレーヤーはステータスを再生終了状態
		/// （ <see cref="CriAtomExPlayer.Status.Playend"/> ）に変更します。
		/// 尚、再生中にエラーが発生した場合には、AtomExプレーヤーはステータスをエラー状態
		/// （ <see cref="CriAtomExPlayer.Status.Error"/> ）に変更します。
		/// AtomExプレーヤーのステータスをチェックし、ステータスに応じて処理を切り替えることで、
		/// 音声の再生状態に連動したプログラムを作成することが可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 関数実行時に発音リソースが確保できない場合（全てのボイスが使用中で、なおかつ
		/// 他のボイスを奪い取れない場合等）、本関数は <see cref="CriAtomEx.InvalidPlaybackId"/> を返します。
		/// しかし、戻り値を元にエラーチェックを行わなくても、ほとんどのケースで問題は発生しません。
		/// 再生ID（ <see cref="CriAtomExPlayback"/> ）を使用する API に対し、 <see cref="CriAtomEx.InvalidPlaybackId"/>
		/// をセットしたとしても、Atomライブラリは特に何も処理しません。
		/// そのため、デバッグ目的で発音が行われたかどうかをチェックしたい場合を除き、
		/// 本関数の結果に応じてアプリケーション側で処理を切り分ける必要はありません。
		/// （ <see cref="CriAtomEx.InvalidPlaybackId"/> が返された際に、有効な再生IDが返された場合と
		/// 同様の処理を行っても、エラーコールバック等は発生しません。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetData"/>
		/// <seealso cref="CriAtomExPlayer.SetFile"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		/// <seealso cref="CriAtomExPlayer.Pause"/>
		/// <seealso cref="CriAtomEx.ExecuteMain"/>
		public CriAtomExPlayback Start()
		{
			return new CriAtomExPlayback(NativeMethods.criAtomExPlayer_Start(NativeHandle));
		}

		/// <summary>音声データのセット（キュー名指定）</summary>
		/// <param name="acbHn">ACBオブジェクト</param>
		/// <param name="cueName">
		/// キュー名
		/// キュー名を、AtomExプレーヤーに関連付けます。
		/// 本関数でキュー名を指定後、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を
		/// 開始すると、指定されたキューが再生されます。
		/// </param>
		/// <remarks>
		/// <para>
		/// 尚、一旦セットしたデータの情報は、他のデータがセットされるまでAtomExプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考:
		/// 第2引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータのキューがプレーヤーにセットされます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// <see cref="CriAtomExPlayer.SetCueName"/> 関数でキューをセットした場合、以下の関数で設定された
		/// パラメーターは無視されます。
		/// - <see cref="CriAtomExPlayer.SetFormat"/>
		/// - <see cref="CriAtomExPlayer.SetNumChannels"/>
		/// - <see cref="CriAtomExPlayer.SetSamplingRate"/>
		/// （音声フォーマットやチャンネル数、サンプリングレート等の情報は、
		/// ACB ファイルの情報を元に自動的にセットされます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public void SetCueName(CriAtomExAcb acbHn, ArgString cueName)
		{
			NativeMethods.criAtomExPlayer_SetCueName(NativeHandle, acbHn?.NativeHandle ?? default, cueName.GetPointer(stackalloc byte[cueName.BufferSize]));
		}

		/// <summary>音声データのセット（キューインデックス指定）</summary>
		/// <param name="acbHn">ACBオブジェクト</param>
		/// <param name="index">
		/// キューインデックス
		/// キューインデックスを、AtomExプレーヤーに関連付けます。
		/// 本関数でキューインデックスを指定後、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を
		/// 開始すると、指定されたキューが再生されます。
		/// </param>
		/// <remarks>
		/// <para>
		/// 尚、一旦セットしたデータの情報は、他のデータがセットされるまでAtomExプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考:
		/// 第2引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューインデックスに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューインデックスを持つACBデータが見つかった時点で、
		/// 当該ACBデータのキューがプレーヤーにセットされます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// <see cref="CriAtomExPlayer.SetCueIndex"/> 関数でキューをセットした場合、以下の関数で設定された
		/// パラメーターは無視されます。
		/// - <see cref="CriAtomExPlayer.SetFormat"/>
		/// - <see cref="CriAtomExPlayer.SetNumChannels"/>
		/// - <see cref="CriAtomExPlayer.SetSamplingRate"/>
		/// （音声フォーマットやチャンネル数、サンプリングレート等の情報は、
		/// ACB ファイルの情報を元に自動的にセットされます。）
		/// 本関数を使用することで、キュー名やキューIDを指定せずにプレーヤーに対して
		/// 音声をセットすることが可能です。
		/// （キュー名やキューIDがわからない場合でも、ACBファイル内のコンテンツを一通り再生
		/// 可能なので、デバッグ用途に利用可能です。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public void SetCueIndex(CriAtomExAcb acbHn, Int32 index)
		{
			NativeMethods.criAtomExPlayer_SetCueIndex(NativeHandle, acbHn?.NativeHandle ?? default, index);
		}

		/// <summary>音声データのセット（オンメモリデータの指定）</summary>
		/// <param name="buffer">バッファーアドレス</param>
		/// <param name="size">バッファーサイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ上に配置された音声データを、AtomExプレーヤーに関連付けます。
		/// 本関数でメモリアドレスとサイズを指定後、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を
		/// 開始すると、指定されたデータが再生されます。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたデータの情報は、他のデータがセットされるまでAtomExプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 注意:
		/// プレーヤーが記憶するのはバッファーのアドレスとサイズのみです。
		/// （バッファー内のデータがコピーされるわけではありません。）
		/// そのため、指定したデータの再生が終了するまでの間、
		/// アプリケーション側でバッファーを保持し続ける必要があります。
		/// メモリ再生を行っているAtomExプレーヤーを停止させた場合でも、
		/// ライブラリ内には当該メモリ領域を参照しているボイスが存在する可能性があります。
		/// 本関数でセットしたメモリ領域を解放する際には、事前に <see cref="CriAtomEx.IsDataPlaying"/>
		/// 関数を実行し、当該メモリ領域への参照が行われていないことを確認してください。
		/// <see cref="CriAtomExPlayer.SetData"/> 関数で音声データをセットする場合、以下の関数を使用して
		/// 再生する音声データの情報を別途指定する必要があります。
		/// - <see cref="CriAtomExPlayer.SetFormat"/>
		/// - <see cref="CriAtomExPlayer.SetNumChannels"/>
		/// - <see cref="CriAtomExPlayer.SetSamplingRate"/>
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFormat"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomEx.IsDataPlaying"/>
		public void SetData(IntPtr buffer, Int32 size)
		{
			NativeMethods.criAtomExPlayer_SetData(NativeHandle, buffer, size);
		}

		/// <summary>フォーマットの指定</summary>
		/// <param name="format">フォーマット</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する音声のフォーマットを指定します。
		/// この関数は、<see cref="CriAtomExPlayer.Start"/> 関数でボイスプールからボイスを
		/// 取得する際の、取得対象を絞り込む関数のひとつです。
		/// この関数では、取得対象ボイスを、
		/// 指定したフォーマットのデータを再生可能なボイスに絞り込みます。
		/// 絞り込みを行う関数は、この関数のほかに、<see cref="CriAtomExPlayer.SetSamplingRate"/> 関数と
		/// <see cref="CriAtomExPlayer.SetNumChannels"/> 関数があります。
		/// 関数実行前のデフォルト設定値はADXフォーマットです。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は、ACBファイルを使用せずに音声を再生する場合にのみセットする必要があります。
		/// キューを再生する場合、フォーマットはキューシートから自動で取得されるため、
		/// 別途本関数を実行する必要はありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetSamplingRate"/>
		/// <seealso cref="CriAtomExPlayer.SetNumChannels"/>
		public void SetFormat(UInt32 format)
		{
			NativeMethods.criAtomExPlayer_SetFormat(NativeHandle, format);
		}

		/// <summary>チャンネル数の指定</summary>
		/// <param name="numChannels">チャンネル数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する音声のチャンネル数を指定します。
		/// この関数は、<see cref="CriAtomExPlayer.Start"/> 関数でボイスプールからボイスを
		/// 取得する際の、取得対象を絞り込む関数のひとつです。
		/// この関数では、取得対象ボイスを、
		/// 指定したチャンネル数のデータを再生可能なボイスに絞り込みます。
		/// 絞り込みを行う関数は、この関数のほかに、<see cref="CriAtomExPlayer.SetFormat"/> 関数と
		/// <see cref="CriAtomExPlayer.SetSamplingRate"/> 関数があります。
		/// 関数実行前のデフォルト設定値は2チャンネルです。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は、ACBファイルを使用せずに音声を再生する場合にのみセットする必要があります。
		/// キューを再生する場合、フォーマットはキューシートから自動で取得されるため、
		/// 別途本関数を実行する必要はありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFormat"/>
		/// <seealso cref="CriAtomExPlayer.SetSamplingRate"/>
		public void SetNumChannels(Int32 numChannels)
		{
			NativeMethods.criAtomExPlayer_SetNumChannels(NativeHandle, numChannels);
		}

		/// <summary>サンプリングレートの指定</summary>
		/// <param name="samplingRate">サンプリングレート</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する音声のサンプリングレートを指定します。
		/// この関数は、<see cref="CriAtomExPlayer.Start"/> 関数でボイスプールからボイスを
		/// 取得する際の、取得対象を絞り込む関数のひとつです。
		/// この関数では、取得対象ボイスを、
		/// 指定したサンプリングレートのデータを再生可能なボイスに絞り込みます。
		/// 絞り込みを行う関数は、この関数のほかに、<see cref="CriAtomExPlayer.SetFormat"/> 関数と
		/// <see cref="CriAtomExPlayer.SetNumChannels"/> 関数があります。
		/// 関数実行前のデフォルト設定値は <see cref="CriAtom.DefaultOutputSamplingRate"/> です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は、ACBファイルを使用せずに音声を再生する場合にのみセットする必要があります。
		/// キューを再生する場合、フォーマットはキューシートから自動で取得されるため、
		/// 別途本関数を実行する必要はありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFormat"/>
		/// <seealso cref="CriAtomExPlayer.SetNumChannels"/>
		public void SetSamplingRate(Int32 samplingRate)
		{
			NativeMethods.criAtomExPlayer_SetSamplingRate(NativeHandle, samplingRate);
		}

		/// <summary>音声データのセット（ファイル名の指定）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="path">ファイルパス</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声ファイルをAtomExプレーヤーに関連付けます。
		/// 本関数でファイルを指定後、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を開始すると、
		/// 指定されたファイルがストリーミング再生されます。
		/// 尚、本関数を実行した時点では、ファイルの読み込みは開始されません。
		/// ファイルの読み込みが開始されるのは、 <see cref="CriAtomExPlayer.Start"/> 関数実行後です。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたファイルの情報は、他のデータがセットされるまでAtomExプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExPlayer.SetFile"/> 関数を実行すると、
		/// 指定したパス文字列がAtomExプレーヤー内に保持されます。
		/// AtomExプレーヤーはデフォルト状態ではパス文字列を保持する領域を1つしか確保しません。
		/// （メモリサイズ削減のため。）
		/// しかし、ファイル再生中に別のファイルをAtomExプレーヤーにセットしたい場合、
		/// 再生中のファイルとセットしたファイルの両方のパスを保持する必要があるため、
		/// 2つのパス文字列を保存する領域が必要になります。
		/// 2つ以上のファイルを同時に再生したい場合には、プレーヤー作成時に指定する
		/// max_path_strings の数を増やす必要があります。
		/// max_path_strings の数を増やすことで、AtomExプレーヤーは指定された数分の
		/// パス文字列を同時に保存できるようになります。
		/// （max_path_strings に2以上の値を指定することで、
		/// 1つのプレーヤーで複数のファイルを同時にパス指定で再生することが可能となります。）
		/// ただし、 max_path_strings の値に応じて必要なワーク領域のサイズは増加します。
		/// <see cref="CriAtomExPlayer.SetFile"/> 関数で音声データをセットする場合、以下の関数を使用して
		/// 再生する音声データの情報を別途指定する必要があります。
		/// - <see cref="CriAtomExPlayer.SetFormat"/>
		/// - <see cref="CriAtomExPlayer.SetNumChannels"/>
		/// - <see cref="CriAtomExPlayer.SetSamplingRate"/>
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Config"/>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public void SetFile(CriFsBinder binder, ArgString path)
		{
			NativeMethods.criAtomExPlayer_SetFile(NativeHandle, binder?.NativeHandle ?? default, path.GetPointer(stackalloc byte[path.BufferSize]));
		}

		/// <summary>音声データのセット（CPKコンテンツIDの指定）</summary>
		/// <param name="binder">バインダーオブジェクト</param>
		/// <param name="id">コンテンツID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コンテンツをAtomExプレーヤーに関連付けます。
		/// CRI File Systemライブラリを使用してCPKファイル内のコンテンツファイルを
		/// ID指定で再生するために使用します。
		/// 本関数にバインダーとコンテンツIDを指定後、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を
		/// 開始すると、指定されたコンテンツファイルがストリーミング再生されます。
		/// 尚、本関数を実行した時点では、ファイルの読み込みは開始されません。
		/// ファイルの読み込みが開始されるのは、 <see cref="CriAtomExPlayer.Start"/> 関数実行後です。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたファイルの情報は、他のデータがセットされるまでAtomExプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomExPlayer.SetContentId"/> 関数で音声データをセットする場合、以下の関数を使用して
		/// 再生する音声データの情報を別途指定する必要があります。
		/// - <see cref="CriAtomExPlayer.SetFormat"/>
		/// - <see cref="CriAtomExPlayer.SetNumChannels"/>
		/// - <see cref="CriAtomExPlayer.SetSamplingRate"/>
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Config"/>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public void SetContentId(CriFsBinder binder, Int32 id)
		{
			NativeMethods.criAtomExPlayer_SetContentId(NativeHandle, binder?.NativeHandle ?? default, id);
		}

		/// <summary>音声データのセット（波形データIDの指定）</summary>
		/// <param name="awb">AWBオブジェクト</param>
		/// <param name="id">波形データID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生する波形データをAtomExプレーヤーに関連付けます。
		/// 本関数にAWBオブジェクトと波形データIDを指定後、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を
		/// 開始すると、指定した波形データがストリーミング再生されます。
		/// 尚、本関数を実行した時点では、ファイルの読み込みは開始されません。
		/// ファイルの読み込みが開始されるのは、 <see cref="CriAtomExPlayer.Start"/> 関数実行後です。
		/// </para>
		/// <para>
		/// 尚、一旦セットしたファイルの情報は、他のデータがセットされるまでAtomExプレーヤー内に保持
		/// されます。
		/// そのため、同じデータを何度も再生する場合には、再生毎にデータをセットしなおす必要
		/// はありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数で音声データをセットする場合、
		/// 以下の関数を使用して再生する音声データの情報を別途指定する必要があります。
		/// - <see cref="CriAtomExPlayer.SetFormat"/>
		/// - <see cref="CriAtomExPlayer.SetNumChannels"/>
		/// - <see cref="CriAtomExPlayer.SetSamplingRate"/>
		/// 本関数でセットした音声を再生中に、 <see cref="CriAtomAwb.Dispose"/> 関数でデータを破棄しないでください。
		/// AWBファイルを破棄する際には、必ず再生を停止した状態で <see cref="CriAtomAwb.Dispose"/> 関数を実行してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Config"/>
		/// <seealso cref="CriAtomExPlayer.CriAtomExPlayer"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public void SetWaveId(CriAtomAwb awb, Int32 id)
		{
			NativeMethods.criAtomExPlayer_SetWaveId(NativeHandle, awb?.NativeHandle ?? default, id);
		}

		/// <summary>ステータスの取得</summary>
		/// <returns>ステータス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーのステータスを取得します。
		/// ステータスはAtomExプレーヤーの再生状態を示す値で、以下の5通りの値が存在します。
		/// -# <see cref="CriAtomExPlayer.Status.Stop"/>
		/// -# <see cref="CriAtomExPlayer.Status.Prep"/>
		/// -# <see cref="CriAtomExPlayer.Status.Playing"/>
		/// -# <see cref="CriAtomExPlayer.Status.Playend"/>
		/// -# <see cref="CriAtomExPlayer.Status.Error"/>
		/// AtomExプレーヤーを作成した時点では、AtomExプレーヤーのステータスは停止状態
		/// （ <see cref="CriAtomExPlayer.Status.Stop"/> ）です。
		/// 再生する音声データをセット後、<see cref="CriAtomExPlayer.Start"/> 関数を実行することで、
		/// AtomExプレーヤーのステータスが準備状態（ <see cref="CriAtomExPlayer.Status.Prep"/> ）に変更されます。
		/// （<see cref="CriAtomExPlayer.Status.Prep"/> は、データ供給やデコードの開始を待っている状態です。）
		/// 再生の開始に充分なデータが供給された時点で、AtomExプレーヤーはステータスを
		/// 再生状態（ <see cref="CriAtomExPlayer.Status.Playing"/> ）に変更し、音声の出力を開始します。
		/// セットされたデータを全て再生し終えると、AtomExプレーヤーはステータスを再生終了状態
		/// （ <see cref="CriAtomExPlayer.Status.Playend"/> ）に変更します。
		/// 尚、再生中にエラーが発生した場合には、AtomExプレーヤーはステータスをエラー状態
		/// （ <see cref="CriAtomExPlayer.Status.Error"/> ）に変更します。
		/// AtomExプレーヤーのステータスをチェックし、ステータスに応じて処理を切り替えることで、
		/// 音声の再生状態に連動したプログラムを作成することが可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public CriAtomExPlayer.Status GetStatus()
		{
			return NativeMethods.criAtomExPlayer_GetStatus(NativeHandle);
		}

		/// <summary>ポーズ／ポーズ解除</summary>
		/// <param name="sw">スイッチ（false = ポーズ解除、true = ポーズ）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生のポーズ／ポーズ解除を行います。
		/// sw に true を指定して本関数を実行すると、AtomExプレーヤーは再生中の
		/// 音声をポーズ（一時停止）します。
		/// sw に false を指定して本関数を実行すると、AtomExプレーヤーはポーズを
		/// 解除し、一時停止していた音声の再生を再開します。
		/// </para>
		/// <para>
		/// 備考:
		/// デフォルト状態（プレーヤー作成直後の状態）では、ポーズは解除されています。
		/// </para>
		/// <para>
		/// 注意:
		/// 第2引数（sw）に false を指定してポーズ解除の操作を行った場合、
		/// 本関数でポーズをかけた音声だけでなく、<see cref="CriAtomExPlayer.Prepare"/>
		/// 関数で再生準備中の音声についても再生が開始されてしまいます。
		/// （旧バージョンとの互換性維持のための仕様です。）
		/// 本関数でポーズをかけた音声についてのみポーズを解除したい場合、
		/// 本関数を使用せず、 <see cref="CriAtomExPlayer.Resume"/>(player, <see cref="CriAtomEx.ResumeMode.PausedPlayback"/>);
		/// を実行してポーズ解除を行ってください。
		/// 本関数を実行すると、プレーヤーで再生している"全ての"音声に対してポーズ／ポーズ解除
		/// の処理が行われます。
		/// 再生中の個々の音声に対し、個別にポーズ／ポーズ解除の処理を行う場合には、
		/// <see cref="CriAtomExPlayback.Pause"/> 関数をご利用ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.IsPaused"/>
		/// <seealso cref="CriAtomExPlayback.Pause"/>
		/// <seealso cref="CriAtomExPlayer.Resume"/>
		public void Pause(NativeBool sw)
		{
			NativeMethods.criAtomExPlayer_Pause(NativeHandle, sw);
		}

		/// <summary>再生の準備</summary>
		/// <returns>再生ID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声データの再生を準備します。
		/// 本関数を実行する前に、事前に <see cref="CriAtomExPlayer.SetData"/> 関数等を使用し、
		/// 再生すべき音声データをAtomExプレーヤーにセットしておく必要があります。
		/// 本関数を実行すると、ポーズをかけた状態で音声の再生を開始します。
		/// 関数実行のタイミングで音声再生に必要なリソースを確保し、
		/// バッファリング（ストリーム再生を行うファイルの読み込み）を開始しますが、
		/// バッファリング完了後も発音は行われません。
		/// （発音可能な状態になっても、ポーズ状態で待機します。）
		/// </para>
		/// <para>
		/// 本関数で再生準備を行った音声を発音するには、
		/// 本関数が返す再生 ID （ <see cref="CriAtomExPlayback"/> ）に対し、
		/// <see cref="CriAtomExPlayback.Pause"/> (id, false); の操作を行う必要があります。
		/// </para>
		/// <para>
		/// 備考:
		/// ストリーミング再生時には、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を開始しても、
		/// 実際に音声の再生が開始されるまでにはタイムラグがあります。
		/// （音声データのバッファリングに時間がかかるため。）
		/// 以下の操作を行うことで、ストリーム再生の音声についても、発音のタイミングを
		/// 制御することが可能になります。
		/// -# <see cref="CriAtomExPlayer.Prepare"/> 関数で準備を開始する。
		/// -# 手順1.で取得した再生IDのステータスを <see cref="CriAtomExPlayback.GetStatus"/> 関数で確認。
		/// -# ステータスが <see cref="CriAtomExPlayback.Status.Playing"/> になった時点で <see cref="CriAtomExPlayback.Pause"/> 関数でポーズを解除。
		/// -# ポーズ解除後、次にサーバー処理が動作するタイミングで発音が開始される。
		/// </para>
		/// <para>
		/// ポーズ解除処理に <see cref="CriAtomExPlayback.Pause"/> 関数を使用した場合、
		/// 本関数による再生準備のためのポーズと、 <see cref="CriAtomExPlayer.Pause"/>
		/// 関数による一時停止処理の両方が解除されます。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズした音声を停止したまま
		/// 本関数で再生準備を行った音声を再生したい場合、ポーズの解除に
		/// <see cref="CriAtomExPlayer.Resume"/> 関数（または <see cref="CriAtomExPlayback.Resume"/>
		/// 関数）をご利用ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.GetStatus"/>
		/// <seealso cref="CriAtomExPlayback.Pause"/>
		public CriAtomExPlayback Prepare()
		{
			return new CriAtomExPlayback(NativeMethods.criAtomExPlayer_Prepare(NativeHandle));
		}

		/// <summary>再生の停止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生の停止要求を発行します。
		/// 音声再生中のAtomExプレーヤーに対して本関数を実行すると、
		/// AtomExプレーヤーは再生を停止（ファイルの読み込みや、発音を停止）し、
		/// ステータスを停止状態（ <see cref="CriAtomExPlayer.Status.Stop"/> ）に遷移します。
		/// </para>
		/// <para>
		/// 備考:
		/// 既に停止しているAtomExプレーヤー（ステータスが <see cref="CriAtomExPlayer.Status.Playend"/> や
		/// <see cref="CriAtomExPlayer.Status.Error"/> のAtomExプレーヤー） に対して本関数を実行すると、
		/// AtomExプレーヤーのステータスを <see cref="CriAtomExPlayer.Status.Stop"/> に変更します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数ではありません。
		/// そのため、関数内で処理が長時間ブロックすることはありませんが、
		/// 関数を抜けた時点では再生が停止していない可能性がある点にご注意ください。
		/// （停止状態になるまでに、時間がかかる場合があります。）
		/// 停止を保証する必要がある場合には、本関数呼び出し後、
		/// AtomExプレーヤーのステータスが停止状態（<see cref="CriAtomExPlayer.Status.Stop"/>）
		/// になることを確認してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		public void Stop()
		{
			NativeMethods.criAtomExPlayer_Stop(NativeHandle);
		}

		/// <summary>再生の停止（リリースタイム無視）</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生の停止要求を発行します。
		/// この際、再生中の音声にエンベロープのリリースタイムが設定されていたとしても、
		/// それを無視して停止します。
		/// 音声再生中のAtomExプレーヤーに対して本関数を実行すると、
		/// AtomExプレーヤーは再生を停止（ファイルの読み込みや、発音を停止）し、
		/// ステータスを停止状態（ <see cref="CriAtomExPlayer.Status.Stop"/> ）に遷移します。
		/// </para>
		/// <para>
		/// 備考:
		/// 既に停止しているAtomExプレーヤー（ステータスが <see cref="CriAtomExPlayer.Status.Playend"/> や
		/// <see cref="CriAtomExPlayer.Status.Error"/> のAtomExプレーヤー） に対して本関数を実行すると、
		/// AtomExプレーヤーのステータスを <see cref="CriAtomExPlayer.Status.Stop"/> に変更します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数ではありません。
		/// そのため、関数内で処理が長時間ブロックすることはありませんが、
		/// 関数を抜けた時点では再生が停止していない可能性がある点にご注意ください。
		/// （停止状態になるまでに、時間がかかる場合があります。）
		/// 停止を保証する必要がある場合には、本関数呼び出し後、
		/// AtomExプレーヤーのステータスが停止状態（<see cref="CriAtomExPlayer.Status.Stop"/>）
		/// になることを確認してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		public void StopWithoutReleaseTime()
		{
			NativeMethods.criAtomExPlayer_StopWithoutReleaseTime(NativeHandle);
		}

		/// <summary>全てのプレーヤーの再生を停止</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 全てのAtomExプレーヤーに対し、再生の停止要求を発行します。
		/// 本関数を実行すると、AtomExプレーヤーは再生を停止（ファイルの読み込みや、発音を停止）し、
		/// ステータスを停止状態（ <see cref="CriAtomExPlayer.Status.Stop"/> ）に遷移します。
		/// </para>
		/// <para>
		/// 備考:
		/// 既に停止しているAtomExプレーヤー（ステータスが <see cref="CriAtomExPlayer.Status.Playend"/> や
		/// <see cref="CriAtomExPlayer.Status.Error"/> のAtomExプレーヤー） についても、
		/// 本関数が実行されるとステータスが <see cref="CriAtomExPlayer.Status.Stop"/> に変更されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数ではありません。
		/// そのため、関数内で処理が長時間ブロックすることはありませんが、
		/// 関数を抜けた時点では再生が停止していない可能性がある点にご注意ください。
		/// （停止状態になるまでに、時間がかかる場合があります。）
		/// 停止を保証する必要がある場合には、本関数呼び出し後、
		/// AtomExプレーヤーのステータスが停止状態（<see cref="CriAtomExPlayer.Status.Stop"/>）
		/// になることを確認してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		public static void StopAllPlayers()
		{
			NativeMethods.criAtomExPlayer_StopAllPlayers();
		}

		/// <summary>全てのプレーヤーの再生を停止（リリースタイム無視）</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 全てのAtomExプレーヤーに対し、再生の停止要求を発行します。
		/// この際、再生中の音声にエンベロープのリリースタイムが設定されていたとしても、
		/// それを無視して停止します。
		/// 本関数を実行すると、AtomExプレーヤーは再生を停止（ファイルの読み込みや、発音を停止）し、
		/// ステータスを停止状態（ <see cref="CriAtomExPlayer.Status.Stop"/> ）に遷移します。
		/// </para>
		/// <para>
		/// 備考:
		/// 既に停止しているAtomExプレーヤー（ステータスが <see cref="CriAtomExPlayer.Status.Playend"/> や
		/// <see cref="CriAtomExPlayer.Status.Error"/> のAtomExプレーヤー） についても、
		/// 本関数が実行されるとステータスが <see cref="CriAtomExPlayer.Status.Stop"/> に変更されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数ではありません。
		/// そのため、関数内で処理が長時間ブロックすることはありませんが、
		/// 関数を抜けた時点では再生が停止していない可能性がある点にご注意ください。
		/// （停止状態になるまでに、時間がかかる場合があります。）
		/// 停止を保証する必要がある場合には、本関数呼び出し後、
		/// AtomExプレーヤーのステータスが停止状態（<see cref="CriAtomExPlayer.Status.Stop"/>）
		/// になることを確認してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		public static void StopAllPlayersWithoutReleaseTime()
		{
			NativeMethods.criAtomExPlayer_StopAllPlayersWithoutReleaseTime();
		}

		/// <summary>プレーヤーの列挙</summary>
		/// <param name="func">プレーヤーコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// アプリケーション中で確保したプレーヤーを列挙します。
		/// 本関数を実行すると、第 1 引数（ func ）
		/// でセットされたコールバック関数がAtomExプレーヤーの数分だけ呼び出されます。
		/// （AtomExプレーヤーオブジェクトが、引数としてコールバック関数に渡されます。）
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomExPlayer.CbFunc"/> の説明をご参照ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.CbFunc"/>
		public static unsafe void EnumeratePlayers(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_EnumeratePlayers((IntPtr)func, obj);
		}

		/// <summary>プレーヤーコールバック関数型</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーの列挙に使用する、コールバック関数の型です。
		/// <see cref="CriAtomExPlayer.EnumeratePlayers"/> 関数に本関数型のコールバック関数を登録することで、
		/// アプリケーション中で作成したプレーヤーをコールバックで受け取ることが可能となります。
		/// </para>
		/// <para>
		/// 注意:
		/// 引数で渡されたAtomExプレーヤーを破棄してはいけません。
		/// （アクセス違反やハングアップ等の重篤な不具合が発生する恐れがあります。）
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.EnumeratePlayers"/>
		public unsafe class CbFunc : NativeCallbackBase<CbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>AtomExプレーヤー</summary>
				public IntPtr player { get; }

				internal Arg(IntPtr player)
				{
					this.player = player;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr player) =>
				InvokeCallbackInternal(obj, new(player));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr player);
			static NativeDelegate callbackDelegate = null;
#endif
			internal CbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ポーズ解除</summary>
		/// <param name="mode">ポーズ解除対象</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 一時停止状態の解除を行います。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数と異なり、 <see cref="CriAtomExPlayer.Prepare"/>
		/// 関数で再生開始待ちの音声と、 <see cref="CriAtomExPlayer.Pause"/> 関数（または
		/// <see cref="CriAtomExPlayback.Pause"/> 関数でポーズをかけた音声とを、
		/// 個別に再開させることが可能です。
		/// 第2引数（mode）に <see cref="CriAtomEx.ResumeMode.PausedPlayback"/> を指定して本関数を実行すると、
		/// ユーザが <see cref="CriAtomExPlayer.Pause"/> 関数（または <see cref="CriAtomExPlayback.Pause"/>
		/// 関数）で一時停止状態になった音声の再生が再開されます。
		/// 第2引数（mode）に <see cref="CriAtomEx.ResumeMode.PreparedPlayback"/> を指定して本関数を実行すると、
		/// ユーザが <see cref="CriAtomExPlayer.Prepare"/> 関数で再生準備を指示した音声の再生が開始されます。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数でポーズ状態のプレーヤーに対して <see cref="CriAtomExPlayer.Prepare"/>
		/// 関数で再生準備を行った場合、その音声は <see cref="CriAtomEx.ResumeMode.PausedPlayback"/>
		/// 指定のポーズ解除処理と、 <see cref="CriAtomEx.ResumeMode.PreparedPlayback"/>
		/// 指定のポーズ解除処理の両方が行われるまで、再生が開始されません。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExPlayer.Pause"/> 関数か <see cref="CriAtomExPlayer.Prepare"/> 関数かに関係なく、
		/// 常に再生を開始したい場合には、第2引数（mode）に <see cref="CriAtomEx.ResumeMode.AllPlayback"/>
		/// を指定して本関数を実行するか、または <see cref="CriAtomExPlayer.Pause"/>(player, false);
		/// を実行してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、プレーヤーで再生している"全ての"音声に対してポーズ解除
		/// の処理が行われます。
		/// 再生中の個々の音声に対し、個別にポーズ解除の処理を行う場合には、
		/// <see cref="CriAtomExPlayback.Resume"/> 関数をご利用ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.Resume"/>
		/// <seealso cref="CriAtomExPlayer.Pause"/>
		public void Resume(CriAtomEx.ResumeMode mode)
		{
			NativeMethods.criAtomExPlayer_Resume(NativeHandle, mode);
		}

		/// <summary>ポーズ状態の取得</summary>
		/// <returns>ポーズ中かどうか（false = ポーズされていない、true = ポーズ中）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーがポーズ中かどうかを返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数が true を返すのは、「全ての再生音がポーズ中の場合」のみです。
		/// <see cref="CriAtomExPlayer.Pause"/> 関数実行後、再生ID指定で個々の音声のポーズを解除
		/// （ <see cref="CriAtomExPlayback.Pause"/> 関数を実行）した場合、本関数は false を
		/// 返します。
		/// 本関数は <see cref="CriAtomExPlayer.Pause"/> 関数でポーズされた音声と、
		/// <see cref="CriAtomExPlayer.Prepare"/> 関数でポーズされた音声とを区別しません。
		/// （ポーズ方法に関係なく、全ての再生音がポーズされているかどうかのみを判定します。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Pause"/>
		/// <seealso cref="CriAtomExPlayback.Pause"/>
		public bool IsPaused()
		{
			return NativeMethods.criAtomExPlayer_IsPaused(NativeHandle);
		}

		/// <summary>プレーヤーステータス</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーの再生状態を示す値です。
		/// <see cref="CriAtomExPlayer.GetStatus"/> 関数で取得可能です。
		/// 再生状態は、通常以下の順序で遷移します。
		/// -# <see cref="CriAtomExPlayer.Status.Stop"/>
		/// -# <see cref="CriAtomExPlayer.Status.Prep"/>
		/// -# <see cref="CriAtomExPlayer.Status.Playing"/>
		/// -# <see cref="CriAtomExPlayer.Status.Playend"/>
		/// AtomExプレーヤー作成直後の状態は、停止状態（ <see cref="CriAtomExPlayer.Status.Stop"/> ）です。
		/// <see cref="CriAtomExPlayer.SetData"/> 関数等でデータをセットし、 <see cref="CriAtomExPlayer.Start"/> 関数を
		/// 実行すると、再生準備状態（ <see cref="CriAtomExPlayer.Status.Prep"/> ）に遷移し、再生準備を始めます。
		/// データが充分供給され、再生準備が整うと、ステータスは再生中（ <see cref="CriAtomExPlayer.Status.Playing"/> ）
		/// に変わり、音声の出力が開始されます。
		/// セットされたデータを全て再生し終えた時点で、ステータスは再生完了
		/// （ <see cref="CriAtomExPlayer.Status.Playend"/> ）に変わります。
		/// </para>
		/// <para>
		/// 備考
		/// AtomExプレーヤーは、Atomプレーヤーと異なり、1つのプレーヤーで複数音の再生が可能です。
		/// そのため、再生中のAtomExプレーヤーに対して <see cref="CriAtomExPlayer.Start"/> 関数を実行すると、
		/// 2つの音が重なって再生されます。
		/// 再生中に <see cref="CriAtomExPlayer.Stop"/> 関数を実行した場合、AtomExプレーヤーで再生中の全ての音声
		/// が停止し、ステータスは <see cref="CriAtomExPlayer.Status.Stop"/> に戻ります。
		/// （ <see cref="CriAtomExPlayer.Stop"/> 関数の呼び出しタイミングによっては、 <see cref="CriAtomExPlayer.Status.Stop"/>
		/// に遷移するまでに時間がかかる場合があります。）
		/// 1つのAtomExプレーヤーで複数回 <see cref="CriAtomExPlayer.Start"/> 関数を実行した場合、
		/// 1つでも再生準備中の音があれば、ステータスは <see cref="CriAtomExPlayer.Status.Prep"/> 状態になります。
		/// （全ての音声が再生中の状態になるまで、ステータスは <see cref="CriAtomExPlayer.Status.Playing"/> 状態に
		/// 遷移しません。）
		/// また、 <see cref="CriAtomExPlayer.Status.Playing"/> 状態のプレーヤーに対し、再度 <see cref="CriAtomExPlayer.Start"/>
		/// 関数を実行した場合、ステータスは一時的に <see cref="CriAtomExPlayer.Status.Prep"/> に戻ります。
		/// 再生中に不正なデータを読み込んだ場合や、ファイルアクセスに失敗した場合、
		/// ステータスは <see cref="CriAtomExPlayer.Status.Error"/> に遷移します。
		/// 複数の音声を再生中にある音声でエラーが発生した場合、プレーヤーのステータスは
		/// 他の音声の状態に関係なく、 <see cref="CriAtomExPlayer.Status.Error"/> に遷移します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.GetStatus"/>
		/// <seealso cref="CriAtomExPlayer.SetData"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Stop"/>
		public enum Status
		{
			/// <summary>停止中</summary>
			Stop = 0,
			/// <summary>再生準備中</summary>
			Prep = 1,
			/// <summary>再生中</summary>
			Playing = 2,
			/// <summary>再生完了</summary>
			Playend = 3,
			/// <summary>エラーが発生</summary>
			Error = 4,
		}
		/// <summary>再生中の音声の列挙</summary>
		/// <param name="func">プレイバックコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーで再生中のプレイバックを列挙します。
		/// 本関数を実行すると、第 2 引数（ func ）
		/// でセットされたコールバック関数が再生中のプレイバックの数分だけ呼び出されます。
		/// （プレイバックIDが、引数としてコールバック関数に渡されます。）
		/// </para>
		/// <para>
		/// 備考:
		/// 第 3 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomExPlayback.CbFunc"/> の説明をご参照ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.CbFunc"/>
		public unsafe void EnumeratePlaybacks(delegate* unmanaged[Cdecl]<IntPtr, UInt32, NativeBool> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_EnumeratePlaybacks(NativeHandle, (IntPtr)func, obj);
		}

		/// <summary>再生中の音声数の取得</summary>
		/// <returns>再生音数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーで現在再生中の音声の数を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は、 <see cref="CriAtomExPlayer.Start"/> 関数で再生を行い、今現在も有効な再生IDの数を返します。
		/// （ 使用中のボイス数の数ではありません。複数の波形データを含むシーケンスを1回再生した場合でも、
		/// 1つとカウントされます。）
		/// 使用中のボイス数を取得したい場合には、 <see cref="CriAtomExVoicePool.GetNumUsedVoices"/> 関数をご利用ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExVoicePool.GetNumUsedVoices"/>
		public Int32 GetNumPlaybacks()
		{
			return NativeMethods.criAtomExPlayer_GetNumPlaybacks(NativeHandle);
		}

		/// <summary>最終再生IDの取得</summary>
		/// <returns>再生ID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーで最後に再生した音声の再生IDを取得します。
		/// </para>
		/// <para>備考:</para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public CriAtomExPlayback GetLastPlaybackId()
		{
			return new CriAtomExPlayback(NativeMethods.criAtomExPlayer_GetLastPlaybackId(NativeHandle));
		}

		/// <summary>再生時刻の取得</summary>
		/// <returns>再生時刻（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで最後に再生した音声の、再生時刻を取得します。
		/// 再生時刻が取得できた場合、本関数は 0 以上の値を返します。
		/// 再生時刻が取得できない場合（ボイスの取得に失敗した場合等）、本関数は負値を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 同一プレーヤーで複数の音声を再生し、本関数を実行した場合、本関数は
		/// "最後に"再生した音声の時刻を返します。
		/// 複数の音声に対して再生時刻をチェックする必要がある場合には、
		/// 再生する音声の数分だけプレーヤーを作成するか、または
		/// <see cref="CriAtomExPlayback.GetTime"/> 関数をご利用ください。
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
		/// AtomExプレーヤー作成時、 <see cref="CriAtomExPlayer.Config"/> 構造体の updates_time を
		/// false に設定した場合、当該プレーヤーから再生時刻を取得することはできなくなります。
		/// 再生中の音声が発音数制御によって消去された場合、
		/// 再生時刻のカウントアップもその時点で停止します。
		/// また、再生開始時点で発音数制御によりボイスが割り当てられなかった場合、
		/// 本関数は正しい時刻を返しません。
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
		/// <seealso cref="CriAtomExPlayback.GetTime"/>
		/// <seealso cref="CriAtomExPlayback.GetNumPlayedSamples"/>
		public Int64 GetTime()
		{
			return NativeMethods.criAtomExPlayer_GetTime(NativeHandle);
		}

		/// <summary>サウンドレンダラタイプの指定</summary>
		/// <param name="type">サウンドレンダラタイプ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する音声の出力先サウンドレンダラを指定します。
		/// <see cref="CriAtomExPlayer.Start"/> 関数で音声を再生した際、AtomExプレーヤーは本関数で
		/// 指定されたサウンドレンダラから出力するボイスを、ボイスプールから取得します。
		/// 関数実行前のデフォルト設定値は <see cref="CriAtom.SoundRendererType.Any"/> です。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SoundRendererType.Any"/> を指定した場合、プレーヤーはボイスの出力先に関係なく、
		/// 最初に見つかったボイスプールを使用して発音を行います。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SoundRendererType"/>
		public void SetSoundRendererType(CriAtom.SoundRendererType type)
		{
			NativeMethods.criAtomExPlayer_SetSoundRendererType(NativeHandle, type);
		}

		/// <summary>グループ番号の指定</summary>
		/// <param name="groupNo">グループ番号</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 発音時にボイスをどのボイスリミットグループから取得するかを指定します。
		/// group_no に <see cref="CriAtomExPlayer.NoGroupLimitation"/> を指定した場合、
		/// プレーヤーはボイスリミットグループによる制限を受けなくなります。
		/// （空きボイスがあるか、または自身より低プライオリティのボイスがあれば、
		/// ボイスリミットグループに関係なくボイスを取得します。）
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生を開始した際、
		/// 指定したボイスリミットグループのボイスが全て使用中だった場合、
		/// 再生した音声が発音されるかどうかは、ボイスプライオリティ制御によって決まります。
		/// （ボイスプライオリティの詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// キュー再生時に本関数を呼び出すと、データ側に設定されているボイスリミットグループ設定を<b>上書き</b>します（データ側の設定値は無視されます）。
		/// ただし、group_no に <see cref="CriAtomExPlayer.NoGroupLimitation"/> を指定した場合はデータ側に設定されているボイスリミットグループを参照します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.NoGroupLimitation"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.SetVoicePriority"/>
		/// <seealso cref="CriAtomExPlayer.SetVoiceControlMethod"/>
		public void SetGroupNumber(Int32 groupNo)
		{
			NativeMethods.criAtomExPlayer_SetGroupNumber(NativeHandle, groupNo);
		}

		/// <summary>ボイス制御方法の指定</summary>
		/// <param name="method">ボイス制御方法</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーにボイス制御方法を設定します。
		/// 本関数でボイス制御方法をセット後、 <see cref="CriAtomExPlayer.Start"/> 関数で音声を再生すると、
		/// 当該プレーヤーで再生する波形データには、本関数で指定した制御方式が適用されます。
		/// ボイス制御方法（ method ）には、以下のいずれかが指定可能です。
		/// - <see cref="CriAtomEx.VoiceControlMethod.PreferLast"/>
		/// - <see cref="CriAtomEx.VoiceControlMethod.PreferFirst"/>
		/// - <see cref="CriAtomEx.VoiceControlMethod.PreferData"/>
		/// 空きボイスがない状態で再生中のボイスと同プライオリティの音声を再生した場合、
		/// ボイス制御方式に <see cref="CriAtomEx.VoiceControlMethod.PreferLast"/> が指定されていれば、
		/// 再生中のボイスを停止して新規に音声の再生を開始します。
		/// 同条件で <see cref="CriAtomEx.VoiceControlMethod.PreferFirst"/> が指定されている場合、
		/// 新規の再生リクエストがキャンセルされ、既存のボイスが再生を続けます。
		/// <see cref="CriAtomEx.VoiceControlMethod.PreferData"/> が指定されている場合、
		/// データにあらかじめ設定されているボイス制御方式（オーサリングツール上で設定した値）
		/// が使用されます。
		/// <see cref="CriAtomEx.VoiceControlMethod.PreferData"/> を指定しているにもかかわらず、単体ファイル再生等、
		/// データにボイス制御方式が設定されていない場合、
		/// 後着優先（ <see cref="CriAtomEx.VoiceControlMethod.PreferLast"/> ）でボイスが制御されます。
		/// 関数実行前のデフォルト設定値はデータ依存（ <see cref="CriAtomEx.VoiceControlMethod.PreferData"/> ）です。
		/// </para>
		/// <para>
		/// 備考:
		/// AtomExプレーヤーが波形データを再生しようとした際、
		/// 当該波形データが所属するボイスリミットグループの発音数が上限に達していた場合や、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// 本関数でセットしたボイス制御方式は、発音制御の際、
		/// 再生しようとした波形データのプライオリティと、
		/// 再生中の波形データのプライオリティが同プライオリティであった場合に考慮されます。
		/// （ボイスプライオリティによる発音制御の詳細は <see cref="CriAtomExPlayer.SetVoicePriority"/>
		/// 関数の説明をご参照ください。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.SetVoicePriority"/>
		public void SetVoiceControlMethod(CriAtomEx.VoiceControlMethod method)
		{
			NativeMethods.criAtomExPlayer_SetVoiceControlMethod(NativeHandle, method);
		}

		/// <summary>ボイスプール識別子の指定</summary>
		/// <param name="identifier">ボイスプール識別子</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 発音時にボイスをどのボイスプールから取得するかを指定します。
		/// 本関数を実行すると、プレーヤーは以降指定されたボイスプール識別子に一致する
		/// ボイスプールからのみボイスを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ボイスプール識別子のデフォルト値は 0 です。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.StandardVoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateStandardVoicePool"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetVoicePoolIdentifier(UInt32 identifier)
		{
			NativeMethods.criAtomExPlayer_SetVoicePoolIdentifier(NativeHandle, identifier);
		}

		/// <summary>HCAデコード先ミキサIDの指定</summary>
		/// <param name="mixerId">ミキサID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXのデコード先ミキサIDを指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は HCA-MX ボイスを使用する場合にのみ効果があります。
		/// （他のボイスを使用する場合、本関数の設定値は無視されます。）
		/// ミキサIDは再生開始前に設定する必要があります。
		/// 既に再生が開始された音声に対し、後からミキサIDを変更することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExHcaMx.VoicePoolConfig"/>
		/// <seealso cref="CriAtomExVoicePool.AllocateHcaMxVoicePool"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetHcaMxMixerId(Int32 mixerId)
		{
			NativeMethods.criAtomExPlayer_SetHcaMxMixerId(NativeHandle, mixerId);
		}

		/// <summary>ASRラックIDの指定</summary>
		/// <param name="rackId">ASRラックID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスの出力先ASRラックIDを指定します。
		/// 複数のASRラックIDを指定したい場合、 <see cref="CriAtomExPlayer.SetAsrRackIdArray"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// キュー再生時に本関数を呼び出すと、データ側に設定されているパラメーターパレットのASRラックID設定を<b>上書き</b>します（データ側の設定値は無視されます）。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は ボイスのサウンドレンダラタイプにASRを使用する場合にのみ効果があります。
		/// （他のボイスを使用する場合、本関数の設定値は無視されます。）
		/// ASRラックIDは再生開始前に設定する必要があります。
		/// 既に再生が開始された音声に対し、後からASRラックIDを変更することはできません。
		/// <see cref="CriAtomExPlayer.SetAsrRackIdArray"/> 関数実行後に本関数を実行すると、 <see cref="CriAtomExPlayer.SetAsrRackIdArray"/> 関数にて
		/// 設定した複数のASRラックID設定は上書きされます。
		/// HCA-MX用にエンコードされた音声データには、本関数の設定が適用されません。
		/// HCA-MX用にエンコードされた音声データについて出力先ASRラックIDを設定する場合、
		/// <see cref="CriAtomExHcaMx.SetAsrRackId"/> 関数を使用して、HCA-MXミキサ自体の出力先ASRラックIDを設定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAsrRackIdArray"/>
		/// <seealso cref="CriAtomExHcaMx.SetAsrRackId"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetAsrRackId(Int32 rackId)
		{
			NativeMethods.criAtomExPlayer_SetAsrRackId(NativeHandle, rackId);
		}

		/// <summary>複数のASRラックIDの指定</summary>
		/// <param name="rackIdArray">ASRラックIDの配列</param>
		/// <param name="numRacks">ASRラックID指定数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスの出力先ASRラックIDを複数指定します。
		/// <see cref="CriAtomExPlayer.MaxAsrRacks"/> に定義された数分のASRラックIDを指定することが可能です。
		/// 単一のASRラックIDを指定する場合は、 <see cref="CriAtomExPlayer.SetAsrRackId"/> 関数を使用することでも指定可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// キュー再生時に本関数を呼び出すと、データ側に設定されているパラメーターパレットのASRラックID設定を<b>上書き</b>します（データ側の設定値は無視されます）。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は ボイスのサウンドレンダラタイプにASRを使用する場合にのみ効果があります。
		/// （他のボイスを使用する場合、本関数の設定値は無視されます。）
		/// ASRラックIDは再生開始前に設定する必要があります。
		/// 既に再生が開始された音声に対し、後からASRラックIDを変更することはできません。
		/// 複数のASRラックIDを指定したプレーヤーを再生した場合、ボイスはその指定されたASRラックIDの数だけ使用されます。
		/// そのため、事前に指定するASRラックID数分のボイスを確保しておく必要があります。
		/// <see cref="CriAtomExPlayer.SetData"/> 関数等を使用したキュー再生以外の再生時では、本関数にて指定した複数のASRラックIDの内、
		/// 1つ目（配列のインデックスが0）の要素に格納されているASRラックIDのみが適用されます。
		/// <see cref="CriAtomExPlayer.SetAsrRackId"/> 関数実行後に本関数を実行すると、 <see cref="CriAtomExPlayer.SetAsrRackId"/> 関数にて
		/// 設定したASRラックID設定は上書きされます。
		/// HCA-MX用にエンコードされた音声データには、本関数の設定が適用されません。
		/// HCA-MX用にエンコードされた音声データについて出力先ASRラックIDを設定する場合、
		/// <see cref="CriAtomExHcaMx.SetAsrRackId"/> 関数を使用して、HCA-MXミキサ自体の出力先ASRラックIDを設定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAsrRackId"/>
		/// <seealso cref="CriAtomExHcaMx.SetAsrRackId"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public unsafe void SetAsrRackIdArray(in Int32 rackIdArray, Int32 numRacks)
		{
			fixed (Int32* rackIdArrayPtr = &rackIdArray)
				NativeMethods.criAtomExPlayer_SetAsrRackIdArray(NativeHandle, rackIdArrayPtr, numRacks);
		}

		/// <summary>再生開始位置の指定</summary>
		/// <param name="startTimeMs">再生開始位置（ミリ秒指定）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する音声について、再生を開始する位置を指定します。
		/// 音声データを途中から再生したい場合、再生開始前に本関数で再生開始位置を
		/// 指定する必要があります。
		/// 再生開始位置の指定はミリ秒単位で行います。
		/// 例えば、 start_time_ms に 10000 をセットして本関数を実行すると、
		/// 次に再生する音声データは 10 秒目の位置から再生されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 音声データ途中からの再生は、音声データ先頭からの再生に比べ、発音開始の
		/// タイミングが遅くなります。
		/// これは、一旦音声データのヘッダーを解析後、指定位置にジャンプしてからデータを読み
		/// 直して再生を開始するためです。
		/// </para>
		/// <para>
		/// 注意:
		/// start_time_ms には64bit値をセット可能ですが、現状、32bit以上の再生時刻を
		/// 指定することはできません。
		/// 機種固有の音声フォーマットについても、再生開始位置を指定できない場合があります。
		/// 再生開始位置を指定してシーケンスを再生した場合、指定位置よりも前に配置された
		/// 波形データは再生されません。
		/// （シーケンス内の個々の波形が途中から再生されることはありません。）
		/// </para>
		/// </remarks>
		public void SetStartTime(Int64 startTimeMs)
		{
			NativeMethods.criAtomExPlayer_SetStartTime(NativeHandle, startTimeMs);
		}

		/// <summary>同期再生IDの設定</summary>
		/// <param name="playbackId">同期対象となる再生ID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生する音声を、指定した再生IDの音声に同期させます。
		/// 本関数で再生IDを設定後に音声を再生すると、
		/// その音声は指定された再生IDと同じ再生位置にシークして再生を始めます。
		/// </para>
		/// <para>
		/// 備考:
		/// 同期対象の再生IDが無効な場合、音声データの先頭から再生が開始されます。
		/// playback_id に <see cref="CriAtomEx.InvalidPlaybackId"/> を指定すると、
		/// 再生IDの登録がクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、単体の波形データを再生する場合にのみ利用可能です。
		/// シーケンスデータには利用できません。
		/// （シーケンスデータに対して使用した場合、
		/// シーケンス中で最初に見つかった波形データに対して同期処理が行われてしまいます。）
		/// 本関数で再生位置を調整可能なコーデックは、以下のコーデックのみです。
		/// - ADX
		/// - HCA
		/// - Wave
		/// 他のコーデックについては、本関数を用いた同期再生は行えません。
		/// （HCA-MXや、ハードウェアデコードを行う音声コーデックでは、本機能は利用できません。）
		/// 本機能による再生位置の同期は、可能な限りサンプル単位で行いますが、
		/// 1サンプルの誤差なく結合することを保証するものではありません。
		/// （ミリ秒レベルのズレが許容される場合にのみご利用ください。）
		/// また、再生位置の同期精度は、プラットフォームによっても異なります。
		/// 本関数と <see cref="CriAtomExPlayer.SetStartTime"/> 関数を併用することはできません。
		/// 本関数を使用した場合、音声の再生開始位置はライブラリ内で自動的に調整されます。
		/// そのため、本関数と <see cref="CriAtomExPlayer.SetStartTime"/> 関数を併用することはできません。
		/// （ <see cref="CriAtomExPlayer.SetStartTime"/> 関数の設定は無視されます。）
		/// 本関数を使用して再生を行った場合、再生開始時にノイズが入る場合があります。
		/// 本機能を使用する場合、可能な限りフェードイン処理を併用してください。
		/// 本関数を使用してキュー再生を行った場合、 <see cref="CriAtomExPlayer.GetTime"/> 関数や
		/// <see cref="CriAtomExPlayback.GetTime"/> 関数による再生時刻の取得は正しく行えません。
		/// 再生時刻の確認には、これらの関数の代わりに、 <see cref="CriAtomExPlayback.GetNumPlayedSamples"/>
		/// 関数をご利用ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayback.GetNumPlayedSamples"/>
		public void SetSyncPlaybackId(CriAtomExPlayback playbackId)
		{
			NativeMethods.criAtomExPlayer_SetSyncPlaybackId(NativeHandle, playbackId.NativeHandle);
		}

		/// <summary>シーケンス再生レシオの設定</summary>
		/// <param name="playbackRatio">シーケンス再生レシオ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーで再生するシーケンスの再生レシオを設定します。
		/// 再生レシオの設定範囲は 0.0f ～ 2.0f です。
		/// 範囲外の値を設定した場合は、下限値もしくは上限値が設定されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数による設定値は、シーケンスタイプのキューを再生する場合にのみ適用されます。
		/// シーケンスにて発音する波形データの再生レシオには利用できません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPlaybackRatio(Single playbackRatio)
		{
			NativeMethods.criAtomExPlayer_SetPlaybackRatio(NativeHandle, playbackRatio);
		}

		/// <summary>ループ回数の制限</summary>
		/// <param name="count">ループ制限回数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 波形データのループ再生回数を制限します。
		/// 例えば、countに1を指定した場合、ループ波形データは1回のみループして再生を終了します。
		/// （ループエンドポイントに到達後、1回だけループスタート位置に戻ります。）
		/// </para>
		/// <para>
		/// 備考:
		/// デフォルト状態では、ループポイント付きの音声データは無限にループ再生されます。
		/// ループ回数を一旦制限した後、ループ回数を再度無限回に戻したい場合には、
		/// count に <see cref="CriAtomExPlayer.NoLoopLimitation"/> を指定してください。
		/// count に <see cref="CriAtomExPlayer.IgnoreLoop"/> を指定することで、
		/// ループポイント付きの音声データをループさせずに再生することも可能です。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// ループ制限回数の指定は、音声再生開始前に行う必要があります。
		/// 再生中に本関数を実行しても、ループ回数は変更されません。
		/// 再生中の任意のタイミングでループ再生を停止したい場合、
		/// ループ再生ではなく、シームレス連結再生で制御を行ってください。
		/// 本関数で指定したループ制限回数は、
		/// あらかじめループポイントが設定された波形データを再生する場合にのみ適用されます。
		/// 波形データ自体にループポイントが設定されていない場合、
		/// 本関数を実行しても何の効果もありません。
		/// 本関数を使用してループ回数を指定した場合でも、
		/// ループ終了時にループエンドポイント以降の波形データが再生されることはありません。
		/// （指定回数分ループした後、ループエンドポイントで再生が停止します。）
		/// 例外的に、以下の条件を満たす場合に限り、ワンショットでループポイント以降の
		/// データを含めて再生することが可能です。（ただしループはされません）
		/// - criatomencd.exe で -nodelterm を指定してデータをエンコードする。
		/// - 本関数に <see cref="CriAtomExPlayer.IgnoreLoop"/> を指定してから再生を行う。
		/// 本関数でループ回数を制限できるのは、ADXコーデックとHCAコーデックのみです。
		/// プラットフォーム依存の音声コーデックに対して本関数を実行しないでください。
		/// （再生が終了しない、ノイズが発生する等の問題が発生します。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void LimitLoopCount(Int32 count)
		{
			NativeMethods.criAtomExPlayer_LimitLoopCount(NativeHandle, count);
		}

		/// <summary>ボリュームの設定</summary>
		/// <param name="volume">ボリューム値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力音声のボリュームを指定します。
		/// 本関数でボリュームを設定後、<see cref="CriAtomExPlayer.Start"/> 関数で再生を開始すると、
		/// 設定されたボリュームで音声が再生されます。
		/// またボリューム設定後に <see cref="CriAtomExPlayer.Update"/> 関数や <see cref="CriAtomExPlayer.UpdateAll"/>
		/// 関数を呼び出すことで、すでに再生された音声のボリュームを更新することも可能です。
		/// ボリューム値は音声データの振幅に対する倍率です（単位はデシベルではありません）。
		/// 例えば、1.0fを指定した場合、原音はそのままのボリュームで出力されます。
		/// 0.5fを指定した場合、原音波形の振幅を半分にしたデータと同じ音量（-6dB）で
		/// 音声が出力されます。
		/// 0.0fを指定した場合、音声はミュートされます（無音になります）。
		/// ボリュームのデフォルト値は1.0fです。
		/// </para>
		/// <para>
		/// 備考:
		/// ボリューム値には0.0f以上の値が設定可能です。
		/// （Atomライブラリ Ver.1.21.07より、
		/// ボリューム値に1.0fを超える値を指定できるようになりました。）
		/// 1.0fを超える値をセットした場合、<b>プラットフォームによっては</b>、
		/// 波形データを元素材よりも大きな音量で再生可能です。
		/// ボリューム値に0.0f未満の値を指定した場合、値は0.0fにクリップされます。
		/// （ボリューム値に負の値を設定した場合でも、
		/// 波形データの位相が反転されることはありません。）
		/// キュー再生時、データ側にボリュームが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数による設定値とを<b>乗算</b>した値が適用されます。
		/// 例えば、データ側のボリュームが0.8f、AtomExプレーヤーのボリュームが0.5fの場合、
		/// 実際に適用されるボリュームは0.4fになります。
		/// デシベルで設定したい場合、以下の計算式で変換してから設定してください。
		/// </para>
		/// <para>
		/// ※db_volがデシベル値、volumeがボリューム値です。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 1.0fを超えるボリュームを指定する場合、以下の点に注意する必要があります。
		/// - プラットフォームごとに挙動が異なる可能性がある。
		/// - 音割れが発生する可能性がある。
		/// 本関数に1.0fを超えるボリューム値を設定した場合でも、
		/// 音声が元の波形データよりも大きな音量で再生されるかどうかは、
		/// プラットフォームや音声圧縮コーデックの種別によって異なります。
		/// そのため、マルチプラットフォームタイトルでボリュームを調整する場合には、
		/// 1.0fを超えるボリューム値を使用しないことをおすすめします。
		/// （1.0fを超えるボリューム値を指定した場合、同じ波形データを再生した場合でも、
		/// 機種ごとに異なる音量で出力される可能性があります。）
		/// また、音量を上げることが可能な機種であっても、
		/// ハードウェアで出力可能な音量には上限があるため、
		/// 音割れによるノイズが発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetVolume(Single volume)
		{
			NativeMethods.criAtomExPlayer_SetVolume(NativeHandle, volume);
		}

		/// <summary>再生パラメーターの更新（再生中の音全て）</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーに設定されている再生パラメーター（AISACコントロール値を含む）を使用して、
		/// このAtomExプレーヤーで再生中の音全ての再生パラメーターを更新します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		public void UpdateAll()
		{
			NativeMethods.criAtomExPlayer_UpdateAll(NativeHandle);
		}

		/// <summary>再生パラメーターの更新（再生ID指定）</summary>
		/// <param name="id">再生ID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーに設定されている再生パラメーター（AISACコントロール値を含む）を使用して、
		/// 再生IDによって指定された音声の再生パラメーターを更新します。
		/// </para>
		/// <para>
		/// 備考:
		/// 再生IDは、このAtomExプレーヤーで再生された音声を指している必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		public void Update(CriAtomExPlayback id)
		{
			NativeMethods.criAtomExPlayer_Update(NativeHandle, id.NativeHandle);
		}

		/// <summary>再生パラメーターの初期化</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーに設定されている再生パラメーター（AISACコントロール値を含む）をリセットし、初期状態（未設定状態）に戻します。
		/// 本関数呼び出し後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、初期状態の再生パラメーターで再生されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数呼び出し後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出したとしても、すでに再生されている音声のパラメーターは初期値には戻りません。
		/// すでに再生されている音声のパラメーターを変える場合は、明示的に<see cref="CriAtomExPlayer.SetVolume"/> 関数等を呼び出してください。
		/// 本関数でリセットされるパラメーターは、各パラメーターの設定を行う関数に対象かどうかを記載しているため、そちらを参照して下さい。
		/// なお、本関数では3D音源オブジェクトや3Dリスナーオブジェクト自体のもつパラメーター（位置等）はリセットされません。「AtomExプレーヤーに設定されているオブジェクトが何か」という設定だけがリセットされます。
		/// これらのオブジェクト自体のパラメーターをリセットしたい場合には、それぞれのオブジェクトのパラメーターリセット関数を呼び出してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource.ResetParameters"/>
		/// <seealso cref="CriAtomEx3dListener.ResetParameters"/>
		public void ResetParameters()
		{
			NativeMethods.criAtomExPlayer_ResetParameters(NativeHandle);
		}

		/// <summary>パラメーターの取得（浮動小数点数）</summary>
		/// <param name="id">パラメーターID</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーに設定されている各種パラメーターの値を取得します。
		/// 値は浮動小数点数で取得されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ParameterId"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterUint32"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterSint32"/>
		public Single GetParameterFloat32(CriAtomEx.ParameterId id)
		{
			return NativeMethods.criAtomExPlayer_GetParameterFloat32(NativeHandle, id);
		}

		/// <summary>パラメーターの取得（符号なし整数）</summary>
		/// <param name="id">パラメーターID</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーに設定されている各種パラメーターの値を取得します。
		/// 値は符号なし整数で取得されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ParameterId"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterFloat32"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterSint32"/>
		public UInt32 GetParameterUint32(CriAtomEx.ParameterId id)
		{
			return NativeMethods.criAtomExPlayer_GetParameterUint32(NativeHandle, id);
		}

		/// <summary>パラメーターの取得（符号付き整数）</summary>
		/// <param name="id">パラメーターID</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーに設定されている各種パラメーターの値を取得します。
		/// 値は符号付き整数で取得されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ParameterId"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterFloat32"/>
		/// <seealso cref="CriAtomExPlayer.GetParameterUint32"/>
		public Int32 GetParameterSint32(CriAtomEx.ParameterId id)
		{
			return NativeMethods.criAtomExPlayer_GetParameterSint32(NativeHandle, id);
		}

		/// <summary>ピッチの設定</summary>
		/// <param name="pitch">ピッチ（セント単位）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力音声のピッチを指定します。
		/// 本関数でピッチを設定後、<see cref="CriAtomExPlayer.Start"/> 関数で再生を開始すると、
		/// 設定されたピッチで音声が再生されます。
		/// またピッチ後に <see cref="CriAtomExPlayer.Update"/> 関数や <see cref="CriAtomExPlayer.UpdateAll"/>
		/// 関数を呼び出すことにより、すでに再生された音声のピッチを更新することが可能です。
		/// ピッチはセント単位で指定します。
		/// 1セントは1オクターブの1/1200です。半音は100セントです。
		/// 例えば、100.0fを指定した場合、ピッチが半音上がります。-100.0fを指定した場合、
		/// ピッチが半音下がります。
		/// ピッチのデフォルト値は0.0fです。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にピッチが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数による設定値とを<b>加算</b>した値が適用されます。
		/// 例えば、データ側のピッチが-100.0f、AtomExプレーヤーのピッチが200.0fの場合、
		/// 実際に適用されるピッチは100.0fになります。
		/// サンプリングレートの周波数比率で設定したい場合、以下の計算式で変換してから設定してください。
		/// </para>
		/// <para>
		/// ※freq_ratioが周波数比率、pitchがピッチの値です。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// HCA-MX用にエンコードされた音声データは、ピッチの変更ができません。
		/// （本関数を実行しても、ピッチは変わりません。）
		/// ピッチを変更したい音声については、ADXやHCA等、他のコーデックでエンコードを行ってください。
		/// 設定可能な最大ピッチは、音声データのサンプリングレートとボイスプールの最大サンプリングレートに依存します。
		/// 例えば、音声データのサンプリングレートが24kHzで、ボイスプールの最大サンプリングレートが48kHzの場合、
		/// 設定可能な最大ピッチは1200(周波数比率2倍)になります。
		/// 再生サンプリングレートの上下によりピッチを実装しているため、
		/// ピッチを変更すると音程と一緒に再生速度も変化します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetMaxPitch"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPitch(Single pitch)
		{
			NativeMethods.criAtomExPlayer_SetPitch(NativeHandle, pitch);
		}

		/// <summary>最大ピッチの設定</summary>
		/// <param name="pitch">最大ピッチ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声の最大ピッチを設定します。
		/// 本関数で最大ピッチを指定することで、指定範囲内でのピッチ変更が即座に反映されるようになります。
		/// </para>
		/// <para>
		/// 備考:
		/// Atom Ver.2.10.00以前のライブラリでは、ピッチを上げた際に音が途切れる
		/// （再生速度が速くなった結果、音声データの供給が足りなくなる）ケースがありました。
		/// この対策として、Atom Ver.2.10.00ではピッチを上げても音が途切れないよう、
		/// 音声を充分にバッファリングしてからピッチを上げるよう動作を変更しています。
		/// 修正により、ピッチ操作によって音が途切れることはなくなりましたが、
		/// ピッチを上げる際にバッファリングを待つ時間分だけピッチ変更が遅れる形になるため、
		/// 音の変化が以前のバージョンと比べて緩慢になる可能性があります。
		/// （短時間にピッチを上げ下げするケースにおいて、音の鳴り方が変わる可能性があります。）
		/// 本関数で最大ピッチをあらかじめ設定した場合、
		/// 指定された速度を想定して常にバッファリングが行われるようになるため、
		/// （指定された範囲内の周波数においては）バッファリングなしにピッチ変更が即座に行われます。
		/// 短時間にピッチを上げ下げするケースについては、
		/// 予想される最大ピッチをあらかじめ本関数で設定してから再生を行ってください。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPitch"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetMaxPitch(Single pitch)
		{
			NativeMethods.criAtomExPlayer_SetMaxPitch(NativeHandle, pitch);
		}

		/// <summary>パンニング3D角度の設定</summary>
		/// <param name="pan3dAngle">パンニング3D角度（-180.0f～180.0f：度単位）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング3D角度を指定します。
		/// 本関数でパンニング3D角度を設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたパンニング3D角度で再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のパンニング3D角度を更新することができます。
		/// 角度は度単位で指定します。
		/// 前方を0度とし、右方向（時計回り）に180.0f、左方向（反時計回り）に-180.0fまで設定できます。
		/// 例えば、45.0fを指定した場合、右前方45度に定位します。-45.0fを指定した場合、左前方45度に定位します。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にパンニング3D角度が設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数による設定値とを<b>加算</b>した値が適用されます。
		/// 例えば、データ側のパンニング3D角度が15.0f、AtomExプレーヤーのパンニング3D角度が30.0fの場合、
		/// 実際に適用されるパンニング3D角度は45.0fになります。
		/// 実際に適用されるパンニング3D角度が180.0fを超える値になった場合、値を-360.0fして範囲内に納めます。
		/// 同様に、実際に適用されるボリューム値が-180.0f未満の値になった場合は、値を+360.0fして範囲内に納めます。
		/// （+360.0f, -360.0fしても定位は変わらないため、実質的には-180.0f～180.0fの範囲を超えて設定可能です。）
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPan3dAngle(Single pan3dAngle)
		{
			NativeMethods.criAtomExPlayer_SetPan3dAngle(NativeHandle, pan3dAngle);
		}

		/// <summary>パンニング3D距離の設定</summary>
		/// <param name="pan3dInteriorDistance">パンニング3D距離（-1.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング3Dでインテリアパンニングを行う際の距離を指定します。
		/// 本関数でパンニング3D距離を設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたパンニング3D距離で再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のパンニング3D距離を更新することができます。
		/// 距離は、リスナー位置を0.0f、スピーカーの配置されている円周上を1.0fとして、-1.0f～1.0fの範囲で指定します。
		/// 負値を指定すると、パンニング3D角度が180度反転し、逆方向に定位します。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にパンニング3D距離が設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数による設定値とを<b>乗算</b>した値が適用されます。
		/// 例えば、データ側のパンニング3D距離が0.8f、AtomExプレーヤーのパンニング3D距離が0.5fの場合、
		/// 実際に適用されるパンニング3D距離は0.4fになります。
		/// 実際に適用されるパンニング3D距離が1.0fを超える値になった場合、値は1.0fにクリップされます。
		/// 同様に、実際に適用されるパンニング3D距離が-1.0f未満の値になった場合も、値は-1.0fにクリップされます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPan3dInteriorDistance(Single pan3dInteriorDistance)
		{
			NativeMethods.criAtomExPlayer_SetPan3dInteriorDistance(NativeHandle, pan3dInteriorDistance);
		}

		/// <summary>パンニング3Dボリュームの設定</summary>
		/// <param name="pan3dVolume">パンニング3Dボリューム（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング3Dのボリュームを指定します。
		/// 本関数でパンニング3Dボリュームを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたパンニング3Dボリュームで再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のパンニング3Dボリュームを更新することができます。
		/// パンニング3Dボリュームは、パンニング3D成分と、
		/// センター／LFEへの出力レベルとを個別に制御する場合に使用します。
		/// 例えば、センドレベルで常にLFEから一定のボリュームで出力させておき、
		/// 定位はパンニング3Dでコントロールするような場合です。
		/// 値の範囲や扱いは、通常のボリュームと同等です。<see cref="CriAtomExPlayer.SetVolume"/> 関数を参照してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetVolume"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPan3dVolume(Single pan3dVolume)
		{
			NativeMethods.criAtomExPlayer_SetPan3dVolume(NativeHandle, pan3dVolume);
		}

		/// <summary>パンタイプの設定</summary>
		/// <param name="panType">パンタイプ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンタイプを指定します。
		/// 本関数でパンタイプを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたパンタイプで再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のパンタイプを更新することができます。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時に本関数を呼び出すと、データ側に設定されているパンタイプ設定を<b>上書き</b>します（データ側の設定値は無視されます）。
		/// 通常はデータ側でパンタイプが設定されているため、本関数を呼び出す必要はありません。
		/// ACBファイルを使用せずに音声を再生する場合に、3Dポジショニング処理を有効にするためには、本関数で<see cref="CriAtomEx.PanType._3dPos"/>を設定してください。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.PanType.Unknown"/> を指定して実行した場合、エラーが発生します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomEx.PanType"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPanType(CriAtomEx.PanType panType)
		{
			NativeMethods.criAtomExPlayer_SetPanType(NativeHandle, panType);
		}

		/// <summary>プレーヤー再生時のパンタイプの取得</summary>
		/// <returns>プレーヤー再生時のパンタイプ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤー再生時のパンタイプを取得します。
		/// 本関数は <see cref="CriAtomExPlayer.SetPanType"/> 関数にて設定したパンタイプに応じたパンタイプが返却されます。
		/// 当該設定関数を呼び出していない場合、データの設定値依存となってしまうため <see cref="CriAtomEx.PanType.Unknown"/> が返却されます。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.PanType.Auto"/> を設定している場合、以下に従って返却されるパンタイプが変化します。
		/// - <see cref="CriAtomEx.Config"/>::enable_auto_matching_in_pan_type_auto の設定
		/// - <see cref="CriAtomEx3dListener"/> が設定されているかどうか
		/// - <see cref="CriAtomEx3dSource"/> が設定されているかどうか
		/// - <see cref="CriAtomEx3dSourceList"/> が設定されているかどうか
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPanType"/>
		public CriAtomEx.PanType GetPanTypeOnPlayback()
		{
			return NativeMethods.criAtomExPlayer_GetPanTypeOnPlayback(NativeHandle);
		}

		/// <summary>パンニング時の出力スピーカータイプ設定</summary>
		/// <param name="panSpeakerType">パンニング時の出力スピーカータイプ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング時の出力スピーカータイプを指定します。
		/// 本関数でパンニング時の出力スピーカータイプを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定された出力スピーカータイプでパンニング計算されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声の出力スピーカータイプを更新することができます。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数の設定はパン3Dと3Dポジショニングにおけるパンニング計算に影響します。
		/// ライブラリ初期化時のデフォルト値は4chパンニング（<see cref="CriAtomEx.PanSpeakerType._4ch"/>）です。
		/// デフォルト値は<see cref="CriAtomExPlayer.ChangeDefaultPanSpeakerType"/> 関数にて変更可能です。
		/// ステレオスピーカーのプラットフォームでは、どれを選んだとしても最終的にはステレオにダウンミックスされます。
		/// 本パラメーターはデータ側には設定できないため、常に本関数の設定値が適用されます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomEx.PanSpeakerType"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPanSpeakerType(CriAtomEx.PanSpeakerType panSpeakerType)
		{
			NativeMethods.criAtomExPlayer_SetPanSpeakerType(NativeHandle, panSpeakerType);
		}

		/// <summary>MixDownCenterボリュームオフセット値の設定</summary>
		/// <param name="mixdownCenterVolumeOffset">MixDownCenterボリュームのオフセット値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Center, LFE以外の信号をモノラルにミックスしてCenterに出力するためのボリューム値を設定します。
		/// 本関数による設定値は、CRI Atom Craftによるデータ設定値に対して加算適用されます。
		/// 本関数の第二引数mixdown_center_volume_offsetには0～1の浮動小数点値で出力ボリュームを設定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定値の範囲外確認は行われません。範囲外を設定する際には以下の点に注意してください。
		/// 1より大きい値：出力振幅値の増幅によりクリッピングノイズ等が発生することがあります。
		/// 負値：データ設定値との和が負となった場合は、正値結果に対して位相を反転した結果が出力されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomEx.PanSpeakerType"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void AddMixDownCenterVolumeOffset(Single mixdownCenterVolumeOffset)
		{
			NativeMethods.criAtomExPlayer_AddMixDownCenterVolumeOffset(NativeHandle, mixdownCenterVolumeOffset);
		}

		/// <summary>MixDownLFEボリュームオフセット値の設定</summary>
		/// <param name="mixdownLfeVolumeOffset">MixDownLFEボリュームのオフセット値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Center, LFE以外の信号をモノラルにミックスしてLFEに出力するためのボリューム値を設定します。
		/// 本関数による設定値は、CRI Atom Craftによるデータ設定値に対して加算適用されます。
		/// 本関数の第二引数mixdown_lfe_volume_offsetには0～1の浮動小数点値で出力ボリュームを設定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 設定値の範囲外確認は行われません。範囲外を設定する際には以下の点に注意してください。
		/// 1より大きい値：出力振幅値の増幅によりクリッピングノイズ等が発生することがあります。
		/// 負値：データ設定値との和が負となった場合は、正値結果に対して位相を反転した結果が出力されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomEx.PanSpeakerType"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void AddMixDownLfeVolumeOffset(Single mixdownLfeVolumeOffset)
		{
			NativeMethods.criAtomExPlayer_AddMixDownLfeVolumeOffset(NativeHandle, mixdownLfeVolumeOffset);
		}

		/// <summary>パンニング時の出力スピーカータイプ設定のデフォルト値変更</summary>
		/// <param name="panSpeakerType">パンニング時の出力スピーカータイプ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング時の出力スピーカータイプのデフォルト値を変更します。
		/// <see cref="CriAtomExPlayer.SetPanSpeakerType"/> 関数を実行していないAtomExプレーヤーは、全て本関数で設定した出力スピーカータイプで再生されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数の設定はパン3Dと3Dポジショニングにおけるパンニング計算に影響します。
		/// ライブラリ初期化時のデフォルト値は ASR の出力 ch 数やチャンネル構成に依存したセンタースピーカーを含めない（<see cref="CriAtomEx.PanSpeakerType.Auto"/>）です。
		/// ステレオスピーカーのプラットフォームでは、どれを選んだとしても最終的にはステレオにダウンミックスされます。
		/// 本パラメーターはデータ側には設定できないため、常に本関数の設定値が適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中の音声がデフォルト値を参照するタイミングはユーザーの操作に依存します。
		/// そのため、再生中にデフォルト値を変更した場合、意図したタイミングで変更が反映されるとは限りません。
		/// 本関数を使用する場合、初期化時など音声を再生する前に実行するようにしてください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPanSpeakerType"/>
		/// <seealso cref="CriAtomEx.PanSpeakerType"/>
		public static void ChangeDefaultPanSpeakerType(CriAtomEx.PanSpeakerType panSpeakerType)
		{
			NativeMethods.criAtomExPlayer_ChangeDefaultPanSpeakerType(panSpeakerType);
		}

		/// <summary>パンニング時の角度タイプ設定</summary>
		/// <param name="panAngleType">パンニング時の角度タイプ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パンニング時の角度タイプを指定します。
		/// 角度タイプは、マルチチャンネル（ステレオ、5.1ch等）の音声素材をパンニングするときに、各入力チャンネルをどのような角度として扱うかを表します。
		/// 本関数でパンニング時の角度タイプを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定された角度タイプでパンニング計算されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声の角度タイプを更新することができます。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数の設定はパン3Dと3Dポジショニングにおけるパンニング計算に影響します。
		/// デフォルト値はオフセット（<see cref="CriAtomEx.PanAngleType.Offset"/>）です。
		/// 本関数は、主にはCRI Audioとの互換用に使用します。
		/// 本関数で <see cref="CriAtomEx.PanAngleType.Fix"/> を設定することで、CRI Audioでのパン3D計算と同じ挙動になります。
		/// 本パラメーターはデータ側には設定できないため、常に本関数の設定値が適用されます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomEx.PanAngleType"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPanAngleType(CriAtomEx.PanAngleType panAngleType)
		{
			NativeMethods.criAtomExPlayer_SetPanAngleType(NativeHandle, panAngleType);
		}

		/// <summary>センドレベルの設定</summary>
		/// <param name="ch">チャンネル番号</param>
		/// <param name="spk">スピーカーID</param>
		/// <param name="level">センドレベル値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// センドレベルを指定します。
		/// センドレベルは、音声データの各チャンネルの音声を、どのスピーカーから
		/// どの程度の音量で出力するかを指定するための仕組みです。
		/// 本関数でセンドレベルを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたセンドレベルで再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のセンドレベルを更新することができます。
		/// 第2引数のチャンネル番号は"音声データのチャンネル番号"を指定します。
		/// 第3引数のスピーカーIDには、指定したチャンネル番号のデータをどのスピーカーから
		/// 出力するかを指定し、第4引数では送信時のレベル（ボリューム）を指定します。
		/// </para>
		/// <para>
		/// センドレベル値の範囲や扱いは、ボリュームと同等です。<see cref="CriAtomExPlayer.SetVolume"/> 関数を参照してください。
		/// なお、センタースピーカーのあるプラットフォームで、モノラル音をセンタースピーカーのみから出力したい場合、
		/// 本関数ではなく<see cref="CriAtomExPlayer.SetPanSpeakerType"/> 関数で<see cref="CriAtomEx.PanSpeakerType._5ch"/>
		/// を設定することをお薦めします。
		/// </para>
		/// <para>
		/// 備考:
		/// センドレベルの設定には「自動設定」「手動設定」の2通りが存在します。
		/// AtomExプレーヤーを作成した直後や、 <see cref="CriAtomExPlayer.ResetParameters"/> 関数で
		/// パラメーターをクリアした場合、センドレベルの設定は「自動設定」となります。
		/// これに対し、本関数を実行した場合、センドレベルの設定は「手動設定」になります。
		/// （ユーザが各スピーカーへのセンドレベルをコントロールし、パンニングを行う必要があります。）
		/// 「自動設定」の場合、AtomExプレーヤーは以下のように音声をルーティングします。
		/// 【モノラル音声を再生する場合】
		/// チャンネル0の音声を左右のスピーカーから約0.7f（-3dB）のボリュームで出力します。
		/// 【ステレオ音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、
		/// チャンネル1の音声をライトスピーカーから出力します。
		/// 【4ch音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、チャンネル1の音声をライトスピーカーから、
		/// チャンネル2の音声をサラウンドレフトスピーカーから、
		/// チャンネル3の音声をサラウンドライトスピーカーからそれぞれ出力します。
		/// 【5.1ch音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、チャンネル1の音声をライトスピーカーから、
		/// チャンネル2の音声をセンタースピーカーから、チャンネル3の音声をLFEから、
		/// チャンネル4の音声をサラウンドレフトスピーカーから、
		/// チャンネル5の音声をサラウンドライトスピーカーからそれぞれ出力します。
		/// 【7.1ch音声を再生する場合】
		/// チャンネル0の音声をレフトスピーカーから、チャンネル1の音声をライトスピーカーから、
		/// チャンネル2の音声をセンタースピーカーから、チャンネル3の音声をLFEから、
		/// チャンネル4の音声をサラウンドレフトスピーカーから、
		/// チャンネル5の音声をサラウンドライトスピーカーからそれぞれ出力します。
		/// チャンネル6の音声をサラウンドバックレフトスピーカーから、
		/// チャンネル7の音声をサラウンドバックライトスピーカーからそれぞれ出力します。
		/// これに対し、本関数を用いて「手動設定」を行った場合、音声データのチャンネル数に
		/// 関係なく、指定されたセンドレベル設定で音声が出力されます。
		/// （音声データのチャンネル数に応じて、適宜センドレベル設定を切り替える必要があります。）
		/// 過去に指定したセンドレベルをクリアし、ルーティングを「自動設定」の状態に戻したい場合は、
		/// <see cref="CriAtomExPlayer.ResetParameters"/> 関数を実行してください。
		/// 本パラメーターはデータ側には設定できないため、常に本関数の設定値が適用されます。
		/// </para>
		/// <para>
		/// 注意:
		/// センドレベルを設定していないチャンネルについては、音声が出力されません。
		/// 例えば、再生する音声データがステレオにもかかわらず、どちらか一方のチャンネルに対して
		/// しかセンドレベルが設定されていない場合、センドレベルを設定していないチャンネルの音声
		/// はミュートされます。
		/// センドレベルをコントロールする際には、必ず出力を行いたい全てのチャンネルについてセンド
		/// レベルの設定を行ってください。
		/// 本関数を用いてセンドレベルを設定した場合、パン3Dや3Dポジショニングの設定は無視されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetVolume"/>
		/// <seealso cref="CriAtomExPlayer.SetPanSpeakerType"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetSendLevel(Int32 ch, CriAtomEx.SpeakerId spk, Single level)
		{
			NativeMethods.criAtomExPlayer_SetSendLevel(NativeHandle, ch, spk, level);
		}

		/// <summary>バスセンドレベルの設定</summary>
		/// <param name="busName">バス名</param>
		/// <param name="level">センドレベル値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスセンドレベルを指定します。
		/// バスセンドレベルは、音声をどのバスにどれだけ流すかを指定するための仕組みです。
		/// 本関数でバスセンドレベルを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたバスセンドレベルで再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のバスセンドレベルを更新することができます。
		/// キュー再生時、データ側にバスセンドレベルが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数による設定値とを<b>乗算</b>した値が適用されます。
		/// 第2引数にはDSPバス設定内のバス名を指定します。
		/// 第3引数では送信時のレベル（ボリューム）を指定します。
		/// 第2引数のバス名で指定したバスが適用中のDSPバス設定に存在しない場合、設定値は無効値として処理されます。
		/// センドレベル値の範囲や扱いは、ボリュームと同等です。<see cref="CriAtomExPlayer.SetVolume"/> 関数を参照してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数に異なるバス名を指定して複数回呼び出すことで、複数のバスに流すこともできます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetVolume"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetBusSendLevelByName(ArgString busName, Single level)
		{
			NativeMethods.criAtomExPlayer_SetBusSendLevelByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), level);
		}

		/// <summary>バスセンドレベルのリセット</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomEx プレーヤーに設定されているバスセンド情報をリセットし、初期状態（未設定状態）に戻します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetBusSendLevelByName"/>
		public void ResetBusSends()
		{
			NativeMethods.criAtomExPlayer_ResetBusSends(NativeHandle);
		}

		/// <summary>バスセンドレベルの取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="level">バスセンドレベル値（0.0f～1.0f）</param>
		/// <returns>バスセンドレベル値が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定した AtomEx プレーヤーに設定されている特定のバスセンドレベルを取得します。
		/// 第2引数にはDSPバス設定内のバス名を指定します。
		/// なお、以下のケースに該当する場合、バスセンドレベルの取得に失敗します。
		/// - 第2引数のバス名で指定したバスが適用中のDSPバス設定に存在しない
		/// - 第1引数にて指定した AtomEx プレーヤーに第2引数のバス名に関するバスセンドレベルの設定を行っていない
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetBusSendLevelByName"/>
		public unsafe bool GetBusSendLevelByName(ArgString busName, out Single level)
		{
			fixed (Single* levelPtr = &level)
				return NativeMethods.criAtomExPlayer_GetBusSendLevelByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), levelPtr);
		}

		/// <summary>バスセンドレベルの設定（オフセット指定）</summary>
		/// <param name="busName">バス名</param>
		/// <param name="levelOffset">センドレベル値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バスセンドレベルをオフセットで指定します。
		/// キュー再生時、データ側にバスセンドレベルが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数による設定値とを<b>加算</b>した値が適用されます。
		/// それ以外の仕様は <see cref="CriAtomExPlayer.SetBusSendLevelByName"/> 関数と同様です。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExPlayer.SetBusSendLevelByName"/> 関数で 0.0f を設定し、かつ本関数でオフセット値を設定することで、
		/// データ側に設定されていたバスセンドレベルを無視して値が設定可能です。（上書き設定）
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetBusSendLevelByName"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetBusSendLevelOffsetByName(ArgString busName, Single levelOffset)
		{
			NativeMethods.criAtomExPlayer_SetBusSendLevelOffsetByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), levelOffset);
		}

		/// <summary>バスセンドレベルのオフセットの取得</summary>
		/// <param name="busName">バス名</param>
		/// <param name="levelOffset">バスセンドレベルのオフセット値（0.0f～1.0f）</param>
		/// <returns>バスセンドレベルのオフセット値が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定した AtomEx プレーヤーに設定されている特定のバスセンドレベルのオフセットを取得します。
		/// 第2引数にはDSPバス設定内のバス名を指定します。
		/// なお、以下のケースに該当する場合、バスセンドレベルのオフセットの取得に失敗します。
		/// - 第2引数のバス名で指定したバスが適用中のDSPバス設定に存在しない
		/// - 第1引数にて指定した AtomEx プレーヤーに第2引数のバス名に関するバスセンドレベルの設定を行っていない
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetBusSendLevelByName"/>
		public unsafe bool GetBusSendLevelOffsetByName(ArgString busName, out Single levelOffset)
		{
			fixed (Single* levelOffsetPtr = &levelOffset)
				return NativeMethods.criAtomExPlayer_GetBusSendLevelOffsetByName(NativeHandle, busName.GetPointer(stackalloc byte[busName.BufferSize]), levelOffsetPtr);
		}

		/// <summary>ADX1互換のパンの設定</summary>
		/// <param name="ch">チャンネル番号</param>
		/// <param name="pan">パン設定値（-1.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADX1互換のパン設定関数です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数はADX1からの移植タイトル用に用意されています。
		/// ADX環境で新規にパン操作を行うアプリケーションを作成する場合、
		/// <see cref="CriAtomExPlayer.SetPan3dAngle"/> 関数を使用してください。
		/// 本関数でパンを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたパンで再生されます。
		/// すでに再生された音声のパンを変更する場合、本関数で新たなパン設定をプレーヤーに指定し、
		/// <see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数で再生中の音声にそのパラメーターを適用する必要があります。
		/// 本関数でパンをコントロール可能なのは、モノラル音声とステレオ音声のみです。
		/// また、左右の定位のみコントロールできます。
		/// 3ch以上の音声に対してパンをコントロールしたい場合や、前後を含めた定位をコントロールしたい場合には、
		/// <see cref="CriAtomExPlayer.SetPan3dAngle"/> 関数や<see cref="CriAtomExPlayer.SetSendLevel"/> 関数を使用する必要があります。
		/// 再生する音声データがステレオの場合、チャンネル0番とチャンネル1番のそれぞれのパン
		/// について、独立してコントロールすることが可能です。
		/// ただし、設定されたパンがモノラル音声向けなのか、ステレオ音声向けなのかは区別
		/// されないため、ステレオ設定用にパン設定を行ったAtomExプレーヤーでモノラル音声を再生
		/// した場合、意図しない位置に音源が定位する可能性があります。
		/// 再生する音声データがステレオにもかかわらず、どちらか一方のチャンネルに対して
		/// しかパンが設定されていない場合、パンを設定していないチャンネルの音声の定位位置
		/// は 0.0f （中央からの出力）になります。
		/// ステレオ音声のパンをコントロールする際には、必ず両方のチャンネルについてパンの
		/// 設定を行ってください。
		/// 本関数と<see cref="CriAtomExPlayer.SetPan3dAngle"/> 関数や <see cref="CriAtomExPlayer.SetSendLevel"/> 関数を併用しないでください。
		/// 両者を併用した場合、意図しないパンで再生される可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetPan3dAngle"/>
		/// <seealso cref="CriAtomExPlayer.SetSendLevel"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetPanAdx1Compatible(Int32 ch, Single pan)
		{
			NativeMethods.criAtomExPlayer_SetPanAdx1Compatible(NativeHandle, ch, pan);
		}

		/// <summary>バンドパスフィルターのパラメーター設定</summary>
		/// <param name="cofLow">正規化低域カットオフ周波数（0.0f～1.0f）</param>
		/// <param name="cofHigh">正規化高域カットオフ周波数（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バンドパスフィルターのカットオフ周波数を指定します。
		/// 本関数でカットオフ周波数を設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたカットオフ周波数でバンドパスフィルターが動作します。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対してバンドパスフィルターのカットオフ周波数を更新することができます。
		/// 正規化カットオフ周波数は、対数軸上の24Hz～24000Hzを、0.0f～1.0fに正規化した値です。
		/// 例えば、正規化低域カットオフ周波数を0.0f、正規化高域カットオフ周波数を1.0fと指定すると、
		/// バンドパスフィルターは全域が通過し、正規化低域カットオフ周波数を上げるほど、
		/// また正規化高域カットオフ周波数を下げるほど、通過域が狭くなっていきます。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にバンドパスフィルターのパラメーターが設定されている場合に本関数を呼び出すと、
		/// 以下のように設定されます。
		/// - cof_low
		/// データに設定された値に対し、「cof_low_rev = 1.0f - cof_low」としてから乗算し、最終的にまた「cof_low = 1.0f - cof_low_rev」と元に戻して適用されます。
		/// つまり、0.0fを「低域側に最もフィルターを開く」として、開き具合を乗算して適用していく形になります。
		/// - cof_high
		/// データに設定された値に対し、乗算して適用されます。
		/// つまり、1.0fを「高域側に最もフィルターを開く」として、開き具合を乗算して適用していく形になります。
		/// 実際に適用される正規化カットオフ周波数が1.0fを超える値になった場合、値は1.0fにクリップされます。
		/// 同様に、実際に適用される正規化カットオフ周波数が0.0f未満の値になった場合も、値は0.0fにクリップされます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetBandpassFilterParameters(Single cofLow, Single cofHigh)
		{
			NativeMethods.criAtomExPlayer_SetBandpassFilterParameters(NativeHandle, cofLow, cofHigh);
		}

		/// <summary>バイクアッドフィルターのパラメーター設定</summary>
		/// <param name="type">フィルタータイプ</param>
		/// <param name="frequency">正規化周波数（0.0f～1.0f）</param>
		/// <param name="gain">ゲイン（デシベル値）</param>
		/// <param name="qValue">Q値</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バイクアッドフィルターの各種パラメーターを指定します。
		/// 本関数でパラメーターを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたパラメーターでバイクアッドフィルターが動作します。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対してバイクアッドフィルターのパラメーターを更新することができます。
		/// 正規化周波数は、対数軸上の24Hz～24000Hzを、0.0f～1.0fに正規化した値です。
		/// ゲインはデシベルで指定します。
		/// ゲインはフィルタータイプが以下の場合のみ有効です。
		/// - <see cref="CriAtomEx.BiquadFilterType.Lowshelf"/>：ローシェルフフィルター
		/// - <see cref="CriAtomEx.BiquadFilterType.Highshelf"/>：ハイシェルフフィルター
		/// - <see cref="CriAtomEx.BiquadFilterType.Peaking"/>：ピーキングフィルター
		/// </para>
		/// <para>
		/// 備考:
		/// - type
		/// データに設定された値を上書きします。
		/// - frequency
		/// データに設定された値に加算されます。
		/// - gain
		/// データに設定された値に乗算されます。
		/// - q_value
		/// データに設定された値に加算されます。
		/// 実際に適用される正規化カットオフ周波数が1.0fを超える値になった場合、値は1.0fにクリップされます。
		/// 同様に、実際に適用される正規化カットオフ周波数が0.0f未満の値になった場合も、値は0.0fにクリップされます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// HCA-MX用にエンコードされた音声データには、バイクアッドフィルターが適用されません。
		/// バイクアッドフィルターを使用したい音声は、ADXやHCA等、他のコーデックでエンコードしてください。
		/// ASRが利用できる環境では、ネイティブボイス出力時にフィルターを使用できません。
		/// ASRが利用可能な環境でバイクアッドフィルターを使用したい場合には、
		/// 出力サウンドレンダラをASRに設定する必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetBiquadFilterParameters(CriAtomEx.BiquadFilterType type, Single frequency, Single gain, Single qValue)
		{
			NativeMethods.criAtomExPlayer_SetBiquadFilterParameters(NativeHandle, type, frequency, gain, qValue);
		}

		/// <summary>ボイスプライオリティの設定</summary>
		/// <param name="priority">ボイスプライオリティ（-255～255）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーにボイスプライオリティを設定します。
		/// 本関数でプライオリティをセット後、 <see cref="CriAtomExPlayer.Start"/> 関数で音声を再生すると、
		/// 再生された音声は本関数でセットしたプライオリティで発音されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のプライオリティを更新することができます。
		/// ボイスプライオリティには、-255～255の範囲で整数値を指定します。
		/// 範囲外の値を設定した場合、範囲に収まるようにクリッピングされます。
		/// 関数実行前のデフォルト設定値は0です。
		/// </para>
		/// <para>
		/// 備考:
		/// AtomExプレーヤーが波形データを再生しようとした際、
		/// 当該波形データが所属するボイスリミットグループの発音数が上限に達していた場合や、
		/// ボイスプール内のボイスが全て使用中であった場合、
		/// ボイスプライオリティによる発音制御が行われます。
		/// （指定された波形データを再生するかどうかを、ボイスプライオリティをもとに判定します。）
		/// 具体的には、再生を行おうとした波形データのプライオリティが、
		/// 現在ボイスで再生中の波形データのプライオリティよりも高い場合、
		/// AtomExプレーヤーは再生中のボイスを奪い取り、リクエストされた波形データの再生を開始します。
		/// （再生中の音声が停止され、別の音声が再生されます。）
		/// 逆に、再生を行おうとした波形データのプライオリティが、
		/// ボイスで再生中の波形データのプライオリティよりも低い場合、
		/// AtomExプレーヤーはリクエストされた波形データの再生を行いません。
		/// （リクエストされた音声は再生されず、再生中の音声が引き続き鳴り続けます。）
		/// 再生しようとした波形データのプライオリティが、
		/// ボイスで再生中の波形データのプライオリティと等しい場合、
		/// AtomExプレーヤーは発音制御方式（先着優先 or 後着優先）に従い、
		/// 以下のような制御が行われます。
		/// - 先着優先時は、再生中の波形データを優先し、リクエストされた波形データを再生しません。
		/// - 後着優先時は、リクエストされた波形データを優先し、ボイスを奪い取ります。
		/// キュー再生時、データ側にボイスプライオリティが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数による設定値とを<b>加算</b>した値が適用されます。
		/// 例えば、データ側のプライオリティが255、AtomExプレーヤーのプライオリティが45の場合、
		/// 実際に適用されるプライオリティは300になります。
		/// 本関数で設定可能な値の範囲は-255～255ですが、ライブラリ内部の計算は CriSint32 の範囲で行われるため、
		/// データ側と加算した結果は-255～255を超える場合があります。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、波形データにセットされた<b>ボイスプライオリティ</b>を制御します。
		/// Atom Craft上でキューに対して設定された<b>カテゴリキュープライオリティ</b>には影響を与えませんので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetVoiceControlMethod"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetVoicePriority(Int32 priority)
		{
			NativeMethods.criAtomExPlayer_SetVoicePriority(NativeHandle, priority);
		}

		/// <summary>AISACコントロール値の設定（コントロールID指定）</summary>
		/// <param name="controlId">コントロールID</param>
		/// <param name="controlValue">コントロール値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コントロールID指定でAISACのコントロール値を指定します。
		/// 本関数でAISACコントロール値を設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたAISACコントロール値で再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のAISACコントロール値を更新することができます。
		/// 設定したコントロール値を削除するには、<see cref="CriAtomExPlayer.ClearAisacControls"/> 関数を使用してください。
		/// AISACコントロール値には、0.0f～1.0fの範囲で実数値を指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// AISACのコントロールタイプによって、以下のように挙動が変わります。
		/// - オフ
		/// - 本関数等によるAISACコントロール値が未設定の場合はそのAISACは動作しません。
		/// - オートモジュレーション
		/// - 本関数の設定値には影響されず、時間経過とともに自動的にAISACコントロール値が変化します。
		/// - ランダム
		/// - 本関数等によって設定されたAISACコントロール値を中央値として、データに設定されたランダム幅でランダマイズし、最終的なAISACコントロール値を決定します。
		/// - ランダマイズ処理は再生開始時のパラメーター適用でのみ行われ、再生中の音声に対するAISACコントロール値変更はできません。
		/// - 再生開始時にAISACコントロール値が設定されていなかった場合、0.0fを中央値としてランダマイズ処理を行います。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlByName"/>
		/// <seealso cref="CriAtomExPlayer.ClearAisacControls"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetAisacControlById(UInt32 controlId, Single controlValue)
		{
			NativeMethods.criAtomExPlayer_SetAisacControlById(NativeHandle, controlId, controlValue);
		}

		/// <summary>AISACコントロール値の設定（コントロール名指定）</summary>
		/// <param name="controlName">コントロール名</param>
		/// <param name="controlValue">コントロール値（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コントロール名指定でAISACのコントロール値を指定します。
		/// 本関数でAISACコントロール値を設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたAISACコントロール値で再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声のAISACコントロール値を更新することができます。
		/// 設定したコントロール値を削除するには、<see cref="CriAtomExPlayer.ClearAisacControls"/> 関数を使用してください。
		/// AISACコントロール値の扱いは<see cref="CriAtomExPlayer.SetAisacControlById"/> 関数と同様です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlById"/>
		/// <seealso cref="CriAtomExPlayer.ClearAisacControls"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetAisacControlByName(ArgString controlName, Single controlValue)
		{
			NativeMethods.criAtomExPlayer_SetAisacControlByName(NativeHandle, controlName.GetPointer(stackalloc byte[controlName.BufferSize]), controlValue);
		}

		/// <summary>プレーヤーに設定されているAISACコントロール値の削除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに設定されているAISACコントロール値を全て削除します。
		/// また削除後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生中の音声に対してAISACコントロール値の削除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlById"/>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlByName"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		public void ClearAisacControls()
		{
			NativeMethods.criAtomExPlayer_ClearAisacControls(NativeHandle);
		}

		/// <summary>3Dリスナーオブジェクトの設定</summary>
		/// <param name="listener">3Dリスナーオブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dポジショニングを実現するための3Dリスナーオブジェクトを設定します。
		/// 3Dリスナーオブジェクトと3D音源オブジェクトまたは3D音源オブジェクトリストを設定することで、3Dリスナーと3D音源オブジェクトまたは
		/// 3D音源オブジェクトリスト内の全ての3D音源オブジェクトの位置関係等から定位や音量、ピッチ等が自動的に適用されます。
		/// 本関数で3Dリスナーオブジェクトを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定された3Dリスナーオブジェクトを参照して再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声が参照する3Dリスナーオブジェクトを変更することができます。
		/// listenerにnullを設定した場合は、すでに設定されている3Dリスナーオブジェクトをクリアします。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数で3Dリスナーオブジェクトを設定していなくても、以下の条件を満たしている場合は自動的に3D音源に対して最も距離の近い3Dリスナーが割り当てられます。
		/// - 3D音源オブジェクトまたは3D音源オブジェクトリストが設定されている
		/// - 3Dリスナーが<see cref="CriAtomEx3dListener.CriAtomEx3dListener"/>によって作成されている
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 3Dリスナーオブジェクトのパラメーターの変更、更新は、AtomExプレーヤーの関数ではなく、3Dリスナーオブジェクトの関数を使用して行います。
		/// デフォルトでは、3Dポジショニングの計算は左手座標系で行われます。
		/// 右手座標系で各種ベクトルを設定する場合は、ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）の設定で
		/// <see cref="CriAtomEx.Config"/>::coordinate_system に<see cref="CriAtomEx.CoordinateSystem.RightHanded"/> を指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dListener"/>
		/// <seealso cref="CriAtomExPlayer.Set3dSourceHn"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void Set3dListenerHn(CriAtomEx3dListener listener)
		{
			NativeMethods.criAtomExPlayer_Set3dListenerHn(NativeHandle, listener?.NativeHandle ?? default);
		}

		/// <summary>3D音源オブジェクトの設定</summary>
		/// <param name="source">3D音源オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 3Dポジショニングを実現するための3D音源オブジェクトを設定します。
		/// 3Dリスナーオブジェクトと3D音源オブジェクトを設定することで、3Dリスナーオブジェクトと3D音源オブジェクトの位置関係等から定位や音量、ピッチ等が自動的に適用されます。
		/// 本関数で3D音源オブジェクトを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定された3D音源オブジェクトを参照して再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声が参照する3D音源オブジェクトを変更することができます。
		/// sourceにnullを設定した場合は、すでに設定されている3D音源オブジェクトをクリアします。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 3D音源オブジェクトのパラメーターの変更、更新は、AtomExプレーヤーの関数ではなく、3D音源オブジェクトの関数を使用して行います。
		/// デフォルトでは、3Dポジショニングの計算は左手座標系で行われます。
		/// 右手座標系で各種ベクトルを設定する場合は、ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）の設定で
		/// <see cref="CriAtomEx.Config"/>::coordinate_system に<see cref="CriAtomEx.CoordinateSystem.RightHanded"/> を指定してください。
		/// ACBファイルを使用せずに音声を再生する場合は、明示的に3Dポジショニングを有効にするために、::criAtomExSetPanType 関数で<see cref="CriAtomEx.PanType._3dPos"/>を設定する必要があります。
		/// 本関数と <see cref="CriAtomExPlayer.Set3dSourceListHn"/> 関数はお互いに設定を上書きします。
		/// 例えば、 <see cref="CriAtomExPlayer.Set3dSourceListHn"/> 関数にて3D音源オブジェクトリストをAtomExプレーヤーに設定後、本関数にてAtomExプレーヤーに3D音源オブジェクトを設定すると、
		/// AtomExプレーヤーには新たに3D音源オブジェクトが設定され、既に設定されていた3D音源オブジェクトリストはAtomExプレーヤーからクリアされます。
		/// 本関数を用いてAtomExプレーヤーに設定された3D音源オブジェクトは、3D音源オブジェクトリストに追加することはできません。
		/// もし3D音源オブジェクトリストに追加する場合は、既に設定されているAtomExプレーヤーの3D音源オブジェクトに関する設定をクリアしてください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSource"/>
		/// <seealso cref="CriAtomEx3dSourceList"/>
		/// <seealso cref="CriAtomExPlayer.Set3dSourceListHn"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void Set3dSourceHn(CriAtomEx3dSource source)
		{
			NativeMethods.criAtomExPlayer_Set3dSourceHn(NativeHandle, source?.NativeHandle ?? default);
		}

		/// <summary>3D音源オブジェクトリストの設定</summary>
		/// <param name="sourceList">3D音源オブジェクトリスト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// マルチポジショニング再生を実現するための3D音源オブジェクトリストを設定します。
		/// 3Dリスナーオブジェクトと3D音源オブジェクトリストを設定することで、3Dリスナーオブジェクトと3D音源オブジェクトリスト内の
		/// 全ての3D音源オブジェクトの位置関係等から定位や音量、ピッチ等が自動的に適用されます。
		/// 本関数で3D音源オブジェクトリストを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定された3D音源オブジェクトリストを参照して再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声が参照する3D音源オブジェクトリストを変更することができます。
		/// source_listにnullを設定した場合は、すでに設定されている3D音源オブジェクトリストをクリアします。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 3D音源オブジェクトリストに追加されている3D音源オブジェクトの変更、更新は、AtomExプレーヤーの関数ではなく、3D音源オブジェクトの関数を使用して行います。
		/// 右手座標系で各種ベクトルを設定する場合は、ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）の設定で
		/// <see cref="CriAtomEx.Config"/>::coordinate_system に<see cref="CriAtomEx.CoordinateSystem.RightHanded"/> を指定してください。
		/// ACBファイルを使用せずに音声を再生する場合は、明示的に3Dポジショニングを有効にするために、::criAtomExSetPanType 関数で<see cref="CriAtomEx.PanType._3dPos"/>を設定する必要があります。
		/// 本関数と <see cref="CriAtomExPlayer.Set3dSourceHn"/> 関数はお互いに設定を上書きします。
		/// 例えば、 <see cref="CriAtomExPlayer.Set3dSourceHn"/> 関数にて3D音源オブジェクトをAtomExプレーヤーに設定後、本関数にてAtomExプレーヤーに3D音源オブジェクトリストを設定すると、
		/// AtomExプレーヤーには新たに3D音源オブジェクトリストが設定され、既に設定されていた3D音源オブジェクトはAtomExプレーヤーからクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx3dSourceList"/>
		/// <seealso cref="CriAtomEx3dSource"/>
		/// <seealso cref="CriAtomExPlayer.Set3dSourceHn"/>
		/// <seealso cref="CriAtomEx3dListener"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void Set3dSourceListHn(CriAtomEx3dSourceList sourceList)
		{
			NativeMethods.criAtomExPlayer_Set3dSourceListHn(NativeHandle, sourceList?.NativeHandle ?? default);
		}

		/// <summary>AISACコントロール値の取得（コントロールID指定）</summary>
		/// <param name="controlId">コントロールID</param>
		/// <returns>コントロール値（0.0f～1.0f）、未設定時は-1.0f</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コントロールID指定でAISACのコントロール値を取得します。
		/// 指定したコントロールIDのAISACコントロール値が設定されていなかった場合、-1.0fを返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、AtomExプレーヤーに設定されたAISACコントロール値を取得します。
		/// 再生中の音声にAISACコントロール値を変更するAISACが設定されていたとしても、その変更結果を取得することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlById"/>
		/// <seealso cref="CriAtomExPlayer.GetAisacControlByName"/>
		public Single GetAisacControlById(UInt32 controlId)
		{
			return NativeMethods.criAtomExPlayer_GetAisacControlById(NativeHandle, controlId);
		}

		/// <summary>AISACコントロール値の取得（コントロール名指定）</summary>
		/// <param name="controlName">コントロール名</param>
		/// <returns>コントロール値（0.0f～1.0f）、未設定時は-1.0f</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// コントロール名指定でAISACのコントロール値を取得します。
		/// 指定したコントロール名のAISACコントロール値が設定されていなかった場合、-1.0fを返します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、AtomExプレーヤーに設定されたAISACコントロール値を取得します。
		/// 再生中の音声にAISACコントロール値を変更するAISACが設定されていたとしても、その変更結果を取得することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAisacControlByName"/>
		/// <seealso cref="CriAtomExPlayer.GetAisacControlById"/>
		public Single GetAisacControlByName(ArgString controlName)
		{
			return NativeMethods.criAtomExPlayer_GetAisacControlByName(NativeHandle, controlName.GetPointer(stackalloc byte[controlName.BufferSize]));
		}

		/// <summary>カテゴリの設定（ID指定）</summary>
		/// <param name="categoryId">カテゴリID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリID指定でカテゴリを設定します。
		/// 設定したカテゴリ情報を削除するには、 <see cref="CriAtomExPlayer.UnsetCategory"/> 関数を使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時に本関数を呼び出すと、データ側に設定されているカテゴリ設定とマージされます。
		/// その際、カテゴリグループが競合している場合には本関数の設定が有効になります。
		/// CRI Atomライブラリ Ver.2.20.31未満では
		/// データ側に設定されているカテゴリ設定を<b>上書き</b>していました。（データ側の設定値は無視されていた）。
		/// 従来の仕様で動作させたい場合はライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）に
		/// <see cref="CriAtomEx.Config"/>::enable_category_override_by_ex_player にtrueを設定してください。
		/// 本関数で設定したカテゴリ情報は、ACFのレジスト、アンレジストを行うとクリアされます。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// カテゴリ設定は再生開始前に行ってください。再生中の音声のカテゴリは更新されません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.UnsetCategory"/>
		/// <seealso cref="CriAtomExPlayer.SetCategoryByName"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetCategoryById(UInt32 categoryId)
		{
			NativeMethods.criAtomExPlayer_SetCategoryById(NativeHandle, categoryId);
		}

		/// <summary>カテゴリの設定（カテゴリ名指定）</summary>
		/// <param name="categoryName">カテゴリ名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリ名指定でカテゴリを設定します。
		/// 設定したカテゴリ情報を削除するには、 <see cref="CriAtomExPlayer.UnsetCategory"/> 関数を使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// カテゴリ指定を名前で行うことを除き、基本的な仕様は<see cref="CriAtomExPlayer.SetCategoryById"/> 関数と同様です。
		/// デフォルトカテゴリ名での指定を行う場合は CRIATOMEXCATEGORY_DEFAULT_NAME_??? を使用してください。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// カテゴリ設定は再生開始前に行ってください。再生中の音声のカテゴリは更新されません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.UnsetCategory"/>
		/// <seealso cref="CriAtomExPlayer.SetCategoryById"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetCategoryByName(ArgString categoryName)
		{
			NativeMethods.criAtomExPlayer_SetCategoryByName(NativeHandle, categoryName.GetPointer(stackalloc byte[categoryName.BufferSize]));
		}

		/// <summary>カテゴリの削除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーオブジェクトに設定されているカテゴリ情報を削除します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetCategoryByName"/>
		/// <seealso cref="CriAtomExPlayer.SetCategoryById"/>
		public void UnsetCategory()
		{
			NativeMethods.criAtomExPlayer_UnsetCategory(NativeHandle);
		}

		/// <summary>カテゴリ数の取得</summary>
		/// <returns>カテゴリ数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーオブジェクトに設定されているカテゴリの数を取得します。
		/// </para>
		/// </remarks>
		public Int32 GetNumCategories()
		{
			return NativeMethods.criAtomExPlayer_GetNumCategories(NativeHandle);
		}

		/// <summary>カテゴリ情報の取得（インデックス指定）</summary>
		/// <param name="index">インデックス</param>
		/// <param name="info">カテゴリ情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インデックスを指定してプレーヤーオブジェクトに設定されているカテゴリ情報を取得します。
		/// 指定したインデックスのカテゴリが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public unsafe bool GetCategoryInfo(UInt16 index, out CriAtomExCategory.Info info)
		{
			fixed (CriAtomExCategory.Info* infoPtr = &info)
				return NativeMethods.criAtomExPlayer_GetCategoryInfo(NativeHandle, index, infoPtr);
		}

		/// <summary>トラック情報の指定</summary>
		/// <param name="numTracks">トラック数</param>
		/// <param name="channelsPerTrack">トラック当たりのチャンネル数</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// マルチチャンネル音声のトラック構成を指定します。
		/// 本関数を使用することで、6chの音声データをモノラル6トラックの音声や、
		/// ステレオ3トラックの音声として扱うことが可能になります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に <see cref="CriAtomExPlayer.SetSendLevel"/> 関数を使用しています。
		/// そのため、本関数実行後に <see cref="CriAtomExPlayer.SetSendLevel"/> 関数を使用した場合、
		/// 音声の出力位置や出力ボリュームが意図しない結果になる可能性があります。
		/// （同様に、 <see cref="CriAtomExPlayer.SetPan3dAngle"/> 関数や <see cref="CriAtomExPlayer.SetSendLevel"/>
		/// 関数も併用できません。）
		/// 本関数は、３チャンネル以上の入力に対応した機種でしか利用できません。
		/// ２チャンネル（ステレオ）以下の入力までしか対応していない機種ではリンクエラーとなります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetTrackVolume"/>
		public unsafe void SetTrackInfo(Int32 numTracks, in Int32 channelsPerTrack)
		{
			fixed (Int32* channelsPerTrackPtr = &channelsPerTrack)
				NativeMethods.criAtomExPlayer_SetTrackInfo(NativeHandle, numTracks, channelsPerTrackPtr);
		}

		/// <summary>トラックのボリューム設定</summary>
		/// <param name="trackNo">トラック番号</param>
		/// <param name="volume">トラックのボリューム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トラックごとのボリュームを設定します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は <see cref="CriAtomExPlayer.SetTrackInfo"/> 関数でトラック情報を設定した
		/// プレーヤーに対してのみ実行可能です。
		/// 本関数は内部的に <see cref="CriAtomExPlayer.SetSendLevel"/> 関数を使用しています。
		/// そのため、本関数実行後に <see cref="CriAtomExPlayer.SetSendLevel"/> 関数を使用した場合、
		/// 音声の出力位置や出力ボリュームが意図しない結果になる可能性があります。
		/// （同様に、 <see cref="CriAtomExPlayer.SetPan3dAngle"/> 関数や <see cref="CriAtomExPlayer.SetSendLevel"/>
		/// 関数も併用できません。）
		/// 本関数は、３チャンネル以上の入力に対応した機種でしか利用できません。
		/// ２チャンネル（ステレオ）以下の入力までしか対応していない機種ではリンクエラーとなります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetTrackInfo"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetTrackVolume(Int32 trackNo, Single volume)
		{
			NativeMethods.criAtomExPlayer_SetTrackVolume(NativeHandle, trackNo, volume);
		}

		/// <summary>無音時処理モードの設定</summary>
		/// <param name="silentMode">無音時処理モード</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 無音時処理モードを指定します。
		/// 本関数で無音時処理モードを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定された無音時処理モードで再生されます。
		/// また設定後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声の無音時処理モードを更新することができます。
		/// 無音時処理モードの詳細は、<see cref="CriAtomEx.SilentMode"/> を参照してください。
		/// 無音時処理モードのデフォルト値は<see cref="CriAtomEx.SilentMode.Normal"/> です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SilentMode"/>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetSilentMode(CriAtomEx.SilentMode silentMode)
		{
			NativeMethods.criAtomExPlayer_SetSilentMode(NativeHandle, silentMode);
		}

		/// <summary>キュープライオリティの設定</summary>
		/// <param name="cuePriority">キュープライオリティ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーにキュープライオリティを設定します。
		/// 本関数でキュープライオリティをセット後、 <see cref="CriAtomExPlayer.Start"/> 関数で音声を再生すると、
		/// 再生された音声は本関数でセットしたキュープライオリティで発音されます。
		/// 関数実行前のデフォルト設定値は0です。
		/// </para>
		/// <para>
		/// 備考:
		/// AtomExプレーヤーがキューを再生した際、再生するキューの所属先カテゴリがリミット数
		/// 分発音済みの場合、プライオリティによる発音制御が行われます。
		/// 具体的には、AtomExプレーヤーの再生リクエストが、再生中のキューのプライオリティよりも
		/// 高い場合、AtomExプレーヤーは再生中のキューを停止し、リクエストによる再生を開始します。
		/// （再生中の音声が停止され、別の音声が再生されます。）
		/// 逆に、AtomExプレーヤーの再生リクエストが、再生中のキューのプライオリティよりも低い場合、
		/// AtomExプレーヤーの再生リクエストが拒否されます。
		/// （リクエストされたキューは再生されません。）
		/// AtomExプレーヤーの再生リクエストが、再生中のキューのプライオリティと等しい場合、
		/// AtomExプレーヤーは後着優先で発音制御を行います。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetCuePriority(Int32 cuePriority)
		{
			NativeMethods.criAtomExPlayer_SetCuePriority(NativeHandle, cuePriority);
		}

		/// <summary>プリディレイタイムの設定</summary>
		/// <param name="predelayTimeMs">プリディレイ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プリディレイタイムを設定します。
		/// 本関数でプリディレイタイムを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、
		/// 設定されたプリディレイタイム発音を待ちます。
		/// プリディレイタイムの単位はms（ミリ秒）です。
		/// プリディレイタイムのデフォルト値は0.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にプリディレイタイムが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値と本関数の設定値を<b>加算</b>した値が適用されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		public void SetPreDelayTime(Single predelayTimeMs)
		{
			NativeMethods.criAtomExPlayer_SetPreDelayTime(NativeHandle, predelayTimeMs);
		}

		/// <summary>エンベロープのアタックタイムの設定</summary>
		/// <param name="attackTimeMs">アタックタイム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのアタックタイムを設定します。
		/// 本関数でアタックタイムを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたアタックタイムで再生されます。
		/// アタックタイムの単位はms（ミリ秒）です。
		/// アタックタイムのデフォルト値は0.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にアタックタイムが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeAttackTime(Single attackTimeMs)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeAttackTime(NativeHandle, attackTimeMs);
		}

		/// <summary>エンベロープのアタックカーブの設定</summary>
		/// <param name="curveType">カーブタイプ</param>
		/// <param name="strength">カーブの強さ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのアタックカーブを設定します。
		/// 本関数でアタックカーブを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたアタックカーブで再生されます。
		/// カーブタイプは <see cref="CriAtomEx.CurveType"/> に定義しているもを指定します。
		/// カーブタイプのデフォルトは <see cref="CriAtomEx.CurveType.Linear"/> です。
		/// カーブの強さは、0.0f～2.0fの範囲で実数値を指定します。
		/// カーブの強さのデフォルト値は1.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にアタックカーブが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeAttackCurve(CriAtomEx.CurveType curveType, Single strength)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeAttackCurve(NativeHandle, curveType, strength);
		}

		/// <summary>エンベロープのホールドタイムの設定</summary>
		/// <param name="holdTimeMs">ホールドタイム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのホールドタイムを設定します。
		/// 本関数でホールドタイムを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたホールドタイムで再生されます。
		/// ホールドタイムの単位はms（ミリ秒）です。
		/// ホールドタイムのデフォルト値は0.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にホールドタイムが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeHoldTime(Single holdTimeMs)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeHoldTime(NativeHandle, holdTimeMs);
		}

		/// <summary>エンベロープのディケイタイムの設定</summary>
		/// <param name="decayTimeMs">ディケイタイム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのディケイタイムを設定します。
		/// 本関数でディケイタイムを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたディケイタイムで再生されます。
		/// ディケイタイムの単位はms（ミリ秒）です。
		/// ディケイタイムのデフォルト値は0.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にディケイタイムが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeDecayTime(Single decayTimeMs)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeDecayTime(NativeHandle, decayTimeMs);
		}

		/// <summary>エンベロープのディケイカーブの設定</summary>
		/// <param name="curveType">カーブタイプ</param>
		/// <param name="strength">カーブの強さ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのディケイカーブを設定します。
		/// 本関数でディケイカーブを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたディケイカーブで再生されます。
		/// カーブタイプは <see cref="CriAtomEx.CurveType"/> に定義しているもを指定します。
		/// カーブタイプのデフォルトは <see cref="CriAtomEx.CurveType.Linear"/> です。
		/// カーブの強さは、0.0f～2.0fの範囲で実数値を指定します。
		/// カーブの強さのデフォルト値は1.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にディケイカーブが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeDecayCurve(CriAtomEx.CurveType curveType, Single strength)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeDecayCurve(NativeHandle, curveType, strength);
		}

		/// <summary>エンベロープのリリースタイムの設定</summary>
		/// <param name="releaseTimeMs">リリースタイム</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのリリースタイムを設定します。
		/// 本関数でリリースタイムを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたリリースタイムで再生されます。
		/// リリースタイムの単位はms（ミリ秒）です。
		/// リリースタイムのデフォルト値は0.0fです。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にリリースタイムが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeReleaseTime(Single releaseTimeMs)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeReleaseTime(NativeHandle, releaseTimeMs);
		}

		/// <summary>エンベロープのリリースカーブの設定</summary>
		/// <param name="curveType">カーブタイプ</param>
		/// <param name="strength">カーブの強さ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのリリースカーブを設定します。
		/// 本関数でリリースカーブを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたリリースカーブで再生されます。
		/// カーブタイプは <see cref="CriAtomEx.CurveType"/> に定義しているもを指定します。
		/// カーブタイプのデフォルトは <see cref="CriAtomEx.CurveType.Linear"/> です。
		/// カーブの強さは、0.0f～2.0fの範囲で実数値を指定します。
		/// カーブの強さのデフォルト値は1.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にリリースカーブが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeReleaseCurve(CriAtomEx.CurveType curveType, Single strength)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeReleaseCurve(NativeHandle, curveType, strength);
		}

		/// <summary>エンベロープのサスティンレベルの設定</summary>
		/// <param name="susutainLevel">サスティンレベル（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// エンベロープのサスティンレベルを設定します。
		/// 本関数でサスティンレベルを設定後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、設定されたサスティンレベルで再生されます。
		/// サスティンレベルには、0.0f～1.0fの範囲で実数値を指定します。
		/// サスティンレベルのデフォルト値は1.0fです。
		/// </para>
		/// <para>
		/// 注意:
		/// 再生中に<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数によって更新することはできません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にサスティンレベルが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値を<b>上書き</b>して適用されます（データ側の設定値は無視されます）。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetEnvelopeSustainLevel(Single susutainLevel)
		{
			NativeMethods.criAtomExPlayer_SetEnvelopeSustainLevel(NativeHandle, susutainLevel);
		}

		/// <summary>データ要求コールバック関数の登録</summary>
		/// <param name="func">データ要求コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// データ要求コールバック関数の登録を行います。
		/// データ要求コールバックは、複数の音声データをシームレスに連結して再生する際に
		/// 使用します。
		/// 登録したコールバック関数は、ボイスが内部的に使用している Atom プレーヤーが
		/// 連結再生用のデータを要求するタイミングで実行されます。
		/// （前回のデータを読み込み終えて、次に再生すべきデータを要求するタイミングで
		/// コールバック関数が実行されます。）
		/// 登録したコールバック関数内で <see cref="CriAtomPlayer.SetData"/> 関数等を用いて Atom プレーヤーに
		/// データをセットすると、セットされたデータは現在再生中のデータに続いてシームレスに
		/// 連結されて再生されます。
		/// また、コールバック関数内で <see cref="CriAtomPlayer.SetPreviousDataAgain"/> 関数を実行することで、
		/// 同一データを繰り返し再生し続けることも可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 登録したコールバック関数内でデータを指定しなかった場合、現在のデータを再生し
		/// 終えた時点で、AtomEx プレーヤーのステータスが <see cref="CriAtomExPlayer.Status.Playend"/> に遷移します。
		/// タイミング等の問題により、データを指定することができないが、ステータスを
		/// <see cref="CriAtomExPlayer.Status.Playend"/> に遷移させたくない場合には、コールバック関数内で
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行してください。
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行することで、約1V後に再度データ要求
		/// コールバック関数が呼び出されます。（コールバック処理をリトライ可能。）
		/// ただし、 <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行した場合、再生が途切れる
		/// （連結箇所に一定時間無音が入る）可能性があります。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数は再生開始前に設定する必要があります。
		/// 再生中の音声に対しコールバックを設定したり、
		/// 設定済みのコールバックを後から変更することはできません。
		/// 複数の波形データを含むキューを再生した場合、
		/// 最初に見つかった波形データの再生が終了するタイミングでコールバック関数が実行されます。
		/// そのため、複数の波形データを含むキューに対して連結再生の操作を行った場合、
		/// 意図しない組み合わせで波形が連結再生される可能性があります。
		/// 本機能を使用する際には、 1 つの波形データのみを含むキューを再生するか、
		/// またはファイルやオンメモリデータ等を再生してください。
		/// データ要求コールバック関数内で長時間処理をブロックすると、音切れ等の問題が
		/// 発生しますので、ご注意ください。
		/// コールバック関数内で実行可能なAPIは、以下のとおりです。
		/// - <see cref="CriAtomExAcb.GetWaveformInfoById"/>（引数のnull指定は不可）
		/// - <see cref="CriAtomExAcb.GetWaveformInfoByName"/>（引数のnull指定は不可）
		/// - <see cref="CriAtomExAcb.GetOnMemoryAwbHandle"/>
		/// - <see cref="CriAtomExAcb.GetStreamingAwbHandle"/>
		/// - <see cref="CriAtomPlayer.SetData"/>
		/// - <see cref="CriAtomPlayer.SetFile"/>
		/// - <see cref="CriAtomPlayer.SetContentId"/>
		/// - <see cref="CriAtomPlayer.SetWaveId"/>
		/// - <see cref="CriAtomPlayer.SetPreviousDataAgain"/>
		/// - <see cref="CriAtomPlayer.DeferCallback"/>
		/// コールバック関数内で上記以外のAPIを実行した場合、
		/// エラーコールバックやデッドロック等の問題が発生する可能性があります。
		/// シームレス連結再生をサポートしないコーデックを使用している場合、
		/// データ要求コールバック関数内で次のデータをセットしても、
		/// データは続けて再生されません。
		/// - HCA-MXコーデックを使用する場合、データがシームレスには連結されず、
		/// 再生中の音声と次に再生する音声との継ぎ目に無音が入ります。
		/// - プラットフォーム固有の音声圧縮コーデックを使用している場合、
		/// エラー等が発生する可能性があります。
		/// シームレス連結再生に使用する波形データのフォーマットは、
		/// 全て同じにする必要があります。
		/// 具体的には、以下のパラメーターが同じである必要があります。
		/// - コーデック
		/// - チャンネル数
		/// - サンプリングレート
		/// パラメーターが異なる波形を連結しようとした場合、
		/// 意図しない速度で音声データが再生されたり、
		/// エラーコールバックが発生する等の問題が発生します。
		/// コールバック関数内でループ付きの波形データをセットした場合でも、
		/// ループ再生は行われません。
		/// （ループポイントが無視され、再生が終了します。）
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.DataRequestCbFunc"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetPreviousDataAgain"/>
		/// <seealso cref="CriAtomPlayer.DeferCallback"/>
		public unsafe void SetDataRequestCallback(delegate* unmanaged[Cdecl]<IntPtr, UInt32, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_SetDataRequestCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetDataRequestCallbackInternal(IntPtr func, IntPtr obj) => SetDataRequestCallback((delegate* unmanaged[Cdecl]<IntPtr, UInt32, IntPtr, void>)func, obj);
		CriAtomExPlayer.DataRequestCbFunc _dataRequestCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetDataRequestCallback" />
		public CriAtomExPlayer.DataRequestCbFunc DataRequestCallback => _dataRequestCallback ?? (_dataRequestCallback = new CriAtomExPlayer.DataRequestCbFunc(SetDataRequestCallbackInternal));

		/// <summary>データ要求コールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// 次に再生するデータを指定するためのコールバック関数です。
		/// 複数の音声データをシームレスに連結して再生する際に使用します。
		/// コールバック関数の登録には <see cref="CriAtomExPlayer.SetDataRequestCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、ボイスが内部的に使用している Atom プレーヤーが
		/// 連結再生用のデータを要求するタイミングで実行されます。
		/// （前回のデータを読み込み終えて、次に再生すべきデータを要求するタイミングで
		/// コールバック関数が実行されます。）
		/// コールバック関数内で <see cref="CriAtomPlayer.SetData"/> 関数等を用いて Atom プレーヤーにデータをセットすると、
		/// セットされたデータは現在再生中のデータに続いてシームレスに連結されて再生されます。
		/// また、本関数内で <see cref="CriAtomPlayer.SetPreviousDataAgain"/> 関数を実行することで、
		/// 同一データを繰り返し再生し続けることも可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数内でデータを指定しなかった場合、現在のデータを再生し終えた時点で、
		/// AtomEx プレーヤーのステータスが <see cref="CriAtomExPlayer.Status.Playend"/> に遷移します。
		/// タイミング等の問題により、データを指定することができないが、ステータスを
		/// <see cref="CriAtomExPlayer.Status.Playend"/> に遷移させたくない場合には、コールバック関数内で
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行してください。
		/// <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行することで、約1V後に再度データ要求
		/// コールバック関数が呼び出されます。（コールバック処理をリトライ可能。）
		/// ただし、 <see cref="CriAtomPlayer.DeferCallback"/> 関数を実行した場合、再生が途切れる
		/// （連結箇所に一定時間無音が入る）可能性があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバックの第 3 引数（ player ）は、 AtomEx プレーヤーではなく、
		/// 下位レイヤの Atom プレーヤーです。
		/// （ AtomExPlayerHn にキャストすると、アクセス違反等の重大な不具合が発生します。）
		/// 複数の波形データを含むキューを再生した場合、
		/// 最初に見つかった波形データの再生が終了するタイミングでコールバック関数が実行されます。
		/// そのため、複数の波形データを含むキューに対して連結再生の操作を行った場合、
		/// 意図しない組み合わせで波形が連結再生される可能性があります。
		/// 本機能を使用する際には、 1 つの波形データのみを含むキューを再生するか、
		/// またはファイルやオンメモリデータ等を再生してください。
		/// 現状、コールバックは波形データを再生し始めたボイスに対してのみ割り当てられます。
		/// そのため、波形データ再生後にボイスがバーチャル化した場合、コールバックは実行されません。
		/// （データ終端に到達した時点で、コールバックが実行されずにPLAYEND状態に遷移します。）
		/// 本コールバック関数内で、シームレス連結再生以外の制御を行わないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// コールバック関数内で実行可能なAPIは、以下のとおりです。
		/// - <see cref="CriAtomExAcb.GetWaveformInfoById"/>（引数のnull指定は不可）
		/// - <see cref="CriAtomExAcb.GetWaveformInfoByName"/>（引数のnull指定は不可）
		/// - <see cref="CriAtomExAcb.GetOnMemoryAwbHandle"/>
		/// - <see cref="CriAtomExAcb.GetStreamingAwbHandle"/>
		/// - <see cref="CriAtomPlayer.SetData"/>
		/// - <see cref="CriAtomPlayer.SetFile"/>
		/// - <see cref="CriAtomPlayer.SetContentId"/>
		/// - <see cref="CriAtomPlayer.SetWaveId"/>
		/// - <see cref="CriAtomPlayer.SetPreviousDataAgain"/>
		/// - <see cref="CriAtomPlayer.DeferCallback"/>
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetDataRequestCallback"/>
		/// <seealso cref="CriAtomPlayer.SetData"/>
		/// <seealso cref="CriAtomPlayer.SetPreviousDataAgain"/>
		/// <seealso cref="CriAtomPlayer.DeferCallback"/>
		public unsafe class DataRequestCbFunc : NativeCallbackBase<DataRequestCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>再生ID</summary>
				public UInt32 id { get; }
				/// <summary>Atomプレーヤーオブジェクト</summary>
				public IntPtr player { get; }

				internal Arg(UInt32 id, IntPtr player)
				{
					this.id = id;
					this.player = player;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, UInt32 id, IntPtr player) =>
				InvokeCallbackInternal(obj, new(id, player));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, UInt32 id, IntPtr player);
			static NativeDelegate callbackDelegate = null;
#endif
			internal DataRequestCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, UInt32, IntPtr, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>乱数種の設定</summary>
		/// <param name="seed">乱数種</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーが保持する疑似乱数生成器に乱数種を設定します。
		/// 乱数種を設定することにより、各種ランダム再生処理に再現性を持たせることができます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.SetRandomSeed"/>
		public void SetRandomSeed(UInt32 seed)
		{
			NativeMethods.criAtomExPlayer_SetRandomSeed(NativeHandle, seed);
		}

		/// <summary>DSPパラメーターの設定</summary>
		/// <param name="paramId">パラメーターID（0～10）</param>
		/// <param name="paramVal">パラメーターID（0.0f～1.0f）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーが保持するインサーションDSPのパラメーターを設定します。
		/// DSPを有効化するには、ボイスプールにあらかじめDSPがアタッチされている必要があります。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetDspParameter(Int32 paramId, Single paramVal)
		{
			NativeMethods.criAtomExPlayer_SetDspParameter(NativeHandle, paramId, paramVal);
		}

		/// <summary>DSPパラメーターの設定</summary>
		/// <param name="isBypassed">バイパス有無</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーが保持するインサーションDSPをバイパスするかを設定します。
		/// この関数で明示的に指定しなければ、インサーションDSPはバイパスされません。
		/// </para>
		/// <para>
		/// 備考:
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetDspBypass(NativeBool isBypassed)
		{
			NativeMethods.criAtomExPlayer_SetDspBypass(NativeHandle, isBypassed);
		}

		/// <summary>プレーヤーにAISACを取り付ける</summary>
		/// <param name="globalAisacName">取り付けるグローバルAISAC名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーにAISACをアタッチ（取り付け）します。
		/// AISACをアタッチすることにより、キューやトラックにAISACを設定していなくても、AISACの効果を得ることができます。
		/// 本関数でAISACをアタッチ後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、アタッチしたAISACを考慮して、各種パラメーターが適用されます。
		/// またアタッチ後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対しても、アタッチしたAISACによる各種パラメーター設定を適用することができます。
		/// AISACのアタッチに失敗した場合、関数内でエラーコールバックが発生します。
		/// AISACのアタッチに失敗した理由については、エラーコールバックのメッセージを確認してください。
		/// </para>
		/// <para>
		/// 備考:
		/// 全体設定（ACFファイル）に含まれるグローバルAISACのみ、アタッチ可能です。
		/// AISACの効果を得るには、キューやトラックに設定されているAISACと同様に、該当するAISACコントロール値を設定する必要があります。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// <para>
		/// 注意:
		/// キューやトラックに「AISACコントロール値を変更するAISAC」が設定されていたとしても、その適用結果のAISACコントロール値は、プレーヤーにアタッチしたAISACには影響しません。
		/// 現在、「オートモジュレーション」や「ランダム」といったコントロールタイプのAISACのアタッチには対応しておりません。
		/// 現在、プレーヤーにアタッチできるAISACの最大数は、8個固定です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.DetachAisac"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void AttachAisac(ArgString globalAisacName)
		{
			NativeMethods.criAtomExPlayer_AttachAisac(NativeHandle, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>プレーヤーからAISACを取り外す</summary>
		/// <param name="globalAisacName">取り外すグローバルAISAC名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーからAISACをデタッチ（取り外し）します。
		/// 本関数でAISACをデタッチ後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、デタッチしたAISACの影響は受けなくなります。
		/// またデタッチ後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対しても、デタッチしたAISACによる影響を受けなくなります。
		/// AISACのデタッチに失敗した場合、関数内でエラーコールバックが発生します。
		/// AISACのデタッチに失敗した理由については、エラーコールバックのメッセージを確認してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachAisac"/>
		public void DetachAisac(ArgString globalAisacName)
		{
			NativeMethods.criAtomExPlayer_DetachAisac(NativeHandle, globalAisacName.GetPointer(stackalloc byte[globalAisacName.BufferSize]));
		}

		/// <summary>プレーヤーから全てのAISACを取り外す</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーから全てのAISACをデタッチ（取り外し）します。
		/// 本関数でAISACをデタッチ後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、デタッチしたAISACの影響は受けなくなります。
		/// またデタッチ後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対しても、デタッチしたAISACによる影響を受けなくなります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachAisac"/>
		public void DetachAisacAll()
		{
			NativeMethods.criAtomExPlayer_DetachAisacAll(NativeHandle);
		}

		/// <summary>プレーヤーにアタッチされているAISAC数を取得する</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーにアタッチされているAISAC数を取得します。
		/// </para>
		/// </remarks>
		public Int32 GetNumAttachedAisacs()
		{
			return NativeMethods.criAtomExPlayer_GetNumAttachedAisacs(NativeHandle);
		}

		/// <summary>プレーヤーにアタッチされているAISACの情報を取得する</summary>
		/// <param name="aisacAttachedIndex">アタッチされているAISACのインデックス</param>
		/// <param name="aisacInfo">AISAC情報</param>
		/// <returns>= 情報が取得できた</returns>
		/// <returns>= 情報が取得できなかった</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーにアタッチされているAISACの情報を取得します。
		/// 無効なインデックスを指定した場合、falseが返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.GetNumAttachedAisacs"/>
		public unsafe bool GetAttachedAisacInfo(Int32 aisacAttachedIndex, out CriAtomEx.AisacInfo aisacInfo)
		{
			fixed (CriAtomEx.AisacInfo* aisacInfoPtr = &aisacInfo)
				return NativeMethods.criAtomExPlayer_GetAttachedAisacInfo(NativeHandle, aisacAttachedIndex, aisacInfoPtr);
		}

		/// <summary>プレーヤーにストリーミングキャッシュを設定します</summary>
		/// <param name="cacheId">プレーヤーで使用するストリーミングキャッシュID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーで使用するストリーミングキャッシュをID指定で設定します。
		/// </para>
		/// <para>
		/// 注意:
		/// プレーヤーで使用中のストリーミングキャッシュを破棄する場合は、
		/// 先にプレーヤーを破棄してください。
		/// 逆の順序で処理した場合の結果は不定です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		/// <seealso cref="CriAtomStreamingCache.Dispose"/>
		public void SetStreamingCacheId(CriAtomExStreamingCache cacheId)
		{
			NativeMethods.criAtomExPlayer_SetStreamingCacheId(NativeHandle, cacheId.NativeHandle);
		}

		/// <summary>プレーヤーにトゥイーンを取り付ける</summary>
		/// <param name="tween">トゥイーンオブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーにトゥイーンをアタッチ（取り付け）します。
		/// トゥイーンをアタッチすることにより、簡単な手順でパラメーターの時間変化を行うことができます。
		/// 本関数でトゥイーンをアタッチ後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、アタッチしたトゥイーンを考慮して、各種パラメーターが適用されます。
		/// またアタッチ後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対しても、アタッチしたトゥイーンによる各種パラメーター設定を適用することができます。
		/// </para>
		/// <para>
		/// 備考:
		/// トゥイーンによって変化したパラメーターは、AtomExプレーヤーに設定されているパラメーターに対し、加算／乗算／上書きされます。
		/// 加算／乗算／上書きのどれに該当するかは、AtomExプレーヤーへの設定関数（<see cref="CriAtomExPlayer.SetVolume"/> 関数等）と同様です。
		/// 例えば、ボリュームであれば乗算され、AISACコントロール値であれば上書きします。
		/// 現在、プレーヤーにアタッチできるトゥイーンの最大数は、8個固定です。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.DetachTween"/>
		/// <seealso cref="CriAtomExPlayer.DetachTweenAll"/>
		/// <seealso cref="CriAtomExTween.CriAtomExTween"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void AttachTween(CriAtomExTween tween)
		{
			NativeMethods.criAtomExPlayer_AttachTween(NativeHandle, tween?.NativeHandle ?? default);
		}

		/// <summary>プレーヤーからトゥイーンを取り外す</summary>
		/// <param name="tween">取り外すトゥイーンオブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーからトゥイーンをデタッチ（取り外し）します。
		/// 本関数でトゥイーンをデタッチ後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、デタッチしたトゥイーンの影響は受けなくなります。
		/// またデタッチ後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対しても、デタッチしたトゥイーンによる影響を受けなくなります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachTween"/>
		public void DetachTween(CriAtomExTween tween)
		{
			NativeMethods.criAtomExPlayer_DetachTween(NativeHandle, tween?.NativeHandle ?? default);
		}

		/// <summary>プレーヤーから全てのトゥイーンを取り外す</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーから全てのトゥイーンをデタッチ（取り外し）します。
		/// 本関数でトゥイーンをデタッチ後、<see cref="CriAtomExPlayer.Start"/> 関数により再生開始すると、デタッチしたトゥイーンの影響は受けなくなります。
		/// またデタッチ後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生された音声に対しても、デタッチしたトゥイーンによる影響を受けなくなります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachTween"/>
		public void DetachTweenAll()
		{
			NativeMethods.criAtomExPlayer_DetachTweenAll(NativeHandle);
		}

		/// <summary>再生開始ブロックのセット（ブロックインデックス指定）</summary>
		/// <param name="index">ブロックインデックス</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生開始ブロックインデックスを、AtomExプレーヤーに関連付けます。
		/// 本関数で再生開始ブロックインデックスを指定後、ブロックシーケンスキューを
		/// <see cref="CriAtomExPlayer.Start"/> 関数で再生開始すると指定したブロックから再生を
		/// 開始します。
		/// </para>
		/// <para>
		/// 備考:
		/// AtomExプレーヤーのデフォルトブロックインデックスは 0 です。
		/// <see cref="CriAtomExPlayer.Start"/> 関数による再生開始時にプレーヤーに設定されているキューが
		/// ブロックシーケンスでない場合は、本関数で設定した値は利用されません。
		/// 指定したインデックスに対応したブロックがない場合は先頭ブロックから再生が行われます。
		/// この際、指定インデックスのブロックが存在しない内容のワーニングが発生します。
		/// </para>
		/// <para>
		/// 備考:
		/// 再生開始後のブロック遷移は <see cref="CriAtomExPlayback.SetNextBlockIndex"/> 関数を使用して行い、
		/// 再生中のブロックインデックス取得は <see cref="CriAtomExPlayback.GetCurrentBlockIndex"/> 関数を使用します。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayback.SetNextBlockIndex"/>
		/// <seealso cref="CriAtomExPlayback.GetCurrentBlockIndex"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetFirstBlockIndex(Int32 index)
		{
			NativeMethods.criAtomExPlayer_SetFirstBlockIndex(NativeHandle, index);
		}

		/// <summary>ブロックトランジションコールバック関数の登録</summary>
		/// <param name="func">ブロックトランジションコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ブロックシーケンス再生時にブロックトランジションが発生したときに呼び出されるコールバック関数を登録します。
		/// 登録されたコールバック関数は、ブロックトランジションが発生すると呼び出されます。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数の登録は、停止中のプレーヤーに対してのみ可能です。
		/// 再生中のプレーヤーに対してコールバックを登録することはできません。
		/// （エラーコールバックが発生し、登録に失敗します。）
		/// コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// コールバック関数内で長時間処理をブロックすると、音切れ等の問題
		/// が発生しますので、ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.BlockTransitionCbFunc"/>
		public unsafe void SetBlockTransitionCallback(delegate* unmanaged[Cdecl]<IntPtr, UInt32, Int32, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_SetBlockTransitionCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetBlockTransitionCallbackInternal(IntPtr func, IntPtr obj) => SetBlockTransitionCallback((delegate* unmanaged[Cdecl]<IntPtr, UInt32, Int32, void>)func, obj);
		CriAtomExPlayer.BlockTransitionCbFunc _blockTransitionCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetBlockTransitionCallback" />
		public CriAtomExPlayer.BlockTransitionCbFunc BlockTransitionCallback => _blockTransitionCallback ?? (_blockTransitionCallback = new CriAtomExPlayer.BlockTransitionCbFunc(SetBlockTransitionCallbackInternal));

		/// <summary>ブロックトランジションコールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// ブロックシーケンス再生時にブロックトランジションが発生したときに呼び出されるコールバック関数です。
		/// コールバック関数の登録には <see cref="CriAtomExPlayer.SetBlockTransitionCallback"/> 関数を使用します。
		/// コールバック関数を登録すると、ブロックトランジションが発生する度に、
		/// コールバック関数が実行されるようになります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetBlockTransitionCallback"/>
		public unsafe class BlockTransitionCbFunc : NativeCallbackBase<BlockTransitionCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>再生ID</summary>
				public UInt32 id { get; }
				/// <summary>キュー内のブロックインデックス値</summary>
				public Int32 index { get; }

				internal Arg(UInt32 id, Int32 index)
				{
					this.id = id;
					this.index = index;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, UInt32 id, Int32 index) =>
				InvokeCallbackInternal(obj, new(id, index));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, UInt32 id, Int32 index);
			static NativeDelegate callbackDelegate = null;
#endif
			internal BlockTransitionCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, UInt32, Int32, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ドライセンドレベルの設定（CRI Audio互換用）</summary>
		/// <param name="spk">スピーカーID</param>
		/// <param name="offset">ドライセンドレベルオフセット（加算値）</param>
		/// <param name="gain">ドライセンドレベルゲイン（乗算値）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 出力音声のドライセンドレベルを設定します。
		/// 本関数はCRI Audioとの互換用であり、CRI Audioにあったドライセンドレベルと同じ挙動をします。
		/// 本関数でドライセンドレベルを設定後、<see cref="CriAtomExPlayer.Start"/> 関数で再生を開始すると、
		/// 設定されたドライセンドレベルで音声が再生されます。
		/// またドライセンドレベル設定後に <see cref="CriAtomExPlayer.Update"/> 関数や <see cref="CriAtomExPlayer.UpdateAll"/>
		/// 関数を呼び出すことで、すでに再生された音声のドライセンドレベルを更新することも可能です。
		/// ドライセンドレベルでは、再生時の各スピーカーへの出力レベルを個別に指定することができます。
		/// 各スピーカーへの出力としてどの入力チャンネルを使用するかは、波形のチャンネル数に依存します。
		/// 例えばモノラル波形の場合は全てのスピーカーへの出力として0チャンネルを入力として使用し、
		/// ステレオ波形の場合はL側のスピーカー（L,SL,SBL）への出力には0チャンネル（Lチャンネル）、
		/// R側のスピーカー（R,SR,SBL）への出力には1チャンネル（Rチャンネル）を入力として使用します。
		/// （ドライセンドレベルの設定では、ステレオの音はセンタースピーカー、LFEへは出力できません。）
		/// ドライセンドレベルは、パン3Dやセンドレベルの設定による出力レベルに対して加算されます。
		/// ドライセンドレベル値の範囲や扱いは、基本的にはボリュームと同等です。<see cref="CriAtomExPlayer.SetVolume"/> 関数を参照してください。
		/// ドライセンドレベルのデフォルト値は0.0fです。
		/// </para>
		/// <para>
		/// 備考:
		/// キュー再生時、データ側にドライセンドレベルが設定されている場合に本関数を呼び出すと、
		/// データ側に設定されている値に対し gain を乗算し、 offset を加算した値が適用されます。
		/// 例えば、データ側のドライセンドレベルが1.0f、AtomExプレーヤーのドライセンドレベルが offset 0.2f、gain 0.5f
		/// の場合、実際に適用されるセンドレベルは0.7fになります。
		/// ドライセンドレベルは通常ではCRI Atom Craftでは設定できず、CRI Audio Craftで作成した
		/// プロジェクトファイルをインポートした場合にのみ、データ側に設定されている場合があります。
		/// 通常では6ch素材を再生した際、自動的にセンター／LFEから出力されますが、
		/// データ側または本関数でドライセンドレベルが設定された場合、自動では出力されなくなります。
		/// また同様に、データ側または本関数でドライセンドレベルが設定された場合、CRI Atom Craftで設定したセンター／LFEミックスレベルは無効となります。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.Start"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetDrySendLevel(CriAtomEx.SpeakerId spk, Single offset, Single gain)
		{
			NativeMethods.criAtomExPlayer_SetDrySendLevel(NativeHandle, spk, offset, gain);
		}

		/// <summary>セレクター情報のプレーヤーへの設定</summary>
		/// <param name="selector">セレクター名</param>
		/// <param name="label">ラベル名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// セレクター名とラベル名を指定して、プレーヤーに設定します。
		/// トラックにセレクターラベルが指定されているキューを再生した場合、本関数で指定したセレクターラベル
		/// と一致したトラックだけを再生します。
		/// セレクター名、ラベル名はACFヘッダーに記載されています。
		/// プレーヤーに設定したラベル情報の個別削除は、 <see cref="CriAtomExPlayer.UnsetSelectorLabel"/> 関数を実行してください。
		/// プレーヤーに設定したラベル情報の一括削除は、 <see cref="CriAtomExPlayer.ClearSelectorLabels"/> 関数を実行してください。
		/// ラベル情報を含む全てのプレーヤー設定値削除は、 <see cref="CriAtomExPlayer.ResetParameters"/> 関数を実行してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.ClearSelectorLabels"/>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		public void SetSelectorLabel(ArgString selector, ArgString label)
		{
			NativeMethods.criAtomExPlayer_SetSelectorLabel(NativeHandle, selector.GetPointer(stackalloc byte[selector.BufferSize]), label.GetPointer(stackalloc byte[label.BufferSize]));
		}

		/// <summary>プレーヤーに設定されているセレクター情報の削除</summary>
		/// <param name="selector">セレクター名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに設定されている指定されたセレクター名とそれに紐づくラベル名の情報を削除します。
		/// また削除後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生中の音声に対してセレクター情報の削除が行えますが、再生中音声が停止することはありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetSelectorLabel"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		public void UnsetSelectorLabel(ArgString selector)
		{
			NativeMethods.criAtomExPlayer_UnsetSelectorLabel(NativeHandle, selector.GetPointer(stackalloc byte[selector.BufferSize]));
		}

		/// <summary>プレーヤーに設定されている全てのセレクター情報の削除</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーに設定されているセレクター名、ラベル名情報を全て削除します。
		/// また削除後、<see cref="CriAtomExPlayer.Update"/> 関数、<see cref="CriAtomExPlayer.UpdateAll"/> 関数を呼び出すことにより、
		/// すでに再生中の音声に対してセレクター情報の削除が行えますが、再生中音声が停止することはありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetSelectorLabel"/>
		/// <seealso cref="CriAtomExPlayer.Update"/>
		/// <seealso cref="CriAtomExPlayer.UpdateAll"/>
		public void ClearSelectorLabels()
		{
			NativeMethods.criAtomExPlayer_ClearSelectorLabels(NativeHandle);
		}

		/// <summary>再生トラック番号通知コールバック関数の登録</summary>
		/// <param name="func">再生トラック番号通知コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生したトラック番号を通知するためのコールバック関数を登録します。
		/// 登録されたコールバック関数は、ポリフォニックタイプ以外のキュー再生時に呼び出されます。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数の登録は、停止中のプレーヤーに対してのみ可能です。
		/// 再生中のプレーヤーに対してコールバックを登録することはできません。
		/// （エラーコールバックが発生し、登録に失敗します。）
		/// コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// コールバック関数内で長時間処理をブロックすると、音切れ等の問題
		/// が発生しますので、ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.PlaybackTrackInfoNotificationCbFunc"/>
		public unsafe void SetPlaybackTrackInfoNotificationCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.TrackInfo*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_SetPlaybackTrackInfoNotificationCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetPlaybackTrackInfoNotificationCallbackInternal(IntPtr func, IntPtr obj) => SetPlaybackTrackInfoNotificationCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.TrackInfo*, void>)func, obj);
		CriAtomExPlayer.PlaybackTrackInfoNotificationCbFunc _playbackTrackInfoNotificationCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetPlaybackTrackInfoNotificationCallback" />
		public CriAtomExPlayer.PlaybackTrackInfoNotificationCbFunc PlaybackTrackInfoNotificationCallback => _playbackTrackInfoNotificationCallback ?? (_playbackTrackInfoNotificationCallback = new CriAtomExPlayer.PlaybackTrackInfoNotificationCbFunc(SetPlaybackTrackInfoNotificationCallbackInternal));

		/// <summary>再生トラック情報取得コールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// ポリフォニックタイプ、トラック遷移タイプ以外のキュー再生時に再生したトラック情報を通知するコールバック関数です。
		/// コールバック関数の登録には <see cref="CriAtomExPlayer.SetPlaybackTrackInfoNotificationCallback"/> 関数を使用します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本コールバック関数内で、AtomライブラリのAPIを実行しないでください。
		/// コールバック関数はAtomライブラリ内のサーバー処理から実行されます。
		/// そのため、サーバー処理への割り込みを考慮しないAPIを実行した場合、
		/// エラーが発生したり、デッドロックが発生する可能性があります。
		/// コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetPlaybackTrackInfoNotificationCallback"/>
		public unsafe class PlaybackTrackInfoNotificationCbFunc : NativeCallbackBase<PlaybackTrackInfoNotificationCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>再生トラック情報</summary>
				public NativeReference<CriAtomExPlayback.TrackInfo> info { get; }

				internal Arg(NativeReference<CriAtomExPlayback.TrackInfo> info)
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
			static void CallbackFunc(IntPtr obj, CriAtomExPlayback.TrackInfo* info) =>
				InvokeCallbackInternal(obj, new(info));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, CriAtomExPlayback.TrackInfo* info);
			static NativeDelegate callbackDelegate = null;
#endif
			internal PlaybackTrackInfoNotificationCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.TrackInfo*, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>再生イベントコールバックの登録</summary>
		/// <param name="func">再生イベントコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 再生イベントコールバックを登録します。
		/// 本関数を使用して再生イベントコールバックを登録することで、
		/// 再生イベント（再生用リソースの確保／解放や、ボイスの割り当て、バーチャル化）発生時の詳細情報
		/// （再生元のAtomExプレーヤーや再生ID）が取得可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomExPlayback.EventCbFunc"/> の説明をご参照ください。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// <para>
		/// 注意:
		/// 1つのAtomExプレーヤーに対し、1つのコールバック関数しか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.VoiceEventCbFunc"/>
		public unsafe void SetPlaybackEventCallback(delegate* unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.Event, CriAtomExPlayback.InfoDetail*, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExPlayer_SetPlaybackEventCallback(NativeHandle, (IntPtr)func, obj);
		}
		unsafe void SetPlaybackEventCallbackInternal(IntPtr func, IntPtr obj) => SetPlaybackEventCallback((delegate* unmanaged[Cdecl]<IntPtr, CriAtomExPlayback.Event, CriAtomExPlayback.InfoDetail*, void>)func, obj);
		CriAtomExPlayback.EventCbFunc _playbackEventCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetPlaybackEventCallback" />
		public CriAtomExPlayback.EventCbFunc PlaybackEventCallback => _playbackEventCallback ?? (_playbackEventCallback = new CriAtomExPlayback.EventCbFunc(SetPlaybackEventCallbackInternal));

		/// <summary>入力音声のチャンネルコンフィグ指定</summary>
		/// <param name="numChannels">チャンネル数</param>
		/// <param name="channelConfig">チャンネルコンフィグ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 入力音声のチャンネルコンフィグを指定します。
		/// 本関数実行後に指定したチャンネル数の音声を再生した場合、当該音声の各チャンネルの属性は指定したチャンネルコンフィグに基づいて判断されます。
		/// </para>
		/// <para>
		/// 備考:
		/// デフォルト値は::criAtom_ChangeDefaultChannelConfig 関数にて変更可能です。
		/// 本パラメーターは <see cref="CriAtomExPlayer.ResetParameters"/> 関数にてクリアされます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.ResetParameters"/>
		public void SetChannelConfig(Int32 numChannels, CriAtom.ChannelConfig channelConfig)
		{
			NativeMethods.criAtomExPlayer_SetChannelConfig(NativeHandle, numChannels, channelConfig);
		}

		/// <summary>フェーダーのアタッチに必要なワーク領域サイズの計算</summary>
		/// <param name="config">フェーダーアタッチ用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AtomExプレーヤーにフェーダーをアタッチするのに必要な、ワーク領域のサイズを取得します。
		/// アロケーターを登録せずにフェーダーをアタッチする場合、あらかじめ本関数で計算した
		/// ワーク領域サイズ分のメモリをワーク領域として <see cref="CriAtomExPlayer.AttachFader"/> 関数に
		/// セットする必要があります。
		/// 引数にnullを指定した場合、デフォルト設定
		/// （ <see cref="CriAtomExFader.SetDefaultConfig"/> 適用時と同じパラメーター）で
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
		/// <seealso cref="CriAtomExFader.Config"/>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		public static unsafe Int32 CalculateWorkSizeForFader(in CriAtomExFader.Config config)
		{
			fixed (CriAtomExFader.Config* configPtr = &config)
				return NativeMethods.criAtomExPlayer_CalculateWorkSizeForFader(configPtr);
		}

		/// <summary>プレーヤーにフェーダーを取り付ける</summary>
		/// <param name="config">フェーダーアタッチ用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーにフェーダーをアタッチ（取り付け）し、
		/// AtomExPlayerをクロスフェード専用のプレーヤーに変化させます。
		/// （複数音の同時再生等、従来のAtomExPlayerの持つ機能が一部利用できなくなります。）
		/// 本関数でフェーダーをアタッチしたプレーヤーは、以降音声再生開始毎
		/// （ <see cref="CriAtomExPlayer.Start"/> 関数や <see cref="CriAtomExPlayer.Prepare"/> 関数を実行する毎）に、
		/// 以下の制御を行います。
		/// - 既にフェードアウト中の音があれば強制停止。
		/// - 現在再生中（またはフェードイン中）の音声をフェードアウト。
		/// - 新規に再生を開始する音声をフェードイン。
		/// また、再生停止時（ <see cref="CriAtomExPlayer.Stop"/> 関数実行時）には、
		/// 以下の制御を行います。
		/// - 既にフェードアウト中の音があれば強制停止。
		/// - 現在再生中（またはフェードイン中）の音声をフェードアウト。
		/// プレーヤーにフェーダーを取り付ける際には、ワーク領域としてメモリを渡す必要があります。
		/// 必要なメモリのサイズは、 <see cref="CriAtomExPlayer.CalculateWorkSizeForFader"/>
		/// 関数で計算します。
		/// （<see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。）
		/// フェーダーのアタッチに失敗した場合、関数内でエラーコールバックが発生します。
		/// フェーダーのアタッチに失敗した理由については、エラーコールバックのメッセージを確認してください。
		/// </para>
		/// <para>
		/// 備考:
		/// フェーダーをアタッチするプレーヤーが音声再生中の場合、本関数を実行したタイミングで
		/// プレーヤーが再生中の音声は全て停止されます。
		/// フェーダーは、アタッチ中のプレーヤーに対して <see cref="CriAtomExPlayer.Start"/>
		/// 関数や、 <see cref="CriAtomExPlayer.Stop"/> 関数が実行される度、
		/// 当該プレーヤーで再生中の音声に対して以下の制御を行います。
		/// -# 既にフェードアウト中の音声が存在する場合、その音声を即座に停止する。
		/// -# フェードイン中の音声（または再生中の音声）が存在する場合、
		/// その音声をその時点の音量から <see cref="CriAtomExPlayer.SetFadeOutTime"/>
		/// 関数で指定された時間をかけてフェードアウトさせる。
		/// -# <see cref="CriAtomExPlayer.Start"/> 関数が実行された場合、
		/// プレーヤーにセットされている音声データをボリューム0で再生開始し、
		/// <see cref="CriAtomExPlayer.SetFadeInTime"/> 関数で指定された時間をかけてフェードインさせる。
		/// （ <see cref="CriAtomExPlayer.Start"/> 関数の代わりに <see cref="CriAtomExPlayer.Prepare"/>
		/// 関数を使用した場合、ポーズを解除する時点で上記の制御が行われます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、AtomExPlayerに対する再生／停止操作が大きく変更されます。
		/// （フェーダーアタッチ前後で挙動が大きく変わります。）
		/// 具体的には、同時に発音可能な音声の数が1音（クロスフェード中のみ2音）に限定され、
		/// <see cref="CriAtomExPlayback"/> を用いた制御も行えなくなります。
		/// 本関数は、クロスフェード処理を行いたい場合にのみ必要となります。
		/// 1音だけのフェードイン／アウトについては、エンベロープやTweenをご利用ください。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをフェーダーデタッチ時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// フェーダーの動作仕様の都合上、フェードイン／アウトの処理対象となるのは、
		/// 過去2回の音声再生のみです。
		/// それ以前に再生された音声は、 <see cref="CriAtomExPlayer.Start"/> 関数や
		/// <see cref="CriAtomExPlayer.Stop"/> 関数が実行された時点で強制的に停止されます。
		/// 強制停止処理のタイミングで意図しないノイズが発生する恐れがありますので、
		/// 同時再生数が3音以上にならないよう注意してください。
		/// （ <see cref="CriAtomExPlayer.GetNumPlaybacks"/> 関数で同時再生数を確認してください。）
		/// フェードイン／アウトが機能するのは『AtomExプレーヤーに対する操作』のみです。
		/// <see cref="CriAtomExPlayer.Start"/> 関数実行時に取得した再生IDに対し、
		/// <see cref="CriAtomExPlayback.Stop"/> を実行しても、フェードアウトは行われません。
		/// （フェーダーの設定が無視され、即座に停止処理が行われます。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExFader.Config"/>
		/// <seealso cref="CriAtomExPlayer.CalculateWorkSizeForFader"/>
		public unsafe void AttachFader(in CriAtomExFader.Config config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomExFader.Config* configPtr = &config)
				NativeMethods.criAtomExPlayer_AttachFader(NativeHandle, configPtr, work, workSize);
		}

		/// <summary>フェードイン時間の設定</summary>
		/// <param name="ms">フェードイン時間（ミリ秒指定）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェーダーをアタッチ済みのプレーヤーに対し、フェードイン時間を指定します。
		/// 次回音声再生時（ <see cref="CriAtomExPlayer.Start"/> 関数実行時）には、本関数で設定された
		/// 時間で新規に音声がフェードイン再生されます。
		/// フェードイン時間のデフォルト値は 0 秒です。
		/// そのため、本関数を使用しない場合フェードインは行われず、即座にフルボリューム
		/// で音声の再生が開始されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、 <see cref="CriAtomExPlayer.AttachFader"/> 関数を使用して
		/// あらかじめプレーヤーにフェーダーをアタッチしておく必要があります。
		/// 本関数で設定した値は、既に再生中の音声には一切影響しません。
		/// 本関数で設定したフェード時間は、本関数実行後に <see cref="CriAtomExPlayer.Start"/> 関数を
		/// 実行するタイミングで適用されます。
		/// （既にフェードインを開始している音声に対しては、
		/// 本関数で後からフェードイン時間を変更することはできません。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		/// <seealso cref="CriAtomExPlayer.SetFadeInTime"/>
		public void SetFadeInTime(Int32 ms)
		{
			NativeMethods.criAtomExPlayer_SetFadeInTime(NativeHandle, ms);
		}

		/// <summary>フェードアウト時間の設定</summary>
		/// <param name="ms">フェードアウト時間（ミリ秒指定）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェーダーをアタッチ済みのプレーヤーに対し、フェードアウト時間を指定します。
		/// 次回音声再生時（ <see cref="CriAtomExPlayer.Start"/> 関数実行時）には、本関数で設定された
		/// 時間で再生中の音声がフェードアウトします。
		/// フェードアウト時間のデフォルト値は 500 ミリ秒です。
		/// </para>
		/// <para>
		/// 備考:
		/// フェードアウト時間が設定されている場合、AtomExプレーヤーは以下の順序で再生を停止します。
		/// -# 指定された時間で音声のボリュームを 0 まで落とす。
		/// -# ボリュームが 0 の状態でディレイ時間が経過するまで再生を続ける。
		/// -# ディレイ時間経過後に再生を停止する。
		/// フェードアウト時のボリュームコントロールは、音声再生停止前に行われます。
		/// そのため、波形データにあらかじめ設定されたエンベロープのリリース時間は無視されます。
		/// （厳密には、ボリュームが 0 になってからエンベロープのリリース処理が適用されます。）
		/// 第2引数（ ms ）に 0 を指定する場合と、 <see cref="CriAtomEx.IgnoreFadeOut"/>
		/// を指定する場合とでは、以下のように挙動が異なります。
		/// - 0指定時：即座にボリュームが 0 に落とされ、停止処理が行われる。
		/// - <see cref="CriAtomEx.IgnoreFadeOut"/>指定時：ボリューム変更は行われず、停止処理が行われる。
		/// 再生停止時にフェードアウト処理を行わず、波形にあらかじめ設定されている
		/// エンベロープのリリース処理を有効にしたい場合、第2引数（ ms ）に、
		/// <see cref="CriAtomEx.IgnoreFadeOut"/> を指定してください。
		/// <see cref="CriAtomEx.IgnoreFadeOut"/> を指定することで、
		/// フェードアウト処理によるボリューム制御が行われなくなるため、
		/// <see cref="CriAtomExPlayer.Stop"/> 関数実行後、ディレイ時間経過後に通常の停止処理が行われます。
		/// （波形データにエンベロープのリリースが設定されている場合、リリース処理が行われます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、 <see cref="CriAtomExPlayer.AttachFader"/> 関数を使用して
		/// あらかじめプレーヤーにフェーダーをアタッチしておく必要があります。
		/// 本関数で設定した値は、既に再生中の音声には一切影響しません。
		/// 本関数で設定したフェード時間は、本関数実行後に <see cref="CriAtomExPlayer.Start"/> 関数や
		/// <see cref="CriAtomExPlayer.Stop"/> 関数を実行するタイミングで適用されます。
		/// （既にフェードアウトを開始している音声に対しては、
		/// 本関数で後からフェードアウト時間を変更することはできません。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		/// <seealso cref="CriAtomExPlayer.SetFadeInTime"/>
		public void SetFadeOutTime(Int32 ms)
		{
			NativeMethods.criAtomExPlayer_SetFadeOutTime(NativeHandle, ms);
		}

		/// <summary>プレーヤーからフェーダーを取り外す</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// プレーヤーからフェーダーをデタッチ（取り外し）します。
		/// 本関数でフェーダーをデタッチしたプレーヤーには、以降フェードイン／アウトの処理が行われなくなります。
		/// </para>
		/// <para>
		/// 備考:
		/// フェーダーをデタッチするプレーヤーが音声再生中の場合、本関数を実行したタイミングで
		/// プレーヤーが再生中の音声は全て停止されます。
		/// 本関数を実行せずにプレーヤーを破棄した場合、プレーヤー破棄時（ <see cref="CriAtomExPlayer.Dispose"/> 関数実行時）
		/// にライブラリ内でフェーダーのデタッチが行われます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		public void DetachFader()
		{
			NativeMethods.criAtomExPlayer_DetachFader(NativeHandle);
		}

		/// <summary>フェードアウト時間の取得</summary>
		/// <returns>フェードアウト時間（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェードアウト時間を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は <see cref="CriAtomExPlayer.SetFadeOutTime"/> 関数でセットした値を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFadeOutTime"/>
		public Int32 GetFadeOutTime()
		{
			return NativeMethods.criAtomExPlayer_GetFadeOutTime(NativeHandle);
		}

		/// <summary>フェードイン時間の取得</summary>
		/// <returns>フェードイン時間（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェードイン時間を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は <see cref="CriAtomExPlayer.SetFadeInTime"/> 関数でセットした値を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFadeInTime"/>
		public Int32 GetFadeInTime()
		{
			return NativeMethods.criAtomExPlayer_GetFadeInTime(NativeHandle);
		}

		/// <summary>フェードイン開始オフセットの設定</summary>
		/// <param name="ms">フェードイン開始オフセット（ミリ秒指定）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェーダーをアタッチ済みのプレーヤーに対し、フェードイン開始オフセットを指定します。
		/// 本関数を使用することで、フェードインを開始するタイミングをフェードアウトに対して
		/// 任意の時間早めたり、遅らせることが可能です。
		/// 例えば、フェードアウト時間を5秒、フェードイン開始オフセットを5秒に設定した場合、
		/// フェードアウトが5秒で完了した直後に次の音声をフェードインさせることが可能です。
		/// 逆に、フェードイン時間を5秒、フェードイン開始オフセットを-5秒に設定した場合、
		/// フェードインが5秒で完了した直後に再生中の音のフェードアウトを開始させることが可能です。
		/// フェードイン開始オフセットのデフォルト値は 0 秒です。
		/// （フェードインとフェードアウトが同時に開始されます。）
		/// </para>
		/// <para>
		/// 備考:
		/// フェードイン開始のタイミングは、フェードインする音声の再生準備が整ったタイミングです。
		/// そのため、フェードイン開始オフセットが 0 秒に設定されている場合でも、フェードイン音声
		/// のバッファリングに時間がかかる場合（ストリーム再生時等）には、フェードアウトの開始までに
		/// しばらく時間がかかります。
		/// （本パラメーターは、フェードインとフェードアウトのタイミングを調整するための相対値です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、 <see cref="CriAtomExPlayer.AttachFader"/> 関数を使用して
		/// あらかじめプレーヤーにフェーダーをアタッチしておく必要があります。
		/// 本関数で設定した値は、既に再生中の音声には一切影響しません。
		/// 本関数で設定したフェード時間は、本関数実行後に <see cref="CriAtomExPlayer.Start"/> 関数や
		/// <see cref="CriAtomExPlayer.Stop"/> 関数を実行するタイミングで適用されます。
		/// （既にフェード処理を開始している音声に対しては、
		/// 本関数で後からフェード処理のタイミングを変更することはできません。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		/// <seealso cref="CriAtomExPlayer.SetFadeInTime"/>
		public void SetFadeInStartOffset(Int32 ms)
		{
			NativeMethods.criAtomExPlayer_SetFadeInStartOffset(NativeHandle, ms);
		}

		/// <summary>フェードイン開始オフセットの取得</summary>
		/// <returns>フェードイン開始オフセット（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェードイン開始オフセットを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は <see cref="CriAtomExPlayer.SetFadeInStartOffset"/> 関数でセットした値を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFadeInStartOffset"/>
		public Int32 GetFadeInStartOffset()
		{
			return NativeMethods.criAtomExPlayer_GetFadeInStartOffset(NativeHandle);
		}

		/// <summary>フェードアウト後のディレイ時間の設定</summary>
		/// <param name="ms">フェードイン開始オフセット（ミリ秒指定）</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェードアウト完了後、ボイスを破棄するまでのディレイ時間を設定します。
		/// 本関数を使用することで、フェードアウトを終えたボイスが破棄されるまでのタイミングを任意に設定可能です。
		/// ディレイ時間のデフォルト値は 500 ミリ秒です。
		/// （フェードアウト音を再生するボイスは、ボリュームが 0 に設定された後、 500 ミリ秒後に破棄されます。）
		/// </para>
		/// <para>
		/// 備考:
		/// 音声のフェードアウトが完了する前にボイスが停止されるプラットフォーム以外は、
		/// 本関数を使用する必要はありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、 <see cref="CriAtomExPlayer.AttachFader"/> 関数を使用して
		/// あらかじめプレーヤーにフェーダーをアタッチしておく必要があります。
		/// 本関数で設定した値は、既に再生中の音声には一切影響しません。
		/// 本関数で設定したフェード時間は、本関数実行後に <see cref="CriAtomExPlayer.Start"/> 関数や
		/// <see cref="CriAtomExPlayer.Stop"/> 関数を実行するタイミングで適用されます。
		/// （既にフェードアウトを開始している音声に対しては、
		/// 本関数で後からフェードアウト後のディレイ時間を変更することはできません。）
		/// ボリュームの制御とボイスの停止が反映されるタイミングは、プラットフォームによって異なります。
		/// そのため、本関数に 0 を指定した場合、プラットフォームによってはボリュームの変更が反映される
		/// 前にボイスが停止される恐れがあります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		/// <seealso cref="CriAtomExPlayer.SetFadeInTime"/>
		public void SetFadeOutEndDelay(Int32 ms)
		{
			NativeMethods.criAtomExPlayer_SetFadeOutEndDelay(NativeHandle, ms);
		}

		/// <summary>フェードアウト後のディレイ時間の取得</summary>
		/// <returns>フェードアウト後のディレイ時間（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェードアウト後のディレイ時間を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は <see cref="CriAtomExPlayer.SetFadeOutEndDelay"/> 関数でセットした値を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetFadeOutEndDelay"/>
		public Int32 GetFadeOutEndDelay()
		{
			return NativeMethods.criAtomExPlayer_GetFadeOutEndDelay(NativeHandle);
		}

		/// <summary>フェード処理中かどうかのチェック</summary>
		/// <returns>フェード処理中かどうか（true = フェード処理中、false = フェード処理中ではない）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェード処理が行われている最中かどうかをチェックします。
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数は、以下の処理期間中 true を返します。
		/// - クロスフェード開始のための同期待ち中。
		/// - フェードイン／フェードアウト処理中（ボリューム変更中）。
		/// - フェードアウト完了後のディレイ期間中。
		/// </para>
		/// </remarks>
		public bool IsFading()
		{
			return NativeMethods.criAtomExPlayer_IsFading(NativeHandle);
		}

		/// <summary>フェーダーパラメーターの初期化</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// フェーダーに設定されている各種パラメーターをクリアし、初期値に戻します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、 <see cref="CriAtomExPlayer.AttachFader"/> 関数を使用して
		/// あらかじめプレーヤーにフェーダーをアタッチしておく必要があります。
		/// 本関数でフェーダーパラメーターをクリアしても、既に再生中の音声には一切影響しません。
		/// 本関数でクリアしたフェーダーパラメーターは、本関数実行後に <see cref="CriAtomExPlayer.Start"/> 関数や
		/// <see cref="CriAtomExPlayer.Stop"/> 関数を実行するタイミングで適用されます。
		/// （既にフェード処理を開始している音声に対しては、
		/// 本関数でクリアしたフェーダーパラメーターを適用することはできません。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AttachFader"/>
		/// <seealso cref="CriAtomExPlayer.SetFadeInTime"/>
		public void ResetFaderParameters()
		{
			NativeMethods.criAtomExPlayer_ResetFaderParameters(NativeHandle);
		}

		/// <summary>グループ制限なし</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスリミットグループによる制限を解除するための定数です。
		/// <see cref="CriAtomExPlayer.SetGroupNumber"/> 関数に対してこの値を指定すると、
		/// 指定されたプレーヤーはボイスリミットグループによる制限を受けなくなります。
		/// （空きボイスがあるか、または自身より低プライオリティのボイスがあれば、
		/// ボイスリミットグループに関係なくボイスを取得します。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetGroupNumber"/>
		public const Int32 NoGroupLimitation = (-1);
		/// <summary>ループ回数制限なし</summary>
		public const Int32 NoLoopLimitation = (CriAtomPlayer.NoLoopLimitation);
		/// <summary>ループ情報を無視</summary>
		public const Int32 IgnoreLoop = (CriAtomPlayer.IgnoreLoop);
		/// <summary>プレーヤーに指定可能な最大ASRラック数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 1つのプレーヤーに対して指定可能なASRラックの最大数です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.SetAsrRackIdArray"/>
		public const Int32 MaxAsrRacks = (8);
		/// <summary>プレーヤーに指定可能な最大出力ポート数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 1つのプレーヤーに対して指定可能な出力ポートの最大数です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExPlayer.AddOutputPort"/>
		/// <seealso cref="CriAtomExPlayer.AddPreferredOutputPort"/>
		public const Int32 MaxOutputPorts = CriAtomExPlayer.MaxAsrRacks;
		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExPlayer(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExPlayer other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExPlayer a, CriAtomExPlayer b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExPlayer a, CriAtomExPlayer b) =>
			!(a == b);

	}
}