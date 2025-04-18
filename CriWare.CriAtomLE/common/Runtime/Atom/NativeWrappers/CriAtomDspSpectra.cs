/****************************************************************************
 *
 * Copyright (c) 2025 CRI Middleware Co., Ltd.
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

	public partial class CriAtomDspSpectra : IDisposable
	{
		/// <summary>スペクトラムアナライザ作成に必要なワーク領域サイズを計算 </summary>
		/// <param name="config">スペクトラムアナライザ作成パラメーター </param>
		/// <returns>CriSint32 必要なワーク領域のサイズ（単位はバイト） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペクトラムアナライザの作成に必要なワークサイズを計算します。
		///  configで与えられるパラメーターに依存し、必要なワークサイズは変化します。
		/// </para>
		/// <para>
		/// 備考:
		/// ワーク領域サイズの計算に失敗した場合、本関数は負値を返します。
		///  （失敗の原因はエラーコールバックで通知されます。）
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtomDspSpectra_CalculateWorkSize(const CriAtomDspSpectraConfig *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtomDspSpectra.Config"/>
		/// <seealso cref="CriAtomDspSpectra.CriAtomDspSpectra"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtomDspSpectra.Config config)
		{
			fixed (CriAtomDspSpectra.Config* configPtr = &config)
				return NativeMethods.criAtomDspSpectra_CalculateWorkSize(configPtr);
		}

		/// <summary>スペクトラムアナライザ作成用パラメーター構造体 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペクトラムアナライザをアタッチする際に指定するパラメーターです。
		/// <see cref="CriAtomDspSpectra.CriAtomDspSpectra"/> 関数の引数に使用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomDspSpectra.CriAtomDspSpectra"/>
		[Serializable]
		public unsafe partial struct Config
		{
			/// <summary>帯域分割数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 周波数軸をいくつの帯域に分割して計測するかを指定します。
			/// <see cref="CriAtomDspSpectra.GetLevels"/> 関数の戻り値（ CriFloat32 配列）は、 本パラメーターで指定した数と同じ長さになります。
			/// </para>
			/// </remarks>
			public UInt32 numBands;

		}
		/// <summary>スペクトラムアナライザの作成 </summary>
		/// <param name="config">スペクトラムアナライザ作成パラメーター </param>
		/// <returns><see cref="CriAtomDspSpectra"/> スペクトラムアナライザオブジェクト </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペクトラムアナライザを作成します。
		///  スペクトラムアナライザは、PCMデータを解析し、 帯域ごとの信号の強さを計測するモジュールです。
		///  PCMデータの入力には、 <see cref="CriAtomDspSpectra.Process"/> 関数を使用します。
		///  解析結果の取得には、 <see cref="CriAtomDspSpectra.GetLevels"/> 関数を使用します。
		///  不要になったスペクトラムアナライザは、 <see cref="CriAtomDspSpectra.Dispose"/> 関数で明示的に破棄する必要があります。
		/// </para>
		/// <para>
		/// 備考:
		/// スペクトラムアナライザの作成に失敗した場合、本関数はnullを返します。
		///  （失敗の原因はエラーコールバックで通知されます。） 
		/// <see cref="CriAtom.SetUserAllocator"/> によるアロケーター登録を行わずに本関数を実行する場合、 <see cref="CriAtomDspSpectra.CalculateWorkSize"/> 関数で計算したサイズ分のメモリをワーク領域として渡す必要があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		///  本関数にセットしたワーク領域は、 <see cref="CriAtomDspSpectra.Dispose"/> 関数を実行するまでの間、アプリケーションで保持する必要があります。
		///  （ <see cref="CriAtomDspSpectra.Dispose"/> 関数実行前に、ワーク領域のメモリを解放しないでください。）
		///  本関数は完了復帰型の関数です。
		///  本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		///  音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。
		/// </para>
		/// <nativeinfo declaration="CriAtomDspSpectraHn criAtomDspSpectra_Create(const CriAtomDspSpectraConfig *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtomDspSpectra.Config"/>
		/// <seealso cref="CriAtomDspSpectra.CalculateWorkSize"/>
		/// <seealso cref="CriAtomDspSpectra.Dispose"/>
		public unsafe CriAtomDspSpectra(in CriAtomDspSpectra.Config config)
		{
			fixed (CriAtomDspSpectra.Config* configPtr = &config)

				NativeHandle = NativeMethods.criAtomDspSpectra_Create(configPtr, default, default);
		}
		/// <summary>デフォルト設定でのインスタンス作成</summary>
		public unsafe CriAtomDspSpectra()
		{
			CriAtomDspSpectra.Config* configPtr = null;
			NativeHandle = NativeMethods.criAtomDspSpectra_Create(configPtr, default, default);
		}

		/// <summary>スペクトラムアナライザの破棄 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペクトラムアナライザを破棄します。
		///  スペクトラムアナライザ作成時に確保されたメモリ領域が解放されます。
		///  （スペクトラムアナライザ作成時にワーク領域を渡した場合、本関数実行後であれば ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		///  本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		///  音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。 
		/// </para>
		/// <nativeinfo declaration="void criAtomDspSpectra_Destroy(CriAtomDspSpectraHn spectra)"/>
		/// </remarks>
		/// <seealso cref="CriAtomDspSpectra.CriAtomDspSpectra"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomDspSpectra_Destroy(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomDspSpectra() => Dispose();
#pragma warning restore 1591

		/// <summary>スペクトラムアナライザのリセット </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スペクトラムアナライザをリセットします。
		///  本関数を実行した時点で、 <see cref="CriAtomDspSpectra.Process"/> 関数にセットしたPCMの情報がクリアされます。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomDspSpectra.GetLevels"/> 関数の戻り値をゼロクリアしたい場合、 本関数を実行してください。
		/// </para>
		/// <nativeinfo declaration="void criAtomDspSpectra_Reset(CriAtomDspSpectraHn spectra)"/>
		/// </remarks>
		/// <seealso cref="CriAtomDspSpectra.Process"/>
		/// <seealso cref="CriAtomDspSpectra.GetLevels"/>
		public void Reset()
		{
			NativeMethods.criAtomDspSpectra_Reset(NativeHandle);
		}

		/// <summary>スペクトラム解析 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// PCMデータを解析します。
		///  解析結果は <see cref="CriAtomDspSpectra.GetLevels"/> 関数で取得可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// 入力するデータ列（pcm）の値は -1.0f ～ +1.0f の範囲を想定しています。
		///  ただ、±1の範囲を超える値を入力した場合でも、<see cref="CriAtomDspSpectra.GetLevels"/> 関数が返す値が大きくなるだけなので、 データ入力時点でクリッピングを行う必要はありません。
		///  本関数は内部で1024点のサンプルが蓄積されるのを待ってからFFT処理を行う為、 スペクトラムは1024サンプル入力毎に更新されます。 
		/// </para>
		/// <nativeinfo declaration="void criAtomDspSpectra_Process(CriAtomDspSpectraHn spectra, CriUint32 num_channels, CriUint32 num_samples, CriFloat32 *pcm[])"/>
		/// </remarks>
		/// <seealso cref="CriAtomDspSpectra.GetLevels"/>
		public unsafe void Process(UInt32 numChannels, UInt32 numSamples, Single[][] pcm)
		{
			var arrayList = stackalloc float*[(int)numChannels];
			FixAndCall(numChannels, numSamples, pcm, 0);

			void FixAndCall(UInt32 outputChannels, UInt32 outputSamples, float[][] outputBuffer, int index){
				if(index > outputChannels)
					NativeMethods.criAtomDspSpectra_Process(NativeHandle, outputChannels, outputSamples, arrayList);
				fixed(float* ptr = outputBuffer[index]){
					arrayList[index] = ptr;
					FixAndCall(outputChannels, outputSamples, outputBuffer, index + 1);
				}
			}
		}

		/// <summary>スペクトル解析結果の取得 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomDspSpectra.Process"/> 関数でセットしたPCMデータの、解析結果を返します。
		///  解析結果は CriFloat32 型の配列です。
		///  配列の要素数は、 <see cref="CriAtomDspSpectra.CriAtomDspSpectra"/> 関数実行時に <see cref="CriAtomDspSpectra.Config"/>::num_bands で指定した数になります。
		///  0 番目の要素が最低帯域の振幅値、 (num_bands - 1) 番目の要素が最高帯域の振幅値です。
		/// </para>
		/// <para>
		/// 備考:
		/// 複数チャンネルのPCMデータを解析した場合、 全てのチャンネルのPCMデータを一旦ミックスし、ミックス結果に対し解析を行います。
		///  そのため、 <see cref="CriAtomDspSpectra.Process"/> 関数に複数チャンネルの音声データをセットした場合でも、 本関数は長さは num_bands の1次元配列を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomDspSpectra.GetLevels"/> 関数が返す値は、帯域ごとの振幅値です。
		///  解析結果を市販のスペクトルアナライザのように表示させたい場合、 本関数が返す値をデシベル値に変換する必要があります。
		/// </para>
		/// <nativeinfo declaration="const CriFloat32* criAtomDspSpectra_GetLevels(CriAtomDspSpectraHn spectra)"/>
		/// </remarks>
		/// <seealso cref="CriAtomDspSpectra.Process"/>
		public IntPtr GetLevels()
		{
			return NativeMethods.criAtomDspSpectra_GetLevels(NativeHandle);
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomDspSpectra(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomDspSpectra other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomDspSpectra a, CriAtomDspSpectra b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomDspSpectra a, CriAtomDspSpectra b) =>
			!(a == b);

	}
}