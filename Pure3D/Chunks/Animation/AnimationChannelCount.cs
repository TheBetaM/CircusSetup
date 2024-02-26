using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using Pure3D;
using Pure3D.Chunks;
using System.Linq;

namespace Pure3D.Chunks
{
    [ChunkType(0x121007)]
    public class AnimationChannelCount : Unknown
    {
        public uint Pad;
        public uint Ref;
        public List<ushort> FrameCounts = new();
        public AnimationChannelCount(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Pad = reader.ReadUInt32();
            Ref = reader.ReadUInt32();

            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                FrameCounts.Add(reader.ReadUInt16());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            if (Chunk.chunkTypeDictionary.ContainsKey(Ref))
            {
                return $"Channel Count: {Chunk.chunkTypeDictionary[Ref].ToString().Split('.').Last()} / {Pad} / {FrameCounts.Count}";
            }
            else
            {   
                return $"Channel Count: {Ref} / {Pad} / {FrameCounts.Count}";
            }
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Animation Channel Count");
            Lines.AppendLine($"Ref: 0x{Ref:X8}");
            if (Chunk.chunkTypeDictionary.ContainsKey(Ref))
            {
                Lines.AppendLine($"RefType: {Chunk.chunkTypeDictionary[Ref].ToString().Split('.').Last()}");
            }
            Lines.AppendLine($"Pad: {Pad}");
            Lines.AppendLine($"Frame Counts: {FrameCounts.Count}");
            for (int i = 0; i < FrameCounts.Count; i++)
            {
                Lines.AppendLine($"#{i}: {FrameCounts[i]}");
            }

            return Lines.ToString();
        }
    }
}