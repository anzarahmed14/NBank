using BOLNBank;
using DALNBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALNBank
{
    public class BALMenu
    {
        List<clsMenu> list;
        List<SqlParameter> plist;
        clsMenu obj;
        public List<clsMenu> GetMenuList(string MenuName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@MenuName", SqlDbType.NVarChar, 100) { Value = MenuName });

            list = (new DALMenu().GetMenuList("GetMenu", plist));
            return list;
        }
        public clsMenu GetMenu(long MenuID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@MenuID", SqlDbType.BigInt) { Value = MenuID });
            obj = (new DALMenu().GetMenu("GetMenu", plist));
            return obj;
        }
    }
}
