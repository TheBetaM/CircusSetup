using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using BigGustave;
using System.Linq;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x19001)]
    public class Image : Named
    {
        public uint Version;
        public uint Width;
        public uint Height;
        public uint Bpp;
        public uint Palettized;
        public uint HasAlpha;
        public uint Format;

        public Image(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            Bpp = reader.ReadUInt32();
            Palettized = reader.ReadUInt32();
            HasAlpha = reader.ReadUInt32();
            Format = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Bpp);
            writer.Write(Palettized);
            writer.Write(HasAlpha);
            writer.Write(Format);
        }

        public override string ToString()
        {
            return $"Image: {Name} ({(Formats)Format}) ({Width}x{Height} {Bpp}bpp)";
        }

        public enum Formats : uint
        {
            RAW = 0,
            PNG = 1,
            TGA = 2,
            BMP = 3,
            IPU = 4,
            DXT = 5,
            DXT1 = 6,
            DXT2 = 7,
            DXT3 = 8,
            DXT4 = 9,
            DXT5 = 10,
            PS2_4bit = 11,
            PS2_8bit = 12,
            PS2_16bit = 13,
            PS2_32bit = 14,
            GameCube_4bit = 15,
            GameCube_8bit = 16,
            GameCube_16bit = 17,
            GameCube_32bit = 18,
            GameCube_DXT1 = 19,
            PSP = 25,
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Image: {Name}");
            Lines.AppendLine($"Format: {(Formats)Format}");
            Lines.AppendLine($"Width: {Width}");
            Lines.AppendLine($"Height: {Height}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Bpp: {Bpp}");
            Lines.AppendLine($"Palettized: {Palettized}");
            Lines.AppendLine($"HasAlpha: {HasAlpha}");

            switch ((Formats)Format)
            {
                default: break;
                case Formats.PS2_4bit:
                case Formats.PS2_8bit:
                case Formats.PS2_16bit:
                case Formats.PS2_32bit:
                case Formats.PSP:
                case Formats.GameCube_4bit:
                case Formats.GameCube_8bit:
                case Formats.GameCube_16bit:
                case Formats.GameCube_32bit:
                case Formats.GameCube_DXT1:
                    {
                        ImageData datachunk = (ImageData)Children[0];
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                P3DI psp = new P3DI();
                                psp.Read(reader, datachunk.Data.Length);
                                Lines.AppendLine($"P3DI Format: {psp.Type}");
                            }
                        }
                        break;
                    }
            }

            return Lines.ToString();
        }

        public override void OnExport(string inpath)
        {
            string parentName = ((Named)Parent).Name;
            string path = System.IO.Path.GetDirectoryName(inpath) + "\\" + parentName.Split('\\').Last().Split('.')[0] + ".png";
            ImageData datachunk = (ImageData)Children[0];
            byte[] data = datachunk.Data;
            
            switch ((Formats)Format)
            {
                default:break;
                case Formats.PNG:
                    {
                        System.IO.File.WriteAllBytes(path, data);
                    }
                    break;
                case Formats.DXT:
                case Formats.DXT1:
                case Formats.DXT2:
                case Formats.DXT3:
                case Formats.DXT4:
                case Formats.DXT5:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                DDSTex tex = new DDSTex();
                                tex.Read(reader, datachunk.Data.Length);
                                if (tex.Colors.Count == 0) return;
                                var png = PngBuilder.Create((int)Width, (int)Height, true);
                                int i = 0;
                                for (int y = (int)Height - 1; y > 0; y--)
                                {
                                    for (int x = 0; x < Width; x++)
                                    {
                                        var B = tex.RawColors[i];
                                        i++;
                                        var G = tex.RawColors[i];
                                        i++;
                                        var R = tex.RawColors[i];
                                        i++;
                                        var A = tex.RawColors[i];
                                        i++;
                                        png.SetPixel(new BigGustave.Pixel(R, G, B, A, false), x, y);                                        
                                    }
                                }
                                System.IO.File.WriteAllBytes(path, png.Save());
                            }
                        }
                    }
                    break;
                case Formats.GameCube_4bit:
                case Formats.GameCube_8bit:
                case Formats.GameCube_16bit:
                case Formats.GameCube_32bit:
                case Formats.GameCube_DXT1:
                case Formats.PS2_4bit:
                case Formats.PS2_8bit:
                case Formats.PS2_16bit:
                case Formats.PS2_32bit:
                case Formats.PSP:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                P3DI psp = new P3DI();
                                psp.Read(reader, datachunk.Data.Length);
                                if (psp.Colors.Count == 0) return;
                                var png = PngBuilder.Create((int)Width, (int)Height, true);
                                int i = 0;
                                for (int y = 0; y < Height; y++)
                                {
                                    for (int x = 0; x < Width; x++)
                                    {
                                        var R = psp.RawColors[i];
                                        i++;
                                        var G = psp.RawColors[i];
                                        i++;
                                        var B = psp.RawColors[i];
                                        i++;
                                        var A = psp.RawColors[i];
                                        i++;
                                        png.SetPixel(new BigGustave.Pixel(R, G, B, A, false), x, y);                                        
                                    }
                                }
                                System.IO.File.WriteAllBytes(path, png.Save());
                            }
                        }
                        break;
                    }
                case Formats.TGA:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                TGATex psp = new TGATex();
                                psp.Read(reader, datachunk.Data.Length);
                                if (psp.Colors.Count == 0) return;
                                var png = PngBuilder.Create((int)Width, (int)Height, true);
                                int i = 0;
                                for (int y = 0; y < Height; y++)
                                {
                                    for (int x = 0; x < Width; x++)
                                    {
                                        var p = psp.Colors[i];
                                        png.SetPixel(new BigGustave.Pixel(p.B, p.G, p.R, p.A, false), x, y);
                                        i++;
                                    }
                                }
                                System.IO.File.WriteAllBytes(path, png.Save());
                            }
                        }
                        break;
                    }
            }
            
            
        }

        public override byte[] OnImagePreview()
        {
            ImageData datachunk = (ImageData)Children[0];

            
            switch ((Formats)Format)
            {
                default: return null;
                case Formats.PNG:
                    return datachunk.Data;
                case Formats.DXT:
                case Formats.DXT1:
                case Formats.DXT2:
                case Formats.DXT3:
                case Formats.DXT4:
                case Formats.DXT5:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                DDSTex tex = new DDSTex();
                                tex.Read(reader, datachunk.Data.Length);
                                if (tex.Colors.Count == 0) return null;
                                var png = PngBuilder.Create((int)Width, (int)Height, true);
                                int i = 0;
                                for (int y = (int)Height - 1; y > 0; y--)
                                {
                                    for (int x = 0; x < Width; x++)
                                    {
                                        var B = tex.RawColors[i];
                                        i++;
                                        var G = tex.RawColors[i];
                                        i++;
                                        var R = tex.RawColors[i];
                                        i++;
                                        var A = tex.RawColors[i];
                                        i++;
                                        png.SetPixel(new BigGustave.Pixel(R, G, B, A, false), x, y);                                        
                                    }
                                }
                                return png.Save();
                            }
                        }
                        return null;
                    }
                case Formats.GameCube_4bit:
                case Formats.GameCube_8bit:
                case Formats.GameCube_16bit:
                case Formats.GameCube_32bit:
                case Formats.GameCube_DXT1:
                case Formats.PS2_4bit:
                case Formats.PS2_8bit:
                case Formats.PS2_16bit:
                case Formats.PS2_32bit:
                case Formats.PSP:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                P3DI psp = new P3DI();
                                psp.Read(reader, datachunk.Data.Length);
                                if (psp.Colors.Count == 0) return null;
                                var png = PngBuilder.Create((int)Width, (int)Height, true);
                                int i = 0;
                                for (int y = 0; y < Height; y++)
                                {
                                    for (int x = 0; x < Width; x++)
                                    {
                                        var R = psp.RawColors[i];
                                        i++;
                                        var G = psp.RawColors[i];
                                        i++;
                                        var B = psp.RawColors[i];
                                        i++;
                                        var A = psp.RawColors[i];
                                        i++;
                                        png.SetPixel(new BigGustave.Pixel(R, G, B, A, false), x, y);                                        
                                    }
                                }
                                return png.Save();
                            }
                        }
                        return null;
                    }
                case Formats.TGA:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                TGATex psp = new TGATex();
                                psp.Read(reader, datachunk.Data.Length);
                                if (psp.Colors.Count == 0) return null;
                                var png = PngBuilder.Create((int)Width, (int)Height, true);
                                int i = 0;
                                for (int y = 0; y < Height; y++)
                                {
                                    for (int x = 0; x < Width; x++)
                                    {
                                        var p = psp.Colors[i];
                                        png.SetPixel(new BigGustave.Pixel(p.B, p.G, p.R, p.A, false), x, y);
                                        i++;
                                    }
                                }
                                return png.Save();
                            }
                        }
                        return null;
                    }
            }
            

            
        }

        public override void OnGodotExport(string inpath)
        {
            string parentName = ((Named)Parent).Name;
            string path = System.IO.Path.GetDirectoryName(inpath) + "\\" + parentName.Split('\\').Last().Split('.')[0] + ".res";
            if (System.IO.File.Exists(path)) return;
            ImageData datachunk = (ImageData)Children[0];
            byte[] data = datachunk.Data;
            List<Color> tex = new List<Color>();

            switch ((Formats)Format)
            {
                default:break;
                case Formats.PNG:
                    {
                        var image = Png.Open(data);
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                var p = image.GetPixel(x, y);
                                tex.Add(Color.FromArgb(p.A, p.R, p.G, p.B));
                            }
                        }
                    }
                    break;
                case Formats.DXT:
                case Formats.DXT1:
                case Formats.DXT2:
                case Formats.DXT3:
                case Formats.DXT4:
                case Formats.DXT5:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                DDSTex psp = new DDSTex();
                                psp.Read(reader, datachunk.Data.Length);
                                tex = new List<Color>(psp.RawData);
                            }
                        }
                    }
                    break;
                case Formats.GameCube_4bit:
                case Formats.GameCube_8bit:
                case Formats.GameCube_16bit:
                case Formats.GameCube_32bit:
                case Formats.GameCube_DXT1:
                case Formats.PS2_4bit:
                case Formats.PS2_8bit:
                case Formats.PS2_16bit:
                case Formats.PS2_32bit:
                case Formats.PSP:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                P3DI psp = new P3DI();
                                psp.Read(reader, datachunk.Data.Length);
                                if (psp.Colors.Count == 0) return;
                                int i = 0;
                                for (int y = 0; y < Height; y++)
                                {
                                    for (int x = 0; x < Width; x++)
                                    {
                                        var R = psp.RawColors[i];
                                        i++;
                                        var G = psp.RawColors[i];
                                        i++;
                                        var B = psp.RawColors[i];
                                        i++;
                                        var A = psp.RawColors[i];
                                        i++;
                                        tex.Add(Color.FromArgb(A, R, G, B));                             
                                    }
                                }
                            }
                        }
                        break;
                    }
                case Formats.TGA:
                    {
                        using (MemoryStream ms = new MemoryStream(datachunk.Data))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                TGATex psp = new TGATex();
                                psp.Read(reader, datachunk.Data.Length);
                                if (psp.Colors.Count == 0) return;
                                foreach (var color in psp.Colors)
                                {
                                    tex.Add(Color.FromArgb(color.A, color.R, color.G, color.B));
                                }
                            }
                        }
                        break;
                    }
            }

            GodotBinaryImageTexture res = new(tex, (int)Width, (int)Height);
            res.WriteToFile(path);
        }
    }
}
