using BALNBank;
using BOLNBank;
using NBank.Report;
using NBank.Transaction;
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

namespace NBank.Other
{
    /// <summary>
    /// Interaction logic for AccountPopup.xaml
    /// </summary>
    public partial class AccountPopup : Window
    {
        List<clsAccount> list;
        long AccountID = 0;
        string AccountName = "";
        internal ChequeEntry objChequeEntry;

        internal ReportFilter objReportFilter;
        internal string ComingFrom = "";
        string MessageTitle = "Account Popup";
        public AccountPopup()
        {
            InitializeComponent();
        }

        private void frmAccountPopup_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtAccountName);
            GetAccount();
        }

        private void txtAccountName_TextChanged(object sender, TextChangedEventArgs e)
        {
            AccountName = txtAccountName.Text.Trim();
            list = (new BALAccount().GetAccountList(AccountName));
            list = list.FindAll(Active => Active.IsActive == true);

            lvAccountList.ItemsSource = list;
        }
        public void GetAccount()
        {
            list = (new BALAccount().GetAccountList());

            list = list.FindAll(Active => Active.IsActive == true);

            lvAccountList.ItemsSource = list;
            lvAccountList.DisplayMemberPath = "AccountName";
            lvAccountList.SelectedValuePath = "AccountID";
        }

        private void lvAccountList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (lvAccountList.SelectedIndex != -1)
                {
                    clsAccount obj = lvAccountList.SelectedItem as clsAccount;
                    //MessageBox.Show(obj.AccountName);
                    if (ComingFrom == "REPORT")
                    {
                        objReportFilter.AccountID = obj.AccountID;
                        objReportFilter.AccountName = obj.AccountName;
                    }
                    else
                    {
                        objChequeEntry.AccountID = obj.AccountID;
                        objChequeEntry.AccountName = obj.AccountName;
                    }
                    
                    Close();
                }else
                {

                    MessageBox.Show("Please Select Party Name");
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmAccountPopup_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) {
                Close();
            }
        }

        private void lvAccountList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (lvAccountList.SelectedIndex != -1)
                {
                    clsAccount obj = lvAccountList.SelectedItem as clsAccount;
                    //MessageBox.Show(obj.AccountName);
                    if (ComingFrom == "REPORT")
                    {
                        objReportFilter.AccountID = obj.AccountID;
                        objReportFilter.AccountName = obj.AccountName;
                    }
                    else
                    {
                        objChequeEntry.AccountID = obj.AccountID;
                        objChequeEntry.AccountName = obj.AccountName;
                    }

                    Close();
                }
                else
                {

                    MessageBox.Show("Please Select Party Name");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
