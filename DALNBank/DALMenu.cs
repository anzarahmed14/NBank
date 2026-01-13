using BOLNBank;
using DALNBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALNBank
{
    public class DALMenu : SQLObject
    {
        List<clsMenu> list;
        clsMenu obj;
        public List<clsMenu> GetMenuList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsMenu>();
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedure;
                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();

                        if (plist.Count > 0)
                        {
                            foreach (var p in plist)
                            {
                                _cmd.Parameters.Add(p);

                            }
                        }
                        using (_reader = _cmd.ExecuteReader())
                        {

                            NullReader = new NullDataReader(_reader);

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    clsMenu obj = new clsMenu();
                                    obj.MenuID = NullReader.GetInt64("MenuID");
                                    obj.DisplayName = NullReader.GetString("DisplayName");
                                    obj.MenuName = NullReader.GetString("MenuName");
                                    obj.AllowCreate = NullReader.GetBoolean("AllowCreate");
                                    obj.AllowDelete = NullReader.GetBoolean("AllowDelete");
                                    obj.AllowEdit = NullReader.GetBoolean("AllowEdit");
                                    obj.AllowPrint = NullReader.GetBoolean("AllowPrint");
                                    obj.AllowView = NullReader.GetBoolean("AllowView");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    list.Add(obj);
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return list;
        }

        public clsMenu GetMenu(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {


                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedure;
                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();


                        if (plist.Count > 0)
                        {
                            foreach (var p in plist)
                            {
                                _cmd.Parameters.Add(p);

                            }
                        }
                        using (_reader = _cmd.ExecuteReader())
                        {

                            NullReader = new NullDataReader(_reader);

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    obj = new clsMenu();
                                    obj.MenuID = NullReader.GetInt64("MenuID");
                                    obj.DisplayName = NullReader.GetString("DisplayName");
                                    obj.MenuName = NullReader.GetString("MenuName");
                                    obj.AllowCreate = NullReader.GetBoolean("AllowCreate");
                                    obj.AllowDelete = NullReader.GetBoolean("AllowDelete");
                                    obj.AllowEdit = NullReader.GetBoolean("AllowEdit");
                                    obj.AllowPrint = NullReader.GetBoolean("AllowPrint");
                                    obj.AllowView = NullReader.GetBoolean("AllowView");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return obj;
        }
    }
}
