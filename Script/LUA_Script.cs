using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace CircusSetup.Script
{
    public class LUA_Script : Script
    {
        public override FileTypes FileType => FileTypes.LUA;
        
        public List<string> Lines = new List<string>();

        public override void Load(BinaryReader reader, long length)
        {
            StreamReader stream = new StreamReader(reader.BaseStream);
            string line = stream.ReadLine();
            while (line != null)
            {
                Lines.Add(line);
                line = stream.ReadLine();
            }
        }
    }
}