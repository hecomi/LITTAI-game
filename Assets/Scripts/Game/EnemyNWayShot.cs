using UnityEngine;
using System.Collections;

public class EnemyNWayShot : MonoBehaviour
{
	public int nWay;
	public float centerAngle;
	public float spreadAngle;
	public int attack;
	public GameObject bulletPrefab;
	public float speed;

	public int offsetCount = 60;
	private int frameCount = 0;
	public int freq = 10;
	public int duration = 240;

	void Shot()
	{
		for (int i = 0; i < nWay; ++i) {
			var offset = transform.eulerAngles.y;
			var a = (centerAngle + (spreadAngle / nWay) * (i - nWay / 2f + 0.5f) + offset) * Mathf.Deg2Rad;
			var dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
			ShotTo(dir);
		}
		Sound.Play("EnemyShot");
	}

	void Update()
	{
		if (frameCount > offsetCount && frameCount <= offsetCount + duration) {
			if (frameCount % freq == 0) {
				Shot();
			}
		}
		++frameCount;
	}

	void ShotTo(Vector2 dir)
	{
		var bullet = Instantiate(bulletPrefab) as GameObject;
		bullet.transform.position = transform.position;
		bullet.transform.parent = transform.parent;
		var enemyBullet = bullet.GetComponent<EnemyBullet>();
		if (enemyBullet) {
			enemyBullet.velocity = new Vector3(dir.x, 0, dir.y) * speed;
			enemyBullet.attack = attack;
		}
	}
}
