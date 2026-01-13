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
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class Category : Window
    {
        internal long CategoryID;
        public CategoryList objCategoryList { get; internal set; }
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "";
        clsCategory obj;
        public Category()
        {
            InitializeComponent();
        }

       

        private void frmCategory_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtCategoryName);
            try
            {
                if (CategoryID > 0)
                {
                    GetCategory();
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
                    if (CategoryID > 0)
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

                throw ex;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            objCategoryList.GetCategoryList();

            Close();
        }
        public void GetCategory()
        {
            obj = new clsCategory();
            obj = (new BALCategory().GetCategory(CategoryID));
            txtCategoryName.Text = obj.CategoryName;
            txtCategoryShortName.Text = obj.CategoryShortName;
            if (obj.IsActive == true)
            {
                chkIsActive.IsChecked = true;
            }
            else
            {
                chkIsActive.IsChecked = false;
            }

        }
        public void Create()
        {
            obj = new clsCategory();
            obj.CategoryName = txtCategoryName.Text.Trim();
            obj.CategoryShortName = txtCategoryShortName.Text.Trim();
            if (chkIsActive.IsChecked ?? true)
            {
                obj.IsActive = true;
            }
            else
            {
                obj.IsActive = true;
            }

            Message = (new BALOperation().Create(obj));
            lblStatus.Text = Message;
            //MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void Update()
        {
            obj = new clsCategory();
            obj.CategoryName = txtCategoryName.Text.Trim();
            obj.CategoryShortName = txtCategoryShortName.Text.Trim();
            if (chkIsActive.IsChecked ?? true)
            {
                obj.IsActive = true;
            }
            else
            {
                obj.IsActive = true;
            }

            obj.CategoryID = CategoryID;

            Message = (new BALOperation().Update(obj));
            lblStatus.Text = Message;
            //MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public bool IsValidate()
        {
            Message = "";
            //MessageBox.Show(cmbDefaultBank.SelectedIndex.ToString());
            if (txtCategoryName.Text.Trim() == "")
            {

                Message += " Enter Category Name \n";
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
    }
}
