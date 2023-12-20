using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;
using System;
using BigGustave;

namespace Pure3D.Chunks
{
    // Raw Texture or some kind of palette?
    [ChunkType(0x19006)]
    public class RawColorTexture : Named
    {

        public RawColorTexture(File file, uint type) : base(file, type)
        {
        }

        public override string ToString()
        {
            RawColorTextureData datachunk = (RawColorTextureData)Children[0];
            int width = (int)Math.Sqrt((datachunk.Data.Length / 4));
            return $"Raw Texture: {Name} ({width}x{width})";
        }

        public override byte[] OnImagePreview()
        {
            RawColorTextureData datachunk = (RawColorTextureData)Children[0];
            int width = (int)Math.Sqrt((datachunk.Data.Length / 4));

            var png = PngBuilder.Create((int)width, (int)width, true);
            int i = 0;
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte R = datachunk.Data[i];
                    i++;
                    byte G = datachunk.Data[i];
                    i++;
                    byte B = datachunk.Data[i];
                    i++;
                    byte A = datachunk.Data[i];
                    i++;
                    png.SetPixel(new BigGustave.Pixel(R, G, B, A, false), x, y);
                }
            }
            return png.Save();
        }
    }
}
