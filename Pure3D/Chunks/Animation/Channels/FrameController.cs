using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x121201)]
    public class FrameController : Named
    {
        public uint Version;
        public string SubType;
        public uint FrameOffset;
        public string AnimParameter;
        public string TargetParameter;

        public string ModelName;
        public ulong ModelName_padding;
        public string AnimName;
        public ulong AnimName_padding;

        public FrameController(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            AnimParameter = Encoding.ASCII.GetString(reader.ReadBytes(4));
            TargetParameter = Encoding.ASCII.GetString(reader.ReadBytes(4));
            SubType = Encoding.ASCII.GetString(reader.ReadBytes(4));
            FrameOffset = reader.ReadUInt32();
            ModelName = Util.ReadString(reader, ref ModelName_padding);
            AnimName = Util.ReadString(reader, ref AnimName_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Frame Controller: {AnimParameter} {TargetParameter} {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frame Controller {Name}");
            Lines.AppendLine($"Model/Object: {ModelName}");
            Lines.AppendLine($"Anim: {AnimName}");
            Lines.AppendLine($"Anim Parameter: {AnimParameter}");
            Lines.AppendLine($"Target Parameter: {TargetParameter}");
            Lines.AppendLine($"FrameOffset: {FrameOffset}");
            Lines.AppendLine($"SubType: {SubType}");
            Lines.AppendLine($"Version: {Version}");

            return Lines.ToString();
        }
    }
}