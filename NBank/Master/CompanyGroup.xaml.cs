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
    /// Interaction logic for CompanyGroup.xaml
    /// </summary>
    public partial class CompanyGroup : Window
    {
        public CompanyGroupList objCompanyGroupList;
        public long CompanyGroupID = 0;
        string MessageTitle = "Company Group Master";
        public clsCompanyGroup obj;
        bool Isvalid = false;
        string Message = "";

        public CompanyGroup()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtCompanyGroupName);
            try
            {
                if (CompanyGroupID > 0)
                {
                    GetCompanyGroup();
                    btnSave.Content = "_Update";
                }
                else
                {
                    chkIsActive.IsChecked = true;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        private void GetCompanyGroup()
        {
            try
            {
                obj = new clsCompanyGroup();
                obj = (new BALCompanyGroup().GetCompanyGroup(CompanyGroupID));
                txtCompanyGroupName.Text = obj.CompanyGroupName;
                txtCompanyGroupCode.Text = obj.CompanyGroupCode;
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
                txtCompanyGroupCode.Text = "";
                txtCompanyGroupName.Text = "";
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }
        private void frmCompanyGroup_KeyDown(object sender, KeyEventArgs e)
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objCompanyGroupList.GetCompanyGroupList();

                Close();
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
                    if (CompanyGroupID > 0)
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
                if (txtCompanyGroupName.Text.Trim() == "")
                {

                    Message += " Enter Company Group Name \n";
                }
                if (txtCompanyGroupCode.Text.Trim() == "")
                {

                    Message += " Enter Company Group Code ";
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
                obj = new clsCompanyGroup();
                obj.CompanyGroupCode = txtCompanyGroupCode.Text.Trim();
                obj.CompanyGroupName = txtCompanyGroupName.Text.Trim();
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
                obj = new clsCompanyGroup();
                obj.CompanyGroupCode = txtCompanyGroupCode.Text.Trim();
                obj.CompanyGroupName = txtCompanyGroupName.Text.Trim();
                obj.CompanyGroupID = CompanyGroupID;
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
    }
}
