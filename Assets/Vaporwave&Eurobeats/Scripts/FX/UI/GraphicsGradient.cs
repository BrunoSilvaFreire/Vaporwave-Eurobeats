using UnityEngine;
using UnityEngine.UI;

namespace Scripts.FX.UI {
    [ExecuteInEditMode]
    public class GraphicsGradient : MonoBehaviour {
        public Graphic Graphic;
        public Gradient Color;
        private float position;
        public float Speed;
        private void Update() {
            position += Time.deltaTime * Speed;
            if (position > 1) {
                --position;
            }

            Graphic.color = Color.Evaluate(position);
        }
    }
}