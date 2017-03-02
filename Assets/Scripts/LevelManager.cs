using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public string levelTile;

	private List<Enemy> enemies;

	private void Awake() {
		enemies = new List<Enemy>();
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
			enemies.Add(obj.GetComponent<Enemy>());
		}
	}

	private void OnTriggerEnter2D(Collider2D _col) {
		print("Level Completion Trigger: " + _col.gameObject.name);
		if(CheckForCompletion())
			OnLevelComplete();
	}

	private bool CheckForCompletion() {
		if(enemies.Count == 0)
			return true;

		return false;
	}

	private void OnLevelComplete() {
		
	}

	private void OnLevelFailed() {
		
	}

}
