using Rewired;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.UI {
    public class UIAnyButtonTrigger : MonoBehaviour {
        public UnityEvent OnAnyButtonPressed;
        public bool DestroyOnPressed;

        private void Update() {
            if (!ReInput.controllers.GetAnyButton()) {
                return;
            }

            OnAnyButtonPressed.Invoke();
            if (DestroyOnPressed) {
                Destroy(this);
            }
        }
    }
}