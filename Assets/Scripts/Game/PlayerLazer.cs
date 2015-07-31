using UnityEngine;
using System.Collections;

public class PlayerLazer : MonoBehaviour
{
	public int attack = 0;
	public GameObject hitEffect;

	void OnTriggerEnter(Collider collider)
	{
		Attack(collider);
	}

	void OnTriggerStay(Collider collider)
	{
		Attack(collider);
	}

	void Attack(Collider collider)
	{
		if (hitEffect) {
			var effect = Instantiate(hitEffect) as GameObject;
			effect.transform.position = collider.transform.position;
			effect.transform.parent = transform.parent;
		}
		collider.transform.SendMessage("OnAttacked", attack, SendMessageOptions.DontRequireReceiver);
	}
}