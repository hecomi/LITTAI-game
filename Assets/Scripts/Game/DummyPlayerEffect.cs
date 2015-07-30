using UnityEngine;
using System.Collections;

public class DummyPlayerEffect : MonoBehaviour
{
	public GameObject[] parts;
	public GameObject deadEffectPrefab;
	public GameObject revivalEffectPrefab;
	public float blinkTime = 0.5f;
	private Coroutine blinkCroutine_;

	IEnumerator Blink()
	{
		bool isShown = false;
		for (;;) {
			SetAlpha(isShown ? 0.3f : 0);
			yield return new WaitForSeconds(blinkTime / 2);
			isShown = !isShown;
		}
	}

	void OnRevival()
	{
		foreach (var part in parts) {
			var effect = Instantiate(revivalEffectPrefab) as GameObject;
			effect.transform.position = part.transform.position;
			effect.transform.SetParent(transform);
		}
		//StopCoroutine(blinkCroutine_);
		SetAlpha(1);
		SetColliderActive(true);

		var p = transform.localPosition;
		GlobalEffects.Riplle(p.x / 2 + 0.5f, p.z / 2 + 0.5f);
	}

	void OnDead()
	{
		foreach (var part in parts) {
			var effect = Instantiate(deadEffectPrefab) as GameObject;
			effect.transform.position = part.transform.position;
			effect.transform.SetParent(transform);
		}
		//blinkCroutine_ = StartCoroutine(Blink());
		SetColliderActive(false);

		var p = transform.localPosition;
		GlobalEffects.Riplle(p.x / 2 + 0.5f, p.z / 2 + 0.5f);
	}

	void SetAlpha(float alpha)
	{
		foreach (var part in parts) {
			var renderer = part.GetComponent<Renderer>();
			var color = renderer.material.color;
			color.a = alpha;
			renderer.material.color = color;
		}
	}

	void SetColor(Color color)
	{
		foreach (var part in parts) {
			var renderer = part.GetComponent<Renderer>();
			renderer.material.color = color;
		}
	}

	void SetColliderActive(bool isActive)
	{
		foreach (var part in parts) {
			var collider = part.GetComponent<Collider>();
			collider.enabled = isActive;
		}
	}
}
