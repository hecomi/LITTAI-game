using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LandoltManager : MonoBehaviour
{
	static private LandoltManager Instance;

	public GameObject landoltPrefab;
	private Dictionary<int, Landolt> landolts_ = new Dictionary<int, Landolt>();


	void Awake()
	{
		Instance = this;
	}


	void Update()
	{
	}


	public static Landolt GetFirst()
	{
		if (Instance.landolts_.Count == 0) return null;
		return Instance.landolts_.First().Value;
	}


	private void CreateImpl(LandoltData data)
	{
		if (!landolts_.ContainsKey(data.id)) {
			var gameObj = Instantiate(landoltPrefab) as GameObject; 
			gameObj.transform.SetParent(GlobalObjects.localStage.transform);
			var landolt = gameObj.GetComponent<Landolt>();
			landolts_.Add(data.id, landolt);
			Sound.Play("LandoltDetected");
		}
		Update(data);
	}


	private void UpdateImpl(LandoltData data)
	{
		if (!landolts_.ContainsKey(data.id)) {
			Create(data);
		} else {
			var landolt = landolts_[data.id];
			landolt.Update(data);
		}
	}


	private void RemoveImpl(LandoltData data)
	{
		if (landolts_.ContainsKey(data.id)) {
			Destroy(landolts_[data.id].gameObject);
			landolts_.Remove(data.id);
		}
	}


	static public void Create(LandoltData data)
	{
		Instance.CreateImpl(data);
	}


	static public void Update(LandoltData data)
	{
		Instance.UpdateImpl(data);
	}


	static public void Remove(LandoltData data)
	{
		Instance.RemoveImpl(data);
	}

	private void AddDummyImpl(int id, Landolt landolt)
	{
		landolts_.Add(id, landolt);
	}

	static public void AddDummy(int id, Landolt landolt)
	{
		Instance.AddDummyImpl(id, landolt);
	}
}
