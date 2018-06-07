using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Valve.KeyValue
{
    public static class KVFile
    {
        private static Regex regRemark;
        private static Regex regKv;
        private static Regex regQuote;

        static KVFile()
        {
            regRemark = new Regex("((\\/\\/+).+)|((\\/\\*+)(.|\\n|\\r)*(\\*\\/))"); //주석 제거
            regKv = new Regex("(\\\"([^\\\"]*?)\\\")|(\\'[^\\']*?\\')|[{}]|([^\\s][^\\s]*)"); //KeyValue 형식으로 나눔
            regQuote = new Regex("(^\"+|\"+$)|(^'+|'+$)"); //양끝 따옴표 제거
        }

        public static KeyValues ImportKeyValue(string content, bool overrideparentnode)
        {
            var rmvRemark = regKv.Matches(regRemark.Replace(content, ""));

            int depth = 0;
            KeyValues kv = new KeyValues("RootKey");
            KVNode current = null;
            for (int i = 0; i < rmvRemark.Count; i++)
            {
                
                string str = regQuote.Replace(rmvRemark[i].Value, "");
                if (str.Equals("}")) //single
                {
                    if (current.Type != KVNode.KVNodeType.Root)
                    {
                        current = current.Parent;
                        depth--;
                    }
                }
                else //two mixed
                {
                    string nxt_str = regQuote.Replace(rmvRemark[i + 1].Value, "");

                    if (str.Equals("#base")) //basefile
                    {
                        kv.AddBaseFile(nxt_str);
                        i++;
                    }
                    else if (nxt_str.Equals("{")) //parent
                    {
                        if (current == null)
                        {
                            kv.Root.KeyName = str;
                            current = kv.Root;
                        }
                        else
                        {
                            if (overrideparentnode && current.FindSingleNode(str, KVNode.KVNodeType.Parent) != null)
                            {
                                current = current.FindSingleNode(str, KVNode.KVNodeType.Parent);
                            }
                            else
                            {
                                var node = new KVNode(str);
                                current.AppendNode(node);
                                current = node;
                            }
                        }
                        depth++;
                    }
                    else //child
                    {
                        current.SetValue(nxt_str, str, true);
                    }
                    i++;
                }
            }
            if(depth == 0)
            {
                return kv;
            }
            return null;
        }

        public static string ExportKeyValue(KeyValues kv)
        {
            StringBuilder sb = new StringBuilder();
            NodeToFullFormatString(kv.Root, sb);
            Console.Write(sb.ToString());
            return sb.ToString();
        }
        private static void NodeToFullFormatString(KVNode node, StringBuilder sb, int depth=0)
        {
            
            if(node.Type == KVNode.KVNodeType.Root || node.Type == KVNode.KVNodeType.Parent)
            {
                sb.Append('	', depth);
                sb.AppendFormat("{0}\n",node.KeyName);
                sb.Append(' ', depth);
                sb.Append("{\n");
                foreach(var n in node.FindNodes())
                {
                    NodeToFullFormatString(n, sb, depth + 1);
                }
                sb.Append('\n');
                sb.Append(' ', depth);
                sb.Append("}");
            }
            else
            {
                sb.Append(' ', depth);
                sb.AppendFormat("\"{0}\" \"{1}\"", node.KeyName, node.GetValue().ToString());
            }
        }
    }
}
