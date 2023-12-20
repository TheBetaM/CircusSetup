using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x19005)]
    public class Sprite : Named
    {
        public uint NativeWidth;
        public uint NativeHeight;
        public string Shader;
        public ulong Shader_padding;
        public uint Width;
        public uint Height;
        public uint ImageCount;
        public uint BlitBorder;


        public Sprite(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            NativeWidth = reader.ReadUInt32();
            NativeHeight = reader.ReadUInt32();
            Shader = Util.ReadString(reader, ref Shader_padding);
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            ImageCount = reader.ReadUInt32();
            BlitBorder = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Sprite: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Sprite: {Name}");
            Lines.AppendLine($"Native Width: {NativeWidth}");
            Lines.AppendLine($"Native Height: {NativeHeight}");
            Lines.AppendLine($"Shader: {Shader}");
            Lines.AppendLine($"Width: {Width}");
            Lines.AppendLine($"Height: {Height}");
            Lines.AppendLine($"ImageCount: {ImageCount}");
            Lines.AppendLine($"BlitBorder: {BlitBorder}");

            return Lines.ToString();
        }

        public override byte[] OnImagePreview()
        {
            Image imagechunk = (Image)Children[0];
            return imagechunk.OnImagePreview();
        }
        public override void OnExport(string path)
        {
            Image imagechunk = (Image)Children[0];
            imagechunk.OnExport(path);
        }

        public override void OnGodotExport(string path)
        {
            Image imagechunk = (Image)Children[0];
            imagechunk.OnGodotExport(path);
        }
    }
}