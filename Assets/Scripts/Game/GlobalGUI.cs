using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlobalGUI : MonoBehaviour
{
	static GlobalGUI Instance;
	public Text title;
	public Text bgm;

	void Awake()
	{
		Instance = this;
	}

	static public void SetText(string text)
	{
		Instance.title.text = text;
	}

	static public void ShowBgmRefer(bool isShown)
	{
		Instance.bgm.enabled = isShown;
	}
}
