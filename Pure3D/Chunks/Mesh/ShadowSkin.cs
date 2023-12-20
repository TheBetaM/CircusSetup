using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x10019)]
    public class ShadowSkin : Named
    {
        public uint Version;
        public string SkeletonName;
        public ulong SkeletonName_padding;
        public uint Vertices;
        public uint Triangles;

        public ShadowSkin(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            SkeletonName = Util.ReadString(reader, ref SkeletonName_padding);
            Vertices = reader.ReadUInt32();
            Triangles = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            Util.WriteString(writer, SkeletonName, SkeletonName_padding);
            writer.Write(Vertices);
            writer.Write(Triangles);
        }

        public override string ToString()
        {
            return $"Shadow Skin: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Skin: {Name}");
            Lines.AppendLine($"Version {Version}");
            Lines.AppendLine($"SkeletonName {SkeletonName}");
            Lines.AppendLine($"Vertices {Vertices}");
            Lines.AppendLine($"Triangles {Triangles}");

            return Lines.ToString();
        }
    }
}
