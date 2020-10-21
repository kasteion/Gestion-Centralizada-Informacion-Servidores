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
    /// Lógica de interacción para RackPage.xaml
    /// </summary>
    public partial class RackPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public RackPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectRack":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertRack":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateRack":
                        BtnEditar.IsEnabled = true;
                        BtnGuardarE.IsEnabled = true;
                        break;
                    case "DeleteRack":
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
            TxtRack.Text = "";
            TxtRackN.Text = "";
            TxtRackE.Text = "";
            TxtDescripcionN.Text = "";
            TxtDescripcionE.Text = "";
            WSGCIS.Data Datos = client.SelectRack("%", App.Current.Properties["Username"].ToString());
            DgRack.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabRack_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectRack("%" + TxtRack.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgRack.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabRack.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgRack.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgRack.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    TxtRackE.Text = row[1].ToString();
                    TxtDescripcionE.Text = row[2].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabRack.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabRack.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtRackN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un RACK vacío.");
            }
            else
            {
                if (client.InsertRack(TxtRackN.Text.Trim(), TxtDescripcionN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("RACK insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar RACK");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabRack.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabRack.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtRackE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un RACK vacío.");
            }
            else
            {
                if (client.UpdateRack(int.Parse(TxtIDE.Text.Trim()), TxtRackE.Text.Trim(), TxtDescripcionE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("RACK actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar RACK");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabRack.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el RACK?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteRack(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("RACK borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar RACK");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabRack.SelectedIndex = 0));
        }
    }
}
