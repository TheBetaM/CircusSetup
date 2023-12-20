using System.IO;

namespace Pure3D.Chunks
{
    /// <summary>
    /// Base class for any chunk that has a string of it's name attached.
    /// Useful for searching by name.
    /// </summary>
    public class Named : Chunk
    {
        public string Name;
        public ulong Name_padding;

        public Named(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Name = Util.ReadString(reader, ref Name_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteString(writer, Name, Name_padding);
        }

        public override string ToString()
        {
            return $"Named Chunk: {Name}";
        }
    }
}
