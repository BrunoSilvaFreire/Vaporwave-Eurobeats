using UnityEditor;
using UnityEngine;

namespace Scripts.UI {
    public abstract class UIElement : UIView {
        public bool Showing {
            get {
                return overrideShowing ? overrideShowingValue : showing;
            }
            set {
                if (value == showing) {
                    return;
                }
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    SnapShowing(value);
                    return;
                }
#endif
                showing = value;
                if (!overrideShowing) {
                    UpdateState();
                }
            }
        }

        public bool AllowOverride = true;

        [SerializeField]
        private bool overrideShowing;

        [SerializeField]
        private bool overrideShowingValue;

        private void Start() {
            UpdateState();
        }

        public void Override(bool value) {
            if (!AllowOverride) {
                return;
            }

            if (!overrideShowing) {
                overrideShowing = true;
            }

            overrideShowingValue = value;
            UpdateState();
        }

        public void ReleaseOverride() {
            overrideShowing = false;
            UpdateState();
        }

        protected void UpdateState() {
            if (Showing) {
                OnShow();
            } else {
                OnHide();
            }
        }

        public void SnapShowing(bool show) {
            showing = show;
            if (show) {
                SnapShow();
            } else {
                SnapHide();
            }
        }

        [SerializeField]
        private bool showing;

        public void Show() {
            showing = true;
            OnShow();
        }

        public void Hide() {
            showing = false;
            OnHide();
        }

        private void OnValidate() {
            SnapShowing(Showing);
        }

        protected abstract void SnapShow();
        protected abstract void SnapHide();
        protected abstract void OnShow();
        protected abstract void OnHide();
    }


    public class UIView : MonoBehaviour { }
}