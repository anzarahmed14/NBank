using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsGroup
    {
        public long GroupID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string GroupShortName { get; set; }
        public bool IsActive { get; set; }

    }
}
