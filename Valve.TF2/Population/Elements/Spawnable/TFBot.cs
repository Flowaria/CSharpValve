using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.TF2.Info;

namespace TF2.Population.Element
{
    public class TFBot : Spawnable
    {
        public int BotAttribute;
        public Attributes BodyAttributes;
        public TFBotAttribute dddd;

        public TFBot()
        {
            
            BodyAttributes.Set(32, 1.0f);
        }
    }
}
