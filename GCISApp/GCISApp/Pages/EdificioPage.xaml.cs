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
    /// Lógica de interacción para EdificioPage.xaml
    /// </summary>
    public partial class EdificioPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public EdificioPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectEdificio":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertEdificio":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateEdificio":
                        BtnEditar.IsEnabled = true;
                        BtnGuardarE.IsEnabled = true;
                        break;
                    case "DeleteEdificio":
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
            TxtEdificio.Text = "";
            TxtEdificioN.Text = "";
            TxtEdificioE.Text = "";
            TxtDireccionN.Text = "";
            TxtDireccionE.Text = "";
            WSGCIS.Data Datos = client.SelectEdificio("%", App.Current.Properties["Username"].ToString());
            DgEdificio.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabEdificio_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectEdificio("%" + TxtEdificio.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgEdificio.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabEdificio.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgEdificio.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgEdificio.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    TxtEdificioE.Text = row[1].ToString();
                    TxtDireccionE.Text = row[2].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabEdificio.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabEdificio.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEdificioN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un EDIFICIO vacío.");
            }
            else if (TxtDireccionN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DIRECCIÓN vacía.");
            }
            else
            {
                if (client.InsertEdificio(TxtEdificioN.Text.Trim(), TxtDireccionN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("EDIFICIO insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar EDIFICIO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabEdificio.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabEdificio.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEdificioE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un EDIFICIO vacío.");
            }
            else if (TxtDireccionE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DIRECCIÓN vacía.");
            }
            else
            {
                if (client.UpdateEdificio(int.Parse(TxtIDE.Text.Trim()), TxtEdificioE.Text.Trim(), TxtDireccionE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("EDIFICIO actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar EDIFICIO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabEdificio.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el EDIFICIO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteEdificio(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("EDIFICIO borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar EDIFICIO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabEdificio.SelectedIndex = 0));
        }
    }
}
