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
    public class BALType
    {
        List<clsType> list;
        List<SqlParameter> plist;
        clsType obj;
        Dictionary<string, string> types;
        public List<clsType> GetTypeList(string TypeName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@TypeName", SqlDbType.NVarChar, 100) { Value = TypeName });

            list = (new DALType().GetTypeList("GetType", plist));
            return list;
        }
        public clsType GetTypeMaster(long TypeID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@TypeID", SqlDbType.BigInt) { Value = TypeID });
            obj = (new DALType().GetType("GetType", plist));
            return obj;
        }
         public Dictionary<string, string> LoadTypes()
         {

            types = (new DALType().LoadTypes());
            return types;
        }
}
}
