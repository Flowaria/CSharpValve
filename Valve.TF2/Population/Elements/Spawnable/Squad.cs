using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2.Population.Element
{
    public class Squad : RandomChoice
    {
        public int FormatationSize { get; set; }
        public bool PreserveSuqad { get; set; } = false;
    }
}
