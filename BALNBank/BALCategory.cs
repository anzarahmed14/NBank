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
    public class BALCategory
    {
        List<clsCategory> list;
        List<SqlParameter> plist;
        clsCategory obj;
        public List<clsCategory> GetCategoryList(string CategoryName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CategoryName", SqlDbType.NVarChar, 100) { Value = CategoryName });

            list = (new DALCategory().GetCategoryList("GetCategory", plist));
            return list;
        }
        public clsCategory GetCategory(long CategoryID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CategoryID", SqlDbType.BigInt) { Value = CategoryID });
            obj = (new DALCategory().GetCategory("GetCategory", plist));
            return obj;
        }
    }
}
