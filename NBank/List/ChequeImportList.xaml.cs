using BALNBank;
using BOLNBank;
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

namespace NBank.List
{
    /// <summary>
    /// Interaction logic for ChequeImportList.xaml
    /// </summary>
    public partial class ChequeImportList : Window
    {
        List<ImportLogModel> list;
        string MessageTitle = "Cheque Import List";
        public ChequeImportList()
        {
            InitializeComponent();
        }
        private void btnControlMinimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                }
                else if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Minimized;
                }
                else
                {
                    WindowState = WindowState.Minimized;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnControlMaximize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnControlClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //UserMenu();

               // Keyboard.Focus(txtAccountName);
                GetImportLogList();
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
        public void GetImportLogList()
        {
            try
            {
                list = (new BALChequeEntry().GetImportLogList());

                dgImportHistory.ItemsSource = list;

                lblStatus.Text = "Rows " + list.Count;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
