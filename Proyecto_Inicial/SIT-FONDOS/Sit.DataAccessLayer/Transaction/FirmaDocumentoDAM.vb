Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class FirmaDocumentoDAM
    Private sqlCommand As String = ""
    Dim DECIMAL_NULO As Decimal = -1
    Public oFirmaDocumentoRow As FirmaDocumentoBE.FirmaDocumentoRow

    Public Sub New()

    End Sub

    Public Sub InicializarFirmaDocumento(ByRef oRow As FirmaDocumentoBE.FirmaDocumentoRow)
        oRow.CodFirmaDocumento = DECIMAL_NULO
        oRow.CodigoPortafolioSBS = String.Empty
        oRow.FechaDocumento = DECIMAL_NULO
        oRow.CodigoOrden = String.Empty
        oRow.CodCargoFirmante = DECIMAL_NULO
        oRow.CodigoInterno = String.Empty
        oRow.Situacion = String.Empty
    End Sub

#Region "Metodos Transaccionales"
    Public Function Insertar(ByVal oFirmaDocumentoBE As FirmaDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        oFirmaDocumentoRow = CType(oFirmaDocumentoBE.FirmaDocumento.Rows(0), FirmaDocumentoBE.FirmaDocumentoRow)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_FirmaDocumento_Insertar")
        db.AddInParameter(dbCommand, "@p_fechaDocumento", DbType.Decimal, oFirmaDocumentoRow.FechaDocumento)
        db.AddInParameter(dbCommand, "@p_codigoOrden", DbType.String, oFirmaDocumentoRow.CodigoOrden)
        db.AddInParameter(dbCommand, "@p_codCargoFirmante", DbType.Decimal, oFirmaDocumentoRow.CodCargoFirmante)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, oFirmaDocumentoRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_codigoInterno", DbType.String, oFirmaDocumentoRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, oFirmaDocumentoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_usuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_horaCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal oFirmaDocumentoBE As FirmaDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        oFirmaDocumentoRow = CType(oFirmaDocumentoBE.FirmaDocumento.Rows(0), FirmaDocumentoBE.FirmaDocumentoRow)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_FirmaDocumento_Modificar")
        db.AddInParameter(dbCommand, "@p_codFirmaDocumento", DbType.Decimal, oFirmaDocumentoRow.CodFirmaDocumento)
        db.AddInParameter(dbCommand, "@p_fechaDocumento", DbType.Decimal, oFirmaDocumentoRow.FechaDocumento)
        db.AddInParameter(dbCommand, "@p_codigoOrden", DbType.String, oFirmaDocumentoRow.CodigoOrden)
        db.AddInParameter(dbCommand, "@p_codCargoFirmante", DbType.Decimal, oFirmaDocumentoRow.CodCargoFirmante)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, oFirmaDocumentoRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_codigoInterno", DbType.String, oFirmaDocumentoRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, oFirmaDocumentoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_usuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_horaModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codFirmaDocumento As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_FirmaDocumento_Eliminar")
        db.AddInParameter(dbCommand, "@p_codFirmaDocumento", DbType.Decimal, codFirmaDocumento)
        db.AddInParameter(dbCommand, "@p_usuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_horaModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region "Metodos No Transaccionales"
    Public Function SeleccionarPorFiltro(ByVal fecha As Decimal, _
                                        ByVal codReporte As String, _
                                        ByVal codCategReporte As String, _
                                        ByVal codCargoFirmante As Decimal, _
                                        ByVal codigoOrden As String, _
                                        ByVal codigoPortafolioSBS As String, _
                                        ByVal estado As String, _
                                        ByVal codigoMercado As String, _
                                        ByVal codigoOperacion As String, _
                                        ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_FirmaDocumento_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_codReporte", DbType.String, codReporte)
        db.AddInParameter(dbCommand, "@p_codCategReporte", DbType.String, codCategReporte)
        db.AddInParameter(dbCommand, "@p_codCargoFirmante", DbType.Decimal, codCargoFirmante)
        db.AddInParameter(dbCommand, "@p_codigoUsuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_codigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_estFirmaD", DbType.String, estado)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigo(ByVal codFirmaDocumento As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase

        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_FirmaDocumento_SeleccionarPorCodigo")
        db.AddInParameter(dbCommand, "@p_codFirmaDocumento", DbType.Decimal, codFirmaDocumento)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function VerificaPermisoFirma(ByVal login As String, ByVal clave As String) As Decimal
        Dim dt As New DataTable
        Dim validaUsuario As Decimal = 0
        Dim db As Database = DatabaseFactory.CreateDatabase

        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_FirmaDocumento_VerificaPermisoFirma")
        db.AddInParameter(dbCommand, "@p_codigoUsuario", DbType.String, login)
        db.AddInParameter(dbCommand, "@p_clave", DbType.String, clave)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        validaUsuario = IIf(dt.Rows(0)("validaUsuario") Is Nothing, 0, Convert.ToDecimal(dt.Rows(0)("validaUsuario")))
        Return validaUsuario
    End Function

#End Region
End Class
