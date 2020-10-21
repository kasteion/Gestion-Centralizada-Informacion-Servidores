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
    /// Lógica de interacción para MarcaPage.xaml
    /// </summary>
    public partial class MarcaPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public MarcaPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectMarca":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertMarca":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateMarca":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteMarca":
                        BtnBorrarE.IsEnabled = true;
                        break;
                }
            }
        }

        private void Limpiar()
        {
            TxtID.Text = "";
            TxtMarca.Text = "";
            TxtIDN.Text = "";
            TxtMarcaN.Text = "";
            TxtIDE.Text = "";
            TxtMarcaE.Text = "";
            WSGCIS.Data Datos = client.SelectMarca("%", App.Current.Properties["Username"].ToString());
            DgMarca.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabMarca_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectMarca("%" + TxtMarca.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgMarca.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMarca.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgMarca.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    TxtMarcaE.Text = row[1].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabMarca.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMarca.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtMarcaN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una MARCA vacía.");
            }
            else
            {
                if (client.InsertMarca(TxtMarcaN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("MARCA insertada con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar MARCA");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabMarca.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMarca.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtMarcaE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una MARCA vacía.");
            }
            else
            {
                if (client.UpdateMarca(int.Parse(TxtIDE.Text.Trim()), TxtMarcaE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("MARCA actualizada con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar MARCA");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabMarca.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la MARCA?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteMarca(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("MARCA borrada con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar MARCA");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabMarca.SelectedIndex = 0));
        }
    }
}
