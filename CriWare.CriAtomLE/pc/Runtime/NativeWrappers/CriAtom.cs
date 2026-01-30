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
		/// ライブラリが必要とするワーク領域のサイズは、ライブラリ初期化用コンフィグ 構造体（ <see cref="CriAtom.ConfigPC"/> ）の内容によって変化します。
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.CalculateWorkSizePC"/> 関数をご利用ください。 
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_CalculateWorkSize_PC(const CriAtomConfig_PC *config)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigPC"/>
		/// <seealso cref="CriAtom.InitializePC"/>
		public static unsafe Int32 CalculateWorkSizePC(in CriAtom.ConfigPC config)
		{
			fixed (CriAtom.ConfigPC* configPtr = &config)
				return NativeMethods.criAtom_CalculateWorkSize_PC(configPtr);
		}

		/// <summary>ライブラリの初期化 </summary>
		/// <param name="config">初期化用コンフィグ構造体 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリを初期化します。
		///  ライブラリの機能を利用するには、必ずこの関数を実行する必要があります。
		///  （ライブラリの機能は、本関数を実行後、 <see cref="CriAtom.FinalizePC"/> 関数を実行するまでの間、 利用可能です。）
		///  ライブラリを初期化する際には、ライブラリが内部で利用するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  ライブラリが必要とするワーク領域のサイズは、初期化用コンフィグ構造体の内容に応じて 変化します。
		///  ワーク領域サイズの計算には、 <see cref="CriAtom.CalculateWorkSizePC"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、 本関数にワーク領域を指定する必要はありません。
		///  （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター から必要なワーク領域サイズ分のメモリが動的に確保されます。） 
		///  引数 config の情報は、関数内でのみ参照されます。
		///  関数を抜けた後は参照されませんので、関数実行後に config の領域を解放しても 問題ありません。 
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数は内部的に以下の関数を実行します。
		/// <list type="bullet">
		/// <item><description>criAtom_Initialize</description></item>
		/// <item><description><see cref="CriAtomAsr.Initialize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Initialize"/> 本関数を実行する場合、上記関数を実行しないでください。
		///  本関数を実行後、必ず対になる <see cref="CriAtom.FinalizePC"/> 関数を実行してください。
		///  また、 <see cref="CriAtom.FinalizePC"/> 関数を実行するまでは、本関数を再度実行しないでください。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.InitializePC"/> 関数をご利用ください。 </description></item>
		/// </list>
		/// </para>
		/// <nativeinfo declaration="void criAtom_Initialize_PC(const CriAtomConfig_PC *config, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.ConfigPC"/>
		/// <seealso cref="CriAtom.FinalizePC"/>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtom.CalculateWorkSizePC"/>
		public static unsafe void InitializePC(in CriAtom.ConfigPC config)
		{
			fixed (CriAtom.ConfigPC* configPtr = &config)
				NativeMethods.criAtom_Initialize_PC(configPtr, default, default);
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
		/// <item><description>criAtom_Finalize</description></item>
		/// <item><description><see cref="CriAtomAsr.Finalize"/></description></item>
		/// <item><description><see cref="CriAtomHcaMx.Finalize"/> 本関数を実行する場合、上記関数を実行しないでください。
		/// <see cref="CriAtom.InitializePC"/> 関数実行前に本関数を実行することはできません。
		///  本関数は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本関数の代わりに <see cref="CriAtomEx.FinalizePC"/> 関数をご利用ください。 </description></item>
		/// </list>
		/// </para>
		/// <nativeinfo declaration="void criAtom_Finalize_PC(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		public static void FinalizePC()
		{
			NativeMethods.criAtom_Finalize_PC();
		}

		/// <summary>サーバー処理スレッドのプライオリティ変更 </summary>
		/// <param name="prio">スレッドプライオリティ </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのプライオリティを変更します。
		///  デフォルト状態（本関数を実行しない場合）では、サーバー処理スレッドのプライオリティは THREAD_PRIORITY_HIGHEST に設定されます。
		/// </para>
		/// <para>
		/// 注意:
		/// : 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		///  他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		///  （エラーコールバックが発生します。）
		///  本関数は初期化後～終了処理前の間に実行する必要があります。
		///  初期化前や終了処理後に本関数を実行しても、効果はありません。
		///  （エラーコールバックが発生します。）
		///  サーバー処理スレッドは、CRI File Systemライブラリでも利用されています。
		///  すでにCRI File SystemライブラリのAPIでサーバー処理スレッドの設定を変更している場合 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetThreadPriority_PC(CriSint32 prio)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.GetThreadPriorityPC"/>
		public static void SetThreadPriorityPC(Int32 prio)
		{
			NativeMethods.criAtom_SetThreadPriority_PC(prio);
		}

		/// <summary>サーバー処理スレッドのプライオリティ取得 </summary>
		/// <returns>int スレッドプライオリティ </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのプライオリティを取得します。
		///  取得に成功すると、本関数はサーバー処理を行うスレッドのプライオリティを返します。
		///  取得に失敗した場合、本関数は THREAD_PRIORITY_ERROR_RETURN を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// : 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		///  他のスレッドモデルを選択した場合、本関数はエラー値を返します。
		///  （エラーコールバックが発生します。）
		///  本関数は初期化後～終了処理前の間に実行する必要があります。
		///  初期化前や終了処理後に本関数を実行した場合、本関数はエラー値を返します。
		///  （エラーコールバックが発生します。）
		/// </para>
		/// <nativeinfo declaration="int criAtom_GetThreadPriority_PC(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.SetThreadPriorityPC"/>
		public static int GetThreadPriorityPC()
		{
			return NativeMethods.criAtom_GetThreadPriority_PC();
		}

		/// <summary>サーバー処理スレッドのアフィニティマスク変更 </summary>
		/// <param name="mask">スレッドアフィニティマスク </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのアフィニティマスクを変更します。
		///  デフォルト状態（本関数を実行しない場合）では、サーバー処理が動作するプロセッサは 一切制限されません。
		/// </para>
		/// <para>
		/// 注意:
		/// : 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		///  他のスレッドモデルを選択した場合、本関数は何も処理を行いません。
		///  （エラーコールバックが発生します。）
		///  本関数は初期化後～終了処理前の間に実行する必要があります。
		///  初期化前や終了処理後に本関数を実行しても、効果はありません。
		///  （エラーコールバックが発生します。）
		///  サーバー処理スレッドは、CRI File Systemライブラリでも利用されています。
		///  すでにCRI File SystemライブラリのAPIでサーバー処理スレッドの設定を変更している場合 本関数により設定が上書きされますのでご注意ください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_SetThreadAffinityMask_PC(DWORD_PTR mask)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.GetThreadAffinityMaskPC"/>
		public static void SetThreadAffinityMaskPC(IntPtr mask)
		{
			NativeMethods.criAtom_SetThreadAffinityMask_PC(mask);
		}

		/// <summary>サーバー処理スレッドのアフィニティマスクの取得 </summary>
		/// <returns>DWORD_PTR スレッドアフィニティマスク </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// サーバー処理（ライブラリの内部処理）を行うスレッドのアフィニティマスクを取得します。
		///  取得に成功すると、本関数はサーバー処理を行うスレッドのアフィニティマスクを返します。
		///  取得に失敗した場合、本関数は 0 を返します。
		/// </para>
		/// <para>
		/// 注意:
		/// : 本関数は、ライブラリ初期化時にスレッドモデルをマルチスレッドモデル （ <see cref="CriAtom.ThreadModel.Multi"/> ）に設定した場合にのみ効果を発揮します。
		///  他のスレッドモデルを選択した場合、本関数はエラー値を返します。
		///  （エラーコールバックが発生します。）
		///  本関数は初期化後～終了処理前の間に実行する必要があります。
		///  初期化前や終了処理後に本関数を実行した場合、本関数はエラー値を返します。
		///  （エラーコールバックが発生します。）
		/// </para>
		/// <nativeinfo declaration="DWORD_PTR criAtom_GetThreadAffinityMask_PC(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		/// <seealso cref="CriAtom.SetThreadAffinityMaskPC"/>
		public static IntPtr GetThreadAffinityMaskPC()
		{
			return NativeMethods.criAtom_GetThreadAffinityMask_PC();
		}

		/// <summary>パラレルミキサー機能有効化用ワーク領域サイズの計算 </summary>
		/// <param name="numSubMixers">メインのミキサーに追加して使用するサブミキサーの数 </param>
		/// <returns>CriSint32 ワーク領域サイズ </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラレルミキサー機能を使用するために必要な、ワーク領域のサイズを取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// パラレルミキサー機能有効化時に必要とするワーク領域のサイズは、サブミキサーの数と、 メインのミキサーの内容に応じて変化します。
		/// </para>
		/// <nativeinfo declaration="CriSint32 criAtom_CalculateStartParallelMixerWorkSize(CriUint32 num_sub_mixers)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.StartParallelMixer"/>
		public static Int32 CalculateStartParallelMixerWorkSize(UInt32 numSubMixers)
		{
			return NativeMethods.criAtom_CalculateStartParallelMixerWorkSize(numSubMixers);
		}

		/// <summary>パラレルミキサー機能の有効化 </summary>
		/// <param name="numSubMixers">メインのミキサーに追加して使用するサブミキサーの数 </param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラレルミキサー機能を有効化します。
		///  （パラレルミキサー機能は、本関数を実行後、 <see cref="CriAtom.FinishParallelMixer"/> 関数を実行するまでの間、 利用可能です。）
		///  パラレルミキサー機能は、cri_atom_game_share.hで提供されるおすそわけ通信機能と 合わせて利用します。サブミキサーでは、ゲスト用に再生された音声に対し、 メインミキサーと同じようなバスエフェクトを掛けることが出来ます。 パラレルミキサー機能を利用するには、必ずこの関数を実行する必要があります。
		///  パラレルミキサー機能を有効化する際には、追加のミキサーを作成するためのメモリ領域（ワーク領域） を確保する必要があります。
		///  追加のミキサーが必要とするワーク領域のサイズは、作成するサブミキサーの数と、 メインのミキサーの内容に応じて変化します。
		///  ワーク領域サイズの計算には、 <see cref="CriAtom.CalculateStartParallelMixerWorkSize"/> 関数を使用してください。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtom.SetUserAllocator"/> メソッドを使用してアロケーターを登録済みの場合、 本関数にワーク領域を指定する必要はありません。
		///  （ work に null 、 work_size に 0 を指定することで、登録済みのアロケーター から必要なワーク領域サイズ分のメモリが動的に確保されます。）
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行後、必ず対になる <see cref="CriAtom.FinishParallelMixer"/> 関数を実行してください。
		///  また、 <see cref="CriAtom.FinishParallelMixer"/> 関数を実行するまでは、本関数を再度実行しないでください。
		/// </para>
		/// <nativeinfo declaration="void criAtom_StartParallelMixer(CriUint32 num_sub_mixers, void *work, CriSint32 work_size)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.FinishParallelMixer"/>
		/// <seealso cref="CriAtom.SetUserAllocator"/>
		/// <seealso cref="CriAtom.CalculateStartParallelMixerWorkSize"/>
		public static void StartParallelMixer(UInt32 numSubMixers)
		{
			NativeMethods.criAtom_StartParallelMixer(numSubMixers, default, default);
		}

		/// <summary>パラレルミキサー機能の終了 </summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラレルミキサー機能を終了します。
		/// </para>
		/// <para>
		/// 注意:
		/// <see cref="CriAtom.StartParallelMixer"/> 関数実行前に本関数を実行することはできません。
		/// </para>
		/// <nativeinfo declaration="void criAtom_FinishParallelMixer(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.StartParallelMixer"/>
		public static void FinishParallelMixer()
		{
			NativeMethods.criAtom_FinishParallelMixer();
		}

		/// <summary>パラレルミキサー機能の有効化状態の取得 </summary>
		/// <returns>CriBool パラレルミキサー機能が有効化中かどうか </returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// パラレルミキサー機能が既に有効化されているかどうかをチェックします。
		/// </para>
		/// <nativeinfo declaration="CriBool criAtom_IsDuringParallelMixer(void)"/>
		/// </remarks>
		/// <seealso cref="CriAtom.StartParallelMixer"/>
		/// <seealso cref="CriAtom.FinishParallelMixer"/>
		public static bool IsDuringParallelMixer()
		{
			return NativeMethods.criAtom_IsDuringParallelMixer();
		}

		/// <summary>Atomライブラリ初期化用コンフィグ構造体</summary>
		/// <remarks>
		/// <para>
		/// 注意:
		/// 本構造体は下位レイヤ向けのAPIです。
		///  AtomExレイヤの機能を利用する際には、本構造体の代わりに <see cref="CriAtomEx.ConfigPC"/> 構造体をご利用ください。 
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtom.InitializePC"/>
		[Serializable]
		public unsafe partial struct ConfigPC
		{
			/// <summary></summary>
			/// <remarks>
			/// <para>Atom初期化用コンフィグ構造体 </para>
			/// </remarks>
			public CriAtom.Config atom;

			/// <summary></summary>
			/// <remarks>
			/// <para>ASR初期化用コンフィグ </para>
			/// </remarks>
			public CriAtomAsr.Config asr;

			/// <summary></summary>
			/// <remarks>
			/// <para>HCA-MX初期化用コンフィグ構造体 </para>
			/// </remarks>
			public CriAtomHcaMx.Config hcaMx;

		}
	}
}