using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace CircusSetup.Script
{
    public class ScriptParser
    {
        public string FullName;

        public Script script;

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
            uint Tag = reader.ReadUInt32();
            uint Tag2 = reader.ReadUInt32();
            reader.BaseStream.Position -= 8;
            script = null;
            switch (Tag)
            {
                case 0x69676542: // "BeginObject" GOD Lua object holder (CTTR)
                    break;
                case 0x65746E65: // "enterCondition" plain text FightTree (CTTR)
                    break;
                case 0x61754C1B: // ".Lua" binary custom Lua compiler (Titans/MoM)
                    script = new BinaryLUA();
                    break;
                case 0x30676966: // "fig0" binary FightTree (Titans/MoM)
                    break;
                case 0xDEC15109: // binary decision tree (Titans/MoM)
                    break;
                default: // plain text Lua (rare)
                    break;
            }
            if (script == null)
            {
                if (Tag2 == 0x61754C1B) // ".Lua" binary custom Lua compiler (MoM FightTree)
                {
                    reader.ReadUInt32();
                    script = new BinaryLUA();
                }
            }
            script.FullName = FullName;
            script.Load(reader, length);
        }

        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder lines = new StringBuilder();
            return lines.ToString();
        }
    }

    public class Script
    {
        public virtual FileTypes FileType => FileTypes.LUA;
        public string FullName;
        public enum FileTypes
        {
            GOD,
            BinaryLUA,
            LUA,
            FightTree,
            BinaryFightTree,
            DecisionTree,
        }

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

        public void Save(string path)
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    Write(writer);
                    ms.Position = 0;
                    buffer = ms.ToArray();
                }
            }
            File.WriteAllBytes(path, buffer);
        }

        public virtual void Load(BinaryReader reader, long length)
        {

        }

        public virtual void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public virtual string? ToDetails()
        {
            return ToString();
        }
    }
}