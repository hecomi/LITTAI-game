using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
	public float length = 2.5f;
	public Camera gameCamera;

	void Update()
	{
		for (int i = 0; i < transform.childCount; ++i) {
			var child = transform.GetChild(i);
			var distanceZ = gameCamera.transform.position.z - child.position.z;
			if (Mathf.Abs(distanceZ) > length) {
				child.position += new Vector3(0, 0, 1) * Mathf.Sign(distanceZ) * length * 2;
			}
		}
	}
}
