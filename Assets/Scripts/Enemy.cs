using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	private float gravity;
	private float jumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;

	private void Update() {
		//float targetVelocityX = input.x * moveSpeed;
		//velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);
	}

	protected override void Attack(Entity _e) {
		
	}

}
