using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider))]
public class MeshColliderCreator : MonoBehaviour
{
	private MeshFilter   meshFilter_;
	private MeshCollider meshCollider_;

	void Start()
	{
		meshFilter_   = GetComponent<MeshFilter>();
		meshCollider_ = GetComponent<MeshCollider>();
	}

	void Update()
	{
		meshCollider_.sharedMesh = meshFilter_.sharedMesh;
	}
}
