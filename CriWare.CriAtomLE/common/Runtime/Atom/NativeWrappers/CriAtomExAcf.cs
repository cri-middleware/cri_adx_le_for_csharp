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
	/// <summary>CriAtomExAcf API</summary>
	public static partial class CriAtomExAcf
	{
		/// <summary>ACFの出力ポートオブジェクトの取得（名前指定）</summary>
		/// <param name="name">出力ポート名</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFに保持されている出力ポートの中から、指定した出力ポート名のオブジェクトを取得します。
		/// ACFの出力ポートオブジェクトはACFの登録時に生成、保持されます。
		/// ACFに登録された出力ポート名は、ACFのヘッダーに記載されています。
		/// 生成後の出力ポートオブジェクトにはデフォルトASRラックが設定されているため、この関数で取得したオブジェクトに対して
		/// <see cref="CriAtomExOutputPort.SetAsrRackId"/> 関数で適切なASRラックを指定する必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExOutputPort.SetAsrRackId"/>
		public static CriAtomExOutputPort GetOutputPortHnByName(ArgString name)
		{
			IntPtr handle;
			return ((handle = NativeMethods.criAtomExAcf_GetOutputPortHnByName(name.GetPointer(stackalloc byte[name.BufferSize]))) == IntPtr.Zero) ? null : new CriAtomExOutputPort(handle);
		}

		/// <summary>ACF位置情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリの初期化処理内でACF登録を行う際の、ACFデータの指定情報です。
		/// ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）に <see cref="CriAtomEx.Config"/>
		/// 構造体のacf_infoメンバに設定します。
		/// </para>
		/// <para>
		/// 備考
		/// データ指定のタイプによって、設定すべき情報が異なります。
		/// 適切なtypeを設定し、共用体infoの中の対応構造体に設定を行ってください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Initialize"/>
		/// <seealso cref="CriAtomEx.Config"/>
		public unsafe partial struct RegistrationInfo
		{
			/// <summary>指定タイプ</summary>
			public CriAtomExAcf.LocationInfoType type;

			/// <summary>ACF位置情報共用体</summary>
			public CriAtomExAcf.LocationInfoTag info;

		}
		/// <summary>ACF指定タイプ</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Atomライブラリの初期化処理内でACF登録を行う際の、ACF情報の指定タイプを表します。
		/// ライブラリ初期化時（ <see cref="CriAtomEx.Initialize"/> 関数 ）に <see cref="CriAtomEx.Config"/>
		/// 構造体の <see cref="CriAtomExAcf.RegistrationInfo"/> にて指定します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomEx.Initialize"/>
		/// <seealso cref="CriAtomEx.Config"/>
		/// <seealso cref="CriAtomExAcf.RegistrationInfo"/>
		public enum LocationInfoType
		{
			/// <summary>未設定</summary>
			None = 0,
			/// <summary>ファイル名</summary>
			Name = 1,
			/// <summary>コンテンツID</summary>
			Id = 2,
			/// <summary>オンメモリデータ</summary>
			Data = 3,
		}
		/// <summary>ACF位置情報共用体</summary>
		[StructLayout(LayoutKind.Explicit)]
		public unsafe partial struct LocationInfoTag
		{
			/// <summary>ファイル名指定時情報</summary>
			[FieldOffset(0)] public CriAtomExAcf.LocationInfoNameTag name;

			/// <summary>ファイルID指定時情報</summary>
			[FieldOffset(0)] public CriAtomExAcf.LocationInfoIdTag id;

			/// <summary>オンメモリデータ指定時情報</summary>
			[FieldOffset(0)] public CriAtomExAcf.LocationInfoDataTag data;

		}
		/// <summary>ファイル名指定時情報</summary>
		public unsafe partial struct LocationInfoNameTag
		{
			/// <summary>バインダーオブジェクト</summary>
			public IntPtr binder;

			/// <summary>ACFファイルパス</summary>
			public NativeString path;

		}
		/// <summary>ファイルID指定時情報</summary>
		public unsafe partial struct LocationInfoIdTag
		{
			/// <summary>バインダーオブジェクト</summary>
			public IntPtr binder;

			/// <summary>コンテンツID</summary>
			public Int32 id;

		}
		/// <summary>オンメモリデータ指定時情報</summary>
		public unsafe partial struct LocationInfoDataTag
		{
			/// <summary>メモリアドレス</summary>
			public IntPtr buffer;

			/// <summary>サイズ</summary>
			public Int32 size;

		}
		/// <summary>AISACコントロール数の取得</summary>
		/// <returns>AISACコントロール数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録されたACFに含まれるAISACコントロールの数を取得します。
		/// ACFが登録されていない場合、-1が返ります。
		/// </para>
		/// </remarks>
		public static Int32 GetNumAisacControls()
		{
			return NativeMethods.criAtomExAcf_GetNumAisacControls();
		}

		/// <summary>AISACコントロール情報の取得</summary>
		/// <param name="index">AISACコントロールインデックス</param>
		/// <param name="info">AISACコントロール情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AISACコントロールインデックスからAISACコントロール情報を取得します。
		/// 指定したインデックスのAISACコントロールが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetAisacControlInfo(UInt16 index, out CriAtomEx.AisacControlInfo info)
		{
			fixed (CriAtomEx.AisacControlInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetAisacControlInfo(index, infoPtr);
		}

		/// <summary>AISACコントロールIDの取得（AISACコントロール名指定）</summary>
		/// <param name="name">AISACコントロール名</param>
		/// <returns>AISACコントロールID</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AISACコントロール名からAISACコントロールIDを取得します。
		/// ACFが登録されていない、または指定したAISACコントロール名のAISACコントロールが存在しない場合、<see cref="CriAtomEx.InvalidAisacControlId"/>が返ります。
		/// </para>
		/// </remarks>
		public static UInt32 GetAisacControlIdByName(ArgString name)
		{
			return NativeMethods.criAtomExAcf_GetAisacControlIdByName(name.GetPointer(stackalloc byte[name.BufferSize]));
		}

		/// <summary>AISACコントロール名の取得（AISACコントロールID指定）</summary>
		/// <param name="id">AISACコントロールID</param>
		/// <returns>CriChar8* AISACコントロール名</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// AISACコントロールIDからAISACコントロール名を取得します。
		/// ACFが登録されていない、または指定したAISACコントロールIDのAISACコントロールが存在しない場合、nullが返ります。
		/// </para>
		/// </remarks>
		public static NativeString GetAisacControlNameById(UInt32 id)
		{
			return NativeMethods.criAtomExAcf_GetAisacControlNameById(id);
		}

		/// <summary>DSPバス設定数の取得</summary>
		/// <returns>DSPバス設定数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリに登録されたACFデータに含まれるDSPバス設定の数を取得します。
		/// ACFデータが登録されていない場合、本関数は -1 を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspSettingNameByIndex"/>
		public static Int32 GetNumDspSettings()
		{
			return NativeMethods.criAtomExAcf_GetNumDspSettings();
		}

		/// <summary>ACFデータからDSPバス設定数を取得</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <returns>DSPバス設定数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたACFに含まれるDSPバス設定の数を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAcf.GetNumDspSettings"/> 関数と異なり、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetNumDspSettings"/>
		public static Int32 GetNumDspSettingsFromAcfData(IntPtr acfData, Int32 acfDataSize)
		{
			return NativeMethods.criAtomExAcf_GetNumDspSettingsFromAcfData(acfData, acfDataSize);
		}

		/// <summary>DSPバス設定名の取得（index指定）</summary>
		/// <param name="index">DSPバス設定インデックス</param>
		/// <returns>CriChar8* DSPバス設定名</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリに登録されたACFデータからDSPバス設定名を取得します。
		/// ACFデータが登録されていないか、
		/// または指定したDSPバス設定インデックスのDSPバス設定が存在しない場合、
		/// 本関数は null を返します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspSettingInformation"/>
		public static NativeString GetDspSettingNameByIndex(UInt16 index)
		{
			return NativeMethods.criAtomExAcf_GetDspSettingNameByIndex(index);
		}

		/// <summary>ACFデータからDSPバス設定名を取得</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <param name="index">DSPバス設定インデックス</param>
		/// <returns>CriChar8* DSPバス設定名</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたACFデータに含まれるDSPバス設定名を取得します。
		/// 第 3 引数（ index ）には、何番目のDSPバス設定の名称を取得するかを指定します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAcf.GetDspSettingNameByIndex"/> 関数と異なり、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspSettingNameByIndex"/>
		public static NativeString GetDspSettingNameByIndexFromAcfData(IntPtr acfData, Int32 acfDataSize, UInt16 index)
		{
			return NativeMethods.criAtomExAcf_GetDspSettingNameByIndexFromAcfData(acfData, acfDataSize, index);
		}

		/// <summary>DSPバス設定情報の取得</summary>
		/// <param name="name">セッティング名</param>
		/// <param name="info">セッティング情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// セッティング名を指定してセッティング情報を取得します。
		/// 指定したセッティング名のDsp settingが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspSettingNameByIndex"/>
		/// <seealso cref="CriAtomExAcf.GetDspBusInformation"/>
		public static unsafe bool GetDspSettingInformation(ArgString name, out CriAtomExAcf.DspSettingInfo info)
		{
			fixed (CriAtomExAcf.DspSettingInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetDspSettingInformation(name.GetPointer(stackalloc byte[name.BufferSize]), infoPtr);
		}

		/// <summary>DSPバス設定の情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定の情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetDspSettingInformation"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspSettingInformation"/>
		public unsafe partial struct DspSettingInfo
		{
			/// <summary>セッティング名</summary>
			public NativeString name;

			/// <summary>DSPバスインデックス配列</summary>
			public InlineArray64<UInt16> busIndexes;

			/// <summary>DSP拡張バスインデックス配列</summary>
			public InlineArray64<UInt16> extendBusIndexes;

			/// <summary>スナップショット開始インデックス</summary>
			public UInt16 snapshotStartIndex;

			/// <summary>有効DSPバス数</summary>
			public Byte numBuses;

			/// <summary>有効拡張DSPバス数</summary>
			public Byte numExtendBuses;

			/// <summary>スナップショット数</summary>
			public UInt16 numSnapshots;

			/// <summary>スナップショット用ワーク領域サイズ</summary>
			public UInt16 snapshotWorkSize;

			/// <summary>ミキサーAISAC数</summary>
			public UInt16 numMixerAisacs;

			/// <summary>ミキサーAISAC開始インデックス</summary>
			public UInt16 mixerAisacStartIndex;

		}
		/// <summary>DSPバス設定スナップショット情報の取得</summary>
		/// <param name="index">スナップショットインデックス</param>
		/// <param name="info">スナップショット情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// スナップショットインデックスを指定してスナップショット情報を取得します。
		/// 指定したセッティング名のスナップショットが存在しない場合、falseが返ります。
		/// スナップショットインデックスは親となるDSPバス設定情報の <see cref="CriAtomExAcf.DspSettingInfo"/> 構造体内の
		/// snapshot_start_indexメンバとnum_snapshotsメンバを元に適切な値を算出してください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspBusInformation"/>
		public static unsafe bool GetDspSettingSnapshotInformation(UInt16 index, out CriAtomExAcf.DspSettingSnapshotInfo info)
		{
			fixed (CriAtomExAcf.DspSettingSnapshotInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetDspSettingSnapshotInformation(index, infoPtr);
		}

		/// <summary>DSPバス設定スナップショットの情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定のスナップショット情報を取得するための構造体です。
		/// </para>
		/// </remarks>
		public unsafe partial struct DspSettingSnapshotInfo
		{
			/// <summary>スナップショット名</summary>
			public NativeString name;

			/// <summary>有効DSPバス数</summary>
			public Byte numBuses;

			/// <summary>有効拡張DSPバス数</summary>
			public Byte numExtendBuses;

			/// <summary>予約領域</summary>
			public InlineArray2<Byte> reserved;

			/// <summary>DSPバスインデックス配列</summary>
			public InlineArray64<UInt16> busIndexes;

			/// <summary>DSP拡張バスインデックス配列</summary>
			public InlineArray64<UInt16> extendBusIndexes;

		}
		/// <summary>DSPバスの取得</summary>
		/// <param name="index">バスインデックス</param>
		/// <param name="info">バス情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インデックスを指定してDSPバス情報を取得します。
		/// 指定したインデックス名のDSPバスが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspSettingInformation"/>
		/// <seealso cref="CriAtomExAcf.GetDspFxName"/>
		/// <seealso cref="CriAtomExAcf.GetDspFxParameters"/>
		/// <seealso cref="CriAtomExAcf.GetDspBusLinkInformation"/>
		public static unsafe bool GetDspBusInformation(UInt16 index, out CriAtomExAcf.DspBusInfo info)
		{
			fixed (CriAtomExAcf.DspBusInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetDspBusInformation(index, infoPtr);
		}

		/// <summary>DSPバス設定情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバス設定情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetDspBusInformation"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspBusInformation"/>
		public unsafe partial struct DspBusInfo
		{
			/// <summary>名前</summary>
			public NativeString name;

			/// <summary>音量</summary>
			public Single volume;

			/// <summary>Pan3D 音量</summary>
			public Single pan3dVolume;

			/// <summary>Pan3D 角度</summary>
			public Single pan3dAngle;

			/// <summary>Pan3D インテリア距離</summary>
			public Single pan3dDistance;

			/// <summary>Pan3D マルチチャンネル音源の広がり</summary>
			public Single pan3dWideness;

			/// <summary>Pan3D スプレッド</summary>
			public Single pan3dSpread;

			/// <summary>DSP FXインデックス配列</summary>
			public InlineArray8<UInt16> fxIndexes;

			/// <summary>DSPバスリンクインデックス配列</summary>
			public InlineArray64<UInt16> busLinkIndexes;

			/// <summary>セッティング内DSPバス番号</summary>
			public UInt16 busNo;

			/// <summary>DSP FX数</summary>
			public Byte numFxes;

			/// <summary>DSPバスリンク数</summary>
			public Byte numBusLinks;

		}
		/// <summary>DSP FX名の取得</summary>
		/// <param name="index">DSP FXインデックス</param>
		/// <returns>CriChar8* 文字列へのポインタ。失敗した場合は、CRI_NULLが返ります。</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インデックスを指定してDSP FX名を文字列で取得します。
		/// DSP FX名はASRを使用する環境かつ、ACF Ver.1.16.00 以降のACFを使用した時に取得可能です。
		/// ASRを使用しない環境、またはACF Ver.1.15.01 以前ではCRI_NULLが
		/// 返ります。<see cref="CriAtomExAcf.GetAcfInfo"/> 関数でACFのバージョンを確認してご使用下さい。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspBusInformation"/>
		/// <seealso cref="CriAtomExAcf.GetDspFxParameters"/>
		/// <seealso cref="CriAtomExAcf.GetAcfInfo"/>
		public static NativeString GetDspFxName(UInt16 index)
		{
			return NativeMethods.criAtomExAcf_GetDspFxName(index);
		}

		/// <summary>DSP FXパラメーターの取得</summary>
		/// <param name="index">DSP FXインデックス</param>
		/// <param name="parameters">DSP FXパラメーター</param>
		/// <param name="size">DSP FXパラメーターワークサイズ</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インデックスを指定してACFからFXパラメーターを取得します。
		/// 指定したインデックス名のDSP FXが存在しない場合、CRI FALSEが返ります。
		/// size引数にはDSP FXタイプに応じたパラメーターのサイズを指定してください。
		/// サウンドレンダラにASRを指定した場合は、ACFにある実行時パラメーターがfloat配列の形式でparametersに取得されます。
		/// ASR以外でのサウンドレンダラでは、パラメーター構造体が得られます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspBusInformation"/>
		/// <seealso cref="CriAtomExAcf.GetDspFxName"/>
		public static bool GetDspFxParameters(UInt16 index, IntPtr parameters, Int32 size)
		{
			return NativeMethods.criAtomExAcf_GetDspFxParameters(index, parameters, size);
		}

		/// <summary>DSPバスリンクの取得</summary>
		/// <param name="index">DSPバスリンクインデックス</param>
		/// <param name="info">DSPバスリンク情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インデックスを指定してバスリンク情報を取得します。
		/// 指定したインデックス名のDSPバスリンクが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspBusInformation"/>
		public static unsafe bool GetDspBusLinkInformation(UInt16 index, out CriAtomExAcf.DspBusLinkInfo info)
		{
			fixed (CriAtomExAcf.DspBusLinkInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetDspBusLinkInformation(index, infoPtr);
		}

		/// <summary>DSPバスリンク情報取得用構造体</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// DSPバスリンク情報を取得するための構造体です。
		/// <see cref="CriAtomExAcf.GetDspBusLinkInformation"/> 関数に引数として渡します。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetDspBusLinkInformation"/>
		public unsafe partial struct DspBusLinkInfo
		{
			/// <summary>タイプ</summary>
			public CriAtomExAcf.DspBusLinkType type;

			/// <summary>センドレベル</summary>
			public Single sendLevel;

			/// <summary>送り先のセッティング内DSPバス番号</summary>
			public UInt16 busNo;

			/// <summary>送り先のセッティング内DSPバスID</summary>
			public UInt16 busId;

		}
		/// <summary>DSPバスリンクタイプ</summary>
		/// <seealso cref="CriAtomExAcf.DspBusLinkInfo"/>
		public enum DspBusLinkType
		{
			/// <summary>プレボリュームタイプ</summary>
			PreVolume = 0,
			/// <summary>ポストボリュームタイプ</summary>
			PostVolume = 1,
			/// <summary>ポストパンタイプ</summary>
			PostPan = 2,
		}
		/// <summary>ACFデータからカテゴリ数を取得</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <returns>カテゴリ数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたACFに含まれるカテゴリの数を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAcf.GetNumCategories"/> 関数と異なり、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetNumCategories"/>
		public static Int32 GetNumCategoriesFromAcfData(IntPtr acfData, Int32 acfDataSize)
		{
			return NativeMethods.criAtomExAcf_GetNumCategoriesFromAcfData(acfData, acfDataSize);
		}

		/// <summary>カテゴリ数の取得</summary>
		/// <returns>カテゴリ数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録されたACFに含まれるカテゴリの数を取得します。
		/// </para>
		/// </remarks>
		public static Int32 GetNumCategories()
		{
			return NativeMethods.criAtomExAcf_GetNumCategories();
		}

		/// <summary>ACFデータから再生毎カテゴリ参照数を取得</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <returns>再生毎カテゴリ参照数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたACFに含まれるカテゴリの数を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAcf.GetNumCategoriesPerPlayback"/> 関数と異なり、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetNumCategoriesPerPlayback"/>
		public static Int32 GetNumCategoriesPerPlaybackFromAcfData(IntPtr acfData, Int32 acfDataSize)
		{
			return NativeMethods.criAtomExAcf_GetNumCategoriesPerPlaybackFromAcfData(acfData, acfDataSize);
		}

		/// <summary>再生毎カテゴリ参照数の取得</summary>
		/// <returns>再生毎カテゴリ参照数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録されたACFに含まれる再生毎カテゴリ参照数を取得します。
		/// </para>
		/// </remarks>
		public static Int32 GetNumCategoriesPerPlayback()
		{
			return NativeMethods.criAtomExAcf_GetNumCategoriesPerPlayback();
		}

		/// <summary>カテゴリ情報の取得（インデックス指定）</summary>
		/// <param name="index">カテゴリインデックス</param>
		/// <param name="info">カテゴリ情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリインデックスからカテゴリ情報を取得します。
		/// 指定したインデックスのカテゴリが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetCategoryInfo(UInt16 index, out CriAtomExCategory.Info info)
		{
			fixed (CriAtomExCategory.Info* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetCategoryInfo(index, infoPtr);
		}

		/// <summary>カテゴリ情報の取得（カテゴリ名指定）</summary>
		/// <param name="name">カテゴリ名</param>
		/// <param name="info">カテゴリ情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリ名からカテゴリ情報を取得します。
		/// 指定したカテゴリ名のカテゴリが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetCategoryInfoByName(ArgString name, out CriAtomExCategory.Info info)
		{
			fixed (CriAtomExCategory.Info* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetCategoryInfoByName(name.GetPointer(stackalloc byte[name.BufferSize]), infoPtr);
		}

		/// <summary>カテゴリ情報の取得（カテゴリID指定）</summary>
		/// <param name="id">カテゴリID</param>
		/// <param name="info">カテゴリ情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// カテゴリIDからカテゴリ情報を取得します。
		/// 指定したカテゴリIDのカテゴリが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetCategoryInfoById(UInt32 id, out CriAtomExCategory.Info info)
		{
			fixed (CriAtomExCategory.Info* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetCategoryInfoById(id, infoPtr);
		}

		/// <summary>Global Aisac数の取得</summary>
		/// <returns>Global Aisac数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録されたACFに含まれるGlobal Aisacの数を取得します。
		/// </para>
		/// </remarks>
		public static Int32 GetNumGlobalAisacs()
		{
			return NativeMethods.criAtomExAcf_GetNumGlobalAisacs();
		}

		/// <summary>Global Aisac情報の取得（インデックス指定）</summary>
		/// <param name="index">Global Aisacインデックス</param>
		/// <param name="info">Global Aisac情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Global AisacインデックスからAisac情報を取得します。
		/// 指定したインデックスのGlobal Aisacが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetGlobalAisacInfo(UInt16 index, out CriAtomEx.GlobalAisacInfo info)
		{
			fixed (CriAtomEx.GlobalAisacInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetGlobalAisacInfo(index, infoPtr);
		}

		/// <summary>Aisacタイプ</summary>
		/// <seealso cref="CriAtomEx.GlobalAisacInfo"/>
		public enum AisacType
		{
			/// <summary>ノーマルタイプ</summary>
			Normal = 0,
			/// <summary>オートモジュレーションタイプ</summary>
			AutoModulation = 1,
		}
		/// <summary>Global Aisac情報の取得（名前指定）</summary>
		/// <param name="name">Global Aisac名</param>
		/// <param name="info">Global Aisac情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Global Aisac名からAisac情報を取得します。
		/// 指定した名前のGlobal Aisacが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetGlobalAisacInfoByName(ArgString name, out CriAtomEx.GlobalAisacInfo info)
		{
			fixed (CriAtomEx.GlobalAisacInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetGlobalAisacInfoByName(name.GetPointer(stackalloc byte[name.BufferSize]), infoPtr);
		}

		/// <summary>Global Aisac Graph情報の取得</summary>
		/// <param name="aisacInfo">Global Aisac情報</param>
		/// <param name="graphIndex">Aisac graphインデックス</param>
		/// <param name="graphInfo">Aisac graph情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Global Aisac情報とgraphインデックスからgraph情報を取得します。
		/// 指定したインデックスのGlobal Aisacが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetGlobalAisacGraphInfo(in CriAtomEx.GlobalAisacInfo aisacInfo, UInt16 graphIndex, out CriAtomEx.AisacGraphInfo graphInfo)
		{
			fixed (CriAtomEx.GlobalAisacInfo* aisacInfoPtr = &aisacInfo)
			fixed (CriAtomEx.AisacGraphInfo* graphInfoPtr = &graphInfo)
				return NativeMethods.criAtomExAcf_GetGlobalAisacGraphInfo(aisacInfoPtr, graphIndex, graphInfoPtr);
		}

		/// <summary>Global Aisac値の取得</summary>
		/// <param name="aisacInfo">Global Aisac情報</param>
		/// <param name="control">AISACコントロール値</param>
		/// <param name="type">グラフタイプ</param>
		/// <param name="value">AISAC値</param>
		/// <returns>値が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// Global Aisac情報、コントロール値、グラフタイプを指定してAisac値を取得します。
		/// 指定したインデックスのGlobal Aisacが存在しない場合やグラフが存在しない場合は、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetGlobalAisacValue(in CriAtomEx.GlobalAisacInfo aisacInfo, Single control, CriAtomEx.AisacGraphType type, out Single value)
		{
			fixed (CriAtomEx.GlobalAisacInfo* aisacInfoPtr = &aisacInfo)
			fixed (Single* valuePtr = &value)
				return NativeMethods.criAtomExAcf_GetGlobalAisacValue(aisacInfoPtr, control, type, valuePtr);
		}

		/// <summary>ACF情報の取得</summary>
		/// <param name="acfInfo">ACF情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ライブラリに登録されたACFデータの各種情報を取得します。
		/// ACF情報の取得に失敗した場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetAcfInfo(out CriAtomExAcf.Info acfInfo)
		{
			fixed (CriAtomExAcf.Info* acfInfoPtr = &acfInfo)
				return NativeMethods.criAtomExAcf_GetAcfInfo(acfInfoPtr);
		}

		/// <summary>ACF情報</summary>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFデータの詳細情報です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetAcfInfo"/>
		/// <seealso cref="CriAtomExAcf.GetAcfInfoFromAcfData"/>
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

			/// <summary>DSP設定数</summary>
			public Int32 numDspSettings;

			/// <summary>カテゴリ数</summary>
			public Int32 numCategories;

			/// <summary>再生毎カテゴリ参照数</summary>
			public Int32 numCategoriesPerPlayback;

			/// <summary>REACT数</summary>
			public Int32 numReacts;

			/// <summary>AISACコントロール数</summary>
			public Int32 numAisacControls;

			/// <summary>グローバルAISAC数</summary>
			public Int32 numGlobalAisacs;

			/// <summary>ゲーム変数数</summary>
			public Int32 numGameVariables;

			/// <summary>DSP設定内最大バス数</summary>
			public Int32 maxBusesOfDspBusSettings;

			/// <summary>バス数</summary>
			public Int32 numBuses;

			/// <summary>ボイスリミットグループ数</summary>
			public Int32 numVoiceLimitGroups;

			/// <summary>出力ポート数</summary>
			public Int32 numOutputPorts;

		}
		/// <summary>ACFデータからACF情報を取得</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <param name="acfInfo">ACF情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたACFデータの各種情報を取得します。
		/// ACF情報の取得に失敗した場合、falseが返ります。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAcf.GetAcfInfo"/> 関数と異なり、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// </para>
		/// <para>
		/// 注意:
		/// 取得したACF情報内のポインタメンバは、ACFデータ領域内を指しています。（名前文字列へのポインタ等）
		/// 取得したACF情報を参照している間は、ACFデータ領域を解放しないようご注意ください。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetAcfInfo"/>
		public static unsafe bool GetAcfInfoFromAcfData(IntPtr acfData, Int32 acfDataSize, out CriAtomExAcf.Info acfInfo)
		{
			fixed (CriAtomExAcf.Info* acfInfoPtr = &acfInfo)
				return NativeMethods.criAtomExAcf_GetAcfInfoFromAcfData(acfData, acfDataSize, acfInfoPtr);
		}

		/// <summary>セレクター数の取得</summary>
		/// <returns>セレクター数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録されたACFに含まれるセレクターの数を取得します。
		/// </para>
		/// </remarks>
		public static Int32 GetNumSelectors()
		{
			return NativeMethods.criAtomExAcf_GetNumSelectors();
		}

		/// <summary>セレクター情報の取得（インデックス指定）</summary>
		/// <param name="index">セレクターインデックス</param>
		/// <param name="info">セレクター情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// セレクターインデックスからセレクター情報を取得します。
		/// 指定したインデックスのセレクターが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetSelectorInfoByIndex(UInt16 index, out CriAtomEx.SelectorInfo info)
		{
			fixed (CriAtomEx.SelectorInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetSelectorInfoByIndex(index, infoPtr);
		}

		/// <summary>セレクター情報の取得（名前指定）</summary>
		/// <param name="name">セレクター名</param>
		/// <param name="info">セレクター情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// セレクター名からセレクター情報を取得します。
		/// 指定した名前のセレクターが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetSelectorInfoByName(ArgString name, out CriAtomEx.SelectorInfo info)
		{
			fixed (CriAtomEx.SelectorInfo* infoPtr = &info)
				return NativeMethods.criAtomExAcf_GetSelectorInfoByName(name.GetPointer(stackalloc byte[name.BufferSize]), infoPtr);
		}

		/// <summary>セレクターラベル情報の取得</summary>
		/// <param name="selectorInfo">セレクター情報</param>
		/// <param name="labelIndex">ラベルインデックス</param>
		/// <param name="labelInfo">セレクターラベル情報</param>
		/// <returns>情報が取得できたかどうか？（取得できた：true／取得できない：false）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// セレクター情報とセレクターラベルインデックスからセレクターラベル情報を取得します。
		/// 指定したインデックスのセレクターラベルが存在しない場合、falseが返ります。
		/// </para>
		/// </remarks>
		public static unsafe bool GetSelectorLabelInfo(in CriAtomEx.SelectorInfo selectorInfo, UInt16 labelIndex, out CriAtomEx.SelectorLabelInfo labelInfo)
		{
			fixed (CriAtomEx.SelectorInfo* selectorInfoPtr = &selectorInfo)
			fixed (CriAtomEx.SelectorLabelInfo* labelInfoPtr = &labelInfo)
				return NativeMethods.criAtomExAcf_GetSelectorLabelInfo(selectorInfoPtr, labelIndex, labelInfoPtr);
		}

		/// <summary>セレクターに対するグローバル参照ラベルの設定</summary>
		/// <param name="selsectorName">セレクター名</param>
		/// <param name="labelName">ラベル名</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイル内に登録されているセレクターに対してグローバル参照されるラベルを設定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ACFファイルを登録しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.SetGlobalLabelToSelectorByIndex"/>
		public static void SetGlobalLabelToSelectorByName(ArgString selsectorName, ArgString labelName)
		{
			NativeMethods.criAtomExAcf_SetGlobalLabelToSelectorByName(selsectorName.GetPointer(stackalloc byte[selsectorName.BufferSize]), labelName.GetPointer(stackalloc byte[labelName.BufferSize]));
		}

		/// <summary>セレクターに対するグローバル参照ラベルの設定</summary>
		/// <param name="selsectorIndex">セレクターインデックス</param>
		/// <param name="labelIndex">ラベルインデックス</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ACFファイル内に登録されているセレクターに対してグローバル参照されるラベルを設定します。
		/// </para>
		/// <para>
		/// 注意:
		/// 本関数を実行する前に、ACFファイルを登録しておく必要があります。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.SetGlobalLabelToSelectorByName"/>
		public static void SetGlobalLabelToSelectorByIndex(UInt16 selsectorIndex, UInt16 labelIndex)
		{
			NativeMethods.criAtomExAcf_SetGlobalLabelToSelectorByIndex(selsectorIndex, labelIndex);
		}

		/// <summary>ACFデータからバス数を取得</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <returns>バス数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたACFに含まれるバスの数を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAcf.GetNumBuses"/> 関数と異なり、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetNumBuses"/>
		public static Int32 GetNumBusesFromAcfData(IntPtr acfData, Int32 acfDataSize)
		{
			return NativeMethods.criAtomExAcf_GetNumBusesFromAcfData(acfData, acfDataSize);
		}

		/// <summary>バス数の取得</summary>
		/// <returns>バス数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録されたACFに含まれるバスの数を取得します。
		/// </para>
		/// </remarks>
		public static Int32 GetNumBuses()
		{
			return NativeMethods.criAtomExAcf_GetNumBuses();
		}

		/// <summary>ACFデータからDSPバス設定内の最大バス数を取得</summary>
		/// <param name="acfData">ACFデータ</param>
		/// <param name="acfDataSize">ACFデータサイズ</param>
		/// <returns>DSPバス設定内の最大バス数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたACFに含まれるDSPバス設定内の最大バスの数を取得します。
		/// </para>
		/// <para>
		/// 備考:
		/// <see cref="CriAtomExAcf.GetMaxBusesOfDspBusSettings"/> 関数と異なり、
		/// ACF情報を登録する前でも本関数は実行可能です。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomExAcf.GetMaxBusesOfDspBusSettings"/>
		public static Int32 GetMaxBusesOfDspBusSettingsFromAcfData(IntPtr acfData, Int32 acfDataSize)
		{
			return NativeMethods.criAtomExAcf_GetMaxBusesOfDspBusSettingsFromAcfData(acfData, acfDataSize);
		}

		/// <summary>DSPバス設定内の最大バス数の取得</summary>
		/// <returns>DSPバス設定内の最大バス数</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 登録されたACFに含まれるDSPバス設定内の最大バスの数を取得します。
		/// </para>
		/// </remarks>
		public static Int32 GetMaxBusesOfDspBusSettings()
		{
			return NativeMethods.criAtomExAcf_GetMaxBusesOfDspBusSettings();
		}

		/// <summary>ACF内のバス名取得</summary>
		/// <param name="busName">バス名</param>
		/// <returns>CriChar8*		ACF内バス名</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// 指定されたバス名のACF内文字列を取得します。
		/// 存在しないバス名を指定した場合はnullが返ります。
		/// </para>
		/// </remarks>
		public static NativeString FindBusName(ArgString busName)
		{
			return NativeMethods.criAtomExAcf_FindBusName(busName.GetPointer(stackalloc byte[busName.BufferSize]));
		}

	}
}