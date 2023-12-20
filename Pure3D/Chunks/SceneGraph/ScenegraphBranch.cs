using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x12010C)]
    public class ScenegraphBranch : VersionNamed
    {
        public uint UnkVar;

        public ScenegraphBranch(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            UnkVar = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Scenegraph Branch: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Scenegraph: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"UnkVar: {UnkVar}");

            return Lines.ToString();
        }
    }
}