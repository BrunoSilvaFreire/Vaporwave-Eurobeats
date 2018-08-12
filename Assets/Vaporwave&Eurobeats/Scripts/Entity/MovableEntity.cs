using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public abstract class Motor : ScriptableObject {

    public abstract void Setup(MovableEntity entity, MoveState state);
    public abstract void Init(MovableEntity entity, MoveState state);
    public abstract void Tick(MovableEntity entity, MoveState state);
    public abstract void FixedTick(MovableEntity entity, MoveState state);
}

public sealed class MovableEntity : MonoBehaviour {

    public byte PlayerNumber;
    
    public Motor Motor;
    public MoveState State;
    public Player Input { get; private set; }
    
    private void Awake() {
        Input = ReInput.players.GetPlayer(PlayerNumber);
        Motor.Setup(this, State);
    }

    private void Start() {
        Motor.Init(this, State);
    }
    
    private void Update() {
        Motor.Tick(this, State);
    }

    private void FixedUpdate() {
        Motor.FixedTick(this, State);
    }
}