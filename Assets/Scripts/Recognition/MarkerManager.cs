using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerManager : MonoBehaviour
{
	[System.Serializable]
	public struct MarkerPlayerInfo
	{
		public int hp;
		public int power;
		public bool isDead;

		public MarkerPlayerInfo(int defaultHp, int defaultPower, bool defaultIsDead)
		{
			hp = defaultHp;
			power = defaultPower;
			isDead = defaultIsDead;
		}
	}

	static private MarkerManager Instance;

	public GameObject markerPrefab;
	private Dictionary<int, Marker> markers_ = new Dictionary<int, Marker>();
	private Dictionary<int, MarkerPlayerInfo> playerInfos_ = new Dictionary<int, MarkerPlayerInfo>();


	void Awake()
	{
		Instance = this;
	}


	void Update()
	{
	}


	static public List<GameObject> GetMarkerObjects()
	{
		var list = new List<GameObject>();
		foreach (var pair in Instance.markers_) {
			list.Add(pair.Value.gameObject);
		}
		return list;
	}


	IEnumerator KillRevivalPlayer(GameObject player)
	{
		yield return new WaitForEndOfFrame();
		if (player) {
			player.SendMessage("OnAttacked", 100000);
		}
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

		if (playerInfos_.ContainsKey(data.id)) {
			var info = playerInfos_[data.id];
			var player = markers_[data.id].GetComponent<Player>();
			var charge = markers_[data.id].GetComponent<ShotCharge>();
			if (player) {
				player.hp = info.hp;
				if (info.isDead) StartCoroutine( KillRevivalPlayer(player.gameObject) );
			}
			if (charge) {
				charge.power = info.power;
			}
		}
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
		var info = new MarkerPlayerInfo(1, 1, false);
		var player = markers_[data.id].GetComponent<Player>();
		var charge = markers_[data.id].GetComponent<ShotCharge>();
		if (player) {
			info.hp = player.hp;
			info.isDead = player.isDead;
		}
		if (charge) {
			info.power = charge.power;
		}
		if (playerInfos_.ContainsKey(data.id)) {
			playerInfos_[data.id] = info;
		} else {
			playerInfos_.Add(data.id, info);
		}

		if (markers_.ContainsKey(data.id)) {
			Destroy(markers_[data.id].gameObject);
			markers_.Remove(data.id);
		}
		Sound.Play("MarkerLost");
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
