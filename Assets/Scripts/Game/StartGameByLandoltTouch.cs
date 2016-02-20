using UnityEngine;
using System.Collections;

public class StartGameByLandoltTouch : MonoBehaviour
{
	private bool isTouching_;
	private int touchCount_ = 0;
	public int touchDuration = 60;
	public GameObject startEffect;

	void Update()
	{
		while (GameSequence.IsGameStarted()) return;

		if (isTouching_) ++touchCount_;
		if (touchCount_ > touchDuration) {
			GameSequence.StartGame();
			var obj = Instantiate(startEffect);
			obj.transform.position = transform.position;
			Destroy(this);
		}
	}

	void OnTouchStart()
	{
		isTouching_ = true;
	}

	void OnTouchEnd()
	{
		isTouching_ = false;
	}
}
