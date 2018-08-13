using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.Singletons;

public class CameraController : Singleton<CameraController> {
    public float MinZoom, MaxZoom;

    private Transform _camera;
    private Transform[] _player, _cursor;

    private void Awake() {
        _camera = Camera.main.transform;
    }

    private void Start() {
        SearchTargets();
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), 0.1f);
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, -_camera.forward * GetTargetZoom(), 0.1f);
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

    public void SearchTargets() {
        var go = GameObject.FindGameObjectsWithTag("Player");
        _player = new Transform[go.Length];
        _cursor = new Transform[go.Length];
        for (int i = 0; i < go.Length; i++) {
            _player[i] = go[i].transform;
            _cursor[i] = _player[i].Find("Cursor");
        }
    }
}