using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5500003)]
    public class LevelDataParam : Named
    {
        public uint UnkInt1;
        public Vector3 Pos1 = new Vector3(); // pos?
        public Vector3 Pos2 = new Vector3(); // rot? (in MoM it's a second position instead, maybe player 2 related?)
        public Vector3 Pos3 = new Vector3(1f, 1f, 1f); // scale?

        // MoM
        public uint ItemCount;
        public string StartingRoomName;
        public ulong StartingRoomName_padding;
        public float UnkFloat1; // y rotation of the room?
        public float UnkFloat2; // y rotation of the room?

        public LevelDataParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            if (UnkInt1 != 1)
            {
                // MoM
                ItemCount = reader.ReadUInt32();
                base.ReadHeader(reader, length);
                StartingRoomName = Util.ReadString(reader, ref StartingRoomName_padding);
                Pos1 = Util.ReadVector3(reader);
                UnkFloat1 = reader.ReadSingle();
                Pos2 = Util.ReadVector3(reader);
                UnkFloat2 = reader.ReadSingle();
            }
            else
            {
                base.ReadHeader(reader, length);
                Pos1 = Util.ReadVector3(reader);
                Pos2 = Util.ReadVector3(reader);
                Pos3 = Util.ReadVector3(reader);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Param: {Name} / {StartingRoomName}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Level Data Param: {Name}");
            Lines.AppendLine($"Starting Room Name: {StartingRoomName}");
            Lines.AppendLine($"Unk Floats: {UnkFloat1}/{UnkFloat2}");
            Lines.AppendLine($"Vectors:");
            Lines.AppendLine($"{Pos1}");
            Lines.AppendLine($"{Pos2}");
            Lines.AppendLine($"{Pos3}");

            return Lines.ToString();
        }
    }
}
