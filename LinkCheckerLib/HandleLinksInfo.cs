using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkCheckerLib
{
    public class HandleLinksInfo
    {
        public int InputTableRow { get; set; }
        public int InputTableColumn { get; set; }
        public int OutputTableRow { get; set; }
        public int OutputTableColumn { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string[] ErrorPatterns { get; set; }
    }
}
