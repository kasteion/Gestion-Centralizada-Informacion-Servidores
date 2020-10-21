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
    /// Lógica de interacción para StatusPage.xaml
    /// </summary>
    public partial class StatusPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();

        public StatusPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectStatus":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertStatus":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateStatus":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteStatus":
                        BtnBorrarE.IsEnabled = true;
                        break;
                }
            }
        }

        private void Limpiar()
        {
            TxtID.Text = "";
            TxtStatus.Text = "";
            TxtIDN.Text = "";
            TxtStatusN.Text = "";
            TxtIDE.Text = "";
            TxtStatusE.Text = "";
            WSGCIS.Data Datos = client.SelectStatus("%", App.Current.Properties["Username"].ToString());
            DgStatus.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabStatus_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectStatus("%" + TxtStatus.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgStatus.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabStatus.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgStatus.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    TxtStatusE.Text = row[1].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabStatus.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }                
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabStatus.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtStatusN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un STATUS vacío.");
            }
            else
            {
                if (client.InsertStatus(TxtStatusN.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("STATUS insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar STATUS");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabStatus.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabStatus.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (TxtStatusE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un STATUS vacío.");
            }
            else
            {
                if (client.UpdateStatus(int.Parse(TxtIDE.Text.Trim()), TxtStatusE.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("STATUS actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar STATUS");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabStatus.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el STATUS?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteStatus(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("STATUS borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar STATUS");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabStatus.SelectedIndex = 0));
        }
    }
}
