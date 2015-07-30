using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestButtonClick : MonoBehaviour {

	Button button;
	public GameObject player;
	Ranking ranking;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button> ();
		button.onClick.AddListener(() =>OnClick ());

		ranking = GameObject.Find ("RankingCam").GetComponent<Ranking> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		var score = (int)(Random.value * 500);
		ranking.post (score, player);
	}
}
