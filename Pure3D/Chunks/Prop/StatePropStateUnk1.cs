using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x8020021)]
    public class StatePropStateUnk1 : Chunk
    {
        public uint UnkInt1;
        public uint UnkInt2;

        public StatePropStateUnk1(File file, uint type) : base(file, type)
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
            return $"SP State Unk1: {UnkInt1} / {UnkInt2}";
        }
    }

    [ChunkType(0x8020022)]
    public class StatePropStateUnk2 : Chunk
    {
        public uint UnkInt1;


        public StatePropStateUnk2(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(UnkInt1);
        }

        public override string ToString()
        {
            return $"SP State Unk2: {UnkInt1}";
        }
    }
}