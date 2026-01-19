using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsChequeEntrySearchParameter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string  ChequeNo { get; set; }
        public long CompanyID { get; set; }
        public long ProjectID { get; set; }
        public long TypeID { get; set; }
        public long SubTypeID { get; set; }
        public long BankID { get; set; }
        public long ParameterID { get; set; }
        public long ChequeEntryID { get; set; }

        public long UserId { get; set; }
    }
}
