using BALNBank;
using BOLNBank;
using NBank.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        bool Isvalid = false;
        string Message = "";
        clsAccount obj;
        List<clsState> list;
       public AccountList objAccountList;
        string MessageTitle = "Account Master";

         public   long AccountID = 0;

        public Account()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtAccountName);

            try
            {
               

                list = (new BALState().GetState());

                //list.Insert(new clsState() {  StateID = -1, StateName ="--Please--"});
                list.Insert(0, new clsState() { StateID = -1, StateName = "--Select State--" });
                cmbState.ItemsSource = list;
                cmbState.DisplayMemberPath = "StateName";
                cmbState.SelectedValuePath = "StateID";

           

                if (AccountID > 0)
                {
                    GetAccount();
                    btnSave.Content = "_Update";
                }
                else
                {
                    cmbState.SelectedValue = -1;
                    chkIsActive.IsChecked = true;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidate())
                {
                    if (AccountID > 0)
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
        private void GetAccount()
        {
            try
            {
                obj = new clsAccount();
                obj = (new BALAccount().GetAccount(AccountID));

                txtAccountName.Text = obj.AccountName;
                txtAccountShortName.Text = obj.AccountShortName;
                txtAddress1.Text = obj.Address1;
                txtAddress2.Text = obj.Address2;
                txtAddress3.Text = obj.Address3;
                //CategoryID.Text = obj.CategoryID;

                if (obj.StateID == 0)
                {
                    cmbState.SelectedValue = -1;
                }
                else
                {
                    cmbState.SelectedValue = obj.StateID;
                }



                txtCityName.Text = obj.CityName;
                txtContactNo1.Text = obj.ContactNo1;
                txtContactNo2.Text = obj.ContactNo2;
                txtContactPersonName.Text = obj.ContactPersonName;
                txtCSTNo.Text = obj.CSTNo;
                txtFaxNo.Text = obj.FaxNo;
                txtNotes.Text = obj.Notes;
                txtOpeningBalance.Text = Convert.ToString(obj.OpeningBalance);
                txtVATNo.Text = obj.VATNo;
                txtPinCode.Text = obj.PinCode;
                txtPANNo.Text = obj.PANNo;
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {  try
            {
             // objAccountList.GetAccount();
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private bool IsValidate()
        {
            Message = "";
            if (txtAccountName.Text.Trim() == "")
            {

                Message = "Please Enter Account Name \n" ;

               
              
            }
            if (Convert.ToInt64(cmbState.SelectedValue) < 0)
            {
                Message += "Select State";
            }


            if (Message.Length > 0)
            {

                Isvalid = false;
                MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                Isvalid = true;
            }

           
            return Isvalid;
        }
        public void Create()
        {
            try
            {
                obj = new clsAccount();
                obj.AccountName = txtAccountName.Text.Trim();
                obj.AccountShortName = txtAccountShortName.Text.Trim();
                obj.Address1 = txtAddress1.Text.Trim();
                obj.Address2 = txtAddress2.Text.Trim();
                obj.Address3 = txtAddress3.Text.Trim();
                obj.CityName = txtCityName.Text.Trim();
                obj.PinCode = txtPinCode.Text.Trim();

                obj.StateID = Convert.ToInt64(cmbState.SelectedValue.ToString());

                obj.VATNo = txtVATNo.Text.Trim();
                obj.CSTNo = txtCSTNo.Text.Trim();
                obj.PANNo = txtPANNo.Text.Trim();
                obj.ContactNo1 = txtContactNo1.Text.Trim();
                obj.ContactNo2 = txtContactNo2.Text.Trim();
                obj.FaxNo = txtFaxNo.Text.Trim();
                obj.ContactPersonName = txtContactPersonName.Text.Trim();
                obj.Notes = txtNotes.Text.Trim();
                if (txtOpeningBalance.Text.Trim().Length > 0)
                {
                    obj.OpeningBalance = Convert.ToDouble(txtOpeningBalance.Text.Trim());
                }
                obj.CreatedDate = DateTime.Now;
                obj.UpdateDate = DateTime.Now;
                obj.UserID = 1;

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
                    //  objAccountList.GetAccount();
                   // MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "Record saved successfully";
                    Initialize();
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
            //lblStatus.Text = Message;
            //MessageBox.Show("Record saved successfully", "Account Master", MessageBoxButton.OK, MessageBoxImage.Information);
            //return Message;
        }
        public void Update()
        {
            try
            {
                obj = new clsAccount();
                obj.AccountID = AccountID;
                obj.AccountName = txtAccountName.Text.Trim();
                obj.AccountShortName = txtAccountShortName.Text.Trim();
                obj.Address1 = txtAddress1.Text.Trim();
                obj.Address2 = txtAddress2.Text.Trim();
                obj.Address3 = txtAddress3.Text.Trim();
                obj.CityName = txtCityName.Text.Trim();
                obj.PinCode = txtPinCode.Text.Trim();

                obj.StateID = Convert.ToInt64(cmbState.SelectedValue.ToString());

                obj.VATNo = txtVATNo.Text.Trim();
                obj.CSTNo = txtCSTNo.Text.Trim();
                obj.PANNo = txtPANNo.Text.Trim();
                obj.ContactNo1 = txtContactNo1.Text.Trim();
                obj.ContactNo2 = txtContactNo2.Text.Trim();
                obj.FaxNo = txtFaxNo.Text.Trim();
                obj.ContactPersonName = txtContactPersonName.Text.Trim();
                obj.Notes = txtNotes.Text.Trim();
                if (txtOpeningBalance.Text.Trim().Length > 0)
                {
                    obj.OpeningBalance = Convert.ToDouble(txtOpeningBalance.Text.Trim());
                }
                obj.CreatedDate = DateTime.Now;
                obj.UpdateDate = DateTime.Now;
                obj.UserID = 1;
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                Message = (new BALOperation().Update(obj));
                //lblStatus.Text = Message;
                //MessageBox.Show(Message, "Account Master", MessageBoxButton.OK, MessageBoxImage.Information);
                //return Message;
                if (Message == "SAVE")
                {
                    //  objAccountList.GetAccount();
                    //MessageBox.Show("Record updated successfully", "Account Master", MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "Record updated successfully";
                }
                else
                {
                    MessageBox.Show(Message,MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);

                    lblStatus.Text = "";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private void txtOpeningBalance_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            try
            {
                var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                    e.Handled = false;

                else
                    e.Handled = true;
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
                txtAccountName.Text = "";
                txtAccountShortName.Text = "";
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtAddress3.Text = "";
                txtCityName.Text = "";
                txtPinCode.Text = "";
                cmbState.SelectedValue = -1;
                txtCSTNo.Text = "";
                txtVATNo.Text = "";
                txtPANNo.Text = "";
                txtContactNo1.Text = "";
                txtContactNo2.Text = "";
                txtFaxNo.Text = "";
                txtContactPersonName.Text = "";
                txtOpeningBalance.Text = "";
                txtNotes.Text = "";
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

      

        private void frmAccountMaster_KeyDown(object sender, KeyEventArgs e)
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
