using BALNBank;
using BOLNBank;
using NBank.List;
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

namespace NBank.Master
{
    /// <summary>
    /// Interaction logic for Bank.xaml
    /// </summary>
    public partial class Bank : Window
    {
        public long BankID = 0;
        clsBank obj;
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "Bank Master";
        public  BankList objBankList;
        public Bank()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtBankName);
            try
            {
                if (BankID > 0)
                {
                    GetBank();
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
                    if (BankID > 0)
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
        public bool IsValidate()
        {
            try
            {
                Message = "";
                if (txtBankName.Text.Trim() == "")
                {

                    Message += " Enter Bank Name \n";
                }
                if (txtBankCode.Text.Trim() == "")
                {

                    Message += " Enter Bank Code ";
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

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return Isvalid;

        }
        public void Create()
        {
            try
            {
                obj = new clsBank();
                obj.BankCode = txtBankCode.Text.Trim();
                obj.BankName = txtBankName.Text.Trim();
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                Message = (new BALOperation().Create(obj));
                if (Message == "SAVE"){
                    //MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "Record saved successfully";
                    Initialize();
                }
                else{
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "";
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
                obj = new clsBank();
                obj.BankCode = txtBankCode.Text.Trim();
                obj.BankName = txtBankName.Text.Trim();
                obj.BankID = BankID;
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }


                Message = (new BALOperation().Update(obj));
                if (Message == "SAVE")
                {
                    //MessageBox.Show("Record updated successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "Record updated successfully";
                }
                else
                {
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "";
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
                objBankList.GetBankList();

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void GetBank()
        {
            try
            {
                obj = new clsBank();
                obj = (new BALBank().GetBank(BankID));
                txtBankName.Text = obj.BankName;
                txtBankCode.Text = obj.BankCode;
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
        private void Initialize()
        {
            try
            {
                txtBankCode.Text = "";
                txtBankCode.Text = "";
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmBank_KeyDown(object sender, KeyEventArgs e)
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
