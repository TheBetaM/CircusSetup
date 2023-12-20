using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x18009)]
    public class FrontendPolygon : Named
    {
        public uint Version;
        public uint Translucency;
        public uint PointCount;
        public List<Vector3> Points = new List<Vector3>();
        public List<ByteColour> Colours = new List<ByteColour>();

        public FrontendPolygon(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Translucency = reader.ReadUInt32();
            PointCount = reader.ReadUInt32();
            for (int i = 0; i < PointCount; i++)
            {
                Points.Add(Util.ReadVector3(reader));
            }
            for (int i = 0; i < PointCount; i++)
            {
                Colours.Add(Util.ReadColour(reader));
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Polygon: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Polygon {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Translucency: {Translucency}");
            Lines.AppendLine($"Point Count: {PointCount}");
            for (int i = 0; i < PointCount; i++)
            {
                Lines.AppendLine($"Point{i}: {Points[i]} Colour: {Colours[i]}");
            }

            return Lines.ToString();
        }
    }
}