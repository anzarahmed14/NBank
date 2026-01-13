using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALNBank
{
    public class DALDataAccess : SQLObject
    {


        /// <summary>
        /// Wiht One Output parameter Message
        /// </summary>
        /// <param name="StoredProcedureName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ExecuteNonQuery(string StoredProcedureName, List<SqlParameter> list)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedureName;

                        foreach (SqlParameter para in list)
                        {
                            _cmd.Parameters.Add(para);
                        }
                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();
                        row = _cmd.ExecuteNonQuery();

                        foreach (SqlParameter para in list)
                        {
                            if (para.Direction == ParameterDirection.Output)
                            {
                                Message = Convert.ToString(((SqlParameter)_cmd.Parameters[para.ParameterName]).Value);
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
                //throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return Message;
        }
        public DataSet GetPagingDataSet(string StoredProcedureName, int PageIndex, int PageSize, List<SqlParameter> list)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedureName;

                        foreach (SqlParameter para in list)
                        {
                            _cmd.Parameters.Add(para);
                        }

                        _cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex;
                        _cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;

                        _cmd.Parameters.Add("@RecordCount", SqlDbType.NVarChar, 100);
                        ((SqlParameter)_cmd.Parameters["@RecordCount"]).Direction = ParameterDirection.Output;

                        using (_da = new SqlDataAdapter(_cmd))
                        {
                            _ds = new DataSet();
                            _da.Fill(_ds, "Table");
                            _dt = new DataTable("Pager");
                            _dt.Columns.Add("PageIndex");
                            _dt.Columns.Add("PageSize");
                            _dt.Columns.Add("RecordCount");
                            _dt.Rows.Add();
                            _dt.Rows[0]["PageIndex"] = PageIndex;
                            _dt.Rows[0]["PageSize"] = PageSize;
                            _dt.Rows[0]["RecordCount"] = _cmd.Parameters["@RecordCount"].Value;
                            _ds.Tables.Add(_dt);

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
        /// <summary>
        /// Get Paging for all record 
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GetPagingDataSet(string StoredProcedureName, int PageIndex, int PageSize)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedureName;

                        _cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex;
                        _cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;

                        _cmd.Parameters.Add("@RecordCount", SqlDbType.NVarChar, 100);
                        ((SqlParameter)_cmd.Parameters["@RecordCount"]).Direction = ParameterDirection.Output;

                        using (_da = new SqlDataAdapter(_cmd))
                        {
                            _ds = new DataSet();
                            _da.Fill(_ds, "Table");
                            _dt = new DataTable("Pager");
                            _dt.Columns.Add("PageIndex");
                            _dt.Columns.Add("PageSize");
                            _dt.Columns.Add("RecordCount");
                            _dt.Rows.Add();
                            _dt.Rows[0]["PageIndex"] = PageIndex;
                            _dt.Rows[0]["PageSize"] = PageSize;
                            _dt.Rows[0]["RecordCount"] = _cmd.Parameters["@RecordCount"].Value;
                            _ds.Tables.Add(_dt);

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

        public DataSet GetDataSet(string StoredProcedureName, List<SqlParameter> list)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedureName;

                        if (list.Count > 0)
                        {
                            foreach (SqlParameter para in list)
                            {
                                _cmd.Parameters.Add(para);
                            }
                        }

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
    }
}
