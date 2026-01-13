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
    /// Interaction logic for ParameterList.xaml
    /// </summary>
    public partial class ParameterList : Window
    {
        List<clsParameter> list;
        string ParameterName = "";
        long ParameterID = 0;
        string MessageTitle = "Parameter List";
        string MenuName = "MenuParameter";
        List<clsUserMenu> FilteredUserMenuList;
        public ParameterList()
        {
            InitializeComponent();
        }
        private void frmParameter_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserMenu();
                Keyboard.Focus(txtParameterName);
                GetParameterList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgParameterList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        clsParameter obj = dgParameterList.SelectedItem as clsParameter;
                        ParameterID = obj.ParameterID;
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgParameterList.SelectedIndex != -1)
                {
                    clsParameter obj = dgParameterList.SelectedItem as clsParameter;
                    ParameterID = obj.ParameterID;
                    Edit();
                    // process stuff
                }
                else
                {
                    MessageBox.Show("Please Select Parameter Name", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
                Parameter obj = new Parameter();
                obj.objParameterList = this;
                obj.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtParameterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ParameterName = txtParameterName.Text.Trim();
                list = (new BALParameter().GetParameterList(ParameterName));
                dgParameterList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetParameterList()
        {
            try
            {
                list = (new BALParameter().GetParameterList());
                dgParameterList.ItemsSource = list;
                lblStatus.Text = "Rows " + list.Count;
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
                Parameter obj = new Parameter();
                obj.ParameterID = ParameterID;
                obj.objParameterList = this;
                obj.ShowDialog();
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
    }
}
