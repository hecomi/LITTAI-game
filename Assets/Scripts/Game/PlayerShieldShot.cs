using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class PlayerShieldShot : PlayerShot
{
	public GameObject shieldPrefab;
	private GameObject shield_;
	public float length = 0.5f;
	public int cost = 3;
	public int attack = 10;

	protected override void OnPattern(List<GameObject> edges)
	{
		if (edges.Count < 2) return;
		var dir = edges[1].transform.localPosition - edges[0].transform.localPosition;
		length = dir.magnitude;
		transform.localRotation = Quaternion.LookRotation(dir) * Quaternion.Euler(Vector3.up * Mathf.PI / 2);
	}

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

	void CreateShield()
	{
		if (!shield_) {
			shield_ = Instantiate(shieldPrefab);
			shield_.transform.position = transform.position;
			shield_.transform.rotation = transform.rotation;
			var scale = shield_.transform.localScale;
			scale.z = length * 2;
			shield_.transform.localScale = scale;
			shield_.transform.SetParent(transform);
			var shield = shield_.GetComponentInChildren<PlayerShield>();
			if (shield) {
				shield.attack = attack;
			}
		}
	}

	void Activate()
	{
		CreateShield();
		shield_.SetActive(true);
	}

	void Deactivate()
	{
		CreateShield();
		shield_.SetActive(false);
	}
}