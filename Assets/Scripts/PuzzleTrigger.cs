using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PuzzleTrigger : MonoBehaviour {

	public PuzzleAction puzzleAction;
	public Spell requiredSpell;

	private AudioSource soundPlayer;

	private bool isTriggered;

	private void Awake() {
		soundPlayer = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D _col) {
		if(_col.gameObject.layer == LayerMask.NameToLayer("Spell") && !isTriggered) {
			if(requiredSpell != null && _col.gameObject.GetComponent<Spell>().spellIdentifier != requiredSpell.spellIdentifier)
				return;

			soundPlayer.Play();
			isTriggered = true;
			puzzleAction.OnPuzzleTriggered();

			this.enabled = false;
			GetComponent<Renderer>().enabled = false;

			//Darken Sprite to indicate that it has already been triggered
//			Color color = GetComponent<SpriteRenderer>().color;
//			color.r -= 0.5f;
//			color.g -= 0.5f;
//			color.b -= 0.5f;
//			GetComponent<SpriteRenderer>().color = color;
		}
	}

}
