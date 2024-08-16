using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;

[DefaultExecutionOrder(100)]
public class SampleChannelsView : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform listParent;

    List<GameObject> gaugeList = new List<GameObject>();

    System.IDisposable disposable;

    void Start()
    {
        disposable = CriAtomExAsr.BusFilterCallbackByName["MasterOut"].Post.WithCopiedPcm().RegisterListener(data => {
            if(gaugeList.Count != data.Length)
            {
                foreach (var obj in gaugeList)
                    Destroy(obj);
                gaugeList.Clear();
                for(int i = 0; i < data.Length; i++)
                {
                    var newObj = Instantiate(prefab, listParent);
                    newObj.GetComponentInChildren<UnityEngine.UI.Text>().text = $"ch{i}";
                    gaugeList.Add(newObj);
                }
            }
			for (int i = 0; i < data.Length; i++)
			{
                if (gaugeList[i] == null) return;
                gaugeList[i].GetComponentInChildren<UnityEngine.UI.Image>().fillAmount = Mathf.InverseLerp(-50, 0, 20 * Mathf.Log10(Mathf.Abs(data[i][0])));
			}
		});
    }

	private void OnDestroy()
	{
        disposable?.Dispose();
	}
}
