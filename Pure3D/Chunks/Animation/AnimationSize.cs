using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121004)]
    public class AnimationSize : Chunk
    {
        public uint Version;
        public uint PC;
        public uint PS2;
        public uint Xbox;
        public uint GameCube;

        public AnimationSize(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            PC = reader.ReadUInt32();
            PS2 = reader.ReadUInt32();
            Xbox = reader.ReadUInt32();
            GameCube = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(PC);
            writer.Write(PS2);
            writer.Write(Xbox);
            writer.Write(GameCube);
        }

        public override string ToString()
        {
            return $"Animation Size";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Animation Size");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"PC {PC}");
            Lines.AppendLine($"PS2 {PS2}");
            Lines.AppendLine($"Xbox {Xbox}");
            Lines.AppendLine($"GameCube {GameCube}");

            return Lines.ToString();
        }
    }
}
