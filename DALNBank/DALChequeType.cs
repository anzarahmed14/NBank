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
    public class DALChequeType:SQLObject
    {
        List<clsChequeType> list;
        clsChequeType obj;
        public List<clsChequeType> GetChequeTypeList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsChequeType>();
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
                                    clsChequeType obj = new clsChequeType();
                                    obj.ChequeTypeID = NullReader.GetInt64("ChequeTypeID");
                                    obj.ChequeTypeCode = NullReader.GetString("ChequeTypeCode");
                                    obj.ChequeTypeName = NullReader.GetString("ChequeTypeName");
                                  //  obj.ChequeTypeShortName = NullReader.GetString("ChequeTypeShortName");
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

        public clsChequeType GetChequeType(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsChequeType();
                                    obj.ChequeTypeID = NullReader.GetInt64("ChequeTypeID");
                                    obj.ChequeTypeCode = NullReader.GetString("ChequeTypeCode");
                                    obj.ChequeTypeName = NullReader.GetString("ChequeTypeName");
                                    //obj.ChequeTypeShortName = NullReader.GetString("ChequeTypeShortName");
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
