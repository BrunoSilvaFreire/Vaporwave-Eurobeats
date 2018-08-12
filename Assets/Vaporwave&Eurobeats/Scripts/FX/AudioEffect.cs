using Shiroi.FX.Effects;
using Sirenix.Utilities;
using UnityEngine;
using UnityUtilities;

namespace Scripts.FX {
    public class AudioEffect : WorldEffect {
        public AudioClip[] Clips;
        public float Volume;

        public override void Execute(Vector3 position) {
            if (Clips.IsNullOrEmpty()) {
                Debug.LogError("Can't play audio effect without any clips!");
                return;
            }

            AudioSource.PlayClipAtPoint(Clips.RandomElement(), position, Volume);
        }
    }
}