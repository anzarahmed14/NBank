using BOLNBank;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Runtime.InteropServices;
using BALNBank;
using System.Data;
using System.Globalization;

namespace NBank.Other
{
    /// <summary>
    /// Interaction logic for ChequeImport.xaml
    /// </summary>
    public partial class ChequeImport : Window
    {
        List<clsCompany> objCompanyList;
        List<clsCompanyWiseBank> objCompanyWiseBankList;
        private long CompanyID = 0;
        List<ChequeImportModel> list;
        public ChequeImport()
        {
          
            InitializeComponent();
            GetCompany();
            //BindControl();

        }
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Excel Files|*.xlsx";

            if (dlg.ShowDialog() == true)
            {
                txtFilePath.Text = dlg.FileName;

               // Validate();
            }
        }
        private async void Import_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (string.IsNullOrEmpty(txtFilePath.Text))
                {
                    MessageBox.Show("Select file first");
                    return;
                }

                list = new List<ChequeImportModel>();
                list = ReadExcel(txtFilePath.Text);
                //ValidatePartyName(list);
                //ValidateParameter(list);
                //ValidateProject(list, CompanyID);

                //ValidateSubType(list);
                //ValidateType(list);

                // 🔹 Run validations in parallel
                await Task.WhenAll(
                    Task.Run(() => ValidatePartyName(list)),
                    Task.Run(() => ValidateParameter(list)),
                    Task.Run(() => ValidateProject(list, CompanyID)),
                    Task.Run(() => ValidateSubType(list)),
                    Task.Run(() => ValidateType(list))
                );


                dgImport.ItemsSource = list;

             
                // 🔢 COUNT SUMMARY
                int totalRows = list.Count;

                int errorRows = list.Count(x =>
                    !x.IsAccountValid ||
                    !x.IsParameterValid ||
                    !x.IsProjectValid ||
                    !x.IsSubTypeValid ||
                    !x.IsTypeValid);

                int validRows = totalRows - errorRows;

                // Display summary
                txtSummary.Text =
                    $"Total Rows: {totalRows}   |   " +
                    $"Valid Rows: {validRows}   |   " +
                    $"Error Rows: {errorRows}";
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;

                MessageBox.Show(ex.Message, "Import", MessageBoxButton.OK);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

        }
        private List<ChequeImportModel> ReadExcel(string path)
        {
            var list = new List<ChequeImportModel>();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            // 1️⃣ Header Dictionary
            Dictionary<string, int> headers =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            for (int col = 1; col <= colCount; col++)
            {
                string header =
                    xlRange.Cells[1, col]?.Value2?
                    .ToString()?.Trim();

                if (!string.IsNullOrEmpty(header))
                    headers[header] = col;
            }

            // 2️⃣ ✅ REQUIRED COLUMN VALIDATION (CALL HERE)

            string[] requiredColumns =
            {
        "IssueDate",
        "ProjectCode",
        "Type",
        "SubType",
        "Parameter",
        "ChequeNo",
        "PartyName",
        "ChequeAmount"
    };

            foreach (var col in requiredColumns)
            {
                if (!headers.ContainsKey(col))
                {
                    MessageBox.Show(
                        $"Excel missing column: {col}",
                        "Header Validation",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    // Close Excel before return
                    xlWorkbook.Close(false);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorksheet);
                    Marshal.ReleaseComObject(xlWorkbook);
                    Marshal.ReleaseComObject(xlApp);

                    return new List<ChequeImportModel>();
                }
            }

            // 3️⃣ Helper Function
            object GetCell(int row, string column)
            {
                if (!headers.ContainsKey(column))
                    return null;

                return xlRange.Cells[row,
                       headers[column]]?.Value2;
            }

            // 4️⃣ Read Data Rows
            for (int i = 2; i <= rowCount; i++)
            {
                list.Add(new ChequeImportModel
                {
                    IssueDate =
                        GetCell(i, "IssueDate") != null
                        ? DateTime.FromOADate(
                            Convert.ToDouble(
                                GetCell(i, "IssueDate")))
                        : (DateTime?)null,

                    ProjectCode =
                        GetCell(i, "ProjectCode")?.ToString(),

                    Type =
                        GetCell(i, "Type")?.ToString(),

                    SubType =
                        GetCell(i, "SubType")?.ToString(),

                    Parameter =
                        GetCell(i, "Parameter")?.ToString(),

                    ChequeNo =
                        GetCell(i, "ChequeNo")?.ToString(),

                    AccountName =
                        GetCell(i, "PartyName")?.ToString(),

                    ChequeAmount =
                        Convert.ToDecimal(
                            GetCell(i, "ChequeAmount") ?? 0)
                });
            }

            // 5️⃣ Close Excel
            xlWorkbook.Close(false);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);

            return list;
        }


        private List<ChequeImportModel> ReadExcel_2(string path)
        {
            var list = new List<ChequeImportModel>();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook =
                xlApp.Workbooks.Open(path);

            Excel._Worksheet xlWorksheet =
                xlWorkbook.Sheets[1];

            Excel.Range xlRange =
                xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;

            // Start from row 2 (Skip Header)
            for (int i = 2; i <= rowCount; i++)
            {
                list.Add(new ChequeImportModel
                {
                    // 1️⃣ Issue Date
                    IssueDate =
                        xlRange.Cells[i, 1]?.Value2 != null
                        ? DateTime.FromOADate(
                            xlRange.Cells[i, 1].Value2)
                        : (DateTime?)null,

                    // 2️⃣ Project Code
                    ProjectCode =
                        xlRange.Cells[i, 2]?.Value2?.ToString(),

                    // 3️⃣ Type
                    Type =
                        xlRange.Cells[i, 3]?.Value2?.ToString(),

                    // 4️⃣ SubType
                    SubType =
                        xlRange.Cells[i, 4]?.Value2?.ToString(),

                    // 5️⃣ Parameter
                    Parameter =
                        xlRange.Cells[i, 5]?.Value2?.ToString(),

                    // 6️⃣ Cheque No
                    ChequeNo =
                        xlRange.Cells[i, 6]?.Value2?.ToString(),

                    // 7️⃣ Party Name
                    AccountName =
                        xlRange.Cells[i, 7]?.Value2?.ToString(),

                    // 8️⃣ Cheque Amount
                    ChequeAmount =
                        Convert.ToDecimal(
                            xlRange.Cells[i, 8]?.Value2 ?? 0),

                    // Validation mapping
                  //  NBankAccountName = ""
                });
            }

            // Close Excel
            xlWorkbook.Close(false);
            xlApp.Quit();

            // Release COM Objects
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);

            return list;
        }
        //Import_Click

        //private void Refresh_Click(object sender, RoutedEventArgs e)
        //{
        //    dgImport.ItemsSource = null;
        //    txtSummary.Text = "";
        //}

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            try
            {
                // ⏳ Change Cursor to Wait
                Mouse.OverrideCursor = Cursors.Wait;

                UploadData();
            }
            finally
            {
                // 🔙 Restore Cursor
                Mouse.OverrideCursor = null;
            }
        }


        public void UploadData()
        {

            var list =
                (List<ChequeImportModel>)
                dgImport.ItemsSource;

            int errorRows = list.Count(x =>
                !x.IsAccountValid ||
                !x.IsParameterValid ||
                !x.IsProjectValid ||
                !x.IsSubTypeValid ||
                !x.IsTypeValid);

            if (errorRows > 0)
            {
                MessageBox.Show(
                    "Fix errors before upload.");
                return;
            }



            long companyId =
                Convert.ToInt64(
                    cmbCompanyName.SelectedValue);

            long bankId =
                Convert.ToInt64(
                    cmbBankName.SelectedValue);

            DataTable dt =
               ConvertToDataTable(list, companyId, bankId);

            long userId = Globals.UserID;   // Login UserID

            string fileName =
                System.IO.Path.GetFileName(
                    txtFilePath.Text);

            DataTable dupTable =
      new BALChequeEntry()
      .ImportChequeEntry(
          dt,
          companyId,
          bankId,
          userId,
          fileName);


            if (dupTable.Rows.Count > 0)
            {
                int totalDuplicates = dupTable.Rows.Count;

                // Decide how many to display
                int displayCount = Math.Min(5, totalDuplicates);

                // Dynamic Header
                string msg =
                    $"Duplicate Found: {totalDuplicates} record(s)\n\n";

                if (totalDuplicates > 5)
                {
                    msg += "Showing Top 5 Records:\n\n";
                }
                else
                {
                    msg += "Duplicate Records:\n\n";
                }

                msg +=
                    "ChequeNo | Date | Bank | Company | Account\n" +
                    "------------------------------------------------\n";

                // Loop records
                for (int i = 0; i < displayCount; i++)
                {
                    DataRow row = dupTable.Rows[i];

                    msg +=
                        $"{row["ChequeNo"]} | " +
                        $"{Convert.ToDateTime(row["ChequeDate"]) .ToString("dd/MM/yyyy")} | " +
                        $"{row["BankName"]} | " +
                        $"{row["CompanyName"]} | " +
                        $"{row["AccountName"]}\n";
                }

                // More records note
                if (totalDuplicates > 5)
                {
                    msg += "\n...and more duplicate records exist.";
                }

                MessageBox.Show(msg,
                                "Duplicate Validation",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                return;
            }


            MessageBox.Show(  "Import Completed Successfully");
        }

        private DataTable ConvertToDataTable(
     List<ChequeImportModel> list,
     long companyId,
     long bankId)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ChequeEntryDate", typeof(DateTime));
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

            foreach (var item in list)
            {
                dt.Rows.Add(
                    item.IssueDate,
                    item.ProjectID,
                    item.AccountID,
                    item.AccountName,

                    
                    bankId,
                    item.ChequeNo,
                    item.TypeID,
                    item.SubTypeID,
                    item.ParameterID,
                    item.ChequeAmount,
                    companyId
                );
            }

            return dt;
        }



        private void ValidateBankCode( List<ChequeImportModel> list)
        {
            var bankDict = (new BALBank(). LoadBanks());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.BankCode))
                {
                    item.IsBankValid = false;
                    continue;
                }

                string excelBank =
                    item.BankCode
                    .Trim()
                    .ToUpper();

                if (bankDict.ContainsKey(excelBank))
                {
                    item.IsBankValid = true;
                }
                else
                {
                    item.IsBankValid = false;
                }
            }
        }
        private void ValidatePartyName(
     List<ChequeImportModel> list)
        {
            var accountDict =
                (new BALAccount().LoadAccounts());

            int matched = 0;
            int notMatched = 0;

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(
                    item.AccountName))
                {
                    item.IsAccountValid = false;
                    item.AccountID = 0;
                    notMatched++;
                    continue;
                }

                string excelName =
                    item.AccountName
                    .Trim()
                    .ToUpper();

                if (accountDict.ContainsKey(excelName))
                {
                    // ✅ VALID
                    item.AccountID =
                        accountDict[excelName];

                    item.IsAccountValid = true;
                    matched++;
                }
                else
                {
                    // ❌ INVALID
                    item.AccountID = 0;
                    item.IsAccountValid = false;
                    notMatched++;
                }
            }
        }

        private void ValidateParameter(
     List<ChequeImportModel> list)
        {
            var paramDict =
                (new BALParameter().LoadParameters());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.Parameter))
                {
                    item.ParameterID = 0;
                    item.IsParameterValid = false;
                    continue;
                }

                string excelParam =
                    item.Parameter
                    .Trim()
                    .ToUpper();

                if (paramDict.ContainsKey(excelParam))
                {
                    // ✅ VALID → Assign ID
                    item.ParameterID =
                        paramDict[excelParam];

                    item.IsParameterValid = true;
                }
                else
                {
                    // ❌ INVALID
                    item.ParameterID = 0;
                    item.IsParameterValid = false;
                }
            }
        }

        private void ValidateSubType(
    List<ChequeImportModel> list)
        {
            var subTypeDict =
                (new BALSubType().LoadSubTypes());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.SubType))
                {
                    item.SubTypeID = 0;
                    item.IsSubTypeValid = false;
                    continue;
                }

                string excelSubType =
                    item.SubType
                    .Trim()
                    .ToUpper();

                if (subTypeDict.ContainsKey(excelSubType))
                {
                    // ✅ VALID → Assign ID
                    item.SubTypeID =
                        subTypeDict[excelSubType];

                    item.IsSubTypeValid = true;
                }
                else
                {
                    // ❌ INVALID
                    item.SubTypeID = 0;
                    item.IsSubTypeValid = false;
                }
            }
        }

        private void ValidateType(
     List<ChequeImportModel> list)
        {
            var typeDict =
                (new BALType().LoadTypes());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.Type))
                {
                    item.TypeID = 0;
                    item.IsTypeValid = false;
                    continue;
                }

                string excelType =
                    item.Type
                    .Trim()
                    .ToUpper();

                if (typeDict.ContainsKey(excelType))
                {
                    // ✅ VALID → Assign ID
                    item.TypeID =
                        typeDict[excelType];

                    item.IsTypeValid = true;
                }
                else
                {
                    // ❌ INVALID
                    item.TypeID = 0;
                    item.IsTypeValid = false;
                }
            }
        }



        private void ValidateProject(
      List<ChequeImportModel> list,
      long companyId)
        {
            var projectDict =
                (new BALProject()
                .LoadProjects(companyId));

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.ProjectCode))
                {
                    item.ProjectID = 0;
                    item.IsProjectValid = false;
                    continue;
                }

                string excelProject =
                    item.ProjectCode
                    .Trim()
                    .ToUpper();

                if (projectDict.ContainsKey(excelProject))
                {
                    // ✅ VALID → Assign ID
                    item.ProjectID =
                        projectDict[excelProject];

                    item.IsProjectValid = true;
                }
                else
                {
                    // ❌ INVALID
                    item.ProjectID = 0;
                    item.IsProjectValid = false;
                }
            }
        }


        private void GetCompany()
        {
            objCompanyList = new List<clsCompany>();
            objCompanyList = (new BALCompany().GetCompanyList("", Globals.UserID));

            /*15-December-2017*/
            objCompanyList = objCompanyList.FindAll(Active => Active.IsActive == true);

            objCompanyList.Insert(0, new clsCompany() { CompanyID = -1, CompanyShortName = "--Select Company--" });

            cmbCompanyName.ItemsSource = objCompanyList;
            cmbCompanyName.DisplayMemberPath = "CompanyShortName";
            cmbCompanyName.SelectedValuePath = "CompanyID";
            cmbCompanyName.SelectedValue = -1;
        }
        private void cmbCompanyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //MessageBox.Show(cmbCompanyName.SelectedValue.ToString());
                CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                if (CompanyID > 0)
                {
                    var company = objCompanyList.Find(x => x.CompanyID == CompanyID);
                    lblSelectedCompanyName.Content = company.CompanyName;
                }
                GetCompanyBank();
               // Validate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "009", "Company selection change", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //cmbBankName_SelectionChanged

        private void cmbBankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                //Validate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "009", "Company selection change", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void GetCompanyBank()
        {

            try
            {
                List<clsCompanyWiseBank> CompanyBank;
                CompanyBank = new List<clsCompanyWiseBank>();
                objCompanyWiseBankList = new List<clsCompanyWiseBank>();


                objCompanyWiseBankList = (new BALCompanyWiseBank().GetCompanyWiseBankList());
                //CompanyBank = objCompanyWiseBankList.FindAll(x => x.CompanyID == CompanyID);
                CompanyBank = objCompanyWiseBankList.FindAll(c => (c.CompanyID == CompanyID) && (c.IsActive == true));


                cmbBankName.ItemsSource = CompanyBank;
                cmbBankName.DisplayMemberPath = "BankName";
                cmbBankName.SelectedValuePath = "BankID";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "  015", "Company Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Validate2()
        {
            long companyId =
               Convert.ToInt64(
                   cmbCompanyName.SelectedValue);

            long bankId =
                Convert.ToInt64(
                    cmbBankName.SelectedValue);


            if (companyId > 0)
            {
                cmbBankName.IsEnabled = true;
            }
            else
            {
                cmbBankName.IsEnabled = false;
            }


            if (companyId > 0 && bankId > 0)
            {
                btnBrowse.IsEnabled = true;
            }
            else
            {
                btnBrowse.IsEnabled = false;
            }

            if (list != null)
            {
                int errorRows = list.Count(x =>
              !x.IsAccountValid ||
              !x.IsParameterValid ||
              !x.IsProjectValid ||
              !x.IsSubTypeValid ||
              !x.IsTypeValid);


                if (errorRows == 0)
                {
                    btnUpload.IsEnabled = true;
                }
                else
                {
                    btnUpload.IsEnabled = false;
                }
            }
            else
            {
                btnUpload.IsEnabled = false;
            }

            if (txtFilePath.Text.Trim().Length > 0  && companyId > 0 && bankId > 0)
            {
                btnImport.IsEnabled = true;
            }
            else
            {
                btnImport.IsEnabled = false;
            }
            

        }
        public void BindControl()
        {
            cmbBankName.IsEnabled = false;
            btnBrowse.IsEnabled = false;
            btnImport.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }
        public bool Validate()
        {
            bool isValid = true;
            string message = "";

            // 1️⃣ Company Validation
            if (cmbCompanyName.SelectedValue == null ||
                Convert.ToInt64(cmbCompanyName.SelectedValue) <= 0)
            {
                message += "Select Company" + Environment.NewLine;
                isValid = false;
            }

            // 2️⃣ Bank Validation
            if (cmbBankName.SelectedValue == null ||
                Convert.ToInt64(cmbBankName.SelectedValue) <= 0)
            {
                message += "Select Bank" + Environment.NewLine;
                isValid = false;
            }

            // 3️⃣ Grid Row Validation
            if (dgImport.Items.Count == 0)
            {
                message += "Grid has no rows. Please upload file first."
                           + Environment.NewLine;
                isValid = false;
            }

            // 4️⃣ Row Error Validation
            if (list != null && list.Count > 0)
            {
                int errorRows = list.Count(x =>
                      !x.IsAccountValid ||
                      !x.IsParameterValid ||
                      !x.IsProjectValid ||
                      !x.IsSubTypeValid ||
                      !x.IsTypeValid);

                if (errorRows > 0)
                {
                    message += $"Grid contains {errorRows} error row(s). "
                            + "Please fix before upload."
                            + Environment.NewLine;

                    isValid = false;
                }


                // Change fields as per your Excel columns
                var duplicateGroups = list
                    .GroupBy(x => new
                    {
                        x.AccountName,
                        x.ChequeNo,
                        x.ChequeAmount,
                        x.IssueDate
                    })
                    .Where(g => g.Count() > 1)
                    .ToList();

                if (duplicateGroups.Any())
                {
                    int duplicateRowCount = duplicateGroups
                                            .Sum(g => g.Count());

                    message += $"Excel contains {duplicateRowCount} duplicate row(s). "
                             + "Duplicate rows are not allowed."
                             + Environment.NewLine;

                    isValid = false;
                }

            }

            var duplicateChequeGroups = list
                .Where(x => !string.IsNullOrWhiteSpace(x.ChequeNo))
                .GroupBy(x => x.ChequeNo.Trim().ToUpper())
                .Where(g => g.Count() > 1)
                .ToList();

                        if (duplicateChequeGroups.Any())
                        {
                            int duplicateRowCount =
                                duplicateChequeGroups.Sum(g => g.Count());

                            string chequeList = string.Join(", ",
                                duplicateChequeGroups
                                    .Select(g => g.Key));

                            message += $"Duplicate Cheque No found: {chequeList}"
                                     + Environment.NewLine;

                            isValid = false;
            }
            // 6️⃣ Issue Date Format Validation
            var invalidDateRows = list
                .Select((x, index) => new
                {
                    RowNo = index + 2, // Excel row number
        x.IssueDate
                })
                .Where(x => x.IssueDate == null)
                .ToList();

            if (invalidDateRows.Any())
            {
                string rows = string.Join(", ",
                    invalidDateRows.Select(x => x.RowNo));

                message += $"Invalid IssueDate format in row(s): {rows}. "
                         + "Required format: dd/MM/yyyy"
                         + Environment.NewLine;

                isValid = false;
            }



            // 5️⃣ Show Message
            if (!isValid)
            {
                MessageBox.Show(message,
                                "Validation",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }

            return isValid;
        }
        private bool IsValidDateFormat(object dateValue)
        {
            if (dateValue == null)
                return false;

            DateTime tempDate;

            // Try parse exact dd/MM/yyyy
            return DateTime.TryParseExact(
                dateValue.ToString(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out tempDate);
        }

    }
}
