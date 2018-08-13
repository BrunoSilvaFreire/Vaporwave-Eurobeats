using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.Singletons;

public class CameraController : Singleton<CameraController> {
    
    public float MinZoom, MaxZoom;

    private Coroutine _moveRoutine;
    private Transform _camera;
    private Transform[] _player, _cursor;

    private bool _moving;

    private void Awake() {
        _camera = Camera.main.transform;
    }

    private void Start() {
        SearchTargets();
    }

    private void Update() {

        if (_moving)
            return;
        
        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), 0.1f);
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, -_camera.forward * GetTargetZoom(), 0.1f);
    }


    public void MoveToPosition(Vector3 position, Quaternion rotation, float timeToMove) {
        if (_moveRoutine != null) {
            StopCoroutine(_moveRoutine);
        }

        _moveRoutine = StartCoroutine(DoMoveToPosition(position, rotation, timeToMove));

    }

    private IEnumerator DoMoveToPosition(Vector3 position, Quaternion rotation, float timeToMove) {
        _moving = true;
        var lastPos = transform.position;
        var lastRotation = transform.rotation;
        float timer = 0;
        while (timer < timeToMove) {
            transform.position = Vector3.Lerp(lastPos, position, timer / timeToMove);
            transform.rotation = Quaternion.Slerp(lastRotation, rotation, timer / timeToMove);
            timer += Time.deltaTime;
            yield return null;
        }
        _moving = false;
        transform.position = position;
        transform.rotation = rotation;
    }

    private float GetTargetZoom() {
        float dist = 0;
        if (_player.Length == 1) {
            dist = (_player[0].position - _cursor[0].position).sqrMagnitude;
        } else if (_player.Length > 1) {
            var midPoints = new Vector3[_player.Length];

            Vector3 realTarget = Vector3.zero;
            for (int i = 0; i < midPoints.Length; i++) {
                midPoints[i] = (_player[i].position - _cursor[i].position) * 0.5f + _player[i].position;
            }

            for (int i = 0; i < midPoints.Length; i++) {
                for (int j = 0; j < midPoints.Length; j++) {
                    if (j <= i)
                        continue;

                    var tempDist = (midPoints[i] - midPoints[j]).sqrMagnitude;
                    if (tempDist > dist) {
                        dist = tempDist;
                    }
                }
            }
        }

        return Mathf.Lerp(MinZoom, MaxZoom, Mathf.Sqrt(dist) / 20);
    }

    private Vector3 GetTargetPosition() {
        var midPoints = new Vector3[_player.Length];

        Vector3 realTarget = Vector3.zero;
        for (int i = 0; i < midPoints.Length; i++) {
            midPoints[i] = (_cursor[i].position - _player[i].position) * 0.5f + _player[i].position;
            realTarget += midPoints[i];
        }

        return realTarget /= midPoints.Length;
    }

    private void SearchTargets() {
        var go = GameObject.FindGameObjectsWithTag("Player");
        _player = new Transform[go.Length];
        _cursor = new Transform[go.Length];
        for (int i = 0; i < go.Length; i++) {
            _player[i] = go[i].transform;
            _cursor[i] = _player[i].Find("Cursor");
        }
    }
}