using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.World;
using Scripts.World.Selection;
using Scripts.World.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

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

		dude.Dudes = GameObject.FindGameObjectsWithTag("Player").ToList();

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


		if (entity.Input.GetButton("Shoot")) {
			dude.Shooting = true;
		}
		else if (entity.Input.GetButton("Succ")) {
			dude.Succ = true;
		}
		else if (entity.Input.GetButton("Granade")) {
			dude.Granade = true;
		}
		else {
			dude.SuccCooldown = 0;
			dude.Succ = false;
			dude.Shooting = false;
			dude.Granade = false;
			dude.WeaponDraw = false;
		}
		
		dude.AttackCooldown += Time.deltaTime;
		if (dude.WeaponDraw) {
			if (dude.Succ) {
				dude.SuccCooldown += Time.deltaTime;
				Succ(dude);
			}
				
			if (dude.Shooting) {
				Shoot(dude);
			}

			if (dude.Granade) {
				Granade(dude);
			}
		}
		dude.CursorDirection = dude.Cursor.localPosition.x > 0 ? 1 : 0;

		if (!dude.Succ) {
			var hori = entity.Input.GetAxis("Horizontal");
			var vert = entity.Input.GetAxis("Vertical");
			if (hori != 0) {
				dude.Direction = hori > 0 ? 1 : 0;
			}
		
			Move(dude, hori, vert);
		}
		
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

		RaycastHit hit;
		if (Physics.Raycast(dude.transform.position + Vector3.up, (dude.Cursor.position - dude.transform.position).normalized, out hit,
			(dude.Cursor.position - dude.transform.position).magnitude, ~((1 << 10) & (1 << 11)))) {
			var selection = Selections.SphereSelection(World.Instance, hit.point.ToVector3Int() + Vector3Int.down,
				dude.SuccArea);

			var tiles = selection.SolidTiles.ToList();

			var cubes = tiles.Count;
	
			dude.StartCoroutine(DoSucc(tiles, cubes));
		
		
			dude.CubeStorage += cubes;
			selection.DeleteAll();
		}
		
		
	}

	private IEnumerator DoSucc(List<WorldTile> tiles, int count) {
		for (int i = 0; i < count; i++) {
			_pool.SpawnFromPool("SuccCube", tiles[i].Position + Vector3.one / 2, Quaternion.identity);
			yield return null;
		}
	}

	private void Granade(DudeMoveState dude) {
		
		if (dude.AttackCooldown * dude.AttackSpeed > 1) {
			dude.AttackCooldown = 0;
			dude.WeaponEffect.Execute(dude.transform.position);
		} else
			return;
		
		if (dude.CubeStorage < dude.MaximumStorage * 0.8f)
			return;
		
		_fx.ScreenShake(0.1f, 0.25f);
		dude.CubeStorage = 0;

		var dir = (dude.Cursor.position - dude.Tr.position).normalized;
		var granade = _pool.SpawnFromPool("Granade", dude.Tr.position + dir, Quaternion.identity);
		granade.GetComponent<ProjectileBehaviour>().Shoot((dir + Vector3.up * 0.5f).normalized, dude.GranadeForce);
		
		dude.Rb.AddForce(-dir * dude.RecoilForce, ForceMode.Impulse);
	}

	private void Shoot(DudeMoveState dude) {


		if (dude.AttackCooldown * dude.AttackSpeed > 1) {
			dude.AttackCooldown = 0;
			dude.WeaponEffect.Execute(dude.transform.position);
		} else
			return;
			
		
		if (dude.CubeStorage < dude.MinimumStorage)
			return;

		dude.CubeStorage--;
		_fx.ScreenShake(0.1f, 0.25f);

		var closest = GetClosestPlayer(dude, dude.Cursor.position);

		var target = closest != Vector3.zero ? closest : dude.Cursor.position;
		
		var dir = ((target - dude.Tr.position + (dude.Cursor.position - dude.Tr.position)).normalized + Random.insideUnitSphere * dude.SpreadAmount).normalized;
		var projectile = _pool.SpawnFromPool("Projectile", dude.Tr.position + dir, Quaternion.identity);
		projectile.GetComponent<ProjectileBehaviour>().Shoot(dir, dude.ShootForce);
		

		dude.Rb.AddForce(-dir * dude.RecoilForce, ForceMode.Impulse);
	}

	private Vector3 GetClosestPlayer(DudeMoveState dude,Vector3 center) {

		var closest = Vector3.zero;
		float minDist = dude.AimAssistRange * dude.AimAssistRange;
		foreach (var d in dude.Dudes) {
			if (d.gameObject == dude.gameObject || !d.gameObject.activeInHierarchy)
				continue;
		
			var dist = (Camera.main.WorldToScreenPoint(center) - Camera.main.WorldToScreenPoint(d.transform.position)).sqrMagnitude;
			if (dist < minDist) {
				
				minDist = dist;
				closest = d.transform.position;
			}
		}

		return closest;
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
