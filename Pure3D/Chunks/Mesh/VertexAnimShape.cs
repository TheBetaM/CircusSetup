using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x121306)]
    public class VertexAnimShape : Chunk
    {
        public uint UnkInt1;
        public uint UnkInt2; // model index?
        public uint UnkID;

        public VertexAnimShape(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            UnkID = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Shape {UnkID}: {UnkInt1} {UnkInt2}";
        }
    }
}