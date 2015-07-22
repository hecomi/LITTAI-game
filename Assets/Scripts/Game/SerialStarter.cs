using UnityEngine;
using System.Collections;

public class SerialStarter : MonoBehaviour
{
	void Start()
	{
		SerialHandler.Open();
	}
}
