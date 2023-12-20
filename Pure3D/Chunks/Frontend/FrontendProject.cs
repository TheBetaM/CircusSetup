using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x18000)]
    public class FrontendProject : Named
    {
        public uint Version;
        public uint Width;
        public uint Height;
        public string Platform;
        public ulong Platform_padding;
        public string PagePath;
        public ulong PagePath_padding;
        public string ResourcePath;
        public ulong ResourcePath_padding;
        public string ScreenPath;
        public ulong ScreenPath_padding;

        public FrontendProject(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            Platform = Util.ReadString(reader, ref Platform_padding);
            PagePath = Util.ReadString(reader, ref PagePath_padding);
            ResourcePath = Util.ReadString(reader, ref ResourcePath_padding);
            ScreenPath = Util.ReadString(reader, ref ScreenPath_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Project: {Name} ({Width}x{Height})";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Project {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Width: {Width}");
            Lines.AppendLine($"Height: {Height}");
            Lines.AppendLine($"Platform: {Platform}");
            Lines.AppendLine($"PagePath: {PagePath}");
            Lines.AppendLine($"ResourcePath: {ResourcePath}");
            Lines.AppendLine($"ScreenPath: {ScreenPath}");

            return Lines.ToString();
        }
    }
}
