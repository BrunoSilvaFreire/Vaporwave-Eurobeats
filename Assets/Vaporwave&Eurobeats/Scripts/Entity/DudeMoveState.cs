﻿using System.Collections;
using System.Collections.Generic;
using Shiroi.FX.Effects;
using UnityEngine;

public class DudeMoveState : MoveState {
	

	public float MoveSpeed;
	public float JumpForce;
	public float AirDrag;
	public float GroundDrag;
	public float ShootForce;
	public float GranadeForce;
	public float SpreadAmount;
	public float RecoilForce;
	public float AttackSpeed;
	public float SuccSpeed;
	public float SuccArea;
	public int MaximumStorage;
	public int MinimumStorage;
	public float FallMultiplier = 2.5f;
	public float AimAssistRadius = 10;
	public int CubeStorage = 0;
	public float GravityScale = 1.0f;

	public WorldEffect WeaponEffect;
	
	public List<GameObject> Dudes { get; set; }
	
	public int CursorDirection { get; set; }
	
	public int Direction { get; set; }

	public bool WeaponDraw { get; set; }
	
	public bool Shooting { get; set; }
	
	public bool Granade { get; set; }
	
	public bool Succ { get; set; }

	public float AttackCooldown { get; set; } = 0f;

	public float SuccCooldown { get; set; }

	public Transform Cursor { get; set; }

	public Transform Tr { get; set; }

	public Rigidbody Rb { get; set; }
}
