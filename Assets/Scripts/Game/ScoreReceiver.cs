using UnityEngine;
using System.Collections;

public class ScoreReceiver : MonoBehaviour
{
	public GameObject attackedEffectPrefab;


	void OnAttacked(int attack)
	{
		Score.Add(attack);
		GenerateEffect();
	}

	void OnScoreGet(int score)
	{
		Score.Add(score);
		GenerateEffect();
		Sound.Play("GetScore");
	}

	void GenerateEffect()
	{
		if (attackedEffectPrefab) {
			var effect = Instantiate(attackedEffectPrefab);
			effect.transform.SetParent(transform);
			effect.transform.position = transform.position;
		}
	}
}
