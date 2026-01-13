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
    public class BALSubType
    {
        List<clsSubType> list;
        List<SqlParameter> plist;
        clsSubType obj;
        public List<clsSubType> GetSubTypeList(string SubTypeName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@SubTypeName", SqlDbType.NVarChar, 100) { Value = SubTypeName });

            list = (new DALSubType().GetSubTypeList("GetSubType", plist));
            return list;
        }
        public clsSubType GetSubType(long SubTypeID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@SubTypeID", SqlDbType.BigInt) { Value = SubTypeID });
            obj = (new DALSubType().GetSubType("GetSubType", plist));
            return obj;
        }
    }
}
