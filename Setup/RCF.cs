using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RCF_Archive
{
    public class BinaryReader2 : BinaryReader
    {
        public BinaryReader2(System.IO.Stream stream) : base(stream) { }

        public override int ReadInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        public override Int16 ReadInt16()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }

        public override Int64 ReadInt64()
        {
            var data = base.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }

        public override UInt32 ReadUInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public override float ReadSingle()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }

    }

    public class BinaryWriter2 : BinaryWriter
    {
        public BinaryWriter2(System.IO.Stream stream) : base(stream) { }

        public void WriteBigEndian(UInt32 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

        public void WriteBigEndian(UInt16 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

        public void WriteBigEndian(UInt64 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

        public void WriteBigEndian(float val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Array.Reverse(data);
            Write(data);
        }

    }


    public class RCF
    {
        public class FileEntry
        {
            public uint CRC;
            public uint Offset;
            public int Size;
            public int CompressedSize;
            public uint CompressionFlag; // 0 - No Compression, 1 - Compressed
            public DateTime LastModifiedTime;
            public string Name;

            public override string ToString()
            {
                return $"CRC: 0x{CRC:X8}\nOffset: 0x{Offset:X8}\nSize: 0x{Size:X8}\nLast Modified: {LastModifiedTime}\nCompression: {CompressionFlag}";
            }
        }

        public List<FileEntry> Files = new List<FileEntry>();
        public string FullName;

        public override string ToString()
        {
            return $"File Count: {Files.Count}";
        }

        public void Load(string path)
        {
            FullName = path;

            if (!File.Exists(path))
                return;

            /*
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
            */

            using (var br = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan))
            {
                using (BinaryReader reader = new BinaryReader(br))
                {
                    Load(reader, reader.BaseStream.Length);
                }
            }

            //using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    Load(fileStream);
        }

        public void Load(BinaryReader reader, long length)
        {
            BinaryReader2 reader2 = new BinaryReader2(reader.BaseStream);
            BinaryReader hReader = reader;
            reader.ReadChars(0x20); // Format Name
            byte Flag1 = reader.ReadByte();
            byte Flag2 = reader.ReadByte();
            bool Flag3 = reader.ReadBoolean();
            bool Flag4 = reader.ReadBoolean();
            if (Flag3)
            {
                hReader = reader2;
            }
            uint T1Offset = hReader.ReadUInt32();
            uint T1Size = hReader.ReadUInt32();
            uint T2Offset = hReader.ReadUInt32();
            uint T2Size = hReader.ReadUInt32();
            uint Zero1 = hReader.ReadUInt32();
            uint FileCount = hReader.ReadUInt32();

            hReader.BaseStream.Position = T1Offset;
            for (int i = 0; i < FileCount; i++)
            {
                FileEntry file = new FileEntry();
                file.CRC = hReader.ReadUInt32();
                file.Offset = hReader.ReadUInt32();
                if (Flag2 != 0)
                {
                    file.Size = hReader.ReadInt32();
                    file.CompressedSize = file.Size;
                }
                else
                {
                    // CTTR GC frontend.rcf - compression handling not yet implemented
                    file.CompressedSize = hReader.ReadInt32();
                    file.Size = hReader.ReadInt32();
                    file.CompressionFlag = hReader.ReadUInt32();
                }
                Files.Add(file);
            }
            List<FileEntry> SortList = Files.OrderBy(a => a.Offset).ToList();

            hReader.BaseStream.Position = T2Offset;
            uint Align = reader.ReadUInt32();
            uint Zero2 = reader.ReadUInt32();
            FileEntry HashDict = null;
            for (int i = 0; i < FileCount; i++)
            {
                FileEntry file = SortList[i];
                uint TimeModified = reader.ReadUInt32();
                file.LastModifiedTime = new DateTime(1970, 1, 1).AddSeconds(TimeModified);
                uint Align2 = reader.ReadUInt32();
                uint Zero3 = reader.ReadUInt32();
                int NameLength = reader.ReadInt32();
                file.Name = new string(reader.ReadChars(NameLength - 1));
                uint Zero4 = reader.ReadUInt32();
                if (file.Name == "hashdictionary.txt")
                {
                    HashDict = file;
                }
            }

            // Some versions of Titans/MoM include a hash dictionary for their hashed file names
            if (HashDict != null)
            {
                reader.BaseStream.Position = HashDict.Offset;
                byte[] hashbuffer = reader.ReadBytes((int)HashDict.Size);
                Dictionary<string, string> hashes = new Dictionary<string, string>();
                using (MemoryStream hashstream = new MemoryStream(hashbuffer))
                {
                    using (StreamReader sr = new StreamReader(hashstream))
                    {
                        string? line = sr.ReadLine();
                        while (line != null)
                        {
                            if (!hashes.ContainsKey(line.Split(" ")[1] + ".p3d"))
                            {
                                hashes.Add(line.Split(" ")[1] + ".p3d", line.Split(" ")[0]);
                            }
                            line = sr.ReadLine();
                        }
                    }
                }

                for (int i = 0; i < FileCount; i++)
                {
                    string key = Files[i].Name.Split("\\").Last();
                    if (hashes.ContainsKey(key))
                    {
                        Files[i].Name = hashes[key];
                    }
                }
            }

        }

        public void ExtractItem(FileEntry entry, string path)
        {
            if (!File.Exists(FullName))
                return;
            if (entry.CompressionFlag == 1)
                throw new NotImplementedException("Compressed files not implemented yet.");

            using (var br = new FileStream(FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan))
            {
                using (var bw = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryReader reader = new BinaryReader(br))
                    {
                        reader.BaseStream.Position = entry.Offset;
                        using (BinaryWriter writer = new BinaryWriter(bw))
                        {
                            writer.Write(reader.ReadBytes(entry.Size));
                        }
                    }
                }
            }
        }
        
        public void ExtractArchive(string path)
        {
            if (!File.Exists(FullName))
                return;
            string mainPath = System.IO.Path.GetDirectoryName(path) + @"\";
            bool ExtractRealAudio = true;

            using (var br = new FileStream(FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan))
            {
                using (BinaryReader freader = new BinaryReader(br))
                {
                    List<RCF.FileEntry> SortList = Files.OrderBy(a => a.Offset).ToList(); // probably faster this way
                    foreach (FileEntry entry in SortList)
                    {
                        if (entry.CompressionFlag == 1)
                        {
                            continue;
                        }
                        string foutPath = mainPath + entry.Name;
                        string fdirPath = System.IO.Path.GetDirectoryName(foutPath);
                        freader.BaseStream.Position = entry.Offset;

                        if (ExtractRealAudio && entry.Name.ToLower().EndsWith(".rsd"))
                        {
                            Pure3D.RSD RSD = new Pure3D.RSD();
                            RSD.Load(freader, entry.Size);

                            if (!Pure3D.Util.ExportToGodot)
                            {
                                string outPath = System.IO.Path.GetDirectoryName(path) + "\\Sounds\\";
                                outPath += RSD.ShortName + ".wav";
                                string dirPath = System.IO.Path.GetDirectoryName(outPath);
                                Directory.CreateDirectory(dirPath);

                                byte[] SoundData = new byte[0];
                                string name1 = outPath;
                                short channels = 1;
                                if (RSD.Channels > 1) channels = 2;
                                uint tracks = (RSD.Channels / 2);
                                switch (RSD.CodecString)
                                {
                                    case "XADP": // XBOX IMA ADPCM
                                        SoundData = IMA_ADPCM.IMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, 0);
                                        break;
                                    case "XMA ": // XBOX 360 XMA 
                                        SoundData = XMA_Audio.XMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, 0);
                                        break;
                                    case "VAG ": // PS2/PSP VAG ADPCM
                                        if (RSD.Channels >= 4)
                                        {
                                            SoundData = CircusSetup.ADPCM.ToPCMQuad(RSD.Data, RSD.Data.Length, (int)RSD.Interleave, 0, RSD.Channels);
                                        }
                                        else if (RSD.Channels == 2)
                                            SoundData = CircusSetup.ADPCM.ToPCMStereo(RSD.Data, RSD.Data.Length, (int)RSD.Interleave);
                                        else if (RSD.Channels == 1)
                                            SoundData = CircusSetup.ADPCM.ToPCMMono(RSD.Data, RSD.Data.Length);
                                        break;
                                    case "AT3+": // PSP ATRAC3+
                                        SoundData = AT3Plus.AT3P_Decoder.Decode(RSD.Data, (int)RSD.Channels, 0);
                                        break;
                                    case "RADP": // GCN/WII IMA ADPCM
                                        break;
                                    case "WADP": // WII NGC DSP
                                        break;
                                    case "PCM ": // WAV PCM
                                        SoundData = RSD.Data;
                                        break;
                                    default:
                                        break;
                                }
                                SoundData = CircusSetup.RIFF.SaveRiff(SoundData, channels, (int)RSD.SampleRate);
                                FileStream file = new FileStream(name1, FileMode.Create, FileAccess.Write);
                                BinaryWriter writer = new BinaryWriter(file);
                                writer.Write(SoundData);
                                writer.Close();

                                if (tracks > 1 && tracks < 32)
                                {
                                    for (int t = 1; t < tracks; t++)
                                    {
                                        string name2 = outPath.Replace(".wav", $"_{t}.wav");
                                        switch (RSD.CodecString)
                                        {
                                            case "XADP": // XBOX IMA ADPCM
                                                SoundData = IMA_ADPCM.IMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, t);
                                                break;
                                            case "XMA ": // XBOX 360 XMA 
                                                SoundData = XMA_Audio.XMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, t);
                                                break;
                                            case "VAG ": // PS2/PSP VAG ADPCM
                                                SoundData = CircusSetup.ADPCM.ToPCMQuad(RSD.Data, RSD.Data.Length, (int)RSD.Interleave, t, RSD.Channels);
                                                break;
                                            case "AT3+": // PSP ATRAC3+
                                                SoundData = AT3Plus.AT3P_Decoder.Decode(RSD.Data, (int)RSD.Channels, t);
                                                break;
                                            case "RADP": // GCN/WII IMA ADPCM
                                                break;
                                            case "WADP": // WII NGC DSP
                                                break;
                                            case "PCM ": // WAV PCM
                                                SoundData = RSD.Data;
                                                break;
                                            default:
                                                break;
                                        }
                                        SoundData = CircusSetup.RIFF.SaveRiff(SoundData, channels, (int)RSD.SampleRate);
                                        FileStream file2 = new FileStream(name2, FileMode.Create, FileAccess.Write);
                                        BinaryWriter writer2 = new BinaryWriter(file2);
                                        writer2.Write(SoundData);
                                        writer2.Close();
                                    }
                                }
                            }
                            else
                            {
                                string outPath = System.IO.Path.GetDirectoryName(path) + "\\Sounds\\";
                                outPath += RSD.ShortName + ".res";
                                string dirPath = System.IO.Path.GetDirectoryName(outPath);
                                Directory.CreateDirectory(dirPath);

                                CircusSetup.GodotBinaryAudioStreamWAV wav1 = new CircusSetup.GodotBinaryAudioStreamWAV(RSD, false, 0);
                                wav1.WriteToFile(outPath);

                                uint tracks = (RSD.Channels / 2);
                                if (tracks > 1 && tracks < 32)
                                {
                                    for (int t = 1; t < tracks; t++)
                                    {
                                        string name2 = outPath.Replace(".res", $"_{t}.res");
                                        CircusSetup.GodotBinaryAudioStreamWAV wav2 = new CircusSetup.GodotBinaryAudioStreamWAV(RSD, false, t);
                                        wav2.WriteToFile(name2);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(fdirPath);
                            using (var bw = new FileStream(foutPath, FileMode.Create, FileAccess.Write))
                            {
                                using (BinaryWriter writer = new BinaryWriter(bw))
                                {
                                    writer.Write(freader.ReadBytes(entry.Size));
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}