using UnityEngine;
using System.Collections;

public class EnemyLocator : MonoBehaviour
{
	public GameObject enemyPrefab;

	void OnActivated()
	{
		if (enemyPrefab) {
			var enemy = Instantiate(enemyPrefab) as GameObject;
			enemy.transform.position = transform.position;
			enemy.transform.rotation = transform.rotation;
			enemy.transform.parent   = transform.parent;
		}
		Destroy(gameObject);
	}
}
