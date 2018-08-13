using System.Collections.Generic;
using UnityEngine;
using UnityUtilities.Singletons;

namespace Scripts.Characters {
    [CreateAssetMenu(menuName = "VE/Characters/CharacterDatabase")]
    public class CharacterDatabase : ScriptableSingleton<CharacterDatabase> {
        public List<Character> Characters;
    }
}