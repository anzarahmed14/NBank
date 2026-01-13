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
    public class BALChequeStatus
    {
        List<clsChequeStatus> list;
        List<SqlParameter> plist;
        clsChequeStatus obj;
        public List<clsChequeStatus> GetChequeStatusList(string ChequeStatusName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeStatusName", SqlDbType.NVarChar, 100) { Value = ChequeStatusName });

            list = (new DALChequeStatus().GetChequeStatusList("GetChequeStatus", plist));
            return list;
        }
        public clsChequeStatus GetChequeStatus(long ChequeStatusID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeStatusID", SqlDbType.BigInt) { Value = ChequeStatusID });
            obj = (new DALChequeStatus().GetChequeStatus("GetChequeStatus", plist));
            return obj;
        }
    }
}
