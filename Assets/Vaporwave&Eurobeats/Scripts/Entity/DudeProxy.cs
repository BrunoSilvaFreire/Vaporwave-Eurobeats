using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeProxy : MonoBehaviour {

	private Animator _anima;

	private DudeMoveState _dude;
	
	private Rigidbody _rb;

	private void Awake() {
		_anima = GetComponent<Animator>();
	}

	private void Start() {
		_rb = GetComponentInParent<Rigidbody>();
		_dude = GetComponentInParent<DudeMoveState>();
	}

	private void LateUpdate() {
		_anima.SetFloat("MoveSpeed", _rb.velocity.sqrMagnitude);
		_anima.SetInteger("Direction", _dude.Direction);
		_anima.SetInteger("CursorDirection", _dude.CursorDirection);
		_anima.SetInteger("VerticalSpeed", Mathf.RoundToInt(_rb.velocity.y));
		_anima.SetBool("Shooting", _dude.Shooting);
		_anima.SetBool("Succ", _dude.Succ);
		_anima.SetBool("Granade", _dude.Granade);
	}

	public void DrawEnded() {
		_dude.WeaponDraw = true;
	}
}
