using UnityEngine;
using System.Collections;

public class PlayerRevivalSphere : MonoBehaviour
{
	public float freeTime = 1f; 
	public float initialSpeed = 1f;
	public float damping = 0.1f;
	public float maxSpeed = 5f;
	private Vector3 velocity;

	public GameObject target { get; set; }
	private bool isFree_ = true;

	void Start()
	{
		velocity = Random.onUnitSphere * initialSpeed;
		StartCoroutine(MoveToPlayer());
	}

	void Update()
	{
		if (!isFree_ && target) {
			var to = (target.transform.localPosition - transform.localPosition).normalized;
			var targetVelocity = maxSpeed * to;
			velocity += (targetVelocity - velocity) * damping;
		}

		transform.localPosition += velocity * Time.deltaTime;
	}

	IEnumerator MoveToPlayer()
	{
		yield return new WaitForSeconds(freeTime);
		isFree_ = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		collision.transform.SendMessage("OnItem", Parameters.Items.RevivalSphere, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
