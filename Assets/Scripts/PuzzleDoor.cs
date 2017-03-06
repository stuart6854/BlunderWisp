using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : PuzzleAction {

	public override void OnPuzzleTriggered() {
		gameObject.SetActive(false);
	}

}
