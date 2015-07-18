using UnityEngine;
using System.Collections;

public class DestroyAtStart : MonoBehaviour
{
	void Start()
	{
		Destroy(gameObject);
	}
}
