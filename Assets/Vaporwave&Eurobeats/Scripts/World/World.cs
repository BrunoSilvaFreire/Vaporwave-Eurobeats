using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Scripts.World {
    public class World : MonoBehaviour {
        //Quantos chunks
        public byte Width;

        //Quantos chunks
        public byte Height;
        public byte ChunkSize;
        public byte ChunkHeight;
        public bool RandomSeed;
        public int Seed;
        public WorldGenerator Generator;

        public Material ChunkMaterial;

        [ShowInInspector]
        public void Generate() {
            ClearPrevious();

            Seed = RandomSeed ? Random.Range(int.MinValue, int.MaxValue) : Seed;
            var data = Generator.Generate(this, Seed);
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    var chunkGO = new GameObject($"Chunk ({x},{y})");
                    var chunk = chunkGO.AddComponent<Chunk>();
                    chunkGO.transform.parent = transform;
                    chunk.transform.position = new Vector3(x * ChunkSize, 0, y * ChunkSize);
                    var chunkData = data[x, y];
                    InitializeChunk(chunk, chunkGO, chunkData);
                }
            }
        }

        [ShowInInspector]
        public void ClearPrevious() {


            var childs = transform.childCount - 1;
            for (var i = childs; i >= 0; i--) {

            #if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                    continue;
                }
            #endif
                Destroy(transform.GetChild(i).gameObject);
            }
        }


        private void InitializeChunk(Chunk chunk, GameObject go, ChunkData chunkData) {
            var col = go.AddComponent<MeshCollider>();
            var filter = go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>().material = ChunkMaterial;
            chunk.Collider = col;
            chunk.Filter = filter;
            chunk.LoadMesh(chunkData);
        }
    }
}

