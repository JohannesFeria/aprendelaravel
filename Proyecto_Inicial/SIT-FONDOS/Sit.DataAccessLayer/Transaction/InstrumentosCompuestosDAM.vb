Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class InstrumentosCompuestosDAM

    Public Sub New()
    End Sub

    Public Function SeleccionarInstrumentosCompuestos(ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As InstrumentosCompuestosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosCompuestos_Seleccionar")

        db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)

        Dim oInstCom As New InstrumentosCompuestosBE
        db.LoadDataSet(dbCommand, oInstCom, "InstrumentosCompuestos")
        Return oInstCom
    End Function

    Public Function IngresarModificarInstrumentosCompuestos(ByVal strCodNemonico As String, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim i As Integer
        Try
            For i = 0 To dtDetalle.Rows.Count - 1
                If VerificarExistencia(strCodNemonico, CType(dtDetalle.Rows(i)("CodigoNemonicoAsociado"), String)) = True Then
                    ModificarInstrumentoCompuesto(strCodNemonico, dtDetalle.Rows(i), dataRequest)
                Else
                    IngresarInstrumentoCompuesto(strCodNemonico, dtDetalle.Rows(i), dataRequest)
                End If
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function VerificarExistencia(ByVal strCodNemonico As String, ByVal strCodNemonicoAsociado As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosCompuestos_VerificarExistencia")
        Dim DtTabla As New DataTable
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoNemonicoAsociado", DbType.String, strCodNemonicoAsociado)
        DtTabla = db.ExecuteDataSet(dbCommand).Tables(0)
        If DtTabla.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IngresarInstrumentoCompuesto(ByVal strCodNemonico As String, ByVal oDR As DataRow, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosCompuestos_Ingresar")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoNemonicoAsociado", DbType.String, oDR("CodigoNemonicoAsociado"))
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oDR("CodigoTipoInstrumento"))
        db.AddInParameter(dbCommand, "@p_Monto", DbType.String, CType(CType(oDR("Monto"), String), Decimal))
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.String, CType(CType(oDR("Cantidad"), String), Decimal))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oDR("Situacion"))

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ModificarInstrumentoCompuesto(ByVal strCodNemonico As String, ByVal oDR As DataRow, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosCompuestos_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoNemonicoAsociado", DbType.String, CType(oDR("CodigoNemonicoAsociado"), String))
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CType(oDR("CodigoTipoInstrumento"), String))
        db.AddInParameter(dbCommand, "@p_Monto", DbType.String, CType(CType(oDR("Monto"), String), Decimal))
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.String, CType(CType(oDR("Cantidad"), String), Decimal))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oDR("Situacion"))

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal strCodNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosCompuestos_Eliminar")

            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodNemonico)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
