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
		/// ライブラリを使用するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ 構造体（ <see cref="CriAtom.ConfigMACOSX"/> ）の内容によって変化します。
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.CalculateWorkSizeMACOSX"/> 関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 CRIAPI criAtom_CalculateWorkSize_MACOSX(const CriAtomConfig_MACOSX *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigMACOSX"/>
		/// <seealso cref="CriAtom.InitializeMACOSX"/>
		public static unsafe Int32 CalculateWorkSizeMACOSX(in CriAtom.ConfigMACOSX config)
		{
			fixed (CriAtom.ConfigMACOSX* configPtr = &config)
				return NativeMethods.criAtom_CalculateWorkSize_MACOSX(configPtr);
		}

		/// <summary>ライブラリの初期化 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		///  ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		///  （ライブラリの機能は、本関数を実行後、 <see cref="CriAtom.FinalizeMACOSX"/> 関数を実行するまでの間、 利用可能です。）
		///  ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて 変化します。
		///  ワーク領域サイズの計算には、 <see cref="CriAtom.CalculateWorkSizeMACOSX"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケータを登録済みの場合、 本関数にワーク領域を指定する必要はありません。
		///  （ work に null 、 work_size に 0 を指定することで、登録済みのアロケータ から必要なワーク領域サイズ分のメモリが動的に確保されます。） 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// <list type="bullet">
		/// <item><description><see cref="CriAtom.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomAsr.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Initialize"/></description></item>
		/// </list>
		/// </para>
		/// <see><see cref="CriAtom.FinalizeMACOSX"/></see>
		/// <see><see cref="CriAtom.FinalizeMACOSX"/></see>
		/// <see><see cref="CriAtomEx.InitializeMACOSX"/></see>
		/// <para>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		///  本関数を実行後、必ず対になる 
		///  関数を実行してください。
		///  また、 
		///  関数を実行するまでは、本関数を再度実行しないでください。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに 
		///  関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Initialize_MACOSX(const CriAtomConfig_MACOSX *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigMACOSX"/>
		/// <seealso cref="CriAtom.FinalizeMACOSX"/>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtom.CalculateWorkSizeMACOSX"/>
		public static unsafe void InitializeMACOSX(in CriAtom.ConfigMACOSX config)
		{
			fixed (CriAtom.ConfigMACOSX* configPtr = &config)
				NativeMethods.criAtom_Initialize_MACOSX(configPtr, default, default);
		}

		/// <summary>ライブラリの終了 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// <list type="bullet">
		/// <item><description><see cref="CriAtom.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomAsr.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Finalize"/></description></item>
		/// </list>
		/// </para>
		/// <see><see cref="CriAtom.InitializeMACOSX"/></see>
		/// <see><see cref="CriAtomEx.FinalizeMACOSX"/></see>
		/// <para>
		/// 本関数を実行する場合、上記関数を実行しないでください。
		///  関数実行前に本関数を実行することはできません。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに 
		///  関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_Finalize_MACOSX(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeMACOSX"/>
		public static void FinalizeMACOSX()
		{
			NativeMethods.criAtom_Finalize_MACOSX();
		}

		/// <summary>サーバスレッドプライオリティの設定 </summary>
		/// <param name="prio">スレッドのプライオリティ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// CRIサーバスレッドのプライオリティを設定します。
		///  引数 prio は pthread のプライオリティ設定値として使用します。
		///  指定できる値の範囲は通常 -16～99で、数字が大きい方が優先度が高くなります。
		///  アプリケーションのメインスレッド(0)よりも高いプライオリティを指定してください。
		///  プライオリティのデフォルト値は10です。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtom.InitializeMACOSX"/> 関数実行前に本関数を実行することはできません。
		///  サーバ処理スレッドは、CRI File Systemライブラリでも利用されています。
		///  すでにCRI File SystemライブラリのAPIでサーバ処理スレッドの設定を変更している場合 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// <nativeinfo declaration="void CRIAPI criAtom_SetServerThreadPriority_MACOSX(int prio)"/>
		/// </remarks>
		public static void SetServerThreadPriorityMACOSX(Int32 prio)
		{
			NativeMethods.criAtom_SetServerThreadPriority_MACOSX(prio);
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 注意:
		/// 本構造体は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本構造体の代わりに <see cref="CriAtomEx.ConfigMACOSX"/> 構造体をご利用ください。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializeMACOSX"/>
		[Serializable]
		public unsafe partial struct ConfigMACOSX
		{
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
			public CriAtom.Config atom;

			/// <summary>ASR初期化用コンフィグ構造体</summary>
			/// <remarks>
			/// <para>
			/// 備考:
			/// デフォルト設定を使用する場合、 <see cref="CriAtomAsr.SetDefaultConfig"/> メソッドで 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomAsr.Initialize"/> 関数 に構造体を指定してください。
			/// </para>
			/// <para>
			/// 注意:
			/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomAsr.SetDefaultConfig"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
			///  （構造体のメンバに不定値が入らないようご注意ください。） 
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomAsr.Initialize"/>
			/// <seealso cref="CriAtomAsr.SetDefaultConfig"/>
			public CriAtomAsr.Config asr;

			/// <summary>HCA-MX初期化用コンフィグ構造体</summary>
			/// <remarks>
			/// <para>
			/// 備考:
			/// デフォルト設定を使用する場合、 <see cref="CriAtomHcaMx.SetDefaultConfig"/> メソッドで 構造体にデフォルトパラメーターをセットした後、 <see cref="CriAtomHcaMx.Initialize"/> 関数 に構造体を指定してください。
			/// </para>
			/// <para>
			/// 注意:
			/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomHcaMx.SetDefaultConfig"/> メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
			///  （構造体のメンバに不定値が入らないようご注意ください。） 
			/// </para>
			/// </remarks>
			/// <seealso cref="CriAtomHcaMx.Initialize"/>
			/// <seealso cref="CriAtomHcaMx.SetDefaultConfig"/>
			public CriAtomHcaMx.Config hcaMx;

		}
	}
}