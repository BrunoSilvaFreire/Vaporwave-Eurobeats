using System;
using Scripts.World.Utilities;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Scripts.World {
    public class World : MonoBehaviour {
        public static World Instance {
            get;
            private set;
        }

        private Chunk[,] chunks;
        public LayerMask ChunkMask;
        public byte WorldWidth;
        public byte WorldDepth;
        public byte ChunkSize;
        public byte ChunkHeight;
        public bool RandomSeed;
        public int Seed;
        public WorldGenerator Generator;
        public Material ChunkMaterial;

        private void Start() {
            Instance = this;
            Generate();
        }

        public void Generate() {
            Clear();

            Seed = RandomSeed ? Random.Range(int.MinValue, int.MaxValue) : Seed;
            var data = Generator.Generate(this, Seed);
            chunks = new Chunk[WorldWidth, WorldDepth];
            for (var x = 0; x < WorldWidth; x++) {
                for (var y = 0; y < WorldDepth; y++) {
                    var chunkGO = new GameObject($"Chunk ({x},{y})");
                    var chunk = chunkGO.AddComponent<Chunk>();
                    chunkGO.transform.parent = transform;
                    chunk.transform.position = new Vector3(x * ChunkSize, 0, y * ChunkSize);
                    chunks[x, y] = chunk;
                    chunk.LoadData(this, data[x, y], x, y);
                }
            }

            for (var x = 0; x < WorldWidth; x++) {
                for (var y = 0; y < WorldDepth; y++) {
                    chunks[x, y].LoadMesh(this);
                }
            }
        }

        public void Clear() {
            chunks = null;
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


        public BlockMaterial GetBlock(Vector3Int pos) {
            if (IsOutOfBounds(pos)) {
                return BlockMaterial.Solid;
            }

            var chunkX = pos.x / ChunkSize;
            var chunkY = pos.z / ChunkSize;
            var chunk = chunks[chunkX, chunkY];
            if (chunk == null) {
                Debug.LogWarning($"Null chunk ({chunkX}, {chunkY}) {pos}, would access @ ({pos.x % ChunkSize}, {pos.y}, {pos.z % ChunkSize}");
                return BlockMaterial.Empty;
            }

            return chunk[pos.x % ChunkSize, pos.y, pos.z % ChunkSize];
        }

        public bool IsOutOfBounds(Vector3Int pos) {
            var sizeX = WorldWidth * ChunkSize;
            var sizeZ = WorldDepth * ChunkSize;
            return pos.x < 0 || pos.y < 0 || pos.z < 0 || pos.x >= sizeX || pos.y >= ChunkHeight || pos.z >= sizeZ;
        }

        public bool IsOutOfBounds(int x, int y, int z) {
            var sizeX = WorldWidth * ChunkSize;
            var sizeZ = WorldDepth * ChunkSize;
            return x < 0 || y < 0 || z < 0 || x >= sizeX || y >= ChunkHeight || z >= sizeZ;
        }

        public BlockMaterial GetBlockRelativeTo(Chunk chunk, int x, int y, int z) {
            return GetBlock(chunk.transform.position.ToVector3Int() + new Vector3Int(x, y, z));
        }

        public Chunk GetChunkAt(Vector3Int pos) {
            var chunkX = pos.x / ChunkSize;
            var chunkY = pos.z / ChunkSize;
            try {
                return chunks[chunkX, chunkY];
            } catch (Exception e) {
                Debug.LogError($"Chunk out pos of bounds @ {pos}, ({chunkX}, {chunkY})");
                throw;
            }
        }

        public Vector3Int ToLocalChunkPosition(Vector3Int pos) {
            return new Vector3Int(pos.x % ChunkSize, pos.y, pos.z % ChunkSize);
        }

        public BlockMaterial GetBlock(int x, int y, int z) {
            if (IsOutOfBounds(x, y, z)) {
                return BlockMaterial.Solid;
            }

            var chunkX = x / ChunkSize;
            var chunkY = z / ChunkSize;
            var chunk = chunks[chunkX, chunkY];
            return chunk == null ? BlockMaterial.Empty : chunk[x % ChunkSize, y, z % ChunkSize];
        }
    }
}