using UnityEngine;
using System.Collections;

public class SerialTestClient : MonoBehaviour {

	void Awake() {
	}

	void onPressed(int id) {
		Debug.Log (System.DateTime.Now + " onPressed: " + id);
    }

	void onPressing(int id) {
		Debug.Log (System.DateTime.Now + " onPressing: " + id);
	}

	void onReleased(int id) {
		Debug.Log (System.DateTime.Now + " onReleased: " + id);
    }

	void onSwitchOn(int id) {
		Debug.Log (System.DateTime.Now + " onSwitchOn: " + id);
	}

	void onSwitchOff(int id) {
		Debug.Log(System.DateTime.Now + " onSwitchOff: " + id);
	}

	// Use this for initialization
	void Start () {
		Debug.Log("@SerialTestClient");
		SerialHandler.Open();
		SerialHandler.Pressed += onPressed;
		SerialHandler.Pressing += onPressing;
		SerialHandler.Released += onReleased;
		SerialHandler.SwitchOn += onSwitchOn;
		SerialHandler.SwitchOff += onSwitchOff;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
