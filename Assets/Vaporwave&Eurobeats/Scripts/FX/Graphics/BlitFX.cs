using UnityEngine;

namespace Scripts.FX.Graphics {
    [ExecuteInEditMode]
    public class BlitFX : VisualFX {
        public Material Material;

        public override Material GetMaterial() {
            return Material;
        }
    }
}