using UnityEngine;
using System.Collections;

public class TestBomb : MonoBehaviour
{
	public GameObject bombPrefab;

	void OnTouchStart()
	{
		var bomb = Instantiate(bombPrefab) as GameObject;
		bomb.transform.position = transform.position;
		bomb.transform.SetParent(transform.parent);
	}
}
