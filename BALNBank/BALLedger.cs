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
    public class BALLedger
    {
      
        List<SqlParameter> plist;
        DataSet _ds;
        
      

        public DataSet GetAccountLedger(DateTime StartDate, DateTime EndDate, long AccountID, long ProjectID, long UserId)
        {
            plist = new List<SqlParameter>();
            if (StartDate != DateTime.MinValue)
            plist.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = StartDate });
            if (EndDate != DateTime.MinValue)
                plist.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = EndDate });

            plist.Add(new SqlParameter("@AccountID", SqlDbType.BigInt) { Value = AccountID });
            plist.Add(new SqlParameter("@ProjectID", SqlDbType.BigInt) { Value = ProjectID });
            plist.Add(new SqlParameter("@DateType", SqlDbType.NVarChar,20) { Value = "CED" });

            if (UserId > 0)
            {
                plist.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserId });
            }

                _ds = (new DALDataAccess().GetDataSet("GetAccountLedger", plist));
            return _ds;
        }
        public DataSet GetChequeList(long ChequeEntryID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeEntryID", SqlDbType.BigInt) { Value = ChequeEntryID });
            _ds = (new DALDataAccess().GetDataSet("GetChequeList", plist));
            return _ds;
        }
    }
}
