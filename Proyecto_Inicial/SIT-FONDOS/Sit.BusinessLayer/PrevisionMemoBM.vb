Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Public Class PrevisionMemoBM

    Public Function InsertarParametriaMemo(ByVal objMemoBE As PrevisionMemo) As Integer
        Dim objDAM As New PrevisionMemoDAM()
        Return objDAM.InsertarParametriaMemo(objMemoBE)
    End Function

    Public Function ListarDatosMemo(ByVal TipoReporte As String, ByVal idTipoOperacion As String) As PrevisionMemo
        Dim objDAM As New PrevisionMemoDAM()
        Return objDAM.ListarDatosMemo(TipoReporte, idTipoOperacion)
    End Function

    Public Function SeleccionarTipoReportexUsuario(ByVal IdTabla As String, ByVal Usuario As String) As DataSet
        Dim objDAM As New PrevisionMemoDAM()
        Dim oData As New DataSet
        Try
            oData = objDAM.SeleccionarTipoReportexUsuario(IdTabla, Usuario)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

    Public Function SeleccionarTipoOperacionxUsuario(ByVal IdTabla As String, ByVal Usuario As String) As DataSet
        Dim objDAM As New PrevisionMemoDAM()
        Dim oData As New DataSet
        Try
            oData = objDAM.SeleccionarTipoOperacionxUsuario(IdTabla, Usuario)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

    Public Function SeleccionarReporteTipoOperacion(ByVal TipoOperacion As String, ByVal FechaPago As Decimal) As DataSet
        Dim objDAM As New PrevisionMemoDAM()
        Dim oData As New DataSet
        Try
            oData = objDAM.SeleccionarReporteTipoOperacion(TipoOperacion, FechaPago)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

    Public Function SeleccionarReporteGeneral(ByVal FechaPago As Decimal) As DataSet
        Dim objDAM As New PrevisionMemoDAM()
        Dim oData As New DataSet
        Try
            oData = objDAM.SeleccionarReporteGeneral(FechaPago)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function
End Class
