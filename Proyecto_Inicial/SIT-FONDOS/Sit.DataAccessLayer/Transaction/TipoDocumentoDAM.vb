Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class TipoDocumentoDAM

    Private sqlCommand As String = ""
    Private oTipoDocumentoRow As TipoDocumentoBE.TipoDocumentoRow
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoTipoDocumento As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoDocumento_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As DataSet

        Dim oTipoDocumentoBE As New TipoDocumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoDocumento_Listar")

        db.LoadDataSet(dbCommand, oTipoDocumentoBE, "TipoDocumento")

        Return oTipoDocumentoBE

    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoTipoDocumento As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoDocumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoDocumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New TipoDocumentoBE
        db.LoadDataSet(dbCommand, objeto, "TipoDocumento")
        Return objeto
    End Function
#End Region

    Public Function Insertar(ByVal ob As TipoDocumentoBE, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoDocumento_Insertar")
        oTipoDocumentoRow = CType(ob.TipoDocumento.Rows(0), TipoDocumentoBE.TipoDocumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, oTipoDocumentoRow.CodigoTipoDocumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoDocumentoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_LongitudMaxima", DbType.Decimal, oTipoDocumentoRow.LongitudMaxima)
        db.AddInParameter(dbCommand, "@p_DigitoChekeo", DbType.String, oTipoDocumentoRow.DigitoChekeo)
        db.AddInParameter(dbCommand, "@p_TipoPersona", DbType.String, oTipoDocumentoRow.TipoPersona)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoDocumentoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Modificar(ByVal ob As TipoDocumentoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoDocumento_Modificar")
        oTipoDocumentoRow = CType(ob.TipoDocumento.Rows(0), TipoDocumentoBE.TipoDocumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, oTipoDocumentoRow.CodigoTipoDocumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoDocumentoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoDocumentoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_LongitudMaxima", DbType.Decimal, oTipoDocumentoRow.LongitudMaxima)
        db.AddInParameter(dbCommand, "@p_DigitoChekeo", DbType.String, oTipoDocumentoRow.DigitoChekeo)
        db.AddInParameter(dbCommand, "@p_TipoPersona", DbType.String, oTipoDocumentoRow.TipoPersona)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoTipoDocumento As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoDocumento_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

End Class

