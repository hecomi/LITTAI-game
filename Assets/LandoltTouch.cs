using UnityEngine;
using System.Collections;

public class LandoltTouch : MonoBehaviour
{
	public Color normalColor = Color.green;
	public Color touchedColor = Color.blue;

	private Renderer renderer_;

	void Start()
	{
		renderer_ = GetComponent<Renderer>();
	}

	void OnTouchStart(Vector2 pos)
	{
		renderer_.material.color = touchedColor;
	}

	void OnTouchMove(Vector2 pos)
	{
	}

	void OnTouchEnd(Vector2 pos)
	{
		renderer_.material.color = normalColor;
	}
}
