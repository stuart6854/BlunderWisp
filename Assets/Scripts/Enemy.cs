using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	public float moveSpeed = 2;
	private float accelerationTimeAirborne = .2f;
	private float accelerationTimeGrounded = .1f;

	private float gravity;
	private float jumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;

	public Vector3[] waypoints;
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

	}

	private void WaypointMove() {
		if(waypoints.Length == 0)
			return;
		
		Vector3 wp = waypoints[currentWaypoint];
		Vector3 input = (transform.position - wp).normalized;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity += Physics.gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);

		print(Vector2.Distance(transform.position, wp));
		if (Vector2.Distance(transform.position, wp) <= 0.5f) {
			currentWaypoint++;
			if (currentWaypoint >= waypoints.Length)
				currentWaypoint = 0;
		}
	}

	protected override void Attack(Entity _e) {
		
	}

	protected override void OnDie() {
		Destroy(gameObject);
	}

}
