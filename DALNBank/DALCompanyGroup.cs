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
    public class DALCompanyGroup : SQLObject
    {
        List<clsCompanyGroup> list;
        clsCompanyGroup obj;
        public List<clsCompanyGroup> GetCompanyGroupList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsCompanyGroup>();
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
                                    clsCompanyGroup obj = new clsCompanyGroup();
                                    obj.CompanyGroupID = NullReader.GetInt64("CompanyGroupID");
                                    obj.CompanyGroupCode = NullReader.GetString("CompanyGroupCode");
                                    obj.CompanyGroupName = NullReader.GetString("CompanyGroupName");
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

        public clsCompanyGroup GetCompanyGroup(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsCompanyGroup();
                                    obj.CompanyGroupID = NullReader.GetInt64("CompanyGroupID");
                                    obj.CompanyGroupCode = NullReader.GetString("CompanyGroupCode");
                                    obj.CompanyGroupName = NullReader.GetString("CompanyGroupName");
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
