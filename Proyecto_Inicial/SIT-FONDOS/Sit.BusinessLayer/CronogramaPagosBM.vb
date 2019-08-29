Imports MotorTransaccionesProxy
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer


Public Class CronogramaPagosBM
    Inherits InvokerCOM
    Dim oCronogramaPagosDAM As New CronogramaPagosDAM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "
    Public Function CronogramaPagos_ListarbyRangoFechaPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaIni As Decimal, ByVal FechaFin As Decimal) As DataTable
        Try
            Return oCronogramaPagosDAM.CronogramaPagos_ListarbyRangoFechaPortafolio(CodigoPortafolioSBS, FechaIni, FechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CronogramaPagos_ListarbyDetalleInstrumento(ByVal CodigoPortafolioSBS As String, ByVal fechaPago As Decimal) As DataTable
        Try
            Return oCronogramaPagosDAM.CronogramaPagos_ListarbyDetalleInstrumento(CodigoPortafolioSBS, fechaPago)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function CronogramaPagos_Insertar(ByVal oRowCP As CronogramaPagosBE, ByVal dataRequest As DataSet) As Boolean
        Dim result As Integer
        Try
            result = oCronogramaPagosDAM.CronogramaPagos_Insertar(oRowCP, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function
#End Region

#Region " /* Funciones Modificar */"
    Public Function CronogramaPagos_Modificar(ByVal oRowCP As CronogramaPagosBE, ByVal dataRequest As DataSet) As Boolean
        Dim result As Integer
        Try
            result = oCronogramaPagosDAM.CronogramaPagos_Modificar(oRowCP, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function


#End Region

#Region " /* Funciones Eliminar */"

#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub InicializarCronogramaPagos(ByRef oRowCP As CronogramaPagosBE)
        Try
            oCronogramaPagosDAM.InicializarCronogramaPagos(oRowCP)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

End Class

