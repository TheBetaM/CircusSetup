using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121112)]
    public class QuaternionChannel2 : Chunk
    {
        public uint Version;
        public uint NumberOfFrames;
        public string Parameter;
        public short[,] Values; // Array of XYZ angles
        public ushort[] Frames;

        public QuaternionChannel2(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            Parameter = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            NumberOfFrames = reader.ReadUInt32();

            Frames = new ushort[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Frames[i] = reader.ReadUInt16();
            }

            Values = new short[NumberOfFrames, 3];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i, 0] = reader.ReadInt16();
                Values[i, 1] = reader.ReadInt16();
                Values[i, 2] = reader.ReadInt16();
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            for (int i = 0; i < 4; i++)
            {
                if (i < Parameter.Length)
                {
                    writer.Write((byte)Parameter[i]);
                }
                else
                {
                    writer.Write((byte)0x00);
                }
            }
            writer.Write(NumberOfFrames);
            for (int i = 0; i < NumberOfFrames; i++)
            {
                writer.Write(Frames[i]);
            }
            for (int i = 0; i < NumberOfFrames; i++)
            {
                writer.Write(Values[i, 0]);
                writer.Write(Values[i, 1]);
                writer.Write(Values[i, 2]);
            }

        }

        public override string ToString()
        {
            return $"Quaternion Channel 2: {Parameter}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Quaternion Channel 2");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"NumberOfFrames: {NumberOfFrames}");
            Lines.AppendLine($"Parameter: {Parameter}");
            for (int i = 0; i < Frames.Length; i++)
            {
                Lines.AppendLine($"Frame{i}: {Frames[i]}");
            }
            for (int i = 0; i < Values.GetLength(0); i++)
            {
                //float angX = 180f * (Values[i, 0] / 32767f);
                //float angY = 180f * (Values[i, 1] / 32767f);
                //float angZ = 180f * (Values[i, 2] / 32767f);
                Lines.AppendLine($"Values{i}: {Values[i,0]} {Values[i,1]} {Values[i,2]}");
                float angX = (float)Values[i, 0] / short.MaxValue;
                float angY = (float)Values[i, 1] / short.MaxValue;
                float angZ = (float)Values[i, 2] / short.MaxValue;
                float angW = (float)Math.Sqrt(1 - (angX * angX + angY * angY + angZ * angZ));
                Lines.AppendLine($"Quat{i}: {angX} {angY} {angZ} {angW}");
            }
            /*
            for (int i = 0; i < Values.GetLength(0); i++)
            {
                //float angX = 180f * (Values[i, 0] / 32767f);
                //float angY = 180f * (Values[i, 1] / 32767f);
                //float angZ = 180f * (Values[i, 2] / 32767f);
                float angX = (float)Math.PI * 0.5f * (Values[i, 0] / 32767f);
                float angY = (float)Math.PI * 0.5f  * (Values[i, 1] / 32767f);
                float angZ = (float)Math.PI * 0.5f  * (Values[i, 2] / 32767f);
                Lines.AppendLine($"Values{i}: {Values[i,0]} {Values[i,1]} {Values[i,2]}");// | {angX} {angY} {angZ}");
                Quaternion quat1 = new Quaternion(angX, angY, angZ, 1f);
                float ang2X = (float)Math.PI * 1f * (Values[i, 0] / 32767f);
                float ang2Y = (float)Math.PI * 1f * (Values[i, 1] / 32767f);
                float ang2Z = (float)Math.PI * 1f * (Values[i, 2] / 32767f);
                Quaternion quat2 = Quaternion.CreateFromYawPitchRoll(ang2Y, ang2X, ang2Z);
                Lines.AppendLine($"Quat1{i}: {quat1.X} {quat1.Y} {quat1.Z} {quat1.W}");
                Lines.AppendLine($"Quat2{i}: {quat2.X} {quat2.Y} {quat2.Z} {quat2.W}");
            }
            */

            return Lines.ToString();
        }
    }
}
