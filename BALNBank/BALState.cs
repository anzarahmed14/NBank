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
    public class BALState
    {
        List<clsState> list;
        List<SqlParameter> plist;
        clsState obj;

      
        IEnumerable<dynamic> list2;
        string StoredProcedureName = "";
        public List<clsState> GetState()
        {
            StoredProcedureName = "GetState";
            list = (new DALState().GetState(StoredProcedureName));
            return list;
        }
        public clsState GetState(long StateID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@StateID", SqlDbType.BigInt) { Value = StateID });
            obj = (new DALState().GetState("GetState", plist));
            return obj;
        }


        public IEnumerable<dynamic> GetState2(string StoredProcedureName)
        {
            list2 = (new DALState().GetState2(StoredProcedureName));
            return list2;
        }
        public List<clsState> GetStateList(string StateName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@StateName", SqlDbType.NVarChar, 100) { Value = StateName });

            list = (new DALState().GetStateList("GetState", plist));
            return list;
        }
    }
}
