using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.World {
    public class World : MonoBehaviour {
        //Quantos chunks
        public byte Width;

        //Quantos chunks
        public byte Height;
        public byte ChunkSize;
        public byte ChunkHeight;
        public WorldGenerator Generator;
        public Material ChunkMaterial;
        [ShowInInspector]
        public void Generate() {
            var data = Generator.Generate(this);
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