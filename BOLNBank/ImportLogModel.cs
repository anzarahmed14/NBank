using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class ImportLogModel
    {
        public long ImportLogID { get; set; }

        public long CompanyID { get; set; }
        public string CompanyName { get; set; }

        public long BankID { get; set; }
        public string BankName { get; set; }

        public int TotalRows { get; set; }
        public string FileName { get; set; }

        public long CreatedUserID { get; set; }
        public string CreatedUserName { get; set; }

        public DateTime CreatedDate { get; set; }
    }

}
