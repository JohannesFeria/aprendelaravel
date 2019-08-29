Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class AprobadorDocumentoDAM
    Private sqlCommand As String = ""
    Dim DECIMAL_NULO As Decimal = -1
    Public oAprobadorDocumentoRow As AprobadorDocumentoBE.AprobadorDocumentoRow

    Public Sub New()

    End Sub

    Public Sub InicializarAprobadorDocumento(ByRef oRow As AprobadorDocumentoBE.AprobadorDocumentoRow)
        oRow.CodigoInterno = ""
        oRow.Firma = ""
        oRow.Administrador = DECIMAL_NULO
        oRow.Firmante = DECIMAL_NULO
        oRow.Operador = DECIMAL_NULO
        oRow.Situacion = ""
        oRow.Clave = ""
    End Sub

#Region "Metodos Transaccionales"
    Public Function Insertar(ByVal oAprobadorDocumentoBE As AprobadorDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.Rows(0), AprobadorDocumentoBE.AprobadorDocumentoRow)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_AprobadorDocumento_Insertar")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oAprobadorDocumentoRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_Administrador", DbType.Decimal, oAprobadorDocumentoRow.Administrador)
        db.AddInParameter(dbCommand, "@p_Firmante", DbType.Decimal, oAprobadorDocumentoRow.Firmante)
        db.AddInParameter(dbCommand, "@p_Operador", DbType.Decimal, oAprobadorDocumentoRow.Operador)
        db.AddInParameter(dbCommand, "@p_Firma", DbType.String, IIf(oAprobadorDocumentoRow.Firma = "", DBNull.Value, oAprobadorDocumentoRow.Firma))
        db.AddInParameter(dbCommand, "@p_Clave", DbType.String, oAprobadorDocumentoRow.Clave)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oAprobadorDocumentoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal oAprobadorDocumentoBE As AprobadorDocumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.Rows(0), AprobadorDocumentoBE.AprobadorDocumentoRow)
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_AprobadorDocumento_Modificar")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oAprobadorDocumentoRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_Administrador", DbType.Decimal, oAprobadorDocumentoRow.Administrador)
        db.AddInParameter(dbCommand, "@p_Firmante", DbType.Decimal, oAprobadorDocumentoRow.Firmante)
        db.AddInParameter(dbCommand, "@p_Operador", DbType.Decimal, oAprobadorDocumentoRow.Operador)
        db.AddInParameter(dbCommand, "@p_Firma", DbType.String, IIf(oAprobadorDocumentoRow.Firma = "", DBNull.Value, oAprobadorDocumentoRow.Firma))
        db.AddInParameter(dbCommand, "@p_Clave", DbType.String, oAprobadorDocumentoRow.Clave)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oAprobadorDocumentoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoInterno As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_AprobadorDocumento_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region "Metodos No Transaccionales"
    Public Function SeleccionarPorFiltro(ByVal oAprobadorDocumentoBE As AprobadorDocumentoBE, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.Rows(0), AprobadorDocumentoBE.AprobadorDocumentoRow)
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_AprobadorDocumento_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_codigoInterno", DbType.String, oAprobadorDocumentoRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, oAprobadorDocumentoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_administrador", DbType.Decimal, oAprobadorDocumentoRow.Administrador)
        db.AddInParameter(dbCommand, "@p_firmante", DbType.Decimal, oAprobadorDocumentoRow.Firmante)
        db.AddInParameter(dbCommand, "@p_operador", DbType.Decimal, oAprobadorDocumentoRow.Operador)
        db.AddInParameter(dbCommand, "@p_clave", DbType.String, oAprobadorDocumentoRow.Clave)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region
End Class
