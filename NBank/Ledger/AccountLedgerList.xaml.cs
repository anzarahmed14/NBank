using BALNBank;
using BOLNBank;
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

namespace NBank.Ledger
{
    /// <summary>
    /// Interaction logic for AccountLedger.xaml
    /// </summary>
    public partial class AccountLedgerList : Window
    {
        List<clsAccount> list;
        List<clsProject> objProjectList;
        long AccountID = 0;
        long ProjectID = 0;
        string AccountName = "";
        string MessageTitle = "Account Ledger";
        public AccountLedgerList()
        {
            InitializeComponent();
        }

        private void frmAccountLedger_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Keyboard.Focus(txtAccountName);
                objProjectList = (new BALProject().GetProjectList());

                objProjectList.Insert(0, (new clsProject { ProjectID = -1, ProjectShortName = "--Select Project--" }));

                cmbProjectName.ItemsSource = objProjectList;
                cmbProjectName.DisplayMemberPath = "ProjectShortName";
                cmbProjectName.SelectedValuePath = "ProjectID";
                cmbProjectName.SelectedValue = -1;

                GetAccount();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void gdAccountList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        clsAccount obj = gdAccountList.SelectedItem as clsAccount;
                        AccountID = obj.AccountID;
                        Edit();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void txtAccountName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                AccountName = txtAccountName.Text.Trim();
                list = (new BALAccount().GetAccountList(AccountName));
                gdAccountList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetAccount()
        {
            try
            {
                list = (new BALAccount().GetAccountList());
                gdAccountList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Edit()
        {
            try
            {
                ProjectID = Convert.ToInt64(cmbProjectName.SelectedValue);

                AccountLedger obj = new AccountLedger();
                obj.AccountID = AccountID;
                obj.ProjectID = ProjectID;
                obj.objAccountList = this;
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnControlMinimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                }
                else if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Minimized;
                }
                else
                {
                    WindowState = WindowState.Minimized;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnControlMaximize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnControlClose_Click(object sender, RoutedEventArgs e)
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

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gdAccountList.SelectedIndex != -1)
                {
                    clsAccount obj = gdAccountList.SelectedItem as clsAccount;
                    AccountID = obj.AccountID;
                    Edit();
                    // process stuff
                }
                else
                {
                    MessageBox.Show("Please Select Account Name", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtAccountSubName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
