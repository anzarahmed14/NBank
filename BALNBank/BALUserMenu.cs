using BOLNBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BALNBank
{
    public class BALUserMenu
    {
        string Message = "";
        List<clsUserMenu> list;
        DataTable _dt;
        public string CreateUserMenu(List<clsUserMenu> list)
        {
           // DataTable dt = ToDataTable(list);
            Message = (new DALNBank.DALUserMenu().CreateUserMenu(GetUserMenuTable(list)));
            return Message;
        }
        public List<clsUserMenu> GetUserMenu(long UserID)
        {
            list = (new DALNBank.DALUserMenu().GetUserMenu(UserID));
            return list;
        }
        private DataTable GetUserMenuTable(List<clsUserMenu> list) {
            _dt = new DataTable();
            _dt.Columns.Add("UserID", typeof(long));
            _dt.Columns.Add("MenuID", typeof(long));
            _dt.Columns.Add("MenuName", typeof(string));
            _dt.Columns.Add("AllowCreate", typeof(string));
            _dt.Columns.Add("AllowView", typeof(string));
            _dt.Columns.Add("AllowDelete", typeof(string));
            _dt.Columns.Add("AllowEdit", typeof(string));
            _dt.Columns.Add("AllowPrint", typeof(string));

            foreach (var m in list) {
                DataRow row = _dt.NewRow();
                row["UserID"] = m.UserID;
                row["MenuID"] = m.MenuID;
                row["MenuName"] = m.MenuName;
                row["AllowCreate"] = m.AllowCreate;
                row["AllowView"] = m.AllowView;
                row["AllowDelete"] = m.AllowDelete;
                row["AllowEdit"] = m.AllowEdit;
                row["AllowPrint"] = m.AllowPrint;
                _dt.Rows.Add(row);
            }


            return _dt;
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
