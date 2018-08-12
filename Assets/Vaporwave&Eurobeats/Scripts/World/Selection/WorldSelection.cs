using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.World.Selection {
    public struct WorldTile : IComparable<WorldTile> {
        public WorldTile(Vector3Int position, BlockMaterial material) {
            this.Position = position;
            this.Material = material;
        }

        public Vector3Int Position {
            get;
        }

        public BlockMaterial Material {
            get;
        }

        public int CompareTo(WorldTile other) {
            var otherPos = other.Position;
            if (Larger(Position, otherPos)) {
                return -1;
            }

            return Larger(otherPos, Position) ? 1 : 0;
        }

        private static bool Larger(Vector3Int a, Vector3Int b) {
            var chunkSize = World.Instance.ChunkSize;
            var chunkXA = a.x / chunkSize;
            var chunkYA = a.z / chunkSize;
            var chunkXB = b.x / chunkSize;
            var chunkYB = b.z / chunkSize;
            return chunkYA > chunkYB || chunkXA > chunkXB;
        }
    }


    public sealed class WorldSelection : IEnumerable<WorldTile> {
        private List<WorldTile> tiles;
        private World world;
        private Vector3Int min;
        private Vector3Int max;

        public int SolidTiles {
            get {
                int count = 0;
                foreach (var tile in tiles) {
                    count += tile.Material == BlockMaterial.Solid ? 1 : 0;
                }

                return count;
            }
        }

        public void DeleteAll() {
            SetAllTo(BlockMaterial.Empty);
        }

        public void SetAllTo(BlockMaterial material) {
            var total = tiles.Count;
            if (total <= 0) {
                Debug.LogWarning("Selection is empty!");
                return;
            }

            var i = 0;
            var chunks = new List<Chunk>();
            do {
                var tile = tiles[i++];
                if (tile.Material == material) {
                    continue;
                }

                var tilePos = tile.Position;
                CheckAdd(tilePos, chunks, Vector2Int.left);
                CheckAdd(tilePos, chunks, Vector2Int.right);
                CheckAdd(tilePos, chunks, Vector2Int.down);
                CheckAdd(tilePos, chunks, Vector2Int.up);

                var chunk = world.GetChunkAt(tilePos);
                if (!chunks.Contains(chunk)) {
                    chunks.Add(chunk);
                }

                chunk[world.ToLocalChunkPosition(tilePos)] = material;
            } while (i < total);

            foreach (var chunk in chunks) {
                chunk.LoadMesh(World.Instance);
            }
        }

        private void CheckAdd(Vector3Int originalPos, ICollection<Chunk> chunks, Vector2Int direction) {
            var tilePos = originalPos;
            tilePos.x += direction.x;
            tilePos.z += direction.y;
            if (world.IsOutOfBounds(tilePos)) {

                return;
            }

            var chunk = world.GetChunkAt(tilePos);
            if (chunk == null) {
                return;
            }

            if (!chunks.Contains(chunk)) {
                chunks.Add(chunk);
            }
        }


        private WorldSelection(List<WorldTile> tiles, World world, Vector3Int min, Vector3Int max) {
            this.tiles = tiles;
            this.world = world;
            this.min = min;
            this.max = max;
        }

        public IEnumerator<WorldTile> GetEnumerator() => ((IEnumerable<WorldTile>) tiles).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static WorldSelection FromEnumerable(World world, IEnumerable<Vector3Int> enumerable) {
            var list = new List<WorldTile>();
            var min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
            var max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
            foreach (var pos in enumerable) {
                if (world.IsOutOfBounds(pos)) {
                    continue;
                }

                if (pos.x < min.x) {
                    min.x = pos.x;
                }

                if (pos.y < min.y) {
                    min.y = pos.y;
                }

                if (pos.z < min.z) {
                    min.z = pos.z;
                }

                if (pos.x > max.x) {
                    max.x = pos.x;
                }

                if (pos.y > max.y) {
                    max.y = pos.y;
                }

                if (pos.z > max.z) {
                    max.z = pos.z;
                }

                list.Add(new WorldTile(pos, world.GetBlock(pos)));
            }

            return new WorldSelection(list, world, min, max);
        }
    }
}