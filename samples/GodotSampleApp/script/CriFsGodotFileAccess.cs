using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CriWare.InteropHelpers;
using CriWare;

/// <summary>
/// The class that setup FileAccess for GodotEngine
/// </summary>
public static unsafe class CriFsGodotFileAccess{
	struct FileHandle{
		public Godot.FileAccess fileAccess;
		public Int64 readSize;
		public Int64 fileSize;
	}

	static nint _lastKey = 0;
	static Dictionary<IntPtr, FileHandle> _handles = new Dictionary<IntPtr, FileHandle>();

	static void EscapeStringDestructive(string str){
#if GODOT_WINDOWS
		fixed(char* ptr = str){
			for(int i=0;i<str.Length;i++)
				if(ptr[i] == '\\') ptr[i] = '/';
		}
#endif
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static CriFs.IoError Exists(NativeString path, NativeBool* result){
		var str = path.ToString();
		EscapeStringDestructive(str);

		*result = Godot.FileAccess.FileExists(str);
		return CriFs.IoError.Ok;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static CriFs.IoError Open(NativeString path, CriFs.FileMode mode, CriFs.FileAccess acces, IntPtr* handle){
		var str = path.ToString();
		EscapeStringDestructive(str);

		var file = Godot.FileAccess.Open(str, acces == CriFs.FileAccess.Read ? Godot.FileAccess.ModeFlags.Read : Godot.FileAccess.ModeFlags.Write);
		if(file == null) return CriFs.IoError.Ng;
		_lastKey++;
		_handles.Add(_lastKey, new(){ fileAccess = file, fileSize = (long)file.GetLength() });
		*handle = _lastKey;
		return CriFs.IoError.Ok;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static CriFs.IoError Close(IntPtr key){
		if(!_handles.ContainsKey(key)) return CriFs.IoError.Ng;
		_handles[key].fileAccess.Dispose();
		_handles.Remove(key);
		return CriFs.IoError.Ok;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static CriFs.IoError GetFileSize(IntPtr key, Int64* fileSize){
		if(!_handles.ContainsKey(key)) return CriFs.IoError.Ng;
		*fileSize = (long)_handles[key].fileSize;
		return CriFs.IoError.Ok;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static CriFs.IoError Read(IntPtr key, Int64 Offset, Int64 ReadSize, void *Buffer, Int64 BufferSize){
		if(!_handles.ContainsKey(key)) return CriFs.IoError.Ng;
		var readsize = Math.Min(ReadSize, BufferSize);
		readsize = Math.Min(readsize, (long)_handles[key].fileSize - Offset);
		CollectionsMarshal.GetValueRefOrNullRef(_handles, key).readSize = Math.Max(readsize, 0);
		if(readsize <= 0) return CriFs.IoError.Ok;
		_handles[key].fileAccess.Seek((ulong)Offset);
		Marshal.Copy(_handles[key].fileAccess.GetBuffer(readsize), 0, (IntPtr)Buffer, (int)readsize);
		return CriFs.IoError.Ok;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static CriFs.IoError IsReadComplete(IntPtr key, NativeBool* result){
		if(!_handles.ContainsKey(key)) return CriFs.IoError.Ng;
		*result = true;
		return CriFs.IoError.Ok;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	static CriFs.IoError GetReadSize(IntPtr key, Int64* result){
		if(!_handles.ContainsKey(key)) return CriFs.IoError.Ng;
		*result = _handles[key].readSize;
		return CriFs.IoError.Ok;
	}

	static CriFs.IoInterface* ioInterface;
	public static void Setup(){
		ioInterface = (CriFs.IoInterface*)NativeMemory.Alloc((nuint)sizeof(CriFs.IoInterface));
		*ioInterface = new(){
			Exists = &Exists,
			Remove = null,
			Rename = null,
			Open = &Open,
			Close = &Close,
			GetFileSize = &GetFileSize,
			Read = &Read,
			IsReadComplete = &IsReadComplete,
			CancelRead = null,
			GetReadSize = &GetReadSize,
			Write = null,
			IsWriteComplete = null,
			CancelWrite = null,
			GetWriteSize = null,
			Flush = null,
			Resize = null,
			GetNativeFileHandle = null,
			SetAddReadProgressCallback = null,
			CanParallelRead = null,
		};

		CriFs.SetSelectIoCallback(&SelectIO);
		
		[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
		static CriErr.Error SelectIO(NativeString filename, CriFs.DeviceId* deviceId, CriFs.IoInterfacePtr* ioi){
			*deviceId = CriFs.DeviceId._00;
			*ioi = ioInterface;
			return CriErr.Error.Ok;
		}
	}

	public static void Cleanup() {
		NativeMemory.Free(ioInterface);
		ioInterface = null;
		CriFs.SetSelectIoCallback(null);
	} 
}

