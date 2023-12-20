using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7011000)]
    public class PhysicsObject : Named
    {
        public uint Version;
        public string MaterialName;
        public ulong MaterialName_padding;
        public uint NumJoints;
        public float Volume;
        public float RestingSensitivity;

        public PhysicsObject(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            MaterialName = Util.ReadString(reader, ref MaterialName_padding);
            NumJoints = reader.ReadUInt32();
            Volume = reader.ReadSingle();
            RestingSensitivity = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            Util.WriteString(writer, MaterialName, MaterialName_padding);
            writer.Write(NumJoints);
            writer.Write(Volume);
            writer.Write(RestingSensitivity);
        }

        public override string ToString()
        {
            return $"Physics Object: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Physics Object: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"MaterialName: {MaterialName}");
            Lines.AppendLine($"NumJoints: {NumJoints}");
            Lines.AppendLine($"Volume: {Volume}");
            Lines.AppendLine($"RestingSensitivity: {RestingSensitivity}");

            return Lines.ToString();
        }
    }
}
