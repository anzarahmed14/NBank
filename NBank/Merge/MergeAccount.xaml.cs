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
using BOLNBank;
using System.Data;
using BALNBank;
namespace NBank.Merge
{
    /// <summary>
    /// Interaction logic for MergeAccount.xaml
    /// </summary>
    public partial class MergeAccount : Window
    {
        long ToAccountID = 0;
        long FromAccountID = 0;
        DataSet _ds;

      
        string Message = "";
        List<clsAccount> alist;
        public MergeAccount()
        {
            InitializeComponent();
        }

        private void frmMergeAccount_Loaded(object sender, RoutedEventArgs e)
        {
           // lblWarningText.Text = "This is Critical Data Operation";
            alist = new List<clsAccount>();
            alist = (new BALNBank.BALAccount().GetAccountList());

            alist.Insert(0, new clsAccount() { AccountID = -1, AccountName = "--Select From Account Name--" });
            cmbFromAccountName.ItemsSource = alist;
            cmbFromAccountName.DisplayMemberPath = "AccountName";
            cmbFromAccountName.SelectedValuePath = "AccountID";
            cmbFromAccountName.SelectedValue = -1;


            alist.RemoveAt(0);
            alist.Insert(0, new clsAccount() { AccountID = -1, AccountName = "--Select To Account Name--" });
            cmbToAccountName.ItemsSource = alist;
            cmbToAccountName.DisplayMemberPath = "AccountName";
            cmbToAccountName.SelectedValuePath = "AccountID";
            cmbToAccountName.SelectedValue = -1;

        }



        private void btnMeregeAccount_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidate())
            {
                _ds = new DataSet();
                _ds = GetMergeRecordAccount();

                StringBuilder sb = new StringBuilder();
                sb.Append("All data with Account Name");
                sb.Append("\"");
                sb.Append("" + cmbFromAccountName.Text + "");
                sb.Append("\"");
                sb.AppendLine(" will be ");
                sb.Append("MERGED with Account Name ");
                sb.Append("\"");
                sb.Append("" + cmbToAccountName.Text + "");
                sb.Append("\"");
                sb.AppendLine("");

                sb.AppendLine("Records: " + _ds.Tables[0].Rows.Count);

                sb.AppendLine("Important: ");
                sb.AppendLine("This is critical data changes, once data is merged this can not be undone.");



                sb.AppendLine("Proceed for Merging?");

                MessageBoxResult obj = MessageBox.Show(sb.ToString(), "Merge Confirmation", MessageBoxButton.YesNo);


                if (obj == MessageBoxResult.Yes)
                {
                    CreateMergeAccount();
                    //MessageBox.Show("Yes");
                }
                else
                {
                    // MessageBox.Show("NO");
                }
            }
        }
        private void CreateMergeAccount()
        {
            try
            {
                ToAccountID = Convert.ToInt64(cmbToAccountName.SelectedValue);
                FromAccountID = Convert.ToInt64(cmbFromAccountName.SelectedValue);

                Message = (new BALMerge().CreateMergeAccount(new clsMerge { ToAccountID = ToAccountID, FromAccountID = FromAccountID, CreatedDate = DateTime.Now, CreatedUserID = Globals.UserID }));
                if (Message == "SAVE")
                    MessageBox.Show("Data Merge successfully.", "Merge", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show(Message, "Merge", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public DataSet GetMergeRecordAccount()
        {
            _ds = new DataSet();
            FromAccountID = Convert.ToInt64(cmbFromAccountName.SelectedValue);
            _ds = (new BALMerge().GetMergeRecordAccount(FromAccountID));

            return _ds;
        }
        private bool IsValidate()
        {
            bool IsValid = false;
            Message = "";
            try
            {
                ToAccountID = Convert.ToInt64(cmbToAccountName.SelectedValue);
                FromAccountID = Convert.ToInt64(cmbFromAccountName.SelectedValue);

               
                if (ToAccountID == 0 || ToAccountID == -1)
                {
                    Message += "Select To Account Name" + System.Environment.NewLine;
                }
                if (FromAccountID == 0 || FromAccountID == -1)
                {
                    Message += "Select From Account Name" + System.Environment.NewLine;
                }

                
                if (Message.Length > 0)
                {
                    IsValid = false;
                    MessageBox.Show(Message);
                }
                else
                {
                    IsValid = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return IsValid;
        }
    }
}
