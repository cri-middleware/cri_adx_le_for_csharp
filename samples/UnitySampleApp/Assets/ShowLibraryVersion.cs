using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;

public class ShowLibraryVersion : MonoBehaviour
{
	void Awake() =>
		GetComponent<UnityEngine.UI.Text>().text = CriAtom.GetVersionString();
}
