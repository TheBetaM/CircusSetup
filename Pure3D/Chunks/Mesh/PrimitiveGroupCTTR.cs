using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using Pure3D;
using System.Collections.Specialized;

namespace Pure3D.Chunks
{
    [ChunkType(0x10020)]
    public class PrimitiveGroupCTTR : PrimitiveGroup
    {
        public uint MemoryImaged;
        public uint Optimised;
        public uint VertexAnimated;
        public uint VertexAnimatedMask;

        public PrimitiveGroupCTTR(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            MemoryImaged = reader.ReadUInt32();
            Optimised = reader.ReadUInt32();
            VertexAnimated = reader.ReadUInt32();
            VertexAnimatedMask = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(MemoryImaged);
            writer.Write(Optimised);
            writer.Write(VertexAnimated);
            writer.Write(VertexAnimatedMask);
        }

        public override string ToString()
        {
            return $"Prim Group: {ShaderName}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Primitive Group CTTR");
            Lines.AppendLine($"Version {Version}");
            Lines.AppendLine($"ShaderName {ShaderName}");
            Lines.AppendLine($"PrimitiveType {PrimitiveType}");
            Lines.AppendLine($"VertexType {VertexType}");
            Lines.AppendLine($"Vertices {NumVertices}");
            Lines.AppendLine($"Indices {NumIndices}");
            Lines.AppendLine($"Matrices {NumMatrices}");
            Lines.AppendLine($"MemoryImaged {MemoryImaged}");
            Lines.AppendLine($"Optimised {Optimised}");
            Lines.AppendLine($"VertexAnimated {VertexAnimated}");
            Lines.AppendLine($"VertexAnimatedMask {VertexAnimatedMask}");

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