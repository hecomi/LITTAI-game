using UnityEngine;
using System.Collections;

public class GlobalEffects : MonoBehaviour
{
	static public GlobalEffects Instance;
	public RippleEffect ripple;

	void Awake()
	{
		Instance = this;
	}

	static public void Riplle(float x, float y)
	{
		Instance.ripple.Emit(x, y);
	}
}
