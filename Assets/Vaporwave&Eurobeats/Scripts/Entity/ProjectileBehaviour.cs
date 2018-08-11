using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

	private Rigidbody _rb;

	private void Awake() {
		_rb = GetComponent<Rigidbody>();	
	}
	
	
	public void Shoot(Vector3 dir, float force) {
		_rb.velocity = Vector3.zero;
		_rb.AddForce(dir * force, ForceMode.Impulse);
		_rb.AddTorque(Random.insideUnitSphere * force, ForceMode.Impulse);
	}
}
