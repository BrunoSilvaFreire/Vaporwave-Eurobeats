#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityUtilities;

namespace Scripts.UI {
    [ExecuteInEditMode]
    public class UIStorageView : UIView {
        public Image FillImage;
        public Image EmptyStorage;
        public MovableEntity Entity;
        public AudioClip Clip;
        public AudioSource Alarm;
        public float FalldownPosition = 0.25F;
        public float AlarmLength;
        public AnimationCurve AlarmColorCurve;
        public Color AlarmColor;

        private void Update() {
            if (Entity == null || FillImage == null) {
                Debug.LogError($"Entity: {Entity}, Image: {FillImage}", this);
                return;
            }

            var state = Entity.State as DudeMoveState;
            if (state == null) {
                Debug.LogError($"No DudeMoveState", this);
                return;
            }

            var val = (float) state.CubeStorage / state.MaximumStorage;
            UpdateStorageAlarm(val);
            FillImage.fillAmount = val;
        }

        private float currentAlarmTime;

        private void UpdateStorageAlarm(float val) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                Alarm.Stop();
                return;
            }
#endif
            var empty = val <= 0;
            if (!empty) {
                EmptyStorage.SetAlpha(AlarmColorCurve.Evaluate(val));
                return;
            }

            if (currentAlarmTime > AlarmLength || currentAlarmTime <= 0) {
                currentAlarmTime %= AlarmLength;
                Alarm.PlayOneShot(Clip);
            }

            currentAlarmTime += Time.deltaTime;

            var falldownPosition = AlarmLength * FalldownPosition;
            var currentAlarmPos = currentAlarmTime / AlarmLength;
            var c = AlarmColor;
            if (currentAlarmPos > falldownPosition) {
                c.a = (AlarmLength - currentAlarmPos) / (AlarmLength - falldownPosition);
            } else {
                c.a = 1;
            }

            EmptyStorage.color = c;
        }
    }
}