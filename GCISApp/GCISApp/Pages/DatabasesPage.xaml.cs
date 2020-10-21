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
    /// Lógica de interacción para DatabasesPage.xaml
    /// </summary>
    public partial class DatabasesPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idusuarios = new List<string>();
        List<string> usuarios = new List<string>();
        List<string> idmaintenance = new List<string>();
        List<string> maintenance = new List<string>();

        public DatabasesPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectDatabase":
                        BtnSelect.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "UpdateDatabase":
                        BtnEditar.IsEnabled = true;
                        BtnUpdateDatabaseE.IsEnabled = true;
                        break;
                    case "DeleteDatabase":
                        BtnDeleteDatabaseE.IsEnabled = true;
                        break;
                    case "InsertDatabaseUsuario":
                        BtnNewUsuarios.IsEnabled = true;
                        BtnGuardarUsuariosU.IsEnabled = true;
                        break;
                    case "DeleteDatabaseUsuario":
                        BtnEditUsuarios.IsEnabled = true;
                        BtnDeleteUsuariosU.IsEnabled = true;
                        break;
                    case "InsertDatabaseMantenimiento":
                        BtnNewMaintenance.IsEnabled = true;
                        BtnGuardarDatabaseM.IsEnabled = true;
                        break;
                    case "DeleteDatabaseMantenimiento":
                        BtnEditMaintenance.IsEnabled = true;
                        BtnDeleteDatabaseM.IsEnabled = true;
                        break;
                    case "SelectDatabaseUsuario":
                        BtnEditUsuarios.IsEnabled = true;
                        break;
                    case "SelectDatabaseMantenimiento":
                        BtnEditMaintenance.IsEnabled = true;
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

        private void LoadGeneral()
        {
            TxtDatabaseID.Text = "";
            TxtNombreDatabase.Text = "";
            WSGCIS.Data Datos = client.SelectDatabase("%", App.Current.Properties["Username"].ToString());
            DgDatabases.ItemsSource = Datos.Tabla.DefaultView;
        } 

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGeneral();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectDatabase("%" + TxtNombreDatabase.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgDatabases.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgDatabases.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    TabEdicion.SelectedIndex = 0;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgDatabases.SelectedItems[0];
                    TxtIDDatabaseE.Text = row[0].ToString().Trim();
                    TxtIDDatabaseU.Text = row[0].ToString().Trim();
                    TxtIDDatabaseM.Text = row[0].ToString().Trim();
                    TxtIDInstanciaE.Text = row[1].ToString().Trim();
                    TxtIDInstanciaU.Text = row[1].ToString().Trim();
                    TxtIDInstanciaM.Text = row[1].ToString().Trim();
                    TxtNombreInstanciaE.Text = row[2].ToString().Trim();
                    TxtNombreInstanciaU.Text = row[2].ToString().Trim();
                    TxtNombreInstanciaM.Text = row[2].ToString().Trim();
                    TxtNombreDatabaseE.Text = row[3].ToString().Trim();
                    TxtNombreDatabaseU.Text = row[3].ToString().Trim();
                    TxtNombreDatabaseM.Text = row[3].ToString().Trim();
                    TxtDescripcionDatabaseE.Text = row[4].ToString().Trim();
                    WSGCIS.Data Datos = client.SelectDatabaseUsuario("%" + TxtNombreDatabaseE.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectDatabaseMantenimiento(int.Parse(TxtIDDatabaseE.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgMaintenance.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectUsuarioInstancia("%" + row[2].ToString().Trim() + "%", App.Current.Properties["Username"].ToString());
                    usuarios.Clear();
                    idusuarios.Clear();
                    for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
                    {
                        idusuarios.Add(Datos.Tabla.Rows[i][0].ToString());
                        usuarios.Add(Datos.Tabla.Rows[i][2].ToString());
                    }
                    ListUsuariosU.ItemsSource = usuarios;
                    ListUsuariosU.Items.Refresh();
                    Datos = client.SelectPlanMantenimiento("%" + row[2].ToString().Trim() + "%", App.Current.Properties["Username"].ToString());
                    maintenance.Clear();
                    idmaintenance.Clear();
                    for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
                    {
                        idmaintenance.Add(Datos.Tabla.Rows[i][0].ToString());
                        maintenance.Add(Datos.Tabla.Rows[i][2].ToString());
                    }
                    ListMantenimientoM.ItemsSource = maintenance;
                    ListMantenimientoM.Items.Refresh();
                    GridEditDatabase.Visibility = Visibility.Visible;
                    //GridNewDatabase.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 1));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBackDatabaseE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 0));
        }

        private void BtnUpdateDatabaseE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreDatabaseE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una BASE DE DATOS sin nombre.");
            }
            else if (TxtDescripcionDatabaseE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una BASE DE DATOS sin descripción.");
            }
            else
            {
                if (client.UpdateDatabase(int.Parse(TxtIDDatabaseE.Text.Trim()), int.Parse(TxtIDInstanciaE.Text.Trim()), TxtNombreDatabaseE.Text.Trim(), TxtDescripcionDatabaseE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                { 
                    WSGCIS.Data Datos = client.SelectDatabase("%", App.Current.Properties["Username"].ToString());
                    DgDatabases.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("BASE DE DATOS actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al actualizar DIRECCIÓN IP.");
                }
            }
        }

        private void BtnDeleteDatabaseE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la BASE DE DATOS?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteDatabase(int.Parse(TxtIDDatabaseE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDatabase("%", App.Current.Properties["Username"].ToString());
                    DgDatabases.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("BASE DE DATOS borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al borrar BASE DE DATOS.");
                }
            }
        }

        private void BtnNewUsuarios_Click(object sender, RoutedEventArgs e)
        {
            GridNewUsuarioU.Visibility = Visibility.Visible;
            GridEditUsuarioU.Visibility = Visibility.Collapsed;
            Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 2));
        }

        private void BtnEditUsuarios_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsuarios.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    GridNewUsuarioU.Visibility = Visibility.Collapsed;
                    GridEditUsuarioU.Visibility = Visibility.Visible;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgUsuarios.SelectedItems[0];
                    ListUsuariosU.SelectedItem = row[3].ToString().Trim();
                    Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 2));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnNewMaintenance_Click(object sender, RoutedEventArgs e)
        {
            GridNewDatabaseM.Visibility = Visibility.Visible;
            GridEditDatabaseM.Visibility = Visibility.Collapsed;
            Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 3));
        }

        private void BtnEditMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (DgMaintenance.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    GridNewDatabaseM.Visibility = Visibility.Collapsed;
                    GridEditDatabaseM.Visibility = Visibility.Visible;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgMaintenance.SelectedItems[0];
                    ListMantenimientoM.SelectedItem = row[3].ToString().Trim();
                    Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 3));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBackUsuariosU_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 1));
        }

        private void BtnGuardarUsuariosU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client.InsertDatabaseUsuario(int.Parse(TxtIDDatabaseU.Text.Trim()), int.Parse(idusuarios[usuarios.IndexOf(ListUsuariosU.SelectedItem.ToString().Trim())]), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDatabaseUsuario("%" + TxtNombreDatabaseU.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al insertar USUARIO.");
                }
            }
            catch
            {
                MessageBox.Show("Seleccione un servidor de la lista.");
            }
        }

        private void BtnDeleteUsuariosU_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la USUARIO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int iddeleteusuario = int.Parse(idusuarios[usuarios.IndexOf(ListUsuariosU.SelectedItem.ToString().Trim())]);
                    if (client.DeleteDatabaseUsuario(int.Parse(TxtIDDatabaseU.Text.Trim()), iddeleteusuario, App.Current.Properties["Username"].ToString()))
                    {
                        WSGCIS.Data Datos = client.SelectDatabaseUsuario("%" + TxtNombreDatabaseU.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                        DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                        MessageBox.Show("USUARIO borrado con éxito.");
                        Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 1));
                    }
                    else
                    {
                        MessageBox.Show("Error al borrar USUARIO.");
                    }
                }
                catch
                {
                    MessageBox.Show("Seleccione un servidor de la lista.");
                }
            }
        }

        private void BtnBackDatabaseM_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 1));
        }

        private void BtnGuardarDatabaseM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client.InsertDatabaseMantenimiento(int.Parse(TxtIDDatabaseU.Text.Trim()), int.Parse(idmaintenance[maintenance.IndexOf(ListMantenimientoM.SelectedItem.ToString().Trim())]), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDatabaseMantenimiento(int.Parse(TxtIDDatabaseM.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgMaintenance.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("PLAN DE MANTENIMIENTO insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 1));
                }
                else
                {
                    MessageBox.Show("Error al insertar PLAN DE MANTENIMIENTO.");
                }
            }
            catch
            {
                MessageBox.Show("Seleccione un servidor de la lista.");
            }
        }

        private void BtnDeleteDatabaseM_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la PLAN DE MANTENIMIENTO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int iddeletemaintenance = int.Parse(idmaintenance[maintenance.IndexOf(ListMantenimientoM.SelectedItem.ToString().Trim())]);
                    if (client.DeleteDatabaseMantenimiento(int.Parse(TxtIDDatabaseM.Text.Trim()), iddeletemaintenance, App.Current.Properties["Username"].ToString()))
                    {
                        WSGCIS.Data Datos = client.SelectDatabaseMantenimiento(int.Parse(TxtIDDatabaseM.Text.Trim()), App.Current.Properties["Username"].ToString());
                        DgMaintenance.ItemsSource = Datos.Tabla.DefaultView;
                        MessageBox.Show("PLAN DE MANTENIMIENTO borrado con éxito.");
                        Dispatcher.BeginInvoke((Action)(() => TabDatabases.SelectedIndex = 1));
                    }
                    else
                    {
                        MessageBox.Show("Error al borrar PLAN DE MANTENIMIENTO.");
                    }
                }
                catch
                {
                    MessageBox.Show("Seleccione un servidor de la lista.");
                }
            }
        }
    }
}
