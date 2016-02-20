using UnityEngine;
using System.Collections;

public class ItemRotate : MonoBehaviour
{
	public Vector3 speed = Vector3.forward * 180;

	void Update()
	{
		transform.Rotate(speed * Time.deltaTime);
	}
}
