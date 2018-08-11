using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.World {
    public class ChunkData : IEnumerable<Vector3Int> {
        public BlockMaterial[,,] blocks;

        public ChunkData(byte chunkSize, byte chunkHeight) {
            blocks = new BlockMaterial[chunkSize, chunkHeight, chunkSize];
        }


        public IEnumerator<Vector3Int> GetEnumerator() {
            var size = blocks.GetLength(0);
            var height = blocks.GetLength(1);
            for (byte x = 0; x < size; x++) {
                for (byte y = 0; y < height; y++) {
                    for (byte z = 0; z < size; z++) {
                        yield return new Vector3Int(x, y, z);
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public BlockMaterial this[Vector3Int position] {
            get {
                return blocks[position.x, position.y, position.z];
            }
            set {
                blocks[position.x, position.y, position.z] = value;
            }
        }
    }
}