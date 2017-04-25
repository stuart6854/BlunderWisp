using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PuzzleDoor : PuzzleAction {

	public AudioClip sound;

	public override void OnPuzzleTriggered() {
		if(sound != null)
			GetComponent<AudioSource>().PlayOneShot(sound);

		gameObject.SetActive(false);
	}

}
