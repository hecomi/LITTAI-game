using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class EdgeData
{
	public int id;
	public Vector3 pos;
	public Vector3 dir;
}


[System.Serializable]
public class PatternData
{
	public int pattern;
	public List<int> ids = new List<int>();
}


[System.Serializable]
public class MarkerData
{
	public int            id;
	public Vector3        pos;
	public float          angle;
	public float          size;
	public int            frameCount;
	public List<Vector3>  polygon     = new List<Vector3>();
	public List<int>      indices     = new List<int>();
	public List<EdgeData> edges       = new List<EdgeData>();
	public List<PatternData> patterns = new List<PatternData>();
}
