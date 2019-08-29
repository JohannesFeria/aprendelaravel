Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class GrupoCategoriaInstrumentoDAM

    Private sqlCommand As String = ""
    Private oGrupoCategoriaInstrumentoRow As GrupoCategoriaInstrumentoBE.GrupoCategoriaInstrumentoRow

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal GrupoInstrumento As String, ByVal Descripcion As String, ByVal Situacion As String, ByVal Tipo As String, ByVal dataRequest As DataSet) As GrupoCategoriaInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_GrupoCategoriaInstrumento_ListarPorTipo")

        db.AddInParameter(dbCommand, "@p_GrupoCategoria", DbType.String, GrupoInstrumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, Tipo)

        Dim objeto As New GrupoCategoriaInstrumentoBE
        db.LoadDataSet(dbCommand, objeto, "GrupoCategoriaInstrumento")
        Return objeto
    End Function

#End Region

    Public Function Insertar(ByVal ob As GrupoCategoriaInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim i As Integer

        For i = 0 To ob.GrupoCategoriaInstrumento.Rows.Count - 1
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_GrupoCategoriaInstrumento")

            oGrupoCategoriaInstrumentoRow = CType(ob.GrupoCategoriaInstrumento.Rows(i), GrupoCategoriaInstrumentoBE.GrupoCategoriaInstrumentoRow)

            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oGrupoCategoriaInstrumentoRow.CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_GrupoCategoria", DbType.String, oGrupoCategoriaInstrumentoRow.GrupoCategoria)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oGrupoCategoriaInstrumentoRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oGrupoCategoriaInstrumentoRow.Situacion)

            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function

    Public Function Modificar(ByVal ob As GrupoCategoriaInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        'eliminar detalle
        eliminarDetalleGrupo(ob)
        'Se realiza similar a un ingreso
        Return Insertar(ob, dataRequest)
    End Function

    Public Function Eliminar(ByVal GrupoCategoria As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_GrupoCategoriaInstrumento")
        db.AddInParameter(dbCommand, "@p_GrupoCategoria", DbType.String, GrupoCategoria)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Private Sub eliminarDetalleGrupo(ByVal ob As GrupoCategoriaInstrumentoBE)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_GrupoCategoriaInstrumento_Detalle")

        oGrupoCategoriaInstrumentoRow = CType(ob.GrupoCategoriaInstrumento.Rows(0), GrupoCategoriaInstrumentoBE.GrupoCategoriaInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_GrupoCategoria", DbType.String, oGrupoCategoriaInstrumentoRow.GrupoCategoria)
        db.ExecuteNonQuery(dbCommand)
    End Sub

End Class
