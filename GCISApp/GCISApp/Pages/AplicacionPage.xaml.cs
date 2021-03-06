﻿using System;
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
    /// Lógica de interacción para AplicacionPage.xaml
    /// </summary>
    public partial class AplicacionPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idproveedor = new List<string>();
        List<string> proveedor = new List<string>();

        public AplicacionPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectAplicacion":
                        BtnSelect.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertAplicacion":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateAplicacion":
                        BtnEditar.IsEnabled = true;
                        BtnSaveE.IsEnabled = true;
                        break;
                    case "DeleteAplicacion":
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
            TxtAplicacion.Text = "";
            TxtAplicacionN.Text = "";
            TxtAplicacionE.Text = "";
            TxtVersionN.Text = "";
            TxtVersionE.Text = "";
            TxtFabricanteN.Text = "";
            TxtFabricanteE.Text = "";
            TxtLicenciaN.Text = "";
            TxtLicenciaE.Text = "";
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
            Datos = client.SelectAplicacion(0, "%", App.Current.Properties["Username"].ToString());
            DgAplicacion.ItemsSource = Datos.Tabla.DefaultView;
            DtVencimientoN.SelectedDate = DateTime.Now;
            DtVencimientoE.SelectedDate = DateTime.Now;
        }

        private void TabAplicacion_Loaded(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos;
            if (CmbProveedor.Text.Trim().Length > 0)
            {
                Datos = client.SelectAplicacion(int.Parse(idproveedor[proveedor.IndexOf(CmbProveedor.Text.Trim())]), "%" + TxtAplicacion.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            }
            else
            {
                Datos = client.SelectAplicacion(0, "%" + TxtAplicacion.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            }
            DgAplicacion.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabAplicacion.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (DgAplicacion.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgAplicacion.SelectedItems[0];
                    TxtIDE.Text = row[0].ToString();
                    CmbProveedorE.Text = row[5].ToString();
                    TxtAplicacionE.Text = row[1].ToString();
                    TxtVersionE.Text = row[2].ToString();
                    TxtFabricanteE.Text = row[3].ToString();
                    TxtLicenciaE.Text = row[4].ToString();
                    DtVencimientoE.Text = row[6].ToString();
                    Dispatcher.BeginInvoke((Action)(() => TabAplicacion.SelectedIndex = 2));
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabAplicacion.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProveedorN.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione un PROVEEDOR válido.");
            }
            else if (TxtAplicacionN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una APLICACIÓN vacía.");
            }
            else if (TxtVersionN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una VERSIÓN vacía.");
            }
            else if (TxtFabricanteN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un FABRICANTE vacío.");
            }
            else
            {
                if (client.InsertAplicacion(TxtAplicacionN.Text.Trim(), TxtVersionN.Text.Trim(), TxtFabricanteN.Text.Trim(), TxtLicenciaN.Text.Trim(), int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorN.Text.Trim())]), (DateTime)DtVencimientoN.SelectedDate, App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("APLICACIÓN insertada con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al insertar APLICACIÓN");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabAplicacion.SelectedIndex = 0));
        }

        private void BtnBackE_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabAplicacion.SelectedIndex = 0));
        }

        private void BtnSaveE_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProveedorE.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione un PROVEEDOR válido.");
            }
            else if (TxtAplicacionE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una APLICACIÓN vacía.");
            }
            else if (TxtVersionE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una VERSIÓN vacía.");
            }
            else if (TxtFabricanteE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un FABRICANTE vacío.");
            }
            else
            {
                if (client.UpdateAplicacion(int.Parse(TxtIDE.Text.Trim()), TxtAplicacionE.Text.Trim(), TxtVersionE.Text.Trim(), TxtFabricanteE.Text.Trim(), TxtLicenciaE.Text.Trim(), int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorE.Text.Trim())]), (DateTime)DtVencimientoE.SelectedDate, App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("APLICACIÓN actualizada con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar APLICACIÓN");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabAplicacion.SelectedIndex = 0));
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la APLICACIÓN?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteAplicacion(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    MessageBox.Show("APLICACIÓN borrada con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al borrar APLICACIÓN");
                }
            }
            Limpiar();
            Dispatcher.BeginInvoke((Action)(() => TabAplicacion.SelectedIndex = 0));
        }
    }
}
