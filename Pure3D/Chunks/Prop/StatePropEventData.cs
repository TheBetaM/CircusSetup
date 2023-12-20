using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x8020004)]
    public class StatePropEventData : Named
    {
        public uint EventEnum;
        public uint State;
        public byte[] Remain;

        public StatePropEventData(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long startpos = reader.BaseStream.Position + length;
            base.ReadHeader(reader, length);
            State = reader.ReadUInt32();
            EventEnum = reader.ReadUInt32();
            // Titans/MoM
            if (reader.BaseStream.Position != startpos)
            {
                reader.BaseStream.Position = startpos;
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Event: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"State Prop Event Data: {Name}");
            Lines.AppendLine($"EventEnum: {EventEnum}");
            Lines.AppendLine($"State: {State}");

            return Lines.ToString();
        }
    }
}