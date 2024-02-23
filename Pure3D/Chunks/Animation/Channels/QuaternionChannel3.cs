using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121113)]
    public class QuaternionChannel3 : Chunk
    {
        public uint Version;
        public uint NumberOfFrames;
        public string Parameter;
        public sbyte[,] Values; // Array of XYZW angles
        public ushort[] Frames;

        public QuaternionChannel3(File file, uint type) : base(file, type)
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

            Values = new sbyte[NumberOfFrames, 4];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i, 0] = reader.ReadSByte();
                Values[i, 1] = reader.ReadSByte();
                Values[i, 2] = reader.ReadSByte();
                Values[i, 3] = reader.ReadSByte();
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
                writer.Write(Values[i, 0]);
                writer.Write(Values[i, 0]);
                writer.Write(Values[i, 0]);
            }

        }

        public override string ToString()
        {
            return $"Quaternion Channel 3: {Parameter}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Quaternion Channel 3");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"NumberOfFrames: {NumberOfFrames}");
            Lines.AppendLine($"Parameter: {Parameter}");
            for (int i = 0; i < Frames.Length; i++)
            {
                Lines.AppendLine($"Frame{i}: {Frames[i]}");
            }
            for (int i = 0; i < Values.Length; i++)
            {
                Lines.AppendLine($"Values{i}: {Values[i, 0]} / {Values[i, 1]} / {Values[i, 2]} / {Values[i, 3]}");
            }

            return Lines.ToString();
        }
    }
}
