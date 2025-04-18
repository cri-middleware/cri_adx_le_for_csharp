using UnityEngine;
using CriWare;
using Unity.Collections.LowLevel.Unsafe;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;

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

	// each config structs are Serializable
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
	CriAtomEx3dListener listener;

	CriAtomExAcb acb;

	void InitializeInstance()
	{
		if (CriAtomEx.IsInitialized()) return;

		CriAtomCSharp.GetDefaultConfig(out var config);
		config.atomEx = exConfig;
		config.asr = asrConfig;
		config.hcaMx = hcaMxConfig;
		CriAtomCSharp.Initialize(config);

#if UNITY_ANDROID && !UNITY_EDITOR
		// Setup the access for StreamingAssets Folder
		using AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		using AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		var activityRef = AndroidJNI.NewGlobalRef(activity.GetRawObject());
		CriFs.EnableAssetsAccessForPrefixANDROID(CriBaseCSharp.GetJavaVM(), activityRef, Application.streamingAssetsPath + "/");
		AndroidJNI.DeleteGlobalRef(activityRef);
#endif

		dbas = new CriAtomDbas();
		CriAtomExVoicePool.SetDefaultConfigForStandardVoicePool(out var poolConfig);
		voicePool = CriAtomExVoicePool.AllocateStandardVoicePool(poolConfig);

		listener = new CriAtomEx3dListener();

		unsafe {
			// use the NativeArray got from GetData<byte>() method if using TextAsset
			CriAtomEx.RegisterAcfData((nint)acfData.GetData<byte>().GetUnsafeReadOnlyPtr(), acfData.GetData<byte>().Length);
		}

		var acbFullpath = 
			System.IO.Path.Join(Application.streamingAssetsPath, acbPath);
		// StreamingAssets folder may hosted in http(Web build).
		if(acbFullpath.StartsWith("http")){
			var req = new UnityWebRequest(acbFullpath);
			acbFullpath = System.IO.Path.Join(Application.temporaryCachePath, acbPath);
			req.downloadHandler = new DownloadHandlerFile(acbFullpath);
			req.SendWebRequest().completed += (op) => {
				acb = CriAtomExAcb.LoadAcbFile(
					null, acbFullpath,
					null, null);
			};
			return;
		}

		acb = CriAtomExAcb.LoadAcbFile(
			null, acbFullpath,
			null, null);	
	}

	private void OnDisable()
	{
		listener?.Dispose();
		listener = null;
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
