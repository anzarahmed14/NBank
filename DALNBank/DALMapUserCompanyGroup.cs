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
    public class DALMapUserCompanyGroup :SQLObject
    {
        List<clsUserCompanyGroupMapping> list;

        List<clsMapUserCompanyGroup>  elist;

        clsMapUserCompanyGroup obj;

        public List<clsUserCompanyGroupMapping> GetList(
       string storedProcedure,
       List<SqlParameter> plist)
        {
            try
            {
                list = new List<clsUserCompanyGroupMapping>();

                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = storedProcedure;

                        if (plist != null && plist.Count > 0)
                        {
                            foreach (var p in plist)
                                _cmd.Parameters.Add(p);
                        }

                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();

                        using (_reader = _cmd.ExecuteReader())
                        {
                            NullReader = new NullDataReader(_reader);

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    clsUserCompanyGroupMapping obj =
                                        new clsUserCompanyGroupMapping();

                                    obj.UserId =
                                        NullReader.GetInt64("UserId");

                                    obj.UserName =
                                        NullReader.GetString("UserName");

                                    obj.CompanyGroupCount =
                                        NullReader.GetInt32("CompanyGroupCount");

                                    list.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
            catch
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


        public string Create(
        string spName,
        long userId,
        DataTable companyGroupIds)
        {
            string message = "";

            using (_conn = new SqlConnection(NBankConnectionString))
            using (_cmd = new SqlCommand(spName, _conn))
            {
                _cmd.CommandType = CommandType.StoredProcedure;

                _cmd.Parameters.AddWithValue("@UserId", userId);

                SqlParameter tvp = _cmd.Parameters.AddWithValue("@CompanyGroupIds", companyGroupIds);
                tvp.SqlDbType = SqlDbType.Structured;
                tvp.TypeName = "dbo.CompanyGroupIdList";

                SqlParameter msg = new SqlParameter("@Message", SqlDbType.NVarChar, 4000)
                {
                    Direction = ParameterDirection.Output
                };
                _cmd.Parameters.Add(msg);

                _conn.Open();
                _cmd.ExecuteNonQuery();

                message = Convert.ToString(msg.Value);
            }

            return message;
        }

        public List<clsMapUserCompanyGroup> GetByUserId(
            string spName,
            long userId)
        {
            List<clsMapUserCompanyGroup> list = new List<clsMapUserCompanyGroup>();

            using (_conn = new SqlConnection(NBankConnectionString))
            using (_cmd = new SqlCommand(spName, _conn))
            {
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Parameters.AddWithValue("@UserId", userId);

                _conn.Open();
                using (_reader = _cmd.ExecuteReader())
                {
                    NullReader = new NullDataReader(_reader);

                    while (_reader.Read())
                    {
                        list.Add(new clsMapUserCompanyGroup
                        {
                            MapUserCompanyGroupId = NullReader.GetInt64("MapUserCompanyGroupId"),
                            UserId = NullReader.GetInt64("UserId"),
                            CompanyGroupId = NullReader.GetInt64("CompanyGroupId")
                        });
                    }
                }
            }

            return list;
        }


        public string Update(
    string spName,
    long userId,
    DataTable companyGroupIds)
        {
            string message = "";

            using (_conn = new SqlConnection(NBankConnectionString))
            using (_cmd = new SqlCommand(spName, _conn))
            {
                _cmd.CommandType = CommandType.StoredProcedure;

                _cmd.Parameters.AddWithValue("@UserId", userId);

                SqlParameter tvp =
                    _cmd.Parameters.AddWithValue("@CompanyGroupIds", companyGroupIds);
                tvp.SqlDbType = SqlDbType.Structured;
                tvp.TypeName = "dbo.CompanyGroupIdList";

                SqlParameter msg =
                    new SqlParameter("@Message", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                _cmd.Parameters.Add(msg);

                _conn.Open();
                _cmd.ExecuteNonQuery();

                message = Convert.ToString(msg.Value);
            }

            return message;
        }



        public List<clsMapUserCompanyGroup> GetCompanyGroupByUserId(
               string storedProcedure,
               List<SqlParameter> plist)
        {
            try
            {
                elist = new List<clsMapUserCompanyGroup>();

                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = storedProcedure;

                        if (plist.Count > 0)
                        {
                            foreach (var p in plist)
                                _cmd.Parameters.Add(p);
                        }

                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();

                        using (_reader = _cmd.ExecuteReader())
                        {
                            NullReader = new NullDataReader(_reader);

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    obj = new clsMapUserCompanyGroup();
                                    obj.MapUserCompanyGroupId = NullReader.GetInt64("MapUserCompanyGroupId");
                                    obj.UserId = NullReader.GetInt64("UserId");
                                    obj.CompanyGroupId = NullReader.GetInt64("CompanyGroupId");

                                    elist.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }

            return elist;
        }
    }
}
