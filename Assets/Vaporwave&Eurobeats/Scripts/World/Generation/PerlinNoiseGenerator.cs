using UnityEngine;

namespace Scripts.World.Generation {
    [CreateAssetMenu(menuName = "VE/World/Generator/PerlinGenerator")]
    public class PerlinNoiseGenerator : WorldGenerator {

        [SerializeField] private float amplitude = 1f;
        [SerializeField] private int octaveCount = 8;
        
        
        public override ChunkData[,] Generate(World world, int seed) {
            var size = world.Width * world.ChunkSize;
            var noise = PerlinNoise.Generate(size, size, octaveCount, amplitude, seed);
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

        private static void FillChunk(World world, ChunkData chunkData, float[,] noise, byte chunkX, byte chunkZ) {
           
            var startX = world.ChunkSize * chunkX;
            var startZ = world.ChunkSize * chunkZ;

            var width = world.ChunkSize;
            
            for (var x = 0; x < width; x++) {
                for (var z = 0; z < width; z++) {
                    var height = (int) (world.ChunkHeight * noise[startX + x, startZ + z]);
                  
                    for (var y = 0; y < world.ChunkHeight; y++) {

                        chunkData.blocks[x, y, z] = y < height ? BlockMaterial.Solid : BlockMaterial.Empty;
                    }
                }
            }
        }
    }
}