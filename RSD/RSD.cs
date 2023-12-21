using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Pure3D
{
    public class RSD
    {
        public string FullName;
        public uint Channels;
        public char Version;
        public string CodecString;
        public uint SampleRate;
        public string Name;
        public string Desc;
        public byte[] Data;
        public uint Interleave;
        public byte XMA_Version = 0;
        public uint XMA_SampleRate = 0;
        public uint XMA_NumSamples = 0;
        public string ShortName;
        const string NameCheck1 = "in_game_art\\sound\\sounds\\";
        const string NameCheck2 = "\\export\\sound\\source\\";

        public void Load(string path)
        {
            FullName = path;

            using (var br = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan))
            {
                byte[] buffer = new byte[br.Length];
                br.Read(buffer, 0, buffer.Length);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (BinaryReader reader = new BinaryReader(memoryStream))
                    {
                        Load(reader, reader.BaseStream.Length);
                    }
                }
            }

            //using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    Load(fileStream);
        }

        public void Load(BinaryReader reader, long length)
        {
            string IDCheck = new string(reader.ReadChars(3));
            if (IDCheck != "RSD") return;
            Version = reader.ReadChar();
            CodecString = new string(reader.ReadChars(4));
            Channels = reader.ReadUInt32();
            Interleave = reader.ReadUInt32(); // 0x10
            SampleRate = reader.ReadUInt32();
            reader.ReadUInt32(); // 0x2A2A2A2A
            Name = new string(reader.ReadChars(0x8C));
            Desc = new string(reader.ReadChars(0x100));
            reader.ReadBytes(0x65C); // padding to 0x800
            if (CodecString == "XMA ")
            {
                BinaryReader2 readerb = new BinaryReader2(reader.BaseStream);
                readerb.BaseStream.Position = 0x800;
                uint XMA_ChunkSize = readerb.ReadUInt32();
                uint XMA_SeekSize = readerb.ReadUInt32();
                uint XMA_StreamSize = readerb.ReadUInt32();
                XMA_Version = readerb.ReadByte();
                if (XMA_Version == 3)
                {
                    readerb.BaseStream.Position = 0x80C + 0x0C;
                    XMA_SampleRate = readerb.ReadUInt32();
                    readerb.BaseStream.Position = 0x80C + 0x18;
                    XMA_NumSamples = readerb.ReadUInt32();
                }
                else
                {
                    readerb.BaseStream.Position = 0x80C + 0x08;
                    XMA_SampleRate = readerb.ReadUInt32();
                    readerb.BaseStream.Position = 0x80C + 0x0C;
                    XMA_NumSamples = readerb.ReadUInt32();
                }
                readerb.BaseStream.Position = 0x80C + XMA_ChunkSize + XMA_SeekSize;
            }
            Data = reader.ReadBytes((int)(length - reader.BaseStream.Position));

            if (Name.Contains(NameCheck1))
            {
                int pos = Name.IndexOf(NameCheck1) + NameCheck1.Length;
                ShortName = Name.Substring(pos).Split('.')[0].TrimEnd('\0');
            }
            else if (Name.Contains(NameCheck2))
            {
                int pos = Name.IndexOf(NameCheck2) + NameCheck2.Length;
                ShortName = Name.Substring(pos).Split('.')[0].TrimEnd('\0');
            }
            else
            {
                ShortName = "UNK";
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write("RSD6".ToCharArray());
            writer.Write(CodecString);
            writer.Write(Channels);
            writer.Write(Interleave);
            writer.Write(SampleRate);
            writer.Write(0x2A2A2A2A);
            writer.Write(Name);
            while (writer.BaseStream.Position < 0xA4)
            {
                writer.Write((byte)0);
            }
            writer.Write(Desc);
            while (writer.BaseStream.Position < 0x1A4)
            {
                writer.Write((byte)0);
            }
            while (writer.BaseStream.Position < 0x1A4)
            {
                writer.Write((byte)0);
            }
            while (writer.BaseStream.Position < 0x800)
            {
                writer.Write((byte)0x2D);
            }
            writer.Write(Data);
        }

        public override string ToString()
        {
            StringBuilder lines = new StringBuilder();
            lines.AppendLine("RSD " + Version);
            lines.AppendLine($"Codec: {CodecString}");
            lines.AppendLine($"Channels: {Channels}");
            lines.AppendLine($"Interleave: {Interleave}");
            lines.AppendLine($"Sample Rate: {SampleRate}");
            lines.AppendLine($"Name: {Name.TrimEnd('\0')}");
            lines.AppendLine($"AutoName: {ShortName}");
            lines.AppendLine($"Desc: {Desc.TrimEnd('\0')}");
            return lines.ToString();
        }
    }
}