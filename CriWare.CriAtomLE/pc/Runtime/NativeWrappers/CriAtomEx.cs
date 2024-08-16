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
	/// <summary>CriAtomEx API</summary>
	public static partial class CriAtomEx
	{
		/// <summary>ライブラリ初期化用ワーク領域サイズの計算</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <returns>ワーク領域サイズ</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを使用するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ
		/// 構造体（ <see cref="CriAtomEx.ConfigWASAPI"/> ）の内容によって変化します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.ConfigWASAPI"/> 構造体のacf_infoメンバに値を設定している場合、本関数は失敗し-1を返します。
		/// 初期化処理内でACFデータの登録を行う場合は、本関数値を使用したメモリ確保ではなくADXシステムによる
		/// メモリアロケーターを使用したメモリ確保処理が必要になります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigWASAPI"/>
		/// <seealso cref="CriAtomEx.InitializeWASAPI"/>
		public static unsafe Int32 CalculateWorkSizeWASAPI(in CriAtomEx.ConfigWASAPI config)
		{
			fixed (CriAtomEx.ConfigWASAPI* configPtr = &config)
				return NativeMethods.criAtomEx_CalculateWorkSize_WASAPI(configPtr);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <seealso cref="CriAtomEx.InitializeWASAPI"/>
		public unsafe partial struct ConfigWASAPI
		{
			/// <summary>AtomEx初期化用コンフィグ構造体</summary>
			public CriAtomEx.Config atomEx;

			/// <summary>ASR初期化用コンフィグ</summary>
			public CriAtomExAsr.Config asr;

			/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
			public CriAtomExHcaMx.Config hcaMx;

		}
		/// <summary>ライブラリの初期化</summary>
		/// <param name="config">初期化用コンフィグ構造体</param>
		/// <param name="work">ワーク領域</param>
		/// <param name="workSize">ワーク領域サイズ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		/// ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		/// （ライブラリの機能は、本関数を実行後、 <see cref="CriAtomEx.FinalizeWASAPI"/> 関数を実行するまでの間、
		/// 利用可能です。）
		/// ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて
		/// 変化します。
		/// ワーク領域サイズの計算には、 <see cref="CriAtomEx.CalculateWorkSizeWASAPI"/>
		/// 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター
		/// から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// - <see cref="CriAtomEx.Initialize"/>
		/// - <see cref="CriAtomExAsr.Initialize"/>
		/// - <see cref="CriAtomExHcaMx.Initialize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// 本関数を実行後、必ず対になる <see cref="CriAtomEx.FinalizeWASAPI"/> 関数を実行してください。
		/// また、 <see cref="CriAtomEx.FinalizeWASAPI"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigWASAPI"/>
		/// <seealso cref="CriAtomEx.FinalizeWASAPI"/>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeWASAPI"/>
		public static unsafe void InitializeWASAPI(in CriAtomEx.ConfigWASAPI config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx.ConfigWASAPI* configPtr = &config)
				NativeMethods.criAtomEx_Initialize_WASAPI(configPtr, work, workSize);
		}

		/// <summary>ライブラリの終了</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// - <see cref="CriAtomEx.Finalize"/>
		/// - <see cref="CriAtomExAsr.Finalize"/>
		/// - <see cref="CriAtomExHcaMx.Finalize"/>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtomEx.InitializeWASAPI"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.InitializeWASAPI"/>
		public static void FinalizeWASAPI()
		{
			NativeMethods.criAtomEx_Finalize_WASAPI();
		}

	}
}