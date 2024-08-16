using Godot;

public partial class SetNextBlock : Button {
	[Export]
	int block;

	void SetNext(){
		CriAtomPreviewPlayer.Instance.SetNextBlockIndex(block);
	}
}
