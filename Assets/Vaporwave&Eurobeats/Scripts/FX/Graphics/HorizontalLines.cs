using UnityEngine;

namespace Scripts.FX.Graphics {
    [ExecuteInEditMode]
    public class HorizontalLines : PostCameraFX<HorizontalLines> {
        [SerializeField, ]
        private float lineHeight;

        [SerializeField, ]
        private float lineMoveSpeed;

        [SerializeField, ]
        private float hardness;

        private void OnValidate() {
            material.SetFloat("_LineMoveSpeed", lineMoveSpeed);
            material.SetFloat("_LineWidth", lineHeight);
            material.SetFloat("_Hardness", hardness);
        }

        private void Start() {
            material = new Material(Shader.Find(ShaderName));
            material.SetFloat("_LineMoveSpeed", lineMoveSpeed);
            material.SetFloat("_LineWidth", lineHeight);
            material.SetFloat("_Hardness", hardness);
        }
    }
}