using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;

public class ShowLibraryVersion : MonoBehaviour
{
	void Awake() =>
		GetComponent<UnityEngine.UI.Text>().text = $"{CriAtom.GetVersionString()}{ScriptingBackend}\nUnity: {Application.unityVersion}";

	const string ScriptingBackend =
#if ENABLE_IL2CPP
		"IL2CPP";
#else
		"Mono";
#endif
}
