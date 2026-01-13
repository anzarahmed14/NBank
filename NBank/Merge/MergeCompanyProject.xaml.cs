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
using BALNBank;
using System.Data;

namespace NBank.Merge
{
    /// <summary>
    /// Interaction logic for Merge.xaml
    /// </summary>
    public partial class Merge : Window
    {
        List<clsCompany> list;
        List<clsProject> plist;
        long ToCompanyID = 0;
        long FromCompanyID = 0;
        long ToProjectID = 0;
        long FromProjectID = 0;
        string Message = "";
        DataSet _ds;

        public Merge()
        {
            InitializeComponent();
        }

        private void frmMerge_Loaded(object sender, RoutedEventArgs e)
        {
            list = (new BALCompany().GetCompanyList());
          

            list.Insert(0, new clsCompany() { CompanyID = -1, CompanyShortName = "--Select From Company--" });

            cmbFromCompanyList.ItemsSource = list;
            cmbFromCompanyList.DisplayMemberPath = "CompanyShortName";
            cmbFromCompanyList.SelectedValuePath = "CompanyID";
            cmbFromCompanyList.SelectedValue = -1;

            list.RemoveAt(0);
            list.Insert(0, new clsCompany() { CompanyID = -1, CompanyShortName = "--Select To Company--" });
            cmbToCompanyList.ItemsSource = list;
            cmbToCompanyList.DisplayMemberPath = "CompanyShortName";
            cmbToCompanyList.SelectedValuePath = "CompanyID";
            cmbToCompanyList.SelectedValue = -1;



            /*Start For Project */
            plist = new List<clsProject>();
            plist.Insert(0, new clsProject() { ProjectID = -1, ProjectShortName = "--Select From Project Short Name--" });
            cmbFromCompanyProjectList.ItemsSource = plist;
            cmbFromCompanyProjectList.DisplayMemberPath = "ProjectShortName";
            cmbFromCompanyProjectList.SelectedValuePath = "ProjectID";
            cmbFromCompanyProjectList.SelectedValue = -1;


            plist = new List<clsProject>();
            plist.Insert(0, new clsProject() { ProjectID = -1, ProjectShortName = "--Select From To Short Name--" });
            cmbToCompanyProjectList.ItemsSource = plist;
            cmbToCompanyProjectList.DisplayMemberPath = "ProjectShortName";
            cmbToCompanyProjectList.SelectedValuePath = "ProjectID";
            cmbToCompanyProjectList.SelectedValue = -1;

            /*Start For Project */



        }

        private void cmbToCompanyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("TO");
            ToCompanyID = Convert.ToInt64(cmbToCompanyList.SelectedValue);
            GetToCompanyProject();
           
        }

        private void cmbFromCompanyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("FROM");
            FromCompanyID = Convert.ToInt64(cmbFromCompanyList.SelectedValue);
            GetFromCompanyProject();
        }
        private void GetToCompanyProject() {
            plist = new List<clsProject>();
            plist = (new BALProject().GetProjectList());
           List<clsProject> CompanyProject = plist.FindAll(x => x.CompanyID == ToCompanyID);

            cmbToCompanyProjectList.ItemsSource = CompanyProject;
            cmbToCompanyProjectList.DisplayMemberPath = "ProjectShortName";
            cmbToCompanyProjectList.SelectedValuePath = "ProjectID";
           
        }
        private void GetFromCompanyProject()
        {
            plist = new List<clsProject>();
            plist = (new BALProject().GetProjectList());
            List<clsProject> CompanyProject = plist.FindAll(x => x.CompanyID == FromCompanyID);

            cmbFromCompanyProjectList.ItemsSource = CompanyProject;
            cmbFromCompanyProjectList.DisplayMemberPath = "ProjectShortName";
            cmbFromCompanyProjectList.SelectedValuePath = "ProjectID";
        }

        private void cmbToCompanyProjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ToProjectID = Convert.ToInt64(cmbToCompanyProjectList.SelectedValue);
            //MessageBox.Show("ToProject");


        }
        private void cmbFromCompanyProjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("FromProject");
            //FromProjectID = Convert.ToInt64(cmbFromCompanyProjectList.SelectedValue);
        }

        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidate())
                {

                    _ds = new DataSet();
                    _ds =   GetMergeRecord();

                    //Message = "There are " + _ds.Tables[0].Rows.Count + " record found " +Environment.NewLine;
                    //Message += "From Company Code " + cmbFromCompanyList.Text + "  and From Project" + cmbFromCompanyProjectList.Text + Environment.NewLine;
                    //Message += "To Company Code " + cmbToCompanyList.Text + " and To Project " + cmbToCompanyProjectList.Text + Environment.NewLine;

                    //MessageBox.Show(cmbFromCompanyList.Text);

                    //Message += "All data with Company Code " + cmbFromCompanyList.Text + "\"  and Project Code " + cmbFromCompanyProjectList.Text+" will be" + Environment.NewLine;
                    //Message += "MERGED with Company Code " + cmbToCompanyList.Text + " and Project Code " + cmbToCompanyProjectList.Text +" " + Environment.NewLine;
                    //Message += "Records: "+ _ds.Tables[0].Rows.Count + ""+Environment.NewLine;
                    //Message += "Important:" + Environment.NewLine;
                    //Message += "This is critical data changes, once data is merged this can not be undone" + Environment.NewLine;
                    //Message += "Proceed for Merging?" + Environment.NewLine;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("All data with Company Code ");
                    sb.Append("\"");
                    sb.Append("" + cmbFromCompanyList.Text + "");
                    sb.Append("\"");
                    sb.Append(" and Project Code ");
                    sb.Append("\"");
                    sb.Append("" + cmbFromCompanyProjectList.Text + "");
                    sb.Append("\"");
                    sb.AppendLine(" will be ");
                    sb.Append("MERGED with Company Code ");
                    sb.Append("\"");
                    sb.Append("" + cmbToCompanyList.Text + "");
                    sb.Append("\"");
                    sb.Append(" and Project Code ");
                    sb.Append("\"");
                    sb.Append("" + cmbToCompanyProjectList.Text + "");
                    sb.Append("\"");
                    sb.AppendLine("");

                    sb.AppendLine("Records: " + _ds.Tables[0].Rows.Count);

                    sb.AppendLine("Important: ");
                    sb.AppendLine("This is critical data changes, once data is merged this can not be undone.");
                   


                    sb.AppendLine("Proceed for Merging?");

                    MessageBoxResult obj = MessageBox.Show(sb.ToString(), "Merge Confirmation", MessageBoxButton.YesNo);


                   if (obj == MessageBoxResult.Yes)
                    {
                        CreateMerge();
                        //MessageBox.Show("Yes");
                    }
                    else
                    {
                       // MessageBox.Show("NO");
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void CreateMerge()
        {
            try
            {
                Message = (new BALMerge().CreateMerge(new clsMerge { FromCompanyID = FromCompanyID, ToCompanyID = ToCompanyID, FromProjectID = FromProjectID,ToProjectID=ToProjectID, CreatedDate = DateTime.Now,CreatedUserID = Globals.UserID }));
                if (Message =="SAVE")
                MessageBox.Show("Data Merge successfully.", "Merge", MessageBoxButton.OK,MessageBoxImage.Information);
                else
                    MessageBox.Show(Message, "Merge", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public DataSet GetMergeRecord()
        {
            _ds  = new DataSet();
            _ds = (new BALMerge().GetMergeRecord(new clsMerge { FromCompanyID = FromCompanyID, ToCompanyID = ToCompanyID, FromProjectID = FromProjectID, ToProjectID = ToProjectID }));

            return _ds;
        }
        private bool IsValidate() {
           bool IsValid = false;
            Message = "";
            try
            {
                ToCompanyID = Convert.ToInt64(cmbToCompanyList.SelectedValue);
                FromCompanyID = Convert.ToInt64(cmbFromCompanyList.SelectedValue);

                ToProjectID = Convert.ToInt64(cmbToCompanyProjectList.SelectedValue);

                FromProjectID = Convert.ToInt64(cmbFromCompanyProjectList.SelectedValue);

                if (ToCompanyID == 0 || ToCompanyID == -1) {
                    Message += "Select To Company" + System.Environment.NewLine;
                }
                if (FromCompanyID == 0 || FromCompanyID == -1)  {
                    Message += "Select From Company" + System.Environment.NewLine;
                }

                if (ToProjectID == 0 || ToProjectID == -1) {
                    Message += "Select To Project" + System.Environment.NewLine;
                }
                if (FromProjectID == 0 || FromProjectID == -1)  {
                    Message += "Select From Project" + System.Environment.NewLine;
                }

                if (Message.Length > 0)
                {
                    IsValid = false;
                    MessageBox.Show(Message);
                }
                else {
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
