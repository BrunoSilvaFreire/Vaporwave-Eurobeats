using UnityEngine;

namespace Scripts.World.Generation {
    [CreateAssetMenu(menuName = "VE/World/Generator/PerlinGenerator")]
    public class PerlinNoiseGenerator : WorldGenerator {

        [SerializeField] private float amplitude = 1f;
        [SerializeField] private int octaveCount = 8;
        public AnimationCurve CurveX, CurveZ, ExplosionCurve;
        
        public int ExplosionsCount = 10;
        public int ExplosionsSize = 4;
        
        
        public override ChunkData[,] Generate(World world, int seed) {
            var size = world.WorldWidth * world.ChunkSize;
            var noise = PerlinNoise.Generate(size, size, octaveCount, amplitude, seed);

            for (int exp = 0; exp < ExplosionsCount; exp++) {

                int randomExpX = Random.Range(0, size);
                int randomExpZ = Random.Range(0, size);      
                Debug.Log("Explosion: " + exp + "At " + new Vector2(randomExpX, randomExpZ));
                for (int x = randomExpX - ExplosionsSize; x < randomExpX + ExplosionsSize; x++) {
                    if (x < 0 || x >= size)
                        continue;
                    for (int z = randomExpZ - ExplosionsSize; z < randomExpZ + ExplosionsSize; z++) {
                        if (z < 0 || z >= size)
                            continue;

                        Debug.Log("xz: " + new Vector2(x, z));
                        noise[x, z] *= ExplosionCurve.Evaluate((float) x / ExplosionsSize * 2) *
                                       ExplosionCurve.Evaluate((float) z / ExplosionsSize * 2);

                    }
                }
            }
            
            
            

            for (int x = 0; x < size; x++) {
                for (int z = 0; z < size; z++) {

                    noise[x, z] *= CurveX.Evaluate((float)x / size) * CurveZ.Evaluate((float)z / size);
                }
            }
            
            var data = new ChunkData[world.WorldWidth, world.WorldDepth];
            for (byte i = 0; i < world.WorldWidth; i++) {
                for (byte j = 0; j < world.WorldDepth; j++) {
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