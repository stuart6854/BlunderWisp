using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {

	public float cooldownTime = 1f;
	public int damage = 1;
	public float speed = 1f;
	public float lifeTime = 4f; // How much time till this spell object is destroyed.

	public LayerMask groundLayer;

	[HideInInspector]
	public Transform target;
	[HideInInspector]
	public float currentCooldown;

	private void Update() {
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0)
			Destroy(gameObject);

		transform.position += transform.right * speed * Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D _col) {
		if(_col.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			Destroy(gameObject);
			return;
		}

		if(target == null) {
			Entity e = _col.transform.GetComponent<Entity>();
			if(e != null) {
				OnHitTarget(e);
				Destroy(gameObject);
			}

			return;
		}

		if(_col.transform == target) {
			OnHitTarget(_col.transform.GetComponent<Entity>());
			Destroy(gameObject);
		}
	}

	private void OnHitTarget(Entity _e) {
		_e.OnAttacked(damage);
	}

}
