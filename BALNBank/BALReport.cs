using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BALNBank
{
    public class BALReport
    {
        DataSet _ds;
        List<SqlParameter> list;
        public DataSet GetReport(string DateType, 
            DateTime StartDate, 
            DateTime EndDate, 
            long ChequeStatusID, 
            long BankID, 
            string ChequeNo, 
            long AccountID,
            //long ChequeEntryID,
            long CompanyID,
            long ParameterID,
            long ProjectID,
            long ChequeTypeID,
            long SubTypeID,
            long TypeID,
            string AccountSubName,
            string ERPID,
            long UserId)
        {
            _ds = new DataSet();
            list = new List<SqlParameter>();
            if (DateType !="")
                list.Add(new SqlParameter("@DateType",       SqlDbType.NVarChar, 20) { Value = DateType });
            else
                list.Add(new SqlParameter("@DateType", SqlDbType.NVarChar, 20) { Value = "CED" }); /*CED =  Cheque Entry Date*/
            if (StartDate != null)
                list.Add(new SqlParameter("@StartDate",      SqlDbType.DateTime) { Value = StartDate });
            if (EndDate != null)
                list.Add(new SqlParameter("@EndDate",        SqlDbType.DateTime) { Value = EndDate });
            if (ChequeStatusID >0)
                list.Add(new SqlParameter("@ChequeStatusID", SqlDbType.BigInt) { Value = ChequeStatusID });
            if (BankID > 0)
                list.Add(new SqlParameter("@BankID",         SqlDbType.BigInt) { Value = BankID });
            if (ChequeNo != "")
                list.Add(new SqlParameter("@ChequeNo",       SqlDbType.NVarChar,20) { Value = ChequeNo });
            if (AccountID > 0)
                list.Add(new SqlParameter("@AccountID",      SqlDbType.BigInt) { Value = AccountID });
           // if (ChequeEntryID > 0)
             //   list.Add(new SqlParameter("@ChequeEntryID",  SqlDbType.BigInt) { Value = ChequeEntryID });
            if (CompanyID > 0)
                list.Add(new SqlParameter("@CompanyID",      SqlDbType.BigInt) { Value = CompanyID });
            if (ParameterID > 0)
                list.Add(new SqlParameter("@ParameterID",    SqlDbType.BigInt) { Value = ParameterID });
            if (ProjectID > 0)
                list.Add(new SqlParameter("@ProjectID",      SqlDbType.BigInt) { Value = ProjectID });
            if (ChequeTypeID > 0)
                list.Add(new SqlParameter("@ChequeTypeID",   SqlDbType.BigInt) { Value = ChequeTypeID });
            if (SubTypeID > 0)
                list.Add(new SqlParameter("@SubTypeID",      SqlDbType.BigInt) { Value = SubTypeID });
            if (TypeID > 0)
                list.Add(new SqlParameter("@TypeID",         SqlDbType.BigInt) { Value = TypeID });
            if (AccountSubName != "")
                list.Add(new SqlParameter("@AccountSubName", SqlDbType.NVarChar,100) { Value = AccountSubName });
            if (ERPID != "")
                list.Add(new SqlParameter("@ERPID", SqlDbType.NVarChar, 4000) { Value = ERPID });
            if (UserId > 0)
                list.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserId });

            _ds = (new DALNBank.DALDataAccess().GetDataSet("GetChequeReport", list));
            return _ds;
        }

    }
}
