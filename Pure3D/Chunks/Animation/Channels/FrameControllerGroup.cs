using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x121204)]
    public class FrameControllerGroup : Chunk
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public FrameControllerGroup(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(UnkInt1);
            writer.Write(UnkInt2);
        }

        public override string ToString()
        {
            return $"Frame Controller Group {UnkInt1} / {UnkInt2}";
        }
    }
}