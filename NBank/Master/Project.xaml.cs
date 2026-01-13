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
    /// Interaction logic for Project.xaml
    /// </summary>
    public partial class Project : Window
    {
        public ProjectList objProjectList;
        public long ProjectID = 0;
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "Project Master";
        clsProject obj;
        List<clsCompany> objCompanyList;

        public Project()
        {
            InitializeComponent();
        }

        private void frmProject_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtProjectName);
            objCompanyList = (new BALCompany().GetCompanyList());
            objCompanyList.Insert(0, (new clsCompany() { CompanyID  = -1, CompanyShortName ="--Select Company--"}));
            cmbCompanyName.ItemsSource = objCompanyList;
            cmbCompanyName.DisplayMemberPath = "CompanyShortName";
            cmbCompanyName.SelectedValuePath = "CompanyID";
            cmbCompanyName.SelectedValue = -1;
            try
            {
                if (ProjectID > 0)
                {
                    GetProject();
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
                    if (ProjectID > 0)
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

                MessageBox.Show(ex.Message , MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objProjectList.GetProjectList();
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GetProject()
        {
            try
            {
                obj = new clsProject();
                obj = (new BALProject().GetProject(ProjectID));
                txtProjectName.Text = obj.ProjectName;
                txtProjectShortName.Text = obj.ProjectShortName;
                cmbCompanyName.SelectedValue = obj.CompanyID;

                if (obj.SquareFit > 0)
                {
                    txtSquareFit.Text = Convert.ToString(obj.SquareFit);
                }
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
                if (txtProjectName.Text.Trim() == "")
                {

                    Message += " Enter Project Name \n";
                }
                if (txtProjectShortName.Text.Trim() == "")
                {

                    Message += " Enter Short Project Name ";
                }
                if ( Convert.ToInt64( cmbCompanyName.SelectedValue) == -1)
                {

                    Message += " Select Company Name";
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
                obj = new clsProject();
                obj.ProjectName = txtProjectName.Text.Trim();
                obj.ProjectShortName = txtProjectShortName.Text.Trim();
                obj.CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                if (txtSquareFit.Text.Trim() != "" && txtSquareFit.Text.Trim().Length > 0)
                {
                    obj.SquareFit = Convert.ToDouble(txtSquareFit.Text.Trim());
                }
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                List<string> exlist = new List<string>();
                exlist.Add("ProjectID");
                exlist.Add("ProjectName");
                exlist.Add("ProjectShortName");
                exlist.Add("ProjectCode");
                exlist.Add("SquareFit");
                exlist.Add("IsActive");
                exlist.Add("CompanyID");
                Message = (new BALOperation().Create(obj, exlist));
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
        }
        public void Update()
        {
            try
            {
                obj = new clsProject();
                obj.ProjectName = txtProjectName.Text.Trim();
                obj.ProjectShortName = txtProjectShortName.Text.Trim();
                obj.CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                if (txtSquareFit.Text.Trim() != "" && txtSquareFit.Text.Trim().Length > 0)
                {
                    obj.SquareFit = Convert.ToDouble(txtSquareFit.Text.Trim());
                }
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }
                obj.ProjectID = ProjectID;



                List<string> exlist = new List<string>();
                exlist.Add("ProjectID");
                exlist.Add("ProjectName");
                exlist.Add("ProjectShortName");
                exlist.Add("ProjectCode");
                exlist.Add("SquareFit");
                exlist.Add("IsActive");
                exlist.Add("CompanyID");

                Message = (new BALOperation().Update(obj, exlist));
                if (Message == "SAVE")
                {
                    //  objAccountList.GetAccount();
                   // MessageBox.Show("Record updated successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void txtSquareFit_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
                txtProjectName.Text = "";
                txtProjectShortName.Text = "";
                txtSquareFit.Text = "";
                cmbCompanyName.SelectedValue = -1;
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmProject_KeyDown(object sender, KeyEventArgs e)
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
