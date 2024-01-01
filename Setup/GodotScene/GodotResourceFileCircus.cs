using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CircusSetup
{
    public class GodotResourceFileCircus : GodotResourceFile
    {

        public static GodotResourceFileCircus Create(string Name)
        {
            GodotResourceFileCircus Res = new GodotResourceFileCircus();
            return Res;
        }

    }
}
