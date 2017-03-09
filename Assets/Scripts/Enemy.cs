using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	public float moveSpeed = 2;

	public Character character;
	public float visionDistance = 5.0f;
	public float attackDistance = 1.0f;

	private float accelerationTimeAirborne = .2f;
	private float accelerationTimeGrounded = .1f;

	private float gravity;
	private float jumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;

	private Vector3[] waypoints;
	private int currentWaypoint;

	private bool seesPlayer;

	private void Start() {
		List<Vector3> wps = new List<Vector3>();
		foreach (Transform child in transform) {
			if (child.gameObject.layer == LayerMask.NameToLayer("Waypoint")) {
				wps.Add(child.position);
				Destroy(child.gameObject);
			}
		}
		waypoints = wps.ToArray();
	}

	private void Update() {
		if(!seesPlayer)
			WaypointMove();
		else
			AttackMove();

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

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity += Physics.gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);
		
		if (Vector2.Distance(transform.position, wp) <= 0.25f) {
			currentWaypoint++;
			if (currentWaypoint >= waypoints.Length)
				currentWaypoint = 0;
		}
	}

	private void AttackMove() {
		float dist = Vector3.Distance(transform.position, character.transform.position);
		if (dist <= attackDistance) {
			Attack(character);
			return;
		}

		Vector3 input = (character.transform.position - transform.position).normalized;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity += Physics.gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);
	}

	private bool SeesPlayer() {
		if (character == null)
			return false;

		Vector3 origin = transform.position;
		Vector3 dir = transform.position - character.transform.position;
		RaycastHit2D hit;
		if (Physics2D.Raycast(origin, dir, visionDistance, LayerMask.GetMask("Character"))) {
			return true;
		}

		return false;
	}

	protected override void Attack(Entity _e) {
		Debug.Log("Enemy Attack");
	}

	protected override void OnDie() {
		Destroy(gameObject);
	}

}
