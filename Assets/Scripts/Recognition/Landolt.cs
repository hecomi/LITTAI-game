using UnityEngine;
using System.Collections;

public class Landolt : MonoBehaviour
{
	private LandoltData data_;
	public float scaleRatio = 1.8f;
	private int lostCount_ = 0;


	void Update()
	{
		++lostCount_;
		if (lostCount_ > 180) {
			LandoltManager.Remove(data_);
		}
	}


	public void Update(LandoltData data)
	{
		data_ = data;
		transform.localPosition = new Vector3(data_.pos.x, 0f, data_.pos.y);
		transform.localScale    = new Vector3(data_.width, 1f, data_.height) * scaleRatio;
		transform.localRotation = Quaternion.Euler(0f, Mathf.Rad2Deg * data_.angle + 90, 0f);

		lostCount_ = 0;
	}
}
