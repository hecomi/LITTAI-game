using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

[RequireComponent(typeof(PolygonCreator))]
public class Marker : MonoBehaviour
{
	private PolygonCreator polygon_;
	private MarkerData data_;
	public MarkerData data
	{
		get { return data_; }
	}
	private int lostCount_ = 0;

	public GameObject edgePrefab;
	private Dictionary<int, GameObject> edges_ = new Dictionary<int, GameObject>();


	void Awake()
	{
		polygon_ = GetComponent<PolygonCreator>();
	}


	void OnDestroy()
	{
		foreach (var edge in edges_) {
			Destroy(edge.Value);
		}
	}


	void Update()
	{
		++lostCount_;
		if (lostCount_ > 180) {
			MarkerManager.Remove(data_);
		}
	}


	public void Update(MarkerData data)
	{
		if (float.IsNaN(data.pos.x) || float.IsNaN(data.pos.y) || float.IsNaN (data.pos.z)) return;
		if (float.IsInfinity(data.pos.x) || float.IsInfinity(data.pos.y) || float.IsInfinity (data.pos.z)) return;

		data_ = data;
		transform.localPosition = data.pos;
		transform.localRotation = Quaternion.AngleAxis(data.angle * Mathf.Rad2Deg, Vector3.down);
		polygon_.polygon = data.polygon;
		polygon_.indices = data.indices;
		UpdateEdge(data.edges);
		lostCount_ = 0;
	}


	public void UpdateEdge(List<EdgeData> edges)
	{
		var updatedMap = new Dictionary<int, bool>();
		foreach (var data in edges_) {
			updatedMap.Add(data.Key, false);
		}

		foreach (var edge in edges) {
			GameObject edgeObj;
			var id = edge.id;
			if (edges_.ContainsKey(id)) {
				edgeObj = edges_[id];
				updatedMap[id] = true;
			} else {
				edgeObj = Instantiate(edgePrefab) as GameObject;
				edgeObj.transform.SetParent(transform);
				var shots = edgeObj.GetComponentsInChildren<PlayerNormalShot>();
				foreach (var shot in shots) {
					shot.shotPower = GetComponent<ShotPower>();
				}
				edges_.Add(id, edgeObj);
			}
			edgeObj.transform.localPosition = edge.pos; 
			var dir = edge.dir.normalized;
			dir.z *= -1;
			edgeObj.transform.localRotation = Quaternion.LookRotation(dir);
		}

		foreach (var data in updatedMap) {
			var id = data.Key;
			if (!data.Value) {
				Destroy(edges_[id]);
				edges_.Remove(id);
			}
		}
	}
}
