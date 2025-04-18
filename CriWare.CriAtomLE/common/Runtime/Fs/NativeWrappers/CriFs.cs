/****************************************************************************
 *
 * Copyright (c) 2025 CRI Middleware Co., Ltd.
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
		/// <summary>メモリ確保関数の登録 </summary>
		/// <param name="func">メモリ確保関数 </param>
		/// <param name="obj">ユーザー指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI File Systemライブラリにメモリ確保関数を登録します。
		///  CRI File Systemライブラリ内がライブラリ内で行なうメモリ確保処理を、 ユーザー独自のメモリ確保処理に置き換えたい場合に使用します。
		///  本関数の使用手順は以下のとおりです。
		///  (1) <see cref="CriFs.MallocFunc"/> インターフェイスに副ったメモリ確保関数を用意する。
		///  (2) <see cref="CriFs.SetUserMallocFunction"/> 関数を使用し、CRI File Systemライブラリに対して メモリ確保関数を登録する。
		///  具体的なコードの例は以下のとおりです。 
		/// </para>
		/// <para>
		/// 例:
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// <para>
		/// 備考:
		/// 引数の obj に指定した値は、 <see cref="CriFs.MallocFunc"/> に引数として渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを本関数の引数にセットしておき、コールバック関数で引数を経由 して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ確保関数を登録する際には、合わせてメモリ解放関数（ <see cref="CriFs.FreeFunc"/> ）を 登録する必要があります。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criFs_SetUserMallocFunction(CriFsMallocFunc func, void *obj)"/>
		/// </remarks>
		/// <seealso cref="CriFs.MallocFunc"/>
		/// <seealso cref="CriFs.SetUserFreeFunction"/>
		public static unsafe void SetUserMallocFunction(delegate* unmanaged[Cdecl]<IntPtr, UInt32, IntPtr> func, IntPtr obj)
		{
			NativeMethods.criFs_SetUserMallocFunction((IntPtr)func, obj);
		}

		/// <summary>メモリ解放関数の登録 </summary>
		/// <param name="func">メモリ解放関数 </param>
		/// <param name="obj">ユーザー指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI File Systemライブラリにメモリ解放関数を登録します。
		///  CRI File Systemライブラリ内がライブラリ内で行なうメモリ解放処理を、 ユーザー独自のメモリ解放処理に置き換えたい場合に使用します。
		///  本関数の使用手順は以下のとおりです。
		///  (1) <see cref="CriFs.FreeFunc"/> インターフェイスに副ったメモリ解放関数を用意する。
		///  (2) <see cref="CriFs.SetUserFreeFunction"/> 関数を使用し、CRI File Systemライブラリに対して メモリ解放関数を登録する。
		///  具体的なコードの例は以下のとおりです。 
		/// </para>
		/// <para>
		/// 例:
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// <para>
		/// 備考:
		/// 引数の obj に指定した値は、 <see cref="CriFs.FreeFunc"/> に引数として渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを本関数の引数にセットしておき、コールバック関数で引数を経由 して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ解放関数を登録する際には、合わせてメモリ確保関数（ <see cref="CriFs.MallocFunc"/> ）を 登録する必要があります。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criFs_SetUserFreeFunction(CriFsFreeFunc func, void *obj)"/>
		/// </remarks>
		/// <seealso cref="CriFs.FreeFunc"/>
		/// <seealso cref="CriFs.SetUserMallocFunction"/>
		public static unsafe void SetUserFreeFunction(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criFs_SetUserFreeFunction((IntPtr)func, obj);
		}

		/// <summary>バインダー使用数の取得 </summary>
		/// <param name="curNum">現在使用中のバインダーの数 </param>
		/// <param name="maxNum">過去に最大同時に利用したバインダーの数 </param>
		/// <param name="limit">利用可能なバインダーの上限数 </param>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// バインダーの使用数に関する情報を取得します。
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFs_GetNumUsedBinders(CriSint32 *cur_num, CriSint32 *max_num, CriSint32 *limit)"/>
		/// </remarks>
		public static unsafe CriErr.Error GetNumUsedBinders(out Int32 curNum, out Int32 maxNum, out Int32 limit)
		{
			fixed (Int32* curNumPtr = &curNum)
			fixed (Int32* maxNumPtr = &maxNum)
			fixed (Int32* limitPtr = &limit)
				return (CriErr.Error)NativeMethods.criFs_GetNumUsedBinders(curNumPtr, maxNumPtr, limitPtr);
		}

		/// <summary>I/O選択コールバックの登録 </summary>
		/// <param name="func">I/O選択コールバック </param>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// I/O選択コールバック関数（ <see cref="CriFs.SelectIoCbFunc"/> ）を登録します。
		///  CRI File Systemライブラリはファイルにアクセスする際、まず初めに、そのファイルが存在するデバイスのID（ <see cref="CriFs.DeviceId"/> ）と、 デバイスにアクセスするためのI/Oインターフェイス（ <see cref="CriFs.IoInterface"/> ）を選択します。
		///  デフォルト状態では、デバイスIDとI/Oインターフェイスの選択はライブラリ内で暗黙的に行なわれますが、 本関数を使用することで、デバイスIDとI/Oインターフェイスをユーザーが自由に指定することが可能になります。
		///  これにより、ユーザーが独自に作成したI/Oインターフェイスを使用してファイルにアクセスすることが可能になります。
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数は1つしか登録できません。
		///  登録操作を複数回行った場合、既に登録済みのコールバック関数が、 後から登録したコールバック関数により上書きされてしまいます。
		///  funcにnullを指定するとことで登録済み関数の登録解除が行えます。
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFs_SetSelectIoCallback(CriFsSelectIoCbFunc func)"/>
		/// </remarks>
		/// <seealso cref="CriFs.SelectIoCbFunc"/>
		public static unsafe CriErr.Error SetSelectIoCallback(delegate* unmanaged[Cdecl]<NativeString, CriFs.DeviceId*, NativeReference<CriFs.IoInterface>*, CriErr.Error> func)
		{
			return (CriErr.Error)NativeMethods.criFs_SetSelectIoCallback((IntPtr)func);
		}
		static unsafe void SetSelectIoCallbackInternal(IntPtr func) => SetSelectIoCallback((delegate* unmanaged[Cdecl]<NativeString, CriFs.DeviceId*, NativeReference<CriFs.IoInterface>*, CriErr.Error>)func);
		static CriFs.SelectIoCbFunc _selectIoCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetSelectIoCallback" />
		public static CriFs.SelectIoCbFunc SelectIoCallback => _selectIoCallback ?? (_selectIoCallback = new CriFs.SelectIoCbFunc(SetSelectIoCallbackInternal));

		/// <summary>ファイルI/Oモードの設定 </summary>
		/// <param name="ioMode">ファイルI/Oモード </param>
		/// <returns><see cref="CriErr.Error"/> エラーコード </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI File Systemライブラリ全体のファイルI/Oモードを設定します。
		/// <see cref="CriFs.FileIoMode.ShareFileHandle"/> を設定すると、ファイルオブジェクトをライブラリ内部で共有 し、ファイルアクセスを効率良く行います。
		///  具体的には、::criFsBinder_BindCpk 関数、<see cref="CriFsBinder.BindFile"/> 関数を呼び出し時に作成 したファイルオブジェクトはアンバインドするまでライブラリ内部で保持し、保持中のファイルに対するアクセスでは ファイルオープンが発生しません。
		/// <see cref="CriFs.FileIoMode.OpenEveryTime"/> を設定すると、ファイルオブジェクトの共有を行わずにファイル アクセスのたびにファイルオープンを行います。
		///  ファイルオープン負荷の分だけファイル読み込みの性能は落ちますが、ファイルアクセスが必要な時のみ ファイルオブジェクトを作成するため、ファイルディスクリプタなどのリソース消費を最小限に抑えることが可能です。
		///  未設定時（CRI File Systemライブラリのデフォルト設定）は、機種ごとに異なります。
		///  機種固有マニュアルに記載がない限り、デフォルト設定は <see cref="CriFs.FileIoMode.ShareFileHandle"/> です。
		/// </para>
		/// <para>
		/// 注意：
		/// 本関数はライブラリ初期化前に呼び出してください。
		///  ライブラリ初期化後に呼び出すことは出来ません。
		/// </para>
		/// <nativeinfo declaration="CriError CRIAPI criFs_ControlFileIoMode(CriFsFileIoMode io_mode)"/>
		/// </remarks>
		/// <seealso cref="CriFs.FileIoMode"/>
		public static CriErr.Error ControlFileIoMode(CriFs.FileIoMode ioMode)
		{
			return (CriErr.Error)NativeMethods.criFs_ControlFileIoMode(ioMode);
		}

		/// <summary>デフォルトデバイスID </summary>
		public const CriFs.DeviceId DeviceDefault = (CriFs.DeviceId._00);
		/// <summary>メモリ確保関数 </summary>
		/// <returns>void* 確保したメモリのアドレス（失敗時はnull） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ確保関数登録用のインターフェイスです。
		///  CRI File Systemライブラリがライブラリ内で行なうメモリ確保処理を、 ユーザー独自のメモリ確保処理に置き換えたい場合に使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// コールバック関数が実行される際には、sizeに必要とされるメモリのサイズがセット されています。
		///  コールバック関数内でsize分のメモリを確保し、確保したメモリのアドレスを 戻り値として返してください。
		///  尚、引数の obj には、<see cref="CriFs.SetUserMallocFunction"/> 関数で登録したユーザー指定 オブジェクトが渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを <see cref="CriFs.SetUserMallocFunction"/> 関数の引数にセットしておき、 本コールバック関数の引数を経由して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリの確保に失敗した場合、エラーコールバックが返されたり、呼び出し元の関数が 失敗する可能性がありますのでご注意ください。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.FreeFunc"/>
		/// <seealso cref="CriFs.SetUserMallocFunction"/>
		public unsafe class MallocFunc : NativeCallbackBase<MallocFunc.Arg, IntPtr>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>要求メモリサイズ（バイト単位） </summary>
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
			static IntPtr CriFsMallocFuncCallbackFunc(IntPtr obj, UInt32 size) =>
				InvokeCallbackInternal(obj, new(size));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate IntPtr NativeDelegate(IntPtr obj, UInt32 size);
			static NativeDelegate callbackDelegate = null;
#endif
			internal MallocFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, UInt32, IntPtr>)&CriFsMallocFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriFsMallocFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>メモリ解放関数 </summary>
		/// <returns>なし </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ解放関数登録用のインターフェイスです。
		///  CRI File Systemライブラリ内がライブラリ内で行なうメモリ解放処理を、 ユーザー独自のメモリ解放処理に置き換えたい場合に使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// コールバック関数が実行される際には、memに解放すべきメモリのアドレスがセット されています。
		///  コールバック関数内でmemの領域のメモリを解放してください。 尚、引数の obj には、<see cref="CriFs.SetUserFreeFunction"/> 関数で登録したユーザー指定 オブジェクトが渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを <see cref="CriFs.SetUserFreeFunction"/> 関数の引数にセットしておき、 本コールバック関数の引数を経由して参照してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.SetUserFreeFunction"/>
		public unsafe class FreeFunc : NativeCallbackBase<FreeFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>解放するメモリアドレス </summary>
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
			static void CriFsFreeFuncCallbackFunc(IntPtr obj, IntPtr mem) =>
				InvokeCallbackInternal(obj, new(mem));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr obj, IntPtr mem);
			static NativeDelegate callbackDelegate = null;
#endif
			internal FreeFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CriFsFreeFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriFsFreeFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>I/O選択コールバック関数 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// I/O選択コールバック関数は、CRI File SystemライブラリのI/O処理を、 ユーザーの独自I/Oインターフェースで置き換える際に使用します。
		///  具体的には、ユーザーは <see cref="CriFs.SelectIoCbFunc"/> 型の関数を実装し、 その関数を <see cref="CriFs.SetSelectIoCallback"/> 関数にセットする必要があります。
		/// <see cref="CriFs.SelectIoCbFunc"/> 関数は、入力されたファイルのパス（引数のpath）を解析し、 そのファイルが存在するデバイスのID（引数のdevice_id）と、 デバイスにアクセスするためのI/Oインターフェイス（引数のioif）を返す必要があります。
		/// </para>
		/// <para>
		/// 補足:
		/// ライブラリがデフォルト状態で利用するI/Oインターフェイスは、 ::criFs_GetDefaultIoInterface 関数で取得可能です。
		///  特定のファイルのみを独自のI/Oインターフェイスを処理したい場合には、 他のファイルを全て ::criFs_GetDefaultIoInterface 関数で取得したI/Oインターフェイスで処理してください。
		/// <NOT SUPPORTED TAG : programlisting/>
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.SetSelectIoCallback"/>
		public unsafe class SelectIoCbFunc : NativeCallbackBase<SelectIoCbFunc.Arg, CriErr.Error>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ファイルのパス </summary>
				public NativeString path { get; }
				/// <summary>デバイスID </summary>
				public NativeReference<CriFs.DeviceId> deviceId { get; }
				/// <summary>I/Oインターフェイス </summary>
				public NativeReference<CriFs.IoInterface>* ioif { get; }

				internal Arg(NativeString path, NativeReference<CriFs.DeviceId> deviceId, NativeReference<CriFs.IoInterface>* ioif)
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
			static CriErr.Error CriFsSelectIoCbFuncCallbackFunc(IntPtr path, CriFs.DeviceId* deviceId, NativeReference<CriFs.IoInterface>* ioif) =>
				InvokeCallbackInternal(default, new(path, deviceId, ioif));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate CriErr.Error NativeDelegate(IntPtr path, CriFs.DeviceId* deviceId, NativeReference<CriFs.IoInterface>* ioif);
			static NativeDelegate callbackDelegate = null;
#endif
			internal SelectIoCbFunc(Action<IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, CriFs.DeviceId*, NativeReference<CriFs.IoInterface>*, CriErr.Error>)&CriFsSelectIoCbFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriFsSelectIoCbFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>デバイスID </summary>
		public enum DeviceId
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>デフォルトデバイス </para>
			/// </remarks>
			_00 = 0,
			_01 = 1,
			_02 = 2,
			_03 = 3,
			_04 = 4,
			_05 = 5,
			_06 = 6,
			/// <summary></summary>
			/// <remarks>
			/// <para>メモリ </para>
			/// </remarks>
			_07 = 7,
			Max = 8,
			/// <summary></summary>
			/// <remarks>
			/// <para>無効 </para>
			/// </remarks>
			Invalid = -1,
			EnumBeSint32 = 2147483647,
		}
		/// <summary>I/Oインターフェイス </summary>
		public unsafe partial struct IoInterface
		{
			/// <summary>ファイルの有無の確認 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルの有無を確認する関数です。
			///  ファイルが存在する場合は true を、 存在しない場合は false を result にセットする必要があります。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Exists)(const CriChar8 *path"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<NativeString, NativeBool*, CriFs.IoError> Exists;

			/// <summary>ファイルの削除 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルを削除する関数です。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Remove)(const CriChar8 *path)"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<NativeString, CriFs.IoError> Remove;

			/// <summary>ファイル名の変更 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイル名の変更を行なう関数です。
			///  old_path で指定されたファイルを、 new_path にリネームします。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Rename)(const CriChar8 *old_path"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<NativeString, NativeString, CriFs.IoError> Rename;

			/// <summary>ファイルのオープン </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルをオープンする関数です。
			///  オープンに成功した場合、<see cref="IntPtr"/> 型のファイルオブジェクトを返す必要があります。
			/// </para>
			/// <para>
			/// 補足:
			/// <see cref="IntPtr"/> は void ポインターとして定義されています。
			///  独自のファイル情報構造体を定義し、そのアドレスを <see cref="IntPtr"/> 型にキャストして返してください。
			///  尚、ファイルオープン時にメモリの確保が必要な場合には、本関数内で動的にメモリの確保を行なってください。
			/// </para>
			/// <para>
			/// 注意:
			/// 戻り値のエラーコード（ <see cref="CriFs.IoError"/> ）には、関数内で継続不能なエラーが発生した 場合に限り <see cref="CriFs.IoError.Ng"/> をセットしてください。
			///  （ファイルのオープンに失敗した場合でも、アプリケーションで処理を継続可能な場合には filehn に null をセットし、<see cref="CriFs.IoError.Ok"/> を返す必要があります。）
			///  また、ディスク挿入待ち等の理由により、関数が実行されたタイミングでオープン処理 を実行できない場合、エラーコードとして <see cref="CriFs.IoError.TryAgain"/> を返すことで、 一定時間後（約10ms後）に再度オープン処理をやり直すことが可能です。
			///  （関数の実行タイミングを先送りすることが可能です。） 
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Open)(const CriChar8 *path"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<NativeString, CriFs.FileMode, CriFs.FileAccess, IntPtr*, CriFs.IoError> Open;

			/// <summary>ファイルのクローズ </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルオブジェクトをクローズする関数です。
			///  ファイルオープン時に動的にメモリの確保を行なった場合は、クローズ時にメモリを解放してください。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Close)(CriFsFileHn filehn)"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> Close;

			/// <summary>ファイルサイズの取得 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルオブジェクトから、当該ファイルのサイズを取得する関数です。
			/// </para>
			/// <para>
			/// 注意:
			/// この関数はメインスレッド上から直接実行される可能性があります。
			///  そのため、この関数の中で長時間処理をブロックすることは避ける必要があります。
			///  ファイルオブジェクトからファイルサイズを取得するのに時間がかかる場合には、 ファイルオープン時にあらかじめファイルサイズを取得（ファイルオブジェクト内に保持） しておき、本関数実行時にその値を返すよう関数を実装してください。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *GetFileSize)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, Int64*, CriFs.IoError> GetFileSize;

			/// <summary>読み込みの開始 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// データの読み込みを開始する関数です。
			///  offset で指定された位置から、 read_size で指定されたサイズ分だけデータを buffer に読み込みます。
			///  関数のインターフェイスとしては非同期I/O処理による実装を想定していますが、 スレッドを使用する場合（スレッドモデルに CRIFS_THREAD_MODEL_MULTI を指定する場合） には、この関数を同期I/O処理を使って実装しても問題ありません。
			///  （関数内でファイルの読み込みを完了するまで待っても問題ありません。）
			/// </para>
			/// <para>
			/// 注意:
			/// 実際に読み込めたサイズは、 GetReadSize 関数で返す必要があります。
			///  同期I/O処理により本関数を実装する場合でも、読み込めたサイズは GetReadSize 関数 が実行されるまで、ファイルオブジェクト内に保持する必要があります。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Read)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, Int64, Int64, IntPtr, Int64, CriFs.IoError> Read;

			/// <summary>読み込み完了チェック </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイルの読み込みが完了したかどうかを確認する関数です。
			///  ファイルの読み込みが完了した場合は true を、 読み込み途中の場合は false を result にセットする必要があります。
			/// </para>
			/// <para>
			/// 注意:
			/// result には、リード処理の成否に関係なく、リード処理が完了した時点 （デバイスへのアクセスが終了した時点）で true をセットする必要があります。
			///  リードエラーが発生した場合でも、 result に true をセットし、 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			///  （リード処理が成功したかどうかについては、 GetReadSize 関数で判別しています。）
			///  result に false を返す限りは、CRI File System ライブラリは他の読み込み要求を一切処理しません。
			///  （リードエラー発生時に result に false をセットし続けた場合、 ファイルのロードができなくなったり、オブジェクトの Destroy 関数から処理が復帰しなくなる可能性があります。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *IsReadComplete)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, NativeBool*, CriFs.IoError> IsReadComplete;

			/// <summary>ファイル読み込みのキャンセル発行 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デバイス側のファイル読み込みに対してキャンセルを発行し、即時に復帰する関数です。 戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			///  <see cref="CriFs.IoError.Ok"/>以外の値を返しても、 CRI File Systemの動作は<see cref="CriFs.IoError.Ok"/>を返した場合と同じです。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *CancelRead)(CriFsFileHn filehn)"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> CancelRead;

			/// <summary>読み込みサイズの取得 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// リード処理を行なった結果、実際にバッファーに読み込めたデータのサイズを返す関数です。
			///  ファイルの終端等では、 Read 関数で指定したサイズ分のデータが必ずしも読み込めるとは限りません。
			/// </para>
			/// <para>
			/// 注意:
			/// リードエラーが発生した場合、 read_size に -1 をセットし、 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。 
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *GetReadSize)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, Int64*, CriFs.IoError> GetReadSize;

			/// <summary>書き込みの開始 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// データの書き込みを開始する関数です。
			///  offset で指定された位置から、 write_size で指定されたサイズ分だけデータを buffer から書き込みます。
			///  関数のインターフェイスとしては非同期I/O処理による実装を想定していますが、 スレッドを使用する場合（スレッドモデルに CRIFS_THREAD_MODEL_MULTI を指定する場合） には、この関数を同期I/O処理を使って実装しても問題ありません。
			///  （関数内でファイルの書き込みを完了するまで待っても問題ありません。）
			/// </para>
			/// <para>
			/// 注意:
			/// 実際に書き込めたサイズは、 GetWriteSize 関数で返す必要があります。
			///  同期I/O処理により本関数を実装する場合でも、書き込めたサイズは GetWriteSize 関数 が実行されるまで、ファイルオブジェクト内に保持する必要があります。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Write)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, Int64, Int64, IntPtr, Int64, CriFs.IoError> Write;

			/// <summary>書き込み完了チェック </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイルの書き込みが完了したかどうかを確認する関数です。
			///  ファイルの書き込みが完了した場合は true を、 書き込み途中の場合は false を result にセットする必要があります。
			/// </para>
			/// <para>
			/// 注意:
			/// ライトエラーが発生した場合、 result に true をセットし、 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。 
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// result には、ライト処理の成否に関係なく、ライト処理が完了した時点 （デバイスへのアクセスが終了した時点）で true をセットする必要があります。
			///  ライトエラーが発生した場合でも、 result に true をセットし、 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			///  （ライト処理が成功したかどうかについては、 GetReadSize 関数で判別しています。）
			///  result に false を返す限りは、CRI File System ライブラリは他の読み込み要求を一切処理しません。
			///  （ライトエラー発生時に result に false をセットし続けた場合、 ファイルのロードができなくなったり、オブジェクトの Destroy 関数から処理が復帰しなくなる可能性があります。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *IsWriteComplete)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, NativeBool*, CriFs.IoError> IsWriteComplete;

			/// <summary>ファイル書き込みのキャンセル発行 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デバイス側のファイル書き込みに対してキャンセルを発行し、即時に復帰する関数です。 戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			///  <see cref="CriFs.IoError.Ok"/>以外の値を返しても、 CRI File Systemの動作は<see cref="CriFs.IoError.Ok"/>を返した場合と同じです。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *CancelWrite)(CriFsFileHn filehn)"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> CancelWrite;

			/// <summary>書き込みサイズの取得 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライト処理を行なった結果、実際にバッファーに読み込めたデータのサイズを返す関数です。
			/// </para>
			/// <para>
			/// 注意:
			/// ライトエラーが発生した場合、 write_size に -1 をセットし、 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。 
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *GetWriteSize)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, Int64*, CriFs.IoError> GetWriteSize;

			/// <summary>フラッシュの実行 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 書き込み用にバッファーリングされているデータを、 強制的にデバイスに書き出す処理を行う関数です。
			///  （ ANSI C 標準の API では fflush 関数に相当する処理です。）
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Flush)(CriFsFileHn filehn)"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> Flush;

			/// <summary>ファイルサイズの変更 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイルのサイズを指定したサイズに変更する関数です。
			/// </para>
			/// <para>
			/// 補足:
			/// 本関数は、DMA転送サイズの制限等によりデバイスへの書き込みがバイト単位で 行なえない場合に、ファイルサイズを補正するために使用します。
			///  そのため、書き込みがバイト単位で可能なデバイスについては、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *Resize)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, Int64, CriFs.IoError> Resize;

			/// <summary>ネイティブファイルハンドルの取得 </summary>
			/// <returns><see cref="CriFs.IoError"/> エラーコード </returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォームSDKで利用されるファイルのオブジェクトを取得する関数です。
			///  例えば、 ANSI C 標準の fopen 関数を使用してファイルをオープンした場合、 native_filehn としてファイルポインター（ FILE * ）を返す必要があります。
			/// </para>
			/// <para>
			/// 備考:
			/// 現状、PLAYSTATION3以外の機種ではこの関数を実装する必要はありません。
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *GetNativeFileHandle)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, CriFs.IoError> GetNativeFileHandle;

			/// <summary>読み込みプログレス加算コールバックの設定 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 本関数は、::criFsLoader_GetProgress で得られる進捗を、単位読み込みサイズより 細かい粒度で更新させるための、読み込みプログレス加算コールバックを設定する関数です。
			///  本関数を実装しない場合や、本関数で渡されたコールバック関数を使用しない場合、 ::criFsLoader_GetProgress で得られる進捗は、基本的に単位読み込みサイズ毎に更新されます。
			///  本関数を実装する場合は、渡されたコールバック関数を Read 関数内で呼び出してください。 また、呼び出す際には第一引数に obj、第二引数にメモリへの読み込みが完了したサイズを バイト単位で渡してください。
			///  例えば、リード要求をデバイス内で 8192byte ずつに分割して読み込む場合は、 8192byte の読み込み完了毎に、第二引数に 8192 を渡して呼び出してください。
			///  この、読み込みプログレス加算コールバック呼び出しによって ::criFsLoader_GetProgress で得られる進捗が更新されます。単位読み込みサイズより細かい粒度で更新を 行うことで ::criFsLoader_GetProgress で得られる進捗の粒度が細かくなります。
			/// </para>
			/// <para>
			/// 備考:
			/// 読み込みリクエストより細かい粒度で読み込み進捗を取得できない場合は、 実装するメリットはありません。 
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *SetAddReadProgressCallback)(CriFsFileHn filehn"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, Int32, void>, IntPtr, CriFs.IoError> SetAddReadProgressCallback;

			/// <summary>複数の同時ファイルアクセス要求が可能かどうかの問い合わせ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// このI/Oインターフェースが複数の同時ファイルアクセス要求が可能であるかどうかを返す関数です。
			///  本関数を実装しない場合、不可能であるとみなされます。
			///  本関数を実装されていて、result が true だった場合、 criFsLoader は効率よく複数ファイルのロードを行うために並列でリード要求を行うようになります。
			/// </para>
			/// <para>
			/// 備考:
			/// 並列でリード要求を行う場合、<see cref="CriFs.FileIoMode"/> が <see cref="CriFs.FileIoMode.OpenEveryTime"/> である必要があります。 
			/// </para>
			/// <nativeinfo declaration="CriFsIoInterface::CriFsIoError(CRIAPI *CanParallelRead)(CriBool *result)"/>
			/// </remarks>
			[System.NonSerialized] public delegate* unmanaged[Cdecl]<NativeBool*, CriFs.IoError> CanParallelRead;

		}
		/// <summary>I/Oインターフェイスのエラーコード </summary>
		/// <remarks>
		/// <para>Error of I/O Interface </para>
		/// </remarks>
		public enum IoError
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>エラーなし </para>
			/// </remarks>
			Ok = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>一般エラー </para>
			/// </remarks>
			Ng = -1,
			/// <summary></summary>
			/// <remarks>
			/// <para>リトライすべき </para>
			/// </remarks>
			TryAgain = -2,
			/// <summary></summary>
			/// <remarks>
			/// <para>個別エラー（ファイル無し） </para>
			/// </remarks>
			NgNoEntry = -11,
			/// <summary></summary>
			/// <remarks>
			/// <para>個別エラー（データが不正） </para>
			/// </remarks>
			NgInvalidData = -12,
			EnumBeSint32 = 2147483647,
		}
		/// <summary>ファイルオープンモード </summary>
		public enum FileMode
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>既存ファイルに追記 </para>
			/// </remarks>
			Append = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>ファイルの新規作成（既存のファイルは上書き） </para>
			/// </remarks>
			Create = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>ファイルの新規作成（上書き不可） </para>
			/// </remarks>
			CreateNew = 2,
			/// <summary></summary>
			/// <remarks>
			/// <para>既存ファイルのオープン </para>
			/// </remarks>
			Open = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>ファイルのオープン（存在しない場合は新規作成） </para>
			/// </remarks>
			OpenOrCreate = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>既存ファイルを0Byteに切り詰めてオープン </para>
			/// </remarks>
			Truncate = 5,
			EnumBeSint32 = 2147483647,
		}
		/// <summary>ファイルアクセス種別 </summary>
		public enum FileAccess
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>読み込みのみ </para>
			/// </remarks>
			Read = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>書き込みのみ </para>
			/// </remarks>
			Write = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>読み書き </para>
			/// </remarks>
			ReadWrite = 2,
			EnumBeSint32 = 2147483647,
		}

		/// <summary>ファイルオープンエラー発生時のリトライ方法 </summary>
		/// <remarks>
		/// <para>ファイルリードエラー発生時のリトライ方法</para>
		/// </remarks>
		public enum FileIoMode
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>機種デフォルトのファイルI/Oモード </para>
			/// </remarks>
			Default = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>ファイルハンドルを共有する </para>
			/// </remarks>
			ShareFileHandle = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>ファイルアクセスごとにファイルのオープンを行う </para>
			/// </remarks>
			OpenEveryTime = 2,
			EnumBeSint32 = 2147483647,
		}
	}
}