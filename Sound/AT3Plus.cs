using System.Collections.Generic;
using System.IO;
namespace AT3Plus
{
    public static class AT3P_Decoder
    {
        

        public static byte[] Decode(byte[] FileData, int channels, int channelpair)
        {
            byte[] outBuff = new byte[0];

            using (BinaryReader reader = new BinaryReader(new MemoryStream(FileData)))
            using (MemoryStream pcmStream = new MemoryStream())
            using (BinaryWriter pcmWriter = new BinaryWriter(pcmStream))
            {
                // Parsing RIFF header (usually 0x60 size?)
                int CodingMode = 0;
                reader.ReadBytes(0x14); // RIFF + size + WAVEfmt + size
                ushort FormatHeader = reader.ReadUInt16();
                ushort AT3_Channels = reader.ReadUInt16();
                uint SampleRate = reader.ReadUInt32();
                uint Bitrate = reader.ReadUInt32();
                ushort BytesPerFrame = reader.ReadUInt16();
                ushort BytesPerSample = reader.ReadUInt16();
                ushort ExtraDataSize = reader.ReadUInt16();
                if (ExtraDataSize == 14)
                {
                    reader.ReadBytes(6);
                    CodingMode = reader.ReadUInt16();
                    reader.ReadBytes(4);
                }
                else
                {
                    reader.ReadBytes(ExtraDataSize - 2);
                }
                uint factTest = reader.ReadUInt32();
                int AT3_EndSample = 0;
                int AT3_SkipSamples = 2048;
                int ImplicitSkip = 0;
                if (factTest == 0x74636166)
                {
                    int size = reader.ReadInt32();
                    if (size == 8)
                    {
                        AT3_EndSample = reader.ReadInt32();
                        AT3_SkipSamples = reader.ReadInt32();
                        ImplicitSkip = 368;
                    }
                    else if (size == 12)
                    {
                        AT3_EndSample = reader.ReadInt32();
                        reader.ReadUInt32();
                        AT3_SkipSamples = reader.ReadInt32();
                        ImplicitSkip = 184;
                    }
                    else
                    {
                        reader.ReadBytes(size);
                    }
                }
                reader.ReadBytes(8); // "data" + size

                // Decoding audio data
                


                outBuff = pcmStream.ToArray();
                pcmWriter.Close();
                pcmStream.Close();
                reader.Close();
            }

            
            
            return outBuff;
        }

    }
}