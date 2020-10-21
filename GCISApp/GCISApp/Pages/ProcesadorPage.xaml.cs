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
    /// Lógica de interacción para ProcesadorPage.xaml
    /// </summary>
    public partial class ProcesadorPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idmarcas = new List<string>();
        List<string> marcas = new List<string>();

        public ProcesadorPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectProcesador":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertProcesador":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateProcesador":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteProcesador":
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
            TxtProcesador.Text = "";
            TxtProcesadorN.Text = "";
            TxtProcesadorE.Text = "";
            WSGCIS.Data Datos = client.SelectProcesador("%", App.Current.Properties["Username"].ToString());
            DgProcesador.ItemsSource = Datos.Tabla.DefaultView;
            Datos = client.SelectMarca("%", App.Current.Properties["Username"].ToString());
            marcas.Clear();
            idmarcas.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idmarcas.Add(Datos.Tabla.Rows[i][0].ToString());
                marcas.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbMarca.ItemsSource = marcas;
            CmbMarcaN.ItemsSource = marcas;
            CmbMarcaE.ItemsSource = marcas;
        }

        private void TabProcesador_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectProcesador("%" + TxtProcesador.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgProcesador.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabProcesador.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgProcesador.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgProcesador.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    CmbMarcaE.Text = row[1].ToString();
                    TxtProcesadorE.Text = row[2].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabProcesador.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabProcesador.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (CmbMarcaN.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione una MARCA válida.");
            }
            else if (TxtProcesadorN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PROCESADOR vacío.");
            }
            else
            {
                if (client.InsertProcesador(int.Parse(idmarcas[marcas.IndexOf(CmbMarcaN.Text.Trim())]), TxtProcesadorN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("PROCESADOR insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar PROCESADOR");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabProcesador.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabProcesador.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (CmbMarcaE.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione una MARCA válida.");
            }
            else if (TxtProcesadorE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PROCESADOR vacío.");
            }
            else
            {
                if (client.UpdateProcesador(int.Parse(TxtIDE.Text.Trim()), int.Parse(idmarcas[marcas.IndexOf(CmbMarcaE.Text.Trim())]), TxtProcesadorE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("PROCESADOR actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar PROCESADOR");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabProcesador.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el PROCESADOR?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteProcesador(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("PROCESADOR borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar PROCESADOR");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabProcesador.SelectedIndex = 0));
        }
    }
}
