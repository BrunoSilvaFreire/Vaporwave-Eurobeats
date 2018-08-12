using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.FX.PostFX {
    [ExecuteInEditMode]
    public class HorizontalLines : PostCameraFX<HorizontalLines> {
        [SerializeField, HideInInspector]
        private float lineHeight;

        [SerializeField, HideInInspector]
        private float lineMoveSpeed;

        [SerializeField, HideInInspector]
        private float hardness;
        [ShowInInspector]
        public float Hardness {
            get {
                return hardness;
            }
            set {
                hardness = value;
                material.SetFloat("_Hardness", value);
            }
        }
        [ShowInInspector]
        public float LineHeight {
            get {
                return lineHeight;
            }
            set {
                lineHeight = value;
                material.SetFloat("_LineWidth", value);
            }
        }

        [ShowInInspector]
        public float LineMoveSpeed {
            get {
                return lineMoveSpeed;
            }
            set {
                lineMoveSpeed = value;
                material.SetFloat("_LineMoveSpeed", value);
            }
        }
    }
}