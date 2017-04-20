using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpellController))]
public class Boss : Enemy {

	private const int STATE_RITUAL = 0;
	private const int STATE_ATTACK_IDLE = 1;
	private const int STATE_ATTACK = 2;
	private const int STATE_DEAD = 3;

	private SpellController spellController;

	private bool isPlaying_Attack;

	private float defaultXScale;

	private void Start() {
		spellController = GetComponent<SpellController>();

		defaultXScale = Mathf.Abs(transform.localScale.x);

		ChangeState(STATE_RITUAL);
	}

	private void Update() {
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("fanngAttack"))
			isPlaying_Attack = true;
		else
			isPlaying_Attack = false;

		if(attackCooldownTimer > 0)
			attackCooldownTimer -= Time.deltaTime;

		if(seesPlayer)
			HandleFaceDir(character.transform.position);

		if(seesPlayer && !isPlaying_Attack) {
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
				if(SeesPlayer()) {
					seesPlayer = true;
					ChangeState(STATE_ATTACK_IDLE);
				}
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

	private void HandleFaceDir(Vector3 _target) {
		Vector3 charDir = (transform.position - _target).normalized;

		Vector3 scale = transform.localScale;
		if(charDir.x < 0)
			scale.x = -defaultXScale;
		else if(charDir.x > 0)
			scale.x = defaultXScale;
		transform.localScale = scale;
	}

	protected override void Attack(Entity _e) {
		Vector3 playerPos = character.transform.position + new Vector3(0, 1f, 0);
		Vector3 dir = (playerPos - spellController.spellOrigin.position).normalized;

		spellController.UseActiveSpell(dir, _e);

		ChangeState(STATE_ATTACK);
	}

	protected override void OnDie() {
		LevelManager.AddScore(LevelManager.SCORE_PER_KILL);
		ChangeState(STATE_DEAD);

		this.enabled = false;

	}

}
