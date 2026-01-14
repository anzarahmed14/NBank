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
using BALNBank;
using BOLNBank;
using NBank.Master;

namespace NBank.List
{
    /// <summary>
    /// Interaction logic for CompanyGroupList.xaml
    /// </summary>
    public partial class CompanyGroupList : Window
    {
        string MessageTitle = "Company Group Master";
        string MenuName = "MenuBank";
        List<clsUserMenu> FilteredUserMenuList;
        List<clsCompanyGroup> list;
        public long CompanyGroupID = 0;
        string CompanyGroupName = "";
        public CompanyGroupList()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserMenu();
                Keyboard.Focus(txtCompanyGroupName);
                GetCompanyGroupList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetCompanyGroupList()
        {
            try
            {
                list = (new BALCompanyGroup().GetCompanyGroupList());
                dgBankList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                        btnAdd.Visibility = Visibility.Collapsed;
                    }
                    if (FilteredUserMenuList[0].AllowEdit == false)
                    {
                        btnEdit.Visibility = Visibility.Collapsed;
                    }
                }
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
            Close();
        }
        private void txtCompanyGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CompanyGroupName = txtCompanyGroupName.Text.Trim();
                list = (new BALCompanyGroup().GetCompanyGroupList(CompanyGroupName));
                dgBankList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompanyGroup obj = new CompanyGroup();
                obj.objCompanyGroupList = this;
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgBankList.SelectedIndex != -1)
                {
                    clsCompanyGroup obj = dgBankList.SelectedItem as clsCompanyGroup;
                    CompanyGroupID = obj.CompanyGroupID;
                    Edit();
                    // process stuff
                }
                else
                {
                    MessageBox.Show("Please Select Bank Name", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
                CompanyGroup obj = new CompanyGroup();
                obj.CompanyGroupID = CompanyGroupID;
                obj.objCompanyGroupList = this;
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void gdCompanyGroupList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        clsCompanyGroup obj = dgBankList.SelectedItem as clsCompanyGroup;
                        CompanyGroupID = obj.CompanyGroupID;
                        if (FilteredUserMenuList != null)
                        {
                            if (FilteredUserMenuList.Count > 0)
                            {
                                if (FilteredUserMenuList[0].AllowEdit == true)
                                {
                                    Edit();
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
}
