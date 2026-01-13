using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsProject
    {
        public long ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectCode { get; set; }
        public double SquareFit { get; set; }
        public bool IsActive { get; set; }
        public long CompanyID { get; set; }
        public string CompanyName { get; set; }
    }
}
