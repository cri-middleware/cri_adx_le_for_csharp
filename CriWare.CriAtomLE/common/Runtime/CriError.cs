/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
namespace CriWare
{
	/// <summary>
	/// CRIWAREが出力するエラーを取り扱うためのクラス
	/// </summary>
	public partial class CriErr
	{
		/// <summary>
		/// エラーコード
		/// </summary>
		public enum Error
		{
			/// <summary>
			///  正常終了 
			/// </summary>
			Ok = 0,
			/// <summary>
			///  エラーが発生 
			/// </summary>
			Ng = -1,
			/// <summary>
			///  引数が不正 
			/// </summary>
			Invalidparameter = -2,
			/// <summary>
			///  メモリの確保に失敗 
			/// </summary>
			Failedtoallocatememory = -3,
			/// <summary>
			///  非スレッドセーフ関数の並列実行 
			/// </summary>
			Unsafefunctioncall = -4,
			/// <summary>
			///  未実装関数の実行 
			/// </summary>
			Functionnotimplemented = -5,
			/// <summary>
			///  ライブラリが未初期化 
			/// </summary>
			Librarynotinitialized = -6,
		}
	}
}
