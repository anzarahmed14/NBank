using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsCompany
    {
        public long CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyShortName { get; set; }
        public long ProjectID { get; set; }
        public long BankID { get; set; }
        public bool IsActive { get; set; }
    }
}
