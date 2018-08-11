using UnityEngine;

namespace Scripts.World {
    public abstract class WorldGenerator : ScriptableObject {
        public abstract ChunkData[,] Generate(global::Scripts.World.World world);
    }
}