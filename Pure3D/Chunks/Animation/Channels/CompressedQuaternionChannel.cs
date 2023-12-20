using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121111)]
    public class CompressedQuaternionChannel : Chunk
    {
        public uint Version;
        public uint NumberOfFrames;
        public string Parameter;
        public Quaternion[] Values;
        public ushort[] Frames;

        public CompressedQuaternionChannel(File file, uint type) : base(file, type)
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

            Values = new Quaternion[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                var w = reader.ReadInt16() / (float)short.MaxValue;
                Values[i] = new Quaternion
                (
                    reader.ReadInt16() / (float)short.MaxValue,
                    reader.ReadInt16() / (float)short.MaxValue,
                    reader.ReadInt16() / (float)short.MaxValue,
                    w
                );
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
                writer.Write(Values[i].W * short.MaxValue);
                writer.Write(Values[i].X * short.MaxValue);
                writer.Write(Values[i].Y * short.MaxValue);
                writer.Write(Values[i].Z * short.MaxValue);
            }
        }

        public override string ToString()
        {
            return $"Compressed Quaternion Channel: {Parameter}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Compressed Quaternion Channel");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"NumberOfFrames: {NumberOfFrames}");
            Lines.AppendLine($"Parameter: {Parameter}");
            for (int i = 0; i < Frames.Length; i++)
            {
                Lines.AppendLine($"Frame{i}: {Frames[i]}");
            }
            for (int i = 0; i < Values.Length; i++)
            {
                Lines.AppendLine($"Values{i}: {Values[i]}");
            }

            return Lines.ToString();
        }
    }
}
