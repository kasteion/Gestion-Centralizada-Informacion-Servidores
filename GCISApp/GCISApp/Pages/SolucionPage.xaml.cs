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
    /// Lógica de interacción para SolucionPage.xaml
    /// </summary>
    public partial class SolucionPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idservidor = new List<string>();
        List<string> servidor = new List<string>();

        public SolucionPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectSolucion":
                        BtnSelect.IsEnabled = true;
                        BtnEditarSolucion.IsEnabled = true;
                        break;
                    case "InsertSolucion":
                        BtnNuevaSolucion.IsEnabled = true;
                        BtnGuardarSolucion.IsEnabled = true;
                        break;
                    case "UpdateSolucion":
                        BtnEditarSolucion.IsEnabled = true;
                        BtnUpdateSolucion.IsEnabled = true;
                        break;
                    case "DeleteSolucion":
                        BtnDeleteSolucion.IsEnabled = true;
                        break;
                    case "SelectServidorSolucion":
                        BtnEditServidor.IsEnabled = true;
                        break;
                    case "InsertServidorSolucion":
                        BtnNewServidor.IsEnabled = true;
                        BtnGuardarServidor.IsEnabled = true;
                        break;
                    case "DeleteServidorSolucion":
                        BtnEditServidor.IsEnabled = true;
                        BtnDeleteServidor.IsEnabled = true;
                        break;
                }
            }
        }

        private void LimpiarGeneral()
        {
            TxtIDSolucion.Text = "";
            TxtNombreSolucion.Text = "";
            WSGCIS.Data Datos = client.SelectSolucion("%", App.Current.Properties["Username"].ToString());
            DgSolucion.ItemsSource = Datos.Tabla.DefaultView;

        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectSolucion("%" + TxtNombreSolucion.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgSolucion.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevaSolucion_Click(object sender, RoutedEventArgs e)
        {
            TxtIDSolucionN.Text = "";
            TxtNombreSolucionN.Text = "";
            TxtDescripcionSolucionN.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 1));
        }

        private void BtnBackSolucion_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
            Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 0));
        }

        private void BtnGuardarSolucion_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreSolucionN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una SOLUCIÓN sin NOMBRE.");
            }
            if (TxtDescripcionSolucionN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una SOLUCIÓN sin DESCRIPCIÓN.");
            }
            else
            {
                if (client.InsertSolucion(TxtNombreSolucionN.Text.Trim(), TxtDescripcionSolucionN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("SOLUCIÓN insertada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al insertar SOLUCIÓN.");
                }
            }
        }

        private void BtnEditarSolucion_Click(object sender, RoutedEventArgs e)
        {
            if (DgSolucion.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgSolucion.SelectedItems[0];
                    TxtIDSolucionE.Text = row[0].ToString().Trim();
                    TxtIDSolucionS.Text = row[0].ToString().Trim();
                    TxtNombreSolucionE.Text = row[1].ToString().Trim();
                    TxtNombreSolucionS.Text = row[1].ToString().Trim();
                    TxtDescripcionSolucionE.Text = row[2].ToString().Trim();
                    WSGCIS.Data Datos = client.SelectServidorSolucion("%" + row[1].ToString().Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgServidores.ItemsSource = Datos.Tabla.DefaultView;
                    Datos = client.SelectServidor("%", App.Current.Properties["Username"].ToString());
                    servidor.Clear();
                    idservidor.Clear();
                    for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
                    {
                        servidor.Add(Datos.Tabla.Rows[i][5].ToString());
                        idservidor.Add(Datos.Tabla.Rows[i][0].ToString());
                    }
                    ListServidores.ItemsSource = servidor;
                    ListServidores.Items.Refresh();
                    Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 2));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnUpdateSolucion_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreSolucionE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una SOLUCIÓN sin NOMBRE.");
            }
            if (TxtDescripcionSolucionE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una SOLUCIÓN sin DESCRIPCIÓN.");
            }
            else
            {
                if (client.UpdateSolucion(int.Parse(TxtIDSolucionE.Text.Trim()), TxtNombreSolucionE.Text.Trim(), TxtDescripcionSolucionE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("SOLUCIÓN actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al actualizar SOLUCIÓN.");
                }
            }
        }

        private void BtnDeleteSolucion_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la SOLUCIÓN?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteSolucion(int.Parse(TxtIDSolucionE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    LimpiarGeneral();
                    MessageBox.Show("SOLUCIÓN borrada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 0));
                }
                else
                {
                    MessageBox.Show("Error al borrar SOLUCIÓN.");
                }
            }
        }

        private void BtnNewServidor_Click(object sender, RoutedEventArgs e)
        {
            GridNewServidor.Visibility = Visibility.Visible;
            GridEditServidor.Visibility = Visibility.Collapsed;
            Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 3));
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
                    GridNewServidor.Visibility = Visibility.Collapsed;
                    GridEditServidor.Visibility = Visibility.Visible;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgServidores.SelectedItems[0];
                    ListServidores.SelectedItem = row[0].ToString().Trim();
                    Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 3));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBackServidor_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 2));
        }

        private void BtnGuardarServidor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client.InsertServidorSolucion(int.Parse(idservidor[servidor.IndexOf(ListServidores.SelectedItem.ToString().Trim())]), int.Parse(TxtIDSolucionS.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectServidorSolucion("%" + TxtNombreSolucionS.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                    DgServidores.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("SERVIDOR insertado con éxito a la solución.");
                    Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar SERVIDOR a la solución.");
                }
            }
            catch
            {
                MessageBox.Show("Seleccione un servidor de la lista.");
            }
        }

        private void BtnDeleteServidor_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la SERVIDOR de la solución?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int iddeleteservidor = int.Parse(idservidor[servidor.IndexOf(ListServidores.SelectedItem.ToString().Trim())]);
                    if (client.DeleteServidorSolucion(iddeleteservidor, int.Parse(TxtIDSolucionS.Text.Trim()), App.Current.Properties["Username"].ToString()))
                    {
                        WSGCIS.Data Datos = client.SelectServidorSolucion("%" + TxtNombreSolucionS.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
                        DgServidores.ItemsSource = Datos.Tabla.DefaultView;
                        MessageBox.Show("SERVIDOR borrado con éxito de la solución.");
                        Dispatcher.BeginInvoke((Action)(() => TabSolucion.SelectedIndex = 2));
                    }
                    else
                    {
                        MessageBox.Show("Error al borrar SERVIDOR de la solución.");
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
