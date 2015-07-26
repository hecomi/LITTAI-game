using UnityEngine;

[System.Serializable]
public class LandoltData
{
	public int     id       = -1;
	public Vector2 pos      = Vector2.zero;
	public float   radius   = 1f;
	public float   width    = 1f;
	public float   height   = 1f;
	public float   angle    = 0f;
	public int     cnt      = 0;
	public bool    touched  = false;
	public Vector2 touchPos = Vector2.zero;
}
