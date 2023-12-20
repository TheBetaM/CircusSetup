using System.Collections.Generic;
using System.IO;
using System.Text;
using CircusSetup;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x10001)]
    public class Skin : Named
    {
        public uint Version;
        public uint NumPrimGroups;
        public string SkeletonName;
        public ulong SkeletonName_padding;

        public Skin(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            SkeletonName = Util.ReadString(reader, ref SkeletonName_padding);
            NumPrimGroups = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            Util.WriteString(writer, SkeletonName, SkeletonName_padding);
            writer.Write(NumPrimGroups);
        }

        public override string ToString()
        {
            return $"Skin: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Skin: {Name}");
            Lines.AppendLine($"Version {Version}");
            Lines.AppendLine($"SkeletonName {SkeletonName}");
            Lines.AppendLine($"NumPrimGroups {NumPrimGroups}");

            return Lines.ToString();
        }

        public override void OnExport(string path)
        {
            ExportGLTF.Export(this, path);
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"{Name}.tscn";
            if (System.IO.File.Exists(outName)) return;
            GodotBinaryArrayMesh bin = new(this);
            bin.WriteToFile(pathDir + $"{Name}.res");

            GodotSceneFileCircus scene = GodotSceneFileCircus.Create(Name);
            GodotFileBase.ExternalResource ModelFileReference = new($"{Name}.res");
            ModelFileReference.SetAsArrayMesh();
            scene.ExternalResourceList.Add(ModelFileReference);
            scene.Nodes[0].Type = "MeshInstance3D";
            scene.Nodes[0].Lines.Add($"mesh=ExtResource(1)");
            int ShaderID = 2;
            int MeshID = 0;

            Dictionary<string, (int, Shader)> shaders = new();
            foreach (var item in Children)
            {
                if (item is PrimitiveGroupCTTR prim)
                {
                    if (!string.IsNullOrEmpty(prim.ShaderName))
                    {
                        if (!shaders.ContainsKey(prim.ShaderName))
                        {
                            var shader = File.RootChunk.GetChildByName<Shader>(prim.ShaderName);
                            if (!string.IsNullOrEmpty(shader.Name))
                            {
                                shaders.Add(prim.ShaderName, (ShaderID, shader));
                                scene.Nodes[0].Lines.Add($"{ExportGodot.materialOverride}/{MeshID}=ExtResource({ShaderID})");
                                ShaderID++;
                                GodotFileBase.ExternalResource MatFile = new($"{prim.ShaderName}.tres");
                                MatFile.Type = "ShaderMaterial";
                                scene.ExternalResourceList.Add(MatFile);
                            }
                        }
                        else
                        {
                            scene.Nodes[0].Lines.Add($"{ExportGodot.materialOverride}/{MeshID}=ExtResource({shaders[prim.ShaderName].Item1})");
                        }
                    }
                    MeshID++;
                }
            }
            foreach (var pair in shaders)
            {
                pair.Value.Item2.OnGodotExport(path);
            }

            if (!string.IsNullOrEmpty(SkeletonName))
            {
                var rig = File.RootChunk.GetChildByName<SkeletonCTTR>(SkeletonName);
                if (!string.IsNullOrEmpty(rig.Name))
                {
                    rig.OnGodotExport(path);
                }
            }

            scene.WriteToFile(outName);
        }
    }
}
