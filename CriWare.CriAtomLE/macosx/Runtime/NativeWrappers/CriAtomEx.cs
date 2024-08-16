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
		/// 構造体（ <see cref="CriAtomEx.ConfigMACOSX"/> ）の内容によって変化します。
		/// 引数 config の情報は、関数内でのみ参照されます。
		/// 関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても
		/// 問題ありません。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.ConfigMACOSX"/> 構造体のacf_infoメンバに値を設定している場合、本関数は失敗し-1を返します。
		/// 初期化処理内でACFデータの登録を行う場合は、本関数値を使用したメモリ確保ではなくADXシステムによる
		/// メモリアロケータを使用したメモリ確保処理が必要になります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigMACOSX"/>
		/// <seealso cref="CriAtomEx.InitializeMACOSX"/>
		public static unsafe Int32 CalculateWorkSizeMACOSX(in CriAtomEx.ConfigMACOSX config)
		{
			fixed (CriAtomEx.ConfigMACOSX* configPtr = &config)
				return NativeMethods.criAtomEx_CalculateWorkSize_MACOSX(configPtr);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <seealso cref="CriAtomEx.InitializeMACOSX"/>
		public unsafe partial struct ConfigMACOSX
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
		/// （ライブラリの機能は、本関数を実行後、 <see cref="CriAtomEx.FinalizeMACOSX"/> 関数を実行するまでの間、
		/// 利用可能です。）
		/// ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域）
		/// を確保する必要があります。
		/// ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて
		/// 変化します。
		/// ワーク領域サイズの計算には、 ::criAtom_CalculateWorkSize_MACOSX
		/// 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomEx.SetUserAllocator"/> メソッドを使用してアロケータを登録済みの場合、
		/// 本関数にワーク領域を指定する必要はありません。
		/// （ work に null 、 work_size に 0 を指定することで、登録済みのアロケータ
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
		/// .
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// 本関数を実行後、必ず対になる <see cref="CriAtomEx.FinalizeMACOSX"/> 関数を実行してください。
		/// また、 <see cref="CriAtomEx.FinalizeMACOSX"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.ConfigMACOSX"/>
		/// <seealso cref="CriAtomEx.FinalizeMACOSX"/>
		/// <seealso cref="CriAtomEx.SetUserAllocator"/>
		/// <seealso cref="CriAtomEx.CalculateWorkSizeMACOSX"/>
		public static unsafe void InitializeMACOSX(in CriAtomEx.ConfigMACOSX config, IntPtr work = default, Int32 workSize = default)
		{
			fixed (CriAtomEx.ConfigMACOSX* configPtr = &config)
				NativeMethods.criAtomEx_Initialize_MACOSX(configPtr, work, workSize);
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
		/// .
		/// 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtomEx.InitializeMACOSX"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.InitializeMACOSX"/>
		public static void FinalizeMACOSX()
		{
			NativeMethods.criAtomEx_Finalize_MACOSX();
		}

		/// <summary>サーバスレッドプライオリティの設定</summary>
		/// <param name="prio">スレッドのプライオリティ</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRIサーバスレッドのプライオリティを設定します。
		/// 引数 prio は pthread のプライオリティ設定値として使用します。
		/// 指定できる値の範囲は通常 -16～99で、数字が大きい方が優先度が高くなります。
		/// アプリケーションのメインスレッド(0)よりも高いプライオリティを指定してください。
		/// プライオリティのデフォルト値は10です。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtomEx.InitializeMACOSX"/> 関数実行前に本関数を実行することはできません。
		/// サーバ処理スレッドは、CRI File Systemライブラリでも利用されています。
		/// すでにCRI File SystemライブラリのAPIでサーバ処理スレッドの設定を変更している場合
		/// 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// </remarks>
		public static void SetServerThreadPriorityMACOSX(Int32 prio)
		{
			NativeMethods.criAtomEx_SetServerThreadPriority_MACOSX(prio);
		}

	}
}