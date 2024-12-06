using UnityEngine;
using CriWare;
using CriWare.Unity;

public class SampleAudioSource : MonoBehaviour
{
	[SerializeField]
	int cueId;

	CriAtomExPlayer player;
	CriAtomEx3dSource source;

	private void Awake(){
		player = new CriAtomExPlayer();
		source = new CriAtomEx3dSource();
		player.Set3dSourceHn(source);
	}

	private void LateUpdate()
	{
		source.SetTransform(transform);
		source.Update();
	}

	private void OnEnable(){
		player.SetCueId(null, cueId);
		player.Start();
	}

	private void OnDestroy()
	{
		player?.Dispose();
		source?.Dispose();
	}
}
