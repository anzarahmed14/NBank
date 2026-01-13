using BALNBank;
using BOLNBank;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace NBank.Transaction
{
    /// <summary>
    /// Interaction logic for ChequeClear.xaml
    /// </summary>
    public partial class ChequeClear : Window
    {
        List<clsBank> objBankList;
        List<clsChequeStatus> objChequeStatusList;
        List<clsChequeEntry> list;
        long ChequeEntryID = 0;
        string MessageTitle = "Cheque Clear List";
        string Message = "";
        DataSet _ds;
        clsChequeEntrySearchParameter objPara;
        List<string> elist = null;

        DateTime StartDate; DateTime EndDate; long ChequeStatusID; long BankID; string ChequeNo; string DateType;

        public ChequeClear()
        {
            InitializeComponent();
        }

        private void frmChequeClear_Loaded(object sender, RoutedEventArgs e)
        {
            //objBankList = new List<clsBank>();
            
            objBankList = (new BALBank().GetBankList());
          //objBankList.Add(new clsBank { BankID = 0, BankName = "--Select--" });


            cmbBankName.ItemsSource = objBankList;
            cmbBankName.DisplayMemberPath = "BankName";
            cmbBankName.SelectedValuePath = "BankID";
            //cmbBankName.SelectedIndex = 0;


            objChequeStatusList = (new BALChequeStatus().GetChequeStatusList());

            objChequeStatusList.Insert(0, new clsChequeStatus() { ChequeStatusID = -1, ChequeStatusName = "--Select Status--" });

            cmbChequeStatus.ItemsSource = objChequeStatusList;
            cmbChequeStatus.DisplayMemberPath = "ChequeStatusName";
            cmbChequeStatus.SelectedValuePath = "ChequeStatusID";
            cmbChequeStatus.SelectedValue = -1;

            cmbDateType.ItemsSource = GetDateType();
            cmbDateType.DisplayMemberPath = "Value";
            cmbDateType.SelectedValuePath = "Key";
            cmbDateType.SelectedIndex = 0;

            GetChequeEntryList();
            lblStatus.Text = "Press F4 For CL, F5 For PE ";

        }
        public void GetChequeEntryList()
        {
            _ds = new DataSet();
            //DateTime StartDate, DateTime EndDate, long ChequeStatusID, long BankID, string ChequeNo,string DateFilter
            StartDate = dtpStartDate.SelectedDate.Value;
            EndDate = dtpEndDate.SelectedDate.Value;
            if (cmbBankName.SelectedIndex > -1)  {
                BankID = Convert.ToInt64(cmbBankName.SelectedValue);
            }
            if (Convert.ToInt64( cmbChequeStatus.SelectedValue) != -1)
            {
                ChequeStatusID = Convert.ToInt64(cmbChequeStatus.SelectedValue);
            }
            ChequeNo =  txtChequeNo.Text.Trim();
            //DateFilter = Convert.ToInt64( cmbDateType.SelectedValue)
            DateType = cmbDateType.SelectedValue.ToString();


           
             // objPara.ChequeNo = txtChequeNo.Text.Trim();
             _ds = (new BALChequeEntry().GetChequeList( StartDate,  EndDate,  ChequeStatusID,  BankID,  ChequeNo,  DateType));
            if (_ds.Tables[0].Rows.Count > 0)
            {
                dgChequeClearList.ItemsSource = _ds.Tables[0].DefaultView;
               // lblStatus.Text = _ds.Tables[0].Rows.Count + " Record Found";
            }
            else
            {
                dgChequeClearList.ItemsSource = _ds.Tables[0].DefaultView;
                // MessageBox.Show("No Record found");
                // lblStatus.Text = "No Record Found";
            }

        }
        public Dictionary<string, string> GetDateType()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("CED", "Entry Date");
            list.Add("CID", "Issue Date");
            list.Add("CCD", "Clear Date");
           
           

            return list;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetChequeEntryList();
        }

        private void dgChequeClearList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4) {
                var row = dgChequeClearList.SelectedItem as System.Data.DataRowView;
                if (row != null)
                {

                    // string gamePath = row["ChequeEntryID"].ToString();
                    ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                    /*5 = CL*/
                    ChangeChequeStatus(ChequeEntryID,5);
                }
                
            }
            if (e.Key == Key.F5)
            {
                var row = dgChequeClearList.SelectedItem as System.Data.DataRowView;
                if (row != null)
                {
                    // string gamePath = row["ChequeEntryID"].ToString();
                    ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                    /*1 = PE*/
                    ChangeChequeStatus(ChequeEntryID, 1);
                }
                
            }
           
        }
        public void ChangeChequeStatus(long ChequeEntryID, long ChequeStatusID)
        {
            Message = (new BALChequeEntry().ChangeChequeStatus(ChequeEntryID, ChequeStatusID));
            //lblStatus.Text = Message;
            GetChequeEntryList();
        }

        private void dgChequeClearList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (dgChequeClearList.SelectedIndex != -1)
                {
                    var row = dgChequeClearList.SelectedItem as System.Data.DataRowView;
                    if (row != null)
                    {
                        // string gamePath = row["ChequeEntryID"].ToString();
                        ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                        GetCheque(ChequeEntryID);

                    }

                    
                }
                else
                {
                //    MessageBox.Show("Please Select Row", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetCheque(long ChequeEntryID)
        {
            _ds = new DataSet();
            _ds = (new BALChequeEntry().GetChequeList(ChequeEntryID));
            if (_ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in _ds.Tables[0].Rows)
                {
                    lblNarrationValue.Text = row["Narration"].ToString();
                    lblPartyNameValue.Content = row["AccountName"].ToString();
                    lblTypeValue.Content = row["TypeName"].ToString();
                    lblSubTypeValue.Content = row["SubTypeName"].ToString();
                    lblParameterValue.Content = row["ParameterName"].ToString();

                }
            }
            else
            {
                // MessageBox.Show("No Record found");
                lblStatus.Text = "No Cheque Entry Found";
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmChequeClear_KeyDown(object sender, KeyEventArgs e)
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
