using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vaporwave&Eurobeats/Motors/CursorMotor")]
public class CursorMotor : Motor {

	[SerializeField] private float _range;
	
	public override void Setup(MovableEntity entity, MoveState state) {
		
	}

	public override void Init(MovableEntity entity, MoveState state) {

	}

	public override void Tick(MovableEntity entity, MoveState state) {
		var hori = entity.Input.GetAxis("CursorHorizontal");
		var vert = entity.Input.GetAxis("CursorVertical");

		entity.transform.localPosition = entity.transform.localPosition.normalized / 2 +
			Vector3.Lerp(entity.transform.localPosition, new Vector3(hori, 0, vert).normalized * _range, 0.5f);

	}

	public override void FixedTick(MovableEntity entity, MoveState state) {
	
	}
}
