using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public bool isActivated = false;
	public GameObject deadEffect;
	public int hp = 100;
	public int score = 50;
	private int maxHp_;

	public GameObject enemyStatusUi;
	public GameObject[] items;
	public Vector3 uiOffset = Vector3.forward * 0.1f;
	private UiGauge hpGauge_;

	void Start()
	{
		var ui = Instantiate(enemyStatusUi) as GameObject;
		ui.transform.position = transform.position + uiOffset;
		ui.transform.SetParent(transform);
		maxHp_ = hp;
		hpGauge_ = ui.GetComponentInChildren<UiGauge>();
	}

	void OnActivated()
	{
		isActivated = true;
	}

	void OnAttacked(int attack)
	{
		hp -= attack;
		hpGauge_.val = 1f * hp / maxHp_;
		if (hp <= 0) OnDead();
	}

	void OnDead()
	{
		if (deadEffect) {
			var effect = Instantiate(deadEffect) as GameObject;
			effect.transform.position = transform.position;
			effect.transform.parent = transform.parent;
		}
		foreach (var item in items) {
			var obj = Instantiate(item) as GameObject;
			obj.transform.position = transform.position;
			obj.transform.SetParent(transform.parent);
		}
		Destroy(gameObject);
		Score.Add(score);
		Sound.Play("EnemyDeath");
	}

	void Update()
	{
		if (!isActivated) return;

		if (!Stage.IsInMarginArea(transform.localPosition)) {
			Destroy(gameObject);
		}
	}
}
