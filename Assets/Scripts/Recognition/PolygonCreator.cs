using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class PolygonCreator : MonoBehaviour
{
	public float height = 1f;
	public int updateInterval = 30;
	private int frameCount = 0;

	private List<Vector3> polygon_ = new List<Vector3>();
	public List<Vector3> polygon 
	{
		get { return polygon_; }
		set { polygon_ = value; }
	}
	private List<int> indices_ = new List<int>();
	public List<int> indices
	{
		get { return indices_; }
		set { indices_ = value; }
	}
	private MeshFilter meshFilter_;

	void Start()
	{
		meshFilter_ = GetComponent<MeshFilter>();
		Test ();
	}

	void Update()
	{
		if (frameCount++ % updateInterval != 0) return;

		if (polygon_.Count == 0) return;

		int vertexNum = polygon_.Count();
		int indexNum  = indices_.Count();
		var vertices  = polygon_.Concat(polygon_).ToList();
		var indices   = indices_.Concat(indices_.ToArray().Reverse()).ToList();

		for (int i = 0; i < vertexNum * 2; ++i) {
			if (i < vertexNum) {
				vertices[i] += Vector3.up * height * 0.5f;
			} else {
				vertices[i] -= Vector3.up * height * 0.5f;
			}
		}

		for (int i = indexNum; i < indexNum * 2; ++i) {
			indices[i] += vertexNum;
		}

		for (int i = 0; i < vertexNum - 1; ++i) {
			var i1 = i;
			var i2 = i + 1;
			var i3 = i + vertexNum;
			var i4 = i + vertexNum + 1;
			indices.Add(i1);
			indices.Add(i3);
			indices.Add(i4);
			indices.Add(i1);
			indices.Add(i4);
			indices.Add(i2);
		}
		indices.Add(vertexNum - 1);
		indices.Add(vertexNum * 2 - 1);
		indices.Add(vertexNum);
		indices.Add(vertexNum - 1);
		indices.Add(vertexNum);
		indices.Add(0);

		foreach (var index in indices) {
			if (index < 0 || index >= vertices.Count) return;
		}

		try {
			var mesh = new Mesh();
			mesh.name = "Custom Mesh";
			mesh.vertices = vertices.ToArray();
			mesh.triangles = indices.ToArray();
			mesh.RecalculateNormals();
			meshFilter_.sharedMesh = mesh;
		} catch (System.Exception e) {
			Debug.LogError(e.Message);
		}
	}

	[ContextMenu("Test")]
	void Test()
	{
		polygon_.Clear();
		polygon_.Add(new Vector3(-0.18958333333333333f, 0f, 0.039583333333333304f) * 10);
		polygon_.Add(new Vector3(-0.21041666666666664f, 0f, 0.1645833333333333f) * 10);
		polygon_.Add(new Vector3(-0.18541666666666667f, 0f, 0.1791666666666667f) * 10);
		polygon_.Add(new Vector3(-0.18958333333333333f, 0f, 0.22083333333333333f) * 10);
		polygon_.Add(new Vector3(-0.10625000000000001f, 0f, 0.23541666666666672f) * 10);
		polygon_.Add(new Vector3(-0.09166666666666667f, 0f, 0.15208333333333335f) * 10);
		polygon_.Add(new Vector3(-0.17708333333333331f, 0f, 0.1333333333333333f) * 10);
		polygon_.Add(new Vector3(-0.16458333333333336f, 0f, 0.039583333333333304f) * 10);
		indices_.Add(7);
		indices_.Add(0);
		indices_.Add(1);
		indices_.Add(6);
		indices_.Add(7);
		indices_.Add(1);
		indices_.Add(5);
		indices_.Add(6);
		indices_.Add(1);
		indices_.Add(5);
		indices_.Add(1);
		indices_.Add(2);
		indices_.Add(5);
		indices_.Add(2);
		indices_.Add(3);
		indices_.Add(5);
		indices_.Add(3);
		indices_.Add(4);
	}
}
