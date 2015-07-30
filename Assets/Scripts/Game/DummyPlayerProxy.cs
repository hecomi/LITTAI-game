using UnityEngine;
using System.Collections;

public class DummyPlayerProxy : MonoBehaviour
{
	void OnAttacked(int hit)
	{
		var player = GetComponentInParent<Player>();
		player.SendMessage("OnAttacked", hit, SendMessageOptions.DontRequireReceiver);
	}
}
