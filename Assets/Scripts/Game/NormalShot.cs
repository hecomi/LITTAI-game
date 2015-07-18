using UnityEngine;
using System.Collections;

public class NormalShot : MonoBehaviour
{
	public ShotPower shotPower;
	public GameObject shotPrefab;
	public float minAttack = 10;
	public float maxAttack = 50;
	public float minScale  = 1f;
	public float maxScale  = 2f;
	public float shotSpeed = 3f;

	public int chargedPower = 0;
	public int maxPower = 300;
	public int chargeRate = 10;

	void Update()
	{
		if (Input.GetButtonDown("Fire1")) {
			shotPower.refCount += 1;
		}
		if (Input.GetButton("Fire1")) {
			if (chargedPower < maxPower && shotPower.Use(chargeRate)) {
				chargedPower += chargeRate;
			}
		}
		if (Input.GetButtonUp("Fire1")) {
			shotPower.refCount -= 1;
			Shot();
		}
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
