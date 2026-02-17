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
    public class BALChequeEntry
    {
        List<clsChequeEntry> list;
        List<SqlParameter> plist;
        clsChequeEntry obj;
        DataSet _ds;
        string Message = "";
        List<string> elist = null;
        public DataSet GetChequeEntryList(clsChequeEntrySearchParameter obj, List<string> elist = null, long UserId = 0)
        {
           
            plist = new List<SqlParameter>();
            // plist.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 100) { Value = GroupName });

            //if (UserId > 0)
            //{
            //    plist.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserId });
            //}
            plist = (new BALDynamicProperty().GetSQLParameter(obj, elist));
            _ds = (new DALChequeEntry().GetChequeEntryList("GetChequeEntry", plist));
            return _ds;
        }
        public List<clsChequeEntry> GetChequeEntryList2()
        {
            plist = new List<SqlParameter>();
            // plist.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 100) { Value = GroupName });
            list = (new DALChequeEntry().GetChequeEntryList2("GetChequeEntry", plist));
            return list;
        }
        public clsChequeEntry GetChequeEntry(long ChequeEntryID, long UserId = 0)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeEntryID", SqlDbType.BigInt) { Value = ChequeEntryID });
            if (UserId > 0)
            plist.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserId });
            obj = (new DALChequeEntry().GetChequeEntry("GetChequeEntry", plist));
            return obj;
        }

        public DataSet GetChequeList(long ChequeEntryID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeEntryID ", SqlDbType.BigInt) { Value = ChequeEntryID });
            
            _ds = new DataSet();
            _ds = (new DALDataAccess().GetDataSet("GetChequeList", plist));
            return _ds;
        }
        public DataSet GetChequeList(DateTime StartDate, DateTime EndDate, long ChequeStatusID, long BankID, string ChequeNo ,string DateType, long ChequeEntryID = 0)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = StartDate });
            plist.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = EndDate });

            if (ChequeStatusID > 0)
            { 
                plist.Add(new SqlParameter("@ChequeStatusID", SqlDbType.BigInt) { Value = ChequeStatusID });
            }
            if (BankID > 0)
            { 
                plist.Add(new SqlParameter("@BankID", SqlDbType.BigInt) { Value = BankID });
            }
            if (ChequeNo != "")
            {
                plist.Add(new SqlParameter("@ChequeNo", SqlDbType.NVarChar, 20) { Value = ChequeNo });
            }

            if (ChequeEntryID > 0)
            {
                plist.Add(new SqlParameter("@ChequeEntryID ", SqlDbType.BigInt) { Value = ChequeEntryID });
            }

            if (DateType != "")
            {

                plist.Add(new SqlParameter("@DateType", SqlDbType.NVarChar, 20) { Value = DateType });
            }
            else
            {
                DateType = "CCD";
                plist.Add(new SqlParameter("@DateType", SqlDbType.NVarChar, 20) { Value = DateType });
            }

            _ds = (new DALDataAccess().GetDataSet("GetChequeList", plist));
            return _ds;
        }
        public string ChangeChequeStatus(long ChequeEntryID, long ChequeStatusID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ChequeEntryID", SqlDbType.BigInt) { Value = ChequeEntryID });
            plist.Add(new SqlParameter("@ChequeStatusID", SqlDbType.BigInt) { Value = ChequeStatusID });
            plist.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 400) { Direction = ParameterDirection.Output });
            Message  = (new DALDataAccess().ExecuteNonQuery("ChangeChequeStatus", plist));
            return Message;
        }


        public DataTable ImportChequeEntry(
    DataTable dt,
    long companyId,
    long bankId,
    long userId,
    string fileName)
        {
            return new DALChequeEntry()
                .ImportChequeEntry(
                    dt,
                    companyId,
                    bankId,
                    userId,
                    fileName);
        }


        public List<ImportLogModel> GetImportLogList()
        {
            return new DALChequeEntry()
                .GetImportLogList();
        }
        public HashSet<ChequeDuplicateKey>   ValidateDuplicateCheque( List<ChequeImportModel> list, long BankID, long CompanyID)
        {
            DataTable dt = ToDataTable(list, BankID, CompanyID);

            return new DALChequeEntry()
                .ValidateDuplicateCheque(dt);
        }
        private DataTable ToDataTable( List<ChequeImportModel> list, long BankID, long CompanyID)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("EntryDate", typeof(DateTime));
            dt.Columns.Add("IssueDate", typeof(DateTime));
            dt.Columns.Add("ProjectID", typeof(long));
            dt.Columns.Add("AccountID", typeof(long));
            dt.Columns.Add("AccountSubName", typeof(string));
            dt.Columns.Add("BankID", typeof(long));
            dt.Columns.Add("ChequeNo", typeof(string));
            dt.Columns.Add("TypeID", typeof(long));
            dt.Columns.Add("SubTypeID", typeof(long));
            dt.Columns.Add("ParameterID", typeof(long));
            dt.Columns.Add("ChequeAmount", typeof(decimal));
            dt.Columns.Add("CompanyID", typeof(long));
            dt.Columns.Add("Narration", typeof(string));

            foreach (var item in list)
            {
                dt.Rows.Add(
                    item.EntryDate,
                    item.IssueDate,
                    item.ProjectID,
                    item.AccountID,
                    item.AccountName,
                   BankID,
                    item.ChequeNo,
                    item.TypeID,
                    item.SubTypeID,
                    item.ParameterID,
                    item.ChequeAmount,
                    CompanyID,
                    item.Narration
                );
            }

            return dt;
        }



    }
}
