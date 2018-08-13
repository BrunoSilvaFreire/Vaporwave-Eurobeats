using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccCube : MonoBehaviour, IPooledObject {

	public float TimeToFade = 1;
	
	public void OnSpawn() {
		var players = Physics.OverlapSphere(transform.position, 50, 1 << 10);
		float maxDist = float.MaxValue;
		Transform target = transform;
		foreach (var p in players) {
			var dist = (transform.position - p.transform.position).sqrMagnitude;
			if (dist < maxDist) {	
				maxDist = dist;
				target = p.transform;
			}
		}
		
		StartCoroutine(DoMoveTo(target));
	}


	private IEnumerator DoMoveTo(Transform target) {
		StartCoroutine(FadeIntoOblivion());
		float timer = 0;
		while (timer < TimeToFade) {
			transform.position = Vector3.Lerp(transform.position + Random.insideUnitSphere * 0.25f, target.position, timer / TimeToFade);
			timer += Time.deltaTime;
			yield return null;
		}
	}
	
	private IEnumerator FadeIntoOblivion() {
	
		var timer = 0f;
		while (timer < TimeToFade) {
			transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / TimeToFade);
			timer += Time.deltaTime;
			yield return null;
		}

		transform.localScale = Vector3.one;
		gameObject.SetActive(false);
	}
}
