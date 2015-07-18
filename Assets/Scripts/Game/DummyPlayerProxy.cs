using UnityEngine;
using System.Collections;

public class DummyPlayerProxy : MonoBehaviour
{
	void OnAttacked(int hit)
	{
		transform.parent.SendMessage("OnAttacked", hit, SendMessageOptions.DontRequireReceiver);
	}
}
