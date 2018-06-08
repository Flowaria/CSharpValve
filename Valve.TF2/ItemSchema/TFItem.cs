using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.TF2.Info;

namespace Valve.TF2.Items
{
    public class TFItem
    {
        public bool ReadOnly { get; set; } = true;

        private string classname;
        private int defid;
        private string name;
        private string displayname;
        private string image;
        private string image_url;
        private TFItemSlot slot;
        public string ClassName
        {
            get { return classname; }
            set { if (!ReadOnly) classname = value; }
        }

        public int DefinitionID
        {
            get { return defid; }
            set { if (!ReadOnly) defid = value; }
        }

        public string Name
        {
            get { return name; }
            set { if (!ReadOnly) name = value; }
        }

        public string DisplayName
        {
            get { return displayname; }
            set { if (!ReadOnly) displayname = value; }
        }

        public string ImageName
        {
            get { return image; }
            set { if (!ReadOnly) image = value; }
        }

        public string ImageURL
        {
            get { return image_url; }
            set { if (!ReadOnly) image_url = value; }
        }

        public TFItemSlot Slot
        {
            get { return slot; }
            set {if (!ReadOnly) slot = value; }
        }

        private bool[] allowedclass;

        public TFItem()
        {
            ReadOnly = false;
            allowedclass = new bool[10] { false, false, false, false, false, false, false, false, false, false };
        }

        public TFItem(string _classname, int _defid, string _name, TFItemSlot _slot, string _image, string _displayname, bool allclass)
        {
            classname = _classname;
            defid = _defid;
            name = _name;
            slot = _slot;
            image_url = _image;
            displayname = _displayname;
            if (allclass)
                allowedclass = new bool[10] { true, true, true, true, true, true, true, true, true, true };
            else
                allowedclass = new bool[10] { false, false, false, false, false, false, false, false, false, false };

            
        }

        public void UsedByClass(TFClass c)
        {
            allowedclass[(int)c] = true;
        }

        public void UnusedByClass(TFClass c)
        {
            allowedclass[(int)c] = false;
        }

        public bool IsCosmetic()
        {
            return (slot == TFItemSlot.Misc);
        }

        public bool IsWearable()
        {
            return (classname.Contains("tf_wearable"));
        }

        public bool IsAllClass()
        {
            return (allowedclass[1] && allowedclass[2] && allowedclass[3]
                && allowedclass[4] && allowedclass[5] && allowedclass[6]
                && allowedclass[7] && allowedclass[8] && allowedclass[9]);
        }

        public bool IsUsedByClass(TFClass cls)
        {
            return allowedclass[(int)cls];
        }

        public void Debug()
        {
            Console.Out.WriteLine(String.Format("{0}:{1}:{2}", classname, name, displayname));
            Console.Out.WriteLine(String.Format(" {0}:{1}:{2}", defid, IsCosmetic(), IsWearable()));
        }
    }
}
