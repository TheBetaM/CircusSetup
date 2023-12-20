using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121104)]
    public class Vector3Channel : Chunk
    {
        public uint Version;
        public uint NumberOfFrames;
        public string Parameter;
        public Vector3[] Values;
        public ushort[] Frames;

        public Vector3Channel(File file, uint type) : base(file, type)
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

            Values = new Vector3[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                Values[i] = Util.ReadVector3(reader);
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
                Util.WriteVector3(writer, Values[i]);
            }
        }

        public override string ToString()
        {
            return $"Vector3 Channel: {Parameter}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Vector3 Channel");
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
