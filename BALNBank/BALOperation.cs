using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DALNBank;
using System.Reflection;

namespace BALNBank
{
    public class BALOperation
    {
        string Message = "";
        List<SqlParameter> list;
        string DeleteID = "";
        string ClassName = "";
       
        public string  Create(dynamic obj, List<string> plist = null)
        {
            // Type t = typeof(obj);

            

            ClassName = obj.GetType().Name;
            list = (new BALDynamicProperty().GetSQLParameter(obj, plist));

            if (ClassName == "clsChequeEntry")
            {
                list.RemoveAll(p =>
                p.ParameterName.Equals("CreatedUserName", StringComparison.OrdinalIgnoreCase) ||
                p.ParameterName.Equals("UpdatedUserName", StringComparison.OrdinalIgnoreCase)
            );
            }
            list.Add(new SqlParameter("@Message", SqlDbType.NVarChar,400) {  Direction = ParameterDirection.Output});

             Message = (new DALDataAccess().ExecuteNonQuery(ClassName.Replace("cls", "Create"), list));
            return Message;
        }
        public string Update(dynamic obj,List<string> plist = null)
        {
            ClassName = obj.GetType().Name;
            list = (new BALDynamicProperty().GetSQLParameter(obj, plist));

            if (ClassName == "clsChequeEntry") {
                list.RemoveAll(p =>
                p.ParameterName.Equals("CreatedUserName", StringComparison.OrdinalIgnoreCase) ||
                p.ParameterName.Equals("UpdatedUserName", StringComparison.OrdinalIgnoreCase)
            );
            }
            


            list.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 400) { Direction = ParameterDirection.Output });

            Message = (new DALDataAccess().ExecuteNonQuery(ClassName.Replace("cls", "Update"), list));
            return Message;
        }
        public string Delete(dynamic obj, long ID)
        {
            ClassName = obj.GetType().Name;
            DeleteID = ClassName.Replace("cls", "");
            DeleteID = DeleteID + "ID";
            list = new List<SqlParameter>();
           list.Add(new SqlParameter("@"+ DeleteID, SqlDbType.BigInt) { Value = ID });
            list.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 400) { Direction = ParameterDirection.Output });

            Message = (new DALDataAccess().ExecuteNonQuery(ClassName.Replace("cls", "Delete"), list));
            return Message;
        }
        /*
        public List<dynamic> Get(long  ID)
        {
            return Message;
        }
        */

    }
}
