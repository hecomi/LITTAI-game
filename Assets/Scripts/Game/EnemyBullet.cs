using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
	public int attack = 0;
	public Vector3 velocity;
	private int frameCount = 0;

	public GameObject hitEffect;


	void Update()
	{
		transform.position += velocity * Time.deltaTime;
		++frameCount;
		CheckBoundaryCondition();
	}


	void CheckBoundaryCondition()
	{
		if (!Stage.IsInMarginArea(transform.localPosition)) {
			Destroy(gameObject);
		}
	}


	void OnCollisionEnter(Collision collision)
	{
		if (hitEffect) {
			var effect = Instantiate(hitEffect) as GameObject;
			effect.transform.position = transform.position;
			effect.transform.parent = transform.parent;
		}
		collision.transform.SendMessage("OnAttacked", attack, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
