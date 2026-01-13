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
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : Window
    {
        List<clsUser> list;
        string UserName = "";
        long UserID = 0;
        string MessageTitle = "User List";

        string MenuName = "MenuUser";
        List<clsUserMenu> FilteredUserMenuList;

        public UserList()
        {
            InitializeComponent();
        }

        private void frmUserList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserMenu();
                Keyboard.Focus(txtUserName);
                GetUserList();
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
        private void dgUserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        clsUser obj = dgUserList.SelectedItem as clsUser;
                        UserID = obj.UserID;
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

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                UserName = txtUserName.Text.Trim();
                list = (new BALUser().GetUserList(UserName));
                dgUserList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
            }
            catch (Exception ex) 
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetUserList()
        {
            try
            {
                list = (new BALUser().GetUserList());
                dgUserList.ItemsSource = list;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Edit()
        {
            try
            {
                User obj = new User();
                obj.UserID = UserID;
                obj.objUserList = this;
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
                if (dgUserList.SelectedIndex != -1)
                {
                    clsUser obj = dgUserList.SelectedItem as clsUser;
                    UserID = obj.UserID;
                    Edit();
                    // process stuff
                }
                else
                {
                    MessageBox.Show("Please Select User Name", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User obj = new User();
                obj.objUserList = this;
                obj.ShowDialog();
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
    }
}
