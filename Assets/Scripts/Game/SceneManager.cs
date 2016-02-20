using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
	public KeyCode restartKey = KeyCode.Return;

	void Update()
	{
		if (Input.GetKeyDown(restartKey)) {
			GameSequence.Restart();
		}
	}
}
