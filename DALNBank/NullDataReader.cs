using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALNBank
{
    public sealed class NullDataReader
    {
        private DateTime defaultDate;
        public NullDataReader(SqlDataReader reader)
        {
            this.defaultDate = DateTime.MinValue;
            this.reader = reader;
        }

        public int GetInt32(String column)
        {
            int data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                    ? (int)0 : (int)reader[column];
            return data;
        }

        public long GetInt64(String column)
        {
            long data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                    ? (long)0 : (long)reader[column];
            return data;
        }


        public short GetInt16(String column)
        {
            short data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                  ? (short)0 : (short)reader[column];
            return data;
        }

        public float GetFloat(String column)
        {
            float data = (reader.IsDBNull(reader.GetOrdinal(column)))
                        ? 0 : float.Parse(reader[column].ToString());
            return data;
        }
        public decimal GetDecimal(String column)
        {
            decimal data = (reader.IsDBNull(reader.GetOrdinal(column)))
                        ? 0 : decimal.Parse(reader[column].ToString());
            return data;
        }
        public double GetDouble(String column)
        {
            double data = (reader.IsDBNull(reader.GetOrdinal(column)))
                        ? 0 : double.Parse(reader[column].ToString());
            return data;
        }

        public bool GetBoolean(String column)
        {
            bool data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                     ? false : (bool)reader[column];
            return data;
        }

        public String GetString(String column)
        {
            String data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                   ? null : reader[column].ToString();
            return data;
        }

        public DateTime GetDateTime(String column)
        {
            DateTime data = (reader.IsDBNull(reader.GetOrdinal(column)))
                               ? defaultDate : (DateTime)reader[column];
            return data;
        }

        public bool Read()
        {
            return this.reader.Read();
        }
        private SqlDataReader reader;
    }
}
