using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121107)]
    public class EntityChannel : Chunk
    {
        public uint Version;
        public uint NumberOfFrames;
        public string Parameter;
        public string[] Values;
        public ushort[] Frames;
        public ulong[] Values_padding;

        public EntityChannel(File file, uint type) : base(file, type)
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

            Values = new string[NumberOfFrames];
            Values_padding = new ulong[NumberOfFrames];
            for (int i = 0; i < NumberOfFrames; i++)
            {
                ulong outpad = 0;
                Values[i] = Util.ReadString(reader, ref outpad);
                Values_padding[i] = outpad;
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Entity Channel: {Parameter}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Entity Channel");
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