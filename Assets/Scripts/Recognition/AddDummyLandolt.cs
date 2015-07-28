using UnityEngine;
using System.Collections;

public class AddDummyLandolt : MonoBehaviour
{
	private Landolt landolt_; 

	void Start()
	{
		landolt_ = GetComponent<Landolt>();
		landolt_._SetRawPos(transform.localPosition);
		LandoltManager.AddDummy(-1, landolt_);
	}

	void Update()
	{
		landolt_._SetLostCountZero();
	}
}
