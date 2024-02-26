using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;
using CircusSetup.VIF;

namespace Pure3D.Chunks
{
    [ChunkType(0x10012)]
    public class NativeVertexList : Unknown
    {
        public List<VertexData> Vertexes = new List<VertexData>();
        public List<ushort> Indices = new List<ushort>();
        public int Version;
        public int UnkParam;
        public int VifSize;
        public uint PSP_MeshType;

        public NativeVertexList(File file, uint type) : base(file, type)
        {
            
        }

        public override string ToString()
        {
            return $"Native VertexList V {Version:X} P {UnkParam} Size {VifSize}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Native Vertex List");
            Lines.AppendLine($"Header: 0x{Version:X8}");
            Lines.AppendLine($"Param: 0x{UnkParam:X8}");
            Lines.AppendLine($"VifSize: 0x{VifSize:X8}");
            if (Version == 0x00040001)
            {
                Lines.AppendLine($"PSP Native Mesh Type: 0x{PSP_MeshType:X8}");
            }
            Lines.AppendLine($"Length: {Data.Length}");
            Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }

        public static List<uint> TypeErrors = new List<uint>();

        public override void ReadHeader(BinaryReader reader, long length)
        {
            /*
            0x00020001 XBOX (SHAR only)
            0x00040001 PSP
            0x00100009 PS2 (SHAR: 0x00100002)
            0x00030004 GCN/WII
            */
            Version = reader.ReadInt32();
            UnkParam = reader.ReadInt32();
            VifSize = reader.ReadInt32();
            
            if (Version == 0x00040001) // PSP
            {
                /*
                Data = new byte[0];
                //var prim = (PrimitiveGroupCTTR)Parent;
                //var item = (Named)prim.Parent;
                //Console.WriteLine($" MODEL {item.Name} {prim.ShaderName}");
                ReadPSP(reader);
                */
                
                Data = reader.ReadBytes((int)length - 12);
                using (var stream = new MemoryStream(Data))
                {
                    using (var preader = new BinaryReader(stream))
                    {
                        bool err = true;
                        try {
                            ReadPSP(preader);
                            err = false;
                        }
                        catch {
                            var prim = (PrimitiveGroup)Parent;
                            var item = (Named)prim.Parent;
                            Console.WriteLine($"FAILED PSP MODEL LONG! {PSP_MeshType:X5} {item.Name} {prim.ShaderName} {File.FullName}");
                            TypeErrors.Add(PSP_MeshType);
                        }
                        if (stream.Position != stream.Length)
                        {
                            var prim = (PrimitiveGroup)Parent;
                            var item = (Named)prim.Parent;
                            Console.WriteLine($"FAILED PSP MODEL SHORT! {PSP_MeshType:X5} {item.Name} {prim.ShaderName} {File.FullName}");
                            TypeErrors.Add(PSP_MeshType);
                        }
                        if (TypeErrors.Contains(PSP_MeshType) && !err)
                        {
                            //var prim = (PrimitiveGroup)Parent;
                            //var item = (Named)prim.Parent;
                            //Console.WriteLine($"PSP MODEL DIDNT ERROR! {PSP_MeshType:X5} {item.Name} {prim.ShaderName} {File.FullName}");
                        }
                    }
                }
                
            }
            else if (Version == 0x00020001) // XBOX
            {
                Data = reader.ReadBytes((int)length - 12);
                using (var stream = new MemoryStream(Data))
                {
                    using (var preader = new BinaryReader(stream))
                    {
                        try {
                            ReadXBOX(preader);
                        }
                        catch {
                            var prim = (PrimitiveGroup)Parent;
                            var item = (Named)prim.Parent;
                            Console.WriteLine($"FAILED XBOX MODEL LONG! {item.Name} {prim.ShaderName}  {File.FullName}");
                        }
                        if (stream.Position != stream.Length)
                        {
                            var prim = (PrimitiveGroup)Parent;
                            var item = (Named)prim.Parent;
                            Console.WriteLine($"FAILED XBOX MODEL SHORT! {item.Name} {prim.ShaderName}  {File.FullName}");
                        }
                    }
                }
            }
            else
            {
                // PS2
                reader.ReadBytes(8);
                Data = reader.ReadBytes((int)length - 0x14);
                Vertexes = CalculateData(Data);
            }
        }

        void ReadPSP(BinaryReader reader)
        {
            var prim = (PrimitiveGroup)Parent;
            var matpal = prim.GetChild<MatrixPalette>();
            uint UnkVal1 = reader.ReadUInt32(); // version? (3 or 4)
            uint Bitfield = reader.ReadUInt32();

            bool HasOneByteUV = (Bitfield & (1 << 0)) != 0; // used only in Titans/MoM
            //bool HasTwoByteUV = (Bitfield & (1 << 1)) != 0; // disabled only in Titans/MoM, when previous one is enabled
            bool HasUnk2 = (Bitfield & (1 << 2)) != 0; // ?
            //bool HasUnk3 = (Bitfield & (1 << 3)) != 0; // always the same as previous one

            bool HasColors = (Bitfield & (1 << 4)) != 0;
            bool HasNormals = (Bitfield & (1 << 5)) != 0;
            //bool Unused1 = (Bitfield & (1 << 6)) != 0;
            bool UncompressedPositions = (Bitfield & (1 << 7)) != 0;

            //bool AlwaysTrue = (Bitfield & (1 << 8)) != 0; // always true - pos?
            bool HasFourBoneIndices = (Bitfield & (1 << 9)) != 0;
            //bool Unused2 = (Bitfield & (1 << 10)) != 0;
            bool HasByteIndices = (Bitfield & (1 << 11)) != 0;

            bool HasShortIndices = (Bitfield & (1 << 12)) != 0;
            //bool Unused3 = (Bitfield & (1 << 13)) != 0;
            bool HasUnk4 = (Bitfield & (1 << 14)) != 0; // ?
            bool HasUnk5 = (Bitfield & (1 << 15)) != 0; // ?

            bool HasEightBoneIndices = (Bitfield & (1 << 16)) != 0;

            PSP_MeshType = Bitfield;

            uint VCount = reader.ReadUInt32();
            uint IndicesCount = 0;
            uint MatricesCount = prim.NumMatrices;
            /*
            if (MatricesCount % 2 != 0)
            {
                MatricesCount++;
            }
            */
            float UV_ScaleX = 1f;
            float UV_ScaleY = 1f;
            float UV_OffsetX = 0f;
            float UV_OffsetY = 0f;
            float ModelScaleX = 1f;
            float ModelScaleY = 1f;
            float ModelScaleZ = 1f;
            float ModelOffsetX = 0f;
            float ModelOffsetY = 0f;
            float ModelOffsetZ = 0f;
            if (VCount != prim.NumVertices)
            {
                VCount = reader.ReadUInt32();
                IndicesCount = reader.ReadUInt32();
                reader.ReadUInt32();
                reader.ReadUInt32();
                reader.ReadUInt32();
                MatricesCount = reader.ReadUInt32();
                reader.ReadUInt32();
                reader.ReadUInt32();
                reader.ReadUInt32();
                UV_ScaleX = reader.ReadSingle();
                UV_ScaleY = reader.ReadSingle();
                UV_OffsetX = reader.ReadSingle();
                UV_OffsetY = reader.ReadSingle();
                ModelScaleX = reader.ReadSingle();
                ModelScaleY = reader.ReadSingle();
                ModelScaleZ = reader.ReadSingle();
                ModelOffsetX = reader.ReadSingle();
                ModelOffsetY = reader.ReadSingle();
                ModelOffsetZ = reader.ReadSingle();
                reader.ReadSingle();
            }
            else
            {
                IndicesCount = reader.ReadUInt32();
                ModelScaleX = reader.ReadSingle();
                ModelScaleY = reader.ReadSingle();
                ModelScaleZ = reader.ReadSingle();
                ModelOffsetX = reader.ReadSingle();
                ModelOffsetY = reader.ReadSingle();
                ModelOffsetZ = reader.ReadSingle();
                UV_ScaleX = reader.ReadSingle();
                UV_ScaleY = reader.ReadSingle();
                UV_OffsetX = reader.ReadSingle();
                UV_OffsetY = reader.ReadSingle();
            }
            reader.ReadBytes(0x60); // 24 floats (bounding box 8x Vector3)
            reader.ReadUInt32(); // vertex data offset
            reader.ReadUInt32(); // index data offset

            uint ExtraPadding = 4 - MatricesCount;
            if (HasEightBoneIndices)
            {
                ExtraPadding = 8 - MatricesCount;
            }
            //if (MatricesCount != 4 && MatricesCount != 8)
            //{
            //    ExtraPadding--;
            //}
            
            for (int i = 0; i < VCount; i++)
            {
                var vert = new VertexData();
                if (HasEightBoneIndices || HasFourBoneIndices)
                {
                    int wpos = 0;
                    for (int a = 0; a < MatricesCount; a++)
                    {
                        byte Wgt = reader.ReadByte();
                        if (Wgt != 0 && wpos < 4)
                        {
                            vert.JointIndexes[wpos] = a;
                            vert.Weights[wpos] = Wgt / 128f;
                            wpos++;
                        }
                    }
                    //if (MatricesCount != 4 && MatricesCount != 8)
                    //{
                    //    reader.ReadByte();
                    //}
                }
                if (prim.UVCount != PrimitiveGroup.VertexUVCount.UVx0)
                {
                    if (HasOneByteUV)
                    {
                        vert.U = ((reader.ReadByte() / 128f) * UV_ScaleX) + UV_OffsetX;
                        vert.V = ((reader.ReadByte() / 128f) * UV_ScaleY) + UV_OffsetY;
                        reader.ReadByte();
                        reader.ReadByte();
                    }
                    else
                    {
                        vert.U = ((reader.ReadUInt16() / 32768f) * UV_ScaleX) + UV_OffsetX;
                        vert.V = ((reader.ReadUInt16() / 32768f) * UV_ScaleY) + UV_OffsetY;
                    }
                }
                if (HasColors)
                {
                    vert.R = reader.ReadByte();
                    vert.G = reader.ReadByte();
                    vert.B = reader.ReadByte();
                    vert.A = reader.ReadByte();
                }
                if (HasNormals)
                {
                    vert.BNX = reader.ReadByte();
                    vert.BNY = reader.ReadByte();
                    vert.BNZ = reader.ReadByte();
                    reader.ReadByte();
                }
                if (HasEightBoneIndices || HasFourBoneIndices)
                {
                    for (int a = 0; a < ExtraPadding; a++)
                    {
                        reader.ReadByte();
                    }
                }
                if (!UncompressedPositions)
                {
                    short X = reader.ReadInt16();
                    short Y = reader.ReadInt16();
                    short Z = reader.ReadInt16();
                    short W = reader.ReadInt16();
                    vert.X = (X / 32768f) * ModelScaleX + ModelOffsetX;
                    vert.Y = (Y / 32768f) * ModelScaleY + ModelOffsetY;
                    vert.Z = (Z / 32768f) * ModelScaleZ + ModelOffsetZ;
                }
                else
                {
                    // also add the model scale and offset?
                    vert.X = reader.ReadSingle();
                    vert.Y = reader.ReadSingle();
                    vert.Z = reader.ReadSingle();
                }
                Vertexes.Add(vert);
            }
            if (HasByteIndices)
            {
                for (int i = 0; i < IndicesCount; i++)
                {
                    Indices.Add(reader.ReadByte());
                }
            }
            else
            {
                for (int i = 0; i < IndicesCount; i++)
                {
                    Indices.Add(reader.ReadUInt16());
                }
            }
            
        }

        void ReadXBOX(BinaryReader reader)
        {
            var prim = (PrimitiveGroup)Parent;
            for (int i = 0; i < prim.NumVertices; i++)
            {
                Vertexes.Add(new VertexData());
            }
            for (int i = 0; i < prim.NumVertices; i++)
            {
                if (prim.HasNormals)
                {
                    Vertexes[i].BNX = reader.ReadByte();
                    Vertexes[i].BNY = reader.ReadByte();
                    Vertexes[i].BNZ = reader.ReadByte();
                    reader.ReadByte();
                }
                if (prim.HasPositions)
                {
                    Vertexes[i].X = reader.ReadSingle();
                    Vertexes[i].Y = reader.ReadSingle();
                    Vertexes[i].Z = reader.ReadSingle();
                }
                if (prim.HasColors)
                {
                    Vertexes[i].R = reader.ReadByte();
                    Vertexes[i].G = reader.ReadByte();
                    Vertexes[i].B = reader.ReadByte();
                    Vertexes[i].A = reader.ReadByte();
                }
                if (prim.UVCount != PrimitiveGroup.VertexUVCount.UVx0)
                {
                    Vertexes[i].U = reader.ReadSingle();
                    Vertexes[i].V = reader.ReadSingle();
                }
            }
        }

        public List<VertexData> CalculateData(byte[] VifCode)
        {
            var vertexes = new List<VertexData>();

            var interpreter = VIFInterpreter.InterpretCode(VifCode);
            var data = interpreter.GetMem();
            var Vertexes = new List<Vector4>();
            var UVW = new List<Vector4>();
            var EmitColor = new List<Vector4>();
            var Colors = new List<Color>();
            var Normals = new List<Vector4>();
            var Connection = new List<bool>();
            var index = 0;
            for (var i = 0; i < data.Count;)
            {
                var verts = (data[i][0].GetBinaryX() & 0xFF);
                var fieldsPresent = FieldsPresent.Vertex;
                var outputAddr = interpreter.GetAddressOutput();
                var fields = 0;
                bool AltUV = false;
                if (verts == 0)
                {
                    i++;
                    continue;
                }
                foreach (var addr in outputAddr[index++])
                {
                    switch (addr)
                    {
                        case 0x3:
                            fieldsPresent |= FieldsPresent.UVs;
                            fields++;

                            var uv_con = data[i].Where((v) => v != null);
                            foreach (var e in uv_con)
                            {
                                //var conn = (e.GetBinaryX() & 0xFF00) >> 8;
                                var conn = (e.GetBinaryX() & 0xFF00) >> 4;
                                Connection.Add(conn == 128 ? false : true);

                                Vector4 uv = new Vector4(e);
                                if (e.FMT == PackFormat.V2_32)
                                {
                                    // V2_32 - no changes, just the UV's as floats, might use last few bits for something?
                                    uv.Y += -0.02f;
                                }
                                else
                                {
                                    // V4_16 - X and Y compressed UVs, has normals? colors all black
                                    short corX = (short)(uv.BX);
                                    short corY = (short)(uv.BY);
                                    uv.X = corX / 4095f; // 0xFFF
                                    uv.Y = corY / 4095f; // 0xFFF
                                    AltUV = true;
                                    //uv.Y += -0.03f;
                                }

                                UVW.Add(uv);
                            }
                            
                            break;
                        case 0x4:
                            //fieldsPresent |= FieldsPresent.Color;
                            //fields++;
                            // Normals? (V3_32)

                            // 00 00 00 7F
                            /*
                            foreach (var e in data[i])
                            {
                                if (e == null)
                                    break;
                                var r = Math.Min(e.GetBinaryX() & 0xFF, 255);
                                var g = Math.Min(e.GetBinaryY() & 0xFF, 255);
                                var b = Math.Min(e.GetBinaryZ() & 0xFF, 255);
                                var a = (e.GetBinaryW() & 0xFF) << 1;

                                Color col = new Color((byte)r, (byte)g, (byte)b, (byte)a);
                                if (AltUV)
                                {
                                    col = new Color(255, 255, 255, 255);
                                }
                                Colors.Add(col);
                            }
                            */
                            /*
                            foreach (var e in data[i + 4])
                            {
                                if (e == null)
                                    break;
                                Normals.Add(new Vector4(e.X, e.Y, e.Z, 1.0f));
                            }
                            */
                            break;
                        case 0x5:
                            fieldsPresent |= FieldsPresent.Vertex;
                            fields++;

                            Vertexes.AddRange(data[i].Where((v) => v != null));
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                TrimList(UVW, Vertexes.Count);
                TrimList(EmitColor, Vertexes.Count);
                TrimList(Normals, Vertexes.Count, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
            }

            for (int i = 0; i < Vertexes.Count; i++)
            {
                var vertData = new VertexData
                {
                    X = Vertexes[i].X,
                    Y = Vertexes[i].Y,
                    Z = Vertexes[i].Z,
                    U = UVW[i].X,
                    V = UVW[i].Y,
                    R = 255,
                    G = 255,
                    B = 255,
                    A = 255,
                    //R = Colors[i].R,
                    //G = Colors[i].G,
                    //B = Colors[i].B,
                    //A = Colors[i].A,
                    //Conn = Connection[i]
                };
                vertexes.Add(vertData);
            }

            return vertexes;
        }

        [System.Flags]
        public enum FieldsPresent
        {
            Vertex = 0,
            //UV_Color = 1,
            //Normals = 2,
            //EmitColors = 4
            UVs = 1,
            Color = 2,
            EmitColors = 4,
        }

        private void TrimList(List<Vector4> list, int desiredLength, Vector4 defaultValue = null)
        {
            if (list != null)
            {
                if (list.Count > desiredLength)
                {
                    list.RemoveRange(desiredLength, list.Count - desiredLength);
                }
                while (list.Count < desiredLength)
                {
                    if (defaultValue != null)
                    {
                        list.Add(new Vector4(defaultValue));
                    }
                    else
                    {
                        list.Add(new Vector4());
                    }
                }
            }
        }

        public class VertexData
        {
            public float X, Y, Z;
            public float U, V;
            public byte R, G, B, A;
            public float[] Weights = new float[4];
            public int[] JointIndexes = new int[4];
            public bool Conn;
            public byte BNX, BNY, BNZ;
        }
    }
}