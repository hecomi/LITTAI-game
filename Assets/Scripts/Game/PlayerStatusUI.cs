using UnityEngine;
using System.Collections;

public class PlayerStatusUI : MonoBehaviour
{
	public UiGauge hpGauge;
	public UiGauge enGauge;
	public GameObject deadPanel;
	public GameObject emptyLabel;

	public float hp
	{
		get { return hpGauge.val; }
		set { hpGauge.val = value; }
	}

	public float en 
	{
		get { return enGauge.val; }
		set { enGauge.val = value; empty = (value <= 0.03f); }
	}

	public bool dead
	{
		get { return deadPanel.activeSelf; }
		set { deadPanel.SetActive(value); }
	}

	public bool empty
	{
		get { return emptyLabel.activeSelf; }
		set { emptyLabel.SetActive(value); }
	}
}
