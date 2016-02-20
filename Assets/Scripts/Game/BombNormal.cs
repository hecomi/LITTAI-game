using UnityEngine;
using System.Collections;

public class BombNormal : MonoBehaviour
{
	private bool isTouching_;
	private int touchCount_ = 0;
	public GameObject shotPrefab;
	public int cost = 1000;
	public int attack = 5;
	public float speed = 1f;
	public float interval = 0.5f;
	public int wave = 10;
	public int nWay = 20;
	public int touchDuration = 60;

	void Update()
	{
		if (!GameSequence.IsGameStarted()) return;

		if (isTouching_) ++touchCount_;
		if (touchCount_ == touchDuration) {
			StartCoroutine(Bomb());
		}
	}

	void OnTouchStart()
	{
		if (!GameSequence.IsGameStarted()) return;
		isTouching_ = true;
	}

	void OnTouchEnd()
	{
		if (!GameSequence.IsGameStarted()) return;
		isTouching_ = false;
		touchCount_ = 0;
	}

	IEnumerator Bomb()
	{
		Score.Sub(cost);
		for (var n = 0; n < wave; ++n) {
			for (var i = 0; i < nWay; ++i) {
				var offset = transform.eulerAngles.y;
				var a = ((360 / nWay) * (i - nWay / 2f + 0.5f) + offset) * Mathf.Deg2Rad;
				var dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
				ShotTo(dir);
			}
			Sound.Play("PlayerShot");
			yield return new WaitForSeconds(interval);
		}
	}

	void ShotTo(Vector2 dir)
	{
		var pos = transform.position + new Vector3(dir.x, 0, dir.y) * transform.localScale.x;
		var shot = Instantiate(shotPrefab, pos, transform.rotation) as GameObject;
		shot.transform.parent = GlobalObjects.localStage.transform;
		var bullet = shot.GetComponent<PlayerBullet>();
		if (bullet) {
			bullet.attack = attack;
			bullet.velocity = new Vector3(dir.x, 0, dir.y) * speed;
		}
	}
}
