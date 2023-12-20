using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks.Game
{
    [ChunkType(0x3000100)]
    public class CarCameraData : Chunk
    {
        public uint Index;
        public float Unknown;
        public float Angle;
        public float Distance;
        public Vector3 Look;

        public CarCameraData(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Index = reader.ReadUInt32();
            Unknown = reader.ReadSingle();
            Angle = reader.ReadSingle();
            Distance = reader.ReadSingle();
            Look = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Unknown);
            writer.Write(Angle);
            writer.Write(Distance);
            Util.WriteVector3(writer, Look);
        }

        public override string ToString()
        {
            return $"Car Camera Data ({Index})";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Car Camera Data");
            Lines.AppendLine($"Index: {Index}");
            Lines.AppendLine($"Unknown: {Unknown}");
            Lines.AppendLine($"Angle: {Angle}");
            Lines.AppendLine($"Distance: {Distance}");
            Lines.AppendLine($"Look: {Look}");

            return Lines.ToString();
        }
    }
}
