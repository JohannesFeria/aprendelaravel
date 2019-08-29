Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class GrupoInstrumentoDAM
    Public Sub New()

    End Sub

#Region "/*Seleccionar*/"
    Public Function SeleccionarPorFiltro(ByVal CodigoGrupoInstrumento As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "GrupoInstrumento")
        Return objeto
    End Function
    Public Function SeleccionarCaracteristicasGrupo(ByVal CodigoGrupoInstrumento As String, ByVal CodigoCaracteristica As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_SeleccionarCaracteristicaGrupo")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)

        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "GrupoInstrumento")
        Return objeto
    End Function
    Public Function SeleccionarCaracteristicasGrupoNivel(ByVal CodigoGrupoInstrumento As String, ByVal CodigoCaracteristica As String, ByVal vista As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_SeleccionarCaracteristicaGrupoNivel")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)
        db.AddInParameter(dbCommand, "@p_Vista", DbType.String, vista)

        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "GrupoInstrumento")
        Return objeto
    End Function
    Public Function SeleccionarCodigoCaracteristicasGrupoNivel(ByVal CodigoGrupoInstrumento As String, _
    ByVal FlagDetalle As String, ByVal CodigoCaracteristica As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_SeleccionarCodigoCaracteristicaGrupoNivel")
        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_FlagDetalle", DbType.String, FlagDetalle)
        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)
        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "GrupoInstrumento")
        Return objeto
    End Function
    Public Function SeleccionarDescripcionValoresPorValorVista(ByVal CodigoValor As String, ByVal Vista As String, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim StrDescripcion As String = String.Empty
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_SeleccionarDescripcionValoresPorValorVista")
        db.AddInParameter(dbCommand, "@p_CodigoValor", DbType.String, CodigoValor)
        db.AddInParameter(dbCommand, "@p_Vista", DbType.String, Vista)
        'Dim objeto As New DataSet
        'db.LoadDataSet(dbCommand, objeto, "GrupoInstrumento")

        StrDescripcion = db.ExecuteScalar(dbCommand)
        If StrDescripcion Is Nothing Then
            StrDescripcion = ""
        End If
        Return StrDescripcion
        'Return objeto
    End Function
    Public Function SeleccionarValoresPorCaracteristica(ByVal CodigoCaracteristica As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_SeleccionarValoresPorCaracteristica")

        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)
        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "GrupoInstrumento")
        Return objeto
    End Function
#End Region


#Region "/*Eliminar*/"
    Public Function Eliminar(ByVal CodigoGrupoInstrumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        InactivarCaracteristicasValores(CodigoGrupoInstrumento, dataRequest)
        Return True
    End Function

    Public Function EliminarRegistrosCaracteristicaGrupoNivel(ByVal CodigoGrupoInstrumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_EliminarCaracteristicaGrupoNivel")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function EliminarUnRegistroCaracteristicaGrupoNivel(ByVal CodigoGrupoInstrumento As String, _
    ByVal CodigoCaracteristica As String, ByVal ValorCaracteristica As String, _
    ByVal GrupoNormativo As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_EliminarUnRegistroCaracteristicaGrupoNivel")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)
        db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, ValorCaracteristica)
        db.AddInParameter(dbCommand, "@p_GrupoNormativo", DbType.String, GrupoNormativo)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function InactivarCaracteristicasValores(ByVal CodigoGrupoInstrumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_InactivarCaracteristicasGrupoNivel")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#End Region

#Region "/* Insertar */"
    Public Function Insertar(ByVal Descripcion As String, ByVal situacion As String, ByVal dtCaracteristicaGrupoNivel As DataTable, ByVal dataRequest As DataSet) As String
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_Insertar")
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        StrCodigo = db.ExecuteScalar(dbCommand)
        'Insertamos las caracteristicas correspondientes
        InsertarCaracteristicaGrupoNivel(StrCodigo, dtCaracteristicaGrupoNivel, situacion, dataRequest)
        Return StrCodigo
    End Function
    Public Sub InsertarCaracteristicaGrupoNivel(ByVal strCodigoGrupoInstrumento As String, _
    ByVal dtCaracteristicasValores As DataTable, ByVal situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        For Each filaLinea As DataRow In dtCaracteristicasValores.Rows
            If filaLinea.RowState <> DataRowState.Deleted Then
                Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_InsertarCaracteristicaGrupoNivel")
                db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, strCodigoGrupoInstrumento)
                db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, filaLinea("CodigoCaracteristica").ToString().Trim())
                db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, filaLinea("ValorCaracteristica").ToString().Trim())
                db.AddInParameter(dbCommand, "@p_ClaseNormativa", DbType.String, filaLinea("ClaseNormativa").ToString().Trim())
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteScalar(dbCommand)
            End If
        Next
    End Sub
#End Region

#Region "/*Modificar*/"
    Public Function Modificar(ByVal CodigoGrupoInstrumento As String, ByVal Descripcion As String, ByVal situacion As String, ByVal dtCaracteristicasValores As DataTable, ByVal dataRequest As DataSet) As String
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoInstrumento_Modificar")
        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, CodigoGrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        StrCodigo = db.ExecuteScalar(dbCommand)
        'eliminar las caracteristicas anteriores
        EliminarRegistrosCaracteristicaGrupoNivel(StrCodigo, dataRequest)
        'Insertar las caracteristicas correspondientes
        InsertarCaracteristicaGrupoNivel(StrCodigo, dtCaracteristicasValores, situacion, dataRequest)
        Return StrCodigo
    End Function
#End Region
End Class
