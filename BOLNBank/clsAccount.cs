using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsAccount
    {
        public long AccountID { get; set; }
        public string AccountCode { get; set; }
        public long CategoryID { get; set; }
        public string Prefix { get; set; }
        public string AccountName { get; set; }
        public string AccountShortName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public long StateID { get; set; }
        public string CSTNo { get; set; }
        public string VATNo { get; set; }
        public string PANNo { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string FaxNo { get; set; }
        public string ContactPersonName { get; set; }
        public string Notes { get; set; }
        public double OpeningBalance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public long UserID { get; set; }
        public bool IsActive { get; set; }
    }
}
