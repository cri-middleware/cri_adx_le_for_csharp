using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CriWare{	
	partial class CriAtomCSharp {
#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX || osx
		static partial void InitializePlatform(in Config config){
			var platformConfig = new CriAtomEx.ConfigMACOSX(){
				atomEx = config.atomEx,
				asr = config.asr,
				hcaMx = config.hcaMx,
			};
			CriAtomEx.InitializeMACOSX(platformConfig);
		}
		static partial void FinalizePlatform() => CriAtomEx.FinalizeMACOSX();
#endif
	}
}