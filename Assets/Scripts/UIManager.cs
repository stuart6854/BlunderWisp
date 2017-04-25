using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public GameObject pauseMenu;

	private bool inGame;

	private void Start() {
		inGame = (SceneManager.GetActiveScene().buildIndex >= 0); //MainMenu should have buildIndex 0
	}

	void Update () {
		if (inGame && Input.GetKeyDown(KeyCode.Escape)) {
			//TODO: Pause Menu
			ShowPauseMenu(!pauseMenu.activeSelf);
		}
	}

	public void ShowPauseMenu(bool _show) {
		pauseMenu.SetActive(_show);
		Time.timeScale = (_show) ? 0f : 1f;
	}

	public void GoToScene(string _sceneName) {
		SceneManager.LoadScene(_sceneName);
	}

	public void GoToScene(int _sceneIndex) {
		SceneManager.LoadScene(_sceneIndex);
	}

	public void RestartScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame() {
		Application.Quit();
	}


}
