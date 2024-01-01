using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace Pure3D
{
    public class TGATex
    {
        public int Width;
        public int Height;
        public int Bpp;
        public List<ByteColour> Colors = new List<ByteColour>();
        public byte[] RawColors = new byte[0];
        public byte[] RawData = new byte[0];

        public void Read(BinaryReader reader, long length)
        {
            byte idsize = reader.ReadByte();
            byte palettetype = reader.ReadByte();
            byte imageFormat = reader.ReadByte();
            short PaletteStart = reader.ReadInt16();
            short PaletteSize = reader.ReadInt16();
            byte PaletteFormat = reader.ReadByte();
            short offsetx = reader.ReadInt16();
            short offsety = reader.ReadInt16();
            Width = reader.ReadInt16();
            Height = reader.ReadInt16();
            Bpp = reader.ReadByte();
            byte unkByte = reader.ReadByte();
            reader.ReadBytes(idsize);

            RawColors = new byte[Width * Height * 4];
            Colors = new List<ByteColour>();

            if (imageFormat == 1)
            {
                // uncompressed color map
                List<ByteColour> palette = new List<ByteColour>();
                if (PaletteFormat == 24)
                {
                    for (int i = 0; i < PaletteSize; i++)
                    {
                        palette.Add(Util.ReadColour24(reader));
                    }
                }
                else
                {
                    // 32-bit
                    for (int i = 0; i < PaletteSize; i++)
                    {
                        palette.Add(Util.ReadColour(reader));
                    }
                }

                if (Bpp == 4)
                {
                    for (int i = 0; i < (Width * Height) / 2; i++)
                    {
                        // 4 bit indexing
                        byte pack = reader.ReadByte();
                        byte ind1 = (byte)(pack & 0x0F);
                        byte ind2 = (byte)(pack >> 4);
                        Colors.Add(palette[ind1]);
                        Colors.Add(palette[ind2]);
                    }
                }
                else if (Bpp == 8)
                {
                    for (int i = 0; i < Width * Height; i++)
                    {
                        // 8 bit indexing
                        Colors.Add(palette[reader.ReadByte()]);
                    }
                }

                for (int i = 0; i < Colors.Count; i++)
                {
                    RawColors[(i * 4) + 0] = Colors[i].B;
                    RawColors[(i * 4) + 1] = Colors[i].G;
                    RawColors[(i * 4) + 2] = Colors[i].R;
                    RawColors[(i * 4) + 3] = Colors[i].A;
                }
            }
            else if (imageFormat == 2)
            {
                // uncompressed true color
                for (int i = 0; i < Width * Height; i++)
                {
                    Colors.Add(Util.ReadColour(reader));
                }

                for (int i = 0; i < Colors.Count; i++)
                {
                    RawColors[(i * 4) + 0] = Colors[i].B;
                    RawColors[(i * 4) + 1] = Colors[i].G;
                    RawColors[(i * 4) + 2] = Colors[i].R;
                    RawColors[(i * 4) + 3] = Colors[i].A;
                }
            }

        }

        public void Write(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

    }
}
