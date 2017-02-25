using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {

	public float cooldownTime = 1;
	public int damage = 1;
	public float speed = 1f;
	public float lifeTime; // How much time till this spell object is destroyed.

	[HideInInspector]
	public Transform target;
	[HideInInspector]
	public float currentCooldown;

	private void Start() {

	}

	private void Update() {
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0)
			Destroy(gameObject);

		transform.position += transform.right * speed * Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D _col) {
		if(target == null) {
			Entity e = _col.transform.GetComponent<Entity>();
			if(e != null)
				OnHitTarget(e);

			return;
		}

		if(_col.transform == target)
			OnHitTarget(_col.transform.GetComponent<Entity>());
	}

	private void OnHitTarget(Entity _e) {
		_e.OnAttacked(damage);
	}

}
