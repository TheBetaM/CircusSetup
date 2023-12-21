using System.Collections.Generic;
using System.IO;
using System.Text;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x1001A)]
    public class ShadowMesh : Named
    {
        public uint Version;
        public uint Positions;
        public uint Topologies;

        public ShadowMesh(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Positions = reader.ReadUInt32();
            Topologies = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Shadow Mesh: {Name}";
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