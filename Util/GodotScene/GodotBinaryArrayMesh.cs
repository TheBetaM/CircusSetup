using System;
using System.Collections.Generic;
using System.Numerics;
using Pure3D.Chunks;

namespace CircusSetup
{
    public class GodotBinaryArrayMesh : GodotBinaryResourceFile
    {

        public override string ResType => "ArrayMesh";

        public GodotBinaryArrayMesh()
        {

        }

        public GodotBinaryArrayMesh(Mesh Model)
        {
            CreateModel(Model);
        }

        public GodotBinaryArrayMesh(Skin Model)
        {
            CreateModel(Model);
        }

        public void CreateModel(Pure3D.Chunk Model)
        {
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            var surfArray = new List<object>();

            List<PrimitiveGroupCTTR> prims = new List<PrimitiveGroupCTTR>();
            foreach (var item in Model.Children)
            {
                if (item is PrimitiveGroupCTTR prim)
                {
                    prims.Add(prim);
                }
            }
            var vanim = Model.GetChild<VertexAnimHeader>();
            List<VertexAnimVector3Channel> vanimc = new List<VertexAnimVector3Channel>();
            List<string> BlendShapeNames = new();
            if (vanim != null)
            {
                // todo: when there's separate blend shape items for different surfaces (titans ps2)
                foreach (var item in vanim.Children)
                {
                    if (item is VertexAnimShape shape && shape.Children[0] is VertexAnimVector3Channel v3)
                    {
                        vanimc.Add(v3);
                    }
                }

                for (int a = 0; a < vanimc.Count; a++)
                {
                    BlendShapeNames.Add($"morph_{a}");
                }
            }

            for (int i = 0; i < prims.Count; i++)
            {
                var dict = new Dictionary<object, object>();
                AABB boundingBox = new AABB();
                List<byte> AttributeData = new();
                ulong format = 0x800000000;
                int index_count = 0;
                List<byte> IndexData = new();
                int vertex_count = 0;
                List<byte> VertexData = new();
                List<byte> SkinData = new();
                List<byte> BlendShapeData = new();
                float MinX = 99999f;
                float MaxX = -99999f;
                float MinY = 99999f;
                float MaxY = -99999f;
                float MinZ = 99999f;
                float MaxZ = -99999f;

                PrimitiveGroupCTTR Sub = prims[i];
                var primType = Sub.PrimitiveType;
                var inds = Sub.GetChild<IndexList>();
                var poslist = Sub.GetChild<PositionList>();
                var norlist = Sub.GetChild<NormalList>();
                var uvlist = Sub.GetChild<UVList>();
                var collist = Sub.GetChild<ColourList>();
                var matlist = Sub.GetChild<MatrixList>();
                var matpal = Sub.GetChild<MatrixPalette>();
                var wgtlist = Sub.GetChild<WeightList>();
                var tanlist = Sub.GetChild<TangentList>();
                var natlist = Sub.GetChild<NativeVertexList>();

                vertex_count = (int)Sub.NumVertices;
                int primCount = 3;
                if (primType == PrimitiveGroupCTTR.PrimitiveTypes.Points)
                {
                    primCount = 1;
                }
                else if (primType == PrimitiveGroupCTTR.PrimitiveTypes.LineList || primType == PrimitiveGroupCTTR.PrimitiveTypes.LineStrip)
                {
                    primCount = 2;
                }
                int rest = 0;
                if (Sub.NumVertices % primCount != 0)
                {
                    rest = primCount - (int)(Sub.NumVertices % primCount);
                }

                if (Sub.HasPositions) format |= (int)ArrayFormatFlags.Vertex;
                if (Sub.HasColors) format |= (int)ArrayFormatFlags.Color;
                if (Sub.UVCount != PrimitiveGroupCTTR.VertexUVCount.UVx0) format |= (int)ArrayFormatFlags.UV;
                if (Sub.HasBoneIndices) format |= (int)ArrayFormatFlags.Bones;
                if (Sub.HasWeights) format |= (int)ArrayFormatFlags.Weights;
                if (Sub.HasNormals) format |= (int)ArrayFormatFlags.Normal;
                //if (Sub.HasTangents) format |= (int)ArrayFormatFlags.Tangent;

                for (int v = 0; v < Sub.NumVertices; v++)
                {
                    if (poslist != null)
                    {
                        byte[] X = BitConverter.GetBytes(poslist.Positions[v].X);
                        byte[] Y = BitConverter.GetBytes(poslist.Positions[v].Y);
                        byte[] Z = BitConverter.GetBytes(poslist.Positions[v].Z);
                        VertexData.AddRange(X);
                        VertexData.AddRange(Y);
                        VertexData.AddRange(Z);
                        if (poslist.Positions[v].X < MinX)
                            MinX = poslist.Positions[v].X;
                        if (poslist.Positions[v].X > MaxX)
                            MaxX = poslist.Positions[v].X;
                        if (poslist.Positions[v].Y < MinY)
                            MinY = poslist.Positions[v].Y;
                        if (poslist.Positions[v].Y > MaxY)
                            MaxY = poslist.Positions[v].Y;
                        if (poslist.Positions[v].Z < MinZ)
                            MinZ = poslist.Positions[v].Z;
                        if (poslist.Positions[v].Z > MaxZ)
                            MaxZ = poslist.Positions[v].Z;
                        format |= (int)ArrayFormatFlags.Vertex;
                    }
                    if (collist != null)
                    {
                        byte R = collist.Colours[v].R;
                        byte G = collist.Colours[v].G;
                        byte B = collist.Colours[v].B;
                        byte A = collist.Colours[v].A;
                        AttributeData.Add(R);
                        AttributeData.Add(G);
                        AttributeData.Add(B);
                        AttributeData.Add(A);
                        format |= (int)ArrayFormatFlags.Color;
                    }
                    if (uvlist != null)
                    {
                        byte[] UV_X = BitConverter.GetBytes(uvlist.UVs[v].X);
                        byte[] UV_Y = BitConverter.GetBytes(-uvlist.UVs[v].Y);
                        AttributeData.AddRange(UV_X);
                        AttributeData.AddRange(UV_Y);
                        format |= (int)ArrayFormatFlags.UV;
                    }
                    if (matlist != null && matpal != null)
                    {
                        byte[] Bone1 = new byte[2] { 0x00, 0x00 };
                        byte[] Bone2 = new byte[2] { 0x00, 0x00 };
                        byte[] Bone3 = new byte[2] { 0x00, 0x00 };
                        byte[] Bone4 = new byte[2] { 0x00, 0x00 };
                        var ind1 = matlist.Matrices[v][3];
                        var ind2 = matlist.Matrices[v][2];
                        var ind3 = matlist.Matrices[v][1];
                        var ind4 = matlist.Matrices[v][0];
                        Bone1 = BitConverter.GetBytes((ushort)matpal.Matrices[ind1]);
                        Bone2 = BitConverter.GetBytes((ushort)matpal.Matrices[ind2]);
                        Bone3 = BitConverter.GetBytes((ushort)matpal.Matrices[ind3]);
                        Bone4 = BitConverter.GetBytes((ushort)matpal.Matrices[ind4]);
                        SkinData.AddRange(Bone1);
                        SkinData.AddRange(Bone2);
                        SkinData.AddRange(Bone3);
                        SkinData.AddRange(Bone4);
                        format |= (int)ArrayFormatFlags.Bones;
                    }
                    if (wgtlist != null)
                    {
                        ushort ConvWeight1 = (ushort)(wgtlist.Weights[v].X * 65535);
                        ushort ConvWeight2 = (ushort)(wgtlist.Weights[v].Y * 65535);
                        ushort ConvWeight3 = (ushort)(wgtlist.Weights[v].Z * 65535);
                        byte[] Weight1 = BitConverter.GetBytes(ConvWeight1);
                        byte[] Weight2 = BitConverter.GetBytes(ConvWeight2);
                        byte[] Weight3 = BitConverter.GetBytes(ConvWeight3);
                        SkinData.AddRange(Weight1);
                        SkinData.AddRange(Weight2);
                        SkinData.AddRange(Weight3);
                        SkinData.Add(0);
                        SkinData.Add(0);
                        format |= (int)ArrayFormatFlags.Weights;
                    }
                    if (natlist != null)
                    {
                        var vert = natlist.Vertexes[v];
                        if (Sub.HasPositions)
                        {
                            byte[] X = BitConverter.GetBytes(vert.X);
                            byte[] Y = BitConverter.GetBytes(vert.Y);
                            byte[] Z = BitConverter.GetBytes(vert.Z);
                            VertexData.AddRange(X);
                            VertexData.AddRange(Y);
                            VertexData.AddRange(Z);
                            if (vert.X < MinX)
                                MinX = vert.X;
                            if (vert.X > MaxX)
                                MaxX = vert.X;
                            if (vert.Y < MinY)
                                MinY = vert.Y;
                            if (vert.Y > MaxY)
                                MaxY = vert.Y;
                            if (vert.Z < MinZ)
                                MinZ = vert.Z;
                            if (vert.Z > MaxZ)
                                MaxZ = vert.Z;
                        }
                        if (Sub.HasColors)
                        {
                            byte R = vert.R;
                            byte G = vert.G;
                            byte B = vert.B;
                            byte A = vert.A;
                            AttributeData.Add(R);
                            AttributeData.Add(G);
                            AttributeData.Add(B);
                            AttributeData.Add(A);
                        }
                        if (Sub.UVCount != PrimitiveGroupCTTR.VertexUVCount.UVx0)
                        {
                            byte[] UV_X = BitConverter.GetBytes(vert.U);
                            byte[] UV_Y = BitConverter.GetBytes(-vert.V);
                            AttributeData.AddRange(UV_X);
                            AttributeData.AddRange(UV_Y);
                        }
                        if (Sub.HasBoneIndices)
                        {
                            byte[] Bone1 = new byte[2] { 0x00, 0x00 };
                            byte[] Bone2 = new byte[2] { 0x00, 0x00 };
                            byte[] Bone3 = new byte[2] { 0x00, 0x00 };
                            byte[] Bone4 = new byte[2] { 0x00, 0x00 };
                            if (matpal != null)
                            {
                                var ind1 = matpal.Matrices[vert.Joint.JointIndex4];
                                var ind2 = matpal.Matrices[vert.Joint.JointIndex3];
                                var ind3 = matpal.Matrices[vert.Joint.JointIndex2];
                                var ind4 = matpal.Matrices[vert.Joint.JointIndex1];
                                Bone1 = BitConverter.GetBytes((ushort)ind1);
                                Bone2 = BitConverter.GetBytes((ushort)ind2);
                                Bone3 = BitConverter.GetBytes((ushort)ind3);
                                Bone4 = BitConverter.GetBytes((ushort)ind4);
                            }
                            else
                            {
                                Bone1 = BitConverter.GetBytes((ushort)vert.Joint.JointIndex1);
                                Bone2 = BitConverter.GetBytes((ushort)vert.Joint.JointIndex2);
                                Bone3 = BitConverter.GetBytes((ushort)vert.Joint.JointIndex3);
                                Bone4 = BitConverter.GetBytes((ushort)vert.Joint.JointIndex4);
                            }
                            SkinData.AddRange(Bone1);
                            SkinData.AddRange(Bone2);
                            SkinData.AddRange(Bone3);
                            SkinData.AddRange(Bone4);
                        }
                        if (Sub.HasWeights)
                        {
                            ushort ConvWeight1 = (ushort)(vert.Joint.Weight1 * 65535);
                            ushort ConvWeight2 = (ushort)(vert.Joint.Weight2 * 65535);
                            ushort ConvWeight3 = (ushort)(vert.Joint.Weight3 * 65535);
                            byte[] Weight1 = BitConverter.GetBytes(ConvWeight1);
                            byte[] Weight2 = BitConverter.GetBytes(ConvWeight2);
                            byte[] Weight3 = BitConverter.GetBytes(ConvWeight3);
                            SkinData.AddRange(Weight1);
                            SkinData.AddRange(Weight2);
                            SkinData.AddRange(Weight3);
                            SkinData.Add(0);
                            SkinData.Add(0);
                        }
                    }
                }
                if (rest != 0)
                {
                    vertex_count += rest;
                    for (int v = 0; v < rest; v++)
                    {
                        if (poslist != null || Sub.HasPositions)
                        {
                            byte[] X = BitConverter.GetBytes(0f);
                            byte[] Y = BitConverter.GetBytes(0f);
                            byte[] Z = BitConverter.GetBytes(0f);
                            VertexData.AddRange(X);
                            VertexData.AddRange(Y);
                            VertexData.AddRange(Z);
                        }
                        if (collist != null || Sub.HasColors)
                        {
                            AttributeData.Add(0);
                            AttributeData.Add(0);
                            AttributeData.Add(0);
                            AttributeData.Add(0);
                        }
                        if (uvlist != null || Sub.UVCount != PrimitiveGroupCTTR.VertexUVCount.UVx0)
                        {
                            byte[] UV_X = BitConverter.GetBytes(0f);
                            byte[] UV_Y = BitConverter.GetBytes(0f);
                            AttributeData.AddRange(UV_X);
                            AttributeData.AddRange(UV_Y);
                        }
                        if ((matlist != null && matpal != null) || Sub.HasBoneIndices)
                        {
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                        }
                        if (wgtlist != null || Sub.HasWeights)
                        {
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                            SkinData.Add(0);
                        }
                    }
                }
                for (int v = 0; v < Sub.NumVertices; v++)
                {
                    if (norlist != null)
                    {
                        byte NX = (byte)(-norlist.Normals[v].X * 127);
                        byte NY = (byte)(-norlist.Normals[v].Y * 127);
                        byte NZ = (byte)(-norlist.Normals[v].Z * 127);
                        VertexData.Add(255); 
                        VertexData.Add(NY); 
                        VertexData.Add(NX); 
                        VertexData.Add(NZ); 
                        format |= (int)ArrayFormatFlags.Normal;
                    }
                    if (tanlist != null)
                    {
                        byte TX = (byte)(tanlist.Tangents[v].X * 127);
                        byte TY = (byte)(tanlist.Tangents[v].Y * 127);
                        byte TZ = (byte)(tanlist.Tangents[v].Z * 127);
                        VertexData.Add(255); 
                        VertexData.Add(TX); 
                        VertexData.Add(TY); 
                        VertexData.Add(TZ); 
                        format |= (int)ArrayFormatFlags.Tangent;
                    }
                    if (natlist != null)
                    {
                        var vert = natlist.Vertexes[v];
                        if (Sub.HasNormals)
                        {
                            byte NX = vert.BNX;
                            byte NY = vert.BNY;
                            byte NZ = vert.BNZ;
                            VertexData.Add(255); 
                            VertexData.Add(NY); 
                            VertexData.Add(NX); 
                            VertexData.Add(NZ); 
                        }
                        if (Sub.HasTangents)
                        {
                            
                        }
                    }
                }
                if (rest != 0)
                {
                    for (int v = 0; v < rest; v++)
                    {
                        if (norlist != null || Sub.HasNormals)
                        {
                            VertexData.Add(255); 
                            VertexData.Add(0); 
                            VertexData.Add(0); 
                            VertexData.Add(0); 
                        }
                        if (tanlist != null || Sub.HasTangents)
                        {
                            VertexData.Add(255); 
                            VertexData.Add(0); 
                            VertexData.Add(0); 
                            VertexData.Add(0); 
                        }
                    }
                }
                for (int a = 0; a < vanimc.Count; a++)
                {
                    for (int x = 0; x < Sub.NumVertices; x++)
                    {
                        if (poslist != null)
                        {
                            float BS_X = poslist.Positions[x].X;
                            float BS_Y = poslist.Positions[x].Y;
                            float BS_Z = poslist.Positions[x].Z;
                            if (vanimc[a].Ind.Contains((uint)x))
                            {
                                int pos = vanimc[a].Ind.IndexOf((uint)x);
                                BS_X += vanimc[a].Pos[pos].X;
                                BS_Y += vanimc[a].Pos[pos].Y;
                                BS_Z += vanimc[a].Pos[pos].Z;
                            }
                            byte[] BSX = BitConverter.GetBytes(BS_X);
                            byte[] BSY = BitConverter.GetBytes(BS_Y);
                            byte[] BSZ = BitConverter.GetBytes(BS_Z);
                            BlendShapeData.AddRange(BSX);
                            BlendShapeData.AddRange(BSY);
                            BlendShapeData.AddRange(BSZ);
                        }
                        if (natlist != null)
                        {
                            var vert = natlist.Vertexes[x];
                            float BS_X = vert.X;
                            float BS_Y = vert.Y;
                            float BS_Z = vert.Z;
                            if (vanimc[a].Ind.Contains((uint)x))
                            {
                                int pos = vanimc[a].Ind.IndexOf((uint)x);
                                BS_X += vanimc[a].Pos[pos].X;
                                BS_Y += vanimc[a].Pos[pos].Y;
                                BS_Z += vanimc[a].Pos[pos].Z;
                            }
                            byte[] BSX = BitConverter.GetBytes(BS_X);
                            byte[] BSY = BitConverter.GetBytes(BS_Y);
                            byte[] BSZ = BitConverter.GetBytes(BS_Z);
                            BlendShapeData.AddRange(BSX);
                            BlendShapeData.AddRange(BSY);
                            BlendShapeData.AddRange(BSZ);
                        }
                    }
                    if (rest != 0)
                    {
                        for (int v = 0; v < rest; v++)
                        {
                            float BS_X = 0f;
                            float BS_Y = 0f;
                            float BS_Z = 0f;
                            byte[] BSX = BitConverter.GetBytes(BS_X);
                            byte[] BSY = BitConverter.GetBytes(BS_Y);
                            byte[] BSZ = BitConverter.GetBytes(BS_Z);
                            BlendShapeData.AddRange(BSX);
                            BlendShapeData.AddRange(BSY);
                            BlendShapeData.AddRange(BSZ);
                        }
                    }
                    for (int v = 0; v < Sub.NumVertices; v++)
                    {
                        if (norlist != null)
                        {
                            byte NX = (byte)(-norlist.Normals[v].X * 127);
                            byte NY = (byte)(-norlist.Normals[v].Y * 127);
                            byte NZ = (byte)(-norlist.Normals[v].Z * 127);
                            BlendShapeData.Add(255); 
                            BlendShapeData.Add(NY); 
                            BlendShapeData.Add(NX); 
                            BlendShapeData.Add(NZ); 
                        }
                        if (tanlist != null)
                        {
                            byte TX = (byte)(tanlist.Tangents[v].X * 127);
                            byte TY = (byte)(tanlist.Tangents[v].Y * 127);
                            byte TZ = (byte)(tanlist.Tangents[v].Z * 127);
                            BlendShapeData.Add(255); 
                            BlendShapeData.Add(TX); 
                            BlendShapeData.Add(TY); 
                            BlendShapeData.Add(TZ); 
                        }
                        if (natlist != null)
                        {
                            var vert = natlist.Vertexes[v];
                            if (Sub.HasNormals)
                            {
                                byte NX = (byte)(-vert.NX * 127);
                                byte NY = (byte)(-vert.NY * 127);
                                byte NZ = (byte)(-vert.NZ * 127);
                                VertexData.Add(255); 
                                VertexData.Add(NY); 
                                VertexData.Add(NX); 
                                VertexData.Add(NZ);
                            }
                            if (Sub.HasTangents)
                            {

                            }
                        }
                    }
                    if (rest != 0)
                    {
                        for (int v = 0; v < rest; v++)
                        {
                            if (norlist != null || Sub.HasNormals)
                            {
                                BlendShapeData.Add(255); 
                                BlendShapeData.Add(0); 
                                BlendShapeData.Add(0); 
                                BlendShapeData.Add(0); 
                            }
                            if (tanlist != null)
                            {
                                BlendShapeData.Add(255); 
                                BlendShapeData.Add(0); 
                                BlendShapeData.Add(0); 
                                BlendShapeData.Add(0); 
                            }
                        }
                    }
                }
                
                if (inds != null)
                {
                    for (int d = 0; d < inds.Indices.Length; d++)
                    {
                        byte[] id = BitConverter.GetBytes((ushort)inds.Indices[d]);
                        IndexData.AddRange(id);
                    }
                    index_count = inds.Indices.Length;
                    /*
                    for (int d = 1; d < inds.Indices.Length - 2; d++) // d = 3
                    {
                        byte[] id1 = BitConverter.GetBytes((ushort)inds.Indices[d + 0]);
                        byte[] id2 = BitConverter.GetBytes((ushort)inds.Indices[d - 1]);
                        byte[] id3 = BitConverter.GetBytes((ushort)inds.Indices[d + 1]);
                        byte[] id4 = BitConverter.GetBytes((ushort)inds.Indices[d + 0]);
                        byte[] id5 = BitConverter.GetBytes((ushort)inds.Indices[d + 1]);
                        byte[] id6 = BitConverter.GetBytes((ushort)inds.Indices[d + 2]);
                        IndexData.AddRange(id1);
                        IndexData.AddRange(id2);
                        IndexData.AddRange(id3);
                        IndexData.AddRange(id4);
                        IndexData.AddRange(id5);
                        IndexData.AddRange(id6);
                        d++;
                        index_count += 6;
                    }
                    */
                    
                    format |= (int)ArrayFormatFlags.Index;
                }
                if (natlist != null)
                {
                    for (int d = 0; d < natlist.Indices.Count; d++)
                    {
                        byte[] id = BitConverter.GetBytes((ushort)natlist.Indices[d]);
                        IndexData.AddRange(id);
                    }
                    index_count = natlist.Indices.Count;
                    
                    format |= (int)ArrayFormatFlags.Index;
                }

                MinX -= 0.1f;
                MaxX += 0.1f;
                MinY -= 0.1f;
                MaxY += 0.1f;
                MinZ -= 0.1f;
                MaxZ += 0.1f;
                boundingBox.Position = new Vector3(MinX, MinY, MinZ);
                boundingBox.Size = new Vector3(Math.Abs(MaxX - MinX), Math.Abs(MaxY - MinY), Math.Abs(MaxZ - MinZ));
                var bbox = Sub.GetChild<BoundingBox>();
                if (bbox != null)
                {
                    // todo proper bbox here
                }
                dict.Add("aabb", boundingBox);
                dict.Add("attribute_data", AttributeData.ToArray());
                if (vanim != null)
                {
                    dict.Add("blend_shapes", BlendShapeData.ToArray());
                }
                dict.Add("format", format);
                dict.Add("index_count", index_count);
                dict.Add("index_data", IndexData.ToArray());
                int primTypeVal = 3;
                switch (primType)
                {
                    default:
                    case PrimitiveGroupCTTR.PrimitiveTypes.Points:
                        primTypeVal = 0;
                        break;
                    case PrimitiveGroupCTTR.PrimitiveTypes.LineList:
                        primTypeVal = 1;
                        break;
                    case PrimitiveGroupCTTR.PrimitiveTypes.LineStrip:
                        primTypeVal = 2;
                        break;
                    case PrimitiveGroupCTTR.PrimitiveTypes.TriangleList:
                        primTypeVal = 3;
                        break;
                    case PrimitiveGroupCTTR.PrimitiveTypes.TriangleStrip:
                        primTypeVal = 4;
                        break;
                }
                dict.Add("primitive", primTypeVal);
                if (SkinData.Count != 0)
                {
                    dict.Add("skin_data", SkinData.ToArray());
                }
                dict.Add("vertex_count", vertex_count);
                dict.Add("vertex_data", VertexData.ToArray());
                surfArray.Add(dict);
            }

            if (vanim != null)
            {
                res.Add("_blend_shape_names", BlendShapeNames.ToArray());
            }
            res.Add("_surfaces", surfArray.ToArray());
            if (vanim != null)
            {
                res.Add("blend_shape_mode", 0);
            }
            Resources.Add(res);
        }

        public enum ArrayFormatFlags{
            Vertex = 1,
            Normal = 2,
            Tangent = 4,
            Color = 8,
            UV = 16,
            UV2 = 32,
            Custom0 = 64,
            Custom1 = 128,
            Custom2 = 256,
            Custom3 = 512,
            Bones = 1024,
            Weights = 2048,
            Index = 4096,
        }

    }
}