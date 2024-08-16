/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using CriWare.InteropHelpers;

namespace CriWare
{
#pragma warning disable 0465
	/// <summary>CriFs API</summary>
	public static partial class CriFs
	{
		/// <summary>コンフィギュレーション</summary>
		/// <remarks>
		/// <para header="説明">
		/// CRI File Systemライブラリの動作仕様を指定するための構造体です。
		/// ライブラリ初期化時（ ::criFs_InitializeLibrary 関数）に引数として本構造体を指定します。
		/// </para>
		/// <para header="CRI File Systemライブラリは、初期化時に指定されたコンフィギュレーションに応じて、内部リソースを必要な数分だけ確保します。">
		/// そのため、コンフィギュレーションに指定する値を小さくすることで、ライブラリが必要とするメモリのサイズを小さく抑えることが可能です。
		/// ただし、コンフィギュレーションに指定した数以上のオブジェクトを確保することはできなくなるため、値を小さくしすぎると、オブジェクトの確保に失敗する可能性があります。
		/// </para>
		/// <para header="備考">デフォルト設定を使用する場合、 ::criFs_SetDefaultConfig 関数でデフォルトパラメータをセットし、 ::criFs_InitializeLibrary 関数に指定してください。</para>
		/// <para header="注意">将来的にメンバーが増える可能性に備え、設定前に::criFs_SetDefaultConfig 関数で初期化してから使用してください。<br/></para>
		/// </remarks>
		public unsafe partial struct Config
		{
			/// <summary>スレッドモデル</summary>
			/// <remarks>
			/// <para header="説明">CRI File Systemのスレッドモデルを指定します。</para>
			/// </remarks>
			/// <seealso cref="CriFs.ThreadModel"/>
			public CriFs.ThreadModel threadModel;

			/// <summary>使用するCriFsBinderの数</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中で使用するバインダー（CriFsBinder）の数を指定します。
			/// アプリケーション中で ::criFsBinder_Create 関数を使用してバインダーを作成する場合、
			/// 本パラメータに使用するバインダーの数を指定する必要があります。
			/// num_bindersには「同時に使用するバインダーの最大数」を指定します。
			/// 例えば、 ::criFsBinder_Create 関数と <see cref="CriFsBinder.Dispose"/> 関数を交互に続けて実行するケースにおいては、
			/// 最大同時には1つのバインダーしか使用しないため、関数の呼び出し回数に関係なくnum_bindersに1を指定することが可能です。
			/// 逆に、ある場面でバインダーを10個使用する場合には、その他の場面でバインダーを全く使用しない場合であっても、
			/// num_bindersに10を指定する必要があります。
			/// </para>
			/// <para header="備考">
			/// CRI File Systemライブラリは、使用するバインダーの数分だけのメモリを初期化時に要求します。
			/// そのため、num_bindersに必要最小限の値をセットすることで、ライブラリが必要とするメモリのサイズを抑えることが可能です。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriFsBinder.Dispose"/>
			public Int32 numBinders;

			/// <summary>使用するCriFsLoaderの数</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中で使用するローダー（CriFsLoader）の数を指定します。
			/// アプリケーション中で ::criFsLoader_Create 関数を使用してローダーを作成する場合、
			/// 本パラメータに使用するローダーの数を指定する必要があります。
			/// num_loadersには「同時に使用するローダーの最大数」を指定します。
			/// 例えば、 ::criFsLoader_Create 関数と ::criFsLoader_Destroy 関数を交互に続けて実行するケースにおいては、
			/// 最大同時には1つのローダーしか使用しないため、関数の呼び出し回数に関係なくnum_loadersに1を指定することが可能です。
			/// 逆に、ある場面でローダーを10個使用する場合には、その他の場面でローダーを全く使用しない場合であっても、
			/// num_loadersに10を指定する必要があります。
			/// </para>
			/// <para header="備考">
			/// CRI File Systemライブラリは、使用するローダーの数分だけのメモリを初期化時に要求します。
			/// そのため、num_loadersに必要最小限の値をセットすることで、ライブラリが必要とするメモリのサイズを抑えることが可能です。
			/// </para>
			/// </remarks>
			public Int32 numLoaders;

			/// <summary>使用するCriFsGroupLoaderの数</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中で使用するグループローダー（CriFsGroupLoader）の数を指定します。
			/// アプリケーション中で ::criFsGroupLoader_Create 関数を使用してグループローダーを作成する場合、
			/// 本パラメータに使用するグループローダーの数を指定する必要があります。
			/// num_group_loadersには「同時に使用するグループローダーの最大数」を指定します。
			/// 例えば、 ::criFsGroupLoader_Create 関数と ::criFsGroupLoader_Destroy 関数を交互に続けて実行するケースにおいては、
			/// 最大同時には1つのグループローダーしか使用しないため、関数の呼び出し回数に関係なくnum_group_loadersに1を指定することが可能です。
			/// 逆に、ある場面でグループローダーを10個使用する場合には、その他の場面でグループローダーを全く使用しない場合であっても、
			/// num_group_loadersに10を指定する必要があります。
			/// </para>
			/// <para header="備考">
			/// CRI File Systemライブラリは、使用するグループローダーの数分だけのメモリを初期化時に要求します。
			/// そのため、num_group_loadersに必要最小限の値をセットすることで、ライブラリが必要とするメモリのサイズを抑えることが可能です。
			/// </para>
			/// </remarks>
			public Int32 numGroupLoaders;

			/// <summary>使用するCriFsStdioの数</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中で使用するCriFsStdioオブジェクトの数を指定します。
			/// アプリケーション中で ::criFsStdio_OpenFile 関数を使用してCriFsStdioオブジェクトを作成する場合、
			/// 本パラメータに使用するCriFsStdioオブジェクトの数を指定する必要があります。
			/// num_stdio_handlesには「同時に使用するCriFsStdioオブジェクトの最大数」を指定します。
			/// 例えば、 ::criFsStdio_OpenFile 関数と ::criFsStdio_CloseFile 関数を交互に続けて実行するケースにおいては、
			/// 最大同時には1つのCriFsStdioオブジェクトしか使用しないため、関数の呼び出し回数に関係なくnum_stdio_handlesに1を指定することが可能です。
			/// 逆に、ある場面でCriFsStdioオブジェクトを10個使用する場合には、その他の場面でCriFsStdioオブジェクトを全く使用しない場合であっても、
			/// num_stdio_handlesに10を指定する必要があります。
			/// </para>
			/// <para header="備考">
			/// CRI File Systemライブラリは、使用するCriFsStdioオブジェクトの数分だけのメモリを初期化時に要求します。
			/// そのため、num_stdio_handlesに必要最小限の値をセットすることで、ライブラリが必要とするメモリのサイズを抑えることが可能です。
			/// </para>
			/// <para header="注意">
			/// ブリッジライブラリを使用してADXライブラリや救声主ライブラリを併用する場合、<br/>
			/// ADXTオブジェクトやcriSsPlyオブジェクトは内部的にCriFsStdioオブジェクトを作成します。<br/>
			/// そのため、ブリッジライブラリを使用する場合には、CRI File Systemライブラリ初期化時に<br/>
			/// num_stdio_handlesにADXTオブジェクトやcriSsPlyオブジェクトの数を加えた値を指定してください。<br/>
			/// </para>
			/// </remarks>
			public Int32 numStdioHandles;

			/// <summary>使用するCriFsInstallerの数</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中で使用するインストーラー（CriFsInstaller）の数を指定します。
			/// アプリケーション中で criFsInstaller_Create 関数を使用してインストーラーを作成する場合、
			/// 本パラメータに使用するインストーラーの数を指定する必要があります。
			/// num_installersには「同時に使用するインストーラーの最大数」を指定します。
			/// 例えば、 criFsInstaller_Create 関数と criFsInstaller_Destroy 関数を交互に続けて実行するケースにおいては、
			/// 最大同時には1つのインストーラーしか使用しないため、関数の呼び出し回数に関係なくnum_installersに1を指定することが可能です。
			/// 逆に、ある場面でインストーラーを10個使用する場合には、その他の場面でインストーラーを全く使用しない場合であっても、
			/// num_installersに10を指定する必要があります。
			/// </para>
			/// <para header="備考">
			/// CRI File Systemライブラリは、使用するインストーラーの数分だけのメモリを初期化時に要求します。
			/// そのため、num_installersに必要最小限の値をセットすることで、ライブラリが必要とするメモリのサイズを抑えることが可能です。
			/// </para>
			/// <para header="注意">
			/// ::criFs_SetDefaultConfig メソッドを使用してコンフィギュレーションを初期化する場合、num_installersの数は0に設定されます。<br/>
			/// そのため、インストーラーを使用する場合には、アプリケーション中でnum_installersを明示的に指定する必要があります。<br/>
			/// </para>
			/// </remarks>
			public Int32 numInstallers;

			/// <summary>最大同時バインド数</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中でバインド処理を行い、保持するバインドID（CriFsBindId）の数を指定します。
			/// アプリケーション中で ::criFsBinder_BindCpk 関数等を使用してバインド処理を行う場合、
			/// 本パラメータに使用するバインドIDの数を指定する必要があります。
			/// max_bindsには「同時に使用するバインドIDの最大数」を指定します。
			/// 例えば、 ::criFsBinder_BindCpk 関数と ::criFsBinder_Unbind 関数を交互に続けて実行するケースにおいては、
			/// 最大同時には1つのバインドIDしか使用しないため、関数の呼び出し回数に関係なくmax_bindsに1を指定することが可能です。
			/// 逆に、ある場面でバインドIDを10個使用する場合には、その他の場面でバインドを一切行わない場合であっても、
			/// max_bindsに10を指定する必要があります。
			/// </para>
			/// <para header="備考">
			/// CRI File Systemライブラリは、使用するバインドIDの数分だけのメモリを初期化時に要求します。
			/// そのため、max_bindsに必要最小限の値をセットすることで、ライブラリが必要とするメモリのサイズを抑えることが可能です。
			/// </para>
			/// </remarks>
			public Int32 maxBinds;

			/// <summary>最大同時オープンファイル数</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中でオープンするファイルの数を指定します。
			/// アプリケーション中で ::criFsStdio_OpenFile 関数等を使用してファイルをオープンする場合、
			/// 本パラメータにオープンするファイルの数を指定する必要があります。
			/// max_filesには「同時にオープンするファイルの最大数」を指定します。
			/// 例えば、 ::criFsStdio_OpenFile 関数と ::criFsStdio_CloseFile 関数を交互に続けて実行するケースにおいては、
			/// 最大同時には1つのファイルしかオープンしないため、関数の呼び出し回数に関係なくmax_filesに1を指定することが可能です。
			/// 逆に、ある場面でファイルを10個オープンする場合には、その他の場面でファイルを1つしかオープンしない場合であっても、
			/// max_filesに10を指定する必要があります。
			/// </para>
			/// <para header="補足">
			/// CRI File Systemライブラリは、以下の関数を実行した場合にファイルをオープンします。
			/// ## ファイルがオープンされる場面
			/// |関数					|備考	|
			/// | --- | --- |
			/// |criFsBinder_BindCpk	|オープンされるファイルの数は1つ。 criFsBinder_Unbind 関数が実行されるまでの間ファイルはオープンされ続ける。	|
			/// |criFsBinder_BindFile	|オープンされるファイルの数は1つ。 criFsBinder_Unbind 関数が実行されるまでの間ファイルはオープンされ続ける。	|
			/// |criFsBinder_BindFiles	|リストに含まれる数分ファイルがオープンされる。 criFsBinder_Unbind 関数が実行されるまでファイルはオープンされ続ける。	|
			/// |criFsLoader_Load		|オープンされるファイルの数は1つ。 ロードが完了するまでの間ファイルはオープンされ続ける。 バインダーを指定した場合、ファイルはオープンされない（バインダーが既にオープン済みのため）。	|
			/// |criFsStdio_OpenFile	|オープンされるファイルの数は1つ。 criFsStdio_CloseFile 関数が実行されるまでの間ファイルはオープンされ続ける。 バインダーを指定した場合、ファイルはオープンされない（バインダーが既にオープン済みのため）。	|
			/// |criFsInstaller_Copy	|オープンされるファイルの数は2つ。 ファイルコピーが完了するまでの間ファイルはオープンされ続ける。 バインダーを指定した場合、オープンされるファイルは1つになる（1つをバインダーが既にオープン済みのため）。	|
			/// </para>
			/// <para header="注意">
			/// ブリッジライブラリを使用してADXライブラリや救声主ライブラリを併用する場合、<br/>
			/// ADXTオブジェクトやcriSsPlyオブジェクトは内部的にCriFsStdioオブジェクトを作成します。<br/>
			/// そのため、ブリッジライブラリを使用する場合には、CRI File Systemライブラリ初期化時に<br/>
			/// max_filesにADXTオブジェクトやcriSsPlyオブジェクトの数を加えた値を指定してください。<br/>
			/// </para>
			/// </remarks>
			public Int32 maxFiles;

			/// <summary>パスの最大長（バイト単位）</summary>
			/// <remarks>
			/// <para header="説明">
			/// アプリケーション中で指定するファイルパスの最大長を指定します。
			/// アプリケーション中で ::criFsLoader_Load 関数等を使用してファイルにアクセスする場合、
			/// 本パラメータにアプリケーションで使用するパス文字列の最大長を指定する必要があります。
			/// max_pathには「使用するパス文字列の最大数」を指定します。
			/// ある場面で256バイトのファイルパスを使用する場合、その他の場面で32バイトのファイルパスしか使わない場合でも、
			/// max_pathには256を指定する必要があります。
			/// </para>
			/// <para header="備考">
			/// パスの最大長には、終端のnull文字を含んだ数を指定する必要があります。
			/// （「文字数＋１バイト」の値を指定する必要があります。）
			/// </para>
			/// <para header="注意">PC等、ユーザーがアプリケーションを自由な場所にインストール可能な場合には、想定される最大サイズを max_path に指定する必要がありますので、ご注意ください。<br/></para>
			/// </remarks>
			public Int32 maxPath;

			/// <summary>ライブラリバージョン番号</summary>
			/// <remarks>
			/// <para header="説明">
			/// CRI File Systemライブラリのバージョン番号です。
			/// ::criFs_SetDefaultConfig 関数により、本ヘッダーに定義されているバージョン番号が設定されます。
			/// </para>
			/// <para header="注意">アプリケーションでは、この値を変更しないでください。<br/></para>
			/// </remarks>
			public UInt32 version;

			/// <summary>ライブラリバージョン文字列</summary>
			/// <remarks>
			/// <para header="説明">
			/// CRI File Systemライブラリのバージョン文字列です。
			/// ::criFs_SetDefaultConfig 関数により、本ヘッダーに定義されているバージョン文字列が設定されます。
			/// </para>
			/// <para header="注意">アプリケーションでは、この値を変更しないでください。<br/></para>
			/// </remarks>
			public NativeString versionString;

			/// <summary>CPKファイルのCRCチェックを行うかどうか</summary>
			/// <remarks>
			/// <para header="説明">
			/// CPKファイル内のCRC情報を使用し、データ整合性チェックを行うかをどうかを切り替えるフラグです。
			/// 本フラグを true に設定すると、以下のタイミングでCRCチェックを行います。
			/// - CPKバインド時にTOC情報のCRCチェック
			/// - コンテンツファイルロード時にコンテンツファイル単位のCRCチェック
			/// CPKに付加されたCRC情報と、実際に読みこんだデータのCRCが一致しない場合、エラーとなります。
			/// </para>
			/// </remarks>
			public NativeBool enableCrcCheck;

		}
		/// <summary>スレッドモデル</summary>
		/// <remarks>
		/// <para header="説明">
		/// CRI File Systemライブラリがどのようなスレッドモデルで動作するかを表します。
		/// ライブラリ初期化時（::criFs_InitializeLibrary 関数）に、<see cref="CriFs.Config"/> 構造体にて指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriFs.Config"/>
		public enum ThreadModel
		{
			/// <summary>EN</summary>
			/// <summary>Multithread</summary>
			/// <remarks>
			/// <para header="Description">
			/// The library creates threads inside and operates in multithread environment.
			/// A thread is created when the ::criFs_InitializeLibrary function is called.
			/// </para>
			/// </remarks>
			Multi = 0,
			/// <summary>マルチスレッド（ユーザー駆動式）</summary>
			/// <remarks>
			/// <para header="説明">
			/// ライブラリは内部でスレッドを作成し、マルチスレッドにて動作します。
			/// スレッドは ::criFs_InitializeLibrary 関数呼び出し時に作成されます。
			/// サーバー処理自体は作成されたスレッド上で実行されますが、
			/// <see cref="CriFs.ThreadModel.Multi"/> とは異なり、自動的には実行されません。
			/// ユーザーは ::criFs_ExecuteMain 関数で明示的にサーバー処理を駆動する必要があります。
			/// （  ::criFs_ExecuteMain 関数を実行すると、スレッドが起動し、サーバー処理が実行されます。）
			/// </para>
			/// </remarks>
			MultiUserDriven = 3,
			/// <summary>EN</summary>
			/// <summary>User multithread</summary>
			/// <remarks>
			/// <para header="Description">No thread is created but exclusion control is performed inside the library for the server processing functions (::criFs_ExecuteFileAccess, ::criFs_ExecuteDataDecompression) to be able to be called from a user-created thread.</para>
			/// </remarks>
			UserMulti = 1,
			/// <summary>EN</summary>
			/// <summary>Single thread</summary>
			/// <remarks>
			/// <para header="Description">
			/// No thread is created inside the library. Exclusion control is not performed inside the library either.
			/// When selecting this model, call the APIs and server processing functions (::criFs_ExecuteFileAccess, ::criFs_ExecuteDataDecompression) from the same thread.
			/// </para>
			/// </remarks>
			Single = 2,
			/// <summary>enum be 4bytes</summary>
			EnumBeSint32 = 2147483647,
		}
	}
}