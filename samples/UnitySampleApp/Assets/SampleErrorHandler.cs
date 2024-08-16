using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SampleErrorHandler
{
#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoadMethod]
#else
	[RuntimeInitializeOnLoadMethod]
#endif
	public static void RegisterErrorHandler()
	{
		if (Application.isBatchMode) return;
		CriWare.CriBaseCSharp.ErrorCallback.Event += Debug.LogError;
	}

}
