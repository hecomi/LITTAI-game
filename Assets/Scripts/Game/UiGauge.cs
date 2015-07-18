using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiGauge : MonoBehaviour
{
	private RectTransform rect_;
	private float max_;

	public float val
	{
		get { return rect_.sizeDelta.x / max_; }
		set { rect_.sizeDelta = new Vector2(value * max_, rect_.sizeDelta.y); }
	}

	void Awake()
	{
		rect_ = GetComponent<RectTransform>();
		max_ = rect_.sizeDelta.x;
	}
}
