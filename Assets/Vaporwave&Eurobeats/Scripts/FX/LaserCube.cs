using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCube : MonoBehaviour, IPooledObject {


	public float Lifetime;
	public float TimeToFade;

	private bool _fading;
	private float _timeAlive;

	private void Update() {

		if (_fading)
			return;
		
		_timeAlive += Time.deltaTime;
		if (_timeAlive > Lifetime)
			StartCoroutine(FadeIntoOblivion());
	}

	private IEnumerator FadeIntoOblivion() {
		_fading = true;
		var timer = 0f;
		while (timer < TimeToFade) {
			transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / TimeToFade);
			timer += Time.deltaTime;
			yield return null;
		}

		transform.localScale = Vector3.one;
		_fading = false;
		gameObject.SetActive(false);
	}

	
	public void OnSpawn() {
		_timeAlive = 0;
	}
}
