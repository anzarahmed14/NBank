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
    public  class DALCompanyWiseBank:SQLObject
    {
        List<clsCompanyWiseBank> list;
        clsCompanyWiseBank obj;
        public List<clsCompanyWiseBank> GetCompanyWiseBankList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsCompanyWiseBank>();
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
                                    clsCompanyWiseBank obj = new clsCompanyWiseBank();
                                    obj.CompanyWiseBankID = NullReader.GetInt64("CompanyWiseBankID");

                                    obj.CompanyShortName = NullReader.GetString("CompanyShortName");
                                    obj.BankName = NullReader.GetString("BankName");
                                    obj.CompanyName = NullReader.GetString("CompanyName");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    obj.BankID = NullReader.GetInt64("BankID");
                                    obj.CompanyID = NullReader.GetInt64("CompanyID");
                                    obj.BankName = NullReader.GetString("BankName");
                                    obj.LastChequeNo = NullReader.GetString("LastChequeNo");
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

        public clsCompanyWiseBank GetCompanyWiseBank(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsCompanyWiseBank();
                                    obj.CompanyWiseBankID = NullReader.GetInt64("CompanyWiseBankID");
                                    obj.CompanyShortName = NullReader.GetString("CompanyShortName");
                                    obj.BankName = NullReader.GetString("BankName");
                                    obj.CompanyName = NullReader.GetString("CompanyName");
                                    obj.BankID = NullReader.GetInt64("BankID");
                                    obj.CompanyID = NullReader.GetInt64("CompanyID");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    obj.LastChequeNo = NullReader.GetString("LastChequeNo");


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
