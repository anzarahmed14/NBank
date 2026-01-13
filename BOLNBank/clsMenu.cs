using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class clsMenu
    {
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowView { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowPrint { get; set; }

    }
}
