using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
     public class ChequeImportModel
    {
        public DateTime? IssueDate { get; set; }
        public string CompanyCode { get; set; }
        public string BankCode { get; set; }
        public string ProjectCode { get; set; }

        public string PartyName { get; set; }
        public string NBankAccountName { get; set; }

        public string ChequeNo { get; set; }
        public decimal ChequeAmount { get; set; }

        public string Type { get; set; }
        public string SubType { get; set; }

        public string NBankParameterName { get; set; }
        public string Parameter { get; set; }


        public bool IsAccountValid { get; set; } = true;
        public bool IsParameterValid { get; set; } = true;
        public bool IsBankValid { get; set; } = true;
        public bool IsSubTypeValid { get; set; } = true;
        public bool IsTypeValid { get; set; } = true;
    }
}
