using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BOLNBank;
namespace DALNBank
{
    public class DALMerge:SQLObject
    {
        public DataSet GetMergeRecordAccount(long FromAccountID)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.CommandText = "GetMergeRecordAccount";
                        _cmd.Connection = _conn;

                        _cmd.Parameters.Add(new SqlParameter("@FromAccountID", SqlDbType.BigInt)).Value = FromAccountID;
                      

                        using (_da = new SqlDataAdapter(_cmd))
                        {
                            _ds = new DataSet();
                            _da.Fill(_ds, "Table");
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _ds;
        }

        public DataSet GetMergeRecord(clsMerge obj)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.CommandText = "GetMergeRecord";
                        _cmd.Connection = _conn;

                        _cmd.Parameters.Add(new SqlParameter("@FromCompanyID", SqlDbType.BigInt)).Value = obj.FromCompanyID;
                        _cmd.Parameters.Add(new SqlParameter("@ToCompanyID", SqlDbType.BigInt)).Value = obj.ToCompanyID;
                        _cmd.Parameters.Add(new SqlParameter("@FromProjectID", SqlDbType.BigInt)).Value = obj.FromProjectID;
                        _cmd.Parameters.Add(new SqlParameter("@ToProjectID", SqlDbType.BigInt)).Value = obj.ToProjectID;

                        _cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output });
                        using (_da = new SqlDataAdapter(_cmd))
                        {
                            _ds = new DataSet();
                            _da.Fill(_ds, "Table");
                        }
                        Message = _cmd.Parameters["@Message"].Value.ToString();
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _ds;
        }
        public string CreateMergeAccount(clsMerge obj) {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = "CreateMergeAccount";
                        _cmd.Parameters.Add(new SqlParameter("@FromAccountID", SqlDbType.BigInt)).Value = obj.FromAccountID;
                        _cmd.Parameters.Add(new SqlParameter("@ToAccountID", SqlDbType.BigInt)).Value = obj.ToAccountID;
                       

                        _cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = obj.CreatedDate;
                        _cmd.Parameters.Add(new SqlParameter("@CreatedUserID", SqlDbType.BigInt)).Value = obj.CreatedUserID;

                        _cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output });

                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();

                        int row = _cmd.ExecuteNonQuery();

                        Message = _cmd.Parameters["@Message"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }

            return Message;
        }
        public string CreateMerge(clsMerge obj)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = "CreateMerge";
                        _cmd.Parameters.Add(new SqlParameter("@FromCompanyID", SqlDbType.BigInt)).Value = obj.FromCompanyID;
                        _cmd.Parameters.Add(new SqlParameter("@ToCompanyID", SqlDbType.BigInt)).Value = obj.ToCompanyID;
                        _cmd.Parameters.Add(new SqlParameter("@FromProjectID", SqlDbType.BigInt)).Value = obj.FromProjectID;
                        _cmd.Parameters.Add(new SqlParameter("@ToProjectID", SqlDbType.BigInt)).Value = obj.ToProjectID;

                        _cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = obj.CreatedDate;
                        _cmd.Parameters.Add(new SqlParameter("@CreatedUserID", SqlDbType.BigInt)).Value = obj.CreatedUserID;

                        _cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar,500) { Direction = ParameterDirection.Output });

                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();

                        int row = _cmd.ExecuteNonQuery();

                        Message = _cmd.Parameters["@Message"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
            finally {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return Message;
        }
    }
}
