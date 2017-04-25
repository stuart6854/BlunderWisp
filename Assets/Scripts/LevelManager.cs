using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	public static int SCORE_PER_SECOND = 1;
	public static int SCORE_PER_LIFE = 10;
	public static int SCORE_PER_KILL = 25;
	public static int SCORE_PER_BOSS_KILL = 50;

	public string levelName;
	public string nextLevel;

	[Header("Scoring")]
	public int scoringTimeLimitSecs = 180;

	[Header("References")]
	public GameObject levelCompleteUI;
	public GameObject levelFailedUI;
	public Text scoreLabel;
	public Text timeLabel;
	public Character characterController;

	private float levelStartTime;
	private static int currentScore;

	public List<Enemy> enemies;

	private void Awake() {
		instance = this;

		enemies = new List<Enemy>();
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
			enemies.Add(obj.GetComponent<Enemy>());
		}
	}

	private void Start() {
		levelStartTime = Time.time;

		currentScore = 0;
		scoreLabel.text = currentScore + "";
	}

	private void Update() {
		float passedTime = Time.time - levelStartTime;
		int minutes = (int)passedTime / 60;
		int seconds = (int) passedTime % 60;

		string m = (minutes >= 10) ? "" + minutes : "0" + minutes;
		string s = (seconds >= 10) ? "" + seconds : "0" + seconds;
		timeLabel.text = m + ":" + s;
	}

	private void OnTriggerEnter2D(Collider2D _col) {
		print("Level Completion Trigger: " + _col.gameObject.name);
		if(CheckForCompletion())
			OnLevelComplete();
	}

	private bool CheckForCompletion() {
		foreach(Enemy enemy in enemies) {
			if(enemy != null && !enemy.isDead)
				return false;
		}

		return true;
	}

	private void OnLevelComplete() {
		Debug.Log("Level Complete!");
		levelCompleteUI.SetActive(true);
		characterController.enabled = false;
		//TODO: Disable Character, AI, etc.

		float completionTime = Time.time - levelStartTime;
//		Debug.Log("Level Completed in " + completionTime + " seconds.");

		int timeScore = 0;
		if (completionTime <= scoringTimeLimitSecs)
			timeScore = (int)(scoringTimeLimitSecs - completionTime) * SCORE_PER_SECOND;
//		Debug.Log("Time Score: " + timeScore);

		int lifeScore = characterController.currentHealth * SCORE_PER_LIFE;
//		Debug.Log("Life Score: " + lifeScore);

		int finalScore = currentScore + timeScore + lifeScore;
//		Debug.Log("Final Score: " + finalScore);
		int topScore = PlayerPrefs.GetInt(levelName + "_topScore");
//		Debug.Log("Top Score: " + topScore);

		if(finalScore > topScore)
			PlayerPrefs.SetInt(levelName + "_topScore", finalScore);

		scoreLabel.text = finalScore + "";
	}

	public void OnLevelFailed() {
		levelFailedUI.SetActive(true);
		characterController.enabled = false;
		//TODO: Disable Character, AI, etc.
	}

	public static void AddScore(int _score) {
		currentScore += _score;
		instance.scoreLabel.text = currentScore + "";
	}

	private void OnDrawGizmos() {
		Bounds bounds = GetComponent<Collider2D>().bounds;

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, bounds.size);
	}

}
