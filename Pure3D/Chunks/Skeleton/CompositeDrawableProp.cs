using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x4516)]
    public class CompositeDrawableProp : Named
    {
        public uint IsTranslucent;
        public uint SkeletonJointID;

        public CompositeDrawableProp(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            ;
            base.ReadHeader(reader, length);
            IsTranslucent = reader.ReadUInt32();
            SkeletonJointID = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(IsTranslucent);
            writer.Write(SkeletonJointID);
        }

        public override string ToString()
        {
            return $"Composite Drawable Prop: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Composite Drawable Prop: {Name}");
            Lines.AppendLine($"IsTranslucent: {IsTranslucent}");
            Lines.AppendLine($"SkeletonJointID: {SkeletonJointID}");

            return Lines.ToString();
        }
    }
}
