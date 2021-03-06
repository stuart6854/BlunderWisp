using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpellController))]
public class Character : Entity {

	private const int STATE_IDLE = 0;
	private const int STATE_WALK = 1;
	private const int STATE_JUMP = 2;
	private const int STATE_ATTACK = 3;
	private const int STATE_DEAD = 4;
	
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;
	private float accelerationTimeAirborne = .2f;
	private float accelerationTimeGrounded = .1f;

	public Image healthBar;
	public AudioClip deathSound;
	public AudioClip hitSound;

	private float gravity;
	private float jumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;

	private SpellController spellController;

	private bool isPlaying_Attack;

	private float defaultXScale;

	private new void Awake() {
		base.Awake();

		spellController = GetComponent<SpellController>();
	}

	private void Start() {
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		defaultXScale = transform.localScale.x;
	}

	private void Update() {
		if(currentHealth <= 0)
			return;

		if(animator.GetCurrentAnimatorStateInfo(0).IsName("wispAttack"))
			isPlaying_Attack = true;
		else
			isPlaying_Attack = false;

		HandleAnims();
		
		if(collisionInfo.above || collisionInfo.below)
			velocity.y = 0;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
		input.y = (Input.GetButtonDown("Jump") ? 1 : 0);

		if(input.y > 0 && collisionInfo.below)
			velocity.y = jumpVelocity;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		Move(velocity * Time.deltaTime);


		if(Input.GetMouseButtonDown(0))
			Attack(null);

	}

	private void HandleAnims() {
		float movement = Input.GetAxisRaw("Horizontal");

		Vector3 scale = transform.localScale;
		if (movement < 0) scale.x = -defaultXScale;
		else if (movement > 0) scale.x = defaultXScale;
		transform.localScale = scale;

		if(Input.GetMouseButtonDown(0) && spellController.CanUseActiveSpell())
			ChangeState(STATE_ATTACK);
		else if(movement == 0 && !isPlaying_Attack)
			ChangeState(STATE_IDLE);
		else if(movement != 0 && !isPlaying_Attack)
			ChangeState(STATE_WALK);

	}

	protected override void Attack(Entity _e) {
		Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 dir = (mousePosWorld - spellController.spellOrigin.position).normalized;

		Vector3 charDir = (mousePosWorld - transform.position).normalized;
		Vector3 scale = transform.localScale;
		if(charDir.x < 0)
			scale.x = -defaultXScale;
		else if(charDir.x > 0)
			scale.x = defaultXScale;
		transform.localScale = scale;

		spellController.UseActiveSpell(dir, _e);
	}

	public override void OnAttacked(int _damage) {
		base.OnAttacked(_damage);
		if(hitSound != null)
			audioPlayer.PlayOneShot(hitSound);

		healthBar.fillAmount = ((float)currentHealth / (float)maxHealth);
	}

	protected override void OnDie() {
		isDead = true;
		ChangeState(STATE_DEAD);
		if(deathSound != null)
			audioPlayer.PlayOneShot(deathSound);

		GameObject lvlManager = GameObject.FindWithTag("LevelManager");
		lvlManager.GetComponent<LevelManager>().OnLevelFailed();
	}

}
