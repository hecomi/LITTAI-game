using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEffect : MonoBehaviour
{
	public GameObject deadEffectPrefab;
	public GameObject revivalEffectPrefab;
	public float blinkTime = 0.5f;
	private Coroutine blinkCroutine_;
	private int frameCount_ = 0;

	void OnDestroy()
	{
		Ripple();
	}

	void Update()
	{
		++frameCount_;
		if (frameCount_ == 10) {
			Ripple();
		}
	}

	void Ripple()
	{
		var p = transform.localPosition;
		GlobalEffects.Riplle(p.x / 2 + 0.5f, p.z / 2 + 0.5f);
	}

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
		var effect = Instantiate(revivalEffectPrefab) as GameObject;
		effect.transform.position = transform.position;
		effect.transform.SetParent(transform);

		//StopCoroutine(blinkCroutine_);
		SetAlpha(1);
		SetColliderActive(true);

		Ripple();
	}

	void OnDead()
	{
		var effect = Instantiate(deadEffectPrefab) as GameObject;
		effect.transform.position = transform.position;
		effect.transform.SetParent(transform);

		//blinkCroutine_ = StartCoroutine(Blink());
		SetColliderActive(false);

		Ripple();
	}

	void SetAlpha(float alpha)
	{
		var renderer = GetComponent<Renderer>();
		var color = renderer.material.color;
		color.a = alpha;
		renderer.material.color = color;
	}

	void SetColliderActive(bool isActive)
	{
		var collider = GetComponent<Collider>();
		collider.enabled = isActive;
	}
}