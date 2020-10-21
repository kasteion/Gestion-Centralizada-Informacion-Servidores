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
    /// Lógica de interacción para ContactoPage.xaml
    /// </summary>
    public partial class ContactoPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idproveedor = new List<string>();
        List<string> proveedor = new List<string>();

        public ContactoPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectContacto":
                        BtnBuscar.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertContacto":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateContacto":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteContacto":
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
            TxtNombreN.Text = "";
            TxtNombreE.Text = "";
            TxtTelefonoN.Text = "";
            TxtTelefonoE.Text = "";
            WSGCIS.Data Datos = client.SelectProveedor("%", App.Current.Properties["Username"].ToString());
            idproveedor.Clear();
            proveedor.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idproveedor.Add(Datos.Tabla.Rows[i][0].ToString());
                proveedor.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbProveedor.ItemsSource = proveedor;
            CmbProveedorN.ItemsSource = proveedor;
            CmbProveedorE.ItemsSource = proveedor;
            Datos = client.SelectContacto(0, App.Current.Properties["Username"].ToString());
            DgContacto.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void TabContacto_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos;
            if (CmbProveedor.Text.Trim().Length > 0)
            {
                Datos = client.SelectContacto(int.Parse(idproveedor[proveedor.IndexOf(CmbProveedor.Text.Trim())]), App.Current.Properties["Username"].ToString());
            }
            else
            {
                Datos = client.SelectContacto(0, App.Current.Properties["Username"].ToString());
            }
            DgContacto.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabContacto.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgContacto.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgContacto.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    CmbProveedorE.Text = row[1].ToString();
                    TxtNombreE.Text = row[2].ToString();
                    TxtTelefonoE.Text = row[3].ToString();
                    if (row[4].ToString().Equals("SI")) { ChkEmergenciaE.IsChecked = true; }
                    else { ChkEmergenciaE.IsChecked = false; }
                    Dispatcher.BeginInvoke((Action)(() => TabContacto.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabContacto.SelectedIndex = 0));
        }

        private string ChkEmegenciaStatusN()
        {
            if (ChkEmergenciaN.IsChecked == true)
            { return "SI"; }
            else
            { return "NO"; }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProveedorN.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione un PROVEEDOR válido.");
            }
            else if (TxtNombreN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CONTACTO vacío.");
            }
            else if (TxtTelefonoN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un TELEFONO vacío.");
            }
            else
            {
                if (client.InsertContacto(int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorN.Text.Trim())]), TxtNombreN.Text, TxtTelefonoN.Text, ChkEmegenciaStatusN(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("CONTACTO insertado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar CONTACTO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabContacto.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabContacto.SelectedIndex = 0));
        }

        private string ChkEmegenciaStatusE()
        {
            if (ChkEmergenciaE.IsChecked == true)
            { return "SI"; }
            else
            { return "NO"; }
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProveedorE.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione un PROVEEDOR válido.");
            }
            else if (TxtNombreE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CONTACTO vacío.");
            }
            else if (TxtTelefonoE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un TELEFONO vacío.");
            }
            else
            {
                if (client.UpdateContacto(int.Parse(TxtIDE.Text.Trim()), int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorE.Text.Trim())]), TxtNombreE.Text.Trim(), TxtTelefonoE.Text.Trim(), ChkEmegenciaStatusE(), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("CONTACTO actualizado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar CONTACTO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabContacto.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el CONTACTO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteContacto(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("CONTACTO borrado con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar CONTACTO");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabContacto.SelectedIndex = 0));
        }
    }
}
