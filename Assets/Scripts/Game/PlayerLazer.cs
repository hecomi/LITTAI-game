using UnityEngine;
using System.Collections;

public class PlayerLazer : MonoBehaviour
{
	public int attack = 0;
	public GameObject hitEffect;

	void OnCollisionEnter(Collision collision)
	{
		Attack(collision);
	}

	void OnCollisionStay(Collision collision)
	{
		Attack(collision);
	}

	void Attack(Collision collision)
	{
		if (hitEffect) {
			var effect = Instantiate(hitEffect) as GameObject;
			effect.transform.position = collision.transform.position;
			effect.transform.parent = transform.parent;
		}
		collision.transform.SendMessage("OnAttacked", attack, SendMessageOptions.DontRequireReceiver);
	}
}