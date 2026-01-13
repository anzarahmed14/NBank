using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BALNBank
{
    public class BALDynamicProperty
    {
        List<SqlParameter> obj = new List<SqlParameter>();

        public List<SqlParameter> GetSQLParameter(dynamic pObject, List<string> plist =null)
        {
            PropertyInfo[] properties = pObject.GetType().GetProperties();
            List<SqlParameter> list = new List<SqlParameter>();

            foreach (PropertyInfo property in properties)
            {
                if (plist != null)
                {
                    if (plist.Count > 0)
                    {
                        // if (plist.Any(str => str.Contains(property.Name))) {
                        if (plist.Contains(property.Name, StringComparer.InvariantCulture))
                        {
                            list.Add(GetSQLParameter(property, pObject));
                        }

                    }
                }
                else
                {
                    /*Specialy for chequer entry for clear date bank*/
                    if (property.Name == "ChequeClearDate" || property.Name == "ChequeIssueDate")
                    {
                        if (property.GetValue(pObject, null) != DateTime.MinValue) {

                            list.Add(GetSQLParameter(property, pObject));
                        }
                    }
                    else {
                        list.Add(GetSQLParameter(property, pObject));
                    }
                    
                }
               
               
                
            }
            return list;
        }

        public SqlParameter GetSQLParameter(PropertyInfo property, dynamic pObject)
        {
            SqlParameter par = new SqlParameter();
            switch (property.PropertyType.ToString())
            {
              
                case "System.Int64":
                    par.SqlDbType = SqlDbType.BigInt;
                    break;
                case "System.Double":
                    par.SqlDbType = SqlDbType.Decimal ;
                    break;
                case "System.Decimal":
                    par.SqlDbType = SqlDbType.Decimal;
                    break;
                case "System.String":
                    par.SqlDbType = SqlDbType.NVarChar;
                    //par.Size = property.GetValue(pObject, null).Length;
                    break;
                case "System.Int32":
                    par.SqlDbType = SqlDbType.Int;
                    break;
                case "System.Boolean":
                    par.SqlDbType = SqlDbType.Bit;
                    break;
                case "System.DateTime":
                    par.SqlDbType = SqlDbType.DateTime;
                    break;
                default:
                    par.SqlDbType = SqlDbType.NVarChar;
                    break;
            }

            string Test = property.Name;
            //par.SqlDbType = SqlDbType.NVarChar;
            par.Direction = ParameterDirection.Input;
            par.Value = property.GetValue(pObject, null);
            par.ParameterName = property.Name;
            return par;
        }
    }
}
