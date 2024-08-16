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
	/// <summary>ACBオブジェクト</summary>
	/// <remarks>
	/// <para>
	/// 説明:
	/// キューシート情報を管理するオブジェクトです。
	/// <see cref="CriAtomExAcb.LoadAcbFile"/> 関数等で読み込んだキューシートファイル内の
	/// 音声を再生する場合、本オブジェクトとキューIDをプレーヤーに対してセットします。
	/// </para>
	/// </remarks>
	/// <seealso cref="CriAtomExAcb.LoadAcbFile"/>
	/// <seealso cref="CriAtomExPlayer.SetCueId"/>
	public partial class CriAtomExAcb : IDisposable
	{
		/// <summary>オンメモリACBデータのロードに必要なワーク領域サイズの計算</summary>
		/// <param name="acbData">ACBデータアドレス</param>
		/// <param name="acbDataSize">ACBデータサイズ</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbPath">AWBファイルのパス</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAcb.LoadAcbData"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExAcb.LoadAcbData"/> 関数でAWBデータをロードする際には、
		/// 本関数が返すサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.LoadAcbData"/>
		public static Int32 CalculateWorkSizeForLoadAcbData(IntPtr acbData, Int32 acbDataSize, CriFsBinder awbBinder, ArgString awbPath)
		{
			return NativeMethods.criAtomExAcb_CalculateWorkSizeForLoadAcbData(acbData, acbDataSize, awbBinder?.NativeHandle ?? default, awbPath.GetPointer(stackalloc byte[awbPath.BufferSize]));
		}

		/// <summary>オンメモリACBデータのロードに必要なワーク領域サイズの計算（CPKコンテンツID指定）</summary>
		/// <param name="acbData">ACBデータアドレス</param>
		/// <param name="acbDataSize">ACBデータサイズ</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbId">CPKファイル内のAWBデータのID</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAcb.LoadAcbDataById"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// ファイルパスの代わりにCPKコンテンツIDを指定する点を除けば、
		/// <see cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbData"/> 関数と機能は同じです。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbData"/>
		/// <seealso cref="CriAtomExAcb.LoadAcbDataById"/>
		public static Int32 CalculateWorkSizeForLoadAcbDataById(IntPtr acbData, Int32 acbDataSize, CriFsBinder awbBinder, UInt16 awbId)
		{
			return NativeMethods.criAtomExAcb_CalculateWorkSizeForLoadAcbDataById(acbData, acbDataSize, awbBinder?.NativeHandle ?? default, awbId);
		}

		/// <summary>オンメモリACBデータのロード</summary>
		/// <param name="acbData">ACBデータアドレス</param>
		/// <param name="acbDataSize">ACBデータサイズ</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbPath">AWBファイルのパス</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ACBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBデータをロードし、キュー再生に必要な情報を取り込みます。
		/// ACBデータのロードに必要なワーク領域のサイズは、
		/// <see cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbData"/> 関数で計算します。
		/// 第3引数の awb_binder 、および第4引数の awb_path には、ストリーム再生用
		/// のAWBファイルを指定します。
		/// （オンメモリ再生のみのACBデータをロードする場合、 awb_binder および
		/// awb_path にセットした値は無視されます。）
		/// ACBデータをロードすると、ACBデータにアクセスするためのACBオブジェクト
		/// （ <see cref="CriAtomExAcb"/> ）が返されます。
		/// AtomExプレーヤーに対し、 <see cref="CriAtomExPlayer.SetCueId"/> 関数でACBオブジェクト、および再生する
		/// キューのIDをセットすることで、ACBデータ内のキューを再生することが可能です。
		/// ACBファイルのロードに成功すると、本関数は戻り値として ACB オブジェクトを返します。
		/// リードエラー等によりACBファイルのロードに失敗した場合、本関数は戻り値として
		/// CRI_NULL を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// 本関数は即時復帰関数です。
		/// ACBファイルを事前にメモリにロードしてから本関数を実行することで、
		/// ACBオブジェクト作成時に処理がブロックされるのを回避可能です。
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数にてセットしたデータ領域やワーク領域のメモリ内容はACBオブジェクト破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// また、データ領域の一部はワークとして使用されます。
		/// ACBデータにはワーク領域も含まれています。
		/// そのため、1つのACBデータ領域を複数回同時にロードすることはできません。
		/// （作成されたACBオブジェクトを複数のAtomExプレーヤーで共有することは可能です。）
		/// ACBオブジェクトは内部的にバインダー（ <see cref="CriFsBinder"/> ）を確保します。
		/// ACBファイルをロードする場合、ACBオブジェクト数分のバインダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbData"/>
		/// <seealso cref="CriAtomExAcb"/>
		/// <seealso cref="CriAtomExPlayer.SetCueId"/>
		public static CriAtomExAcb LoadAcbData(IntPtr acbData, Int32 acbDataSize, CriFsBinder awbBinder, ArgString awbPath, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_LoadAcbData(acbData, acbDataSize, awbBinder?.NativeHandle ?? default, awbPath.GetPointer(stackalloc byte[awbPath.BufferSize]), work, workSize)) == IntPtr.Zero) ? null : new CriAtomExAcb(handle);
		}

		/// <summary>オンメモリACBデータのロード（CPKコンテンツID指定）</summary>
		/// <param name="acbData">ACBデータアドレス</param>
		/// <param name="acbDataSize">ACBデータサイズ</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbId">CPKファイル内のAWBデータのID</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ACBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBデータをロードし、キュー再生に必要な情報を取り込みます。
		/// ファイルパスの代わりにCPKコンテンツIDを指定する点を除けば、
		/// <see cref="CriAtomExAcb.LoadAcbData"/> 関数と機能は同じです。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.LoadAcbData"/>
		public static CriAtomExAcb LoadAcbDataById(IntPtr acbData, Int32 acbDataSize, CriFsBinder awbBinder, UInt16 awbId, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_LoadAcbDataById(acbData, acbDataSize, awbBinder?.NativeHandle ?? default, awbId, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExAcb(handle);
		}

		/// <summary>ACBファイルのロードに必要なワーク領域サイズの計算</summary>
		/// <param name="acbBinder">ACBファイルを含むバインダーのオブジェクト</param>
		/// <param name="acbPath">ACBファイルのパス</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbPath">AWBファイルのパス</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAcb.LoadAcbFile"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドによるアロケーター登録を行わずに
		/// <see cref="CriAtomExAcb.LoadAcbFile"/> 関数でACBファイルをロードする際には、
		/// 本関数が返すサイズ分のメモリをワーク領域として渡す必要があります。
		/// ワーク領域サイズの計算に失敗すると、本関数は -1 を返します。
		/// ワーク領域サイズの計算に失敗した理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// ワーク領域のサイズはライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数実行時）
		/// に指定したパラメーターによって変化します。
		/// そのため、本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数は、関数実行開始時に criFsLoader_Create 関数でローダーを確保し、
		/// 終了時に criFsLoader_Destroy 関数でローダーを破棄します。
		/// 本関数を実行する際には、空きローダーオブジェクトが１つ以上ある状態になるよう、
		/// ローダー数を調整してください。
		/// 本関数は完了復帰型の関数です。
		/// ACBファイルのロードにかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ACBファイルのロードは、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.LoadAcbFile"/>
		public static Int32 CalculateWorkSizeForLoadAcbFile(CriFsBinder acbBinder, ArgString acbPath, CriFsBinder awbBinder, ArgString awbPath)
		{
			return NativeMethods.criAtomExAcb_CalculateWorkSizeForLoadAcbFile(acbBinder?.NativeHandle ?? default, acbPath.GetPointer(stackalloc byte[acbPath.BufferSize]), awbBinder?.NativeHandle ?? default, awbPath.GetPointer(stackalloc byte[awbPath.BufferSize]));
		}

		/// <summary>ACBファイルのロードに必要なワーク領域サイズの計算（CPKコンテンツID指定）</summary>
		/// <param name="acbBinder">ACBファイルを含むバインダーのオブジェクト</param>
		/// <param name="acbId">CPKファイル内のACBデータのID</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbId">CPKファイル内のAWBデータのID</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAcb.LoadAcbFileById"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// ファイルパスの代わりにCPKコンテンツIDを指定する点を除けば、
		/// <see cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbFile"/> 関数と機能は同じです。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbFile"/>
		/// <seealso cref="CriAtomExAcb.LoadAcbFileById"/>
		public static Int32 CalculateWorkSizeForLoadAcbFileById(CriFsBinder acbBinder, UInt16 acbId, CriFsBinder awbBinder, UInt16 awbId)
		{
			return NativeMethods.criAtomExAcb_CalculateWorkSizeForLoadAcbFileById(acbBinder?.NativeHandle ?? default, acbId, awbBinder?.NativeHandle ?? default, awbId);
		}

		/// <summary>ACBファイルのロード</summary>
		/// <param name="acbBinder">ACBファイルを含むバインダーのオブジェクト</param>
		/// <param name="acbPath">ACBファイルのパス</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbPath">AWBファイルのパス</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ACBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBファイルをロードし、キュー再生に必要な情報を取り込みます。
		/// ACBファイルのロードに必要なワーク領域のサイズは、
		/// <see cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbFile"/> 関数で計算します。
		/// 第3引数の awb_binder 、および第4引数の awb_path には、ストリーム再生用
		/// のAWBファイルを指定します。
		/// （オンメモリ再生のみのACBデータをロードする場合、 awb_binder および
		/// awb_path にセットした値は無視されます。）
		/// ACBファイルをロードすると、ACBデータにアクセスするためのACBオブジェクト
		/// （ <see cref="CriAtomExAcb"/> ）が返されます。
		/// AtomExプレーヤーに対し、 <see cref="CriAtomExPlayer.SetCueId"/> 関数でACBオブジェクト、および再生する
		/// キューのIDをセットすることで、ACBファイル内のキューを再生することが可能です。
		/// ACBファイルのロードに成功すると、本関数は戻り値として ACB オブジェクトを返します。
		/// リードエラー等によりACBファイルのロードに失敗した場合、本関数は戻り値として
		/// CRI_NULL を返します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ライブラリを初期化しておく必要があります。
		/// 本関数にワーク領域をセットした場合、セットした領域のメモリをACBオブジェクト破棄時
		/// までアプリケーション中で保持し続ける必要があります。
		/// （セット済みのワーク領域に値を書き込んだり、メモリ解放したりしてはいけません。）
		/// ACBオブジェクトは内部的にバインダー（ <see cref="CriFsBinder"/> ）とローダー（ CriFsLoaderHn ）を確保します。
		/// ACBファイルをロードする場合、ACBオブジェクト数分のバインダーとローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// 本関数は完了復帰型の関数です。
		/// ACBファイルのロードにかかる時間は、プラットフォームによって異なります。
		/// ゲームループ等の画面更新が必要なタイミングで本関数を実行するとミリ秒単位で
		/// 処理がブロックされ、フレーム落ちが発生する恐れがあります。
		/// ACBファイルのロードは、シーンの切り替わり等、負荷変動を許容できる
		/// タイミングで行うようお願いいたします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbFile"/>
		/// <seealso cref="CriAtomExAcb"/>
		/// <seealso cref="CriAtomExPlayer.SetCueId"/>
		public static CriAtomExAcb LoadAcbFile(CriFsBinder acbBinder, ArgString acbPath, CriFsBinder awbBinder, ArgString awbPath, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_LoadAcbFile(acbBinder?.NativeHandle ?? default, acbPath.GetPointer(stackalloc byte[acbPath.BufferSize]), awbBinder?.NativeHandle ?? default, awbPath.GetPointer(stackalloc byte[awbPath.BufferSize]), work, workSize)) == IntPtr.Zero) ? null : new CriAtomExAcb(handle);
		}

		/// <summary>ACBファイルのロード（CPKコンテンツID指定）</summary>
		/// <param name="acbBinder">ACBファイルを含むバインダーのオブジェクト</param>
		/// <param name="acbId">CPKファイル内のACBデータのID</param>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbId">CPKファイル内のAWBデータのID</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <returns>ACBオブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBファイルをロードし、キュー再生に必要な情報を取り込みます。
		/// ファイルパスの代わりにCPKコンテンツIDを指定する点を除けば、
		/// <see cref="CriAtomExAcb.LoadAcbFile"/> 関数と機能は同じです。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.LoadAcbFile"/>
		public static CriAtomExAcb LoadAcbFileById(CriFsBinder acbBinder, UInt16 acbId, CriFsBinder awbBinder, UInt16 awbId, IntPtr work = default, Int32 workSize = default)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_LoadAcbFileById(acbBinder?.NativeHandle ?? default, acbId, awbBinder?.NativeHandle ?? default, awbId, work, workSize)) == IntPtr.Zero) ? null : new CriAtomExAcb(handle);
		}

		/// <summary>ACBオブジェクトのリリース</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトを解放します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// ACBオブジェクト作成時に確保されたメモリ領域が解放されます。
		/// （ACBオブジェクト作成時にワーク領域を渡した場合、本関数実行後であれば
		/// ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数でACBオブジェクトを破棄する際には、
		/// 当該ACBオブジェクトを参照しているキューは全て停止されます。
		/// （本関数実行後に、ACBオブジェクトの作成に使用したワーク領域や、
		/// ACBデータが配置されていた領域が参照されることはありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、破棄しようとしているACBデータを参照している
		/// Atomプレーヤーの存在を検索する処理が動作します。
		/// そのため、本関数実行中に他スレッドでAtomプレーヤーの作成／破棄を行うと、
		/// アクセス違反やデッドロック等の重大な不具合を誘発する恐れがあります。
		/// 本関数実行時にAtomプレーヤーの作成／破棄を他スレッドで行う必要がある場合、
		/// Atomプレーヤーの作成／破棄を <see cref="CriAtomEx.Lock"/> 関数でロックしてから実行ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.LoadAcbData"/>
		/// <seealso cref="CriAtomExAcb.LoadAcbFile"/>
		public void Dispose()
		{
			if (NativeHandle.IsDestroyable)
				NativeMethods.criAtomExAcb_Release(NativeHandle);
		}
#pragma warning disable 1591
		/// <exclude />
		~CriAtomExAcb() => Dispose();
#pragma warning restore 1591

		/// <summary>ACBオブジェクトが即時解放可能かどうかのチェック</summary>
		/// <returns>ACBの状態（true = 即時解放可能、false = 再生中のプレーヤーあり）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトを即座に解放可能かどうかをチェックします。
		/// 本関数が false を返すタイミングで <see cref="CriAtomExAcb.Dispose"/> 関数を実行すると、
		/// ACBオブジェクトを参照しているプレーヤーに対する停止処理が行われます。
		/// （ストリーム再生用のACBオブジェクトの場合、ファイル読み込み完了を待つため、
		/// <see cref="CriAtomExAcb.Dispose"/> 関数内で長時間処理がブロックされる可能性があります。）
		/// </para>
		/// <para>
		/// 備考:
		/// ACBオブジェクトを再生していたプレーヤーを全て停止させた場合でも、
		/// ライブラリ内では当該ACBオブジェクトを参照しているボイスが存在する可能性があります。
		/// （ <see cref="CriAtomExPlayer.StopWithoutReleaseTime"/> 関数で停止処理を行った場合や、
		/// ボイスの奪い取りが発生した場合、プレーヤーからボイスは切り離されますが、
		/// その後もボイス側でファイルの読み込み完了待ちを行うケースがあります。）
		/// <see cref="CriAtomExAcb.Dispose"/> 関数内で処理がブロックされるのを避ける必要がある場合には、
		/// 本関数が true を返すまで、<see cref="CriAtomExAcb.Dispose"/> 関数を実行しないでください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、指定したACBデータを参照しているAtomプレーヤーの存在を
		/// 検索する処理が動作します。
		/// そのため、本関数実行中に他スレッドでAtomプレーヤーの作成／破棄を行うと、
		/// アクセス違反やデッドロック等の重大な不具合を誘発する恐れがあります。
		/// 本関数実行時にAtomプレーヤーの作成／破棄を他スレッドで行う必要がある場合、
		/// Atomプレーヤーの作成／破棄を <see cref="CriAtomEx.Lock"/> 関数でロックしてから実行ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.Dispose"/>
		public bool IsReadyToRelease()
		{
			return NativeMethods.criAtomExAcb_IsReadyToRelease(NativeHandle);
		}

		/// <summary>全てのACBオブジェクトをリリース</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ロード済みの全てのACBオブジェクトを解放します。
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// ACBオブジェクト作成時に確保されたメモリ領域が解放されます。
		/// （ACBオブジェクト作成時にワーク領域を渡した場合、本関数実行後であれば
		/// ワーク領域を解放可能です。）
		/// </para>
		/// <para>
		/// 備考:
		/// 本関数を実行すると、全てのキュー再生がその時点で停止します。
		/// （本関数実行後に、ACBオブジェクトの作成に使用したワーク領域や、
		/// ACBデータが配置されていた領域が参照されることはありません。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行すると、指定したACBデータを参照しているAtomプレーヤーの存在を
		/// 検索する処理が動作します。
		/// そのため、本関数実行中に他スレッドでAtomプレーヤーの作成／破棄を行うと、
		/// アクセス違反やデッドロック等の重大な不具合を誘発する恐れがあります。
		/// 本関数実行時にAtomプレーヤーの作成／破棄を他スレッドで行う必要がある場合、
		/// Atomプレーヤーの作成／破棄を <see cref="CriAtomEx.Lock"/> 関数でロックしてから実行ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.LoadAcbData"/>
		/// <seealso cref="CriAtomExAcb.LoadAcbFile"/>
		public static void ReleaseAll()
		{
			NativeMethods.criAtomExAcb_ReleaseAll();
		}

		/// <summary>ACBオブジェクトの列挙</summary>
		/// <param name="func">ACBオブジェクトコールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <returns>列挙されたACBオブジェクトの数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトを列挙します。
		/// 本関数を実行すると、第 1 引数（ func ）
		/// でセットされたコールバック関数がACBオブジェクトの数分だけ呼び出されます。
		/// コールバック関数には、ACBオブジェクトが引数として渡されます。
		/// </para>
		/// <para>
		/// 備考:
		/// 第 2 引数（ obj ）にセットした値は、コールバック関数の引数として渡されます。
		/// コールバック関数のその他の引数については、
		/// 別途 <see cref="CriAtomExAcb.HandleCbFunc"/> の説明をご参照ください。
		/// 戻り値は列挙されたACBオブジェクトの数（登録したコールバック関数が呼び出された回数）です。
		/// （初回コールバック時に列挙を中止した場合でも 1 が返されます。）
		/// ACBオブジェクトが存在しない場合、本関数は 0 を返します。
		/// また、引数が不正な場合等、エラーが発生した際には -1 を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// ACBオブジェクトをコールバック関数内で破棄してはいけません。
		/// 全てのACBオブジェクトを一括で破棄する場合には、本関数の代わりに、
		/// <see cref="CriAtomExAcb.ReleaseAll"/> 関数を使用してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.HandleCbFunc"/>
		/// <seealso cref="CriAtomExAcb.ReleaseAll"/>
		public static unsafe Int32 EnumerateHandles(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, NativeBool> func, IntPtr obj)
		{
			return NativeMethods.criAtomExAcb_EnumerateHandles((IntPtr)func, obj);
		}

		/// <summary>ACBオブジェクトコールバック関数型</summary>
		/// <returns>列挙を続けるかどうか（true：継続、false：中止）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトの通知に使用される、コールバック関数の型です。
		/// <see cref="CriAtomExAcb.EnumerateHandles"/> 関数に本関数型のコールバック関数を登録することで、
		/// ACBオブジェクトをコールバック経由で受け取ることが可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// ACBオブジェクトをコールバック関数内で破棄してはいけません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.EnumerateHandles"/>
		/// <seealso cref="CriAtomExAcb"/>
		public unsafe class HandleCbFunc : NativeCallbackBase<HandleCbFunc.Arg, NativeBool>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ACBオブジェクト</summary>
				public IntPtr acbHn { get; }

				internal Arg(IntPtr acbHn)
				{
					this.acbHn = acbHn;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static NativeBool CallbackFunc(IntPtr obj, IntPtr acbHn) =>
				InvokeCallbackInternal(obj, new(acbHn));
#if !NET5_0_OR_GREATER
			delegate NativeBool NativeDelegate(IntPtr obj, IntPtr acbHn);
			static NativeDelegate callbackDelegate = null;
#endif
			internal HandleCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, IntPtr, NativeBool>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>オンメモリACBのバージョン取得</summary>
		/// <param name="acbData">ACBデータアドレス</param>
		/// <param name="acbDataSize">ACBデータサイズ</param>
		/// <param name="flag">ロード可能フラグ</param>
		/// <returns>ACBフォーマットバージョン</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// メモリ上に配置されたACBデータのフォーマットバージョンを取得します。
		/// また、flag引数にロード可能なバージョンかどうかをBool値で返します。
		/// </para>
		/// </remarks>
		public static UInt32 GetVersion(IntPtr acbData, Int32 acbDataSize, IntPtr flag)
		{
			return NativeMethods.criAtomExAcb_GetVersion(acbData, acbDataSize, flag);
		}

		/// <summary>ACBファイルのバージョン取得</summary>
		/// <param name="acbBinder">ACBファイルを含むバインダーのオブジェクト</param>
		/// <param name="acbPath">ACBファイルのパス</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <param name="flag">ロード可能フラグ</param>
		/// <returns>ACBフォーマットバージョン</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBファイルをメモリにロードしACBデータのフォーマットバージョンを取得します。
		/// ACB情報の登録に必要なワーク領域のサイズは、
		/// <see cref="CriAtomExAcb.CalculateWorkSizeForLoadAcbFile"/> 関数で計算します。
		/// ACBファイルフォーマットバージョンを元にflag引数にロード可能なバージョンかどうかをBool値で返します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数にセットしたワーク領域は、 アプリケーションで保持する必要はありません。
		/// （メモリにロードしたデータは関数終了時に解放されます。）
		/// 本関数は、関数実行開始時に criFsLoader_Create 関数でローダーを確保し、
		/// 終了時に criFsLoader_Destroy 関数でローダーを破棄します。
		/// 本関数を実行する際には、空きローダーオブジェクトが１つ以上ある状態になるよう、
		/// ローダー数を調整してください。
		/// </para>
		/// </remarks>
		public static unsafe UInt32 GetVersionFromFile(CriFsBinder acbBinder, ArgString acbPath, out NativeBool flag, IntPtr work = default, Int32 workSize = default)
		{
			fixed (NativeBool* flagPtr = &flag)
				return NativeMethods.criAtomExAcb_GetVersionFromFile(acbBinder?.NativeHandle ?? default, acbPath.GetPointer(stackalloc byte[acbPath.BufferSize]), work, workSize, flagPtr);
		}

		/// <summary>ロード可能バージョン情報取得</summary>
		/// <param name="versionLow">ロード可能下位バージョン</param>
		/// <param name="versionHigh">ロード可能上位バージョン</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ロード可能なACBのバージョン情報を取得します。
		/// 上位バージョンはライブラリビルド時点での情報のため、この値より上位のACBでも
		/// ロード可能な場合もあります。
		/// </para>
		/// </remarks>
		public static unsafe void GetSupportedVersion(out UInt32 versionLow, out UInt32 versionHigh)
		{
			fixed (UInt32* versionLowPtr = &versionLow)
			fixed (UInt32* versionHighPtr = &versionHigh)
				NativeMethods.criAtomExAcb_GetSupportedVersion(versionLowPtr, versionHighPtr);
		}

		/// <summary>キュー数の取得</summary>
		/// <returns>キュー数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBデータに含まれるキュー数を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数（ acb_hn ）に null を指定した場合、最後にロードしたACBデータを処理対象とします。
		/// </para>
		/// </remarks>
		public Int32 GetNumCues()
		{
			return NativeMethods.criAtomExAcb_GetNumCues(NativeHandle);
		}

		/// <summary>キューの存在確認（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <returns>キューが存在するかどうか（存在する：true／存在しない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したIDのキューが存在するかどうかを取得します。
		/// 存在した場合にはtrueを返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータが検索対象となります。
		/// （指定したIDを持つACBデータが1つでも存在すれば、本関数は true を返します。）
		/// </para>
		/// </remarks>
		public bool ExistsId(Int32 id)
		{
			return NativeMethods.criAtomExAcb_ExistsId(NativeHandle, id);
		}

		/// <summary>キューの存在確認（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>キューが存在するかどうか（存在する：true／存在しない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定した名前のキューが存在するかどうかを取得します。
		/// 存在した場合にはtrueを返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータが検索対象となります。
		/// （指定したキュー名を持つACBデータが1つでも存在すれば、本関数は true を返します。）
		/// </para>
		/// </remarks>
		public bool ExistsName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_ExistsName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>キューの存在確認（キューインデックス指定）</summary>
		/// <param name="index">キューインデックス</param>
		/// <returns>キューが存在するかどうか（存在する：true／存在しない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定したインデックスのキューが存在するかどうかを取得します。
		/// 存在した場合にはtrueを返します。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータが検索対象となります。
		/// （指定したキューインデックスを持つACBデータが1つでも存在すれば、本関数は true を返します。）
		/// </para>
		/// </remarks>
		public bool ExistsIndex(Int32 index)
		{
			return NativeMethods.criAtomExAcb_ExistsIndex(NativeHandle, index);
		}

		/// <summary>キューIDの取得（キューインデックス指定）</summary>
		/// <param name="index">キューインデックス</param>
		/// <returns>キューID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューインデックスからキューIDを取得します。
		/// 指定したキューインデックスのキューが存在しない場合、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューインデックスに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューインデックスを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのIDが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int32 GetCueIdByIndex(Int32 index)
		{
			return NativeMethods.criAtomExAcb_GetCueIdByIndex(NativeHandle, index);
		}

		/// <summary>キューIDの取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>キューID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名からキューIDを取得します。
		/// 指定したキュー名のキューが存在しない場合、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのIDが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int32 GetCueIdByName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_GetCueIdByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>キュー名の取得（キューインデックス指定）</summary>
		/// <param name="index">キューインデックス</param>
		/// <returns>CriChar8* キュー名</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューインデックスからキュー名を取得します。
		/// 指定したキューインデックスのキューが存在しない場合、nullが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューインデックスに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューインデックスを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの名前が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public NativeString GetCueNameByIndex(Int32 index)
		{
			return NativeMethods.criAtomExAcb_GetCueNameByIndex(NativeHandle, index);
		}

		/// <summary>キュー名の取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <returns>CriChar8* キュー名</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDからキュー名を取得します。
		/// 指定したキューIDのキューが存在しない場合、nullが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの名前が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public NativeString GetCueNameById(Int32 id)
		{
			return NativeMethods.criAtomExAcb_GetCueNameById(NativeHandle, id);
		}

		/// <summary>キューインデックスの取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <returns>キューインデックス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDからキューインデックスを取得します。
		/// 指定したキューIDのキューが存在しない場合、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのインデックスが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int32 GetCueIndexById(Int32 id)
		{
			return NativeMethods.criAtomExAcb_GetCueIndexById(NativeHandle, id);
		}

		/// <summary>キューインデックスの取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>キューインデックス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名からキューインデックスを取得します。
		/// 指定したキュー名のキューが存在しない場合、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのインデックスが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int32 GetCueIndexByName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_GetCueIndexByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>ユーザデータ文字列の取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <returns>CriChar8 *	ユーザデータ文字列</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、キューのユーザデータ文字列を取得します。
		/// 指定したキューIDのキューが存在しない場合、nullが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのユーザデータが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public NativeString GetUserDataById(Int32 id)
		{
			return NativeMethods.criAtomExAcb_GetUserDataById(NativeHandle, id);
		}

		/// <summary>ユーザデータ文字列の取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>CriChar8 * ユーザデータ文字列</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キューのユーザデータ文字列を取得します。
		/// 指定したキュー名のキューが存在しない場合、nullが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのユーザデータが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public NativeString GetUserDataByName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_GetUserDataByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>キューの長さの取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <returns>キューの長さ（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、キューの長さを取得します。キューの長さはミリ秒単位です。
		/// 指定したキューIDのキューが存在しない場合や、無限ループするキューの場合、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの長さが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int64 GetLengthById(Int32 id)
		{
			return NativeMethods.criAtomExAcb_GetLengthById(NativeHandle, id);
		}

		/// <summary>キューの長さの取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>キューの長さ（ミリ秒単位）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キューの長さを取得します。キューの長さはミリ秒単位です。
		/// 指定したキュー名のキューが存在しない場合や、無限ループするキューの場合、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの長さが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int64 GetLengthByName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_GetLengthByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>キューでコントロール可能なAISAC Controlの個数の取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <returns>AISAC Controlの個数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、キューでコントロール可能なAISAC Controlの個数を取得します。
		/// 指定したキューIDのキューが存在しない場合は、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのコントロール可能なAISAC Controlの個数が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetNumUsableAisacControlsByName"/>
		/// <seealso cref="CriAtomExAcb.GetUsableAisacControlById"/>
		/// <seealso cref="CriAtomExAcb.GetUsableAisacControlByName"/>
		public Int32 GetNumUsableAisacControlsById(Int32 id)
		{
			return NativeMethods.criAtomExAcb_GetNumUsableAisacControlsById(NativeHandle, id);
		}

		/// <summary>キューでコントロール可能なAISAC Controlの個数の取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>AISAC Controlの個数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キューでコントロール可能なAISAC Controlの個数を取得します。
		/// 指定したキュー名のキューが存在しない場合は、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのコントロール可能なAISAC Controlの個数が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetNumUsableAisacControlsById"/>
		/// <seealso cref="CriAtomExAcb.GetUsableAisacControlById"/>
		/// <seealso cref="CriAtomExAcb.GetUsableAisacControlByName"/>
		public Int32 GetNumUsableAisacControlsByName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_GetNumUsableAisacControlsByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>キューでコントロール可能なAISAC Controlの取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <param name="index">AISAC Controlインデックス</param>
		/// <param name="info">AISAC Control情報</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDとAISAC Controlインデックスを指定して、AISAC Control情報を取得します。
		/// 指定したキューIDのキューが存在しない場合は、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのAISAC Control情報が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetNumUsableAisacControlsById"/>
		/// <seealso cref="CriAtomExAcb.GetNumUsableAisacControlsByName"/>
		/// <seealso cref="CriAtomExAcb.GetUsableAisacControlByName"/>
		public unsafe bool GetUsableAisacControlById(Int32 id, UInt16 index, out CriAtomEx.AisacControlInfo info)
		{
			fixed (CriAtomEx.AisacControlInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcb_GetUsableAisacControlById(NativeHandle, id, index, infoPtr);
		}

		/// <summary>キューでコントロール可能なAISAC Controlの取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <param name="index">AISAC Controlインデックス</param>
		/// <param name="info">AISAC Control情報</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名とAISAC Controlインデックスを指定して、AISAC Control情報を取得します。
		/// 指定したキュー名のキューが存在しない場合は、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのAISAC Control情報が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetNumUsableAisacControlsById"/>
		/// <seealso cref="CriAtomExAcb.GetNumUsableAisacControlsByName"/>
		/// <seealso cref="CriAtomExAcb.GetUsableAisacControlById"/>
		public unsafe bool GetUsableAisacControlByName(ArgString name, UInt16 index, out CriAtomEx.AisacControlInfo info)
		{
			fixed (CriAtomEx.AisacControlInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcb_GetUsableAisacControlByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]), index, infoPtr);
		}

		/// <summary>キューがAISAC Controlでコントロール可能かどうかの取得（ID指定）</summary>
		/// <param name="id">キューID</param>
		/// <param name="aisacControlId">AISAC Control id</param>
		/// <returns>コントロール可能かどうか（可能：true、不可能：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDとAISAC Control Idを指定して、キューがAISAC Controlでコントロール可能かどうかを取得します。
		/// 指定したキューIDのキューが存在しない場合は、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのAISAC Control情報を元に値を返します）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.IsUsingAisacControlByName"/>
		public bool IsUsingAisacControlById(Int32 id, UInt32 aisacControlId)
		{
			return NativeMethods.criAtomExAcb_IsUsingAisacControlById(NativeHandle, id, aisacControlId);
		}

		/// <summary>キューがAISAC Controlでコントロール可能かどうかの取得（名前指定）</summary>
		/// <param name="name">キュー名</param>
		/// <param name="aisacControlName">AISAC Controlインデックス</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名とAISAC Controlインデックスを指定して、AISAC Control情報を取得します。
		/// 指定したキュー名のキューが存在しない場合は、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのAISAC Control情報を元に値を返します）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.IsUsingAisacControlById"/>
		public bool IsUsingAisacControlByName(ArgString name, ArgString aisacControlName)
		{
			return NativeMethods.criAtomExAcb_IsUsingAisacControlByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]), aisacControlName.GetPointer(stackalloc byte[aisacControlName.BufferSize]));
		}

		/// <summary>キューに設定されているプライオリティの取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <returns>プライオリティ（取得に失敗した場合-1が帰ります）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、キューに設定されているプライオリティを取得します。
		/// 指定したキューIDのキューが存在しない場合は、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのプライオリティが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetCuePriorityByName"/>
		public Int32 GetCuePriorityById(Int32 id)
		{
			return NativeMethods.criAtomExAcb_GetCuePriorityById(NativeHandle, id);
		}

		/// <summary>キューに設定されているプライオリティの取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>プライオリティ（取得に失敗した場合-1が帰ります）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キューに設定されているプライオリティを取得します。
		/// 指定したキュー名のキューが存在しない場合は、-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューのプライオリティが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetCuePriorityById"/>
		public Int32 GetCuePriorityByName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_GetCuePriorityByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>音声波形情報の取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <param name="waveformInfo">音声波形情報</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、そのキューで再生される音声波形の情報を取得します。
		/// そのキューで再生される音声波形が複数ある場合、初めのトラックで最初に再生される音声波形の情報が取得されます。
		/// 指定したキューIDのキューが存在しない場合、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの音声波形情報が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public unsafe bool GetWaveformInfoById(Int32 id, out CriAtomEx.WaveformInfo waveformInfo)
		{
			fixed (CriAtomEx.WaveformInfo* waveformInfoPtr = &waveformInfo)
				return NativeMethods.criAtomExAcb_GetWaveformInfoById(NativeHandle, id, waveformInfoPtr);
		}

		/// <summary>音声波形情報の取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <param name="waveformInfo">音声波形情報</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、そのキューで再生される音声波形の情報を取得します。
		/// そのキューで再生される音声波形が複数ある場合、初めのトラックで最初に再生される音声波形の情報が取得されます。
		/// 指定したキュー名のキューが存在しない場合、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの音声波形情報が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public unsafe bool GetWaveformInfoByName(ArgString name, out CriAtomEx.WaveformInfo waveformInfo)
		{
			fixed (CriAtomEx.WaveformInfo* waveformInfoPtr = &waveformInfo)
				return NativeMethods.criAtomExAcb_GetWaveformInfoByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]), waveformInfoPtr);
		}

		/// <summary>オンメモリ再生用 AWB オブジェクトの取得</summary>
		/// <returns>AWB オブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACB データからオンメモリ再生用の AWB オブジェクトを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ACB データ内には、オンメモリ再生用の波形データが AWB フォーマットで格納されています。
		/// ACB オブジェクトを作成する際、 Atom ライブラリはオンメモリ再生用に
		/// AWB データを読み込み、再生用のオブジェクト（ AWB オブジェクト）を作成します。
		/// 本関数を使用することで、 Atom ライブラリが内部的に作成した AWB オブジェクトを
		/// 取得することが可能です。
		/// 取得した AWB オブジェクトを使用することで、 ACB オブジェクト内のオンメモリ波形データを、
		/// アプリケーション側から <see cref="CriAtomExPlayer.SetWaveId"/>
		/// 関数を使用して再生することが可能になります。
		/// （キューに含まれる波形データをシームレス連結再生する際や、
		/// デバッグ用途で ACB データ内に含まれるオンメモリ波形データを再生する、
		/// といった用途に利用可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// ACB オブジェクトが保持する AWB オブジェクトは、 ACB オブジェクトリリース時に破棄されます。
		/// 本関数で取得した AWB オブジェクトを個別に破棄したり、
		/// 取得済みの AWB オブジェクトに ACB オブジェクトリリース後にアクセスしたりすると、
		/// アクセス違反等の重大な不具合が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetStreamingAwbHandle"/>
		public CriAtomAwb GetOnMemoryAwbHandle()
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_GetOnMemoryAwbHandle(NativeHandle)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>ストリーム再生用 AWB オブジェクトの取得</summary>
		/// <returns>AWB オブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACB データからストリーム再生用の AWB オブジェクトを取得します。
		/// なお、本関数ではスロットの先頭の AWB オブジェクトのみを取得可能です。
		/// 先頭以外のスロットにある AWB オブジェクトを取得する際は以下の関数を使用してください。
		/// - <see cref="CriAtomExAcb.GetStreamingAwbHandleBySlotName"/>
		/// - <see cref="CriAtomExAcb.GetStreamingAwbHandleBySlotIndex"/>
		/// </para>
		/// <para>
		/// 備考:
		/// ACB データ内には、ストリーム再生用の AWB ファイルが関連付けられています。
		/// ACB オブジェクトを作成する際、 Atom ライブラリはストリーム再生用に
		/// AWB データを読み込み、再生用のオブジェクト（ AWB オブジェクト）を作成します。
		/// 本関数を使用することで、 Atom ライブラリが内部的に作成した AWB オブジェクトを
		/// 取得することが可能です。
		/// 取得した AWB オブジェクトを使用することで、 ストリーム再生用の波形データを、
		/// アプリケーション側から <see cref="CriAtomExPlayer.SetWaveId"/>
		/// 関数を使用して再生することが可能になります。
		/// （キューに含まれる波形データをシームレス連結再生する際や、
		/// デバッグ用途で ACB データに関連付けられたストリーム再生用波形データを再生する、
		/// といった用途に利用可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// ACB オブジェクトが保持する AWB オブジェクトは、 ACB オブジェクトリリース時に破棄されます。
		/// 本関数で取得した AWB オブジェクトを個別に破棄したり、
		/// 取得済みの AWB オブジェクトに ACB オブジェクトリリース後にアクセスしたりすると、
		/// アクセス違反等の重大な不具合が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetOnMemoryAwbHandle"/>
		public CriAtomAwb GetStreamingAwbHandle()
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_GetStreamingAwbHandle(NativeHandle)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>指定した AWB スロット名のストリーム再生用 AWB オブジェクトの取得</summary>
		/// <param name="awbSlotName">AWB スロット名</param>
		/// <returns>AWB オブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACB データから指定した AWB スロット名のストリーム再生用の AWB オブジェクトを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ACB データ内には、ストリーム再生用の AWB ファイルが関連付けられています。
		/// ACB オブジェクトを作成する際、 Atom ライブラリはストリーム再生用に
		/// AWB データを読み込み、再生用のオブジェクト（ AWB オブジェクト）を作成します。
		/// 本関数を使用することで、 Atom ライブラリが内部的に作成した AWB オブジェクトを
		/// 取得することが可能です。
		/// 取得した AWB オブジェクトを使用することで、 ストリーム再生用の波形データを、
		/// アプリケーション側から <see cref="CriAtomExPlayer.SetWaveId"/>
		/// 関数を使用して再生することが可能になります。
		/// （キューに含まれる波形データをシームレス連結再生する際や、
		/// デバッグ用途で ACB データに関連付けられたストリーム再生用波形データを再生する、
		/// といった用途に利用可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// ACB オブジェクトが保持する AWB オブジェクトは、 ACB オブジェクトリリース時に破棄されます。
		/// 本関数で取得した AWB オブジェクトを個別に破棄したり、
		/// 取得済みの AWB オブジェクトに ACB オブジェクトリリース後にアクセスしたりすると、
		/// アクセス違反等の重大な不具合が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetOnMemoryAwbHandle"/>
		public CriAtomAwb GetStreamingAwbHandleBySlotName(ArgString awbSlotName)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_GetStreamingAwbHandleBySlotName(NativeHandle, awbSlotName.GetPointer(stackalloc byte[awbSlotName.BufferSize]))) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>指定した AWB スロットインデックスのストリーム再生用 AWB オブジェクトの取得</summary>
		/// <param name="awbSlotIndex">AWB スロットインデックス</param>
		/// <returns>AWB オブジェクト</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACB データから指定した AWB スロットインデックスのストリーム再生用の AWB オブジェクトを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ACB データ内には、ストリーム再生用の AWB ファイルが関連付けられています。
		/// ACB オブジェクトを作成する際、 Atom ライブラリはストリーム再生用に
		/// AWB データを読み込み、再生用のオブジェクト（ AWB オブジェクト）を作成します。
		/// 本関数を使用することで、 Atom ライブラリが内部的に作成した AWB オブジェクトを
		/// 取得することが可能です。
		/// 取得した AWB オブジェクトを使用することで、 ストリーム再生用の波形データを、
		/// アプリケーション側から <see cref="CriAtomExPlayer.SetWaveId"/>
		/// 関数を使用して再生することが可能になります。
		/// （キューに含まれる波形データをシームレス連結再生する際や、
		/// デバッグ用途で ACB データに関連付けられたストリーム再生用波形データを再生する、
		/// といった用途に利用可能です。）
		/// </para>
		/// <para>
		/// 注意:
		/// ACB オブジェクトが保持する AWB オブジェクトは、 ACB オブジェクトリリース時に破棄されます。
		/// 本関数で取得した AWB オブジェクトを個別に破棄したり、
		/// 取得済みの AWB オブジェクトに ACB オブジェクトリリース後にアクセスしたりすると、
		/// アクセス違反等の重大な不具合が発生する可能性があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetOnMemoryAwbHandle"/>
		public CriAtomAwb GetStreamingAwbHandleBySlotIndex(UInt16 awbSlotIndex)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcb_GetStreamingAwbHandleBySlotIndex(NativeHandle, awbSlotIndex)) == IntPtr.Zero) ? null : new CriAtomAwb(handle);
		}

		/// <summary>キュー情報の取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <param name="info">キュー情報</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キュー情報を取得します。
		/// 指定したキュー名のキューが存在しない場合、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの情報が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetCueInfoById"/>
		/// <seealso cref="CriAtomExAcb.GetCueInfoByIndex"/>
		public unsafe bool GetCueInfoByName(ArgString name, out CriAtomEx.CueInfo info)
		{
			fixed (CriAtomEx.CueInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcb_GetCueInfoByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]), infoPtr);
		}

		/// <summary>キュータイプ</summary>
		/// <seealso cref="CriAtomEx.CueInfo"/>
		public enum CueType
		{
			/// <summary>ポリフォニック</summary>
			Polyphonic = 0,
			/// <summary>シーケンシャル</summary>
			Sequential = 1,
			/// <summary>シャッフル再生</summary>
			Shuffle = 2,
			/// <summary>ランダム</summary>
			Random = 3,
			/// <summary>ランダム非連続（前回再生した音以外をランダムに鳴らす）</summary>
			RandomNoRepeat = 4,
			/// <summary>スイッチ再生（ゲーム変数を参照して再生トラックの切り替える）</summary>
			SwitchGameVariable = 5,
			/// <summary>コンボシーケンシャル（「コンボ時間」内に連続コンボが決まるとシーケンシャル、最後までいくと「コンボループバック」地点に戻る）</summary>
			ComboSequential = 6,
			/// <summary>スイッチ再生（セレクターを参照して再生トラックを切り替える）</summary>
			SwitchSelector = 7,
			/// <summary>トラックトランジション再生（セレクターを参照して再生トラックを切り替える）</summary>
			TrackTransitionBySelector = 8,
		}
		/// <summary>キュー情報の取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <param name="info">キュー情報</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、キュー情報を取得します。
		/// 指定したキューIDのキューが存在しない場合、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの情報が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetCueInfoByName"/>
		/// <seealso cref="CriAtomExAcb.GetCueInfoByIndex"/>
		public unsafe bool GetCueInfoById(Int32 id, out CriAtomEx.CueInfo info)
		{
			fixed (CriAtomEx.CueInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcb_GetCueInfoById(NativeHandle, id, infoPtr);
		}

		/// <summary>キュー情報の取得（キューインデックス指定）</summary>
		/// <param name="index">キューインデックス</param>
		/// <param name="info">キュー情報</param>
		/// <returns>取得に成功したかどうか（成功：true、失敗：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューインデックスを指定して、キュー情報を取得します。
		/// 指定したキューインデックスのキューが存在しない場合、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの情報が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetCueInfoByName"/>
		/// <seealso cref="CriAtomExAcb.GetCueInfoById"/>
		public unsafe bool GetCueInfoByIndex(Int32 index, out CriAtomEx.CueInfo info)
		{
			fixed (CriAtomEx.CueInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcb_GetCueInfoByIndex(NativeHandle, index, infoPtr);
		}

		/// <summary>キューリミットが設定されているキューの発音数の取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <returns>発音数（キューリミットが設定されていないキューを指定した場合-1が帰ります）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キューリミットが設定されているキューの発音数を取得します。
		/// 指定したキュー名のキューが存在しない場合や、キューリミットが設定されていないキューを指定した場合は-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの発音数が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetNumCuePlayingCountById"/>
		/// <seealso cref="CriAtomExAcb.GetNumCuePlayingCountByIndex"/>
		public Int32 GetNumCuePlayingCountByName(ArgString name)
		{
			return NativeMethods.criAtomExAcb_GetNumCuePlayingCountByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>キューリミットが設定されているキューの発音数の取得（キューID指定）</summary>
		/// <param name="id">キューID名</param>
		/// <returns>発音数（キューリミットが設定されていないキューを指定した場合-1が帰ります）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、キューリミットが設定されているキューの発音数を取得します。
		/// 指定したキューIDのキューが存在しない場合や、キューリミットが設定されていないキューを指定した場合は-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの発音数が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetNumCuePlayingCountByName"/>
		/// <seealso cref="CriAtomExAcb.GetNumCuePlayingCountByIndex"/>
		public Int32 GetNumCuePlayingCountById(Int32 id)
		{
			return NativeMethods.criAtomExAcb_GetNumCuePlayingCountById(NativeHandle, id);
		}

		/// <summary>キューリミットが設定されているキューの発音数の取得（キューインデックス指定）</summary>
		/// <param name="index">キューインデックス</param>
		/// <returns>発音数（キューリミットが設定されていないキューを指定した場合-1が帰ります）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キューリミットが設定されているキューの発音数を取得します。
		/// 指定したキューインデックスのキューが存在しない場合や、キューリミットが設定されていないキューを指定した場合は-1が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキューの発音数が返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetNumCuePlayingCountByName"/>
		/// <seealso cref="CriAtomExAcb.GetNumCuePlayingCountById"/>
		public Int32 GetNumCuePlayingCountByIndex(Int32 index)
		{
			return NativeMethods.criAtomExAcb_GetNumCuePlayingCountByIndex(NativeHandle, index);
		}

		/// <summary>ブロックインデックスの取得（キューインデックス指定）</summary>
		/// <param name="index">キューインデックス</param>
		/// <param name="blockName">ブロック名</param>
		/// <returns>ブロックインデックス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューインデックスとブロック名からブロックインデックスを取得します。
		/// 指定したキューインデックスのキューが存在しない場合やブロック名が存在しない場合は、
		/// <see cref="CriAtomEx.InvalidBlockIndex"/> が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューインデックスに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューインデックスを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキュー内のブロックインデックスが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int32 GetBlockIndexByIndex(Int32 index, ArgString blockName)
		{
			return NativeMethods.criAtomExAcb_GetBlockIndexByIndex(NativeHandle, index, blockName.GetPointer(stackalloc byte[blockName.BufferSize]));
		}

		/// <summary>ブロックインデックスの取得（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <param name="blockName">ブロック名</param>
		/// <returns>ブロックインデックス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDとブロック名からブロックインデックスを取得します。
		/// 指定したキューIDのキューが存在しない場合やブロック名が存在しない場合は、
		/// <see cref="CriAtomEx.InvalidBlockIndex"/> が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキューIDに
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキューIDを持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキュー内のブロックインデックスが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int32 GetBlockIndexById(Int32 id, ArgString blockName)
		{
			return NativeMethods.criAtomExAcb_GetBlockIndexById(NativeHandle, id, blockName.GetPointer(stackalloc byte[blockName.BufferSize]));
		}

		/// <summary>ブロックインデックスの取得（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <param name="blockName">ブロック名</param>
		/// <returns>ブロックインデックス</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名とブロック名からブロックインデックスを取得します。
		/// 指定したキュー名のキューが存在しない場合やブロック名が存在しない場合は、
		/// <see cref="CriAtomEx.InvalidBlockIndex"/> が返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// 第1引数（ ach_hn ）に null を指定した場合、全てのACBデータを対象に、指定したキュー名に
		/// 合致するデータの存在を検索する処理が動作します。
		/// （指定したキュー名を持つACBデータが見つかった時点で、
		/// 当該ACBデータ内のキュー内のブロックインデックスが返されます。）
		/// この際、検索の順序は、ACBデータのロード順とは逆順で行われます。
		/// （後からロードされたデータから優先的に検索が行われます。）
		/// </para>
		/// </remarks>
		public Int32 GetBlockIndexByName(ArgString name, ArgString blockName)
		{
			return NativeMethods.criAtomExAcb_GetBlockIndexByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]), blockName.GetPointer(stackalloc byte[blockName.BufferSize]));
		}

		/// <summary>インゲームプレビュー用データのロード検知コールバック関数の登録</summary>
		/// <param name="func">ロード検知コールバック関数</param>
		/// <param name="obj">ユーザ指定オブジェクト</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インゲームプレビュー用データのロードを検知した場合に呼び出すコールバック関数を登録します。
		/// 登録されたコールバック関数は、ACBロード関数内でACBの内容解析を行ったタイミングで実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// コールバック関数は1つしか登録できません。
		/// 登録操作を複数回行った場合、既に登録済みのコールバック関数が、
		/// 後から登録したコールバック関数により上書きされてしまいます。
		/// funcにnullを指定することで登録済み関数の登録解除が行えます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.DetectionInGamePreviewDataCbFunc"/>
		public static unsafe void SetDetectionInGamePreviewDataCallback(delegate* unmanaged[Cdecl]<IntPtr, NativeString, void> func, IntPtr obj)
		{
			NativeMethods.criAtomExAcb_SetDetectionInGamePreviewDataCallback((IntPtr)func, obj);
		}
		static unsafe void SetDetectionInGamePreviewDataCallbackInternal(IntPtr func, IntPtr obj) => SetDetectionInGamePreviewDataCallback((delegate* unmanaged[Cdecl]<IntPtr, NativeString, void>)func, obj);
		static CriAtomExAcb.DetectionInGamePreviewDataCbFunc _detectionInGamePreviewDataCallback = null;
		/// <summary>コールバックイベントオブジェクト</summary>
		/// <seealso cref="SetDetectionInGamePreviewDataCallback" />
		public static CriAtomExAcb.DetectionInGamePreviewDataCbFunc DetectionInGamePreviewDataCallback => _detectionInGamePreviewDataCallback ?? (_detectionInGamePreviewDataCallback = new CriAtomExAcb.DetectionInGamePreviewDataCbFunc(SetDetectionInGamePreviewDataCallbackInternal));

		/// <summary>インゲームプレビュー用データのロード検知コールバック関数</summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>説明:</para>
		/// <para>
		/// 説明:
		/// インゲームプレビュー用データのロードを検知した場合に呼び出すコールバック関数です。
		/// インゲームプレビュー用データを使用しているか調査する際に使用します。
		/// コールバック関数の登録には <see cref="CriAtomExAcb.SetDetectionInGamePreviewDataCallback"/> 関数を使用します。
		/// 登録したコールバック関数は、ACBロード関数内でACBの内容解析を行ったタイミングで実行されます。
		/// </para>
		/// <para>
		/// 注意:
		/// 基本的に、コールバック関数内ではAtomライブラリAPIを使用しないでください。
		/// 本コールバック関数内で長時間処理をブロックすると、音切れ等の問題が発生しますので、
		/// ご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.SetDetectionInGamePreviewDataCallback"/>
		public unsafe class DetectionInGamePreviewDataCbFunc : NativeCallbackBase<DetectionInGamePreviewDataCbFunc.Arg>
		{
			/// <summary>コールバックイベント引数型</summary>
			public struct Arg
			{
				/// <summary>ACB名</summary>
				public NativeString acbName { get; }

				internal Arg(NativeString acbName)
				{
					this.acbName = acbName;
				}
			}

#if ENABLE_IL2CPP
	[AOT.MonoPInvokeCallback(typeof(NativeDelegate))]
#endif
#if NET5_0_OR_GREATER
	[UnmanagedCallersOnly(CallConvs = new System.Type[]{typeof(CallConvCdecl)})]
#endif
			static void CallbackFunc(IntPtr obj, NativeString acbName) =>
				InvokeCallbackInternal(obj, new(acbName));
#if !NET5_0_OR_GREATER
			delegate void NativeDelegate(IntPtr obj, NativeString acbName);
			static NativeDelegate callbackDelegate = null;
#endif
			internal DetectionInGamePreviewDataCbFunc(Action<IntPtr, IntPtr> setFunction) :
				base(setFunction,
#if NET5_0_OR_GREATER
			(IntPtr)(delegate*unmanaged[Cdecl]<IntPtr, NativeString, void>)&CallbackFunc
#else
					Marshal.GetFunctionPointerForDelegate<NativeDelegate>(callbackDelegate = CallbackFunc)
#endif
				)
			{ }
		}
		/// <summary>ACB情報の取得</summary>
		/// <param name="acbInfo">ACB情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBデータの各種情報を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// 引数（ acb_hn ）に null を指定した場合、最後にロードしたACBデータを処理対象とします。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.Info"/>
		public unsafe bool GetAcbInfo(out CriAtomExAcb.Info acbInfo)
		{
			fixed (CriAtomExAcb.Info* acbInfoPtr = &acbInfo)
				return NativeMethods.criAtomExAcb_GetAcbInfo(NativeHandle, acbInfoPtr);
		}

		/// <summary>ACB情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBデータの各種情報です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.GetAcbInfo"/>
		public unsafe partial struct Info
		{
			/// <summary>名前</summary>
			public NativeString name;

			/// <summary>サイズ</summary>
			public UInt32 size;

			/// <summary>ACBバージョン</summary>
			public UInt32 version;

			/// <summary>文字コード</summary>
			public CriAtomEx.CharacterEncoding characterEncoding;

			/// <summary>キューシートボリューム</summary>
			public Single volume;

			/// <summary>キュー数</summary>
			public Int32 numCues;

		}
		/// <summary>キュータイプステートのリセット（キュー名指定）</summary>
		/// <param name="name">キュー名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キュー名を指定して、キュータイプステートをリセットします。
		/// </para>
		/// <para>
		/// 注意:
		/// リセット対象は指定したキューのステートのみです。キューに含まれるサブシンセやキューリンク先の
		/// ステートはリセットされません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュータイプステートは、ポリフォニックタイプキュー以外のキュー再生時の前回再生トラックを
		/// ステートとして管理する仕組みです。
		/// 本関数は、ステート管理領域をリセットしACBロード直後の状態に戻します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.ResetCueTypeStateById"/>
		/// <seealso cref="CriAtomExAcb.ResetCueTypeStateByIndex"/>
		public void ResetCueTypeStateByName(ArgString name)
		{
			NativeMethods.criAtomExAcb_ResetCueTypeStateByName(NativeHandle, name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>キュータイプステートのリセット（キューID指定）</summary>
		/// <param name="id">キューID</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューIDを指定して、キュータイプステートをリセットします。
		/// </para>
		/// <para>
		/// 注意:
		/// リセット対象は指定したキューのステートのみです。キューに含まれるサブシンセやキューリンク先の
		/// ステートはリセットされません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュータイプステートは、ポリフォニックタイプキュー以外のキュー再生時の前回再生トラックを
		/// ステートとして管理する仕組みです。
		/// 本関数は、ステート管理領域をリセットしACBロード直後の状態に戻します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.ResetCueTypeStateByName"/>
		/// <seealso cref="CriAtomExAcb.ResetCueTypeStateByIndex"/>
		public void ResetCueTypeStateById(Int32 id)
		{
			NativeMethods.criAtomExAcb_ResetCueTypeStateById(NativeHandle, id);
		}

		/// <summary>キュータイプステートのリセット（キューインデックス指定）</summary>
		/// <param name="index">キューインデックス</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// キューインデックスを指定して、キュータイプステートをリセットします。
		/// </para>
		/// <para>
		/// 注意:
		/// リセット対象は指定したキューのステートのみです。キューに含まれるサブシンセやキューリンク先の
		/// ステートはリセットされません。
		/// </para>
		/// <para>
		/// 備考:
		/// キュータイプステートは、ポリフォニックタイプキュー以外のキュー再生時の前回再生トラックを
		/// ステートとして管理する仕組みです。
		/// 本関数は、ステート管理領域をリセットしACBロード直後の状態に戻します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.ResetCueTypeStateByName"/>
		/// <seealso cref="CriAtomExAcb.ResetCueTypeStateById"/>
		public void ResetCueTypeStateByIndex(Int32 index)
		{
			NativeMethods.criAtomExAcb_ResetCueTypeStateByIndex(NativeHandle, index);
		}

		/// <summary>ストリーム用AWBファイルのアタッチ</summary>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbPath">AWBファイルのパス</param>
		/// <param name="awbName">AWB名</param>
		/// <param name="work">アタッチで必要な追加ワーク</param>
		/// <param name="workSize">追加ワークサイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトに対してストリーム用のAWBファイルをアタッチします。
		/// 第2引数の awb_binder 、および第3引数の awb_path には、ストリーム再生用
		/// のAWBファイルを指定します。
		/// 第5引数の awb_name はAWBをアタッチするスロットを指定するために使用します。
		/// このため、AtomCraftが出力したAWB名（ファイル名から拡張子を取り除いた部分）を変更している場合
		/// はオリジナルのAWB名を指定してください。
		/// AWBファイルのアタッチを行うには、ライブラリが内部で利用するためのメモリ領域
		/// （ワーク領域）を確保する必要があります。
		/// AWBファイルのアタッチに失敗した場合、エラーコールバックが発生します。
		/// 失敗の理由については、エラーコールバックのメッセージで確認可能です。
		/// </para>
		/// <para>
		/// 備考:
		/// ワーク領域を確保する方法には、以下の2通りの方法があります。
		/// <b>(a) User Allocator方式</b>：メモリの確保／解放に、ユーザが用意した関数を使用する方法。
		/// <b>(b) Fixed Memory方式</b>：必要なメモリ領域を直接ライブラリに渡す方法。
		/// User Allocator方式を用いる場合、ユーザがワーク領域を用意する必要はありません。
		/// workにnull、work_sizeに0を指定するだけで、必要なメモリを登録済みのメモリ確保関数から確保します。
		/// アタッチ時に確保されたメモリは、デタッチ時（ <see cref="CriAtomExAcb.DetachAwbFile"/>
		/// 関数実行時）か、ACBオブジェクトリリース時（ <see cref="CriAtomExAcb.Dispose"/> 関数実行時）に解放されます。
		/// Fixed Memory方式を用いる場合、ワーク領域として別途確保済みのメモリ領域を本関数に
		/// 設定する必要があります。
		/// ワーク領域のサイズは <see cref="CriAtomExAcb.CalculateWorkSizeForAttachAwbFile"/> 関数で取得可能です。
		/// 本関数呼び出し時に <see cref="CriAtomExAcb.CalculateWorkSizeForAttachAwbFile"/> 関数で取得した
		/// サイズ分のメモリを予め確保しておき、本関数に設定してください。
		/// 尚、Fixed Memory方式を用いた場合、ワーク領域はデタッチ処理（ <see cref="CriAtomExAcb.DetachAwbFile"/>
		/// 関数実行時）か、ACBオブジェクトリリース処理（ <see cref="CriAtomExAcb.Dispose"/> 関数実行時）を行うまでの間、
		/// ライブラリ内で利用され続けます。
		/// AWBファイルをアタッチするとライブラリ内部的にバインダー（ <see cref="CriFsBinder"/> ）とローダー（ CriFsLoaderHn ）
		/// を確保します。
		/// 追加でAWBファイルをアタッチする場合、追加数分のバインダーとローダーが確保できる設定で
		/// Atomライブラリ（またはCRI File Systemライブラリ）を初期化する必要があります。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.DetachAwbFile"/>
		/// <seealso cref="CriAtomExAcb.Dispose"/>
		/// <seealso cref="CriAtomExAcb.CalculateWorkSizeForAttachAwbFile"/>
		public void AttachAwbFile(CriFsBinder awbBinder, ArgString awbPath, ArgString awbName, IntPtr work = default, Int32 workSize = default)
		{
			NativeMethods.criAtomExAcb_AttachAwbFile(NativeHandle, awbBinder?.NativeHandle ?? default, awbPath.GetPointer(stackalloc byte[awbPath.BufferSize]), awbName.GetPointer(stackalloc byte[awbName.BufferSize]), work, workSize);
		}

		/// <summary>ストリーム用AWBファイルのデタッチ</summary>
		/// <param name="awbName">AWB名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトにアタッチされているストリーム用のAWBファイルをデタッチします。
		/// 第2引数の awb_name はAWBをアタッチ時に指定したものと同じAWB名を指定指定ください。
		/// アタッチ時のワーク領域確保にUser Allocator方式を用いた場合は、アタッチ時に確保したメモリ領域が
		/// 本関数処理時に開放されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.AttachAwbFile"/>
		public void DetachAwbFile(ArgString awbName)
		{
			NativeMethods.criAtomExAcb_DetachAwbFile(NativeHandle, awbName.GetPointer(stackalloc byte[awbName.BufferSize]));
		}

		/// <summary>ストリーム用AWBファイルのアタッチに必要なワークサイズ取得</summary>
		/// <param name="awbBinder">AWBファイルを含むバインダーのオブジェクト</param>
		/// <param name="awbPath">AWBファイルのパス</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <returns>正常に処理が完了</returns>
		/// <returns>エラーが発生</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// <see cref="CriAtomExAcb.LoadAcbFileById"/> 関数の実行に必要なワーク領域サイズを計算します。
		/// </para>
		/// <para>
		/// 備考：
		/// データがCPKにパックされていない場合、引数binderにはnullを指定してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcb.AttachAwbFile"/>
		public static Int32 CalculateWorkSizeForAttachAwbFile(CriFsBinder awbBinder, ArgString awbPath)
		{
			return NativeMethods.criAtomExAcb_CalculateWorkSizeForAttachAwbFile(awbBinder?.NativeHandle ?? default, awbPath.GetPointer(stackalloc byte[awbPath.BufferSize]));
		}

		/// <summary>ストリーム用AWBスロット数の取得</summary>
		/// <returns>ストリームAWBスロット数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトが必要とするストリームAWBの数を取得します。
		/// </para>
		/// </remarks>
		public Int32 GetNumAwbFileSlots()
		{
			return NativeMethods.criAtomExAcb_GetNumAwbFileSlots(NativeHandle);
		}

		/// <summary>ストリーム用AWBスロットの取得</summary>
		/// <param name="index">スロットインデックス</param>
		/// <returns>CriChar8*	ストリームAWBポート名</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インデックスを指定してACBオブジェクト内のストリームAWBスロット名を取得します。
		/// 取得したスロット名は <see cref="CriAtomExAcb.AttachAwbFile"/> 関数の第4引数や、
		/// <see cref="CriAtomExAcb.DetachAwbFile"/> 関数の第2引数のスロット指定に使用します。
		/// </para>
		/// </remarks>
		public NativeString GetAwbFileSlotName(UInt16 index)
		{
			return NativeMethods.criAtomExAcb_GetAwbFileSlotName(NativeHandle, index);
		}

		/// <summary>ストリーム用AWBファイルのアタッチ状態取得</summary>
		/// <param name="awbName">AWB名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACBオブジェクトにAWBファイルがアタッチされているかを取得します。
		/// 第2引数の awb_name はAWBをアタッチするスロット名です。状態を取得したいスロットのAWB名を指定してください。
		/// </para>
		/// </remarks>
		public bool IsAttachedAwbFile(ArgString awbName)
		{
			return NativeMethods.criAtomExAcb_IsAttachedAwbFile(NativeHandle, awbName.GetPointer(stackalloc byte[awbName.BufferSize]));
		}

		/// <summary>ネイティブハンドル</summary>

		public NativeHandleIntPtr NativeHandle { get; }

		/// <exclude/>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CriAtomExAcb(IntPtr handle) =>
			NativeHandle = handle;
		/// <exclude />
		public override bool Equals(object obj) =>
			obj is CriAtomExAcb other && NativeHandle.Equals(other.NativeHandle);
		/// <exclude />
		public override int GetHashCode() =>
			NativeHandle.GetHashCode();
		/// <exclude />
		public static bool operator ==(CriAtomExAcb a, CriAtomExAcb b)
		{
			if (a is null) return b is null;
			return a.Equals(b);
		}
		/// <exclude />
		public static bool operator !=(CriAtomExAcb a, CriAtomExAcb b) =>
			!(a == b);

	}
}