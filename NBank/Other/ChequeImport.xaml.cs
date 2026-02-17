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
        string MenuName = "MenuChequeImport";
        List<clsUserMenu> FilteredUserMenuList;
        string MessageTitle = "Cheque Import";
        public ChequeImport()
        {
          
            InitializeComponent();
            GetCompany();
            //BindControl();

        }
        private void UserMenu()
        {

            try
            {
                FilteredUserMenuList = Globals.UserMenuList.Where(x => x.MenuName == MenuName).ToList();

                if (FilteredUserMenuList.Count > 0)
                {
                    if (FilteredUserMenuList[0].AllowCreate == false)
                    {
                       // btnAdd.Visibility = Visibility.Collapsed;
                    }
                    if (FilteredUserMenuList[0].AllowEdit == false)
                    {
                       // btnEdit.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserMenu();

                //Keyboard.Focus(txtAccountName);
                //GetAccount();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
                list = list
        .OrderBy(x => x.IssueDate)   // Change property name if different
        .ToList();

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
        private List<ChequeImportModel> ReadExcel2(string path)
        {
            try
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
                "EntryDate",
        "IssueDate",
        "ProjectCode",
        "Type",
        "SubType",
        "Parameter",
        "ChequeNo",
        "PartyName",
        "ChequeAmount",
        "Narration"
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

                        EntryDate =
                            GetCell(i, "EntryDate") != null
                            ? DateTime.FromOADate(
                                Convert.ToDouble(
                                    GetCell(i, "EntryDate")))
                            : (DateTime?)null,

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
                                GetCell(i, "ChequeAmount") ?? 0),

                        Narration =
                            GetCell(i, "Narration")?.ToString(),
                    });
                }

                // 5️⃣ Close Excel
                xlWorkbook.Close(false);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlApp);

              
            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.Message, "Reading Excel File");
            }
            return list;
        }


        private List<ChequeImportModel> ReadExcel(string path)
        {
            var list = new List<ChequeImportModel>();
            Excel.Application xlApp = null;
            Excel.Workbook xlWorkbook = null;
            Excel._Worksheet xlWorksheet = null;
            Excel.Range xlRange = null;

            try
            {
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(path);
                xlWorksheet = xlWorkbook.Sheets[1];
                xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                // 1️⃣ Header Dictionary
                Dictionary<string, int> headers =
                    new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                for (int col = 1; col <= colCount; col++)
                {
                    string header = xlRange.Cells[1, col]?.Value2?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(header))
                        headers[header] = col;
                }

                // 2️⃣ Required Column Validation
                string[] requiredColumns =
                {
            "EntryDate","IssueDate","ProjectCode","Type","SubType",
            "Parameter","ChequeNo","PartyName","ChequeAmount","Narration"
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

                        return new List<ChequeImportModel>();
                    }
                }

                // 3️⃣ Helper Function
                object GetCell(int row, string column)
                {
                    if (!headers.ContainsKey(column))
                        return null;
                    return xlRange.Cells[row, headers[column]]?.Value2;
                }

                // 4️⃣ Read Data Rows with Error Context
                for (int i = 2; i <= rowCount; i++)
                {
                    try
                    {
                        var model = new ChequeImportModel
                        {
                            EntryDate = GetCell(i, "EntryDate") != null
                                ? DateTime.FromOADate(Convert.ToDouble(GetCell(i, "EntryDate")))
                                : (DateTime?)null,

                            IssueDate = GetCell(i, "IssueDate") != null
                                ? DateTime.FromOADate(Convert.ToDouble(GetCell(i, "IssueDate")))
                                : (DateTime?)null,

                            ProjectCode = GetCell(i, "ProjectCode")?.ToString(),
                            Type = GetCell(i, "Type")?.ToString(),
                            SubType = GetCell(i, "SubType")?.ToString(),
                            Parameter = GetCell(i, "Parameter")?.ToString(),
                            ChequeNo = GetCell(i, "ChequeNo")?.ToString(),
                            AccountName = GetCell(i, "PartyName")?.ToString(),
                            ChequeAmount = Convert.ToDecimal(GetCell(i, "ChequeAmount") ?? 0),
                            Narration = GetCell(i, "Narration")?.ToString(),
                        };

                        list.Add(model);
                    }
                    catch (Exception exRow)
                    {
                        // Collect snapshot of row values for debugging
                        string rowSnapshot = string.Join(", ",
                            headers.Keys.Select(h => $"{h}={GetCell(i, h)}"));

                        MessageBox.Show(
                            $"Error at Row {i}: {exRow.Message}\n" +
                            $"Row Snapshot: {rowSnapshot}",
                            "Excel Read Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Reading Excel File");
            }
            finally
            {
                // 5️⃣ Ensure Excel is closed
                if (xlWorkbook != null) xlWorkbook.Close(false);
                if (xlApp != null) xlApp.Quit();

                if (xlRange != null) Marshal.ReleaseComObject(xlRange);
                if (xlWorksheet != null) Marshal.ReleaseComObject(xlWorksheet);
                if (xlWorkbook != null) Marshal.ReleaseComObject(xlWorkbook);
                if (xlApp != null) Marshal.ReleaseComObject(xlApp);
            }

            return list;
        }



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
                    "ChequeNo | Issue Date | Bank | Company \n" +
                    "------------------------------------------------\n";

                // Loop records
                for (int i = 0; i < displayCount; i++)
                {
                    DataRow row = dupTable.Rows[i];

                    msg +=
                        $"{row["ChequeNo"]} | " +
                        $"{Convert.ToDateTime(row["ChequeDate"]) .ToString("dd/MM/yyyy")} | " +
                        $"{row["BankName"]} | " +
                        $"{row["CompanyName"]}  \n";
                       
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

                    
                    bankId,
                    item.ChequeNo,
                    item.TypeID,
                    item.SubTypeID,
                    item.ParameterID,
                    item.ChequeAmount,
                    companyId,
                    item.Narration
                );
            }

            return dt;
        }


        private void ValidateBankCode(List<ChequeImportModel> list)
        {
            try
            {
                var bankDict = new BALBank().LoadBanks();

                foreach (var item in list)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.BankCode))
                        {
                            item.IsBankValid = false;
                            continue;
                        }

                        string excelBank = item.BankCode.Trim().ToUpper();

                        if (bankDict.ContainsKey(excelBank))
                        {
                            item.IsBankValid = true;
                        }
                        else
                        {
                            item.IsBankValid = false;
                        }
                    }
                    catch (Exception exItem)
                    {
                        // Row-level error handling
                        item.IsBankValid = false;

                        MessageBox.Show(
                            $"Error validating BankCode '{item?.BankCode}'.\n" +
                            $"Details: {exItem.Message}",
                            "Bank Validation Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                // Critical error (e.g., LoadBanks failed)
                MessageBox.Show(
                    $"Critical error during bank validation.\nDetails: {ex.Message}",
                    "Bank Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        private void ValidateBankCode2( List<ChequeImportModel> list)
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
        private void ValidatePartyName(List<ChequeImportModel> list)
        {
            try
            {
                var accountDict = new BALAccount().LoadAccounts();

                int matched = 0;
                int notMatched = 0;

                foreach (var item in list)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.AccountName))
                        {
                            item.IsAccountValid = false;
                            item.AccountID = 0;
                            notMatched++;
                            continue;
                        }

                        string excelName = item.AccountName.Trim().ToUpper();

                        if (accountDict.ContainsKey(excelName))
                        {
                            // ✅ VALID
                            item.AccountID = accountDict[excelName];
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
                    catch (Exception exItem)
                    {
                        // Catch unexpected issues for this row
                        item.IsAccountValid = false;
                        item.AccountID = 0;
                        notMatched++;

                        MessageBox.Show(
                            $"Error validating AccountName '{item?.AccountName}' in list item.\n" +
                            $"Details: {exItem.Message}",
                            "Validation Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                }

                // Optional summary message
                //MessageBox.Show(
                //    $"Validation complete.\nMatched: {matched}, Not Matched: {notMatched}",
                //    "Validation Summary",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Information
                //);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Critical error during account validation.\nDetails: {ex.Message}",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        private void ValidatePartyName2(
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

        private void ValidateParameter2(
     List<ChequeImportModel> list)
        {
            var paramDict =
                (new BALParameter().LoadParameters());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.Parameter))
                {
                    item.ParameterID = -1;
                    item.IsParameterValid = true;
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
                    item.ParameterID = -1;
                    item.IsParameterValid = false;
                }
            }
        }
        private void ValidateParameter(List<ChequeImportModel> list)
        {
            try
            {
                var paramDict = new BALParameter().LoadParameters();

                foreach (var item in list)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.Parameter))
                        {
                            item.ParameterID = -1;
                            item.IsParameterValid = true; // treat blank as valid
                            continue;
                        }

                        string excelParam = item.Parameter.Trim().ToUpper();

                        if (paramDict.ContainsKey(excelParam))
                        {
                            // ✅ VALID → Assign ID
                            item.ParameterID = paramDict[excelParam];
                            item.IsParameterValid = true;
                        }
                        else
                        {
                            // ❌ INVALID
                            item.ParameterID = -1;
                            item.IsParameterValid = false;
                        }
                    }
                    catch (Exception exItem)
                    {
                        // Row-level error handling
                        item.ParameterID = -1;
                        item.IsParameterValid = false;

                        MessageBox.Show(
                            $"Error validating Parameter '{item?.Parameter}'.\n" +
                            $"Details: {exItem.Message}",
                            "Parameter Validation Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                // Critical error (e.g., LoadParameters failed)
                MessageBox.Show(
                    $"Critical error during parameter validation.\nDetails: {ex.Message}",
                    "Parameter Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }


        private void ValidateSubType2(
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

        private void ValidateSubType(List<ChequeImportModel> list)
        {
            try
            {
                var subTypeDict = new BALSubType().LoadSubTypes();

                foreach (var item in list)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.SubType))
                        {
                            item.SubTypeID = 0;
                            item.IsSubTypeValid = false;
                            continue;
                        }

                        string excelSubType = item.SubType.Trim().ToUpper();

                        if (subTypeDict.ContainsKey(excelSubType))
                        {
                            // ✅ VALID → Assign ID
                            item.SubTypeID = subTypeDict[excelSubType];
                            item.IsSubTypeValid = true;
                        }
                        else
                        {
                            // ❌ INVALID
                            item.SubTypeID = 0;
                            item.IsSubTypeValid = false;
                        }
                    }
                    catch (Exception exItem)
                    {
                        // Row-level error handling
                        item.SubTypeID = 0;
                        item.IsSubTypeValid = false;

                        MessageBox.Show(
                            $"Error validating SubType '{item?.SubType}'.\n" +
                            $"Details: {exItem.Message}",
                            "SubType Validation Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                // Critical error (e.g., LoadSubTypes failed)
                MessageBox.Show(
                    $"Critical error during subtype validation.\nDetails: {ex.Message}",
                    "SubType Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void ValidateType2(
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
        private void ValidateType(List<ChequeImportModel> list)
        {
            try
            {
                var typeDict = new BALType().LoadTypes();

                foreach (var item in list)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.Type))
                        {
                            item.TypeID = 0;
                            item.IsTypeValid = false;
                            continue;
                        }

                        string excelType = item.Type.Trim().ToUpper();

                        if (typeDict.ContainsKey(excelType))
                        {
                            // ✅ VALID → Assign ID
                            item.TypeID = typeDict[excelType];
                            item.IsTypeValid = true;
                        }
                        else
                        {
                            // ❌ INVALID
                            item.TypeID = 0;
                            item.IsTypeValid = false;
                        }
                    }
                    catch (Exception exItem)
                    {
                        // Row-level error handling
                        item.TypeID = 0;
                        item.IsTypeValid = false;

                        MessageBox.Show(
                            $"Error validating Type '{item?.Type}'.\n" +
                            $"Details: {exItem.Message}",
                            "Type Validation Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                // Critical error (e.g., LoadTypes failed)
                MessageBox.Show(
                    $"Critical error during type validation.\nDetails: {ex.Message}",
                    "Type Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }


        private void ValidateProject2(
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
        private void ValidateProject(List<ChequeImportModel> list, long companyId)
        {
            try
            {
                var projectDict = new BALProject().LoadProjects(companyId);

                foreach (var item in list)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.ProjectCode))
                        {
                            item.ProjectID = 0;
                            item.IsProjectValid = false;
                            continue;
                        }

                        string excelProject = item.ProjectCode.Trim().ToUpper();

                        if (projectDict.ContainsKey(excelProject))
                        {
                            // ✅ VALID → Assign ID
                            item.ProjectID = projectDict[excelProject];
                            item.IsProjectValid = true;
                        }
                        else
                        {
                            // ❌ INVALID
                            item.ProjectID = 0;
                            item.IsProjectValid = false;
                        }
                    }
                    catch (Exception exItem)
                    {
                        // Row-level error handling
                        item.ProjectID = 0;
                        item.IsProjectValid = false;

                        MessageBox.Show(
                            $"Error validating ProjectCode '{item?.ProjectCode}'.\n" +
                            $"Details: {exItem.Message}",
                            "Project Validation Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                // Critical error (e.g., LoadProjects failed)
                MessageBox.Show(
                    $"Critical error during project validation.\nDetails: {ex.Message}",
                    "Project Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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





            // 6️⃣ EntryDate NULL + FORMAT Validation
            var invalidEntryRows = list
                .Select((x, index) => new
                {
                    RowNo = index + 2,
                    EntryDate = NormalizeExcelDate(x.EntryDate) // <-- FIX
        })
                .Where(x => x.EntryDate == null || !IsDateInDDMMYYYY(x.EntryDate))
                .ToList();

            if (invalidEntryRows.Any())
            {
                string rows = string.Join(", ", invalidEntryRows.Select(x => x.RowNo));

                message += $"EntryDate invalid in row(s): {rows}. "
                         + "Required format: dd/MM/yyyy"
                         + Environment.NewLine;

                isValid = false;
            }

            // 7️⃣ IssueDate NULL + FORMAT Validation
            var invalidIssueRows = list
                .Select((x, index) => new
                {
                    RowNo = index + 2,
                    IssueDate = NormalizeExcelDate(x.IssueDate) // <-- FIX
        })
                .Where(x => x.IssueDate == null || !IsDateInDDMMYYYY(x.IssueDate))
                .ToList();

            if (invalidIssueRows.Any())
            {
                string rows = string.Join(", ", invalidIssueRows.Select(x => x.RowNo));

                message += $"IssueDate invalid in row(s): {rows}. "
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
        // Helper: Convert Excel serials or DateTime into nullable DateTime
        private DateTime? NormalizeExcelDate(object rawValue)
        {
            if (rawValue == null || rawValue == DBNull.Value)
                return null;

            if (int.TryParse(rawValue.ToString(), out int serial))
                return new DateTime(1899, 12, 30).AddDays(serial);

            if (rawValue is DateTime dt)
                return dt;

            return null;
        }

        private bool IsDateInDDMMYYYY(DateTime? date)
        {
            if (date == null)
                return false;

            // string formatted = date.Value.ToString("dd/MM/yyyy");

            string formatted =
    date.Value.ToString("dd/MM/yyyy")
              .Replace("-", "/");
            DateTime tempDate;

            return DateTime.TryParseExact(
                formatted,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out tempDate);
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
