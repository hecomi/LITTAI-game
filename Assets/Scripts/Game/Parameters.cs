using UnityEngine;
using System.Collections.Generic;

public static class Parameters
{
	private static readonly Dictionary<int, int> IdMap = new Dictionary<int, int>() {
		{ -1, 0   }, // dummy
		{ 10, 583 },
		{ 11, 415 },
		{ 14, 233 },
		{ 15, 203 },
	};

	public static int GetMarkerId(int hwId)
	{
		int markerId = 0;
		IdMap.TryGetValue(hwId, out markerId);
		return markerId;
	}


	public enum Items
	{
		RevivalSphere,
		Coin
	}
}
