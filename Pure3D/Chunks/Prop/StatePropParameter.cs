using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x8020008)]
    public class StatePropParameter : Named
    {
        public string Param;
        public ulong Param_padding;

        public StatePropParameter(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Param = Util.ReadString(reader, ref Param_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Param: {Name}: {Param}";
        }
    }
}