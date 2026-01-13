using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsChequeType
    {
        public long ChequeTypeID { get; set; }
        public string ChequeTypeName { get; set; }
        public string ChequeTypeCode { get; set; }
        public bool IsActive { get; set; }

    }
}
