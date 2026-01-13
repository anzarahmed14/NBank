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
    /// Interaction logic for SubType.xaml
    /// </summary>
    public partial class SubType : Window
    {
        public SubTypeList objSubTypeList;
        internal long SubTypeID;
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "";
        clsSubType obj;


        public SubType()
        {
            InitializeComponent();
        }

     
        private void frmSubType_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtSubTypeName);
            try
            {
                if (SubTypeID > 0)
                {
                    GetSubType();
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
                    if (SubTypeID > 0)
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
                objSubTypeList.GetSubTypeList();

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        public void GetSubType()
        {
            try
            {
                obj = new clsSubType();
                obj = (new BALSubType().GetSubType(SubTypeID));
                txtSubTypeName.Text = obj.SubTypeName;
                txtSubTypeShortName.Text = obj.SubTypeShortName;
                txtSubTypePrintName.Text = obj.SubTypePrintName;
                if (obj.CreditDays > 0)
                {
                    txtCreditDays.Text = Convert.ToString(obj.CreditDays);
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
        public void Create()
        {
            try
            {
                obj = new clsSubType();
                obj.SubTypeName = txtSubTypeName.Text.Trim();
                obj.SubTypeShortName = txtSubTypeShortName.Text.Trim();
                obj.SubTypePrintName = txtSubTypePrintName.Text.Trim();

                if (txtCreditDays.Text.Trim().Length > 0 && txtCreditDays.Text.Trim() != "")
                {
                    obj.CreditDays = Convert.ToInt32(txtCreditDays.Text.Trim());
                }

                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = true;
                }




                Message = (new BALOperation().Create(obj));
                if (Message == "SAVE")
                {

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
                obj = new clsSubType();
                obj.SubTypeName = txtSubTypeName.Text.Trim();
                obj.SubTypeShortName = txtSubTypeShortName.Text.Trim();
                obj.SubTypePrintName = txtSubTypePrintName.Text.Trim();



                if (txtCreditDays.Text.Trim().Length > 0 && txtCreditDays.Text.Trim() != "")
                {
                    obj.CreditDays = Convert.ToInt32(txtCreditDays.Text.Trim());
                }


                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }
                obj.SubTypeID = SubTypeID;


                Message = (new BALOperation().Update(obj));
                if (Message == "SAVE")
                {
                  
                    //MessageBox.Show("Record updated successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "Record upated successfully";
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
        public bool IsValidate()
        {
            try
            {
                Message = "";
                //MessageBox.Show(cmbDefaultBank.SelectedIndex.ToString());
                if (txtSubTypeName.Text.Trim() == "")
                {
                    Message += " Enter SubType Name \n";
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

        private void txtCreditDays_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                var regex = new System.Text.RegularExpressions.Regex(@"^[0-9]*(?:\.[0-9]*)?$");
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
                txtCreditDays.Text = "";
                txtSubTypeName.Text = "";
                txtSubTypeShortName.Text = "";
                txtSubTypePrintName.Text = "";
              
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmSubType_KeyDown(object sender, KeyEventArgs e)
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
