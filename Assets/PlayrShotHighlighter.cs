using UnityEngine;
using System.Collections;

public class PlayrShotHighlighter : MonoBehaviour
{
	private Renderer renderer_;
	private TrailRenderer trail_;
	private Color originalColor_;
	private Color originalTrailColor_;
	public ParticleSystem[] particles;
	public Color shotColor;

	void Start()
	{
		renderer_ = GetComponent<Renderer>();
		trail_ = GetComponent<TrailRenderer>();
		originalColor_ = renderer_.material.color;

		SerialHandler.Pressed  += OnPressed;
		SerialHandler.Released += OnReleased;
	}

	void OnDestroy()
	{
		SerialHandler.Pressed  -= OnPressed;
		SerialHandler.Released -= OnReleased;
	}

	void OnPressed(int id)
	{
		renderer_.material.color = shotColor;
		if (trail_) trail_.material.color = shotColor;
		foreach (var particle in particles) {
			var alpha = particle.startColor.a;
			var color = shotColor;
			color.a = alpha;
			particle.startColor = color;
		}
	}

	void OnReleased(int id)
	{
		renderer_.material.color = originalColor_;
		if (trail_) trail_.material.color = originalColor_;
		foreach (var particle in particles) {
			var alpha = particle.startColor.a;
			var color = originalColor_;
			color.a = alpha;
			particle.startColor = color;
		}
	}
}
