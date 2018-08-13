using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeProxy : MonoBehaviour {

	private Animator _anima;

	private Rigidbody _rb;

	private void Awake() {
		_anima = GetComponent<Animator>();
	}

	private void Start() {
		_rb = GetComponentInParent<Rigidbody>();
	}

	private void LateUpdate() {
		_anima.SetFloat("MoveSpeed", _rb.velocity.sqrMagnitude);
		_anima.SetInteger("Direction", _rb.velocity.x > 0 ? 1 : 0);
		_anima.SetInteger("VerticalSpeed", Mathf.RoundToInt(_rb.velocity.y));
	}
}
