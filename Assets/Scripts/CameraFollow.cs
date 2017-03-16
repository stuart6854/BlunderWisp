using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public float trackingTime = 1.0f;
	
	// Update is called once per frame
	void LateUpdate () {
		if(target == null)
			return;

		transform.DOMove(target.position + offset, trackingTime);
	}
}
