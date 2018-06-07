using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valve.KeyValue
{
    public class KeyValues
    {
        private List<string> basefile;
        public KVNode Root { get; }
        public string[] BaseFile
        {
            get
            {
                return basefile.ToArray();
            }
        }
        public KeyValues(string root)
        {
            basefile = new List<string>();
            Root = new KVNode(root);
        }

        public void AddBaseFile(string filename)
        {
            if(basefile.Exists(x=>x.Equals(filename)))
            {
                basefile.Add(filename);
            }
        }

        public void RemoveBaseFile(string filename)
        {
            if (basefile.Exists(x => x.Equals(filename)))
            {
                basefile.Remove(filename);
            }
        }

        public void Debug(KVNode start, int depth = 0)
        {
            foreach(KVNode node in start.FindNodes())
            {
                for (int i = 0;i<depth;i++)
                {
                    Console.Write("=");
                }
                if (node.Type == KVNode.KVNodeType.Parent)
                {
                    Console.WriteLine(node.KeyName);
                    Debug(node, depth+1);
                }
                else
                {
                    Console.WriteLine(String.Format("{0} {1}",node.KeyName, node.GetValue()));
                }
            }
        }
    }
}
