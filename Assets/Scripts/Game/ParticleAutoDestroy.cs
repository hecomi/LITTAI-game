using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour
{
	private float lifeTime_;
	private float elapsedTime_;

	void Start()
	{
		var particle = GetComponent<ParticleSystem>();
		lifeTime_ = particle.duration + particle.startLifetime;
	}

	void Update()
	{
		elapsedTime_ += Time.deltaTime;
		if (elapsedTime_ > lifeTime_) {
			Destroy(gameObject);
		}
	}
}
