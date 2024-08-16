/****************************************************************************
 *
 * Copyright (c) 2024 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.InteropServices;

#if NET7_0_OR_GREATER
[assembly:System.Runtime.CompilerServices.DisableRuntimeMarshalling]
#endif

#pragma warning disable 0465
namespace CriWare {
	/// <summary>ADXをC#から利用するための補助機能を持つクラス</summary>
	public static partial class CriAtomCSharp {
		internal const string libraryName =
#if (UNITY_IOS && !UNITY_EDITOR) || ios
			"__Internal";
#else
			"cri_atom";
#endif

		internal const CallingConvention callingConversion = CallingConvention.Cdecl;

#if !CRI_BUILD_LE
		static CriAtomCSharp() {
			CriErr.GetErrorCount(CriErr.Level.Error);
			CriFs.GetDefaultIoInterface(out var _);
		}
#elif android
		static CriAtomCSharp() => CriBaseCSharp.NativeMethods.criErr_GetErrorCount(CriErr.Level.Error);
#endif

		/// <summary>プラットフォーム共通初期化コンフィグ</summary>
		[System.Serializable]
		public struct Config {
			/// <summary>AtomExコンフィグ</summary>
			public CriAtomEx.Config atomEx;
			/// <summary>ASRコンフィグ</summary>
			public CriAtomExAsr.Config asr;
			/// <summary>HCA-MXコンフィグ</summary>
			public CriAtomExHcaMx.Config hcaMx;
		}

		/// <summary>初期化コンフィグのデフォルト値取得</summary>
		public static void GetDefaultConfig(out Config config){
			CriAtomEx.SetDefaultConfig(out config.atomEx);
			config.atomEx.threadModel = CriAtomEx.ThreadModel.MultiWithSonicsync;
			CriAtomExAsr.SetDefaultConfig(out config.asr);
			CriAtomExHcaMx.SetDefaultConfig(out config.hcaMx);
		}

		/// <summary>C#での利用向けのアロケータ登録</summary>
		/// <remarks>
		/// 本プラグインは専用のアロケータ関数を登録する利用方法を想定しています。
		/// 各プラットフォームごとの初期化関数などを利用する場合は本メソッドを利用してアロケータ登録を行ってください。
		/// <see cref="CriAtomCSharp.Initialize(Config)"/>を利用する場合は初期化処理内でアロケータ登録が行われるため、本メソッドの呼び出しは不要です。
		/// </remarks>
		public unsafe static void SetupDefaultAllocator(){
			CriAtom.SetUserMallocFunction(default, IntPtr.Zero);
			CriAtom.SetUserFreeFunction(default, IntPtr.Zero);	
			CriAtom.SetUserMallocFunction(NativeAllocator.GetAllocateFunc(), IntPtr.Zero);
			CriAtom.SetUserFreeFunction(NativeAllocator.GetFreeFunc(), IntPtr.Zero);	
		}

		/// <summary>
		/// プラットフォーム共通初期化メソッド
		/// </summary>
		/// <param name="config">初期化コンフィグ</param>
		/// <remarks>
		/// ADXライブラリの初期化を行います。
		/// ADXの初期化処理はプラットフォーム毎に別々のメソッドが提供されていますが、共通のメソッドで処理を行いたい場合は本メソッドを利用できます。
		/// 本メソッドを利用してADXを初期化した場合、ライブラリ終了には<see cref="CriAtomCSharp.Finalize"/>を利用してください。
		/// </remarks>
		public static void Initialize(Config config){
			SetupDefaultAllocator();

			CriAtomEx.SetDefaultConfig(out var defaultConfig);
			config.atomEx.versionString = defaultConfig.versionString;
			config.atomEx.versionExString = defaultConfig.versionExString;

			InitializePlatform(config);
		}

		/// <summary>
		/// プラットフォーム共通終了メソッド
		/// </summary>
		/// <remarks>
		/// ADXライブラリの終了を行います。
		/// <see cref="CriAtomCSharp.Initialize(Config)"/>を利用して初期化した場合、本メソッドでライブラリを終了してください。 
		/// </remarks>
		public static void Finalize() =>
			FinalizePlatform();

		static partial void InitializePlatform(in Config config);
		static partial void FinalizePlatform();

		[Serializable]
		class CriAtomCSharpLibrary : Interfaces.ILibrary
		{
			public Config config;
			public Type[] DependentLibraries { get; } = new Type[] { 
#if !CRI_BUILD_LE
				typeof(CriFsCSharp.CriFsCSharpLibrary)
#endif
			};
			public bool IsInitialized => CriAtomEx.IsInitialized();
			public void FinalizeLibrary() => CriAtomCSharp.Finalize();
			public void InitializeLibrary() => CriAtomCSharp.Initialize(config);
			public CriAtomCSharpLibrary() => CriAtomCSharp.GetDefaultConfig(out config);
		}
	}
}