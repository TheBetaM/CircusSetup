using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x8020005)]
    public class StatePropCallbackData : Named
    {
        public uint EventEnum;
        public float OnFrame;
        public byte[] Remain;

        public StatePropCallbackData(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long startpos = reader.BaseStream.Position + length;
            base.ReadHeader(reader, length);
            EventEnum = reader.ReadUInt32();
            OnFrame = reader.ReadSingle();
            // Titans/MoM
            long rest = startpos - reader.BaseStream.Position;
            Remain = reader.ReadBytes((int)rest);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Callback: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"State Prop Visibility Data: {Name}");
            Lines.AppendLine($"EventEnum: {EventEnum}");
            Lines.AppendLine($"OnFrame: {OnFrame}");

            return Lines.ToString();
        }
    }
}