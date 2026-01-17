using BALNBank;
using BOLNBank;
using NBank.Master;
using NBank.Other;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace NBank.Transaction
{
    /// <summary>
    /// Interaction logic for ChequeEntry.xaml
    /// </summary>
    public partial class ChequeEntry : Window
    {
        //ex.Message + "017"
        List<clsBank>      objBankList;
        List<clsProject>   objProjectList;
        List<clsCompany>   objCompanyList;
        List<clsType>      objTypeList;
        List<clsSubType>   objSubTypeList;
        List<clsParameter> objParameterList;
        List<clsChequeStatus> objChequeStatusList;
        List<clsAccount> objAccountList;

        List<clsChequeType> objChequeTypeList;

        List<clsCompanyWiseBank> objCompanyWiseBankList;


        clsChequeEntry obj;
        string Message = "";
        string MessageTitle = "Cheque Entry Master";
        bool Isvalid = false;
        DataSet _ds;
        string TotalAmount = "";
        public ChequeEntryList objChequeEntryList;
        public long ChequeEntryID = 0;
        public long AccountID = 0;
        public string AccountName = "";
        private long CompanyID = 0;
        private long BankID = 0;
        private long ProjectID =0;
        private long ChequeTypeID = 0;


        public Dictionary<string, string> ParaField = null;
        //cmbChequeTypeName
        public ChequeEntry()
        {
            InitializeComponent();
        }

        private void frmChequeEntry_Loaded(object sender, RoutedEventArgs e)
        {


            try
            {
              

                Keyboard.Focus(dtpChequeEntryDate);
                objCompanyList = (new BALCompany().GetCompanyList("",Globals.UserID));

                /*15-December-2017*/
                objCompanyList = objCompanyList.FindAll(Active => Active.IsActive == true);

                objCompanyList.Insert(0, new clsCompany() { CompanyID = -1, CompanyShortName = "--Select Company--" });

                cmbCompanyName.ItemsSource = objCompanyList;
                cmbCompanyName.DisplayMemberPath = "CompanyShortName";
                cmbCompanyName.SelectedValuePath = "CompanyID";
                cmbCompanyName.SelectedValue = -1;


                /*
                objBankList = (new BALBank().GetBankList());
                cmbBankName.ItemsSource = objBankList;
                cmbBankName.DisplayMemberPath = "BankName";
                cmbBankName.SelectedValuePath = "BankID";
                */
                /*
                objProjectList = (new BALProject().GetProjectList());
                cmbProjectName.ItemsSource = objProjectList;
                cmbProjectName.DisplayMemberPath = "ProjectName";
                cmbProjectName.SelectedValuePath = "ProjectID";
                */

                objTypeList = (new BALType().GetTypeList());
                /*15-December-2017*/
                objTypeList = objTypeList.FindAll(Active => Active.IsActive == true);
                objTypeList.Insert(0, new clsType() { TypeID = -1, TypeShortName = "--Select Type--" });
                cmbType.ItemsSource = objTypeList;
                //cmbType.ItemsSource = matchingCars;
                cmbType.DisplayMemberPath = "TypeShortName";
                cmbType.SelectedValuePath = "TypeID";
                cmbType.SelectedValue = -1;


                objSubTypeList = (new BALSubType().GetSubTypeList());
                /*15-December-2017*/
                objSubTypeList = objSubTypeList.FindAll(Active => Active.IsActive == true);
                objSubTypeList.Insert(0, new clsSubType() { SubTypeID = -1, SubTypeShortName = "--Select SubType--" });
                cmbSubType.ItemsSource = objSubTypeList;
                cmbSubType.DisplayMemberPath = "SubTypeShortName";
                cmbSubType.SelectedValuePath = "SubTypeID";
                cmbSubType.SelectedValue = -1;

                objParameterList = (new BALParameter().GetParameterList());

                /*15-December-2017*/
                objParameterList = objParameterList.FindAll(Active => Active.IsActive == true);

                objParameterList.Insert(0, new clsParameter() { ParameterID = -1, ParameterShortName = "--Select Parameter--" });
                cmbParameter.ItemsSource = objParameterList;
                cmbParameter.DisplayMemberPath = "ParameterShortName";
                cmbParameter.SelectedValuePath = "ParameterID";
                cmbParameter.SelectedValue = -1;

                objChequeStatusList = (new BALChequeStatus().GetChequeStatusList());
                cmbChequeStatus.ItemsSource = objChequeStatusList;
                cmbChequeStatus.DisplayMemberPath = "ChequeStatusName";
                cmbChequeStatus.SelectedValuePath = "ChequeStatusID";

                if (ChequeEntryID == 0) {
                    cmbChequeStatus.SelectedValue = 1;
                }


                objChequeTypeList = (new BALChequeType().GetChequeTypeList());
                cmbChequeTypeName.ItemsSource = objChequeTypeList;
                cmbChequeTypeName.DisplayMemberPath = "ChequeTypeName";
                cmbChequeTypeName.SelectedValuePath = "ChequeTypeID";

                cmbChequeTypeName.SelectedIndex = 1;
                //objChequeTypeList

                /*Require to fill the party name text box*/
                objAccountList = (new BALAccount().GetAccountList());


               
                if (ChequeEntryID > 0)
                {
                    GetChequeEntry();
                    btnSave.Content = "_Update";
                    GetCompanyProject();
                    GetCompanyBank();
                }
                else
                {
                    btnPrint.Visibility = Visibility.Collapsed;
                    btnDelete.Visibility = Visibility.Collapsed;

                    if ( Convert.ToInt64( cmbChequeTypeName.SelectedValue) ==2) { 
                    /*Set Clear Date For Next three Days*/
                    dtpChequeClearDate.SelectedDate = DateTime.Now.AddDays(3);
                    }

                    dtpChequeIssueDate.SelectedDate = null;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "004", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidate())
                {
                    if (ChequeEntryID > 0)
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

                MessageBox.Show(ex.Message + "001", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objChequeEntryList.GetChequeEntryList();
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "005", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtChequeAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

                MessageBox.Show(ex.Message + "006", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtTDSAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;

            else
                e.Handled = true;
        }

       
        private void txtPartyName_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void txtPartyName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.F4)
            {
                AccountPopup obj = new AccountPopup();
                obj.objChequeEntry = this;
                obj.ShowDialog();
                txtPartyName.Text = AccountName;

            }
            if (e.Key == Key.F3) {
                Account objAccount = new Account();
                objAccount.ShowDialog();
            }
            lblStatus.Text = "";
            
        }

        private void txtPartyName_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
           
            lblStatus.Text = "Press F3 For New Party,  F4 For Party Name Entry";
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            objChequeEntryList.GetChequeEntryList();
            Delete();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Print();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "007", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void dtpChequeIssueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            /*Automatically set clear date to next three days*/
            try
            {
                if (Convert.ToInt32(cmbChequeTypeName.SelectedValue) == 1)
                {

                }
                else {
                    if (dtpChequeClearDate != null)
                    {
                        if (dtpChequeIssueDate.SelectedDate !=null)
                        dtpChequeClearDate.SelectedDate = dtpChequeIssueDate.SelectedDate.Value.AddDays(3);
                    }
                }
                /*
                if (dtpChequeClearDate != null)
                {
                    dtpChequeClearDate.SelectedDate = dtpChequeIssueDate.SelectedDate.Value.AddDays(3);
                }
                */
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "008", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbCompanyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //MessageBox.Show(cmbCompanyName.SelectedValue.ToString());
                CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                if (CompanyID > 0) { 
                    var company = objCompanyList.Find(x => x.CompanyID == CompanyID);
                    lblSelectedCompanyName.Content = company.CompanyName;
                }
                GetCompanyBank();
                GetCompanyProject();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "009", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void cmbBankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbBankName.SelectedValue != null)
                {
                    BankID = Convert.ToInt64(cmbBankName.SelectedValue);
                    if (ChequeEntryID == 0)
                    {
                      GetChequeNo();
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "018", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Create()
        {
            try
            {
                obj = new clsChequeEntry();
                obj.ChequeEntryDate = dtpChequeEntryDate.SelectedDate.Value;
                obj.CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);

                obj.ProjectID = Convert.ToInt64(cmbProjectName.SelectedValue);

                obj.ChequeTypeID = Convert.ToInt64(cmbChequeTypeName.SelectedValue);
                obj.AccountID = AccountID;
                obj.AccountSubName = txtAccountSubName.Text.Trim();
                obj.BankID = Convert.ToInt64(cmbBankName.SelectedValue);
                obj.ChequeNo = txtChequeNo.Text.Trim();
                obj.ChequeStatusID = Convert.ToInt64(cmbChequeStatus.SelectedValue);

                if(txtChequeAmount.Text.Trim() != "") {
                    obj.ChequeAmount = Convert.ToDouble (txtChequeAmount.Text.Trim());
                }
                if (txtChequeAmountTDS.Text.Trim() != "") { 
                obj.ChequeAmountTDS = Convert.ToDouble(txtChequeAmountTDS.Text.Trim());
                }
                obj.ParameterID = Convert.ToInt64(cmbParameter.SelectedValue);
                obj.TypeID = Convert.ToInt64(cmbType.SelectedValue);
                obj.SubTypeID = Convert.ToInt64(cmbSubType.SelectedValue);
                obj.Narration = txtNarration.Text.Trim();

                if (dtpChequeIssueDate.SelectedDate != null)
                {
                    obj.ChequeIssueDate = dtpChequeIssueDate.SelectedDate.Value;
                }

                if (dtpChequeClearDate.SelectedDate != null) { 
                    obj.ChequeClearDate = dtpChequeClearDate.SelectedDate.Value;
                }
                obj.CreatedUserID = Globals.UserID;
                obj.UpdatedUserID = Globals.UserID;

                obj.ERPID = txtERPID.Text.Trim();

                Message = (new BALOperation().Create(obj));

                //MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                if (Message == "SAVE")
                {
                    Initialize();
                    lblStatus.Text = "Record saved successfully";
                }
                else if (Message == "DUCH") {
                    lblStatus.Text = "Duplicate Cheque Number";
                    MessageBox.Show("Duplicate Cheque Number", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {

                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "002", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Update()
        {
            try
            {
                obj = new clsChequeEntry();
                obj.ChequeEntryDate = dtpChequeEntryDate.SelectedDate.Value;
                obj.CompanyID = Convert.ToInt64(cmbCompanyName.SelectedValue);
                obj.ProjectID = Convert.ToInt64(cmbProjectName.SelectedValue);
                obj.ChequeTypeID = Convert.ToInt64(cmbChequeTypeName.SelectedValue);
                obj.AccountID = AccountID;
                obj.AccountSubName = txtAccountSubName.Text.Trim();
                obj.BankID = Convert.ToInt64(cmbBankName.SelectedValue);
                obj.ChequeNo = txtChequeNo.Text.Trim();
                obj.ChequeStatusID = Convert.ToInt64(cmbChequeStatus.SelectedValue);
                obj.ChequeAmount = Convert.ToDouble (txtChequeAmount.Text.Trim());
                obj.ChequeAmountTDS = Convert.ToDouble(txtChequeAmountTDS.Text.Trim());
                obj.ParameterID = Convert.ToInt64(cmbParameter.SelectedValue);
                obj.TypeID = Convert.ToInt64(cmbType.SelectedValue);
                obj.SubTypeID = Convert.ToInt64(cmbSubType.SelectedValue);
                obj.Narration = txtNarration.Text.Trim();

                if (dtpChequeIssueDate.SelectedDate != null) { 
                    obj.ChequeIssueDate = dtpChequeIssueDate.SelectedDate.Value;
                }
                if (dtpChequeClearDate.SelectedDate != null)
                {
                    obj.ChequeClearDate = dtpChequeClearDate.SelectedDate.Value;
                }
                obj.ChequeEntryID = ChequeEntryID;

                //obj.CreatedUserID = Globals.UserID;
                obj.UpdatedUserID = Globals.UserID;
                obj.ERPID = txtERPID.Text.Trim();

                Message = (new BALOperation().Update(obj));

                if (Message == "SAVE"){
                    lblStatus.Text = "Record updated successfully";

                }
                else if (Message == "DUCH")
                {
                    lblStatus.Text = "Duplicate Cheque Number";
                    MessageBox.Show("Duplicate Cheque Number", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else {
                    lblStatus.Text = "";
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "010", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool IsValidate()
        {
            try
            {
                Message = "";
                if ( Convert.ToInt64( cmbCompanyName.SelectedValue) <0 )
                {
                    Message += " Select Company Name\n";
                }
                if (cmbProjectName.SelectedIndex == -1)
                {
                    Message += " Select Project Name\n";
                }
                if (cmbBankName.SelectedIndex == -1)
                {
                    Message += " Select Bank Name\n";
                }
                if (AccountID == 0)
                {
                    Message += " Select Party Name\n";
                }
                if (txtChequeAmount.Text.Trim() == "")
                {
                    Message += " Enter Cheque Amount\n";
                }
                if (txtChequeNo.Text.Trim() == "")
                {
                    Message += " Enter Cheque No.\n";
                }


                if (Convert.ToInt64(cmbType.SelectedValue) <0)
                {
                    Message += " Select Type Name\n";
                }
                if (Convert.ToInt64(cmbSubType.SelectedValue) <0)
                {
                    Message += " Select Sub Type Name\n";
                }

                /*
                if (Convert.ToInt64(cmbParameter.SelectedValue) <0)
                {
                    Message += " Select Parameter Name\n";
                }
                */

                if (dtpChequeClearDate.SelectedDate != null)
                {
                    if (dtpChequeClearDate.SelectedDate.Value < dtpChequeIssueDate.SelectedDate.Value)
                    {
                        Message += " Clear data should be greater than issue date\n";
                    }
                }

                
                /*
                if (dtpChequeEntryDate.SelectedDate.Value < dtpChequeIssueDate.SelectedDate.Value)
                {
                    Message += " Issue Date should be greater than or equal to issue date\n";
                }
                */


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

                MessageBox.Show(ex.Message + "011", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return Isvalid;
        }
        public void GetChequeEntry()
        {
            try
            {
                obj = new clsChequeEntry();
                obj = (new BALChequeEntry().GetChequeEntry(ChequeEntryID));
                AccountID = obj.AccountID;

                //objAccountList

                var account = objAccountList.Find(x => x.AccountID == AccountID);

                txtPartyName.Text = account.AccountName;


                dtpChequeEntryDate.SelectedDate = obj.ChequeEntryDate;
                cmbCompanyName.SelectedValue = obj.CompanyID;
                cmbProjectName.SelectedValue = obj.ProjectID;
                cmbChequeTypeName.SelectedValue = obj.ChequeTypeID;
                cmbBankName.SelectedValue = obj.BankID;
                txtChequeNo.Text = obj.ChequeNo;
                cmbChequeStatus.SelectedValue = obj.ChequeStatusID;
                txtChequeAmount.Text = Convert.ToString(obj.ChequeAmount);
                txtChequeAmountTDS.Text = Convert.ToString(obj.ChequeAmountTDS);
                cmbParameter.SelectedValue = obj.ParameterID;
                cmbType.SelectedValue = obj.TypeID;
                cmbSubType.SelectedValue = obj.SubTypeID;
                txtNarration.Text = obj.Narration;

                if (obj.ChequeClearDate != DateTime.MinValue)
                {
                    dtpChequeClearDate.SelectedDate = obj.ChequeClearDate;
                }
                else {
                    dtpChequeClearDate.SelectedDate = null;
                }

                //dtpChequeIssueDate.SelectedDate = obj.ChequeIssueDate;

                if (obj.ChequeIssueDate != DateTime.MinValue)
                {
                    dtpChequeIssueDate.SelectedDate = obj.ChequeIssueDate;
                }
                else
                {
                    dtpChequeIssueDate.SelectedDate = null;
                }


                txtAccountSubName.Text = obj.AccountSubName;
                CompanyID = obj.CompanyID;
                ProjectID = obj.ProjectID;

                txtERPID.Text = obj.ERPID;

            }
            catch (Exception ex)
            {
               
                MessageBox.Show(ex.Message + "012", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //.Text = obj.Narration;
        }

        public void Print()
        {

            try
            {
               

                ReportPreview obj = new ReportPreview();
                
                ParaField = new Dictionary<string, string>();
              

                TotalAmount = Convert.ToString(Convert.ToDouble(txtChequeAmount.Text.Trim()) + Convert.ToDouble(txtChequeAmountTDS.Text.Trim()));
                string AmountInwords = "";
                AmountInwords = "Rupees " + (new clsCurrencyConverter().ConvertToWords(TotalAmount));
                ParaField.Add("AmountInwords", AmountInwords);
                ParaField.Add("@ChequeEntryID", Convert.ToString(ChequeEntryID));
                ParaField.Add("@FromDate", null);
                ParaField.Add("@ToDate", null);
                ParaField.Add("@ChequeNo", null);
                ParaField.Add("@CompanyID", null);
                ParaField.Add("@ProjectID", null);
                ParaField.Add("@BankID", null);
                ParaField.Add("@ColumnName", null);
               

                obj.ReportName = "rpVoucher.rpt";
               
                obj.ParaField = ParaField;
                obj.ShowDialog();
              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "013", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                //MessageBox.Show("D", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Delete()
        {
            try
            {

                Message = (new BALOperation().Delete(obj, ChequeEntryID));

                if (Message == "SAVE")
                {
                    lblStatus.Text = "Record deleted successfully";
                    MessageBox.Show("Record deleted successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else {
                    MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Close();
                objChequeEntryList.GetChequeEntryList();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "014", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
        public void GetCompanyBank()
        {

            try
            {
                List<clsCompanyWiseBank> CompanyBank;
                CompanyBank = new List<clsCompanyWiseBank>();
                objCompanyWiseBankList = new List<clsCompanyWiseBank>();
          

                objCompanyWiseBankList = (new BALCompanyWiseBank().GetCompanyWiseBankList());
                //CompanyBank = objCompanyWiseBankList.FindAll(x => x.CompanyID == CompanyID);
                CompanyBank = objCompanyWiseBankList.FindAll(c => (c.CompanyID == CompanyID) && (c.IsActive == true));


                cmbBankName.ItemsSource = CompanyBank;
                cmbBankName.DisplayMemberPath = "BankName";
                cmbBankName.SelectedValuePath = "BankID";

                if (ChequeEntryID > 0)
                {
                    cmbBankName.SelectedValue = BankID;
                }
                else
                {

                    clsCompany DefualtBank = objCompanyList.Find(x => x.CompanyID == CompanyID);

                    if (DefualtBank != null)
                    {
                        cmbBankName.SelectedValue = DefualtBank.BankID;
                        BankID = DefualtBank.BankID;
                        GetChequeNo();
                    }
                    else
                    {
                        cmbBankName.SelectedValue = BankID;
                    }
                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "  015", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetCompanyProject()
        {
            try
            {
               

                objProjectList = (new BALProject().GetProjectList());

                // List<clsProject> CompanyProject = objProjectList.FindAll(x => x.CompanyID == CompanyID);

                List<clsProject> CompanyProject = objProjectList.FindAll(c => (c.CompanyID == CompanyID) && (c.IsActive == true));

                cmbProjectName.ItemsSource = CompanyProject;
                cmbProjectName.DisplayMemberPath = "ProjectShortName";
                cmbProjectName.SelectedValuePath = "ProjectID";

                if (ChequeEntryID > 0)
                {
                    cmbProjectName.SelectedValue = ProjectID;
                }
                else {

                    clsCompany DefualtProject = objCompanyList.Find(x => x.CompanyID == CompanyID);
                    if (DefualtProject != null)
                    {
                        cmbProjectName.SelectedValue = DefualtProject.ProjectID;
                    }
                    else
                    {
                        cmbProjectName.SelectedValue = ProjectID;
                    }
                }
                
                
                // cmbProjectName.SelectedValue = ProjectID;
                //cmbProjectName.SelectedValue = CompanyProject[0].ProjectID;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "016", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetChequeNo()
        {
            try
            {
                if (ChequeEntryID == 0)
                {
                    //clsCompanyWiseBank BankChequeNo = objCompanyWiseBankList.Find(x => x.BankID == BankID );
                    objCompanyWiseBankList = new List<clsCompanyWiseBank>();

                    objCompanyWiseBankList =  (new BALCompanyWiseBank().GetCompanyWiseBankList()); 

                    clsCompanyWiseBank BankChequeNo = objCompanyWiseBankList.Find(c => (c.BankID == BankID) && (c.CompanyID == CompanyID));

                    if (BankChequeNo != null)
                    {
                        txtChequeNo.Text = BankChequeNo.LastChequeNo;
                    }
                }
            }
            catch (Exception ex) 
            {

                MessageBox.Show(ex.Message + "019", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbChequeTypeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (cmbBankName.SelectedIndex != -1)
                {
                    ChequeTypeID = Convert.ToInt64(cmbChequeTypeName.SelectedValue);
                    if (AccountID == 0)
                    {
                        if (ChequeTypeID == 1)
                        {

                            clsCompanyWiseBank BankChequeNo = objCompanyWiseBankList.Find(x => (x.BankID == BankID) && (x.CompanyID == CompanyID));
                            txtChequeNo.Text = BankChequeNo.LastChequeNo;
                        }
                        else
                        {
                            txtChequeNo.Text = "";
                        }
                    }
                    
                }


                /*Date And Status Chnage Man*/
                /*1=Issue, 2 Deposit */
                if (Convert.ToInt64(cmbChequeTypeName.SelectedValue) == 2)
                {

                    cmbChequeStatus.SelectedValue = 5; /*CL*/
                                                       //  dtpChequeClearDate.SelectedDate = DateTime.Now.AddDays(3);
                    if(dtpChequeIssueDate.SelectedDate != null)
                    dtpChequeClearDate.SelectedDate = dtpChequeIssueDate.SelectedDate.Value.AddDays(3);

                }
                else {
                    cmbChequeStatus.SelectedValue = 1; /*PE*/

                    //dtpChequeClearDate.SelectedDate = dtpChequeIssueDate.SelectedDate.Value;

                    dtpChequeClearDate.SelectedDate = null;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "017", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void Initialize()
        {
            try
            {
                Keyboard.Focus(dtpChequeEntryDate);
                ChequeEntryID = 0;
               // CompanyID = 0;
               // ProjectID = 0;
                //BankID = 0;
               // cmbCompanyName.SelectedValue = -1;
              //   cmbBankName.SelectedValue =-1;
               // cmbParameter.SelectedValue = -1;
              //cmbProjectName.SelectedValue = -1;
                //cmbType.SelectedValue = -1;
               // cmbSubType.SelectedValue = -1;
                txtChequeNo.Text = "";
                txtChequeAmount.Text = "";
                txtChequeAmountTDS.Text = "";
                txtNarration.Text = "";
              //  txtPartyName.Text = "";
                txtAccountSubName.Text = "";

                //cmbChequeStatus.SelectedValue = 1;
               // cmbChequeTypeName.SelectedValue = 1;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        

        private void btnSelectTodayDate_Click(object sender, RoutedEventArgs e)
        {
            dtpChequeIssueDate.SelectedDate = System.DateTime.Now;
        }

        private void frmChequeEntry_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            /*
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
            */
        }

        private void frmChequeEntry_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter && (e.IsToggled == true || (sender as Button) == null))
            //{
            //    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            //    UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

            //    if (keyboardFocus != null)
            //    {
            //        keyboardFocus.MoveFocus(request);

            //        e.Handled = true;
            //    }
            //}
            NavigationControl(sender,e);
        }
        private void NavigationControl(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) // Is Alt key pressed
            {
                if (Keyboard.IsKeyDown(Key.Enter))
                {
                    if (e.Key == Key.Enter)
                    {
                        TextBox s = e.Source as TextBox;

                        Control a = e.Source as Control;
                        /*
                        if (s != null)
                        {
                            s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }
                        */

                        if (a.GetType().Name == "TextBox")
                        {
                            s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                            e.Handled = true;
                        }
                        else if (a.GetType().Name == "ComboBox")
                        {
                            ComboBox c = e.Source as ComboBox;
                            if (c.IsDropDownOpen)
                            {
                                e.Handled = false;

                            }
                            else
                            {
                                c.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                                e.Handled = true;
                            }
                        }
                        else
                        {

                            a.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                            e.Handled = true;
                        }

                    }
                }

            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    TextBox s = e.Source as TextBox;

                    Control a = e.Source as Control;
                    /*
                    if (s != null)
                    {
                        s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                    */

                    if (a.GetType().Name == "TextBox")
                    {
                        s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        e.Handled = true;
                    }
                    else if (a.GetType().Name == "ComboBox")
                    {
                        ComboBox c = e.Source as ComboBox;
                        if (c.IsDropDownOpen)
                        {
                            e.Handled = false;

                        }
                        else
                        {
                            c.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                            e.Handled = true;
                        }
                    }
                    else if (a.GetType().Name == "DatePicker")
                    {
                        TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                        UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                        if (keyboardFocus != null)
                        {
                            keyboardFocus.MoveFocus(request);

                            e.Handled = true;
                        }
                    }
                    else
                    {

                        a.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        e.Handled = true;
                    }

                }

            }

            if (e.Key == Key.Space)
            {
                Control a = e.Source as Control;
                if (a.GetType().Name == "ComboBox")
                {
                    ComboBox c = e.Source as ComboBox;
                    c.IsDropDownOpen = true;
                }
            }


        }

        private void dtpChequeClearDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtpChequeIssueDate.SelectedDate == null)
                {
                    dtpChequeClearDate.SelectedDate = null;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "020", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      

        private void btnClearDateSelectTodayDate_Click(object sender, RoutedEventArgs e)
        {
            dtpChequeClearDate.SelectedDate = System.DateTime.Now;
        }

        private void btnClearIssueDate_Click(object sender, RoutedEventArgs e)
        {
            dtpChequeIssueDate.SelectedDate = null;
        }

        
        private void btnClearClearDate_Click(object sender, RoutedEventArgs e)
        {
            dtpChequeClearDate.SelectedDate = null;
        }

        private void cmbBankName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ChequeEntryID == 0 && CompanyID >0 )
            {
                GetChequeNo();
            }
        }
    }
}
