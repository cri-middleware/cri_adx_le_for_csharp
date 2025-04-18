/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Runtime.InteropServices;
using CriWare.Interfaces;
using CriWare.InteropHelpers;

namespace CriWare {

	/// <summary>
	/// CRIWARE基本機能クラス
	/// </summary>
	public partial class CriBaseCSharp{
		static ErrorCallbackFunc _errorCallback;

		/// <summary>
		/// エラーコールバックイベント
		/// </summary>
		/// <remarks>
		/// CRIWAREからのエラー通知はコールバックとして行われます。
		/// CRIWAREの初期化前に本コールバックにデリーゲートを登録し、適切なログ出力などを行ってください。
		/// 本コールバック内部ではイベント通知のたびにヒープ上にstringが確保されます。
		/// アロケーションを避ける場合は<see cref="CriErr.Callback"/>と<see cref="CriErr.ConvertIdToMessage"/>をご利用ください。
		///	</remarks>
		public static ErrorCallbackFunc ErrorCallback => _errorCallback ??= new ErrorCallbackFunc();

		/// <summary>エラーコールバックイベント型</summary>
		public class ErrorCallbackFunc : CriWare.Interfaces.ICallback<string>
		{
			Action<string> callbacks = null;

			/// <inhetitdoc/>
			public event Action<string> Event {
				add { 
					if(callbacks == null)
						CriErr.Callback.Event += Callback_GenericEvent;
					callbacks += value; 
				}
				remove { 
					callbacks -= value; 
					if(callbacks == null)
						CriErr.Callback.Event -= Callback_GenericEvent;
				}
			}
			private unsafe void Callback_GenericEvent((NativeString errid, uint p1, uint p2, IntPtr parray) arg){
#if browser
				var buffer = (stackalloc byte[512]);
				fixed(byte* ptr = buffer){
					criErr_ConvertIdToMessageInternal(arg.errid, arg.p1, arg.p2, ptr, (uint)buffer.Length);
					callbacks?.Invoke(Marshal.PtrToStringUTF8((nint)ptr));
				}

				[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
				static extern NativeString criErr_ConvertIdToMessageInternal(NativeString errid, uint p1, uint p2, byte* msg, uint msg_size);
#else
				callbacks?.Invoke(CriErr.ConvertIdToMessage(arg.errid, arg.p1, arg.p2));
#endif
			}
		}

		internal const string LibraryName =
#if CRI_BUILD_LE
			CriAtomCSharp.libraryName;
#elif (ENABLE_IL2CPP && !UNITY_STANDALONE) || ios
			"__Internal";
#else
			"cri_base";
#endif

		internal const CallingConvention callingConvention = CallingConvention.Cdecl;
	}
}
