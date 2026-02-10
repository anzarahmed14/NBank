
using System;
using System.Collections.Generic;
using System.Data;
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


namespace NBank.Ledger
{
    /// <summary>
    /// Interaction logic for AccountLedger.xaml
    /// </summary>
    public partial class AccountLedger : Window
    {
        internal long AccountID;
        internal long ProjectID;
        public AccountLedgerList objAccountList { get; internal set; }
        DataSet _ds;
        DateTime StartDate, EndDate;
        double TotalDebit, TotalCredit, DebitTDS, CreditTDS, OpeningBalance, Debit, ClosingBalance, Credit = 0;

       
        long ChequeTypeID = 0; /*1 = ISSUE, 2 = Deposit*/
        long ChequeEntryID = 0;

        // double OpeningBalance = 0;
        string Message = "";
        string MessageTitle = "Account Ledger";
        string Path = Environment.CurrentDirectory;
        string FileName = "";
        string AccountName = "";
        string AccountShortName = "";
        public Dictionary<string, object> ParaField = null;
        string ExportcolumnName = "";
        string ExportcolumnValue = "";
        public AccountLedger()
        {
            InitializeComponent();
        }

        private void frmAccountLedger_Loaded(object sender, RoutedEventArgs e)
        {
            //  long test = ProjectID;
            try
            {
                GetAccountLedger(DateTime.MinValue, DateTime.MinValue);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void GetAccountLedger(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                Debit = 0;
                Credit = 0;
                TotalDebit = 0;
                TotalCredit = 0;
                DebitTDS = 0;
                CreditTDS = 0;
                _ds = new DataSet();
                _ds = (new BALNBank.BALLedger().GetAccountLedger(StartDate, EndDate, AccountID, ProjectID,Globals.UserID));
                dgAccountLedgerList.ItemsSource = _ds.Tables[0].DefaultView;

                if (_ds.Tables[0].Rows.Count > 0)
                {
                    lblAccountName.Content = _ds.Tables[0].Rows[0]["AccountName"].ToString();

                    AccountName = _ds.Tables[0].Rows[0]["AccountName"].ToString();

                    lblAccountSubName.Content = _ds.Tables[0].Rows[0]["AccountShortName"].ToString();
                    AccountShortName = _ds.Tables[0].Rows[0]["AccountShortName"].ToString();
                    lblOpeningBalance.Content = _ds.Tables[0].Rows[0]["OpeningBalance"].ToString();

                    OpeningBalance = Convert.ToDouble(_ds.Tables[0].Rows[0]["OpeningBalance"].ToString());
                }
                else {
                    lblAccountName.Content = AccountName;
                    lblAccountSubName.Content = AccountShortName;
                }

                foreach (DataRow row in _ds.Tables[0].Rows)
                {
                    ChequeTypeID = Convert.ToInt64(row["ChequeTypeID"].ToString());

                    if (ChequeTypeID == 1)
                    {

                        if (!row.IsNull("ChequeAmount"))
                        {
                            Debit = Debit + Convert.ToDouble(row["ChequeAmount"].ToString());
                        }
                        if (!row.IsNull("ChequeAmountTDS"))
                        {
                            DebitTDS = DebitTDS + Convert.ToDouble(row["ChequeAmountTDS"].ToString());
                        }


                    }
                    if (ChequeTypeID == 2)
                    {
                        if (!row.IsNull("ChequeAmount"))
                        {
                            Credit = Credit + Convert.ToDouble(row["ChequeAmount"].ToString());
                        }
                        if (!row.IsNull("ChequeAmountTDS"))
                        {
                            CreditTDS = CreditTDS + Convert.ToDouble(row["ChequeAmountTDS"].ToString());
                        }
                    }

                }

                //lblDebit.Content = Debit + DebitTDS;
                //lblCredit.Content = Credit + CreditTDS;
                TotalDebit = Debit + DebitTDS;
                TotalCredit = Credit + CreditTDS;

                lblDebit.Content = TotalDebit;
                lblCredit.Content = TotalCredit;

                ClosingBalance = (OpeningBalance + (TotalDebit - TotalCredit));

                lblClosingBalance.Content = ClosingBalance;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void dgAccountLedgerList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = dgAccountLedgerList.SelectedItem as System.Data.DataRowView;
            try
            {
                if (row != null)
                {
                    //MessageBox.Show(row["ChequeNo"].ToString());
                    ChequeEntryID = Convert.ToInt64( row["ChequeEntryID"].ToString());
                    GetChequeList();
                }
            }
            catch (Exception ex)
            {

                //throw;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgAccountLedgerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dgAccountLedgerList.SelectedItem as System.Data.DataRowView;
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

        private void GetChequeList()
        {
            try
            {
                _ds = new DataSet();
                _ds = (new BALNBank.BALLedger().GetChequeList(ChequeEntryID));

                foreach (DataRow row in _ds.Tables[0].Rows) {
                    lblNaration.Text         = row["Narration"].ToString();
                    lblTypeName.Content         = row["TypeName"].ToString();
                    lblSubTypeName.Content      = row["SubTypeName"].ToString();
                    lblStatus.Content = row["ChequeStatusName"].ToString();

                    lblParameterName.Content = row["ParameterName"].ToString();

                    if (!row.IsNull("ChequeIssueDate")) {
                        lblChequeIssueDate.Content = Convert.ToDateTime(row["ChequeIssueDate"].ToString()).ToShortDateString();
                    }
                    if (!row.IsNull("ChequeClearDate"))
                    {
                        lblChequeClearDate.Content = Convert.ToDateTime(row["ChequeClearDate"].ToString()).ToShortDateString();
                    }
                 
                  



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                Print();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
               
                Mouse.OverrideCursor = null;
            }

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cursor = Cursors.Wait;
                //  btnExport.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;

                _ds = new DataSet();
                _ds = (new BALNBank.BALLedger().GetAccountLedger(StartDate, EndDate, AccountID,ProjectID,Globals.UserID));
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
          
            //open();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartDate = dtpFromDate.SelectedDate.Value;
                EndDate = dtpToDate.SelectedDate.Value;

                GetAccountLedger(StartDate, EndDate);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
                            // ExcelApp.Cells[k + 2, l + 1] =
                            // table.Rows[k].ItemArray[l].ToString();
                            ExportcolumnName = table.Columns[l].ColumnName;
                            if ((ExportcolumnName == "ChequeEntryDate") || (ExportcolumnName == "ChequeIssueDate") || (ExportcolumnName == "ChequeClearDate"))
                            {
                                ExportcolumnValue = table.Rows[k].ItemArray[l].ToString();
                                if (ExportcolumnValue != null && ExportcolumnValue != "")
                                {
                                    ExcelApp.Cells[k + 2, l + 1] = Convert.ToDateTime(ExportcolumnValue).ToShortDateString();
                                }
                                else
                                {
                                    ExcelApp.Cells[k + 2, l + 1] = "";
                                }

                            }
                            else
                            {
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
                if (!System.IO.Directory.Exists(Path)) {
                    System.IO.Directory.CreateDirectory(Path);
                }
                FileName = AccountID + "-"+  DateTime.Now.ToString("yyyy-dd-MMMM--HH-mm-ss");
                Path = Path   +  @"\"+ FileName + ".xls";

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

                for (int k = 1; k < ds.Tables[0].Columns.Count + 1; k++)
                {
                    xlWorkSheet.Cells[1, k] = ds.Tables[0].Columns[k - 1].ColumnName;
                }
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
        private void Print() {

            ReportPreview obj = new ReportPreview();
            ParaField = new Dictionary<string, object>();

            ParaField.Add("@DateType", "CED");
            ParaField.Add("@StartDate", null);
            ParaField.Add("@EndDate", null);
            ParaField.Add("@ChequeStatusID", null);
            ParaField.Add("@BankID", null);
            ParaField.Add("@ChequeNo", null);
            ParaField.Add("@AccountID", Convert.ToString( AccountID));
            ParaField.Add("@UserId", Convert.ToString(Globals.UserID));
            ParaField.Add("@ProjectID",null);

            ParaField.Add("OpeningBalance", Convert.ToString(OpeningBalance));
            ParaField.Add("ClosingBalance", Convert.ToString(ClosingBalance));




            obj.ReportName = "rpAccountLedger.rpt";
  
            obj.ParaField = ParaField;
            obj.ShowDialog();
        }
    }
}
