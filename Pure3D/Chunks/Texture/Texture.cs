using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x19000)]
    public class Texture : Named
    {
        public uint Version;
        public uint Width;
        public uint Height;
        public uint Bpp;
        public uint AlphaDepth;
        public uint TextureType;
        public uint Usage;
        public uint Priority;
        public uint NumMipMaps;

        public Texture(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            Bpp = reader.ReadUInt32();
            AlphaDepth = reader.ReadUInt32();
            NumMipMaps = reader.ReadUInt32();
            TextureType = reader.ReadUInt32();
            Usage = reader.ReadUInt32();
            Priority = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Bpp);
            writer.Write(AlphaDepth);
            writer.Write(NumMipMaps);
            writer.Write(TextureType);
            writer.Write(Usage);
            writer.Write(Priority);
        }

        public override string ToString()
        {
            return $"Texture: {Name} ({(TextureFormat)TextureType} {Width}x{Height} {Bpp}bpp)";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Texture: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Width: {Width}");
            Lines.AppendLine($"Height: {Height}");
            Lines.AppendLine($"Bpp: {Bpp}");
            Lines.AppendLine($"AlphaDepth: {AlphaDepth}");
            Lines.AppendLine($"NumMipMaps: {NumMipMaps}");
            Lines.AppendLine($"TextureType: {TextureType} {(TextureFormat)TextureType}");
            Lines.AppendLine($"Usage: {Usage}");
            Lines.AppendLine($"Priority: {Priority}");

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

    public enum TextureFormat
    {
        RGB, // PSP
        Palettized,
        Luminance,
        Bumpmap,
        DXT1,
        DXT2,
        DXT3,
        DXT4,
        DXT5,
        IPU,
        Z,
        Linear,
        RenderTarget,
        PS2_4bit, //PSMT4 / PAL4
        PS2_8bit, //PSMT8 / PAL8
        PS2_16bit, //PSMCT16 / ARGB1555
        PS2_32bit, //PSMCT32 / ARGB8888
        GameCube_4bit,
        GameCube_8bit,
        GameCube_16bit,
        GameCube_32bit,
        GameCube_DXT1,

        PSP_V1 = 30, // CTTR
        PSP_V2 = 31, // Titans/MoM
    }
}
