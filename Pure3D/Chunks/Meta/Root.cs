using System.IO;

namespace Pure3D.Chunks
{
    /// <summary>
    /// A dummy chunk we use to represent the Root of a file.
    /// </summary>
    public class Root : Chunk
    {
        public Root(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
        }

        public override void WriteHeader(BinaryWriter writer)
        {
        }

        public override string ToString()
        {
            return $"Root {File.FullName.Substring(File.FullName.Length - 24, 24)}";
        }
    }
}
