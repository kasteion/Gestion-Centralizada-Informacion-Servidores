using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace WCFGCIS
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "GCIS" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione GCIS.svc o GCIS.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class GCIS : IGCIS
    {
        protected SqlConnection Conexion;
        protected SqlCommand Comando;
        protected SqlDataAdapter Adaptador;

        private bool Open()
        {
            try
            {
                if (Conexion.State == ConnectionState.Open) { return true; }
                else { Conexion.Open(); return true; }
            }
            catch
            {
                return false;
            }
        }

        private bool Close()
        {
            try
            {
                if (Conexion.State == ConnectionState.Closed) { return true; }
                else { Conexion.Close(); return true; }
            }
            catch
            {
                return false;
            }
        }

        private GCIS()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EEGSA\GCIS\QAS");
            string ConnString = "Server=" + Core.Utils.StringCipher.Decrypt((string)key.GetValue("Server"), (string)key.GetValue("SecurityKey")) + ";Database=" + Core.Utils.StringCipher.Decrypt((string)key.GetValue("Database"), (string)key.GetValue("SecurityKey")) + ";User Id=" + Core.Utils.StringCipher.Decrypt((string)key.GetValue("User"), (string)key.GetValue("SecurityKey")) + ";Password=" + Core.Utils.StringCipher.Decrypt((string)key.GetValue("Password"), (string)key.GetValue("SecurityKey")) + ";";
            Conexion = new SqlConnection(ConnString);
            Comando = new SqlCommand("", Conexion);
            Comando.CommandTimeout = 90;
            Adaptador = new SqlDataAdapter(Comando);
        }

        //public string Passphrase()
        //{
        //    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EEGSA\GCIS\QAS");
        //    return Core.Utils.StringCipher.Decrypt((string)key.GetValue("Database"), (string)key.GetValue("SecurityKey"));
        //}

        #region Log
        private bool InsertLog(string Username, string Consulta, string Variables)
        {
            try
            {
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = "Insert into LOG values (@USUARIO, @FECHA, @ACCION)";
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Username;
                Comando.Parameters.Add("FECHA", SqlDbType.DateTime).Value = DateTime.Now;
                Comando.Parameters.Add("ACCION", SqlDbType.NVarChar, 4000).Value = "{ " + Consulta + ", " + Variables +" }";
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Acceso
        public Data SelectAcceso(string Username)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from ACCESO Where Usuario = @USUARIO";                
                //InsertLog("\"Usuario\": \"" + Username + "\"", "\"Consulta\": \"" + Query + "\"", "\"USUARIO\": \"" + Username + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Username;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }
        #endregion

        #region Ambiente
        public Data SelectAmbiente(string Ambiente, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from AMBIENTE Where Ambiente Like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Ambiente + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 10).Value = Ambiente;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertAmbiente(string Ambiente, string Usuario)
        {
            try
            {
                string Query = "Insert into AMBIENTE Values (@AMBIENTE)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"AMBIENTE\": \"" + Ambiente + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("AMBIENTE", SqlDbType.NVarChar, 10).Value = Ambiente;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateAmbiente(int ID, string Ambiente, string Usuario)
        {
            try
            {
                string Query = "Update AMBIENTE Set Ambiente = @AMBIENTE Where IDAmbiente = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"AMBIENTE\": \"" + Ambiente + "\", \"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("AMBIENTE", SqlDbType.NVarChar, 10).Value = Ambiente;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteAmbiente(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete AMBIENTE Where IDAmbiente = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Aplicacion
        public Data SelectAplicacion(int Proveedor, string Aplicacion, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDAplicacion, Aplicacion, Version, Fabricante, Licencia, PROVEEDOR.Nombre as Proveedor, FechaVencimiento from APLICACION, PROVEEDOR Where APLICACION.IDProveedor = PROVEEDOR.IDProveedor";
                if (Proveedor > 0)
                {
                    Query += " And APLICACION.IDProveedor = @PROVEEDOR";
                }
                Query += " And Aplicacion like @APLICACION";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"PROVEEDOR\": \"" + Proveedor + "\", \"APLICACION\": \"" + Aplicacion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt).Value = Proveedor;
                Comando.Parameters.Add("APLICACION", SqlDbType.NVarChar, 100).Value = Aplicacion;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertAplicacion(string Aplicacion, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario)
        {
            try
            {
                string Query = "Insert into APLICACION Values (@APLICACION, @VERSION, @FABRICANTE, @LICENCIA, @PROVEEDOR, @FECHAVENCIMIENTO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"APLICACION\": \"" + Aplicacion + "\", \"VERSION\": \"" + Version + "\", \"FABRICANTE\": \"" + Fabricante + "\", \"LICENCIA\": \"" + Licencia + "\", \"PROVEEDOR\": \"" + Proveedor + "\", \"FECHAVENCIMIENTO\": \"" + FechaVencimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("APLICACION", SqlDbType.NVarChar, 100).Value = Aplicacion;
                Comando.Parameters.Add("VERSION", SqlDbType.NVarChar, 50).Value = Version;
                Comando.Parameters.Add("FABRICANTE", SqlDbType.NVarChar, 100).Value = Fabricante;
                Comando.Parameters.Add("LICENCIA", SqlDbType.NVarChar, 100).Value = Licencia;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt).Value = Proveedor;
                Comando.Parameters.Add("FECHAVENCIMIENTO", SqlDbType.DateTime).Value = FechaVencimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateAplicacion(int ID, string Aplicacion, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario)
        {
            try
            {
                string Query = "Update APLICACION Set Aplicacion = @APLICACION, Version = @VERSION, Fabricante = @FABRICANTE, Licencia = @LICENCIA, IDProveedor = @PROVEEDOR, FechaVencimiento = @FECHAVENCIMIENTO Where IDAplicacion = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"APLICACION\": \"" + Aplicacion + "\", \"VERSION\": \"" + Version + "\", \"FABRICANTE\": \"" + Fabricante + "\", \"LICENCIA\": \"" + Licencia + "\", \"PROVEEDOR\": \"" + Proveedor + "\", \"FECHAVENCIMIENTO\": \"" + FechaVencimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("APLICACION", SqlDbType.NVarChar, 100).Value = Aplicacion;
                Comando.Parameters.Add("VERSION", SqlDbType.NVarChar, 50).Value = Version;
                Comando.Parameters.Add("FABRICANTE", SqlDbType.NVarChar, 100).Value = Fabricante;
                Comando.Parameters.Add("LICENCIA", SqlDbType.NVarChar, 100).Value = Licencia;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt).Value = Proveedor;
                Comando.Parameters.Add("FECHAVENCIMIENTO", SqlDbType.DateTime).Value = FechaVencimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteAplicacion(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete APLICACION Where IDAplicacion = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region BackupExecJobs
        public Data SelectBackupExecJobsIDServer(int IDServidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from BACKUPEXEC_JOBS Where IDServidor = @IDSERVIDOR";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectBackupExecJobs(string BackupJob, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from BACKUPEXEC_JOBS Where BackupJob like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + BackupJob + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 100).Value = BackupJob;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertBackupExecJobs(int IDServidor, string BackupExecJob, string Descripcion, string DiaSemana, string Hora, string Usuario)
        {
            try
            {
                string Query = "Insert into BACKUPEXEC_JOBS Values (@IDSERVIDOR, @BACKUPJOB, @DESCRIPCION, @DIASEMANA, @HORA)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"BACKUPJOB\": \"" + BackupExecJob + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"DIASEMANA\": \"" + DiaSemana + "\", \"HORA\": \"" + Hora + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("BACKUPJOB", SqlDbType.NVarChar, 100).Value = BackupExecJob;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("DIASEMANA", SqlDbType.NVarChar, 7).Value = DiaSemana;
                Comando.Parameters.Add("HORA", SqlDbType.NVarChar, 5).Value = Hora;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateBackupExecJobs(int ID, int IDServidor, string BackupExecJob, string Descripcion, string DiaSemana, string Hora, string Usuario)
        {
            try
            {
                string Query = "Update BACKUPEXEC_JOBS Set IDServidor = @IDSERVIDOR, BackupJob = @BACKUPJOB, Descripcion = @DESCRIPCION, DiaSemana = @DIASEMANA, Hora = @HORA Where IDBackupJob = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"IDSERVIDOR\": \"" + IDServidor + "\", \"BACKUPJOB\": \"" + BackupExecJob + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"DIASEMANA\": \"" + DiaSemana + "\", \"HORA\": \"" + Hora + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("BACKUPJOB", SqlDbType.NVarChar, 100).Value = BackupExecJob;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("DIASEMANA", SqlDbType.NVarChar, 7).Value = DiaSemana;
                Comando.Parameters.Add("HORA", SqlDbType.NVarChar, 5).Value = Hora;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteBackupExecJobs(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete BACKUPEXEC_JOBS Where IDBackupJob = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Cluster
        public Data SelectCluster(string Cluster, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDCluster, Cluster, Descripcion, PROVEEDOR.Nombre As Proveedor, Aplicacion As IDSoftwareVirtualizacion from CLUSTER, APLICACION, PROVEEDOR Where CLUSTER.IDSoftwareVirtualizacion = APLICACION.IDAplicacion And APLICACION.IDProveedor = PROVEEDOR.IDProveedor And Cluster like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Cluster + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Cluster;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertCluster(string Cluster, string Descripcion, int IDAplicacion, string Usuario)
        {
            try
            {
                string Query = "Insert into CLUSTER Values (@CLUSTER, @DESCRIPCION, @IDAPLICACION)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"CLUSTER\": \"" + Cluster + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"IDAPLICACION\": \"" + IDAplicacion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("CLUSTER", SqlDbType.NVarChar, 50).Value = Cluster;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("IDAPLICACION", SqlDbType.SmallInt).Value = IDAplicacion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateCluster(int IDCluster, string Cluster, string Descripcion, int IDAplicacion, string Usuario)
        {
            try
            {
                string Query = "Update CLUSTER Set Cluster = @CLUSTER, Descripcion = @DESCRIPCION, IDSoftwareVirtualizacion = @IDAPLICACION Where IDCluster = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"CLUSTER\": \"" + Cluster + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"IDAPLICACION\": \"" + IDAplicacion + "\", \"ID\": \"" + IDCluster + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("CLUSTER", SqlDbType.NVarChar, 50).Value = Cluster;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("IDAPLICACION", SqlDbType.SmallInt).Value = IDAplicacion;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = IDCluster;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteCluster(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete CLUSTER Where IDCluster = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");                
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region ClusterServidorFisico
        public Data SelectClusterServidorFisico(string Like, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select SERVIDOR.Servidor As IDServidorFisico, CLUSTER.Cluster As IDCluster from CLUSTER_SERVIDOR_FISICO, CLUSTER, SERVIDOR Where CLUSTER_SERVIDOR_FISICO.IDCluster = CLUSTER.IDCluster And CLUSTER_SERVIDOR_FISICO.IDServidorFisico = SERVIDOR.IDServidor And Cluster like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Like + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Like;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertClusterServidorFisico(int IDServidorFisico, int IDCluster, string Usuario)
        {
            try
            {
                string Query = "Insert into CLUSTER_SERVIDOR_FISICO Values (@IDSERVIDORFISICO, @IDCLUSTER)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDORFISICO\": \"" + IDServidorFisico + "\", \"IDCLUSTER\": \"" + IDCluster + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDORFISICO", SqlDbType.SmallInt).Value = IDServidorFisico;
                Comando.Parameters.Add("IDCLUSTER", SqlDbType.SmallInt).Value = IDCluster;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateClusterServidorFisico(int IDServidorFisico, int IDCluster, string Usuario)
        {
            try
            {
                string Query = "Update CLUSTER_SERVIDOR_FISICO Set IDServidorFisico = @IDSERVIDORFISICO Where IDCluster = @IDCLUSTER";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDORFISICO\": \"" + IDServidorFisico + "\", \"IDCLUSTER\": \"" + IDCluster + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDORFISICO", SqlDbType.SmallInt).Value = IDServidorFisico;
                Comando.Parameters.Add("IDCLUSTER", SqlDbType.SmallInt).Value = IDCluster;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteClusterServidorFisico(int IDServidorFisico, int IDCluster, string Usuario)
        {
            try
            {
                string Query = "Delete CLUSTER_SERVIDOR_FISICO Where IDServidorFisico = @IDSERVIDORFISICO And @IDCluster = @IDCLUSTER";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDORFISICO\": \"" + IDServidorFisico + "\", \"IDCLUSTER\": \"" + IDCluster + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDORFISICO", SqlDbType.SmallInt).Value = IDServidorFisico;
                Comando.Parameters.Add("IDCLUSTER", SqlDbType.SmallInt).Value = IDCluster;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Contacto
        public Data SelectContacto(int Proveedor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDContacto, PROVEEDOR.Nombre as Proveedor, CONTACTO.Nombre As Contacto, Telefono, Emergencia from CONTACTO, PROVEEDOR Where CONTACTO.IDProveedor = PROVEEDOR.IDProveedor";
                if (Proveedor > 0)
                {
                    Query += " And CONTACTO.IDProveedor = @ID";
                }
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + Proveedor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = Proveedor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertContacto(int Proveedor, string Nombre, string Telefono, string Emergencia, string Usuario)
        {
            try
            {
                string Query = "Insert into CONTACTO Values (@PROVEEDOR, @NOMBRE, @TELEFONO, @EMERGENCIA)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"PROVEEDOR\": \"" + Proveedor + "\", \"NOMBRE\": \"" + Nombre + "\", \"TELEFONO\": \"" + Telefono + "\", \"EMERGENCIA\": \"" + Emergencia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt, 50).Value = Proveedor;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 100).Value = Nombre;
                Comando.Parameters.Add("TELEFONO", SqlDbType.NVarChar, 50).Value = Telefono;
                Comando.Parameters.Add("EMERGENCIA", SqlDbType.NVarChar, 2).Value = Emergencia;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateContacto(int ID, int Proveedor, string Nombre, string Telefono, string Emergencia, string Usuario)
        {
            try
            {
                string Query = "Update CONTACTO Set IDProveedor = @PROVEEDOR, Nombre = @NOMBRE, Telefono=@TELEFONO, Emergencia = @EMERGENCIA Where IDContacto = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"PROVEEDOR\": \"" + Proveedor + "\", \"NOMBRE\": \"" + Nombre + "\", \"TELEFONO\": \"" + Telefono + "\", \"EMERGENCIA\": \"" + Emergencia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt, 50).Value = Proveedor;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 100).Value = Nombre;
                Comando.Parameters.Add("TELEFONO", SqlDbType.NVarChar, 50).Value = Telefono;
                Comando.Parameters.Add("EMERGENCIA", SqlDbType.NVarChar, 2).Value = Emergencia;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteContacto(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete CONTACTO Where IDContacto = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Database
        public Data SelectDatabaseIDInstancia(int IDInstancia, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDDatabase, INSTANCIA_APLICACION.Instancia As IDInstancia, Nombre, Descripcion from [dbo].[DATABASE], INSTANCIA_APLICACION Where [dbo].[DATABASE].IDInstancia = INSTANCIA_APLICACION.IDInstancia And INSTANCIA_APLICACION.IDInstancia = @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + IDInstancia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.SmallInt).Value = IDInstancia;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectDatabase(string NombreDatabase, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDDatabase, [dbo].[DATABASE].IDInstancia, INSTANCIA_APLICACION.Instancia, Nombre As NombreDatabase, Descripcion from [dbo].[DATABASE], INSTANCIA_APLICACION Where [dbo].[DATABASE].IDInstancia = INSTANCIA_APLICACION.IDInstancia And Nombre like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + NombreDatabase + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = NombreDatabase;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertDatabase(int IDInstancia, string Nombre, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Insert into [dbo].[DATABASE] VAlues (@IDINSTANCIA, @NOMBRE, @DESCRIPCION)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDINSTANCIA\": \"" + IDInstancia + "\", \"NOMBRE\": \"" + Nombre + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 50).Value = Nombre;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateDatabase(int IDDatabase, int IDInstancia, string Nombre, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Update [dbo].[DATABASE] Set IDInstancia = @IDINSTANCIA, Nombre = @NOMBRE, Descripcion = @DESCRIPCION Where IDDatabase = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDINSTANCIA\": \"" + IDInstancia + "\", \"NOMBRE\": \"" + Nombre + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"ID\": \"" + IDDatabase + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 50).Value = Nombre;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteDatabase(int IDDatabase, string Usuario)
        {
            try
            {
                string Query = "Delete [dbo].[DATABASE] Where IDDatabase = @IDDATABASE";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + IDDatabase + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDATABASE", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region DatabaseMantenimiento
        public Data SelectDatabaseMantenimiento(int IDDatabase, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select DATABASE_MANTENIMIENTO.IDDatabase, [dbo].[DATABASE].Nombre as NombreDatabase, DATABASE_MANTENIMIENTO.IDPlanMantenimiento, PLAN_MANTENIMIENTO.Nombre as NombreMantenimiento from DATABASE_MANTENIMIENTO, [dbo].[DATABASE], PLAN_MANTENIMIENTO Where DATABASE_MANTENIMIENTO.IDDatabase = [dbo].[DATABASE].IDDatabase And DATABASE_MANTENIMIENTO.IDPlanMantenimiento = PLAN_MANTENIMIENTO.IDPlanMantenimiento And DATABASE_MANTENIMIENTO.IDDatabase Like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + IDDatabase + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.SmallInt).Value = IDDatabase;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertDatabaseMantenimiento(int IDDatabase, int IDPlanMantenimiento, string Usuario)
        {
            try
            {
                string Query = "Insert into DATABASE_MANTENIMIENTO Values (@IDDATABASE, @IDPLANMANTENIMIENTO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDATABASE\": \"" + IDDatabase + "\", \"IDPLANMANTENIMIENTO\": \"" + IDPlanMantenimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDATABASE", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.Parameters.Add("IDPLANMANTENIMIENTO", SqlDbType.SmallInt).Value = IDPlanMantenimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateDatabaseMantenimiento(int IDDatabase , int IDPlanMantenimiento, string Usuario)
        {
            try
            {
                string Query = "Update DATABASE_MANTENIMIENTO Set IDPlanMantenimiento = @IDPLANMANTENIMIENTO Where IDDatabase = @IDDATABASE ";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDATABASE\": \"" + IDDatabase + "\", \"IDPLANMANTENIMIENTO\": \"" + IDPlanMantenimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDATABASE", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.Parameters.Add("IDPLANMANTENIMIENTO", SqlDbType.SmallInt).Value = IDPlanMantenimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteDatabaseMantenimiento(int IDDatabase, int IDPlanMantenimiento, string Usuario)
        {
            try
            {
                string Query = "Delete DATABASE_MANTENIMIENTO Where IDDatabase = @IDDATABASE And IDPlanMantenimiento = @IDPLANMANTENIMIENTO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDATABASE\": \"" + IDDatabase + "\", \"IDPLANMANTENIMIENTO\": \"" + IDPlanMantenimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDATABASE", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.Parameters.Add("IDPLANMANTENIMIENTO", SqlDbType.SmallInt).Value = IDPlanMantenimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region DatabaseUsuario
        public Data SelectDatabaseUsuario(string DatabaseNombre, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select DATABASE_USUARIO.IDDatabase, [dbo].[DATABASE].Nombre as NombreDatabase, DATABASE_USUARIO.IDUsuario, Usuario from DATABASE_USUARIO, [dbo].[DATABASE], USUARIO_INSTANCIA Where DATABASE_USUARIO.IDDatabase = [dbo].[DATABASE].IDDatabase And DATABASE_USUARIO.IDUsuario = USUARIO_INSTANCIA.IDUsuario And [dbo].[DATABASE].Nombre like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + DatabaseNombre + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = DatabaseNombre;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertDatabaseUsuario(int IDDatabase, int IDUsuario, string Usuario)
        {
            try
            {
                string Query = "Insert into DATABASE_USUARIO Values (@IDDATABASE, @IDUSUARIO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDATABASE\": \"" + IDDatabase + "\", \"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDATABASE", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateDatabaseUsuario(int IDDatabase, int IDUsuario, string Usuario)
        {
            try
            {
                string Query = "Update DATABASE_USUARIO Set IDUsuario = @IDUSUARIO Where IDDatabase = @IDDATABASE";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDATABASE\": \"" + IDDatabase + "\", \"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDATABASE", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteDatabaseUsuario(int IDDatabase, int IDUsuario, string Usuario)
        {
            try
            {
                string Query = "Delete DATABASE_USUARIO Where IDDatabase = @IDDATABASE And IDUsuario = @IDUSUARIO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDATABASE\": \"" + IDDatabase + "\", \"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDATABASE", SqlDbType.SmallInt).Value = IDDatabase;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Disco
        public Data SelectDisco(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDDisco, DISCO.IDServidor, SERVIDOR.Servidor, Disco, Capacidad, SAN.San, POOL.Pool, VolumenCompartido from DISCO, SERVIDOR, SAN, POOL Where DISCO.IDServidor = SERVIDOR.IDServidor And DISCO.IDPool = POOL.IDPool And POOL.IDSan = SAN.IDSan And Servidor like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertDisco(int IDServidor, string Disco, int Capacidad, int IDPool, string VolumenCompartido, string Usuario)
        {
            try
            {
                string Query = "Insert into DISCO Values (@IDSERVIDOR, @DISCO, @CAPACIDAD, @IDPOOL, @VOLUMENCOMPARTIDO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"DISCO\": \"" + Disco + "\", \"CAPACIDAD\": \"" + Capacidad + "\", \"IDPOOL\": \"" + IDPool + "\", \"VOLUMENCOMPARTIDO\": \"" + VolumenCompartido + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("DISCO", SqlDbType.NVarChar, 50).Value = Disco;
                Comando.Parameters.Add("CAPACIDAD", SqlDbType.SmallInt).Value = Capacidad;
                Comando.Parameters.Add("IDPOOL", SqlDbType.SmallInt).Value = IDPool;
                Comando.Parameters.Add("VOLUMENCOMPARTIDO", SqlDbType.NVarChar,250).Value = VolumenCompartido;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateDisco(int IDDisco, int IDServidor, string Disco, int Capacidad, int IDPool, string VolumenCompartido, string Usuario)
        {
            try
            {
                string Query = "Update DISCO Set IDServidor = @IDSERVIDOR, Disco = @DISCO, Capacidad = @CAPACIDAD, IDPool = @IDPOOL, VolumenCompartido = @VOLUMENCOMPARTIDO Where IDDisco = @IDDISCO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"DISCO\": \"" + Disco + "\", \"CAPACIDAD\": \"" + Capacidad + "\", \"IDPOOL\": \"" + IDPool + "\", \"VOLUMENCOMPARTIDO\": \"" + VolumenCompartido + "\", \"IDDISCO\": \"" + IDDisco + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("DISCO", SqlDbType.NVarChar, 50).Value = Disco;
                Comando.Parameters.Add("CAPACIDAD", SqlDbType.SmallInt).Value = Capacidad;
                Comando.Parameters.Add("IDPOOL", SqlDbType.SmallInt).Value = IDPool;
                Comando.Parameters.Add("VOLUMENCOMPARTIDO", SqlDbType.NVarChar,250).Value = VolumenCompartido;
                Comando.Parameters.Add("IDDISCO", SqlDbType.SmallInt).Value = IDDisco;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteDisco(int IDDisco, string Usuario)
        {
            try
            {
                string Query = "Delete DISCO Where IDDisco = @IDDISCO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDISCO\": \"" + IDDisco + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDISCO", SqlDbType.SmallInt).Value = IDDisco;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region DocumentoServidor
        public Data SelectDocumentoServidor(int IDServidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDDocumento, IDServidor, NombreDocumento, Documento from DOCUMENTO_SERVIDOR Where IDServidor = @IDSERVIDOR";
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertDocumentoServidor(int IDServidor, string NombreDocumento, byte[] Documento, string Usuario)
        {
            try
            {
                string Query = "Insert into DOCUMENTO_SERVIDOR Values (@IDSERVIDOR, @NOMBREDOCUMENTO, @DOCUMENTO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"NOMBREDOCUMENTO\": \"" + NombreDocumento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("NOMBREDOCUMENTO", SqlDbType.VarChar, NombreDocumento.Length).Value = NombreDocumento;
                Comando.Parameters.Add("DOCUMENTO", SqlDbType.VarBinary, Documento.Length).Value = Documento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateDocumentoServidor(int IDDocumento, string NombreDocumento, byte[] Documento, string Usuario)
        {
            try
            {
                string Query = "Update DOCUMENTO_SERVIDOR Set NombreDocumento=@NOMBREDOCUMENTO, Documento=@DOCUMENTO Where IDDocumento=@IDDOCUMENTO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDocumento\": \"" + IDDocumento + "\", \"NOMBREDOCUMENTO\": \"" + NombreDocumento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("NOMBREDOCUMENTO", SqlDbType.VarChar, NombreDocumento.Length).Value = NombreDocumento;
                Comando.Parameters.Add("DOCUMENTO", SqlDbType.VarBinary, Documento.Length).Value = Documento;
                Comando.Parameters.Add("IDDOCUMENTO", SqlDbType.SmallInt).Value = IDDocumento;
                Comando.ExecuteNonQuery();
                Close();
                return true;

            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteDocumentoServidor(int IDDocumento, string Usuario)
        {
            try
            {
                string Query = "Delete DOCUMENTO_SERVIDOR Where IDDocumento=@IDDOCUMENTO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDDocumento\": \"" + IDDocumento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDDOCUMENTO", SqlDbType.SmallInt).Value = IDDocumento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        #endregion

        #region Edificio
        public Data SelectEdificio(string Edificio, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from EDIFICIO Where Edificio like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Edificio + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Edificio;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertEdificio(string Edificio, string Direccion, string Usuario)
        {
            try
            {
                string Query = "Insert into EDIFICIO Values (@EDIFICIO, @DIRECCION)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"EDIFICIO\": \"" + Edificio + "\", \"DIRECCION\": \"" + Direccion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("EDIFICIO", SqlDbType.NVarChar, 50).Value = Edificio;
                Comando.Parameters.Add("DIRECCION", SqlDbType.NVarChar, 100).Value = Direccion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateEdificio(int ID, string Edificio, string Direccion, string Usuario)
        {
            try
            {
                string Query = "Update EDIFICIO Set Edificio = @EDIFICIO, Direccion = @DIRECCION Where IDEdificio = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"EDIFICIO\": \"" + Edificio + "\", \"DIRECCION\": \"" + Direccion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("EDIFICIO", SqlDbType.NVarChar, 50).Value = Edificio;
                Comando.Parameters.Add("DIRECCION", SqlDbType.NVarChar, 100).Value = Direccion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteEdificio(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete EDIFICIO Where IDEdificio = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Ethernet
        public Data SelectEthernet(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select DireccionIP, SERVIDOR.Servidor from ETHERNET, SERVIDOR Where ETHERNET.IDServidor = SERVIDOR.IDServidor And Servidor like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertEthernet(string DireccionIP, int IDServidor, string Usuario)
        {
            try
            {
                string Query = "Insert into ETHERNET Values (@DIRECCIONIP, @IDSERVIDOR)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"DIRECCIONIP\": \"" + DireccionIP + "\", \"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("DIRECCIONIP", SqlDbType.NVarChar, 50).Value = DireccionIP;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateEthernet(string DireccionIP, int IDServidor, string NewDireccionIP, string Usuario)
        {
            try
            {
                string Query = "Update ETHERNET Set DireccionIP = @NEWDIRECCIONIP Where IDServidor = @IDSERVIDOR And DireccionIP = @DIRECCIONIP";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"DIRECCIONIP\": \"" + DireccionIP + "\", \"IDSERVIDOR\": \"" + IDServidor + "\", \"NEWDIRECCIONIP\": \"" + NewDireccionIP + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("DIRECCIONIP", SqlDbType.NVarChar, 50).Value = DireccionIP;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("NEWDIRECCIONIP", SqlDbType.NVarChar, 50).Value = NewDireccionIP;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteEthernet(string DireccionIP, string Usuario)
        {
            try
            {
                string Query = "Delete ETHERNET Where DireccionIP = @DIRECCIONIP";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"DIRECCIONIP\": \"" + DireccionIP + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("DIRECCIONIP", SqlDbType.NVarChar, 50).Value = DireccionIP;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region InstanciaAplicacion
        public Data SelectInstanciaAplicacionIDServer(int IDServidor, string UsuarioLog)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDInstancia, IDServidor, Ambiente, PROVEEDOR.Nombre As Proveedor, Aplicacion, Instancia, UsuarioAdministrador, PasswordAdministrador from INSTANCIA_APLICACION, AMBIENTE, APLICACION, PROVEEDOR Where IDServidor = @IDSERVIDOR And INSTANCIA_APLICACION.IDAmbiente = AMBIENTE.IDAmbiente And INSTANCIA_APLICACION.IDAplicacion = APLICACION.IDAplicacion And APLICACION.IDProveedor = PROVEEDOR.IDProveedor";
                //InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Select IDInstancia, IDServidor, Ambiente, PROVEEDOR.Nombre As Proveedor, Aplicacion, Instancia, UsuarioAdministrador, CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE('Global Cardamomo Indigo Soledad', PasswordAdministrador)) As PasswordAdministrador from INSTANCIA_APLICACION, AMBIENTE, APLICACION, PROVEEDOR Where IDServidor = @IDSERVIDOR And INSTANCIA_APLICACION.IDAmbiente = AMBIENTE.IDAmbiente And INSTANCIA_APLICACION.IDAplicacion = APLICACION.IDAplicacion And APLICACION.IDProveedor = PROVEEDOR.IDProveedor";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectInstanciaAplicacion(string Instancia, string UsuarioLog)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDInstancia, SERVIDOR.IDServidor, SERVIDOR.Servidor, AMBIENTE.Ambiente, PROVEEDOR.Nombre As Proveedor, APLICACION.Aplicacion, Instancia, UsuarioAdministrador, PasswordAdministrador from INSTANCIA_APLICACION, SERVIDOR, AMBIENTE, PROVEEDOR, APLICACION Where INSTANCIA_APLICACION.IDServidor = SERVIDOR.IDServidor And INSTANCIA_APLICACION.IDAmbiente = AMBIENTE.IDAmbiente And INSTANCIA_APLICACION.IDAplicacion = APLICACION.IDAplicacion And APLICACION.IDProveedor = PROVEEDOR.IDProveedor And Instancia like @LIKE";
                //InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Instancia + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Select IDInstancia, SERVIDOR.IDServidor, SERVIDOR.Servidor, AMBIENTE.Ambiente, PROVEEDOR.Nombre As Proveedor, APLICACION.Aplicacion, Instancia, UsuarioAdministrador, CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE('Global Cardamomo Indigo Soledad', PasswordAdministrador)) As PasswordAdministrador from INSTANCIA_APLICACION, SERVIDOR, AMBIENTE, PROVEEDOR, APLICACION Where INSTANCIA_APLICACION.IDServidor = SERVIDOR.IDServidor And INSTANCIA_APLICACION.IDAmbiente = AMBIENTE.IDAmbiente And INSTANCIA_APLICACION.IDAplicacion = APLICACION.IDAplicacion And APLICACION.IDProveedor = PROVEEDOR.IDProveedor And Instancia like @LIKE";
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 100).Value = Instancia;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertInstanciaAplicacion(int IDServidor, int IDAmbiente, int IDAplicacion, string Instancia, string Usuario, string Password, string UsuarioLog)
        {
            try
            {
                string Query = "Insert into INSTANCIA_APLICACION Values (@IDSERVIDOR, @IDAMBIENTE, @IDAPLICACION, @INSTANCIA, @USUARIO, @PASSWORD)";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDAMBIENTE\": \"" + IDAmbiente + "\", \"IDAPLICACION\": \"" + IDAplicacion + "\", \"INSTANCIA\": \"" + Instancia + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Insert into INSTANCIA_APLICACION Values (@IDSERVIDOR, @IDAMBIENTE, @IDAPLICACION, @INSTANCIA, @USUARIO, ENCRYPTBYPASSPHRASE('Global Cardamomo Indigo Soledad', @PASSWORD))";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDAMBIENTE", SqlDbType.SmallInt).Value = IDAmbiente;
                Comando.Parameters.Add("IDAPLICACION", SqlDbType.SmallInt).Value = IDAplicacion;
                Comando.Parameters.Add("INSTANCIA", SqlDbType.NVarChar, 100).Value = Instancia;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateInstanciaAplicacion(int IDInstancia, int IDServidor, int IDAmbiente, int IDAplicacion, string Instancia, string Usuario, string Password, string UsuarioLog)
        {
            try
            {
                string Query = "Update INSTANCIA_APLICACION Set IDServidor = @IDSERVIDOR, IDAmbiente = @IDAMBIENTE, IDAplicacion = @IDAPLICACION, Instancia = @INSTANCIA, UsuarioAdministrador = @USUARIO, PasswordAdministrador = @PASSWORD Where IDInstancia = @IDINSTANCIA";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDAMBIENTE\": \"" + IDAmbiente + "\", \"IDAPLICACION\": \"" + IDAplicacion + "\", \"INSTANCIA\": \"" + Instancia + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\", \"IDINSTANCIA\": \"" + IDInstancia + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Update INSTANCIA_APLICACION Set IDServidor = @IDSERVIDOR, IDAmbiente = @IDAMBIENTE, IDAplicacion = @IDAPLICACION, Instancia = @INSTANCIA, UsuarioAdministrador = @USUARIO, PasswordAdministrador = ENCRYPTBYPASSPHRASE('Global Cardamomo Indigo Soledad', @PASSWORD) Where IDInstancia = @IDINSTANCIA";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDAMBIENTE", SqlDbType.SmallInt).Value = IDAmbiente;
                Comando.Parameters.Add("IDAPLICACION", SqlDbType.SmallInt).Value = IDAplicacion;
                Comando.Parameters.Add("INSTANCIA", SqlDbType.NVarChar, 100).Value = Instancia;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteInstanciaAplicacion(int IDInstancia, string UsuarioLog)
        {
            try
            {
                string Query = "Delete INSTANCIA_APLICACION Where IDInstancia = @IDINSTANCIA";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDINSTANCIA\": \"" + IDInstancia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Marca
        public Data SelectMarca(string Marca, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from MARCA Where Marca like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Marca + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Marca;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertMarca(string Marca, string Usuario)
        {
            try
            {
                string Query = "Insert into MARCA Values (@MARCA)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"MARCA\": \"" + Marca + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("MARCA", SqlDbType.NVarChar, 50).Value = Marca;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateMarca(int ID, string Marca, string Usuario)
        {
            try
            {
                string Query = "Update MARCA Set Marca = @MARCA Where IDMarca = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"MARCA\": \"" + Marca + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("MARCA", SqlDbType.NVarChar, 50).Value = Marca;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteMarca(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete MARCA Where IDMarca = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Modelo
        public Data SelectModeloByMarca(string Marca, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDModelo, Marca, Modelo from MODELO, MARCA Where MODELO.IDMarca = MARCA.IDMarca And Marca like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Marca + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Marca;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectModelo(string Modelo, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDModelo, Marca, Modelo from MODELO, MARCA Where MODELO.IDMarca = MARCA.IDMarca And Modelo like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Modelo + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Modelo;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertModelo(int IDMarca, string Modelo, string Usuario)
        {
            try
            {
                string Query = "Insert into MODELO Values (@IDMARCA, @MODELO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDMARCA\": \"" + IDMarca + "\", \"MODELO\": \"" + Modelo + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDMARCA", SqlDbType.SmallInt).Value = IDMarca;
                Comando.Parameters.Add("MODELO", SqlDbType.NVarChar, 50).Value = Modelo;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateModelo(int ID, int IDMarca, string Modelo, string Usuario)
        {
            try
            {
                string Query = "Update MODELO Set IDMarca = @IDMARCA, Modelo = @MODELO Where IDModelo = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"IDMARCA\": \"" + IDMarca + "\", \"MODELO\": \"" + Modelo + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("IDMARCA", SqlDbType.SmallInt).Value = IDMarca;
                Comando.Parameters.Add("MODELO", SqlDbType.NVarChar, 50).Value = Modelo;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteModelo(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete MODELO Where IDModelo = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region PlanMantenimiento
        public Data SelectPlanMantenimiento(string NombreInstancia, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDPlanMantenimiento, INSTANCIA_APLICACION.Instancia, Nombre, Descripcion, DiaSemana, Hora from PLAN_MANTENIMIENTO, INSTANCIA_APLICACION Where PLAN_MANTENIMIENTO.IDInstancia = INSTANCIA_APLICACION.IDInstancia And INSTANCIA_APLICACION.Instancia Like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + NombreInstancia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = NombreInstancia;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertPlanMantenimiento(int IDInstancia, string Nombre, string Descripcion, string DiaSemana, string Hora, string Usuario)
        {
            try
            {
                string Query = "Insert into PLAN_MANTENIMIENTO Values (@IDINSTANCIA, @NOMBRE, @DESCRIPCION, @DIASEMANA, @HORA)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDINSTANCIA\": \"" + IDInstancia + "\", \"NOMBRE\": \"" + Nombre + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"DIASEMANA\": \"" + DiaSemana + "\", \"HORA\": \"" + Hora + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 50).Value = Nombre;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("DIASEMANA", SqlDbType.NVarChar, 7).Value = DiaSemana;
                Comando.Parameters.Add("HORA", SqlDbType.NVarChar, 5).Value = Hora;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdatePlanMantenimiento(int IDPlanMantenimiento, int IDInstancia, string Nombre, string Descripcion, string DiaSemana, string Hora, string Usuario)
        {
            try
            {
                string Query = "Update PLAN_MANTENIMIENTO Set IDInstancia = @IDINSTANCIA, Nombre = @NOMBRE, Descripcion = @DESCRIPCION, DiaSemana = @DIASEMANA, Hora = @HORA Where IDPlanMantenimiento = @IDPLANMANTENIMIENTO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDPLANMANTENIMIENTO\": \"" + IDPlanMantenimiento + "\", \"IDINSTANCIA\": \"" + IDInstancia + "\", \"NOMBRE\": \"" + Nombre + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"DIASEMANA\": \"" + DiaSemana + "\", \"HORA\": \"" + Hora + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDPLANMANTENIMIENTO", SqlDbType.SmallInt).Value = IDPlanMantenimiento;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 50).Value = Nombre;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("DIASEMANA", SqlDbType.NVarChar, 7).Value = DiaSemana;
                Comando.Parameters.Add("HORA", SqlDbType.NVarChar, 5).Value = Hora;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeletePlanMantenimiento(int IDPlanMantenimiento, string Usuario)
        {
            try
            {
                string Query = "Delete PLAN_MANTENIMIENTO Where IDPlanMantenimiento = @IDPLANMANTENIMIENTO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDPLANMANTENIMIENTO\": \"" + IDPlanMantenimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDPLANMANTENIMIENTO", SqlDbType.SmallInt).Value = IDPlanMantenimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Pool
        public Data SelectPoolBySan(string San, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDPool, San as IDSan, Pool, Descripcion from POOL, SAN Where POOL.IDSan = SAN.IDSan And San Like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + San + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = San;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectPool(string Pool, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDPool, San as IDSan, Pool, Descripcion from POOL, SAN Where POOL.IDSan = SAN.IDSan And Pool Like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Pool + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Pool;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertPool(int IDSan, string Pool, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Insert into POOL Values (@IDSAN, @POOL, @DESCRIPCION)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSAN\": \"" + IDSan + "\", \"POOL\": \"" + Pool + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.Parameters.Add("POOL", SqlDbType.NVarChar, 50).Value = Pool;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdatePool(int IDPool, int IDSan, string Pool, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Update POOL Set IDSan = @IDSAN, Pool = @POOL, Descripcion = @DESCRIPCION Where IDPool = @IDPOOL";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDPOOL\": \"" + IDPool + "\", \"IDSAN\": \"" + IDSan + "\", \"POOL\": \"" + Pool + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDPOOL", SqlDbType.SmallInt).Value = IDPool;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.Parameters.Add("POOL", SqlDbType.NVarChar, 50).Value = Pool;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeletePool(int IDPool, string Usuario)
        {
            try
            {
                string Query = "Delete POOL Where IDPool = @IDPOOL";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDPOOL\": \"" + IDPool + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDPOOL", SqlDbType.SmallInt).Value = IDPool;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Procesador
        public Data SelectProcesador(string Like, string Usuario)
        {
            string Query = "Select IDProcesador, Marca, Procesador from PROCESADOR, MARCA Where PROCESADOR.IDMarca = MARCA.IDMarca And Procesador like @LIKE";
            //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Like + "\"");
            Data datos = new Data();
            try
            {
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Like;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertProcesador(int IDMarca, string Procesador, string Usuario)
        {
            try
            {
                string Query = "Insert into PROCESADOR Values (@IDMARCA, @PROCESADOR)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDMARCA\": \"" + IDMarca + "\", \"PROCESADOR\": \"" + Procesador + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDMARCA", SqlDbType.SmallInt).Value = IDMarca;
                Comando.Parameters.Add("PROCESADOR", SqlDbType.NVarChar, 50).Value = Procesador;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateProcesador(int ID, int IDMarca, string Procesador, string Usuario)
        {
            try
            {
                string Query = "Update PROCESADOR Set IDMarca = @IDMARCA, Procesador = @PROCESADOR Where IDProcesador = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"IDMARCA\": \"" + IDMarca + "\", \"PROCESADOR\": \"" + Procesador + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("IDMARCA", SqlDbType.SmallInt).Value = IDMarca;
                Comando.Parameters.Add("PROCESADOR", SqlDbType.NVarChar, 50).Value = Procesador;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteProcesador(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete PROCESADOR Where IDProcesador = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Proveedor
        public Data SelectProveedor(string NombreProveedor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from PROVEEDOR Where Nombre like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + NombreProveedor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 100).Value = NombreProveedor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertProveedor(string Proveedor, string Usuario)
        {
            try
            {
                string Query = "Insert into PROVEEDOR Values (@NOMBRE)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"NOMBRE\": \"" + Proveedor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 100).Value = Proveedor;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateProveedor(int ID, string Proveedor, string Usuario)
        {
            try
            {
                string Query = "Update PROVEEDOR Set Nombre = @NOMBRE Where IDProveedor = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"NOMBRE\": \"" + Proveedor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 100).Value = Proveedor;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteProveedor(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete PROVEEDOR Where IDProveedor = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Rack
        public Data SelectRack(string Rack, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from RACK Where Rack like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Rack + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Rack;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertRack(string Rack, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Insert into RACK Values (@RACK, @DESCRIPCION)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"RACK\": \"" + Rack + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("RACK", SqlDbType.NVarChar, 50).Value = Rack;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateRack(int ID, string Rack, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Update RACK Set Rack = @RACK, Descripcion = @DESCRIPCION Where IDRack = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"RACK\": \"" + Rack + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("RACK", SqlDbType.NVarChar, 50).Value = Rack;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteRack(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete RACK Where IDRack = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Redundancia
        public Data SelectRedundancia(int IDServidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select REDUNDANCIA.IDServidor, IDRedundancia, SERVIDOR.Servidor as Redundancia from REDUNDANCIA, SERVIDOR Where REDUNDANCIA.IDRedundancia = SERVIDOR.IDServidor And REDUNDANCIA.IDServidor = @IDSERVIDOR";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertRedundancia(int IDServidor, int IDRedundancia, string Usuario)
        {
            try
            {
                string Query = "Insert into REDUNDANCIA Values (@IDSERVIDOR, @IDREDUNDANCIA)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDREDUNDANCIA\": \"" + IDRedundancia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDREDUNDANCIA", SqlDbType.SmallInt).Value = IDRedundancia;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateRedundancia(int IDServidor, int IDRedundancia, string Usuario)
        {
            try
            {
                string Query = "Update REDUNDANCIA Set IDRedundancia = @IDREDUNDANCIA Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDREDUNDANCIA\": \"" + IDRedundancia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDREDUNDANCIA", SqlDbType.SmallInt).Value = IDRedundancia;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteRedundancia(int IDServidor, int IDRedundancia, string Usuario)
        {
            try
            {
                string Query = "Delete REDUNDANCIA Where IDRedundancia = @IDREDUNDANCIA And IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDREDUNDANCIA\": \"" + IDRedundancia + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDREDUNDANCIA", SqlDbType.SmallInt).Value = IDRedundancia;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region SAN
        public Data SelectSAN(string San, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDSan, San from SAN Where San like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + San + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = San;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertSAN(string San, string Usuario)
        {
            try
            {
                string Query = "Insert into SAN Values (@SAN)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"SAN\": \"" + San + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("SAN", SqlDbType.NVarChar, 50).Value = San;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateSAN(int IDSan, string San, string Usuario)
        {
            try
            {
                string Query = "Update SAN Set San = @SAN Where IDSan = @IDSAN";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSAN\": \"" + IDSan + "\", \"SAN\": \"" + San + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.Parameters.Add("SAN", SqlDbType.NVarChar, 50).Value = San;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteSAN(int IDSan, string Usuario)
        {
            try
            {
                string Query = "Delete SAN Where IDSan = @IDSAN";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSAN\": \"" + IDSan + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Servidor
        public Data SelectServidor4Redundancia(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDServidor, Servidor from SERVIDOR Where Servidor <> @SERVIDOR";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"SERVIDOR\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("SERVIDOR", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectServidor(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDServidor, STATUS.Status as IDStatus, SISTEMA_OPERATIVO.SistemaOperativo as IDSistemaOperativo, AMBIENTE.Ambiente as IDAmbiente, TIPO.Tipo as IDTipo, Servidor, Descripcion, NoProcesadores, Ram, NoEthernet, TeamEthernet, FechaParches  from SERVIDOR, STATUS, SISTEMA_OPERATIVO, AMBIENTE, TIPO Where SERVIDOR.IDStatus = STATUS.IDStatus And SERVIDOR.IDSistemaOperativo = SISTEMA_OPERATIVO.IDSistemaOperativo And SERVIDOR.IDAmbiente = AMBIENTE.IDAmbiente And SERVIDOR.IDTipo = TIPO.IDTipo And Servidor like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertServidor(int IDStatus, int IDSistemaOperativo, int IDAmbiente, int IDTipo, string Servidor, string Descripcion, int NoProcesadores, int Ram, int NoEthernet, string TeamEthernet, DateTime FechaParches, string Usuario)
        {
            try
            {
                string Query = "Insert into SERVIDOR Values (@IDSTATUS, @IDSISTEMAOPERATIVO, @IDAMBIENTE, @IDTIPO, @SERVIDOR, @DESCRIPCION, @NOPROCESADORES, @RAM, @NOETHERNET, @TEAMETHERNET, @FECHAPARCHES)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSTATUS\": \"" + IDStatus + "\", \"IDSISTEMAOPERATIVO\": \"" + IDSistemaOperativo + "\", \"IDAMBIENTE\": \"" + IDAmbiente + "\", \"IDTIPO\": \"" + IDTipo + "\", \"SERVIDOR\": \"" + Servidor + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"NOPROCESADORES\": \"" + NoProcesadores + "\", \"RAM\": \"" + Ram + "\", \"NOETHERNET\": \"" + NoEthernet + "\", \"TEAMETHERNET\": \"" + TeamEthernet + "\", \"FECHAPARCHES\": \"" + FechaParches + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSTATUS", SqlDbType.SmallInt).Value = IDStatus;
                Comando.Parameters.Add("IDSISTEMAOPERATIVO", SqlDbType.SmallInt).Value = IDSistemaOperativo;
                Comando.Parameters.Add("IDAMBIENTE", SqlDbType.SmallInt).Value = IDAmbiente;
                Comando.Parameters.Add("IDTIPO", SqlDbType.SmallInt).Value = IDTipo;
                Comando.Parameters.Add("SERVIDOR", SqlDbType.NVarChar, 50).Value = Servidor;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("NOPROCESADORES", SqlDbType.SmallInt).Value = NoProcesadores;
                Comando.Parameters.Add("RAM", SqlDbType.SmallInt).Value = Ram;
                Comando.Parameters.Add("NOETHERNET", SqlDbType.SmallInt).Value = NoEthernet;
                Comando.Parameters.Add("TEAMETHERNET", SqlDbType.Char, 2).Value = TeamEthernet;
                Comando.Parameters.Add("FECHAPARCHES", SqlDbType.DateTime).Value = FechaParches;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateServidor(int IDServidor, int IDStatus, int IDSistemaOperativo, int IDAmbiente, int IDTipo, string Servidor, string Descripcion, int NoProcesadores, int Ram, int NoEthernet, string TeamEthernet, DateTime FechaParches, string Usuario)
        {
            try
            {
                string Query = "Update SERVIDOR Set IDStatus = @IDSTATUS, IDSistemaOperativo = @IDSISTEMAOPERATIVO, IDAmbiente = @IDAMBIENTE, IDTipo = @IDTIPO, Servidor = @SERVIDOR, Descripcion = @DESCRIPCION, NoProcesadores = @NOPROCESADORES, Ram = @RAM, NoEthernet = @NOETHERNET, TeamEthernet = @TEAMETHERNET, FechaParches = @FECHAPARCHES Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDSTATUS\": \"" + IDStatus + "\", \"IDSISTEMAOPERATIVO\": \"" + IDSistemaOperativo + "\", \"IDAMBIENTE\": \"" + IDAmbiente + "\", \"IDTIPO\": \"" + IDTipo + "\", \"SERVIDOR\": \"" + Servidor + "\", \"DESCRIPCION\": \"" + Descripcion + "\", \"NOPROCESADORES\": \"" + NoProcesadores + "\", \"RAM\": \"" + Ram + "\", \"NOETHERNET\": \"" + NoEthernet + "\", \"TEAMETHERNET\": \"" + TeamEthernet + "\", \"FECHAPARCHES\": \"" + FechaParches + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDSTATUS", SqlDbType.SmallInt).Value = IDStatus;
                Comando.Parameters.Add("IDSISTEMAOPERATIVO", SqlDbType.SmallInt).Value = IDSistemaOperativo;
                Comando.Parameters.Add("IDAMBIENTE", SqlDbType.SmallInt).Value = IDAmbiente;
                Comando.Parameters.Add("IDTIPO", SqlDbType.SmallInt).Value = IDTipo;
                Comando.Parameters.Add("SERVIDOR", SqlDbType.NVarChar, 50).Value = Servidor;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.Parameters.Add("NOPROCESADORES", SqlDbType.SmallInt).Value = NoProcesadores;
                Comando.Parameters.Add("RAM", SqlDbType.SmallInt).Value = Ram;
                Comando.Parameters.Add("NOETHERNET", SqlDbType.SmallInt).Value = NoEthernet;
                Comando.Parameters.Add("TEAMETHERNET", SqlDbType.Char, 2).Value = TeamEthernet;
                Comando.Parameters.Add("FECHAPARCHES", SqlDbType.DateTime).Value = FechaParches;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteServidor(int IDServidor, string Usuario)
        {
            try
            {
                string Query = "Delete SERVIDOR Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region ServidorFisico
        public Data SelectServidorFisico(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select SERVIDOR_FISICO.IDServidor, SERVIDOR.Servidor, MARCA.Marca, MODELO.Modelo, EDIFICIO.Edificio, Procesador, ServiceTag, FuentesRedundantes, FechaFinGarantia, FechaCompra, RACK.Rack, OU  from SERVIDOR_FISICO, SERVIDOR, MARCA, MODELO, EDIFICIO, RACK Where SERVIDOR_FISICO.IDServidor = SERVIDOR.IDServidor And SERVIDOR_FISICO.IDRack = RACK.IDRack And SERVIDOR_FISICO.IDModelo = MODELO.IDModelo And SERVIDOR_FISICO.IDEdificio = EDIFICIO.IDEdificio And MODELO.IDMarca = MARCA.IDMarca And SERVIDOR.Servidor like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertServidorFisico(int IDServidor, int IDModelo, int IDEdificio, string Procesador, string ServiceTag, string FuentesRedundantes, DateTime FechaFinGarantia, DateTime FechaCompra, int IDRack, string OU, string Usuario)
        {
            try
            {
                string Query = "Insert into SERVIDOR_FISICO Values (@IDSERVIDOR, @IDMODELO, @IDEDIFICIO, @PROCESADOR, @SERVICETAG, @FUENTESREDUNDANTE, @FECHAFINGARANTIA, @FECHACOMPRA, @IDRACK, @OU)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDMODELO\": \"" + IDModelo + "\", \"IDEDIFICIO\": \"" + IDEdificio + "\", \"PROCESADOR\": \"" + Procesador + "\", \"SERVICETAG\": \"" + ServiceTag + "\", \"FUENTESREDUNDANTE\": \"" + FuentesRedundantes + "\", \"FECHAFINGARANTIA\": \"" + FechaFinGarantia + "\", \"FECHACOMPRA\": \"" + FechaCompra + "\", \"IDRACK\": \"" + IDRack + "\", \"OU\": \"" + OU + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDMODELO", SqlDbType.SmallInt).Value = IDModelo;
                Comando.Parameters.Add("IDEDIFICIO", SqlDbType.SmallInt).Value = IDEdificio;
                Comando.Parameters.Add("PROCESADOR", SqlDbType.NVarChar, 50).Value = Procesador;
                Comando.Parameters.Add("SERVICETAG", SqlDbType.NVarChar, 50).Value = ServiceTag;
                Comando.Parameters.Add("FUENTESREDUNDANTE", SqlDbType.Char, 2).Value = FuentesRedundantes;
                Comando.Parameters.Add("FECHAFINGARANTIA", SqlDbType.DateTime).Value = FechaFinGarantia;
                Comando.Parameters.Add("FECHACOMPRA", SqlDbType.DateTime).Value = FechaCompra;
                Comando.Parameters.Add("IDRACK", SqlDbType.SmallInt).Value = IDRack;
                Comando.Parameters.Add("OU", SqlDbType.NVarChar, 10).Value = OU;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateservidorFisico(int IDServidor, int IDModelo, int IDEdificio, string Procesador, string ServiceTag, string FuentesRedundantes, DateTime FechaFinGarantia, DateTime FechaCompra, int IDRack, string OU, string Usuario)
        {
            try
            {
                string Query = "Update SERVIDOR_FISICO Set IDModelo = @IDMODELO, IDEdificio = @IDEDIFICIO, Procesador = @PROCESADOR, ServiceTag = @SERVICETAG, FuentesRedundantes = @FUENTESREDUNDATES, FechaFinGarantia = @FECHAFINGARANTIA, FechaCompra = @FECHACOMPRA, IDRack = @IDRACK, OU=@OU Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDMODELO\": \"" + IDModelo + "\", \"IDEDIFICIO\": \"" + IDEdificio + "\", \"PROCESADOR\": \"" + Procesador + "\", \"SERVICETAG\": \"" + ServiceTag + "\", \"FUENTESREDUNDANTE\": \"" + FuentesRedundantes + "\", \"FECHAFINGARANTIA\": \"" + FechaFinGarantia + "\", \"FECHACOMPRA\": \"" + FechaCompra + "\", \"IDRACK\": \"" + IDRack + "\", \"OU\": \"" + OU + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDMODELO", SqlDbType.SmallInt).Value = IDModelo;
                Comando.Parameters.Add("IDEDIFICIO", SqlDbType.SmallInt).Value = IDEdificio;
                Comando.Parameters.Add("PROCESADOR", SqlDbType.NVarChar, 50).Value = Procesador;
                Comando.Parameters.Add("SERVICETAG", SqlDbType.NVarChar, 50).Value = ServiceTag;
                Comando.Parameters.Add("FUENTESREDUNDATES", SqlDbType.Char, 2).Value = FuentesRedundantes;
                Comando.Parameters.Add("FECHAFINGARANTIA", SqlDbType.DateTime).Value = FechaFinGarantia;
                Comando.Parameters.Add("FECHACOMPRA", SqlDbType.DateTime).Value = FechaCompra;
                Comando.Parameters.Add("IDRACK", SqlDbType.SmallInt).Value = IDRack;
                Comando.Parameters.Add("OU", SqlDbType.NVarChar, 10).Value = OU;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteServidorFisico(int IDServidor, string Usuario)
        {
            try
            {
                string Query = "EXEC DELETE_SERVIDOR_FISICO @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region ServidorSolucion
        public Data SelectServidorSolucion(string Solucion, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select SERVIDOR.Servidor, SOLUCION.Solucion from SERVIDOR_SOLUCION, SERVIDOR, SOLUCION Where SERVIDOR_SOLUCION.IDServidor = SERVIDOR.IDServidor And SERVIDOR_SOLUCION.IDSolucion = SOLUCION.IDSolucion And SOLUCION.Solucion like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Solucion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Solucion;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertServidorSolucion(int IDServidor, int IDSolucion, string Usuario)
        {
            try
            {
                string Query = "Insert into SERVIDOR_SOLUCION Values (@IDSERVIDOR, @IDSOLUCION)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDSOLUCION\": \"" + IDSolucion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDSOLUCION", SqlDbType.SmallInt).Value = IDSolucion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateServidorSolucion(int IDServidor, int IDSolucion, string Usuario)
        {
            try
            {
                string Query = "Update SERVIDOR_SOLUCION Set IDSolucion = @IDSOLUCION Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDSOLUCION\": \"" + IDSolucion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDSOLUCION", SqlDbType.SmallInt).Value = IDSolucion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteServidorSolucion(int IDServidor, int IDSolucion, string Usuario)
        {
            try
            {
                string Query = "Delete SERVIDOR_SOLUCION Where IDServidor = @IDSERVIDOR And IDSolucion = @IDSOLUCION";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDSOLUCION\": \"" + IDSolucion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDSOLUCION", SqlDbType.SmallInt).Value = IDSolucion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region ServidorUsuario
        public Data SelectServidorUsuario(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select SERVIDOR.IDServidor, USUARIO.Usuario from SERVIDOR_USUARIO, SERVIDOR, USUARIO Where SERVIDOR_USUARIO.IDServidor = SERVIDOR.IDServidor And SERVIDOR_USUARIO.IDUsuario = USUARIO.IDUsuario And SERVIDOR.Servidor like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertServidorUsuario(int IDServidor, int IDUsuario, string Usuario)
        {
            try
            {
                string Query = "Insert into SERVIDOR_USUARIO Values (@IDSERVIDOR, @IDUSUARIO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateServidorUsuario(int IDServidor, int IDUsuario, string Usuario)
        {
            try
            {
                string Query = "Update SERVIDOR_USUARIO Set IDUsuario = @IDUSUARIO Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteServidorUsuario(int IDServidor, int IDUsuario, string Usuario)
        {
            try
            {
                string Query = "Delete SERVIDOR_USUARIO Where IDServidor = @IDSERVIDOR And IDUsuario = @IDUSUARIO";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region ServidorVirtual
        public Data SelectServidorVirtual(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select SERVIDOR.IDServidor, SERVIDOR.Servidor, CLUSTER.Cluster from SERVIDOR_VIRTUAL, SERVIDOR, CLUSTER Where SERVIDOR_VIRTUAL.IDServidor = SERVIDOR.IDServidor And SERVIDOR_VIRTUAL.IDCluster = CLUSTER.IDCluster And SERVIDOR.Servidor like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertServidorVirtual(int IDServidor, int IDCluster, string Usuario)
        {
            try
            {
                string Query = "Insert into SERVIDOR_VIRTUAL Values (@IDSERVIDOR, @IDCLUSTER)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDCLUSTER\": \"" + IDCluster + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDCLUSTER", SqlDbType.SmallInt).Value = IDCluster;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateServidorVirtual(int IDServidor, int IDCluster, string Usuario)
        {
            try
            {
                string Query = "Update SERVIDOR_VIRTUAL Set IDCluster = @IDCLUSTER Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDCLUSTER\": \"" + IDCluster + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDCLUSTER", SqlDbType.SmallInt).Value = IDCluster;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteServidorVirtual(int IDServidor, string Usuario)
        {
            try
            { 
                string Query = "EXEC DELETE_SERVIDOR_VIRTUAL @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region ServidorWebservice
        public Data SelectServidorWebservice(string Servidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select SERVIDOR.Servidor, WEBSERVICE.Nombre from SERVIDOR_WEBSERVICE, SERVIDOR, WEBSERVICE where SERVIDOR_WEBSERVICE.IDServidor = SERVIDOR.IDServidor And SERVIDOR_WEBSERVICE.IDWebservice = WEBSERVICE.IDWebservice And SERVIDOR.Servidor like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Servidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Servidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertServidorWebservice(int IDServidor, int IDWebservice, string Usuario)
        {
            try
            {
                string Query = "Insert into SERVIDOR_WEBSERVICE Values (@IDSERVIDOR, @IDWEBSERVICE)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDWEBSERVICE\": \"" + IDWebservice + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDWEBSERVICE", SqlDbType.SmallInt).Value = IDWebservice;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateServidorWebservice(int IDServidor, int IDWebservice, string Usuario)
        {
            try
            {
                string Query = "Update SERVIDOR_WEBSERVICE Set IDWebservice = @IDWEBSERVICE Where IDServidor = @IDSERVIDOR";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDWEBSERVICE\": \"" + IDWebservice + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDWEBSERVICE", SqlDbType.SmallInt).Value = IDWebservice;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteServidorWebservice(int IDServidor, int IDWebservice, string Usuario)
        {
            try
            {
                string Query = "Delete SERVIDOR_WEBSERVICE Where IDServidor = @IDSERVIDOR And IDWebservice = @IDWEBSERVICE";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"IDWEBSERVICE\": \"" + IDWebservice + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("IDWEBSERVICE", SqlDbType.SmallInt).Value = IDWebservice;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region SistemaOperativo
        public Data SelectSistemaOperativo(int Proveedor, string SistemaOperativo, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDSistemaOperativo, SistemaOperativo, Version, Fabricante, Licencia, PROVEEDOR.Nombre As PROVEEDOR, FechaVencimiento from SISTEMA_OPERATIVO, PROVEEDOR Where SISTEMA_OPERATIVO.IDProveedor = PROVEEDOR.IDProveedor";
                if (Proveedor > 0)
                {
                    Query += " And SISTEMA_OPERATIVO.IDProveedor = @PROVEEDOR";
                }
                Query += " And SistemaOperativo like @SISTEMAOPERATIVO";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"PROVEEDOR\": \"" + Proveedor + "\", \"SISTEMAOPERATIVO\": \"" + SistemaOperativo + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt).Value = Proveedor;
                Comando.Parameters.Add("SISTEMAOPERATIVO", SqlDbType.NVarChar, 100).Value = SistemaOperativo;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertSistemaOperativo(string SistemaOperativo, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario)
        {
            try
            {
                string Query = "Insert into SISTEMA_OPERATIVO Values(@SISTEMAOPERATIVO, @VERSION, @FABRICANTE, @LICENCIA, @PROVEEDOR, @FECHAVENCIMIENTO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"SISTEMAOPERATIVO\": \"" + SistemaOperativo + "\", \"VERSION\": \"" + Version + "\", \"FABRICANTE\": \"" + Fabricante + "\", \"LICENCIA\": \"" + Licencia + "\", \"PROVEEDOR\": \"" + Proveedor + "\", \"FECHAVENCIMIENTO\": \"" + FechaVencimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("SISTEMAOPERATIVO", SqlDbType.NVarChar, 100).Value = SistemaOperativo;
                Comando.Parameters.Add("VERSION", SqlDbType.NVarChar, 50).Value = Version;
                Comando.Parameters.Add("FABRICANTE", SqlDbType.NVarChar, 100).Value = Fabricante;
                Comando.Parameters.Add("LICENCIA", SqlDbType.NVarChar, 100).Value = Licencia;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt).Value = Proveedor;
                Comando.Parameters.Add("FECHAVENCIMIENTO", SqlDbType.DateTime).Value = FechaVencimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateSistemaOperativo(int ID, string SistemaOperativo, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario)
        {
            try
            {
                string Query = "Update SISTEMA_OPERATIVO Set SistemaOperativo = @SISTEMAOPERATIVO, Version = @VERSION, Fabricante = @FABRICANTE, Licencia = @LICENCIA, IDProveedor = @PROVEEDOR, FechaVencimiento = @FECHAVENCIMIENTO Where IDSistemaOperativo = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"SISTEMAOPERATIVO\": \"" + SistemaOperativo + "\", \"VERSION\": \"" + Version + "\", \"FABRICANTE\": \"" + Fabricante + "\", \"LICENCIA\": \"" + Licencia + "\", \"PROVEEDOR\": \"" + Proveedor + "\", \"FECHAVENCIMIENTO\": \"" + FechaVencimiento + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("SISTEMAOPERATIVO", SqlDbType.NVarChar, 100).Value = SistemaOperativo;
                Comando.Parameters.Add("VERSION", SqlDbType.NVarChar, 50).Value = Version;
                Comando.Parameters.Add("FABRICANTE", SqlDbType.NVarChar, 100).Value = Fabricante;
                Comando.Parameters.Add("LICENCIA", SqlDbType.NVarChar, 100).Value = Licencia;
                Comando.Parameters.Add("PROVEEDOR", SqlDbType.SmallInt).Value = Proveedor;
                Comando.Parameters.Add("FECHAVENCIMIENTO", SqlDbType.DateTime).Value = FechaVencimiento;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteSistemaOperativo(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete SISTEMA_OPERATIVO Where IDSistemaOperativo = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Solucion
        public Data SelectSolucion(string Solucion, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDSolucion, Solucion, Descripcion from SOLUCION Where Solucion like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Solucion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Solucion;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertSolucion(string Solucion, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Insert into SOLUCION Values (@SOLUCION, @DESCRIPCION)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"SOLUCION\": \"" + Solucion + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("SOLUCION", SqlDbType.NVarChar, 50).Value = Solucion;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateSolucion(int IDSolucion, string Solucion, string Descripcion, string Usuario)
        {
            try
            {
                string Query = "Update SOLUCION Set Solucion = @SOLUCION, Descripcion = @DESCRIPCION Where IDSolucion = @IDSOLUCION";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSOLUCION\": \"" + IDSolucion + "\", \"SOLUCION\": \"" + Solucion + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSOLUCION", SqlDbType.SmallInt).Value = IDSolucion;
                Comando.Parameters.Add("SOLUCION", SqlDbType.NVarChar, 50).Value = Solucion;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteSolucion(int IDSolucion, string Usuario)
        {
            try
            {
                string Query = "Delete SOLUCION Where IDSolucion = @IDSOLUCION";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSOLUCION\": \"" + IDSolucion + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSOLUCION", SqlDbType.SmallInt).Value = IDSolucion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Status
        public Data SelectStatus(string Status, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from STATUS Where Status Like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Status + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Status;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertStatus(string Status, string Usuario)
        {
            try
            {
                string Query = "Insert into STATUS Values (@STATUS)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"STATUS\": \"" + Status + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("STATUS", SqlDbType.NVarChar, 50).Value = Status;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateStatus(int ID, string Status, string Usuario)
        {
            try
            {
                string Query = "Update STATUS Set Status = @STATUS Where IDStatus = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"STATUS\": \"" + Status + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("STATUS", SqlDbType.NVarChar, 50).Value = Status;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteStatus(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete STATUS Where IDStatus = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Tipo
        public Data SelectTipo(string Tipo, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from TIPO Where Tipo like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Tipo + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Tipo;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertTipo(string Tipo, string Usuario)
        {
            try
            {
                string Query = "Insert into TIPO Values (@TIPO)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"TIPO\": \"" + Tipo + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("TIPO", SqlDbType.NVarChar, 50).Value = Tipo;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateTipo(int ID, string Tipo, string Usuario)
        {
            try
            {
                string Query = "Update TIPO Set Tipo = @TIPO Where IDTipo = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\", \"TIPO\": \"" + Tipo + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.Parameters.Add("TIPO", SqlDbType.NVarChar, 50).Value = Tipo;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteTipo(int ID, string Usuario)
        {
            try
            {
                string Query = "Delete TIPO Where IDTipo = @ID";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"ID\": \"" + ID + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("ID", SqlDbType.SmallInt).Value = ID;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region UrlAdministracionSAN
        public Data SelectUrlAdministracionSAN(string San, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDUrl, SAN.San, Url from URL_ADMINISTRACION_SAN, SAN Where URL_ADMINISTRACION_SAN.IDSan = SAN.IDSan And SAN.San like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + San + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = San;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertUrlAdministracionSAN(int IDSan, string Url, string Usuario)
        {
            try
            {
                string Query = "Insert into URL_ADMINISTRACION_SAN Values (@IDSAN, @URL)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSAN\": \"" + IDSan + "\", \"URL\": \"" + Url + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.Parameters.Add("URL", SqlDbType.NVarChar, 250).Value = Url;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateUrlAdministracionSAN(int IDUrl, int IDSan, string Url, string Usuario)
        {
            try
            {
                string Query = "Update URL_ADMINISTRACION_SAN Set IDSan = @IDSAN, Url = @URL Where IDUrl = @IDURL";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDURL\": \"" + IDUrl + "\", \"IDSAN\": \"" + IDSan + "\", \"URL\": \"" + Url + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDURL", SqlDbType.SmallInt).Value = IDUrl;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.Parameters.Add("URL", SqlDbType.NVarChar, 250).Value = Url;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteUrlAdministracionSAN(int IDUrl, string Usuario)
        {
            try
            {
                string Query = "Delete URL_ADMINISTRACION_SAN Where IDUrl = @IDURL";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDURL\": \"" + IDUrl + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDURL", SqlDbType.SmallInt).Value = IDUrl;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Usuario
        public Data SelectUsuarioIDServer(int IDServidor, string UsuarioLog)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDUsuario, IDServidor, Dominio, Usuario, Password, Descripcion from USUARIO Where IDServidor = @IDSERVIDOR";
                //InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Select IDUsuario, IDServidor, Dominio, Usuario, CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE('Gran Contaminador Internacional Seguro', Password)) As Password, Descripcion from USUARIO Where IDServidor = @IDSERVIDOR";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectUsuario(string Usuario, string UsuarioLog)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDUsuario, USUARIO.IDServidor, SERVIDOR.Servidor, Dominio, Usuario, Password, USUARIO.Descripcion from USUARIO, SERVIDOR Where USUARIO.IDServidor = SERVIDOR.IDServidor And Usuario like @LIKE";
                //InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + Usuario + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Select IDUsuario, USUARIO.IDServidor, SERVIDOR.Servidor, Dominio, Usuario, CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE('Gran Contaminador Internacional Seguro', Password)) As Password, USUARIO.Descripcion from USUARIO, SERVIDOR Where USUARIO.IDServidor = SERVIDOR.IDServidor And Usuario like @LIKE";
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = Usuario;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertUsuario(int IDServidor, string Dominio, string Usuario, string Password, string Descripcion, string UsuarioLog)
        {
            try
            {
                string Query = "Insert into USUARIO Values (@IDSERVIDOR, @DOMINIO, @USUARIO, @PASSWORD, @DESCRIPCION)";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"DOMINIO\": \"" + Dominio + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Insert into USUARIO Values (@IDSERVIDOR, @DOMINIO, @USUARIO, ENCRYPTBYPASSPHRASE('Gran Contaminador Internacional Seguro', @PASSWORD), @DESCRIPCION)";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("DOMINIO", SqlDbType.NVarChar, 50).Value = Dominio;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateUsuario(int IDUsuario, int IDServidor, string Dominio, string Usuario, string Password, string Descripcion, string UsuarioLog)
        {
            try
            {
                string Query = "Update USUARIO Set Dominio = @DOMINIO, IDServidor = @IDSERVIDOR, Usuario = @USUARIO, Password = @PASSWORD, Descripcion = @DESCRIPCION Where IDUsuario = @IDUSUARIO";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDUSUARIO\": \"" + IDUsuario + "\", \"IDSERVIDOR\": \"" + IDServidor + "\", \"DOMINIO\": \"" + Dominio + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\", \"DESCRIPCION\": \"" + Descripcion + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Update USUARIO Set Dominio = @DOMINIO, IDServidor = @IDSERVIDOR, Usuario = @USUARIO, Password = ENCRYPTBYPASSPHRASE('Gran Contaminador Internacional Seguro', @PASSWORD), Descripcion = @DESCRIPCION Where IDUsuario = @IDUSUARIO";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("DOMINIO", SqlDbType.NVarChar, 50).Value = Dominio;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.Parameters.Add("DESCRIPCION", SqlDbType.NVarChar, 250).Value = Descripcion;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteUsuario(int IDUsuario, string UsuarioLog)
        {
            try
            {
                string Query = "Delete USUARIO Where IDUsuario = @IDUSUARIO";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region UsuarioInstancia
        public Data SelectUsuarioInstancia(string InstanciaAplicacion, string UsuarioLog)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDUsuario, INSTANCIA_APLICACION.Instancia, Usuario, Password from USUARIO_INSTANCIA, INSTANCIA_APLICACION Where USUARIO_INSTANCIA.IDInstancia = INSTANCIA_APLICACION.IDInstancia And INSTANCIA_APLICACION.Instancia like @LIKE";
                //InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + InstanciaAplicacion + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Select IDUsuario, INSTANCIA_APLICACION.Instancia, Usuario, CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE('Gordo Calculador Intentando Solucionar', Password)) as Password from USUARIO_INSTANCIA, INSTANCIA_APLICACION Where USUARIO_INSTANCIA.IDInstancia = INSTANCIA_APLICACION.IDInstancia And INSTANCIA_APLICACION.Instancia like @LIKE";
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 100).Value = InstanciaAplicacion;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertUsuarioInstancia(int IDInstancia, string Usuario, string Password, string UsuarioLog)
        {
            try
            {
                string Query = "Insert into USUARIO_INSTANCIA Values (@IDINSTANCIA, @USUARIO, @PASSWORD)";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDINSTANCIA\": \"" + IDInstancia + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Insert into USUARIO_INSTANCIA Values (@IDINSTANCIA, @USUARIO, ENCRYPTBYPASSPHRASE('Gordo Calculador Intentando Solucionar', @PASSWORD))";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateUsuarioInstancia(int IDUsuario, int IDInstancia, string Usuario, string Password, string UsuarioLog)
        {
            try
            {
                string Query = "Update USUARIO_INSTANCIA Set IDInstancia = @IDINSTANCIA, Usuario = @USUARIO, Password = @PASSWORD Where IDUsuario = @IDUSUARIO";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDUSUARIO\": \"" + IDUsuario + "\", \"IDINSTANCIA\": \"" + IDInstancia + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Update USUARIO_INSTANCIA Set IDInstancia = @IDINSTANCIA, Usuario = @USUARIO, Password = ENCRYPTBYPASSPHRASE('Gordo Calculador Intentando Solucionar', @PASSWORD) Where IDUsuario = @IDUSUARIO";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.Parameters.Add("IDINSTANCIA", SqlDbType.SmallInt).Value = IDInstancia;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteUsuarioInstancia(int IDUsuario, string UsuarioLog)
        {
            try
            {
                string Query = "Delete USUARIO_INSTANCIA Where IDUsuario = @IDUSUARIO";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region UsuarioSAN
        public Data SelectUsuarioSAN(string San, string UsuarioLog)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDUsuario, SAN.San, Usuario, Password from USUARIO_SAN, SAN Where USUARIO_SAN.IDSan = SAN.IDSan And SAN.San like @LIKE";
                //InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + San + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Select IDUsuario, SAN.San, Usuario, CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE('Gobierno Corrupto Inicia Sanciones', Password)) as Password from USUARIO_SAN, SAN Where USUARIO_SAN.IDSan = SAN.IDSan And SAN.San like @LIKE";
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = San;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertUsuarioSAN(int IDSan, string Usuario, string Password, string UsuarioLog)
        {
            try
            {
                string Query = "Insert into USUARIO_SAN Values (@IDSAN, @USUARIO, @PASSWORD)";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDSAN\": \"" + IDSan + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Insert into USUARIO_SAN Values (@IDSAN, @USUARIO, ENCRYPTBYPASSPHRASE('Gobierno Corrupto Inicia Sanciones', @PASSWORD))";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateUsuarioSAN(int IDUsuario, int IDSan, string Usuario, string Password, string UsuarioLog)
        {
            try
            {
                string Query = "Update USUARIO_SAN Set IDSan = @IDSAN, Usuario = @USUARIO, Password = @PASSWORD Where IDUsuario = @IDUSUARIO";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDUSUARIO\": \"" + IDUsuario + "\", \"IDSAN\": \"" + IDSan + "\", \"USUARIO\": \"" + Usuario + "\", \"PASSWORD\": \"" + Password + "\"");
                Open();
                Comando.Parameters.Clear();
                //Comando.CommandText = "Update USUARIO_SAN Set IDSan = @IDSAN, Usuario = @USUARIO, Password = ENCRYPTBYPASSPHRASE('Gobierno Corrupto Inicia Sanciones', @PASSWORD) Where IDUsuario = @IDUSUARIO";
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.Parameters.Add("IDSAN", SqlDbType.SmallInt).Value = IDSan;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Comando.Parameters.Add("PASSWORD", SqlDbType.NVarChar, 4000).Value = Password;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteUsuarioSAN(int IDUsuario, string UsuarioLog)
        {
            try
            {
                string Query = "Delete USUARIO_SAN Where IDUsuario = @IDUSUARIO";
                InsertLog(UsuarioLog, "\"Consulta\": \"" + Query + "\"", "\"IDUSUARIO\": \"" + IDUsuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDUSUARIO", SqlDbType.SmallInt).Value = IDUsuario;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region WebService
        public Data SelectWebServiceIDServer(int IDServidor, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select * from WEBSERVICE Where IDServidor = @IDSERVIDOR";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public Data SelectWebService(string NombreWebService, string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select IDWebservice, WEBSERVICE.IDServidor, SERVIDOR.Servidor, Nombre, Carpeta, URL, PublicadoInternet, URLExterna from WEBSERVICE, SERVIDOR Where WEBSERVICE.IDServidor = SERVIDOR.IDServidor And Nombre Like @LIKE";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"LIKE\": \"" + NombreWebService + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("LIKE", SqlDbType.NVarChar, 50).Value = NombreWebService;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }

        public bool InsertWebService(int IDServidor, string NombreWebService, string Carpeta, string Url, string PublicadoInternet, string UrlExterna, string Usuario)
        {
            try
            {
                string Query = "Insert into WEBSERVICE Values (@IDSERVIDOR, @NOMBRE, @CARPETA, @URL, @PUBLICADOINTERNET, @URLEXTERNA)";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDSERVIDOR\": \"" + IDServidor + "\", \"NOMBRE\": \"" + NombreWebService + "\", \"CARPETA\": \"" + Carpeta + "\", \"URL\": \"" + Url + "\", \"PUBLICADOINTERNET\": \"" + PublicadoInternet + "\", \"URLEXTERNA\": \"" + UrlExterna + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 50).Value = NombreWebService;
                Comando.Parameters.Add("CARPETA", SqlDbType.NVarChar, 250).Value = Carpeta;
                Comando.Parameters.Add("URL", SqlDbType.NVarChar, 250).Value = Url;
                Comando.Parameters.Add("PUBLICADOINTERNET", SqlDbType.Char, 2).Value = PublicadoInternet;
                Comando.Parameters.Add("URLEXTERNA", SqlDbType.NVarChar, 250).Value = UrlExterna;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool UpdateWebService(int IDWebService, int IDServidor, string NombreWebService, string Carpeta, string Url, string PublicadoInternet, string UrlExterna, string Usuario)
        {
            try
            {
                string Query = "Update WEBSERVICE Set IDServidor = @IDSERVIDOR, Nombre = @NOMBRE, Carpeta = @CARPETA, URL = @URL, PublicadoInternet = @PUBLICADOINTERNET, URLExterna = @URLEXTERNA where IDWebservice = @IDWEBSERVICE";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDWEBSERVICE\": \"" + IDWebService + "\", \"IDSERVIDOR\": \"" + IDServidor + "\", \"NOMBRE\": \"" + NombreWebService + "\", \"CARPETA\": \"" + Carpeta + "\", \"URL\": \"" + Url + "\", \"PUBLICADOINTERNET\": \"" + PublicadoInternet + "\", \"URLEXTERNA\": \"" + UrlExterna + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDWEBSERVICE", SqlDbType.SmallInt).Value = IDWebService;
                Comando.Parameters.Add("IDSERVIDOR", SqlDbType.SmallInt).Value = IDServidor;
                Comando.Parameters.Add("NOMBRE", SqlDbType.NVarChar, 50).Value = NombreWebService;
                Comando.Parameters.Add("CARPETA", SqlDbType.NVarChar, 250).Value = Carpeta;
                Comando.Parameters.Add("URL", SqlDbType.NVarChar, 250).Value = Url;
                Comando.Parameters.Add("PUBLICADOINTERNET", SqlDbType.Char, 2).Value = PublicadoInternet;
                Comando.Parameters.Add("URLEXTERNA", SqlDbType.NVarChar, 250).Value = UrlExterna;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool DeleteWebService(int IDWebService, string Usuario)
        {
            try
            {
                string Query = "Delete WEBSERVICE Where IDWebservice = @IDWEBSERVICE";
                InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"IDWEBSERVICE\": \"" + IDWebService + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("IDWEBSERVICE", SqlDbType.SmallInt).Value = IDWebService;
                Comando.ExecuteNonQuery();
                Close();
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }
        #endregion

        #region Permisos
        public Data SelectPermisosUsuarios(string Usuario)
        {
            Data datos = new Data();
            try
            {
                string Query = "Select Usuario, PERFIL.Perfil, Funcion from ACCESO, PERFIL, PERFIL_PERMISOS Where ACCESO.Perfil = PERFIL.IDPerfil And PERFIL_PERMISOS.IDPerfil = ACCESO.Perfil And Usuario = @USUARIO";
                //InsertLog(Usuario, "\"Consulta\": \"" + Query + "\"", "\"USUARIO\": \"" + Usuario + "\"");
                Open();
                Comando.Parameters.Clear();
                Comando.CommandText = Query;
                Comando.Parameters.Add("USUARIO", SqlDbType.NVarChar, 50).Value = Usuario;
                Adaptador.Fill(datos.Tabla);
                Close();
            }
            catch
            {
                Close();
            }
            return datos;
        }
        #endregion
        //public DataHey GetDataHey()
        //{
        //    return new DataHey();
        //}
    }
}
