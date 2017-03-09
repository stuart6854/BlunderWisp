using UnityEngine;

[RequireComponent(typeof(SpellController))]
public class Character : Entity {

	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;
	private float accelerationTimeAirborne = .2f;
	private float accelerationTimeGrounded = .1f;

	private float gravity;
	private float jumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;

	private SpellController spellController;

	private new void Awake() {
		base.Awake();

		spellController = GetComponent<SpellController>();
	}

	private void Start() {
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}

	private void Update() {
		if(collisionInfo.above || collisionInfo.below)
			velocity.y = 0;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(input.y > 0 && collisionInfo.below)
			velocity.y = jumpVelocity;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);

		if(Input.GetMouseButtonDown(0))
			Attack(null);
	}

	protected override void Attack(Entity _e) {
		Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 dir = (mousePosWorld - spellController.spellOrigin.position).normalized;

		spellController.UseActiveSpell(dir, _e);
	}

	protected override void OnDie() {
		Destroy(gameObject);
	}

}
