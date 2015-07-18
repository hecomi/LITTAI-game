using UnityEngine;
using System.Collections;

public class EnemyPlayerCollision : MonoBehaviour
{
	public int attack = 100;
	public bool isDead = true;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player") {
			collision.transform.SendMessage("OnAttacked", attack, SendMessageOptions.DontRequireReceiver);
			if (isDead) {
				SendMessage("OnDead", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
