using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public int hp = 1000;
	public int maxHp = 1000;
	public bool isDead = false;

	public float deadWaitTime = 3f;
	private float deadElapsedTime_ = 0f;

	public GameObject uiPrefab;
	private GameObject ui_;
	private PlayerStatusUI status_;
	private ShotCharge charge_;

	private int frameCount_ = 0;
	private Collider collider_;

	void Start()
	{
		charge_ = GetComponent<ShotCharge>();
		collider_ = GetComponent<Collider>();
	}

	void OnDestroy()
	{
		if (ui_) {
			Destroy(ui_);
		}
	}

	void Update()
	{
		if (isDead) {
			deadElapsedTime_ += Time.deltaTime;
			if (deadElapsedTime_ >= deadWaitTime) {
			}
		}

		if (status_) {
			status_.hp = 1f * hp / maxHp;
			status_.en = 1f * charge_.power / charge_.maxPower;
			status_.dead = isDead;
		}

		if (frameCount_ == 10) {
			ui_ = Instantiate(uiPrefab) as GameObject;
			ui_.transform.SetParent(transform.parent);
			ui_.transform.position = transform.position + Vector3.right * 0.2f;
			ui_.transform.rotation = transform.rotation;
			ui_.transform.localScale = Vector3.zero;
			var motion = ui_.GetComponent<PlayerUiMotion>();
			if (motion) {
				motion.target = transform;
				status_ = motion.GetComponentInChildren<PlayerStatusUI>();
			}
		} else if (frameCount_ > 10 && frameCount_ < 60) {
			ui_.transform.localScale += (Vector3.one - ui_.transform.localScale) * 0.1f;
		}

		++frameCount_;
	}

	void OnAttacked(int damage)
	{
		if (isDead) return;
		hp -= damage;
		if (hp <= 0) BroadcastMessage("OnDead", SendMessageOptions.DontRequireReceiver);
	}

	void OnDead()
	{
		isDead = true;
		deadElapsedTime_ = 0f;
		Sound.Play("PlayerDeath");
		if (collider_) {
			// collider_.enabled = false;
			Destroy(collider_);
		}

		StartCoroutine(Dead());
	}

	void OnRevival()
	{
		isDead = false;
		hp = maxHp;
		if (!collider_) {
			gameObject.AddComponent<MeshCollider>();
		}
		Sound.Play("PlayerRevival");
	}

	IEnumerator Dead()
	{
		if (deadWaitTime < 1f) deadWaitTime += 1f;
		yield return new WaitForSeconds(deadWaitTime - 1f);

		var landolt = LandoltManager.GetFirst();
		if (landolt) {
			landolt.EmitRevivalSphere(gameObject);
		}

		yield return new WaitForSeconds(1f);
		BroadcastMessage("OnRevival");
	}

	void OnItem(Parameters.Items item)
	{
		switch (item) {
			case Parameters.Items.RevivalSphere:
				break;
		}
	}
}
