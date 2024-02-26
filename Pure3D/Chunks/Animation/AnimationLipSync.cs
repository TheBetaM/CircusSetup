using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x3F00100)]
    public class AnimationLipSync : Named
    {
        public List<ushort> Flags = new();
        public List<float> Value1 = new();
        public List<float> Value2 = new();
        public ushort UnkInt;
        public AnimationLipSync(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            UnkInt = reader.ReadUInt16();
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                Flags.Add(reader.ReadUInt16());
                Value1.Add(reader.ReadSingle());
                Value2.Add(reader.ReadSingle());
            }

        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Animation Lip Sync: {Name} ({Flags.Count})";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Animation Lip Sync");
            Lines.AppendLine($"File: {Name}");
            Lines.AppendLine($"UnkInt: {UnkInt}");
            Lines.AppendLine($"Frames: {Flags.Count}");
            for (int i = 0; i < Flags.Count; i++)
            {
                Lines.AppendLine($"#{i}: {Flags[i]} / {Value1[i]} / {Value2[i]}");
            }

            return Lines.ToString();
        }
    }
}
