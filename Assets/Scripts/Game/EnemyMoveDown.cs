using UnityEngine;
using System.Collections;

public class EnemyMoveDown : MonoBehaviour
{
	public bool isStop = true;
	public Vector3 move = new Vector3(0f, 0f, -0.2f);
	public float offsetTime = 0f;
	public float duration = 0.5f;
	public iTween.EaseType easeType = iTween.EaseType.easeInOutQuad;

	private Vector3 velocity;

	void OnActivated()
	{
		velocity = move / duration;
		StartCoroutine(MoveDown());
	}

	IEnumerator MoveDown()
	{
		yield return new WaitForSeconds(offsetTime);
		if (isStop) {
			iTween.MoveBy(gameObject, iTween.Hash("amount", move, "time", duration, "easetype", easeType));
		} else {
			for (;;) {
				transform.localPosition += velocity * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
