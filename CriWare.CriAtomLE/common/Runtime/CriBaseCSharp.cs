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
			private void Callback_GenericEvent((NativeString errid, uint p1, uint p2, IntPtr parray) arg) =>
				callbacks?.Invoke(CriErr.ConvertIdToMessage(arg.errid, arg.p1, arg.p2));
		}

		internal const string LibraryName = 
#if (UNITY_IOS && !UNITY_EDITOR) || ios
			"__Internal";
#elif CRI_BUILD_LE
			"cri_atom";
#else
			"cri_base";
#endif

		internal const CallingConvention callingConvention = CallingConvention.Cdecl;
	}

	/// <exclude/>
	public static unsafe class NativeAllocator {

		struct MemoryInfo {
#pragma warning disable CS0649
			public MemoryInfo* prev;
			public MemoryInfo* next;
			public int size;
			public int id;
			public IntPtr handle;
#pragma warning restore CS0649
		}

		static MemoryInfo* dummyRoot;

		internal static nint GetContainingMemory(nint handle){
			static nint GetMemory(MemoryInfo* current, nint handle){
				if((nint)current == 0)
					return 0;
				if((nint)current <= handle && handle < (nint)current + current->size){
					if(current -> handle != (nint)0)
						current -> handle = handle;
					return (nint)current;
				}
				return GetMemory(current->next, handle);
			}

			if(dummyRoot == null)
				dummyRoot = NativeMethods.criNativeAllocator_GetRoot();
			return GetMemory(dummyRoot->next, handle);
		}

		internal static bool IsAlive(nint memory)
		{
			static nint GetMemory(MemoryInfo* current, nint memory)
			{
				if ((nint)current == 0) return 0;
				if ((nint)current == memory) return (nint)current;
				return GetMemory(current->next, memory);
			}

			if (dummyRoot == null)
				dummyRoot = NativeMethods.criNativeAllocator_GetRoot();
			return GetMemory(dummyRoot->next, memory) != 0;
		}

		internal static int GetSize(nint memory) => ((MemoryInfo*)memory)->size;
		internal static int GetId(nint memory) => ((MemoryInfo*)memory)->id;
		internal static nint GetHandle(nint memory) => ((MemoryInfo*)memory)->handle;

		/// <exclude/>
		public unsafe static delegate*unmanaged[Cdecl]<nint, UInt32, nint> GetAllocateFunc() => (delegate*unmanaged[Cdecl]<nint, UInt32, nint>)NativeMethods.criNativeAllocator_GetAllocateFunc();
		/// <exclude/>
		public unsafe static delegate*unmanaged[Cdecl]<nint, nint, void> GetFreeFunc() => (delegate*unmanaged[Cdecl]<nint, nint, void>)NativeMethods.criNativeAllocator_GetFreeFunc();

		static class NativeMethods {
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern MemoryInfo* criNativeAllocator_GetRoot();
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern IntPtr criNativeAllocator_GetAllocateFunc();
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern IntPtr criNativeAllocator_GetFreeFunc();
		}
	}
}
