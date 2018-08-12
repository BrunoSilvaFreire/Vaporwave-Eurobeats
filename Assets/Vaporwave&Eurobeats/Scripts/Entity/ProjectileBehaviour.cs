﻿using System.Collections;
using System.Collections.Generic;
using Scripts.World;
using Scripts.World.Selection;
using Scripts.World.Utilities;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
    private Rigidbody _rb;
    private ObjectPooler _pool;
    public float DestructionArea = 5;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _pool = ObjectPooler.Instance;
    }


    public void Shoot(Vector3 dir, float force) {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * force, ForceMode.Impulse);
        _rb.AddTorque(Random.insideUnitSphere * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other) {

        _pool.SpawnFromPool("LaserExplosion", transform.position, Quaternion.identity);
        
        var selection = Selections.SphereSelection(World.Instance, transform.position.ToVector3Int(), DestructionArea);
        selection.DeleteAll();
        gameObject.SetActive(false);
    }
}