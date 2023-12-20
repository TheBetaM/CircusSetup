using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x121000)]
    public class Animation : VersionNamed
    {
        public string AnimType;
        public float NumberOfFrames;
        public float FrameRate;
        public uint Looping;

        public Animation(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            AnimType = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
            NumberOfFrames = reader.ReadSingle();
            FrameRate = reader.ReadSingle();
            Looping = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            for (int i = 0; i < 4; i++)
            {
                if (i < AnimType.Length)
                {
                    writer.Write((byte)AnimType[i]);
                }
                else
                {
                    writer.Write((byte)0x00);
                }
            }
            writer.Write(NumberOfFrames);
            writer.Write(FrameRate);
            writer.Write(Looping);
        }

        public override string ToString()
        {
            return $"Animation: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Animation: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Type: {Type}");
            Lines.AppendLine($"Frames: {NumberOfFrames}");
            Lines.AppendLine($"FrameRate: {FrameRate}");
            Lines.AppendLine($"Looping: {Looping}");

            return Lines.ToString();
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"{Name}.res";
            if (System.IO.File.Exists(outName)) return;
            GodotBinaryAnimation bin = new(this);
            bin.WriteToFile(outName);
        }
    }
}
