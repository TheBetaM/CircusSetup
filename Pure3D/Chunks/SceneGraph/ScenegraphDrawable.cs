using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x12010F)]
    public class ScenegraphDrawable : Named
    {
        public uint UnkVar1;
        public uint UnkVar2;
        public string Drawable;
        public ulong Drawable_padding;

        public ScenegraphDrawable(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkVar1 = reader.ReadUInt32();
            UnkVar2 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            Drawable = Util.ReadString(reader, ref Drawable_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Scenegraph Drawable: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Scenegraph Drawable: {Name}");
            Lines.AppendLine($"UnkVar1: {UnkVar1}");
            Lines.AppendLine($"UnkVar2: {UnkVar2}");
            Lines.AppendLine($"Drawable: {Drawable}");

            return Lines.ToString();
        }
    }
}