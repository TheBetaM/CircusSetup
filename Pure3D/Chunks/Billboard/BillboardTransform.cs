using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17007)]
    public class BillboardTransform : Unknown
    {
        public uint UnkInt1;
        public Quaternion Rot = new();
        public Vector3 Pos = new();
        public BillboardTransform(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            Rot = Util.ReadQuaternion(reader);
            Pos = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Billboard Transform";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Transform");
            Lines.AppendLine($"UnkInt: {UnkInt1}");
            Lines.AppendLine($"Rot: {Rot}");
            Lines.AppendLine($"Pos: {Pos}");

            return Lines.ToString();
        }
    }
}