using NBank.Ledger;
using NBank.List;
using NBank.Master;
using NBank.Other;
using NBank.Report;
using NBank.Transaction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NBank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Message = "";
        string MessageTitle = "Main NBank";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetVersion();
                // MessageBox.Show(version);

                //MenuTransaction.Visibility = Visibility.Collapsed;
                // MenuFile.Visibility = Visibility.Collapsed;
                //MenuUser.Visibility = Visibility.Collapsed;
                // MenuTransaction.Visibility = Visibility.Visible;
                //  MenuFile.Visibility = Visibility.Visible;
                //MenuUser.Visibility = Visibility.Visible;
                //HideMenu.Visibility = Visibility.Collapsed;

                Login obj = new Login();
                obj.ShowDialog();
                if (Globals.UserID > 0)
                {
                    Globals.UserMenuList = (new BALNBank.BALUserMenu().GetUserMenu(Globals.UserID));
                    HideMenu();

                }
                lblStatus.Content = Globals.UserName;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void HideMenu() {

            try
            {
                if (Globals.UserMenuList.Count > 0){

                    foreach (var m in Globals.UserMenuList)
                    {
                        if (
                            (m.AllowCreate == false) &&
                            (m.AllowView == false) &&
                            (m.AllowEdit == false) &&
                            (m.AllowPrint == false) &&
                            (m.AllowDelete == false)
                            )
                        {
                            /*Master*/
                            if (m.MenuName == "MenuAccount")
                            {
                                MenuAccount.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuCompany")
                            {
                                MenuCompany.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuProject")
                            {
                                MenuProject.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuBank")
                            {
                                MenuBank.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuCompanyWiseBank")
                            {
                                MenuCompanyWiseBank.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuType")
                            {
                                MenuType.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuSubType")
                            {
                                MenuSubType.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuGroup")
                            {
                                MenuGroup.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuCategory")
                            {
                                MenuCategory.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuParameter")
                            {
                                MenuParameter.Visibility = Visibility.Collapsed;
                            }
                            /*Transaction*/
                            if (m.MenuName == "MenuChequeEntry")
                            {
                                MenuChequeEntry.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuChequeClear")
                            {
                                MenuChequeClear.Visibility = Visibility.Collapsed;
                            }
                            /*Report*/
                            if (m.MenuName == "MenuLedger")
                            {
                                MenuLedger.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuReport")
                            {
                                MenuReport.Visibility = Visibility.Collapsed;
                            }
                            /*User Access*/
                            if (m.MenuName == "MenuUser")
                            {
                                MenuUser.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuChangePassword")
                            {
                                MenuChangePassword.Visibility = Visibility.Collapsed;
                            }
                            if (m.MenuName == "MenuUserAccess")
                            {
                                MenuUserAccess.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
                else {
                    MenuAccount.Visibility         = Visibility.Collapsed;
                    MenuCompany.Visibility         = Visibility.Collapsed;
                    MenuProject.Visibility         = Visibility.Collapsed;
                    MenuBank.Visibility            = Visibility.Collapsed;
                    MenuCompanyWiseBank.Visibility = Visibility.Collapsed;
                    MenuType.Visibility            = Visibility.Collapsed;
                    MenuSubType.Visibility         = Visibility.Collapsed;
                    MenuGroup.Visibility           = Visibility.Collapsed;
                    MenuCategory.Visibility        = Visibility.Collapsed;
                    MenuParameter.Visibility       = Visibility.Collapsed;
                    MenuChequeEntry.Visibility     = Visibility.Collapsed;
                    MenuChequeClear.Visibility     = Visibility.Collapsed;
                    MenuLedger.Visibility          = Visibility.Collapsed;
                    MenuReport.Visibility          = Visibility.Collapsed;
                    MenuUser.Visibility            = Visibility.Collapsed;
                    MenuChangePassword.Visibility  = Visibility.Collapsed;
                    MenuUserAccess.Visibility      = Visibility.Collapsed;
                    MenuState.Visibility           = Visibility.Collapsed;
                }

               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private void MenuAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountList obj = new AccountList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuBank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BankList obj = new BankList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProjectList obj = new ProjectList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompanyList obj = new CompanyList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TypeList obj = new TypeList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuSubType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SubTypeList obj = new SubTypeList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error); throw;
            }
        }

        private void MenuGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GroupList obj = new GroupList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CategoryList obj = new CategoryList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuParameter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParameterList obj = new ParameterList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuCompanyWiseBank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompanyWiseBankList obj = new CompanyWiseBankList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuChequeEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChequeEntryList obj = new ChequeEntryList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserList obj = new UserList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MenuChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangePassword obj = new ChangePassword();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MenuLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login obj = new Login();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuChequeClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChequeClear obj = new ChequeClear();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MenuLedger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountLedgerList obj = new AccountLedgerList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportFilter obj = new ReportFilter();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Email_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(e.Uri.AbsoluteUri);
                e.Handled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuUserAccess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserMenu obj = new UserMenu();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string GetVersion() {
            string version = "";

            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                version = fvi.FileVersion;
                lblVersion.Content = "Version:"+  fvi.FileVersion;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return version;

        }

        private void MenuState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateList obj = new StateList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuCompanyGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompanyGroupList obj = new CompanyGroupList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuMapCompanyGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MapCompanyGroupList obj = new MapCompanyGroupList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MenuMapUserCompanyGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MapUserCompanyGroupList obj = new MapUserCompanyGroupList();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //MenuMapUserCompanyGroup_Click
        private void frmMain_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.F6)
                {
                    ChequeEntryList obj = new ChequeEntryList();
                    obj.ShowDialog();
                }
                if (e.Key == Key.F7)
                {
                    ChequeClear obj = new ChequeClear();
                    obj.ShowDialog();

                }
                if (e.Key == Key.F8)
                {
                    AccountLedgerList obj = new AccountLedgerList();
                    obj.ShowDialog();

                }
                if (e.Key == Key.F5)
                {
                    AccountList obj = new AccountList();
                    obj.ShowDialog();

                }
                if (e.Key == Key.F9)
                {
                    TypeList obj = new TypeList();
                    obj.ShowDialog();

                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error); throw;
            }
        }

        private void MenuMerge_Click(object sender, RoutedEventArgs e)
        {
            Merge.Merge obj = new Merge.Merge();
            obj.ShowDialog();
        }

        private void MenuMergeAccount_Click(object sender, RoutedEventArgs e)
        {
            Merge.MergeAccount obj = new Merge.MergeAccount();
            obj.ShowDialog();
        }
    }
}
