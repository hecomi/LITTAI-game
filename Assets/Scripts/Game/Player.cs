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
	public GameObject uiPrefab;
	private GameObject ui_;
	private PlayerStatusUI status_;
	private ShotPower shotPower_;

	void Start()
	{
		ui_ = Instantiate(uiPrefab) as GameObject;
		ui_.transform.SetParent(transform.parent);
		ui_.transform.position = transform.position;
		ui_.transform.rotation = transform.rotation;
		var motion = ui_.GetComponent<PlayerUiMotion>();
		if (motion) {
			motion.target = transform;
			status_ = motion.GetComponentInChildren<PlayerStatusUI>();
		}

		shotPower_ = GetComponent<ShotPower>();
	}

	void OnDestroy()
	{
		Destroy(ui_);
	}

	void Update()
	{
		if (isDead) {
			deadElapsedTime_ += Time.deltaTime;
			if (deadElapsedTime_ >= deadWaitTime) {
				BroadcastMessage("OnRevival");
			}
		}

		status_.hp = 1f * hp / maxHp;
		status_.en = 1f * shotPower_.power / shotPower_.maxPower;
		status_.dead = isDead;
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
