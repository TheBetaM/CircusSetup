using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17001)]
    public class BillboardQuad : Named
    {
        public uint Version;
        public string BillboardMode;
        public Vector3 Translation;
        public uint Colour;
        public Vector2 Uv0;
        public Vector2 Uv1;
        public Vector2 Uv2;
        public Vector2 Uv3;
        public float Width;
        public float Height;
        public float Distance;
        public Vector2 UVOffset;

        public BillboardQuad(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            BillboardMode = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            Translation = Util.ReadVector3(reader);
            Colour = reader.ReadUInt32();
            Uv0 = Util.ReadVector2(reader);
            Uv1 = Util.ReadVector2(reader);
            Uv2 = Util.ReadVector2(reader);
            Uv3 = Util.ReadVector2(reader);
            Width = reader.ReadSingle();
            Height = reader.ReadSingle();
            Distance = reader.ReadSingle();
            UVOffset = Util.ReadVector2(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            base.WriteHeader(writer);
            for (int i = 0; i < 4; i++)
            {
                if (i < BillboardMode.Length)
                {
                    writer.Write((byte)BillboardMode[i]);
                }
                else
                {
                    writer.Write((byte)0x00);
                }
            }
            Util.WriteVector3(writer, Translation);
            writer.Write(Colour);
            Util.WriteVector2(writer, Uv0);
            Util.WriteVector2(writer, Uv1);
            Util.WriteVector2(writer, Uv2);
            Util.WriteVector2(writer, Uv3);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Distance);
            Util.WriteVector2(writer, UVOffset);
        }

        public override string ToString()
        {
            return $"Billboard Quad: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Quad: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"BillboardMode: {BillboardMode}");
            Lines.AppendLine($"Translation: {Translation}");
            Lines.AppendLine($"Colour: {Colour}");
            Lines.AppendLine($"Uv0: {Uv0}");
            Lines.AppendLine($"Uv1: {Uv1}");
            Lines.AppendLine($"Uv2: {Uv2}");
            Lines.AppendLine($"Uv3: {Uv3}");
            Lines.AppendLine($"Width: {Width}");
            Lines.AppendLine($"Height: {Height}");
            Lines.AppendLine($"Distance: {Distance}");
            Lines.AppendLine($"UvOffset: {UVOffset}");

            return Lines.ToString();
        }
    }
}
