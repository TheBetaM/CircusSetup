using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x18022)]
    public class FrontendSprite : Named
    {
        public uint Version;
        public uint Width;
        public uint Height;
        public uint Width2;
        public uint Height2;
        public uint Width3;
        public uint Height3;
        public byte[] UnkData;

        public uint SpriteCount;
        public List<string> Sprites = new List<string>();
        public List<ulong> Sprites_padding = new List<ulong>();

        public FrontendSprite(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            Width2 = reader.ReadUInt32();
            Height2 = reader.ReadUInt32();
            Width3 = reader.ReadUInt32();
            Height3 = reader.ReadUInt32();

            UnkData = reader.ReadBytes(0x0D);

            SpriteCount = reader.ReadUInt32();
            for (int i = 0; i < SpriteCount; i++)
            {
                ulong Pad = 0;
                Sprites.Add(Util.ReadString(reader, ref Pad));
                Sprites_padding.Add(Pad);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Sprite: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Sprite: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Width: {Width}");
            Lines.AppendLine($"Height: {Height}");
            Lines.AppendLine($"Width 2: {Width2}");
            Lines.AppendLine($"Height 2: {Height2}");
            Lines.AppendLine($"Width 3: {Width3}");
            Lines.AppendLine($"Height 3: {Height3}");
            Lines.AppendLine($"UnkData: {UnkData.ToLine()}");
            Lines.AppendLine($"Sprite Count: {SpriteCount}");
            for (int i = 0; i < SpriteCount; i++)
            {
                Lines.AppendLine($"Sprite{i}: {Sprites[i]}");
            }

            return Lines.ToString();
        }
    }
}