using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x10F02)]
    public class VertexAnimColorChannel : Named
    {
        public uint UnkInt1;
        public List<uint> Ind = new List<uint>();
        public List<ByteColour> Col = new List<ByteColour>();
        public List<uint> UnkInts = new List<uint>();

        public VertexAnimColorChannel(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            Name = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                Ind.Add(reader.ReadUInt32());
                Col.Add(Util.ReadColour(reader));
                UnkInts.Add(reader.ReadUInt32());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Color Channel {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Color Channel {Name}");
            Lines.AppendLine($"VCount {Ind.Count}");
            for (int i = 0; i < Ind.Count; i++)
            {
                Lines.AppendLine($"{Ind[i]}: {Col[i]} ({UnkInts[i]})");
            }

            return Lines.ToString();
        }
    }
}