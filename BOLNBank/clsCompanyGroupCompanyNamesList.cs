using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsCompanyGroupCompanyNamesList
    {
        public long CompanyGroupID { get; set; }
        public string CompanyGroupCode { get; set; }
        public string CompanyGroupName { get; set; }
        public bool IsActive { get; set; }

        public string CompanyNames { get; set; }
    }
}
