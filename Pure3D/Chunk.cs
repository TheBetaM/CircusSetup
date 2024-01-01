using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pure3D
{
    public abstract class Chunk
    {
        public uint Type;
        public List<Chunk> Children;
        public File File;
        public Chunk Parent;
        public long chunkStart;
        public uint headerSize;
        public uint chunkSize;
        public long chunkEnd;
        public bool FailedToLoad;

        public bool IsRoot
        {
            get
            {
                return this == File.RootChunk;
            }
        }

        public Chunk(File file, uint type)
        {
            Children = new List<Chunk>();
            Type = type;
            File = file;
        }

        public T GetChild<T>() where T : Chunk
        {
            return (T)Children.Find(delegate (Chunk c) { return c is T; });
        }

        public T[] GetChildren<T>() where T : Chunk
        {
            return Children.FindAll(delegate (Chunk c) { return c is T; }).Cast<T>().ToArray();
        }

        public T[] GetChildrenByName<T>(string name) where T : Chunks.Named
        {
            return Children.FindAll(delegate (Chunk c) { return c is T && ((Chunks.Named)c).Name == name; }).Cast<T>().ToArray();
        }

        public T GetChildByName<T>(string name) where T : Chunks.Named
        {
            return (T)Children.Find(delegate (Chunk c) { return c is T && ((Chunks.Named)c).Name == name; });
        }

        public int GetChildIndexByName<T>(string name) where T : Chunks.Named
        {
            if (GetChildByName<T>(name) != null)
            {
                Chunk targetChunk = GetChildByName<T>(name);
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i] == targetChunk)
                    {
                        return i;
                    }
                }
                return -1;
            }
            else
            {
                return -1;
            }
        }

        public void ReadChildren(BinaryReader reader)
        {
            // todo: probably move all this logic to a seperate method.
            while (reader.BaseStream.Position < chunkEnd)
            {
                uint type = reader.ReadUInt32();
                Chunk chunk = NewChunkFromType(File, type);
                //Debug.WriteLine($"{chunk.GetType()} 0x{reader.BaseStream.Position:X8}");
                //Debug.WriteLine($"{chunk.GetType()} {chunk.ToString()} 0x{reader.BaseStream.Position:X8}");

                // sort hierarchy
                chunk.Parent = this;
                Children.Add(chunk);

                chunk.Read(reader, chunkEnd);
            }
        }

        public void WriteChildren(BinaryWriter writer)
        {
            if (Children.Count > 0)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    writer.Write(Children[i].Type);

                    Children[i].Write(writer, true, chunkEnd);
                }
            }
        }

        public void Read(BinaryReader reader, long parentChunkEnd)
        {
            chunkStart = reader.BaseStream.Position - 4;
            headerSize = reader.ReadUInt32();
            chunkSize = reader.ReadUInt32();
            //Console.WriteLine($"Header size: {headerSize}, chunk size {chunkSize}.");

            if (headerSize > chunkSize)
                throw new Exception($"Header size {headerSize} greater then chunk size {chunkSize}.");

            if ((reader.BaseStream.Position + chunkSize - 12) > parentChunkEnd)
                throw new Exception("Chunk size too high.");

            chunkEnd = chunkStart + chunkSize;

            try
            {
                ReadHeader(reader, headerSize - 12);
                ReadChildren(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chunk read error: {ex.Message}");
                FailedToLoad = true;
                reader.BaseStream.Position = chunkEnd;
            }

            if (reader.BaseStream.Position != chunkEnd)
            {
                //throw new Exception($"Stream position expected {chunkEnd} but is {reader.BaseStream.Position}");
                Console.WriteLine($"Chunk read error: Stream position expected {chunkEnd:X8} but is {reader.BaseStream.Position:X8}");
                FailedToLoad = true;
                reader.BaseStream.Position = chunkEnd;
            }
        }

        public abstract void ReadHeader(BinaryReader reader, long length);

        public void Write(BinaryWriter writer, bool writeChildren, long parentChunkEnd)
        {
            long headerPos = writer.BaseStream.Position;
            writer.Write(headerSize);
            long chunkSizePos = writer.BaseStream.Position;
            writer.Write(chunkSize);

            WriteHeader(writer);

            headerSize = (uint)(writer.BaseStream.Length - (headerPos - 4));
            long tempPos = writer.BaseStream.Position;
            writer.BaseStream.Position = headerPos;
            writer.Write(headerSize);
            writer.BaseStream.Position = tempPos;

            if (writeChildren)
            {
                WriteChildren(writer);
            }

            chunkSize = (uint)(writer.BaseStream.Length - (headerPos - 4));
            tempPos = writer.BaseStream.Position;
            writer.BaseStream.Position = chunkSizePos;
            writer.Write(chunkSize);
            writer.BaseStream.Position = tempPos;
            //Console.WriteLine($"Header size: {headerSize}, chunk size {chunkSize}.");

        }

        public abstract void WriteHeader(BinaryWriter writer);

        protected static Dictionary<uint, Type> chunkTypeDictionary = null;
        public static Chunk NewChunkFromType(File file, uint type)
        {
            // cache the list
            if (chunkTypeDictionary == null)
            {
                chunkTypeDictionary = new Dictionary<uint, Type>();

                foreach (var chunk in ChunkType.GetSupported())
                {
                    ChunkType chunkAttr = (ChunkType)chunk.GetCustomAttribute(typeof(ChunkType), false);
                    chunkTypeDictionary[chunkAttr.TypeID] = chunk;
                    //Debug.WriteLine($"0x{chunkAttr.TypeID:X8}: {chunk.Name}");
                }
            }

            if (!chunkTypeDictionary.ContainsKey(type))
                return new Chunks.Unknown(file, type);

            Type chunkType = chunkTypeDictionary[type];
            return (Chunk)Activator.CreateInstance(chunkType, new object[] { file, type });
            
        }

        public override string ToString()
        {
            return $"Chunk Type: {Type}";
        }

        public virtual string? ToDetails()
        {
            return ToString();
        }

        public virtual void OnExport(string path)
        {

        }

        public virtual byte[] OnImagePreview()
        {
            return null;
        }

        public virtual void OnGodotExport(string path)
        {

        }
    }
}
