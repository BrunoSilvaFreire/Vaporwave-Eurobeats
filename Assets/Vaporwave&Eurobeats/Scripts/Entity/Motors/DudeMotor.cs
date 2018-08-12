using System.Collections;
using System.Collections.Generic;
using Scripts.World;
using Scripts.World.Selection;
using Scripts.World.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Vaporwave&Eurobeats/Motors/DudeMotor")]
public class DudeMotor : Motor {

	private ObjectPooler _pool;
	private ScreenEffects _fx;

	public override void Setup(MovableEntity entity, MoveState state) {
		
		var dude = state as DudeMoveState;
		
		dude.Tr = entity.transform;
		
		dude.Rb = entity.gameObject.AddComponent<Rigidbody>();
		dude.Rb.freezeRotation = true;
		dude.Rb.useGravity = false;
		dude.Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

		_pool = ObjectPooler.Instance;
		_fx = ScreenEffects.Instance;
	}

	public override void Init(MovableEntity entity, MoveState state) {
		var dude = state as DudeMoveState;
		
		dude.Cursor = entity.transform.Find("Cursor");
	}

	public override void Tick(MovableEntity entity, MoveState state) { 

		var dude = state as DudeMoveState;
		
		if (dude.Grounded) {
			dude.Rb.drag = dude.GroundDrag;
			if (entity.Input.GetButtonDown("Jump")) {
				Jump(dude);
			}
		}
		else {
			dude.Rb.drag = dude.AirDrag;
		}


		
		
		if (entity.Input.GetButton("Succ")) {
			dude.SuccCooldown += Time.deltaTime;
			Succ(dude);
		}
		else if (entity.Input.GetButtonUp("Succ")) {
			dude.SuccCooldown = 0;
		}
		
		dude.AttackCooldown += Time.deltaTime;
		if (entity.Input.GetButton("Shoot")) {
			Shoot(dude);
		}
		
		var hori = entity.Input.GetAxis("Horizontal");
		var vert = entity.Input.GetAxis("Vertical");
		Move(dude, hori, vert);
	}

	public override void FixedTick(MovableEntity entity, MoveState state) {
		
		var dude = state as DudeMoveState;	
		
		if (dude.Rb.velocity.y < -5.5f || dude.Rb.velocity.y > 0 && !entity.Input.GetButton("Jump")) {
			dude.GravityScale = dude.FallMultiplier;
		}
		else {
			dude.GravityScale = 1;
		}
		
		ApplyGravity(dude);
	}

	private void ApplyGravity(DudeMoveState dude) {
		var gravity = Physics.gravity * (dude.GravityScale + 5);
		dude.Rb.AddForce(gravity, ForceMode.Acceleration);
	}

	private void Succ(DudeMoveState dude) {
		if (dude.CubeStorage > dude.MaximumStorage || dude.SuccCooldown * dude.SuccSpeed < 1)
			return;

		dude.SuccCooldown = 0;
		var selection = Selections.SphereSelection(World.Instance, dude.Cursor.position.ToVector3Int(), dude.SuccArea);
		dude.CubeStorage += selection.SolidTiles;
		selection.DeleteAll();
	}

	private void Shoot(DudeMoveState dude) {
		if (dude.CubeStorage < dude.MinimumStorage || dude.AttackCooldown * dude.AttackSpeed < 1)
			return;

		
		_fx.ScreenShake(0.1f, 0.25f);
		
		dude.AttackCooldown = 0f;
		dude.CubeStorage--;
		var dir = ((dude.Cursor.position - dude.Tr.position).normalized + Random.insideUnitSphere * dude.SpreadAmount).normalized;
		var projectile = _pool.SpawnFromPool("Projectile", dude.Tr.position + dir, Quaternion.identity);
		projectile.GetComponent<ProjectileBehaviour>().Shoot(dir, dude.ShootForce);
		

		dude.Rb.AddForce(-dir * dude.RecoilForce, ForceMode.Impulse);
	}
	
	private void Jump(DudeMoveState dude) {

		var momentum = dude.Rb.velocity * 4;
		
		dude.Rb.AddForce(momentum.x, dude.JumpForce, momentum.z, ForceMode.Impulse);
	}

	private void Move(DudeMoveState dude, float hori, float vert) {
		
		dude.Rb.AddForce(new Vector3(hori, 0, vert).normalized
		             * (dude.Grounded ? dude.MoveSpeed : dude.MoveSpeed * 0.33f)
		             * Time.deltaTime,
			ForceMode.VelocityChange);
	}
}
