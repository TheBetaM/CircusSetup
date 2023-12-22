using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x7000748)]
    public class FenceUnkTitans1 : Chunk
    {
        public List<int> UnkInt1 = new List<int>();
        public List<int> UnkInt2 = new List<int>();
        public List<int> UnkInt3 = new List<int>();
        public List<Vector3> UnkVec1 = new List<Vector3>();
        public List<Vector3> UnkVec2 = new List<Vector3>();
        public FenceUnkTitans1(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                UnkInt1.Add(reader.ReadInt32());
                UnkInt2.Add(reader.ReadInt32());
                UnkInt3.Add(reader.ReadInt32());
                UnkVec1.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                UnkVec2.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Fence Unk Titans: {UnkInt1.Count}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Fence Unk List 4");
            Lines.AppendLine($"Count: {UnkInt1.Count}");
            for (int i = 0; i < UnkInt1.Count; i++)
            {
                Lines.AppendLine($"#{i}: {UnkInt1[i]} / {UnkInt2[i]} / {UnkInt3[i]} / {UnkVec1[i]} / {UnkVec2[i]}");
            }

            return Lines.ToString();
        }
    }
}