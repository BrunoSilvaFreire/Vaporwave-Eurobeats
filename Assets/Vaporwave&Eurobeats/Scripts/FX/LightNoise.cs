using UnityEngine;

namespace Scripts.FX {
    [ExecuteInEditMode]
    public class LightNoise : MonoBehaviour {
        public Light Light;
        public float Intensity;
        public int OctavesCount;
        public float Amplitude;
        private float[] perlin;
        public int Width, Height;

        private void Start() {
            Reload();
        }

        private void Reload() {
            var matrix = PerlinNoise.Generate(Width, Height, OctavesCount, Amplitude, 0);
            var maxX = matrix.GetLength(0);
            var maxY = matrix.GetLength(1);
            perlin = new float[maxX * maxY];
            for (var i = 0; i < maxX; i++) {
                for (var j = 0; j < maxY; j++) {
                    var index = j * maxX + i;
                    perlin[index] = matrix[i, j];
                }
            }
        }

        private void OnValidate() {
            Reload();
        }

        private int current;

        private void Update() {
            if (current >= perlin.Length) {
                current %= perlin.Length;
            }

            Light.intensity = perlin[current++] * Intensity;
        }
    }
}