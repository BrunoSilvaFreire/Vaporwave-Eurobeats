using UnityEngine;

namespace Scripts.UI {
    public class UITransitioner : MonoBehaviour {
        public UIElement[] ToClose;
        public UIElement[] ToOpen;

        public void Transition() {
            foreach (var element in ToClose) {
                element.Hide();
            }

            foreach (var element in ToOpen) {
                element.Show();
            }
        }
    }
}