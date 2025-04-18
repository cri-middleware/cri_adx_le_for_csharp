using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CriWare
{
	/// <exclude/>
	public static unsafe class NativeAllocator
	{

		internal struct MemoryInfo
		{
#pragma warning disable CS0649
			public MemoryInfo* prev;
			public MemoryInfo* next;
			public int size;
			public int id;
			public IntPtr handle;
#pragma warning restore CS0649
		}

		static MemoryInfo* dummyRoot;

		internal static nint GetContainingMemory(nint handle)
		{
			static nint GetMemory(MemoryInfo* current, nint handle)
			{
				if ((nint)current == 0)
					return 0;
				if ((nint)current <= handle && handle < (nint)current + current->size)
				{
					if (current->handle == (nint)0)
						current->handle = handle;
					return (nint)current;
				}
				return GetMemory(current->next, handle);
			}

			if (dummyRoot == null)
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
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static void SetAsOwnerless() => NativeMethods.criNativeAllocator_GetRoot()->next->handle = (IntPtr)1;
		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static bool IsOwnerless(nint memory) => GetHandle(memory) == (IntPtr)1;

		/// <exclude/>
		public unsafe static delegate* unmanaged[Cdecl]<nint, UInt32, nint> GetAllocateFunc() => (delegate* unmanaged[Cdecl]<nint, UInt32, nint>)NativeMethods.criNativeAllocator_GetAllocateFunc();
		/// <exclude/>
		public unsafe static delegate* unmanaged[Cdecl]<nint, nint, void> GetFreeFunc() => (delegate* unmanaged[Cdecl]<nint, nint, void>)NativeMethods.criNativeAllocator_GetFreeFunc();

		static class NativeMethods
		{
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern MemoryInfo* criNativeAllocator_GetRoot();
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern IntPtr criNativeAllocator_GetAllocateFunc();
			[DllImport(CriBaseCSharp.LibraryName, CallingConvention = CriBaseCSharp.callingConvention)]
			internal static extern IntPtr criNativeAllocator_GetFreeFunc();
		}
	}
}