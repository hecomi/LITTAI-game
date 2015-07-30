using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShotCharge : MonoBehaviour
{
	public int power = 0;
	public int maxPower = 1000;
	public int chargeSpeed = 3;
	private HashSet<GameObject> refs_ = new HashSet<GameObject>();


	public bool Use(int point)
	{
		if (!CanUse(point)) return false;
		power -= point;
		return true;
	}

	public bool CanUse(int point)
	{
		return power >= point;
	}

	public void Get(GameObject user)
	{
		refs_.Add(user);
	}

	public void Release(GameObject user)
	{
		if (refs_.Contains(user)) refs_.Remove(user);
	}

	void Update()
	{
		if (refs_.Count == 0 && power < maxPower) {
			power += chargeSpeed;
			if (power > maxPower) power = maxPower;
		}
	}
}
