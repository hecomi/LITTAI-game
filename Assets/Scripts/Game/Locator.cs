using UnityEngine;
using System.Collections;

public class Locator : MonoBehaviour
{
	public GameObject prefab;
	public bool isGlobal;

	void OnActivated()
	{
		if (prefab) {
			var obj = Instantiate(prefab);
			obj.transform.position = transform.position;
			obj.transform.rotation = transform.rotation;
			if (!isGlobal) {
				obj.transform.SetParent(GlobalObjects.localStage.transform);
			} else {
				obj.transform.SetParent(GlobalObjects.worldStage.transform);
			}
		}
		Destroy(gameObject);
	}
}
