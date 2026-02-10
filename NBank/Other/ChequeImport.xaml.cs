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

namespace NBank.Other
{
    /// <summary>
    /// Interaction logic for ChequeImport.xaml
    /// </summary>
    public partial class ChequeImport : Window
    {
        public ChequeImport()
        {
            InitializeComponent();
            MessageBox.Show("dsadsa");
        }
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Excel Files|*.xlsx";

            if (dlg.ShowDialog() == true)
            {
                txtFilePath.Text = dlg.FileName;
            }
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Select file first");
                return;
            }

            var list = ReadExcel(txtFilePath.Text);
            ValidatePartyName(list);
            ValidateParameter(list);
            ValidateBankCode(list);

            ValidateSubType(list);
            ValidateType(list);
            dgImport.ItemsSource = list;

            txtSummary.Text =
                $"File Rows: {list.Count}";
        }
        private List<ChequeImportModel> ReadExcel(string path)
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
                    IssueDate = DateTime.FromOADate(xlRange.Cells[i, 1].Value2),
                    CompanyCode = xlRange.Cells[i, 2]?.Value2?.ToString(),
                    BankCode = xlRange.Cells[i, 3]?.Value2?.ToString(),
                    ProjectCode = xlRange.Cells[i, 4]?.Value2?.ToString(),
                    SubType = xlRange.Cells[i, 6]?.Value2?.ToString(),

                    PartyName =
                        xlRange.Cells[i, 9]?.Value2?.ToString(),

                    ChequeNo =
                        xlRange.Cells[i, 8]?.Value2?.ToString(),

                    ChequeAmount =
                        Convert.ToDecimal(
                            xlRange.Cells[i, 10]?.Value2 ?? 0),

                    Type =
                        xlRange.Cells[i, 5]?.Value2?.ToString(),

                    Parameter =
                        xlRange.Cells[i, 7]?.Value2?.ToString(),

                    NBankAccountName = ""
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            dgImport.ItemsSource = null;
            txtSummary.Text = "";
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
        private void ValidatePartyName( List<ChequeImportModel> list)
        {
            var accountDict =(new BALAccount().LoadAccounts());

            int matched = 0;
            int notMatched = 0;

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.PartyName))
                {
                    item.NBankAccountName = "Not Found";
                    item.IsAccountValid = false;
                    notMatched++;
                    continue;
                }

                string excelName =
                    item.PartyName
                    .Trim()
                    .ToUpper();

                if (accountDict.ContainsKey(excelName))
                {
                    // Matched
                    item.NBankAccountName =
                        item.PartyName;

                    item.IsAccountValid = true;
                    matched++;
                }
                else
                {
                    // Not Found
                    item.NBankAccountName = "Not Found";
                    item.IsAccountValid = false;
                    notMatched++;
                }
            }

            txtSummary.Text =
                $"File Rows: {list.Count}, " +
                $"Matched: {matched}, " +
                $"Not Matched: {notMatched}";
        }
        private void ValidateParameter( List<ChequeImportModel> list)
        {
            var paramDict =
                (new BALParameter().LoadParameters());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.Parameter))
                {
                    item.IsParameterValid = false;
                    continue;
                }

                string excelParam =
                    item.Parameter
                    .Trim()
                    .ToUpper();

                if (paramDict.ContainsKey(excelParam))
                {
                    item.IsParameterValid = true;
                }
                else
                {
                    item.IsParameterValid = false;
                }
            }
        }
        private void ValidateSubType( List<ChequeImportModel> list)
        {
            var subTypeDict = (new BALSubType(). LoadSubTypes());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.SubType))
                {
                    item.IsSubTypeValid = false;
                    continue;
                }

                string excelSubType =
                    item.SubType
                    .Trim()
                    .ToUpper();

                if (subTypeDict.ContainsKey(excelSubType))
                {
                    item.IsSubTypeValid = true;
                }
                else
                {
                    item.IsSubTypeValid = false;
                }
            }
        }
        private void ValidateType( List<ChequeImportModel> list)
        {
            var typeDict = (new BALType(). LoadTypes());

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.Type))
                {
                    item.IsTypeValid = false;
                    continue;
                }

                string excelType =
                    item.Type
                    .Trim()
                    .ToUpper();

                if (typeDict.ContainsKey(excelType))
                {
                    item.IsTypeValid = true;
                }
                else
                {
                    item.IsTypeValid = false;
                }
            }
        }

    }
}
