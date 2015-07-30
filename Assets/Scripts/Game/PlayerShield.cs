using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
	public int attack = 0;
	public float damping = 0.5f;
	public GameObject hitEffect;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Enemy")
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

	void OnAttacked(int attack)
	{
		var player = GetComponentInParent<Player>();
		player.SendMessage("OnAttacked", attack * damping, SendMessageOptions.DontRequireReceiver);
	}
}