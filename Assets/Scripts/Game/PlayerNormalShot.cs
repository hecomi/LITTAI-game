using UnityEngine;
using System.Collections;

public class PlayerNormalShot : MonoBehaviour
{
	public bool isEmulation = false;
	public int hwId = 0;

	public ShotPower shotPower;
	public GameObject shotPrefab;
	public int cost = 10;
	public int attack = 10;
	public float shotSpeed = 3f;
	public int rate = 5;

	private bool isDead_ = false;
	private int frameCount_ = 0;

	void Start()
	{
		SerialHandler.Pressing += OnContinuousShot;
	}

	void OnDestroy()
	{
		SerialHandler.Pressing -= OnContinuousShot;
	}

	bool IsOwnEvent(int id)
	{
		// return id == hwId;
		return true;
	}

	void OnContinuousShot(int id)
	{
		if (!IsOwnEvent(id) || isDead_) return;
		if (frameCount_ % rate == 0) { 
			Shot();
		}
		++frameCount_;
	}

	void Shot()
	{
		if (shotPower.Use(cost)) {
			var shot = Instantiate(shotPrefab, transform.position, transform.rotation) as GameObject;
			shot.transform.parent = GlobalObjects.localStage.transform;
			var bullet = shot.GetComponent<PlayerBullet>();
			if (bullet) {
				bullet.attack = attack;
				bullet.velocity = transform.forward * shotSpeed;
			}
		}
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
