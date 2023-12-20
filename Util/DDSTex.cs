using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using Dxt;
using System.Drawing;

namespace Pure3D
{
    public class DDSTex
    {
        public int Width;
        public int Height;
        public int Bpp;
        public List<ByteColour> Colors = new List<ByteColour>();
        public byte[] RawColors = new byte[0];
        public Color[] RawData;

        public void Read(BinaryReader reader, long length)
        {
            uint Magic = reader.ReadUInt32();
            if (Magic != 0x20534444){ // "DDS "
                throw new Exception("DDS invalid!");
            }
            reader.ReadUInt32(); // 0x7C
            uint flags = reader.ReadUInt32();
            Height = (int)reader.ReadUInt32();
            Width = (int)reader.ReadUInt32();
            reader.ReadUInt32();
            uint depth = reader.ReadUInt32();
            uint mipmapCount = reader.ReadUInt32();
            reader.ReadBytes(0x2C);
            uint dwSize = reader.ReadUInt32();
            uint dwFlags = reader.ReadUInt32();
            uint imageFormat = reader.ReadUInt32();
            reader.ReadBytes(0x28);
            byte[] data = reader.ReadBytes((int)(length - reader.BaseStream.Position));

            RawColors = new byte[Width * Height * 4];
            RawData = new Color[Width * Height];
            Colors = new List<ByteColour>() { new ByteColour() };

            switch (imageFormat)
            {
                case 0x31545844: // DXT1
                    DxtDecoder.DecompressDXT1(data, Width, Height, RawColors);
                    break;
                case 0x33545844: // DXT3
                    DxtDecoder.DecompressDXT3(data, Width, Height, RawColors);
                    break;
                case 0x35545844: // DXT5
                    DxtDecoder.DecompressDXT5(data, Width, Height, RawColors);
                    break;
                default:
                    throw new Exception("DDS unsupported!");
                    break;
            }

            int b = 0;
            int c = 0;
            for (int y = Height - 1; y >= 0; y--)
            {
                c = (Width * y);
                for (int x = 0; x < Width; x++)
                {
                    RawData[c + x] = Color.FromArgb(RawColors[b + 3], RawColors[b + 2], RawColors[b + 1], RawColors[b + 0]);
                    b += 4;
                }
            }

        }

        public void Write(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

    }
}
