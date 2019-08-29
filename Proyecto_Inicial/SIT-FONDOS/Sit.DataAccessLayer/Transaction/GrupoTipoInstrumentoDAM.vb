Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class GrupoTipoInstrumentoDAM

    Private sqlCommand As String = ""
    Private oGrupoTipoInstrumentoRow As GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow

#Region " /* Funciones Seleccionar */ "

    Public Function Listar() As GrupoTipoInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoTipoInstrumento_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As GrupoTipoInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoTipoInstrumento_Listar")

        Dim objeto As New GrupoTipoInstrumentoBE
        db.LoadDataSet(dbCommand, objeto, "GrupoTipoInstrumento")
        Return objeto
    End Function


    Public Function SeleccionarPorFiltro(ByVal GrupoInstrumento As String, ByVal Descripcion As String, ByVal Situacion As String, ByVal Tipo As String, ByVal dataRequest As DataSet) As GrupoTipoInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoTipoInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_GrupoInstrumento", DbType.String, GrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, Tipo)

        Dim objeto As New GrupoTipoInstrumentoBE
        db.LoadDataSet(dbCommand, objeto, "GrupoTipoInstrumento")
        Return objeto
    End Function


#End Region


    Public Function Insertar(ByVal ob As GrupoTipoInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim i As Integer

        For i = 0 To ob.GrupoTipoInstrumento.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoTipoInstrumento_Insertar")

            oGrupoTipoInstrumentoRow = CType(ob.GrupoTipoInstrumento.Rows(i), GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow)

            db.AddInParameter(dbCommand, "@p_GrupoInstrumento", DbType.String, oGrupoTipoInstrumentoRow.GrupoInstrumento)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oGrupoTipoInstrumentoRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oGrupoTipoInstrumentoRow.CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oGrupoTipoInstrumentoRow.Situacion)

            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function


    Public Function Modificar(ByVal ob As GrupoTipoInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        'eliminar detalle
        eliminarDetalleGrupo(ob)
        Dim i As Integer

        For i = 0 To ob.GrupoTipoInstrumento.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoTipoInstrumento_Insertar")

            oGrupoTipoInstrumentoRow = CType(ob.GrupoTipoInstrumento.Rows(i), GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow)

            db.AddInParameter(dbCommand, "@p_GrupoInstrumento", DbType.String, oGrupoTipoInstrumentoRow.GrupoInstrumento)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oGrupoTipoInstrumentoRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oGrupoTipoInstrumentoRow.CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oGrupoTipoInstrumentoRow.Situacion)

            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function


    Private Sub eliminarDetalleGrupo(ByVal ob As GrupoTipoInstrumentoBE)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoTipoInstrumento_EliminarDetalle")

        oGrupoTipoInstrumentoRow = CType(ob.GrupoTipoInstrumento.Rows(0), GrupoTipoInstrumentoBE.GrupoTipoInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_GrupoInstrumento", DbType.String, oGrupoTipoInstrumentoRow.GrupoInstrumento)
        db.ExecuteNonQuery(dbCommand)
    End Sub


    Public Function Eliminar(ByVal GrupoInstrumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoTipoInstrumento_Eliminar")

        db.AddInParameter(dbCommand, "@p_GrupoInstrumento", DbType.String, GrupoInstrumento)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class
