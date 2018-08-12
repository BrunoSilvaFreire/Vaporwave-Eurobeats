using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeMoveState : MoveState {
	

	public float MoveSpeed;
	public float JumpForce;
	public float AirDrag;
	public float GroundDrag;
	public float ShootForce;
	public float SpreadAmount;
	public float RecoilForce;
	public float AttackSpeed;
	public float SuccSpeed;
	public int MaximumStorage;
	public int MinimumStorage;
	public float FallMultiplier = 2.5f;
	public int CubeStorage = 0;
	public float GravityScale = 1.0f;


	public float AttackCooldown { get; set; } = 0f;

	public float SuccCooldown { get; set; }

	public Transform Cursor { get; set; }

	public Transform Tr { get; set; }

	public Rigidbody Rb { get; set; }
}
