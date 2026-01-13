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
    public class BALChequeType
    {
        List<clsChequeType> list;
        List<SqlParameter> plist;
        clsChequeType obj;
        public List<clsChequeType> GetChequeTypeList(string ChequeTypeName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeTypeName", SqlDbType.NVarChar, 100) { Value = ChequeTypeName });

            list = (new DALChequeType().GetChequeTypeList("GetChequeType", plist));
            return list;
        }
        public clsChequeType GetChequeType(long ChequeTypeID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeTypeID", SqlDbType.BigInt) { Value = ChequeTypeID });
            obj = (new DALChequeType().GetChequeType("GetChequeType", plist));
            return obj;
        }
    }
}
