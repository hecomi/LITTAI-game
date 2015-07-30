using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ranking : MonoBehaviour
{
	static Ranking Instance = null;
	public string serverUri = "http://localhost:3000/pic"; 

	Camera selfy;
	public RenderTexture rTexture;
	Texture2D texture;
	const int sqr = 512;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		selfy = GetComponent<Camera> ();
		selfy.aspect = 1.0f;

//		rTexture = new RenderTexture (sqr, sqr, 0);
		selfy.targetTexture = rTexture;

		texture = new Texture2D (sqr, sqr, TextureFormat.RGB24, false);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void post(float score, GameObject player){
		capture (player);
		_post (score, texture);
	}

	static public void Post(float score, GameObject player)
	{
		Instance.post(score, player);
	}

	void capture(GameObject player){
		// AREA
		Bounds bounds = player.GetComponent<Renderer> ().bounds;
		
		var width = bounds.max.x - bounds.min.x;
		var height = bounds.max.z - bounds.min.z;
		
		float size = 0;
		if (width > height)
			size = width;
		else
			size = height;

		// CAM SET
		selfy.orthographicSize = size/2;
		selfy.transform.position = new Vector3 (bounds.center.x, 10, bounds.center.z);

		// CAPTURE
    	selfy.Render ();

		// ENCODE TO TEXTURE
		RenderTexture.active = rTexture;
		texture.ReadPixels (new Rect (0, 0, sqr, sqr), 0, 0);
		RenderTexture.active = null;
	}

	void _post(float score, Texture2D _texture) {
		WWWForm form = new WWWForm ();

		form.AddField ("score", ((int)(score)).ToString());
		byte[] bytes = _texture.EncodeToPNG ();
		form.AddBinaryData ("pic", bytes, "hoge", "image/png");

		WWW www = new WWW (serverUri, form);
		StartCoroutine (WaitForRequest (www));

		Debug.Log (www.text);
	}

	WWW GET(string url){
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www));
		return www;
	}
	
	WWW POST(string url, Dictionary<string, string> post) {
		WWWForm form = new WWWForm ();
		foreach(KeyValuePair<string, string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW (url, form);
		StartCoroutine (WaitForRequest (www));
		return www;
	}
	
	IEnumerator WaitForRequest(WWW www) {
		yield return www;
		//check for errors
		if (www.error == null) {
			Debug.Log ("WWW Ok!: " + www.text);
		} else {
			Debug.Log ("WWW Error!: " + www.error);
    }
  }
}
