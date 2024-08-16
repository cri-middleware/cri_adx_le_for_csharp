using System.Collections.Generic;
using System.Runtime.InteropServices;

#pragma warning disable 0465
namespace CriWare{	
	partial class CriAtomCSharp {
#if (UNITY_IOS && !UNITY_EDITOR) || ios
		static partial void InitializePlatform(in Config config){
			var platformConfig = new CriAtomEx.ConfigIOS(){
				atomEx = config.atomEx,
				asr = config.asr,
				hcaMx = config.hcaMx,
			};
			CriAtomEx.InitializeIOS(platformConfig);
		}
		static partial void FinalizePlatform() => CriAtomEx.FinalizeIOS();
#endif
	}
}