using BOLNBank;
using DALNBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALNBank
{
    
        public class BALCompanyWiseBank
        {
            List<clsCompanyWiseBank> list;
            List<SqlParameter> plist;
            clsCompanyWiseBank obj;
            public List<clsCompanyWiseBank> GetCompanyWiseBankList(string CompanyWiseBankName = "")
            {
                plist = new List<SqlParameter>();
                plist.Add(new SqlParameter("@CompanyWiseBankName", SqlDbType.NVarChar, 100) { Value = CompanyWiseBankName });

                list = (new DALCompanyWiseBank().GetCompanyWiseBankList("GetCompanyWiseBank", plist));
                return list;
            }
            public clsCompanyWiseBank GetCompanyWiseBank(long CompanyWiseBankID)
            {
                plist = new List<SqlParameter>();
                plist.Add(new SqlParameter("@CompanyWiseBankID", SqlDbType.BigInt) { Value = CompanyWiseBankID });
                obj = (new DALCompanyWiseBank().GetCompanyWiseBank("GetCompanyWiseBank", plist));
                return obj;
            }
        }
    }

