using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class ChequeImportModel
    {
        public DateTime? EntryDate { get; set; }
        public DateTime? IssueDate { get; set; }

       

        public long CompanyID { get; set; }
        public string CompanyCode { get; set; }

        public long BankID { get; set; }
        public string BankCode { get; set; }

        public long ProjectID { get; set; }
        public string ProjectCode { get; set; }

        public string AccountName { get; set; }
        public long AccountID { get; set; }


        public string ChequeNo { get; set; }
        public decimal ChequeAmount { get; set; }

        public long TypeID { get; set; }
        public string Type { get; set; }


        public long SubTypeID { get; set; }
        public string SubType { get; set; }

        public long ParameterID { get; set; }   
        public string Parameter { get; set; }

        public string Narration { get; set; }


        public bool IsAccountValid { get; set; } = true;
        public bool IsParameterValid { get; set; } = true;
        public bool IsBankValid { get; set; } = true;
        public bool IsSubTypeValid { get; set; } = true;
        public bool IsTypeValid { get; set; } = true;
        public bool IsProjectValid { get; set; } = true;
    }
}
