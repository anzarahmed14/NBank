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
    public class DALCompany:SQLObject
    {
        List<clsCompany> list;
        clsCompany obj;
        public List<clsCompany> GetCompanyList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsCompany>();
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
                                    clsCompany obj = new clsCompany();
                                    obj.CompanyID = NullReader.GetInt64("CompanyID");
                                    obj.CompanyCode = NullReader.GetString("CompanyCode");
                                    obj.CompanyName = NullReader.GetString("CompanyName");
                                    obj.CompanyShortName = NullReader.GetString("CompanyShortName");
                                    obj.ProjectID = NullReader.GetInt64("ProjectID");
                                    obj.BankID = NullReader.GetInt64("BankID");
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

        public clsCompany GetCompany(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsCompany();
                                    obj.CompanyID = NullReader.GetInt64("CompanyID");
                                    obj.CompanyCode = NullReader.GetString("CompanyCode");
                                    obj.CompanyName = NullReader.GetString("CompanyName");
                                    obj.CompanyShortName = NullReader.GetString("CompanyShortName");
                                    obj.ProjectID = NullReader.GetInt64("ProjectID");
                                    obj.BankID = NullReader.GetInt64("BankID");
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
    }
}
