using Godot;

public partial class PlayCue : TextureButton {
	[Export]
	int cueId;

	void Play() => CriAtomPreviewPlayer.Instance.Play(cueId, false);

	void PlayToggled(bool on){
		if(on)
			CriAtomPreviewPlayer.Instance.Play(cueId);
		else
			CriAtomPreviewPlayer.Instance.Stop();
	}

}
