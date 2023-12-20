using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x10F00)]
    public class VertexAnimVector3Channel : Named
    {
        public uint UnkInt1;
        public string Parameter;
        public List<uint> Ind = new List<uint>();
        public List<Vector3> Pos = new List<Vector3>();

        public VertexAnimVector3Channel(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            Parameter = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                Ind.Add(reader.ReadUInt32());
                Pos.Add(Util.ReadVector3(reader));
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Channel {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Channel {Name}");
            Lines.AppendLine($"VCount {Ind.Count}");
            for (int i = 0; i < Ind.Count; i++)
            {
                Lines.AppendLine($"{Ind[i]}: {Pos[i]}");
            }

            return Lines.ToString();
        }
    }
}