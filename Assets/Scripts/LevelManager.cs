using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public string nextLevel;

	[Header("References")]
	public GameObject levelCompleteUI;
	public GameObject levelFailedUI;
	public Character characterController;

	public List<Enemy> enemies;

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
		foreach(Enemy enemy in enemies) {
			if(enemy != null)
				return false;
		}

		return true;
	}

	private void OnLevelComplete() {
		levelCompleteUI.SetActive(true);
		characterController.enabled = false;
		//TODO: Disable Character, AI, etc.
	}

	private void OnLevelFailed() {
		levelFailedUI.SetActive(true);
		characterController.enabled = false;
		//TODO: Disable Character, AI, etc.
	}

	private void OnDrawGizmos() {
		Bounds bounds = GetComponent<Collider2D>().bounds;

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, bounds.size);
	}

}
