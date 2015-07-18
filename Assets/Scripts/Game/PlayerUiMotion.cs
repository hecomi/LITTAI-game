using UnityEngine;
using System.Collections;

public class PlayerUiMotion : MonoBehaviour
{
	public Transform parentPlayer;
	public float distance = 0.1f;
	public float damping = 0.1f;

	private LineRenderer line;

	void Start()
	{
		line = GetComponent<LineRenderer>();
		line.SetVertexCount(2);
	}

	void LateUpdate()
	{
		var dir = (parentPlayer.position - transform.position).normalized;
		var targetPos = parentPlayer.position - dir * distance;
		transform.position += (targetPos - transform.position) * damping;

		var offset = Vector3.down * 1f;
		line.SetPosition(1, transform.position + offset);
		line.SetPosition(0, parentPlayer.position + offset);

		var from = transform.rotation;
		var to = parentPlayer.rotation;
		transform.rotation = Quaternion.Slerp(from, to, 0.1f);
	}
}
