using UnityEngine;
using System.Collections;

public class PlayerStatusUI : MonoBehaviour
{
	public UiGauge hpGauge;
	public UiGauge enGauge;
	public GameObject deadPanel;

	public float hp
	{
		get { return hpGauge.val; }
		set { hpGauge.val = value; }
	}

	public float en 
	{
		get { return enGauge.val; }
		set { enGauge.val = value; }
	}

	public bool dead
	{
		get { return deadPanel.activeSelf; }
		set { deadPanel.SetActive(value); }
	}
}
