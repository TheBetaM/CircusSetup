using System;
using System.IO;

namespace Pure3D
{
    public class File
    {
        public readonly Chunks.Root RootChunk;
        public string FullName;

        public File()
        {
            RootChunk = new Chunks.Root(this, 0);
        }

        public void Load(string path)
        {
            FullName = path;

            using (var br = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan))
            {
                byte[] buffer = new byte[br.Length];
                br.Read(buffer, 0, buffer.Length);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (BinaryReader reader = new BinaryReader(memoryStream))
                    {
                        Load(reader);
                    }
                }
            }

            //using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    Load(fileStream);
        }

        public void Load(BinaryReader reader)
        {
            FileTypes fileType = (FileTypes)reader.ReadUInt32();

            if (fileType == FileTypes.RZ)
            {
                throw new Exception("RZ Pure3D not supported.");
            }
            if (fileType == FileTypes.CompressedPure3D)
            {
                throw new Exception("Compressed Pure3D not supported.");
            }
            if (fileType == FileTypes.Pure3DBE)
            {
                BinaryReader2 bigEnd = new BinaryReader2(reader.BaseStream);
                reader = bigEnd;
            }

            RootChunk.Read(reader, true, reader.BaseStream.Length);
        }

        public void Save(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    Save(writer);
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write((uint)FileTypes.Pure3D);

            RootChunk.Write(writer, true, 0);
        }
    }

    public enum FileTypes : uint
    {
        RZ = 0x5A52, // 'RZ' zlib deflate
        CompressedPure3D = 0x5A443350, // 'P3DZ' proprietary compression
        Pure3D = 0xFF443350, // 'P3D' normal
        Pure3DBE = 0x503344FF, // 'P3D' big endian
    }
}
