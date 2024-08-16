using System.Collections.Generic;
using System.Runtime.InteropServices;

#pragma warning disable 0465
namespace CriWare{	
	partial class CriAtomCSharp {
#if (UNITY_ANDROID && !UNITY_EDITOR) || android
		static partial void InitializePlatform(in Config config){
			var platformConfig = new CriAtomEx.ConfigANDROID(){
				atomEx = config.atomEx,
				asr = config.asr,
				hcaMx = config.hcaMx,
			};
			CriAtomEx.InitializeANDROID(platformConfig);
		}
		static partial void FinalizePlatform() => CriAtomEx.FinalizeANDROID();
#endif
	}
}