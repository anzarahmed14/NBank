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
    public class BALCompany
    {
        List<clsCompany> list;
        List<SqlParameter> plist;
        clsCompany obj;
        public List<clsCompany> GetCompanyList(string CompanyName = "", long UserID = 0)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CompanyName", SqlDbType.NVarChar, 100) { Value = CompanyName });

            if (UserID > 0)
            plist.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserID });

            list = (new DALCompany().GetCompanyList("GetCompany", plist));
            return list;
        }
        public clsCompany GetCompany(long CompanyID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CompanyID", SqlDbType.BigInt) { Value = CompanyID });
            obj = (new DALCompany().GetCompany("GetCompany", plist));
            return obj;
        }
    }
}
