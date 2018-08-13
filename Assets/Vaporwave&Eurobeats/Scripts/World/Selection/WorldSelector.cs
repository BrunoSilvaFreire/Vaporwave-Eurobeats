using System.Collections.Generic;
using UnityEngine;

namespace Scripts.World.Selection {
    public static class Selections {
        public static WorldSelection SphereSelection(World world, Vector3Int center, float radius) {
            return WorldSelection.FromEnumerable(world, SelectSphere(center, radius));
        }

        public static IEnumerable<Vector3Int> SelectSphere(Vector3Int center, float radius) {
            var minX = (int) (center.x - radius);
            var minY = (int) (center.y - radius);
            var minZ = (int) (center.z - radius);
            var maxX = (int) (center.x + radius);
            var maxY = (int) (center.y + radius);
            var maxZ = (int) (center.z + radius);
            for (var x = minX; x < maxX; x++) {
                for (var y = minY; y < maxY; y++) {
                    for (var z = minZ; z < maxZ; z++) {
                        if (DistanceSquared(x, y, z, center) < radius) {
                            yield return new Vector3Int(x, y, z);
                        }
                    }
                }
            }
        }

        private static float DistanceSquared(int x, int y, int z, Vector3Int center) {
            var dX = Mathf.Abs(center.x - x);
            var dY = Mathf.Abs(center.y - y);
            var dZ = Mathf.Abs(center.z - z);
            return Mathf.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        public static WorldSelection CuboidSelection(World world, Vector3Int min, Vector3Int max) {
            return WorldSelection.FromEnumerable(world, SelectCuboid(min, max));
        }

        private static IEnumerable<Vector3Int> SelectCuboid(Vector3Int min, Vector3Int max) {
            for (var x = min.x; x < max.x; x++) {
                for (var y = min.y; y < max.y; x++) {
                    for (var z = min.z; z < max.z; x++) {
                        yield return new Vector3Int(x, y, z);
                    }
                }
            }
        }
    }
}