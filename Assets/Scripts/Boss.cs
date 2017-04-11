using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpellController))]
public class Boss : Enemy {

	private SpellController spellController;

	private float defaultXScale;

	private void Start() {
		spellController = GetComponent<SpellController>();

		defaultXScale = transform.localScale.x;
	}

	private void Update() {
		if(attackCooldownTimer > 0)
			attackCooldownTimer -= Time.deltaTime;

		if(seesPlayer) {
			float dist = Vector3.Distance(transform.position, character.transform.position);
			if(dist <= attackDistance && attackCooldownTimer <= 0f) {
				Attack(character);
				attackCooldownTimer = attackCooldownTime;
			}
		}

		if(character == null)
			return;

		if(!seesPlayer)
			if(Vector3.Distance(transform.position, character.transform.position) <= visionDistance)
				if(SeesPlayer())
					seesPlayer = true;
	}

	private bool SeesPlayer() {
		if(character == null)
			return false;

		Vector3 origin = transform.position;
		Vector3 dir = (character.transform.position + new Vector3(0, 0.5f, 0)) - transform.position;
		if(Physics2D.Raycast(origin, dir, visionDistance, LayerMask.GetMask("Character"))) {
			Debug.DrawRay(origin, dir * visionDistance, Color.green);
			return true;
		} else {
			Debug.DrawRay(origin, dir * visionDistance, Color.red);
		}

		return false;
	}

	protected override void Attack(Entity _e) {
		Vector3 playerPos = character.transform.position + new Vector3(0, 1f, 0);
		Vector3 dir = (playerPos - spellController.spellOrigin.position).normalized;

		Vector3 charDir = (playerPos - transform.position).normalized;
		Vector3 scale = transform.localScale;
		if(charDir.x < 0)
			scale.x = -defaultXScale;
		else if(charDir.x > 0)
			scale.x = defaultXScale;
		transform.localScale = scale;

		spellController.UseActiveSpell(dir, _e);
	}

	protected override void OnDie() {
		LevelManager.AddScore(LevelManager.SCORE_PER_KILL);
		Destroy(gameObject);
	}

}
