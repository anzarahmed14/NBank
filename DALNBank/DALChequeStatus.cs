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
    public class DALChequeStatus:SQLObject
    {
        List<clsChequeStatus> list;
        clsChequeStatus obj;
        public List<clsChequeStatus> GetChequeStatusList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsChequeStatus>();
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
                                    clsChequeStatus obj = new clsChequeStatus();
                                    obj.ChequeStatusID = NullReader.GetInt64("ChequeStatusID");
                                    obj.ChequeStatusCode = NullReader.GetString("ChequeStatusCode");
                                    obj.ChequeStatusName = NullReader.GetString("ChequeStatusName");
                                    obj.ChequeStatusShortName = NullReader.GetString("ChequeStatusShortName");
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

        public clsChequeStatus GetChequeStatus(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsChequeStatus();
                                    obj.ChequeStatusID = NullReader.GetInt64("ChequeStatusID");
                                    obj.ChequeStatusCode = NullReader.GetString("ChequeStatusCode");
                                    obj.ChequeStatusName = NullReader.GetString("ChequeStatusName");
                                    obj.ChequeStatusShortName = NullReader.GetString("ChequeStatusShortName");
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
