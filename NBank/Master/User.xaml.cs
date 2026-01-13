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
using NBank.List;
using BOLNBank;
using BALNBank;

namespace NBank.Master
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        internal long UserID;
        public UserList objUserList { get; internal set; }
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "User Master";
        clsUser obj;
        public User()
        {
            InitializeComponent();
        }

      

        private void frmUser_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtUserName);
            try
            {
                if (UserID > 0)
                {
                    
                    RowPassword.Height = new GridLength(0);
                    RowPasswordBorder.Height = new GridLength(0);
                    GetUser();
                    btnSave.Content = "_Update";
                }
                else {
                    chkIsActive.IsChecked = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidate())
                {
                    if (UserID > 0)
                    {
                      
                        Update();
                    }
                    else
                    {
                        Create();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objUserList.GetUserList();

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void GetUser()
        {
            try
            {
                obj = new clsUser();
                obj = (new BALUser().GetUser(UserID));
                txtUserName.Text = obj.UserName;
                txtContactNo.Text = obj.ContactNo;
                txtEmailAddress.Text = obj.EmailAddress;
                txtFirstName.Text = obj.FirstName;
                txtLastName.Text = obj.LastName;
                txtMobileNo.Text = obj.MobileNo;
                txtPassword.Password = obj.UserPassword;


                if (obj.IsActive == true)
                {
                    chkIsActive.IsChecked = true;
                }
                else
                {
                    chkIsActive.IsChecked = false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        public bool IsValidate()
        {
            try
            {
                Message = "";
                //MessageBox.Show(cmbDefaultBank.SelectedIndex.ToString());
                if (txtUserName.Text.Trim() == "")
                {

                    Message += " Enter Username \n";
                }
                else {

                    if (txtUserName.Text.Trim().Length < 5)
                    {
                        Message += " Username should be minimum 5 characters \n";
                    }
                }
                if (UserID == 0)
                {
                    if (txtPassword.Password.Trim() == "")
                    {
                        Message += " Enter Password \n";
                    }
                    else {
                        if (txtPassword.Password.Trim().Length < 6)
                        {
                            Message += " Password should be minimum 6 characters \n";
                        }
                    }
                    
                }

                if (txtEmailAddress.Text != "") {
                    if (!IsValidEmail(txtEmailAddress.Text))
                    {
                        Message += " Enter valid email address \n";
                    }
                }

                
                if (Message.Length > 0)
                {
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                    Isvalid = false;
                }
                else
                {
                    Isvalid = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information); throw;
            }

            return Isvalid;
        }
        public void Create()
        {
            try
            {
                obj = new clsUser();
                obj.UserName = txtUserName.Text.Trim();
                obj.ContactNo = txtContactNo.Text.Trim();
                obj.EmailAddress = txtEmailAddress.Text.Trim();
                obj.FirstName = txtFirstName.Text.Trim();
                obj.LastName = txtLastName.Text.Trim();
                obj.MobileNo = txtMobileNo.Text.Trim();
                obj.UserPassword = txtPassword.Password.Trim();


                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                Message = (new BALOperation().Create(obj));
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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void Update()
        {
            try
            {
                obj = new clsUser();
                obj = new clsUser();
                obj.UserName = txtUserName.Text.Trim();
                obj.ContactNo = txtContactNo.Text.Trim();
                obj.EmailAddress = txtEmailAddress.Text.Trim();
                obj.FirstName = txtFirstName.Text.Trim();
                obj.LastName = txtLastName.Text.Trim();
                obj.MobileNo = txtMobileNo.Text.Trim();
                obj.UserPassword = txtPassword.Password.Trim();
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                obj.UserID = UserID;

                Message = (new BALOperation().Update(obj));
                if (Message == "SAVE")
                {
                    lblStatus.Text = "Record saved successfully";
                    //MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Initialize()
        {
            try
            {
                txtContactNo.Text = "";
                txtEmailAddress.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtMobileNo.Text = "";
                txtPassword.Password = "";
                txtUserName.Text = "";
             
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void frmUser_KeyDown(object sender, KeyEventArgs e)
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
