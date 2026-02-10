using BOLNBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALNBank;
using System.Data.SqlClient;
using System.Data;

namespace BALNBank
{
    public class BALAccount
    {
        List<clsAccount> list;
        List<SqlParameter> plist;
        clsAccount obj;
        Dictionary<string, string> accounts;
        public List<clsAccount> GetAccountList(string AccountName = "", string AccountShortName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@AccountName", SqlDbType.NVarChar,100) { Value = AccountName });
            plist.Add(new SqlParameter("@AccountShortName", SqlDbType.NVarChar, 100) { Value = AccountShortName });

            list = (new DALAccount().GetAccountList("GetAccountMaster", plist));
           return list;
        }
        public clsAccount GetAccount(long AccountID )
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@AccountID", SqlDbType.BigInt) { Value = AccountID});
             obj = (new DALAccount().GetAccount("GetAccountMaster", plist));
            return obj;
        }

        public Dictionary<string, string> LoadAccounts()
        {
            accounts = (new DALAccount().LoadAccounts());
            return accounts;
        }
    }
}
