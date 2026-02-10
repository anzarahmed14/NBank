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
    public class DALSubType:SQLObject
    {
        List<clsSubType> list;
        clsSubType obj;
        public List<clsSubType> GetSubTypeList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsSubType>();
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
                                    clsSubType obj = new clsSubType();
                                    obj.SubTypeID = NullReader.GetInt64("SubTypeID");
                                    obj.SubTypeCode = NullReader.GetString("SubTypeCode");
                                    obj.SubTypeName = NullReader.GetString("SubTypeName");
                                    obj.SubTypeShortName = NullReader.GetString("SubTypeShortName");
                                    obj.SubTypePrintName = NullReader.GetString("SubTypePrintName");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    obj.CreditDays = NullReader.GetInt32("CreditDays");
                                    // obj.ProjectID = NullReader.GetInt64("ProjectID");
                                    //obj.BankID = NullReader.GetInt64("BankID");


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

        public clsSubType GetSubType(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsSubType();
                                    obj.SubTypeID = NullReader.GetInt64("SubTypeID");
                                    obj.SubTypeCode = NullReader.GetString("SubTypeCode");
                                    obj.SubTypeName = NullReader.GetString("SubTypeName");
                                    obj.SubTypeShortName = NullReader.GetString("SubTypeShortName");
                                    obj.SubTypePrintName = NullReader.GetString("SubTypePrintName");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    obj.CreditDays = NullReader.GetInt32("CreditDays");
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
        
        public Dictionary<string, string> LoadSubTypes()
        {
            var dict = new Dictionary<string, string>();

            using (SqlConnection con =
                new SqlConnection(NBankConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT SubTypeShortName
              FROM SubTypeMaster
              ORDER BY SubTypeShortName ASC",
                    con))
                {
                    using (SqlDataReader dr =
                        cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string name =
                                dr["SubTypeShortName"]
                                .ToString()
                                .Trim()
                                .ToUpper();

                            if (!dict.ContainsKey(name))
                                dict.Add(name, name);
                        }
                    }
                }
            }

            return dict;
        }

    }
}
