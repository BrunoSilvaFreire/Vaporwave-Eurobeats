using Scripts.World.Utilities;
using UnityEngine;

namespace Scripts.World {
    public class Chunk : MonoBehaviour {
        public MeshFilter Filter;
        public MeshCollider Collider;
        private ChunkData data;

        public void LoadMesh(ChunkData data) {
            this.data = data;
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

                        if (IsTransparent(blocks, x - 1, y, z))
                            builder.AddFace(new Vector3(x, y, z), Vector3.up, Vector3.forward, false);
                        // Right wall
                        if (IsTransparent(blocks, x + 1, y, z))
                            builder.AddFace(new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true);

                        // Bottom wall
                        if (IsTransparent(blocks, x, y - 1, z))
                            builder.AddFace(new Vector3(x, y, z), Vector3.forward, Vector3.right, false);
                        // Top wall
                        if (IsTransparent(blocks, x, y + 1, z))
                            builder.AddFace(new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true);

                        // Back
                        if (IsTransparent(blocks, x, y, z - 1))
                            builder.AddFace(new Vector3(x, y, z), Vector3.up, Vector3.right, true);
                        // Front
                        if (IsTransparent(blocks, x, y, z + 1))
                            builder.AddFace(new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false);
                        /*var block = blocks[x, y, z];
                        var position = new Vector3Int(x, y, z);
                        foreach (var face in BlockFaceExtensions.All) {
                            /*var neightboor = GetNeightboor(blocks, position, face);
                            if (neightboor == null) {
                                continue;
                            }

                            var neightboorBlock = neightboor.Value;
                            if (neightboorBlock == block) {
                                continue;
                            }

                            var positionF = (Vector3) position;
                            builder.AddFace(positionF + face.ToVector3(), position + face.GetUp(),
                                position + face.GetRight());#1#

                            // Left wall
                           
                        }*/
                    }
                }
            }

            var mesh = builder.Build();

            Filter.sharedMesh = mesh;
            Collider.sharedMesh = mesh;
        }

        private static bool IsTransparent(BlockMaterial[,,] blocks, int p1, int p2, int p3) {
            if (IsOutOfBounds(new Vector3Int(p1, p2, p3), blocks)) {
                return true;
            }

            return blocks[p1, p2, p3] == BlockMaterial.Empty;
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
    }
}