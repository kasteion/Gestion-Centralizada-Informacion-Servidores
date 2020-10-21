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
using System.Configuration;

namespace GCISApp
{
    /// <summary>
    /// Lógica de interacción para SANPage.xaml
    /// </summary>
    public partial class SANPage : Page
    {

        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public SANPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectSAN":
                        BtnSelect.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertSAN":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateSAN":
                        BtnEditar.IsEnabled = true;
                        BtnGuardarE.IsEnabled = true;
                        break;
                    case "DeleteSAN":
                        BtnBorrarE.IsEnabled = true;
                        break;
                    case "SelectUrlAdministracionSAN":
                        BtnEditURL.IsEnabled = true;
                        break;
                    case "InsertUrlAdministracionSAN":
                        BtnNewURL.IsEnabled = true;
                        BtnGuardarURL.IsEnabled = true;
                        break;
                    case "UpdateUrlAdministracionSAN":
                        BtnEditURL.IsEnabled = true;
                        BtnUpdateURL.IsEnabled = true;
                        break;
                    case "DeleteUrlAdministracionSAN":
                        BtnDeleteURL.IsEnabled = true;
                        break;
                    case "SelectUsuarioSAN":
                        BtnEditUsuario.IsEnabled = true;
                        break;
                    case "InsertUsuarioSAN":
                        BtnNewUsuario.IsEnabled = true;
                        BtnGuardarUsuario.IsEnabled = true;
                        break;
                    case "UpdateUsuarioSAN":
                        BtnEditUsuario.IsEnabled = true;
                        BtnUpdateUsuario.IsEnabled = true;
                        break;
                    case "DeleteUsuarioSAN":
                        BtnDeleteUsuario.IsEnabled = true;
                        break;
                    case "SelectPoolBySan":
                        BtnEditPool.IsEnabled = true;
                        break;
                    case "InsertPool":
                        BtnNewPool.IsEnabled = true;
                        BtnGuardarPool.IsEnabled = true;
                        break;
                    case "UpdatePool":
                        BtnEditPool.IsEnabled = true;
                        BtnUpdatePool.IsEnabled = true;
                        break;
                    case "DeletePool":
                        BtnDeletePool.IsEnabled = true;
                        break;
                    case "VerPassword":
                        TxtPassword.Visibility = Visibility.Collapsed;
                        VerPassword.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void BtnMaestros_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MaestrosPage());
        }

        private void BtnServidores_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ServidoresPage());
        }

        private void BtnInstancias_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InstanciasPage());
        }

        private void BtnDatabases_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DatabasesPage());
        }

        private void BtnSoluciones_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SolucionesPage());
        }

        private void BtnSAN_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SANPage());
        }

        private void LimpiarGeneral()
        {
            TxtIDSan.Text = "";
            TxtNombreSan.Text = "";
            WSGCIS.Data Datos = client.SelectSAN("%", App.Current.Properties["Username"].ToString());
            DgSAN.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectSAN("%" + TxtNombreSan.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgSAN.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            TxtIDSanN.Text = "";
            TxtNombreSanN.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 1));
        }
    
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgSAN.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    TabEdicion.SelectedIndex = 0;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgSAN.SelectedItems[0];
                    TxtIDSanE.Text = row[0].ToString().Trim();
                    TxtIDSanP.Text = row[0].ToString().Trim();
                    TxtIDSanU.Text = row[0].ToString().Trim();
                    TxtIDSanUrl.Text = row[0].ToString().Trim();
                    TxtNombreSanE.Text = row[1].ToString().Trim();
                    TxtNombreSanP.Text = row[1].ToString().Trim();
                    TxtNombreSanU.Text = row[1].ToString().Trim();
                    TxtNombreSanUrl.Text = row[1].ToString().Trim();
                    WSGCIS.Data Datos = client.SelectUrlAdministracionSAN("%" + row[1].ToString().Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUrl.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectUsuarioSAN("%" + row[1].ToString().Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUsuario.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectPoolBySan("%" + row[1].ToString().Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgPool.ItemsSource = Datos.Tabla.DefaultView;
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
            Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreSanN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una SAN sin NOMBRE.");
            }
            else
            {
                if (client.InsertSAN(TxtNombreSanN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("SAN insertada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al insertar SAN.");
                }
            }
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
            Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 0));
        }

        private void BtnGuardarE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreSanE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una SAN sin NOMBRE.");
            }
            else
            {
                if (client.UpdateSAN(int.Parse(TxtIDSanE.Text.Trim()), TxtNombreSanE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("SAN actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al actualizar SAN.");
                }
            }
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la SAN?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteSAN(int.Parse(TxtIDSanE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("SAN borrada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al borrar SAN.");
                }
            }
        }

        private void BtnNewURL_Click(object sender, RoutedEventArgs e)
        {
            GridNewURL.Visibility = Visibility.Visible;
            GridEditURL.Visibility = Visibility.Collapsed;
            TxtIDUrl.Text = "";
            TxtUrl.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 3));
        }

        private void BtnEditURL_Click(object sender, RoutedEventArgs e)
        {
            if (DgUrl.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgUrl.SelectedItems[0];
                    TxtIDUrl.Text = row[0].ToString().Trim();
                    TxtUrl.Text = row[2].ToString().Trim();
                    GridNewURL.Visibility = Visibility.Collapsed;
                    GridEditURL.Visibility = Visibility.Visible;
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 3));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBackURL_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
        }

        private void BtnGuardarURL_Click(object sender, RoutedEventArgs e)
        {
            if (TxtUrl.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una URL de Administración vacía.");
            }
            else
            {
                if (client.InsertUrlAdministracionSAN(int.Parse(TxtIDSanUrl.Text.Trim()), TxtUrl.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUrlAdministracionSAN("%" + TxtNombreSanUrl.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUrl.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("URL insertada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar URL.");
                }
            }
        }

        private void BtnUpdateURL_Click(object sender, RoutedEventArgs e)
        {
            if (TxtUrl.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una URL de Administración vacía.");
            }
            else
            {
                if (client.UpdateUrlAdministracionSAN(int.Parse(TxtIDUrl.Text), int.Parse(TxtIDSanUrl.Text.Trim()), TxtUrl.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUrlAdministracionSAN("%" + TxtNombreSanUrl.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUrl.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("URL actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar URL.");
                }
            }
        }

        private void BtnDeleteURL_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la URL?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteUrlAdministracionSAN(int.Parse(TxtIDUrl.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUrlAdministracionSAN("%" + TxtNombreSanUrl.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUrl.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("URL borrada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar URL.");
                }
            }
        }

        private void BtnNewUsuario_Click(object sender, RoutedEventArgs e)
        {
            GridNewUsuario.Visibility = Visibility.Visible;
            GridEditUsuario.Visibility = Visibility.Collapsed;
            TxtIDUsuario.Text = "";
            TxtUsuario.Text = "";
            TxtPassword.Password = "";
            VerPassword.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 4));
        }

        private void BtnEditUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsuario.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    
                    System.Data.DataRowView row = (System.Data.DataRowView)DgUsuario.SelectedItems[0];
                    TxtIDUsuario.Text = row[0].ToString().Trim();
                    TxtUsuario.Text = row[2].ToString().Trim();
                    TxtPassword.Password = Core.Utils.StringCipher.Decrypt(row[3].ToString().Trim(), ConfigurationManager.AppSettings["UsuarioSAN"].Trim());
                    VerPassword.Text = Core.Utils.StringCipher.Decrypt(row[3].ToString().Trim(), ConfigurationManager.AppSettings["UsuarioSAN"].Trim());
                    GridNewUsuario.Visibility = Visibility.Collapsed;
                    GridEditUsuario.Visibility = Visibility.Visible;
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 4));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (VerPassword.Visibility == Visibility.Visible)
            {
                TxtPassword.Password = VerPassword.Text;
            }
            if (TxtUsuario.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un USUARIO vacío.");
            }
            else if (TxtPassword.Password.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un usuario sin PASSWORD.");
            }
            else
            {
                if (client.InsertUsuarioSAN(int.Parse(TxtIDSanU.Text.Trim()), TxtUsuario.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPassword.Password.Trim(), ConfigurationManager.AppSettings["UsuarioSAN"].Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioSAN("%" + TxtNombreSanU.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUsuario.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar USUARIO.");
                }
            }
        }

        private void BtnUpdateUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (VerPassword.Visibility == Visibility.Visible)
            {
                TxtPassword.Password = VerPassword.Text;
            }
            if (TxtUsuario.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un USUARIO vacío.");
            }
            else if (TxtPassword.Password.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un usuario sin PASSWORD.");
            }
            else
            {
                if (client.UpdateUsuarioSAN(int.Parse(TxtIDUsuario.Text.Trim()), int.Parse(TxtIDSanU.Text.Trim()), TxtUsuario.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPassword.Password.Trim(), ConfigurationManager.AppSettings["UsuarioSAN"].Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioSAN("%" + TxtNombreSanU.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUsuario.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar USUARIO.");
                }
            }
        }

        private void BtnDeleteUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el USUARIO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteUsuarioSAN(int.Parse(TxtIDUsuario.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioSAN("%" + TxtNombreSanU.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUsuario.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar USUARIO.");
                }
            }
        }

        private void BtnNewPool_Click(object sender, RoutedEventArgs e)
        {
            GridNewPool.Visibility = Visibility.Visible;
            GridEditPool.Visibility = Visibility.Collapsed;
            TxtIDPool.Text = "";
            TxtNombrePool.Text = "";
            TxtDescripcionPool.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 5));
        }

        private void BtnEditPool_Click(object sender, RoutedEventArgs e)
        {
            if (DgPool.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgPool.SelectedItems[0];
                    TxtIDPool.Text = row[0].ToString().Trim();
                    TxtNombrePool.Text = row[2].ToString().Trim();
                    TxtDescripcionPool.Text = row[3].ToString().Trim();
                    GridNewPool.Visibility = Visibility.Collapsed;
                    GridEditPool.Visibility = Visibility.Visible;
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 5));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarPool_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombrePool.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un POOL vacío.");
            }
            else if (TxtDescripcionPool.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un pool sin DESCRIPCIÓN.");
            }
            else
            {
                if (client.InsertPool(int.Parse(TxtIDSanP.Text.Trim()), TxtNombrePool.Text.Trim(), TxtDescripcionPool.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectPoolBySan("%" + TxtNombreSanP.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgPool.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("POOL insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar POOL.");
                }
            }
        }

        private void BtnUpdatePool_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombrePool.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un POOL vacío.");
            }
            else if (TxtDescripcionPool.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un pool sin DESCRIPCIÓN.");
            }
            else
            {
                if (client.UpdatePool(int.Parse(TxtIDPool.Text.Trim()), int.Parse(TxtIDSanP.Text.Trim()), TxtNombrePool.Text.Trim(), TxtDescripcionPool.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectPoolBySan("%" + TxtNombreSanP.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgPool.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("POOL actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar POOL.");
                }
            }
        }

        private void BtnDeletePool_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el POOL?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeletePool(int.Parse(TxtIDPool.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectPoolBySan("%" + TxtNombreSanP.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgPool.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("POOL borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSAN.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar POOL.");
                }
            }
        }
    }
}
