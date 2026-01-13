using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALNBank
{
    public class SQLObject
    {
        protected DataTable _dt;
        protected DataRow dataRow;
        protected string Message = "";
        protected int ErrorNumber = 0;
        protected DataSet _ds;
        protected SqlDataAdapter _da;
        protected SqlDataReader _reader;
        protected SqlCommand _cmd;
        protected SqlConnection _conn;
        protected SqlParameter _para;
        protected SqlParameter[] _listPara;
        protected NullDataReader NullReader;
        protected string NBankConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NBankConnectionString"].ConnectionString;
        protected object row;
    }
}
