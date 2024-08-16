using System.Runtime.InteropServices;

namespace CriWare{
	partial class CriFsCSharp {
		internal const string libraryName = CriAtomCSharp.libraryName;
		internal const CallingConvention callingConversion = CallingConvention.Cdecl;
	}

	partial class CriFs {
		/// <summary>Error of I/O Interface</summary>
		public enum IoError
		{
			/// <summary>エラーなし</summary>
			Ok = 0,
			/// <summary>一般エラー</summary>
			Ng = -1,
			/// <summary>リトライすべき</summary>
			TryAgain = -2,
			/// <summary>個別エラー（ファイル無し）</summary>
			NgNoEntry = -11,
			/// <summary>個別エラー（データが不正）</summary>
			NgInvalidData = -12,
			/// <summary>enum be 4bytes</summary>
			EnumBeSint32 = 2147483647,
		}
	}
}