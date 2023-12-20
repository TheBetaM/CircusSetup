using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x4512)]
    public class CompositeDrawable : Named
    {
        public string SkeletonName;
        public ulong SkeletonName_padding;

        public CompositeDrawable(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            SkeletonName = Util.ReadString(reader, ref SkeletonName_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            Util.WriteString(writer, SkeletonName, SkeletonName_padding);
        }

        public override string ToString()
        {
            return $"Composite Drawable: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Composite Drawable: {Name}");
            Lines.AppendLine($"SkeletonName: {SkeletonName}");

            return Lines.ToString();
        }
    }
}
