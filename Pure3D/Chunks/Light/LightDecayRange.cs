using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x13006)]
    public class LightDecayRange : Chunk
    {
        public uint UnkInt;
        public Vector3 Vec1 = new Vector3();
        public Vector3 Vec2 = new Vector3();

        public LightDecayRange(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt = reader.ReadUInt32();
            Vec1 = Util.ReadVector3(reader);
            Vec2 = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(UnkInt);
            Util.WriteVector3(writer, Vec1);
            Util.WriteVector3(writer, Vec2);
        }

        public override string ToString()
        {
            return $"Light Decay Range";
        }
    }
}