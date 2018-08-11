using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public abstract class Motor : ScriptableObject {

    public abstract void Setup(MovableEntity entity);
    public abstract void Init(MovableEntity entity);
    public abstract void Tick(MovableEntity entity);
    public abstract void FixedTick(MovableEntity entity);
}

public class MoveState : MonoBehaviour {
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

public sealed class MovableEntity : MonoBehaviour {

    public byte PlayerNumber;
    
    public Motor Motor;
    
    public Player Input { get; private set; }
    
    private void Awake() {
        Input = ReInput.players.GetPlayer(PlayerNumber);
        Motor.Setup(this);
    }

    private void Start() {
        Motor.Init(this);
    }
    
    private void Update() {
        Motor.Tick(this);
    }

    private void FixedUpdate() {
        Motor.FixedTick(this);
    }
}