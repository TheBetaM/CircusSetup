using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17002)]
    public class BillboardQuadGroup : Named
    {
        public uint Version;
        public string Shader;
        public ulong Shader_padding;
        public uint ZTest;
        public uint ZWrite;
        public uint Fog;
        public uint NumQuads;

        public BillboardQuadGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32(); // version before name, rare case.
            base.ReadHeader(reader, length);
            Shader = Util.ReadString(reader, ref Shader_padding);
            ZTest = reader.ReadUInt32();
            ZWrite = reader.ReadUInt32();
            Fog = reader.ReadUInt32();
            NumQuads = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            base.WriteHeader(writer);
            Util.WriteString(writer, Shader, Shader_padding);
            writer.Write(ZTest);
            writer.Write(ZWrite);
            writer.Write(Fog);
            writer.Write(NumQuads);
        }

        public override string ToString()
        {
            return $"Billboard Quad Group: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Quad Group: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Shader: {Shader}");
            Lines.AppendLine($"ZTest: {ZTest}");
            Lines.AppendLine($"ZWrite: {ZWrite}");
            Lines.AppendLine($"Fog: {Fog}");
            Lines.AppendLine($"NumQuads: {NumQuads}");

            return Lines.ToString();
        }
    }
}
