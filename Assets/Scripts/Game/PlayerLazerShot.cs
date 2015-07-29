using UnityEngine;
using System.Collections;

public class PlayerLazerShot : PlayerShot
{
	public GameObject lazerPrefab;
	private GameObject lazer_;
	public int cost = 50;
	public int attack = 10;

	protected override void OnPressed()
	{
	}

	protected override void OnPressing()
	{
		if (IsDead()) {
			Deactivate();
			return;
		}
		if (Use(cost)) {
			Activate();
		} else {
			Deactivate();
		}
	}

	protected override void OnReleased()
	{
		Deactivate();
	}

	void CreateLazer()
	{
		if (!lazer_) {
			lazer_ = Instantiate(lazerPrefab);
			lazer_.transform.position = transform.position;
			lazer_.transform.rotation = transform.rotation;
			lazer_.transform.SetParent(transform);
			var lazer = lazer_.GetComponentInChildren<PlayerLazer>();
			if (lazer) {
				lazer.attack = attack;
			}
		}
	}

	void Activate()
	{
		CreateLazer();
		lazer_.SetActive(true);
	}

	void Deactivate()
	{
		CreateLazer();
		lazer_.SetActive(false);
	}
}
