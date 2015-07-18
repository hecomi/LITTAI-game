﻿using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using SimpleJSON;
using System.Collections.Generic;

public class OscServer : MonoBehaviour
{
    public int listenPort = 6666;
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
	}


	LandoltData convertToLandoltData(JSONNode json)
	{
		LandoltData data = new LandoltData();

		data.id      = json["id"].AsInt;
		data.pos     = new Vector2(json["x"].AsFloat, json["y"].AsFloat);
		data.radius  = json["radius"].AsFloat;
		data.width   = json["width"].AsFloat;
		data.height  = json["height"].AsFloat;
		data.angle   = json["angle"].AsFloat;
		data.cnt     = 0;

		return data;
	}


	MarkerData convertToMarkerData(JSONNode json)
	{
		MarkerData data = new MarkerData();

		var polygonData = json["polygon"].AsArray;
		var edgesData   = json["edges"].AsArray;
		var indicesData = json["indices"].AsArray;
		List<Vector3> polygon = new List<Vector3>();
		List<EdgeData> edges = new List<EdgeData>();
		List<int> indices = new List<int>();
		foreach (JSONClass vert in polygonData) {
			polygon.Add(new Vector3(vert["x"].AsFloat, 0, vert["y"].AsFloat));
		}
		foreach (JSONClass edgeData in edgesData) {
			var edge = new EdgeData();
			var dir = edgeData["direction"].AsObject;
			edge.id  = edgeData["id"].AsInt;
			edge.pos = new Vector3(edgeData["x"].AsFloat, 0, edgeData["y"].AsFloat);
			edge.dir = new Vector3(dir["x"].AsFloat, 0, dir["y"].AsFloat);
			edges.Add(edge);
		}
		foreach (JSONData index in indicesData) {
			indices.Add(index.AsInt);
		}

		data.id      = json["id"].AsInt;
		data.pos     = new Vector3(json["x"].AsFloat, 0f, json["y"].AsFloat);
		data.size    = json["size"].AsFloat;
		data.angle   = json["angle"].AsFloat;
		data.polygon = polygon;
		data.edges   = edges;
		data.indices = indices;
		data.edges   = edges;

		return data;
	}
}