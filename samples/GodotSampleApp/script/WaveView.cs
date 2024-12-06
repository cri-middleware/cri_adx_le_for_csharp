using Godot;
using System;
using System.Linq;

namespace CriWare;

public partial class WaveView : Line2D {
	public override void _Ready()
	{
		float[][] data = null;
		var applyWaves = Callable.From(() => {
			var size = GetParent<Control>().Size;
			lock(data){
				Points = data[0].Select((value, index) => new Vector2(index * size.X / data[0].Length, size.Y / 2 + value * size.Y / 2)).ToArray();
			}
		});
		CriAtomPreviewPlayer.Instance.Player.FilterCallback.WithCopiedPcm().Event += pcm => {
			data = pcm;
			applyWaves.CallDeferred();
		};
	}
}

