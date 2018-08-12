using UnityEngine;

namespace Scripts.World.Utilities {
    public static class VectorUtility {
        public static Vector3Int ToVector3Int(this Vector3 vec) {
            return new Vector3Int((int) vec.x, (int) vec.y, (int) vec.z);
        }
    }
}