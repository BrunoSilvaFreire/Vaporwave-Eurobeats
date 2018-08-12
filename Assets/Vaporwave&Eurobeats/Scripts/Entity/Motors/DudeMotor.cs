using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vaporwave&Eurobeats/Motors/DudeMotor")]
public class DudeMotor : Motor {

	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _airDrag;
	[SerializeField] private float _groundDrag;
	[SerializeField] private float _shootForce;
	[SerializeField] private float _spreadAmount;
	[SerializeField] private float _recoilForce;
	[SerializeField] private float _attackSpeed;
	[SerializeField] private float _succRate;
	[SerializeField] private int _maximumStorage;
	[SerializeField] private int _minimumStorage;
	[SerializeField] private float _fallMultiplier = 2.5f;

	private float _gravityScale = 1.0f;
	private float _attackCooldown = 0f;
	private MoveState _state;
	private Transform _cursor;
	private Transform _tr;
	private Rigidbody _rb;
	private ObjectPooler _pool;

	public override void Setup(MovableEntity entity) {
		_tr = entity.transform;
		
		_rb = entity.gameObject.AddComponent<Rigidbody>();
		_rb.freezeRotation = true;
		_rb.useGravity = false;
		_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		
		_state = entity.gameObject.AddComponent<MoveState>();

		_pool = ObjectPooler.Instance;
	}

	public override void Init(MovableEntity entity) {
		_cursor = entity.transform.Find("Cursor");
	}

	public override void Tick(MovableEntity entity) {
		
		if (_state.Grounded) {
			_rb.drag = _groundDrag;
			if (entity.Input.GetButtonDown("Jump")) {
				Jump();
			}
		}
		else {
			_rb.drag = _airDrag;
		}


		_attackCooldown += Time.deltaTime;
		if (entity.Input.GetButton("Succ")) {
			Succ();
		}
		else if (entity.Input.GetButton("Shoot")) {
			Shoot();
		}
		
		var hori = entity.Input.GetAxis("Horizontal");
		var vert = entity.Input.GetAxis("Vertical");
		Move(hori, vert);
	}

	public override void FixedTick(MovableEntity entity) {
		if (_rb.velocity.y < -5.5f || _rb.velocity.y > 0 && !entity.Input.GetButton("Jump")) {
			_gravityScale = _fallMultiplier;
		}
		else {
			_gravityScale = 1;
		}
		
		ApplyGravity();
	}

	private void ApplyGravity() {
		var gravity = Physics.gravity * (_gravityScale + 5);
		_rb.AddForce(gravity, ForceMode.Acceleration);
	}

	private void Succ() {
		if (_cursor.localScale.x > _maximumStorage)
			return;

		;
		_cursor.localScale += Vector3.one * _succRate * Time.deltaTime;
	}

	private void Shoot() {
		if (_cursor.localScale.x < _minimumStorage || _attackCooldown * _attackSpeed < 1)
			return;

		_attackCooldown = 0f;
		_cursor.localScale -= Vector3.one * _succRate * 0.5f * Time.deltaTime;
		var dir = ((_cursor.position - _tr.position).normalized + Random.insideUnitSphere * _spreadAmount).normalized;

		var projectile = _pool.SpawnFromPool("Projectile", _tr.position + dir, Quaternion.identity);
		projectile.GetComponent<ProjectileBehaviour>().Shoot(dir, _shootForce);
		

		_rb.AddForce(-dir * _recoilForce, ForceMode.Impulse);
	}
	
	private void Jump() {

		var momentum = _rb.velocity * 4;
		
		_rb.AddForce(momentum.x, _jumpForce, momentum.z, ForceMode.Impulse);
	}

	private void Move(float hori, float vert) {
		
		_rb.AddForce(new Vector3(hori, 0, vert).normalized
		             * (_state.Grounded ? _moveSpeed : _moveSpeed * 0.33f)
		             * Time.deltaTime,
			ForceMode.VelocityChange);
	}
}
