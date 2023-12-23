using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010021)]
    public class CollisionVolumeOwner : Chunk
    {
        public uint UnkInt;
        public CollisionVolumeOwner(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(UnkInt);
        }

        public override string ToString()
        {
            return $"Collision Volume Owner: {UnkInt}";
        }
    }
}