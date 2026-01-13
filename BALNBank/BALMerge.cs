using BOLNBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALNBank;
using System.Data;

namespace BALNBank
{
    public class BALMerge
    {
        string Message = "";
        DataSet _ds;
        public string CreateMerge(clsMerge obj)
        {
            Message = (new DALMerge().CreateMerge(obj));

            return Message;
        }
        public string CreateMergeAccount(clsMerge obj) {
            Message = (new DALMerge().CreateMergeAccount(obj));

            return Message;
        }
        public DataSet GetMergeRecord(clsMerge obj) {
           _ds = new DataSet();
           _ds = (new DALMerge().GetMergeRecord(obj));
            return _ds;
        }
        public DataSet GetMergeRecordAccount(long AccountID)
        {
            _ds = new DataSet();
            _ds = (new DALMerge().GetMergeRecordAccount(AccountID));
            return _ds;
        }
    }
}
