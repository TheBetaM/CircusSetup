using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x2200)]
    public class Camera : Named
    {
        public uint Version;
        public float FieldOfView;
        public float AspectRatio;
        public float NearClip;
        public float FarClip;
        public Vector3 Position;
        public Vector3 Look;
        public Vector3 Up;

        public Camera(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            FieldOfView = reader.ReadSingle();
            AspectRatio = reader.ReadSingle();
            NearClip = reader.ReadSingle();
            FarClip = reader.ReadSingle();
            Position = Util.ReadVector3(reader);
            Look = Util.ReadVector3(reader);
            Up = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            writer.Write(FieldOfView);
            writer.Write(AspectRatio);
            writer.Write(NearClip);
            writer.Write(FarClip);
            Util.WriteVector3(writer, Position);
            Util.WriteVector3(writer, Look);
            Util.WriteVector3(writer, Up);
        }

        public override string ToString()
        {
            return $"Camera: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Camera: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"FieldOfView: {FieldOfView}");
            Lines.AppendLine($"AspectRatio: {AspectRatio}");
            Lines.AppendLine($"NearClip: {NearClip}");
            Lines.AppendLine($"FarClip: {FarClip}");
            Lines.AppendLine($"Position: {Position}");
            Lines.AppendLine($"Look: {Look}");
            Lines.AppendLine($"Up: {Up}");

            return Lines.ToString();
        }
    }
}
