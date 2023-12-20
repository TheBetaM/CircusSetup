using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x12010D)]
    public class ScenegraphTransform : VersionNamed
    {
        public uint UnkVar;
        public Matrix4x4 Matrix;

        public ScenegraphTransform(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            UnkVar = reader.ReadUInt32();
            Matrix = Util.ReadMatrix(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Scenegraph Transform: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Scenegraph: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"UnkVar: {UnkVar}");
            Lines.AppendLine($"Matrix: {Matrix}");

            return Lines.ToString();
        }
    }
}