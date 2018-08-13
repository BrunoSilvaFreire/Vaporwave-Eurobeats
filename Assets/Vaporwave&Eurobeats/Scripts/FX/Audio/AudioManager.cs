using System.Collections;
using UnityEngine;
using UnityUtilities.Singletons;

namespace Scripts.FX.Audio {
    public class AudioManager : Singleton<AudioManager> {
        public AudioSourcePool Pool;

        public void PlayAudio(AudioClip clip, Vector3 position, float volume, float pitch) {
            var audioSource = Pool.Get();
            StartCoroutine(PlayClip(clip, audioSource, position, volume, pitch));
        }

        private IEnumerator PlayClip(AudioClip clip, AudioSource audioSource, Vector3 position, float volume, float pitch) {
            audioSource.volume = volume;
            audioSource.transform.position = position;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
            Pool.Return(audioSource);
        }
    }
}