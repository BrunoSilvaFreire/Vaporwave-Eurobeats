using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.FX.Graphics {
    [ExecuteInEditMode]
    public abstract class PostCameraFX<T> : MonoBehaviour {
        public static readonly string ShaderName = $"PostFX/{typeof(T).Name}";

        [ShowInInspector, ReadOnly]
        protected Material material;

        [Button]
        private void Reset() {
            material = null;
        }
        

        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (material == null) {
                Debug.Log("Creating new material with shader " + ShaderName);
                material = new Material(Shader.Find(ShaderName));
            }

            UnityEngine.Graphics.Blit(src, dest, material);
        }
    }
}