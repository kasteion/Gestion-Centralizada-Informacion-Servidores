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
    /// Lógica de interacción para ProveedorPage.xaml
    /// </summary>
    public partial class ProveedorPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public ProveedorPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectProveedor":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertProveedor":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateProveedor":
                        BtnEditar.IsEnabled = true;
                        BtnGuardarE.IsEnabled = true;
                        break;
                    case "DeleteProveedor":
                        BtnBorrarE.IsEnabled = true;
                        break;
                }
            }
        }

        public void Limpiar()
        {
            TxtID.Text = "";
            TxtIDN.Text = "";
            TxtIDE.Text = "";
            TxtProveedor.Text = "";
            TxtProveedorN.Text = "";
            TxtProveedorE.Text = "";
            WSGCIS.Data Datos = client.SelectProveedor("%", App.Current.Properties["Username"].ToString());
            DgProveedor.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabProveedor_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectProveedor("%" + TxtProveedor.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgProveedor.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabProveedor.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgProveedor.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgProveedor.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    TxtProveedorE.Text = row[1].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabProveedor.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabProveedor.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtProveedorN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PROVEEDOR vacío.");
            }
            else
            {
                if (client.InsertProveedor(TxtProveedorN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("PROVEEDOR insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar PROVEEDOR");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabProveedor.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabProveedor.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtProveedorE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PROVEEDOR vacío.");
            }
            else
            {
                if (client.UpdateProveedor(int.Parse(TxtIDE.Text.Trim()), TxtProveedorE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("PROVEEDOR actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar PROVEEDOR");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabProveedor.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el PROVEEDOR?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteProveedor(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("PROVEEDOR borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar PROVEEDOR");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabProveedor.SelectedIndex = 0));
        }
    }

}
