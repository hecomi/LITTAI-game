using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreChanger : MonoBehaviour
{
	Text text;

	void Start()
	{
		text = GetComponent<Text>();
	}

	void Update()
	{
		text.text = Score.Get().ToString();
	}
}
