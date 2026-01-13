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
    public class BALGroup
    {
        List<clsGroup> list;
        List<SqlParameter> plist;
        clsGroup obj;
        public List<clsGroup> GetGroupList(string GroupName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 100) { Value = GroupName });

            list = (new DALGroup().GetGroupList("GetGroup", plist));
            return list;
        }
        public clsGroup GetGroup(long GroupID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@GroupID", SqlDbType.BigInt) { Value = GroupID });
            obj = (new DALGroup().GetGroup("GetGroup", plist));
            return obj;
        }
    }
}
