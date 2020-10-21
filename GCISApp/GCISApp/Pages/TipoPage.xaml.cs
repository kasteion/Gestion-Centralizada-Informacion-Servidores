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
    /// Lógica de interacción para TipoPage.xaml
    /// </summary>
    public partial class TipoPage : Page
    {

        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public TipoPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectTipo":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertTipo":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateTipo":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteTipo":
                        BtnBorrarE.IsEnabled = true;
                        break;
                }
            }
        }

        private void Limpiar()
        {
            TxtID.Text = "";
            TxtTipo.Text = "";
            TxtIDE.Text = "";
            TxtTipoE.Text = "";
            TxtIDN.Text = "";
            TxtTipoN.Text = "";
            WSGCIS.Data Datos = client.SelectTipo("%", App.Current.Properties["Username"].ToString());
            DgTipo.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabTipo_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectTipo("%" + TxtTipo.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgTipo.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabTipo.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgTipo.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    TxtTipoE.Text = row[1].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabTipo.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabTipo.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtTipoN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un TIPO vacío.");
            }
            else
            {
                if (client.InsertTipo(TxtTipoN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("TIPO insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar TIPO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabTipo.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabTipo.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtTipoE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un TIPO vacío.");
            }
            else
            {
                if (client.UpdateTipo(int.Parse(TxtIDE.Text.Trim()), TxtTipoE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("TIPO actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar TIPO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabTipo.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el TIPO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteTipo(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("TIPO borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar TIPO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabTipo.SelectedIndex = 0));
        }
    }
}
