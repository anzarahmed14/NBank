using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Collections.Specialized;
using System.Configuration;
namespace NBank
{
    /// <summary>
    /// Interaction logic for ReportPreview.xaml
    /// </summary>
    public partial class ReportPreview : Window
    {
        string MessageTitle = "Report Preview";
        /*Connection */
        string ServerName = ConfigurationManager.AppSettings["ServerName"];
        string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        string UserID  = ConfigurationManager.AppSettings["UserID"];
        string Password   = ConfigurationManager.AppSettings["Password"];
        /*End Connection*/

        public Dictionary<string, string> ParaField = null;
        public string ReportName = "";


        ParameterFieldDefinitions crParameterFieldDefinitions  ;
        ParameterFieldDefinition crParameterFieldDefinition  ;
        ParameterValues crParameterValues = new ParameterValues();
        ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

        /*Connection String*/
       

        public ReportPreview()
        {
            InitializeComponent();
        }
        private void frmReportPreview_Loaded(object sender, RoutedEventArgs e)
        {
            //CrystalReportViewer1 .Owner = Window.GetWindow(this);

            PrintReport();
        }
        public void PrintReport()
        {
            if (Environment.MachineName == "USERP45")
            {
                /*Connection */
                //ServerName = "USERP45\\SQL_SERVER_2012";
               // DatabaseName = "NBank";
               // UserID = "sa";
               // Password = "sa@2014";
                /*End Connection*/
            }
            else
            {        /*Connection */
               // ServerName = "208.91.198.59";
               // DatabaseName = "TestDemo";
               // UserID = "TestDemo";
               // Password = "Anzar@2014";
                /*End Connection*/

            }

            try
            {

                 string Str = System.AppDomain.CurrentDomain.BaseDirectory + "ReportFile\\" + ReportName;
                //string Str = "E:\\Project\\WPFProject\\NBank\\NBank\\ReportFile\\" + ReportName;
                /*
                if (!System.IO.Directory.Exists(Str))
                {
                    System.IO.Directory.CreateDirectory(Str);
                }
                Str = Str + ReportName;
                */


                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(Str);

                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                crConnectionInfo.ServerName = ServerName;
                crConnectionInfo.DatabaseName = DatabaseName;
                crConnectionInfo.UserID = UserID;
                crConnectionInfo.Password = Password;

                CrTables = cryRpt.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

             
                if (ParaField != null)
                {
                    if (ParaField.Count > 0)
                    {

                        foreach (KeyValuePair<string, string> custKeyVal in ParaField)
                        {
                            crParameterValues = new ParameterValues();
                            crParameterDiscreteValue = new ParameterDiscreteValue();

                            //Parameter Field
                            //Base On the Field Value Select the Recored from the Data Base
                            crParameterDiscreteValue.Value = custKeyVal.Value;
                            //-- Value of crosponding Field
                            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
                            //Parameter Field Name In Crystal reports 
                            //crParameterFieldDefinition =  crParameterFieldDefinitions.Item(custKeyVal.Key);

                            crParameterFieldDefinition = crParameterFieldDefinitions[custKeyVal.Key];

                            //--Name In the Crystal report
                            crParameterValues = crParameterFieldDefinition.CurrentValues;
                            crParameterValues.Clear();
                            crParameterValues.Add(crParameterDiscreteValue);
                            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                            //End Parameter Field
                        }
                    }
                }

               // CrystalReportViewer1.Owner = Window.GetWindow(this);
                CrystalReportViewer1.ViewerCore.ReportSource = cryRpt;
                
                //CrystalReportViewer1.Owner = Window.GetWindow(this);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "001", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
   
}
