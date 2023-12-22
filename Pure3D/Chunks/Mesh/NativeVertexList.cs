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
            Lines.AppendLine($"Compressed Positions: {HasComprPos}");
            Lines.AppendLine($"Length: {Data.Length}");
            Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }

        public byte[] VifCode { get; set; }
        public List<VertexData> Vertexes = new List<VertexData>();
        public int Version;
        public int UnkParam;
        public int VifSize;
        bool HasComprPos;

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadInt32();
            UnkParam = reader.ReadInt32();
            VifSize = reader.ReadInt32();
            Data = reader.ReadBytes((int)length - 12);
            //reader.BaseStream.Position -= length;
            //reader.ReadBytes(0x14); // version, unkparam, vifSize
            //VifCode = reader.ReadBytes((int)length - 0x14);
            //Vertexes = CalculateData();
            if (Version == 0x40001)
            {
                // PSP
                using (var stream = new MemoryStream(Data))
                {
                    using (var preader = new BinaryReader(stream))
                    {
                        try{
                            ReadPSP(preader, stream.Length);
                        }
                        catch{
                            Console.WriteLine("FAILED TO LOAD PSP MODEL!");
                        }
                        
                    }
                }
            }
        }

        void ReadPSP(BinaryReader reader, long length)
        {
            var prim = (PrimitiveGroupCTTR)Parent;
            var matpal = prim.GetChild<MatrixPalette>();
            uint UnkVal1 = reader.ReadUInt32();
            uint Bitfield = reader.ReadUInt32();

            bool CompressedPositions = (Bitfield & (1 << 0)) != 0;
            bool UnkBit2 = (Bitfield & (1 << 1)) != 0;
            bool UnkBit3 = (Bitfield & (1 << 2)) != 0; // binormals?
            bool UnkBit4 = (Bitfield & (1 << 3)) != 0; // tangents?

            bool HasColors = (Bitfield & (1 << 4)) != 0;
            bool UnkBit5 = (Bitfield & (1 << 5)) != 0;
            bool HasNormals = (Bitfield & (1 << 6)) != 0;
            bool HasPos = (Bitfield & (1 << 7)) != 0;

            bool HasUVs = (Bitfield & (1 << 8)) != 0;
            HasComprPos = CompressedPositions;

            uint VCount = reader.ReadUInt32();
            if (VCount < prim.NumVertices)
            {
                VCount = reader.ReadUInt32();
                reader.ReadBytes(0xB4);
            }
            else
            {
                reader.ReadBytes(0x94);
            }
            for (int i = 0; i < VCount; i++)
            {
                Vertexes.Add(new VertexData());
            }
            for (int i = 0; i < VCount; i++)
            {
                if (prim.HasWeights)
                {
                    reader.ReadUInt32();
                    Vertexes[i].Joint.Weight1 = 1f;
                    //Vertexes[i].Joint.Weight1 = reader.ReadByte() / 255f;
                    //Vertexes[i].Joint.Weight2 = reader.ReadByte() / 255f;
                    //Vertexes[i].Joint.Weight3 = reader.ReadByte() / 255f;
                    //Vertexes[i].Joint.Weight4 = reader.ReadByte() / 255f;
                }
                if (prim.HasBoneIndices)
                {
                    reader.ReadUInt32();
                    Vertexes[i].Joint.JointIndex1 = 0;//reader.ReadByte();
                    Vertexes[i].Joint.JointIndex2 = 0;//reader.ReadByte();
                    Vertexes[i].Joint.JointIndex3 = 0;//reader.ReadByte();
                    Vertexes[i].Joint.JointIndex4 = 0;//reader.ReadByte();
                }
                if (prim.UVCount != PrimitiveGroupCTTR.VertexUVCount.UVx0)
                {
                    Vertexes[i].U = reader.ReadInt16() / 32768f;
                    Vertexes[i].V = reader.ReadInt16() / 32768f;
                }
                if (prim.HasColors)
                {
                    Vertexes[i].R = reader.ReadByte();
                    Vertexes[i].G = reader.ReadByte();
                    Vertexes[i].B = reader.ReadByte();
                    Vertexes[i].A = reader.ReadByte();
                }
                if (prim.HasNormals)
                {
                    Vertexes[i].NX = (reader.ReadByte() / 127f) - 1f;
                    Vertexes[i].NY = (reader.ReadByte() / 127f) - 1f;
                    Vertexes[i].NZ = (reader.ReadByte() / 127f) - 1f;
                    reader.ReadByte();
                }
                if (prim.HasPositions)
                {
                    if (CompressedPositions)
                    {
                        //todo
                        short X = reader.ReadInt16();
                        short Y = reader.ReadInt16();
                        short Z = reader.ReadInt16();
                        short W = reader.ReadInt16();
                        Vertexes[i].X = 0f;
                        Vertexes[i].Y = 0f;
                        Vertexes[i].Z = 0f;
                    }
                    else
                    {
                        Vertexes[i].X = reader.ReadSingle();
                        Vertexes[i].Y = reader.ReadSingle();
                        Vertexes[i].Z = reader.ReadSingle();
                    }
                }
            }
        }

        public List<VertexData> CalculateData()
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
                            fieldsPresent |= FieldsPresent.Vertex;
                            fields++;
                            break;
                        case 0x4:
                            fieldsPresent |= FieldsPresent.UVs;
                            fields++;
                            break;
                        case 0x5:
                            fieldsPresent |= FieldsPresent.Color;
                            fields++;
                            break;
                        case 0x6:
                            fieldsPresent |= FieldsPresent.EmitColors;
                            fields++;
                            break;
                    }
                    if (i + fields + 2 >= data.Count)
                        break;

                }
                Vertexes.AddRange(data[i + 2].Where((v) => v != null));
                if (fieldsPresent.HasFlag(FieldsPresent.UVs))
                {
                    var uv_con = data[i + 3].Where((v) => v != null);
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
                }
                if (fieldsPresent.HasFlag(FieldsPresent.Color))
                {
                    // 00 00 00 7F
                    foreach (var e in data[i + 4])
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
                    /*
                    foreach (var e in data[i + 4])
                    {
                        if (e == null)
                            break;
                        Normals.Add(new Vector4(e.X, e.Y, e.Z, 1.0f));
                    }
                    */
                }
                if (fieldsPresent.HasFlag(FieldsPresent.EmitColors))
                {
                    // not used?
                    throw new NotImplementedException();
                    foreach (var e in data[i + fields + 1])
                    {
                        if (e == null)
                            break;
                        Vector4 emit = new Vector4(e);
                        emit.X = (emit.GetBinaryX() & 0xFF);// / 256.0f;
                        emit.Y = (emit.GetBinaryY() & 0xFF);// / 256.0f;
                        emit.Z = (emit.GetBinaryZ() & 0xFF);// / 256.0f;
                        emit.W = (emit.GetBinaryW() & 0xFF);// / 256.0f;
                        EmitColor.Add(emit);
                    }
                }
                i += fields + 2;
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
                    R = Colors[i].R,
                    G = Colors[i].G,
                    B = Colors[i].B,
                    A = Colors[i].A,
                    NX = Normals[i].X,
                    NY = Normals[i].Y,
                    NZ = Normals[i].Z,
                    ER = (byte)EmitColor[i].X,
                    EG = (byte)EmitColor[i].Y,
                    EB = (byte)EmitColor[i].Z,
                    EA = (byte)EmitColor[i].W,
                    Conn = Connection[i]
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

        public class JointInfo
        {
            public float Weight1;
            public float Weight2;
            public float Weight3;
            public float Weight4;
            public int JointIndex1;
            public int JointIndex2;
            public int JointIndex3;
            public int JointIndex4;
        }

        public class VertexData
        {
            public float X, Y, Z;
            public float NX, NY, NZ;
            public float U, V;
            public byte R, G, B, A;
            public JointInfo Joint = new JointInfo();
            public byte ER, EG, EB, EA; // Emit colors
            public bool Conn;
            public List<BlendShapeVertex> BlendShapes = new List<BlendShapeVertex>();
        }

        public class BlendShapeVertex
        {
            public Vector4 Offset;
        }
    }
}