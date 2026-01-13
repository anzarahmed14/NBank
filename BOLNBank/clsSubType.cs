using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsSubType
    {
        public long SubTypeID { get; set; }
        public string SubTypeCode { get; set; }
        public string SubTypeShortName { get; set; }
        public string SubTypeName { get; set; }
        public string SubTypePrintName { get; set; }
        public bool IsActive { get; set; }
        public int CreditDays { get; set; }

    }
}
