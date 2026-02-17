using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class ChequeDuplicateKey
    {
        public string ChequeNo { get; set; }
        public DateTime IssueDate { get; set; }
        public long BankID { get; set; }
        public long CompanyID { get; set; }

        public override bool Equals(object obj)
        {
            ChequeDuplicateKey other =
                obj as ChequeDuplicateKey;

            if (other == null)
                return false;

            return
                string.Equals(
                    ChequeNo,
                    other.ChequeNo,
                    StringComparison.OrdinalIgnoreCase)
                &&
                IssueDate.Date ==
                    other.IssueDate.Date
                &&
                BankID == other.BankID
                &&
                CompanyID == other.CompanyID;
        }

        public override int GetHashCode()
        {
            return
                (ChequeNo ?? "")
                .ToUpper()
                .GetHashCode()
                ^
                IssueDate.Date.GetHashCode()
                ^
                BankID.GetHashCode()
                ^
                CompanyID.GetHashCode();
        }
    }


}
