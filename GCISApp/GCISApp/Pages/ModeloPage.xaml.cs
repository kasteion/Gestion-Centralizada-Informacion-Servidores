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
    /// Lógica de interacción para ModeloPage.xaml
    /// </summary>
    public partial class ModeloPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idmarcas = new List<string>();
        List<string> marcas = new List<string>();

        public ModeloPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectModelo":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertModelo":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateModelo":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteModelo":
                        BtnBorrarE.IsEnabled = true;
                        break;
                }
        }
        }

        private void Limpiar()
        {
            TxtID.Text = "";
            TxtIDN.Text = "";
            TxtIDE.Text = "";
            TxtModelo.Text = "";
            TxtModeloN.Text = "";
            TxtModeloE.Text = "";
            WSGCIS.Data Datos = client.SelectModelo("%", App.Current.Properties["Username"].ToString());
            DgModelo.ItemsSource = Datos.Tabla.DefaultView;
            Datos = client.SelectMarca("%", App.Current.Properties["Username"].ToString());
            marcas.Clear();
            idmarcas.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idmarcas.Add(Datos.Tabla.Rows[i][0].ToString());
                marcas.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            //CmbMarca.ItemsSource = marcas;
            CmbMarcaN.ItemsSource = marcas;
            CmbMarcaE.ItemsSource = marcas;
        }

        private void TabModelo_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectModelo("%" + TxtModelo.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgModelo.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabModelo.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgModelo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgModelo.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    CmbMarcaE.Text = row[1].ToString();
                    TxtModeloE.Text = row[2].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabModelo.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabModelo.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (CmbMarcaN.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione una MARCA válida.");
            }
            else if (TxtModeloN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un MODELO vacío.");
            }
            else
            {
                if (client.InsertModelo(int.Parse(idmarcas[marcas.IndexOf(CmbMarcaN.Text.Trim())]), TxtModeloN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("MODELO insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar MODELO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabModelo.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabModelo.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (CmbMarcaE.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione una MARCA válida.");
            }
            else if (TxtModeloE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un MODELO vacío.");
            }
            else
            {
                if (client.UpdateModelo(int.Parse(TxtIDE.Text.Trim()), int.Parse(idmarcas[marcas.IndexOf(CmbMarcaE.Text.Trim())]), TxtModeloE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("MODELO actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar MODELO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabModelo.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el MODELO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteModelo(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("MODELO borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar MODELO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabModelo.SelectedIndex = 0));
        }
    }
}
