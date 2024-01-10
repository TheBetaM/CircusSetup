using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using Pure3D.Chunks;
using System.Drawing;

namespace Pure3D
{
    public class P3DI
    {
        public int Type;
        public int Width;
        public int Height;
        public int Bpp;
        public List<ByteColour> Colors = new List<ByteColour>();
        public byte[] RawColors = new byte[0];
        public byte[] RawData = new byte[0];

        public void Read(BinaryReader sreader, long length)
        {
            BinaryReader reader = sreader;
            string check = new string(reader.ReadChars(4)); // P3DI / ID3P
            if (check == "ID3P")
            {
                BinaryReader2 bigEnd = new BinaryReader2(sreader.BaseStream);
                reader = bigEnd;
            }
            Type = reader.ReadInt32();
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Bpp = reader.ReadInt32();
            int PaletteSize = reader.ReadInt32();
            int ImageSize = reader.ReadInt32();
            List<ByteColour> palette = new List<ByteColour>();
            if (PaletteSize != 0)
            {
                for (int i = 0; i < PaletteSize / 4; i++)
                {
                    palette.Add(Util.ReadColour(reader));
                }
            }
            RawData = reader.ReadBytes(ImageSize);
            RawColors = new byte[Width * Height * 4];
            Colors = new List<ByteColour>();

            switch (Type)
            {
                default: break;
                case 0:
                    {
                        // PSP 4bit/8bit
                        if (Bpp == 4)
                        {
                            EzSwizzle ez = new EzSwizzle();
                            byte[] texData = ez.PSP_UnSwizzle(Width, Height, Bpp, RawData);
                            var indData = new List<byte>();
                            for (int i = 0; i < ImageSize && i < texData.Length; i++)
                            {
                                byte pack = texData[i];
                                byte ind1 = (byte)(pack & 0x0F);
                                byte ind2 = (byte)(pack >> 4);
                                Colors.Add(palette[ind1]);
                                Colors.Add(palette[ind2]);
                                indData.Add(ind1);
                                indData.Add(ind2);
                            }
                            var indArray = indData.ToArray();
                            Flip(ref indArray, Width, Height);
                            for (int i = 0; i < Width * Height; i++)
                            {
                                RawColors[(i * 4) + 0] = palette[indArray[i]].R;
                                RawColors[(i * 4) + 1] = palette[indArray[i]].G;
                                RawColors[(i * 4) + 2] = palette[indArray[i]].B;
                                RawColors[(i * 4) + 3] = palette[indArray[i]].A;
                            }
                        }
                        else if (Bpp == 8)
                        {
                            var ColorPal = palette.ToArray();
                            EzSwizzle ez = new EzSwizzle();
                            Colors.Add(palette[RawData[0]]);
                            byte[] texData = ez.PSP_UnSwizzle(Width, Height, Bpp, RawData);
                            Flip(ref texData, Width, Height);
                            for (var i = 0; i < Width * Height; ++i)
                            {
                                RawColors[(i * 4) + 0] = ColorPal[texData[i]].R;
                                RawColors[(i * 4) + 1] = ColorPal[texData[i]].G;
                                RawColors[(i * 4) + 2] = ColorPal[texData[i]].B;
                                RawColors[(i * 4) + 3] = ColorPal[texData[i]].A;
                            }
                        }
                        break;
                    }
                case 13:
                case 14:
                    {
                        // PS2 4bit/8bit
                        bool IsSwizzled = false;
                        if (Bpp == 8)
                        {
                            if ((Width == 256 || Width == 128) && Height >= 64) IsSwizzled = true;
                            if (Width == 512 && Height != 512 && Height >= 64) IsSwizzled = true;
                            if (Width == Height && Width >= 16) IsSwizzled = true;
                        }
                        else
                        {
                            if (Width == 64 && Height >= 32 && Height <= 128) IsSwizzled = true;
                            if (Width == 128 && Height >= 128) IsSwizzled = true;
                            if (Width == Height && Width >= 32) IsSwizzled = true;
                        }
                        if (Util.IsDemo)
                        {
                            IsSwizzled = false;
                        }
                        Colors.Add(palette[0]);
                        var ColorPal = palette.ToArray();
                        int dbw = Width / 128;
                        int textureBufferWidth = Width / 64;
                        int rrw = Width / 2;
                        int rrh = Height / 2;
                        EzSwizzle ez = new EzSwizzle();
                        byte[] imageData = RawData;
                        ez.cleanGs();
                        var texData = new byte[Width * Height];

                        if (Bpp == 4)
                        {
                            rrw = Width / 2;
                            rrh = Height / 4;

                            if (IsSwizzled)
                            {
                                ez.writeTexPSMCT32(0, dbw, 0, 0, rrw, rrh, imageData);
                                ez.readTexPSMT4_mod(0, textureBufferWidth, 0, 0, Width, Height, ref texData);
                            }
                            else
                            {
                                texData = imageData;
                            }

                            var indData = new List<byte>();
                            for (int i = 0; i < ImageSize; i++)
                            {
                                byte pack = texData[i];
                                byte ind1 = (byte)(pack & 0x0F);
                                byte ind2 = (byte)(pack >> 4);
                                indData.Add(ind1);
                                indData.Add(ind2);
                            }
                            var indArray = indData.ToArray();
                            Flip(ref indArray, Width, Height);
                            for (int i = 0; i < Width * Height; i++)
                            {
                                RawColors[(i * 4) + 0] = palette[indArray[i]].R;
                                RawColors[(i * 4) + 1] = palette[indArray[i]].G;
                                RawColors[(i * 4) + 2] = palette[indArray[i]].B;
                                RawColors[(i * 4) + 3] = (byte)Math.Clamp(ColorPal[indArray[i]].A * 2, 0, 255);
                            }
                        }
                        else if (Bpp == 8)
                        {
                            if (IsSwizzled)
                            {
                                ez.writeTexPSMCT32(0, dbw, 0, 0, rrw, rrh, imageData);
                                ez.readTexPSMT8(0, textureBufferWidth, 0, 0, Width, Height, ref texData);
                            }
                            else
                            {
                                texData = imageData;
                            }
                            SwapPalette(ref ColorPal);
                            Flip(ref texData, Width, Height);
                            for (var i = 0; i < Width * Height; ++i)
                            {
                                RawColors[(i * 4) + 0] = ColorPal[texData[i]].R;
                                RawColors[(i * 4) + 1] = ColorPal[texData[i]].G;
                                RawColors[(i * 4) + 2] = ColorPal[texData[i]].B;
                                RawColors[(i * 4) + 3] = (byte)Math.Clamp(ColorPal[texData[i]].A * 2, 0, 255);
                            }
                        }                      
                        break;
                    }
                case 18:
                    {
                        // Gamecube/Wii 4bit/8bit
                        break;
                    }
            }
            

        }

        public void Write(BinaryWriter writer) 
        {
            throw new System.NotImplementedException();
        }

        public static void SwapPalette(ref ByteColour[] palette)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 8 + i * 32; j < 16 + i * 32; j++)
                {
                    ByteColour tmp = palette[j];
                    palette[j] = palette[j + 8];
                    palette[j + 8] = tmp;
                }
            }
        }

        public static void Flip(ref byte[] Indexes, int width, int height)
        {
            for (uint y = 0; y < height / 2; y++)
            {
                for (uint x = 0; x < width; x++)
                {
                    byte tmp = Indexes[y * width + x];
                    Indexes[y * width + x] = Indexes[(height - y - 1) * width + x];
                    Indexes[(height - y - 1) * width + x] = tmp;
                }
            }
        }

        


    }

}
