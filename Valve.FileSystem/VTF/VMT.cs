using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.FileSystem.KeyValue;

namespace Valve.FileSystem.TextureFormat
{
    public class VMT
    {
        public VMT()
        {
            KeyValues kv = KeyValues.ImportKeyValue("filename", false);
            var basetexture = (string)kv.Root.GetValue("$baseTexture");
        }
    }
}
