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
    public   class DALProject:SQLObject
    {

        List<clsProject> list;
        clsProject obj;
        public List<clsProject> GetProjectList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsProject>();
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
                                    clsProject obj = new clsProject();
                                    obj.ProjectID = NullReader.GetInt64("ProjectID");
                                    obj.ProjectCode = NullReader.GetString("ProjectCode");
                                    obj.ProjectName = NullReader.GetString("ProjectName");
                                    obj.ProjectShortName = NullReader.GetString("ProjectShortName");
                                    obj.SquareFit = NullReader.GetDouble("SquareFit");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    obj.CompanyID = NullReader.GetInt64("CompanyID");
                                    obj.CompanyName = NullReader.GetString("CompanyName");
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

        public clsProject GetProject(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsProject();
                                    obj.ProjectID = NullReader.GetInt64("ProjectID");
                                    obj.ProjectCode = NullReader.GetString("ProjectCode");
                                    obj.ProjectName = NullReader.GetString("ProjectName");
                                    obj.ProjectShortName = NullReader.GetString("ProjectShortName");
                                    obj.SquareFit = NullReader.GetDouble("SquareFit");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    obj.CompanyID = NullReader.GetInt64("CompanyID");
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
    }
}
