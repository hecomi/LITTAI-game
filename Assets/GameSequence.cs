using UnityEngine;
using System.Collections;

public class GameSequence : MonoBehaviour
{
	public KeyCode rankingDebugKey = KeyCode.Alpha9; 

	void Update()
	{
		if (Input.GetKeyDown(rankingDebugKey)) {
			var markers = MarkerManager.GetMarkerObjects();
			if (markers.Count == 0) return;
			Ranking.Post(1000, markers[0]);
		}
	}
}
