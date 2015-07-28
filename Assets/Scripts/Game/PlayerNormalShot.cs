using UnityEngine;
using System.Collections.Generic;

public class PlayerNormalShot : PlayerShot
{
	public GameObject shotPrefab;
	public int cost = 10;
	public int attack = 10;
	public float shotSpeed = 3f;
	public int rate = 5;
	private int frameCount_ = 0;

	protected override void OnPressed()
	{
	}

	protected override void OnPressing()
	{
		if (IsDead()) return;
		if (frameCount_ % rate == 0) { 
			Shot();
		}
		++frameCount_;
	}

	protected override void OnReleased()
	{
	}

	void Shot()
	{
		if (Use(cost)) {
			var shot = Instantiate(shotPrefab, transform.position, transform.rotation) as GameObject;
			shot.transform.parent = GlobalObjects.localStage.transform;
			var bullet = shot.GetComponent<PlayerBullet>();
			if (bullet) {
				bullet.attack = attack;
				bullet.velocity = transform.forward * shotSpeed;
			}
			Sound.Play("PlayerShot");
		}
	}
}
