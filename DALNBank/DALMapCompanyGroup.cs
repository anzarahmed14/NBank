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
    public class DALMapCompanyGroup : SQLObject
    {
        List<clsMapCompanyGroup> list;
        clsMapCompanyGroup obj;

        #region INSERT (Create)

        public string CreateMapCompanyGroup(
        string storedProcedure,
        DataTable companyIds,
        long companyGroupId)
        {
            string message = string.Empty;

            using (_conn = new SqlConnection(NBankConnectionString))
            using (_cmd = new SqlCommand(storedProcedure, _conn))
            {
                _cmd.CommandType = CommandType.StoredProcedure;

                _cmd.Parameters.AddWithValue("@CompanyGroupId", companyGroupId);

                SqlParameter tvp = _cmd.Parameters.AddWithValue("@CompanyIds", companyIds);
                tvp.SqlDbType = SqlDbType.Structured;
                tvp.TypeName = "dbo.CompanyIdList";

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

        #endregion

        #region UPDATE
        public string UpdateMapCompanyGroup(
            string storedProcedure,
            DataTable companyIds,
            long companyGroupId)
        {
            string message = string.Empty;

            using (_conn = new SqlConnection(NBankConnectionString))
            using (_cmd = new SqlCommand(storedProcedure, _conn))
            {
                _cmd.CommandType = CommandType.StoredProcedure;

                // ✅ EXACT PARAMETER NAME
                _cmd.Parameters.AddWithValue("@CompanyGroupId", companyGroupId);

                // ✅ EXACT TVP NAME
                SqlParameter tvp = _cmd.Parameters.AddWithValue("@CompanyIds", companyIds);
                tvp.SqlDbType = SqlDbType.Structured;
                tvp.TypeName = "dbo.CompanyIdList";

                // ✅ OUTPUT PARAM
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


        #endregion

        #region GET LIST

        public List<clsMapCompanyGroup> GetMapCompanyGroupList(
            string StoredProcedure,
            List<SqlParameter> plist)
        {
            try
            {
                list = new List<clsMapCompanyGroup>();

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
                                _cmd.Parameters.Add(p);
                        }

                        using (_reader = _cmd.ExecuteReader())
                        {
                            NullReader = new NullDataReader(_reader);

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    obj = new clsMapCompanyGroup();
                                    obj.MapCompanyGroupId = NullReader.GetInt64("MapCompanyGroupId");
                                    obj.CompanyGroupId = NullReader.GetInt64("CompanyGroupId");
                                    obj.CompanyId = NullReader.GetInt64("CompanyId");
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

        #endregion

        public List<clsCompanyGroupList> GetCompanyGroupSummary(
    string StoredProcedure,
    List<SqlParameter> plist)
        {
            List<clsCompanyGroupList> list = new List<clsCompanyGroupList>();

            using (_conn = new SqlConnection(NBankConnectionString))
            using (_cmd = new SqlCommand(StoredProcedure, _conn))
            {
                _cmd.CommandType = CommandType.StoredProcedure;

                if (plist.Count > 0)
                    _cmd.Parameters.AddRange(plist.ToArray());

                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();

                using (_reader = _cmd.ExecuteReader())
                {
                    NullReader = new NullDataReader(_reader);

                    while (_reader.Read())
                    {
                        list.Add(new clsCompanyGroupList
                        {
                            CompanyGroupID = NullReader.GetInt64("CompanyGroupID"),
                            CompanyGroupName = NullReader.GetString("CompanyGroupName"),
                        
                            CompanyCount = NullReader.GetInt32("CompanyCount"),

                            CompanyNames =  NullReader.GetString("CompanyNames"),
                        });
                    }
                }
            }
            return list;
        }


        public List<clsMapCompanyGroup> GetByCompanyGroupId(
                 string storedProcedure,
                 List<SqlParameter> plist)
        {
            try
            {
                list = new List<clsMapCompanyGroup>();

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
                                    obj = new clsMapCompanyGroup();
                                    obj.MapCompanyGroupId = NullReader.GetInt64("MapCompanyGroupId");
                                    obj.CompanyGroupId = NullReader.GetInt64("CompanyGroupId");
                                    obj.CompanyId = NullReader.GetInt64("CompanyId");

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


    }

}
