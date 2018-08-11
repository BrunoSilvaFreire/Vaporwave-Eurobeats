using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockMaterial : byte {
    Empty,
    Solid
}

public abstract class WorldGenerator : ScriptableObject {
    public abstract void Generate(World world);
}

public class Chunk {

    public BlockMaterial[,,] blocks;

    public Chunk(byte chunkSize, byte chunkHeight) {
        blocks = new BlockMaterial[chunkSize, chunkHeight, chunkSize];
    }
}

public class PerlinNoiseGenerator : WorldGenerator {
    public override void Generate(World world) {

        var size = world.Width * world.ChunkSize;
        var noise = PerlinNoise.Generate(size, size, 8, 0);
        for (byte i = 0; i < world.Width; i++) {
            for (byte j = 0; j < world.Height; j++) {
                var chunk = new Chunk(world.ChunkSize, world.ChunkHeight);
                
                FillChunk(world, chunk, noise, i, j);
            }
        }
    }

    private static void FillChunk(World world, Chunk chunk, float[,] noise, byte i, byte j) {
        var endX = world.ChunkSize * i + world.ChunkSize;
        var endZ = world.ChunkSize * j + world.ChunkSize;

        var startX = endX - world.ChunkSize;
        var startZ = endZ - world.ChunkSize;

        for (var x = startX; x < endX; x++) {
            for (var z = startZ; z < endX; z++) {
                var height = (int)(world.ChunkHeight * noise[x, z]);
                
                for (var y = 0; y < world.ChunkHeight; y++) {
                    if (y < height)
                        chunk.blocks[x, y, z] = BlockMaterial.Solid;
                    else 
                        chunk.blocks[x, y, z] = BlockMaterial.Empty;                  
                }
            }
        }
    }
}

public class World : MonoBehaviour {
    //Quantos chunks
    public byte Width;

    //Quantos chunks
    public byte Height;
    public byte ChunkSize;
    public byte ChunkHeight;
}