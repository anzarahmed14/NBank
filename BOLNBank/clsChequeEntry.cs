using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsChequeEntry
    {
        public long ChequeEntryID { get; set; }
        public DateTime ChequeEntryDate { get; set; }
        public string ChequeEntryCode { get; set; }
        public long ProjectID { get; set; }
        public long AccountID { get; set; }
        public string AccountSubName { get; set; }
        public long BankID { get; set; }
        public string ChequeNo { get; set; }
        public long TypeID { get; set; }
        public long SubTypeID { get; set; }
        public long ParameterID { get; set; }
        public long ChequeTypeID { get; set; }
        public long ChequeStatusID { get; set; }
        public DateTime ChequeIssueDate { get; set; }
        public DateTime ChequeClearDate { get; set; }
        public double  ChequeAmount { get; set; }
        public double ChequeAmountTDS { get; set; }
        public string Narration { get; set; }

        public long CompanyID { get; set; }

        public long CreatedUserID { get; set; }
        public long UpdatedUserID { get; set; }

        public string ERPID { get; set; }


    }
}
