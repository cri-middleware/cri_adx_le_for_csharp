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
	/// <summary>CriAtomInstrument API</summary>
	public static partial class CriAtomInstrument
	{
		/// <summary>ユーザ定義インストゥルメントインターフェースの登録</summary>
		/// <param name="ainstInterface">ユーザ定義インストゥルメントのバージョン情報付きインターフェース</param>
		/// <returns>false:登録に失敗した）</returns>
		/// <remarks>
		/// <para>
		/// 説明:
		/// ユーザ定義インストゥルメントインターフェースをAtomライブラリに登録します。
		/// ユーザ定義インストゥルメントインターフェースを登録したインストゥルメントは、インストゥルメントトラックの再生に使用できるようになります。
		/// 以下の条件に該当する場合は、ユーザ定義インストゥルメントインターフェースの登録に失敗し、エラーコールバックが返ります:
		/// - 同一のエフェクト名を持つユーザ定義インストゥルメントインターフェースが既に登録されている
		/// - Atomが使用しているユーザ定義インストゥルメントインターフェースと異なる
		/// - ユーザ定義エフェクトインターフェースの登録数上限（ <see cref="CriAtomExAsr.MaxNumUserEffectInterfaces"/> ）に達した
		/// </para>
		/// <para>
		/// 注意:
		/// ユーザ定義インストゥルメントインターフェースは、インストゥルメントトラックを再生する前に
		/// 本関数によって登録を行って下さい。
		/// Atomライブラリ使用中にインターフェースの登録解除を行う場合は、 <see cref="CriAtomInstrument.UnregisterInstrumentInterface"/> を使用して下さい。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomInstrument.UnregisterInstrumentInterface"/>
		public static bool RegisterInstrumentInterface(IntPtr ainstInterface)
		{
			return NativeMethods.criAtomInstrument_RegisterInstrumentInterface(ainstInterface);
		}

		/// <summary>ユーザ定義インストゥルメントインターフェースの登録解除</summary>
		/// <param name="ainstInterface">ユーザ定義インストゥルメントのバージョン情報付きインターフェース</param>
		/// <remarks>
		/// <para>
		/// 説明:
		/// インストゥルメントインターフェースの登録を解除します。
		/// 登録を解除したインストゥルメントはインストゥルメントトラックを再生する際に使用できなくなります。
		/// 登録処理を行っていないインストゥルメントインターフェースの登録を解除することはできません（エラーコールバックが返ります）。
		/// </para>
		/// <para>
		/// 注意:
		/// 登録を行ったユーザ定義インストゥルメントインターフェースはプレーヤーの再生中に参照され続けるため、
		/// 全てのプレーヤーが停止させた後で、本関数を実行してください。
		/// Atomライブラリの終了時（::criAtom_Finalize 関数の呼び出し時）には全てのユーザ定義インストゥルメントインターフェースの登録が解除されます。
		/// </para>
		/// </remarks>
		/// <seealso cref="CriAtomInstrument.RegisterInstrumentInterface"/>
		public static void UnregisterInstrumentInterface(IntPtr ainstInterface)
		{
			NativeMethods.criAtomInstrument_UnregisterInstrumentInterface(ainstInterface);
		}

		/// <remarks>
		/// <para>
		/// 説明:
		/// インストゥルメントが再生可能なプレーヤーを作成する際に、動作仕様を指定するための構造体です。
		/// 作成されるプレーヤーは、オブジェクト作成時に本構造体で指定された設定に応じて、
		/// 内部リソースを必要なだけ確保します。
		/// プレーヤーが必要とするワーク領域のサイズは、本構造体で指定されたパラメーターに応じて変化します。
		/// </para>
		/// <para>
		/// 注意:
		/// 将来的にメンバが増える可能性があるため、 <see cref="CriAtomPlayer.SetDefaultConfigForInstrumentPlayer"/>
		/// メソッドを使用しない場合には、使用前に必ず構造体をゼロクリアしてください。
		/// （構造体のメンバに不定値が入らないようご注意ください。）
		/// </para>
		/// </remarks>
		public unsafe partial struct PlayerConfig
		{
			/// <summary>インターフェース名</summary>
			public NativeString interfaceName;

			/// <summary>インストゥルメント名</summary>
			public NativeString instrumentName;

			/// <summary>最大チャンネル数</summary>
			public Int32 maxChannels;

			/// <summary>最大サンプリングレート</summary>
			public Int32 maxSamplingRate;

			/// <summary>出力先のサウンドレンダラタイプ</summary>
			public CriAtom.SoundRendererType soundRendererType;

		}
	}
}