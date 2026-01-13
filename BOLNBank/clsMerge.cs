using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsMerge
    {
        public long ToCompanyID { get; set; }
        public long FromCompanyID { get; set; }
        public long ToProjectID { get; set; }
        public long FromProjectID { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserID { get; set; }

        public long FromAccountID { get;set;}
        public long ToAccountID { get; set; }
    }

}
