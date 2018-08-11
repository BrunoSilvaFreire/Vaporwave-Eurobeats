using UnityEngine;

namespace Scripts.World.Generation {
    [CreateAssetMenu(menuName = "VE/World/Generator/PerlinGenerator")]
    public class PerlinNoiseGenerator : WorldGenerator {
        public override ChunkData[,] Generate(World world) {
            var size = world.Width * world.ChunkSize;
            var noise = PerlinNoise.Generate(size, size, 8, 0);
            var data = new ChunkData[world.Width, world.Height];
            for (byte i = 0; i < world.Width; i++) {
                for (byte j = 0; j < world.Height; j++) {
                    var chunk = new ChunkData(world.ChunkSize, world.ChunkHeight);
                    FillChunk(world, chunk, noise, i, j);
                    data[i, j] = chunk;
                }
            }

            return data;
        }

        private static void FillChunk(World world, ChunkData chunkData, float[,] noise, byte i, byte j) {
            var endX = world.ChunkSize * i + world.ChunkSize - 1;
            var endZ = world.ChunkSize * j + world.ChunkSize - 1;

            var startX = endX - world.ChunkSize;
            var startZ = endZ - world.ChunkSize;

            for (var x = startX; x < endX; x++) {
                for (var z = startZ; z < endX; z++) {
                    var height = (int) (world.ChunkHeight * noise[x, z]);

                    for (var y = 0; y < world.ChunkHeight; y++) {
                        if (y < height) {
                            chunkData.blocks[x, y, z] = BlockMaterial.Solid;
                        } else {
                            chunkData.blocks[x, y, z] = BlockMaterial.Empty;
                        }
                    }
                }
            }
        }
    }
}