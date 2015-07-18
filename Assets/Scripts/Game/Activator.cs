using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
	void OnTriggerEnter(Collider target)
	{
		target.transform.parent = GlobalObjects.localStage.transform;
		target.gameObject.SendMessage("OnActivated", SendMessageOptions.DontRequireReceiver);
	}
}
