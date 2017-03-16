using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class KillZone : MonoBehaviour {
	
	private void OnTriggerEnter2D(Collider2D _col) {
		Entity e = _col.gameObject.GetComponent<Entity>();
		if (e)
			e.OnAttacked(e.maxHealth);
	}

	private void OnDrawGizmos() {
		Bounds bounds = GetComponent<Collider2D>().bounds;

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, bounds.size);
	}
}
