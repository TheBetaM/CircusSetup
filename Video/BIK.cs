using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Controls;

namespace Pure3D
{
    public class BIK
    {
        public string FullName;
        public byte Version;
        public List<AudioTrack> AudioTracks = new List<AudioTrack>();
        

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
            Version = reader.ReadByte();
            reader.ReadUInt32(); // file size
            uint FrameCount = reader.ReadUInt32();
            uint LargestFrameSize = reader.ReadUInt32();
            uint FrameCount2 = reader.ReadUInt32();
            uint VideoWidth = reader.ReadUInt32();
            uint VideoHeight = reader.ReadUInt32();
            uint FPS_Dividend = reader.ReadUInt32();
            uint FPS_Divider = reader.ReadUInt32();
            uint VideoFlags = reader.ReadUInt32();
            uint AudioTrackCount = reader.ReadUInt32();
            for (int i = 0; i < AudioTrackCount; i++)
            {
                var track = new AudioTrack();
                reader.ReadUInt16();
                track.Channels = reader.ReadUInt16();
                AudioTracks.Add(track);
            }
            for (int i = 0; i < AudioTrackCount; i++)
            {
                AudioTracks[i].SampleRate = reader.ReadUInt16();
                AudioTracks[i].Flags = reader.ReadUInt16();
            }
            for (int i = 0; i < AudioTrackCount; i++)
            {
                AudioTracks[i].TrackID = reader.ReadUInt32();
            }
            List<uint> FrameOffsets = new List<uint>();
            for (int i = 0; i < FrameCount; i++)
            {
                uint FrameOffset = reader.ReadUInt32();
                if ((FrameOffset & 0x01) != 0)
                {
                    // is keyframe
                    FrameOffset--;
                }
                FrameOffsets.Add(FrameOffset);
            }
            reader.ReadUInt32(); // end of file

            for (int a = 0; a < FrameOffsets.Count; a++)
            {
                reader.BaseStream.Position = FrameOffsets[a];
                for (int i = 0; i < AudioTrackCount; i++)
                {
                    uint AudioPacketSize = reader.ReadUInt32();
                    uint PacketSampleCount = reader.ReadUInt32();
                    if (AudioPacketSize != 0)
                    {
                        AudioTracks[i].DataList.AddRange(reader.ReadBytes((int)AudioPacketSize - 4));
                    }
                }
                // video data here
            }

        }

        public void Write(BinaryWriter writer)
        {
            
        }

        public override string ToString()
        {
            StringBuilder lines = new StringBuilder();
            lines.AppendLine("BIK " + Version);
            //lines.AppendLine(Data.ToLine());
            return lines.ToString();
        }

        public class AudioTrack
        {
            public ushort Channels;
            public ushort SampleRate;
            public ushort Flags;
            public uint TrackID;

            public List<byte> DataList = new List<byte>();
            public byte[] SoundData;
        }
    }
}