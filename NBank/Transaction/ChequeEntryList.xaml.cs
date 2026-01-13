using BALNBank;
using BOLNBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for ChequeEntryList.xaml
    /// </summary>
    public partial class ChequeEntryList : Window
    {
        List<clsChequeEntry> list;
        long ChequeEntryID = 0;
        string MessageTitle = "Cheque Entry List";
        DataSet _ds;
        clsChequeEntrySearchParameter objPara;
        List<string> elist = null;
        string MenuName = "MenuChequeEntry";
        List<clsUserMenu> FilteredUserMenuList;

        public ChequeEntryList()
        {
            InitializeComponent();
        }

        private void frmChequeEntryList_Loaded(object sender, RoutedEventArgs e)
        {
            UserMenu();
            GetChequeEntryList();
        }
        private void UserMenu()
        {

            try
            {
                FilteredUserMenuList = Globals.UserMenuList.Where(x => x.MenuName == MenuName).ToList();

                if (FilteredUserMenuList != null) {
                    if (FilteredUserMenuList.Count > 0)
                    {
                        if (FilteredUserMenuList[0].AllowCreate == false)
                        {
                            btnAdd.Visibility = Visibility.Collapsed;
                        }
                        if (FilteredUserMenuList[0].AllowEdit == false)
                        {
                            btnEdit.Visibility = Visibility.Collapsed;
                        }
                    }
                } 
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void dgChequeEntryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dgChequeEntryList.SelectedIndex != -1)
                {
                    var row = dgChequeEntryList.SelectedItem as DataRowView;
                    if (row != null)
                    {
                        // string gamePath = row["ChequeEntryID"].ToString();
                        ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                    }


                    /*User Access*/
                    if (FilteredUserMenuList.Count > 0)
                    {
                        if (FilteredUserMenuList[0].AllowEdit == true)
                        {
                            Edit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetChequeEntryList()
        {
            try
            {
                elist = new List<string>();
                elist.Add("FromDate");
                elist.Add("ToDate");
                elist.Add("ChequeNo");
                objPara = new clsChequeEntrySearchParameter();
                objPara.FromDate = dtpFromDate.SelectedDate.Value;
                objPara.ToDate = dtpToDate.SelectedDate.Value;
                objPara.ChequeNo = txtChequeNo.Text.Trim();
                _ds = (new BALChequeEntry().GetChequeEntryList(objPara, elist));
                dgChequeEntryList.ItemsSource = _ds.Tables[0].DefaultView;
                lblStatus.Text = _ds.Tables[0].Rows.Count + " Record Found";
                if (_ds.Tables[0].Rows.Count > 0)
                {
                    double TotalNetAmount = Convert.ToDouble(_ds.Tables[0].Compute("SUM(ChequeAmount)", string.Empty));
                    //lblNetAmountValue.Content = String.Format("{0:N}", TotalNetAmount);
                    //lblNetAmountValue.ContentStringFormat = "{}{0:N0}";

                    //double parsed = double.Parse(TotalNetAmount.ToString(), CultureInfo.InvariantCulture);
                    //CultureInfo hindi = new CultureInfo("hi-IN");
                    //string text = string.Format(hindi, "{0:c}", parsed);
                    //lblNetAmountValue.Content = text.Substring(1);

                    lblNetAmountValue.Content = FormatStringIndianCurrency(TotalNetAmount.ToString ());



                }
                else
                {
                    // MessageBox.Show("No Record found");
                    lblStatus.Text = "No Record Found";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            GetChequeEntryList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ChequeEntry obj =  new ChequeEntry();
            obj.objChequeEntryList = this;
            obj.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (dgChequeEntryList.SelectedIndex != -1)
                {
                    var row = dgChequeEntryList.SelectedItem as DataRowView;
                    if (row != null)
                    {
                        // string gamePath = row["ChequeEntryID"].ToString();
                        ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                    }


                    /*User Access*/
                    if (FilteredUserMenuList.Count > 0)
                    {
                        if (FilteredUserMenuList[0].AllowEdit == true)
                        {
                            Edit();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Row", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void Edit()
        {
            ChequeEntry obj = new ChequeEntry();
            obj.ChequeEntryID = ChequeEntryID;
            obj.objChequeEntryList = this;
            obj.ShowDialog();
        }

        private void dgChequeEntryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                if (dgChequeEntryList.SelectedIndex != -1)
                {
                    var row = dgChequeEntryList.SelectedItem as System.Data.DataRowView;
                    if (row != null)
                    {
                        // string gamePath = row["ChequeEntryID"].ToString();
                        ChequeEntryID = Convert.ToInt64(row["ChequeEntryID"].ToString());
                        GetCheque(ChequeEntryID);
                       
                    }

                   // Edit();
                    // process stuff
                }
                else
                {
                    //MessageBox.Show("Please Select Row", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetCheque(long ChequeEntryID )
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
        private string FormatStringIndianCurrency(string TotalNetAmount) {
            double parsed = double.Parse(TotalNetAmount.ToString(), CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            string text = string.Format(hindi, "{0:c}", parsed);
            //lblNetAmountValue.Content = text.Substring(1);
            return text.Substring(1);
        }

        private void btnCS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtChequeNo.Text.Trim().Length > 0 && txtChequeNo.Text.Trim() != "")
                {
                    elist = new List<string>();
                    // elist.Add("FromDate");
                    // elist.Add("ToDate");
                    elist.Add("ChequeNo");
                    objPara = new clsChequeEntrySearchParameter();
                    //objPara.FromDate = dtpFromDate.SelectedDate.Value;
                    //objPara.ToDate = dtpToDate.SelectedDate.Value;
                    objPara.ChequeNo = txtChequeNo.Text.Trim();
                    _ds = (new BALChequeEntry().GetChequeEntryList(objPara, elist));
                    dgChequeEntryList.ItemsSource = _ds.Tables[0].DefaultView;
                    lblStatus.Text = _ds.Tables[0].Rows.Count + " Record Found";
                    if (_ds.Tables[0].Rows.Count > 0)
                    {
                        double TotalNetAmount = Convert.ToDouble(_ds.Tables[0].Compute("SUM(ChequeAmount)", string.Empty));

                        lblNetAmountValue.Content = FormatStringIndianCurrency(TotalNetAmount.ToString());



                    }
                    else
                    {
                        // MessageBox.Show("No Record found");
                        lblStatus.Text = "No Record Found";
                    }

                }
                else {
                    MessageBox.Show("Enter Cheque", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }


             
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
