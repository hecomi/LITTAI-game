using UnityEngine;
using System.Collections;

public class ScoreSphere : MonoBehaviour
{
	public int score;
	public Vector3 velocity;
	public float initialSpeed = 1f;
	public float targetSpeed = 1f;
	public float damping = 0.2f;
	private int frameCount_ = 0;

	void Start()
	{
		velocity = Random.onUnitSphere * initialSpeed;
	}

	void Update()
	{
		if (frameCount_ > 30) {
			var target = LandoltManager.GetFirst();
			var targetPos = GlobalObjects.localStage.transform.position + Vector3.back * 1.5f;
			if (target) {
				targetPos = target.transform.position;
			}
			var dir = (targetPos - transform.position).normalized; 
			velocity += (dir * targetSpeed - velocity) * damping;
		}
		if (frameCount_ > 120) {
			Destroy(gameObject);
		}
		transform.position += velocity * Time.deltaTime;
		++frameCount_;
	}

	void OnCollisionEnter(Collision collision)
	{
		collision.transform.SendMessage("OnScoreGet", score, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
