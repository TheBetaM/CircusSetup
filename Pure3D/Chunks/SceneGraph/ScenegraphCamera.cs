using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x12010E)]
    public class ScenegraphCamera : Named
    {
        public uint UnkVar1;
        public uint UnkVar2;
        public uint UnkVar3;

        public ScenegraphCamera(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkVar1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkVar2 = reader.ReadUInt32();
            UnkVar3 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Scenegraph Camera: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Scenegraph Camera: {Name}");
            Lines.AppendLine($"UnkVar1: {UnkVar1}");
            Lines.AppendLine($"UnkVar2: {UnkVar2}");
            Lines.AppendLine($"UnkVar3: {UnkVar3}");

            return Lines.ToString();
        }
    }
}