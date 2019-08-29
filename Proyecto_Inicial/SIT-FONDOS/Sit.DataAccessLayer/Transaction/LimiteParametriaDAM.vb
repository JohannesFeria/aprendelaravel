Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class LimiteParametriaDAM
    Public Sub New()
    End Sub
#Region "GrupoPorTipoMoneda"
    Public Sub GrupoPorTipoMoneda_Insertar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Cartera As String, ByVal Situacion As String,ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_GrupoPorTipoMoneda")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoTipoMoneda", DbType.String, CodigoTipoMoneda)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@MonedaPortafolio", DbType.String, MonedaPortafolio)
        db.AddInParameter(dbCommand, "@Forward", DbType.String, Forward)
        db.AddInParameter(dbCommand, "@Cartera", DbType.String, Cartera)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub GrupoPorTipoMoneda_Modificar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Cartera As String, ByVal Situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_upd_GrupoPorTipoMoneda")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoTipoMoneda", DbType.String, CodigoTipoMoneda)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@MonedaPortafolio", DbType.String, MonedaPortafolio)
        db.AddInParameter(dbCommand, "@Forward", DbType.String, Forward)
        db.AddInParameter(dbCommand, "@Cartera", DbType.String, Cartera)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function GrupoPorTipoMoneda_Seleccionar(ByVal CodigoTipoMoneda As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorTipoMoneda")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoTipoMoneda", DbType.String, CodigoTipoMoneda)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorTipoMoneda_Buscar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_bus_GrupoPorTipoMoneda")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoTipoMoneda", DbType.String, CodigoTipoMoneda)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
#End Region
#Region "Clase Instrumento"
    Public Function GrupoClaseInstrumento_Buscar(ByVal CodigoClaseInstrumento As String, ByVal Descripcion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_bus_GrupoPorClaseInstrumento")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoClaseInstrumento", DbType.String, CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoClaseInstrumento_Seleccionar(ByVal CodigoClaseInstrumento As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorClaseInstrumento")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoClaseInstrumento", DbType.String, CodigoClaseInstrumento)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoClaseInstrumento_Grupo(ByVal CodigoClaseInstrumento As String, ByVal Descripcion As String, ByVal Existe As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_ClaseInstrumento_Grupo")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoClaseInstrumento", DbType.String, CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@P_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@Existe", DbType.String, Existe)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Sub GrupoClaseInstrumento_Insertar(ByVal CodigoClaseInstrumento As String, ByVal Descripcion As String, ByVal ClaseInstrumento As String,
    ByVal Situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_GrupoPorClaseInstrumento")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoClaseInstrumento", DbType.String, CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@ClaseInstrumento", DbType.String, ClaseInstrumento)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub GrupoClaseInstrumento_Borrar(ByVal CodigoClaseInstrumento As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_GrupoPorClaseInstrumento")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoClaseInstrumento", DbType.String, CodigoClaseInstrumento)
        db.ExecuteNonQuery(dbCommand)
    End Sub
#End Region
#Region "Entidad"
    Public Sub GrupoPorEntidad_Insertar(ByVal CodigoGrupoEntidad As String, ByVal Descripcion As String, ByVal CodigoEntidad As String,
   ByVal Situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_GrupoPorEntidad")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoEntidad", DbType.String, CodigoGrupoEntidad)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@CodigoEntidad", DbType.String, CodigoEntidad)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub GrupoPorEntidad_Borrar(ByVal CodigoGrupoEntidad As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_GrupoPorEntidad")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoEntidad", DbType.String, CodigoGrupoEntidad)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function GrupoPorEntidad_Grupo(ByVal CodigoGrupoEntidad As String, Descripcion As String, ByVal Existe As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorEntidad_Grupo")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoEntidad", DbType.String, CodigoGrupoEntidad)
        db.AddInParameter(dbCommand, "@P_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@Existe", DbType.String, Existe)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorEntidad_Seleccionar(ByVal CodigoGrupoEntidad As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorEntidad")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoEntidad", DbType.String, CodigoGrupoEntidad)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorEntidad_Buscar(ByVal CodigoGrupoEntidad As String, ByVal Descripcion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_bus_GrupoPorEntidad")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoEntidad", DbType.String, CodigoGrupoEntidad)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
#End Region
#Region "GrupoPorDerivados"
    Public Sub GrupoPorDerivados_Insertar(ByVal CodigoGrupoDerivado As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Swap As String, ByVal NominalRecibir As String, ByVal Situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_GrupoPorDerivados")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoDerivado", DbType.String, CodigoGrupoDerivado)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@MonedaPortafolio", DbType.String, MonedaPortafolio)
        db.AddInParameter(dbCommand, "@Forward", DbType.String, Forward)
        db.AddInParameter(dbCommand, "@Swap", DbType.String, Swap)
        db.AddInParameter(dbCommand, "@NominalRecibir", DbType.String, NominalRecibir)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub GrupoPorDerivados_Modificar(ByVal CodigoGrupoDerivado As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Swap As String, ByVal NominalRecibir As String, ByVal Situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_upd_GrupoPorDerivados")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoDerivado", DbType.String, CodigoGrupoDerivado)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@MonedaPortafolio", DbType.String, MonedaPortafolio)
        db.AddInParameter(dbCommand, "@Forward", DbType.String, Forward)
        db.AddInParameter(dbCommand, "@Swap", DbType.String, Swap)
        db.AddInParameter(dbCommand, "@NominalRecibir", DbType.String, NominalRecibir)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function GrupoPorDerivados_Seleccionar(ByVal CodigoGrupoDerivado As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorDerivados")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoDerivado", DbType.String, CodigoGrupoDerivado)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorDerivados_Buscar(ByVal CodigoGrupoDerivado As String, ByVal Descripcion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_bus_GrupoPorDerivados")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoDerivado", DbType.String, CodigoGrupoDerivado)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
#End Region
#Region "GrupoPorNemonico"
    Public Sub GrupoPorNemonico_Insertar(ByVal CodigoGrupoNemonico As String, ByVal Descripcion As String, ByVal CodigoNemonico As String,
   ByVal Situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_GrupoPorNemonico")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoNemonico", DbType.String, CodigoGrupoNemonico)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub GrupoPorNemonico_Borrar(ByVal CodigoGrupoNemonico As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_GrupoPorNemonico")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoNemonico", DbType.String, CodigoGrupoNemonico)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function GrupoPorNemonico_Grupo(ByVal CodigoGrupoNemonico As String, Descripcion As String, ByVal Existe As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorNemonico_Grupo")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoNemonico", DbType.String, CodigoGrupoNemonico)
        db.AddInParameter(dbCommand, "@P_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@Existe", DbType.String, Existe)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorNemonico_Seleccionar(ByVal CodigoGrupoNemonico As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorNemonico")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoNemonico", DbType.String, CodigoGrupoNemonico)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorNemonico_Buscar(ByVal CodigoGrupoNemonico As String, ByVal Descripcion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_bus_GrupoPorNemonico")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoNemonico", DbType.String, CodigoGrupoNemonico)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
#End Region
#Region "GrupoPorCalificacion"
    Public Sub GrupoPorCalificacion_Insertar(ByVal CodigoGrupoClasificacion As String, ByVal Descripcion As String, ByVal CodigoCalificacion As String,
    ByVal Local As String, ByVal DPZ As String,ByVal Situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_GrupoPorCalificacion")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoClasificacion", DbType.String, CodigoGrupoClasificacion)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@CodigoCalificacion", DbType.String, CodigoCalificacion)
        db.AddInParameter(dbCommand, "@Local", DbType.String, Local)
        db.AddInParameter(dbCommand, "@DPZ", DbType.String, DPZ)
        db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub GrupoPorCalificacion_Borrar(ByVal CodigoGrupoClasificacion As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_GrupoPorCalificacion")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoClasificacion", DbType.String, CodigoGrupoClasificacion)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function GrupoPorCalificacion_Grupo(ByVal CodigoGrupoClasificacion As String, ByVal Existe As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorCalificacion_Grupo")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoClasificacion", DbType.String, CodigoGrupoClasificacion)
        db.AddInParameter(dbCommand, "@Existe", DbType.String, Existe)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorCalificacion_Seleccionar(ByVal CodigoGrupoClasificacion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_GrupoPorCalificacion")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoClasificacion", DbType.String, CodigoGrupoClasificacion)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoPorCalificacion_Buscar(ByVal CodigoGrupoClasificacion As String, ByVal Descripcion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_bus_GrupoPorCalificacion")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@CodigoGrupoClasificacion", DbType.String, CodigoGrupoClasificacion)
        db.AddInParameter(dbCommand, "@Descripcion", DbType.String, Descripcion)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
#End Region
End Class
