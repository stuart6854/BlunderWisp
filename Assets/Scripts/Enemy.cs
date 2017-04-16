using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
	
	private const int STATE_WALK = 0;
	private const int STATE_ATTACK_1 = 1;
	private const int STATE_ATTACK_2 = 2;
	private const int STATE_DEAD = 3;

	[Header("Enemy Specific Stats")]
	public float moveSpeed = 2;
	public int attackDamage = 1;

	public Character character;
	public float visionDistance = 5.0f;
	public float attackDistance = 1.0f;
	public float attackCooldownTime = 2.0f;

	private float accelerationTimeAirborne = .2f;
	private float accelerationTimeGrounded = .1f;

	private float gravity;
	private float jumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;

	protected Vector3[] waypoints;
	protected int currentWaypoint;

	protected bool seesPlayer;
	protected float attackCooldownTimer;

	private bool isPlaying_Attack;

	private float defaultXScale;

	private void Start() {
		List<Vector3> wps = new List<Vector3>();
		foreach (Transform child in transform) {
			if (child.gameObject.layer == LayerMask.NameToLayer("Waypoint")) {
				wps.Add(child.position);
				Destroy(child.gameObject);
			}
		}
		waypoints = wps.ToArray();

		defaultXScale = transform.localScale.x;
	}

	private void Update() {
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("knightAttack1") 
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("knightAttack2"))
			isPlaying_Attack = true;
		else
			isPlaying_Attack = false;

		if(attackCooldownTimer > 0)
			attackCooldownTimer -= Time.deltaTime;

		if(!seesPlayer)
			WaypointMove();
		else
			AttackMove();

		if(character == null)
			return;

		if(!seesPlayer)
			if(Vector3.Distance(transform.position, character.transform.position) <= visionDistance)
				if (SeesPlayer())
					seesPlayer = true;

	}

	private void WaypointMove() {
		if(waypoints.Length == 0)
			return;
		
		Vector3 wp = waypoints[currentWaypoint];
		Vector3 input = (wp - transform.position).normalized;

		HandleFaceDir(wp);

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity += Physics.gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);

		ChangeState(STATE_WALK);
		
		if (Vector2.Distance(transform.position, wp) <= 0.25f) {
			currentWaypoint++;
			if (currentWaypoint >= waypoints.Length)
				currentWaypoint = 0;
		}
	}

	private void AttackMove() {
		if(isPlaying_Attack)
			return;

		HandleFaceDir(character.transform.position);

		float dist = Vector3.Distance(transform.position, character.transform.position);
		if (dist <= attackDistance && attackCooldownTimer <= 0f && !isPlaying_Attack) {
			Attack(character);
			attackCooldownTimer = attackCooldownTime;
			return;
		}

		Vector3 input = (character.transform.position - transform.position).normalized;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity += Physics.gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);

		if(!isPlaying_Attack)
			ChangeState(STATE_WALK);
	}

	private bool SeesPlayer() {
		if (character == null)
			return false;

		Vector3 origin = transform.position;
		Vector3 dir = transform.position - character.transform.position;
		if (Physics2D.Raycast(origin, dir, visionDistance, LayerMask.GetMask("Character"))) {
			return true;
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
		int x = Random.Range(0, 2);
		ChangeState((x == 0 ? STATE_ATTACK_1 : STATE_ATTACK_2));
		_e.OnAttacked(attackDamage);
	}

	protected override void OnDie() {
		LevelManager.AddScore(LevelManager.SCORE_PER_KILL);
		ChangeState(STATE_DEAD);

		this.enabled = false; //Disable AI
	}

}
