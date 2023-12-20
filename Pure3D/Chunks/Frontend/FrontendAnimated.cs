using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x18024)]
    public class FrontendAnimated : Named
    {
        public uint Version;
        public int PositionX;
        public int PositionY;
        public uint Width;
        public uint Height;
        public uint UnkInt5;
        public uint UnkInt6;
        public byte UnkByte2;
        public int UnkInt7;
        public uint UnkInt8;
        public uint UnkInt9;
        public string SkeletonName;
        public ulong SkeletonName_padding;
        public string CameraName;
        public ulong CameraName_padding;
        public string CameraName2;
        public ulong CameraName2_padding;
        public byte UnkShort; // possibly 1 more element

        public FrontendAnimated(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            PositionX = reader.ReadInt32();
            PositionY = reader.ReadInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            UnkInt5 = reader.ReadUInt32();
            UnkInt6 = reader.ReadUInt32();
            UnkByte2 = reader.ReadByte();
            UnkInt7 = reader.ReadInt32();
            UnkInt8 = reader.ReadUInt32();
            UnkInt9 = reader.ReadUInt32();
            SkeletonName = Util.ReadString(reader, ref SkeletonName_padding);
            CameraName = Util.ReadString(reader, ref CameraName_padding);
            CameraName2 = Util.ReadString(reader, ref CameraName2_padding);
            UnkShort = reader.ReadByte();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Animated: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Animated: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Position: {PositionX} / {PositionY}");
            Lines.AppendLine($"Size: {Width} / {Height}");
            Lines.AppendLine($"Int5: {UnkInt5}");
            Lines.AppendLine($"Int6: {UnkInt6}");
            Lines.AppendLine($"Byte2: {UnkByte2}");
            Lines.AppendLine($"Int7: {UnkInt7}");
            Lines.AppendLine($"Int8: {UnkInt8}");
            Lines.AppendLine($"Int9: {UnkInt9}");

            Lines.AppendLine($"Skeleton: {SkeletonName}");
            Lines.AppendLine($"Camera: {CameraName}");
            Lines.AppendLine($"Camera2: {CameraName2}");

            return Lines.ToString();
        }
    }
}