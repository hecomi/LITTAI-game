using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	static private Score Instance;
	private int rawScore_ = 0;
	private int score_ = 0;
	public int dScore = 10;

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		score_ = Mathf.Min(score_ + dScore, rawScore_);
	}

	static public int Get()
	{
		return Instance.score_;
	}

	static public void Add(int value)
	{
		Instance.rawScore_ += value;
	}

	static public void Sub(int value)
	{
		Instance.rawScore_ -= value;
	}
}
