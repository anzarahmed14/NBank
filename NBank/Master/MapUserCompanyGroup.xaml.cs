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
    /// Interaction logic for MapUserCompanyGroup.xaml
    /// </summary>
    public partial class MapUserCompanyGroup : Window
    {
        string MessageTitle = "Map User Company Group";
        public MapUserCompanyGroupList objMapCompanyGroupList;
        public long CompanyGroupID = 0;
        public long UserId = 0;
        string Message = "";

        BALMapUserCompanyGroup bALMapUserCompanyGroup;

        List<clsMapUserCompanyGroup> mapUserGroupList;
        public MapUserCompanyGroup()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindUser();
            BindCompanyGroup();
          
            Keyboard.Focus(cmbUser);
            try
            {
                if (UserId > 0)
                {
                    cmbUser.IsEnabled = false;
                    SetSelectedGroups();
                    btnSave.Content = "_Update";

                    cmbUser.SelectedValue = UserId;
                }
                //else
                //{
                //    // chkIsActive.IsChecked = true;

                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        private void SetSelectedGroups()
        {
            // 1️⃣ Get mapped groups for the user
            bALMapUserCompanyGroup = new BALMapUserCompanyGroup();
            mapUserGroupList = bALMapUserCompanyGroup.GetCompanyGroupByUserId(UserId);

            if (mapUserGroupList == null)
                mapUserGroupList = new List<clsMapUserCompanyGroup>();

            // 2️⃣ Store mapped CompanyGroupIds
            HashSet<long> mappedGroupIds = new HashSet<long>();
            foreach (var item in mapUserGroupList)
            {
                mappedGroupIds.Add(item.CompanyGroupId);
            }

            // 3️⃣ Get current list in ORIGINAL order
            var originalList = lstCompanyGroup.Items
                                              .Cast<clsCompanyGroupCompanyNamesList>()
                                              .ToList();

            // 4️⃣ Move selected groups to TOP (NO sorting)
            var selected = new List<clsCompanyGroupCompanyNamesList>();
            var unselected = new List<clsCompanyGroupCompanyNamesList>();

            foreach (var group in originalList)
            {
                if (mappedGroupIds.Contains(group.CompanyGroupID))
                    selected.Add(group);
                else
                    unselected.Add(group);
            }

            // 5️⃣ Rebind ONCE (selected first)
            var finalList = new List<clsCompanyGroupCompanyNamesList>();
            finalList.AddRange(selected);
            finalList.AddRange(unselected);

            lstCompanyGroup.ItemsSource = finalList;
            lstCompanyGroup.UpdateLayout();

            // 6️⃣ Pre-check selected groups
            foreach (var item in lstCompanyGroup.Items)
            {
                ListBoxItem listBoxItem =
                    lstCompanyGroup.ItemContainerGenerator
                                   .ContainerFromItem(item) as ListBoxItem;

                if (listBoxItem != null)
                {
                    CheckBox chk = FindVisualChild<CheckBox>(listBoxItem);
                    if (chk != null)
                    {
                        long groupId = Convert.ToInt64(chk.Tag);
                        chk.IsChecked = mappedGroupIds.Contains(groupId);
                    }
                }
            }
        }

        private void BindUser()
        {
            BALUser bal = new BALUser();

            var users = bal.GetUserList();

            cmbUser.ItemsSource = users;

            if (users.Count > 0)
                cmbUser.SelectedIndex = 0;
        }
        private void BindCompanyGroup()
        {
            BALCompanyGroup bal = new BALCompanyGroup();

            var companyGroups = bal.GetCompanyGroupList()
                               .Where(x => x.IsActive)
                               .ToList();

            lstCompanyGroup.ItemsSource = companyGroups;
        }
        private void frmMapUserCompanyGroup_KeyDown(object sender, KeyEventArgs e)
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

                if (UserId > 0)
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objMapCompanyGroupList.GetMapCompanyGroupList();

                Close();
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
                // Validate Company Group
                if (cmbUser.SelectedValue == null)
                {
                    lblStatus.Text = "Please select Company Group";
                    return;
                }

                long companyGroupId = Convert.ToInt64(cmbUser.SelectedValue);

                // Get selected Company IDs from CheckBoxes
                List<long> selectedCompanyIds = new List<long>();

                foreach (var item in lstCompanyGroup.Items)
                {
                    ListBoxItem listBoxItem =
                        lstCompanyGroup.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

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
                Message = (new BALMapUserCompanyGroup())
                    .Update(companyGroupId, selectedCompanyIds );

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
        public void Create()
        {
            try
            {
                // Validate Company Group
                if (cmbUser.SelectedValue == null)
                {
                    lblStatus.Text = "Please select Company Group";
                    return;
                }

                 var userId = Convert.ToInt64(cmbUser.SelectedValue);

                // Get selected Company IDs from CheckBoxes
                List<long> selectedCompanyIds = new List<long>();

                foreach (var item in lstCompanyGroup.Items)
                {
                    ListBoxItem listBoxItem =
                        lstCompanyGroup.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

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
                Message = (new BALMapUserCompanyGroup())
                    .Create(userId, selectedCompanyIds);

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
    }
}
