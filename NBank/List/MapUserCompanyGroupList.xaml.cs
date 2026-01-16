using BALNBank;
using BOLNBank;
using NBank.Master;
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

namespace NBank.List
{
    /// <summary>
    /// Interaction logic for MapUserCompanyGroupList.xaml
    /// </summary>
    public partial class MapUserCompanyGroupList : Window
    {
        List<clsUserCompanyGroupMapping> list;
        string MessageTitle = "Map User Company Group List";
        string MenuName = "MenuMapUserCompanyGroup";
        List<clsUserMenu> FilteredUserMenuList;

        public long UserId = 0;
        public MapUserCompanyGroupList()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserMenu();
                Keyboard.Focus(txtUserName);
                GetMapCompanyGroupList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetMapCompanyGroupList()
        {
            try
            {
                list = (new BALMapUserCompanyGroup().GetList());
                dgMapUserCompanyGroupList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MapUserCompanyGroup obj = new MapUserCompanyGroup();
                obj.objMapCompanyGroupList = this;
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
                if (dgMapUserCompanyGroupList.SelectedIndex != -1)
                {
                    clsUserCompanyGroupMapping obj = dgMapUserCompanyGroupList.SelectedItem as clsUserCompanyGroupMapping;
                    UserId = obj.UserId;
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //BankName = txtCompanyGroupName.Text.Trim();
                //list = (new BALMapCompanyGroup().GetMapCompanyGroupList(BankName));
                //dgMapCompanyGroupList.ItemsSource = list;
                //lblStatus.Text = "Rows " + list.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void dgMapUserCompanyGroupList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        clsUserCompanyGroupMapping obj = dgMapUserCompanyGroupList.SelectedItem as clsUserCompanyGroupMapping;
                        UserId = obj.UserId;
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
        private void Edit()
        {
            try
            {
                MapUserCompanyGroup obj = new MapUserCompanyGroup();
                obj.UserId = UserId;
                obj.objMapCompanyGroupList = this;
                obj.ShowDialog();
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
    }
}
