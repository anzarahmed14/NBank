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
    /// Interaction logic for TypeMaster.xaml
    /// </summary>
    public partial class TypeMaster : Window
    {
        internal TypeList objTypeList;
        internal long TypeID;
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "Type Master";
        clsType obj;

        public TypeMaster()
        {
            InitializeComponent();
        }

        private void frmType_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtTypeName);
            try
            {
                if (TypeID > 0)
                {
                    GetTypeMaster();
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            
            objTypeList.GetTypeList();
            Close();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidate())
                {
                    if (TypeID > 0)
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
        private void GetTypeMaster()
        {
            try
            {
                obj = new clsType();
                obj = (new BALType().GetTypeMaster(TypeID));
                txtTypeName.Text = obj.TypeName;
                txtTypeShortName.Text = obj.TypeShortName;
                //if (chkIsActive.IsChecked )
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
                if (txtTypeName.Text.Trim() == "")
                {

                    Message += " Enter Type Name \n";
                }
                if (txtTypeShortName.Text.Trim() == "")
                {

                    Message += " Enter Short Type Name ";
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
                obj = new clsType();
                obj.TypeName = txtTypeName.Text.Trim();
                obj.TypeShortName = txtTypeShortName.Text.Trim();

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

                    ////MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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
            obj = new clsType();
            try
            {
                obj.TypeName = txtTypeName.Text.Trim();
                obj.TypeShortName = txtTypeShortName.Text.Trim();
                obj.TypeID = TypeID;
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
        private void Initialize()
        {
            try
            {
                txtTypeName.Text = "";
                txtTypeShortName.Text = "";
               
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmType_KeyDown(object sender, KeyEventArgs e)
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
