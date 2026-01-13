using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsCompanyWiseBank
    {
        public long CompanyWiseBankID { get; set; }
        public long BankID { get; set; }
        public long CompanyID { get; set; }
        public bool IsActive { get; set; }
        public string CompanyWiseBankCode { get; set; }

        public string CompanyName { get; set; }
        public string BankName { get; set; }

        public string CompanyCode { get; set; }
        public string BankCode { get; set; }
        public string CompanyShortName { get; set; }

        public string LastChequeNo { get; set; }


    }
}
