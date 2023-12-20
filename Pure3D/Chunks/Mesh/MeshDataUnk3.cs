using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10022)]
    public class MeshDataUnk3 : Unknown
    {
        public MeshDataUnk3(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"MeshDataUnk3";
        }
    }

    [ChunkType(0x10029)]
    public class MeshDataUnk4 : Unknown
    {
        public MeshDataUnk4(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"MeshDataUnk4";
        }
    }

    [ChunkType(0x1002B)]
    public class MeshDataUnk5 : Unknown
    {
        public MeshDataUnk5(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"MeshDataUnk5";
        }
    }

    [ChunkType(0x1002C)]
    public class MeshDataUnk6 : Unknown
    {
        public MeshDataUnk6(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"MeshDataUnk6";
        }
    }

    [ChunkType(0x1002D)]
    public class MeshDataUnk7 : Unknown
    {
        public MeshDataUnk7(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"MeshDataUnk7";
        }
    }
}