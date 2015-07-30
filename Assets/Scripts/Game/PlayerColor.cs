using UnityEngine;
using System.Collections;

public class PlayerColor : MonoBehaviour
{
	private int markerId_ = 0;

	private Renderer renderer_;
	private TrailRenderer trail_;
	private Color originalColor_;
	private Color originalTrailColor_;
	public ParticleSystem[] particles;
	public Color shotColor;
	public Color emptyColor;
	public Color deadColor;
	private bool isDead_ = false;

	private ShotCharge charge_;

	void Start()
	{
		renderer_ = GetComponent<Renderer>();
		trail_ = GetComponent<TrailRenderer>();
		originalColor_ = renderer_.material.color;

		SerialHandler.Pressed  += OnPressed;
		SerialHandler.Pressing += OnPressing;
		SerialHandler.Released += OnReleased;

		var marker = GetComponentInParent<Marker>();
		if (marker) {
			markerId_ = marker.data.id; 
		}

		charge_ = GetComponent<ShotCharge>();
		if (!charge_) {
			charge_ = GetComponentInParent<ShotCharge>();
		}
	}

	void OnDestroy()
	{
		SerialHandler.Pressed  -= OnPressed;
		SerialHandler.Pressing -= OnPressing;
		SerialHandler.Released -= OnReleased;
	}

	void OnDead()
	{
		if (trail_) {
			var color = trail_.material.color;
			color.a = originalColor_.a * 0.1f;
			trail_.material.color = color;
		}
		renderer_.material.color = deadColor;
		isDead_ = true;
	}

	void OnRevival()
	{
		if (trail_) {
			var color = trail_.material.color;
			color.a = originalColor_.a;
			trail_.material.color = color;
		}
		renderer_.material.color = originalColor_;
		isDead_ = false;
	}

	void OnPressed(int hwId)
	{
		if (Parameters.GetMarkerId(hwId) != markerId_) return;
		if (isDead_) return;

		renderer_.material.color = shotColor;
		if (trail_) trail_.material.color = shotColor;
		foreach (var particle in particles) {
			var alpha = particle.startColor.a;
			var color = shotColor;
			color.a = alpha;
			particle.startColor = color;
		}
	}

	void OnPressing(int hwId)
	{
		if (Parameters.GetMarkerId(hwId) != markerId_) return;
		if (isDead_) return;

		if (charge_.power < charge_.maxPower * 0.03) {
			renderer_.material.color = emptyColor;
			if (trail_) trail_.material.color = emptyColor;
			foreach (var particle in particles) {
				var alpha = particle.startColor.a;
				var color = emptyColor;
				color.a = alpha;
				particle.startColor = color;
			}
		}
	}

	void OnReleased(int hwId)
	{
		if (Parameters.GetMarkerId(hwId) != markerId_) return;
		if (isDead_) return;

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
