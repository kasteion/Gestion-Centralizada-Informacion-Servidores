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
    /// Lógica de interacción para ClustersPage.xaml
    /// </summary>
    public partial class ClustersPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idproveedor = new List<string>();
        List<string> proveedor = new List<string>();
        List<string> idaplicacion = new List<string>();
        List<string> aplicacion = new List<string>();
        List<string> idservidor = new List<string>();
        List<string> servidor = new List<string>();

        public ClustersPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectCluster":
                        BtnSelectCluster.IsEnabled = true;
                        BtnEditarCluster.IsEnabled = true;
                        break;
                    case "InsertCluster":
                        BtnNuevoCluster.IsEnabled = true;
                        BtnGuardarCluster.IsEnabled = true;
                        break;
                    case "UpdateCluster":
                        BtnEditarCluster.IsEnabled = true;
                        BtnUpdateCluster.IsEnabled = true;
                        break;
                    case "DeleteCluster":
                        BtnDeleteCluster.IsEnabled = true;
                        break;
                    case "InsertClusterServidorFisico":
                        BtnGuardarClusterS.IsEnabled = true;
                        BtnNewServidor.IsEnabled = true;
                        break;
                    case "DeleteClusterServidorFisico":
                        BtnDeleteClusterS.IsEnabled = true;
                        BtnEditServidor.IsEnabled = true;
                        break;
                    case "SelectClusterServidorFisico":
                        BtnEditServidor.IsEnabled = true;
                        break;
                }
            }
        }

        private void LimpiarGeneral()
        {
            TxtIDCluster.Text = "";
            TxtNombreCluster.Text = "";
            WSGCIS.Data Datos = client.SelectCluster("%", App.Current.Properties["Username"].ToString());
            DgCluster.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabClusters_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
        }

        private void BtnSelectCluster_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectCluster("%" + TxtNombreCluster.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgCluster.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevoCluster_Click(object sender, RoutedEventArgs e)
        {
            TxtNombreClusterN.Text = "";
            TxtDescripcionClusterN.Text = "";
            WSGCIS.Data Datos = client.SelectProveedor("%", App.Current.Properties["Username"].ToString());
            proveedor.Clear();
            idproveedor.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                proveedor.Add(Datos.Tabla.Rows[i][1].ToString());
                idproveedor.Add(Datos.Tabla.Rows[i][0].ToString());
            }
            CmbProveedorClusterN.ItemsSource = proveedor;
            CmbProveedorClusterN.Items.Refresh();
            Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 1));
        }

        private void BtnEditarCluster_Click(object sender, RoutedEventArgs e)
        {
            if (DgCluster.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgCluster.SelectedItems[0];
                    TxtIDClusterE.Text = row[0].ToString().Trim();
                    TxtIDClusterS.Text = row[0].ToString().Trim();
                    TxtNombreClusterE.Text = row[1].ToString().Trim();
                    TxtNombreClusterS.Text = row[1].ToString().Trim();
                    TxtDescripcionClusterE.Text = row[2].ToString().Trim();
                    WSGCIS.Data Datos = client.SelectProveedor("%", App.Current.Properties["Username"].ToString());
                    proveedor.Clear();
                    idproveedor.Clear();
                    for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
                    {
                        proveedor.Add(Datos.Tabla.Rows[i][1].ToString());
                        idproveedor.Add(Datos.Tabla.Rows[i][0].ToString());
                    }
                    CmbProveedorClusterE.ItemsSource = proveedor;
                    CmbProveedorClusterE.Items.Refresh();
                    CmbProveedorClusterE.Text = row[3].ToString().Trim();
                    CmbAplicacionClusterE.Text = row[4].ToString().Trim();
                    Datos = client.SelectClusterServidorFisico("%" + row[1].ToString().Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgServidores.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectServidorFisico("%", App.Current.Properties["Username"].ToString());
                    servidor.Clear();
                    idservidor.Clear();
                    for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
                    {
                        servidor.Add(Datos.Tabla.Rows[i][1].ToString());
                        idservidor.Add(Datos.Tabla.Rows[i][0].ToString());
                    }
                    ListServidores.ItemsSource = servidor;
                    ListServidores.Items.Refresh();
                    Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 2));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void CmbProveedorClusterN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectAplicacion(int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorClusterN.SelectedItem.ToString().Trim())]), "%", App.Current.Properties["Username"].ToString());
            aplicacion.Clear();
            idaplicacion.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                aplicacion.Add(Datos.Tabla.Rows[i][1].ToString());
                idaplicacion.Add(Datos.Tabla.Rows[i][0].ToString());
            }
            CmbAplicacionClusterN.ItemsSource = aplicacion;
            CmbAplicacionClusterN.Items.Refresh();
        }

        private void BtnBackCluster_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
            Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 0));
        }

        private void BtnGuardarCluster_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreClusterN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER sin NOMBRE.");
            }
            if (TxtDescripcionClusterN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER sin DESCRIPCIÓN.");
            }
            if (CmbAplicacionClusterN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER sin APLICACIÓN DE VIRTUALIZACIÓN.");
            }
            else
            {
                if (client.InsertCluster(TxtNombreClusterN.Text.Trim(), TxtDescripcionClusterN.Text.Trim(), int.Parse(idaplicacion[aplicacion.IndexOf(CmbAplicacionClusterN.SelectedItem.ToString().Trim())]), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("CLUSTER insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al insertar CLUSTER.");
                }
            }
        }

        private void CmbProveedorClusterE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectAplicacion(int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorClusterE.SelectedItem.ToString().Trim())]), "%", App.Current.Properties["Username"].ToString());
            aplicacion.Clear();
            idaplicacion.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                aplicacion.Add(Datos.Tabla.Rows[i][1].ToString());
                idaplicacion.Add(Datos.Tabla.Rows[i][0].ToString());
            }
            CmbAplicacionClusterE.ItemsSource = aplicacion;
            CmbAplicacionClusterE.Items.Refresh();
        }

        private void BtnNewServidor_Click(object sender, RoutedEventArgs e)
        {
            GridNewCluster.Visibility = Visibility.Visible;
            GridEditCluster.Visibility = Visibility.Collapsed;
            Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 3));
        }

        private void BtnEditServidor_Click(object sender, RoutedEventArgs e)
        {
            if (DgServidores.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    GridNewCluster.Visibility = Visibility.Collapsed;
                    GridEditCluster.Visibility = Visibility.Visible;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgServidores.SelectedItems[0];
                    ListServidores.SelectedItem = row[0].ToString().Trim();
                    Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 3));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBackClusterS_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 2));
        }

        private void BtnGuardarClusterS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client.InsertClusterServidorFisico(int.Parse(idservidor[servidor.IndexOf(ListServidores.SelectedItem.ToString().Trim())]), int.Parse(TxtIDClusterS.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectClusterServidorFisico("%" + TxtNombreClusterS.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgServidores.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("SERVIDOR insertado con éxito al CLUSTER.");
                    Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar SERVIDOR al CLUSTER.");
                }
            }
            catch
            {
                MessageBox.Show("Seleccione un servidor de la lista.");
            }
        }

        private void BtnDeleteClusterS_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la SERVIDOR del CLUSTER?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int iddeleteservidor = int.Parse(idservidor[servidor.IndexOf(ListServidores.SelectedItem.ToString().Trim())]);
                    if (client.DeleteClusterServidorFisico(iddeleteservidor, int.Parse(TxtIDClusterS.Text.Trim()), App.Current.Properties["Username"].ToString()))
                    {
                        WSGCIS.Data Datos = client.SelectClusterServidorFisico("%" + TxtNombreClusterS.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                        DgServidores.ItemsSource = Datos.Tabla.DefaultView;
                        MessageBox.Show("SERVIDOR borrado con éxito del CLUSTER.");
                        Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 2));
                    }
                    else
                    {
                        MessageBox.Show("Error al borrar SERVIDOR del CLUSTER.");
                    }
                }
                catch
                {
                    MessageBox.Show("Seleccione un servidor de la lista.");
                }
            }
        }

        private void BtnUpdateCluster_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreClusterE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER sin NOMBRE.");
            }
            if (TxtDescripcionClusterE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER sin DESCRIPCIÓN.");
            }
            if (CmbAplicacionClusterE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER sin APLICACIÓN DE VIRTUALIZACIÓN.");
            }
            else
            {
                if (client.UpdateCluster(int.Parse(TxtIDClusterE.Text.Trim()), TxtNombreClusterE.Text.Trim(), TxtDescripcionClusterE.Text.Trim(), int.Parse(idaplicacion[aplicacion.IndexOf(CmbAplicacionClusterE.SelectedItem.ToString().Trim())]), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("CLUSTER actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al actualizar CLUSTER.");
                }
            }
        }

        private void BtnDeleteCluster_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el CLUSTER?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteCluster(int.Parse(TxtIDClusterE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("CLUSTER borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabClusters.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al borrar CLUSTER.");
                }
            }
        }
    }
}
