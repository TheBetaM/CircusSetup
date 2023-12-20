using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    public abstract class ShaderParam : Chunk
    {
        public string Param;

        public ShaderParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Param = Util.ZeroTerminate(Encoding.ASCII.GetString(reader.ReadBytes(4)));
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i < Param.Length)
                {
                    writer.Write((byte)Param[i]);
                }
                else
                {
                    writer.Write((byte)0x00);
                }
            }
        }

        public override string ToString()
        {
            return $"Shader Parameter: {Param}";
        }
    }

    [ChunkType(0x11002)]
    public class ShaderTextureParam : ShaderParam
    {
        public string Value;
        public ulong Value_paddding;

        public ShaderTextureParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Value = Util.ReadString(reader, ref Value_paddding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            Util.WriteString(writer, Value, Value_paddding);
        }

        public override string ToString()
        {
            return $"Texture {Param}: {Value}";
        }
    }

    [ChunkType(0x11003)]
    public class ShaderIntParam : ShaderParam
    {
        public uint Value;

        public ShaderIntParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Value = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Value);
        }

        public override string ToString()
        {
            return $"Int {Param}: {Value}";
        }
    }

    [ChunkType(0x11004)]
    public class ShaderFloatParam : ShaderParam
    {
        public float Value;

        public ShaderFloatParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Value = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Value);
        }

        public override string ToString()
        {
            return $"Float {Param}: {Value}";
        }
    }

    [ChunkType(0x11005)]
    public class ShaderColourParam : ShaderParam
    {
        public ByteColour Color;

        public ShaderColourParam(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Color = Util.ReadColour(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            Util.WriteColour(writer, Color);
        }

        public override string ToString()
        {
            return $"Colour {Param}: {Color}";
        }
    }
}
