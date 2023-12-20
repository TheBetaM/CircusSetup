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
        public byte[] Values1; // Array of 4 bytes
        public byte[] Values2;
        public byte[] Values3;
        public byte[] Values4;
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

            Values1 = new byte[NumberOfFrames];
            Values2 = new byte[NumberOfFrames];
            Values3 = new byte[NumberOfFrames];
            Values4 = new byte[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values1[i] = reader.ReadByte();
                Values2[i] = reader.ReadByte();
                Values3[i] = reader.ReadByte();
                Values4[i] = reader.ReadByte();
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
                writer.Write(Values1[i]);
                writer.Write(Values2[i]);
                writer.Write(Values3[i]);
                writer.Write(Values4[i]);
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
            for (int i = 0; i < Values1.Length; i++)
            {
                Lines.AppendLine($"Values{i}: {Values1[i]} / {Values2[i]} / {Values3[i]} / {Values4[i]}");
            }

            return Lines.ToString();
        }
    }
}
