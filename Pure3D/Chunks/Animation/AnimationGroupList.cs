using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121002)]
    public class AnimationGroupList : Chunk
    {
        public uint Version;
        public uint NumberOfGroups;

        public AnimationGroupList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            NumberOfGroups = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(NumberOfGroups);
        }

        public override string ToString()
        {
            return $"Animation Group List: {NumberOfGroups}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Animation Group List");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Number of groups: {NumberOfGroups}");

            return Lines.ToString();
        }
    }
}
