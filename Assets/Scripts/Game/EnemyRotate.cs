using UnityEngine;
using System.Collections;

public class EnemyRotate : MonoBehaviour
{
	public float offsetTime = 2f;
	public float stopTime   = 10f;
	public float rotationSpeed = 180f;
	bool isRotating_ = false;

	void Start()
	{
		StartCoroutine(Wait());
		StartCoroutine(Stop());
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(offsetTime);
		isRotating_ = true;
	}

	IEnumerator Stop()
	{
		yield return new WaitForSeconds(stopTime);
		isRotating_ = false;
	}

	void Update()
	{
		if (isRotating_) {
			transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
		}
	}
}
