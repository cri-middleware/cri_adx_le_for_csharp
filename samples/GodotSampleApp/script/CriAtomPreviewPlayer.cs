using System;
using System.Runtime.InteropServices;
using CriWare;
using CriWare.InteropHelpers;

public partial class CriAtomPreviewPlayer : IDisposable {
	static CriAtomPreviewPlayer _instance;
	public static CriAtomPreviewPlayer Instance => _instance ?? (_instance = new CriAtomPreviewPlayer());
	
	CriAtomExPlayer player;
	CriAtomExAcb currentAcb;
	CriAtomExVoicePool voicePool;
	CriAtomExPlayback playback;
	CriAtomDbas dbas;
	GCHandle acfHandle;

	public CriAtomExPlayer Player => player;
	
	private CriAtomPreviewPlayer(){
		// initialize ADX and reserve required resources
		CriAtomCSharp.GetDefaultConfig(out var config);
		// by default ADX uses left-handed coodinate, Godot has right-handed coodinate
		config.atomEx.coordinateSystem = CriAtomEx.CoordinateSystem.RightHanded;
		CriAtomCSharp.Initialize(config);
		
		CriAtomDbas.SetDefaultConfig(out var dbasConfig);
		dbas = new CriAtomDbas(dbasConfig);

		CriAtomExVoicePool.SetDefaultConfigForStandardVoicePool(out var poolConfig);
		poolConfig.playerConfig.streamingFlag = true;
		poolConfig.playerConfig.maxSamplingRate = 96000;
		voicePool = CriAtomExVoicePool.AllocateStandardVoicePool(poolConfig);
		
		CriAtomExPlayer.SetDefaultConfig(out var playerConfig);
		player = new CriAtomExPlayer(playerConfig);
	}

	public void Dispose()
	{
		player?.Dispose();
		currentAcb?.Dispose();
		voicePool?.Dispose();
		dbas?.Dispose();
		CriAtomCSharp.Finalize();
		if(acfHandle.IsAllocated)
			acfHandle.Free();
	}
	
	public void LoadAcb(ArgString path, ArgString awbPath){
		currentAcb?.Dispose();
		currentAcb = CriAtomExAcb.LoadAcbFile(null, path, null, awbPath);
	}

	public void LoadAcf(ArgString path) =>
		CriAtomEx.RegisterAcfFile(null, path);
	
	public System.Collections.Generic.IEnumerable<CriAtomEx.CueInfo> GetCurrentCueInfoList(){
		for(int i=0; i < currentAcb.GetNumCues(); i++){
			currentAcb.GetCueInfoByIndex(i, out var info);
			yield return info;
		}
	}

	public CriAtomExPlayback Play(int id, bool stop = true){
		if (currentAcb == null) return new CriAtomExPlayback();
		if (stop) player.Stop();
		player.SetCueId(currentAcb, id);
		player.ResetParameters();
		player.SetPanType(CriAtomEx.PanType.Pan3d);
		playback = player.Start();
		return playback;
	}
	
	public void Stop(){
		player.Stop();
	}
	
	public void Pause(bool pause){
		player.Pause(pause);
	}
	
	public void SetAisacControlByName(string name, float val){
		player.SetAisacControlByName(name, val);
		player.Update(playback);
	}
	
	public void SetNextBlockIndex(int index){
		playback.SetNextBlockIndex(index);
	}
}
