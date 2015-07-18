using UnityEngine;
using System.Collections;

public class StopPlayerParticleEffectWhenDead : MonoBehaviour
{
	private ParticleSystem particle_;
	private float emissionRate_;

	void Start()
	{
		particle_ = GetComponent<ParticleSystem>();
		emissionRate_ = particle_.emissionRate;
	}

	void OnDead()
	{
		particle_.emissionRate = 0;
	}

	void OnRevival()
	{
		particle_.emissionRate = emissionRate_;
	}
}
