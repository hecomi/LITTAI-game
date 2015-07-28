using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerManager : MonoBehaviour
{
	static private MarkerManager Instance;

	public GameObject markerPrefab;
	private Dictionary<int, Marker> markers_ = new Dictionary<int, Marker>();


	void Awake()
	{
		Instance = this;
	}


	void Update()
	{
	}


	private void CreateImpl(MarkerData data)
	{
		if (!markers_.ContainsKey(data.id)) {
			var gameObj = Instantiate(markerPrefab) as GameObject; 
			gameObj.transform.SetParent(GlobalObjects.localStage.transform);
			var marker = gameObj.GetComponent<Marker>();
			markers_.Add(data.id, marker);
			Sound.Play("MarkerDetected");
		}
		Update(data);
	}


	private void UpdateImpl(MarkerData data)
	{
		if (!markers_.ContainsKey(data.id)) {
			Create(data);
		} else {
			var marker = markers_[data.id];
			marker.Update(data);
		}
	}


	private void RemoveImpl(MarkerData data)
	{
		if (markers_.ContainsKey(data.id)) {
			Destroy(markers_[data.id].gameObject);
			markers_.Remove(data.id);
		}
	}


	static public void Create(MarkerData data)
	{
		Instance.CreateImpl(data);
	}


	static public void Update(MarkerData data)
	{
		Instance.UpdateImpl(data);
	}


	static public void Remove(MarkerData data)
	{
		Instance.RemoveImpl(data);
	}
}
