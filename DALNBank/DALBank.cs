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
    public class DALBank : SQLObject
    {
        List<clsBank> list;
        clsBank obj;
        public List<clsBank> GetBankList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsBank>();
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
                                    clsBank obj = new clsBank();
                                    obj.BankID = NullReader.GetInt64("BankID");
                                    obj.BankCode = NullReader.GetString("BankCode");
                                    obj.BankName = NullReader.GetString("BankName");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    list.Add(obj);
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception )
            {

                throw ;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return list;
        }

        public clsBank GetBank(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsBank();
                                    obj.BankID = NullReader.GetInt64("BankID");
                                    obj.BankCode = NullReader.GetString("BankCode");
                                    obj.BankName = NullReader.GetString("BankName");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception )
            {

                throw ;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return obj;
        }

        public Dictionary<string, string> LoadBanks()
        {
            var dict = new Dictionary<string, string>();

            using (SqlConnection con =
                new SqlConnection(NBankConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT BankCode
              FROM BankMaster
              ORDER BY BankCode ASC",
                    con))
                {
                    using (SqlDataReader dr =
                        cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string code =
                                dr["BankCode"]
                                .ToString()
                                .Trim()
                                .ToUpper();

                            if (!dict.ContainsKey(code))
                                dict.Add(code, code);
                        }
                    }
                }
            }

            return dict;
        }

    }
}
