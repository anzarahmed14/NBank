using BALNBank;
using BOLNBank;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace NBank.Master
{
    /// <summary>
    /// Interaction logic for UserMenu.xaml
    /// </summary>
    public partial class UserMenu : Window
    {
        bool Isvalid = false;
        List<clsUser> list;
        List<clsMenu> listMenu;
        List<clsUserMenu> listUserMenu;
        string UserName = "";
        long UserID = 0;
        string MessageTitle = "User Menu";
        string Message = "";
        long SelectedUserID = 0;
        public UserMenu()
        {
            InitializeComponent();
        }

        private void frmUserMenu_Loaded(object sender, RoutedEventArgs e)
        {
            GetUserList();
            GetUserMenuList();
        }
        public void GetUserList()
        {
            try
            {
                list = (new BALUser().GetUserList());

                list.Insert(0, new clsUser() { UserID = -1, UserName = "--Select User Name--" });
                cmbUserName.ItemsSource = list;
                cmbUserName.DisplayMemberPath = "UserName";
                cmbUserName.SelectedValuePath = "UserID";
                cmbUserName.SelectedValue = -1;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetUserMenuList()
        {
            try
            {
                listMenu = (new BALMenu().GetMenuList());
                dgUserMenuList.ItemsSource = listMenu;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkAllowCreate_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                foreach (var row in listMenu)
                {

                    bool Test = row.AllowCreate;
                    row.AllowCreate = true;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkAllowCreate_Unchecked(object sender, RoutedEventArgs e)
        {
            //listMenu = (new BALMenu().GetMenuList());

            try
            {
                foreach (var row in listMenu)
                {

                    bool Test = row.AllowCreate;
                    row.AllowCreate = false;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void chkAllowView_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var row in listMenu)
                {


                    row.AllowView = true;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkAllowView_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var row in listMenu)
                {


                    row.AllowView = false;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void chkAllowDelete_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var row in listMenu)
                {


                    row.AllowDelete = true;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void chkAllowDelete_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                foreach (var row in listMenu)
                {


                    row.AllowDelete = false;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void chkAllowEdit_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var row in listMenu)
                {


                    row.AllowEdit = true;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void chkAllowEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var row in listMenu)
                {
                    row.AllowEdit = false;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       

        private void chkAllowPrint_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var row in listMenu)
                {
                    row.AllowEdit = true;
                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void chkAllowPrint_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var row in listMenu)
                {
                    row.AllowPrint = false;

                }

                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidate()) {
                    SelectedUserID = Convert.ToInt64(cmbUserName.SelectedValue);

                    listUserMenu = new List<clsUserMenu>();
                    foreach (clsMenu m in dgUserMenuList.ItemsSource)
                    {
                        //bool isChecked = p.AllowCreate; 
                        listUserMenu.Add(new clsUserMenu {  UserID = SelectedUserID,MenuID = m.MenuID, MenuName = "", AllowCreate = m.AllowCreate, AllowView = m.AllowView, AllowDelete = m.AllowDelete, AllowEdit = m.AllowEdit,  AllowPrint = m.AllowPrint });
                    }
                    Message = (new BALUserMenu().CreateUserMenu(listUserMenu));

                    if (Message == "SAVE")
                    {
                        //MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                        lblStatus.Text = "Record saved successfully";
                    }
                    else
                    {
                        MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,MessageTitle, MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void cmbUserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbUserName.SelectedValue != null)
                {
                    SelectedUserID = Convert.ToInt64(cmbUserName.SelectedValue);
                    UpdateUserMenu(SelectedUserID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void UpdateUserMenu(long UserID )
        {
            try
            {
                listUserMenu = new List<clsUserMenu>();
                listUserMenu = (new BALUserMenu().GetUserMenu(SelectedUserID));

                if (listUserMenu.Count > 0)
                {
                    foreach (var row in listMenu)
                    {
                        foreach (clsUserMenu m in listUserMenu)
                        {
                            if (row.MenuID == m.MenuID)
                            {
                                row.AllowCreate = m.AllowCreate;
                                row.AllowView = m.AllowView;
                                row.AllowPrint = m.AllowPrint;
                                row.AllowDelete = m.AllowDelete;
                                row.AllowEdit = m.AllowEdit;

                            }
                        }

                    }
                }
                else
                {
                    foreach (var row in listMenu)
                    {
                        row.AllowCreate = false;
                        row.AllowView = false;
                        row.AllowPrint = false;
                        row.AllowDelete = false;
                        row.AllowEdit = false;
                    }
                }
                dgUserMenuList.ItemsSource = listMenu;
                dgUserMenuList.Items.Refresh();



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
        private bool IsValidate()
        {
            Message = "";

            try
            {
                if (Convert.ToInt64(cmbUserName.SelectedValue) < 0)
                {
                    Message += "Select User Name";
                }


                if (Message.Length > 0)
                {

                    Isvalid = false;
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Isvalid = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return Isvalid;
        }

        private void frmUserMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (e.IsToggled == true || (sender as Button) == null))
            {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(request);

                    e.Handled = true;
                }
            }
        }
    }
}
