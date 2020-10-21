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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace GCISApp
{
    /// <summary>
    /// Lógica de interacción para LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private bool badlogin = false;

        public LoginPage()
        {
            InitializeComponent();
            Username.Focus();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private bool AuthenticateUser(string domain, string user, string password)
        {
            bool result = false;
            try
            {
                System.DirectoryServices.DirectoryEntry de = new System.DirectoryServices.DirectoryEntry("LDAP://" + domain, user, password);
                System.DirectoryServices.DirectorySearcher ds = new System.DirectoryServices.DirectorySearcher(de);
                System.DirectoryServices.SearchResult sr = null;
                sr = ds.FindOne();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        private void Password_KeyUp(object sender, KeyEventArgs e)
        {
            if (badlogin == false)
            {
                if (e.Key.ToString().Equals("Return"))
                {
                    Login();
                }
            }
            else
            {
                badlogin = false;
            }
        }

        private void Login()
        {
            if (AuthenticateUser("eegsacorp", Username.Text, Password.Password))
            {
                WSGCIS.GCISClient client = new WSGCIS.GCISClient();
                WSGCIS.Data acceso = new WSGCIS.Data();
                acceso = client.SelectAcceso(Username.Text.Trim());
                if (acceso.Tabla.Rows.Count > 0)
                {
                    App.Current.Properties["Username"] = Username.Text.Trim();
                    //MessageBox.Show(acceso.Tabla.Rows[0][0].ToString());
                    this.NavigationService.Navigate(new MainPage());
                }
                else { MessageBox.Show("El usuario no tiene acceso a la aplicación."); }
            }
            else { badlogin = true; Password.Password = ""; MessageBox.Show("Usuario no autenticado."); }
        }
    }
}
