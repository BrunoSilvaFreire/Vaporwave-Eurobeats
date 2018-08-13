using Scripts.FX.Audio;
using Shiroi.FX.Effects;
using UnityEngine;
using UnityUtilities;

namespace Scripts.FX {
    public class AudioEffect : WorldEffect {
        public AudioClip[] Clips;
        public float Volume;
        public float PitchMin;
        public float PitchMax;

        public override void Execute(Vector3 position) {
            if (Clips.Length == 0) {
                Debug.LogError("Can't play audio effect without any clips!");
                return;
            }

            var pitch = PitchMin + (PitchMax - PitchMin * Random.value);
            AudioManager.Instance.PlayAudio(Clips.RandomElement(), position, Volume, pitch);
        }
    }
}