using Scripts.World.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.World {
    public class Chunk : MonoBehaviour {
        public MeshFilter Filter;
        public MeshCollider Collider;

        [ShowInInspector, ReadOnly]
        private ChunkData data;

        public Vector2Int CachedPos {
            get;
            private set;
        }


        public void LoadMesh(World world) {
            var blocks = data.blocks;
            var size = blocks.GetLength(0);
            var height = blocks.GetLength(1);
            var builder = new MeshBuilder();
            for (byte x = 0; x < size; x++) {
                for (byte y = 0; y < height; y++) {
                    for (byte z = 0; z < size; z++) {
                        if (blocks[x, y, z] == BlockMaterial.Empty) {
                            continue;
                        }

                        if (IsTransparent(world, x - 1, y, z)) {
                            builder.AddFace(new Vector3(x, y, z), Vector3.up, Vector3.forward, false);
                        }

                        if (IsTransparent(world, x + 1, y, z)) {
                            builder.AddFace(new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true);
                        }

                        if (IsTransparent(world, x, y - 1, z)) {
                            builder.AddFace(new Vector3(x, y, z), Vector3.forward, Vector3.right, false);
                        }

                        if (IsTransparent(world, x, y + 1, z)) {
                            builder.AddFace(new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true);
                        }

                        if (IsTransparent(world, x, y, z - 1)) {
                            builder.AddFace(new Vector3(x, y, z), Vector3.up, Vector3.right, true);
                        }

                        if (IsTransparent(world, x, y, z + 1)) {
                            builder.AddFace(new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false);
                        }

                        /*var position = new Vector3Int(x, y, z);
                        foreach (var face in BlockFaces.All) {
                            var neightboor = GetNeightboor(blocks, position, face);
                            if (neightboor == null) {
                                continue;
                            }

                            var neightboorBlock = neightboor.Value;
                            if (neightboorBlock == BlockMaterial.Empty) {
                                continue;
                            }

                            var positionF = (Vector3) position;
                            builder.AddFace(positionF + face.ToVector3(), position + face.GetUp(), position + face.GetRight(), face.IsReversed());

                            // Left wall
                           
                        }*/
                    }
                }
            }

            var mesh = builder.Build();
            Filter.sharedMesh = mesh;
            Collider.sharedMesh = mesh;
        }

        private bool IsTransparent(World world, int p1, int p2, int p3) {
            return world.GetBlockRelativeTo(this, p1, p2, p3) == BlockMaterial.Empty;
        }


        private static BlockMaterial? GetNeightboor(BlockMaterial[,,] blocks, Vector3Int offset, BlockFace face) {
            var position = offset + face.ToVector3();
            if (IsOutOfBounds(position, blocks)) {
                return null;
            }

            return blocks[position.x, position.y, position.z];
        }

        private static bool IsOutOfBounds(Vector3Int position, BlockMaterial[,,] blocks) {
            if (position.x < 0 || position.y < 0 || position.z < 0) {
                return true;
            }

            var size = blocks.GetLength(0);
            var height = blocks.GetLength(1);
            return position.x >= size || position.y >= height || position.z >= size;
        }

        public BlockMaterial this[int x, int y, int z] {
            get {
                return data[x, y, z];
            }
            set {
                data[x, y, z] = value;
            }
        }

        public void LoadData(World world, ChunkData chunkData, int x, int y) {
            var go = gameObject;
            var col = go.AddComponent<MeshCollider>();
            var filter = go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>().material = world.ChunkMaterial;
            Collider = col;
            Filter = filter;
            data = chunkData;
            CachedPos = new Vector2Int(x,y);
        }

        public BlockMaterial this[Vector3Int localPos] {
            get {
                return data[localPos];
            }
            set {
                data[localPos] = value;
            }
        }

    }
}