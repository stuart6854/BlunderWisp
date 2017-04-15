using UnityEngine;

public class PuzzleTrigger : MonoBehaviour {

	public PuzzleAction puzzleAction;
	public Spell requiredSpell;

	private bool isTriggered;

	private void OnTriggerEnter2D(Collider2D _col) {
		if(_col.gameObject.layer == LayerMask.NameToLayer("Spell") && !isTriggered) {
			if(requiredSpell != null && _col.gameObject.GetComponent<Spell>().spellIdentifier != requiredSpell.spellIdentifier)
				return;

			isTriggered = true;
			puzzleAction.OnPuzzleTriggered();

			Destroy(gameObject);

			//Darken Sprite to indicate that it has already been triggered
//			Color color = GetComponent<SpriteRenderer>().color;
//			color.r -= 0.5f;
//			color.g -= 0.5f;
//			color.b -= 0.5f;
//			GetComponent<SpriteRenderer>().color = color;
		}
	}

}
