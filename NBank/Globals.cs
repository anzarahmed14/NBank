using BOLNBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBank
{
    public static class Globals
    {
        public static long UserID { get; set; }
        public static string UserName { get; set; }
        public static string FirstName { get; set; }
        public static List<clsUserMenu> UserMenuList { get;set;}
    }

}
