using System.Collections.Generic;
using System.IO;
using System.Text;
using CircusSetup;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x11000)]
    public class Shader : Named
    {
        public uint Version;
        public string PddiShaderName;
        public ulong PddiShaderName_padding;
        public uint HasTranslucency;
        public uint VertexNeeds;
        public uint VertexMask;
        protected uint NumParams; // Should match the number of children

        public Shader(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);

            Version = reader.ReadUInt32();
            PddiShaderName = Util.ReadString(reader, ref PddiShaderName_padding);
            HasTranslucency = reader.ReadUInt32();
            VertexNeeds = reader.ReadUInt32();
            VertexMask = reader.ReadUInt32();
            NumParams = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);

            writer.Write(Version);
            Util.WriteString(writer, PddiShaderName, PddiShaderName_padding);
            writer.Write(HasTranslucency);
            writer.Write(VertexNeeds);
            writer.Write(VertexMask);
            writer.Write(NumParams);
        }

        public override string ToString()
        {
            return $"Shader: {Name} ({PddiShaderName})";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Shader: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"PddiShaderName: {PddiShaderName}");
            Lines.AppendLine($"HasTranslucency: {HasTranslucency}");
            Lines.AppendLine($"VertexNeeds: {VertexNeeds}");
            Lines.AppendLine($"VertexMask: {VertexMask}");
            Lines.AppendLine($"NumParams: {NumParams}");

            return Lines.ToString();
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"{Name}.tres";
            if (System.IO.File.Exists(outName)) return;
            GodotResourceFileCircus MaterialFile = new();
            MaterialFile.Resource.Type = "ShaderMaterial";
            int ExtraShaderID = -1;
            string ExtraShaderType = "";
            string shaderAdd = "shader_parameter/";
            var TargetResource = MaterialFile.Resource;
            string shaderName = "TexScroll";

            foreach (var item in Children)
            {
                if (item is ShaderIntParam intp)
                {
                    if (intp.Param == "ATST" && intp.Value != 0)
                    {
                        shaderName = "TexScrollAlphaTest";
                    }
                    else if (intp.Param == "BLMD" && intp.Value != 0)
                    {
                        if (intp.Value == 1)
                        {
                            shaderName = "TexScrollAlphaBlendMix";
                        }
                        else if (intp.Value == 2)
                        {
                            shaderName = "TexScrollAlphaBlendAdd";
                        }
                        else
                        {
                            shaderName = "TexScrollAlphaBlendSub";
                        }
                    }
                    else if (intp.Param == "2SID" && intp.Value != 0)
                    {
                        //matbuild.WithDoubleSide(true);
                    }
                    else if (intp.Param == "LIT" && intp.Value == 0)
                    {
                        shaderName = "Unlit" + shaderName;
                    }
                }
            }

            if (ExtraShaderID == -1 || ExtraShaderType != shaderName)
            {
                //ExternalResource ShResource = new ExternalResource($"../Shaders/{shaderName}.gdshader");
                GodotFileBase.ExternalResource ShResource = new($"res://shaders/{shaderName}.gdshader");
                ShResource.SetAsShader();
                MaterialFile.ExternalResourceList.Add(ShResource);
                TargetResource.Lines.Add($"shader=ExtResource({MaterialFile.ExternalResourceList.Count})");
                ExtraShaderID = MaterialFile.ExternalResourceList.Count;
                ExtraShaderType = shaderName;
            }
            else
            {
                TargetResource.Lines.Add($"shader=ExtResource({ExtraShaderID})");
            }

            foreach (var item in Children)
            {
                if (item is ShaderTextureParam texp)
                {
                    if (string.IsNullOrEmpty(texp.Value))
                        continue;
                    
                    Pure3D.Chunks.Texture tex = File.RootChunk.GetChildByName<Pure3D.Chunks.Texture>(texp.Value);
                    if (tex != null)
                    {
                        tex.OnGodotExport(path);
                    }

                    if (texp.Param == "TEX")
                    {
                        GodotFileBase.ExternalResource TexResource = new($"{texp.Value.Split('.')[0]}.res");
                        TexResource.SetAsTexture();
                        MaterialFile.ExternalResourceList.Add(TexResource);
                        TargetResource.Lines.Add($"{shaderAdd}albedo_texture=ExtResource({MaterialFile.ExternalResourceList.Count})");
                    }
                    else if (texp.Param == "REFL")
                    {
                        // todo env map
                        //matbuild.WithChannelImage(KnownChannel.Transmission, new MemoryImage(Colors));
                    }
                }
                else if (item is ShaderColourParam colp)
                {
                    if (colp.Param == "DIFF")
                    {
                        // todo colors
                        //matbuild.WithBaseColor(new Vector4(colp.Color.R / 255f, colp.Color.G / 255f, colp.Color.B / 255f, colp.Color.A / 255f));
                    }
                }
            }

            MaterialFile.WriteToFile(outName);
        }
    }
}
