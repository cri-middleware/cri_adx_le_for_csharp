using Godot;
using System;
using CriWare;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

public partial class SampleScript : Node {
	[Export]
	NodePath cueListPath;
	ItemList _cueList = null;
	ItemList CueList => _cueList ?? (_cueList = GetNode<ItemList>(cueListPath));

	IDisposable errorHandler;

	public override void _Ready() {
		// Register error callback
		errorHandler = CriBaseCSharp.ErrorCallback.RegisterListener(msg => {
			GD.Print(msg);
			GD.Print(new StackTrace());
		}, false);
		
		// Godot uses customized file system
		// CriFsGodotFileAccess class setups the file sytem to make ADX uses Godot.FileAccess
		CriFsGodotFileAccess.Setup();

		// Load ACF
		CriAtomPreviewPlayer.Instance.LoadAcf("res://data/DemoProj.acf"u8);

		// Load ACB
		CriAtomPreviewPlayer.Instance.LoadAcb("res://data/DemoProj.acb"u8, "res://data/DemoProj.awb"u8);
		foreach(var info in CriAtomPreviewPlayer.Instance.GetCurrentCueInfoList())
			CueList.AddItem(info.name);
	}


	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		errorHandler?.Dispose();
		CriFsGodotFileAccess.Cleanup();
	}
	
	private void _on_play_button_pressed(){
		CriAtomPreviewPlayer.Instance.Play(CueList.GetSelectedItems().First());
	}
	
	private void _on_stop_button_pressed(){
		CriAtomPreviewPlayer.Instance.Stop();
	}
	
	private void _on_pause_button_toggled(bool toggled_on)
	{
		CriAtomPreviewPlayer.Instance.Pause(toggled_on);
	}

	private void _on_aisac_control_value_changed(double val) {
		CriAtomPreviewPlayer.Instance.SetAisacControlByName("rpm", (float)val);
	}
}
