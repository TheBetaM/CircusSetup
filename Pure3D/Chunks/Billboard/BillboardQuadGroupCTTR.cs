using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x17006)]
    public class BillboardQuadGroupCTTR : Named
    {
        public uint Version;
        public string MaterialName;
        public ulong MaterialName_padding;
        public uint CutOffEnabled;
        public uint ZTest;
        public uint ZWrite;
        public uint Fog;
        public uint QuadCount;
        public BillboardQuadGroupCTTR(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            MaterialName = Util.ReadString(reader, ref MaterialName_padding);
            CutOffEnabled = reader.ReadUInt32();
            ZTest = reader.ReadUInt32();
            ZWrite = reader.ReadUInt32();
            Fog = reader.ReadUInt32();
            QuadCount = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Billboard Quad Group: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Quad Group CTTR: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Shader: {MaterialName}");
            Lines.AppendLine($"CutOffEnabled: {CutOffEnabled}");
            Lines.AppendLine($"ZTest: {ZTest}");
            Lines.AppendLine($"ZWrite: {ZWrite}");
            Lines.AppendLine($"Fog: {Fog}");
            Lines.AppendLine($"QuadCount: {QuadCount}");

            return Lines.ToString();
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"{Name}.tscn";
            if (System.IO.File.Exists(outName)) return;

            GodotSceneFileCircus scene = GodotSceneFileCircus.Create(Name);
            scene.WriteToFile(outName);
        }
    }
}