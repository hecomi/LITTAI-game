using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using SimpleJSON;
using System.Collections.Generic;

public class OscServer : MonoBehaviour
{
    public int listenPort = 6666;
	public bool isReversed = true;
	private int sign { get { return isReversed ? -1 : 1; } }
	private float offsetAngle { get { return isReversed ? 0 : Mathf.PI; } }

    private UdpClient udpClient_;
    private IPEndPoint endPoint_;
    private Osc.Parser osc_ = new Osc.Parser();
    

    void Start()
    {
        endPoint_  = new IPEndPoint(IPAddress.Any, listenPort);
        udpClient_ = new UdpClient(endPoint_);
    }


	void OnDestroy()
	{
		udpClient_.Close();
	}


    void Update()
    {
        while (udpClient_.Available > 0) {
            osc_.FeedData(udpClient_.Receive(ref endPoint_));
        }

        while (osc_.MessageCount > 0) {
	        var msg = osc_.PopMessage();
			if (msg.path.IndexOf("/marker") == 0) {
				ProcessMarkerMessage(msg);
			} else if (msg.path.IndexOf("/landolt") == 0) {
				ProcessLandoltMessage(msg);
			}
        }
    }


	void ProcessMarkerMessage(Osc.Message msg)
	{
		var json = JSON.Parse(msg.data[0].ToString());
		var data = convertToMarkerData(json);

		int markerIndex = "/marker".Length;
		if (msg.path.IndexOf("/create") == markerIndex) {
			MarkerManager.Create(data);
		} else if (msg.path.IndexOf("/update") == markerIndex) {
			MarkerManager.Update(data);
		} else if (msg.path.IndexOf("/remove") == markerIndex) {
			MarkerManager.Remove(data);
		} else {
			Debug.LogWarning("unknown message: " + msg);
		}
	}


	void ProcessLandoltMessage(Osc.Message msg)
	{
		try {
			var json = JSON.Parse(msg.data[0].ToString());
			var data = convertToLandoltData(json);

			int landoltIndex = "/landolt".Length;
			if (msg.path.IndexOf("/create") == landoltIndex) {
				LandoltManager.Create(data);
			} else if (msg.path.IndexOf("/update") == landoltIndex) {
				LandoltManager.Update(data);
			} else if (msg.path.IndexOf("/remove") == landoltIndex) {
				LandoltManager.Remove(data);
			} else {
				Debug.LogWarning("unknown message: " + msg);
			}
		} catch (Exception e) {
			Debug.LogWarning(e.Message);
			Debug.LogWarning(msg);
		}
	}


	LandoltData convertToLandoltData(JSONNode json)
	{
		LandoltData data = new LandoltData();

		data.id       = json["id"].AsInt;
		data.pos      = new Vector2(sign * json["x"].AsFloat, sign * json["y"].AsFloat);
		data.radius   = json["radius"].AsFloat;
		data.width    = json["width"].AsFloat;
		data.height   = json["height"].AsFloat;
		data.angle    = json["angle"].AsFloat + offsetAngle;
		data.cnt      = 0;
		data.touched  = json["touched"].AsBool;
		data.touchPos = new Vector2(json["touchX"].AsFloat, json["touchY"].AsFloat);

		return data;
	}


	MarkerData convertToMarkerData(JSONNode json)
	{
		MarkerData data = new MarkerData();

		var polygonData  = json["polygon"].AsArray;
		var edgesData    = json["edges"].AsArray;
		var indicesData  = json["indices"].AsArray;
		var patternsData = json["patterns"].AsArray;
		var polygon = new List<Vector3>();
		var edges = new List<EdgeData>();
		var indices = new List<int>();
		var patterns = new List<PatternData>();
		foreach (JSONClass vert in polygonData) {
			polygon.Add(new Vector3(sign * vert["x"].AsFloat, 0, sign * vert["y"].AsFloat));
		}
		foreach (JSONClass edgeData in edgesData) {
			var edge = new EdgeData();
			var dir = edgeData["direction"].AsObject;
			edge.id  = edgeData["id"].AsInt;
			edge.pos = new Vector3(sign * edgeData["x"].AsFloat, 0, sign * edgeData["y"].AsFloat);
			edge.dir = new Vector3(sign * dir["x"].AsFloat, 0, sign * dir["y"].AsFloat);
			edges.Add(edge);
		}
		foreach (JSONData index in indicesData) {
			indices.Add(index.AsInt);
		}
		foreach (JSONClass pattern in patternsData) {
			var p = new PatternData();
			var ids = pattern["ids"].AsArray;
			p.pattern = pattern["pattern"].AsInt;
			foreach (JSONData id in ids) {
				p.ids.Add(id.AsInt);
			}
			patterns.Add(p);
		}

		data.id      = json["id"].AsInt;
		data.pos     = new Vector3(sign * json["x"].AsFloat, 0f, sign * json["y"].AsFloat);
		data.size    = json["size"].AsFloat;
		data.angle   = json["angle"].AsFloat;
		data.polygon = polygon;
		data.edges   = edges;
		data.indices = indices;
		data.edges   = edges;
		data.patterns = patterns;

		return data;
	}
}