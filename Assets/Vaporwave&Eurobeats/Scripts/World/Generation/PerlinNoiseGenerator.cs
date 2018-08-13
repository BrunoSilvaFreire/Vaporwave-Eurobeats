using System.Linq;
using System.Net;
using Scripts.World.Selection;
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


                var selection = Selections.SelectSphere(
                    new Vector3Int(randomExpX, (int) (world.ChunkHeight * 0.25f), randomExpZ), ExplosionsSize).ToList();

                for (int i = 0; i < selection.Count; i++) {
                    if (selection[i].x < 0 || selection[i].x >= size || selection[i].z < 0 || selection[i].z >= size)
                        continue;
                    
                    noise[selection[i].x, selection[i].z] = 0;
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