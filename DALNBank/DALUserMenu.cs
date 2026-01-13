using BOLNBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALNBank
{
    public class DALUserMenu  : SQLObject
    {
        List<clsUserMenu> list;
        clsBank obj;
        public string CreateUserMenu(DataTable  list) {

            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = "CreateUserMenu";
                        _cmd.Parameters.Add(new SqlParameter("@UserMenu", SqlDbType.Structured)).Value = list;
                        _cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 400)).Direction = ParameterDirection.Output;
                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();

                        row =  _cmd.ExecuteNonQuery();
                        /*
                        using (_da = new SqlDataAdapter(_cmd)) {
                            _ds = new DataSet();
                            _da.Fill(_ds);
                        }
                        */

                       Message = Convert.ToString(((SqlParameter)_cmd.Parameters["@Message"]).Value);

                    }
                }

            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
            finally {

            }
            return Message;
        }
        public List<clsUserMenu> GetUserMenu(long UserID)
        {

            try
            {
                list = new List<clsUserMenu>();
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = "GetUserMenu";
                        _cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt)).Value = UserID;

                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();


                        using (_reader = _cmd.ExecuteReader())
                        {

                            NullReader = new NullDataReader(_reader);

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    clsUserMenu obj = new clsUserMenu();
                                    obj.UserID = NullReader.GetInt64("UserID");
                                    obj.MenuID = NullReader.GetInt64("MenuID");
                                    obj.AllowCreate = NullReader.GetBoolean("AllowCreate");
                                    obj.AllowDelete = NullReader.GetBoolean("AllowDelete");
                                    obj.AllowEdit = NullReader.GetBoolean("AllowEdit");
                                    obj.AllowPrint = NullReader.GetBoolean("AllowPrint");
                                    obj.AllowView = NullReader.GetBoolean("AllowView");
                                    obj.MenuName = NullReader.GetString ("MenuName");
                                    list.Add(obj);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
            finally {
                if (_conn.State == ConnectionState.Open )
                    _conn.Close();
            }
            return list;
        }
    }
}
