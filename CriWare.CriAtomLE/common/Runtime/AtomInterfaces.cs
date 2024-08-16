/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using CriWare.InteropHelpers;

namespace CriWare.Interfaces
{
	/// <summary>
	/// 座標指定可能なオブジェクトのインターフェイス
	/// </summary>
	/// <remarks>
	/// <see cref="CriAtomEx3dSource"/>などの、空間内での座標や姿勢を指定できるインスタンスは本インターフェイスを介して制御できます。
	/// 各フレームワークやゲームエンジン固有のベクター型や回転/姿勢を表現する型を利用する場合、本インターフェイスに対する拡張メソッドを定義することをおすすめします。
	/// 指定した座標や姿勢を実際の振る舞いに反映するには、<see cref="IUpdatable.Update"/>を呼び出す必要がある点にご注意ください。
	///	</remarks>
	public interface IPositionable : IUpdatable
	{
		/// <summary>
		/// 座標の指定
		/// </summary>
		/// <param name="vector">空間内での座標</param>
		public void SetPosition(in CriAtomEx.Vector vector);
		/// <summary>
		/// 姿勢の指定
		/// </summary>
		/// <param name="front">前方向ベクトル</param>
		/// <param name="top">上方向ベクトル</param>
		public void SetOrientation(in CriAtomEx.Vector front, in CriAtomEx.Vector top);
		/// <summary>
		/// 速度の指定
		/// </summary>
		/// <param name="velocity">空間内での速度</param>
		public void SetVelocity(in CriAtomEx.Vector velocity);
		/// <summary>
		/// 3Dリージョンの指定
		/// </summary>
		/// <param name="region">3Dリージョンオブジェクト</param>
		public void Set3dRegionHn(CriAtomEx3dRegion region);
	}

	/// <summary>
	/// PCMデータを参照する構造体のインターフェイス
	/// </summary>
	public interface IPcmData {
		/// <summary>PCMの形式</summary>
		public CriAtom.PcmFormat format { get; }
		/// <summary>チャンネル数</summary>
		public Int32 numChannels { get; }
		/// <summary>サンプル数</summary>
		public Int32 numSamples { get; }
		/// <summary>PCMデータのチャンネル配列</summary>
		public NativeReference<IntPtr> data { get; }
	}
}
