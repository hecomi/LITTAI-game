using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

[RequireComponent(typeof(PolygonCreator)), RequireComponent(typeof(ShotCharge))]
public class Marker : MonoBehaviour
{
	[System.Serializable]
	public struct PatternPrefab
	{
		public int id;
		public GameObject prefab;
	}

	private PolygonCreator polygon_;
	private MarkerData data_;
	public MarkerData data
	{
		get { return data_; }
	}
	private int lostCount_ = 0;

	public GameObject edgePrefab;
	public List<PatternPrefab> patternPrefabs;
	private Dictionary<int, GameObject> edges_ = new Dictionary<int, GameObject>();
	private Dictionary<string, GameObject> patterns_ = new Dictionary<string, GameObject>();
	private Dictionary<string, int> patternsLostCount_ = new Dictionary<string, int>();
	private Dictionary<string, int> patternsDetectCount_ = new Dictionary<string, int>();

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
		charge_  = GetComponent<ShotCharge>();
	}


	void Start()
	{
		rawPos_ = transform.position;
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
			if (to.eulerAngles.y - from.eulerAngles.y >=  180) from *= Quaternion.Euler(Vector3.up * 360);
			if (to.eulerAngles.y - from.eulerAngles.y <= -180) from *= Quaternion.Euler(-Vector3.up * 360);
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
		UpdateEdges(data.id, data.edges);
		UpdatePatterns(data.id, data.patterns);
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


	public void UpdateEdges(int markerId, List<EdgeData> edges)
	{
		var updatedMap = new Dictionary<int, bool>();
		foreach (var data in edges_) {
			updatedMap.Add(data.Key, false);
		}

		foreach (var edge in edges) {
			var id = edge.id;
			GameObject edgeObj;
			if (edges_.ContainsKey(id)) {
				edgeObj = edges_[id];
				updatedMap[id] = true;
			} else {
				edgeObj = Instantiate(edgePrefab) as GameObject;
				edgeObj.transform.SetParent(transform);
				edges_.Add(id, edgeObj);
			}
			var shots = edgeObj.GetComponentsInChildren<PlayerShot>();
			foreach (var shot in shots) {
				shot.charge = charge_;
				shot.id = markerId;
				shot.active = true;
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

	void UpdatePatterns(int markerId, List<PatternData> patterns)
	{
		var updatedMap = new Dictionary<string, bool>();
		foreach (var data in patterns_) {
			updatedMap.Add(data.Key, false);
		}

		foreach (var pattern in patterns) {
			var id = "";
			var averagePos = Vector3.zero;
			var averageDir = Vector3.zero;
			foreach (var edgeId in pattern.ids) {
				id += "/" + edgeId.ToString();
				if (!edges_.ContainsKey(edgeId)) {
					return;
				}
				var edge = edges_[edgeId];
				averagePos += edge.transform.localPosition;
				averageDir += edge.transform.forward;
			}
			id += "/" + pattern.pattern.ToString();

			if (!patternsDetectCount_.ContainsKey(id)) {
				patternsDetectCount_.Add (id, 1);
			} else if (++patternsDetectCount_[id] < 30) {
				averagePos /= pattern.ids.Count;
				averageDir /= pattern.ids.Count;

				GameObject patternObj;
				if (patterns_.ContainsKey(id)) {
					patternObj = patterns_[id];
					updatedMap[id] = true;
				} else {
					patternObj = InstantiatePattern(pattern.pattern);
					if (!patternObj) continue;
					patternObj.transform.SetParent(transform);
					patterns_.Add(id, patternObj);
				}

				patternObj.transform.localPosition = averagePos;
				if (averageDir != Vector3.zero) {
					patternObj.transform.rotation = Quaternion.LookRotation(averageDir);
				}

				var patternShots = patternObj.GetComponentsInChildren<PlayerShot>();
				foreach (var shot in patternShots) {
					shot.charge = charge_;
					shot.id = markerId;
					shot.OnPattern(pattern, edges_);
				}

				foreach (var edgeId in pattern.ids) {
					var edge = edges_[edgeId];
					var normalShots = edge.GetComponentsInChildren<PlayerShot>();
					foreach (var shot in normalShots) {
						shot.active = false;
					}
				}
			}
		}

		foreach (var data in updatedMap) {
			var id = data.Key;
			if (!data.Value) {
				if (patternsLostCount_.ContainsKey(id)) {
					if (patternsLostCount_[id]++ > 10) {
						Destroy(patterns_[id]);
						patternsLostCount_.Remove(id);
						if (patterns_.ContainsKey(id)) {
							patterns_.Remove(id);
						}
						if (patternsDetectCount_.ContainsKey(id)) {
							patternsDetectCount_.Remove(id);
						}
					}
				} else {
					patternsLostCount_.Add(id, 1);
				}
			} else {
				if (patternsLostCount_.ContainsKey(id)) {
					patternsLostCount_[id] = 0;
				}
			}
		}
	}

	GameObject InstantiatePattern(int patternId)
	{
		var prefab = patternPrefabs.Find((PatternPrefab p) => p.id == patternId).prefab;
		if (prefab) {
			return Instantiate(prefab) as GameObject;
		} else {
			return null;
		}
	}
}
