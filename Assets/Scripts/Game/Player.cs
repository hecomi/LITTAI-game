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
	public PlayerStatusUI ui;
	public ShotPower shotPower;

	void Update()
	{
		if (isDead) {
			deadElapsedTime_ += Time.deltaTime;
			if (deadElapsedTime_ >= deadWaitTime) {
				BroadcastMessage("OnRevival");
			}
		}

		ui.hp = 1f * hp / maxHp;
		ui.en = 1f * shotPower.power / shotPower.maxPower;
	}

	void OnAttacked(int damage)
	{
		if (isDead) return;
		hp -= damage;
		if (hp <= 0) BroadcastMessage("OnDead", SendMessageOptions.DontRequireReceiver);

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
