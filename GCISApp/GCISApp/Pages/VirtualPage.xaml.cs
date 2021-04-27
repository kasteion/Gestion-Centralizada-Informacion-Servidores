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
using System.Configuration;

namespace GCISApp
{
    /// <summary>
    /// Lógica de interacción para VirtualPage.xaml
    /// </summary>
    public partial class VirtualPage : Page
    {
        WSGCIS.GCISClient client = new WSGCIS.GCISClient();
        List<string> idstatus = new List<string>();
        List<string> status = new List<string>();
        List<string> idsistemaoperativo = new List<string>();
        List<string> sistemaoperativo = new List<string>();
        List<string> idambiente = new List<string>();
        List<string> ambiente = new List<string>();
        List<string> idtipo = new List<string>();
        List<string> tipo = new List<string>();
        List<string> idredundancia = new List<string>();
        List<string> redundancia = new List<string>();
        List<string> idsan = new List<string>();
        List<string> san = new List<string>();
        List<string> idpool = new List<string>();
        List<string> pool = new List<string>();
        List<string> idproveedor = new List<string>();
        List<string> proveedor = new List<string>();
        List<string> idaplicacion = new List<string>();
        List<string> aplicacion = new List<string>();
        List<string> idcluster = new List<string>();
        List<string> cluster = new List<string>();

        public VirtualPage()
        {
            InitializeComponent();
            WSGCIS.Data Permisos = client.SelectPermisosUsuarios(App.Current.Properties["Username"].ToString());
            for (int i = 0; i < Permisos.Tabla.Rows.Count; i++)
            {
                switch (Permisos.Tabla.Rows[i]["Funcion"].ToString().Trim())
                {
                    case "SelectServidorVirtual":
                        BtnSelect.IsEnabled = true;
                        BtnEditar.IsEnabled = true;
                        break;
                    case "InsertServidor":
                        BtnNuevo.IsEnabled = true;
                        BtnGuardar.IsEnabled = true;
                        break;
                    case "UpdateServidor":
                        BtnEditar.IsEnabled = true;
                        BtnGuardarE.IsEnabled = true;
                        break;
                    case "DeleteServidor":
                        BtnBorrarE.IsEnabled = true;
                        break;
                    case "SelectEthernet":
                        BtnEditEthernet.IsEnabled = true;
                        break;
                    case "InsertEthernet":
                        BtnNewEthernet.IsEnabled = true;
                        BtnGuardarEthernet.IsEnabled = true;
                        break;
                    case "UpdateEthernet":
                        BtnEditEthernet.IsEnabled = true;
                        BtnUpdateEthernet.IsEnabled = true;
                        break;
                    case "DeleteEthernet":
                        BtnDeleteEthernet.IsEnabled = true;
                        break;
                    case "SelectRedundancia":
                        BtnEditRedundancia.IsEnabled = true;
                        break;
                    case "InsertRedundancia":
                        BtnNewRedundancia.IsEnabled = true;
                        BtnGuardarRedundancia.IsEnabled = true;
                        break;
                    case "DeleteRedundancia":
                        BtnEditRedundancia.IsEnabled = true;
                        BtnDeleteRedundancia.IsEnabled = true;
                        break;
                    case "SelectDisco":
                        BtnEditDiscos.IsEnabled = true;
                        break;
                    case "InsertDisco":
                        BtnNewDiscos.IsEnabled = true;
                        BtnGuardarDisco.IsEnabled = true;
                        break;
                    case "UpdateDisco":
                        BtnEditDiscos.IsEnabled = true;
                        BtnUpdateDisco.IsEnabled = true;
                        break;
                    case "DeleteDisco":
                        BtnDeleteDisco.IsEnabled = true;
                        break;
                    case "SelectBackupExecJobsIDServer":
                        BtnEditBackupExec.IsEnabled = true;
                        break;
                    case "InsertBackupExecJobs":
                        BtnNewBackupExec.IsEnabled = true;
                        BtnGuardarBackupExecJob.IsEnabled = true;
                        break;
                    case "UpdateBackupExecJobs":
                        BtnEditBackupExec.IsEnabled = true;
                        BtnUpdateBackupExecJob.IsEnabled = true;
                        break;
                    case "DeleteBackupExecJobs":
                        BtnDeleteBackupExecJob.IsEnabled = true;
                        break;
                    case "SelectUsuarioIDServer":
                        BtnEditUsuarios.IsEnabled = true;
                        break;
                    case "InsertUsuario":
                        BtnNewUsuarios.IsEnabled = true;
                        BtnGuardarUsuario.IsEnabled = true;
                        break;
                    case "UpdateUsuario":
                        BtnEditUsuarios.IsEnabled = true;
                        BtnUpdateUsuario.IsEnabled = true;
                        break;
                    case "DeleteUsuario":
                        BtnDeleteUsuario.IsEnabled = true;
                        break;
                    case "SelectWebServiceIDServer":
                        BtnEditWebServices.IsEnabled = true;
                        break;
                    case "InsertWebService":
                        BtnNewWebServices.IsEnabled = true;
                        BtnGuardarWebService.IsEnabled = true;
                        break;
                    case "UpdateWebService":
                        BtnEditWebServices.IsEnabled = true;
                        BtnUpdateWebService.IsEnabled = true;
                        break;
                    case "DeleteWebService":
                        BtnDeleteWebService.IsEnabled = true;
                        break;
                    case "SelectInstanciaAplicacionIDServer":
                        BtnEditInstancias.IsEnabled = true;
                        break;
                    case "InsertInstanciaAplicacion":
                        BtnNewInstancias.IsEnabled = true;
                        BtnGuardarInstancia.IsEnabled = true;
                        break;
                    case "UpdateInstanciaAplicacion":
                        BtnEditInstancias.IsEnabled = true;
                        BtnUpdateInstancia.IsEnabled = true;
                        break;
                    case "DeleteInstanciaAplicacion":
                        BtnDeleteInstancia.IsEnabled = true;
                        break;
                    case "SelectDocumentoServidor":
                        BtnEditDocumento.IsEnabled = true;
                        break;
                    case "InsertDocumentoServidor":
                        BtnNewDocumento.IsEnabled = true;
                        BtnGuardarDocumento.IsEnabled = true;
                        break;
                    case "UpdateDocumentoServidor":
                        BtnEditDocumento.IsEnabled = true;
                        BtnUpdateDocumento.IsEnabled = true;
                        break;
                    case "DeleteDocumentoServidor":
                        BtnDeleteDocumento.IsEnabled = true;
                        break;
                    case "VerPassword":
                        VerPassword.Visibility = Visibility.Visible;
                        TxtPassword.Visibility = Visibility.Collapsed;
                        VerPasswordInstancia.Visibility = Visibility.Visible;
                        TxtPasswordInstancia.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void LimpiarGeneral()
        {
            TxtID.Text = "";
            TxtServidor.Text = "";
            WSGCIS.Data Datos = client.SelectServidorVirtual("%", App.Current.Properties["Username"].ToString());
            DgVirtual.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void LoadCombos()
        {
            //Status
            WSGCIS.Data Datos = client.SelectStatus("%", App.Current.Properties["Username"].ToString());
            status.Clear();
            idstatus.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idstatus.Add(Datos.Tabla.Rows[i][0].ToString());
                status.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbStatusN.ItemsSource = status;
            CmbStatusE.ItemsSource = status;
            //Sistema Operativo
            Datos = client.SelectSistemaOperativo(0, "%", App.Current.Properties["Username"].ToString());
            sistemaoperativo.Clear();
            idsistemaoperativo.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idsistemaoperativo.Add(Datos.Tabla.Rows[i][0].ToString());
                sistemaoperativo.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbSistemaOperativoN.ItemsSource = sistemaoperativo;
            CmbSistemaOperativoE.ItemsSource = sistemaoperativo;
            //Ambiente
            Datos = client.SelectAmbiente("%", App.Current.Properties["Username"].ToString());
            ambiente.Clear();
            idambiente.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idambiente.Add(Datos.Tabla.Rows[i][0].ToString());
                ambiente.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbAmbienteN.ItemsSource = ambiente;
            CmbAmbienteE.ItemsSource = ambiente;
            CmbAmbienteInstancia.ItemsSource = ambiente;
            //Tipo
            Datos = client.SelectTipo("%", App.Current.Properties["Username"].ToString());
            tipo.Clear();
            idtipo.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idtipo.Add(Datos.Tabla.Rows[i][0].ToString());
                tipo.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbTipoN.ItemsSource = tipo;
            CmbTipoE.ItemsSource = tipo;
            //SAN
            Datos = client.SelectSAN("%", App.Current.Properties["Username"].ToString());
            san.Clear();
            idsan.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idsan.Add(Datos.Tabla.Rows[i][0].ToString());
                san.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbSAN.ItemsSource = san;
            //PROVEEDOR
            Datos = client.SelectProveedor("%", App.Current.Properties["Username"].ToString());
            proveedor.Clear();
            idproveedor.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idproveedor.Add(Datos.Tabla.Rows[i][0].ToString());
                proveedor.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbProveedorInstancia.ItemsSource = proveedor;
            //CLUSTERS
            Datos = client.SelectCluster("%", App.Current.Properties["Username"].ToString());
            cluster.Clear();
            idcluster.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idcluster.Add(Datos.Tabla.Rows[i][0].ToString());
                cluster.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbClusterN.ItemsSource = cluster;
            CmbClusterE.ItemsSource = cluster;
        }

        private void LimpiarNuevo()
        {
            TxtIDN.Text = "";
            TxtNombreN.Text = "";
            TxtDescripcionN.Text = "";
            TxtNoProcesadoresN.Text = "";
            TxtRamN.Text = "";
            TxtNoEthernetN.Text = "";
            ChkTeamEthernetN.IsChecked = false;
            DtFechaParchesN.SelectedDate = DateTime.Now;
        }

        private void LimpiarEdicion()
        {
            if (DgVirtual.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    TabEdicion.SelectedIndex = 0;
                    System.Data.DataRowView row = (System.Data.DataRowView)DgVirtual.SelectedItems[0];
                    int IDServidor = int.Parse(row[0].ToString().Trim());
                    string Servidor = row[1].ToString().Trim();
                    WSGCIS.Data DatosServidor = client.SelectServidor(Servidor, App.Current.Properties["Username"].ToString());
                    if (DatosServidor.Tabla.Rows.Count > 0)
                    {
                        TxtIDE.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorEthernet.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorRedundancia.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorDisco.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorBackupExecJobs.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorUsuario.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorWebService.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorInstancia.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();
                        TxtIDServidorDocumento.Text = DatosServidor.Tabla.Rows[0][0].ToString().Trim();

                        CmbStatusE.Text = DatosServidor.Tabla.Rows[0][1].ToString().Trim();
                        CmbSistemaOperativoE.Text = DatosServidor.Tabla.Rows[0][2].ToString().Trim();
                        CmbAmbienteE.Text = DatosServidor.Tabla.Rows[0][3].ToString().Trim();
                        CmbTipoE.Text = DatosServidor.Tabla.Rows[0][4].ToString().Trim();

                        TxtNombreE.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();
                        TxtNombreServidorEthernet.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();
                        TxtNombreServidorRedundancia.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();
                        TxtNombreServidorDisco.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();
                        TxtNombreServidorBackupExecJobs.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();
                        TxtNombreServidorUsuario.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();
                        TxtNombreServidorWebService.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();
                        TxtNombreServidorInstancia.Text = DatosServidor.Tabla.Rows[0][5].ToString().Trim();

                        TxtDescripcionE.Text = DatosServidor.Tabla.Rows[0][6].ToString().Trim();
                        TxtNoProcesadoresE.Text = DatosServidor.Tabla.Rows[0][7].ToString().Trim();
                        TxtRamE.Text = DatosServidor.Tabla.Rows[0][8].ToString().Trim();
                        TxtNoEthernetE.Text = DatosServidor.Tabla.Rows[0][9].ToString().Trim();
                        if (DatosServidor.Tabla.Rows[0][10].ToString().Trim().Equals("SI"))
                        {
                            ChkTeamEthernetE.IsChecked = true;
                        }
                        DtFechaParchesE.Text = DatosServidor.Tabla.Rows[0][11].ToString().Trim();
                        WSGCIS.Data Datos = client.SelectEthernet(Servidor, App.Current.Properties["Username"].ToString());
                        DgEthernet.ItemsSource = Datos.Tabla.DefaultView;
                        Datos = client.SelectRedundancia(IDServidor, App.Current.Properties["Username"].ToString());
                        DgRedundancia.ItemsSource = Datos.Tabla.DefaultView;
                        Datos = client.SelectServidor4Redundancia(TxtNombreServidorRedundancia.Text.Trim(), App.Current.Properties["Username"].ToString());
                        redundancia.Clear();
                        idredundancia.Clear();
                        for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
                        {
                            idredundancia.Add(Datos.Tabla.Rows[i][0].ToString());
                            redundancia.Add(Datos.Tabla.Rows[i][1].ToString());
                        }
                        ListRedundancia.ItemsSource = redundancia;
                        ListRedundancia.Items.Refresh();
                        Datos = client.SelectDisco(Servidor, App.Current.Properties["Username"].ToString());
                        DgDiscos.ItemsSource = Datos.Tabla.DefaultView;
                        Datos = client.SelectBackupExecJobsIDServer(IDServidor, App.Current.Properties["Username"].ToString());
                        DgBackupExec.ItemsSource = Datos.Tabla.DefaultView;
                        Datos = client.SelectUsuarioIDServer(IDServidor, App.Current.Properties["Username"].ToString());
                        DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                        Datos = client.SelectWebServiceIDServer(IDServidor, App.Current.Properties["Username"].ToString());
                        DgWebServices.ItemsSource = Datos.Tabla.DefaultView;
                        Datos = client.SelectInstanciaAplicacionIDServer(IDServidor, App.Current.Properties["Username"].ToString());
                        DgInstancias.ItemsSource = Datos.Tabla.DefaultView;
                        Datos = client.SelectDocumentoServidor(IDServidor, App.Current.Properties["Username"].ToString());
                        DgDocumentos.ItemsSource = Datos.Tabla.DefaultView;

                        CmbClusterE.Text = row[2].ToString().Trim();

                        Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                    }
                    else
                    {
                        MessageBox.Show("Error al seleccionar datos de SERVIDOR VIRTUAL.");
                    }
                }
                catch { MessageBox.Show("Seleccione una fila válida."); }
            }
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarGeneral();
            LoadCombos();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectServidorVirtual("%" + TxtServidor.Text.Trim() + "%", App.Current.Properties["Username"].ToString());
            DgVirtual.ItemsSource = Datos.Tabla.DefaultView;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarNuevo();
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 1));
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarEdicion();
        }

        private void TxtNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 0));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (CmbStatusN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un STATUS vacío.");
            }
            else if (CmbSistemaOperativoN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un SISTEMA OPERATIVO vacío.");
            }
            else if (CmbAmbienteN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un AMBIENTE vacío.");
            }
            else if (CmbTipoN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un TIPO vacío.");
            }
            else if (TxtNombreN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un NOMBRE vacío.");
            }
            else if (TxtDescripcionN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DESCRIPCIÓN vacía.");
            }
            else if (TxtNoProcesadoresN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CANTIDAD DE PROCESADORES vacía.");
            }
            else if (TxtRamN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CANTIDAD DE RAM vacía.");
            }
            else if (TxtNoEthernetN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CANTIDAD DE TARJETAS DE RED vacía.");
            }
            else if (CmbClusterN.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER vacío.");
            }
            else
            {
                string TeamEthernetStatus = "";
                if (ChkTeamEthernetN.IsChecked == true)
                {
                    TeamEthernetStatus = "SI";
                }
                else
                {
                    TeamEthernetStatus = "NO";
                }

                if (client.InsertServidor(int.Parse(idstatus[status.IndexOf(CmbStatusN.Text.Trim())]), int.Parse(idsistemaoperativo[sistemaoperativo.IndexOf(CmbSistemaOperativoN.Text.Trim())]), int.Parse(idambiente[ambiente.IndexOf(CmbAmbienteN.Text.Trim())]), int.Parse(idtipo[tipo.IndexOf(CmbTipoN.Text.Trim())]), TxtNombreN.Text.Trim(), TxtDescripcionN.Text.Trim(), int.Parse(TxtNoProcesadoresN.Text.Trim()), int.Parse(TxtRamN.Text.Trim()), int.Parse(TxtNoEthernetN.Text.Trim()), TeamEthernetStatus, (DateTime)DtFechaParchesN.SelectedDate, App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectServidor(TxtNombreN.Text.Trim(), App.Current.Properties["Username"].ToString());
                    int IDServidor = 0;
                    if (Datos.Tabla.Rows.Count > 0)
                    {
                        IDServidor = int.Parse(Datos.Tabla.Rows[0][0].ToString());
                        if (client.InsertServidorVirtual(IDServidor, int.Parse(idcluster[cluster.IndexOf(CmbClusterN.Text.Trim())]), App.Current.Properties["Username"].ToString()))
                        {
                            LimpiarGeneral();
                            MessageBox.Show("SERVIDOR VIRTUAL insertado con éxito.");
                            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 0));
                        }
                        else
                        {
                            client.DeleteServidor(IDServidor, App.Current.Properties["Username"].ToString());
                            MessageBox.Show("Error al insertar SERVIDOR VIRTUAL.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error al insertar SERVIDOR VIRTUAL.");
                }
            }
        }

        private void BtnGuardarE_Click(object sender, RoutedEventArgs e)
        {
            if (CmbStatusE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un STATUS vacío.");
            }
            else if (CmbSistemaOperativoE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un SISTEMA OPERATIVO vacío.");
            }
            else if (CmbAmbienteE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un AMBIENTE vacío.");
            }
            else if (CmbTipoE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un TIPO vacío.");
            }
            else if (TxtNombreE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un NOMBRE vacío.");
            }
            else if (TxtDescripcionE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DESCRIPCIÓN vacía.");
            }
            else if (TxtNoProcesadoresE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CANTIDAD DE PROCESADORES vacía.");
            }
            else if (TxtRamE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CANTIDAD DE RAM vacía.");
            }
            else if (TxtNoEthernetE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CANTIDAD DE TARJETAS DE RED vacía.");
            }
            else if (CmbClusterE.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un CLUSTER vacío.");
            }
            else
            {
                string TeamEthernetStatus = "";
                if (ChkTeamEthernetN.IsChecked == true)
                {
                    TeamEthernetStatus = "SI";
                }
                else
                {
                    TeamEthernetStatus = "NO";
                }

                if (client.UpdateServidor(int.Parse(TxtIDE.Text.Trim()), int.Parse(idstatus[status.IndexOf(CmbStatusE.Text.Trim())]), int.Parse(idsistemaoperativo[sistemaoperativo.IndexOf(CmbSistemaOperativoE.Text.Trim())]), int.Parse(idambiente[ambiente.IndexOf(CmbAmbienteE.Text.Trim())]), int.Parse(idtipo[tipo.IndexOf(CmbTipoE.Text.Trim())]), TxtNombreE.Text.Trim(), TxtDescripcionE.Text.Trim(), int.Parse(TxtNoProcesadoresE.Text.Trim()), int.Parse(TxtRamE.Text.Trim()), int.Parse(TxtNoEthernetE.Text.Trim()), TeamEthernetStatus, (DateTime)DtFechaParchesE.SelectedDate, App.Current.Properties["Username"].ToString()))
                {
                    if (client.UpdateServidorVirtual(int.Parse(TxtIDE.Text.Trim()), int.Parse(idcluster[cluster.IndexOf(CmbClusterE.Text.Trim())]), App.Current.Properties["Username"].ToString()))
                    {
                        LimpiarGeneral();
                        MessageBox.Show("SERVIDOR VIRTUAL actualizado con éxito.");
                        Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 0));
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar SERVIDOR VIRTUAL.");
                    }
                }
                else
                {
                    MessageBox.Show("Error al actualizar SERVIDOR VIRTUAL.");
                }
            }
        }

        private void BtnBorrarE_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el SERVIDOR VIRTUAL?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteServidorVirtual(int.Parse(TxtIDE.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data data = client.SelectServidorVirtual(TxtNombreE.Text.Trim(), App.Current.Properties["Username"].ToString());
                    if (data.Tabla.Rows.Count > 0)
                    {
                        MessageBox.Show("Error al borrar SERVIDOR VIRTUAL.");
                    }
                    else
                    {
                        LimpiarGeneral();
                        MessageBox.Show("SERVIDOR VIRTUAL borrado con éxito.");
                        Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 0));
                    }
                }
                else
                {
                    MessageBox.Show("Error al borrar SERVIDOR VIRTUAL.");
                }
            }
        }

        private void BtnNewEthernet_Click(object sender, RoutedEventArgs e)
        {
            GridEditEthernet.Visibility = Visibility.Collapsed;
            GridNewEthernet.Visibility = Visibility.Visible;
            TxtNewDireccionIP.Visibility = Visibility.Collapsed;
            TbNewDireccionIP.Visibility = Visibility.Collapsed;
            TxtDireccionIP.IsEnabled = true;
            TxtDireccionIP.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 3));
        }

        private void BtnEditEthernet_Click(object sender, RoutedEventArgs e)
        {
            if (DgEthernet.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgEthernet.SelectedItems[0];
                    TxtDireccionIP.Text = row[0].ToString().Trim();
                    GridEditEthernet.Visibility = Visibility.Visible;
                    GridNewEthernet.Visibility = Visibility.Collapsed;
                    TxtNewDireccionIP.Visibility = Visibility.Visible;
                    TbNewDireccionIP.Visibility = Visibility.Visible;
                    TxtDireccionIP.IsEnabled = false;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 3));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnBackEthernet_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
        }

        private void BtnGuardarEthernet_Click(object sender, RoutedEventArgs e)
        {
            if (TxtDireccionIP.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una IP vacía.");
            }
            else
            {
                if (client.InsertEthernet(TxtDireccionIP.Text.Trim(), int.Parse(TxtIDServidorEthernet.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectEthernet(TxtNombreServidorEthernet.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgEthernet.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("DIRECCIÓN IP insertada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar DIRECCIÓN IP.");
                }
            }
        }

        private void BtnUpdateEthernet_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNewDireccionIP.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una IP vacía.");
            }
            else
            {
                if (client.UpdateEthernet(TxtDireccionIP.Text.Trim(), int.Parse(TxtIDServidorEthernet.Text.Trim()), TxtNewDireccionIP.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectEthernet(TxtNombreServidorEthernet.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgEthernet.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("DIRECCIÓN IP actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar DIRECCIÓN IP.");
                }
            }
        }

        private void BtnDeleteEthernet_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el DIRECCIÓN IP?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteEthernet(TxtDireccionIP.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectEthernet(TxtNombreServidorEthernet.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgEthernet.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("DIRECCIÓN IP borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar DIRECCIÓN IP.");
                }
            }
        }

        private void BtnNewRedundancia_Click(object sender, RoutedEventArgs e)
        {
            GridEditRedundancia.Visibility = Visibility.Collapsed;
            GridNewRedundancia.Visibility = Visibility.Visible;
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 4));
        }

        private void BtnEditRedundancia_Click(object sender, RoutedEventArgs e)
        {
            if (DgRedundancia.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgRedundancia.SelectedItems[0];
                    ListRedundancia.SelectedItem = row[2].ToString().Trim();
                    GridEditRedundancia.Visibility = Visibility.Visible;
                    GridNewRedundancia.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 4));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarRedundancia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client.InsertRedundancia(int.Parse(TxtIDServidorRedundancia.Text.Trim()), int.Parse(idredundancia[redundancia.IndexOf(ListRedundancia.SelectedItem.ToString().Trim())]), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectRedundancia(int.Parse(TxtIDServidorRedundancia.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgRedundancia.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("REDUNDANCIA insertada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar REDUNDANCIA.");
                }
            }
            catch
            {
                MessageBox.Show("Seleccione un servidor de la lista.");
            }
        }

        private void BtnDeleteRedundancia_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la REDUNDANCIA?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int iddeleteredundancia = int.Parse(idredundancia[redundancia.IndexOf(ListRedundancia.SelectedItem.ToString().Trim())]);
                    if (client.DeleteRedundancia(int.Parse(TxtIDServidorRedundancia.Text.Trim()), iddeleteredundancia, App.Current.Properties["Username"].ToString()))
                    {
                        WSGCIS.Data Datos = client.SelectRedundancia(int.Parse(TxtIDServidorRedundancia.Text.Trim()), App.Current.Properties["Username"].ToString());
                        DgRedundancia.ItemsSource = Datos.Tabla.DefaultView;
                        MessageBox.Show("REDUNDANCIA borrada con éxito.");
                        Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                    }
                    else
                    {
                        MessageBox.Show("Error al borrar REDUNDANCIA.");
                    }
                }
                catch
                {
                    MessageBox.Show("Seleccione un servidor de la lista.");
                }
            }
        }

        private void BtnNewDiscos_Click(object sender, RoutedEventArgs e)
        {
            GridEditDisco.Visibility = Visibility.Collapsed;
            GridNewDisco.Visibility = Visibility.Visible;
            TxtNombreDisco.Text = "";
            TxtCapacidad.Text = "";
            TxtVolumenCompartido.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 5));
        }

        private void BtnEditDiscos_Click(object sender, RoutedEventArgs e)
        {
            if (DgDiscos.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgDiscos.SelectedItems[0];
                    TxtIDDisco.Text = row[0].ToString().Trim();
                    TxtIDServidorDisco.Text = row[1].ToString().Trim();
                    TxtNombreServidorDisco.Text = row[2].ToString().Trim();
                    TxtNombreDisco.Text = row[3].ToString().Trim();
                    TxtCapacidad.Text = row[4].ToString().Trim();
                    CmbSAN.Text = row[5].ToString().Trim();
                    CmbPool.Text = row[6].ToString().Trim();
                    TxtVolumenCompartido.Text = row[7].ToString().Trim();
                    GridEditDisco.Visibility = Visibility.Visible;
                    GridNewDisco.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 5));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarDisco_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreDisco.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede insertar un NOMBRE DISCO vacío.");
            }
            else if (TxtCapacidad.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede insertar una CAPACIDAD vacía.");
            }
            else if (CmbPool.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede insertar un POOL vacío.");
            }
            else
            {
                //CmbPool.Text.Trim()   
                if (client.InsertDisco(int.Parse(TxtIDServidorDisco.Text.Trim()), TxtNombreDisco.Text.Trim(), int.Parse(TxtCapacidad.Text.Trim()), int.Parse(idpool[pool.IndexOf(CmbPool.Text.Trim())]), TxtVolumenCompartido.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDisco(TxtNombreServidorDisco.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgDiscos.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("DISCO insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar DISCO.");
                }
            }
        }

        private void BtnUpdateDisco_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreDisco.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede insertar un NOMBRE DISCO vacío.");
            }
            else if (TxtCapacidad.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede insertar una CAPACIDAD vacía.");
            }
            else if (CmbPool.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede insertar un POOL vacío.");
            }
            else
            {
                if (client.UpdateDisco(int.Parse(TxtIDDisco.Text.Trim()), int.Parse(TxtIDServidorDisco.Text.Trim()), TxtNombreDisco.Text.Trim(), int.Parse(TxtCapacidad.Text.Trim()), int.Parse(idpool[pool.IndexOf(CmbPool.Text.Trim())]), TxtVolumenCompartido.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDisco(TxtNombreServidorDisco.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgDiscos.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("DISCO actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar DISCO.");
                }
            }
        }

        private void BtnDeleteDisco_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el DISCO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteDisco(int.Parse(TxtIDDisco.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDisco(TxtNombreServidorDisco.Text.Trim(), App.Current.Properties["Username"].ToString());
                    DgDiscos.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("DISCO borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar DISCO.");
                }
            }
        }

        private void BtnNewBackupExec_Click(object sender, RoutedEventArgs e)
        {
            GridEditBackupExecJob.Visibility = Visibility.Collapsed;
            GridNewBackupExecJob.Visibility = Visibility.Visible;
            TxtIDBackupExecJobs.Text = "";
            TxtBackupExecJob.Text = "";
            TxtDescripcionBackupExecJob.Text = "";
            chkLunes.IsChecked = false;
            chkMartes.IsChecked = false;
            chkMiercoles.IsChecked = false;
            chkJueves.IsChecked = false;
            chkViernes.IsChecked = false;
            chkSabado.IsChecked = false;
            chkDomingo.IsChecked = false;
            cmbHora.SelectedIndex = 0;
            cmbMinuto.SelectedIndex = 0;
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 6));
        }

        private void BtnEditBackupExec_Click(object sender, RoutedEventArgs e)
        {
            if (DgBackupExec.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgBackupExec.SelectedItems[0];
                    TxtIDBackupExecJobs.Text = row[0].ToString().Trim();
                    TxtBackupExecJob.Text = row[2].ToString().Trim();
                    TxtDescripcionBackupExecJob.Text = row[3].ToString().Trim();
                    if (row[4].ToString().Trim().Contains("L")) { chkLunes.IsChecked = true; } else { chkLunes.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("M")) { chkMartes.IsChecked = true; } else { chkMartes.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("X")) { chkMiercoles.IsChecked = true; } else { chkMiercoles.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("J")) { chkJueves.IsChecked = true; } else { chkJueves.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("V")) { chkViernes.IsChecked = true; } else { chkViernes.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("S")) { chkSabado.IsChecked = true; } else { chkSabado.IsChecked = false; }
                    if (row[4].ToString().Trim().Contains("D")) { chkDomingo.IsChecked = true; } else { chkDomingo.IsChecked = false; }
                    cmbHora.Text = row[5].ToString().Trim().Substring(0, 2);
                    cmbMinuto.Text = row[5].ToString().Trim().Substring(3, 2);
                    GridEditBackupExecJob.Visibility = Visibility.Visible;
                    GridNewBackupExecJob.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 6));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarBackupExecJob_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBackupExecJob.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un JOB sin nombre.");
            }
            else if (TxtDescripcionBackupExecJob.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DESCRIPCIÓN vacía.");
            }
            else if (chkLunes.IsChecked == false && chkMartes.IsChecked == false && chkMiercoles.IsChecked == false && chkJueves.IsChecked == false && chkViernes.IsChecked == false && chkSabado.IsChecked == false && chkDomingo.IsChecked == false)
            {
                MessageBox.Show("Es necesario seleccionar los DÍAS de ejecución del JOB.");
            }
            else
            {
                string Dias = "";
                string Hora = "";
                if (chkLunes.IsChecked == true) { Dias = Dias + "L"; } else { Dias = Dias + "-"; }
                if (chkMartes.IsChecked == true) { Dias = Dias + "M"; } else { Dias = Dias + "-"; }
                if (chkMiercoles.IsChecked == true) { Dias = Dias + "X"; } else { Dias = Dias + "-"; }
                if (chkJueves.IsChecked == true) { Dias = Dias + "J"; } else { Dias = Dias + "-"; }
                if (chkViernes.IsChecked == true) { Dias = Dias + "V"; } else { Dias = Dias + "-"; }
                if (chkSabado.IsChecked == true) { Dias = Dias + "S"; } else { Dias = Dias + "-"; }
                if (chkDomingo.IsChecked == true) { Dias = Dias + "D"; } else { Dias = Dias + "-"; }
                Hora = cmbHora.Text.Trim() + ":" + cmbMinuto.Text.Trim();
                if (client.InsertBackupExecJobs(int.Parse(TxtIDServidorBackupExecJobs.Text.Trim()), TxtBackupExecJob.Text.Trim(), TxtDescripcionBackupExecJob.Text.Trim(), Dias.Trim(), Hora, App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectBackupExecJobsIDServer(int.Parse(TxtIDServidorBackupExecJobs.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgBackupExec.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("BACKUP EXEC JOB insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar BACKUP EXEC JOB.");
                }
            }
        }

        private void BtnUpdateBackupExecJob_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBackupExecJob.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un JOB sin nombre.");
            }
            else if (TxtDescripcionBackupExecJob.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una DESCRIPCIÓN vacía.");
            }
            else if (chkLunes.IsChecked == false && chkMartes.IsChecked == false && chkMiercoles.IsChecked == false && chkJueves.IsChecked == false && chkViernes.IsChecked == false && chkSabado.IsChecked == false && chkDomingo.IsChecked == false)
            {
                MessageBox.Show("Es necesario seleccionar los DÍAS de ejecución del JOB.");
            }
            else
            {
                string Dias = "";
                string Hora = "";
                if (chkLunes.IsChecked == true) { Dias = Dias + "L"; } else { Dias = Dias + "-"; }
                if (chkMartes.IsChecked == true) { Dias = Dias + "M"; } else { Dias = Dias + "-"; }
                if (chkMiercoles.IsChecked == true) { Dias = Dias + "X"; } else { Dias = Dias + "-"; }
                if (chkJueves.IsChecked == true) { Dias = Dias + "J"; } else { Dias = Dias + "-"; }
                if (chkViernes.IsChecked == true) { Dias = Dias + "V"; } else { Dias = Dias + "-"; }
                if (chkSabado.IsChecked == true) { Dias = Dias + "S"; } else { Dias = Dias + "-"; }
                if (chkDomingo.IsChecked == true) { Dias = Dias + "D"; } else { Dias = Dias + "-"; }
                Hora = cmbHora.Text.Trim() + ":" + cmbMinuto.Text.Trim();
                if (client.UpdateBackupExecJobs(int.Parse(TxtIDBackupExecJobs.Text.Trim()), int.Parse(TxtIDServidorBackupExecJobs.Text.Trim()), TxtBackupExecJob.Text.Trim(), TxtDescripcionBackupExecJob.Text.Trim(), Dias.Trim(), Hora, App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectBackupExecJobsIDServer(int.Parse(TxtIDServidorBackupExecJobs.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgBackupExec.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("BACKUP EXEC JOB actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar BACKUP EXEC JOB.");
                }
            }
        }

        private void BtnDeleteBackupExecJob_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el JOB?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteBackupExecJobs(int.Parse(TxtIDBackupExecJobs.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectBackupExecJobsIDServer(int.Parse(TxtIDServidorBackupExecJobs.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgBackupExec.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("JOB borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar JOB.");
                }
            }
        }

        private void BtnNewUsuarios_Click(object sender, RoutedEventArgs e)
        {
            GridEditUsuario.Visibility = Visibility.Collapsed;
            GridNewUsuario.Visibility = Visibility.Visible;
            TxtIDUsuario.Text = "";
            TxtDominio.Text = "";
            TxtUsuario.Text = "";
            TxtPassword.Password = "";
            VerPassword.Text = "";
            TxtDescripcion.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 7));
        }

        private void BtnEditUsuarios_Click(object sender, RoutedEventArgs e)
        {
            if (DgUsuarios.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgUsuarios.SelectedItems[0];
                    TxtIDUsuario.Text = row[0].ToString().Trim();
                    TxtDominio.Text = row[2].ToString().Trim();
                    TxtUsuario.Text = row[3].ToString().Trim();
                    TxtPassword.Password = Core.Utils.StringCipher.Decrypt(row[4].ToString().Trim(), ConfigurationManager.AppSettings["Usuario"]);
                    VerPassword.Text = Core.Utils.StringCipher.Decrypt(row[4].ToString().Trim(), ConfigurationManager.AppSettings["Usuario"]);
                    TxtDescripcion.Text = row[5].ToString().Trim();
                    GridEditUsuario.Visibility = Visibility.Visible;
                    GridNewUsuario.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 7));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (VerPassword.Visibility == Visibility.Visible)
            {
                TxtPassword.Password = VerPassword.Text;
            }
            if (TxtDominio.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un DOMINIO vacío.");
            }
            else if (TxtUsuario.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un USUARIO vacío.");
            }
            else if (TxtPassword.Password.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PASSWORD vacío.");
            }
            else
            {
                if (client.InsertUsuario(int.Parse(TxtIDServidorUsuario.Text.Trim()), TxtDominio.Text.Trim(), TxtUsuario.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPassword.Password.Trim(), ConfigurationManager.AppSettings["Usuario"].Trim()), TxtDescripcion.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioIDServer(int.Parse(TxtIDServidorUsuario.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar USUARIO.");
                }
            }
        }

        private void BtnUpdateUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (VerPassword.Visibility == Visibility.Visible)
            {
                TxtPassword.Password = VerPassword.Text;
            }
            if (TxtDominio.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un DOMINIO vacío.");
            }
            else if (TxtUsuario.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un USUARIO vacío.");
            }
            else if (TxtPassword.Password.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un PASSWORD vacío.");
            }
            else
            {
                if (client.UpdateUsuario(int.Parse(TxtIDUsuario.Text.Trim()), int.Parse(TxtIDServidorDisco.Text.Trim()), TxtDominio.Text.Trim(), TxtUsuario.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPassword.Password.Trim(), ConfigurationManager.AppSettings["Usuario"]), TxtDescripcion.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioIDServer(int.Parse(TxtIDServidorUsuario.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar USUARIO.");
                }
            }
        }

        private void BtnDeleteUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el USUARIO?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteUsuario(int.Parse(TxtIDUsuario.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectUsuarioIDServer(int.Parse(TxtIDServidorUsuario.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgUsuarios.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("USUARIO borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar USUARIO.");
                }
            }
        }

        private void BtnNewWebServices_Click(object sender, RoutedEventArgs e)
        {
            GridEditWebService.Visibility = Visibility.Collapsed;
            GridNewWebService.Visibility = Visibility.Visible;
            TxtIDWebService.Text = "";
            TxtNombreWebService.Text = "";
            TxtCarpetaWebService.Text = "";
            TxtUrlWebService.Text = "";
            ChkPublicado.IsChecked = false;
            TxtUrlExternaWebService.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 8));
        }

        private void BtnEditWebServices_Click(object sender, RoutedEventArgs e)
        {
            if (DgWebServices.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgWebServices.SelectedItems[0];
                    TxtIDWebService.Text = row[0].ToString().Trim();
                    TxtNombreWebService.Text = row[2].ToString().Trim();
                    TxtCarpetaWebService.Text = row[3].ToString().Trim();
                    TxtUrlWebService.Text = row[4].ToString().Trim();
                    if (row[5].ToString().Trim().Contains("SI"))
                    {
                        ChkPublicado.IsChecked = true;
                    }
                    else
                    {
                        ChkPublicado.IsChecked = false;
                    }
                    TxtUrlExternaWebService.Text = row[6].ToString().Trim();
                    GridEditWebService.Visibility = Visibility.Visible;
                    GridNewWebService.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 8));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarWebService_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreWebService.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un WEBSERVICE sin nombre.");
            }
            else if (TxtCarpetaWebService.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CARPETA vacía.");
            }
            else if (TxtUrlWebService.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una URL vacía");
            }
            else
            {
                string PUB = "";
                if (ChkPublicado.IsChecked == true)
                {
                    PUB = "SI";
                }
                else
                {
                    PUB = "NO";
                }
                if (client.InsertWebService(int.Parse(TxtIDServidorWebService.Text.Trim()), TxtNombreWebService.Text.Trim(), TxtCarpetaWebService.Text.Trim(), TxtUrlWebService.Text.Trim(), PUB, TxtUrlExternaWebService.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectWebServiceIDServer(int.Parse(TxtIDServidorWebService.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgWebServices.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("WEBSERVICE insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar WEBSERVICE.");
                }
            }
        }

        private void BtnUpdateWebService_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNombreWebService.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar un WEBSERVICE sin nombre.");
            }
            else if (TxtCarpetaWebService.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una CARPETA vacía.");
            }
            else if (TxtUrlWebService.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una URL vacía");
            }
            else
            {
                string PUB = "";
                if (ChkPublicado.IsChecked == true)
                {
                    PUB = "SI";
                }
                else
                {
                    PUB = "NO";
                }

                if (client.UpdateWebService(int.Parse(TxtIDWebService.Text.Trim()), int.Parse(TxtIDServidorWebService.Text.Trim()), TxtNombreWebService.Text.Trim(), TxtCarpetaWebService.Text.Trim(), TxtUrlWebService.Text.Trim(), PUB, TxtUrlExternaWebService.Text.Trim(), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectWebServiceIDServer(int.Parse(TxtIDServidorWebService.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgWebServices.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("WEBSERVICE actualizado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar WEBSERVICE.");
                }
            }
        }

        private void BtnDeleteWebService_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el WEBSERVICE?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteWebService(int.Parse(TxtIDWebService.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectWebServiceIDServer(int.Parse(TxtIDServidorWebService.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgWebServices.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("WEBSERVICE borrado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar WEBSERVICE.");
                }
            }
        }

        private void BtnNewInstancias_Click(object sender, RoutedEventArgs e)
        {
            GridEditInstancia.Visibility = Visibility.Collapsed;
            GridNewInstancia.Visibility = Visibility.Visible;
            TxtIDInstancia.Text = "";
            TxtInstancia.Text = "";
            TxtUsuarioInstancia.Text = "";
            TxtPasswordInstancia.Password = "";
            VerPasswordInstancia.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 9));
        }

        private void BtnEditInstancias_Click(object sender, RoutedEventArgs e)
        {
            if (DgInstancias.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgInstancias.SelectedItems[0];
                    TxtIDInstancia.Text = row[0].ToString().Trim();
                    CmbAmbienteInstancia.Text = row[2].ToString().Trim();
                    CmbProveedorInstancia.Text = row[3].ToString().Trim();
                    CmbAplicacionInstancia.Text = row[4].ToString().Trim();
                    TxtInstancia.Text = row[5].ToString().Trim(); ;
                    TxtUsuarioInstancia.Text = row[6].ToString().Trim();
                    TxtPasswordInstancia.Password = Core.Utils.StringCipher.Decrypt(row[7].ToString().Trim(), ConfigurationManager.AppSettings["InstanciaAplicacion"]);
                    VerPasswordInstancia.Text = Core.Utils.StringCipher.Decrypt(row[7].ToString().Trim(), ConfigurationManager.AppSettings["InstanciaAplicacion"]);
                    GridEditInstancia.Visibility = Visibility.Visible;
                    GridNewInstancia.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 9));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnGuardarInstancia_Click(object sender, RoutedEventArgs e)
        {
            if (VerPasswordInstancia.Visibility == Visibility.Visible)
            {
                TxtPasswordInstancia.Password = VerPasswordInstancia.Text;
            }
            if (CmbAmbienteInstancia.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin AMBIENTE.");
            }
            else if (CmbAplicacionInstancia.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin APLICACIÓN.");
            }
            else if (TxtInstancia.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin NOMBRE.");
            }
            else
            {
                if (client.InsertInstanciaAplicacion(int.Parse(TxtIDServidorInstancia.Text.Trim()), int.Parse(idambiente[ambiente.IndexOf(CmbAmbienteInstancia.Text.Trim())]), int.Parse(idaplicacion[aplicacion.IndexOf(CmbAplicacionInstancia.Text.Trim())]), TxtInstancia.Text.Trim(), TxtUsuarioInstancia.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPasswordInstancia.Password.Trim(), ConfigurationManager.AppSettings["InstanciaAplicacion"]), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectInstanciaAplicacionIDServer(int.Parse(TxtIDServidorInstancia.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgInstancias.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("INSTANCIA DE APLICACIÓN insertada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar INSTANCIA DE APLICACIÓN.");
                }
            }
        }

        private void BtnUpdateInstancia_Click(object sender, RoutedEventArgs e)
        {
            if (VerPasswordInstancia.Visibility == Visibility.Visible)
            {
                TxtPasswordInstancia.Password = VerPasswordInstancia.Text;
            }
            if (CmbAmbienteInstancia.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin AMBIENTE.");
            }
            else if (CmbAplicacionInstancia.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin APLICACIÓN.");
            }
            else if (TxtInstancia.Text.Trim().Length == 0)
            {
                MessageBox.Show("No se puede ingresar una INSTANCIA sin NOMBRE.");
            }
            else
            {
                if (client.UpdateInstanciaAplicacion(int.Parse(TxtIDInstancia.Text.Trim()), int.Parse(TxtIDServidorInstancia.Text.Trim()), int.Parse(idambiente[ambiente.IndexOf(CmbAmbienteInstancia.Text.Trim())]), int.Parse(idaplicacion[aplicacion.IndexOf(CmbAplicacionInstancia.Text.Trim())]), TxtInstancia.Text.Trim(), TxtUsuarioInstancia.Text.Trim(), Core.Utils.StringCipher.Encrypt(TxtPasswordInstancia.Password.Trim(), ConfigurationManager.AppSettings["InstanciaAplicacion"]), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectInstanciaAplicacionIDServer(int.Parse(TxtIDServidorInstancia.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgInstancias.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("INSTANCIA DE APLICACIÓN actualizada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al actualizar INSTANCIA DE APLICACIÓN.");
                }
            }
        }

        private void BtnDeleteInstancia_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar la INSTANCIA DE APLICACIÓN?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (client.DeleteInstanciaAplicacion(int.Parse(TxtIDInstancia.Text.Trim()), App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectInstanciaAplicacionIDServer(int.Parse(TxtIDServidorInstancia.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgInstancias.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("INSTANCIA DE APLICACIÓN borrada con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al borrar INSTANCIA DE APLICACIÓN.");
                }
            }
        }

        private void CmbProveedorInstancia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Aplicacion
            WSGCIS.Data Datos = client.SelectAplicacion(int.Parse(idproveedor[proveedor.IndexOf(CmbProveedorInstancia.SelectedItem.ToString().Trim())]), "%", App.Current.Properties["Username"].ToString());
            aplicacion.Clear();
            idaplicacion.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idaplicacion.Add(Datos.Tabla.Rows[i][0].ToString());
                aplicacion.Add(Datos.Tabla.Rows[i][1].ToString());
            }
            CmbAplicacionInstancia.ItemsSource = aplicacion;
            CmbAplicacionInstancia.Items.Refresh();
        }

        private void CmbSAN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WSGCIS.Data Datos = client.SelectPoolBySan(CmbSAN.SelectedItem.ToString().Trim(), App.Current.Properties["Username"].ToString());
            pool.Clear();
            idpool.Clear();
            for (int i = 0; i < Datos.Tabla.Rows.Count; i++)
            {
                idpool.Add(Datos.Tabla.Rows[i][0].ToString());
                pool.Add(Datos.Tabla.Rows[i][2].ToString());
            }
            CmbPool.ItemsSource = pool;
            CmbPool.Items.Refresh();
        }

        private void BtnNewDocumento_Click(object sender, RoutedEventArgs e)
        {
            GridEditDocumento.Visibility = Visibility.Collapsed;
            GridNewDocumento.Visibility = Visibility.Visible;
            TxtIDDocumento.Text = "";
            TxtNombreDocumento.Text = "";
            TxtDocumento.Text = "";
            Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 10));
        }

        private void BtnEditDocumento_Click(object sender, RoutedEventArgs e)
        {
            if (DgDocumentos.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una fila de la tabla.");
            }
            else
            {
                try
                {
                    System.Data.DataRowView row = (System.Data.DataRowView)DgDocumentos.SelectedItems[0];
                    TxtIDDocumento.Text = row[0].ToString().Trim();
                    TxtIDServidorDocumento.Text = row[1].ToString().Trim();
                    TxtDocumento.Text = "";
                    TxtNombreDocumento.Text = row[2].ToString().Trim();
                    GridEditDocumento.Visibility = Visibility.Visible;
                    GridNewDocumento.Visibility = Visibility.Collapsed;
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 10));
                }
                catch
                {
                    MessageBox.Show("Seleccione una fila válida.");
                }
            }
        }

        private void BtnAddDocumento_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                TxtDocumento.Text = dlg.FileName;
                TxtNombreDocumento.Text = dlg.FileName.Substring(dlg.FileName.LastIndexOf(@"\") + 1);
            }
        }

        private void BtnGuardarDocumento_Click(object sender, RoutedEventArgs e)
        {
            if (TxtDocumento.Text.Trim().Length == 0)
            {
                MessageBox.Show("Seleccione un documento para cargar.");
            }
            else
            {
                byte[] Documento;
                using (var stream = new System.IO.FileStream(TxtDocumento.Text, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (var reader = new System.IO.BinaryReader(stream))
                    {
                        Documento = reader.ReadBytes((int)stream.Length);
                    }
                }
                if (client.InsertDocumentoServidor(int.Parse(TxtIDServidorDocumento.Text.Trim()), TxtNombreDocumento.Text.Trim(), Documento, App.Current.Properties["Username"].ToString()))
                {
                    WSGCIS.Data Datos = client.SelectDocumentoServidor(int.Parse(TxtIDServidorDocumento.Text.Trim()), App.Current.Properties["Username"].ToString());
                    DgDocumentos.ItemsSource = Datos.Tabla.DefaultView;
                    MessageBox.Show("DOCUMENTO ASOCIADO AL SERVIDOR insertado con éxito.");
                    Dispatcher.BeginInvoke((Action)(() => TabVirtual.SelectedIndex = 2));
                }
                else
                {
                    MessageBox.Show("Error al insertar DOCUMENTO ASOCIADO AL SERVIDOR.");
                }
            }
        }

        private void BtnUpdateDocumento_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteDocumento_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabVirtual_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
