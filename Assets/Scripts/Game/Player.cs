using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public int hp = 1000;
	public int maxHp = 1000;
	public bool isDead = false;

	public float deadWaitTime = 3f;
	private float deadElapsedTime_ = 0f;

	public GameObject hitEffectPrefab;

	void Update()
	{
		if (isDead) {
			deadElapsedTime_ += Time.deltaTime;
			if (deadElapsedTime_ >= deadWaitTime) {
				OnRevival();
			}
		}
	}

	void OnAttacked(int damage)
	{
		if (isDead) return;
		hp -= damage;
		if (hp <= 0) OnDead();

		if (hitEffectPrefab) {
			var effect = Instantiate(hitEffectPrefab) as GameObject;
			effect.transform.position = transform.position;
			effect.transform.parent = transform;
		}
	}

	void OnDead()
	{
		isDead = true;
		deadElapsedTime_ = 0f;
	}

	void OnRevival()
	{
		isDead = false;
		hp = maxHp;
	}
}
