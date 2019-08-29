Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class FirmaDocumentoDetDAM
    Private sqlCommand As String = ""
    Dim DECIMAL_NULO As Decimal = -1
    Public oFirmaDocumentoDetRow As FirmaDocumentoDetBE.FirmaDocumentoDetRow

    Public Sub New()

    End Sub

    Public Sub InicializarFirmaDocumentoDet(ByRef oRow As FirmaDocumentoDetBE.FirmaDocumentoDetRow)
        oRow.CodFirmaDocumento = DECIMAL_NULO
        oRow.CodFirmaDocumentoDet = DECIMAL_NULO
        oRow.CodigoInterno = String.Empty
        oRow.Situacion = String.Empty

    End Sub

#Region "Metodos Transaccionales"
    Public Function Insertar(ByVal codFirmaDocumento As Decimal, ByVal login As String, ByRef validaFirma As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim result As Decimal = 0
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_FirmaDocumentoDet_Insertar")
        db.AddInParameter(dbCommand, "@p_codFirmaDocumento", DbType.Decimal, codFirmaDocumento)
        db.AddInParameter(dbCommand, "@p_login", DbType.String, login)
        db.AddInParameter(dbCommand, "@p_usuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_horaCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_validaFirma", DbType.Decimal, validaFirma)
        db.ExecuteNonQuery(dbCommand)
        validaFirma = db.GetParameterValue(dbCommand, "@p_validaFirma")
        Return True
    End Function

    Public Function Eliminar(ByVal codFirmaDocumento As Decimal, ByVal login As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_FirmaDocumentoDet_Eliminar")
        db.AddInParameter(dbCommand, "@p_codFirmaDocumento", DbType.Decimal, codFirmaDocumento)
        db.AddInParameter(dbCommand, "@p_login", DbType.String, login)
        db.AddInParameter(dbCommand, "@p_usuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_horaModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal codFirmaDocumentoDet As Decimal, ByVal situacion As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_FirmaDocumentoDet_Modificar")
        db.AddInParameter(dbCommand, "@p_codFirmaDocumentoDet", DbType.Decimal, codFirmaDocumentoDet)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_usuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_horaModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region "Metodos No Transaccionales"

#End Region
End Class
