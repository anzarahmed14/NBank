using BOLNBank;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALNBank
{
    public  class DALState :SQLObject
    {
        List<clsState> list;
        clsState obj;
       
        public clsState GetState(string StoredProcedure, List<SqlParameter> plist)
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
                                    obj = new clsState();
                                    obj.StateID = NullReader.GetInt64("StateID");
                                    obj.StateName = NullReader.GetString("StateName");
                                    obj.StateShortName = NullReader.GetString("StateShortName");
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
        public List<clsState> GetState(string StoredProcedureName)
        {
            try
            {

                list = new List<clsState>();
                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = StoredProcedureName;
                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open ();
                        
                        using (_reader = _cmd.ExecuteReader())
                        {

                            NullReader = new NullDataReader(_reader);

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    clsState obj = new clsState();
                                    obj.StateID = NullReader.GetInt64("StateID");
                                    obj.StateName = NullReader.GetString("StateName");
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
            finally {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
            return list;
        }

        public IEnumerable<dynamic> GetState2(string StoredProcedureName)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;
            

                using (_conn = new SqlConnection(NBankConnectionString))
                {
                    using (_cmd = new SqlCommand())
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Connection = _conn;
                        _cmd.CommandText = "GetStateMaster";
                        if (_conn.State == ConnectionState.Closed)
                            _conn.Open();

                        using (_reader = _cmd.ExecuteReader())
                        {

                            var names = Enumerable.Range(0, _reader.FieldCount).Select(_reader.GetName).ToList();
                            foreach (IDataRecord record in _reader as IEnumerable)
                            {
                               
                                foreach (var name in names)
                                    expando[name] = record[name];

                                yield return expando;
                            }
                        }

                    }

                }

            
           
          
           
            //  return list;
        }

        public List<clsState> GetStateList(string StoredProcedure, List<SqlParameter> plist)
        {
            try
            {

                list = new List<clsState>();
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
                                    clsState obj = new clsState();
                                    obj.StateID = NullReader.GetInt64("StateID");
                                    obj.StateShortCode = NullReader.GetString("StateShortCode");
                                    obj.StateName = NullReader.GetString("StateName");
                                    obj.StateShortName = NullReader.GetString("StateShortName");
                                    obj.IsActive = NullReader.GetBoolean("IsActive");
                                    obj.StateNo = NullReader.GetInt32("StateNo");
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
    }
}
