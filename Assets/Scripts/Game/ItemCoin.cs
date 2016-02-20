using UnityEngine;
using System.Collections;

public class ItemCoin : MonoBehaviour
{
	public Parameters.Items type;
	public GameObject getEffectPrefab;
	public GameObject scoreSphere;
	public int scoreSphereNum;

	void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.SendMessage("OnItem", type, SendMessageOptions.DontRequireReceiver);
		if (getEffectPrefab) {
			var effect = Instantiate(getEffectPrefab);
			effect.transform.position = transform.position;
			effect.transform.SetParent(GlobalObjects.worldStage.transform);
		}
		if (scoreSphere) {
			for (int i = 0; i < scoreSphereNum; ++i) {
				var obj = Instantiate(scoreSphere);
				obj.transform.position = transform.position;
				obj.transform.SetParent(GlobalObjects.localStage.transform);
			}
		}
		Sound.Play("GetCoin");
		Destroy(gameObject);
	}
}
