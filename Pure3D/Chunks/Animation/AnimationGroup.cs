using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x121001)]
    public class AnimationGroup : VersionNamed
    {
        public uint NumberOfChannels;
        public uint GroupId;

        public AnimationGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            GroupId = reader.ReadUInt32();
            NumberOfChannels = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(GroupId);
            writer.Write(NumberOfChannels);
        }

        public override string ToString()
        {
            return $"Group {GroupId}: {Name} - Chan: {NumberOfChannels} ";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Animation Group: {Name}, Version {Version}");
            Lines.AppendLine($"Channels {NumberOfChannels}");
            Lines.AppendLine($"Group {GroupId}");

            return Lines.ToString();
        }
    }
}
