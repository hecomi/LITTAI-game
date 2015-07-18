using UnityEngine;
using System.Collections;

public class GlobalObjects : MonoBehaviour
{
	static public GameObject localStage;
	static public GameObject worldStage;

	void Start()
	{
		localStage = GameObject.FindWithTag("Local Stage");
		worldStage = GameObject.FindWithTag("World Stage");
	}
}
