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
    /// Interaction logic for Company.xaml
    /// </summary>
    public partial class Company : Window
    {
        List<clsProject> plist;
        List<clsBank> blist;


        internal long CompanyID;
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "";
        clsCompany obj;
        public CompanyList objCompanyList { get; internal set; }
        public Company()
        {
            InitializeComponent();
        }

        private void frmCompany_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtCompanyName);
            blist = (new BALBank().GetBankList());
            blist.Insert(0, (new clsBank() { BankID = -1, BankName = "--Select Bank--" }));
            cmbDefaultBank.ItemsSource = blist;
            cmbDefaultBank.DisplayMemberPath = "BankName";
            cmbDefaultBank.SelectedValuePath = "BankID";
            cmbDefaultBank.SelectedValue = -1;



            plist = (new BALProject().GetProjectList());
            plist.Insert(0, (new clsProject() { ProjectID = -1, ProjectName = "--Select Project--" }));
            cmbDefaultProject.ItemsSource = plist;
            cmbDefaultProject.DisplayMemberPath = "ProjectName";
            cmbDefaultProject.SelectedValuePath = "ProjectID";
            cmbDefaultProject.SelectedValue = -1;




            try
            {
                if (CompanyID > 0)
                {
                    GetCompany();
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
                    if (CompanyID > 0)
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
                objCompanyList.GetCompanyList();
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
                if (txtCompanyName.Text.Trim() == "")
                {

                    Message += " Enter Company Name \n";
                }
                if (txtCompanyShortName.Text.Trim() == "")
                {

                    Message += " Enter Company Short Name \n";
                }
                if (Convert.ToInt64( cmbDefaultBank.SelectedValue) == -1)
                {

                    Message += " Select Default Bank \n";
                }
                /*
                if (cmbDefaultProject.SelectedIndex == -1)
                {

                    Message += " Select Default Project \n";
                }
                */

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
                obj = new clsCompany();
                obj.CompanyName = txtCompanyName.Text.Trim();
                obj.CompanyShortName = txtCompanyShortName.Text.Trim();
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                obj.BankID = Convert.ToInt64(cmbDefaultBank.SelectedValue);
                obj.ProjectID = Convert.ToInt64(cmbDefaultProject.SelectedValue);

                Message = (new BALOperation().Create(obj));
                if (Message == "SAVE")
                {
                    //  objAccountList.GetAccount();
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
                obj = new clsCompany();
                obj.CompanyName = txtCompanyName.Text.Trim();
                obj.CompanyShortName = txtCompanyShortName.Text.Trim();
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }
                obj.BankID = Convert.ToInt64(cmbDefaultBank.SelectedValue);
                obj.ProjectID = Convert.ToInt64(cmbDefaultProject.SelectedValue);
                obj.CompanyID = CompanyID;

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
        public void GetCompany()
        {
            try
            {
                obj = new clsCompany();
                obj = (new BALCompany().GetCompany(CompanyID));
                txtCompanyName.Text = obj.CompanyName;
                txtCompanyShortName.Text = obj.CompanyShortName;


                cmbDefaultBank.SelectedValue = obj.BankID;
                cmbDefaultProject.SelectedValue = obj.ProjectID;

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
                txtCompanyName.Text = "";
                txtCompanyShortName.Text = "";
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmCompany_KeyDown(object sender, KeyEventArgs e)
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
