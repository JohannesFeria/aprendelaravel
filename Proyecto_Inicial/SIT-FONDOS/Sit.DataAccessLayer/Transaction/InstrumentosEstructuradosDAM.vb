Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class InstrumentosEstructuradosDAM

    Public Sub New()
    End Sub

    Public Function SeleccionarInstrumentosEstructurados(ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As InstrumentosEstructuradosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosEstructurados_Seleccionar")

        db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)

        Dim oInstEst As New InstrumentosEstructuradosBE
        db.LoadDataSet(dbCommand, oInstEst, "InstrumentosEstructurados")
        Return oInstEst

    End Function

    Public Function IngresarModificarInstrumentosEstructurados(ByVal strCodNemonico As String, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim i As Integer
        Try
            Eliminar(strCodNemonico, dataRequest)
            For i = 0 To dtDetalle.Rows.Count - 1
                IngresarInstrumentoEstructurado(strCodNemonico, dtDetalle.Rows(i), dataRequest) 'RGF 20090324
            Next

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function VerificarExistencia(ByVal strCodNemonico As String, ByVal strCodNemonicoAsociado As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosEstructurados_VerificarExistencia")
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

    Public Function IngresarInstrumentoEstructurado(ByVal strCodNemonico As String, ByVal oDR As DataRow, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosEstructurados_Ingresar")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoNemonicoAsociado", DbType.String, oDR("CodigoNemonicoAsociado"))
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oDR("CodigoTipoInstrumento"))
        db.AddInParameter(dbCommand, "@p_Monto", DbType.String, Nothing) 'LETV 20090421
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.String, CType(CType(oDR("Cantidad"), String), Decimal))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oDR("Situacion"))

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        'RGF 20090324
        db.AddInParameter(dbCommand, "@p_MonedaPrima", DbType.String, oDR("MonedaPrima"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function ModificarInstrumentoEstructurado(ByVal strCodNemonico As String, ByVal oDR As DataRow, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InstrumentosEstructurados_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoNemonicoAsociado", DbType.String, oDR("CodigoNemonicoAsociado"))
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oDR("CodigoTipoInstrumento"))
        db.AddInParameter(dbCommand, "@p_Monto", DbType.String, CType(CType(oDR("Monto"), String), Decimal))
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.String, CType(CType(oDR("Cantidad"), String), Decimal))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oDR("Situacion"))

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Eliminar(ByVal strCodNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosEstructurados_Eliminar")

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

    'RGF 20090331
    Public Function ListarUnidadesBloqueadas(ByVal codigoNemonico As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ListarUnidadesBloqueadas")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)

        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    'LETV 20091109
    Public Function ListarInstrumentoEstructuradoNocional(ByVal codigoNemonico As String) As DataTable
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_InstrumentoEstructuradoNocional_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
            Return db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'LETV 20091110
    Public Function InsertarInstrumentoEstructuradosNocional(ByVal oListaIENocional As DataTable) As Boolean
        For Each oIENocional As DataRow In oListaIENocional.Rows
            InsertarInstrumentoEstructuradoNocional(oIENocional)
        Next
        Return True
    End Function
    'LETV 20091110
    Private Function InsertarInstrumentoEstructuradoNocional(ByVal oIENocional As DataRow) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_InstrumentoEstructuradoNocional_Ingresar")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oIENocional.Item("CodigoNemonico"))
        db.AddInParameter(dbCommand, "@p_Nocional", DbType.Decimal, oIENocional.Item("Nocional"))
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oIENocional.Item("CodigoPortafolioSBS"))
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oIENocional.Item("CodigoMoneda"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ModificarInstrumentoEstructuradosNocional(ByVal oListaIENocional As DataTable) As Boolean
        If oListaIENocional.Rows.Count = 0 Then
            Return False
        End If

        Dim existe As Boolean = False
        Dim oListaIENocionalOriginal As DataTable = ListarInstrumentoEstructuradoNocional(oListaIENocional.Rows(0)("CodigoNemonico"))

        For Each oNocionalOriginal As DataRow In oListaIENocionalOriginal.Rows
            existe = False
            For Each oNocionalNueva As DataRow In oListaIENocional.Rows
                If oNocionalNueva.Item("CodigoInstrumentoEstructuradoNocional") = oNocionalOriginal.Item("CodigoInstrumentoEstructuradoNocional") Then
                    ModificarInstrumentoEstructuradosNocional(oNocionalNueva)
                    existe = True
                    Exit For
                End If
            Next
            If (Not existe) Then
                EliminarInstrumentoEstructuradosNocional(oNocionalOriginal("CodigoInstrumentoEstructuradoNocional"))
            End If
        Next

        For Each oNocionalNueva As DataRow In oListaIENocional.Rows
            If oNocionalNueva.Item("CodigoInstrumentoEstructuradoNocional") < 0 Then
                InsertarInstrumentoEstructuradoNocional(oNocionalNueva)
            End If
        Next

        Return True
    End Function
    Public Function ModificarInstrumentoEstructuradosNocional(ByVal oIENocional As DataRow) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_InstrumentoEstructuradoNocional_Modificar")
        db.AddInParameter(dbCommand, "@p_CodigoInstrumentoEstructuradoNocional", DbType.Int32, oIENocional.Item("CodigoInstrumentoEstructuradoNocional"))
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oIENocional.Item("CodigoNemonico"))
        db.AddInParameter(dbCommand, "@p_Nocional", DbType.Decimal, oIENocional.Item("Nocional"))
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oIENocional.Item("CodigoPortafolioSBS"))
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oIENocional.Item("CodigoMoneda"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function EliminarInstrumentoEstructuradosNocional(ByVal CodigoInstrumentoEstructuradoNocional As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_InstrumentoEstructuradoNocional_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoInstrumentoEstructuradoNocional", DbType.Int32, CodigoInstrumentoEstructuradoNocional)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

End Class

