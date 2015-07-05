using UnityEngine;
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

	public PolygonCreator polygonCreator;
    
    void Start()
    {
        endPoint_  = new IPEndPoint(IPAddress.Any, listenPort);
        udpClient_ = new UdpClient(endPoint_);
    }

    void Update()
    {
        while (udpClient_.Available > 0) {
            osc_.FeedData(udpClient_.Receive(ref endPoint_));
        }

        while (osc_.MessageCount > 0) {
            var msg = osc_.PopMessage();
			if (msg.path.IndexOf("/marker") != -1) {
				var json = JSON.Parse(msg.data[0].ToString());
				var polygonData = json["polygon"].AsArray;
				var indicesData = json["indices"].AsArray;
				List<Vector3> polygon = new List<Vector3>();
				List<int> indices = new List<int>();
				foreach (JSONClass vert in polygonData) {
					polygon.Add(new Vector3(vert["x"].AsFloat, 0, vert["y"].AsFloat));
				}
				foreach (JSONData index in indicesData) {
					indices.Add(index.AsInt);
				}
				polygonCreator.polygon = polygon;
				polygonCreator.indices = indices;
			}
        }
    }
}