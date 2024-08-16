
using Godot;
using CriWare;
using System;

public partial class CriWareConsole : RichTextLabel {
	IDisposable errorHandler;

	public override void _Ready()
	{
		base._Ready();
		errorHandler = CriBaseCSharp.ErrorCallback.RegisterListener(msg => AddText(msg + "\n"));
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		errorHandler?.Dispose();
	}
}
