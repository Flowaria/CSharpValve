using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.TF2.Info;

namespace Valve.TF2.Items
{
    public class TFAttribute
    {
        public bool ReadOnly { get; set; } = true;

        private string name;
        private int defid; //defindex
        private string attribute_class;
        private string description_string;
        private TFAttrDescriptionFormat description_format;
        private TFAttrEffectType effect_type;
        private bool hidden;
        private TFItemSlot slot;

        public string Name
        {
            get { return name; }
            set { if (!ReadOnly) name = value; }
        }

        public int DefinitionID
        {
            get { return defid; }
            set { if (!ReadOnly) defid = value; }
        }

        public string AttributeClass
        {
            get { return attribute_class; }
            set { if (!ReadOnly) attribute_class = value; }
        }

        public string Description
        {
            get { return description_string; }
            set { if (!ReadOnly) description_string = value; }
        }

        public TFAttrDescriptionFormat DescriptionFormat
        {
            get { return description_format; }
            set { if (!ReadOnly) description_format = value; }
        }

        public TFAttrEffectType EffectType
        {
            get { return effect_type; }
            set { if (!ReadOnly) effect_type = value; }
        }

        public TFAttribute()
        {
            ReadOnly = false;
        }
    }
}
