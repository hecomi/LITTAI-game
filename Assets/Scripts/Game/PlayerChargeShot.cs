using UnityEngine;
using System.Collections;

public class PlayerChargeShot : PlayerShot
{
	public GameObject shotPrefab;
	public float minAttack = 10;
	public float maxAttack = 50;
	public float minScale  = 1f;
	public float maxScale  = 2f;
	public float shotSpeed = 3f;

	public int chargedPower = 0;
	public int maxPower = 300;
	public int chargeRate = 10;
	private int chargeCount = 0;

	protected override void OnPressed()
	{
	}

	protected override void OnPressing()
	{
		if (IsDead()) return;
		++chargeCount;
		if (chargedPower < maxPower && CanUse(chargeRate)) {
			chargedPower += chargeRate;
		}
	}

	protected override void OnReleased()
	{
		if (IsDead()) return;
		Shot();
		chargeCount = 0;
	}


	void Shot()
	{
		var shot = Instantiate(shotPrefab, transform.position, transform.rotation) as GameObject;
		shot.transform.parent = GlobalObjects.localStage.transform;
		var ratio = 1f * chargedPower / maxPower;
		shot.transform.localScale *= minScale * (1 - ratio) + maxScale * ratio; 
		var bullet = shot.GetComponent<PlayerBullet>();
		if (bullet) {
			bullet.attack = (int)(minAttack * (1f - ratio) + maxAttack * ratio);
			bullet.velocity = transform.forward * shotSpeed;
		}
		chargedPower = 0;
	}
}