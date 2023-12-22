using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x7000003)]
    public class FencePositionList : Chunk
    {
        public List<Vector3> Positions = new List<Vector3>();

        public FencePositionList(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                Positions.Add(Util.ReadVector3(reader));
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Fence Position List: {Positions.Count}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Fence Position List");
            Lines.AppendLine($"Count: {Positions.Count}");
            for (int i = 0; i < Positions.Count; i++)
            {
                Lines.AppendLine($"Pos{i}: {Positions[i]}");
            }

            return Lines.ToString();
        }
    }
}