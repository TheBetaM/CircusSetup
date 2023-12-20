using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x8020003)]
    public class StatePropFrameControllerData : Named
    {
        public uint Cyclic;
        public uint NumCycles;
        public uint HoldFrame; // or float
        public float MinFrame;
        public float MaxFrame;
        public float RelativeSpeed;

        public StatePropFrameControllerData(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Cyclic = reader.ReadUInt32();
            NumCycles = reader.ReadUInt32();
            HoldFrame = reader.ReadUInt32();
            MinFrame = reader.ReadSingle();
            MaxFrame = reader.ReadSingle();
            RelativeSpeed = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Frame Controller: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"State Prop Frame Controller Data: {Name}");
            Lines.AppendLine($"Cyclic: {Cyclic}");
            Lines.AppendLine($"NumCycles: {NumCycles}");
            Lines.AppendLine($"HoldFrame: {HoldFrame}");
            Lines.AppendLine($"MinFrame: {MinFrame}");
            Lines.AppendLine($"MaxFrame: {MaxFrame}");
            Lines.AppendLine($"RelativeSpeed: {RelativeSpeed}");

            return Lines.ToString();
        }
    }
}