using Scripts.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    [ExecuteInEditMode]
    public class UICharacterPreviewView : UIView {
        public const string ShaderName = "VE/CharacterPreviewShader";

        [SerializeField]
        private Character character;

        public Image PreviewImage;
        public Text AliasLabel;

        public Character Character {
            get {
                return character;
            }
            set {
                character = value;
                UpdateView();
            }
        }

        private Material dudeMaterial;

        private void OnValidate() {
            UpdateView();
        }

        private void UpdateView() {
            CheckDudeMaterial();
            if (character == null) {
                return;
            }

            dudeMaterial.SetFloat("_HueShift", character.HueShift);
        }

        private void CheckDudeMaterial() {
            if (dudeMaterial == null) {
                dudeMaterial = new Material(Shader.Find(ShaderName));
            }

            PreviewImage.material = dudeMaterial;
            AliasLabel.text = character.Alias;
        }
    }
}