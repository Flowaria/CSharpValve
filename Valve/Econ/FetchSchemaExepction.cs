using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valve.Econ
{
    public enum FetchSchemaExepctionType
    {
        FAIL_INVALID_APIKEY,
        FAIL_ACCESS,
    }

    public class FetchSchemaExepction : Exception
    {
        public FetchSchemaExepctionType Type { get; set; }
    }
}
