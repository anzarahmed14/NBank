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
    /// Interaction logic for Parameter.xaml
    /// </summary>
    public partial class Parameter : Window
    {
        internal long ParameterID;
        public ParameterList objParameterList { get; internal set; }
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "";
        clsParameter obj;
        public Parameter()
        {
            InitializeComponent();
        }

      
        private void frmParameter_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtParameterName);
            try
            {
                if (ParameterID > 0)
                {
                    GetParameter();
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
                    if (ParameterID > 0)
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
                objParameterList.GetParameterList();

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void GetParameter()
        {
            try
            {
                obj = new clsParameter();
                obj = (new BALParameter().GetParameter(ParameterID));
                txtParameterName.Text = obj.ParameterName;
                txtParameterShortName.Text = obj.ParameterShortName;
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
                if (txtParameterName.Text.Trim() == "")
                {

                    Message += " Enter Parameter Name \n";
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
                obj = new clsParameter();
                obj.ParameterName = txtParameterName.Text.Trim();
                obj.ParameterShortName = txtParameterShortName.Text.Trim();
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
                obj = new clsParameter();
                obj.ParameterName = txtParameterName.Text.Trim();
                obj.ParameterShortName = txtParameterShortName.Text.Trim();
                if (chkIsActive.IsChecked ?? true)
                {
                    obj.IsActive = true;
                }
                else
                {
                    obj.IsActive = false;
                }

                obj.ParameterID = ParameterID;

                Message = (new BALOperation().Update(obj));
                if (Message == "SAVE")
                {
                    //  objAccountList.GetAccount();
                    //MessageBox.Show("Record updated successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = Message;
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
        private void Initialize()
        {
            try
            {
                txtParameterName.Text = "";
                txtParameterShortName.Text = "";
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmParameter_KeyDown(object sender, KeyEventArgs e)
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
