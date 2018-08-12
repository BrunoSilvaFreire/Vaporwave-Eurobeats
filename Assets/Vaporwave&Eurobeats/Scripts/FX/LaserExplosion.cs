using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserExplosion : MonoBehaviour, IPooledObject {

	public byte CubeCount = 5;
	public float ExplosionForce;
	public float ExplosionRadius;
	
	private ObjectPooler _pool;
	
	private void Start() {
		_pool = ObjectPooler.Instance;
	}

	public void OnSpawn() {
		for (int i = 0; i < CubeCount; i++) {
			_pool.SpawnFromPool("LaserCube", transform.position + Random.insideUnitSphere,
				Quaternion.Euler(Random.insideUnitSphere));
		}

		var cols = Physics.OverlapSphere(transform.position, ExplosionRadius, (1 << 11) | (1 << 10));
		foreach (var col in cols) {
			col.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 1f);
		}
	}
}
