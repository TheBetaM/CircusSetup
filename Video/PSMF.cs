using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Controls;

namespace Pure3D
{
    public class PSMF
    {
        public string FullName;
        public string Version;
        public List<AudioTrack> AudioTracks = new List<AudioTrack>() { new AudioTrack()};
        

        public void Load(string path)
        {
            FullName = path;

            using (var br = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan))
            {
                byte[] buffer = new byte[br.Length];
                br.Read(buffer, 0, buffer.Length);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (BinaryReader2 reader = new BinaryReader2(memoryStream))
                    {
                        Load(reader, reader.BaseStream.Length);
                    }
                }
            }

            //using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    Load(fileStream);
        }

        public void Load(BinaryReader2 reader, long length)
        {
            string IDCheck = new string(reader.ReadChars(4));
            if (IDCheck != "PSMF") return;
            Version = new string(reader.ReadChars(4));
            uint DataOffset = reader.ReadUInt32();
            uint DataSize = reader.ReadUInt32();
            reader.ReadBytes(0x40);
            reader.ReadUInt32(); // size of mapping table
            reader.ReadUInt16();
            uint TickFreq = reader.ReadUInt32();
            reader.ReadUInt16();
            uint DurationInTicks = reader.ReadUInt32();
            uint MuxRate = reader.ReadUInt32();
            reader.BaseStream.Position = DataOffset;

            // MPEG2 header parsing
            uint Header = reader.ReadUInt32();
            int CurrentTrack = 0;
            while (Header != (uint)PSS_SectionType.FileEnd && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                switch (Header)
                {
                    case (uint)PSS_SectionType.FileStart:
                    {
                        reader.BaseStream.Position += 0x0A;
                    }
                    break;
                    default:
                    if (Header <= 0x1AF)
                    {
                        reader.BaseStream.Position += 0x0A;
                    }
                    else if (AudioStreamBlockIDs.Contains(Header))
                    {
                        ushort BlockSize = reader.ReadUInt16();
                        CurrentTrack = 0;
                        reader.BaseStream.Position += 0x10;
                        CurrentTrack = reader.ReadByte();
                        if (CurrentTrack != 0 && CurrentTrack > AudioTracks.Count - 1)
                        {
                            AudioTracks.Add(new AudioTrack());
                        }
                        byte[] Buffer;
                        if (AudioTracks[CurrentTrack].DataList.Count == 0)
                        {
                            reader.ReadUInt32(); // SShd
                            reader.ReadUInt32(); // size
                            AudioTracks[CurrentTrack].Codec = reader.ReadUInt32();
                            AudioTracks[CurrentTrack].SampleRate = reader.ReadUInt32();
                            AudioTracks[CurrentTrack].Channels = reader.ReadUInt32();
                            AudioTracks[CurrentTrack].Interleave = reader.ReadUInt32();
                            reader.BaseStream.Position += 0x10; // padding, SSbd, size
                            Buffer = reader.ReadBytes(BlockSize - 0x39);
                        }
                        else
                        {
                            Buffer = reader.ReadBytes(BlockSize - 0x11);
                        }
                        AudioTracks[CurrentTrack].DataList.AddRange(Buffer);
                    }
                    else if (VideoStreamBlockIDs.Contains(Header))
                    {
                        ushort BlockSize = reader.ReadUInt16();
                        reader.BaseStream.Position += BlockSize;
                    }
                    else if (Header == (uint)PSS_SectionType.SystemHeader || Header == (uint)PSS_SectionType.PaddingStream)
                    {
                        ushort BlockSize = reader.ReadUInt16();
                        reader.BaseStream.Position += BlockSize;
                    }
                    else
                    {
                        throw new Exception($"PSMF: Unknown block ID {Header:X8}.");
                    }
                    break;
                }

                if (reader.BaseStream.Position < reader.BaseStream.Length)
                    Header = reader.ReadUInt32();
            }

        }

        public void Write(BinaryWriter writer)
        {
            
        }

        public override string ToString()
        {
            StringBuilder lines = new StringBuilder();
            lines.AppendLine("PSMF");
            //lines.AppendLine(Data.ToLine());
            return lines.ToString();
        }

        static List<uint> AudioStreamBlockIDs = new List<uint>()
        {
            0x01C0,
            0x01C1,
            0x01C2,
            0x01C3,
            0x01C4,
            0x01C5,
            0x01C6,
            0x01C7,
            0x01C8,
            0x01C9,
            0x01CA,
            0x01CB,
            0x01CC,
            0x01CD,
            0x01CE,
            0x01CF,
            0x01D0,
            0x01D1,
            0x01D2,
            0x01D3,
            0x01D4,
            0x01D5,
            0x01D6,
            0x01D7,
            0x01D8,
            0x01D9,
            0x01DA,
            0x01DB,
            0x01DC,
            0x01DD,
            0x01DE,
            0x01DF,

            0x01BD,
            0x01BF,
        };
        static List<uint> VideoStreamBlockIDs = new List<uint>()
        {
            0x01E0,
            0x01E1,
            0x01E2,
            0x01E3,
            0x01E4,
            0x01E5,
            0x01E6,
            0x01E7,
            0x01E8,
            0x01E9,
            0x01EA,
            0x01EB,
            0x01EC,
            0x01ED,
            0x01EE,
            0x01EF,
        };
        enum PSS_SectionType
        {
            FileStart = 0x000001BA,
            FileEnd = 0x000001B9,
            SystemHeader = 0x000001BB,
            PaddingStream = 0x000001BE,
        }

        public class AudioTrack
        {
            public uint TrackID;
            public uint Codec; // 1 - PCM16LE
            public uint SampleRate;
            public uint Channels;
            public uint Interleave;
            public List<byte> DataList = new List<byte>();
            public bool IsSideL = true;
        }
    }
}