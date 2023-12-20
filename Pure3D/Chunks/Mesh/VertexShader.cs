using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10011)]
    public class VertexShader : Chunk
    {
        public string VertexShaderName;
        public ulong VertexShaderName_padding;

        public VertexShader(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            VertexShaderName = Util.ReadString(reader, ref VertexShaderName_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteString(writer, VertexShaderName, VertexShaderName_padding);
        }

        public override string ToString()
        {
            return $"Vertex Shader: {VertexShaderName}";
        }
    }
}
