using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x17003)]
    public class BillboardDisplayInfo : Chunk
    {
        public uint Version;
        public Quaternion Rotation;
        public string CutOffMode;
        public Vector2 UVOffsetRange;
        public float SourceRange;
        public float EdgeRange;

        public BillboardDisplayInfo(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            Rotation = Util.ReadQuaternion(reader);
            CutOffMode = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            UVOffsetRange = Util.ReadVector2(reader);
            SourceRange = reader.ReadSingle();
            EdgeRange = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            Util.WriteQuaternion(writer, Rotation);
            for (int i = 0; i < 4; i++)
            {
                if (i < CutOffMode.Length)
                {
                    writer.Write((byte)CutOffMode[i]);
                }
                else
                {
                    writer.Write((byte)0x00);
                }
            }
            Util.WriteVector2(writer, UVOffsetRange);
            writer.Write(SourceRange);
            writer.Write(EdgeRange);
        }

        public override string ToString()
        {
            return "Billboard Display Info";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Display Info");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Rotation: {Rotation}");
            Lines.AppendLine($"CutOffMode: {CutOffMode}");
            Lines.AppendLine($"UVOffsetRange: {UVOffsetRange}");
            Lines.AppendLine($"SourceRange: {SourceRange}");
            Lines.AppendLine($"EdgeRange: {EdgeRange}");

            return Lines.ToString();
        }
    }
}
