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
    public class DALUser:SQLObject
    {

        List<clsUser> list;
        clsUser obj;
        public List<clsUser> GetUserList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsUser>();
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
                                    clsUser obj = new clsUser();
                                    obj.UserID = NullReader.GetInt64("UserID");
                                    obj.UserCode = NullReader.GetString("UserCode");
                                    obj.UserName = NullReader.GetString("UserName");
                                    obj.FirstName = NullReader.GetString("FirstName");
                                    obj.LastName = NullReader.GetString("LastName");
                                    obj.EmailAddress = NullReader.GetString("EmailAddress");
                                    obj.ContactNo = NullReader.GetString("ContactNo");
                                    obj.MobileNo = NullReader.GetString("MobileNo");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    list.Add(obj);
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return list;
        }

        public clsUser GetUser(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsUser();
                                    obj.UserID = NullReader.GetInt64("UserID");
                                    obj.UserCode = NullReader.GetString("UserCode");
                                    obj.UserName = NullReader.GetString("UserName");
                                    obj.FirstName = NullReader.GetString("FirstName");
                                    obj.LastName = NullReader.GetString("LastName");
                                    obj.EmailAddress = NullReader.GetString("EmailAddress");
                                    obj.ContactNo = NullReader.GetString("ContactNo");
                                    obj.MobileNo = NullReader.GetString("MobileNo");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");


                                }
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return obj;
        }
        public DataSet UserLogin(string StoredProcedure, string UserName, string UserPassword)
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
                        _cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 20)).Value = UserName;
                        _cmd.Parameters.Add(new SqlParameter("@UserPassword ", SqlDbType.NVarChar, 20)).Value = UserPassword;

                        using (_da = new SqlDataAdapter(_cmd))
                        {
                            _ds = new DataSet();
                            _da.Fill(_ds);
                        }
                     
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return _ds;
        }
        public string ChangePassword(string StoredProcedure, string OldPassword, string NewPassword, long  UserID)
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
                        _cmd.Parameters.Add(new SqlParameter("@OldPassword", SqlDbType.NVarChar, 20)).Value = OldPassword;
                        _cmd.Parameters.Add(new SqlParameter("@NewPassword", SqlDbType.NVarChar, 20)).Value = NewPassword;
                        _cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt, 20)).Value = UserID;
                        _cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 400)).Direction = ParameterDirection.Output;

                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();
                        row = _cmd.ExecuteNonQuery();
                        Message = Convert.ToString(((SqlParameter)_cmd.Parameters["@Message"]).Value);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return Message;
        }
    }
}
