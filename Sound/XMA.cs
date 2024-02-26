using System.Collections.Generic;
using System.IO;
using Pure3D;
namespace XMA_Audio
{
    public static class XMA_Decoder
    {
        

        public static byte[] Decode(byte[] FileData, int channels, int channelpair)
        {
            byte[] outBuff = new byte[0];
            int BlockAlign = 2048;

            using (BinaryReader2 reader = new BinaryReader2(new MemoryStream(FileData)))
            using (MemoryStream pcmStream = new MemoryStream())
            using (BinaryWriter pcmWriter = new BinaryWriter(pcmStream))
            {
                uint XMA_SampleRate = 0;
                uint XMA_NumSamples = 0;

                // Parsing XMA header
                uint XMA_ChunkSize = reader.ReadUInt32();
                uint XMA_SeekSize = reader.ReadUInt32();
                uint XMA_StreamSize = reader.ReadUInt32();
                byte XMA_Version = reader.ReadByte();
                if (XMA_Version == 3)
                {
                    reader.BaseStream.Position = 0xC + 0x0C;
                    XMA_SampleRate = reader.ReadUInt32();
                    reader.BaseStream.Position = 0xC + 0x18;
                    XMA_NumSamples = reader.ReadUInt32();
                }
                else
                {
                    reader.BaseStream.Position = 0xC + 0x08;
                    XMA_SampleRate = reader.ReadUInt32();
                    reader.BaseStream.Position = 0xC + 0x0C;
                    XMA_NumSamples = reader.ReadUInt32();
                }
                reader.BaseStream.Position = 0x8 + XMA_ChunkSize + XMA_SeekSize;
                uint XMA_Duration = reader.ReadUInt32();
                // set skip samples to 576?
                

                // Decoding audio data
                //while (reader.BaseStream.Position < reader.BaseStream.Length)
                //{
                //    byte[] Data = reader.ReadBytes(0x400);
                //    int Duration = (Data[0] >> 2) * 512;
                //}


                outBuff = pcmStream.ToArray();
                pcmWriter.Close();
                pcmStream.Close();
                reader.Close();
            }

            
            
            return outBuff;
        }

    }
}