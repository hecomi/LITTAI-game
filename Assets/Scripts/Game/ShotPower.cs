using UnityEngine;
using System.Collections;

public class ShotPower : MonoBehaviour
{
	public int power = 0;
	public int maxPower = 1000;
	public int chargeSpeed = 3;
	public int refCount = 0;

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

	void Update()
	{
		if (refCount < 0) {
			refCount = 0; // bad hack
		}
		if (refCount == 0 && power < maxPower) {
			power += chargeSpeed;
			if (power > maxPower) power = maxPower;
		}
	}
}
