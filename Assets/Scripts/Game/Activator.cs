using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
	void OnTriggerEnter(Collider target)
	{
		target.gameObject.SendMessage("OnActivated", SendMessageOptions.DontRequireReceiver);
	}
}
