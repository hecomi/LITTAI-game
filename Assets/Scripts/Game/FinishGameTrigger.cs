using UnityEngine;
using System.Collections;

public class FinishGameTrigger : MonoBehaviour
{
	void OnActivated()
	{
		GameSequence.FinishGame();
		Destroy(gameObject);
	}
}
