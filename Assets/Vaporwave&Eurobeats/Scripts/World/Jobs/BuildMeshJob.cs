using System;
using Scripts.World.Utilities;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Scripts.World.Jobs {
    public struct BuildMeshJob : IJob, IDisposable {
        private readonly ChunkData data;
        private readonly Vector3Int chunkOffset;
        public NativeArray<int>? Indices;
        public NativeArray<Vector3>? Vertices;
        public NativeArray<Vector2>? Uvs;

        public BuildMeshJob(ChunkData data, Vector3Int chunkOffset) {
            this.data = data;
            this.chunkOffset = chunkOffset;
            Indices = null;
            Vertices = null;
            Uvs = null;
        }

        public void Dispose() {
            Indices?.Dispose();
            Vertices?.Dispose();
            Uvs?.Dispose();
        }

        public void Execute() {
            var world = World.Instance;
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
            Dispose();
            Indices = new NativeArray<int>(builder.Indices.ToArray(), Allocator.Temp);
            Vertices = new NativeArray<Vector3>(builder.Vertices.ToArray(), Allocator.Temp);
            Uvs = new NativeArray<Vector2>(builder.Uvs.ToArray(), Allocator.Temp);
        }

        private bool IsTransparent(World world, int p1, int p2, int p3) {
            return world.GetBlock(chunkOffset.x + p1, chunkOffset.y + p2, chunkOffset.z + p3) == BlockMaterial.Empty;
        }
    }
}