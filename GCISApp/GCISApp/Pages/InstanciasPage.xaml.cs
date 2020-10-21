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
    /// Lógica de interacción para InstanciasPage.xaml
    /// </summary>
    public partial class InstanciasPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idambiente = new List<string>();
        List<string> ambiente = new List<string>();
        List<string> idproveedor = new List<string>();
        List<string> proveedor = new List<string>();
        List<string> idaplicacion = new List<string>();
        List<string> aplicacion = new List<string>();
        List<string> idmaintenance = new List<string>();
        List<string> maintenance = new List<string>();

        public InstanciasPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectInstanciaAplicacion":
                        BtnSelect.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "UpdateInstanciaAplicacion":
                        BtnEditar.IsEnabled = true;
                        BtnGuardarE.IsEnabled = true;
                        break;
                    case "DeleteInstanciaAplicacion":
                        BtnBorrarE.IsEnabled = true;
                        break;
                    case "SelectUsuarioInstancia":
                        BtnEditUsuario.IsEnabled = true;
                        break;
                    case "InsertUsuarioInstancia":
                        BtnNewUsuario.IsEnabled = true;
                        BtnGuardarUsuarios.IsEnabled = true;
                        break;
                    case "UpdateUsuarioInstancia":
                        BtnEditUsuario.IsEnabled = true;
                        BtnUpdateUsuarios.IsEnabled = true;
                        break;
                    case "DeleteUsuarioInstancia":
                        BtnBorrarUsuarios.IsEnabled = true;
                        break;
                    case "SelectPlanMantenimiento":
                        BtnEditMantenimiento.IsEnabled = true;
                        break;
                    case "InsertPlanMantenimiento":
                        BtnNewMantenimiento.IsEnabled = true;
                        BtnGuardarMaintenance.IsEnabled = true;
                        break;
                    case "UpdatePlanMantenimiento":
                        BtnEditMantenimiento.IsEnabled = true;
                        BtnUpdateMaintenance.IsEnabled = true;
                        break;
                    case "DeletePlanMantenimiento":
                        BtnDeleteMaintenance.IsEnabled = true;
                        break;
                    case "SelectDatabaseIDInstancia":
                        BtnEditDB.IsEnabled = true;
                        break;
                    case "InsertDatabase":
                        BtnNewDB.IsEnabled = true;
                        BtnGuardarDatabase.IsEnabled = true;
                        break;
                    case "UpdateDatabase":
                        BtnEditDB.IsEnabled = true;
                        BtnUpdateDatabase.IsEnabled = true;
                        break;
                    case "DeleteDatabase":
                        BtnDeleteDatabase.IsEnabled = true;
                        break;
                    case "VerPassword":
                        TxtPasswordE.Visibility = Visibility.Collapsed;
                        VerPasswordE.Visibility = Visibility.Visible;
                        TxtPasswordU.Visibility = Visibility.Collapsed;
                        VerPasswordU.Visibility = Visibility.Visible;
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

        private void LimpiarInicio()
        {
            TxtID.Text = "";
            TxtNombreServidor.Text = "";
            WSGCIS.Data Datos = client.SelectInstanciaAplicacion("%", App.Current.Properties["Username"].ToString());
            DgInstancia.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabInstancias_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarInicio();
            //Ambiente
            WSGCIS.Data Datos = Datos = client.SelectAmbiente("%", App.Current.Properties["Username"].ToString());
            idambiente.Clear();
            ambiente.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idambiente.Add(Datos.Tabla.Rows[i][0].ToString());
                ambiente.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbAmbienteE.ItemsSource = ambiente;
            //Proveedor
            Datos = client.SelectProveedor("%", App.Current.Properties["Username"].ToString());
            idproveedor.Clear();
            proveedor.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idproveedor.Add(Datos.Tabla.Rows[i][0].ToString());
                proveedor.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbProveedorE.ItemsSource = proveedor;
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectInstanciaAplicacion("%" + TxtNombreServidor.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgInstancia.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgInstancia.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    TabEdicion.SelectedIndex = 0;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgInstancia.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString().Trim();
                    TxtIDInstanciaM.Text = row[0].ToString().Trim();
                    TxtIDInstanciaU.Text = row[0].ToString().Trim();
                    TxtIDInstanciaD.Text = row[0].ToString().Trim();
                    TxtIDServidorE.Text = row[1].ToString().Trim();
                    TxtNombreServidorE.Text = row[2].ToString().Trim();
                    CmbAmbienteE.Text = row[3].ToString().Trim();
                    CmbProveedorE.Text = row[4].ToString().Trim();
                    CmbAplicacionE.Text = row[5].ToString().Trim();
                    TxtNombreInstanciaE.Text = row[6].ToString().Trim();
                    TxtNombreInstanciaU.Text = row[6].ToString().Trim();
                    TxtNombreInstanciaM.Text = row[6].ToString().Trim();
                    TxtNombreInstanciaD.Text = row[6].ToString().Trim();
                    TxtUsuarioE.Text = row[7].ToString().Trim();
                    TxtPasswordE.Password = Core.Utils.StringCipher.Decrypt(row[8].ToString().Trim(), ConfigurationManager.AppSettings["InstanciaAplicacion"].Trim());
                    VerPasswordE.Text = Core.Utils.StringCipher.Decrypt(row[8].ToString().Trim(), ConfigurationManager.AppSettings["InstanciaAplicacion"].Trim());
                    WSGCIS.Data Datos = client.SelectUsuarioInstancia(row[6].ToString().Trim(), App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectPlanMantenimiento(row[6].ToString().Trim(), App.Current.Properties["Username"].ToString());
                    DgMaintenance.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectDatabaseIDInstancia(int.Parse(row[0].ToString().Trim()), App.Current.Properties["Username"].ToString());
                    DgDatabase.ItemsSource = Datos.Tabla.DefaultView;
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }                
            }
        }

        private void CmbProveedorE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Aplicaciones
            WSGCIS.Data Datos = client.SelectAplicacion(int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorE.SelectedItem.ToString().Trim())]), "%", App.Current.Properties["Username"].ToString());
            idaplicacion.Clear();
            aplicacion.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idaplicacion.Add(Datos.Tabla.Rows[i][0].ToString());
                aplicacion.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbAplicacionE.ItemsSource = aplicacion;
            CmbAplicacionE.Items.Refresh();
        }

        private void BtnNewUsuario_Click(object sender, RoutedEventArgs e)
        {
            GridNewUsuario.Visibility = Visibility.Visible;
            GridEditUsuario.Visibility = Visibility.Collapsed;
            TxtIDUsuarioU.Text = "";
            TxtUsuarioU.Text = "";
            TxtPasswordU.Password = "";
            VerPasswordU.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 2));
        }

        private void BtnEditUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsuarios.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    GridNewUsuario.Visibility = Visibility.Collapsed;
                    GridEditUsuario.Visibility = Visibility.Visible;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgUsuarios.SelectedItems[0];
                    TxtIDUsuarioU.Text = row[0].ToString().Trim();
                    TxtUsuarioU.Text = row[2].ToString().Trim();
                    TxtPasswordU.Password = Core.Utils.StringCipher.Decrypt(row[3].ToString().Trim(), ConfigurationManager.AppSettings["UsuarioInstancia"].Trim());
                    VerPasswordU.Text = Core.Utils.StringCipher.Decrypt(row[3].ToString().Trim(), ConfigurationManager.AppSettings["UsuarioInstancia"].Trim());
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 2));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnNewMantenimiento_Click(object sender, RoutedEventArgs e)
        {
            GridNewMaintenancePlan.Visibility = Visibility.Visible;
            GridEditMaintenancePlan.Visibility = Visibility.Collapsed;
            TxtNombreMaintenanceM.Text = "";
            TxtDescripcionMaintenanceM.Text = "";
            chkLunes.IsChecked = false;
            chkMartes.IsChecked = false;
            chkMiercoles.IsChecked = false;
            chkJueves.IsChecked = false;
            chkViernes.IsChecked = false;
            chkSabado.IsChecked = false;
            chkDomingo.IsChecked = false;
            cmbHora.SelectedIndex = 0;
            cmbMinuto.SelectedIndex = 0;
            Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 3));
        }

        private void BtnEditMantenimiento_Click(object sender, RoutedEventArgs e)
        {
            if (DgMaintenance.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    GridNewMaintenancePlan.Visibility = Visibility.Collapsed;
                    GridEditMaintenancePlan.Visibility = Visibility.Visible;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgMaintenance.SelectedItems[0];
                    TxtIDMaintenanceM.Text = row[0].ToString().Trim();
                    TxtNombreMaintenanceM.Text = row[2].ToString().Trim();
                    TxtDescripcionMaintenanceM.Text = row[3].ToString().Trim();
                    if (row[4].ToString().Trim().Contains("L")) { chkLunes.IsChecked = true; } else { chkLunes.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("M")) { chkMartes.IsChecked = true; } else { chkMartes.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("X")) { chkMiercoles.IsChecked = true; } else { chkMiercoles.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("J")) { chkJueves.IsChecked = true; } else { chkJueves.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("V")) { chkViernes.IsChecked = true; } else { chkViernes.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("S")) { chkSabado.IsChecked = true; } else { chkSabado.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("D")) { chkDomingo.IsChecked = true; } else { chkDomingo.IsChecked = false; }
                    cmbHora.Text = row[5].ToString().Trim().Substring(0, 2);
                    cmbMinuto.Text = row[5].ToString().Trim().Substring(3, 2);
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 3));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnNewDB_Click(object sender, RoutedEventArgs e)
        {
            GridNewDatabase.Visibility = Visibility.Visible;
            GridEditDatabase.Visibility = Visibility.Collapsed;
            TxtNombreDatabaseD.Text = "";
            TxtDescripcionDatabaseD.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 4));
        }

        private void BtnEditDB_Click(object sender, RoutedEventArgs e)
        {
            if (DgDatabase.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    GridNewDatabase.Visibility = Visibility.Collapsed;
                    GridEditDatabase.Visibility = Visibility.Visible;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgDatabase.SelectedItems[0];
                    TxtIDDatabaseD.Text = row[0].ToString();
                    TxtNombreDatabaseD.Text = row[2].ToString();
                    TxtDescripcionDatabaseD.Text = row[3].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 4));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            LimpiarInicio();
            Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 0));
        }

        private void BtnGuardarE_Click(object sender, RoutedEventArgs e)
        {
            if (VerPasswordE.Visibility == Visibility.Visible)
            {
                TxtPasswordE.Password = VerPasswordE.Text;
            }
            if (TxtNombreInstanciaE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin NOMBRE.");
            }
            if (CmbAmbienteE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin AMBIENTE.");
            }
            if (CmbAplicacionE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin APLICACIÓN.");
            }
            else
            {
                if (client.UpdateInstanciaAplicacion(int.Parse(TxtIDE.Text.Trim()), int.Parse(TxtIDServidorE.Text.Trim()), int.Parse(idambiente[ambiente.IndexOf(CmbAmbienteE.Text.Trim())]), int.Parse(idaplicacion[aplicacion.IndexOf(CmbAplicacionE.Text.Trim())]), TxtNombreInstanciaE.Text.Trim(), TxtUsuarioE.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPasswordE.Password.Trim(), ConfigurationManager.AppSettings["InstanciaAplicacion"].Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectInstanciaAplicacion("%", App.Current.Properties["Username"].ToString());
                    DgInstancia.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("INSTANCIA DE APLICACIÓN actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al actualizar INSTANCIA DE APLICACIÓN.");
                }
            }
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la INSTANCIA DE APLICACIÓN?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteInstanciaAplicacion(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectInstanciaAplicacion("%", App.Current.Properties["Username"].ToString());
                    DgInstancia.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("INSTANCIA DE APLICACIÓN borrada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al borrar INSTANCIA DE APLICACIÓN.");
                }
            }
        }

        private void BtnBackUsuarios_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
        }

        private void BtnGuardarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            if (VerPasswordU.Visibility == Visibility.Visible)
            {
                TxtPasswordU.Password = VerPasswordU.Text;
            }
            if (TxtUsuarioU.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un USUARIO vacío.");
            }
            else if (TxtPasswordU.Password.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PASSWORD vacío.");
            }
            else
            {
                if (client.InsertUsuarioInstancia(int.Parse(TxtIDInstanciaU.Text.Trim()), TxtUsuarioU.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPasswordU.Password.Trim(), ConfigurationManager.AppSettings["UsuarioInstancia"].Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioInstancia(TxtNombreInstanciaU.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al insertar USUARIO.");
                }
            }
        }

        private void BtnUpdateUsuarios_Click(object sender, RoutedEventArgs e)
        {
            if (VerPasswordU.Visibility == Visibility.Visible)
            {
                TxtPasswordU.Password = VerPasswordU.Text;
            }
            if (TxtUsuarioU.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un USUARIO vacío.");
            }
            else if (TxtPasswordU.Password.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PASSWORD vacío.");
            }
            else
            {
                if (client.UpdateUsuarioInstancia(int.Parse(TxtIDUsuarioU.Text.Trim()), int.Parse(TxtIDInstanciaU.Text.Trim()), TxtUsuarioU.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPasswordU.Password.Trim(), ConfigurationManager.AppSettings["UsuarioInstancia"].Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioInstancia(TxtNombreInstanciaU.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al actualizar USUARIO.");
                }
            }
        }

        private void BtnBorrarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el USUARIO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteUsuarioInstancia(int.Parse(TxtIDUsuarioU.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioInstancia(TxtNombreInstanciaU.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al borrar INSTANCIA DE APLICACIÓN.");
                }
            }
        }

        private void BtnGuardarMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreMaintenanceM.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un JOB sin nombre.");
            }
            else if (TxtDescripcionMaintenanceM.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DESCRIPCIÓN vacía.");
            }
            else if (chkLunes.IsChecked == false && chkMartes.IsChecked == false && chkMiercoles.IsChecked == false && chkJueves.IsChecked == false && chkViernes.IsChecked == false && chkSabado.IsChecked == false && chkDomingo.IsChecked == false)
            {
                MessageBox.Show("Es necesario seleccionar los DÍAS de ejecución del JOB.");
            }
            else
            {
                string Dias = "";
                string Hora = "";
                if (chkLunes.IsChecked == true) { Dias = Dias + "L"; } else { Dias = Dias + "-"; }
                if (chkMartes.IsChecked == true) { Dias = Dias + "M"; } else { Dias = Dias + "-"; }
                if (chkMiercoles.IsChecked == true) { Dias = Dias + "X"; } else { Dias = Dias + "-"; }
                if (chkJueves.IsChecked == true) { Dias = Dias + "J"; } else { Dias = Dias + "-"; }
                if (chkViernes.IsChecked == true) { Dias = Dias + "V"; } else { Dias = Dias + "-"; }
                if (chkSabado.IsChecked == true) { Dias = Dias + "S"; } else { Dias = Dias + "-"; }
                if (chkDomingo.IsChecked == true) { Dias = Dias + "D"; } else { Dias = Dias + "-"; }
                Hora = cmbHora.Text.Trim() + ":" + cmbMinuto.Text.Trim();
                //int.Parse(TxtIDServidorBackupExecJobs.Text.Trim()), TxtBackupExecJob.Text.Trim(), TxtDescripcionBackupExecJob.Text.Trim(), Dias.Trim(), Hora)
                if (client.InsertPlanMantenimiento(int.Parse(TxtIDInstanciaM.Text.Trim()), TxtNombreMaintenanceM.Text.Trim(), TxtDescripcionMaintenanceM.Text.Trim(), Dias, Hora, App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectPlanMantenimiento("%" + TxtNombreInstanciaM.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgMaintenance.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("PLAN DE MANTENIMIENTO insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al insertar PLAN DE MANTENIMIENTO.");
                }
            }
        }

        private void BtnUpdateMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreMaintenanceM.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un JOB sin nombre.");
            }
            else if (TxtDescripcionMaintenanceM.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DESCRIPCIÓN vacía.");
            }
            else if (chkLunes.IsChecked == false && chkMartes.IsChecked == false && chkMiercoles.IsChecked == false && chkJueves.IsChecked == false && chkViernes.IsChecked == false && chkSabado.IsChecked == false && chkDomingo.IsChecked == false)
            {
                MessageBox.Show("Es necesario seleccionar los DÍAS de ejecución del JOB.");
            }
            else
            {
                string Dias = "";
                string Hora = "";
                if (chkLunes.IsChecked == true) { Dias = Dias + "L"; } else { Dias = Dias + "-"; }
                if (chkMartes.IsChecked == true) { Dias = Dias + "M"; } else { Dias = Dias + "-"; }
                if (chkMiercoles.IsChecked == true) { Dias = Dias + "X"; } else { Dias = Dias + "-"; }
                if (chkJueves.IsChecked == true) { Dias = Dias + "J"; } else { Dias = Dias + "-"; }
                if (chkViernes.IsChecked == true) { Dias = Dias + "V"; } else { Dias = Dias + "-"; }
                if (chkSabado.IsChecked == true) { Dias = Dias + "S"; } else { Dias = Dias + "-"; }
                if (chkDomingo.IsChecked == true) { Dias = Dias + "D"; } else { Dias = Dias + "-"; }
                Hora = cmbHora.Text.Trim() + ":" + cmbMinuto.Text.Trim();
                if (client.UpdatePlanMantenimiento(int.Parse(TxtIDMaintenanceM.Text.Trim()), int.Parse(TxtIDInstanciaM.Text.Trim()), TxtNombreMaintenanceM.Text.Trim(), TxtDescripcionMaintenanceM.Text.Trim(), Dias, Hora, App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectPlanMantenimiento("%" + TxtNombreInstanciaM.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgMaintenance.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("PLAN DE MANTENIMIENTO actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al actualizar PLAN DE MANTENIMIENTO.");
                }
            }
        }

        private void BtnDeleteMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el PLAN DE MANTENIMIENTO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeletePlanMantenimiento(int.Parse(TxtIDMaintenanceM.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectPlanMantenimiento("%" + TxtNombreInstanciaM.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgMaintenance.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("PLAN DE MANTENIMIENTO borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al borrar PLAN DE MANTENIMIENTO.");
                }
            }
        }

        private void BtnGuardarDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreDatabaseD.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una base de datos sin NOMBRE.");
            }
            else if (TxtDescripcionDatabaseD.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una base de datos sin DESCRIPCIÓN.");
            }
            else
            {
                if (client.InsertDatabase(int.Parse(TxtIDInstanciaD.Text.Trim()), TxtNombreDatabaseD.Text.Trim(), TxtDescripcionDatabaseD.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDatabaseIDInstancia(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgDatabase.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("BASE DE DATOS insertada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al insertar BASE DE DATOS.");
                }
            }
        }

        private void BtnUpdateDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreDatabaseD.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una base de datos sin NOMBRE.");
            }
            else if (TxtDescripcionDatabaseD.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una base de datos sin DESCRIPCIÓN.");
            }
            else
            {
                if (client.UpdateDatabase(int.Parse(TxtIDDatabaseD.Text.Trim()), int.Parse(TxtIDInstanciaD.Text.Trim()), TxtNombreDatabaseD.Text.Trim(), TxtDescripcionDatabaseD.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDatabaseIDInstancia(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgDatabase.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("BASE DE DATOS actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al actualizar BASE DE DATOS.");
                }
            }
        }

        private void BtnDeleteDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la BASE DE DATOS?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteDatabase(int.Parse(TxtIDDatabaseD.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDatabaseIDInstancia(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgDatabase.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("BASE DE DATOS borrada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabInstancias.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al borrar BASE DE DATOS.");
                }
            }
        }
    }
}
