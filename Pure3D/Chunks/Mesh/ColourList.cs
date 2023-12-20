using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10008)]
    public class ColourList : Chunk
    {
        public ByteColour[] Colours;

        public ColourList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Colours = new ByteColour[len];
            for (int i = 0; i < len; i++)
                Colours[i] = Util.ReadColourBGR(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Colours.Length);
            for (int i = 0; i < Colours.Length; i++)
            {
                Util.WriteColourBGR(writer, Colours[i]);
                //writer.Write(Colours[i]);
            }
        }

        public override string ToString()
        {
            return $"Colour List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Colour List: {Colours.Length} colours");
            for (int i = 0; i < Colours.Length; i++)
            {
                Lines.AppendLine($"Colour{i}: {Colours[i]}");
            }

            return Lines.ToString();
        }
    }
}
