using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour
{
	static public Vector2 MinPos  = new Vector2(-1f, -1f);
	static public Vector2 MaxPos  = new Vector2(1f, 1f);
	static public float PosMarginX = 1.0f;
	static public float PosMarginY = 0.2f;

	static public bool IsInArea(Vector2 p)
	{
		return 
			p.x > MinPos.x &&
		    p.x < MaxPos.x &&
		    p.y > MinPos.y &&
		    p.y < MaxPos.y;
	}

	static public bool IsInArea(Vector3 p)
	{
		return 
			p.x > MinPos.x &&
		    p.x < MaxPos.x &&
		    p.z > MinPos.y &&
		    p.z < MaxPos.y;
	}

	static public bool IsInMarginArea(Vector2 p)
	{
		return 
			p.x > MinPos.x - PosMarginX &&
		    p.x < MaxPos.x + PosMarginX &&
		    p.y > MinPos.y - PosMarginY &&
		    p.y < MaxPos.y + PosMarginY;
	}

	static public bool IsInMarginArea(Vector3 p)
	{
		return 
			p.x > MinPos.x - PosMarginX &&
		    p.x < MaxPos.x + PosMarginX &&
		    p.z > MinPos.y - PosMarginY &&
		    p.z < MaxPos.y + PosMarginY;
	}
}
