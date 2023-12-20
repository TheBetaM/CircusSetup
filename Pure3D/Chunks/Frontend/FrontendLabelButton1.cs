using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    // it's empty?
    [ChunkType(0x18013)]
    public class FrontendLabelButton1 : Chunk
    {
        public FrontendLabelButton1(File file, uint type) : base(file, type)
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
            return $"FE Label Button";
        }
    }

    [ChunkType(0x18014)]
    public class FrontendLabelButton2 : Chunk
    {
        public FrontendLabelButton2(File file, uint type) : base(file, type)
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
            return $"FE Label Button 2";
        }
    }

    [ChunkType(0x18015)]
    public class FrontendSpriteButton1 : Chunk
    {
        public FrontendSpriteButton1(File file, uint type) : base(file, type)
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
            return $"FE Sprite Button 1";
        }
    }

    [ChunkType(0x18016)]
    public class FrontendSpriteButton2 : Chunk
    {
        public FrontendSpriteButton2(File file, uint type) : base(file, type)
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
            return $"FE Sprite Button 2";
        }
    }
}