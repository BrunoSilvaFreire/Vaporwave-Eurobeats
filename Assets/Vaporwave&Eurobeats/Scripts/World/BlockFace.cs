using System;
using UnityEngine;

namespace Scripts.World {
    public enum BlockFace {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Backward
    }

    public static class BlockFaceExtensions {
        public static BlockFace[] All = {
            BlockFace.Up,
            BlockFace.Down,
            BlockFace.Left,
            BlockFace.Right,
            BlockFace.Forward,
            BlockFace.Backward
        };

        public static Vector3Int ToVector3(this BlockFace face) {
            switch (face) {
                case BlockFace.Up:
                    return Vector3Int.up;
                case BlockFace.Down:
                    return Vector3Int.down;
                case BlockFace.Left:
                    return Vector3Int.left;
                case BlockFace.Right:
                    return Vector3Int.right;
                case BlockFace.Forward:
                    return new Vector3Int(0, 0, 1);
                case BlockFace.Backward:
                    return new Vector3Int(0, 0, -1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(face), face, null);
            }
        }

        public static Vector3 GetRight(this BlockFace face) {
            switch (face) {
                case BlockFace.Left:
                case BlockFace.Right:
                    return Vector3.forward;
                default:
                    return Vector3.right;
            }
        }

        public static Vector3 GetUp(this BlockFace face) {
            switch (face) {
                case BlockFace.Up:
                case BlockFace.Down:
                    return Vector3.forward;
                default:
                    return Vector3.up;
            }
        }
    }
}