using BALNBank;
using BOLNBank;
using NBank.Other;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace NBank.Report
{
    /// <summary>
    /// Interaction logic for ReportFilter.xaml
    /// </summary>
    public partial class ReportFilter : Window
    {

        List<clsBank> objBankList;
        List<clsProject> objProjectList;
        List<clsCompany> objCompanyList;
        List<clsType> objTypeList;
        List<clsSubType> objSubTypeList;
        List<clsParameter> objParameterList;
        List<clsChequeStatus> objChequeStatusList;
        //List<clsAccount> objAccountList;
        List<clsChequeType> objChequeTypeList;
        List<clsCompanyWiseBank> objCompanyWiseBankList;

     
        internal string AccountName = "";
        internal long AccountID;

        DateTime StartDate, EndDate;
       
        string DateType, ChequeNo, AccountSubName, ERPID;
        long ChequeEntryID = 0, ParameterID = 0, ChequeTypeID = 0, SubTypeID = 0, TypeID = 0, BankID = 0, ChequeStatusID= 0, CompanyID, ProjectID = 0;

        DataSet _ds;
        string Message = "";
        string MessageTitle = "Report";
        string Path = Environment.CurrentDirectory;
        string FileName = "";
        string ReportName = "";
        public Dictionary<string, string> ParaField = null;
        string crName = "";

        string ExportcolumnName ="";
        string ExportcolumnValue = "";
        public ReportFilter()
        {
            InitializeComponent();
        }

        private void frmReportFilter_Loaded(object sender, RoutedEventArgs e)
        {
          

            objCompanyList = (new BALCompany().GetCompanyList());

            objCompanyList.Insert(0, new clsCompany() { CompanyID = -1, CompanyShortName = "--Select Company--" });

            cmbCompanyName.ItemsSource = objCompanyList;
            cmbCompanyName.DisplayMemberPath = "CompanyShortName";
            cmbCompanyName.SelectedValuePath = "CompanyID";
            cmbCompanyName.SelectedValue = -1;


            objTypeList = (new BALType().GetTypeList());
            objTypeList.Insert(0, new clsType() { TypeID = -1, TypeShortName = "--Select Type--" });

            cmbTypeName.ItemsSource = objTypeList;
            cmbTypeName.DisplayMemberPath = "TypeShortName";
            cmbTypeName.SelectedValuePath = "TypeID";
            cmbTypeName.SelectedValue = -1;


            /*SubType */
            objSubTypeList = (new BALSubType().GetSubTypeList());

            objSubTypeList.Insert(0, new clsSubType() { SubTypeID = -1, SubTypeShortName = "--Select SubType--" });
            cmbSubTypeName.ItemsSource = objSubTypeList;
            cmbSubTypeName.DisplayMemberPath = "SubTypeShortName";
            cmbSubTypeName.SelectedValuePath = "SubTypeID";
            cmbSubTypeName.SelectedValue = -1;


            /*Parameter */
            objParameterList = (new BALParameter().GetParameterList());
            objParameterList.Insert(0, new clsParameter() { ParameterID = -1, ParameterShortName = "--Select Parameter--" });
            cmbParameterName.ItemsSource = objParameterList;
            cmbParameterName.DisplayMemberPath = "ParameterShortName";
            cmbParameterName.SelectedValuePath = "ParameterID";
            cmbParameterName.SelectedValue = -1;

            /*Bank*/
            objBankList = (new BALBank().GetBankList());
            objBankList.Insert(0, new clsBank() { BankID = -1, BankName = "--Select Bank--" });
            cmbBankName.ItemsSource = objBankList;
            cmbBankName.DisplayMemberPath = "BankName";
            cmbBankName.SelectedValuePath = "BankID";
            cmbBankName.SelectedValue = -1;

            /*Status*/
            objChequeStatusList = (new BALChequeStatus().GetChequeStatusList());
            objChequeStatusList.Insert(0, new clsChequeStatus() { ChequeStatusID = -1, ChequeStatusName = "--Select Status--" });
            cmbChequeStatus.ItemsSource = objChequeStatusList;
            cmbChequeStatus.DisplayMemberPath = "ChequeStatusName";
            cmbChequeStatus.SelectedValuePath = "ChequeStatusID";
            cmbChequeStatus.SelectedValue = -1;
            //  cmbChequeStatus.SelectedIndex = 2;

            /*Cheque Type Name*/
            objChequeTypeList = (new BALChequeType().GetChequeTypeList());
            objChequeTypeList.Insert(0, new clsChequeType() { ChequeTypeID = -1, ChequeTypeName = "--Select I/D--" });
            cmbChequeTypeName.ItemsSource = objChequeTypeList;
            cmbChequeTypeName.DisplayMemberPath = "ChequeTypeName";
            cmbChequeTypeName.SelectedValuePath = "ChequeTypeID";
            cmbChequeTypeName.SelectedValue = -1;

            /*Date Type*/
            cmbDateType.ItemsSource = GetDateType();
            
            cmbDateType.DisplayMemberPath = "Value";
            cmbDateType.SelectedValuePath = "Key";
            cmbDateType.SelectedIndex = 1;

            /*Group By*/
            cmbGroupBy.ItemsSource = GetGroupBy();
            cmbGroupBy.DisplayMemberPath = "Value";
            cmbGroupBy.SelectedValuePath = "Key";
            cmbGroupBy.SelectedIndex = 0;

            /*GetReportType()*/
            cmbReportTypeName.ItemsSource = GetReportType();
            cmbReportTypeName.DisplayMemberPath = "Value";
            cmbReportTypeName.SelectedValuePath = "Key";
            cmbReportTypeName.SelectedIndex = 0;

            try
            {
                GetData();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }
        public void GetCompanyProject()
        {
            objProjectList = (new BALProject().GetProjectList());

            List<clsProject> CompanyProject = objProjectList.FindAll(x => x.CompanyID == CompanyID);

            cmbProjectName.ItemsSource = CompanyProject;
            cmbProjectName.DisplayMemberPath = "ProjectShortName";
            cmbProjectName.SelectedValuePath = "ProjectID";
            cmbProjectName.SelectedValue = ProjectID;
        }

        private void dgReportFilter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = dgReportFilter.SelectedItem as DataRowView;
            try
            {
                if (row != null)
                {
                    //MessageBox.Show(row["ChequeNo"].ToString());
                    ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                    GetChequeList();
                }
            }
            catch (Exception ex)
            {

                //throw;
                MessageBox.Show(ex.Message);
            }
        }

        public void GetCompanyBank()
        {

            objCompanyWiseBankList = (new BALCompanyWiseBank().GetCompanyWiseBankList());

            List<clsCompanyWiseBank> CompanyBank = objCompanyWiseBankList.FindAll(x => x.CompanyID == CompanyID);


            //cmbBankName.ItemsSource = CompanyBank;
            //cmbBankName.DisplayMemberPath = "BankName";
            //cmbBankName.SelectedValuePath = "BankID";
        }

       
        public Dictionary<string, string> GetDateType()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("CCD", "Clear Date");
            list.Add("CED", "Entry Date");
            list.Add("CID", "Issue Date");

            return list;
        }
        public Dictionary<string, string> GetGroupBy()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("LIST", "list");
            list.Add("PROJ", "Project");
            list.Add("PROJTYPE", "Project + Type");
            list.Add("PROJSUBTYPE", "Project + SubType");
            list.Add("TYPE", "Type");
            list.Add("TYPESUBTYPE", "Type + SubType");
            list.Add("TYPEPROJ", "Type + Project");
            list.Add("SUBTYPEPROJ", "SubType+ Project");
            list.Add("CPT", "Company+ Project + Type");
            list.Add("CPS", "Company+ Project + SubType");
            list.Add("CMP", "Company");

            return list;
        }
        public Dictionary<string, string> GetReportType()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("D", "Detail");
            list.Add("S", "Summary");
            return list;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        private void Reset() {
            dtpStartDate.SelectedDate = System.DateTime.Now;
            dtpEndDate.SelectedDate = System.DateTime.Now;
            cmbCompanyName.SelectedValue = -1;
            cmbBankName.SelectedValue = -1;
            cmbTypeName.SelectedValue = -1;
            cmbSubTypeName.SelectedValue = -1;
            cmbParameterName.SelectedValue = -1;
            cmbProjectName.SelectedValue = -1;
            txtPartyName.Text = "";
            AccountID = 0;
            txtAccountSubName.Text = "";
            txtChequeNo.Text = "";
        }

        private void dgReportFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dgReportFilter.SelectedItem as DataRowView;
            try
            {
                if (row != null)
                {
                    //MessageBox.Show(row["ChequeNo"].ToString());
                    ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                    GetChequeList();
                }
            }
            catch (Exception ex)
            {

                //throw;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPartyName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                AccountPopup obj = new AccountPopup();
                obj.objReportFilter = this;
                obj.ComingFrom = "REPORT";
                obj.ShowDialog();
                txtPartyName.Text = AccountName;

            }
        }

        private void txtPartyName_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            lblStatus.Text = "Press F4 For Party Name Entry";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cursor = Cursors.Wait;
                //  btnExport.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;

               
                _ds = new DataSet();

                StartDate = dtpStartDate.SelectedDate.Value;
                EndDate = dtpEndDate.SelectedDate.Value;
                ChequeStatusID = Convert.ToInt64(cmbChequeStatus.SelectedValue);
                BankID = Convert.ToInt64(cmbBankName.SelectedValue);
                CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                ParameterID = Convert.ToInt64(cmbParameterName.SelectedValue);
                ProjectID = Convert.ToInt64(cmbProjectName.SelectedValue);
                TypeID = Convert.ToInt64(cmbTypeName.SelectedValue);
                SubTypeID = Convert.ToInt64(cmbSubTypeName.SelectedValue);
                ChequeTypeID = Convert.ToInt64(cmbChequeTypeName.SelectedValue);
                AccountSubName = txtAccountSubName.Text.Trim();
                DateType = cmbDateType.SelectedValue.ToString();
                ChequeNo = txtChequeNo.Text.Trim();
                _ds = (new BALReport().GetReport(DateType,
                 StartDate,
                 EndDate,
                 ChequeStatusID,
                 BankID,
                 ChequeNo,
                 AccountID,
                // ChequeEntryID,
                 CompanyID,
                 ParameterID,
                 ProjectID,
                 ChequeTypeID,
                 SubTypeID,
                 TypeID,
                 AccountSubName,ERPID));
                if (_ds.Tables[0].Rows.Count > 0)
                {
                    ExportDataSetToExcel(_ds);
                }
                else
                {
                    Message = "No Record ";
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //Cursor = Cursors.Hand;
                //btnExport.Cursor = null;
                Mouse.OverrideCursor = null;
            }
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // Mouse.OverrideCursor = Cursors.Wait;
                Print();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
               // Mouse.OverrideCursor = null;

            }
        }
        private void cmbCompanyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
            GetCompanyProject();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void GetData()
        {
            _ds = new DataSet();

            StartDate = dtpStartDate.SelectedDate.Value;
            EndDate = dtpEndDate.SelectedDate.Value;
            ChequeStatusID   = Convert.ToInt64( cmbChequeStatus.SelectedValue);
            BankID           = Convert.ToInt64(cmbBankName.SelectedValue);
            CompanyID        = Convert.ToInt64(cmbCompanyName.SelectedValue);
            ParameterID      = Convert.ToInt64(cmbParameterName.SelectedValue);
            ProjectID        = Convert.ToInt64(cmbProjectName.SelectedValue);
            TypeID           = Convert.ToInt64(cmbTypeName.SelectedValue);
            SubTypeID        = Convert.ToInt64(cmbSubTypeName.SelectedValue);
            ChequeTypeID     = Convert.ToInt64(cmbChequeTypeName.SelectedValue);
            AccountSubName   = txtAccountSubName.Text.Trim();
            DateType         = cmbDateType.SelectedValue.ToString();
            ChequeNo         = txtChequeNo.Text.Trim();
            ERPID            = txtERPID.Text.Trim();
            _ds = (new BALReport().GetReport( DateType,
             StartDate,
             EndDate,
             ChequeStatusID,
             BankID,
             ChequeNo,
             AccountID,
            // ChequeEntryID,
             CompanyID,
             ParameterID,
             ProjectID,
             ChequeTypeID,
             SubTypeID,
             TypeID,
             AccountSubName, ERPID));


            dgReportFilter.ItemsSource = _ds.Tables[0].DefaultView;

            if (_ds.Tables[0].Rows.Count > 0)
            {
                lblStatus.Text = "Rows " + _ds.Tables[0].Rows.Count;

                double TotalNetAmount = Convert.ToDouble(_ds.Tables[0].Compute("SUM(NetAmount)", string.Empty));
                // MessageBox.Show(""+sum);
                lblTotalNetAmount.Content = FormatStringIndianCurrency(TotalNetAmount.ToString ());
            }
            else
            {
                lblStatus.Text = "No record found";

            }
        }
        private void GetChequeList()
        {
            try
            {
                _ds = new DataSet();
                _ds = (new BALLedger().GetChequeList(ChequeEntryID));

                foreach (DataRow row in _ds.Tables[0].Rows)
                { 
                    lblTypeName.Content          = row["TypeName"].ToString();
                    lblSubTypeName.Content       = row["SubTypeName"].ToString();
                    lblParameterName.Content     = row["ParameterName"].ToString();
                    lblNaration.Text = row["Narration"].ToString();
                    lblChequeStatusName.Content  = row["ChequeStatusName"].ToString();
                    lblAccountSubName.Content = row["AccountSubName"].ToString();


                    if (!row.IsNull("ChequeIssueDate"))
                    {
                        lblChequeIssueDate.Content = Convert.ToDateTime(row["ChequeIssueDate"].ToString()).ToShortDateString();
                    }
                    else {
                        lblChequeIssueDate.Content = "";
                    }
                    if (!row.IsNull("ChequeClearDate"))
                    {
                        lblChequeClearDate.Content = Convert.ToDateTime(row["ChequeClearDate"].ToString()).ToShortDateString();
                    }
                    else {
                        lblChequeClearDate.Content = "";
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ExportDataSetToExcel(DataSet ds)
        {
            try
            {
                Excel._Application ExcelApp = new Excel.Application();

                //Excel.ApplicationClass ExcelApp = new Excel.ApplicationClass();
                Excel.Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);

                // Loop over DataTables in DataSet.
                DataTableCollection collection = ds.Tables;

                for (int i = collection.Count; i > 0; i--)
                {
                    Excel.Sheets xlSheets = null;
                    Excel.Worksheet xlWorksheet = null;
                    //Create Excel Sheets
                    xlSheets = ExcelApp.Sheets;
                    xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                                   Type.Missing, Type.Missing, Type.Missing);

                    System.Data.DataTable table = collection[i - 1];
                    xlWorksheet.Name = table.TableName;

                    for (int j = 1; j < table.Columns.Count + 1; j++)
                    {
                        ExcelApp.Cells[1, j] = table.Columns[j - 1].ColumnName;
                    }

                    // Storing Each row and column value to excel sheet
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                     

                        for (int l = 0; l < table.Columns.Count; l++)
                        {
                            ExportcolumnName = table.Columns[l].ColumnName;
                            if ((ExportcolumnName == "ChequeEntryDate") || (ExportcolumnName == "ChequeIssueDate") || (ExportcolumnName == "ChequeClearDate"))
                            {
                                ExportcolumnValue = table.Rows[k].ItemArray[l].ToString();
                                if (ExportcolumnValue != null && ExportcolumnValue != "")
                                {
                                    ExcelApp.Cells[k + 2, l + 1] = Convert.ToDateTime(ExportcolumnValue).ToShortDateString();
                                }
                                else {
                                    ExcelApp.Cells[k + 2, l + 1] = "";
                                } 
                                
                            }
                            else {
                                ExcelApp.Cells[k + 2, l + 1] =
                                table.Rows[k].ItemArray[l].ToString();
                            }
                            
                        }
                    }
                    ExcelApp.Columns.AutoFit();
                }
              ((Excel.Worksheet)ExcelApp.ActiveWorkbook.Sheets[ExcelApp.ActiveWorkbook.Sheets.Count]).Delete();
                ExcelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ExportDataSetToExcel2(DataSet ds)
        {
            try
            {
                Path = Path + "\\ExcelFiles";
                if (!System.IO.Directory.Exists(Path))
                {
                    System.IO.Directory.CreateDirectory(Path);
                }
                FileName = AccountID + "-" + DateTime.Now.ToString("yyyy-dd-MMMM--HH-mm-ss");
                Path = Path + @"\" + FileName + ".xls";

                string data = null;
                int i = 0;
                int j = 0;

                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                    {
                        data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                        xlWorkSheet.Cells[i + 1, j + 1] = data;
                    }
                }
                // var b = Environment.CurrentDirectory + @"\c.xls";

                xlWorkBook.SaveAs(Path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();


                /*Open Excel Sheet*/
                Excel.Workbook wb = xlApp.Workbooks.Open(Path);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                Excel.Range xlRange = ws.UsedRange;
                xlApp.Visible = true;
                /*End Excel Sheet*/


                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                GC.Collect();
            }
        }
        public void Print()
        {

            try
            {
                ReportName = cmbGroupBy.Text +  " Wise " +  " From Date: " + StartDate.ToString("dd-MM-yyyy") +" To Date: " + EndDate.ToString("dd-MM-yyyy");
                ReportPreview obj = new ReportPreview();
                ParaField = new Dictionary<string, string>();

                if (cmbDateType.SelectedValue != null)
                {
                    ParaField.Add("@DateType", cmbDateType.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@DateType", "CED");
                }
                /*
                ParaField.Add("@StartDate", dtpStartDate.SelectedDate.Value.ToShortDateString());
                ParaField.Add("@EndDate", dtpStartDate.SelectedDate.Value.ToShortDateString());
                */

                StartDate = dtpStartDate.SelectedDate.Value;
                EndDate = dtpEndDate.SelectedDate.Value;

                ParaField.Add("@StartDate", StartDate.ToString("yyyy-MM-dd"));
                ParaField.Add("@EndDate",EndDate.ToString("yyyy-MM-dd"));

                /*Status Name*/
                if (cmbChequeStatus.SelectedValue != null && Convert.ToInt64( cmbChequeStatus.SelectedValue) != -1)
                {
                    ParaField.Add("@ChequeStatusID", cmbChequeStatus.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@ChequeStatusID", null);
                }
                
                /*Bank Name*/
                if (cmbBankName.SelectedValue != null && Convert.ToInt64(cmbBankName.SelectedValue) != -1)
                {
                    ParaField.Add("@BankID", cmbBankName.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@BankID", null);
                }
                
                /*ChequeNo*/
                if (txtChequeNo.Text.Trim() != "" && txtChequeNo.Text.Trim().Length > 0){
                    ParaField.Add("@ChequeNo", txtChequeNo.Text.Trim());
                }
                else {
                    ParaField.Add("@ChequeNo", null);
                }

                if (AccountID > 0)
                {
                    ParaField.Add("@AccountID", AccountID.ToString());
                }
                else {
                    ParaField.Add("@AccountID", null);
                }
                

                ParaField.Add("@ChequeEntryID", null);
                
                /*Company Name*/
                if (cmbCompanyName.SelectedValue != null && Convert.ToInt64(cmbCompanyName.SelectedValue) != -1)
                {
                    ParaField.Add("@CompanyID", cmbCompanyName.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@CompanyID", null);
                }
                /*Project Name*/
                if (cmbProjectName.SelectedValue != null && Convert.ToInt64(cmbProjectName.SelectedValue) != -1)  {
                    ParaField.Add("@ProjectID", cmbProjectName.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@ProjectID", null);
                }
                /*Parameter Name*/
                if (cmbParameterName.SelectedValue != null && Convert.ToInt64(cmbParameterName.SelectedValue) != -1)
                {
                    ParaField.Add("@ParameterID", cmbParameterName.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@ParameterID", null);
                }
                /*Cheque Type*/
                if (cmbChequeTypeName.SelectedValue != null && Convert.ToInt64(cmbChequeTypeName.SelectedValue) != -1)
                {
                    ParaField.Add("@ChequeTypeID", cmbChequeTypeName.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@ChequeTypeID", null);
                }

                /*Sub Type Name*/
                if (cmbSubTypeName.SelectedValue != null && Convert.ToInt64(cmbSubTypeName.SelectedValue) != -1)
                {
                    ParaField.Add("@SubTypeID", cmbSubTypeName.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@SubTypeID", null);
                }
                /*Type Name*/
                if (cmbTypeName.SelectedValue != null && Convert.ToInt64(cmbTypeName.SelectedValue) != -1)
                {
                    ParaField.Add("@TypeID", cmbTypeName.SelectedValue.ToString());
                }
                else {
                    ParaField.Add("@TypeID", null);
                }
                /*Account Name*/
                if (txtAccountSubName.Text.Trim() != "" && txtAccountSubName.Text.Trim().Length > 0)
                {
                    ParaField.Add("@AccountSubName", txtAccountSubName.Text.Trim());
                }
                else {
                    ParaField.Add("@AccountSubName", null);
                }
                

                ParaField.Add("HeaderReportName", ReportName);
                ParaField.Add("HeaderCompanyName", "KANAKIA SPACES");


                /*Narration*/
                if (chkPrintNarration.IsChecked == true){
                    ParaField.Add("PrintNarration", Convert.ToString(true));
                }
                else {
                    ParaField.Add("PrintNarration", Convert.ToString( false));
                }

                /*Account SubName*/
                if (chkPrintSubName.IsChecked == true)
                {
                    ParaField.Add("PrintAccountSubName", Convert.ToString(true));
                }
                else
                {
                    ParaField.Add("PrintAccountSubName", Convert.ToString(false));
                }

                crName = cmbGroupBy.SelectedValue.ToString();
                switch (crName) {
                    case "LIST":
                        obj.ReportName = "rpList.rpt";
                        break;
                    case "PROJ":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S") {
                            obj.ReportName = "rpGBProjectSummary.rpt";
                            
                        }  else {
                            obj.ReportName = "rpGBProject.rpt";
                        }
                        break;
                    case "PROJTYPE":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S")  {
                            obj.ReportName = "rpGBProjectTypeSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBProjectType.rpt";
                        }
                         
                        break;
                    case "PROJSUBTYPE":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S")  {
                            obj.ReportName = "rpGBProjectSubTypeSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBProjectSubType.rpt";
                        }
                          
                        break;
                    case "TYPE":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S"){
                            obj.ReportName = "rpGBTypeSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBType.rpt";
                        }
                        break;
                    case "TYPESUBTYPE":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S")  {
                            obj.ReportName = "rpGBTypeSubTypeSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBTypeSubType.rpt";
                        }
                        break;
                    case "TYPEPROJ":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S") {
                            obj.ReportName = "rpGBTypeProjectSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBTypeProject.rpt";
                        }
                        break;
                    case "SUBTYPEPROJ":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S"){
                            obj.ReportName = "rpGBSubTypeProjectSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBSubTypeProject.rpt";
                        }
                         
                        break;
                    case "CPT":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S"){
                            obj.ReportName = "rpGBCompanyProjectTypeSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBCompanyProjectType.rpt";
                        }
                         
                        break;
                    case "CPS":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S"){
                            obj.ReportName = "rpGBCompanyProjectSubTypeSummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBCompanyProjectSubType.rpt";
                        }
                        
                        break;
                    case "CMP":
                        if (cmbReportTypeName.SelectedValue.ToString() == "S") {
                            obj.ReportName = "rpGBCompanySummary.rpt";
                        }
                        else {
                            obj.ReportName = "rpGBCompany.rpt";
                        }
                         
                        break;
                    //CPT
                    default:
                        obj.ReportName = "rpList.rpt";
                        break;


                }
                //obj.ReportName = "rpList.rpt";
                obj.ParaField = ParaField;

              //  obj.CrystalReportViewer1.Owner = GetWindow(this);

                obj.ShowDialog();
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string FormatStringIndianCurrency(string TotalNetAmount)
        {
            double parsed = double.Parse(TotalNetAmount.ToString(), CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            string text = string.Format(hindi, "{0:c}", parsed);
            //lblNetAmountValue.Content = text.Substring(1);
            return text.Substring(1);
        }
    }
}
