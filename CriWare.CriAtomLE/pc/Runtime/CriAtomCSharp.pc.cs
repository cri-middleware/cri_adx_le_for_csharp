using System.Collections.Generic;
using System.Runtime.InteropServices;

#pragma warning disable 0465
namespace CriWare{	
	partial class CriAtomCSharp {
#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN || win
		static partial void InitializePlatform(in Config config){
			CriFs.SwitchPathUnicodeToUtf8PC(true);
			var platformConfig = new CriAtomEx.ConfigWASAPI(){
				atomEx = config.atomEx,
				asr = config.asr,
				hcaMx = config.hcaMx,
			};
			CriAtomEx.InitializeWASAPI(platformConfig);
		}
		static partial void FinalizePlatform() => CriAtomEx.FinalizeWASAPI();
#endif
	}
}