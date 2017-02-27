using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public string levelTile;

	private List<Enemy> enemies;

	private void Awake() {

	}

	private void OnTriggerEnter2D(Collider2D _col) {
		Debug.Log("test Trigger");
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
