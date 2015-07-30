using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
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
		if (meshCollider_) {
			meshCollider_.sharedMesh = meshFilter_.sharedMesh;
		} else {
			meshCollider_ = GetComponent<MeshCollider>();
		}
	}
}
