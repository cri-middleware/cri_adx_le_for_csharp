/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
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
#pragma warning disable 0465
	/// <summary>CriFs API</summary>
	public static partial class CriFs
	{
		public partial struct IoInterfacePtr{
			IntPtr pointer;
		}


		/// <summary>メモリ確保関数の登録</summary>
		/// <param name="func">メモリ確保関数</param>
		/// <param name="obj">ユーザー指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI File Systemライブラリにメモリ確保関数を登録します。
		/// CRI File Systemライブラリ内がライブラリ内で行なうメモリ確保処理を、
		/// ユーザー独自のメモリ確保処理に置き換えたい場合に使用します。
		/// 本関数の使用手順は以下のとおりです。
		/// (1) <see cref="CriFs.MallocFunc"/> インターフェイスに副ったメモリ確保関数を用意する。
		/// (2) <see cref="CriFs.SetUserMallocFunction"/> 関数を使用し、CRI File Systemライブラリに対して
		/// メモリ確保関数を登録する。
		/// 具体的なコードの例は以下のとおりです。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数の obj に指定した値は、 <see cref="CriFs.MallocFunc"/> に引数として渡されます。
		/// メモリ確保時にメモリマネージャー等を参照する必要がある場合には、
		/// 当該オブジェクトを本関数の引数にセットしておき、コールバック関数で引数を経由
		/// して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ確保関数を登録する際には、合わせてメモリ解放関数（ <see cref="CriFs.FreeFunc"/> ）を
		/// 登録する必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.MallocFunc"/>
		/// <seealso cref="CriFs.SetUserFreeFunction"/>
		public static unsafe void SetUserMallocFunction(delegate* unmanaged[Cdecl]<IntPtr, UInt32, IntPtr> func, IntPtr obj)
		{
			NativeMethods.criFs_SetUserMallocFunction((IntPtr)func, obj);
		}

		/// <summary>メモリ確保関数</summary>
		/// <returns>確保したメモリのアドレス（失敗時はnull）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ確保関数登録用のインターフェイスです。
		/// CRI File Systemライブラリがライブラリ内で行なうメモリ確保処理を、
		/// ユーザー独自のメモリ確保処理に置き換えたい場合に使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// コールバック関数が実行される際には、sizeに必要とされるメモリのサイズがセット
		/// されています。
		/// コールバック関数内でsize分のメモリを確保し、確保したメモリのアドレスを
		/// 戻り値として返してください。
		/// 尚、引数の obj には、<see cref="CriFs.SetUserMallocFunction"/> 関数で登録したユーザー指定
		/// オブジェクトが渡されます。
		/// メモリ確保時にメモリマネージャー等を参照する必要がある場合には、
		/// 当該オブジェクトを <see cref="CriFs.SetUserMallocFunction"/> 関数の引数にセットしておき、
		/// 本コールバック関数の引数を経由して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリの確保に失敗した場合、エラーコールバックが返されたり、呼び出し元の関数が
		/// 失敗する可能性がありますのでご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.FreeFunc"/>
		/// <seealso cref="CriFs.SetUserMallocFunction"/>
		public unsafe class MallocFunc : NativeCallbackBase<MallocFunc.Arg, IntPtr>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>要求メモリサイズ（バイト単位）</summary>
				public UInt32 size { get; }

				internal Arg(UInt32 size)
				{
					this.size = size;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static IntPtr CallbackFunc(IntPtr obj, UInt32 size) =>
				InvokeCallbackInternal(obj, new(size));
#if !NET5_0_OR_GREATER
			delegate IntPtr NativeDelegate(IntPtr obj, UInt32 size);
			static NativeDelegate callbackDelegate = null;
#endif
			internal MallocFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, UInt32, IntPtr>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>メモリ解放関数の登録</summary>
		/// <param name="func">メモリ解放関数</param>
		/// <param name="obj">ユーザー指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI File Systemライブラリにメモリ解放関数を登録します。
		/// CRI File Systemライブラリ内がライブラリ内で行なうメモリ解放処理を、
		/// ユーザー独自のメモリ解放処理に置き換えたい場合に使用します。
		/// 本関数の使用手順は以下のとおりです。
		/// (1) <see cref="CriFs.FreeFunc"/> インターフェイスに副ったメモリ解放関数を用意する。
		/// (2) <see cref="CriFs.SetUserFreeFunction"/> 関数を使用し、CRI File Systemライブラリに対して
		/// メモリ解放関数を登録する。
		/// 具体的なコードの例は以下のとおりです。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数の obj に指定した値は、 <see cref="CriFs.FreeFunc"/> に引数として渡されます。
		/// メモリ確保時にメモリマネージャー等を参照する必要がある場合には、
		/// 当該オブジェクトを本関数の引数にセットしておき、コールバック関数で引数を経由
		/// して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ解放関数を登録する際には、合わせてメモリ確保関数（ <see cref="CriFs.MallocFunc"/> ）を
		/// 登録する必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.FreeFunc"/>
		/// <seealso cref="CriFs.SetUserMallocFunction"/>
		public static unsafe void SetUserFreeFunction(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criFs_SetUserFreeFunction((IntPtr)func, obj);
		}

		/// <summary>メモリ解放関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ解放関数登録用のインターフェイスです。
		/// CRI File Systemライブラリ内がライブラリ内で行なうメモリ解放処理を、
		/// ユーザー独自のメモリ解放処理に置き換えたい場合に使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// コールバック関数が実行される際には、memに解放すべきメモリのアドレスがセット
		/// されています。
		/// コールバック関数内でmemの領域のメモリを解放してください。
		/// 尚、引数の obj には、<see cref="CriFs.SetUserFreeFunction"/> 関数で登録したユーザー指定
		/// オブジェクトが渡されます。
		/// メモリ確保時にメモリマネージャー等を参照する必要がある場合には、
		/// 当該オブジェクトを <see cref="CriFs.SetUserFreeFunction"/> 関数の引数にセットしておき、
		/// 本コールバック関数の引数を経由して参照してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.SetUserFreeFunction"/>
		public unsafe class FreeFunc : NativeCallbackBase<FreeFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>解放するメモリアドレス</summary>
				public IntPtr mem { get; }

				internal Arg(IntPtr mem)
				{
					this.mem = mem;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, IntPtr mem) =>
				InvokeCallbackInternal(obj, new(mem));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, IntPtr mem);
			static NativeDelegate callbackDelegate = null;
#endif
			internal FreeFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>I/O選択コールバックの登録</summary>
		/// <param name="func">I/O選択コールバック</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// I/O選択コールバック関数（ <see cref="CriFs.SelectIoCbFunc"/> ）を登録します。
		/// CRI File Systemライブラリはファイルにアクセスする際、まず初めに、そのファイルが存在するデバイスのID（ <see cref="CriFs.DeviceId"/> ）と、
		/// デバイスにアクセスするためのI/Oインターフェイス（ ::CriFsIoInterface ）を選択します。
		/// デフォルト状態では、デバイスIDとI/Oインターフェイスの選択はライブラリ内で暗黙的に行なわれますが、
		/// 本関数を使用することで、デバイスIDとI/Oインターフェイスをユーザーが自由に指定することが可能になります。
		/// これにより、ユーザーが独自に作成したI/Oインターフェイスを使用してファイルにアクセスすることが可能になります。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定するとことで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.SelectIoCbFunc"/>
		public static unsafe CriErr.Error SetSelectIoCallback(delegate* unmanaged[Cdecl]<NativeString, CriFs.DeviceId*, IoInterfacePtr*, CriErr.Error> func)
		{
			return NativeMethods.criFs_SetSelectIoCallback((IntPtr)func);
		}

		/// <summary>I/O選択コールバック関数</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// I/O選択コールバック関数は、CRI File SystemライブラリのI/O処理を、
		/// ユーザーの独自I/Oインターフェースで置き換える際に使用します。
		/// 具体的には、ユーザーは <see cref="CriFs.SelectIoCbFunc"/> 型の関数を実装し、
		/// その関数を <see cref="CriFs.SetSelectIoCallback"/> 関数にセットする必要があります。
		/// <see cref="CriFs.SelectIoCbFunc"/> 関数は、入力されたファイルのパス（引数のpath）を解析し、
		/// そのファイルが存在するデバイスのID（引数のdevice_id）と、
		/// デバイスにアクセスするためのI/Oインターフェイス（引数のioif）を返す必要があります。
		/// </para>
		/// <para>
		/// 補足:
		/// ライブラリがデフォルト状態で利用するI/Oインターフェイスは、 ::criFs_GetDefaultIoInterface 関数で取得可能です。
		/// 特定のファイルのみを独自のI/Oインターフェイスを処理したい場合には、
		/// 他のファイルを全て ::criFs_GetDefaultIoInterface 関数で取得したI/Oインターフェイスで処理してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.SetSelectIoCallback"/>
		public unsafe class SelectIoCbFunc : NativeCallbackBase<SelectIoCbFunc.Arg, CriErr.Error>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				NativeString path;
				/// <summary>デバイスID</summary>
				public NativeReference<CriFs.DeviceId> deviceId { get; }
				/// <summary>I/Oインターフェイス</summary>
				public NativeReference<CriFs.IoInterfacePtr> ioif { get; }

				internal Arg(NativeString path, NativeReference<CriFs.DeviceId> deviceId, NativeReference<CriFs.IoInterfacePtr> ioif)
				{
					this.path = path;
					this.deviceId = deviceId;
					this.ioif = ioif;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static CriErr.Error CallbackFunc(NativeString path, CriFs.DeviceId* deviceId, IoInterfacePtr* ioif) =>
				InvokeCallbackInternal(default, new(path, deviceId, ioif));
#if !NET5_0_OR_GREATER
			delegate CriErr.Error NativeDelegate(NativeString path, CriFs.DeviceId* deviceId, IoInterfacePtr* ioif);
			static NativeDelegate callbackDelegate = null;
#endif
			internal SelectIoCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<NativeString, CriFs.DeviceId*, IoInterfacePtr*, CriErr.Error>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>EN</summary>
		/// <summary>Device ID</summary>
		public enum DeviceId
		{
			/// <summary>デフォルトデバイス</summary>
			_00 = 0,
			_01 = 1,
			_02 = 2,
			_03 = 3,
			_04 = 4,
			_05 = 5,
			_06 = 6,
			/// <summary>メモリ</summary>
			_07 = 7,
			Max = 8,
			/// <summary>無効</summary>
			Invalid = -1,
			/// <summary>enum be 4bytes</summary>
			EnumBeSint32 = 2147483647,
		}
		/// <summary>デフォルトデバイスID</summary>
		public const CriFs.DeviceId DeviceDefault = (CriFs.DeviceId._00);
	}
}