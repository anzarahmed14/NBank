using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public  class clsChequeStatus
    {
        public long ChequeStatusID { get; set; }
        public string ChequeStatusName { get; set; }
        public string ChequeStatusShortName { get; set; }
        public string ChequeStatusCode { get; set; }
        public bool IsActive { get; set; }

    }

}
