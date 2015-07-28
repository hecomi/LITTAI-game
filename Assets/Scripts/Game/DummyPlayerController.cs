using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ShotCharge))]
public class DummyPlayerController : MonoBehaviour
{
	public Vector2 speed;
	public float rotationSpeed;
	public bool isPosRestricted = true;

	void Start()
	{
		var shots = transform.GetComponentsInChildren<PlayerShot>();
		var charge = GetComponent<ShotCharge>();
		foreach (var shot in shots) {
			shot.charge = charge;
			shot.id = 0;
		}
	}

	void Update()
	{
		var x = Input.GetAxisRaw("Horizontal");
		var y = Input.GetAxisRaw("Vertical");
		var v = new Vector3(x, 0, y).normalized * speed.magnitude;
		transform.position += v * Time.deltaTime;

		float w = 0f;
		if (Input.GetKey(KeyCode.Q)) w -= 1;
		if (Input.GetKey(KeyCode.E)) w += 1;
		transform.rotation *= Quaternion.Euler(0, w * rotationSpeed * Time.deltaTime, 0);

		if (isPosRestricted) {
			var p = transform.localPosition;
			if (p.x > Stage.MaxPos.x) p.x = Stage.MaxPos.x; 
			if (p.z > Stage.MaxPos.y) p.z = Stage.MaxPos.y; 
			if (p.x < Stage.MinPos.x) p.x = Stage.MinPos.x; 
			if (p.z < Stage.MinPos.y) p.z = Stage.MinPos.y; 
			transform.localPosition = p;
		}
	}
}
