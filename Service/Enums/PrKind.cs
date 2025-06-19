using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Enums
{
    public enum PrKind
    {
        [Description("Regular Pr")]
        RegularPr = 1,
        [Description("Pr For Consolodation")]
        PrForConsolodation = 2,
        [Description("Consolidated Pr")]
        ConsolidatedPr = 3
    }
}
