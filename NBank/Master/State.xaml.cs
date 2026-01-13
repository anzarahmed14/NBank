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
    /// Interaction logic for State.xaml
    /// </summary>
    public partial class State : Window
    {
        internal long StateID;
        public StateList objStateList { get; internal set; }
        bool Isvalid = false;
        string Message = "";
        string MessageTitle = "State Master";
        clsState obj;
        public State()
        {
            InitializeComponent();
        }
        private void frmState_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtStateName);
            try
            {
                if (StateID > 0)
                {
                    GetState();
                    btnSave.Content = "_Update";
                }
                else
                {
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
                    if (StateID > 0)
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

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objStateList.GetStateList();
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void GetState()
        {
            obj = new clsState();
            obj = (new BALState().GetState(StateID));
            txtStateName.Text = obj.StateName;
            txtStateShortName.Text = obj.StateShortName;
            if (obj.IsActive == true)
            {
                chkIsActive.IsChecked = true;
            }
            else
            {
                chkIsActive.IsChecked = false;
            }

        }
        public bool IsValidate()
        {
            Message = "";
            //MessageBox.Show(cmbDefaultBank.SelectedIndex.ToString());
            if (txtStateName.Text.Trim() == "")
            {

                Message += " Enter State Name \n";
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
        public void Create()
        {
            obj = new clsState();
            obj.StateName = txtStateName.Text.Trim();
            obj.StateShortName = txtStateShortName.Text.Trim();
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
                //MessageBox.Show("Record saved successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                lblStatus.Text = "Record saved successfully";
                Initialize();
            }
            else
            {
                MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);

                lblStatus.Text = "";
            }
        }
        public void Update()
        {
            obj = new clsState();
            obj.StateName = txtStateName.Text.Trim();
            obj.StateShortName = txtStateShortName.Text.Trim();
            if (chkIsActive.IsChecked ?? true)
            {
                obj.IsActive = true;
            }
            else
            {
                obj.IsActive = true;
            }

            obj.StateID = StateID;

            Message = (new BALOperation().Update(obj));
            if (Message == "SAVE")
            {
                //  objAccountList.GetAccount();
               // MessageBox.Show("Record updated successfully", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                lblStatus.Text = "Record updated successfully";
            }
            else
            {
                MessageBox.Show(Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);

                lblStatus.Text = "";
            }
        }
        private void Initialize()
        {
            try
            {
                txtStateName.Text = "";
                txtStateShortName.Text = "";
                chkIsActive.IsChecked = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "003", MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void frmState_KeyDown(object sender, KeyEventArgs e)
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
