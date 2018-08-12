using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.Singletons;

public class ScreenEffects : Singleton<ScreenEffects> {

	private Coroutine _screenShakeRoutine;

	private Transform _camera;
	private Transform _cameraPivot;

	private Quaternion _lastRotation;
	
	private void Start () {
		_cameraPivot = CameraController.Instance.transform;
		_camera = Camera.main.transform;
	}
	
	private void Update () {
		
	}
	
	public void ScreenShake(float timeToShake, float intensity) {
		
		if (_screenShakeRoutine != null) {
			StopCoroutine(_screenShakeRoutine);
			_camera.rotation = _lastRotation;
		}		
		
		_screenShakeRoutine = StartCoroutine(DoScreenShake(timeToShake, intensity));
	}

	private IEnumerator DoScreenShake(float timeToShake, float intensity) {	
		
		_lastRotation = _camera.rotation;
		float shakeTimer = 0;
		while (shakeTimer < timeToShake) {
			var random = Random.insideUnitSphere * intensity;

			_cameraPivot.position += random;
			_camera.rotation = Quaternion.Euler(_camera.rotation.eulerAngles.x, _camera.rotation.eulerAngles.y,
				Mathf.Sin(shakeTimer * Mathf.PI * 16) * intensity * 5);
			intensity *= 0.9f;
			shakeTimer += Time.deltaTime;
			yield return null;
		}

		shakeTimer = 0;
		while (shakeTimer < 0.25f) {
			_camera.rotation = Quaternion.Lerp(_camera.rotation, _lastRotation, shakeTimer / 0.25f);
			shakeTimer += Time.deltaTime;
			yield return null;
		}
		_camera.rotation = _lastRotation;
	}
}
