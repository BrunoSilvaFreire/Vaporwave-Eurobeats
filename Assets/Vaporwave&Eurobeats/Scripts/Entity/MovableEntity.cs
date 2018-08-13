using Rewired;
using Scripts.Characters;
using UnityEngine;

public abstract class Motor : ScriptableObject {
    public abstract void Setup(MovableEntity entity, MoveState state);
    public abstract void Init(MovableEntity entity, MoveState state);
    public abstract void Tick(MovableEntity entity, MoveState state);
    public abstract void FixedTick(MovableEntity entity, MoveState state);
}

public sealed class MovableEntity : MonoBehaviour {
    [SerializeField]
    private byte playerNumber;

    public Motor Motor;
    public MoveState State;

    public Player Input {
        get;
        private set;
    }

    public byte PlayerNumber {
        get {
            return playerNumber;
        }
        set {
            playerNumber = value;
            Input = ReInput.players.GetPlayer(PlayerNumber);
        }
    }

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

    public void LoadCharacter(Character character) {
        
    }
}