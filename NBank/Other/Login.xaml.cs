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
using System.Data;
using BOLNBank;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace NBank.Other
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        /*Hide Close Buttom*/
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        /*End Hide Close Buttom*/


        bool Isvalid = false;
        string MessageTitle = "Login";
        string Message = "";
        DataSet _ds;
        public Login()
        {
            InitializeComponent();
        }

        private void frmLogin_Loaded(object sender, RoutedEventArgs e)
        {
            /*Hide Close Buttom*/
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            /*Hide Close Buttom*/

            Keyboard.Focus(txtUserName);

            if (Environment.MachineName == "INFO") {
                txtUserName.Text = "Anzar";
                txtPassword.Password = "Anzar@2014";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidate())
                {
                   
                    _ds = (new BALNBank.BALUser().UserLogin(txtUserName.Text, txtPassword.Password));
                    if (_ds != null && _ds.Tables.Count>0)
                    {
                        if (_ds.Tables[0].Rows.Count > 0)
                        {
                            // Globals.UserID = 20;

                            foreach (DataRow row in _ds.Tables[0].Rows) {
                                Globals.UserID = Convert.ToInt64(row["UserID"].ToString());
                                Globals.UserName =row["UserName"].ToString();
                                Globals.FirstName = row["FirstName"].ToString();

                               
                            }
                            Close();
                        }
                        else
                        {

                            Message = "Invalid Username or Password";
                        }
                    }
                    else
                    {
                        Message = "Invalid Username or Password";
                        lblStatus.Text = Message;
                        MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
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

            // Close();
            System.Windows.Application.Current.Shutdown();

        }
        public bool IsValidate()
        {
            Message = "";
            //MessageBox.Show(cmbDefaultBank.SelectedIndex.ToString());
            if (txtUserName.Text.Trim() == "")
            {
                Message += "Enter User Name\n";
            }
            if (txtPassword.Password.Trim() == "")
            {

                Message += "Enter Password\n";
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

        private void txtPassword_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                
               // btnSave.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        /*For Close buttom*/

    }
}
