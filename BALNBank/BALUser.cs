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
    public class BALUser
    {
        List<clsUser> list;
        List<SqlParameter> plist;
        clsUser obj;
        string Message = "";
        DataSet _ds;
        public List<clsUser> GetUserList(string UserName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 100) { Value = UserName });

            list = (new DALUser().GetUserList("GetUser", plist));
            return list;
        }
        public clsUser GetUser(long UserID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@UserID", SqlDbType.BigInt) { Value = UserID });
            obj = (new DALUser().GetUser("GetUser", plist));
            return obj;
        }
        public string ChangePassword(string OldPassword, string NewPassword, long UserID)
        {
            //ChangePassword
            Message = (new DALUser().ChangePassword("ChangePassword",OldPassword,NewPassword,UserID));
            return Message;
        }
        public DataSet UserLogin( string UserName, string UserPassword)
        {
           // _ds = new DataSet();
            _ds = (new DALUser().UserLogin("UserLogin", UserName, UserPassword));
            return _ds;
        }
    }
}
