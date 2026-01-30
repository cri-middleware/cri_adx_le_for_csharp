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
			public IntPtr data;
			public Int64 data_size;
#pragma warning restore CS0649
		}

		static MemoryInfo* dummyRoot;

		internal static nint GetContainingMemory(nint handle)
		{
			static nint GetMemory(MemoryInfo* current, nint handle)
			{
				if ((nint)current == 0)
					return 0;
				if (((nint)current <= handle && handle < (nint)current + current->size) ||
					((nint)current->data <= handle && handle < (nint)current->data + current->data_size))
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
		internal static int GetId(nint memory) => memory == default ? 0 : ((MemoryInfo*)memory)->id;
		internal static nint GetHandle(nint memory) => ((MemoryInfo*)memory)->handle;

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static void SetAsOwnerless() => NativeMethods.criNativeAllocator_GetRoot()->next->handle = (IntPtr)1;
		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static bool IsOwnerless(nint memory) => GetHandle(memory) == (IntPtr)1;
		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref struct BindDataSectionScope
		{
			nint data;
			Int64 size;
			MemoryInfo* first;
			/// <exclude/>
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public BindDataSectionScope(nint data, Int64 size, bool skipAllocationCheck = false)
			{
				this.data = data;
				this.size = size;
				this.first = skipAllocationCheck ? default : NativeMethods.criNativeAllocator_GetRoot()->next;
			}
			/// <exclude/>
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public void Dispose()
			{
				var root = NativeMethods.criNativeAllocator_GetRoot();
				if (first != default)
					if (root->next->next != first)
						throw new Exception("Unexpected allocation while binding data.");
				root->next->data = data;
				root->next->data_size = size;
			}
		}

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