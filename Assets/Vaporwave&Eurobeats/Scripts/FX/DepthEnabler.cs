using UnityEngine;

namespace Scripts.FX {
    [ExecuteInEditMode]
    public class DepthEnabler : MonoBehaviour {
        private void OnEnable() {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        }
        private void OnDisable() {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
        }
    }
}