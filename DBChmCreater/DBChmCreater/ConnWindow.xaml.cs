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

namespace DBChmCreater
{
    /// <summary>
    /// Interaction logic for wndConn.xaml
    /// </summary>
    public partial class ConnWindow : Window
    {
        public bool chosed { get; set; }
        public int index { get; set; }
        public string conn { get; set; }


        public ConnWindow(int idx)
        {
            InitializeComponent();

            this.index = idx;
            cbbDbType.SelectedIndex = idx;
        }


        private void ckbSimple_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (ckbSimple.IsChecked.Value)
            {
                txtServer.IsReadOnly = true;
            }
            else
            {
                txtServer.IsReadOnly = false;
            }
            SetConn(sender, e);
        }

        private void ckbWindow_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (ckbWindow.IsChecked.Value)
            {
                txtUserID.IsReadOnly = true;
                
                //txtPassword.IsReadOnly = true;
            }
            else
            {
                txtUserID.IsReadOnly = false;
                //txtPassword.IsReadOnly = false;
            }
            SetConn(sender, e);
        }
        private void okbtn_Click(object sender, RoutedEventArgs e)
        {
            SetConn(sender, e);
            chosed = true;
            conn = txtConn.Text; ;
            index = cbbDbType.SelectedIndex;
            this.Close();
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            chosed = false;

            this.Close();

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Control textBox = e.Source as Control;
            if (textBox.Tag != null)
            {
                SetConn(sender, e);
            }
        }

        private void SetConn(object sender, RoutedEventArgs e)
        {
            if (cbbDbType.Text.StartsWith("SQL"))
            {
                if (ckbWindow.IsChecked.Value)
                {
                    txtConn.Text = $"server={txtServer.Text};database={txtDbName.Text};integrated security=sspi;";
                }
                else
                {
                    txtConn.Text = $"server={txtServer.Text};database={txtDbName.Text};uid={txtUserID.Text};pwd={txtPassword.Password}";
                }
            }
            else if (cbbDbType.Text.StartsWith("Oracle"))
            {
                if (ckbSimple.IsChecked.Value)
                {
                    txtConn.Text = $"Data Source={txtDbName.Text};User ID={txtUserID.Text};pwd={txtPassword.Password}";
                }
                else
                {
                    txtConn.Text = $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={txtServer.Text})  (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME={txtDbName.Text})));Persist Security Info=True;User Id={txtUserID.Text}; Password={txtPassword.Password}";
                }
            }
            else if (cbbDbType.Text.StartsWith("Postgresql"))
            {
                if (ckbWindow.IsChecked.Value)
                {
                    txtConn.Text = $"server={txtServer.Text};database={txtDbName.Text};integrated security=sspi;";
                }
                else
                {
                    txtConn.Text = $"server={txtServer.Text};database={txtDbName.Text};uid={txtUserID.Text};pwd={txtPassword.Password}";
                }

            }
        }

        private void cbbDbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (cbbDbType.SelectedValue.ToString().StartsWith("Oracle"))
            {
                ckbSimple.IsChecked = true;
                ckbSimple.Visibility = Visibility.Visible;// IsVisible = true;
                ckbWindow.Visibility = Visibility.Hidden;//IsVisible = false;
                txtServer.IsReadOnly = true;
            }
            else
            {
                txtServer.IsReadOnly = false;
                ckbSimple.Visibility = Visibility.Hidden;//IsVisible = false;
                ckbWindow.Visibility = Visibility.Visible;//IsVisible = true;
            }
        }
    }
}
