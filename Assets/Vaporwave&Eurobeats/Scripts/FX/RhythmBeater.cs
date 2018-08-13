using UnityEngine;

namespace Scripts.FX {
    [ExecuteInEditMode]
    public class RhythmBeater : MonoBehaviour {
        public float BPM;
        public AnimationCurve Curve;
        public float Scale = 5;
        public AnimationCurve FOVCurve = AnimationCurve.Linear(0, 65, 60, 0);
        public Light Light;
        public Camera Camera;
        private float currentTime;

        private void Update() {
            var secondsPerBeat = 60 / BPM;
            currentTime += Time.deltaTime;
            if (currentTime > secondsPerBeat) {
                currentTime %= secondsPerBeat;
            }

            var pos = currentTime / secondsPerBeat;
            Light.intensity = Curve.Evaluate(pos) * Scale;
            Camera.fieldOfView = FOVCurve.Evaluate(pos);
        }
    }
}