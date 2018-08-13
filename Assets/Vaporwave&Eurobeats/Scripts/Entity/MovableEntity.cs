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
    public const  float Glossiness = 1.25F;
    public const  float Emission = .2F;
    [SerializeField]
    private byte playerNumber;

    public Motor Motor;
    public MoveState State;
    public SpriteRenderer Renderer;

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
            Input = ReInput.players.GetPlayer(value);
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
        this.character = character;
        UpdateView();
    }

    private const string ShaderName = "Custom/EntityShader";
    private Material dudeMaterial;
    private Character character;

    private void UpdateView() {
        CheckDudeMaterial();
        if (character == null) {
            return;
        }

        dudeMaterial.SetFloat("_HueShift", character.HueShift);
    }

    private void CheckDudeMaterial() {
        if (dudeMaterial == null) {
            dudeMaterial = new Material(Shader.Find(ShaderName));
            dudeMaterial.SetFloat("_Metallic", Glossiness);
            dudeMaterial.SetFloat("_EmissionScale", Emission);
        }

        Renderer.material = dudeMaterial;
    }
}