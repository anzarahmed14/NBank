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

namespace NBank.Other
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        long UserID = 1;
        string OldPassword = "";
        string NewPassword = "";
        string Message = "";
        bool Isvalid = false;
        string MessageTitle = "Change Password";
        List<clsUserMenu> FilteredUserMenuList;
        string MenuName = "MenuChangePassword";
        public ChangePassword()
        {
            InitializeComponent();
        }
        private void frmChangePassword_Loaded(object sender, RoutedEventArgs e)
        {
            UserMenu();
            UserID = Globals.UserID;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (IsValidate())
            {
                OldPassword = txtOldPassword.Password.Trim();
                NewPassword = txtNewPassword.Password.Trim();
                Message = (new BALNBank.BALUser().ChangePassword(OldPassword, NewPassword, UserID));
                if (Message == "SAVE")
                {
                    lblStatus.Text = "Record saved successfully";
                    //MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    Initialize();
                }
                else
                {
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public bool IsValidate()
        {
            Message = "";
            //MessageBox.Show(cmbDefaultBank.SelectedIndex.ToString());
            if (txtOldPassword.Password.Trim() == "")
            {
                Message += "Enter Old Password\n";
            }
            if (txtNewPassword.Password.Trim() == "")
            {

                Message += "Enter New Password\n";
            }
            if (txtConfirmNewPassword.Password.Trim() == "")
            {


                Message += "Enter Confirm New Password\n";
            }

            if (txtNewPassword.Password.Trim() != "" && txtConfirmNewPassword.Password.Trim() != "")
            {
                if (!string.Equals(txtNewPassword.Password.Trim(), txtConfirmNewPassword.Password.Trim(), StringComparison.CurrentCulture))
                {
                    Message += "Your new password and confirmation password do not match\n";
                }
                else
                {
                    if (txtNewPassword.Password.Trim().Length < 6)
                    {
                        Message += " Password should be minimum 6 characters \n";
                    }
                }

            }



            if (Message.Length > 0)
            {
                MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                Isvalid = false;
            }
            else
            {
                Isvalid = true;
            }

            return Isvalid;

        }
        private void Initialize()
        {
            try
            {
                txtConfirmNewPassword.Password = "";
                txtNewPassword.Password = "";
                txtOldPassword.Password = "";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void frmChangePassword_KeyDown(object sender, KeyEventArgs e)
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
        private void UserMenu()
        {

            try
            {
                FilteredUserMenuList = Globals.UserMenuList.Where(x => x.MenuName == MenuName).ToList();

                if (FilteredUserMenuList.Count > 0)
                {
                    if (FilteredUserMenuList[0].AllowCreate == false)
                    {
                        btnSave.Visibility = Visibility.Collapsed;
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
