using UnityEngine;

namespace Scripts.Characters {
    [CreateAssetMenu(menuName = "VE/Characters/Character")]
    public class Character : ScriptableObject {
        public Color UIColor;
        public float HueShift;
        public string Alias = "Dude McGroove";
    }
}