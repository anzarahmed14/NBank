using BALNBank;
using BOLNBank;
using NBank.List;
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

namespace NBank.Master
{
    /// <summary>
    /// Interaction logic for MapCompanyGroup.xaml
    /// </summary>
    public partial class MapCompanyGroup : Window
    {
        public MapCompanyGroupList objMapCompanyGroupList;
        public long CompanyGroupID = 0;
        string MessageTitle = "Map Company Group";
        string Message = "";
        BALMapCompanyGroup bALMapCompanyGroup;
        List<clsMapCompanyGroup> mapCompanyList;
        public MapCompanyGroup()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindCompanyGroups();
            BindCompanies();
            Keyboard.Focus(cmbCompanyGroup);
            try
            {
                if (CompanyGroupID > 0)
                {
                    cmbCompanyGroup.IsEnabled = false;
                    SetSelectedCompanies();
                    btnSave.Content = "_Update";

                    cmbCompanyGroup.SelectedValue = CompanyGroupID;
                }
                else
                {
                   // chkIsActive.IsChecked = true;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        private void SetSelectedCompanies()
        {
            bALMapCompanyGroup = new BALMapCompanyGroup();
            mapCompanyList = bALMapCompanyGroup.GetByCompanyGroupId(CompanyGroupID);

            if (mapCompanyList == null)
                mapCompanyList = new List<clsMapCompanyGroup>();

            // Step 1: Get mapped company IDs
            HashSet<long> mappedCompanyIds = new HashSet<long>();
            foreach (var item in mapCompanyList)
            {
                mappedCompanyIds.Add(item.CompanyId);
            }

            // Step 2: Get current company list
            var allCompanies = lstCompanies.Items.Cast<clsCompany>().ToList();

            // Step 3: Split selected & unselected
            var selectedCompanies = allCompanies
                .Where(c => mappedCompanyIds.Contains(c.CompanyID))
                .OrderBy(c => c.CompanyName)
                .ToList();

            var unSelectedCompanies = allCompanies
                .Where(c => !mappedCompanyIds.Contains(c.CompanyID))
                .OrderBy(c => c.CompanyName)
                .ToList();

            // Step 4: Merge (Selected first)
            var finalList = new List<clsCompany>();
            finalList.AddRange(selectedCompanies);
            finalList.AddRange(unSelectedCompanies);

            // Step 5: Rebind ListBox
            lstCompanies.ItemsSource = finalList;

            // Step 6: Check selected companies
            lstCompanies.UpdateLayout();

            foreach (var item in lstCompanies.Items)
            {
                ListBoxItem listBoxItem =
                    lstCompanies.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                if (listBoxItem != null)
                {
                    CheckBox chk = FindVisualChild<CheckBox>(listBoxItem);
                    if (chk != null)
                    {
                        long companyId = Convert.ToInt64(chk.Tag);
                        chk.IsChecked = mappedCompanyIds.Contains(companyId);
                    }
                }
            }
        }

        


        private void BindCompanyGroups()
        {
            BALCompanyGroup bal = new BALCompanyGroup();

            var groups = bal.GetCompanyGroupList();

            cmbCompanyGroup.ItemsSource = groups;

            if (groups.Count > 0)
                cmbCompanyGroup.SelectedIndex = 0;
        }
        private void BindCompanies()
        {
            BALCompany bal = new BALCompany();

            var companies = bal.GetCompanyList()
                               .Where(x => x.IsActive)
                               .ToList();

            lstCompanies.ItemsSource = companies;
        }
        private void frmMapCompanyGroup_KeyDown(object sender, KeyEventArgs e)
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
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (CompanyGroupID > 0)
                {
                    Update();
                }
                else
                {
                    Create();
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
                // Validate Company Group
                if (cmbCompanyGroup.SelectedValue == null)
                {
                    lblStatus.Text = "Please select Company Group";
                    return;
                }

                long companyGroupId = Convert.ToInt64(cmbCompanyGroup.SelectedValue);

                // Get selected Company IDs from CheckBoxes
                List<long> selectedCompanyIds = new List<long>();

                foreach (var item in lstCompanies.Items)
                {
                    ListBoxItem listBoxItem =
                        lstCompanies.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                    if (listBoxItem != null)
                    {
                        CheckBox chk = FindVisualChild<CheckBox>(listBoxItem);
                        if (chk != null && chk.IsChecked == true)
                        {
                            selectedCompanyIds.Add(Convert.ToInt64(chk.Tag));
                        }
                    }
                }

                if (selectedCompanyIds.Count == 0)
                {
                    lblStatus.Text = "Please select at least one Company";
                    return;
                }

                // Call BAL
                Message = (new BALMapCompanyGroup())
                    .CreateMapCompanyGroup(selectedCompanyIds, companyGroupId);

                if (Message == "SAVE" || Message.Contains("success"))
                {
                    lblStatus.Text = "Record saved successfully";
                   // Initialize();
                }
                else
                {
                    MessageBox.Show(Message, MessageTitle,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void Update()
        {
            try
            {
                // Validate Company Group
                if (cmbCompanyGroup.SelectedValue == null)
                {
                    lblStatus.Text = "Please select Company Group";
                    return;
                }

                long companyGroupId = Convert.ToInt64(cmbCompanyGroup.SelectedValue);

                // Get selected Company IDs from CheckBoxes
                List<long> selectedCompanyIds = new List<long>();

                foreach (var item in lstCompanies.Items)
                {
                    ListBoxItem listBoxItem =
                        lstCompanies.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                    if (listBoxItem != null)
                    {
                        CheckBox chk = FindVisualChild<CheckBox>(listBoxItem);
                        if (chk != null && chk.IsChecked == true)
                        {
                            selectedCompanyIds.Add(Convert.ToInt64(chk.Tag));
                        }
                    }
                }

                if (selectedCompanyIds.Count == 0)
                {
                    lblStatus.Text = "Please select at least one Company";
                    return;
                }

                // Call BAL
                Message = (new BALMapCompanyGroup())
                    .UpdateMapCompanyGroup(selectedCompanyIds, companyGroupId);

                if (Message == "SAVE" || Message.Contains("success"))
                {
                    lblStatus.Text = "Record saved successfully";
                    // Initialize();
                }
                else
                {
                    MessageBox.Show(Message, MessageTitle,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    lblStatus.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                T result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // objBankList.GetBankList();

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
