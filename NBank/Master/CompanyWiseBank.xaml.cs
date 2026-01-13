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
    /// Interaction logic for CompanyWiseBank.xaml
    /// </summary>
    public partial class CompanyWiseBank : Window
    {
        List<clsBank> objBankList;
        List<clsCompany> objCompanyList;
        List<string> plist;
        clsCompanyWiseBank obj;
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "";
        public CompanyWiseBankList objCompanyWiseBankList { get; internal set; }
        public long CompanyWiseBankID; 
        public CompanyWiseBank()
        {
            InitializeComponent();
        }

       

        private void frmCompanyWiseBank_Loaded(object sender, RoutedEventArgs e)
        {
            objBankList = new List<clsBank>();
            objBankList = (new BALBank().GetBankList());
            cmbBankName.ItemsSource = objBankList;
            cmbBankName.DisplayMemberPath = "BankName";
            cmbBankName.SelectedValuePath = "BankID";

            objCompanyList = new List<clsCompany>();
            objCompanyList = (new BALCompany().GetCompanyList());
            cmbCompanyName.ItemsSource = objCompanyList;
            cmbCompanyName.DisplayMemberPath = "CompanyName";
            cmbCompanyName.SelectedValuePath = "CompanyID";

          //  Keyboard.Focus(txtCompanyWiseBankName);
            try
            {
                if (CompanyWiseBankID > 0)
                {
                    GetCompanyWiseBank();
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
                    if (CompanyWiseBankID > 0)
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

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objCompanyWiseBankList.GetCompanyWiseBankList();

                Close();
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
                if (cmbBankName.SelectedIndex == -1)
                {

                    Message += " Please Select Bank Name \n";
                }
                if (cmbCompanyName.SelectedIndex == -1)
                {

                    Message += " Please Select Company Name  \n";
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
            }
            catch (Exception ex) 
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return Isvalid;
        }
        public void GetCompanyWiseBank()
        {
            try
            {
                obj = new clsCompanyWiseBank();
                obj = (new BALCompanyWiseBank().GetCompanyWiseBank(CompanyWiseBankID));
                cmbBankName.SelectedValue = obj.BankID;
                cmbCompanyName.SelectedValue = obj.CompanyID;
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
        public void Create()
        {
            try
            {
                obj = new clsCompanyWiseBank();
                obj.BankID = Convert.ToInt64(cmbBankName.SelectedValue);
                obj.CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }



                Message = (new BALOperation().Create(obj, GetParameterList()));
                if (Message == "SAVE")
                {
                    //MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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
        }
        public void Update()
        {
            try
            {
                obj = new clsCompanyWiseBank();
                obj.BankID = Convert.ToInt64(cmbBankName.SelectedValue);
                obj.CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                obj.CompanyWiseBankID = CompanyWiseBankID;

                Message = (new BALOperation().Update(obj, GetParameterList()));
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
        public List<string> GetParameterList()
        {
            try
            {
                plist = new List<string>();
                plist.Add("CompanyWiseBankID");
                plist.Add("BankID");
                plist.Add("CompanyID");
                plist.Add("IsActive");
                plist.Add("CompanyWiseBankCode");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return plist;
        }
        private void Initialize()
        {
            try
            {
                cmbBankName.SelectedValue = -1;
                cmbBankName.SelectedValue = -1;
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmCompanyWiseBank_KeyDown(object sender, KeyEventArgs e)
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
