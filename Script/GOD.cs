using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace CircusSetup.Script
{
    public class GOD : Script
    {
        public override FileTypes FileType => FileTypes.GOD;
        
        public List<GOD_Object> Objects = new List<GOD_Object>();

        public override void Load(BinaryReader reader, long length)
        {
            StreamReader stream = new StreamReader(reader.BaseStream);
            string line = stream.ReadLine();
            GOD_Object obj = null;
            while (line != null)
            {
                if (line == "EndObject")
                {
                    Objects.Add(obj);
                }
                else if (line.StartsWith("BeginObject"))
                {
                    obj = new GOD_Object();
                    string[] split = line.Split(' ');
                    obj.Type = split[1];
                    obj.Name = split[2];
                }
                else if (obj != null)
                {
                    obj.Lines.Add(line);
                }
                line = stream.ReadLine();
            }
        }

        public class GOD_Object
        {
            public string Name;
            public string Type;
            public List<string> Lines = new List<string>();
        }
    }
}