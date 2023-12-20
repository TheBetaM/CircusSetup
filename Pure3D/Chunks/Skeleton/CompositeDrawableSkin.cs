using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x4515)]
    public class CompositeDrawableSkin : Named
    {
        public uint IsTranslucent;

        public CompositeDrawableSkin(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            IsTranslucent = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(IsTranslucent);
        }

        public override string ToString()
        {
            return $"Composite Drawable Skin: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Composite Drawable Skin: {Name}");
            Lines.AppendLine($"IsTranslucent: {IsTranslucent}");

            return Lines.ToString();
        }
    }
}
