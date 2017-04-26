using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Narrative : MonoBehaviour {

	public void Update() {
		if(Input.GetKeyUp(KeyCode.Space))
			SceneManager.LoadScene(1);
	}
	
}
