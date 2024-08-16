/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;

namespace CriWare.InteropHelpers {
	/// <summary>
	/// ネイティブハンドル型
	/// </summary>
	/// <remarks>
	/// CRIWAREのオブジェクトインスタンスを取り扱うハンドルを表現する構造体です。
	/// 各ネイティブラッパーオブジェクト内で利用されます。
	/// </remarks>
	public struct NativeHandleIntPtr : IEquatable<NativeHandleIntPtr> {
		IntPtr handle;
		IntPtr memory;
		int memoryId;

		/// <summary>
		/// ハンドルの有効状態
		/// </summary>
		/// <remarks>
		/// CRIWAREのオブジェクトでは、マネージドオブジェクトが存在する場合でも内部のネイティブリソースが破棄済みの場合があります。
		/// ネイティブリソースが破棄されている場合、本プロパティはfalseを返します。
		/// </remarks>
		public bool IsAvailable => NativeAllocator.IsAlive(memory) && NativeAllocator.GetId(memory) == memoryId;
		/// <summary>
		/// リソース破棄を実行可能か否か
		/// </summary>
		/// <remarks>
		/// CRIWAREのネイティブリソースについて、
		/// 暗黙的に確保されたリソース場合など明示的な破棄が許されていないリソースが存在します。
		/// 明示的な破棄が許されていない場合、本プロパティはfalseを返します。
		/// <see cref="System.IDisposable"/>を実装した各CRIWAREオブジェクトの<see cref="System.IDisposable.Dispose"/>メソッド内では本プロパティに従ったハンドリングを行っているため、
		/// 破棄の呼び出し前に本プロパティを確認する必要はありません。
		/// </remarks>
		public bool IsDestroyable => IsAvailable && (NativeAllocator.GetHandle(memory) == handle);

		/// <inheritdoc/>
		public bool Equals(NativeHandleIntPtr other) =>
			handle == other.handle && memory == other.memory && memoryId == other.memoryId;
		/// <inheritdoc/>
		public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object obj) =>
			obj is NativeHandleIntPtr other && Equals(other);
		/// <inheritdoc/>
		public override int GetHashCode() =>
			HashCode.Combine(handle, memory, memoryId);

		/// <summary>
		/// <see cref="IntPtr"/>からのキャスト
		/// </summary>
		public static implicit operator NativeHandleIntPtr(IntPtr pointer){
			if(pointer == IntPtr.Zero)
				throw new Exception("[CRIWARE] Returned NativeHandle is NULL.");
			var mem = NativeAllocator.GetContainingMemory(pointer);
			return new NativeHandleIntPtr(){
				handle = pointer,
				memory = mem,
				memoryId = NativeAllocator.GetId(mem),
			};
		}
		/// <summary>
		/// <see cref="IntPtr"/>へのキャスト
		/// </summary>
		public static implicit operator IntPtr(NativeHandleIntPtr handle){
			if(handle.handle == IntPtr.Zero) return IntPtr.Zero;
			if(!handle.IsAvailable)
				throw new Exception($"[CRIWARE] Native object already disposed.");
			return handle.handle;
		}
	}
}