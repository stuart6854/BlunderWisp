using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Entity : MonoBehaviour {

	[Header("Stats")]
	public int maxHealth;
	public int currentHealth;

	[Header("Controller")]
	public float skinWidth = 0.015f;
	public int verticalRayCount = 4;
	public int horizontalRayCount = 4;
	public LayerMask groundCollisionMask;

	private float verticalRaySpacing;
	private float horizontalRaySpacing;

	private BoxCollider2D col;
	private RayOrigins rayOrigins;
	protected CollisionInfo collisionInfo;

	protected Animator animator;
	private int currentAnimState;

	protected void Awake() {
		col = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();

		CalculateRaySpacing();

		currentHealth = maxHealth;
	}

	protected void Move(Vector3 _velocity) {
		UpdateRayOrigins();
		collisionInfo.Reset();

		if(_velocity.x != 0)
			HorizontalCollisions(ref _velocity);
		if(_velocity.y != 0)
			VerticalCollision(ref _velocity);

		transform.Translate(_velocity);
	}

	protected abstract void Attack(Entity _e);

	public virtual void OnAttacked(int _damage) {
		currentHealth -= _damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		if(currentHealth <= 0)
			OnDie();
	}

	protected abstract void OnDie();

	private void HorizontalCollisions(ref Vector3 _velocity) {
		float dirX = Mathf.Sign(_velocity.x);
		float rayLength = Mathf.Abs(_velocity.x) + skinWidth; //Raycast further the faster we move to prevent high-speed collision misses

		for(int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (dirX == -1) ? rayOrigins.BL : rayOrigins.BR; //Raycast from left or right depending on X velocity
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, groundCollisionMask);
			if(hit) {
				_velocity.x = (hit.distance - skinWidth) * dirX;
				rayLength = hit.distance; //Only raycast as far as closest point for remainding rays
				collisionInfo.left = dirX == -1;
				collisionInfo.right = dirX == 1;
			}
			Debug.DrawRay(rayOrigin, Vector2.right * dirX * rayLength, Color.red);
		}
	}

	private void VerticalCollision(ref Vector3 _velocity) {
		float dirY = Mathf.Sign(_velocity.y);
		float rayLength = Mathf.Abs(_velocity.y) + skinWidth; //Raycast further the faster we fall to prevent high-speed collision misses

		for(int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (dirY == -1) ? rayOrigins.BL : rayOrigins.TL; //Raycast from top or bottom depending on Y velocity
			rayOrigin += Vector2.right * (verticalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dirY, rayLength, groundCollisionMask);
			if(hit) {
				_velocity.y = (hit.distance - skinWidth) * dirY;
				rayLength = hit.distance; //Only raycast as far as closest point for remainding rays
				collisionInfo.below = dirY == -1;
				collisionInfo.above = dirY == 1;
			}
			Debug.DrawRay(rayOrigin, Vector2.up * dirY * rayLength, Color.red);
		}
	}

	private void UpdateRayOrigins() {
		Bounds b = col.bounds;
		b.Expand(skinWidth * -2);

		rayOrigins.BL = new Vector2(b.min.x, b.min.y);
		rayOrigins.BR = new Vector2(b.max.x, b.min.y);
		rayOrigins.TL = new Vector2(b.min.x, b.max.y);
		rayOrigins.TR = new Vector2(b.max.x, b.max.y);
	}

	private void CalculateRaySpacing() {
		Bounds b = col.bounds;
		b.Expand(skinWidth * -2);

		horizontalRaySpacing = b.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = b.size.x / (verticalRayCount - 1);
	}

	protected void ChangeState(int _state) {
		if(currentAnimState == _state)
			return;

		animator.SetInteger("state", _state);
		currentAnimState = _state;
	}

	struct RayOrigins {
		public Vector2 TL, TR;
		public Vector2 BL, BR;
	}

	protected struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public void Reset() {
			above = below = false;
			left = right = false;
		}
	}

}
