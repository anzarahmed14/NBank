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
    public class BALBank
    {
        List<clsBank> list;
        List<SqlParameter> plist;
        clsBank obj;
        public List<clsBank> GetBankList(string BankName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@BankName", SqlDbType.NVarChar, 100) { Value = BankName });

            list = (new DALBank().GetBankList("GetBank", plist));
            return list;
        }
        public clsBank GetBank(long BankID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@BankID", SqlDbType.BigInt) { Value = BankID });
            obj = (new DALBank().GetBank("GetBank", plist));
            return obj;
        }
    }
}
