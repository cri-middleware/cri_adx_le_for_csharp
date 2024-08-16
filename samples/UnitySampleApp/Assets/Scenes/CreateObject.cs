using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
	[SerializeField]
	GameObject original;

	List<GameObject> instances = new List<GameObject>();

	public void SpawnOne() =>
		instances.Add(Instantiate(original));

	private void Awake() => SpawnOne();

	public void DestroyAll()
	{
		foreach (var obj in instances)
			Destroy(obj);
		instances.Clear();
	}

	private void OnDestroy() => DestroyAll();
}
