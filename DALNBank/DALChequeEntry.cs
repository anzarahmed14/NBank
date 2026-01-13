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
    public class DALChequeEntry:SQLObject
    {
        List<clsChequeEntry> list;
        clsChequeEntry obj;
        DataSet _ds;
        public DataSet  GetChequeEntryList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        if (plist.Count > 0)
                        {
                            foreach (var p in plist)
                            {
                                _cmd.Parameters.Add(p);

                            }
                        }
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedure;
                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();
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

        public List<clsChequeEntry> GetChequeEntryList2(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsChequeEntry>();
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
                                    clsChequeEntry obj = new clsChequeEntry();
                                    obj.ChequeEntryID = NullReader.GetInt64("ChequeEntryID");
                                    //obj.ChequeEntryCode = NullReader.GetString("ChequeEntryCode");
                                    //obj.ChequeEntryName = NullReader.GetString("ChequeEntryName");
                                    //obj.ChequeEntryShortName = NullReader.GetString("ChequeEntryShortName");
                                    //obj.IsActive = NullReader.GetBoolean("IsActive");
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

        public clsChequeEntry GetChequeEntry(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsChequeEntry();
                                    obj.ChequeEntryID   = NullReader.GetInt64("ChequeEntryID");
                                    obj.ChequeEntryDate = NullReader.GetDateTime("ChequeEntryDate");
                                    obj.ProjectID       = NullReader.GetInt64("ProjectID");
                                    obj.AccountID       = NullReader.GetInt64("AccountID");
                                    obj.AccountSubName  = NullReader.GetString("AccountSubName");
                                    obj.BankID          = NullReader.GetInt64("BankID");
                                    obj.ChequeNo        = NullReader.GetString("ChequeNo");
                                    obj.TypeID          = NullReader.GetInt64("TypeID");
                                    obj.SubTypeID       = NullReader.GetInt64("SubTypeID");
                                    obj.ParameterID     = NullReader.GetInt64("ParameterID");
                                    obj.ChequeTypeID    = NullReader.GetInt64("ChequeTypeID");
                                    obj.ChequeStatusID  = NullReader.GetInt64("ChequeStatusID");
                                    obj.ChequeIssueDate = NullReader.GetDateTime("ChequeIssueDate");
                                    obj.ChequeClearDate = NullReader.GetDateTime("ChequeClearDate");
                                    obj.ChequeAmount    = NullReader.GetDouble ("ChequeAmount");
                                    obj.ChequeAmountTDS = NullReader.GetDouble("ChequeAmountTDS");
                                    obj.Narration       = NullReader.GetString("Narration");
                                    obj.CompanyID       = NullReader.GetInt64("CompanyID");
                                    obj.ERPID           = NullReader.GetString("ERPID");

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
