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
    public class BALCompanyGroup
    {
        List<clsCompanyGroup> list;
        List<clsCompanyGroupCompanyNamesList> elist;
        List<SqlParameter> plist;
        clsCompanyGroup obj;
        public List<clsCompanyGroupCompanyNamesList> GetCompanyGroupList(string CompanyGroupName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CompanyGroupName", SqlDbType.NVarChar, 100) { Value = CompanyGroupName });

            elist = (new DALCompanyGroup().GetCompanyGroupList("GetCompanyGroup", plist));
            return elist;
        }
        public clsCompanyGroup GetCompanyGroup(long CompanyGroupID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CompanyGroupID", SqlDbType.BigInt) { Value = CompanyGroupID });
            obj = (new DALCompanyGroup().GetCompanyGroup("GetCompanyGroup", plist));
            return obj;
        }
    }
}
