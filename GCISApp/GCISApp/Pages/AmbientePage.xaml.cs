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
    /// Lógica de interacción para AmbientePage.xaml
    /// </summary>
    public partial class AmbientePage : Page
    {

        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public AmbientePage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectAmbiente":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertAmbiente":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateAmbiente":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteAmbiente":
                        BtnBorrarE.IsEnabled = true;
                        break;
                }
                
            }
        }

        private void Limpiar()
        {
            TxtID.Text = "";
            TxtAmbiente.Text = "";
            TxtIDE.Text = "";
            TxtAmbienteE.Text = "";
            TxtIDN.Text = "";
            TxtAmbienteN.Text = "";
            WSGCIS.Data Datos = client.SelectAmbiente("%", App.Current.Properties["Username"].ToString());
            DgAmbiente.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabTipo_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectAmbiente("%" + TxtAmbiente.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgAmbiente.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabAmbiente.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgAmbiente.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgAmbiente.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    TxtAmbienteE.Text = row[1].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabAmbiente.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabAmbiente.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtAmbienteN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un AMBIENTE vacío.");
            }
            else
            {
                if (client.InsertAmbiente(TxtAmbienteN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("AMBIENTE insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar AMBIENTE");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabAmbiente.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabAmbiente.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtAmbienteE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un AMBIENTE vacío.");
            }
            else
            {
                if (client.UpdateAmbiente(int.Parse(TxtIDE.Text.Trim()), TxtAmbienteE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("AMBIENTE actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar AMBIENTE");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabAmbiente.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el AMBIENTE?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteAmbiente(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("AMBIENTE borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar AMBIENTE");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabAmbiente.SelectedIndex = 0));
        }
    }
}
