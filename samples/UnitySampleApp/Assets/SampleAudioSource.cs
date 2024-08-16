using UnityEngine;
using CriWare;
using CriWare.Unity;

public class SampleAudioSource : MonoBehaviour
{
	[SerializeField]
	int cueId;

    CriAtomEx3dSource source;
	CriAtomExPlayback playback;

	private void Awake() =>
		source = new CriAtomEx3dSource();

	private void LateUpdate()
	{
		source.SetTransform(transform);
		source.Update();
	}

	private void OnEnable() =>
		playback = SampleAudioPlayer.Instance.Play(cueId, source);

	private void OnDestroy()
	{
		playback.Stop();
		source?.Dispose();
	}
}
