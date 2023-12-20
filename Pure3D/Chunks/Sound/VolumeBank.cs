using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000002)]
    public class VolumeBank : Chunk
    {
        public string Name;
        public uint UnkInt;
        public uint MainVol1;
        public uint MainVol2;
        public List<string> VolNames = new List<string>();
        public List<float> Vol1 = new List<float>();
        public List<float> Vol2 = new List<float>();
        public List<float> Vol3 = new List<float>();
        public List<float> Vol4 = new List<float>();
        public List<byte> UnkBytes = new List<byte>();

        public VolumeBank(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            UnkInt = reader.ReadUInt32();
            MainVol1 = reader.ReadUInt32();
            MainVol2 = reader.ReadUInt32();
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                NameLength = reader.ReadInt32();
                VolNames.Add(new string(reader.ReadChars(NameLength)));
                reader.ReadByte();
                Vol1.Add(reader.ReadSingle());
                Vol2.Add(reader.ReadSingle());
                Vol3.Add(reader.ReadSingle());
                Vol4.Add(reader.ReadSingle());
                UnkBytes.Add(reader.ReadByte());
                // todo: 360 has an extra byte per item here
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Volume Bank {Name}";
        }
    }
}
