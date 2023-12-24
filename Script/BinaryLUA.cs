using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using CircusSetup.Script.LUA;

namespace CircusSetup.Script
{
    public class BinaryLUA : Script
    {
        public override FileTypes FileType => FileTypes.BinaryLUA;

        public LuaFunc MainFunc = new LuaFunc();

        public override void Load(BinaryReader reader, long length)
        {
            uint Tag = reader.ReadUInt32();
            if (Tag != 0x61754C1B) return;
            byte Version = reader.ReadByte(); //0x51 Lua 5.1
            byte Endianness = reader.ReadByte(); // 1 - little
            byte Size_Int = reader.ReadByte();
            byte Size_SizeT = reader.ReadByte();
            byte Size_Instruction = reader.ReadByte();
            byte Size_Number = reader.ReadByte(); 
            float SampleNumber = reader.ReadUInt32(); // 31415926... aka Pi
            MainFunc.LoadFunction(reader, "(chunk)", 0, 1);
        }

        public override void Write(BinaryWriter writer)
        {
            Write_5_0(writer);
        }

        public void Write_5_0(BinaryWriter writer)
        {
            writer.Write(0x61754C1B);
            writer.Write((byte)0x50);
            writer.Write((byte)0x01);
            writer.Write((byte)0x04);
            writer.Write((byte)0x04);
            writer.Write((byte)0x04);
            writer.Write((byte)0x06);
            writer.Write((byte)0x08);
            writer.Write((byte)0x09);
            writer.Write((byte)0x09);
            writer.Write((byte)0x08);
            writer.Write(0x417DF5E7689309B6);
            MainFunc.WriteFunction_5_0(writer);
        }

        public void Write_5_1(BinaryWriter writer)
        {
            writer.Write(0x61754C1B);
            writer.Write((byte)0x51);
            writer.Write((byte)0x00);
            writer.Write((byte)0x01);
            writer.Write((byte)0x04);
            writer.Write((byte)0x04);
            writer.Write((byte)0x04);
            writer.Write((byte)0x08); // Number
            writer.Write((byte)0x00);
            MainFunc.WriteFunction_5_1(writer);
        }



        public class LuaFunc
        {
            public string sourceFileName;
            public int level;
            public byte Nups;
            public byte NumParams;
            public byte IsVararg;
            public byte MaxStackSize;
            public uint LineDefined;
            public List<uint> Code = new List<uint>();
            public List<object> Constants = new List<object>();
            public List<ushort> LineInfos = new List<ushort>();
            public List<string> Upvalues = new List<string>();
            public List<(string, uint, uint)> Locals = new List<(string, uint, uint)>(); //varname, startpc, endpc
            public List<LuaFunc> LocalFuncs = new List<LuaFunc>();
            public LuaFunc Parent;

            public void LoadFunction(BinaryReader reader, string funcName, int num, int inlevel)
            {
                level = inlevel;
                sourceFileName = LoadString(reader);
                LineDefined = reader.ReadUInt32();
                Nups = reader.ReadByte();
                NumParams = reader.ReadByte();
                IsVararg = reader.ReadByte();
                MaxStackSize = reader.ReadByte();
                LoadLineInfos(reader);
                LoadLocals(reader);
                LoadUpvalues(reader);
                LoadConstants(reader);
                LoadFunctions(reader);
                LoadCode(reader);
            }

            void LoadLineInfos(BinaryReader reader)
            {
                int size = reader.ReadInt32();
                for (int i = 0; i < size; i++)
                {
                    LineInfos.Add(reader.ReadUInt16());
                }
            }

            void LoadLocals(BinaryReader reader)
            {
                int size = reader.ReadInt32();
                for (int i = 0; i < size; i++)
                {
                    string varname = LoadString(reader);
                    uint startpc = reader.ReadUInt32();
                    uint endpc = reader.ReadUInt32();
                    Locals.Add((varname, startpc, endpc));
                }
            }

            void LoadUpvalues(BinaryReader reader)
            {
                int size = reader.ReadInt32();
                for (int i = 0; i < size; i++)
                {
                    Upvalues.Add(LoadString(reader));
                }
            }

            void LoadConstants(BinaryReader reader)
            {
                int size = reader.ReadInt32();
                for (int i = 0; i < size; i++)
                {
                    byte type = reader.ReadByte();
                    if (type == 0)
                    {
                        Constants.Add(null);
                    }
                    else if (type == 1)
                    {
                        Constants.Add((bool)(reader.ReadByte() != 0));
                    }
                    else if (type == 3)
                    {
                        Constants.Add(reader.ReadSingle());
                    }
                    else if (type == 4)
                    {
                        Constants.Add(LoadString(reader));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            void LoadFunctions(BinaryReader reader)
            {
                int size = reader.ReadInt32();
                for (int i = 0; i < size; i++)
                {
                    LuaFunc func = new LuaFunc();
                    func.Parent = this;
                    func.LoadFunction(reader, sourceFileName, i, level + 1);
                    LocalFuncs.Add(func);
                }
            }

            void LoadCode(BinaryReader reader)
            {
                int size = reader.ReadInt32();
                for (int i = 0; i < size; i++)
                {
                    Code.Add(reader.ReadUInt32());
                }
            }

            string LoadString(BinaryReader reader)
            {
                int Length = reader.ReadInt32() - 1;
                if (Length < 0) return string.Empty;
                if (Length == 0)
                {
                    reader.ReadByte();
                    return string.Empty;
                }
                string val = new string(reader.ReadChars(Length));
                reader.ReadByte();
                return val;
            }

            public void WriteFunction_5_0(BinaryWriter writer)
            {
                WriteString(writer, sourceFileName);
                writer.Write(LineDefined);
                writer.Write(Nups);
                writer.Write(NumParams);
                writer.Write(IsVararg);
                writer.Write(MaxStackSize);
                WriteLineInfos(writer);
                WriteLocals(writer);
                WriteUpvalues(writer);
                WriteConstants(writer);
                WriteFunctions_5_0(writer);
                WriteCode(writer);
            }

            public void WriteFunction_5_1(BinaryWriter writer)
            {
                WriteString(writer, sourceFileName);
                writer.Write(LineDefined);
                writer.Write((uint)0);
                writer.Write(Nups);
                writer.Write(NumParams);
                writer.Write(IsVararg);
                writer.Write(MaxStackSize);
                WriteCode(writer);
                WriteConstants(writer);
                WriteFunctions_5_1(writer);
                WriteLineInfos(writer);
                WriteLocals(writer);
                WriteUpvalues(writer);
            }

            void WriteLineInfos(BinaryWriter writer)
            {
                writer.Write(LineInfos.Count);
                foreach (var item in LineInfos)
                {
                    writer.Write((uint)item);
                }
            }

            void WriteLocals(BinaryWriter writer)
            {
                writer.Write(Locals.Count);
                foreach (var item in Locals)
                {
                    WriteString(writer, item.Item1);
                    writer.Write(item.Item2);
                    writer.Write(item.Item3);
                }
            }

            void WriteUpvalues(BinaryWriter writer)
            {
                writer.Write(Upvalues.Count);
                foreach (var item in Upvalues)
                {
                    WriteString(writer, item);
                }
            }

            void WriteConstants(BinaryWriter writer)
            {
                writer.Write(Constants.Count);
                foreach (var item in Constants)
                {
                    if (item is null)
                    {
                        writer.Write((byte)0);
                    }
                    else if (item is bool bval)
                    {
                        writer.Write((byte)1);
                        if (bval)
                            writer.Write((byte)1);
                        else
                            writer.Write((byte)0);
                    }
                    else if (item is float fval)
                    {
                        writer.Write((byte)3);
                        writer.Write((double)fval);
                    }
                    else if (item is string sval)
                    {
                        writer.Write((byte)4);
                        WriteString(writer, sval);
                    }
                }
            }

            void WriteFunctions_5_0(BinaryWriter writer)
            {
                writer.Write(LocalFuncs.Count);
                foreach (var item in LocalFuncs)
                {
                    item.WriteFunction_5_0(writer);
                }
            }

            void WriteFunctions_5_1(BinaryWriter writer)
            {
                writer.Write(LocalFuncs.Count);
                foreach (var item in LocalFuncs)
                {
                    item.WriteFunction_5_1(writer);
                }
            }

            void WriteCode(BinaryWriter writer)
            {
                writer.Write(Code.Count);
                foreach (var item in Code)
                {
                    var opcode = item & 0x3F;
                    if (opcode == (uint)OpCodeTitans.TitansAdd)
                    {
                        uint fixCode = item - opcode;
                        //fixCode += (uint)OpCodeTitans.NewTable;
                        fixCode += (uint)OpCodeTitans.Closure;
                        writer.Write(fixCode);
                        //writer.Write(item);
                    }
                    else
                    {
                        writer.Write(item);
                    }
                }
            }
            
            void WriteString(BinaryWriter writer, string text)
            {
                writer.Write((uint)text.Length + 1);
                writer.Write(text.ToCharArray());
                writer.Write((byte)0);
            }
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Binary LUA");
            NestedFunc(MainFunc, Lines, "");

            return Lines.ToString();
        }

        void NestedFunc(LuaFunc func, StringBuilder sb, string add)
        {
            sb.AppendLine($"{add}{func.sourceFileName} (LEV: {func.level})");
            sb.AppendLine($"{add}Nups: {func.Nups} NumParams: {func.NumParams} IsVararg: {func.IsVararg} MaxStackSize {func.MaxStackSize}");
            sb.AppendLine($"{add}Line defined: {func.LineDefined}");
            sb.AppendLine($"{add}Lineinfos: {func.LineInfos.Count}");
            sb.AppendLine($"{add}Locals: {func.Locals.Count}");
            sb.AppendLine($"{add}Upvalues: {func.Upvalues.Count}");
            sb.AppendLine($"{add}Constants: {func.Constants.Count}");
            sb.AppendLine($"{add}Functions: {func.LocalFuncs.Count}");
            sb.AppendLine($"{add}Codes: {func.Code.Count}");
            int i = 0;
            foreach (var item in func.Code)
            {
                var opcode = item & 0x3F;
                var itemA = (item >> 6) & 0xFF;
                var itemB = (item >> 14) & 0x1FF;
                var itemC = (item >> 23) & 0x1FF;
                sb.AppendLine($"{add}[{i}]{item:X8}: {opcode}/{(OpCodeTitans)opcode} {itemA} {itemB} {itemC}");
                i++;
            }
            foreach (var item in func.LocalFuncs)
            {
                NestedFunc(item, sb, add + "---");
            }
        }
    }

}