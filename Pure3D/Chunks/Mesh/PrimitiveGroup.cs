using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10002)]
    public class PrimitiveGroup : Chunk
    {
        public uint Version;
        public string ShaderName;
        public ulong ShaderName_padding;
        public PrimitiveTypes PrimitiveType;
        public uint VertexType;
        public uint NumVertices;
        public uint NumIndices;
        public uint NumMatrices;

        public VertexUVCount UVCount;
        public bool HasNormals;
        public bool HasColors;
        public bool HasSpecular;
        public bool HasBoneIndices;
        public bool HasWeights;
        public bool HasSize;
        public bool HasUnk;
        public bool HasBinormal;
        public bool HasTangents;
        public bool HasPositions;
        public bool HasMultipleColors;
        public VertexColorCount ColorCount;

        public PrimitiveGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            ShaderName = Util.ReadString(reader, ref ShaderName_padding);
            PrimitiveType = (PrimitiveTypes)reader.ReadUInt32();
            VertexType = reader.ReadUInt32();
            NumVertices = reader.ReadUInt32();
            NumIndices = reader.ReadUInt32();
            NumMatrices = reader.ReadUInt32();

            UVCount = (VertexUVCount)(VertexType & 0x0F);
            ColorCount = (VertexColorCount)(VertexType >> 15);
            int prim = (int)VertexType;
            HasNormals = (VertexType & (1 << 4)) != 0;
            HasColors = (VertexType & (1 << 5)) != 0;
            HasSpecular = (VertexType & (1 << 6)) != 0;
            HasBoneIndices = (VertexType & (1 << 7)) != 0;
            HasWeights = (VertexType & (1 << 8)) != 0;
            HasSize = (VertexType & (1 << 9)) != 0;
            HasUnk = (VertexType & (1 << 10)) != 0;
            HasBinormal = (VertexType & (1 << 11)) != 0;
            HasTangents = (VertexType & (1 << 12)) != 0;
            HasPositions = (VertexType & (1 << 13)) != 0;
            HasMultipleColors = (VertexType & (1 << 14)) != 0;
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            Util.WriteString(writer, ShaderName, ShaderName_padding);
            writer.Write((uint)PrimitiveType);
            writer.Write((uint)VertexType);
            writer.Write(NumVertices);
            writer.Write(NumIndices);
            writer.Write(NumMatrices);
        }

        public override string ToString()
        {
            return $"Prim Group: {ShaderName}";
        }

        public enum PrimitiveTypes : uint
        {
            TriangleList,
            TriangleStrip,
            LineList,
            LineStrip,
            Points,
        }

        [System.Flags]
        public enum VertexUVCount : uint
        {
            UVx0 = 0,
            UVx1 = 1,
            UVx2 = 2,
            UVx3 = 3,
            UVx4 = 4,
            UVx5 = 5,
            UVx6 = 6,
            UVx7 = 7,
            UVx8 = 8,
        }

        [System.Flags]
        public enum VertexColorCount : uint
        {
            COLORx0 = 0,
            COLORx1 = 1,
            COLORx2 = 2,
            COLORx3 = 3,
            COLORx4 = 4,
            COLORx5 = 5,
            COLORx6 = 6,
            COLORx7 = 7,
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Primitive Group");
            Lines.AppendLine($"Version {Version}");
            Lines.AppendLine($"ShaderName {ShaderName}");
            Lines.AppendLine($"PrimitiveType {PrimitiveType}");
            Lines.AppendLine($"VertexType {VertexType}");
            Lines.AppendLine($"Vertices {NumVertices}");
            Lines.AppendLine($"Indices {NumIndices}");
            Lines.AppendLine($"Matrices {NumMatrices}");

            Lines.AppendLine($"UVCount {UVCount}");
            Lines.AppendLine($"ColorCount {ColorCount}");
            Lines.AppendLine($"Has Normals {HasNormals}");
            Lines.AppendLine($"Has Colors {HasColors}");
            Lines.AppendLine($"Has Specular {HasSpecular}");
            Lines.AppendLine($"Has Bone Indices {HasBoneIndices}");
            Lines.AppendLine($"Has Weights {HasWeights}");
            Lines.AppendLine($"Has Size {HasSize}");
            Lines.AppendLine($"Has Unk {HasUnk}");
            Lines.AppendLine($"Has BiNormals {HasBinormal}");
            Lines.AppendLine($"Has Tangents {HasTangents}");
            Lines.AppendLine($"Has Positions {HasPositions}");
            Lines.AppendLine($"Has Multiple Colors {HasMultipleColors}");

            return Lines.ToString();
        }
    }
}
