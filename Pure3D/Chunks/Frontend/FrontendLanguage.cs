using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Pure3D;
using Pure3D.Chunks;
using System.Diagnostics;

namespace Pure3D.Chunks
{
    [ChunkType(0x1800E)]
    public class FrontendLanguage : Named
    {

        public uint StringAmount;
        public uint BufferSize;
        public char LanguageLetter; // E F I G S
        public uint Modulo; // always the same value
        public uint[] HashesList;
        public uint[] OffsetsList;
        public byte[] Buffer;
        public List<string> StringHashes; //todo, variable names that the text refers to
        public List<string> TextStrings;

        public FrontendLanguage(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            LanguageLetter = (char)reader.ReadByte();
            StringAmount = reader.ReadUInt32();
            Modulo = reader.ReadUInt32();
            BufferSize = reader.ReadUInt32();
            HashesList = new uint[StringAmount];
            OffsetsList = new uint[StringAmount];
            for (int i = 0; i < StringAmount; i++)
            {
                HashesList[i] = reader.ReadUInt32();
            }
            for (int i = 0; i < StringAmount; i++)
            {
                OffsetsList[i] = reader.ReadUInt32();
            }
            Buffer = new byte[BufferSize];
            for (int i = 0; i < BufferSize; i++)
            {
                Buffer[i] = reader.ReadByte();
            }

            try
            {
                TextStrings = new List<string>();
                int pos = 0;
                string targetString = "";
                for (int i = 0; i < StringAmount; i++)
                {
                    targetString = "";
                    if (i != StringAmount - 1)
                    {
                        while (pos < OffsetsList[i + 1])
                        {
                            targetString += (char)Buffer[pos];
                            pos += 2;
                        }
                    }
                    else
                    {
                        while (pos < BufferSize)
                        {
                            targetString += (char)Buffer[pos];
                            pos += 2;
                        }
                    }
                    TextStrings.Add(Util.ZeroTerminate(targetString));
                }
            }
            catch
            {
                // a few packs in titans
                Debug.WriteLine($"Failed to load FrontendLanguage {Name}");
            }
            

        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write((byte)LanguageLetter);

            StringAmount = (uint)TextStrings.Count;
            writer.Write(StringAmount);
            writer.Write(Modulo);

            BufferSize = 0;
            for (int i = 0; i < StringAmount; i++)
            {
                BufferSize += (uint)(TextStrings[i].Length + 1) * 2;
            }

            Buffer = new byte[BufferSize];
            int BufferPos = 0;
            for (int i = 0; i < StringAmount; i++)
            {
                OffsetsList[i] = (uint)BufferPos;
                for (int a = 0; a < TextStrings[i].Length; a++)
                {
                    Buffer[BufferPos] = (byte)TextStrings[i][a];
                    BufferPos++;
                    BufferPos++;
                }
                BufferPos += 2;
            }

            writer.Write(BufferSize);
            for (int i = 0; i < StringAmount; i++)
            {
                writer.Write(HashesList[i]);
            }
            for (int i = 0; i < StringAmount; i++)
            {
                writer.Write(OffsetsList[i]);
            }
            for (int i = 0; i < BufferSize; i++)
            {
                writer.Write(Buffer[i]);
            }
        }

        public override string ToString()
        {
            return $"FE Language: {LanguageLetter}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Language: {Name}");
            Lines.AppendLine($"StringAmount: {StringAmount}");
            Lines.AppendLine($"BufferSize: {BufferSize}");
            Lines.AppendLine($"LanguageLetter: {LanguageLetter}");
            Lines.AppendLine($"Modulo: {Modulo}");
            for (int i = 0; i < TextStrings.Count; i++)
            {
                Lines.AppendLine($"String{i}: {TextStrings[i]}");
            }

            return Lines.ToString();
        }
    }
}