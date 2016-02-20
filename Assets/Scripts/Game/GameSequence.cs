using UnityEngine;
using System.Collections;

public class GameSequence : MonoBehaviour
{
	static GameSequence Instance;
	static bool IsGameStarted_ = false;
	static bool IsGameReady_ = false;

	public MonoBehaviour[] makeActiveAtStartScripts;

	public KeyCode rankingDebugKey = KeyCode.Alpha9; 

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		if (Input.GetKeyDown(rankingDebugKey)) {
			var markers = MarkerManager.GetMarkerObjects();
			if (markers.Count == 0) return;
			Ranking.Post(1000, markers[0]);
		}
	}

	public static void Restart()
	{
		IsGameReady_ = IsGameStarted_ = false;
		Application.LoadLevel(0);
	}

	public static bool IsGameStarted()
	{
		return IsGameStarted_;
	}

	public static void StartGame()
	{
		if (IsGameStarted_ || IsGameReady_) return;
		Sound.Play("GameReady");
		IsGameReady_ = true;
		Instance.StartCoroutine(Instance.StartGameSequence());
	}

	public static void FinishGame()
	{
		Sound.Play("GameFinish");
		Instance.StartCoroutine(Instance.FinishGameSequence());
	}

	IEnumerator StartGameSequence()
	{
		GlobalGUI.SetText("Ready...");
		Score.Set(0);
		yield return new WaitForSeconds(5f);
		GlobalGUI.SetText("GO!!");
		Sound.Play("GameStart");
		yield return new WaitForSeconds(1f);
		IsGameStarted_ = true;

		foreach (var obj in Instance.makeActiveAtStartScripts) {
			obj.enabled = true;
		}

		GlobalGUI.SetText("GO!!");
		yield return new WaitForSeconds(1f);
		GlobalGUI.SetText("");
		yield return new WaitForSeconds(1f);
		Sound.Play("BGM");
	}

	IEnumerator FinishGameSequence()
	{
		GlobalGUI.SetText("Finish!");
		yield return new WaitForSeconds(3f);
		GlobalGUI.SetText("Your Score is...");
		yield return new WaitForSeconds(2f);
		GlobalGUI.SetText(Score.Get().ToString());
		yield return new WaitForSeconds(5f);

		var markers = MarkerManager.GetMarkerObjects();
		if (markers.Count == 0) {
			while (markers.Count == 0) {
				GlobalGUI.SetText("Please Put LITTAI button!");
				yield return new WaitForSeconds(1f);
				markers = MarkerManager.GetMarkerObjects();
			}
		}
		yield return new WaitForSeconds(1f);
		try {
			bool isHit = false;
			foreach (var marker in markers) {
				if (marker) {
					Ranking.Post(Score.Get(), markers[0].gameObject);
					isHit = true;
					break;
				}
			}
			Debug.LogError("No Marker...");
		} catch (System.Exception e) {
			Debug.LogError(e.Message);
		}

		GlobalGUI.SetText("Thank you for playing!");
		GlobalGUI.ShowBgmRefer(true);
		yield return new WaitForSeconds(6f);

		Restart();
	}
}
