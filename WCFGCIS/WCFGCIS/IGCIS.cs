using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WCFGCIS
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IGCIS" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IGCIS
    {
        //[OperationContract]
        //string Passphrase();

        #region Acceso
        [OperationContract]
        Data SelectAcceso(string Usuario);
        #endregion

        #region Ambiente
        [OperationContract]
        Data SelectAmbiente (string Ambiente, string Usuario);

        [OperationContract]
        bool InsertAmbiente(string Ambiente, string Usuario);

        [OperationContract]
        bool UpdateAmbiente(int ID, string Ambiente, string Usuario);

        [OperationContract]
        bool DeleteAmbiente(int ID, string Usuario);
        #endregion

        #region Aplicacion
        [OperationContract]
        Data SelectAplicacion(int Proveedor, string Aplicacion, string Usuario);

        [OperationContract]
        bool InsertAplicacion(string Aplicacion, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario);

        [OperationContract]
        bool UpdateAplicacion(int ID, string Aplicacion, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario);

        [OperationContract]
        bool DeleteAplicacion(int ID, string Usuario);
        #endregion

        #region BackupExecJobs
        [OperationContract]
        Data SelectBackupExecJobsIDServer(int IDServidor, string Usuario);

        [OperationContract]
        Data SelectBackupExecJobs(string BackupJob, string Usuario);

        [OperationContract]
        bool InsertBackupExecJobs(int IDServidor, string BackupExecJob, string Descripcion, string DiaSemana, string Hora, string Usuario);

        [OperationContract]
        bool UpdateBackupExecJobs(int ID, int IDServidor, string BackupExecJob, string Descripcion, string DiaSemana, string Hora, string Usuario);

        [OperationContract]
        bool DeleteBackupExecJobs(int ID, string Usuario);
        #endregion

        #region Cluster
        [OperationContract]
        Data SelectCluster(string Cluster, string Usuario);

        [OperationContract]
        bool InsertCluster(string Cluster, string Descripcion, int IDAplicacion, string Usuario);

        [OperationContract]
        bool UpdateCluster(int IDCluster, string Cluster, string Descripcion, int IDAplicacion, string Usuario);

        [OperationContract]
        bool DeleteCluster(int ID, string Usuario);
        #endregion

        #region ClusterServidorFisico
        [OperationContract]
        Data SelectClusterServidorFisico(string Cluster, string Usuario);

        [OperationContract]
        bool InsertClusterServidorFisico(int IDServidorFisico, int IDCluster, string Usuario);

        [OperationContract]
        bool UpdateClusterServidorFisico(int IDServidorFisico, int IDCluster, string Usuario);

        [OperationContract]
        bool DeleteClusterServidorFisico(int IDServidorFisico, int IDCluster, string Usuario);
        #endregion

        #region Contacto
        [OperationContract]
        Data SelectContacto(int Proveedor, string Usuario);

        [OperationContract]
        bool InsertContacto(int Proveedor, string Nombre, string Telefono, string Emergencia, string Usuario);

        [OperationContract]
        bool UpdateContacto(int ID, int Proveedor, string Nombre, string Telefono, string Emergencia, string Usuario);

        [OperationContract]
        bool DeleteContacto(int ID, string Usuario);
        #endregion

        #region Database
        [OperationContract]
        Data SelectDatabaseIDInstancia(int IDInstancia, string Usuario);

        [OperationContract]
        Data SelectDatabase(string NombreDatabase, string Usuario);

        [OperationContract]
        bool InsertDatabase(int IDInstancia, string Nombre, string Descripcion, string Usuario);

        [OperationContract]
        bool UpdateDatabase(int IDDatabase, int IDInstancia, string Nombre, string Descripcion, string Usuario);

        [OperationContract]
        bool DeleteDatabase(int IDDatabase, string Usuario);
        #endregion

        #region DatabaseMantenimiento
        [OperationContract]
        Data SelectDatabaseMantenimiento(int IDDatabase, string Usuario);

        [OperationContract]
        bool InsertDatabaseMantenimiento(int IDDatabase, int IDPlanMantenimiento, string Usuario);

        [OperationContract]
        bool UpdateDatabaseMantenimiento(int IDDatabase, int IDPlanMantenimiento, string Usuario);

        [OperationContract]
        bool DeleteDatabaseMantenimiento(int IDDatabase, int IDPlanMantenimiento, string Usuario);
        #endregion

        #region DatabaseUsuario
        [OperationContract]
        Data SelectDatabaseUsuario(string DatabaseNombre, string Usuario);

        [OperationContract]
        bool InsertDatabaseUsuario(int IDDatabase, int IDUsuario, string Usuario);

        [OperationContract]
        bool UpdateDatabaseUsuario(int IDDatabase, int IDUsuario, string Usuario);

        [OperationContract]
        bool DeleteDatabaseUsuario(int IDDatabase, int IDUsuario, string Usuario);
        #endregion

        #region Disco
        [OperationContract]
        Data SelectDisco(string Servidor, string Usuario);

        [OperationContract]
        bool InsertDisco(int IDServidor, string Disco, int Capacidad, int IDPool, string VolumenCompartido, string Usuario);

        [OperationContract]
        bool UpdateDisco(int IDDisco, int IDServidor, string Disco, int Capacidad, int IDPool, string VolumenCompartido, string Usuario);

        [OperationContract]
        bool DeleteDisco(int IDDisco, string Usuario);
        #endregion

        #region DocumentoServidor
        [OperationContract]
        Data SelectDocumentoServidor(int IDServidor, string Usuario);

        [OperationContract]
        bool InsertDocumentoServidor(int IDServidor, string NombreDocumento, byte[] Documento, string Usuario);

        [OperationContract]
        bool UpdateDocumentoServidor(int IDDocumento, string NombreDocumento, byte[] Documento, string Usuario);

        [OperationContract]
        bool DeleteDocumentoServidor(int IDDocumento, string Usuario);
        #endregion

        #region Edificio
        [OperationContract]
        Data SelectEdificio(string Edificio, string Usuario);

        [OperationContract]
        bool InsertEdificio(string Edificio, string Direccion, string Usuario);

        [OperationContract]
        bool UpdateEdificio(int ID, string Edificio, string Direccion, string Usuario);

        [OperationContract]
        bool DeleteEdificio(int ID, string Usuario);
        #endregion

        #region Ethernet
        [OperationContract]
        Data SelectEthernet(string Servidor, string Usuario);

        [OperationContract]
        bool InsertEthernet(string DireccionIP, int IDServidor, string Usuario);

        [OperationContract]
        bool UpdateEthernet(string DireccionIP, int IDServidor, string NewDireccionIP, string Usuario);

        [OperationContract]
        bool DeleteEthernet(string DireccionIP, string Usuario);
        #endregion

        #region InstanciaAplicacion
        [OperationContract]
        Data SelectInstanciaAplicacionIDServer(int IDServidor, string UsuarioLog);

        [OperationContract]
        Data SelectInstanciaAplicacion(string Instancia, string UsuarioLog);

        [OperationContract]
        bool InsertInstanciaAplicacion(int IDServidor, int IDAmbiente, int IDAplicacion, string Instancia, string Usuario, string Password, string UsuarioLog);

        [OperationContract]
        bool UpdateInstanciaAplicacion(int IDInstancia, int IDServidor, int IDAmbiente, int IDAplicacion, string Instancia, string Usuario, string Password, string UsuarioLog);

        [OperationContract]
        bool DeleteInstanciaAplicacion(int IDInstancia, string UsuarioLog);
        #endregion

        #region Marca
        [OperationContract]
        Data SelectMarca(string Marca, string Usuario);

        [OperationContract]
        bool InsertMarca(string Status, string Usuario);

        [OperationContract]
        bool UpdateMarca(int ID, string Status, string Usuario);

        [OperationContract]
        bool DeleteMarca(int ID, string Usuario);
        #endregion

        #region Modelo
        [OperationContract]
        Data SelectModeloByMarca(string Marca, string Usuario);

        [OperationContract]
        Data SelectModelo(string Modelo, string Usuario);

        [OperationContract]
        bool InsertModelo(int IDMarca, string Modelo, string Usuario);

        [OperationContract]
        bool UpdateModelo(int ID, int IDMarca, string Modelo, string Usuario);

        [OperationContract]
        bool DeleteModelo(int ID, string Usuario);
        #endregion

        #region PlanMantenimiento
        [OperationContract]
        Data SelectPlanMantenimiento(string NombreInstancia, string Usuario);

        [OperationContract]
        bool InsertPlanMantenimiento(int IDInstancia, string Nombre, string Descripcion, string DiaSemana, string Hora, string Usuario);

        [OperationContract]
        bool UpdatePlanMantenimiento(int IDPlanMantenimiento, int IDInstancia, string Nombre, string Descripcion, string DiaSemana, string Hora, string Usuario);

        [OperationContract]
        bool DeletePlanMantenimiento(int IDPlanMantenimiento, string Usuario);
        #endregion

        #region Pool
        [OperationContract]
        Data SelectPoolBySan(string San, string Usuario);

        [OperationContract]
        Data SelectPool(string Pool, string Usuario);

        [OperationContract]
        bool InsertPool(int IDSan, string Pool, string Descripcion, string Usuario);

        [OperationContract]
        bool UpdatePool(int IDPool, int IDSan, string Pool, string Descripcion, string Usuario);

        [OperationContract]
        bool DeletePool(int IDPool, string Usuario);
        #endregion

        #region Procesador
        [OperationContract]
        Data SelectProcesador(string Like, string Usuario);

        [OperationContract]
        bool InsertProcesador(int IDMarca, string Procesador, string Usuario);

        [OperationContract]
        bool UpdateProcesador(int ID, int IDMarca, string Procesador, string Usuario);

        [OperationContract]
        bool DeleteProcesador(int ID, string Usuario);
        #endregion

        #region Proveedor
        [OperationContract]
        Data SelectProveedor(string NombreProveedor, string Usuario);

        [OperationContract]
        bool InsertProveedor(string Proveedor, string Usuario);

        [OperationContract]
        bool UpdateProveedor(int ID, string Proveedor, string Usuario);

        [OperationContract]
        bool DeleteProveedor(int ID, string Usuario);
        #endregion

        #region Rack
        [OperationContract]
        Data SelectRack(string Like, string Usuario);

        [OperationContract]
        bool InsertRack(string Rack, string Descripcion, string Usuario);

        [OperationContract]
        bool UpdateRack(int ID, string Rack, string Descripcion, string Usuario);

        [OperationContract]
        bool DeleteRack(int ID, string Usuario);
        #endregion

        #region Redundancia
        [OperationContract]
        Data SelectRedundancia(int IDServidor, string Usuario);

        [OperationContract]
        bool InsertRedundancia(int IDServidor, int IDRedundancia, string Usuario);

        [OperationContract]
        bool UpdateRedundancia(int IDServidor, int IDRedundancia, string Usuario);

        [OperationContract]
        bool DeleteRedundancia(int IDServidor, int IDRedundancia, string Usuario);
        #endregion

        #region SAN
        [OperationContract]
        Data SelectSAN(string San, string Usuario);

        [OperationContract]
        bool InsertSAN(string San, string Usuario);

        [OperationContract]
        bool UpdateSAN(int IDSan, string San, string Usuario);

        [OperationContract]
        bool DeleteSAN(int IDSan, string Usuario);
        #endregion

        #region Servidor
        [OperationContract]
        Data SelectServidor4Redundancia(string Servidor, string Usuario);

        [OperationContract]
        Data SelectServidor(string Servidor, string Usuario);

        [OperationContract]
        bool InsertServidor(int IDStatus, int IDSistemaOperativo, int IDAmbiente, int IDTipo, string Servidor, string Descripcion, int NoProcesadores, int Ram, int NoEthernet, string TeamEthernet, DateTime FechaParches, string Usuario);

        [OperationContract]
        bool UpdateServidor(int IDServidor, int IDStatus, int IDSistemaOperativo, int IDAmbiente, int IDTipo, string Servidor, string Descripcion, int NoProcesadores, int Ram, int NoEthernet, string TeamEthernet, DateTime FechaParches, string Usuario);

        [OperationContract]
        bool DeleteServidor(int IDServidor, string Usuario);
        #endregion

        #region ServidorFisico
        [OperationContract]
        Data SelectServidorFisico(string Servidor, string Usuario);

        [OperationContract]
        bool InsertServidorFisico(int IDServidor, int IDModelo, int IDEdificio, string Procesador, string ServiceTag, string FuentesRedundantes, DateTime FechaFinGarantia, DateTime FechaCompra, int IDRack, string OU, string Usuario);

        [OperationContract]
        bool UpdateservidorFisico(int IDServidor, int IDModelo, int IDEdificio, string Procesador, string ServiceTag, string FuentesRedundantes, DateTime FechaFinGarantia, DateTime FechaCompra, int IDRack, string OU, string Usuario);

        [OperationContract]
        bool DeleteServidorFisico(int IDServidor, string Usuario);
        #endregion

        #region ServidorSolucion
        [OperationContract]
        Data SelectServidorSolucion(string Solucion, string Usuario);

        [OperationContract]
        bool InsertServidorSolucion(int IDServidor, int IDSolucion, string Usuario);

        [OperationContract]
        bool UpdateServidorSolucion(int IDServidor, int IDSolucion, string Usuario);

        [OperationContract]
        bool DeleteServidorSolucion(int IDServidor, int IDSolucion, string Usuario);
        #endregion

        #region ServidorUsuario
        [OperationContract]
        Data SelectServidorUsuario(string Servidor, string Usuario);

        [OperationContract]
        bool InsertServidorUsuario(int IDServidor, int IDUsuario, string Usuario);

        [OperationContract]
        bool UpdateServidorUsuario(int IDServidor, int IDUsuario, string Usuario);

        [OperationContract]
        bool DeleteServidorUsuario(int IDServidor, int IDUsuario, string Usuario);
        #endregion

        #region ServidorVirtual
        [OperationContract]
        Data SelectServidorVirtual(string Servidor, string Usuario);

        [OperationContract]
        bool InsertServidorVirtual(int IDServidor, int IDCluster, string Usuario);

        [OperationContract]
        bool UpdateServidorVirtual(int IDServidor, int IDCluster, string Usuario);

        [OperationContract]
        bool DeleteServidorVirtual(int IDServidor, string Usuario);
        #endregion

        #region ServidorWebservice
        [OperationContract]
        Data SelectServidorWebservice(string Servidor, string Usuario);

        [OperationContract]
        bool InsertServidorWebservice(int IDServidor, int IDWebservice, string Usuario);

        [OperationContract]
        bool UpdateServidorWebservice(int IDServidor, int IDWebservice, string Usuario);

        [OperationContract]
        bool DeleteServidorWebservice(int IDServidor, int IDWebservice, string Usuario);
        #endregion

        #region SistemaOperativo
        [OperationContract]
        Data SelectSistemaOperativo(int Proveedor, string SistemaOperativo, string Usuario);

        [OperationContract]
        bool InsertSistemaOperativo(string SistemaOperativo, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario);

        [OperationContract]
        bool UpdateSistemaOperativo(int ID, string SistemaOperativo, string Version, string Fabricante, string Licencia, int Proveedor, DateTime FechaVencimiento, string Usuario);

        [OperationContract]
        bool DeleteSistemaOperativo(int ID, string Usuario);
        #endregion

        #region Solucion
        [OperationContract]
        Data SelectSolucion(string Solucion, string Usuario);

        [OperationContract]
        bool InsertSolucion(string Solucion, string Descripcion, string Usuario);

        [OperationContract]
        bool UpdateSolucion(int IDSolucion, string Solucion, string Descripcion, string Usuario);

        [OperationContract]
        bool DeleteSolucion(int IDSolucion, string Usuario);
        #endregion

        #region Status
        [OperationContract]
        Data SelectStatus(string Status, string Usuario);

        [OperationContract]
        bool InsertStatus(string Status, string Usuario);

        [OperationContract]
        bool UpdateStatus(int ID, string Status, string Usuario);

        [OperationContract]
        bool DeleteStatus(int ID, string Usuario);
        #endregion

        #region Tipo
        [OperationContract]
        Data SelectTipo(string Tipo, string Usuario);

        [OperationContract]
        bool InsertTipo(string Status, string Usuario);

        [OperationContract]
        bool UpdateTipo(int ID, string Status, string Usuario);

        [OperationContract]
        bool DeleteTipo(int ID, string Usuario);
        #endregion

        #region UrlAdministracionSAN
        [OperationContract]
        Data SelectUrlAdministracionSAN(string San, string Usuario);

        [OperationContract]
        bool InsertUrlAdministracionSAN(int IDSan, string Url, string Usuario);

        [OperationContract]
        bool UpdateUrlAdministracionSAN(int IDUrl, int IDSan, string Url, string Usuario);

        [OperationContract]
        bool DeleteUrlAdministracionSAN(int IDUrl, string Usuario);
        #endregion

        #region Usuario
        [OperationContract]
        Data SelectUsuarioIDServer(int IDServidor, string UsuarioLog);

        [OperationContract]
        Data SelectUsuario(string Usuario, string UsuarioLog);

        [OperationContract]
        bool InsertUsuario(int IDServidor, string Dominio, string Usuario, string Password, string Descripcion, string UsuarioLog);

        [OperationContract]
        bool UpdateUsuario(int IDUsuario, int IDServidor, string Dominio, string Usuario, string Password, string Descripcion, string UsuarioLog);

        [OperationContract]
        bool DeleteUsuario(int IDUsuario, string UsuarioLog);
        #endregion

        #region UsuarioInstancia
        [OperationContract]
        Data SelectUsuarioInstancia(string InstanciaAplicacion, string UsuarioLog);

        [OperationContract]
        bool InsertUsuarioInstancia(int IDInstancia, string Usuario, string Password, string UsuarioLog);

        [OperationContract]
        bool UpdateUsuarioInstancia(int IDUsuario, int IDInstancia, string Usuario, string Password, string UsuarioLog);

        [OperationContract]
        bool DeleteUsuarioInstancia(int IDUsuario, string UsuarioLog);
        #endregion

        #region UsuarioSAN
        [OperationContract]
        Data SelectUsuarioSAN(string San, string UsuarioLog);

        [OperationContract]
        bool InsertUsuarioSAN(int IDSan, string Usuario, string Password, string UsuarioLog);

        [OperationContract]
        bool UpdateUsuarioSAN(int IDUsuario, int IDSan, string Usuario, string Password, string UsuarioLog);

        [OperationContract]
        bool DeleteUsuarioSAN(int IDUsuario, string UsuarioLog);
        #endregion

        #region WebService
        [OperationContract]
        Data SelectWebServiceIDServer(int IDServidor, string Usuario);

        [OperationContract]
        Data SelectWebService(string NombreWebService, string Usuario);

        [OperationContract]
        bool InsertWebService(int IDServidor, string NombreWebService, string Carpeta, string Url, string PublicadoInternet, string UrlExterna, string Usuario);

        [OperationContract]
        bool UpdateWebService(int IDWebService, int IDServidor, string NombreWebService, string Carpeta, string Url, string PublicadoInternet, string UrlExterna, string Usuario);

        [OperationContract]
        bool DeleteWebService(int IDWebService, string Usuario);
        #endregion

        #region Permisos
        [OperationContract]
        Data SelectPermisosUsuarios(string Usuario);
        #endregion

        //[OperationContract]
        //DataHey GetDataHey();
    }

    [DataContract]
    public class Data
    {
        [DataMember]
        public DataTable Tabla { get; set; }

        public Data()
        {
            this.Tabla = new DataTable("Data");
        }
    }

    //[DataContract]
    //public class DataHey
    //{
    //    [DataMember]
    //    public DataTable Hey { get; set; }
    //}
}
