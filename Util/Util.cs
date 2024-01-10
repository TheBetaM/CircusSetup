using System.IO;
using System.Text;
using System.Numerics;
using System;

namespace Pure3D
{
    public static class Util
    {
        public static bool ExportToGodot = true;
        public static bool IsDemo = false; // for CTTR demo

        // ReadString accessor because Pure3D loves null terminated strings.
        public static string ReadString(BinaryReader reader, ref ulong padding)
        {
            byte strLen = reader.ReadByte();
            if (strLen == 0) return string.Empty;
            string str = Encoding.ASCII.GetString(reader.ReadBytes(strLen));
            str = ZeroTerminate(str);
            padding = strLen - (ulong)str.Length;

            return str;
        }

        public static void WriteString(BinaryWriter writer, string str, ulong padding)
        {
            if (padding > 0)
            {
                for (ulong i = 0; i < padding; i++)
                {
                    str += char.MinValue;
                }
            }
            writer.Write(str);
        }

        public static string ZeroTerminate(string str)
        {
            int length = str.IndexOf(char.MinValue);
            return length != -1 ? str.Substring(0, length) : str;
        }

        public static string ReadString2(BinaryReader reader)
        {
            int strLen = reader.ReadInt32();
            if (strLen == 0) return string.Empty;
            string str = Encoding.ASCII.GetString(reader.ReadBytes(strLen));
            reader.ReadByte();

            return str;
        }
        public static void WriteString2(BinaryWriter writer, string str)
        {
            writer.Write((uint)str.Length);
            writer.Write(str.ToCharArray());
            writer.Write((byte)0);
        }

        public static Vector2 ReadVector2(BinaryReader reader)
        {
            Vector2 vector = new Vector2();

            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();

            return vector;
        }

        public static void WriteVector2 (BinaryWriter writer, Vector2 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
        }

        public static Vector3 ReadVector3(BinaryReader reader)
        {
            Vector3 vector = new Vector3();

            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();

            return vector;
        }

        public static void WriteVector3(BinaryWriter writer, Vector3 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        public static Vector4 ReadVector4(BinaryReader reader)
        {
            Vector4 vector = new Vector4();

            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();
            vector.W = reader.ReadSingle();

            return vector;
        }

        public static void WriteVector4(BinaryWriter writer, Vector4 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
            writer.Write(vector.W);
        }

        public static Quaternion ReadQuaternion(BinaryReader reader)
        {
            Quaternion vector = new Quaternion();

            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();
            vector.W = reader.ReadSingle();

            return vector;
        }

        public static void WriteQuaternion(BinaryWriter writer, Quaternion vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
            writer.Write(vector.W);
        }

        public static Matrix4x4 ReadMatrix(BinaryReader reader)
        {
            Matrix4x4 matrix = new Matrix4x4();

            matrix.M11 = reader.ReadSingle();
            matrix.M12 = reader.ReadSingle();
            matrix.M13 = reader.ReadSingle();
            matrix.M14 = reader.ReadSingle();
            matrix.M21 = reader.ReadSingle();
            matrix.M22 = reader.ReadSingle();
            matrix.M23 = reader.ReadSingle();
            matrix.M24 = reader.ReadSingle();
            matrix.M31 = reader.ReadSingle();
            matrix.M32 = reader.ReadSingle();
            matrix.M33 = reader.ReadSingle();
            matrix.M34 = reader.ReadSingle();
            matrix.M41 = reader.ReadSingle();
            matrix.M42 = reader.ReadSingle();
            matrix.M43 = reader.ReadSingle();
            matrix.M44 = reader.ReadSingle();

            return matrix;
        }

        public static void WriteMatrix(BinaryWriter writer, Matrix4x4 matrix)
        {
            writer.Write(matrix.M11);
            writer.Write(matrix.M12);
            writer.Write(matrix.M13);
            writer.Write(matrix.M14);
            writer.Write(matrix.M21);
            writer.Write(matrix.M22);
            writer.Write(matrix.M23);
            writer.Write(matrix.M24);
            writer.Write(matrix.M31);
            writer.Write(matrix.M32);
            writer.Write(matrix.M33);
            writer.Write(matrix.M34);
            writer.Write(matrix.M41);
            writer.Write(matrix.M42);
            writer.Write(matrix.M43);
            writer.Write(matrix.M44);
        }

        public static ByteColour ReadColour(BinaryReader reader)
        {
            ByteColour vector = new ByteColour();

            vector.R = reader.ReadByte();
            vector.G = reader.ReadByte();
            vector.B = reader.ReadByte();
            vector.A = reader.ReadByte();

            return vector;
        }

        public static ByteColour ReadColourBGR(BinaryReader reader)
        {
            ByteColour vector = new ByteColour();

            vector.B = reader.ReadByte();
            vector.G = reader.ReadByte();
            vector.R = reader.ReadByte();
            vector.A = reader.ReadByte();

            return vector;
        }

        public static ByteColour ReadColour24(BinaryReader reader)
        {
            ByteColour vector = new ByteColour();

            vector.R = reader.ReadByte();
            vector.G = reader.ReadByte();
            vector.B = reader.ReadByte();
            vector.A = 255;

            return vector;
        }

        public static ByteColour ReadColour2(BinaryReader2 reader)
        {
            ByteColour vector = new ByteColour();

            vector.A = reader.ReadByte();
            vector.B = reader.ReadByte();
            vector.G = reader.ReadByte();
            vector.R = reader.ReadByte();

            return vector;
        }

        public static void WriteColour(BinaryWriter writer, ByteColour vector)
        {
            writer.Write(vector.R);
            writer.Write(vector.G);
            writer.Write(vector.B);
            writer.Write(vector.A);
        }

        public static void WriteColourBGR(BinaryWriter writer, ByteColour vector)
        {
            writer.Write(vector.B);
            writer.Write(vector.G);
            writer.Write(vector.R);
            writer.Write(vector.A);
        }

        public static Topology ReadTopology(BinaryReader reader)
        {
            Topology vector = new Topology();

            vector.V0 = reader.ReadUInt16();
            vector.V1 = reader.ReadUInt16();
            vector.V2 = reader.ReadUInt16();
            vector.N0 = reader.ReadUInt16();
            vector.N1 = reader.ReadUInt16();
            vector.N2 = reader.ReadUInt16();

            return vector;
        }

        public static void WriteTopology(BinaryWriter writer, Topology vector)
        {
            writer.Write(vector.V0);
            writer.Write(vector.V1);
            writer.Write(vector.V2);
            writer.Write(vector.N0);
            writer.Write(vector.N1);
            writer.Write(vector.N2);
        }

        public static string ToLine(this byte[] array)
        {
            StringBuilder line = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                line.Append(array[i].ToString("X2"));
            }
            return line.ToString();
        }
    }

    public class BinaryReader2 : BinaryReader
    {
        public BinaryReader2(System.IO.Stream stream) : base(stream) { }

        public override int ReadInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        public override Int16 ReadInt16()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }

        public override Int64 ReadInt64()
        {
            var data = base.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }

        public override UInt32 ReadUInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public override float ReadSingle()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }

    }

    public class BinaryWriter2 : BinaryWriter
    {
        public BinaryWriter2(System.IO.Stream stream) : base(stream) { }

        public void WriteBigEndian(UInt32 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

        public void WriteBigEndian(UInt16 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

        public void WriteBigEndian(UInt64 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

        public void WriteBigEndian(float val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

    }
}
