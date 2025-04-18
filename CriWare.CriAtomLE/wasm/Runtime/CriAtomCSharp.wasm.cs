using System.Collections.Generic;
using System.Runtime.InteropServices;

#pragma warning disable 0465
namespace CriWare{	
	partial class CriAtomCSharp {
#if (UNITY_WEBGL && !UNITY_EDITOR) || browser
		static partial void InitializePlatform(in Config config){
			var platformConfig = new CriAtomEx.ConfigWEBAUDIO(){
				atomEx = config.atomEx,
				asr = config.asr,
				hcaMx = config.hcaMx,
			};
			platformConfig.atomEx.threadModel = CriAtomEx.ThreadModel.Single;
			CriAtomEx.InitializeWEBAUDIO(platformConfig);
		}
		static partial void FinalizePlatform() => CriAtomEx.FinalizeWEBAUDIO();
#endif
	}
}