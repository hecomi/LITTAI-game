using UnityEngine;
using System.Collections;

public class EnemyPlayerCollision : MonoBehaviour
{
	public int attack = 100;
	public bool isDead = true;

	void OnCollisionEnter(Collision collision)
	{
		var tag = collision.transform.tag;
		if (tag == "Player") {
			collision.transform.SendMessage("OnAttacked", attack, SendMessageOptions.DontRequireReceiver);
			if (isDead) {
				SendMessage("OnDead", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
