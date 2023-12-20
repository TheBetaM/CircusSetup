using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x121101)]
    public class Float2Channel : Chunk
    {
        public uint Version;
        public uint NumberOfFrames;
        public string Parameter;
        public Vector2[] Values;
        public ushort[] Frames;

        public Float2Channel(File file, uint type) : base(file, type)
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

            Values = new Vector2[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Vector2 vec = new Vector2();
                vec.X = reader.ReadSingle();
                vec.Y = reader.ReadSingle();
                Values[i] = vec;
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
                writer.Write(Values[i].X);
                writer.Write(Values[i].Y);
            }
        }

        public override string ToString()
        {
            return $"Float2 Channel: {Parameter}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Float2 Channel");
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