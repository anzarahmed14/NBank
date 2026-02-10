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
    public class BALParameter
    {
        List<clsParameter> list;
        List<SqlParameter> plist;
        clsParameter obj;
        Dictionary<string, string> parameters;
        public List<clsParameter> GetParameterList(string ParameterName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ParameterName", SqlDbType.NVarChar, 100) { Value = ParameterName });

            list = (new DALParameter().GetParameterList("GetParameter", plist));
            return list;
        }
        public clsParameter GetParameter(long ParameterID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ParameterID", SqlDbType.BigInt) { Value = ParameterID });
            obj = (new DALParameter().GetParameter("GetParameter", plist));
            return obj;
        }
        public Dictionary<string, string> LoadParameters()
        {
            parameters = (new DALParameter().LoadParameters());
            return parameters;
        }
    }
}
