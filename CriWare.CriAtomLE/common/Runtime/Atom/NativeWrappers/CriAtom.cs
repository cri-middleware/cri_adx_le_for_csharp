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
	/// <summary>CriAtom API</summary>
	public static partial class CriAtom
	{
		/// <summary>ライブラリ初期化用ワーク領域サイズの計算 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <returns>CriSint32 ワーク領域サイズ </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリを使用するために必要な、ワーク領域のサイズを取得します。
		///  ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		///  ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// CRI Atomライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ 構造体（ <see cref="CriAtom.Config"/> ）の内容によって変化します。
		///  引数にnullを指定した場合、デフォルト設定 （ <see cref="CriAtom.SetDefaultConfig"/> 適用時と同じパラメーター）で ワーク領域サイズを計算します。 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 CRIAPI criAtom_CalculateWorkSize(const CriAtomConfig *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Config"/>
		/// <seealso cref="CriAtom.Initialize"/>
		public static unsafe Int32 CalculateWorkSize(in CriAtom.Config config)
		{
			fixed (CriAtom.Config* configPtr = &config)
				return NativeMethods.criAtom_CalculateWorkSize(configPtr);
		}

		/// <summary>ライブラリの初期化 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリを初期化します。
		///  ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		///  （ライブラリの機能は、本関数を実行後、 <see cref="CriAtom.Finalize"/> 関数を実行するまでの間、 利用可能です。）
		///  ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		///  User Allocator方式を用いる場合、ユーザはCRI Atomライブラリにメモリ確保関数を登録しておきます。
		///  workにnull、work_sizeに0を指定して本関数を呼び出すことで、 ライブラリは登録済みのメモリ確保関数を使用して必要なメモリを自動的に確保します。
		///  ユーザがワーク領域を用意する必要はありません。
		///  初期化時に確保されたメモリは、終了処理時（ <see cref="CriAtom.Finalize"/> 関数実行時）に解放されます。
		///  Fixed Memory方式を用いる場合、ワーク領域として別途確保済みのメモリ領域を本関数に 設定する必要があります。
		///  ワーク領域のサイズは <see cref="CriAtom.CalculateWorkSize"/> 関数で取得可能です。
		///  初期化処理の前に <see cref="CriAtom.CalculateWorkSize"/> 関数で取得したサイズ分のメモリを予め 確保しておき、本関数に設定してください。
		///  尚、Fixed Memory方式を用いた場合、ワーク領域はライブラリの終了処理（ <see cref="CriAtom.Finalize"/> 関数） を行うまでの間、ライブラリ内で利用され続けます。
		///  ライブラリの終了処理を行う前に、ワーク領域のメモリを解放しないでください。
		/// </para>
		/// <para>
		/// 例:
		/// 【User Allocator方式によるライブラリの初期化】
		///  User Allocator方式を用いる場合、ライブラリの初期化／終了の手順は以下の通りです。
		/// <list type="number">
		/// <item><description>初期化処理実行前に、 <see cref="CriAtom.SetUserMallocFunction"/> 関数と <see cref="CriAtom.SetUserFreeFunction"/> 関数を用いてメモリ確保／解放関数を登録する。
		/// </description></item>
		/// <item><description>初期化用コンフィグ構造体にパラメーターをセットする。
		/// </description></item>
		/// <item><description><see cref="CriAtom.Initialize"/> 関数で初期化処理を行う。
		///  （workにはnull、work_sizeには0を指定する。）
		/// </description></item>
		/// <item><description>アプリケーション終了時に <see cref="CriAtom.Finalize"/> 関数で終了処理を行う。
		/// </description></item>
		/// </list>
		/// </para>
		/// <list>
		/// <list type="number">
		/// <item><description>初期化用コンフィグ構造体にパラメーターをセットする。
		/// </description></item>
		/// <item><description>ライブラリの初期化に必要なワーク領域のサイズを、 <see cref="CriAtom.CalculateWorkSize"/> 関数を使って計算する。
		/// </description></item>
		/// <item><description>ワーク領域サイズ分のメモリを確保する。
		/// </description></item>
		/// <item><description><see cref="CriAtom.Initialize"/> 関数で初期化処理を行う。
		///  （workには確保したメモリのアドレスを、work_sizeにはワーク領域のサイズを指定する。）
		/// </description></item>
		/// <item><description>アプリケーション終了時に <see cref="CriAtom.Finalize"/> 関数で終了処理を行う。
		/// </description></item>
		/// <item><description>ワーク領域のメモリを解放する。
		/// </description></item>
		/// </list>
		/// </list>
		/// <para>
		/// 具体的なコードは以下のとおりです。
		///  【Fixed Memory方式によるライブラリの初期化】
		///  Fixed Memory方式を用いる場合、ライブラリの初期化／終了の手順は以下の通りです。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Initialize(const CriAtomConfig *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		public static unsafe void Initialize(in CriAtom.Config config)
		{
			fixed (CriAtom.Config* configPtr = &config)
				NativeMethods.criAtom_Initialize(configPtr, default, default);
		}

		/// <summary>ライブラリの終了 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリを終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtom.Initialize"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Finalize(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Initialize"/>
		public static void Finalize()
		{
			NativeMethods.criAtom_Finalize();
		}

		/// <summary>ユーザアロケーターの登録 </summary>
		/// <param name="pMallocFunc">メモリ確保関数 </param>
		/// <param name="pFreeFunc">メモリ解放関数 </param>
		/// <param name="pObj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atom ライブラリにメモリアロケーター（メモリの確保／解放関数）を登録します。
		///  本メソッドでアロケーターを登録すると、Atomライブラリがワーク領域を必要とするタイミングで、 ユーザが登録したメモリ確保／解放処理が呼び出されることになります。
		///  その結果、ワーク領域を必要とする関数（ <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数等）に対し、 個別にワーク領域をセットする処理を省略することが可能になります。
		///  （ワーク領域に null ポインタ、ワーク領域サイズに 0 バイトを指定した場合でも、 アロケーターからの動的メモリ確保によりライブラリが問題なく動作するようになります。）
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ確保／解放関数のポインタに null を指定することで、 アロケーターの登録を解除することも可能です。
		///  ただし、未解放のメモリ領域が残っている状態で登録を解除すると、 エラーコールバックが返され、登録の解除に失敗します。
		///  （引き続き登録済みのアロケーターが呼び出されることになります。）
		///  本メソッドは内部的に <see cref="CriAtom.SetUserMallocFunction"/> 関数と <see cref="CriAtom.SetUserFreeFunction"/> 関数を呼び出します。
		///  本関数とこれらの API を併用しないようご注意ください。
		///  （本関数の呼び出しにより、上記 API にセットした内容が上書きされます。）
		///  また、登録されたメモリアロケーター関数はマルスレッドモード時に複数のスレッドからコール されることがあります。従って、メモリアロケート処理がスレッドセーフでない場合は独自に メモリアロケート処理を排他制御する必要があります。 
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetUserAllocator_(CriAtomMallocFunc p_malloc_func, CriAtomFreeFunc p_free_func, void *p_obj)"/>
		/// </remarks>
		public static unsafe void SetUserAllocator(delegate* unmanaged[Cdecl]<IntPtr, UInt32, IntPtr> pMallocFunc, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> pFreeFunc, IntPtr pObj)
		{
			NativeMethods.criAtom_SetUserAllocator_((IntPtr)pMallocFunc, (IntPtr)pFreeFunc, pObj);
		}

		/// <summary><see cref="CriAtom.Config"/>へのデフォルトパラメーターをセット </summary>
		/// <param name="pConfig">初期化用コンフィグ構造体へのポインタ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtom.Initialize"/> 関数に設定するコンフィグ構造体（ <see cref="CriAtom.Config"/> ）に、 デフォルトの値をセットします。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetDefaultConfig_(CriAtomConfig *p_config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Config"/>
		public static unsafe void SetDefaultConfig(out CriAtom.Config pConfig)
		{
			fixed (CriAtom.Config* pConfigPtr = &pConfig)
				NativeMethods.criAtom_SetDefaultConfig_(pConfigPtr);
		}

		/// <summary>ライブラリのバージョン番号やビルド情報を返します。 </summary>
		/// <returns>const CriChar8* ライブラリ情報文字列 </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリのバージョン、ビルドした日時、プラットフォームの情報が表示されます。 
		/// </para>
		/// <nativeinfo declaration="const CriChar8* CRIAPI criAtom_GetVersionString(void)"/>
		/// </remarks>
		public static NativeString GetVersionString()
		{
			return NativeMethods.criAtom_GetVersionString();
		}

		/// <summary>ライブラリ初期化用コンフィグ構造体 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリの動作仕様を指定するための構造体です。
		/// <see cref="CriAtom.Initialize"/> 関数の引数に指定します。
		///  CRI Atomライブラリは、初期化時に本構造体で指定された設定に応じて、内部リソースを 必要なだけ確保します。
		///  ライブラリが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて 変化します。 
		/// </para>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtom.SetDefaultConfig"/> メソッドで構造体にデフォルト パラメーターをセットした後、 <see cref="CriAtom.Initialize"/> 関数に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtom.SetDefaultConfig"/> メソッドで必ず構造体を初期化してください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.Initialize"/>
		/// <seealso cref="CriAtom.SetDefaultConfig"/>
		[Serializable]
		public unsafe partial struct Config
		{
			/// <summary>スレッドモデル </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリのスレッドモデルを指定します。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.ThreadModel"/>
			public CriAtom.ThreadModel threadModel;

			/// <summary>サーバー処理の実行頻度 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// サーバー処理を実行する頻度を指定します。
			///  通常、アプリケーションのフレームレートと同じ値を指定します。
			///  CRI Atomライブラリは、ファイル読み込みの管理や、音声データのデコード、音声の出力、 ステータスの更新等、ライブラリ内部で行う処理のほとんどを1つの関数でまとめて 行います。
			///  CRIミドルウェアでは、こういったライブラリ内の処理を一括して行う関数のことを "サーバー処理"と呼んでいます。
			///  スレッドモデルが <see cref="CriAtom.ThreadModel.Multi"/> の場合、サーバー処理はCRI Atomライブラリ が作成するスレッドで、定期的に実行されます。
			///  スレッドモデルが <see cref="CriAtom.ThreadModel.Single"/> や <see cref="CriAtom.ThreadModel.UserMulti"/> の場合、サーバー処理は <see cref="CriAtom.ExecuteMain"/> 関数内で実行されます。
			///  server_frequency には、サーバー処理を実行する頻度を指定します。
			///  スレッドモデルが <see cref="CriAtom.ThreadModel.Multi"/> の場合、CRI Atomライブラリは指定された 頻度でサーバー処理が実行されるよう、サーバー処理の呼び出し間隔を調節します。
			///  スレッドモデルが <see cref="CriAtom.ThreadModel.Single"/> や <see cref="CriAtom.ThreadModel.UserMulti"/> の場合、ユーザは <see cref="CriAtom.ExecuteMain"/> 関数を server_frequency で指定した頻度以上 で実行する必要があります。
			///  アプリケーションのフレームレートの変動が大きく、サーバー処理を実行する頻度にバラツキ ができてしまう場合には、最悪のフレームレートを想定して server_frequency の値を指定 するか、またはスレッドモデルに <see cref="CriAtom.ThreadModel.Multi"/> を指定してください。 
			/// </para>
			/// <para>
			/// 備考:
			/// サーバー処理の実行頻度を多くすると、単位サーバー処理当たりの処理量（デコード量等） が少なくなります。 その結果、単位サーバー当たりの処理負荷は小さくなります（負荷が分散されます）が、 サーバー処理の実行に伴うオーバーヘッドは大きくなります。
			///  （携帯ゲーム機等、CPUリソースが少ない環境でサーバー処理の実行頻度を多くしすぎた場合、 サーバー処理の実行に伴うオーバーヘッドが無視できなくなる可能性があります。）
			///  サーバー処理の実行頻度を少なくすると、単位サーバー処理当たりの処理量が多くなります。
			///  サーバー処理の実行に伴うオーバーヘッドは低減されますが、単位サーバー処理当たりの負荷 が高くなるため、フレーム落ち等の問題が発生する恐れがあります。
			/// </para>
			/// <para>
			/// 注意:
			/// スレッドモデルに <see cref="CriAtom.ThreadModel.Single"/> や <see cref="CriAtom.ThreadModel.UserMulti"/> を指定したにもかかわらず、 <see cref="CriAtom.ExecuteMain"/> 関数が server_frequency で 指定した値以下の頻度でしか実行されなかった場合、再生中の音が途切れる等の問題が 発生する可能性がありますので、ご注意ください。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.ExecuteMain"/>
			public Single serverFrequency;

			/// <summary>CRI File System の初期化パラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI File Systemの初期化パラメーターへのポインタを指定します。 nullを指定した場合、デフォルトパラメーターでCRI File Systemを初期化します。 
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.Initialize"/>
			public NativeReference<CriFs.Config> fsConfig;

			/// <summary>プラットフォーム固有の初期化パラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリを動作させるために必要な、 プラットフォーム固有の初期化パラメーターへのポインタを指定します。 nullを指定した場合、デフォルトパラメーターでプラットフォーム毎に必要な初期化を行います。
			///  パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。 パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。 
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtom.Initialize"/>
			public IntPtr context;

			/// <summary>ライブラリバージョン番号 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリのバージョン番号です。
			/// <see cref="CriAtom.SetDefaultConfig"/> メソッドにより、本ヘッダーに定義されているバージョン番号が設定されます。
			/// </para>
			/// <para>
			/// 注意:
			/// アプリケーションでは、この値を変更しないでください。
			/// </para>
			/// </remarks>
			public UInt32 version;

			/// <summary>ライブラリバージョン文字列 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// CRI Atomライブラリのバージョン文字列です。
			/// <see cref="CriAtom.SetDefaultConfig"/> メソッドにより、本ヘッダーに定義されているバージョン文字列が設定されます。
			/// </para>
			/// <para>
			/// 注意:
			/// アプリケーションでは、この値を変更しないでください。
			/// </para>
			/// </remarks>
			public NativeString versionString;

			/// <summary>最大プレーヤー数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// アプリケーション中で使用するプレーヤー（CriAtomPlayer）の数を指定します。
			///  アプリケーション中で <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数を使用してプレーヤーを作成する場合、 本パラメーターに使用するプレーヤーの数を指定する必要があります。
			///  max_playersには「同時に使用するプレーヤーの最大数」を指定します。
			///  例えば、 <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数と <see cref="CriAtomPlayer.Dispose"/> 関数を交互に続けて実行するケースにおいては、 最大同時には1つのプレーヤーしか使用しないため、関数の呼び出し回数に関係なくmax_playersに1を指定することが可能です。
			///  逆に、ある場面でプレーヤーを10個使用する場合には、その他の場面でプレーヤーを全く使用しない場合であっても、 max_playersに10を指定する必要があります。
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomPlayer.CreateStandardPlayer"/>
			/// <seealso cref="CriAtomPlayer.Dispose"/>
			public Int32 maxPlayers;

		}
		/// <summary>スレッドモデル </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリがどのようなスレッドモデルで動作するかを表します。
		///  ライブラリ初期化時（ <see cref="CriAtom.Initialize"/> 関数 ）に <see cref="CriAtom.Config"/> 構造体にて 指定します。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.Initialize"/>
		/// <seealso cref="CriAtom.Config"/>
		public enum ThreadModel
		{
			/// <summary>マルチスレッド </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリは内部でスレッドを作成し、マルチスレッドにて動作します。
			///  スレッドは <see cref="CriAtom.Initialize"/> 関数呼び出し時に作成されます。
			///  ライブラリのサーバー処理は、作成されたスレッド上で定期的に実行されます。
			/// </para>
			/// </remarks>
			Multi = 0,
			MultiWithSonicsync = 4,
			/// <summary>マルチスレッド（ユーザ駆動式） </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリは内部でスレッドを作成し、マルチスレッドにて動作します。
			///  スレッドは <see cref="CriAtom.Initialize"/> 関数呼び出し時に作成されます。
			///  サーバー処理自体は作成されたスレッド上で実行されますが、 <see cref="CriAtom.ThreadModel.Multi"/> とは異なり、自動的には実行されません。
			///  ユーザは <see cref="CriAtom.ExecuteMain"/> 関数で明示的にサーバー処理を駆動する必要があります。
			///  （ <see cref="CriAtom.ExecuteMain"/> 関数を実行すると、スレッドが起動し、サーバー処理が実行されます。）
			/// </para>
			/// </remarks>
			MultiUserDriven = 3,
			/// <summary>ユーザマルチスレッド </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリ内部ではスレッドを作成しませんが、ユーザが独自に作成したスレッド からサーバー処理関数を呼び出せるよう、内部の排他制御は行います。
			///  サーバー処理は <see cref="CriAtom.ExecuteMain"/> 関数内で同期実行されます。
			/// </para>
			/// </remarks>
			UserMulti = 1,
			/// <summary>シングルスレッド </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ライブラリ内部でスレッドを作成しません。また、内部の排他制御も行いません。
			///  サーバー処理は <see cref="CriAtom.ExecuteMain"/> 関数内で同期実行されます。
			/// </para>
			/// <para>
			/// 注意:
			/// このモデルを選択した場合、各APIとサーバー処理関数とを同一スレッドから呼び出すようにしてください。
			/// </para>
			/// </remarks>
			Single = 2,
		}
		/// <summary>メモリ確保関数の登録 </summary>
		/// <param name="func">メモリ確保関数 </param>
		/// <param name="obj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリにメモリ確保関数を登録します。
		///  CRI Atomライブラリ内がライブラリ内で行うメモリ確保処理を、 ユーザ独自のメモリ確保処理に置き換えたい場合に使用します。
		///  本関数の使用手順は以下のとおりです。
		///  (1) <see cref="CriAtom.MallocFunc"/> インターフェースに副ったメモリ確保関数を用意する。
		///  (2) <see cref="CriAtom.SetUserMallocFunction"/> 関数を使用し、CRI Atomライブラリに対して メモリ確保関数を登録する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数の obj に指定した値は、 <see cref="CriAtom.MallocFunc"/> に引数として渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを本関数の引数にセットしておき、コールバック関数で引数を経由 して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ確保関数を登録する際には、合わせてメモリ解放関数（ <see cref="CriAtom.FreeFunc"/> ）を 登録する必要があります。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetUserMallocFunction(CriAtomMallocFunc func, void *obj)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.MallocFunc"/>
		/// <seealso cref="CriAtom.SetUserFreeFunction"/>
		public static unsafe void SetUserMallocFunction(delegate* unmanaged[Cdecl]<IntPtr, UInt32, IntPtr> func, IntPtr obj)
		{
			NativeMethods.criAtom_SetUserMallocFunction((IntPtr)func, obj);
		}

		/// <summary>メモリ解放関数の登録 </summary>
		/// <param name="func">メモリ解放関数 </param>
		/// <param name="obj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリにメモリ解放関数を登録します。
		///  CRI Atomライブラリ内がライブラリ内で行うメモリ解放処理を、 ユーザ独自のメモリ解放処理に置き換えたい場合に使用します。
		///  本関数の使用手順は以下のとおりです。
		///  (1) <see cref="CriAtom.FreeFunc"/> インターフェースに副ったメモリ解放関数を用意する。
		///  (2) <see cref="CriAtom.SetUserFreeFunction"/> 関数を使用し、CRI Atomライブラリに対して メモリ解放関数を登録する。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数の obj に指定した値は、 <see cref="CriAtom.FreeFunc"/> に引数として渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを本関数の引数にセットしておき、コールバック関数で引数を経由 して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリ解放関数を登録する際には、合わせてメモリ確保関数（ <see cref="CriAtom.MallocFunc"/> ）を 登録する必要があります。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetUserFreeFunction(CriAtomFreeFunc func, void *obj)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.FreeFunc"/>
		/// <seealso cref="CriAtom.SetUserMallocFunction"/>
		public static unsafe void SetUserFreeFunction(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtom_SetUserFreeFunction((IntPtr)func, obj);
		}

		/// <summary>ライブラリ初期化状態の取得 </summary>
		/// <returns>CriBool 初期化中かどうか </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリが既に初期化されているかどうかをチェックします。
		/// </para>
		/// <nativeinfo declaration="CriBool CRIAPI criAtom_IsInitialized(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Initialize"/>
		/// <seealso cref="CriAtom.Finalize"/>
		public static bool IsInitialized()
		{
			return NativeMethods.criAtom_IsInitialized();
		}

		/// <summary>オーディオ出力が有効かどうかのチェック </summary>
		/// <returns>CriBool オーディオ出力が有効かどうか </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オーディオ出力が有効化どうかをチェックします。
		///  CRI Atomライブラリ初期化時、サウンドデバイスが利用可能であった場合、 本関数はtrueを返します。
		///  CRI Atomライブラリ初期化時に、サウンドデバイスが利用不可能であった場合、 本関数はfalseを返します。
		/// </para>
		/// <para>
		/// 補足:
		/// サウンドデバイスが無効な場合でも、Atomライブラリは音声を出力せずに動作します。
		///  （音声データ消費量をタイマーを元に計算し、可能な限り音声出力が有効な場合と同等の動作をエミュレートします。）
		///  そのため、音声出力デバイスが使用できないケースであっても、 アプリケーション側でAtomライブラリのAPI呼び出しを回避する必要はありません。
		///  （PC環境等、ユーザがサウンドデバイスを無効化しているケースに対し通知を行いたい場合に、 本関数を使用してください。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は、「Atomライブラリ初期化時点でオーディオデバイスが利用可能だったかどうか」を返します。
		///  ライブラリ初期化後にユーザがサウンドデバイスを抜き差しするケースについては、本関数では検知できません。
		///  （各機種固有のAPIを使用する必要があります。）
		///  オーディオ出力が無効な状態でAtomライブラリを初期化後、 ユーザがオーディオデバイスを有効化したとしても、Atomライブラリは音声出力を行いません。
		///  （オーディオデバイスが接続されたことをAtomライブラリが自動で検出することはありません。）
		///  アプリケーション実行中にオーディオデバイスを有効化したい場合には、 Atomライブラリの初期化処理をやり直す必要があります。
		/// </para>
		/// <nativeinfo declaration="CriBool CRIAPI criAtom_IsAudioOutputActive(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Initialize"/>
		public static bool IsAudioOutputActive()
		{
			return NativeMethods.criAtom_IsAudioOutputActive();
		}

		/// <summary>マルチスレッド用サーバー処理の実行 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリの内部状態を更新します。
		///  アプリケーションは、この関数を定期的に実行する必要があります。
		///  サーバー処理を実行すべき回数は、ライブラリ初期化時のパラメーターに依存します。
		///  ライブラリ初期化時にスレッドモデルを <see cref="CriAtom.ThreadModel.Multi"/> に設定した場合、 本関数の呼び出し頻度は少なくても問題は発生しません。
		///  なぜなら、リアルタイム性を要求される処理は全てCRI Atomライブラリ内で 定期的に自動実行されるためです。
		///  （最低でも毎秒1回程度実行されていれば、音切れ等の問題が発生することはありません。）
		///  ライブラリ初期化時にスレッドモデルを <see cref="CriAtom.ThreadModel.Single"/> や <see cref="CriAtom.ThreadModel.UserMulti"/> に設定した場合、ファイルの読み込み管理や、 データのデコード、音声の出力等、音声再生に必要な処理のほぼ全てが本関数内で実行されます。
		///  また、音声再生処理に同期して、CRI File Systemライブラリのファイルアクセスとデータ展開処理を実行します。
		///  そのため、以下の場合は音切れなどの問題が発生する可能性があるので注意してください。
		///  ・ライブラリ初期化時に指定したサーバー処理の実行頻度 （ <see cref="CriAtom.Config"/> 構造体のserver_frequency ）を下回る頻度で本関数を実行した場合
		///  ・大きいデータの読み込み、圧縮ファイルの読み込み等を行う場合
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリ初期化時にスレッドモデルを <see cref="CriAtom.ThreadModel.Multi"/> に設定した場合でも、 本関数を実行する必要があります。
		///  （スレッドモデルを <see cref="CriAtom.ThreadModel.Multi"/> に設定した場合、ステータス更新等、ごく一部の 処理のみを行うため、本関数内で長時間処理がブロックされることはありません。） 
		///  CRI File Systemライブラリのサーバー処理は、CRI Atomライブラリ内部で実行されます。
		///  そのため、本関数を実行している場合、アプリケーション側で別途CRI File Systemライブラリ のサーバー処理を呼び出す必要はありません。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_ExecuteMain(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ExecuteAudioProcess"/>
		public static void ExecuteMain()
		{
			NativeMethods.criAtom_ExecuteMain();
		}

		/// <summary>ユーザーマルチスレッド用サーバー処理の実行 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリのみを更新します。
		///  スレッドモデルが<see cref="CriAtom.ThreadModel.UserMulti"/>の場合、 アプリケーションは、この関数を定期的に実行する必要があります。
		/// </para>
		/// <see><see cref="CriAtom.Config"/></see>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.ThreadModel.Single"/> に設定した場合、サーバー処理の排他制御が行われないので、 複数のスレッドから呼び出さないようにしてください。
		/// </para>
		/// <para>
		///  ファイルの読み込み管理や、データのデコード、音声の出力等、 音声再生に必要な処理のほぼ全てが本関数内で実行されます。
		///  そのため、ライブラリ初期化時に指定したサーバー処理の実行頻度（ 
		///  構造体の server_frequency ）を下回る頻度で本関数を実行した場合、音切れ等の問題が発生する可能性 があります。
		///  また、本関数は<see cref="CriAtom.ExecuteMain"/> 関数と異なり、CRI File Systemライブラリのサーバー処理を実行しません。
		///  アプリケーションが必要なサーバー処理を正しい順序で実行してください。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_ExecuteAudioProcess(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ExecuteMain"/>
		public static void ExecuteAudioProcess()
		{
			NativeMethods.criAtom_ExecuteAudioProcess();
		}

		/// <summary>メモリ確保関数 </summary>
		/// <returns>void* 確保したメモリのアドレス（失敗時はnull） </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ確保関数登録用のインターフェースです。
		///  CRI Atomライブラリがライブラリ内で行うメモリ確保処理を、 ユーザ独自のメモリ確保処理に置き換えたい場合に使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// コールバック関数が実行される際には、sizeに必要とされるメモリのサイズがセット されています。
		///  コールバック関数内でsize分のメモリを確保し、確保したメモリのアドレスを 戻り値として返してください。
		///  尚、引数の obj には、<see cref="CriAtom.SetUserMallocFunction"/> 関数で登録したユーザ指定 オブジェクトが渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを <see cref="CriAtom.SetUserMallocFunction"/> 関数の引数にセットしておき、 本コールバック関数の引数を経由して参照してください。
		/// </para>
		/// <para>
		/// 注意:
		/// メモリの確保に失敗した場合、エラーコールバックが返されたり、呼び出し元の関数が 失敗する可能性がありますのでご注意ください。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.FreeFunc"/>
		/// <seealso cref="CriAtom.SetUserMallocFunction"/>
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
			static IntPtr CriAtomMallocFuncCallbackFunc(IntPtr obj, UInt32 size) =>
				InvokeCallbackInternal(obj, new(size));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate IntPtr NativeDelegate(IntPtr obj, UInt32 size);
			static NativeDelegate callbackDelegate = null;
#endif
			internal MallocFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, UInt32, IntPtr>)&CriAtomMallocFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomMallocFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>メモリ解放関数 </summary>
		/// <returns>なし </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ解放関数登録用のインターフェースです。
		///  CRI Atomライブラリ内がライブラリ内で行うメモリ解放処理を、 ユーザ独自のメモリ解放処理に置き換えたい場合に使用します。
		/// </para>
		/// <para>
		/// 備考:
		/// コールバック関数が実行される際には、memに解放すべきメモリのアドレスがセット されています。
		///  コールバック関数内でmemの領域のメモリを解放してください。 尚、引数の obj には、<see cref="CriAtom.SetUserFreeFunction"/> 関数で登録したユーザ指定 オブジェクトが渡されます。
		///  メモリ確保時にメモリマネージャー等を参照する必要がある場合には、 当該オブジェクトを <see cref="CriAtom.SetUserFreeFunction"/> 関数の引数にセットしておき、 本コールバック関数の引数を経由して参照してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.MallocFunc"/>
		/// <seealso cref="CriAtom.SetUserFreeFunction"/>
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
			static void CriAtomFreeFuncCallbackFunc(IntPtr obj, IntPtr mem) =>
				InvokeCallbackInternal(obj, new(mem));
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr obj, IntPtr mem);
			static NativeDelegate callbackDelegate = null;
#endif
			internal FreeFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, void>)&CriAtomFreeFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomFreeFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>オーディオフレーム開始コールバック関数の登録 </summary>
		/// <param name="func">オーディオフレーム開始コールバック関数 </param>
		/// <param name="obj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリにオーディオフレーム開始コールバックを登録します。
		///  オーディオフレームは、CRI Atomライブラリ内でサーバー処理を実行するタイミングを示します。
		///  本関数で登録したコールバック関数は、オーディオフレームの開始時（サーバー処理開始直前）に 実行されます。 
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリ初期化時にスレッドモデルをマルチスレッド（ <see cref="CriAtom.ThreadModel.Multi"/> ） に設定した場合、コールバック関数はCRI Atomライブラリ内で作成されたスレッドから 呼び出されます。
		///  ライブラリ初期化時にスレッドモデルをユーザマルチスレッド（ <see cref="CriAtom.ThreadModel.UserMulti"/> ）、 またはシングルスレッド（ <see cref="CriAtom.ThreadModel.Single"/> ）に設定した場合、コールバック関数 <see cref="CriAtom.ExecuteMain"/> 関数内で呼び出されます。
		///  引数の obj に指定した値は、 <see cref="CriAtom.AudioFrameStartCbFunc"/> に引数として渡されます。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数は1つしか登録できません。
		///  登録操作を複数回行った場合、既に登録済みのコールバック関数が、 後から登録したコールバック関数により上書きされてしまいます。
		///  funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetAudioFrameStartCallback(CriAtomAudioFrameStartCbFunc func, void *obj)"/>
		/// </remarks>
		public static unsafe void SetAudioFrameStartCallback(delegate* unmanaged[Cdecl]<IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtom_SetAudioFrameStartCallback((IntPtr)func, obj);
		}
		static unsafe void SetAudioFrameStartCallbackInternal(IntPtr func, IntPtr obj) => SetAudioFrameStartCallback((delegate* unmanaged[Cdecl]<IntPtr, void>)func, obj);
		static CriAtom.AudioFrameStartCbFunc _audioFrameStartCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetAudioFrameStartCallback" />
		public static CriAtom.AudioFrameStartCbFunc AudioFrameStartCallback => _audioFrameStartCallback ?? (_audioFrameStartCallback = new CriAtom.AudioFrameStartCbFunc(SetAudioFrameStartCallbackInternal));

		/// <summary>オーディオフレーム開始コールバック関数 </summary>
		/// <returns>なし </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オーディオフレームの開始時に実行されるコールバック関数です。
		///  オーディオフレームは、CRI Atomライブラリ内でサーバー処理を実行するタイミングを示します。
		///  コールバック関数の登録には <see cref="CriAtom.SetAudioFrameStartCallback"/> 関数を使用します。
		///  登録したコールバック関数は、オーディオフレームの開始時（サーバー処理開始直前）に実行されます。 
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリ初期化時にスレッドモデルをマルチスレッド（ <see cref="CriAtom.ThreadModel.Multi"/> ） に設定した場合、本コールバック関数はCRI Atomライブラリ内で作成されたスレッドから 呼び出されます。
		///  ライブラリ初期化時にスレッドモデルをユーザマルチスレッド（ <see cref="CriAtom.ThreadModel.UserMulti"/> ）、 またはシングルスレッド（ <see cref="CriAtom.ThreadModel.Single"/> ）に設定した場合、本コールバック関数 <see cref="CriAtom.ExecuteMain"/> 関数内で呼び出されます。
		///  尚、引数の obj には、<see cref="CriAtom.SetAudioFrameStartCallback"/> 関数で登録したユーザ指定 オブジェクトが渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioFrameStartCallback"/>
		public unsafe class AudioFrameStartCbFunc : NativeCallbackBase<AudioFrameStartCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{

			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CriAtomAudioFrameStartCbFuncCallbackFunc(IntPtr obj) =>
				InvokeCallbackInternal(obj, new());
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr obj);
			static NativeDelegate callbackDelegate = null;
#endif
			internal AudioFrameStartCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, void>)&CriAtomAudioFrameStartCbFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomAudioFrameStartCbFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>オーディオフレーム終了コールバック関数の登録 </summary>
		/// <param name="func">オーディオフレーム終了コールバック関数 </param>
		/// <param name="obj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリにオーディオフレーム終了コールバックを登録します。
		///  オーディオフレームは、CRI Atomライブラリ内でサーバー処理を実行するタイミングを示します。
		///  本関数で登録したコールバック関数は、オーディオフレームの終了時（サーバー処理終了直後）に 実行されます。 
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリ初期化時にスレッドモデルをマルチスレッド（ <see cref="CriAtom.ThreadModel.Multi"/> ） に設定した場合、コールバック関数はCRI Atomライブラリ内で作成されたスレッドから 呼び出されます。
		///  ライブラリ初期化時にスレッドモデルをユーザマルチスレッド（ <see cref="CriAtom.ThreadModel.UserMulti"/> ）、 またはシングルスレッド（ <see cref="CriAtom.ThreadModel.Single"/> ）に設定した場合、コールバック関数 <see cref="CriAtom.ExecuteMain"/> 関数内で呼び出されます。
		///  引数の obj に指定した値は、 <see cref="CriAtom.AudioFrameEndCbFunc"/> に引数として渡されます。
		/// </para>
		/// <para>
		/// 注意:
		/// コールバック関数は1つしか登録できません。
		///  登録操作を複数回行った場合、既に登録済みのコールバック関数が、 後から登録したコールバック関数により上書きされてしまいます。
		///  funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetAudioFrameEndCallback(CriAtomAudioFrameEndCbFunc func, void *obj)"/>
		/// </remarks>
		public static unsafe void SetAudioFrameEndCallback(delegate* unmanaged[Cdecl]<IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtom_SetAudioFrameEndCallback((IntPtr)func, obj);
		}
		static unsafe void SetAudioFrameEndCallbackInternal(IntPtr func, IntPtr obj) => SetAudioFrameEndCallback((delegate* unmanaged[Cdecl]<IntPtr, void>)func, obj);
		static CriAtom.AudioFrameEndCbFunc _audioFrameEndCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetAudioFrameEndCallback" />
		public static CriAtom.AudioFrameEndCbFunc AudioFrameEndCallback => _audioFrameEndCallback ?? (_audioFrameEndCallback = new CriAtom.AudioFrameEndCbFunc(SetAudioFrameEndCallbackInternal));

		/// <summary>オーディオフレーム終了コールバック関数 </summary>
		/// <returns>なし </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// オーディオフレームの終了時に実行されるコールバック関数です。
		///  オーディオフレームは、CRI Atomライブラリ内でサーバー処理を実行するタイミングを示します。
		///  コールバック関数の登録には <see cref="CriAtom.SetAudioFrameEndCallback"/> 関数を使用します。
		///  登録したコールバック関数は、オーディオフレームの終了時（サーバー処理終了直後）に実行されます。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリ初期化時にスレッドモデルをマルチスレッド（ <see cref="CriAtom.ThreadModel.Multi"/> ） に設定した場合、本コールバック関数はCRI Atomライブラリ内で作成されたスレッドから 呼び出されます。
		///  ライブラリ初期化時にスレッドモデルをユーザマルチスレッド（ <see cref="CriAtom.ThreadModel.UserMulti"/> ）、 またはシングルスレッド（ <see cref="CriAtom.ThreadModel.Single"/> ）に設定した場合、本コールバック関数 <see cref="CriAtom.ExecuteMain"/> 関数内で呼び出されます。
		///  尚、引数の obj には、<see cref="CriAtom.SetAudioFrameEndCallback"/> 関数で登録したユーザ指定 オブジェクトが渡されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetAudioFrameEndCallback"/>
		public unsafe class AudioFrameEndCbFunc : NativeCallbackBase<AudioFrameEndCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{

			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CriAtomAudioFrameEndCbFuncCallbackFunc(IntPtr obj) =>
				InvokeCallbackInternal(obj, new());
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr obj);
			static NativeDelegate callbackDelegate = null;
#endif
			internal AudioFrameEndCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, void>)&CriAtomAudioFrameEndCbFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomAudioFrameEndCbFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>デバイス更新通知の登録 </summary>
		/// <param name="func">デバイス更新通知コールバック関数 </param>
		/// <param name="obj">ユーザ指定オブジェクト </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デバイスの更新通知を受け取るためのコールバックを設定します。
		///  本関数を実行すると、デバイスが更新された際、第 1 引数（ func ） でセットされたコールバック関数が呼び出されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetDeviceUpdateCallback(CriAtomDeviceUpdateCbFunc func, void *obj)"/>
		/// </remarks>
		public static unsafe void SetDeviceUpdateCallback(delegate* unmanaged[Cdecl]<IntPtr, void> func, IntPtr obj)
		{
			NativeMethods.criAtom_SetDeviceUpdateCallback((IntPtr)func, obj);
		}
		static unsafe void SetDeviceUpdateCallbackInternal(IntPtr func, IntPtr obj) => SetDeviceUpdateCallback((delegate* unmanaged[Cdecl]<IntPtr, void>)func, obj);
		static CriAtom.DeviceUpdateCbFunc _deviceUpdateCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetDeviceUpdateCallback" />
		public static CriAtom.DeviceUpdateCbFunc DeviceUpdateCallback => _deviceUpdateCallback ?? (_deviceUpdateCallback = new CriAtom.DeviceUpdateCbFunc(SetDeviceUpdateCallbackInternal));

		/// <summary>デバイス更新通知コールバック </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// デバイスの更新通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtom.SetDeviceUpdateCallback"/> 関数に本関数型のコールバック関数を登録することで、 デバイスが更新された際にコールバック経由で通知を受け取ることが可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.SetDeviceUpdateCallback"/>
		public unsafe class DeviceUpdateCbFunc : NativeCallbackBase<DeviceUpdateCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{

			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CriAtomDeviceUpdateCbFuncCallbackFunc(IntPtr obj) =>
				InvokeCallbackInternal(obj, new());
#if !NET5_0_OR_GREATER
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			delegate void NativeDelegate(IntPtr obj);
			static NativeDelegate callbackDelegate = null;
#endif
			internal DeviceUpdateCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, void>)&CriAtomDeviceUpdateCbFuncCallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CriAtomDeviceUpdateCbFuncCallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>サーバー処理の割り込みを防止 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理の割り込みを防止します。
		///  本関数実行後、<see cref="CriAtom.Unlock"/> 関数実行までの間、サーバー処理の動作を防止します。
		///  複数のAPIを同一オーディオフレーム内で確実に実行したい場合には、本関数でサーバー処理の 割り込みを防止し、それらの関数を実行してください。 
		/// </para>
		/// <para>
		/// 注意:
		/// 上記の例のように、複数のプレーヤーで同時に再生をスタートする場合でも、 ストリーム再生時は同時に発音が開始されるとは限りません。
		///  （バッファリングに伴う再生遅延があるため。）
		///  本関数実行後、長時間<see cref="CriAtom.Unlock"/> 関数を呼ばない場合、音声再生が途切れる恐れがあります。
		///  サーバー処理の割り込みを防止する区間は、最小限に抑える必要があります。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Lock(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Unlock"/>
		public static void Lock()
		{
			NativeMethods.criAtom_Lock();
		}

		/// <summary>サーバー処理の割り込み防止を解除 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtom.Lock"/> 関数による、サーバー処理の割り込み防止を解除します。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Unlock(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.Lock"/>
		public static void Unlock()
		{
			NativeMethods.criAtom_Unlock();
		}

		/// <summary>チャンネルコンフィグのデフォルト値変更 </summary>
		/// <param name="numChannels">チャンネル数 </param>
		/// <param name="channelConfig">チャンネルコンフィグ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声データの各チャンネルと出力スピーカーの対応付けを変更します。
		///  Atomライブラリは、デフォルト状態では音声データが以下のチャンネル構成であると想定して動作します。
		/// <list type="table">
		/// <item><description>チャンネル数   </description><description>想定されるチャンネル構成    </description></item>
		/// <item><description>1   </description><description><see cref="CriAtom.ChannelConfig.Mono"/>    </description></item>
		/// <item><description>2   </description><description><see cref="CriAtom.ChannelConfig.Stereo"/>    </description></item>
		/// <item><description>3   </description><description><see cref="CriAtom.ChannelConfig._3Lrc"/>    </description></item>
		/// <item><description>4   </description><description><see cref="CriAtom.ChannelConfig.Quad"/>    </description></item>
		/// <item><description>5   </description><description><see cref="CriAtom.ChannelConfig._5"/>    </description></item>
		/// <item><description>6   </description><description><see cref="CriAtom.ChannelConfig._5_1"/>    </description></item>
		/// <item><description>7   </description><description><see cref="CriAtom.ChannelConfig._6_1"/>    </description></item>
		/// <item><description>8   </description><description><see cref="CriAtom.ChannelConfig._7_1"/>    </description></item>
		/// <item><description>9   </description><description><see cref="CriAtom.ChannelConfig.Ambisonics2p"/>    </description></item>
		/// <item><description>10   </description><description><see cref="CriAtom.ChannelConfig._7_1_2"/>    </description></item>
		/// <item><description>12   </description><description><see cref="CriAtom.ChannelConfig._7_1_4"/>    </description></item>
		/// <item><description>16   </description><description><see cref="CriAtom.ChannelConfig.Ambisonics3p"/>   </description></item>
		/// </list>
		/// 再生する音声データのチャンネル構成が上記と異なる場合には、本関数を使用してチャンネル構成を変更する必要があります。
		/// </para>
		/// <para>
		/// 注意:
		/// 構成変更の影響は、第一引数（num_channels）で指定したチャンネルの音声データにのみ影響します。
		///  そのため、異なるチャンネル数の音声データが混在する場合、各チャンネル数に対してそれぞれチャンネル構成の変更を行う必要があります。
		///  例えば、4ch音声と8ch音声の両方についてチャンネル構成を変更したい場合、num_channelsが4のケースと8のケースの2パターンのチャンネル構成を指定する必要があります。
		///  再生中の音声がデフォルト値を参照するタイミングはユーザーの操作に依存します。
		///  そのため、再生中にデフォルト値を変更した場合、意図したタイミングで変更が反映されるとは限りません。
		///  本関数を使用する場合、初期化時など音声を再生する前に実行してください。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_ChangeDefaultChannelConfig(CriSint32 num_channels, CriAtomChannelConfig channel_config)"/>
		/// </remarks>
		public static void ChangeDefaultChannelConfig(Int32 numChannels, CriAtom.ChannelConfig channelConfig)
		{
			NativeMethods.criAtom_ChangeDefaultChannelConfig(numChannels, channelConfig);
		}

		/// <summary>チャンネルコンフィグ情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 入力音声のチャンネル構成を示す値です。
		/// </para>
		/// </remarks>
		public enum ChannelConfig
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>不明 </para>
			/// </remarks>
			Unknown = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>モノラル </para>
			/// </remarks>
			Mono = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>ステレオ </para>
			/// </remarks>
			Stereo = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>3ch（L, R, C） </para>
			/// </remarks>
			_3Lrc = 7,
			/// <summary></summary>
			/// <remarks>
			/// <para>3ch（L, R, Cs） </para>
			/// </remarks>
			_3Lrs = 259,
			/// <summary></summary>
			/// <remarks>
			/// <para>4ch（L, R, Ls, Rs） </para>
			/// </remarks>
			Quad = 3075,
			/// <summary></summary>
			/// <remarks>
			/// <para>5ch </para>
			/// </remarks>
			_5 = 3079,
			/// <summary></summary>
			/// <remarks>
			/// <para>4.1ch </para>
			/// </remarks>
			_4_1 = 3083,
			/// <summary></summary>
			/// <remarks>
			/// <para>5.1ch </para>
			/// </remarks>
			_5_1 = 3087,
			/// <summary></summary>
			/// <remarks>
			/// <para>6.1ch </para>
			/// </remarks>
			_6_1 = 3343,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1ch </para>
			/// </remarks>
			_7_1 = 3135,
			/// <summary></summary>
			/// <remarks>
			/// <para>5.1.2ch </para>
			/// </remarks>
			_5_1_2 = 789519,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1.2ch </para>
			/// </remarks>
			_7_1_2 = 789567,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1.4ch </para>
			/// </remarks>
			_7_1_4 = 212031,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1.4.4ch </para>
			/// </remarks>
			_7_1_4_4 = 63126591,
			/// <summary></summary>
			/// <remarks>
			/// <para>1st Order Ambisonics </para>
			/// </remarks>
			Ambisonics1p = 2130706433,
			/// <summary></summary>
			/// <remarks>
			/// <para>2nd Order Ambisonics </para>
			/// </remarks>
			Ambisonics2p = 2130706434,
			/// <summary></summary>
			/// <remarks>
			/// <para>3rd Order Ambisonics </para>
			/// </remarks>
			Ambisonics3p = 2130706435,
		}
		/// <summary>チャンネル順序のデフォルト値変更 </summary>
		/// <param name="numChannels">チャンネル数 </param>
		/// <param name="channelOrder">チャンネル順序 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したチャンネル数の音声データについて、チャンネルの読み込み順序を変更します。
		///  Atomライブラリは、デフォルト状態では音声データが以下のチャンネル順序でインターリーブされていると想定して動作します。
		/// <list type="table">
		/// <item><description>チャンネル番号   </description><description>想定されるチャンネル    </description></item>
		/// <item><description>0   </description><description>レフト    </description></item>
		/// <item><description>1   </description><description>ライト    </description></item>
		/// <item><description>2   </description><description>センター    </description></item>
		/// <item><description>3   </description><description>LFE    </description></item>
		/// <item><description>4   </description><description>サラウンドレフト    </description></item>
		/// <item><description>5   </description><description>サラウンドライト    </description></item>
		/// <item><description>6   </description><description>サラウンドバックレフト    </description></item>
		/// <item><description>7   </description><description>サラウンドバックライト    </description></item>
		/// <item><description>8   </description><description>トップフロントレフト    </description></item>
		/// <item><description>9   </description><description>トップフロントライト    </description></item>
		/// <item><description>10   </description><description>トップバックレフト    </description></item>
		/// <item><description>11   </description><description>トップバックライト   </description></item>
		/// </list>
		/// 再生する音声データの並び順が上記と異なる場合には、本関数を使用してチャンネルの読み込み順序を変更する必要があります。
		///  例えば、入力音声のサラウンドバック成分がサラウンド成分より前にある7.1chの音声データ （4、5チャンネル目と6、7チャンネル目とが入れ替わった音声データ）を使用する場合、 以下のようなコードでチャンネル並び順を指定する必要があります。
		/// </para>
		/// <para>
		/// 補足:
		/// モノラル音声はセンターチャンネルと解釈されます。
		///  ただし、デフォルト状態ではレフトスピーカーとライトスピーカーを使用した、いわゆるファントムセンターで出力されます。
		///  （モノラル音声をセンタースピーカーから出力するには、パンスピーカータイプを変更する必要があります。）
		///  5.1.2chの音声データは、デフォルトで以下のチャンネル順序と解釈されます。
		/// <list type="table">
		/// <item><description>チャンネル番号   </description><description>想定されるチャンネル    </description></item>
		/// <item><description>0   </description><description>レフト    </description></item>
		/// <item><description>1   </description><description>ライト    </description></item>
		/// <item><description>2   </description><description>センター    </description></item>
		/// <item><description>3   </description><description>LFE    </description></item>
		/// <item><description>4   </description><description>サラウンドレフト    </description></item>
		/// <item><description>5   </description><description>サラウンドライト    </description></item>
		/// <item><description>6   </description><description>トップレフト    </description></item>
		/// <item><description>7   </description><description>トップトライト   </description></item>
		/// </list>
		/// </para>
		/// <para>
		/// 注意:
		/// 順序変更の影響は、第一引数（num_channels）で指定したチャンネルの音声データにのみ影響します。
		///  そのため、異なるチャンネル数の音声データが混在する場合、各チャンネル数に対してそれぞれチャンネル順序の変更を行う必要があります。
		///  例えば、7.1ch音声と7.1.4ch音声の両方についてチャンネル順序を変更したい場合、num_channelsが8のケースと12のケースの2パターンのチャンネル順序を指定する必要があります。
		///  再生中の音声がデフォルト値を参照するタイミングはユーザーの操作に依存します。
		///  そのため、再生中にデフォルト値を変更した場合、意図したタイミングで変更が反映されるとは限りません。
		///  本関数を使用する場合、初期化時など音声を再生する前に実行してください。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_ChangeDefaultChannelOrder(CriSint32 num_channels, const CriSint32 *channel_order)"/>
		/// </remarks>
		public static unsafe void ChangeDefaultChannelOrder(Int32 numChannels, in Int32 channelOrder)
		{
			fixed (Int32* channelOrderPtr = &channelOrder)
				NativeMethods.criAtom_ChangeDefaultChannelOrder(numChannels, channelOrderPtr);
		}

		/// <summary>Ambisonics音声のフォーマット指定 </summary>
		/// <param name="format">フォーマット </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Ambisonics音声データのチャンネル並び順と正規化方式を指定します。
		///  デフォルトのフォーマットは CRIATOM_AMBISONICS_ACN_SN3D です。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetAmbisonicsInputFormat(CriAtomAmbisonicsFormat format)"/>
		/// </remarks>
		public static void SetAmbisonicsInputFormat(CriAtom.AmbisonicsFormat format)
		{
			NativeMethods.criAtom_SetAmbisonicsInputFormat(format);
		}

		/// <summary>Ambisonnicsフォーマット情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Ambisonics音声のチャンネル並び順と正規化方式を示す値です。
		/// </para>
		/// </remarks>
		public enum AmbisonicsFormat
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>FuMa maxN </para>
			/// </remarks>
			FumaMaxn = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>ACN SN3D </para>
			/// </remarks>
			AcnSn3d = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>ACN N3D </para>
			/// </remarks>
			AcnN3d = 2,
		}
		/// <summary>パフォーマンスモニター機能の追加 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス計測機能を追加し、パフォーマンス計測処理を開始します。
		///  本関数を実行後、 <see cref="CriAtom.GetPerformanceInfo"/> 関数を実行することで、 サーバー処理の負荷や、サーバー処理の実行間隔等、ライブラリのパフォーマンス情報を 取得することが可能です。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_AttachPerformanceMonitor(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.GetPerformanceInfo"/>
		/// <seealso cref="CriAtom.DetachPerformanceMonitor"/>
		public static void AttachPerformanceMonitor()
		{
			NativeMethods.criAtom_AttachPerformanceMonitor();
		}

		/// <summary>パフォーマンスモニターのリセット </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 現在までの計測結果を破棄します。
		///  パフォーマンスモニターは、 <see cref="CriAtom.AttachPerformanceMonitor"/> 関数実行直後 からパフォーマンス情報の取得を開始し、計測結果を累積します。
		///  これから計測する区間に対し、以前の計測結果を以降の計測結果に含めたくない場合には、 本関数を実行し、累積された計測結果を一旦破棄する必要があります。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_ResetPerformanceMonitor(void)"/>
		/// </remarks>
		public static void ResetPerformanceMonitor()
		{
			NativeMethods.criAtom_ResetPerformanceMonitor();
		}

		/// <summary>パフォーマンス情報の取得 </summary>
		/// <param name="info">パフォーマンス情報 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス情報を取得します。
		///  本関数は、 <see cref="CriAtom.AttachPerformanceMonitor"/> 関数実行後から <see cref="CriAtom.DetachPerformanceMonitor"/> 関数を実行するまでの間、利用可能です。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_GetPerformanceInfo(CriAtomPerformanceInfo *info)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.AttachPerformanceMonitor"/>
		/// <seealso cref="CriAtom.DetachPerformanceMonitor"/>
		public static unsafe void GetPerformanceInfo(out CriAtom.PerformanceInfo info)
		{
			fixed (CriAtom.PerformanceInfo* infoPtr = &info)
				NativeMethods.criAtom_GetPerformanceInfo(infoPtr);
		}

		/// <summary>パフォーマンスモニター機能の削除 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス計測処理を終了し、パフォーマンス計測機能を削除します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は完了復帰型の関数です。
		///  本関数を実行すると、しばらくの間Atomライブラリのサーバー処理がブロックされます。
		///  音声再生中に本関数を実行すると、音途切れ等の不具合が発生する可能性があるため、 本関数の呼び出しはシーンの切り替わり等、負荷変動を許容できるタイミングで行ってください。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_DetachPerformanceMonitor(void)"/>
		/// </remarks>
		public static void DetachPerformanceMonitor()
		{
			NativeMethods.criAtom_DetachPerformanceMonitor();
		}

		/// <summary>パフォーマンス情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パフォーマンス情報を取得するための構造体です。
		/// <see cref="CriAtom.GetPerformanceInfo"/> 関数で利用します。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.GetPerformanceInfo"/>
		public unsafe partial struct PerformanceInfo
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理実行回数 </para>
			/// </remarks>
			public UInt32 serverProcessCount;

			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理時間の最終計測値（マイクロ秒単位） </para>
			/// </remarks>
			public UInt32 lastServerTime;

			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理時間の最大値（マイクロ秒単位） </para>
			/// </remarks>
			public UInt32 maxServerTime;

			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理時間の平均値（マイクロ秒単位） </para>
			/// </remarks>
			public UInt32 averageServerTime;

			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理実行間隔の最終計測値（マイクロ秒単位） </para>
			/// </remarks>
			public UInt32 lastServerInterval;

			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理実行間隔の最大値（マイクロ秒単位） </para>
			/// </remarks>
			public UInt32 maxServerInterval;

			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理実行間隔の平均値（マイクロ秒単位） </para>
			/// </remarks>
			public UInt32 averageServerInterval;

		}
		/// <summary>ADXデータのビットレート計算 </summary>
		/// <param name="numChannels">データのチャンネル数 </param>
		/// <param name="samplingRate">データのサンプリングレート </param>
		/// <returns>CriSint32 ビットレート[bps] </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ADXデータのビットレートを計算します。
		///  計算に失敗すると本関数は-1を返します。
		///  計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <nativeinfo declaration="CriSint32 CRIAPI criAtom_CalculateAdxBitrate(CriSint32 num_channels, CriSint32 sampling_rate)"/>
		/// </remarks>
		public static Int32 CalculateAdxBitrate(Int32 numChannels, Int32 samplingRate)
		{
			return NativeMethods.criAtom_CalculateAdxBitrate(numChannels, samplingRate);
		}

		/// <summary>HCAデータのビットレート計算 </summary>
		/// <param name="numChannels">データのチャンネル数 </param>
		/// <param name="samplingRate">データのサンプリングレート </param>
		/// <param name="quality">データのエンコード品質 </param>
		/// <returns>CriSint32 ビットレート[bps] </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCAデータのビットレートを計算します。
		///  計算に失敗すると本関数は-1を返します。
		///  計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// qualityにはCRI Atom CraftまたはCRI Atom Encoderで設定したエンコード品質を指定します。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 CRIAPI criAtom_CalculateHcaBitrate(CriSint32 num_channels, CriSint32 sampling_rate, CriAtomEncodeQuality quality)"/>
		/// </remarks>
		public static Int32 CalculateHcaBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality)
		{
			return NativeMethods.criAtom_CalculateHcaBitrate(numChannels, samplingRate, quality);
		}

		/// <summary>エンコード品質 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atom Encoder, CRI Atom Craftで設定されるエンコード品質のデータ型です。
		///  音声データのビットレートを計算するときに使用します。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.CalculateHcaBitrate"/>
		/// <seealso cref="CriAtom.CalculateHcaMxBitrate"/>
		public enum EncodeQuality
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>最低品質設定 </para>
			/// </remarks>
			Lowest = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>低品質設定 </para>
			/// </remarks>
			Low = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>中品質設定 </para>
			/// </remarks>
			Middle = 2,
			/// <summary></summary>
			/// <remarks>
			/// <para>高品質設定 </para>
			/// </remarks>
			High = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>最高品質設定 </para>
			/// </remarks>
			Highest = 4,
		}
		/// <summary>HCA-MXデータのビットレート計算 </summary>
		/// <param name="numChannels">データのチャンネル数 </param>
		/// <param name="samplingRate">データのサンプリングレート </param>
		/// <param name="quality">データのエンコード品質 </param>
		/// <returns>CriSint32 ビットレート[bps] </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXデータのビットレートを計算します。
		///  計算に失敗すると本関数は-1を返します。
		///  計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// qualityにはCRI Atom CraftまたはCRI Atom Encoderで設定したエンコード品質を指定します。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 CRIAPI criAtom_CalculateHcaMxBitrate(CriSint32 num_channels, CriSint32 sampling_rate, CriAtomEncodeQuality quality)"/>
		/// </remarks>
		public static Int32 CalculateHcaMxBitrate(Int32 numChannels, Int32 samplingRate, CriAtom.EncodeQuality quality)
		{
			return NativeMethods.criAtom_CalculateHcaMxBitrate(numChannels, samplingRate, quality);
		}

		/// <summary>ストリーミング情報の取得 </summary>
		/// <param name="streamingInfo">ストリーミング情報保存先のポインタ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリのストリーミング管理モジュールからストリーミング情報を取得します。
		///  本関数は、呼び出された時点のストリーミング情報を streaming_info に保存します。
		/// </para>
		/// <para>
		/// 注意:
		/// Atomサーバー内の処理と一部排他制御しているため、 優先度逆転によりAtomサーバーを止めてしまわないように注意してください。
		///  一部のプラットフォームでは、ストリーミング情報を取得できません。
		///  本関数の戻り値を確認してください。
		///  エラーが原因でストリーミング情報を取得できなかった場合については、
		///  エラーコールバックが発生していないかを確認してください。 
		/// </para>
		/// <nativeinfo declaration="CriBool CRIAPI criAtom_GetStreamingInfo(CriAtomStreamingInfo *streaming_info)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.StreamingInfo"/>
		public static unsafe bool GetStreamingInfo(out CriAtom.StreamingInfo streamingInfo)
		{
			fixed (CriAtom.StreamingInfo* streamingInfoPtr = &streamingInfo)
				return NativeMethods.criAtom_GetStreamingInfo(streamingInfoPtr);
		}

		/// <summary>ストリーミング情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtom.GetStreamingInfo"/> 関数で取得した時点でのストリーミングの状況です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.GetStreamingInfo"/>
		public unsafe partial struct StreamingInfo
		{
			/// <summary>現在のストリーミング数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 情報を取得した時点でのストリーミング数です。
			///  ストリーミング再生の増減に伴い、この値も変化します。
			/// </para>
			/// </remarks>
			public Int32 numStreaming;

			/// <summary>現在のストリーミング総ビットレート </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 情報を取得した時点での全ストリーミング再生の合計消費ビットレートです。
			///  ストリーミング再生の増減に伴い、この値も変化します。
			///  単位は [bps] （bit / 秒）です。
			/// </para>
			/// </remarks>
			public Single totalBps;

			/// <summary>現在の最悪再生開始レイテンシ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 情報を取得した時点での、新しいストリーミング再生の再生開始レイテンシ の予測最悪値です。
			///  単位は [秒] です。
			///  この値は遅延の最大値について予測した情報であり、 新しいストリーミング再生が常にこの値の時間分だけ遅延するわけではありません。
			/// </para>
			/// <para>
			/// 注意:
			/// 実際には、新しいストリーミング再生が追加された後、
			///  ストリームデータのビットレートが読み込まれてから正確な遅延時間が再計算されます。
			/// </para>
			/// </remarks>
			public Single worstLatency;

		}
		/// <summary>ファイルI/Oの空き時間を使ったストリーミング読み込みを行うかどうか </summary>
		/// <param name="flag">CRI_TRUE=ファイルI/Oの空き時間を使って読み込む </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRI Atomライブラリのストリーミング管理モジュールに対して、
		///  ファイルI/Oの空き時間を使ってストリーミング読み込みを行うかどうかを設定します。
		///  trueを設定すると、CRI Atomライブラリのストリーミング管理モジュールは ファイルI/Oの空き時間を使って、空きバッファーに対してデータを余分に読み込みます。
		///  falseを設定すると、CRI Atomライブラリのストリーミング管理モジュールは ファイルI/Oの空き時間を使わなくなり、余分なストリーミング読み込みを行わなくなります。
		///  デフォルトではtrueを設定した状態です。
		/// </para>
		/// <para>
		/// 備考：
		/// ファイルI/Oの空き時間を使い、空きバッファーに対してデータを余分に読み込んでおくことで、 シークの発生頻度を減らす事ができ、総合的なファイルI/Oの効率が向上します。
		///  一方、通常ファイルのロード処理は、ストリーミングの読み込みよりも優先度が低いため、 空きバッファーが大きすぎると通常ファイルのロード処理を大幅に遅延させてしまいます。
		/// </para>
		/// <para>
		/// 注意:
		/// Atomサーバー内の処理と一部排他制御しているため、 優先度逆転によりAtomサーバーを止めてしまわないように注意してください。 
		/// </para>
		/// <nativeinfo declaration="CriBool CRIAPI criAtom_SetFreeTimeBufferingFlagForDefaultDevice(CriBool flag)"/>
		/// </remarks>
		public static bool SetFreeTimeBufferingFlagForDefaultDevice(NativeBool flag)
		{
			return NativeMethods.criAtom_SetFreeTimeBufferingFlagForDefaultDevice(flag);
		}

		/// <summary>サウンドレンダラタイプ </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomプレーヤー、またはASRがが内部で作成するサウンドレンダラの種別を指定するためのデータ型です。
		///  AtomプレーヤーやASR作成時にコンフィグ構造体のパラメーターとして指定します。 
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtom.SoundRendererType.Any"/> は <see cref="CriAtomExPlayer.SetSoundRendererType"/> 関数に対してのみ指定可能です。
		///  ボイスプール作成時には使用できません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.AdxPlayerConfig"/>
		/// <seealso cref="CriAtomPlayer.CreateAdxPlayer"/>
		/// <seealso cref="CriAtomExAsrRack.CriAtomExAsrRack"/>
		public enum SoundRendererType
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>プラットフォームライブラリへ出力 </para>
			/// </remarks>
			Native = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>Atom Sound Rendererへ出力 </para>
			/// </remarks>
			Asr = 2,
			Extended = 3,
			Spatial = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>機種固有定義1 </para>
			/// </remarks>
			Hw1 = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>機種固有定義2 </para>
			/// </remarks>
			Hw2 = 65537,
			/// <summary></summary>
			/// <remarks>
			/// <para>機種固有定義3 </para>
			/// </remarks>
			Hw3 = 131073,
			/// <summary></summary>
			/// <remarks>
			/// <para>機種固有定義4 </para>
			/// </remarks>
			Hw4 = 196609,
			/// <summary></summary>
			/// <remarks>
			/// <para>Platform Specific </para>
			/// </remarks>
			ForcedNative = 983041,
			/// <summary></summary>
			/// <remarks>
			/// <para>振動 </para>
			/// </remarks>
			Haptic = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>無音 </para>
			/// </remarks>
			Pseudo = 65539,
			/// <summary></summary>
			/// <remarks>
			/// <para>スペシャライザ付きチャンネルベース再生 </para>
			/// </remarks>
			SpatialChannels = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>Ambisonics再生 </para>
			/// </remarks>
			Ambisonics = 65540,
			/// <summary></summary>
			/// <remarks>
			/// <para>パススルー再生 </para>
			/// </remarks>
			Passthrough = 131076,
			/// <summary></summary>
			/// <remarks>
			/// <para>オブジェクトベース再生 </para>
			/// </remarks>
			Object = 196612,
			/// <summary></summary>
			/// <remarks>
			/// <para>出力方式を制限しない </para>
			/// </remarks>
			Any = 0,
		}
		/// <summary></summary>
		/// <remarks>
		/// <para>標準プレーヤー作成用コンフィグ構造体</para>
		/// </remarks>
		[Serializable]
		public unsafe partial struct StandardPlayerConfig
		{
			/// <summary></summary>
			/// <remarks>
			/// <code>
			/// <code>    \brief 最大出力チャンネル数
			///     \par 説明:
			///     Atomプレーヤーで再生する音声のチャンネル数を指定します。
			///     <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数で作成されたAtomプレーヤーは、max_channelsで指定した
			///     チャンネル数"以下の"音声データを再生可能です。
			///     最大出力チャンネル数として指定する値と、作成されたAtomプレーヤーで再生可能なデータの
			///     関係を以下に示します。
			/// </code>
			/// </code>
			/// <list>
			/// <list type="table"><listheader><term><b>最大出力チャンネル数と再生可能なデータの関係</b></term></listheader>
			/// <item><description>最大出力チャンネル数（指定する値）  </description><description>作成されたAtomプレーヤーで再生可能なデータ   </description></item>
			/// <item><description>1  </description><description>モノラル   </description></item>
			/// <item><description>2  </description><description>モノラル、ステレオ   </description></item>
			/// <item><description>6  </description><description>モノラル、ステレオ、5.1ch   </description></item>
			/// <item><description>8  </description><description>モノラル、ステレオ、5.1ch、7.1ch   </description></item>
			/// </list>
			/// </list>
			/// <para>
			/// 備考:
			/// サウンド出力時にハードウェアリソースを使用するプラットフォームにおいては、 出力チャンネル数を小さくすることで、ハードウェアリソースの消費を抑えることが 可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大出力チャンネル数を超えるデータは、再生することはできません。
			///  例えば、最大出力チャンネル数を1に設定した場合、作成されたAtomプレーヤーで ステレオ音声を再生することはできません。
			///  （モノラルにダウンミックスされて出力されることはありません。） 
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>最大サンプリングレート </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のサンプリングレートを指定します。
			/// <see cref="CriAtomPlayer.CreateStandardPlayer"/> 関数で作成されたAtomプレーヤーは、max_sampling_rateで指定した サンプリングレート"以下の"音声データを再生可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// 最大サンプリングレートを下げることで、Atomプレーヤー作成時に必要となるワークメモリ のサイズを抑えることが可能です。 
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大サンプリングレートを超えるデータは、再生することはできません。
			///  例えば、最大サンプリングレートを24000に設定した場合、作成されたAtomプレーヤーで 48000Hzの音声を再生することはできません。
			///  （ダウンサンプリングされて出力されることはありません。） 
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>ストリーミング再生を行うかどうか </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーでストリーミング再生（ファイルからの再生）を行うかどうかを指定します。
			///  streaming_flagにfalseを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生（ <see cref="CriAtomPlayer.SetData"/> 関数で指定したメモリアドレスの再生）のみをサポート します。（ファイルからの再生はできません。）
			///  streaming_flagにtrueを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生に加え、ファイルからの再生（ <see cref="CriAtomPlayer.SetFile"/> 関数や <see cref="CriAtomPlayer.SetContentId"/> 関数で指定されたファイルの再生）をサポートします。
			/// </para>
			/// <para>
			/// 補足:
			/// streaming_flagをtrueにした場合、Atomプレーヤー作成時にファイル読み込み用のリソース が確保されます。
			///  そのため、streaming_flagをfalseの場合に比べ、Atomプレーヤーの作成に必要なメモリの サイズが大きくなります。 
			/// </para>
			/// </remarks>
			public NativeBool streamingFlag;

			/// <summary>サウンドレンダラタイプ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーが使用するサウンドレンダラの種別を指定します。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererDefault"/> を指定した場合、 音声データはデフォルト設定のサウンドレンダラに転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、 音声データは各プラットフォームのサウンド出力に転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合、 音声データはASR（Atom Sound Renderer）に転送されます。
			///  （ASRの出力先は、ASR初期化時に別途指定。） 
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>デコード処理のレイテンシ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デコード処理のレイテンシを指定します。
			///  decode_latency を 0 に設定した場合、プレーヤーは音声再生開始時に 音声データのデコードを可能な限り遅延なく行います。
			///  （オンメモリ再生時は <see cref="CriAtomPlayer.Start"/> 関数を実行後、 最初のサーバー処理内で再生開始に必要な量のデータをデコードし、 音声の出力を開始します。）
			///  これに対し decode_latency を 1 以上に設定した場合、 再生開始に必要なデータのデコードを、複数回のサーバー処理に分割して行います。
			///  （オンメモリ再生時であっても <see cref="CriAtomPlayer.Start"/> 関数を実行後、 decode_latency に指定した回数サーバー処理が動作するまでは音声の出力が開始されません。）
			/// </para>
			/// <para>
			/// 備考:
			/// Atomプレーヤーはサウンドバッファー内のデータ残量を元に、 サーバー処理当たりの音声データのデコード量を決定しています。
			///  音声再生開始前はサウンドバッファーが空の状態のため、 音声再生中に比べて多くのデータ（再生中の 2 ～ 4 倍程度）がデコードされます。
			///  プレーヤー当たりの音声データのデコード処理負荷は小さいため、 一音一音の発音開始時の処理負荷が問題になることはほとんどありません。
			///  しかし、アプリケーション中で 1V に大量の発音リクエストを同時に発行した場合、 全てのプレーヤーの処理負荷のピークが同期し、負荷が目に見えて大きくなる場合があります。
			///  こういった制御を行うケースでは、 decode_latency の値を増やすことで、 局所的に処理負荷が高くなる症状を回避することが可能です。
			///  decode_latency のデフォルト値は、ほとんどの環境で 0 に設定されています。
			///  しかし、携帯ゲーム機等、わずかな負荷変動でもアプリケーションに大きな影響を及ぼす環境では、 デフォルト値が 1 以上に設定されている可能性があります。
			///  （実際にセットされる値については <see cref="CriAtomPlayer.SetDefaultConfigForAdxPlayer"/> メソッドの適用結果を確認してください。）
			///  現状 decode_latency の値に 4 以上の値を指定することはできません。
			///  （ decode_latency に 4 以上の値を指定した場合でも、ライブラリ内で 3 に変更されます。）
			/// </para>
			/// </remarks>
			public Int32 decodeLatency;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。 nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでプレーヤーを作成します。
			///  パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。 パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。 
			/// </para>
			/// </remarks>
			public IntPtr context;

		}
		/// <summary></summary>
		/// <remarks>
		/// <para>ADXプレーヤー作成用コンフィグ構造体</para>
		/// </remarks>
		[Serializable]
		public unsafe partial struct AdxPlayerConfig
		{
			/// <summary></summary>
			/// <remarks>
			/// <code>
			/// <code>    \brief 最大出力チャンネル数
			///     \par 説明:
			///     Atomプレーヤーで再生する音声のチャンネル数を指定します。
			///     <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数で作成されたAtomプレーヤーは、max_channelsで指定した
			///     チャンネル数"以下の"音声データを再生可能です。
			///     最大出力チャンネル数として指定する値と、作成されたAtomプレーヤーで再生可能なデータの
			///     関係を以下に示します。
			/// </code>
			/// </code>
			/// <list>
			/// <list type="table"><listheader><term><b>最大出力チャンネル数と再生可能なデータの関係</b></term></listheader>
			/// <item><description>最大出力チャンネル数（指定する値）  </description><description>作成されたAtomプレーヤーで再生可能なデータ   </description></item>
			/// <item><description>1  </description><description>モノラル   </description></item>
			/// <item><description>2  </description><description>モノラル、ステレオ   </description></item>
			/// <item><description>6  </description><description>モノラル、ステレオ、5.1ch   </description></item>
			/// <item><description>8  </description><description>モノラル、ステレオ、5.1ch、7.1ch   </description></item>
			/// </list>
			/// </list>
			/// <para>
			/// 備考:
			/// サウンド出力時にハードウェアリソースを使用するプラットフォームにおいては、 出力チャンネル数を小さくすることで、ハードウェアリソースの消費を抑えることが 可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大出力チャンネル数を超えるデータは、再生することはできません。
			///  例えば、最大出力チャンネル数を1に設定した場合、作成されたAtomプレーヤーで ステレオ音声を再生することはできません。
			///  （モノラルにダウンミックスされて出力されることはありません。） 
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>最大サンプリングレート </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のサンプリングレートを指定します。
			/// <see cref="CriAtomPlayer.CreateAdxPlayer"/> 関数で作成されたAtomプレーヤーは、max_sampling_rateで指定した サンプリングレート"以下の"音声データを再生可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// 最大サンプリングレートを下げることで、Atomプレーヤー作成時に必要となるワークメモリ のサイズを抑えることが可能です。 
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大サンプリングレートを超えるデータは、再生することはできません。
			///  例えば、最大サンプリングレートを24000に設定した場合、作成されたAtomプレーヤーで 48000Hzの音声を再生することはできません。
			///  （ダウンサンプリングされて出力されることはありません。） 
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>ストリーミング再生を行うかどうか </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーでストリーミング再生（ファイルからの再生）を行うかどうかを指定します。
			///  streaming_flagにfalseを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生（ <see cref="CriAtomPlayer.SetData"/> 関数で指定したメモリアドレスの再生）のみをサポート します。（ファイルからの再生はできません。）
			///  streaming_flagにtrueを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生に加え、ファイルからの再生（ <see cref="CriAtomPlayer.SetFile"/> 関数や <see cref="CriAtomPlayer.SetContentId"/> 関数で指定されたファイルの再生）をサポートします。
			/// </para>
			/// <para>
			/// 補足:
			/// streaming_flagをtrueにした場合、Atomプレーヤー作成時にファイル読み込み用のリソース が確保されます。
			///  そのため、streaming_flagをfalseの場合に比べ、Atomプレーヤーの作成に必要なメモリの サイズが大きくなります。 
			/// </para>
			/// </remarks>
			public NativeBool streamingFlag;

			/// <summary>サウンドレンダラタイプ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーが使用するサウンドレンダラの種別を指定します。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererDefault"/> を指定した場合、 音声データはデフォルト設定のサウンドレンダラに転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合、 音声データはASR（Atom Sound Renderer）に転送されます。
			///  （ASRの出力先は、ASR初期化時に別途指定。） 
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>デコード処理のレイテンシ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デコード処理のレイテンシを指定します。
			///  decode_latency を 0 に設定した場合、プレーヤーは音声再生開始時に 音声データのデコードを可能な限り遅延なく行います。
			///  （オンメモリ再生時は <see cref="CriAtomPlayer.Start"/> 関数を実行後、 最初のサーバー処理内で再生開始に必要な量のデータをデコードし、 音声の出力を開始します。）
			///  これに対し decode_latency を 1 以上に設定した場合、 再生開始に必要なデータのデコードを、複数回のサーバー処理に分割して行います。
			///  （オンメモリ再生時であっても <see cref="CriAtomPlayer.Start"/> 関数を実行後、 decode_latency に指定した回数サーバー処理が動作するまでは音声の出力が開始されません。）
			/// </para>
			/// <para>
			/// 備考:
			/// Atomプレーヤーはサウンドバッファー内のデータ残量を元に、 サーバー処理当たりの音声データのデコード量を決定しています。
			///  音声再生開始前はサウンドバッファーが空の状態のため、 音声再生中に比べて多くのデータ（再生中の 2 ～ 4 倍程度）がデコードされます。
			///  プレーヤー当たりの音声データのデコード処理負荷は小さいため、 一音一音の発音開始時の処理負荷が問題になることはほとんどありません。
			///  しかし、アプリケーション中で 1V に大量の発音リクエストを同時に発行した場合、 全てのプレーヤーの処理負荷のピークが同期し、負荷が目に見えて大きくなる場合があります。
			///  こういった制御を行うケースでは、 decode_latency の値を増やすことで、 局所的に処理負荷が高くなる症状を回避することが可能です。
			///  decode_latency のデフォルト値は、ほとんどの環境で 0 に設定されています。
			///  しかし、携帯ゲーム機等、わずかな負荷変動でもアプリケーションに大きな影響を及ぼす環境では、 デフォルト値が 1 以上に設定されている可能性があります。
			///  （実際にセットされる値については <see cref="CriAtomPlayer.SetDefaultConfigForAdxPlayer"/> メソッドの適用結果を確認してください。）
			///  現状 decode_latency の値に 4 以上の値を指定することはできません。
			///  （ decode_latency に 4 以上の値を指定した場合でも、ライブラリ内で 3 に変更されます。）
			/// </para>
			/// </remarks>
			public Int32 decodeLatency;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。 nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでプレーヤーを作成します。
			///  パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。 パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。 
			/// </para>
			/// </remarks>
			public IntPtr context;

		}
		/// <summary></summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCAが再生可能なプレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomPlayer.CreateHcaPlayer"/> 関数の引数に指定します。
		///  作成されるプレーヤーは、オブジェクト作成時に本構造体で指定された設定に応じて、 内部リソースを必要なだけ確保します。
		///  プレーヤーが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomPlayer.SetDefaultConfigForHcaPlayer"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// <para>HCAプレーヤー作成用コンフィグ構造体 </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateHcaPlayer"/>
		/// <seealso cref="CriAtomPlayer.SetDefaultConfigForHcaPlayer"/>
		[Serializable]
		public unsafe partial struct HcaPlayerConfig
		{
			/// <summary>最大出力チャンネル数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のチャンネル数を指定します。
			/// <see cref="CriAtomPlayer.CreateHcaPlayer"/> 関数で作成されたAtomプレーヤーは、max_channelsで指定した チャンネル数"以下の"音声データを再生可能です。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>最大サンプリングレート </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のサンプリングレートを指定します。
			/// <see cref="CriAtomPlayer.CreateHcaPlayer"/> 関数で作成されたAtomプレーヤーは、max_sampling_rateで指定した サンプリングレート"以下の"音声データを再生可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// 最大サンプリングレートを下げることで、Atomプレーヤー作成時に必要となるワークメモリ のサイズを抑えることが可能です。 
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大サンプリングレートを超えるデータは、再生することはできません。
			///  例えば、最大サンプリングレートを24000に設定した場合、作成されたAtomプレーヤーで 48000Hzの音声を再生することはできません。
			///  （ダウンサンプリングされて出力されることはありません。） 
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>ストリーミング再生を行うかどうか </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーでストリーミング再生（ファイルからの再生）を行うかどうかを指定します。
			///  streaming_flagにfalseを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生（ <see cref="CriAtomPlayer.SetData"/> 関数で指定したメモリアドレスの再生）のみをサポート します。（ファイルからの再生はできません。）
			///  streaming_flagにtrueを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生に加え、ファイルからの再生（ <see cref="CriAtomPlayer.SetFile"/> 関数や <see cref="CriAtomPlayer.SetContentId"/> 関数で指定されたファイルの再生）をサポートします。
			/// </para>
			/// <para>
			/// 補足:
			/// streaming_flagをtrueにした場合、Atomプレーヤー作成時にファイル読み込み用のリソース が確保されます。
			///  そのため、streaming_flagをfalseの場合に比べ、Atomプレーヤーの作成に必要なメモリの サイズが大きくなります。 
			/// </para>
			/// </remarks>
			public NativeBool streamingFlag;

			/// <summary>サウンドレンダラタイプ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーが使用するサウンドレンダラの種別を指定します。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererDefault"/> を指定した場合、 音声データはデフォルト設定のサウンドレンダラに転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合、 音声データはASR（Atom Sound Renderer）に転送されます。
			///  （ASRの出力先は、ASR初期化時に別途指定。） 
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>デコード処理のレイテンシ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デコード処理のレイテンシを指定します。
			///  decode_latency を 0 に設定した場合、プレーヤーは音声再生開始時に 音声データのデコードを可能な限り遅延なく行います。
			///  （オンメモリ再生時は <see cref="CriAtomPlayer.Start"/> 関数を実行後、 最初のサーバー処理内で再生開始に必要な量のデータをデコードし、 音声の出力を開始します。）
			///  これに対し decode_latency を 1 以上に設定した場合、 再生開始に必要なデータのデコードを、複数回のサーバー処理に分割して行います。
			///  （オンメモリ再生時であっても <see cref="CriAtomPlayer.Start"/> 関数を実行後、 decode_latency に指定した回数サーバー処理が動作するまでは音声の出力が開始されません。）
			/// </para>
			/// <para>
			/// 備考:
			/// Atomプレーヤーはサウンドバッファー内のデータ残量を元に、 サーバー処理当たりの音声データのデコード量を決定しています。
			///  音声再生開始前はサウンドバッファーが空の状態のため、 音声再生中に比べて多くのデータ（再生中の 2 ～ 4 倍程度）がデコードされます。
			///  プレーヤー当たりの音声データのデコード処理負荷は小さいため、 一音一音の発音開始時の処理負荷が問題になることはほとんどありません。
			///  しかし、アプリケーション中で 1V に大量の発音リクエストを同時に発行した場合、 全てのプレーヤーの処理負荷のピークが同期し、負荷が目に見えて大きくなる場合があります。
			///  こういった制御を行うケースでは、 decode_latency の値を増やすことで、 局所的に処理負荷が高くなる症状を回避することが可能です。
			///  decode_latency のデフォルト値は、ほとんどの環境で 0 に設定されています。
			///  しかし、携帯ゲーム機等、わずかな負荷変動でもアプリケーションに大きな影響を及ぼす環境では、 デフォルト値が 1 以上に設定されている可能性があります。
			///  （実際にセットされる値については <see cref="CriAtomPlayer.SetDefaultConfigForAdxPlayer"/> メソッドの適用結果を確認してください。）
			///  現状 decode_latency の値に 4 以上の値を指定することはできません。
			///  （ decode_latency に 4 以上の値を指定した場合でも、ライブラリ内で 3 に変更されます。）
			/// </para>
			/// </remarks>
			public Int32 decodeLatency;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。 nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでプレーヤーを作成します。
			///  パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。 パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。 
			/// </para>
			/// </remarks>
			public IntPtr context;

		}
		/// <summary></summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// WAVEが再生可能なプレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomPlayer.CreateWavePlayer"/> 関数の引数に指定します。
		///  作成されるプレーヤーは、オブジェクト作成時に本構造体で指定された設定に応じて、 内部リソースを必要なだけ確保します。
		///  プレーヤーが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomPlayer.SetDefaultConfigForWavePlayer"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// <para>WAVEプレーヤー作成用コンフィグ構造体 </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateWavePlayer"/>
		/// <seealso cref="CriAtomPlayer.SetDefaultConfigForWavePlayer"/>
		[Serializable]
		public unsafe partial struct WavePlayerConfig
		{
			/// <summary>最大出力チャンネル数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のチャンネル数を指定します。
			/// <see cref="CriAtomPlayer.CreateWavePlayer"/> 関数で作成されたAtomプレーヤーは、max_channelsで指定した チャンネル数"以下の"音声データを再生可能です。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>最大サンプリングレート </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のサンプリングレートを指定します。
			/// <see cref="CriAtomPlayer.CreateWavePlayer"/> 関数で作成されたAtomプレーヤーは、max_sampling_rateで指定した サンプリングレート"以下の"音声データを再生可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// 最大サンプリングレートを下げることで、Atomプレーヤー作成時に必要となるワークメモリ のサイズを抑えることが可能です。 
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大サンプリングレートを超えるデータは、再生することはできません。
			///  例えば、最大サンプリングレートを24000に設定した場合、作成されたAtomプレーヤーで 48000Hzの音声を再生することはできません。
			///  （ダウンサンプリングされて出力されることはありません。） 
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>ストリーミング再生を行うかどうか </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーでストリーミング再生（ファイルからの再生）を行うかどうかを指定します。
			///  streaming_flagにfalseを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生（ <see cref="CriAtomPlayer.SetData"/> 関数で指定したメモリアドレスの再生）のみをサポート します。（ファイルからの再生はできません。）
			///  streaming_flagにtrueを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生に加え、ファイルからの再生（ <see cref="CriAtomPlayer.SetFile"/> 関数や <see cref="CriAtomPlayer.SetContentId"/> 関数で指定されたファイルの再生）をサポートします。
			/// </para>
			/// <para>
			/// 補足:
			/// streaming_flagをtrueにした場合、Atomプレーヤー作成時にファイル読み込み用のリソース が確保されます。
			///  そのため、streaming_flagをfalseの場合に比べ、Atomプレーヤーの作成に必要なメモリの サイズが大きくなります。 
			/// </para>
			/// </remarks>
			public NativeBool streamingFlag;

			/// <summary>サウンドレンダラタイプ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーが使用するサウンドレンダラの種別を指定します。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererDefault"/> を指定した場合、 音声データはデフォルト設定のサウンドレンダラに転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合、 音声データはASR（Atom Sound Renderer）に転送されます。
			///  （ASRの出力先は、ASR初期化時に別途指定。） 
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>デコード処理のレイテンシ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デコード処理のレイテンシを指定します。
			///  decode_latency を 0 に設定した場合、プレーヤーは音声再生開始時に 音声データのデコードを可能な限り遅延なく行います。
			///  （オンメモリ再生時は <see cref="CriAtomPlayer.Start"/> 関数を実行後、 最初のサーバー処理内で再生開始に必要な量のデータをデコードし、 音声の出力を開始します。）
			///  これに対し decode_latency を 1 以上に設定した場合、 再生開始に必要なデータのデコードを、複数回のサーバー処理に分割して行います。
			///  （オンメモリ再生時であっても <see cref="CriAtomPlayer.Start"/> 関数を実行後、 decode_latency に指定した回数サーバー処理が動作するまでは音声の出力が開始されません。）
			/// </para>
			/// <para>
			/// 備考:
			/// Atomプレーヤーはサウンドバッファー内のデータ残量を元に、 サーバー処理当たりの音声データのデコード量を決定しています。
			///  音声再生開始前はサウンドバッファーが空の状態のため、 音声再生中に比べて多くのデータ（再生中の 2 ～ 4 倍程度）がデコードされます。
			///  プレーヤー当たりの音声データのデコード処理負荷は小さいため、 一音一音の発音開始時の処理負荷が問題になることはほとんどありません。
			///  しかし、アプリケーション中で 1V に大量の発音リクエストを同時に発行した場合、 全てのプレーヤーの処理負荷のピークが同期し、負荷が目に見えて大きくなる場合があります。
			///  こういった制御を行うケースでは、 decode_latency の値を増やすことで、 局所的に処理負荷が高くなる症状を回避することが可能です。
			///  decode_latency のデフォルト値は、ほとんどの環境で 0 に設定されています。
			///  しかし、携帯ゲーム機等、わずかな負荷変動でもアプリケーションに大きな影響を及ぼす環境では、 デフォルト値が 1 以上に設定されている可能性があります。
			///  （実際にセットされる値については <see cref="CriAtomPlayer.SetDefaultConfigForAdxPlayer"/> メソッドの適用結果を確認してください。）
			///  現状 decode_latency の値に 4 以上の値を指定することはできません。
			///  （ decode_latency に 4 以上の値を指定した場合でも、ライブラリ内で 3 に変更されます。）
			/// </para>
			/// </remarks>
			public Int32 decodeLatency;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。 nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでプレーヤーを作成します。
			///  パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。 パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。 
			/// </para>
			/// </remarks>
			public IntPtr context;

		}
		/// <summary></summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AIFFが再生可能なプレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomPlayer.CreateAiffPlayer"/> 関数の引数に指定します。
		///  作成されるプレーヤーは、オブジェクト作成時に本構造体で指定された設定に応じて、 内部リソースを必要なだけ確保します。
		///  プレーヤーが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomPlayer.SetDefaultConfigForAiffPlayer"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// <para>AIFFプレーヤー作成用コンフィグ構造体 </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateAiffPlayer"/>
		/// <seealso cref="CriAtomPlayer.SetDefaultConfigForAiffPlayer"/>
		[Serializable]
		public unsafe partial struct AiffPlayerConfig
		{
			/// <summary>最大出力チャンネル数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のチャンネル数を指定します。
			/// <see cref="CriAtomPlayer.CreateAiffPlayer"/> 関数で作成されたAtomプレーヤーは、max_channelsで指定した チャンネル数"以下の"音声データを再生可能です。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>最大サンプリングレート </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のサンプリングレートを指定します。
			/// <see cref="CriAtomPlayer.CreateAiffPlayer"/> 関数で作成されたAtomプレーヤーは、max_sampling_rateで指定した サンプリングレート"以下の"音声データを再生可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// 最大サンプリングレートを下げることで、Atomプレーヤー作成時に必要となるワークメモリ のサイズを抑えることが可能です。 
			/// </para>
			/// <para>
			/// 注意:
			/// 指定された最大サンプリングレートを超えるデータは、再生することはできません。
			///  例えば、最大サンプリングレートを24000に設定した場合、作成されたAtomプレーヤーで 48000Hzの音声を再生することはできません。
			///  （ダウンサンプリングされて出力されることはありません。） 
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>ストリーミング再生を行うかどうか </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーでストリーミング再生（ファイルからの再生）を行うかどうかを指定します。
			///  streaming_flagにfalseを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生（ <see cref="CriAtomPlayer.SetData"/> 関数で指定したメモリアドレスの再生）のみをサポート します。（ファイルからの再生はできません。）
			///  streaming_flagにtrueを指定した場合、作成されたAtomプレーヤーはオンメモリのデータ 再生に加え、ファイルからの再生（ <see cref="CriAtomPlayer.SetFile"/> 関数や <see cref="CriAtomPlayer.SetContentId"/> 関数で指定されたファイルの再生）をサポートします。
			/// </para>
			/// <para>
			/// 補足:
			/// streaming_flagをtrueにした場合、Atomプレーヤー作成時にファイル読み込み用のリソース が確保されます。
			///  そのため、streaming_flagをfalseの場合に比べ、Atomプレーヤーの作成に必要なメモリの サイズが大きくなります。 
			/// </para>
			/// </remarks>
			public NativeBool streamingFlag;

			/// <summary>サウンドレンダラタイプ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーが使用するサウンドレンダラの種別を指定します。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererDefault"/> を指定した場合、 音声データはデフォルト設定のサウンドレンダラに転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合、 音声データはASR（Atom Sound Renderer）に転送されます。
			///  （ASRの出力先は、ASR初期化時に別途指定。） 
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>デコード処理のレイテンシ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デコード処理のレイテンシを指定します。
			///  decode_latency を 0 に設定した場合、プレーヤーは音声再生開始時に 音声データのデコードを可能な限り遅延なく行います。
			///  （オンメモリ再生時は <see cref="CriAtomPlayer.Start"/> 関数を実行後、 最初のサーバー処理内で再生開始に必要な量のデータをデコードし、 音声の出力を開始します。）
			///  これに対し decode_latency を 1 以上に設定した場合、 再生開始に必要なデータのデコードを、複数回のサーバー処理に分割して行います。
			///  （オンメモリ再生時であっても <see cref="CriAtomPlayer.Start"/> 関数を実行後、 decode_latency に指定した回数サーバー処理が動作するまでは音声の出力が開始されません。）
			/// </para>
			/// <para>
			/// 備考:
			/// Atomプレーヤーはサウンドバッファー内のデータ残量を元に、 サーバー処理当たりの音声データのデコード量を決定しています。
			///  音声再生開始前はサウンドバッファーが空の状態のため、 音声再生中に比べて多くのデータ（再生中の 2 ～ 4 倍程度）がデコードされます。
			///  プレーヤー当たりの音声データのデコード処理負荷は小さいため、 一音一音の発音開始時の処理負荷が問題になることはほとんどありません。
			///  しかし、アプリケーション中で 1V に大量の発音リクエストを同時に発行した場合、 全てのプレーヤーの処理負荷のピークが同期し、負荷が目に見えて大きくなる場合があります。
			///  こういった制御を行うケースでは、 decode_latency の値を増やすことで、 局所的に処理負荷が高くなる症状を回避することが可能です。
			///  decode_latency のデフォルト値は、ほとんどの環境で 0 に設定されています。
			///  しかし、携帯ゲーム機等、わずかな負荷変動でもアプリケーションに大きな影響を及ぼす環境では、 デフォルト値が 1 以上に設定されている可能性があります。
			///  （実際にセットされる値については <see cref="CriAtomPlayer.SetDefaultConfigForAdxPlayer"/> メソッドの適用結果を確認してください。）
			///  現状 decode_latency の値に 4 以上の値を指定することはできません。
			///  （ decode_latency に 4 以上の値を指定した場合でも、ライブラリ内で 3 に変更されます。）
			/// </para>
			/// </remarks>
			public Int32 decodeLatency;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。 nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでプレーヤーを作成します。
			///  パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。 パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。 
			/// </para>
			/// </remarks>
			public IntPtr context;

		}
		/// <summary></summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// RawPCMが再生可能なプレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// <see cref="CriAtomPlayer.CreateRawPcmPlayer"/> 関数の引数に指定します。
		///  作成されるプレーヤーは、オブジェクト作成時に本構造体で指定された設定に応じて、 内部リソースを必要なだけ確保します。
		///  プレーヤーが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。 
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomPlayer.SetDefaultConfigForRawPcmPlayer"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// <para>RawPCMプレーヤー作成用コンフィグ構造体 </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.CreateRawPcmPlayer"/>
		/// <seealso cref="CriAtomPlayer.SetDefaultConfigForRawPcmPlayer"/>
		[Serializable]
		public unsafe partial struct RawPcmPlayerConfig
		{
			/// <summary>PCMフォーマット </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のPCMフォーマットを指定します。
			/// </para>
			/// <para>
			/// 注意:
			/// 指定されたフォーマット以外のRawPCMフォーマットのデータは再生できません。
			///  再生データがどんなフォーマットであっても、ここで指定されたフォーマットとして再生されます。
			/// </para>
			/// </remarks>
			public CriAtom.PcmFormat pcmFormat;

			/// <summary>出力チャンネル数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のチャンネル数を指定します。
			/// <see cref="CriAtomPlayer.CreateRawPcmPlayer"/> 関数で作成されたAtomプレーヤーは、 max_channelsで指定したチャンネル数"以下の"音声データを再生可能です。
			/// </para>
			/// <para>
			/// 注意:
			/// 指定されたチャンネル数以外のRawPCMフォーマットのデータは再生できません。
			///  再生データがどんなフォーマットであっても、ここで指定されたチャンネル数として再生されます。
			/// </para>
			/// </remarks>
			public Int32 maxChannels;

			/// <summary>サンプリングレート </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーで再生する音声のサンプリングレートを指定します。
			/// <see cref="CriAtomPlayer.CreateRawPcmPlayer"/> 関数で作成されたAtomプレーヤーは、max_sampling_rateで指定した サンプリングレート"以下の"音声データを再生可能です。
			/// </para>
			/// <para>
			/// 備考:
			/// 最大サンプリングレートを下げることで、Atomプレーヤー作成時に必要となるワークメモリ のサイズを抑えることが可能です。 
			/// </para>
			/// <para>
			/// 注意:
			/// 指定されたサンプリングレートと違うデータは再生できません。
			///  再生データがどんなフォーマットであっても、ここで指定されたサンプリングレートとして再生されます。
			/// </para>
			/// </remarks>
			public Int32 maxSamplingRate;

			/// <summary>サウンドレンダラタイプ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// Atomプレーヤーが使用するサウンドレンダラの種別を指定します。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererDefault"/> を指定した場合、 音声データはデフォルト設定のサウンドレンダラに転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Native"/> を指定した場合、 音声データはデフォルト設定の各プラットフォームのサウンド出力に転送されます。
			///  sound_renderer_type に <see cref="CriAtom.SoundRendererType.Asr"/> を指定した場合、 音声データはASR（Atom Sound Renderer）に転送されます。
			///  （ASRの出力先は、ASR初期化時に別途指定。） 
			/// </para>
			/// </remarks>
			public CriAtom.SoundRendererType soundRendererType;

			/// <summary>デコード処理のレイテンシ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// デコード処理のレイテンシを指定します。
			///  decode_latency を 0 に設定した場合、プレーヤーは音声再生開始時に 音声データのデコードを可能な限り遅延なく行います。
			///  （オンメモリ再生時は <see cref="CriAtomPlayer.Start"/> 関数を実行後、 最初のサーバー処理内で再生開始に必要な量のデータをデコードし、 音声の出力を開始します。）
			///  これに対し decode_latency を 1 以上に設定した場合、 再生開始に必要なデータのデコードを、複数回のサーバー処理に分割して行います。
			///  （オンメモリ再生時であっても <see cref="CriAtomPlayer.Start"/> 関数を実行後、 decode_latency に指定した回数サーバー処理が動作するまでは音声の出力が開始されません。）
			/// </para>
			/// <para>
			/// 備考:
			/// Atomプレーヤーはサウンドバッファー内のデータ残量を元に、 サーバー処理当たりの音声データのデコード量を決定しています。
			///  音声再生開始前はサウンドバッファーが空の状態のため、 音声再生中に比べて多くのデータ（再生中の 2 ～ 4 倍程度）がデコードされます。
			///  プレーヤー当たりの音声データのデコード処理負荷は小さいため、 一音一音の発音開始時の処理負荷が問題になることはほとんどありません。
			///  しかし、アプリケーション中で 1V に大量の発音リクエストを同時に発行した場合、 全てのプレーヤーの処理負荷のピークが同期し、負荷が目に見えて大きくなる場合があります。
			///  こういった制御を行うケースでは、 decode_latency の値を増やすことで、 局所的に処理負荷が高くなる症状を回避することが可能です。
			///  decode_latency のデフォルト値は、ほとんどの環境で 0 に設定されています。
			///  しかし、携帯ゲーム機等、わずかな負荷変動でもアプリケーションに大きな影響を及ぼす環境では、 デフォルト値が 1 以上に設定されている可能性があります。
			///  （実際にセットされる値については <see cref="CriAtomPlayer.SetDefaultConfigForRawPcmPlayer"/> メソッドの適用結果を確認してください。）
			///  現状 decode_latency の値に 4 以上の値を指定することはできません。
			///  （ decode_latency に 4 以上の値を指定した場合でも、ライブラリ内で 3 に変更されます。）
			/// </para>
			/// </remarks>
			public Int32 decodeLatency;

			/// <summary>プラットフォーム固有のパラメーターへのポインタ </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// プラットフォーム固有のパラメーターへのポインタを指定します。 nullを指定した場合、プラットフォーム毎のデフォルトパラメーターでプレーヤーを作成します。
			///  パラメーター構造体は各プラットフォーム固有ヘッダーに定義されています。 パラメーター構造体が定義されていないプラットフォームでは、常にnullを指定してください。 
			/// </para>
			/// </remarks>
			public IntPtr context;

		}
		/// <summary>PCMフォーマット </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// PCMデータの型情報です。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetFilterCallback"/>
		public enum PcmFormat
		{
			Sint16 = 0,
			Float32 = 1,
		}
		/// <summary>音声データフォーマット情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声データのフォーマット情報です。
		/// <see cref="CriAtomPlayer.GetFormatInfo"/> 関数で使用します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.GetFormatInfo"/>
		public unsafe partial struct FormatInfo
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>フォーマット種別 </para>
			/// </remarks>
			public UInt32 format;

			/// <summary></summary>
			/// <remarks>
			/// <para>サンプリング周波数 </para>
			/// </remarks>
			public Int32 samplingRate;

			/// <summary></summary>
			/// <remarks>
			/// <para>総サンプル数 </para>
			/// </remarks>
			public Int64 numSamples;

			/// <summary></summary>
			/// <remarks>
			/// <para>ループ開始サンプル </para>
			/// </remarks>
			public Int64 loopOffset;

			/// <summary></summary>
			/// <remarks>
			/// <para>ループ区間サンプル数 </para>
			/// </remarks>
			public Int64 loopLength;

			/// <summary></summary>
			/// <remarks>
			/// <para>チャンネル数 </para>
			/// </remarks>
			public Int32 numChannels;

			/// <summary></summary>
			/// <remarks>
			/// <para>予約領域 </para>
			/// </remarks>
			public InlineArray1<UInt32> reserved;

		}
		/// <summary>スピーカーID </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 音声を出力するスピーカーを指定するためのIDです。
		/// <see cref="CriAtomPlayer.SetSendLevel"/> 関数で利用します。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomPlayer.SetSendLevel"/>
		public enum SpeakerId
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>フロントレフトスピーカー </para>
			/// </remarks>
			FrontLeft = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>フロントライトスピーカー </para>
			/// </remarks>
			FrontRight = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>フロントセンタースピーカー </para>
			/// </remarks>
			FrontCenter = 2,
			/// <summary></summary>
			/// <remarks>
			/// <para>LFE（≒サブウーハー） </para>
			/// </remarks>
			LowFrequency = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>サラウンドレフトスピーカー </para>
			/// </remarks>
			SurroundLeft = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>サラウンドライトスピーカー </para>
			/// </remarks>
			SurroundRight = 5,
			/// <summary></summary>
			/// <remarks>
			/// <para>サラウンドバックレフトスピーカー </para>
			/// </remarks>
			SurroundBackLeft = 6,
			/// <summary></summary>
			/// <remarks>
			/// <para>サラウンドバックライトスピーカー </para>
			/// </remarks>
			SurroundBackRight = 7,
			/// <summary></summary>
			/// <remarks>
			/// <para>トップフロントレフトスピーカー </para>
			/// </remarks>
			TopFrontLeft = 8,
			/// <summary></summary>
			/// <remarks>
			/// <para>トップフロントライトスピーカー </para>
			/// </remarks>
			TopFrontRight = 9,
			/// <summary></summary>
			/// <remarks>
			/// <para>トップバックレフトスピーカー </para>
			/// </remarks>
			TopBackLeft = 10,
			/// <summary></summary>
			/// <remarks>
			/// <para>トップバックライトスピーカー </para>
			/// </remarks>
			TopBackRight = 11,
		}
		/// <summary>パラメーターID </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラメーターを指定するためのIDです。
		/// </para>
		/// </remarks>
		public enum ParameterId
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>ボリューム </para>
			/// </remarks>
			Volume = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>周波数比 </para>
			/// </remarks>
			FrequencyRatio = 1,
		}
		/// <summary>レベルメーター機能追加用コンフィグ構造体 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// レベルメーター機能を追加するための構造体です。
		/// <see cref="CriAtomMeter.AttachLevelMeter"/> 関数の引数に指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomMeter.SetDefaultConfigForLevelMeter"/> メソッドで 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomMeter.AttachLevelMeter"/> 関数 に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomMeter.SetDefaultConfigForLevelMeter"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLevelMeter"/>
		[Serializable]
		public unsafe partial struct LevelMeterConfig
		{
			/// <summary>測定間隔（ミリ秒単位） </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定結果を更新する間隔です。
			/// </para>
			/// </remarks>
			public Int32 interval;

			/// <summary>ピークホールド時間（ミリ秒単位） </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ピーク値がより大きい値で更新されたとき、下がらないようにホールドする時間です。
			/// </para>
			/// </remarks>
			public Int32 holdTime;

		}
		/// <summary>レベル情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// レベル情報を取得するための構造体です。
		/// <see cref="CriAtomMeter.GetLevelInfo"/> 関数で利用します。 
		/// </para>
		/// <para>
		/// 備考:
		/// 各レベル値の単位はdBです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.GetLevelInfo"/>
		public unsafe partial struct LevelInfo
		{
			/// <summary>有効チャンネル数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定結果が有効なチャンネル数です。
			/// </para>
			/// </remarks>
			public Int32 numChannels;

			/// <summary>RMSレベル </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定間隔間の音声振幅のRMS（二乗平均平方根）を計算した値です。
			///  音圧レベルとして扱われます。 
			/// </para>
			/// </remarks>
			public InlineArray16<Single> rmsLevels;

			/// <summary>ピークレベル </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定間隔間の音声振幅の最大値です。
			/// </para>
			/// </remarks>
			public InlineArray16<Single> peakLevels;

			/// <summary>ピークホールドレベル </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ホールドしているピークレベル値です。
			/// </para>
			/// </remarks>
			public InlineArray16<Single> peakHoldLevels;

		}
		/// <summary>ラウドネスメーター機能追加用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomMeter.SetDefaultConfigForLoudnessMeter"/> メソッドで 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomMeter.AttachLoudnessMeter"/> 関数 に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomMeter.SetDefaultConfigForLoudnessMeter"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachLoudnessMeter"/>
		[Serializable]
		public unsafe partial struct LoudnessMeterConfig
		{
			/// <summary>ショートターム測定時間 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 短期の平均ラウドネスの測定時間（秒単位）です。
			///  標準は3秒です。
			/// </para>
			/// </remarks>
			public Int32 shortTermTime;

			/// <summary>インテグレーテッド測定時間 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 長期の平均ラウドネスの測定時間（秒単位）です。
			///  インテグレーテッド値はコンテンツ単位の平均ラウドネスです。
			/// </para>
			/// </remarks>
			public Int32 integratedTime;

		}
		/// <summary>ラウドネス情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ラウドネス情報を取得するための構造体です。
		/// <see cref="CriAtomMeter.GetLoudnessInfo"/> 関数で利用します。 
		/// </para>
		/// <para>
		/// 備考:
		/// 各レベル値の単位はLKFSです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.GetLoudnessInfo"/>
		public unsafe partial struct LoudnessInfo
		{
			/// <summary>測定カウント </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定した回数です。
			///  0.1秒に1回測定されます。
			/// </para>
			/// </remarks>
			public Int32 count;

			/// <summary>モーメンタリー値 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 瞬間のラウドネスレベルです。
			/// </para>
			/// </remarks>
			public Single momentary;

			/// <summary>ショートターム値 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 短期のラウドネス平均レベルです。
			/// </para>
			/// </remarks>
			public Single shortTerm;

			/// <summary>インテグレーテッド値 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 長期のラウドネス平均レベルです。
			/// </para>
			/// </remarks>
			public Single integrated;

		}
		/// <summary>トゥルーピークメーター機能追加用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 備考:
		/// デフォルト設定を使用する場合、 <see cref="CriAtomMeter.SetDefaultConfigForTruePeakMeter"/> メソッドで 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomMeter.AttachTruePeakMeter"/> 関数 に構造体を指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomMeter.SetDefaultConfigForTruePeakMeter"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		///  （構造体のメンバに不定値が入らないようご注意ください。） 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.AttachTruePeakMeter"/>
		[Serializable]
		public unsafe partial struct TruePeakMeterConfig
		{
			/// <summary>クリッピング </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 波形のサンプルを測定前にクリップするフラグです。
			///  波形のサンプルを測定前にクリップするフラグです。
			/// </para>
			/// </remarks>
			public NativeBool sampleClipping;

			/// <summary>測定間隔（ミリ秒単位） </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定結果を更新する間隔です。
			/// </para>
			/// </remarks>
			public Int32 interval;

			/// <summary>ホールド時間（ミリ秒単位） </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ピーク値がより大きい値で更新されたとき、下がらないようにホールドする時間です。
			/// </para>
			/// </remarks>
			public Int32 holdTime;

		}
		/// <summary>トゥルーピーク情報 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// トゥルーピーク情報を取得するための構造体です。
		/// <see cref="CriAtomMeter.GetTruePeakInfo"/> 関数で利用します。 
		/// </para>
		/// <para>
		/// 備考:
		/// 各レベル値の単位はdBです。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomMeter.GetTruePeakInfo"/>
		public unsafe partial struct TruePeakInfo
		{
			/// <summary>有効チャンネル数 </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// 測定結果が有効なチャンネル数です。
			/// </para>
			/// </remarks>
			public Int32 numChannels;

			/// <summary>トゥルーピークレベル </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// トゥルーピークメーターの測定結果です。
			/// </para>
			/// </remarks>
			public InlineArray16<Single> levels;

			/// <summary>ピークホールドレベル </summary>
			/// <remarks>
			/// <para>
			/// 説明:
			/// ホールドしているピークレベルです。
			/// </para>
			/// </remarks>
			public InlineArray16<Single> holdLevels;

		}
		/// <summary>スピーカーマッピング </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スピーカー構成を表します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsr.Config"/>
		/// <seealso cref="CriAtomExAsrRack.Config"/>
		public enum SpeakerMapping
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>自動設定 </para>
			/// </remarks>
			Auto = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>1ch </para>
			/// </remarks>
			Mono = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>2ch </para>
			/// </remarks>
			Stereo = 2,
			/// <summary></summary>
			/// <remarks>
			/// <para>5.1ch </para>
			/// </remarks>
			_5_1 = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1ch </para>
			/// </remarks>
			_7_1 = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>5.1.2ch </para>
			/// </remarks>
			_5_1_2 = 5,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1.2ch </para>
			/// </remarks>
			_7_1_2 = 6,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1.4ch </para>
			/// </remarks>
			_7_1_4 = 7,
			/// <summary></summary>
			/// <remarks>
			/// <para>7.1.4.4ch </para>
			/// </remarks>
			_7_1_4_4 = 8,
			/// <summary></summary>
			/// <remarks>
			/// <para>1st order Ambisonics </para>
			/// </remarks>
			Ambisonics1p = 9,
			/// <summary></summary>
			/// <remarks>
			/// <para>2nd order Ambisonics </para>
			/// </remarks>
			Ambisonics2p = 10,
			/// <summary></summary>
			/// <remarks>
			/// <para>3rd order Ambisonics </para>
			/// </remarks>
			Ambisonics3p = 11,
			/// <summary></summary>
			/// <remarks>
			/// <para>オブジェクトベース再生 </para>
			/// </remarks>
			Object = 12,
			Custom = 13,
		}
		/// <summary>Ambisonicsオーダー（廃止済み） </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Ambisonicsオーダータイプを示します。現在は廃止済みです。 
		/// </para>
		/// </remarks>
		public enum AmbisonicsOrderType
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>未設定 </para>
			/// </remarks>
			None = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>1st Order </para>
			/// </remarks>
			First = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>1 Periphonic(1st Orderと同義) </para>
			/// </remarks>
			_1p = 1,
		}
		/// <summary>デバイスタイプ </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 現在再生しているデバイスの種類を表します。
		/// <see cref="CriAtomExAsrRack.GetDeviceType"/> 関数でASRラックが出力しているデバイスの種類を 取得することができます。
		/// </para>
		/// <para>
		/// 備考:
		/// 各タイプが具体的にどのデバイスを指すかはプラットフォームによって異なります。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAsrRack.GetDeviceType"/>
		public enum DeviceType
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>HDMI </para>
			/// </remarks>
			Hdmi = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>ヘッドホン </para>
			/// </remarks>
			Headphone = 2,
			/// <summary></summary>
			/// <remarks>
			/// <para>内蔵スピーカー </para>
			/// </remarks>
			BuiltInSpeaker = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>パッドスピーカー </para>
			/// </remarks>
			PadSpeaker = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>振動 </para>
			/// </remarks>
			Vibration = 5,
			/// <summary></summary>
			/// <remarks>
			/// <para>デバイスタイプの確認ができない </para>
			/// </remarks>
			Unknown = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>出力デバイスが存在しない </para>
			/// </remarks>
			Unavailable = -1,
		}
		/// <summary>ボイス停止理由 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ボイスの停止理由を表します。 
		/// </para>
		/// </remarks>
		public enum VoiceStopReason
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>EXプレーヤー停止 </para>
			/// </remarks>
			ReasonExplayerStop = 0,
			/// <summary></summary>
			/// <remarks>
			/// <para>EXプレーヤー停止 </para>
			/// </remarks>
			ReasonExplayerStopwithoutrelease = 1,
			/// <summary></summary>
			/// <remarks>
			/// <para>再生ID指定停止 </para>
			/// </remarks>
			ReasonPlaybackStop = 2,
			/// <summary></summary>
			/// <remarks>
			/// <para>再生ID指定即時停止 </para>
			/// </remarks>
			ReasonPlaybackStopwithoutrelease = 3,
			/// <summary></summary>
			/// <remarks>
			/// <para>サウンドプレーヤー破棄 </para>
			/// </remarks>
			ReasonSoundplayerDestroy = 4,
			/// <summary></summary>
			/// <remarks>
			/// <para>フェーダー停止 </para>
			/// </remarks>
			ReasonFaderStop = 5,
			/// <summary></summary>
			/// <remarks>
			/// <para>プレーヤー停止 </para>
			/// </remarks>
			ReasonPlayerStop = 6,
			/// <summary></summary>
			/// <remarks>
			/// <para>AWB停止 </para>
			/// </remarks>
			ReasonAwbStop = 7,
			/// <summary></summary>
			/// <remarks>
			/// <para>ストリーミングキャッシュ停止 </para>
			/// </remarks>
			ReasonStreamingCacheStop = 8,
			/// <summary></summary>
			/// <remarks>
			/// <para>プレーヤー停止 </para>
			/// </remarks>
			ReasonPlayerForceStop = 9,
			/// <summary></summary>
			/// <remarks>
			/// <para>プレーヤー破棄 </para>
			/// </remarks>
			ReasonPlayerDestroy = 10,
			/// <summary></summary>
			/// <remarks>
			/// <para>MTプレーヤー破棄 </para>
			/// </remarks>
			ReasonMtplayerStop = 11,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイスプール破棄 </para>
			/// </remarks>
			ReasonVoicePoolDestroy = 12,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイス奪い取り </para>
			/// </remarks>
			ReasonVoiceStealCase1 = 13,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイス奪い取り </para>
			/// </remarks>
			ReasonVoiceStealCase2 = 14,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイス奪い取り </para>
			/// </remarks>
			ReasonVoiceStealCase3 = 15,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイス奪い取り </para>
			/// </remarks>
			ReasonVoiceStealCase4 = 16,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultVirtualVoiceBeyondLifeTime = 17,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultCantCalc3dpos = 18,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultInternalPlaybackCancel = 19,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultNoPlayerNoRetry = 20,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultFailedRetryBeyondLifeTime = 21,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultPlayerStatusPlayend = 22,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultPlayerStatusError = 23,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultImpossibleRetry = 24,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultPlayerStatusStop = 25,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultInvalidServerRequest = 26,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultSilentModeStop = 27,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultSoundcomplexStatusError = 28,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultNoElementCase1 = 29,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultNoElementCase2 = 30,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultStopNotPlayingElement = 31,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultNotActiveVoice = 32,
			/// <summary></summary>
			/// <remarks>
			/// <para>エレメント解放 </para>
			/// </remarks>
			ReasonElementFree = 33,
			/// <summary></summary>
			/// <remarks>
			/// <para>エラープレーヤーの停止 </para>
			/// </remarks>
			ReasonExplayerStopErrorHn = 34,
			/// <summary></summary>
			/// <remarks>
			/// <para>ACB解放 </para>
			/// </remarks>
			ReasonAcbRelease = 35,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイス確保失敗 </para>
			/// </remarks>
			ReasonVoiceAllocateFail = 36,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイスリセット </para>
			/// </remarks>
			ReasonVoiceReset = 37,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイスリバーチャル化 </para>
			/// </remarks>
			ReasonVoiceVirtualize = 38,
			/// <summary></summary>
			/// <remarks>
			/// <para>プレイバックサウンド確保失敗 </para>
			/// </remarks>
			ReasonPlaybacksoundAllocateFail = 39,
			/// <summary></summary>
			/// <remarks>
			/// <para>ビート同期停止アクション </para>
			/// </remarks>
			ReasonStopActionWithBeatsync = 40,
			/// <summary></summary>
			/// <remarks>
			/// <para>ビート同期再生キャンセル </para>
			/// </remarks>
			ReasonStartCancelWithBeatsync = 41,
			/// <summary></summary>
			/// <remarks>
			/// <para>停止アクション </para>
			/// </remarks>
			ReasonStopAction = 42,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロック遷移 </para>
			/// </remarks>
			ReasonBlockTransitionCase0 = 43,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロック遷移 </para>
			/// </remarks>
			ReasonBlockTransitionCase1 = 44,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロック遷移 </para>
			/// </remarks>
			ReasonBlockTransitionCase2 = 45,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロック遷移 </para>
			/// </remarks>
			ReasonBlockTransitionCase3 = 46,
			/// <summary></summary>
			/// <remarks>
			/// <para>カテゴリキューリミット </para>
			/// </remarks>
			ReasonCategoryCueLimit = 47,
			/// <summary></summary>
			/// <remarks>
			/// <para>ACB内部解放 </para>
			/// </remarks>
			ReasonUnsetAcb = 48,
			/// <summary></summary>
			/// <remarks>
			/// <para>シーケンスエンドマーカー </para>
			/// </remarks>
			ReasonSequenceEnd = 49,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロックエンド </para>
			/// </remarks>
			ReasonBlodkEnd = 50,
			/// <summary></summary>
			/// <remarks>
			/// <para>シーケンスサーバー処理 </para>
			/// </remarks>
			ReasonSequenceExecute = 51,
			/// <summary></summary>
			/// <remarks>
			/// <para>トラックモノモード処理 </para>
			/// </remarks>
			ReasonTrackMono = 52,
			/// <summary></summary>
			/// <remarks>
			/// <para>フェーダー即時停止 </para>
			/// </remarks>
			ReasonFaderStopImmediate = 53,
			/// <summary></summary>
			/// <remarks>
			/// <para>キューリミット処理 </para>
			/// </remarks>
			ReasonCueLimit = 54,
			/// <summary></summary>
			/// <remarks>
			/// <para>(廃止)全ACB停止処理 </para>
			/// </remarks>
			ReasonStopAcbNouse = 55,
			/// <summary></summary>
			/// <remarks>
			/// <para>サウンドオブジェクトへのプレーヤー追加処理 </para>
			/// </remarks>
			ReasonSoundObjectAddPlayer = 56,
			/// <summary></summary>
			/// <remarks>
			/// <para>サウンドオブジェクトからプレーヤーの削除処理 </para>
			/// </remarks>
			ReasonSoundObjectDeletePlayer = 57,
			/// <summary></summary>
			/// <remarks>
			/// <para>サウンドオブジェクトからプレーヤーの削除処理 </para>
			/// </remarks>
			ReasonSoundObjectDeleteAllPlayer = 58,
			/// <summary></summary>
			/// <remarks>
			/// <para>ACFのアンレジスト処理 </para>
			/// </remarks>
			ReasonUnregisterAcf = 59,
			/// <summary></summary>
			/// <remarks>
			/// <para>CriAtomExPlayerハンドルの破棄 </para>
			/// </remarks>
			ReasonExplayerDestroy = 60,
			/// <summary></summary>
			/// <remarks>
			/// <para>CriAtomExPlayerへのフェーダー付加 </para>
			/// </remarks>
			ReasonExplayerAttachFader = 61,
			/// <summary></summary>
			/// <remarks>
			/// <para>CriAtomExPlayerへのフェーダー取り外し </para>
			/// </remarks>
			ReasonExplayerDetachFader = 62,
			/// <summary></summary>
			/// <remarks>
			/// <para>AWB解放処理 </para>
			/// </remarks>
			ReasonDetachAwb = 63,
			/// <summary></summary>
			/// <remarks>
			/// <para>多重再生禁止時間内再生 </para>
			/// </remarks>
			MultiplePlaybackProhibitionTime = 64,
			/// <summary></summary>
			/// <remarks>
			/// <para>カテゴリ停止 </para>
			/// </remarks>
			ReasonCategoryStop = 65,
			/// <summary></summary>
			/// <remarks>
			/// <para>カテゴリ即時停止 </para>
			/// </remarks>
			ReasonCategoryStopwithoutrelease = 66,
			/// <summary></summary>
			/// <remarks>
			/// <para>タイムライン停止 </para>
			/// </remarks>
			ReasonNoteOff = 67,
			/// <summary></summary>
			/// <remarks>
			/// <para>ACFの登録処理 </para>
			/// </remarks>
			ReasonRegisterAcf = 68,
			/// <summary></summary>
			/// <remarks>
			/// <para>フェード付き停止アクション </para>
			/// </remarks>
			ReasonStopActionWithFade = 69,
			/// <summary></summary>
			/// <remarks>
			/// <para>トラックパラメーターのリセット </para>
			/// </remarks>
			ReasonResetTrackParameter = 70,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロック遷移 </para>
			/// </remarks>
			ReasonBlockTransitionCase4 = 71,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロック遷移 </para>
			/// </remarks>
			ReasonBlockTransitionCase5 = 72,
			/// <summary></summary>
			/// <remarks>
			/// <para>ブロック遷移 </para>
			/// </remarks>
			ReasonBlockTransitionCase6 = 73,
			/// <summary></summary>
			/// <remarks>
			/// <para>ノート停止 </para>
			/// </remarks>
			ReasonNoteOff2 = 74,
			/// <summary></summary>
			/// <remarks>
			/// <para>全ノート停止 </para>
			/// </remarks>
			ReasonAllNoteOff = 75,
			/// <summary></summary>
			/// <remarks>
			/// <para>全ノート停止 </para>
			/// </remarks>
			ReasonAllNoteOffWithoutrelease = 76,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理 </para>
			/// </remarks>
			ReasonSoundServerResultEnvelopeLevelZero = 77,
			/// <summary></summary>
			/// <remarks>
			/// <para>外部入力停止 </para>
			/// </remarks>
			ReasonAuxInStop = 78,
			/// <summary></summary>
			/// <remarks>
			/// <para>非同期ACB解放 </para>
			/// </remarks>
			ReasonAcbReleaseAsync = 79,
			/// <summary></summary>
			/// <remarks>
			/// <para>InGamePreview ACF 更新前処理 </para>
			/// </remarks>
			ReasonIngamepreviewPrepareOverwriteAcf = 80,
			/// <summary></summary>
			/// <remarks>
			/// <para>InGamePreview ACB 更新前処理 </para>
			/// </remarks>
			ReasonIngamepreviewPrepareOverwriteAcb = 81,
			/// <summary></summary>
			/// <remarks>
			/// <para>レジュームでのキューリミット処理 </para>
			/// </remarks>
			ReasonCueLimitResumePrepare = 82,
			/// <summary></summary>
			/// <remarks>
			/// <para>MidiPlayer ノートオフ </para>
			/// </remarks>
			ReasonMidiPlayerNoteOff = 83,
			/// <summary></summary>
			/// <remarks>
			/// <para>サーバー処理：OBA 再生でのボイスドロップ発生 </para>
			/// </remarks>
			ReasonVoiceDropInOba = 84,
			/// <summary></summary>
			/// <remarks>
			/// <para>ボイスリミットによるキャンセル </para>
			/// </remarks>
			ReasonCancelVoiceLimit = 85,
			ReasonNone = 2147483646,
		}

		public const Int32 DefaultOutputSamplingRate = (48000);

		public const CriAtom.SoundRendererType SoundRendererDefault = (CriAtom.SoundRendererType.Asr);
		/// <summary>不正なストリーミングキャッシュID値 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomStreamingCache.CriAtomStreamingCache"/> 関数に失敗した際に返る値です。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomStreamingCache.CriAtomStreamingCache"/>
		/// <seealso cref="CriAtomStreamingCache.Dispose"/>
		public const Int32 StreamingCacheIllegalId = (0);
		/// <summary>HCA-MXの出力チャンネル数の最大値 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// HCA-MXの出力チャンネル数の最大値です。 <see cref="CriAtomHcaMx.Config"/>::output_channels の値は、この値以下に設定する必要があります。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomHcaMx.Config"/>
		public const Int32 HcaMxMaxOutputChannels = (8);
		/// <summary></summary>
		/// <remarks>
		/// <para>ADX </para>
		/// </remarks>
		public const Int32 FormatAdx = (0x00000001);
		/// <summary></summary>
		/// <remarks>
		/// <para>HCA </para>
		/// </remarks>
		public const Int32 FormatHca = (0x00000003);
		/// <summary></summary>
		/// <remarks>
		/// <para>HCA-MX </para>
		/// </remarks>
		public const Int32 FormatHcaMx = (0x00000004);
		/// <summary></summary>
		/// <remarks>
		/// <para>Wave </para>
		/// </remarks>
		public const Int32 FormatWave = (0x00000005);
		/// <summary></summary>
		/// <remarks>
		/// <para>Raw PCM </para>
		/// </remarks>
		public const Int32 FormatRawPcm = (0x00000006);
		/// <summary></summary>
		/// <remarks>
		/// <para>AIFF </para>
		/// </remarks>
		public const Int32 FormatAiff = (0x00000007);
		/// <summary></summary>
		/// <remarks>
		/// <para>振動 </para>
		/// </remarks>
		public const Int32 FormatVibration = (0x00000008);
		/// <summary></summary>
		/// <remarks>
		/// <para>AudioBuffer </para>
		/// </remarks>
		public const Int32 FormatAudioBuffer = (0x00000009);
		/// <summary></summary>
		/// <remarks>
		/// <para>ハードウェア固有 </para>
		/// </remarks>
		public const Int32 FormatHw1 = (0x00010001);
		/// <summary></summary>
		/// <remarks>
		/// <para>ハードウェア固有 </para>
		/// </remarks>
		public const Int32 FormatHw2 = (0x00010002);




	}
}