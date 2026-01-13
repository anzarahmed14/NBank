using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public  class clsState
    {
        public long StateID { get; set; }
        public string StateName { get; set; }
        public string StateShortName { get; set; }
        public string StateShortCode { get; set; }
        public int StateNo { get; set; }
        public bool IsActive { get; set; }

    }
}
