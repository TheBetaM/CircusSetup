using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x1001B)]
    public class TopologyList : Chunk
    {
        public Topology[] Topologies;

        public TopologyList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Topologies = new Topology[len];
            for (int i = 0; i < len; i++)
            {
                Topologies[i] = Util.ReadTopology(reader);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Topology List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Topology List");
            Lines.AppendLine($"Count: {Topologies.Length}");
            for (int i = 0; i < Topologies.Length; i++)
            {
                Lines.AppendLine($"Top{i}: {Topologies[i]}");
            }

            return Lines.ToString();
        }
    }
}