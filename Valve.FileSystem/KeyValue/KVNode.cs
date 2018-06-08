using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valve.FileSystem.KeyValue
{
    public class KVNode
    {
        private KVNode parent;
        public KVNode Parent
        {
            get
            {
                return parent;
            }
        }
        private KVNodeType type;
        public KVNodeType Type
        {
            get
            {
                return type;
            }
        }
        public string KeyName { get; set; }
        private List<KVNode> Child { get; }
        
        private object value;
        public enum KVNodeType
        {
            Root,
            KeyValue,
            Parent,
            None
        }

        public KVNode(string name, bool root = false) //as root, parent
        {
            KeyName = name;
            if (root)
                type = KVNodeType.Root;
            else
                type = KVNodeType.None;
            Child = new List<KVNode>();
        }

        public KVNode(string name, params KVNode[] children) //as root, parent
        {
            KeyName = name;
            this.type = KVNodeType.Parent;
            Child = new List<KVNode>();
            foreach(var c in children)
            {
                AppendNode(c);
            }
        }

        public KVNode(string name, object value) //as keyvalue
        {
            KeyName = name;
            this.type = KVNodeType.KeyValue;
            this.value = value;
            Child = new List<KVNode>();
        }

        public void AppendNode(KVNode node)
        {
            if(type != KVNodeType.Root)
                type = KVNodeType.Parent;
            node.SetParent(this);
            this.value = null;
            if(node.Type == KVNodeType.KeyValue)
            {
                if (!Child.Exists(x=>x.KeyName.Equals(node.KeyName)))
                {
                    Child.Add(node);
                }
                else
                {
                    SetValue(node.GetValue(), node.KeyName);
                }
            }
            else
            {
                Child.Add(node);
            }
        }

        public void SetParent(KVNode node)
        {
            parent = node;
        }

        public bool SetValue(object value, string key = null, bool create = true)
        {
            if (key == null && this.type != KVNodeType.Parent && this.type != KVNodeType.Root)
            {
                this.value = value;
                type = KVNodeType.KeyValue;
                return true;
            }
            var bring = FindSingleNode(key, KVNodeType.KeyValue);
            if (bring != null)
            {
                bring.SetValue(value);
                return true;
            }
            else if(create == true)
            {
                AppendNode(new KVNode(key, value));
            }
            return false;
        }

        public object GetValue(string key = null)
        {
            if (type != KVNodeType.Root && type != KVNodeType.Parent)
            {
                return value.ToString();
            }
            KVNode bring = FindSingleNode(key, KVNodeType.KeyValue);
            if (bring != null)
            {
                return bring.GetValue(null);
            }
            return null;
        }

        public KVNode FindSingleNode(string key, KVNodeType type)
        {
            try
            {
                return Child.Find(x => x.Type == type && x.KeyName.ToLower().Equals(key.ToLower()));
            }
            catch
            {
                return null;
            }
        }

        public KVNode[] FindNodes(string key)
        {
            try
            {
                return Child.FindAll(x => x.KeyName.ToLower().Equals(key)).ToArray();
            }
            catch
            {
                return null;
            }
        }

        public KVNode[] FindNodes()
        {
            try
            {
                return Child.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public void DeleteThis()
        {
            if(Type != KVNodeType.Root)
            {
                Parent.Child.Remove(this);
            }
            foreach(var c in Child)
            {
                Delete(c);
            }
        }

        public void DeleteKey(string key)
        {
            foreach (var node in Child.FindAll(x => x.KeyName.ToLower().Equals(key.ToLower())))
            {
                Delete(node);
            }
        }

        public void Delete(KVNode node)
        {
            node.parent = null;
            if(node.Type == KVNodeType.Parent)
            {
                node.DeleteThis();
            }
        }
    }
}
