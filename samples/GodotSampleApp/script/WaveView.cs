using Godot;
using System.Linq;

namespace CriWare;

public partial class WaveView : Line2D {
	System.IDisposable filterCallback;
	public override void _Ready()
	{
		filterCallback = CriAtomPreviewPlayer.Instance.Player.FilterCallback.WithCopiedPcm().RegisterListener(data => {
			var size = GetParent<Control>().Size;
			Points = data[0].Select((value, index) => new Vector2(index * size.X / data[0].Length, size.Y / 2 + value * size.Y / 2)).ToArray();
		});
	}

	protected override void Dispose(bool disposing)
	{
		filterCallback?.Dispose();
		base.Dispose(disposing);
	}
}

