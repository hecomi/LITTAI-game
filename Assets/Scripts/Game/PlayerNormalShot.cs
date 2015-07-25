using UnityEngine;
using System.Collections;

public class PlayerNormalShot : MonoBehaviour
{
	public bool isEmulation = false;
	public int hwId = 0;

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

	private bool isDead_ = false;
	private bool isCharging_ = false;

	private int chargeCount = 0;

	void Start()
	{
		SerialHandler.Pressed  += OnChargeStart;
		SerialHandler.Pressing += OnCharging;
		SerialHandler.Released += OnShot;
	}

	bool IsOwnEvent(int id)
	{
		// return id == hwId;
		return true;
	}

	void OnChargeStart(int id)
	{
		if (!IsOwnEvent(id) || isDead_ || isCharging_) return;
		Shot();
		isCharging_ = true;
		shotPower.refCount += 1;
	}

	void OnCharging(int id)
	{
		if (!IsOwnEvent(id) || isDead_ || !isCharging_) return;
		++chargeCount;
		if (chargeCount % 5 == 0) Shot();
		return;
		if (chargedPower < maxPower && shotPower.Use(chargeRate)) {
			chargedPower += chargeRate;
		}
	}

	void OnShot(int id)
	{
		if (!IsOwnEvent(id) || isDead_ || !isCharging_) return;
		isCharging_ = false;
		shotPower.refCount -= 1;
		//Shot();
		chargeCount = 0;
	}

	void OnDestroy()
	{
		if (isCharging_) shotPower.refCount -= 1;
		SerialHandler.Pressed  -= OnChargeStart;
		SerialHandler.Pressing -= OnCharging;
		SerialHandler.Released -= OnShot;
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

	void OnDead()
	{
		isDead_ = true;
	}

	void OnRevival()
	{
		isDead_ = false;
	}
}
