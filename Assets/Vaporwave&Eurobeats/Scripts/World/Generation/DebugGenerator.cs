using UnityEngine;

namespace Scripts.World.Generation {
    [CreateAssetMenu(menuName = "VE/World/Generator/DebugGenerator")]
    public class DebugGenerator : WorldGenerator {
        public byte CutoutValueA = 8;
        public byte CutoutValueB = 8;
        public byte CutoutValueC = 8;
        public byte Size = 4;

        public override ChunkData[,] Generate(World world, int seed) {
            byte width = world.WorldWidth, height = world.ChunkHeight;
            var data = new ChunkData[width, height];
            byte chunkSize = world.ChunkSize, chunkHeight = world.ChunkHeight;
            for (byte x = 0; x < width; x++) {
                for (byte y = 0; y < height; y++) {
                    var chunk = new ChunkData(chunkSize, chunkHeight);
                    FillChunk(chunk);
                    data[x, y] = chunk;
                }
            }

            return data;
        }

        private void FillChunk(ChunkData chunk) {
            foreach (var position in chunk) {
                var pair = position.x % CutoutValueA > Size && position.z % CutoutValueB > Size && position.y % CutoutValueC > Size;
                chunk[position] = pair ? BlockMaterial.Solid : BlockMaterial.Empty;
            }
        }
    }
}