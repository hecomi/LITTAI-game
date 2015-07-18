using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public bool isActivated = false;
	public GameObject deadEffect;
	public int hp = 100;

	void OnActivated()
	{
		isActivated = true;
	}

	void OnAttacked(int attack)
	{
		hp -= attack;
		if (hp <= 0) OnDead();
	}

	void OnDead()
	{
		if (deadEffect) {
			var effect = Instantiate(deadEffect) as GameObject;
			effect.transform.position = transform.position;
			effect.transform.parent = transform.parent;
		}
		Destroy(gameObject);
	}

	void Update()
	{
		if (!isActivated) return;

		if (!Stage.IsInMarginArea(transform.localPosition)) {
			Destroy(gameObject);
		}
	}
}
