using UnityEngine;
using CriWare;
using Unity.Collections.LowLevel.Unsafe;
using System;

[CreateAssetMenu(menuName = "AtomSamplePlayer")]
public class SampleAudioPlayer : ScriptableObject
{
	public static SampleAudioPlayer Instance { get; private set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void Initialize()
	{
		Instance = Resources.Load<SampleAudioPlayer>(nameof(SampleAudioPlayer));
		Instance.InitializeInstance();
	}

	// each condif structs are Serializable
	// see Assets/Resources/SampleAudioPlayer.asset
	[SerializeField]
	CriAtomEx.Config exConfig;
	[SerializeField]
	CriAtomExAsr.Config asrConfig;
	[SerializeField]
	CriAtomExHcaMx.Config hcaMxConfig;

	[SerializeField]
	TextAsset acfData;
	[SerializeField]
	string acbPath;
	[SerializeField]
	string awbPath;

	CriAtomDbas dbas;
	CriAtomExVoicePool voicePool;
	CriAtomExPlayer player;
	CriAtomEx3dListener listener;

	CriAtomExAcb acb;

	unsafe void InitializeInstance()
	{
		if (CriAtomEx.IsInitialized()) return;

		CriAtomCSharp.GetDefaultConfig(out var config);
		config.atomEx = exConfig;
		config.asr = asrConfig;
		config.hcaMx = hcaMxConfig;
		CriAtomCSharp.Initialize(config);

#if UNITY_ANDROID && !UNITY_EDITOR
		// Setup the Access for StreamingAssets Folder
		using AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		using AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		var activityRef = AndroidJNI.NewGlobalRef(activity.GetRawObject());
		CriFs.EnableAssetsAccessANDROID(CriBaseCSharp.GetJavaVM(), activityRef);
		AndroidJNI.DeleteGlobalRef(activityRef);
#endif

		dbas = new CriAtomDbas();
		CriAtomExVoicePool.SetDefaultConfigForStandardVoicePool(out var poolConfig);
		voicePool = CriAtomExVoicePool.AllocateStandardVoicePool(poolConfig);

		player = new CriAtomExPlayer();
		listener = new CriAtomEx3dListener();

		// use the NativeArray got from GetData<byte>() method if using TextAsset
		// NativeArray<byte>.AsSpan() mathod is available if using Unity 2022 or newer
		var acfSpan = new System.ReadOnlySpan<byte>(acfData.GetData<byte>().GetUnsafeReadOnlyPtr(), (int)acfData.dataSize);
		CriAtomEx.RegisterAcfData(acfSpan);

		var streamingAssetsPath =
#if !UNITY_ANDROID || UNITY_EDITOR
			Application.streamingAssetsPath;
#else
			"";
#endif
		acb = CriAtomExAcb.LoadAcbFile(
			null, System.IO.Path.Join(streamingAssetsPath, acbPath),
			null, System.IO.Path.Join(streamingAssetsPath, awbPath));
	}

	public CriAtomExPlayback Play(int cueId, CriAtomEx3dSource source)
	{
		player.Set3dSourceHn(source);
		player.SetCueId(acb, cueId);
		return player.Start();
	}

	private void OnDisable()
	{
		listener?.Dispose();
		listener = null;
		player?.Dispose();
		player = null;
		acb?.Dispose();
		acb = null;
		voicePool?.Dispose();
		voicePool = null;
		dbas?.Dispose();
		dbas = null;
		if (CriAtomEx.IsInitialized())
			CriAtomCSharp.Finalize();
	}

	private void Reset()
	{
		CriAtomEx.SetDefaultConfig(out exConfig);
		exConfig.threadModel = CriAtomEx.ThreadModel.MultiWithSonicsync;
		CriAtomExAsr.SetDefaultConfig(out asrConfig);
		CriAtomExHcaMx.SetDefaultConfig(out hcaMxConfig);
	}
}
