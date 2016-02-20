using UnityEngine;
using System.Collections;

public class Landolt : MonoBehaviour
{
	private LandoltData data_;
	public float scaleRatio = 1.8f;
	private int lostCount_ = 0;
	private float localScaleY_ = 1f;

	private Vector3 prePos_ = Vector3.zero;
	private float currentAngle_ = 0f;
	private float preAngle_ = 0f;
	private Vector3 velocity_ = Vector3.zero;
	private float angleVelocity_ = 0f;
	private float lastTime_ = 0f;
	private float lastInterpTime_ = 0f;
	public int maxInterpDuration = 5;
	private int updateCount_ = 0;
	private int interpCount_ = 0;

	private Vector3 rawPos_ = Vector3.zero;
	public float filter = 0.5f;

	private bool isInitialized_ = false;
	private bool touched_ = false;

	public GameObject revivalSpherePrefab;
	public GameObject scoreDecreseEffectPrefab;


	void Awake()
	{
		localScaleY_ = transform.localScale.y;
	}


	void Update()
	{
		++lostCount_;
		if (lostCount_ > 180) {
			LandoltManager.Remove(data_);
		}

		if (isInitialized_) {
			transform.localPosition = (rawPos_ - transform.localPosition) * filter;
			var from = transform.localRotation;
			var to = Quaternion.Euler(0f, Mathf.Rad2Deg * currentAngle_ + 90, 0f);
			transform.localRotation = Quaternion.Slerp(from, to, filter);
		} else {
			transform.localPosition = rawPos_;
			transform.localRotation = Quaternion.Euler(0f, Mathf.Rad2Deg * currentAngle_ + 90, 0f);
		}
	}


	public void Update(LandoltData data)
	{
		if (float.IsNaN(data.pos.x) || float.IsNaN(data.pos.y)) return;
		if (float.IsInfinity(data.pos.x) || float.IsInfinity(data.pos.y)) return;

		data_ = data;
		rawPos_ = new Vector3(data.pos.x, 0f, data.pos.y);
		transform.localScale = new Vector3(data_.width * scaleRatio, localScaleY_, data_.height * scaleRatio);
		lostCount_ = 0;

		var dt = Time.time - lastTime_;
		if (dt == 0) return;

		velocity_ = (rawPos_ - prePos_) / dt;
		prePos_ = rawPos_;
		angleVelocity_ = (data.angle - preAngle_) / dt;
		currentAngle_ = preAngle_ = data.angle;
		lastTime_ = lastInterpTime_ = Time.time;
		++updateCount_;
		interpCount_ = 0;

		if (touched_ != data.touched) {
			touched_ = data.touched;
			if (touched_) {
				SendMessage("OnTouchStart", data.touchPos, SendMessageOptions.DontRequireReceiver);
				Sound.Play("LandoltTouch");
			} else {
				SendMessage("OnTouchEnd", data.touchPos, SendMessageOptions.DontRequireReceiver);
			}
		} else {
			if (touched_) {
				SendMessage("OnTouchMove", data.touchPos, SendMessageOptions.DontRequireReceiver);
			}
		}
	}


	public void UpdateInterpolation()
	{
		if (updateCount_ <= 2 || interpCount_ > maxInterpDuration) return;
		if (Time.time == lastInterpTime_) return;

		var dt = Time.time - lastInterpTime_;
		rawPos_ += velocity_ * dt;
		currentAngle_ += angleVelocity_ * dt;

		lastInterpTime_ = Time.time;
		++interpCount_;
	}


	public void _SetLostCountZero()
	{
		lostCount_ = 0;
	}

	public void _SetRawPos(Vector3 pos)
	{
		rawPos_ = pos;
	}

	public void EmitRevivalSphere(GameObject target)
	{
		if (!revivalSpherePrefab) return;
		for (var i = 0; i < 5; ++i) {
			var obj = Instantiate(revivalSpherePrefab);
			obj.transform.SetParent(GlobalObjects.localStage.transform);
			obj.transform.localPosition = transform.localPosition;
			var sphere = obj.GetComponent<PlayerRevivalSphere>();
			if (sphere) {
				sphere.target = target;
			}
			Score.Sub(2000 / 5);
			StartCoroutine(GenerateScoreDecreaseEffect(i * 0.1f));
		}
	}

	IEnumerator GenerateScoreDecreaseEffect(float offset)
	{
		yield return new WaitForSeconds(offset);
		Sound.Play("EmitRevivalSphere");
		var effect = Instantiate(scoreDecreseEffectPrefab);
		effect.transform.SetParent(transform);
		effect.transform.position = transform.position;
	}
}
