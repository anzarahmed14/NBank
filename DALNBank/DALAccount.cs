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
   public class DALAccount:SQLObject
    {
        List<clsAccount> list;
        clsAccount obj;
        public List<clsAccount> GetAccountList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsAccount>();
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
                                    clsAccount obj = new clsAccount();
                                    obj.AccountID = NullReader.GetInt64("AccountID");
                                    obj.AccountCode = NullReader.GetString("AccountCode");
                                    obj.AccountName = NullReader.GetString("AccountName");
                                    obj.AccountShortName = NullReader.GetString("AccountShortName");
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

        public clsAccount  GetAccount(string StoredProcedure, List<SqlParameter> plist)
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
                            foreach (var p in plist) {
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
                                    obj = new clsAccount();
                                    obj.AccountID = NullReader.GetInt64("AccountID");
                                    obj.AccountCode = NullReader.GetString("AccountCode");
                                    obj.AccountName = NullReader.GetString("AccountName");
                                    obj.AccountShortName = NullReader.GetString("AccountShortName");
                                    obj.Address1 = NullReader.GetString("Address1");
                                    obj.Address2 = NullReader.GetString("Address2");
                                    obj.Address3 = NullReader.GetString("Address3");
                                    obj.CategoryID = NullReader.GetInt64("CategoryID");
                                    obj.CityName = NullReader.GetString("CityName");
                                    obj.ContactNo1 = NullReader.GetString("ContactNo1");
                                    obj.ContactNo2 = NullReader.GetString("ContactNo2");
                                    obj.ContactPersonName = NullReader.GetString("ContactPersonName");
                                    obj.CSTNo = NullReader.GetString("CSTNo");
                                    obj.FaxNo = NullReader.GetString("FaxNo");
                                    obj.Notes = NullReader.GetString("Notes");
                                    obj.OpeningBalance = NullReader.GetDouble("OpeningBalance");
                                    obj.PANNo = NullReader.GetString("PANNo");
                                    obj.PinCode = NullReader.GetString("PinCode");
                                    obj.Prefix = NullReader.GetString("Prefix");
                                    obj.StateID = NullReader.GetInt64("StateID");
                                    obj.VATNo = NullReader.GetString("VATNo");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");


                                }
                            }
                        }

                    }

                }

            }
            catch (Exception)
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


        public Dictionary<string, string> LoadAccounts()
        {
            var dict = new Dictionary<string, string>();

            using (SqlConnection con =
                new SqlConnection(NBankConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT AccountName FROM AccountMaster",
                    con);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string name =
                        dr["AccountName"]
                        .ToString()
                        .Trim()
                        .ToUpper();

                    if (!dict.ContainsKey(name))
                        dict.Add(name, name);
                }
            }

            return dict;
        }

    }
}
