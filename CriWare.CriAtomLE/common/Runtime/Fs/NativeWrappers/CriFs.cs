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
		/// <summary>デフォルトデバイスID</summary>
		public const CriFs.DeviceId DeviceDefault = (CriFs.DeviceId._00);
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
		/// <summary>I/O選択コールバックの登録</summary>
		/// <param name="func">I/O選択コールバック</param>
		/// <returns>エラーコード</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// I/O選択コールバック関数（ <see cref="CriFs.SelectIoCbFunc"/> ）を登録します。
		/// CRI File Systemライブラリはファイルにアクセスする際、まず初めに、そのファイルが存在するデバイスのID（ <see cref="CriFs.DeviceId"/> ）と、
		/// デバイスにアクセスするためのI/Oインターフェイス（ <see cref="CriFs.IoInterface"/> ）を選択します。
		/// デフォルト状態では、デバイスIDとI/Oインターフェイスの選択はライブラリ内で暗黙的に行なわれますが、
		/// 本関数を使用することで、デバイスIDとI/Oインターフェイスをユーザーが自由に指定することが可能になります。
		/// これにより、ユーザーが独自に作成したI/Oインターフェイスを使用してファイルにアクセスすることが可能になります。
		/// 注意:
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定するとことで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.SelectIoCbFunc"/>
		public static unsafe CriErr.Error SetSelectIoCallback(delegate* unmanaged[Cdecl]<NativeString, CriFs.DeviceId*, NativeReference<CriFs.IoInterface>*, CriErr.Error> func)
		{
			return NativeMethods.criFs_SetSelectIoCallback((IntPtr)func);
		}
		static unsafe void SetSelectIoCallbackInternal(IntPtr func) => SetSelectIoCallback((delegate* unmanaged[Cdecl]<NativeString, CriFs.DeviceId*, NativeReference<CriFs.IoInterface>*, CriErr.Error>)func);
		static CriFs.SelectIoCbFunc _selectIoCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetSelectIoCallback" />
		public static CriFs.SelectIoCbFunc SelectIoCallback => _selectIoCallback ?? (_selectIoCallback = new CriFs.SelectIoCbFunc(SetSelectIoCallbackInternal));

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
				/// <summary>ファイルのパス</summary>
				public NativeString path { get; }
				/// <summary>デバイスID</summary>
				public NativeReference<CriFs.DeviceId> deviceId { get; }
				/// <summary>I/Oインターフェイス</summary>
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
			static CriErr.Error CriFsSelectIoCbFuncCallbackFunc(NativeString path, CriFs.DeviceId* deviceId, NativeReference<CriFs.IoInterface>* ioif) =>
				InvokeCallbackInternal(default, new(path, deviceId, ioif));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate CriErr.Error NativeDelegate(NativeString path, CriFs.DeviceId* deviceId, NativeReference<CriFs.IoInterface>* ioif);
			static NativeDelegate callbackDelegate = null;
#endif
			internal SelectIoCbFunc(Action<IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<NativeString, CriFs.DeviceId*, NativeReference<CriFs.IoInterface>*, CriErr.Error>)&CriFsSelectIoCbFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriFsSelectIoCbFuncCallbackFunc)
#endif
				)
			{ }
		}
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
		/// <summary>I/O Interface</summary>
		public unsafe partial struct IoInterface
		{
			/// <summary>ファイルの有無の確認</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルの有無を確認する関数です。
			/// ファイルが存在する場合は true を、
			/// 存在しない場合は false を result にセットする必要があります。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<NativeString, NativeBool*, CriFs.IoError> Exists;

			/// <summary>ファイルの削除</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルを削除する関数です。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<NativeString, CriFs.IoError> Remove;

			/// <summary>ファイル名の変更</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイル名の変更を行なう関数です。
			/// old_path で指定されたファイルを、 new_path にリネームします。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<NativeString, NativeString, CriFs.IoError> Rename;

			/// <summary>ファイルのオープン</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルをオープンする関数です。
			/// オープンに成功した場合、<see cref="CriFs.FileHn"/> 型のファイルオブジェクトを返す必要があります。
			/// </para>
			/// <para>
			/// 補足:
			/// <see cref="CriFs.FileHn"/> は void ポインターとして定義されています。
			/// 独自のファイル情報構造体を定義し、そのアドレスを <see cref="CriFs.FileHn"/> 型にキャストして返してください。
			/// 尚、ファイルオープン時にメモリの確保が必要な場合には、本関数内で動的にメモリの確保を行なってください。
			/// </para>
			/// <para>
			/// 注意:
			/// 戻り値のエラーコード（ <see cref="CriFs.IoError"/> ）には、関数内で継続不能なエラーが発生した
			/// 場合に限り <see cref="CriFs.IoError.Ng"/> をセットしてください。
			/// （ファイルのオープンに失敗した場合でも、アプリケーションで処理を継続可能な場合には
			/// filehn に null をセットし、<see cref="CriFs.IoError.Ok"/> を返す必要があります。）
			/// また、ディスク挿入待ち等の理由により、関数が実行されたタイミングでオープン処理
			/// を実行できない場合、エラーコードとして <see cref="CriFs.IoError.TryAgain"/> を返すことで、
			/// 一定時間後（約10ms後）に再度オープン処理をやり直すことが可能です。
			/// （関数の実行タイミングを先送りすることが可能です。）
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<NativeString, CriFs.FileMode, CriFs.FileAccess, IntPtr*, CriFs.IoError> Open;

			/// <summary>ファイルのクローズ</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルオブジェクトをクローズする関数です。
			/// ファイルオープン時に動的にメモリの確保を行なった場合は、クローズ時にメモリを解放してください。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> Close;

			/// <summary>ファイルサイズの取得</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 指定されたファイルオブジェクトから、当該ファイルのサイズを取得する関数です。
			/// </para>
			/// <para>
			/// 注意:
			/// この関数はメインスレッド上から直接実行される可能性があります。
			/// そのため、この関数の中で長時間処理をブロックすることは避ける必要があります。
			/// ファイルオブジェクトからファイルサイズを取得するのに時間がかかる場合には、
			/// ファイルオープン時にあらかじめファイルサイズを取得（ファイルオブジェクト内に保持）
			/// しておき、本関数実行時にその値を返すよう関数を実装してください。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, Int64*, CriFs.IoError> GetFileSize;

			/// <summary>読み込みの開始</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// データの読み込みを開始する関数です。
			/// offset で指定された位置から、 read_size で指定されたサイズ分だけデータを
			/// buffer に読み込みます。
			/// 関数のインターフェイスとしては非同期I/O処理による実装を想定していますが、
			/// スレッドを使用する場合（スレッドモデルに CRIFS_THREAD_MODEL_MULTI を指定する場合）
			/// には、この関数を同期I/O処理を使って実装しても問題ありません。
			/// （関数内でファイルの読み込みを完了するまで待っても問題ありません。）
			/// </para>
			/// <para>
			/// 注意:
			/// 実際に読み込めたサイズは、 GetReadSize 関数で返す必要があります。
			/// 同期I/O処理により本関数を実装する場合でも、読み込めたサイズは GetReadSize 関数
			/// が実行されるまで、ファイルオブジェクト内に保持する必要があります。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, Int64, Int64, IntPtr, Int64, CriFs.IoError> Read;

			/// <summary>読み込み完了チェック</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイルの読み込みが完了したかどうかを確認する関数です。
			/// ファイルの読み込みが完了した場合は true を、
			/// 読み込み途中の場合は false を result にセットする必要があります。
			/// </para>
			/// <para>
			/// 注意:
			/// result には、リード処理の成否に関係なく、リード処理が完了した時点
			/// （デバイスへのアクセスが終了した時点）で true をセットする必要があります。
			/// リードエラーが発生した場合でも、 result に true をセットし、
			/// 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			/// （リード処理が成功したかどうかについては、 GetReadSize 関数で判別しています。）
			/// result に false を返す限りは、CRI File System
			/// ライブラリは他の読み込み要求を一切処理しません。
			/// （リードエラー発生時に result に false をセットし続けた場合、
			/// ファイルのロードができなくなったり、オブジェクトの
			/// Destroy 関数から処理が復帰しなくなる可能性があります。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, NativeBool*, CriFs.IoError> IsReadComplete;

			/// <summary>ファイル読み込みのキャンセル発行</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デバイス側のファイル読み込みに対してキャンセルを発行し、即時に復帰する関数です。
			/// 戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			/// <see cref="CriFs.IoError.Ok"/>以外の値を返しても、
			/// CRI File Systemの動作は<see cref="CriFs.IoError.Ok"/>を返した場合と同じです。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> CancelRead;

			/// <summary>読み込みサイズの取得</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// リード処理を行なった結果、実際にバッファーに読み込めたデータのサイズを返す関数です。
			/// ファイルの終端等では、 Read 関数で指定したサイズ分のデータが必ずしも読み込めるとは限りません。
			/// </para>
			/// <para>
			/// 注意:
			/// リードエラーが発生した場合、 read_size に -1 をセットし、
			/// 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, Int64*, CriFs.IoError> GetReadSize;

			/// <summary>書き込みの開始</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// データの書き込みを開始する関数です。
			/// offset で指定された位置から、 write_size で指定されたサイズ分だけデータを
			/// buffer から書き込みます。
			/// 関数のインターフェイスとしては非同期I/O処理による実装を想定していますが、
			/// スレッドを使用する場合（スレッドモデルに CRIFS_THREAD_MODEL_MULTI を指定する場合）
			/// には、この関数を同期I/O処理を使って実装しても問題ありません。
			/// （関数内でファイルの書き込みを完了するまで待っても問題ありません。）
			/// </para>
			/// <para>
			/// 注意:
			/// 実際に書き込めたサイズは、 GetWriteSize 関数で返す必要があります。
			/// 同期I/O処理により本関数を実装する場合でも、書き込めたサイズは GetWriteSize 関数
			/// が実行されるまで、ファイルオブジェクト内に保持する必要があります。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, Int64, Int64, IntPtr, Int64, CriFs.IoError> Write;

			/// <summary>書き込み完了チェック</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイルの書き込みが完了したかどうかを確認する関数です。
			/// ファイルの書き込みが完了した場合は true を、
			/// 書き込み途中の場合は false を result にセットする必要があります。
			/// </para>
			/// <para>
			/// 注意:
			/// ライトエラーが発生した場合、 result に true をセットし、
			/// 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// result には、ライト処理の成否に関係なく、ライト処理が完了した時点
			/// （デバイスへのアクセスが終了した時点）で true をセットする必要があります。
			/// ライトエラーが発生した場合でも、 result に true をセットし、
			/// 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			/// （ライト処理が成功したかどうかについては、 GetReadSize 関数で判別しています。）
			/// result に false を返す限りは、CRI File System
			/// ライブラリは他の読み込み要求を一切処理しません。
			/// （ライトエラー発生時に result に false をセットし続けた場合、
			/// ファイルのロードができなくなったり、オブジェクトの
			/// Destroy 関数から処理が復帰しなくなる可能性があります。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, NativeBool*, CriFs.IoError> IsWriteComplete;

			/// <summary>ファイル書き込みのキャンセル発行</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デバイス側のファイル書き込みに対してキャンセルを発行し、即時に復帰する関数です。
			/// 戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			/// <see cref="CriFs.IoError.Ok"/>以外の値を返しても、
			/// CRI File Systemの動作は<see cref="CriFs.IoError.Ok"/>を返した場合と同じです。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> CancelWrite;

			/// <summary>書き込みサイズの取得</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライト処理を行なった結果、実際にバッファーに読み込めたデータのサイズを返す関数です。
			/// </para>
			/// <para>
			/// 注意:
			/// ライトエラーが発生した場合、 write_size に -1 をセットし、
			/// 関数の戻り値は <see cref="CriFs.IoError.Ok"/> を返してください。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, Int64*, CriFs.IoError> GetWriteSize;

			/// <summary>フラッシュの実行</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 書き込み用にバッファーリングされているデータを、
			/// 強制的にデバイスに書き出す処理を行う関数です。
			/// （ ANSI C 標準の API では fflush 関数に相当する処理です。）
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, CriFs.IoError> Flush;

			/// <summary>ファイルサイズの変更</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ファイルのサイズを指定したサイズに変更する関数です。
			/// </para>
			/// <para>
			/// 補足:
			/// 本関数は、DMA転送サイズの制限等によりデバイスへの書き込みがバイト単位で
			/// 行なえない場合に、ファイルサイズを補正するために使用します。
			/// そのため、書き込みがバイト単位で可能なデバイスについては、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// デバイスで書き込みを行なわない場合には、この関数を実装せず、
			/// 構造体のメンバーに CRI_NULL を指定することも可能です。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, Int64, CriFs.IoError> Resize;

			/// <summary>ネイティブファイルオブジェクトの取得</summary>
			/// <returns>エラーコード</returns>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォームSDKで利用されるファイルのオブジェクトを取得する関数です。
			/// 例えば、 ANSI C 標準の fopen 関数を使用してファイルをオープンした場合、
			/// native_filehn としてファイルポインター（ FILE * ）を返す必要があります。
			/// </para>
			/// <para>
			/// 備考:
			/// 現状、PLAYSTATION3以外の機種ではこの関数を実装する必要はありません。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, CriFs.IoError> GetNativeFileHandle;

			/// <summary>読み込みプログレス加算コールバックの設定</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 本関数は、::criFsLoader_GetProgress で得られる進捗を、単位読み込みサイズより
			/// 細かい粒度で更新させるための、読み込みプログレス加算コールバックを設定する関数です。
			/// 本関数を実装しない場合や、本関数で渡されたコールバック関数を使用しない場合、
			/// ::criFsLoader_GetProgress で得られる進捗は、基本的に単位読み込みサイズ毎に更新されます。
			/// 本関数を実装する場合は、渡されたコールバック関数を Read 関数内で呼び出してください。
			/// また、呼び出す際には第一引数に obj、第二引数にメモリへの読み込みが完了したサイズを
			/// バイト単位で渡してください。
			/// 例えば、リード要求をデバイス内で 8192byte ずつに分割して読み込む場合は、
			/// 8192byte の読み込み完了毎に、第二引数に 8192 を渡して呼び出してください。
			/// この、読み込みプログレス加算コールバック呼び出しによって ::criFsLoader_GetProgress
			/// で得られる進捗が更新されます。単位読み込みサイズより細かい粒度で更新を
			/// 行うことで ::criFsLoader_GetProgress で得られる進捗の粒度が細かくなります。
			/// </para>
			/// <para>
			/// 備考:
			/// 読み込みリクエストより細かい粒度で読み込み進捗を取得できない場合は、
			/// 実装するメリットはありません。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, Int32, void>, IntPtr, CriFs.IoError> SetAddReadProgressCallback;

			/// <summary>複数の同時ファイルアクセス要求が可能かどうかの問い合わせ</summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// このI/Oインターフェースが複数の同時ファイルアクセス要求が可能であるかどうかを返す関数です。
			/// 本関数を実装しない場合、不可能であるとみなされます。
			/// 本関数を実装されていて、result が true だった場合、
			/// criFsLoader は効率よく複数ファイルのロードを行うために並列でリード要求を行うようになります。
			/// </para>
			/// <para>
			/// 備考:
			/// 並列でリード要求を行う場合、CriFsFileIoMode が CRIFS_FILE_IO_MODE_OPEN_EVERY_TIME である必要があります。
			/// </para>
			/// </remarks>
			public delegate* unmanaged[Cdecl]<NativeBool*, CriFs.IoError> CanParallelRead;

		}
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
		/// <summary>File Opening Mode</summary>
		public enum FileMode
		{
			/// <summary>既存ファイルに追記								*/	/*EN< Appends to an existing file</summary>
			Append = 0,
			/// <summary>ファイルの新規作成（既存のファイルは上書き）		*/	/*EN< Creates a new file always</summary>
			Create = 1,
			/// <summary>ファイルの新規作成（上書き不可）					*/	/*EN< Creates a new file (Can not overwrite)</summary>
			CreateNew = 2,
			/// <summary>既存ファイルのオープン							*/	/*EN< Opens an existing file</summary>
			Open = 3,
			/// <summary>ファイルのオープン（存在しない場合は新規作成）	*/	/*EN< Opens a file if available (Or creates new file)</summary>
			OpenOrCreate = 4,
			/// <summary>既存ファイルを0Byteに切り詰めてオープン			*/	/*EN< Opens a file and truncates it</summary>
			Truncate = 5,
			/// <summary>enum be 4bytes</summary>
			EnumBeSint32 = 2147483647,
		}
		/// <summary>Kind of File Access</summary>
		public enum FileAccess
		{
			/// <summary>読み込みのみ		*/	/*EN< Read Only</summary>
			Read = 0,
			/// <summary>書き込みのみ		*/	/*EN< Write Only</summary>
			Write = 1,
			/// <summary>読み書き			*/	/*EN< Read and Write</summary>
			ReadWrite = 2,
			/// <summary>enum be 4bytes</summary>
			EnumBeSint32 = 2147483647,
		}

	}
}