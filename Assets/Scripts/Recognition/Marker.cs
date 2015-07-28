using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

[RequireComponent(typeof(PolygonCreator)), RequireComponent(typeof(ShotCharge))]
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

	private Vector3 prePos_ = Vector3.zero;
	private float currentAngle_ = 0f;
	private float preAngle_ = 0f;
	private Vector3 velocity_ = Vector3.zero;
	private float angleVelocity_ = 0f;
	private float lastTime_ = 0f;
	private float lastInterpTime_ = 0f;
	public int maxInterpDuration = 5;
	private int updateCount_ = 0;
	private int interpCount_ = 0;

	private Vector3 rawPos_ = Vector3.zero;
	public float filter = 0.5f;

	private bool isInitialized_ = false;
	private ShotCharge charge_;

	void Awake()
	{
		polygon_ = GetComponent<PolygonCreator>();
	}


	void Start()
	{
		rawPos_ = transform.position;
		charge_ = GetComponent<ShotCharge>();
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
		if (lostCount_ > 120) {
			MarkerManager.Remove(data_);
		}

		UpdateInterpolation();

		if (!isInitialized_) {
			transform.localPosition = rawPos_;
			transform.localRotation = Quaternion.AngleAxis(currentAngle_ * Mathf.Rad2Deg, Vector3.down);
			isInitialized_ = true;
		} else {
			transform.localPosition += (rawPos_ - transform.localPosition) * filter;
			var from = transform.localRotation;
			var to   = Quaternion.AngleAxis(currentAngle_ * Mathf.Rad2Deg, Vector3.down);
			transform.localRotation = Quaternion.Slerp(from, to, filter);
		}
	}


	public void Update(MarkerData data)
	{
		if (float.IsNaN(data.pos.x) || float.IsNaN(data.pos.y) || float.IsNaN (data.pos.z)) return;
		if (float.IsInfinity(data.pos.x) || float.IsInfinity(data.pos.y) || float.IsInfinity (data.pos.z)) return;

		data_ = data;
		rawPos_ = data.pos;
		polygon_.polygon = data.polygon;
		polygon_.indices = data.indices;
		UpdateEdge(data.id, data.edges);
		lostCount_ = 0;

		var dt = Time.time - lastTime_;
		if (dt == 0) return;

		velocity_ = (data.pos - prePos_) / dt;
		angleVelocity_ = (data.angle - preAngle_) / dt;
		currentAngle_ = preAngle_ = data.angle;
		prePos_ = rawPos_;
		lastTime_ = lastInterpTime_ = Time.time;
		++updateCount_;
		interpCount_ = 0;
	}


	void UpdateInterpolation()
	{
		if (updateCount_ <= 2 || interpCount_ > maxInterpDuration) return;
		if (Time.time == lastInterpTime_) return;

		var dt = Time.time - lastInterpTime_;
		rawPos_ += velocity_ * dt;
		currentAngle_ += angleVelocity_ * dt;
		if (currentAngle_ >  180) currentAngle_ -= 180;
		if (currentAngle_ < -180) currentAngle_ += 180;

		lastInterpTime_ = Time.time;
		++interpCount_;
	}


	public void UpdateEdge(int markerId, List<EdgeData> edges)
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
				var shots = edgeObj.GetComponentsInChildren<PlayerShot>();
				foreach (var shot in shots) {
					shot.charge = charge_;
					shot.id = markerId;
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
