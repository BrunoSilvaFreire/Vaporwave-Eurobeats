using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveState : MonoBehaviour {
    public bool Grounded { get; private set; }

    private float _colHalfHeight;
    private Collider _col;

    private void Awake() {
        _col = GetComponent<Collider>();
    }
    
    private void Start() {
        
        _colHalfHeight = _col.bounds.size.y / 2;
    }

    private void Update() {
        Grounded = Physics.Raycast(transform.position, Vector3.down, _colHalfHeight + 0.1f);
    }
}